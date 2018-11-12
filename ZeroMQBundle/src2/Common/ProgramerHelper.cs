using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace Common
{
    public class ProgramerHelper
    {
        public static bool Verbose = true;

        public static void Console_WriteZFrame(string format, ZFrame frame, params object[] data)
        {
            var renderer = new StringBuilder();

            var list = new List<object>(data);

            // here the renderer

            renderer.Append(format);
            renderer.Append(": ");
            renderer.Append("{");
            renderer.Append(0 + data.Length);
            renderer.Append("}");

            // now the message

            frame.Position = 0;

            if (frame.Length == 0)
                list.Add("0");
            else
                list.Add(frame.ReadString());

            frame.Position = 0;

            Console.WriteLine(renderer.ToString(), list.ToArray());
        }

        public static void Console_WriteZMessage(string format, ZMessage message, params object[] data)
        {
            Console_WriteZMessage(format, 0, message, data);
        }

        public static void Console_WriteZMessage(string format, int messagesNotToRead, ZMessage message, params object[] data)
        {
            var renderer = new StringBuilder();

            var list = new List<object>(data);

            for (int i = messagesNotToRead, c = message.Count; i < c; ++i)
            {
                // here the renderer
                if (i == messagesNotToRead)
                {
                    renderer.Append(format);
                    renderer.Append(": ");
                }
                else
                {
                    renderer.Append(", ");
                }
                renderer.Append("{");
                renderer.Append((i - messagesNotToRead) + data.Length);
                renderer.Append("}");

                // now the message
                ZFrame frame = message[i];

                frame.Position = 0;

                if (frame.Length == 0)
                    list.Add("0");
                else
                    list.Add(frame.ReadString());

                frame.Position = 0;
            }

            Console.WriteLine(renderer.ToString(), list.ToArray());
        }
    }
}
