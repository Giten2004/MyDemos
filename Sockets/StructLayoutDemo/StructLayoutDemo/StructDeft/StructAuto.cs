using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StructLayoutDemo.StructDeft
{
    [StructLayout(LayoutKind.Auto)]
    public struct StructAuto
    {
        public bool i;   //1Byte
        public double c; //8byte
        public bool b;   //1byte
    }
}
