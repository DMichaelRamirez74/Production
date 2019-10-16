using Fingerprints.Hubs.ExecutiveHubs;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
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
        protected String SqlConnectionString { get; set; }
        protected void Application_Start()
        {

            SqlConnectionString = ConfigurationManager.ConnectionStrings[FingerprintsData.connection.ConnectionString].ConnectionString;
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

          //  RouteTable.Routes.MapConnection<ExecutiveDashboardTickerConnection>("executiveDashboardTicker", "\executiveDashboardTicker");

            if (!String.IsNullOrEmpty(SqlConnectionString))
            {
                try
                {
                    SqlDependency.Start(SqlConnectionString);

                }
                catch (Exception ex)
                {
                    FingerprintsModel.clsError.WriteException(ex);
                }

            }


        }

        protected void Application_End()
        {
            if (!String.IsNullOrEmpty(SqlConnectionString))
            {
                try
                {
                    SqlDependency.Stop(SqlConnectionString);
                }
                catch (Exception ex)
                {
                    FingerprintsModel.clsError.WriteException(ex);
                }

            }
        }
        protected void Application_BeginRequest()

        {

            var requestUrl = Request.Url.AbsoluteUri;

            if (requestUrl.Contains("WeeklyAttendance"))
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
                else
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
            }
        }
        

    }
}