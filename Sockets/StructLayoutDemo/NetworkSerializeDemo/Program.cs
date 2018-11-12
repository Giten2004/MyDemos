using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkSerializeDemo.MessageDemo;

namespace NetworkSerializeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //tosend
            var heatbeatMsg = new HereMessage();
            var heatBeatpackage = heatbeatMsg.ToDataPackage();

            //received
            var response = new byte[10];
            var messagename = response.ExtGetMessageHeader();
            switch (messagename)
            {
                case "EROR":
                    var eror = response.ToObjectPackage<ErorMessage>();
                    break;
                default:
                    break;
            }
        }
    }

    public static class Utility
    {
        private static byte[] ExtCutTheByteArray(this byte[] source, int length)
        {
            var destarray = new byte[length];
            Array.Copy(source, destarray, length);
            return destarray;
        }

        /// <summary>
        /// get the message head and ip,port
        /// </summary>
        /// <param name="message">header bytearray</param>
        /// <returns></returns>
        public static string ExtGetMessageHeader(this byte[] message)
        {
            var messagesize = message.ExtCutTheByteArray(4);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(messagesize);
            }

            var messagetype = Encoding.ASCII.GetString(message, 4, 4);

            return CheckMessageSize(messagetype, message) ? messagetype : string.Empty;
        }

        /// <summary>
        /// size validation
        /// </summary>
        /// <param name="messagetype"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool CheckMessageSize(string messagetype, byte[] message)
        {
            switch (messagetype)
            {
              
                case "EROR":
                    return ErorMessage.CheckMessageSize(message);
               
            }
            return false;
        }
    }
}
