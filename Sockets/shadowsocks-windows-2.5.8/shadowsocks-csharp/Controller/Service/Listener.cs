using Shadowsocks.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Shadowsocks.Controller
{
    public class UDPState
    {
        public byte[] Buffer = new byte[4096];
        public EndPoint RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    public class Listener
    {
        private Configuration _config;
        private bool _shareOverLAN;
        private Socket _tcpSocket;
        private Socket _udpSocket;
        private IList<IService> _services;

        public Listener(IList<IService> services)
        {
            _services = services;
        }

        public void Start(Configuration config)
        {
            this._config = config;
            this._shareOverLAN = config.shareOverLan;

            if (CheckIfPortInUse(_config.localPort))
                throw new Exception(I18N.GetString("Port already in use"));

            try
            {
                // Create a TCP/IP socket.
                _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _tcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                IPEndPoint localEndPoint = null;
                if (_shareOverLAN)
                {
                    localEndPoint = new IPEndPoint(IPAddress.Any, _config.localPort);
                }
                else
                {
                    localEndPoint = new IPEndPoint(IPAddress.Loopback, _config.localPort);
                }

                // Bind the socket to the local endpoint and listen for incoming connections.
                _tcpSocket.Bind(localEndPoint);
                _udpSocket.Bind(localEndPoint);

                _tcpSocket.Listen(1024);

                // Start an asynchronous socket to listen for connections.
                Console.WriteLine("Shadowsocks started");
                _tcpSocket.BeginAccept(new AsyncCallback(TcpSocketAcceptCallback), _tcpSocket);

                UDPState udpState = new UDPState();
                _udpSocket.BeginReceiveFrom(udpState.Buffer, 0, udpState.Buffer.Length, 
                                            SocketFlags.None, 
                                            ref udpState.RemoteEndPoint, 
                                            new AsyncCallback(UDPRecvFromCallback), 
                                            udpState);
            }
            catch (SocketException)
            {
                _tcpSocket.Close();
                throw;
            }
        }

        public void Stop()
        {
            if (_tcpSocket != null)
            {
                _tcpSocket.Close();
                _tcpSocket = null;
            }

            if (_udpSocket != null)
            {
                _udpSocket.Close();
                _udpSocket = null;
            }
        }

        private bool CheckIfPortInUse(int port)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    return true;
                }
            }
            return false;
        }

        private void UDPRecvFromCallback(IAsyncResult ar)
        {
            UDPState state = (UDPState)ar.AsyncState;
            try
            {
                int bytesRead = _udpSocket.EndReceiveFrom(ar, ref state.RemoteEndPoint);

                foreach (IService service in _services)
                {
                    if (service.Handle(state.Buffer, bytesRead, _udpSocket, state))
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                try
                {
                    _udpSocket.BeginReceiveFrom(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ref state.RemoteEndPoint, new AsyncCallback(UDPRecvFromCallback), state);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void TcpSocketAcceptCallback(IAsyncResult asyncResult)
        {
            Socket listenerSocket = (Socket)asyncResult.AsyncState;
            try
            {
                Socket conn = listenerSocket.EndAccept(asyncResult);

                byte[] buf = new byte[4096];
                object[] state = new object[] { conn, buf };

                conn.BeginReceive(buf, 0, buf.Length, SocketFlags.None, new AsyncCallback(TcpReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                try
                {
                    listenerSocket.BeginAccept(new AsyncCallback(TcpSocketAcceptCallback), listenerSocket);
                }
                catch (Exception e)
                {
                    Logging.LogUsefulException(e);
                }
            }
        }

        private void TcpReceiveCallback(IAsyncResult ar)
        {
            object[] state = (object[])ar.AsyncState;

            Socket conn = (Socket)state[0];
            byte[] buf = (byte[])state[1];

            try
            {
                int bytesRead = conn.EndReceive(ar);

                foreach (IService service in _services)
                {
                    if (service.Handle(buf, bytesRead, conn, null))
                    {
                        return;
                    }
                }

                // no service found for this
                if (conn.ProtocolType == ProtocolType.Tcp)
                {
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                conn.Close();
            }
        }
    }
}
