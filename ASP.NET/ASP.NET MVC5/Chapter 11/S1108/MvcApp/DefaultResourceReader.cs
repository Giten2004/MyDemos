using MvcApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp
{
    public class DefaultResourceReader : ResourceReader
    {
        public override string GetString(string name)
        {
            return Resources.ResourceManager.GetString(name);
        }
    }
}