using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ListViewAssemblies(this HtmlHelper helper)
        {
            TagBuilder ul = new TagBuilder("ul");
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("App_Web_")))
            {
                TagBuilder li = new TagBuilder("li");
                li.InnerHtml = assembly.FullName;
                ul.InnerHtml += li.ToString();
            }
            return new MvcHtmlString(ul.ToString());
        }
    }
}