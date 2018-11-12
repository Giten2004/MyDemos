using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBMApplication
{
    class LBMWRcvSourceNotify
    {
        public void sourceNotification(string topic, string source, object cbArg)
        {
            System.Console.Error.WriteLine("new topic ["
                + topic
                + "], source ["
                + source
                + "]");
            System.Console.Out.Flush();
        }
    }
}
