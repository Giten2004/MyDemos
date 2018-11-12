using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StructLayoutDemo.StructDeft
{
    [StructLayout(LayoutKind.Explicit)]
    public struct StructExplicit
    {
        [FieldOffset(0)]
        public bool i;     //1Byte
        [FieldOffset(1)]
        public double c;   //8byte
        [FieldOffset(9)]
        public bool b;     //1byte
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct BadStruct
    {
        [FieldOffset(0)]
        public bool i;     //1Byte
        [FieldOffset(0)]
        public double c;   //8byte
        [FieldOffset(0)]
        public bool b;     //1byte

        // sizeof(BadStruct)得到的结果是9byte
    }
}
