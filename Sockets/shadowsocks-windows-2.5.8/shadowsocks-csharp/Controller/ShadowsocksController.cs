﻿using System.IO;
using Shadowsocks.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using Shadowsocks.Controller.Strategy;
using System.Net;

namespace Shadowsocks.Controller
{
    public class PathEventArgs : EventArgs
    {
        public string Path;
    }

    // controller:
    // handle user actions
    // manipulates UI
    // interacts with low level logic
    public class ShadowsocksController
    {
        private Thread _ramThread;

        private Listener _listener;
        private PACServer _pacServer;
        private Configuration _config;
        private StrategyManager _strategyManager;
        private PolipoRunner _polipoRunner;
        private GFWListUpdater _gfwListUpdater;
        private AvailabilityStatistics _availabilityStatics;
        private bool _stopped = false;

        private bool _systemProxyIsDirty = false;

        public event EventHandler ConfigChanged;
        public event EventHandler EnableStatusChanged;
        public event EventHandler EnableGlobalChanged;
        public event EventHandler ShareOverLANStatusChanged;

        // when user clicked Edit PAC, and PAC file has already created
        public event EventHandler<PathEventArgs> PACFileReadyToOpen;
        public event EventHandler<PathEventArgs> UserRuleFileReadyToOpen;
        public event EventHandler<GFWListUpdater.ResultEventArgs> UpdatePACFromGFWListCompleted;
        public event ErrorEventHandler UpdatePACFromGFWListError;
        public event ErrorEventHandler Errored;

        public ShadowsocksController()
        {
            _config = Configuration.Load();
            _strategyManager = new StrategyManager(this);

            StartReleasingMemory();
        }

        public void Start()
        {
            Reload();
        }

        protected void ReportError(Exception e)
        {
            if (Errored != null)
            {
                Errored(this, new ErrorEventArgs(e));
            }
        }

        public Server GetCurrentServer()
        {
            return _config.GetCurrentServer();
        }

        // always return copy
        public Configuration GetConfigurationCopy()
        {
            return Configuration.Load();
        }

        // always return current instance
        public Configuration GetCurrentConfiguration()
        {
            return _config;
        }

        public IList<IStrategy> GetStrategies()
        {
            return _strategyManager.GetStrategies();
        }

        public IStrategy GetCurrentStrategy()
        {
            foreach (var strategy in _strategyManager.GetStrategies())
            {
                if (strategy.ID == this._config.strategy)
                {
                    return strategy;
                }
            }
            return null;
        }

        public Server GetAServer(IStrategyCallerType type, IPEndPoint localIPEndPoint)
        {
            IStrategy strategy = GetCurrentStrategy();
            if (strategy != null)
            {
                return strategy.GetAServer(type, localIPEndPoint);
            }
            if (_config.index < 0)
            {
                _config.index = 0;
            }
            return GetCurrentServer();
        }

        public void SaveServers(List<Server> servers, int localPort)
        {
            _config.configs = servers;
            _config.localPort = localPort;
            SaveConfig(_config);
        }

        public bool AddServerBySSURL(string ssURL)
        {
            try
            {
                var server = new Server(ssURL);
                _config.configs.Add(server);
                _config.index = _config.configs.Count - 1;
                SaveConfig(_config);
                return true;
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
                return false;
            }
        }

        public void ToggleEnable(bool enabled)
        {
            _config.enabled = enabled;
            UpdateSystemProxy();
            SaveConfig(_config);
            if (EnableStatusChanged != null)
            {
                EnableStatusChanged(this, new EventArgs());
            }
        }

        public void ToggleGlobal(bool global)
        {
            _config.global = global;
            UpdateSystemProxy();
            SaveConfig(_config);
            if (EnableGlobalChanged != null)
            {
                EnableGlobalChanged(this, new EventArgs());
            }
        }

        public void ToggleShareOverLAN(bool enabled)
        {
            _config.shareOverLan = enabled;
            SaveConfig(_config);
            if (ShareOverLANStatusChanged != null)
            {
                ShareOverLANStatusChanged(this, new EventArgs());
            }
        }

        public void SelectServerIndex(int index)
        {
            _config.index = index;
            _config.strategy = null;
            SaveConfig(_config);
        }

        public void SelectStrategy(string strategyID)
        {
            _config.index = -1;
            _config.strategy = strategyID;
            SaveConfig(_config);
        }

        public void Stop()
        {
            if (_stopped)
            {
                return;
            }
            _stopped = true;
            if (_listener != null)
            {
                _listener.Stop();
            }
            if (_polipoRunner != null)
            {
                _polipoRunner.Stop();
            }
            if (_config.enabled)
            {
                SystemProxy.Update(_config, true);
            }
        }

        public void TouchPACFile()
        {
            string pacFilename = _pacServer.TouchPACFile();
            if (PACFileReadyToOpen != null)
            {
                PACFileReadyToOpen(this, new PathEventArgs() { Path = pacFilename });
            }
        }

        public void TouchUserRuleFile()
        {
            string userRuleFilename = _pacServer.TouchUserRuleFile();
            if (UserRuleFileReadyToOpen != null)
            {
                UserRuleFileReadyToOpen(this, new PathEventArgs() { Path = userRuleFilename });
            }
        }

