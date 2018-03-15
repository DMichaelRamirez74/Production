using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Fingerprints.Filters
{
    public class
        CustAuthFilter : AuthorizeAttribute
    {
        string [] Usertype;
        public CustAuthFilter(string userType)
        {
            Usertype = userType.Split(',');
        }
        public CustAuthFilter()
        {

        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                filterContext.Result = new RedirectResult("~/login/Loginagency");
                
            }
            else
            {
                if(filterContext.HttpContext.Session["Roleid"]!=null )
                {
                    if(Usertype.Count() > 0)
                    {
                        if(!Usertype.Contains(filterContext.HttpContext.Session["Roleid"].ToString()))
                        {
                            filterContext.Result = new RedirectResult("~/login/Loginagency");
                            return;
                        }

                    }
                }
            }
            return;
        }
    }
}