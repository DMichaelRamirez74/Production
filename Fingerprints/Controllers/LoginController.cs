using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using Fingerprints.ViewModel;
using FingerprintsModel.Enums;
using System.Threading;

namespace Fingerprints.Controllers
{
    public class LoginController : Controller
    {
        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         role=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
         roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacvher)
         roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
         roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
         roleid=7c2422ba-7bd4-4278-99af-b694dcab7367(executive)
         roleid=c352f959-cfd5-4902-a529-71de1f4824cc(Social Services Manager)
         roleid=4b77aab6-eed1-4ac3-b498-f3e80cf129c0(education manager)
         */
        LoginData LoginData = new LoginData();
        public ActionResult Login()
        {
            if (Session["UserID"] != null && Session["EmailID"] != null && Session["RoleName"] != null)
            {
                string newLocation = string.Empty;
                if (Session["Roleid"].ToString().Contains("f87b4a71-f0a8-43c3-aea7-267e5e37a59d"))
                    newLocation = "~/Home/SuperAdminDashboard";
                if (!string.IsNullOrEmpty(newLocation))
                    return Redirect(newLocation);
            }
            HttpCookie Emailid = Request.Cookies["Emailid"];
            HttpCookie Password = Request.Cookies["Password"];
            FingerprintsModel.Login _login = new Login();
            if (Emailid != null && Password != null && !string.IsNullOrEmpty(Emailid.Value.Trim()) && !string.IsNullOrEmpty(Password.Value.Trim()))
            {
                _login.Emailid = Emailid.Value.ToString();
                _login.Password = Password.Value.ToString();

            }

            return View(_login);
        }
        [HttpPost]
        public ActionResult Login(FingerprintsModel.Login User, bool? chkRememberMe)
        {
            try
            {
                string IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(IPAddress))
                    IPAddress = Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrWhiteSpace(User.Emailid) || string.IsNullOrWhiteSpace(User.Password))
                {
                    ViewBag.message = "Please enter email and password .";
                    return View();

                }
                string result = string.Empty;
               
