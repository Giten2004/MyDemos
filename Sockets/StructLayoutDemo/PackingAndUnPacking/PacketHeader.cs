using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PackingAndUnPacking
{
    /// <summary>
    /// http://www.cnblogs.com/jiangj/archive/2010/08/18/1802357.html
    /// </summary>
    //协议包
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PACKETHEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] HEAD;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] LENGTH;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] ISZIP;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] PACKTYPE;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] SERVICE;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public byte[] PARAMENT;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] TAIL;
    };
}
