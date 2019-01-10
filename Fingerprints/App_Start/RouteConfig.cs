﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fingerprints
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
           //     constraints:new { lang = @"(\w{2})|(\w{2}-\w{2})" },
                defaults: new { controller = "Login", action = "Loginagency", id = UrlParameter.Optional },
                namespaces:new[] { "Fingerprints.Controllers"}
            );
        }
    }
}