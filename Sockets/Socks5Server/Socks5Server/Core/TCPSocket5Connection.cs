/* ***********************************************
 * Author		:  kingthy
 * Email		:  kingthy@gmail.com
 * DateTime		:  2008-10-22 13:03:23
 * Description	:  TCPSocks5Connection 的摘要说明
 *
 * ***********************************************/

using System;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading;
namespace Kingthy.Test.Socks5.Server.Core
{
    /// <summary>
    /// TCPSocks5Connection
    /// </summary>
    public class TCPSocks5Connection
    {
        /// <summary>
        /// TCPSocks5Connection
        /// </summary>
        public TCPSocks5Connection(TCPSocks5Server server, Socket client)
        {
            this.Server = server;
            this.Client = client;
        }
        /// <summary>
        /// 服务端
        /// </summary>
        private TCPSocks5Server Server
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端
        /// </summary>
        internal Socket Client
        {
            private set;
            get;
        }

        /// <summary>
        /// 被连接的远程端地址
        /// </summary>
        private IPEndPoint RemoteEndPoint
        {
            get;
            set;
        }
        /// <summary>
        /// 处理请求
        /// </summary>
        private void DoRequest(object state)
        {
            if (this.Server.IsStarting)
            {
                if (!this.DoShakeHands())
                {
                    goto __CLOSE;
                }
                if (this.Server.RequireValidate)
                {
                    if (!this.ValidateIdentity())
                    {
                        //身份验证失败
                        goto __CLOSE;
                    }
                }
                if (!this.DoProtocolRequest())
                {
                    //处理连接请求
                    goto __CLOSE;
                }
                //建立代理
                this.CreateProxyBridge();
                goto __EXIT;
            }
        __CLOSE:
            this.Close();
        __EXIT:
            return;
        }

        /// <summary>
        /// 处理握手
        /// </summary>
        private bool DoShakeHands()
        {
            byte[] buffer;
            byte method = 0xFF; //命令不支持
            if (SocketUtils.Receive(this.Client, 2, out buffer))
            {
                if (buffer.Length == 2)
                {
                    //取得认证方法列
                    if (SocketUtils.Receive(this.Client, (uint)buffer[1], out buffer))
                    {
                        if (this.Server.RequireValidate)
                        {
                            //需要验证身份,所以判断客户端是否支持用户名与密码验证
                            foreach (byte b in buffer)
                            {
                                if (b == 0x02) method = 0x02;   //客户端支持户名与密码验证
                            }
                        }
                        else
                        {
                            //不需要验证身份
                            method = 0x00;
                        }
                    }
                }
            }
            //发送应答
            SocketUtils.Send(this.Client, new byte[] { 0x05, method });
            return (method != 0xFF);
        }

