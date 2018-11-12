using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StructLayoutDemo.StructDeft
{
    ////C#编译器会自动在上面运用[StructLayout(LayoutKind.Sequential)]
    public struct StructSequential
    {
        public bool i;   //1Byte
        public double c; //8byte
        public bool b;   //1byte

        // sizeof(StructSequential)???
    }
}
