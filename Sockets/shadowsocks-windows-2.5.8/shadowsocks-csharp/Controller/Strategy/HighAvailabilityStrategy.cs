using Shadowsocks.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shadowsocks.Controller.Strategy
{
    public class ServerStatus
    {
        /// <summary>
        /// time interval between SYN and SYN+ACK
        /// </summary>
        public TimeSpan Latency { get; set; }

        public DateTime LastTimeDetectLatency { get; set; }

        /// <summary>
        /// last time anything received
        /// </summary>
        public DateTime LastRead { get; set; }

        /// <summary>
        /// last time anything sent
        /// </summary>
        public DateTime LastWrite { get; set; }

        /// <summary>
        /// connection refused or closed before anything received
        /// </summary>
        public DateTime LastFailure { get; set; }

        public Server Server { get; set; }

        public double Score { get; set; }
    }

    class HighAvailabilityStrategy : IStrategy
    {
        protected ServerStatus _currentServer;
        protected Dictionary<Server, ServerStatus> _serverStatus;
        private ShadowsocksController _controller;
        private Random _random;

        public HighAvailabilityStrategy(ShadowsocksController controller)
        {
            _controller = controller;
            _random = new Random();
            _serverStatus = new Dictionary<Server, ServerStatus>();
        }

        #region Implement methods of interface IStrategy

        public string Name
        {
            get { return I18N.GetString("High Availability"); }
        }

        public string ID
        {
            get { return "com.shadowsocks.strategy.ha"; }
        }

        public void ReloadServers()
        {
            // make a copy to avoid locking
            var newServerStatus = new Dictionary<Server, ServerStatus>(_serverStatus);

            foreach (var server in _controller.GetCurrentConfiguration().configs)
            {
                if (!newServerStatus.ContainsKey(server))
                {
                    var status = new ServerStatus();
                    status.Server = server;
                    status.LastFailure = DateTime.MinValue;
                    status.LastRead = DateTime.Now;
                    status.LastWrite = DateTime.Now;
                    status.Latency = new TimeSpan(0, 0, 0, 0, 10);
                    status.LastTimeDetectLatency = DateTime.Now;
                    newServerStatus[server] = status;
                }
                else
                {
                    // update settings for existing server
                    newServerStatus[server].Server = server;
                }
            }
            _serverStatus = newServerStatus;

            ChooseNewServer();
        }

        public Server GetAServer(IStrategyCallerType type, System.Net.IPEndPoint localIPEndPoint)
        {
            if (type == IStrategyCallerType.TCP)
            {
                ChooseNewServer();
            }
            if (_currentServer == null)
            {
                return null;
            }
            return _currentServer.Server;
        }

        public void UpdateLatency(Model.Server server, TimeSpan latency)
        {
            Logging.Debug(String.Format("latency: {0} {1}", server.FriendlyName(), latency));

            ServerStatus status;
            if (_serverStatus.TryGetValue(server, out status))
            {
                status.Latency = latency;
                status.LastTimeDetectLatency = DateTime.Now;
            }
        }

        public void UpdateLastRead(Model.Server server)
        {
            Logging.Debug(String.Format("last read: {0}", server.FriendlyName()));

            ServerStatus status;
            if (_serverStatus.TryGetValue(server, out status))
            {
                status.LastRead = DateTime.Now;
            }
        }

        public void UpdateLastWrite(Model.Server server)
        {
            Logging.Debug(String.Format("last write: {0}", server.FriendlyName()));

            ServerStatus status;
            if (_serverStatus.TryGetValue(server, out status))
            {
                status.LastWrite = DateTime.Now;
            }
        }

        public void SetFailure(Model.Server server)
        {
            Logging.Debug(String.Format("failure: {0}", server.FriendlyName()));

            ServerStatus status;
            if (_serverStatus.TryGetValue(server, out status))
            {
                status.LastFailure = DateTime.Now;
            }
        }

        #endregion

        /**
        * once failed, try after 5 min
        * and (last write - last read) < 5s
        * and (now - last read) <  5s  // means not stuck
        * and latency < 200ms, try after 30s
        */
        private void ChooseNewServer()
        {
            ServerStatus oldServer = _currentServer;
            List<ServerStatus> servers = new List<ServerStatus>(_serverStatus.Values);
            DateTime now = DateTime.Now;
            foreach (var status in servers)
            {
                // all of failure, latency, (lastread - lastwrite) normalized to 1000, then
                // 100 * failure - 2 * latency - 0.5 * (lastread - lastwrite)
                status.Score =
                    100 * 1000 * Math.Min(5 * 60, (now - status.LastFailure).TotalSeconds)
                    - 2 * 5 * (Math.Min(2000, status.Latency.TotalMilliseconds) / (1 + (now - status.LastTimeDetectLatency).TotalSeconds / 30 / 10) +
                    -0.5 * 200 * Math.Min(5, (status.LastRead - status.LastWrite).TotalSeconds));
                Logging.Debug(String.Format("server: {0} latency:{1} score: {2}", status.Server.FriendlyName(), status.Latency, status.Score));
            }
            ServerStatus max = null;
            foreach (var status in servers)
            {
                if (max == null)
                {
                    max = status;
                }
                else
                {
                    if (status.Score >= max.Score)
                    {
                        max = status;
                    }
                }
            }
            if (max != null)
            {
                if (_currentServer == null || max.Score - _currentServer.Score > 200)
                {
                    _currentServer = max;
                    Console.WriteLine("HA switching to server: {0}", _currentServer.Server.FriendlyName());
                }
            }
        }
    }
}
