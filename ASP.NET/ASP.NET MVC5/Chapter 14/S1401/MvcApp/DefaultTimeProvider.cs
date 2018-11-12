using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp
{
    public class DefaultTimeProvider : ITimeProvider
    {
        [CachingCallHandler("00:00:03")]
        public DateTime GetCurrentTime(DateTimeKind dateTimeKind)
        {
            switch (dateTimeKind)
            {
                case DateTimeKind.Local: return DateTime.Now.ToLocalTime();
                case DateTimeKind.Utc: return DateTime.Now.ToUniversalTime();
                default: return DateTime.Now;
            }
        }
    }
}