using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBMApplication
{
    class LBMWRcvTopicFilter
    {
        public int comparePattern(string topic, object cbArg)
        {
            /* match everything */
            System.Console.Error.WriteLine("[" + topic + "] topic accepted.");
            return 0;
        }
    }
}
