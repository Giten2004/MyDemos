using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructLayoutDemo.StructDeft;

namespace StructLayoutDemo
{
    /// <summary>
    /// http://blog.csdn.net/masterft/article/details/1699009
    /// 
    /// http://www.developerfusion.com/article/84519/mastering-structs-in-c/
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            unsafe
            {
                StructSequential e = new StructSequential();
                e.c = 0;
                e.i = true;
                Console.WriteLine("e.c:{0}, sizeof(StructSequential):{1}", e.c, sizeof(StructSequential));

                Console.ReadLine();

                Console.WriteLine("begin to test BadStruct");
                BadStruct badStruct = new BadStruct();
                badStruct.c = 0;
                badStruct.i = true;
                Console.WriteLine("e.c:{0}, sizeof(BadStruct):{1}", badStruct.c, sizeof(BadStruct));
               

                Console.ReadLine();

                Console.WriteLine("begin to test StructExplicit");
                StructExplicit structExplicit = new StructExplicit();
                structExplicit.c = 0;
                structExplicit.i = true;
                Console.WriteLine("e.c:{0}, sizeof(StructExplicit):{1}", structExplicit.c, sizeof(StructExplicit));

                Console.ReadLine();

                Console.WriteLine("begin to test StructAuto");

                StructAuto s = new StructAuto();
                Console.WriteLine("s.c:{0}, sizeof(StructAuto):{1}", s.c, sizeof(StructAuto));

                Console.WriteLine(string.Format("i:{0}", (int)&(s.i)));
                Console.WriteLine(string.Format("c:{0}", (int)&(s.c)));
                Console.WriteLine(string.Format("b:{0}", (int)&(s.b)));


                Console.Read();
            }
        }
    }
}
