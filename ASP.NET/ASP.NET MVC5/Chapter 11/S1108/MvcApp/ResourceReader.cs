using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp
{
    public abstract class ResourceReader
    {
        public abstract string GetString(string name);
    }
}