                FingerprintsModel.Login UserInfo = LoginData.LoginUser(out result, User.Emailid.Trim(), User.Password.Trim(), IPAddress);
                if (!result.ToLower().Contains("success"))// UserInfo == null)
                {
                    User.UserName = "";
                    User.Password = "";
                    ViewBag.message = result;
                    return View();
                }
                else
                {
                    Session["RoleName"] = UserInfo.RoleName.Replace(" ", string.Empty);
                    if (Session["RoleName"].ToString().ToUpper().Contains("SUPERADMIN"))
                    {
                        if (chkRememberMe != null && Convert.ToBoolean(chkRememberMe))
                        {

                            HttpCookie Emailid = new HttpCookie("Emailid", UserInfo.Emailid);
                            Emailid.Expires = DateTime.Now.AddYears(1);
                            HttpCookie Password = new HttpCookie("Password", User.Password);
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
                        Session["Roleid"] = UserInfo.roleId;
                        Session["EmailID"] = UserInfo.Emailid;
                        Session["FullName"] = UserInfo.UserName;
                        Session["HasSignatureCode"] =( UserInfo.StaffSignature.StaffSignatureID > 0);
                        Session["StaffSignature"] = UserInfo.StaffSignature.Signature;
                        //if (UserInfo.AgencyId != null)
                        //    Session["AgencyID"] = UserInfo.AgencyId;


                        string newLocation = string.Empty;
                        if (Session["Roleid"].ToString().Contains("f87b4a71-f0a8-43c3-aea7-267e5e37a59d"))
                            newLocation = "~/Home/SuperAdminDashboard";
                        //if (Session["RoleName"].ToString().ToUpper().Contains("AGENCYADMIN"))
                        //    newLocation = "~/Home/AgencyAdminDashboard?id=" + Session["AgencyID"].ToString();
                        //if (Session["RoleName"].ToString().ToUpper().Contains("AGENCYHR"))
                        //    newLocation = "~/Home/AgencyHRDashboard";

                        return Redirect(newLocation);
                    }
                    else
                    {
                        User.UserName = "";
                        User.Password = "";
                        ViewBag.message = "Invalid Login";
                        Session.Abandon();
                        return View();
                    }
                }

            }
            catch (Exception Ex)
            {
                ViewBag.message = Ex.Message;
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return Redirect("~/login/login");
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(string EmailId)
        {
            try
            {
                string RandomPassword = GenerateRandomPassword.GenerateRandomCode(10);
                if (!LoginData.CheckEmailIdExist(EmailId, RandomPassword))
                {
                    ViewBag.message = "If the entered email id exists then new password has been sent to the entered email id.";
                    return View();
                }
                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
              bool isResult= SendMail.Sendchangepassword(EmailId, RandomPassword, string.Empty, Server.MapPath("~/MailTemplate"), imagepath);
             
                
                if(isResult)
                 ViewBag.message = "If the entered email id exists then new password has been sent to the entered email id.";

                else
                    ViewBag.message = "Error occurred. Please, try again later.";

                return View();
            }
            catch (Exception Ex)
            {
                ViewBag.message = Ex.Message;
                return View();
            }
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792,a31b1716-b042-46b7-acc0-95794e378b26,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult changePasswordAjax(string currentPassword, string newPassword)
        {
            try
            {


                StaffDetails staff = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();
                    
                    
                 string result=   LoginData.ChangePassword(currentPassword.Trim(), newPassword.Trim(),staff.UserId.ToString());



                if (result=="1")
                {
                    string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");

                    Thread thread = new Thread(delegate ()
                        {
                            SendMail.Sendchangepassword(staff.EmailID, newPassword.Trim(), string.Empty, Server.MapPath("~/MailTemplate"), imagepath);

                        });
                        thread.Start();
                }


                return Json(result);
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }
        public ActionResult Loginagency()
      {

            StaffDetails staff = StaffDetails.GetInstance();

            if (Session["UserID"] != null && Session["EmailID"] != null && Session["RoleName"] != null)
           {
                string newLocation = string.Empty; 
                if (Session["Roleid"].ToString().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5")&& Session["MenuEnable"] != null && Convert.ToBoolean(Session["MenuEnable"]))
                    newLocation = "~/Home/AgencyAdminDashboard";

                else if (Session["Roleid"].ToString().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5") && Session["MenuEnable"] != null && !Convert.ToBoolean(Session["MenuEnable"]))
                    newLocation = "~/Agency/AgencyProfile";

                else if (Session["Roleid"].ToString().Contains("2d9822cd-85a3-4269-9609-9aabb914d792"))
                    newLocation = "~/Home/AgencyHRDashboard";

                else if (Session["Roleid"].ToString().Contains("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d"))
                    newLocation = "~/Home/AgencystaffDashboard";

                else if (Session["Roleid"].ToString() == "a31b1716-b042-46b7-acc0-95794e378b26")
                    //  newLocation = "~/Home/ApplicationApprovalDashboard";
                    newLocation = "~/Roster/Roster";

                else if (Session["Roleid"].ToString() == "e4c80fc2-8b64-447a-99b4-95d1510b01e9")
                    newLocation = "~/Home/AgencystaffDashboard";

                else if (Session["Roleid"].ToString() == "82b862e6-1a0f-46d2-aad4-34f89f72369a")
                    newLocation = "~/Teacher/Roster";

                //else if (Session["Roleid"].ToString() == "82b862e6-1a0f-46d2-aad4-34f89f72369a")
                else if (Session["Roleid"].ToString() == "2adfe9c6-0768-4a35-9088-e0e6ea91f709")
                    newLocation = "~/Teacher/Roster";

                else if (Session["Roleid"].ToString() == "b4d86d72-0b86-41b2-adc4-5ccce7e9775b")
                    newLocation = "~/Home/Dashboard";

                else if (Session["Roleid"].ToString() == "9ad1750e-2522-4717-a71b-5916a38730ed")
                    // newLocation = "~/Home/HealthManager";
                    newLocation = "~/HealthManager/Dashboard";

                else if (Session["Roleid"].ToString() == "7c2422ba-7bd4-4278-99af-b694dcab7367")
                    newLocation = "~/Home/Dashboard";

                else if (Session["Roleid"].ToString() == "047c02fe-b8f1-4a9b-b01f-539d6a238d80")
                    newLocation = "~/Home/AgencyDisabilityManagerDashboard";

                else if (Session["Roleid"].ToString() == "699168ac-ad2d-48ac-b9de-9855d5dc9af8")
                    newLocation = "~/MentalHealth/MentalHealthDashboard";

                else if (Session["Roleid"].ToString() == "9c34ec8e-2359-4704-be89-d9f4b7706e82")
                    newLocation = "~/Home/DisabilityStaffDashboard";

                else if (staff.RoleId.ToString().ToLowerInvariant() == EnumHelper.GetEnumDescription(FingerprintsModel.Enums.RoleEnum.SocialServiceManager).ToLowerInvariant())
                    newLocation = "~/Home/Dashboard";

                else if (Session["Roleid"].ToString() == "6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")
                    newLocation = "~/Transportation/Dashboard";

                else if (Session["Roleid"].ToString() == "ae148380-f94e-4f7a-a378-897c106f1a52")
                    newLocation = "~/BusMonitor/Dashboard";

                else if (Session["Roleid"].ToString() == "825f6940-9973-42d2-b821-56c7c937bfe")
                    newLocation = "~/Home/AgencyFacilitiesManagerDashboard";

                else if (Session["Roleid"].ToString() == "cb540cea-154c-482e-82a6-c1e0a189f611")
                    newLocation = "~/Home/FacilityWorkerDashboard";

                else if (Session["Roleid"].ToString() == "4b77aab6-eed1-4ac3-b498-f3e80cf129c0")
                    newLocation = "~/EducationManager/EducationManagerDashboard";               
                else
                    newLocation = "~/Home/Dashboard";
                if (!string.IsNullOrEmpty(newLocation))
                    return Redirect(newLocation);

            }
            HttpCookie Emailid = Request.Cookies["Emailid"];
            HttpCookie Password = Request.Cookies["Password"];
            //HttpCookie Agencycode = Request.Cookies["Agencycode"];
            FingerprintsModel.Login _login = new Login();
            if (Emailid != null && Password != null && !string.IsNullOrEmpty(Emailid.Value.Trim()) && !string.IsNullOrEmpty(Password.Value.Trim()))
            {
                _login.Emailid = Emailid.Value.ToString();
                _login.Password = Password.Value.ToString();
            }

            return View(_login);
        }
        [HttpPost]
        public ActionResult Loginagency(FingerprintsModel.Login User, bool? chkRememberMe)
        {
            try
            {
                string IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                bool isCoreTeam = false;
              
                if (string.IsNullOrEmpty(IPAddress))
                    IPAddress = Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrWhiteSpace(User.Emailid) || string.IsNullOrWhiteSpace(User.Password))
                {
                    ViewBag.message = "Please enter email and password.";
                    return View();
                }
                string result = string.Empty;
            
                FingerprintsModel.Login UserInfo = LoginData.LoginUser(out result,User.Emailid.Trim(), User.Password.Trim(), IPAddress);
                if (!result.ToLower().Contains("success"))
                {
                    User.UserName = string.Empty;
                    User.Password = string.Empty;
                    ViewBag.message = result;
                    return View();
                }
                else
                {
                    if (chkRememberMe != null && Convert.ToBoolean(chkRememberMe))
                    {

                        HttpCookie Emailid = new HttpCookie("Emailid", UserInfo.Emailid);
                        Emailid.Expires = DateTime.Now.AddYears(1);
                        HttpCookie Password = new HttpCookie("Password", User.Password);
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
                    Session["MenuEnable"] = UserInfo.MenuEnable;
                    Session["IsCoreTeam"] = false;
                    Session["IsDemographic"] = false;
                    Session["isAcceptance"] = false;
                    Session["IsShowPIR"] = UserInfo.IsShowPIR;
                    Session["IsShowSectionB"] = UserInfo.IsShowSectionB;
                    Session["IsShowScreening"] = UserInfo.IsShowScreening;
                    Session["IsInAcceptanceProcess"] = UserInfo.IsInAcceptanceProcess;
                    Session["AllowCaseNoteTeacher"] = UserInfo.AllowCaseNoteTeacher;
                    Session["AvailableDailyHealthChecks"] = UserInfo.AvailableDailyHealthChecks;
                    Session["HasSignatureCode"] = (UserInfo.StaffSignature.StaffSignatureID > 0);
                    Session["StaffSignature"] = UserInfo.StaffSignature.Signature;

                 

                    if (UserInfo.AgencyId != null)
                    {
                       Session["AgencyID"] = UserInfo.AgencyId;
                        isCoreTeam = new LoginData().IsDevelopmentTeam(UserInfo.UserId, UserInfo.AgencyId, UserInfo.roleId);
                        List<Tuple<string, string,int,bool>> AccessList = new List<Tuple<string, string,int,bool>>();
                        bool isAcceptanceProcess = false;
                        AccessList = new LoginData().GetAccessPageByUserId(ref isAcceptanceProcess,UserInfo.UserId, UserInfo.AgencyId, UserInfo.roleId);

                        Session["AccessList"] = AccessList;
                        UserInfo.IsInAcceptanceProcess = isAcceptanceProcess;
                        Session["IsInAcceptanceProcess"] = UserInfo.IsInAcceptanceProcess;

                        if (isCoreTeam)
                        {
                            Session["IsCoreTeam"] = true;
                        }
                      
                        if (UserInfo.RoleList != null)
                        {
                            Session["RoleList"] = UserInfo.RoleList;
                        }
                    }

                    

                    if (!string.IsNullOrEmpty(UserInfo.AccessStart))
                        Session["AccessStart"] = UserInfo.AccessStart;
                    if (!string.IsNullOrEmpty(UserInfo.AccessStop))
                        Session["AccessStop"] = UserInfo.AccessStop;
                }

                StaffDetails staff = StaffDetails.GetInstance();
                string newLocation = string.Empty;
                if (Session["Roleid"].ToString().Contains("f87b4a71-f0a8-43c3-aea7-267e5e37a59d"))
                    newLocation = "~/Home/SuperAdminDashboard";

                else if (Session["Roleid"].ToString().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5") && Session["MenuEnable"] != null && Convert.ToBoolean(Session["MenuEnable"]))
                    newLocation = "~/Home/AgencyAdminDashboard";

                else if (Session["Roleid"].ToString().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5") && Session["MenuEnable"] != null && !Convert.ToBoolean(Session["MenuEnable"]))
                    newLocation = "~/Agency/AgencyProfile/" + UserInfo.AgencyId;

                else if (Session["Roleid"].ToString().Contains("2d9822cd-85a3-4269-9609-9aabb914d792"))
                    newLocation = "~/Home/AgencyHRDashboard";

                else if (Session["Roleid"].ToString().Contains("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d"))
                    newLocation = "~/Home/AgencystaffDashboard";

                else if (Session["Roleid"].ToString() == "a31b1716-b042-46b7-acc0-95794e378b26")
                   // newLocation = "~/Home/ApplicationApprovalDashboard";
                newLocation = "~/Roster/Roster";

                else if (Session["Roleid"].ToString() == "e4c80fc2-8b64-447a-99b4-95d1510b01e9")
                    newLocation = "~/Home/AgencystaffDashboard";

                else if (Session["Roleid"].ToString() == "82b862e6-1a0f-46d2-aad4-34f89f72369a")
                    newLocation = "~/Teacher/Roster";

                else if (Session["Roleid"].ToString() == "b4d86d72-0b86-41b2-adc4-5ccce7e9775b")
                    newLocation = "~/Home/Dashboard";

                //else if (Session["Roleid"].ToString() == "2ADFE9C6-0768-4A35-9088-E0E6EA91F709")
                else if (Session["Roleid"].ToString() == "2adfe9c6-0768-4a35-9088-e0e6ea91f709")
                    newLocation = "~/Teacher/Roster";

                else if (Session["Roleid"].ToString() == "9ad1750e-2522-4717-a71b-5916a38730ed")
                    // newLocation = "~/Home/HealthManager";
                    newLocation = "~/HealthManager/Dashboard";

                else if (Session["Roleid"].ToString() == "7c2422ba-7bd4-4278-99af-b694dcab7367")
                    newLocation = "~/Home/Dashboard";

                else if (Session["Roleid"].ToString() == "047c02fe-b8f1-4a9b-b01f-539d6a238d80")
                    newLocation = "~/Home/AgencyDisabilityManagerDashboard";

                else if (Session["Roleid"].ToString() == "699168ac-ad2d-48ac-b9de-9855d5dc9af8")
                    newLocation = "~/MentalHealth/MentalHealthDashboard";

                else if (Session["Roleid"].ToString() == "9c34ec8e-2359-4704-be89-d9f4b7706e82")
                    newLocation = "~/Home/DisabilityStaffDashboard";

                else if (staff.RoleId.ToString().ToLowerInvariant() == EnumHelper.GetEnumDescription(RoleEnum.SocialServiceManager).ToLowerInvariant())
                    newLocation = "~/Home/Dashboard";

                else if (Session["Roleid"].ToString().Contains("5ac211b2-7d4a-4e54-bd61-5c39d67a1106"))
                    newLocation = "~/Parent/ParentInfo";

                else if (Session["Roleid"].ToString().ToUpper().Contains("944D3851-75CC-41E9-B600-3FA904CF951F"))
                    newLocation = "~/Billing/FamilyOverride";

                else if (Session["Roleid"].ToString().ToUpper().Contains("B65759BA-4813-4906-9A69-E180156E42FC"))
                    newLocation = "~/ERSEA/CenterAnalysis";

                else if (Session["Roleid"].ToString() == "6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")
                    newLocation = "~/Transportation/Dashboard";

                else if (Session["Roleid"].ToString() == "825f6940-9973-42d2-b821-5b6c7c937bfe")
                    newLocation = "~/Home/AgencyFacilitiesManagerDashboard";

                else if (Session["Roleid"].ToString() == "cb540cea-154c-482e-82a6-c1e0a189f611")
                    newLocation = "~/Home/FacilityWorkerDashboard";

                else if (Session["Roleid"].ToString() == "4b77aab6-eed1-4ac3-b498-f3e80cf129c0")
                    newLocation = "~/EducationManager/EducationManagerDashboard";
                else
                    newLocation = "~/Home/Dashboard";
                return Redirect(newLocation);

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                ViewBag.message = "Error Occurred. Please try again.";
                return View();
            }
        }
        public ActionResult LogoutAgency()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return Redirect("~/login/Loginagency");
        }
        public ActionResult LogoutParent()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return Redirect("~/parentportal");
        }
        public ActionResult ForgetPasswordagency()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPasswordagency(string EmailId)
        {
            try
            {
                string RandomPassword = GenerateRandomPassword.GenerateRandomCode(10);
                if (!LoginData.CheckEmailIdExist(EmailId, RandomPassword))
                {
                    ViewBag.message = "If the entered email id exists then new password has been sent to the entered email id.";
                    return View();
                }
                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                bool isResult=SendMail.Sendchangepassword(EmailId, RandomPassword, string.Empty, Server.MapPath("~/MailTemplate"), imagepath);

                if (isResult)
                    ViewBag.message = "If the entered email id exists then new password has been sent to the entered email id.";

                else
                    ViewBag.message = "Error occurred. Please, try again later.";

                return View();
            }
            catch (Exception Ex)
            {
                ViewBag.message = Ex.Message;
                return View();
            }
        }
        //Added on 4Jan2017
        // [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792,a31b1716-b042-46b7-acc0-95794e378b26,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,7c2422ba-7bd4-4278-99af-b694dcab7367")]
        public JsonResult checkPassword(string EmailId, string Password)
        {
            try
            {
                return Json(LoginData.CheckPassword(EmailId, Password));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }

        //End
        //End
    }
}