        /// <summary>
        /// 处理身份验证
        /// </summary>
        private bool ValidateIdentity()
        {
            byte[] buffer;
            byte ep = 0xFF;
            string username = string.Empty, password = string.Empty ;

            //报文格式:0x01 | 用户名长度（1字节）| 用户名（长度根据用户名长度域指定） | 口令长度（1字节） | 口令（长度由口令长度域指定）
            if (SocketUtils.Receive(this.Client, 2, out buffer))
            {
                if (buffer.Length == 2)
                {
                    //用户名为空
                    if (buffer[1] == 0x00)
                    {
                        if (string.IsNullOrEmpty(this.Server.UserName))
                        {
                            ep = 0x00;  //用户名为空
                        }
                    }
                    else
                    {
                        if (SocketUtils.Receive(this.Client, (uint)buffer[1], out buffer))
                        {
                            username = Encoding.ASCII.GetString(buffer);
                            if (!string.IsNullOrEmpty(this.Server.UserName))
                            {
                                ep = (byte)(username.Equals(this.Server.UserName) ? 0x00 : 0xFF);
                            }
                        }
                    }
                    if (ep == 0x00)
                    {
                        ep = 0xFF;
                        //判断密码
                        if (SocketUtils.Receive(this.Client, 1, out buffer))
                        {
                            if (buffer[0] == 0x00)
                            {
                                if (!string.IsNullOrEmpty(this.Server.Password))
                                {
                                    ep = 0x00;  //密码为空
                                }
                            }
                            else
                            {
                                if (SocketUtils.Receive(this.Client, (uint)buffer[0], out buffer))
                                {
                                    password = Encoding.ASCII.GetString(buffer);
                                    if (!string.IsNullOrEmpty(this.Server.Password))
                                    {
                                        ep = (byte)(password.Equals(this.Server.Password) ? 0x00 : 0xFF);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //输出应答
            SocketUtils.Send(this.Client, new byte[] { 0x01, ep });
            this.Server.WriteLineLog(string.Format("于{0}接收到客户端{1}的身份验证请求[UserName={3},Password={4}],结果:{2}...",
                DateTime.Now, this.Client.RemoteEndPoint, (ep == 0x00 ? "通过" : "失败") , username, password));
            return (ep == 0x00);
        }

        /// <summary>
        /// 处理协议请求
        /// </summary>
        private bool DoProtocolRequest()
        {
            //取前4字节
            byte[] buffer;
            IPAddress ipAddress = null;
            byte rep = 0x07;            //不支持的命令
            if (SocketUtils.Receive(this.Client, 4, out buffer))
            {
                if (buffer.Length == 4)
                {
                    //判断地址类型
                    switch (buffer[3])
                    {
                        case 0x01:
                            //IPV4
                            if (SocketUtils.Receive(this.Client, 4, out buffer))
                            {
                                ipAddress = new IPAddress(buffer);
                            }
                            break;
                        case 0x03:
                            //域名
                            if (SocketUtils.Receive(this.Client, 1, out buffer))
                            {
                                //取得域名的长度
                                if (SocketUtils.Receive(this.Client, (uint)(buffer[0]), out buffer))
                                {
                                    //取得域名地址
                                    string address = Encoding.ASCII.GetString(buffer);
                                    IPAddress[] addresses = Dns.GetHostAddresses(address);
                                    if (addresses.Length != 0)
                                    {
                                        ipAddress = addresses[0];
                                    }
                                    else
                                    {
                                        rep = 0x04;  //主机不可达
                                    }
                                }
                            }
                            break;
                        case 0x04:
                            //IPV6;
                            if (SocketUtils.Receive(this.Client, 16, out buffer))
                            {
                                ipAddress = new IPAddress(buffer);
                            }
                            break;
                        default:
                            rep = 0x08; //不支持的地址类型
                            break;
                    }
                }
            }

            if (ipAddress != null && rep == 0x07)
            {
                //取得端口号
                if (SocketUtils.Receive(this.Client, 2, out buffer))
                {
                    Array.Reverse(buffer);  //反转端口值
                    this.RemoteEndPoint = new IPEndPoint(ipAddress, BitConverter.ToUInt16(buffer, 0));
                    rep = 0x00;
                    this.Server.WriteLineLog(string.Format("于{0}接收到客户端要求对主机{1}进行连接的请求....", DateTime.Now, this.RemoteEndPoint));
                }
            }

            //输出应答
            MemoryStream stream = new MemoryStream();
            stream.WriteByte(0x05);
            stream.WriteByte(rep);
            stream.WriteByte(0x00);
            stream.WriteByte(0x01);
            IPEndPoint localEP = (IPEndPoint)Client.LocalEndPoint;
            byte[] localIP = localEP.Address.GetAddressBytes();
            stream.Write(localIP,0,localIP.Length);
            byte[] localPort = BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder(localEP.Port));
            stream.Write(localPort, 0, localPort.Length);
            SocketUtils.Send(this.Client, stream.ToArray());

            return (this.RemoteEndPoint != null);
        }

        #region 建立代理桥
        private byte[] _ClientBuffer;
        private byte[] _ProxyBuffer;
        /// <summary>
        /// 代理
        /// </summary>
        private TcpClient Proxy
        {
            get;
            set;
        }

        /// <summary>
        /// 建立代理桥
        /// </summary>
        private void CreateProxyBridge()
        {
            if (this.Client.Connected)
            {
                this.Proxy = new TcpClient();
                try
                {
                    this.Proxy.Connect(this.RemoteEndPoint);
                    if (this.Proxy.Connected)
                    {
                        _ClientBuffer = new byte[this.Client.ReceiveBufferSize];
                        _ProxyBuffer = new byte[this.Proxy.ReceiveBufferSize];
                        this.Client.BeginReceive(_ClientBuffer, 0, _ClientBuffer.Length, SocketFlags.None, this.OnClientReceive, this.Client);
                        this.Proxy.Client.BeginReceive(_ProxyBuffer, 0, _ProxyBuffer.Length, SocketFlags.None, this.OnProxyReceive, this.Proxy.Client);
                    }
                    else
                    {
                        this.Close();
                    }
                }
                catch
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }
        /// <summary>
        /// 当接收到客户端数据时转发给代理
        /// </summary>
        /// <param name="result"></param>
        private void OnClientReceive(IAsyncResult result)
        {
            if (this.Server.IsStarting)
            {
                try
                {
                    Socket socket = result.AsyncState as Socket;
                    SocketError error;
                    int size = this.Client.EndReceive(result, out error);
                    if (size > 0)
                    {
                        //转发给代理
                        SocketUtils.Send(this.Proxy.Client, this._ClientBuffer, 0, size);
                        //this.Server.WriteLineLog(string.Format("从客户端{0}接收到数据,大小{1} Byte", this.Client.RemoteEndPoint, size));
                        if (this.Server.IsStarting)
                            this.Client.BeginReceive(_ClientBuffer, 0, _ClientBuffer.Length, SocketFlags.None, this.OnClientReceive, this.Client);

                    }
                    else
                    {
                        //客户端已断开连接
                        this.Close();
                    }
                }
                catch
                {
                    this.Close();
                }
            }
        }
        /// <summary>
        /// 当代理接收到数据时转发给客户端
        /// </summary>
        /// <param name="result"></param>
        private void OnProxyReceive(IAsyncResult result)
        {
            if (this.Server.IsStarting)
            {
                try
                {
                    Socket socket = result.AsyncState as Socket;
                    SocketError error;
                    int size = socket.EndReceive(result, out error);
                    if (size > 0)
                    {
                        //转发给客户端
                        SocketUtils.Send(this.Client, _ProxyBuffer, 0, size);
                        //this.Server.WriteLineLog(string.Format("从服务端{0}接收到数据,大小{1} Byte", socket.RemoteEndPoint, size));
                        if (this.Server.IsStarting)
                            socket.BeginReceive(_ProxyBuffer, 0, _ProxyBuffer.Length, SocketFlags.None, this.OnProxyReceive, socket);

                    }
                    else
                    {
                        //服务端已断开连接
                        this.Close();
                    }
                }
                catch
                {
                    this.Close();
                }
            }
        }
        #endregion

        /// <summary>
        /// 关闭连接
        /// </summary>
        private void Close()
        {
            if (this.Client != null)
            {
                this.Server.WriteLineLog(string.Format("已于{0}关闭与客户端{1}的连接...", DateTime.Now, this.Client.RemoteEndPoint));
                this.Client.Close(3);
                this.Client = null;
            }
            if (this.Proxy != null)
            {
                this.Server.WriteLineLog(string.Format("代理端已于{0}关闭与远程服务器{1}的连接...", DateTime.Now, this.Proxy.Client.RemoteEndPoint));
                this.Proxy.Close();
                this.Proxy = null;
            }
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="server"></param>
        /// <param name="client"></param>
        public static void DoRequest(TCPSocks5Server server, Socket client)
        {
            TCPSocks5Connection connection = new TCPSocks5Connection(server, client);
            ThreadPool.QueueUserWorkItem(new WaitCallback(connection.DoRequest));
        }
    }
}
