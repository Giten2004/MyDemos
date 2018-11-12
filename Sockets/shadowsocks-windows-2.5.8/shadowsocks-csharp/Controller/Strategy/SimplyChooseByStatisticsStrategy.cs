﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Shadowsocks.Model;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace Shadowsocks.Controller.Strategy
{
    public class StatisticsData
    {
        public int SuccessTimes { get; set; }
        public int TimedOutTimes { get; set; }
        public int AverageResponse { get; set; }
        public int MinResponse { get; set; }
        public int MaxResponse { get; set; }
    }

    class SimplyChooseByStatisticsStrategy : IStrategy
    {
        private ShadowsocksController _controller;
        private Server _currentServer;
        private Timer _timer;
        /*
return a dict:
{
    'ServerFriendlyName1':StatisticsData,
    'ServerFriendlyName2':...
}
*/
        private Dictionary<string, StatisticsData> _statisticsDictionary;

        private static readonly int CachedInterval = 30 * 60 * 1000; //choose a new server every 30 minutes

        public SimplyChooseByStatisticsStrategy(ShadowsocksController controller)
        {
            _controller = controller;
            var servers = controller.GetCurrentConfiguration().configs;
            int randomIndex = new Random().Next() % servers.Count();
            _currentServer = servers[randomIndex];  //choose a server randomly at first
            _timer = new Timer(ReloadStatisticsAndChooseAServer);
        }

        private void ReloadStatisticsAndChooseAServer(object obj)
        {
            Logging.Debug("Reloading statistics and choose a new server....");

            List<Server> servers = _controller.GetCurrentConfiguration().configs;

            LoadStatistics();
            ChooseNewServer(servers);
        }

        private void LoadStatistics()
        {
            try
            {
                var path = AvailabilityStatistics.AvailabilityStatisticsFile;
                Logging.Debug(string.Format("loading statistics from{0}", path));
                _statisticsDictionary = (from l in File.ReadAllLines(path)
                                  .Skip(1)
                              let strings = l.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                              let rawData = new
                              {
                                  ServerName = strings[1],
                                  IPStatus = strings[2],
                                  RoundtripTime = int.Parse(strings[3])
                              }
                              group rawData by rawData.ServerName into server
                              select new
                              {
                                  ServerName = server.Key,
                                  data = new StatisticsData
                                  {
                                      SuccessTimes = server.Count(data => IPStatus.Success.ToString().Equals(data.IPStatus)),
                                      TimedOutTimes = server.Count(data => IPStatus.TimedOut.ToString().Equals(data.IPStatus)),
                                      AverageResponse = Convert.ToInt32(server.Average(data => data.RoundtripTime)),
                                      MinResponse = server.Min(data => data.RoundtripTime),
                                      MaxResponse = server.Max(data => data.RoundtripTime)
                                  }
                              }).ToDictionary(server => server.ServerName, server => server.data);
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
            }
        }

        //return the score by data
        //server with highest score will be choosen
        private static double GetScore(StatisticsData data)
        {
            return (double)data.SuccessTimes / (data.SuccessTimes + data.TimedOutTimes); //simply choose min package loss
        }

        private void ChooseNewServer(List<Server> servers)
        {
            if (_statisticsDictionary == null)
            {
                return;
            }
            try
            {
                var bestResult = (from server in servers
                                  let name = server.FriendlyName()
                                  where _statisticsDictionary.ContainsKey(name)
                                  select new
                                  {
                                      server,
                                      score = GetScore(_statisticsDictionary[name])
                                  }
                                  ).Aggregate((result1, result2) => result1.score > result2.score ? result1 : result2);

                if (_controller.GetCurrentStrategy().ID == ID && _currentServer != bestResult.server) //output when enabled
                {
                    Console.WriteLine("Switch to server: {0} by package loss:{1}", bestResult.server.FriendlyName(), 1 - bestResult.score);
                }
                _currentServer = bestResult.server;
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
            }
        }

        #region Implement methods of interface IStrategy

        public string ID
        {
            get { return "com.shadowsocks.strategy.scbs"; }
        }

        public string Name
        {
            get { return I18N.GetString("Choose By Total Package Loss"); }
        }

        public void ReloadServers()
        {
            ChooseNewServer(_controller.GetCurrentConfiguration().configs);
            _timer?.Change(0, CachedInterval);
        }

        public Server GetAServer(IStrategyCallerType type, IPEndPoint localIPEndPoint)
        {
            var oldServer = _currentServer;
            if (oldServer == null)
            {
                ChooseNewServer(_controller.GetCurrentConfiguration().configs);
            }
            if (oldServer != _currentServer)
            {
            }
            return _currentServer;  //current server cached for CachedInterval
        }

        public void UpdateLatency(Server server, TimeSpan latency)
        {
            //TODO: combine this part of data with ICMP statics
        }

        public void UpdateLastRead(Server server)
        {
            //TODO: combine this part of data with ICMP statics
        }

        public void UpdateLastWrite(Server server)
        {
            //TODO: combine this part of data with ICMP statics
        }

        public void SetFailure(Server server)
        {
            Logging.Debug(String.Format("failure: {0}", server.FriendlyName()));
        }
        #endregion
    }
}
