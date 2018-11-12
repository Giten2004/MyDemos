using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Shadowsocks.Controller;
using Shadowsocks.Properties;
using Shadowsocks.Util;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Shadowsocks.View
{
    public class MenuViewController
    {
        private MenuItem _autoStartupItem;
        private MenuItem _availabilityStatistics;

        private ConfigForm _configForm;

        private MenuItem _configItem;
        private MenuItem _editGFWUserRuleItem;
        private MenuItem _editLocalPACItem;
        private MenuItem _editOnlinePACItem;
        private MenuItem _enableItem;
        private MenuItem _globalModeItem;

        private bool _isFirstRun;
        private MenuItem _localPACItem;
        private MenuItem _modeItem;

        private readonly NotifyIcon _notifyIcon;
        private MenuItem _onlinePACItem;
        private MenuItem _PACModeItem;
        private MenuItem _seperatorItem;
        private MenuItem _serversItem;
        // yes this is just a menu view _shadowsocksController
        // when config form is closed, it moves away from RAM
        // and it should just do anything related to the config form

        private readonly ShadowsocksController _shadowsocksController;
        private MenuItem _shareOverLANItem;
        private ContextMenu _trayIconcontextMenu;
        private readonly UpdateChecker _updateChecker;
        private MenuItem _updateFromGFWListItem;
        private string _urlToOpen;

        public MenuViewController(ShadowsocksController shadowsocksController)
        {
            _shadowsocksController = shadowsocksController;
            _shadowsocksController.EnableStatusChanged += controller_EnableStatusChanged;
            _shadowsocksController.ConfigChanged += controller_ConfigChanged;
            _shadowsocksController.PACFileReadyToOpen += controller_FileReadyToOpen;
            _shadowsocksController.UserRuleFileReadyToOpen += controller_FileReadyToOpen;
            _shadowsocksController.ShareOverLANStatusChanged += controller_ShareOverLANStatusChanged;
            _shadowsocksController.EnableGlobalChanged += controller_EnableGlobalChanged;
            _shadowsocksController.Errored += controller_Errored;
            _shadowsocksController.UpdatePACFromGFWListCompleted += controller_UpdatePACFromGFWListCompleted;
            _shadowsocksController.UpdatePACFromGFWListError += controller_UpdatePACFromGFWListError;

            LoadMenu();

            _notifyIcon = new NotifyIcon();

            UpdateTrayIcon();

            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenu = _trayIconcontextMenu;
            _notifyIcon.MouseDoubleClick += notifyIcon1_DoubleClick;

            _updateChecker = new UpdateChecker();
            _updateChecker.NewVersionFound += updateChecker_NewVersionFound;

            LoadCurrentConfiguration();

            _updateChecker.CheckUpdate(_shadowsocksController.GetConfigurationCopy());

            if (_shadowsocksController.GetConfigurationCopy().isDefault)
            {
                _isFirstRun = true;
                ShowConfigForm();
            }
        }

        private void controller_Errored(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.GetException().ToString(),
                string.Format(I18N.GetString("Shadowsocks Error: {0}"), e.GetException().Message));
        }

        private void UpdateTrayIcon()
        {
            int dpi;
            var graphics = Graphics.FromHwnd(IntPtr.Zero);
            dpi = (int) graphics.DpiX;
            graphics.Dispose();
            Bitmap icon = null;
            if (dpi < 97)
            {
                // dpi = 96;
                icon = Resources.ss16;
            }
            else if (dpi < 121)
            {
                // dpi = 120;
                icon = Resources.ss20;
            }
            else
            {
                icon = Resources.ss24;
            }
            var config = _shadowsocksController.GetConfigurationCopy();
            var enabled = config.enabled;
            var global = config.global;
            if (!enabled)
            {
                var iconCopy = new Bitmap(icon);
                for (var x = 0; x < iconCopy.Width; x++)
                {
                    for (var y = 0; y < iconCopy.Height; y++)
                    {
                        var color = icon.GetPixel(x, y);
                        iconCopy.SetPixel(x, y, Color.FromArgb((byte) (color.A/1.25), color.R, color.G, color.B));
                    }
                }
                icon = iconCopy;
            }
            _notifyIcon.Icon = Icon.FromHandle(icon.GetHicon());

            string serverInfo = null;
            if (_shadowsocksController.GetCurrentStrategy() != null)
            {
                serverInfo = _shadowsocksController.GetCurrentStrategy().Name;
            }
            else
            {
                serverInfo = config.GetCurrentServer().FriendlyName();
            }
            // we want to show more details but notify icon title is limited to 63 characters
            var text = I18N.GetString("Shadowsocks") + " " + UpdateChecker.Version + "\n" +
                       (enabled
                           ? I18N.GetString("System Proxy On: ") +
                             (global ? I18N.GetString("Global") : I18N.GetString("PAC"))
                           : string.Format(I18N.GetString("Running: Port {0}"), config.localPort))
                // this feedback is very important because they need to know Shadowsocks is running
                       + "\n" + serverInfo;
            _notifyIcon.Text = text.Substring(0, Math.Min(63, text.Length));
        }

        private MenuItem CreateMenuItem(string text, EventHandler click)
        {
            return new MenuItem(I18N.GetString(text), click);
        }

        private MenuItem CreateMenuGroup(string text, MenuItem[] items)
        {
            return new MenuItem(I18N.GetString(text), items);
        }

        private void LoadMenu()
        {
            _enableItem = CreateMenuItem("Enable System Proxy", EnableItem_Click);
            _modeItem = CreateMenuGroup("Mode", new[]
            {
                _PACModeItem = CreateMenuItem("PAC", PACModeItem_Click),
                _globalModeItem = CreateMenuItem("Global", GlobalModeItem_Click)
            });
            _serversItem = CreateMenuGroup("Servers", new[]
            {
                _seperatorItem = new MenuItem("-"),
                _configItem = CreateMenuItem("Edit Servers...", Config_Click),
                CreateMenuItem("Show QRCode...", QRCodeItem_Click),
                CreateMenuItem("Scan QRCode from Screen...", ScanQRCodeItem_Click)
            });
            var pacItem = CreateMenuGroup("PAC ", new[]
            {
                _localPACItem = CreateMenuItem("Local PAC", LocalPACItem_Click),
                _onlinePACItem = CreateMenuItem("Online PAC", OnlinePACItem_Click),
                new MenuItem("-"),
                _editLocalPACItem = CreateMenuItem("Edit Local PAC File...", EditPACFileItem_Click),
                _updateFromGFWListItem = CreateMenuItem("Update Local PAC from GFWList", UpdatePACFromGFWListItem_Click),
                _editGFWUserRuleItem = CreateMenuItem("Edit User Rule for GFWList...", EditUserRuleFileForGFWListItem_Click),
                _editOnlinePACItem = CreateMenuItem("Edit Online PAC URL...", UpdateOnlinePACURLItem_Click)
            });
            _autoStartupItem = CreateMenuItem("Start on Boot", AutoStartupItem_Click);
            _availabilityStatistics = CreateMenuItem("Availability Statistics", AvailabilityStatisticsItem_Click);
            _shareOverLANItem = CreateMenuItem("Allow Clients from LAN", ShareOverLANItem_Click);
            var showLogItem = CreateMenuItem("Show Logs...", ShowLogItem_Click);
            var aboutItem = CreateMenuItem("About...", AboutItem_Click);
            var quitItem = CreateMenuItem("Quit", Quit_Click);

            _trayIconcontextMenu = new ContextMenu(new[]
            {
                _enableItem, _modeItem, _serversItem, pacItem,
                new MenuItem("-"),
                _autoStartupItem, _availabilityStatistics, _shareOverLANItem,
                new MenuItem("-"),
                showLogItem, aboutItem,
                new MenuItem("-"),
                quitItem
            });
        }

        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            LoadCurrentConfiguration();
            UpdateTrayIcon();
        }

        private void controller_EnableStatusChanged(object sender, EventArgs e)
        {
            _enableItem.Checked = _shadowsocksController.GetConfigurationCopy().enabled;
            _modeItem.Enabled = _enableItem.Checked;
        }

        private void controller_ShareOverLANStatusChanged(object sender, EventArgs e)
        {
            _shareOverLANItem.Checked = _shadowsocksController.GetConfigurationCopy().shareOverLan;
        }

        private void controller_EnableGlobalChanged(object sender, EventArgs e)
        {
            _globalModeItem.Checked = _shadowsocksController.GetConfigurationCopy().global;
            _PACModeItem.Checked = !_globalModeItem.Checked;
        }

        private void controller_FileReadyToOpen(object sender, PathEventArgs e)
        {
            var argument = @"/select, " + e.Path;

            Process.Start("explorer.exe", argument);
        }

        private void ShowBalloonTip(string title, string content, ToolTipIcon icon, int timeout)
        {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = content;
            _notifyIcon.BalloonTipIcon = icon;
            _notifyIcon.ShowBalloonTip(timeout);
        }

        private void controller_UpdatePACFromGFWListError(object sender, ErrorEventArgs e)
        {
            ShowBalloonTip(I18N.GetString("Failed to update PAC file"), e.GetException().Message, ToolTipIcon.Error,
                5000);
            Logging.LogUsefulException(e.GetException());
        }

        private void controller_UpdatePACFromGFWListCompleted(object sender, GFWListUpdater.ResultEventArgs e)
        {
            var result = e.Success
                ? I18N.GetString("PAC updated")
                : I18N.GetString("No updates found. Please report to GFWList if you have problems with it.");
            ShowBalloonTip(I18N.GetString("Shadowsocks"), result, ToolTipIcon.Info, 1000);
        }

        private void updateChecker_NewVersionFound(object sender, EventArgs e)
        {
            ShowBalloonTip(
                string.Format(I18N.GetString("Shadowsocks {0} Update Found"), _updateChecker.LatestVersionNumber),
                I18N.GetString("Click here to download"), ToolTipIcon.Info, 5000);
            _notifyIcon.BalloonTipClicked += notifyIcon1_BalloonTipClicked;
            _isFirstRun = false;
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            Process.Start(_updateChecker.LatestVersionURL);
            _notifyIcon.BalloonTipClicked -= notifyIcon1_BalloonTipClicked;
        }

        private void LoadCurrentConfiguration()
        {
            var config = _shadowsocksController.GetConfigurationCopy();
            UpdateServersMenu();

            _enableItem.Checked = config.enabled;
            _modeItem.Enabled = config.enabled;
            _globalModeItem.Checked = config.global;
            _PACModeItem.Checked = !config.global;
            _shareOverLANItem.Checked = config.shareOverLan;
            _autoStartupItem.Checked = AutoStartup.Check();
            _availabilityStatistics.Checked = config.availabilityStatistics;
            _onlinePACItem.Checked = _onlinePACItem.Enabled && config.useOnlinePac;
            _localPACItem.Checked = !_onlinePACItem.Checked;

            UpdatePACItemsEnabledStatus();
        }

        private void UpdateServersMenu()
        {
            var items = _serversItem.MenuItems;

            while (items[0] != _seperatorItem)
            {
                items.RemoveAt(0);
            }

            var i = 0;
            foreach (var strategy in _shadowsocksController.GetStrategies())
            {
                var item = new MenuItem(strategy.Name);
                item.Tag = strategy.ID;
                item.Click += AStrategyItem_Click;
                items.Add(i, item);

                i++;
            }

            var strategyCount = i;
            var configuration = _shadowsocksController.GetConfigurationCopy();

            foreach (var server in configuration.configs)
            {
                var item = new MenuItem(server.FriendlyName());
                item.Tag = i - strategyCount;
                item.Click += AServerItem_Click;
                items.Add(i, item);

                i++;
            }

            foreach (MenuItem item in items)
            {
                if (item.Tag != null &&
                    (item.Tag.ToString() == configuration.index.ToString() ||
                     item.Tag.ToString() == configuration.strategy))
                {
                    item.Checked = true;
                }
            }
        }

        private void ShowConfigForm()
        {
            if (_configForm != null)
            {
                _configForm.Activate();
            }
            else
            {
                _configForm = new ConfigForm(_shadowsocksController);
                _configForm.Show();
                _configForm.FormClosed += configForm_FormClosed;
            }
        }

        private void configForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _configForm = null;
            Utils.ReleaseMemory(true);
            ShowFirstTimeBalloon();
        }

        private void Config_Click(object sender, EventArgs e)
        {
            ShowConfigForm();
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            _shadowsocksController.Stop();
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        private void ShowFirstTimeBalloon()
        {
            if (_isFirstRun)
            {
                _notifyIcon.BalloonTipTitle = I18N.GetString("Shadowsocks is here");
                _notifyIcon.BalloonTipText = I18N.GetString("You can turn on/off Shadowsocks in the context menu");
                _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                _notifyIcon.ShowBalloonTip(0);
                _isFirstRun = false;
            }
        }

        private void AboutItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/shadowsocks/shadowsocks-windows");
        }

        private void notifyIcon1_DoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowConfigForm();
            }
        }

        private void EnableItem_Click(object sender, EventArgs e)
        {
            _shadowsocksController.ToggleEnable(!_enableItem.Checked);
        }

        private void GlobalModeItem_Click(object sender, EventArgs e)
        {
            _shadowsocksController.ToggleGlobal(true);
        }

        private void PACModeItem_Click(object sender, EventArgs e)
        {
            _shadowsocksController.ToggleGlobal(false);
        }

        private void ShareOverLANItem_Click(object sender, EventArgs e)
        {
            _shareOverLANItem.Checked = !_shareOverLANItem.Checked;
            _shadowsocksController.ToggleShareOverLAN(_shareOverLANItem.Checked);
        }

        private void EditPACFileItem_Click(object sender, EventArgs e)
        {
            _shadowsocksController.TouchPACFile();
        }

        private void UpdatePACFromGFWListItem_Click(object sender, EventArgs e)
        {
            _shadowsocksController.UpdatePACFromGFWList();
        }

        private void EditUserRuleFileForGFWListItem_Click(object sender, EventArgs e)
        {
            _shadowsocksController.TouchUserRuleFile();
        }

        private void AServerItem_Click(object sender, EventArgs e)
        {
            var item = (MenuItem) sender;
            _shadowsocksController.SelectServerIndex((int) item.Tag);
        }

        private void AStrategyItem_Click(object sender, EventArgs e)
        {
            var item = (MenuItem) sender;
            _shadowsocksController.SelectStrategy((string) item.Tag);
        }

        private void ShowLogItem_Click(object sender, EventArgs e)
        {
            var argument = Logging.LogFile;

            new LogForm(argument).Show();
        }

        private void QRCodeItem_Click(object sender, EventArgs e)
        {
            var qrCodeForm = new QRCodeForm(_shadowsocksController.GetQRCodeForCurrentServer());
            //qrCodeForm.Icon = this.Icon;
            // TODO
            qrCodeForm.Show();
        }

        private void ScanQRCodeItem_Click(object sender, EventArgs e)
        {
            foreach (var screen in Screen.AllScreens)
            {
                using (var fullImage = new Bitmap(screen.Bounds.Width,
                    screen.Bounds.Height))
                {
                    using (var g = Graphics.FromImage(fullImage))
                    {
                        g.CopyFromScreen(screen.Bounds.X,
                            screen.Bounds.Y,
                            0, 0,
                            fullImage.Size,
                            CopyPixelOperation.SourceCopy);
                    }
                    var maxTry = 10;
                    for (var i = 0; i < maxTry; i++)
                    {
                        var marginLeft = (int) ((double) fullImage.Width*i/2.5/maxTry);
                        var marginTop = (int) ((double) fullImage.Height*i/2.5/maxTry);
                        var cropRect = new Rectangle(marginLeft, marginTop, fullImage.Width - marginLeft*2,
                            fullImage.Height - marginTop*2);
                        var target = new Bitmap(screen.Bounds.Width, screen.Bounds.Height);

                        var imageScale = screen.Bounds.Width/(double) cropRect.Width;
                        using (var g = Graphics.FromImage(target))
                        {
                            g.DrawImage(fullImage, new Rectangle(0, 0, target.Width, target.Height),
                                cropRect,
                                GraphicsUnit.Pixel);
                        }
                        var source = new BitmapLuminanceSource(target);
                        var bitmap = new BinaryBitmap(new HybridBinarizer(source));
                        var reader = new QRCodeReader();
                        var result = reader.decode(bitmap);
                        if (result != null)
                        {
                            var success = _shadowsocksController.AddServerBySSURL(result.Text);
                            var splash = new QRCodeSplashForm();
                            if (success)
                            {
                                splash.FormClosed += splash_FormClosed;
                            }
                            else if (result.Text.StartsWith("http://") || result.Text.StartsWith("https://"))
                            {
                                _urlToOpen = result.Text;
                                splash.FormClosed += openURLFromQRCode;
                            }
                            else
                            {
                                MessageBox.Show(I18N.GetString("Failed to decode QRCode"));
                                return;
                            }
                            double minX = int.MaxValue, minY = int.MaxValue, maxX = 0, maxY = 0;
                            foreach (var point in result.ResultPoints)
                            {
                                minX = Math.Min(minX, point.X);
                                minY = Math.Min(minY, point.Y);
                                maxX = Math.Max(maxX, point.X);
                                maxY = Math.Max(maxY, point.Y);
                            }
                            minX /= imageScale;
                            minY /= imageScale;
                            maxX /= imageScale;
                            maxY /= imageScale;
                            // make it 20% larger
                            var margin = (maxX - minX)*0.20f;
                            minX += -margin + marginLeft;
                            maxX += margin + marginLeft;
                            minY += -margin + marginTop;
                            maxY += margin + marginTop;
                            splash.Location = new Point(screen.Bounds.X, screen.Bounds.Y);
                            // we need a panel because a window has a minimal size
                            // TODO: test on high DPI
                            splash.TargetRect = new Rectangle((int) minX + screen.Bounds.X, (int) minY + screen.Bounds.Y,
                                (int) maxX - (int) minX, (int) maxY - (int) minY);
                            splash.Size = new Size(fullImage.Width, fullImage.Height);
                            splash.Show();
                            return;
                        }
                    }
                }
            }
            MessageBox.Show(I18N.GetString("No QRCode found. Try to zoom in or move it to the center of the screen."));
        }

        private void splash_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShowConfigForm();
        }

        private void openURLFromQRCode(object sender, FormClosedEventArgs e)
        {
            Process.Start(_urlToOpen);
        }

        private void AutoStartupItem_Click(object sender, EventArgs e)
        {
            _autoStartupItem.Checked = !_autoStartupItem.Checked;
            if (!AutoStartup.Set(_autoStartupItem.Checked))
            {
                MessageBox.Show(I18N.GetString("Failed to update registry"));
            }
        }

        private void AvailabilityStatisticsItem_Click(object sender, EventArgs e)
        {
            _availabilityStatistics.Checked = !_availabilityStatistics.Checked;
            _shadowsocksController.ToggleAvailabilityStatistics(_availabilityStatistics.Checked);
        }

        private void LocalPACItem_Click(object sender, EventArgs e)
        {
            if (!_localPACItem.Checked)
            {
                _localPACItem.Checked = true;
                _onlinePACItem.Checked = false;
                _shadowsocksController.UseOnlinePAC(false);
                UpdatePACItemsEnabledStatus();
            }
        }

        private void OnlinePACItem_Click(object sender, EventArgs e)
        {
            if (!_onlinePACItem.Checked)
            {
                if (string.IsNullOrEmpty(_shadowsocksController.GetConfigurationCopy().pacUrl))
                {
                    UpdateOnlinePACURLItem_Click(sender, e);
                }
                if (!string.IsNullOrEmpty(_shadowsocksController.GetConfigurationCopy().pacUrl))
                {
                    _localPACItem.Checked = false;
                    _onlinePACItem.Checked = true;
                    _shadowsocksController.UseOnlinePAC(true);
                }
                UpdatePACItemsEnabledStatus();
            }
        }

        private void UpdateOnlinePACURLItem_Click(object sender, EventArgs e)
        {
            var origPacUrl = _shadowsocksController.GetConfigurationCopy().pacUrl;
            var pacUrl = Interaction.InputBox(
                I18N.GetString("Please input PAC Url"),
                I18N.GetString("Edit Online PAC URL"),
                origPacUrl, -1, -1);
            if (!string.IsNullOrEmpty(pacUrl) && pacUrl != origPacUrl)
            {
                _shadowsocksController.SavePACUrl(pacUrl);
            }
        }

        private void UpdatePACItemsEnabledStatus()
        {
            if (_localPACItem.Checked)
            {
                _editLocalPACItem.Enabled = true;
                _updateFromGFWListItem.Enabled = true;
                _editGFWUserRuleItem.Enabled = true;
                _editOnlinePACItem.Enabled = false;
            }
            else
            {
                _editLocalPACItem.Enabled = false;
                _updateFromGFWListItem.Enabled = false;
                _editGFWUserRuleItem.Enabled = false;
                _editOnlinePACItem.Enabled = true;
            }
        }
    }
}