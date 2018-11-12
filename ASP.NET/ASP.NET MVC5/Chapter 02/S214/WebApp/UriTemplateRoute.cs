using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApp
{
    public class UriTemplateRoute : RouteBase
    {
        public UriTemplate UriTemplate { get; private set; }
        public IRouteHandler RouteHandler { get; private set; }
        public RouteValueDictionary DataTokens { get; private set; }

        public UriTemplateRoute(string template, string physicalPath, object dataTokens = null)
        {
            this.UriTemplate = new UriTemplate(template);
            this.RouteHandler = new PageRouteHandler(physicalPath);
            if (null != dataTokens)
            {
                this.DataTokens = new RouteValueDictionary(dataTokens);
            }
            else
            {
                this.DataTokens = new RouteValueDictionary();
            }
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            Uri uri = httpContext.Request.Url;
            Uri baseAddress = new Uri(string.Format("{0}://{1}", uri.Scheme, uri.Authority));
            UriTemplateMatch match = this.UriTemplate.Match(baseAddress, uri);
            if (null == match)
            {
                return null;
            }
            RouteData routeData = new RouteData();
            routeData.RouteHandler = this.RouteHandler;
            routeData.Route = this;
            foreach (string name in match.BoundVariables.Keys)
            {
                routeData.Values.Add(name, match.BoundVariables[name]);
            }
            foreach (var token in this.DataTokens)
            {
                routeData.DataTokens.Add(token.Key, token.Value);
            }
            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            Uri uri = requestContext.HttpContext.Request.Url;
            Uri baseAddress = new Uri(string.Format("{0}://{1}",uri.Scheme, uri.Authority));
            Dictionary<string, string> variables = new Dictionary<string, string>();
            foreach (var item in values)
            {
                variables.Add(item.Key, item.Value.ToString());
            }

            //确定段变量是否被提供
            foreach (var name in this.UriTemplate.PathSegmentVariableNames)
            {
                if (!this.UriTemplate.Defaults.Keys.Any(
                    key => string.Compare(name, key, true) == 0) &&
                    !values.Keys.Any(key => string.Compare(name, key, true) == 0))
                {
                    return null;
                }
            }
            //确定查询变量是否被提供
            foreach (var name in this.UriTemplate.QueryValueVariableNames)
            {
                if (!this.UriTemplate.Defaults.Keys.Any(
                    key => string.Compare(name, key, true) == 0) &&
                    !values.Keys.Any(key => string.Compare(name, key, true) == 0))
                {
                    return null;
                }
            }

            Uri virtualPath = this.UriTemplate.BindByName(baseAddress, variables);
            string strVirtualPath = virtualPath.ToString().ToLower().Replace(baseAddress.ToString().ToLower(), "");
            VirtualPathData virtualPathData = new VirtualPathData(this,strVirtualPath);
            foreach (var token in this.DataTokens)
            {
                virtualPathData.DataTokens.Add(token.Key, token.Value);
            }
            return virtualPathData;
        }
    }
}