using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp
{
    public class WeatherAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Weather"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            object defaults = new
            {
                areacode = "010",
                days = 2,
                defaultCity = "BeiJing",
                defaultDays = 2
            };
            object constraints = new { areacode = @"0\d{2,3}", days = @"[1-3]" };
            context.MapRoute("weatherDefault", "weather/{areacode}/{days}", defaults,constraints);
        }
    }
}