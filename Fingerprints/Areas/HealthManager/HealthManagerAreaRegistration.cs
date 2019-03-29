using System.Web.Mvc;

namespace Fingerprints.Areas.HealthManager
{
    public class HealthManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HealthManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HealthManager_default",
                "HealthManager/{controller}/{action}/{id}",
                new { action = "Dashboard", id = UrlParameter.Optional }
            );
        }
    }
}
