using Shadowsocks.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Shadowsocks.Controller.Strategy
{
    public enum IStrategyCallerType
    {
        TCP,
        UDP
    }

    /*
     * IStrategy
     *
     * Subclasses must be thread-safe
     */
    public interface IStrategy
    {
        string Name { get; }

        string ID { get; }

        /// <summary>
        ///  Called when servers need to be reloaded, i.e. new configuration saved
        /// </summary>
        void ReloadServers();

        /// <summary>
        /// Get a new server to use in TCPRelay or UDPRelay
        /// </summary>
        /// <param name="type"></param>
        /// <param name="localIPEndPoint"></param>
        /// <returns></returns>
        Server GetAServer(IStrategyCallerType type, IPEndPoint localIPEndPoint);

        /// <summary>
        /// TCPRelay will call this when latency of a server detected
        /// </summary>
        /// <param name="server"></param>
        /// <param name="latency"></param>
        void UpdateLatency(Server server, TimeSpan latency);

        /// <summary>
        /// TCPRelay will call this when reading from a server
        /// </summary>
        /// <param name="server"></param>
        void UpdateLastRead(Server server);

        /// <summary>
        /// TCPRelay will call this when writing to a server
        /// </summary>
        /// <param name="server"></param>
        void UpdateLastWrite(Server server);

        /// <summary>
        /// TCPRelay will call this when fatal failure detected
        /// </summary>
        /// <param name="server"></param>
        void SetFailure(Server server);
    }
}
