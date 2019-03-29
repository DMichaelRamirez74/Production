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

        //public CustAuthFilter(string [] userTypeArray)
        //{
        //    Usertype = Array.ConvertAll(userTypeArray,x=>x.ToLowerInvariant());
            
        //}

        public CustAuthFilter(params FingerprintsModel.Enums.RoleEnum[] allowedRoles)
        {

            // Usertype = allowedRoles.Select(x => FingerprintsModel.Role.RolesDictionary[(int)x].ToLowerInvariant()).ToArray();

           // Usertype = allowedRoles.Select(x => FingerprintsModel.EnumHelper.GetDescription(x).ToString().ToLowerInvariant()).ToArray();

            Usertype = allowedRoles.Select(x => FingerprintsModel.EnumHelper.GetEnumDescription(x).ToString().ToLowerInvariant()).ToArray();

           
        }
        public CustAuthFilter()
        {
            Usertype = new string[0];
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

                            if(filterContext.HttpContext.Request.IsAjaxRequest())
                            {
                                filterContext.HttpContext.Response.StatusCode = 401;
                                filterContext.Result =new JsonResult{Data= "Login",JsonRequestBehavior=  JsonRequestBehavior.AllowGet };
                                return;
                            }
                            else
                            {
                                filterContext.Result = new RedirectResult("~/login/Loginagency");
                                return;
                            }

                           
                        }

                    }
                    else
                    {
                        return;
                    }
                }
            }
            return;
        }
    }
}