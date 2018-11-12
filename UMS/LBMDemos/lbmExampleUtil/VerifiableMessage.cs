
/*
  Copyright (c) 2005-2013 Informatica Corporation  Permission is granted to licensees to use
  or alter this software for any purpose, including commercial applications,
  according to the terms laid out in the Software License Agreement.

  This source code example is provided by Informatica for educational
  and evaluation purposes only.

  THE SOFTWARE IS PROVIDED "AS IS" AND INFORMATICA DISCLAIMS ALL WARRANTIES 
  EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION, ANY IMPLIED WARRANTIES OF 
  NON-INFRINGEMENT, MERCHANTABILITY OR FITNESS FOR A PARTICULAR 
  PURPOSE.  INFORMATICA DOES NOT WARRANT THAT USE OF THE SOFTWARE WILL BE 
  UNINTERRUPTED OR ERROR-FREE.  INFORMATICA SHALL NOT, UNDER ANY CIRCUMSTANCES, BE 
  LIABLE TO LICENSEE FOR LOST PROFITS, CONSEQUENTIAL, INCIDENTAL, SPECIAL OR 
  INDIRECT DAMAGES ARISING OUT OF OR RELATED TO THIS AGREEMENT OR THE 
  TRANSACTIONS CONTEMPLATED HEREUNDER, EVEN IF INFORMATICA HAS BEEN APPRISED OF 
  THE LIKELIHOOD OF SUCH DAMAGES.
*/
using System;

namespace LBMApplication
{
    public class VerifiableMessage
    {
        private static readonly byte[] MAGIC_NUMBER = { (byte)0xab, (byte)0x33, (byte)0x56, (byte)0xda };
        private static readonly int MAGIC_NUMBER_LEN = MAGIC_NUMBER.Length;
        public static readonly int MINIMUM_VERIFIABLE_MSG_LEN = 4 + MAGIC_NUMBER_LEN;
        private static Random GENERATOR = new Random();

        public static byte[] constructVerifiableMessage(int len)
        {
            // bytes 0 and 1 are for the checksum
            // bytes 2 through 5 are for the 4 byte magic number
            // remaining bytes are generated randomly
            byte[] message = new byte[len];
            long cksum;

            Array.Copy(MAGIC_NUMBER, 0, message, 2, 4);

            for (int i = 6; i < len; i++)
                message[i] = (byte)(GENERATOR.Next() & 0xff);

            cksum = VerifiableMessage.inet_cksum(message, len);

            message[0] = (byte)((cksum >> 8) & 0xff);
            message[1] = (byte)(cksum & 0xff);


            return message;
        }

        public static int verifyMessage(byte[] data, int len, bool verbose)
        {
            long calced_cksum = 0;

            if (len < MINIMUM_VERIFIABLE_MSG_LEN)
                return -1; // too small to be a verifiable msg

            calced_cksum = VerifiableMessage.inet_cksum(data, len);

            if (verbose)
                System.Console.WriteLine("Calculated cksum = " + calced_cksum);

            if (calced_cksum == 0)
                return 1; // success


            for (int i = 0; i < 4; i++)
            {
                if (data[i + 2] != MAGIC_NUMBER[i])
                    return -1; // no magic number found, not a verifiable message - failed
            }

            return 0; // magic number found, but bad checksum - failed
        }

        private static long inet_cksum(byte[] data, int len)
        {
            int nleft = len;
            int idx = 0;
            long sum = 0;

            while (nleft > 1)
            {
                sum = sum + ((long)((((short)data[idx]) & 0xff) << 8)) + ((long)(((short)data[idx + 1]) & 0xff));
                idx += 2;

                if ((sum & 0x80000000) != 0)
                    sum = (sum >> 16) + (sum & 0xffff);

                nleft -= 2;
            }

            if (nleft == 1)
                sum += ((long)(((short)data[idx] & 0xff) << 8));

            while ((sum >> 16) != 0)
                sum = (sum >> 16) + (sum & 0xffff);

            return (~sum & 0xffff);
        }
    }
}