        public string GetQRCodeForCurrentServer()
        {
            Server server = GetCurrentServer();
            return GetQRCode(server);
        }

        public static string GetQRCode(Server server)
        {
            string parts = server.method + ":" + server.password + "@" + server.server + ":" + server.server_port;
            string base64 = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(parts));
            return "ss://" + base64;
        }

        public void UpdatePACFromGFWList()
        {
            if (_gfwListUpdater != null)
            {
                _gfwListUpdater.UpdatePACFromGFWList(_config);
            }
        }

        public void ToggleAvailabilityStatistics(bool enabled)
        {
            if (_availabilityStatics != null)
            {
                _availabilityStatics.Set(enabled);
                _config.availabilityStatistics = enabled;
                SaveConfig(_config);
            }
        }

        public void SavePACUrl(string pacUrl)
        {
            _config.pacUrl = pacUrl;
            UpdateSystemProxy();
            SaveConfig(_config);
            if (ConfigChanged != null)
            {
                ConfigChanged(this, new EventArgs());
            }
        }

        public void UseOnlinePAC(bool useOnlinePac)
        {
            _config.useOnlinePac = useOnlinePac;
            UpdateSystemProxy();
            SaveConfig(_config);
            if (ConfigChanged != null)
            {
                ConfigChanged(this, new EventArgs());
            }
        }

        protected void Reload()
        {
            // some logic in configuration updated the config when saving, we need to read it again
            _config = Configuration.Load();

            if (_polipoRunner == null)
            {
                _polipoRunner = new PolipoRunner();
            }

            if (_pacServer == null)
            {
                _pacServer = new PACServer();
                _pacServer.PACFileChanged += pacServer_PACFileChanged;
            }

            _pacServer.UpdateConfiguration(_config);

            if (_gfwListUpdater == null)
            {
                _gfwListUpdater = new GFWListUpdater();
                _gfwListUpdater.UpdateCompleted += pacServer_PACUpdateCompleted;
                _gfwListUpdater.Error += pacServer_PACUpdateError;
            }

            if (_listener != null)
            {
                _listener.Stop();
            }

            if (_availabilityStatics == null)
            {
                _availabilityStatics = new AvailabilityStatistics();
                _availabilityStatics.UpdateConfiguration(_config);
            }

            // don't put polipoRunner.Start() before pacServer.Stop()
            // or bind will fail when switching bind address from 0.0.0.0 to 127.0.0.1
            // though UseShellExecute is set to true now
            // http://stackoverflow.com/questions/10235093/socket-doesnt-close-after-application-exits-if-a-launched-process-is-open
            _polipoRunner.Stop();

            try
            {
                var strategy = GetCurrentStrategy();
                if (strategy != null)
                {
                    strategy.ReloadServers();
                }

                _polipoRunner.Start(_config);

                TCPRelay tcpRelay = new TCPRelay(this);
                UDPRelay udpRelay = new UDPRelay(this);

                List<IService> services = new List<IService>();
                services.Add(tcpRelay);
                services.Add(udpRelay);
                services.Add(_pacServer);
                services.Add(new PortForwarder(_polipoRunner.RunningPort));

                _listener = new Listener(services);
                _listener.Start(_config);
            }
            catch (Exception e)
            {
                // translate Microsoft language into human language
                // i.e. An attempt was made to access a socket in a way forbidden by its access permissions => Port already in use
                if (e is SocketException)
                {
                    SocketException se = (SocketException)e;
                    if (se.SocketErrorCode == SocketError.AccessDenied)
                    {
                        e = new Exception(I18N.GetString("Port already in use"), e);
                    }
                }
                Logging.LogUsefulException(e);
                ReportError(e);
            }

            if (ConfigChanged != null)
            {
                ConfigChanged(this, new EventArgs());
            }

            UpdateSystemProxy();
            Util.Utils.ReleaseMemory(true);
        }

        protected void SaveConfig(Configuration newConfig)
        {
            Configuration.Save(newConfig);
            Reload();
        }

        private void UpdateSystemProxy()
        {
            if (_config.enabled)
            {
                SystemProxy.Update(_config, false);
                _systemProxyIsDirty = true;
            }
            else
            {
                // only switch it off if we have switched it on
                if (_systemProxyIsDirty)
                {
                    SystemProxy.Update(_config, false);
                    _systemProxyIsDirty = false;
                }
            }
        }

        private void pacServer_PACFileChanged(object sender, EventArgs e)
        {
            UpdateSystemProxy();
        }

        private void pacServer_PACUpdateCompleted(object sender, GFWListUpdater.ResultEventArgs e)
        {
            if (UpdatePACFromGFWListCompleted != null)
                UpdatePACFromGFWListCompleted(this, e);
        }

        private void pacServer_PACUpdateError(object sender, ErrorEventArgs e)
        {
            if (UpdatePACFromGFWListError != null)
                UpdatePACFromGFWListError(this, e);
        }

        private void StartReleasingMemory()
        {
            _ramThread = new Thread(new ThreadStart(ReleaseMemory));
            _ramThread.IsBackground = true;
            _ramThread.Start();
        }

        private void ReleaseMemory()
        {
            while (true)
            {
                Util.Utils.ReleaseMemory(false);
                Thread.Sleep(30 * 1000);
            }
        }
    }
}
