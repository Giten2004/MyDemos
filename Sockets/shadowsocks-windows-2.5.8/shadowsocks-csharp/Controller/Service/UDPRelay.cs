using System;
using System.Collections.Generic;
using System.Text;
using Shadowsocks.Encryption;
using Shadowsocks.Model;
using System.Net.Sockets;
using System.Net;
using System.Runtime.CompilerServices;
using Shadowsocks.Controller.Strategy;

namespace Shadowsocks.Controller
{
    public class UDPHandler
    {
        private Socket _local;
        private Socket _remote;

        private Server _server;
        private byte[] _buffer = new byte[1500];

        private IPEndPoint _localEndPoint;
        private IPEndPoint _remoteEndPoint;

        public UDPHandler(Socket local, Server server, IPEndPoint localEndPoint)
        {
            _local = local;
            _server = server;
            _localEndPoint = localEndPoint;

            // TODO async resolving
            IPAddress ipAddress;
            bool parsed = IPAddress.TryParse(server.server, out ipAddress);
            if (!parsed)
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(server.server);
                ipAddress = ipHostInfo.AddressList[0];
            }
            _remoteEndPoint = new IPEndPoint(ipAddress, server.server_port);
            _remote = new Socket(_remoteEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

        }

        public void Send(byte[] data, int length)
        {
            IEncryptor encryptor = EncryptorFactory.GetEncryptor(_server.method, _server.password);

            //?what's the futrue of data
            byte[] dataIn = new byte[length - 3];
            Array.Copy(data, 3, dataIn, 0, length - 3);

            //?what's the futrue of data
            byte[] dataOut = new byte[length - 3 + 16];
            int outlen;
            encryptor.Encrypt(dataIn, dataIn.Length, dataOut, out outlen);

            _remote.SendTo(dataOut, _remoteEndPoint);
        }

        public void Receive()
        {
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _remote.BeginReceiveFrom(_buffer, 0, _buffer.Length, 0, ref remoteEndPoint, new AsyncCallback(RecvFromCallback), null);
        }

        private void RecvFromCallback(IAsyncResult ar)
        {
            try
            {
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                int bytesRead = _remote.EndReceiveFrom(ar, ref remoteEndPoint);

                byte[] dataOut = new byte[bytesRead];
                int outlen;

                IEncryptor encryptor = EncryptorFactory.GetEncryptor(_server.method, _server.password);
                encryptor.Decrypt(_buffer, bytesRead, dataOut, out outlen);

                byte[] sendBuf = new byte[outlen + 3];
                Array.Copy(dataOut, 0, sendBuf, 3, outlen);

                _local.SendTo(sendBuf, outlen + 3, 0, _localEndPoint);

                Receive();
            }
            catch (ObjectDisposedException)
            {
                // TODO: handle the ObjectDisposedException
            }
            catch (Exception)
            {
                // TODO: need more think about handle other Exceptions, or should remove this catch().
            }
            finally
            {
            }
        }

        public void Close()
        {
            try
            {
                _remote.Close();
            }
            catch (ObjectDisposedException)
            {
                // TODO: handle the ObjectDisposedException
            }
            catch (Exception)
            {
                // TODO: need more think about handle other Exceptions, or should remove this catch().
            }
            finally
            {
            }
        }
    }

    class UDPRelay : IService
    {
        private ShadowsocksController _controller;
        private LRUCache<IPEndPoint, UDPHandler> _cache;

        public UDPRelay(ShadowsocksController controller)
        {
            this._controller = controller;
            this._cache = new LRUCache<IPEndPoint, UDPHandler>(512);  // todo: choose a smart number
        }

        public bool Handle(byte[] firstPacket, int length, Socket socket, object state)
        {
            if (socket.ProtocolType != ProtocolType.Udp)
            {
                return false;
            }

            //?What feature ?
            if (length < 4)
            {
                return false;
            }

            UDPState udpState = (UDPState)state;
            IPEndPoint remoteEndPoint = (IPEndPoint)udpState.RemoteEndPoint;

            UDPHandler handler = _cache.Get(remoteEndPoint);
            if (handler == null)
            {
                handler = new UDPHandler(socket, _controller.GetAServer(IStrategyCallerType.UDP, remoteEndPoint), remoteEndPoint);
                _cache.Add(remoteEndPoint, handler);
            }

            handler.Send(firstPacket, length);
            handler.Receive();

            return true;
        }
    }


    // cc by-sa 3.0 http://stackoverflow.com/a/3719378/1124054
    //LRU = Least Recently Used
    class LRUCache<K, V> where V : UDPHandler
    {
        private int _capacity;
        private Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>> _cacheMap = new Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>>();
        private LinkedList<LRUCacheItem<K, V>> _lruList = new LinkedList<LRUCacheItem<K, V>>();

        public LRUCache(int capacity)
        {
            this._capacity = capacity;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public V Get(K key)
        {
            LinkedListNode<LRUCacheItem<K, V>> node;
            if (_cacheMap.TryGetValue(key, out node))
            {
                V value = node.Value.Value;
                _lruList.Remove(node);
                _lruList.AddLast(node);
                return value;
            }
            return default(V);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(K key, V val)
        {
            if (_cacheMap.Count >= _capacity)
            {
                RemoveFirst();
            }

            LRUCacheItem<K, V> cacheItem = new LRUCacheItem<K, V>(key, val);
            LinkedListNode<LRUCacheItem<K, V>> node = new LinkedListNode<LRUCacheItem<K, V>>(cacheItem);

            _lruList.AddLast(node);
            _cacheMap.Add(key, node);
        }

        private void RemoveFirst()
        {
            // Remove from LRUPriority
            LinkedListNode<LRUCacheItem<K, V>> node = _lruList.First;
            _lruList.RemoveFirst();

            // Remove from cache
            _cacheMap.Remove(node.Value.Key);
            node.Value.Value.Close();
        }
    }

    class LRUCacheItem<K, V>
    {
        public LRUCacheItem(K key, V value)
        {
            Key = key;
            this.Value = value;
        }

        public K Key { get; private set; }
        public V Value { get; private set; }
    }
}
