using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class RouteData
    {
        public IDictionary<string, object> Values { get; private set; }
        public IDictionary<string, object> DataTokens { get; private set; }
        public IRouteHandler RouteHandler { get; set; }
        public RouteBase Route { get; set; }

        public RouteData()
        {
            this.Values = new Dictionary<string, object>();
            this.DataTokens = new Dictionary<string, object>();
            this.DataTokens.Add("namespaces", new List<string>());
        }

        public string Controller
        {
            get
            {
                object controllerName = string.Empty;
                this.Values.TryGetValue("controller", out controllerName);
                return controllerName.ToString();
            }
        }

        public string ActionName
        {
            get
            {
                object actionName = string.Empty;
                this.Values.TryGetValue("action", out actionName);
                return actionName.ToString();
            }
        }
    }
}