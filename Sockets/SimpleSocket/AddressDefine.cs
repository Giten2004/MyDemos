using System;

namespace UDPMulticasting
{
    public static class AddressDefine
    {
        public const string MulticastIP = @"224.5.6.7";
        public const string MulticastPort = @"4567";
        /// <summary>
        /// This sets the time to live for the socket - this is very important in defining scope for the multicast data. 
        /// Setting a value of 1 will mean the multicast data will not leave the local network, 
        /// setting it to anything above this will allow the multicast data to pass through several routers, 
        /// with each router decrementing the TTL by 1. 
        /// Getting the TTL value right is important for bandwidth considerations.
        /// </summary>
        public const int TTL = 10;
    }
}
