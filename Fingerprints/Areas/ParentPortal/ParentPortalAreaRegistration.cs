using System.Web.Mvc;

namespace Fingerprints.Areas.ParentPortal
{
    public class ParentPortalAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ParentPortal";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ParentPortal_default",
                "ParentPortal/{controller}/{action}/{id}",
                //new { action = "ParentLogin", id = UrlParameter.Optional }
                new { controller = "ParentLogin", action = "ParentLogin", id = UrlParameter.Optional }
            );
        }
    }
}
