using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsModel;
using FingerprintsData;
using System.Threading;
using System.Globalization;

namespace Fingerprints.Areas.ParentPortal.Controllers
{
    public class ParentLoginController : Controller
    {
        //
        // GET: /ParentPortal/Parent/

        [HttpGet]
        public ActionResult ParentLogin()
        {
            return View();
        }

        /// <summary>
        /// POST : /ParentPortal/ParentLogin/
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult ParentLogin(FingerprintsModel.Login parent, bool? chkRememberMe)
        {

            string IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(IPAddress))
                IPAddress = Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrWhiteSpace(parent.Emailid) || string.IsNullOrWhiteSpace(parent.Password))
            {
                ViewBag.message = "Please enter email and password.";
                return View();
            }
            string result = string.Empty;
            int primarylang;


            FingerprintsModel.Login UserInfo = new LoginData().LoginParent(out result,parent.Emailid.Trim(), parent.Password.Trim(), IPAddress,out primarylang);
            if (!result.ToLower().Contains("success"))
            {
                parent.UserName = string.Empty;
                parent.Password = string.Empty;
                ViewBag.message = result;
                return View();
            }

            else
            {
                if (chkRememberMe != null && Convert.ToBoolean(chkRememberMe))
                {

                    HttpCookie Emailid = new HttpCookie("Emailid", UserInfo.Emailid);
                    Emailid.Expires = DateTime.Now.AddYears(1);
                    HttpCookie Password = new HttpCookie("Password", parent.Password);
                    Password.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(Emailid);
                    Response.Cookies.Add(Password);
                }
                else
                {
                    HttpCookie Emailid = new HttpCookie("Emailid");
                    Emailid.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(Emailid);
                    HttpCookie Password = new HttpCookie("Password");
                    Password.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(Password);

                }
                Session["UserID"] = UserInfo.UserId;
                Session["RoleName"] = UserInfo.RoleName.Replace(" ", string.Empty);
                Session["EmailID"] = UserInfo.Emailid;
                Session["FullName"] = UserInfo.UserName;
                Session["AgencyName"] = UserInfo.AgencyName;
                Session["Roleid"] = UserInfo.roleId;
                if (primarylang == 10)
                {
                    Session["CurrentCluture"] = "es";
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("es");
                }
                else {
                    Session["CurrentCluture"] = "en";
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                }
                
            }

             return RedirectToAction("FamilyInfo","Home");
        }

        public ActionResult ForgotPasswordParent()
        {
            return View();
        }

      


    }
}
