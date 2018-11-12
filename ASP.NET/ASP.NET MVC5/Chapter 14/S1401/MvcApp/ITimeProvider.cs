using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp
{
    public interface ITimeProvider
    {
        DateTime GetCurrentTime(DateTimeKind dateTimeKind);
    }
}