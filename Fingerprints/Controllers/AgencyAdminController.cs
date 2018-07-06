using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using System.Threading;
using Fingerprints.ViewModel;
using System.Globalization;
using System.IO;
using System.Configuration;
using Fingerprints.CustomClasses;
using System.Text;
using System.Reflection;

namespace Fingerprints.Controllers
{
    public class AgencyAdminController : Controller
    {
        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         roleid=3b49b025-68eb-4059-8931-68a0577e5fa2 (Agency Admin)
         */
        agencyData agencyData = new agencyData();
        Center _center = new Center();
        RaceSubcategoryData _raceSubcategoryData = new RaceSubcategoryData();
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult enrollmentcodeGeneration()
        {
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);

            }
            return View();
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult enrollmentcodeGeneration(char activate, string Command, string enrollmentCode, string emailId, string Description)
        {
            try
            {
                if (Command == "GenerateCode")
                {
                    ViewBag.message = agencyData.enrollmentcodeGeneration(activate, Guid.Parse(Session["UserID"].ToString()), Guid.Parse(Session["AgencyID"].ToString()), Description);
                    ViewBag.description = Description;
                }
                if (Command == "SendEmail")
                {
                    DateTime expirytime = agencyData.getexpirytime(enrollmentCode);
                    string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                    string link = UrlExtensions.LinkToRegistrationProcess("/AgencyUser/staffRegistration");
                    string path = Server.MapPath("~/MailTemplate/RegistrationLink.xml");
                    string agencyname = Convert.ToString(Session["AgencyName"]);
                    Thread thread = new Thread(delegate()
                    {
                        sendenrolement(emailId, enrollmentCode, agencyname, expirytime, path, link, imagepath);

                    });
                    thread.Start();
                    //SendMail.Sendenrollmentemail(emailId, enrollmentCode, Convert.ToString(Session["AgencyName"]), expirytime, Server.MapPath("~/MailTemplate/RegistrationLink.xml"), UrlExtensions.LinkToRegistrationProcess("/AgencyUser/staffRegistration"),imagepath);
                    ViewBag.emailalert = "Invitation email has been sent to mentioned email ID.";

                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);

            }
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult pendingApproval()
        {
            try
            {
                ViewData["Title"] = "Pending Approval";

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult listpendingApproval(string sortOrder, string sortDirection, string search, int pageSize, string clear, int requestedPage = 1)
        {
            try
            {
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = agencyData.getpendingApproval(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString()).ToList();
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult approverejectRequest(string id, string action, string roleid, string emailid, string name)
        {
            try
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                name = textInfo.ToTitleCase(name);
                string message = agencyData.approverejectRequest(id, action, roleid, Convert.ToString(Session["UserID"]));
                string path = Server.MapPath("~/MailTemplate/EmailVerification.xml");
                string link = UrlExtensions.LinkToRegistrationProcess("/Login/loginagency");
                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                if (message.Contains("1"))
                {
                    Thread thread = new Thread(delegate()
                    {
                        sendMail(emailid, name, link, path, imagepath);
                    });
                    thread.Start();
                }
                return Json(message);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult ResendEmailForVerification(string emaild, string name, string id)
        {
            try
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                name = textInfo.ToTitleCase(name);
                string path = Server.MapPath("~/MailTemplate/EmailVerification.xml");
                string link = UrlExtensions.LinkToRegistrationProcess("/AgencyUser/staffemailverification?id=" + id);
                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                Thread thread = new Thread(delegate()
                {
                    sendMail(emaild, name, link, path, imagepath);
                });
                thread.Start();
                return Json("0");
            }

            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        public void sendMail(string email, string name, string path, string template, string imagepath)
        {

            SendMail.Sendverificationemail(email, name, path, template, imagepath);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792")]
        public ActionResult pendingVerification()
        {
            try
            {
                ViewData["Title"] = "Pending Verification";

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792")]
        public JsonResult listpendingVerification(string sortOrder, string sortDirection, string search, int pageSize, string clear, int requestedPage = 1)
        {
            try
            {
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = agencyData.getpendingVerification(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString()).ToList();
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792")]
        public JsonResult AutoCompleteAgencystaff(string term, string Active = "0")
        {
            try
            {
                var result = agencyData.AutoCompleteAgencystaffList(term, Session["AgencyID"].ToString(), Active);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult listEnrolementmentcode(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = agencyData.enrollmentcode(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString()).ToList();
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult EndSession()
        {
            try
            {

                Session["AgencyID"] = null;
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);

            }
            return Json("1");
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Resendemail(string emailId, string enrollmentCode)
        {
            try
            {
                DateTime expirytime = agencyData.getexpirytime(enrollmentCode);
                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                string link = UrlExtensions.LinkToRegistrationProcess("/AgencyUser/staffRegistration");
                string path = Server.MapPath("~/MailTemplate/RegistrationLink.xml");
                string agencyname = Convert.ToString(Session["AgencyName"]);
                Thread thread = new Thread(delegate()
                {
                    sendenrolement(emailId, enrollmentCode, agencyname, expirytime, path, link, imagepath);

                });
                thread.Start();
                //DateTime expirytime = agencyData.getexpirytime(enrollmentCode);
                //string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                //SendMail.Sendenrollmentemail(emailId, enrollmentCode, Convert.ToString(Session["AgencyName"]), expirytime, Server.MapPath("~/MailTemplate/RegistrationLink.xml"), UrlExtensions.LinkToRegistrationProcess("/AgencyUser/staffRegistration"), imagepath);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);

            }
            return Json("1");
        }
        public void sendenrolement(string emailid, string agencycode, string agencyname, DateTime expiryttime, string path, string link, string imagepath)
        {
            SendMail.Sendenrollmentemail(emailid, agencycode, agencyname, expiryttime, path, link, imagepath);
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Adminsetstaffinfo(string id = "0")
        {
            AgencyStaff _staffList = null;
            try
            {


                ViewBag.mode = 1;
                _staffList = agencyData.GetData_AllDropdownforstaff(id);
                Session["oldemailid"] = _staffList.EmailAddress;

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(_staffList);

        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult Adminsetstaffinfo(AgencyStaff agencystaff, FormCollection collection)
        {
            AgencyStaff _staffList = new AgencyStaff();
            try
            {
                agencystaff.AgencyStaffId = Guid.Parse(collection["AgencyStaffId"].ToString());
                #region verifying Agency user code here
                string message = "";
                ViewBag.mode = 1;
                Updatestaff(agencystaff, collection, out message);
                if (message == "1")
                {
                    TempData["message"] = "Record updated successfully. ";
                    if (Session["oldemailid"] != null)
                    {
                        //if (Session["oldemailid"].ToString().ToUpper() != agencystaff.EmailAddress.ToUpper())
                        //{
                        //    string oldemailid = Session["oldemailid"].ToString();
                        //    string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                        //    Thread thread = new Thread(delegate()
                        //    {
                        //        sendMail(oldemailid, agencystaff.EmailAddress, char.ToUpper(agencystaff.FirstName[0]) + agencystaff.FirstName.Substring(1), oldemailid + "," + agencystaff.EmailAddress, Server.MapPath("~/MailTemplate"), imagepath);

                        //    });
                        //    thread.Start();
                        //    Session["oldemailid"] = agencystaff.EmailAddress;
                        //}
                    }
                    return Redirect("~/Agency/viewagencystaff");
                }
                else if (message == "2")
                    ViewBag.message = "Email already exist.";
                else if (message == "4")
                    ViewBag.message = "You don't have access to change the role of agency admin.";
                else if (message == "5")
                    ViewBag.message = "You don't have access to change your role.";


                else
                    ViewBag.message = message;
                _staffList = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString(), 1, agencystaff.AgencyStaffId);
                ViewData["Title"] = "Edit Staff";
                #endregion
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(_staffList);

        }
        [CustAuthFilter()]
        public void Updatestaff(AgencyStaff agencystaff, FormCollection collection, out string res)
        {
            res = "";
            try
            {
                agencystaff.AgencyStaffId = Guid.Parse(collection["AgencyStaffId"].ToString());
                agencystaff.UpdatedBy = agencystaff.CreatedBy = Session["UserId"].ToString();
                agencystaff.Race = collection["DdlRaceList"].ToString() == "0" ? null : collection["DdlRaceList"].ToString();
                agencystaff.Natinality = collection["DdlNationList"].ToString() == "0" ? null : collection["DdlNationList"].ToString();
                agencystaff.HighestEducation = collection["DdlHighestEducation"].ToString() == "-1" ? null : collection["DdlHighestEducation"].ToString();
                agencystaff.EarlyChildHood = collection["DdlEarlyChildHood"].ToString() == "-1" ? null : collection["DdlEarlyChildHood"].ToString();
                agencystaff.GettingDegree = collection["DdlGettingDegree"].ToString() == "-1" ? null : collection["DdlGettingDegree"].ToString();
                agencystaff.Contractor = collection["DdlContractor"].ToString() == "-1" ? null : collection["DdlContractor"].ToString();
                agencystaff.AssociatedProgram = collection["DdlAssociatedProgram"].ToString() == "-1" ? null : collection["DdlAssociatedProgram"].ToString();
                agencystaff.Replacement = collection["DdlReplacement"].ToString() == "-1" ? null : collection["DdlReplacement"].ToString();
                agencystaff.AccessDays = collection["DdlAccessType"].ToString() == "-1" ? null : collection["DdlAccessType"].ToString();
                agencystaff.HRCenter = collection["DdlHrCenter"].ToString() == "0" ? null : collection["DdlHrCenter"].ToString();
                agencystaff.Gender = collection["DdlGender"].ToString() == "-1" ? null : collection["DdlGender"].ToString();
                agencystaff.PirRoleid = collection["DdlpirList"].ToString() == "0" ? null : collection["DdlpirList"].ToString();
                string DdlAgencyList, DdlRoleList, AvatarFile, AvatarHfile, AvatarSfile, AvatarTfile;
                DdlAgencyList = Session["AgencyID"].ToString();
                DdlRoleList = collection["DdlRoleList"].ToString();
                if (DdlRoleList == "3b49b025-68eb-4059-8931-68a0577e5fa2")
                {
                    agencystaff.AccessDays = "0";
                }

                #region upload Avatar icons
                string Uploadpath = "~/" + ConfigurationManager.AppSettings["Avtar"].ToString();
                if (!Directory.Exists(Server.MapPath(Uploadpath)))
                {
                    try
                    {
                        Directory.CreateDirectory(Server.MapPath(Uploadpath));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (agencystaff.Avatar != null)
                {
                    AvatarFile = agencystaff.Avatar.FileName;
                    string Fullpath = Uploadpath + "/" + AvatarFile;
                    #region Delete previous file if exist
                    try
                    {
                        if (!String.IsNullOrEmpty(agencystaff.AvatarUrl))
                        {

                            FileInfo fin = new FileInfo(Server.MapPath(Uploadpath + "/" + agencystaff.AvatarUrl.ToString()));
                            if (fin.Exists)
                            {
                                try
                                {
                                    fin.Delete();
                                }
                                catch (Exception ex)
                                {
                                    clsError.WriteException(ex);
                                    Server.ClearError();
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    #endregion
                    string _newfilename = clsError.GetUniqueFilePath(Fullpath);
                    try
                    {
                        agencystaff.Avatar.SaveAs(Server.MapPath(_newfilename));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    agencystaff.AvatarUrl = Path.GetFileName(_newfilename);

                }
                if (agencystaff.AvatarH != null)
                {
                    AvatarHfile = agencystaff.AvatarH.FileName;
                    string Fullpath = Uploadpath + "/" + AvatarHfile;
                    #region Delete previous file if exist
                    try
                    {
                        if (!String.IsNullOrEmpty(agencystaff.AvatarhUrl))
                        {
                            FileInfo fin = new FileInfo(Server.MapPath(Uploadpath + "/" + agencystaff.AvatarhUrl.ToString()));
                            if (fin.Exists)
                            {
                                try
                                {
                                    fin.Delete();
                                }
                                catch (Exception ex)
                                {
                                    clsError.WriteException(ex);
                                    Server.ClearError();
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    #endregion
                    string _newfilename = clsError.GetUniqueFilePath(Fullpath);
                    try
                    {
                        agencystaff.AvatarH.SaveAs(Server.MapPath(_newfilename));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    agencystaff.AvatarhUrl = Path.GetFileName(_newfilename);
                }

                if (agencystaff.AvatarS != null)
                {
                    AvatarSfile = agencystaff.AvatarS.FileName;
                    string Fullpath = Uploadpath + "/" + AvatarSfile;
                    #region Delete previous file if exist
                    try
                    {
                        if (!String.IsNullOrEmpty(agencystaff.AvatarsUrl))
                        {
                            FileInfo fin = new FileInfo(Server.MapPath(Uploadpath + "/" + agencystaff.AvatarsUrl.ToString()));
                            if (fin.Exists)
                            {
                                try
                                {
                                    fin.Delete();
                                }
                                catch (Exception ex)
                                {
                                    clsError.WriteException(ex);
                                    Server.ClearError();
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    #endregion
                    string _newfilename = clsError.GetUniqueFilePath(Fullpath);
                    try
                    {
                        agencystaff.AvatarS.SaveAs(Server.MapPath(_newfilename));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    agencystaff.AvatarsUrl = Path.GetFileName(_newfilename);
                }
                if (agencystaff.AvatarT != null)
                {
                    AvatarTfile = agencystaff.AvatarT.FileName;
                    string Fullpath = Uploadpath + "/" + AvatarTfile;
                    #region Delete previous file if exist
                    try
                    {
                        if (!String.IsNullOrEmpty(agencystaff.AvatartUrl))
                        {
                            FileInfo fin = new FileInfo(Server.MapPath(Uploadpath + "/" + agencystaff.AvatartUrl.ToString()));
                            if (fin.Exists)
                            {
                                try
                                {
                                    fin.Delete();
                                }
                                catch (Exception ex)
                                {
                                    clsError.WriteException(ex);
                                    Server.ClearError();
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    #endregion
                    string _newfilename = clsError.GetUniqueFilePath(Fullpath);
                    try
                    {
                        agencystaff.AvatarT.SaveAs(Server.MapPath(_newfilename));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    agencystaff.AvatartUrl = Path.GetFileName(_newfilename);
                }
                #endregion



                string StaffID = "", AgencyCode = "";
                string message = string.Empty;
                message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "1", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode);
                res = message;
                // return View(agencystaff);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                Server.ClearError();
                ViewBag.message = Ex.Message;

            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]

        public ActionResult SaveAcceptancePrirorityRoles(List<AcceptanceRole> Roles)
        {
            agencyData.SaveAcceptancePrirorityRoles(Session["AgencyId"].ToString(), Session["UserID"].ToString(), Roles);
            return Json("");
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult PendingApprovalRequest(string id = "0")
        {
            AgencyStaff _staffList = null;
            try
            {
                _staffList = agencyData.GetUserRequestDropdown(Session["AgencyID"].ToString(), 1, Guid.Parse(id));
                _staffList.LoginAllowed = true;

                _staffList.enrollid = id;
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(_staffList);

        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult PendingApprovalRequest(AgencyStaff agencystaff, FormCollection collection, FamilyHousehold.Center Centers, FamilyHousehold.Role Rolelist, List<PrimaryLanguages> PrimaryLanguages)
        {
            AgencyStaff _staffList = new AgencyStaff();
            try
            {
                StringBuilder _string = new StringBuilder();
                if (Centers.CenterID != null)
                {
                    foreach (string str in Centers.CenterID)
                    {
                        _string.Append(str + ",");
                    }
                    agencystaff.centerlist = _string.ToString().Substring(0, _string.Length - 1);
                }
                _string.Clear();
                if (Rolelist.RoleID != null)
                {
                    foreach (string str in Rolelist.RoleID)
                    {
                        _string.Append(str + ",");
                    }
                    agencystaff.Rolelist = _string.ToString().Substring(0, _string.Length - 1);
                }
                agencystaff.AgencyStaffId = Guid.Parse(collection["AgencyStaffId"].ToString());
                agencystaff.UpdatedBy = agencystaff.CreatedBy = Session["UserId"].ToString();
                agencystaff.Race = collection["DdlRaceList"] == null ? null : collection["DdlRaceList"].ToString();
                agencystaff.Natinality = collection["DdlNationList"] == null ? null : collection["DdlNationList"].ToString();
                agencystaff.HighestEducation = collection["DdlHighestEducation"] == null ? null : collection["DdlHighestEducation"].ToString();
                agencystaff.EarlyChildHood = collection["DdlEarlyChildHood"] == null ? null : collection["DdlEarlyChildHood"].ToString();
                agencystaff.GettingDegree = collection["DdlGettingDegree"] == null ? null : collection["DdlGettingDegree"].ToString();
                agencystaff.Contractor = collection["DdlContractor"] == null ? null : collection["DdlContractor"].ToString();
                agencystaff.AssociatedProgram = collection["DdlAssociatedProgram"] == null ? null : collection["DdlAssociatedProgram"].ToString();
                agencystaff.Replacement = collection["DdlReplacement"] == null ? null : collection["DdlReplacement"].ToString();
                agencystaff.HRCenter = collection["DdlHrCenter"] == null ? null : collection["DdlHrCenter"].ToString();
                agencystaff.Gender = collection["DdlGender"] == null ? null : collection["DdlGender"].ToString();
                agencystaff.PirRoleid = collection["DdlpirList"] == null ? null : collection["DdlpirList"].ToString();
                string DdlAgencyList, DdlRoleList, AvatarFile, AvatarHfile, AvatarSfile, AvatarTfile;
                DdlAgencyList = Session["AgencyID"].ToString();
                DdlRoleList = collection["DdlRoleList"] == null ? null : collection["DdlRoleList"].ToString(); ;
                agencystaff.SelectedAgencyId = Guid.Parse(DdlAgencyList);
                agencystaff.SelectedRoleId = DdlRoleList;
                if (DdlRoleList == "a65bb7c2-e320-42a2-aed4-409a321c08a5")
                {
                    agencystaff.AccessDays = "0";
                }
                agencystaff.Classrooms = collection["DdlClassList"] == null ? null : collection["DdlClassList"].ToString();//Changes
                #region upload Avatar icons
                string Uploadpath = "~/" + ConfigurationManager.AppSettings["Avtar"].ToString();
                if (!Directory.Exists(Server.MapPath(Uploadpath)))
                {
                    try
                    {
                        Directory.CreateDirectory(Server.MapPath(Uploadpath));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (agencystaff.Avatar != null)
                {
                    AvatarFile = agencystaff.Avatar.FileName;
                    string Fullpath = Uploadpath + "/" + AvatarFile;
                    string _newfilename = clsError.GetUniqueFilePath(Fullpath);
                    try
                    {
                        agencystaff.Avatar.SaveAs(Server.MapPath(_newfilename));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    agencystaff.AvatarUrl = Path.GetFileName(_newfilename);

                }
                if (agencystaff.AvatarH != null)
                {
                    AvatarHfile = agencystaff.AvatarH.FileName;
                    string Fullpath = Uploadpath + "/" + AvatarHfile;
                    string _newfilename = clsError.GetUniqueFilePath(Fullpath);
                    try
                    {
                        agencystaff.AvatarH.SaveAs(Server.MapPath(_newfilename));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    agencystaff.AvatarhUrl = Path.GetFileName(_newfilename);
                }

                if (agencystaff.AvatarS != null)
                {
                    AvatarSfile = agencystaff.AvatarS.FileName;
                    string Fullpath = Uploadpath + "/" + AvatarSfile;
                    string _newfilename = clsError.GetUniqueFilePath(Fullpath);
                    try
                    {
                        agencystaff.AvatarS.SaveAs(Server.MapPath(_newfilename));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    agencystaff.AvatarsUrl = Path.GetFileName(_newfilename);
                }
                if (agencystaff.AvatarT != null)
                {
                    AvatarTfile = agencystaff.AvatarT.FileName;
                    string Fullpath = Uploadpath + "/" + AvatarTfile;
                    string _newfilename = clsError.GetUniqueFilePath(Fullpath);
                    try
                    {
                        agencystaff.AvatarT.SaveAs(Server.MapPath(_newfilename));
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                        Server.ClearError();
                    }
                    agencystaff.AvatartUrl = Path.GetFileName(_newfilename);
                }
                #endregion
                string StaffID = "", AgencyCode = "";
                string message = string.Empty;
                string name = string.Empty;
                agencystaff.LangList = PrimaryLanguages;
                if (collection["ddlapprovereject"].ToString() == "0")
                    message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "5", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode);
                else
                    message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "4", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode);
                if (message.Contains("1"))
                {
                    TempData["message"] = "User already approved.";
                    return Redirect("~/AgencyAdmin/pendingApproval");
                }
                else if (message == "2")
                {
                    ViewBag.message = "Email already exist.";
                    _staffList = agencyData.GetUserRequestDropdown(Session["AgencyID"].ToString(), 1, Guid.Parse(agencystaff.enrollid));
                    return View(_staffList);
                }
                else if (message.Contains("3"))
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    name = textInfo.ToTitleCase(agencystaff.FirstName + " " + agencystaff.LastName);
                    string path = Server.MapPath("~/MailTemplate/EmailVerification.xml");
                    string link = UrlExtensions.LinkToRegistrationProcess("/Login/loginagency");
                    string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                    Thread thread = new Thread(delegate()
                    {
                        sendMail(agencystaff.EmailAddress, name, link, path, imagepath);
                    });
                    thread.Start();
                    TempData["message"] = "User approved successfully.";
                    return Redirect("~/AgencyAdmin/pendingApproval");
                }
                else if (message == "4")
                {
                    ViewBag.message = "Selected agency might be disabled.Please select another agency.";
                    _staffList = agencyData.GetUserRequestDropdown(Session["AgencyID"].ToString(), 1, Guid.Parse(agencystaff.enrollid));
                    return View(_staffList);
                }
                else if (message == "5")
                {
                    TempData["message"] = "User already rejected.";
                    return Redirect("~/AgencyUser/rejectedList");
                }
                else if (message == "6")
                {
                    TempData["message"] = "User rejected successfully.";
                    return Redirect("~/AgencyUser/rejectedList");
                }

                else if (message == "8")
                {

                    ViewBag.message = "User role already exists in center. Please select another center.";
                    _staffList = agencyData.GetUserRequestDropdown(Session["AgencyID"].ToString(), 1, Guid.Parse(agencystaff.enrollid));
                    return View(_staffList);
                }




                else
                {
                    _staffList = agencyData.GetUserRequestDropdown(Session["AgencyID"].ToString(), 1, Guid.Parse(agencystaff.enrollid));
                    ViewBag.message = message;
                    return View(_staffList);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(_staffList);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult RaceSubcategory()
        {
            RaceSubcategory _race = (new RaceSubcategoryData()).GetData_AllDropdown();
            return View(_race);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult RaceSubcategory(RaceSubcategory info, FormCollection collection)
        {
            RaceSubcategory _race = _raceSubcategoryData.GetData_AllDropdown();
            try
            {
                info.RaceID = collection["DdlRaceList"].ToString() == "0" ? null : collection["DdlRaceList"].ToString();
                if (String.IsNullOrWhiteSpace(info.RaceID) || info.RaceID == "0")
                {
                    ViewBag.message = "Please select race category from list.";
                    return View();
                }
                else if (String.IsNullOrWhiteSpace(info.SubCategoryName.Trim()))
                {
                    ViewBag.message = "Please enter subcategory name.";
                    return View();
                }

                info.IsActive = collection["DdlStatusList"].ToString() == "1" ? true : false;
                info.CreatedBy = Convert.ToString(Session["UserID"]);
                info.RaceDescription = info.RaceDescription;
                string message = (new RaceSubcategoryData()).addeditRaceInfo(info);

                if (message == "1")
                {
                    ViewBag.message = "Record added successfully.";

                }
                else if (message == "2")
                {
                    ViewBag.message = "Record updated successfully.";

                }
                else if (message == "3")
                {
                    ViewBag.message = "Race subcategory already exist in race category.";
                }
                else
                {
                    ViewBag.message = "An error occurred while adding data.";
                }
                info = null;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(_race);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult RaceSubcategorydetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = _raceSubcategoryData.RaceSubcategorydetails(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString()); //Session["AgencyID"].ToString()).ToList()
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Center(string id = "0")
        {
            try
            {

                if (id == "0")
                {
                    ViewBag.mode = 0;
                    ViewData["Title"] = "Add Center";

                }
                else
                {
                    ViewBag.mode = 1;
                    ViewData["Title"] = "Edit Center";
                }
                _center = new CenterData().editcentre(id, Session["AgencyID"].ToString());
                ViewBag.Classroom = _center.Classroom;
                TempData["Classroom"] = _center.Classroom;
                TempData["timezonelist"] = _center.TimeZonelist;

                return View(_center);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult Center(string Command, Center info, FormCollection collection, List<FingerprintsModel.Center.ClassRoom> Classroom)
        {
            try
            {
                info.TimeZonelist = (List<TimeZoneinfo>)TempData["timezonelist"];
                TempData.Keep();
                if (info.CenterId == 0)
                {
                    info.mode = 0;
                    ViewData["Title"] = "Add Center";
                }
                else
                {
                    info.mode = 1;
                    ViewData["Title"] = "Edit Center";
                }
                info.AgencyId = Session["AgencyID"].ToString();
                info.CreatedBy = Session["UserID"].ToString();
                string message = new CenterData().addeditcenter(info, Classroom);




                if (Session["MenuEnable"] != null && Convert.ToBoolean(Session["MenuEnable"]))
                {

                    if (message == "1")
                    {
                        TempData["message"] = "Record added successfully.";
                        return Redirect("~/AgencyAdmin/centerlist");
                    }
                    else if (message == "2")
                    {
                        TempData["message"] = "Record updated successfully.";
                        return Redirect("~/AgencyAdmin/centerlist");

                    }
                    else if (message == "3")
                    {
                        ViewBag.message = "Center already exist.";
                    }
                    else
                    {
                        ViewBag.message = "An error occurred while adding data.";
                    }
                }
                else
                {

                    if(Command =="SubmitCommand")
                    {
                        if (message == "1")
                        {
                            TempData["message"] = "Record added successfully.";
                            return Redirect("~/AgencyAdmin/Center");
                        }
                        else if (message == "2")
                        {
                            TempData["message"] = "Record updated successfully.";
                            return Redirect("~/AgencyAdmin/Center");

                        }
                        else if (message == "3")
                        {
                            TempData["message"] = "Center already exist.";
                        }
                        else
                        {
                            TempData["message"] = "An error occurred while adding data.";
                        }

                    }
                    else if (Command == "NextCommand")
                    {
                        if (message == "1")
                        {
                            TempData["message"] = "Record added successfully.";
                            return Redirect("~/AgencyAdmin/Slots");
                        }
                        else if (message == "2")
                        {
                            TempData["message"] = "Record updated successfully.";
                            return Redirect("~/AgencyAdmin/Slots");

                        }
                        else if (message == "3")
                        {
                            TempData["message"] = "Center already exist.";
                        }
                        else
                        {
                            TempData["message"] = "An error occurred while adding data.";
                        }




                    }



                }







                return View(info);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View(info);

            }

        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult centerlist()
        {
            try
            {
                ViewData["Title"] = "Center list";

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult listcenter(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = new CenterData().centerList(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult updatecenter(string id, int mode)
        {
            try
            {
                return Json(new CenterData().UpdateCenter(id, mode, Guid.Parse(Convert.ToString(Session["UserID"]))));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        //[CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        //public ActionResult CommunityResource()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        //public ActionResult CommunityResource(CommunityResource info, FormCollection collection, string id)
        //{
        //    try
        //    {
        //        CommunityResource objdata = new CommunityResource();
        //        TempData.Keep();
        //        string message = "";
        //        CommunityResourceData obj = new CommunityResourceData();
        //        if (info.CommunityID == 0)
        //        {
        //            info.Community = (collection["DdlCommunityList"].ToString() == "-1") ? null : collection["DdlCommunityList"];
        //            message = obj.AddCommunity(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());

        //        }
        //        else
        //        {
        //            info.Community = (collection["DdlCommunityList"].ToString() == "-1") ? null : collection["DdlCommunityList"];
        //            message = obj.AddCommunity(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
        //            ViewBag.result = "Sucess";
        //        }
        //        if (message == "1")
        //        {
        //            ViewBag.result = "Sucess";
        //            //   TempData["message"] = "Record added successfully.";
        //            ViewBag.message = "Record added successfully.";
        //        }
        //        else if (message == "2")
        //        {
        //            ViewBag.result = "Sucess";
        //            ViewBag.message = "Record updated successfully.";
        //        }
        //        else if (message == "3")
        //        {
        //            ViewBag.message = "Community resource already exist.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    return View(info);

        //}
        //[CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        //public JsonResult Communitydetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        //{
        //    try
        //    {
        //        CommunityResourceData info = new CommunityResourceData();
        //        string totalrecord;
        //        int skip = pageSize * (requestedPage - 1);
        //        var list = info.Communitydetails(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString());
        //        return Json(new { list, totalrecord });
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //        return Json(Ex.Message);
        //    }
        //    // return View();
        //}
        //[CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        //public JsonResult Getcommunity(string CommunityID = "0")
        //{
        //    CommunityResourceData obj = new CommunityResourceData();
        //    try
        //    {
        //        return Json(obj.GetcommunityDetails(CommunityID, Session["AgencyID"].ToString()));
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //        return Json("Error occurred please try again.");
        //    }
        //}
        //[CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        //public JsonResult Deletecommunity(string CommunityID = "0")
        //{
        //    CommunityResourceData obj = new CommunityResourceData();
        //    try
        //    {
        //        return Json(obj.Deletecommunity(CommunityID, Session["AgencyID"].ToString()));
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //        return Json("Error occured please try again.");
        //    }
        //}
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult CommunityResource()
        {
            ViewBag.mode = 0;
            CommunityResourceData obj = new CommunityResourceData();
            try
            {
                CommunityResource _communityinfo = obj.GetServiceData(Session["AgencyID"].ToString());
                TempData["Centers"] = _communityinfo.HrcenterList;
                TempData["ServiceInfo"] = _communityinfo.AvailableService;
                return View(_communityinfo);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View();
            }
        }
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult CommunityResource(CommunityResource info, FormCollection collection, string id, CommunityResource.PostedService PostedPostedService, FamilyHousehold.Center Centers)
        {
            CommunityResourceData obj = new CommunityResourceData();
            CommunityResource objdata = new CommunityResource();
            objdata = info;
            try
            {
                StringBuilder _string = new StringBuilder();
                if (PostedPostedService.ServiceID != null)
                {
                    foreach (string str in PostedPostedService.ServiceID)
                    {
                        _string.Append(str + ",");
                    }
                    info.Services = _string.ToString().Substring(0, _string.Length - 1);
                }
                TempData.Keep();
                string message = "";
                StringBuilder centers = new StringBuilder();
                if (Centers.CenterID != null)
                {
                    foreach (string str in Centers.CenterID)
                    {
                        centers.Append(str + ",");
                    }
                    info.Centers = centers.ToString().Substring(0, centers.Length - 1);
                }





                if (info.CommunityID == 0)
                {
                    info.Community = (collection["DdlCommunityList"].ToString() == "-1") ? null : collection["DdlCommunityList"];
                    message = obj.AddCommunity(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                }
                else
                {
                    info.Community = (collection["DdlCommunityList"].ToString() == "-1") ? null : collection["DdlCommunityList"];
                    message = obj.AddCommunity(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                    ViewBag.result = "Sucess";
                }
                if (message == "1")
                {
                    ViewBag.result = "Sucess";
                    ViewBag.message = "Record added successfully.";
                }
                else if (message == "2")
                {
                    ViewBag.result = "Sucess";
                    ViewBag.message = "Record updated successfully.";
                }
                else if (message == "3")
                {
                    ViewBag.message = "Community resource already exist.";
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            objdata.AvailableService = (List<CommunityResource.CategoryServiceInfo>)TempData["ServiceInfo"];
            objdata.HrcenterList = (List<HrCenterInfo>)TempData["Centers"];
            TempData.Keep();
            return View(objdata);
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Communitydetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                CommunityResourceData info = new CommunityResourceData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.Communitydetails(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Getcommunity(string CommunityID = "0")
        {
            CommunityResourceData obj = new CommunityResourceData();
            try
            {
                return Json(obj.GetcommunityDetails(CommunityID, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Deletecommunity(string CommunityID = "0")
        {
            CommunityResourceData obj = new CommunityResourceData();
            try
            {
                return Json(obj.Deletecommunity(CommunityID, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [JsonMaxLengthAttribute]
        public JsonResult listClassroomDetails(string CenterId = "0")
        {
            try
            {

                CenterData obj = new CenterData();
                var listClassroom = obj.ClassDetails(CenterId, Session["AgencyID"].ToString()).ToList();
                return Json(new { listClassroom });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult DeleteClassroomInfo(string ClassroomID = "0", string CenterId = "0")
        {
            CenterData obj = new CenterData();
            try
            {

                return Json(obj.DeleteClassroomdetails(ClassroomID, CenterId, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult SchoolDistrict()
        {
            return View();
        }
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult SchoolDistrict(SchoolDistrict info)
        {
            try
            {

                TempData.Keep();
                string message = "";
                SchoolDistrictData obj = new SchoolDistrictData();
                if (info.SchoolDistrictID == 0)
                {

                    message = obj.AddSchool(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                    ViewBag.result = "Sucess";
                }
                else
                {

                    message = obj.AddSchool(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                    ViewBag.result = "Sucess";
                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
                    ViewBag.message = "Record added successfully.";
                }
                else if (message == "2")
                {
                    ViewBag.message = "Record updated successfully.";
                }
                else if (message == "3")
                {
                    ViewBag.message = "Record already exist.";
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(info);

        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Schooldetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                SchoolDistrictData info = new SchoolDistrictData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.SchoolInfo(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Getschooldetails(string SchoolDistrictID = "0")
        {
            SchoolDistrictData obj = new SchoolDistrictData();
            try
            {
                return Json(obj.Getschoolinfo(SchoolDistrictID, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Deleteschool(string SchoolDistrictID = "0")
        {
            SchoolDistrictData obj = new SchoolDistrictData();
            try
            {
                return Json(obj.Deleteschoolinfo(SchoolDistrictID, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter()]
        public JsonResult GetClassroom(string ClassroomID = "0", string CenterID = "0")
        {
            CenterData obj = new CenterData();
            try
            {
                return Json(obj.GetClassroominfo(ClassroomID, CenterID, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult ProgramType()
        {
            //ProgramType _prog = (new ProgramTypeData()).GetData_AllDropdown();
            ProgramTypeData progData = new ProgramTypeData();
            ProgramType _prog = progData.GetData_AllDropdown();
            ViewBag.RefList = _prog.refList;
            TempData["RefList"] = ViewBag.RefList;
            return View(_prog);
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult ProgramType(ProgramType info, FormCollection collection)
        {
            try
            {

                TempData.Keep();
                string message = "";
                ProgramTypeData obj = new ProgramTypeData();
                info.AgencyId = (Session["AgencyID"].ToString());
                if (info.ProgramID == 0)
                {
                    info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                    message = obj.AddProg(info, 0, Guid.Parse(Session["UserID"].ToString()));

                }
                else
                {
                    info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                    message = obj.AddProg(info, 1, Guid.Parse(Session["UserID"].ToString()));

                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
                    ViewBag.message = "Record added successfully.";
                    ViewBag.result = "Sucess";
                }
                else if (message == "2")
                {
                    ViewBag.message = "Record updated successfully.";
                    ViewBag.result = "Sucess";
                }
                else if (message == "3")
                {
                    ViewBag.message = "Record already exist.";
                }
                ViewBag.RefList = TempData["RefList"];
                TempData.Keep();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            ViewBag.RefList = TempData["RefList"];
            TempData.Keep();
            return View(info);

        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Programdetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                ProgramTypeData info = new ProgramTypeData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.ProgInfo(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Getprogdetails(string ProgramID = "0")
        {
            ProgramTypeData obj = new ProgramTypeData();
            try
            {
                return Json(obj.Getproginfo(ProgramID, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult updateprogstatus(string ProgramID, int mode)
        {
            ProgramTypeData obj = new ProgramTypeData();
            try
            {
                return Json(obj.updatestatus(ProgramID, mode, Guid.Parse(Convert.ToString(Session["AgencyID"]))));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult SelectionPoints()
        {
            SelectPointsData progData = new SelectPointsData();
            SelectPoints _prog = null;
            _prog = progData.GetData_AllDropdown(Session["AgencyID"].ToString());
            ViewBag.RefList = _prog.refList;
            TempData["RefList"] = ViewBag.RefList;


            return View(_prog);
            //return View();
        }
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult SelectionPoints(SelectPoints info, string Command, FormCollection collection, List<FingerprintsModel.SelectPoints.CustomQuestion> CustomQues)
        {
            try
            {

                TempData.Keep();
                string message = "";
                SelectPointsData obj = new SelectPointsData();
                info.AgencyID = (Session["AgencyID"].ToString());
                if (info.SPID == 0)
                {
                    if (Command == "SaveLockSelectDetail")
                    {
                        info.IsLocked = true;
                        info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                        message = obj.AddEditSelectPoint(info, 0, Guid.Parse(Session["UserID"].ToString()), CustomQues);

                    }
                    else
                    {
                        info.IsLocked = false;
                        info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                        message = obj.AddEditSelectPoint(info, 0, Guid.Parse(Session["UserID"].ToString()), CustomQues);
                    }
                    // info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                    //  message = obj.AddEditSelectPoint(info, 0, Guid.Parse(Session["UserID"].ToString()), CustomQues);

                }
                else
                {
                    if (info.IsLocked == false)
                    {
                        if (Command == "SaveLockSelectDetail")
                        {
                            info.IsLocked = true;
                            info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                            message = obj.AddEditSelectPoint(info, 1, Guid.Parse(Session["UserID"].ToString()), CustomQues);
                        }
                        else
                        {
                            info.IsLocked = false;
                            info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                            message = obj.AddEditSelectPoint(info, 1, Guid.Parse(Session["UserID"].ToString()), CustomQues);

                        }
                    }
                    else
                    {
                        ViewBag.message = "Record already locked.";
                        ViewBag.result = "Sucess";
                    }
                    //info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                    //message = obj.AddEditSelectPoint(info, 1, Guid.Parse(Session["UserID"].ToString()), CustomQues);

                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
                    ViewBag.message = "Record added successfully.";
                    ViewBag.result = "Sucess";
                }
                else if (message == "2")
                {
                    ViewBag.message = "Record updated successfully.";
                    ViewBag.result = "Sucess";
                }
                else if (message == "3")
                {
                    ViewBag.message = "Record already locked.";
                    ViewBag.result = "Sucess";
                }
                ViewBag.RefList = TempData["RefList"];
                TempData.Keep();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            ViewBag.RefList = TempData["RefList"];
            TempData.Keep();
            return View(info);


        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Deletecustomques(string CQID = "0")
        {
            SelectPointsData obj = new SelectPointsData();
            try
            {
                return Json(obj.DeleteQues(CQID, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult GetSelectPointlist(string ProgramId = "0")//string RestrictedId = "0",
        {
            SelectPointsData obj = new SelectPointsData();
            try
            {
                return Json(obj.EditSelectPointInfo(ProgramId, Session["AgencyID"].ToString()));//RestrictedId,
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [JsonMaxLengthAttribute]
        public JsonResult listQuesDetails(string ProgramId = "0")//string CQID = "0",
        {
            try
            {

                SelectPointsData obj = new SelectPointsData();
                var listQues = obj.QuesDetails(ProgramId, Session["AgencyID"].ToString()).ToList();
                return Json(new { listQues });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult GetRefProglist(string ProgramId = "0")//string RestrictedId = "0",
        {
            SelectPointsData obj = new SelectPointsData();
            try
            {
                return Json(obj.GetRefProglistInfo(ProgramId, Session["AgencyID"].ToString()));//RestrictedId,
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter()]
        public JsonResult Deleteclassroom(string AgencyID, string CenterId = "0")
        {
            CenterData obj = new CenterData();
            try
            {
                if (string.IsNullOrEmpty(AgencyID))
                {
                    if (Session["AgencyID"] != null)
                        AgencyID = Session["AgencyID"].ToString();
                }

                return Json(obj.DeleteClassroominfo(CenterId, AgencyID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        //Changes on 8Feb2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,a65bb7c2-e320-42a2-aed4-409a321c08a5,f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        [JsonMaxLengthAttribute]
        public JsonResult Checkaddress(int Zipcode, string Address = "", string HouseHoldId = "0")
        {
            try
            {
                FamilyData familyData = new FamilyData();
                string Result;
                var Zipcodelist = familyData.Checkaddress(out Result, Address, HouseHoldId, Zipcode);
                return Json(new { Zipcodelist, Result });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Deleteclass(string classId = "0")
        {
            CenterData obj = new CenterData();
            try
            {


                return Json(obj.DeleteClassroom(classId, Convert.ToString(Session["AgencyID"])));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult CoreTeam()
        {
            try
            {
                ViewBag.message = "";
                ViewBag.CoreTeamList = agencyData.GetCoreTeam(Session["AgencyID"].ToString(), Session["UserID"].ToString());
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult CoreTeam(List<CoreTeam> CoreTeams)
        {
            try
            {
                string message = "";
                ViewBag.CoreTeamList = agencyData.SaveCoreTeam(ref message, CoreTeams, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                if (message == "1")
                {
                    ViewBag.message = "Record saved successfully.";
                }
                else
                {
                    ViewBag.message = "Please try again.";
                }
                Response.Redirect(Request.RawUrl);
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Demographic()
        {
            try
            {
                ViewBag.message = "";
                ViewBag.DemographicList = agencyData.GetDemographicSection(Session["AgencyID"].ToString(), Session["UserID"].ToString());
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2")]
        [HttpGet]
        public ViewResult PIRAccessRoles()
        {
            List<PIRAccessRoles> accessRolesList = new List<PIRAccessRoles>();
            try
            {
                ViewBag.Message = "";
               accessRolesList=  new agencyData().GetPIRAccessRoles();
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(accessRolesList);
        }
        [HttpPost]
        [CustAuthFilter()]
        public ViewResult PIRAccessRoles(List<PIRAccessRoles> PIRAccessRoles)
        {
            List<PIRAccessRoles> accessRolesList = new List<FingerprintsModel.PIRAccessRoles>();
            try
            {
                int rowsAffected = 0;
                accessRolesList = new agencyData().InsertPIRAccessRoles(out rowsAffected, PIRAccessRoles);

                ViewBag.Message = (rowsAffected > 0) ? "Record saved successfully" : "Please try again";
               
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(accessRolesList);
        }


        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        [CustAuthFilter()]
        public ActionResult Demographic(List<Demographic> Demographics)
        {
            try
            {
                string message = "";
                ViewBag.DemographicList = agencyData.SaveDemographic(ref message, Demographics, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                if (message == "1")
                {
                    ViewBag.message = "Record saved successfully.";
                }
                else
                {
                    ViewBag.message = "Please try again.";
                }
                Response.Redirect(Request.RawUrl);
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult AcceptanceRole()
        {
            RoleData rd = new RoleData();
            AcceptanceRole AR = new FingerprintsModel.AcceptanceRole();
            AR.RoleList = rd.RoleList();
            return View(AR);
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Acceptance()
        {
            try
            {
                ViewBag.message = "";
                ViewBag.AcceptanceProcessList = agencyData.GetAcceptanceProcess(Session["AgencyID"].ToString());
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter()]
        public JsonResult GetAcceptanceRole()
        {
            return Json(agencyData.GetAcceptanceProcess(Session["AgencyID"].ToString()));

        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult Acceptance(List<AcceptanceProcess> AcceptanceProcess)
        {
            try
            {
                string message = "";
                ViewBag.DemographicList = agencyData.SaveAcceptanceProcess(ref message, AcceptanceProcess, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                if (message == "1")
                {
                    ViewBag.message = "Record saved successfully.";
                }
                else
                {
                    ViewBag.message = "Please try again.";
                }
                Response.Redirect(Request.RawUrl);
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult NoOfSeats(string AgencyID, string Classid, string Seats = "0")
        {
            CenterData obj = new CenterData();
            try
            {
                if (string.IsNullOrEmpty(AgencyID))
                {
                    if (Session["AgencyID"] != null)
                        AgencyID = Session["AgencyID"].ToString();
                }

                return Json(obj.NoOfSeats(Seats, Classid, AgencyID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult GetSlot(string ProgramId = "0")
        {

            try
            {
                string Slots = "";
                var list = agencyData.GetSlot(ref Slots, ProgramId, Session["AgencyID"].ToString());
                return Json(new { list, Slots });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Slots()
        {
            AgencySlots agencySlot = new AgencySlots();
            try
            {
                agencySlot = agencyData.GetRefProgram(Session["AgencyID"].ToString());
                TempData["AgencySlot"] = agencySlot;
                Session["MenuEnable"] = agencySlot.MenuEnabled;
                return View(agencySlot);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View(agencySlot);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult Slots(AgencySlots Slot, List<ClassRoom> ClassSlot)
        {

            try
            {
                string message = agencyData.AddSlots(ref Slot, ClassSlot, Session["UserID"].ToString(), Session["AgencyID"].ToString());
                if (message == "1")
                {
                    ViewBag.message = "Program total slots must be equal to purchase slots. ";

                }
                else if (message == "2")
                {
                    ViewBag.message = "Seats already assigned. Please assign seats according to available seats.";

                }
                else if (message == "3")
                {
                    ViewBag.message = "Record updated successfully.";

                }
                else
                {
                    Slot = (AgencySlots)TempData["AgencySlot"];
                }
                TempData.Keep();
                Session["MenuEnable"] = Slot.MenuEnabled;
                return View(Slot);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                Slot = (AgencySlots)TempData["AgencySlot"];
                TempData.Keep();
                return View(Slot);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult CheckProgram(string Agencyid)
        {
            try
            {
                return Json(agencyData.CheckProgram(Agencyid, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult CheckForcenters(string Agencyid)
        {
            try
            {
                return Json(agencyData.CheckForcenters(Agencyid, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpGet]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2")]

        public ViewResult IncomeReviewRoles()
        {
            List<IncomeReviewRoles> incomeReviewRolesList = new List<IncomeReviewRoles>();
            try
            {
                ViewBag.Message = "";
                incomeReviewRolesList = new agencyData().GetIncomeReviewRoles();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(incomeReviewRolesList);
        }

        /// <summary>
        /// POST method to insert the Roles who can review the income of the Family.
        /// </summary>
        /// <param name="IncomeReviewRoles"></param>
        /// <returns>List<IncomeReviewRoles></returns>
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2")]
        public ActionResult IncomeReviewRoles(List<IncomeReviewRoles> IncomeReviewRoles)
        {
            List<IncomeReviewRoles> incomeReviewRolesList = new List<IncomeReviewRoles>();
            try
            {
                int rowsAffected = 0;
                incomeReviewRolesList = new agencyData().InsertIncomeReviewRoles(out rowsAffected, IncomeReviewRoles);

                ViewBag.Message = (rowsAffected > 0) ? "Record saved successfully" : "Please try again";

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(incomeReviewRolesList);
        }


        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2")]
        public ActionResult AccessRoles()
        {
            AccessRoles accessRoles = new FingerprintsModel.AccessRoles();
            try
            {
               
                accessRoles = new agencyData().SP_AccessRole(7,Session["AgencyID"].ToString());
                accessRoles.TitleId = 7;
                ViewBag.Titleid = 7;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(accessRoles);
        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2")]
        [HttpPost]
        public ActionResult AccessRoles(List<Role> Role,int TitleId,string Command)
        {
            AccessRoles accessRoles = new FingerprintsModel.AccessRoles();
            try
            {
              
                    accessRoles = new agencyData().SaveAccessRoles(Role, Session["AgencyID"].ToString(), Session["UserID"].ToString(), TitleId);

                
                accessRoles.TitleId = TitleId;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            ViewBag.Titleid = TitleId;
            return View(accessRoles);
        }




        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2")]
        public ActionResult MoveSeats()
        {


            MoveSeats moveSeats = new FingerprintsModel.MoveSeats();
            
            moveSeats = new agencyData().GetCenterandClassRoomSeats(moveSeats);
            return View(moveSeats);

        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2")]
        public JsonResult GetClassroomsByCenter(string centerId = "0")
        {
            try
            {
                centerId = EncryptDecrypt.Decrypt64(centerId);
                return Json(new RosterData().Getclassrooms(centerId, Session["AgencyID"].ToString()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [CustAuthFilter()]
        public JsonResult GetAvailSeatsByClassroom(dynamic center, dynamic clsroom, dynamic agency)
        {

            try
            {

                center = Convert.ToInt64(EncryptDecrypt.Decrypt64(center[0]));
                clsroom = Convert.ToInt64(EncryptDecrypt.Decrypt64(clsroom[0]));
                agency = new Guid(agency[0]);
                return Json(new agencyData().GetSeatsBy(center, clsroom, agency, 1), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json("Error Occured. Please,try again later.", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult AddSeatsDisplacement(MoveSeats centerPair)
        {
            string result = "";

            try
            {
                if (centerPair != null && centerPair.CenterClassPairList != null && centerPair.CenterClassPairList.Count > 0)
                {
                    centerPair.CenterClassPairList.ForEach(x =>
                   {

                       x.FromCenter = EncryptDecrypt.Decrypt64(x.FromCenter);
                       x.FromClassRoom = EncryptDecrypt.Decrypt64(x.FromClassRoom);
                       x.ToCenter = EncryptDecrypt.Decrypt64(x.ToCenter);
                       x.ToClassRoom = EncryptDecrypt.Decrypt64(x.ToClassRoom);
                   });

                    result = new agencyData().AddMovedSeats(centerPair);

                }

                else
                {
                    result = "0";
                }



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


            return Json(result,JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter()]
        public ActionResult GetCenterWithSeats(string agencyId,int reqPage,int skip,int take, string searchText)
        {
            MoveSeats seatsDis = new FingerprintsModel.MoveSeats();
            try
            {
                skip= (take * (reqPage - 1));

                seatsDis.AgencyID = new Guid(agencyId);
                seatsDis.SearchTerm = searchText;
                seatsDis.RequestedPage = reqPage;
                seatsDis.Skip = skip;
                seatsDis.Take = take;
                seatsDis = new agencyData().GetCenterandClassRoomSeats(seatsDis);
                seatsDis.AgencyCenterList = Fingerprints.Utilities.Helper.GetCentersByUserId(Session["UserID"].ToString(), agencyId, Session["RoleID"].ToString(), false);
                seatsDis.AgencyCenterList.ForEach(x => x.Value =(x.Value=="0")?x.Value: EncryptDecrypt.Encrypt64(x.Value));
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }

            return PartialView("~/Views/Partialviews/MoveSeatsPartial.cshtml", seatsDis);
        }

        [CustAuthFilter()]
        public JsonResult AutoCompleteCenter(string searchText,string agencyId)
        {
            try
            {

            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(null);

        }
        [CustAuthFilter()]
        public ActionResult GetStaffsByRole(string roleID)
        {

            AccessStaffs access = new AccessStaffs();
            try
            {
                access = new agencyData().GetStaffsByRole(roleID);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(access);


        }
   

        [HttpPost]
        [CustAuthFilter()]
        public PartialViewResult AccessRoleList(int type)
        {

            AccessRoles accessRoles = new FingerprintsModel.AccessRoles();
            try
            {
               accessRoles = new agencyData().SP_AccessRole(type, Session["AgencyID"].ToString());
                accessRoles.TitleId = type;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            if(type==6)
                return PartialView("~/Views/Partialviews/ScreeningAccessRoles.cshtml", accessRoles);
            else
            return PartialView("~/Views/Partialviews/AccessRolesListPartial.cshtml", accessRoles);
        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult EndOfProgramYear()
        {

            NewProgramYearTransition newProgramYear = new NewProgramYearTransition();

            newProgramYear=  new agencyData().EndOfProgramYear();

            return View(newProgramYear);
        }


        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpGet]
        public ActionResult EndOfYearProgramTypes()
        {
            Agency agency = new Agency();

            try
            {

             //   AgencyStaff _staff = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString());

             //   agency = agencyData.editAgency(Session["AgencyID"].ToString());
               // ViewBag.RefList = _staff.refList;
                agency = agencyData.GetEndOfYearFunds_Programs();
               // ViewBag.NextProgramYear = agency.ActiveProgYear.Split('-')[1] + "-" + (int.Parse(agency.ActiveProgYear.Split('-')[1]) + 1).ToString();


            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(agency);
        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult EndOfYearProgramTypes(Agency agencyinfo, FormCollection collection)
        {
            string result = "";

            result= new agencyData().InsertEndOfYearFundsPrograms(agencyinfo,agencyinfo.FundSourcedata);


             if (result == "1")
                ViewBag.message = "Record updated successfully";
            else if(result=="")

                ViewBag.message = "Error Occured.Please, try again later.";

            agencyinfo = agencyData.GetEndOfYearFunds_Programs();

            //return RedirectToAction("EndOfYearProgramTypes");
           return View(agencyinfo);
        }


    }
}
