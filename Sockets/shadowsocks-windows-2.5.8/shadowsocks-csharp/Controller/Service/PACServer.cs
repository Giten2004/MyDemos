﻿using Shadowsocks.Model;
using Shadowsocks.Properties;
using Shadowsocks.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Shadowsocks.Controller
{
    class PACServer : IService
    {
        public static string PAC_FILE = "pac.txt";
        public static string USER_RULE_FILE = "user-rule.txt";

        private FileSystemWatcher _fileSystemWatcher;
        private Configuration _config;

        public event EventHandler PACFileChanged;

        public PACServer()
        {
            this.WatchPacFile();
        }

        #region Implement methods of interface IService

        public bool Handle(byte[] firstPacket, int length, Socket socket, object state)
        {
            if (socket.ProtocolType != ProtocolType.Tcp)
            {
                return false;
            }

            try
            {
                string httpRequestContent = Encoding.UTF8.GetString(firstPacket, 0, length);
                string[] lines = httpRequestContent.Split('\r', '\n');

                bool hostMatch = false;
                bool pathMatch = false;
                bool useSocks = false;

                foreach (string line in lines)
                {
                    string[] kv = line.Split(new char[] { ':' }, 2);
                    if (kv.Length == 2)
                    {
                        if (kv[0] == "Host")
                        {
                            if (kv[1].Trim() == ((IPEndPoint)socket.LocalEndPoint).ToString())
                            {
                                hostMatch = true;
                            }
                        }
                        else if (kv[0] == "User-Agent")
                        {
                            // we need to drop connections when changing servers
                            /* if (kv[1].IndexOf("Chrome") >= 0)
                            {
                                useSocks = true;
                            } */
                        }
                    }
                    else if (kv.Length == 1)
                    {
                        if (line.IndexOf("pac") >= 0)
                        {
                            pathMatch = true;
                        }
                    }
                }//end of foreach

                if (hostMatch && pathMatch)
                {
                    SendResponse(firstPacket, length, socket, useSocks);
                    return true;
                }
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        #endregion

        public void UpdateConfiguration(Configuration config)
        {
            this._config = config;
        }

        public string TouchPACFile()
        {
            if (File.Exists(PAC_FILE))
            {
                return PAC_FILE;
            }
            else
            {
                FileManager.UncompressFile(PAC_FILE, Resources.proxy_pac_txt);
                return PAC_FILE;
            }
        }

        internal string TouchUserRuleFile()
        {
            if (File.Exists(USER_RULE_FILE))
            {
                return USER_RULE_FILE;
            }
            else
            {
                File.WriteAllText(USER_RULE_FILE, Resources.user_rule);
                return USER_RULE_FILE;
            }
        }

        private void SendResponse(byte[] firstPacket, int length, Socket socket, bool useSocks)
        {
            try
            {
                string pac = GetPACContent();

                IPEndPoint localEndPoint = (IPEndPoint)socket.LocalEndPoint;

                string proxy = GetPACAddress(firstPacket, length, localEndPoint, useSocks);

                pac = pac.Replace("__PROXY__", proxy);

                string text = String.Format(@"HTTP/1.1 200 OK
Server: Shadowsocks
Content-Type: application/x-ns-proxy-autoconfig
Content-Length: {0}
Connection: Close

", System.Text.Encoding.UTF8.GetBytes(pac).Length) + pac;

                byte[] response = System.Text.Encoding.UTF8.GetBytes(text);
                socket.BeginSend(response, 0, response.Length, 0, new AsyncCallback(SendCallback), socket);

                Util.Utils.ReleaseMemory(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                socket.Close();
            }
        }

        private string GetPACContent()
        {
            if (File.Exists(PAC_FILE))
            {
                return File.ReadAllText(PAC_FILE, Encoding.UTF8);
            }
            else
            {
                return Utils.UnGzip(Resources.proxy_pac_txt);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket conn = (Socket)ar.AsyncState;
            try
            {
                conn.Shutdown(SocketShutdown.Send);
            }
            catch
            { }
        }

        private void WatchPacFile()
        {
            if (_fileSystemWatcher != null)
            {
                _fileSystemWatcher.Dispose();
            }

            _fileSystemWatcher = new FileSystemWatcher(Directory.GetCurrentDirectory());
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _fileSystemWatcher.Filter = PAC_FILE;
            _fileSystemWatcher.Changed += FileSystemWatcherChanged;
            _fileSystemWatcher.Created += FileSystemWatcherChanged;
            _fileSystemWatcher.Deleted += FileSystemWatcherChanged;
            _fileSystemWatcher.Renamed += FileSystemWatcherChanged;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            if (PACFileChanged != null)
            {
                PACFileChanged(this, EventArgs.Empty);
            }
        }

        private string GetPACAddress(byte[] requestBuf, int length, IPEndPoint localEndPoint, bool useSocks)
        {
            //try
            //{
            //    string requestString = Encoding.UTF8.GetString(requestBuf);
            //    if (requestString.IndexOf("AppleWebKit") >= 0)
            //    {
            //        string address = "" + localEndPoint.Address + ":" + config.GetCurrentServer().local_port;
            //        proxy = "SOCKS5 " + address + "; SOCKS " + address + ";";
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            return (useSocks ? "SOCKS5 " : "PROXY ") + localEndPoint.Address + ":" + this._config.localPort + ";";
        }
    }
}
