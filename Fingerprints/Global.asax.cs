using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Fingerprints
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
        protected void Application_BeginRequest()

        {

            var requestUrl = Request.Url.AbsoluteUri;

           if( requestUrl.Contains("WeeklyAttendance"))
            {
                Response.Cache.SetCacheability(HttpCacheability.Private);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(2));
             
            }
            else
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();

            }


            
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Session != null)
            {
                if (Session["CurrentCluture"] != null)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["CurrentCluture"].ToString());
                }
                else {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
            }
        }
        

    }
}