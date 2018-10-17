using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Globalization;
using Fingerprints.ViewModel;
using Fingerprints.CustomClasses;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Collections;
using GoogleMaps.LocationServices;

namespace Fingerprints.Controllers
{
    public class AgencyUserController : Controller
    {
        /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
         roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacvher)
         roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
         roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
         roleid=c352f959-cfd5-4902-a529-71de1f4824cc(Social Service Manager)
         roleid= 7c2422ba-7bd4-4278-99af-b694dcab7367(Executive)
         roleid=2af7205e-87b4-4ca7-8ca8-95827c08564c(Area Manager)
         roleid=047c02fe-b8f1-4a9b-b01f-539d6a238d80 (Disabilities Manager)
         */
        agencyData agencyData = new agencyData();
        FamilyData familyData = new FamilyData();
        CommunityResourceData communitydata = new CommunityResourceData();
        List<WellBabyExamModel> wellBabyList=new List<WellBabyExamModel>();
        [CustAuthFilter()]
        public ActionResult staffRegistration()
        {
            try
            {
                if (Session["RoleID"] != null && Session["RoleID"].ToString().ToUpper().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5"))
                    return Redirect("~/login/loginagency");
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }

        }
        [HttpPost]
        [CustAuthFilter()]
        public ActionResult staffRegistration(staffRegistration staffregistration)
        {
            try
            {
                AgencyStaff staffinfo = new AgencyStaff();
                string staffId = "";
                string message = agencyData.Staffregistration(staffregistration, out staffId);
                if (message == "4")
                {
                    //  TempData["message"] = "Record added successfully. ";
                    ModelState.Clear();
                    //return Redirect("~/AgencyUser/staffpersonalinformation?id="+staffId);
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    string name = textInfo.ToTitleCase(staffregistration.firstName + " " + Convert.ToString(staffregistration.lastName));
                    string path = Server.MapPath("~/MailTemplate/staffRegistration.xml");
                    string link = UrlExtensions.LinkToRegistrationProcess("/AgencyUser/staffpersonalinformation?id=" + staffId);
                    string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                    Thread thread = new Thread(delegate ()
                    {
                        sendMail(staffregistration.emailid, name, link, path, imagepath);
                    });
                    thread.Start();
                    ViewBag.message = "Your registration request has been submitted successfully. Email confirmation link has been sent to your registered email id.";
                }
                else if (message == "1")
                    TempData["message"] = "Email already exists.";
                else if (message == "2")
                    TempData["message"] = "Enrollment Code does not exists. Please enter valid enrollment code. ";
                else if (message == "3")
                    TempData["message"] = "Enrollment code expired. Please enter valid enrollment code.";
                else
                    TempData["message"] = message;
                return View();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View();
            }
        }
        [CustAuthFilter()]
        public ActionResult staffemailverificationnew()
        {

            return View();
        }
        public ActionResult staffemailverification()
        {
            AgencyStaff _staffList = new AgencyStaff();
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    //agencyData.emailVerification(Request.QueryString["id"].ToString());
                    _staffList = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString());
                    List<string> staffinfo = new List<string>();
                        staffinfo=agencyData.getEmail(Request.QueryString["id"].ToString());
                    if (staffinfo.Count>0 && staffinfo[4].ToString() == "True")
                    {
                        return Redirect("~/AgencyUser/staffemailverificationnew");
                    }
                    if (staffinfo.Count > 0)
                    {
                        _staffList.EmailAddress = staffinfo[0].ToString();
                        _staffList.FirstName = staffinfo[1].ToString();
                        _staffList.LastName = staffinfo[2].ToString();
                        _staffList.CellNumber = staffinfo[3].ToString();

                    }
                  

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(_staffList);
            //try
            //{
            //    if (Request.QueryString["id"] != null)
            //    {
            //        agencyData.emailVerification(Request.QueryString["id"].ToString());
            //    }
            //    return View();
            //}
            //catch (Exception ex)
            //{
            //    clsError.WriteException(ex);
            //    return View();
            //}
        }
        [HttpPost]
        [CustAuthFilter()]
        public ActionResult staffemailverification(AgencyStaff _staflist, FormCollection collection)
        {

            try
            {
                if (Request.QueryString["id"] != null)
                {
                    _staflist.Race = collection["DdlRaceList"].ToString() == "0" ? null : collection["DdlRaceList"].ToString();
                    _staflist.Natinality = collection["DdlNationList"].ToString() == "0" ? null : collection["DdlNationList"].ToString();
                    _staflist.HighestEducation = collection["DdlHighestEducation"].ToString() == "-1" ? null : collection["DdlHighestEducation"].ToString();
                    _staflist.EarlyChildHood = collection["DdlEarlyChildHood"].ToString() == "-1" ? null : collection["DdlEarlyChildHood"].ToString();
                    _staflist.GettingDegree = collection["DdlGettingDegree"].ToString() == "-1" ? null : collection["DdlGettingDegree"].ToString();
                    _staflist.Gender = collection["DdlGender"].ToString() == "-1" ? null : collection["DdlGender"].ToString();
                    if (agencyData.emailVerification(_staflist, Request.QueryString["id"].ToString()) == "1")
                    {
                        return Redirect("~/AgencyUser/staffemailverificationnew");
                    }
                    else
                    {
                        return Redirect("~/AgencyUser/staffemailverificationnew");
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View();
            //try
            //{
            //    if (Request.QueryString["id"] != null)
            //    {
            //        agencyData.emailVerification(Request.QueryString["id"].ToString());
            //    }
            //    return View();
            //}
            //catch (Exception ex)
            //{
            //    clsError.WriteException(ex);
            //    return View();
            //}
        }
        [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult pendingVerificationuser(string id = "0",string ak="0")
        {

            AgencyStaff _staffList = null;
            try
            {

                ViewData["Title"] = "Approve/Reject User";
                ViewBag.mode = 1;
                bool isEndYear = string.IsNullOrEmpty(ak) ? false : ak == "1" ? true : false;

                ViewBag.IsEndOfYear= string.IsNullOrEmpty(ak) ? "0" : ak == "1" ? "1" : "0";


                _staffList = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString(), 1, Guid.Parse(id),null,isEndYear);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return View(_staffList);
        }
        [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [CustAuthFilter()]
        [HttpPost]
        public ActionResult pendingVerificationuser(AgencyStaff agencystaff, FormCollection collection, FamilyHousehold.Center Centers, FamilyHousehold.Role Rolelist, List<PrimaryLanguages> PrimaryLanguages,string endOfYear="")
        {
            AgencyStaff _staffList = new AgencyStaff();
            try
            {

                bool isEndYear = endOfYear == "1" ? true : false;

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

                #region verifying Agency user code here

                string message = "";
                ViewBag.mode = 1;
                agencystaff.LangList = PrimaryLanguages;
                UpdatependingverificationUser(agencystaff, collection, out message,isEndYear);
                if (message == "0")
                {
                    ViewBag.message = "User reject successfully.";
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    string name = textInfo.ToTitleCase(agencystaff.FirstName + " " + agencystaff.LastName);
                    string path = Server.MapPath("~/MailTemplate/HRVerificationReject.xml");
                    string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                    //Thread thread = new Thread(delegate()
                    //{
                    //    sendMailVerificationEmail(agencystaff.EmailAddress, name, "", path, "", imagepath);
                    //});
                    //thread.Start();
                }
                else if (message == "1")
                {
                    TempData["message"] = "Record approved successfully.";
                    return Redirect("~/AgencyUser/pendingVerificationuser?id=" + agencystaff.AgencyStaffId+"&ak="+endOfYear+"");
                }
                else if (message == "2")
                    ViewBag.message = "Email already exist.";
                else if (message == "3")
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    string name = textInfo.ToTitleCase(agencystaff.FirstName + " " + agencystaff.LastName);
                    string path = Server.MapPath("~/MailTemplate/HRVerificationApprove.xml");
                    string link = UrlExtensions.LinkToRegistrationProcess("/login/Loginagency");
                    //string agencycode = Session["AgencyCode"].ToString();
                    string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                    //Thread thread = new Thread(delegate()
                    //{
                    //    sendMailVerificationEmail(agencystaff.EmailAddress, name, link, path, " ", imagepath);
                    //});
                    //thread.Start();
                    TempData["message"] = "User verified successfully.";
                    return Redirect("~/Agency/viewagencystaff?ak="+endOfYear+"");
                }
                else if (message == "4")
                {
                    ViewBag.message = "User already rejected.";

                }
                else if (message == "5")
                    ViewBag.message = "User already  approved.";
                else if (message == "6")
                    ViewBag.message = "You can't reject yourself .";
                else if (message == "8")
                    ViewBag.message = "User role already exists in center. Please select another center.";
                else
                    ViewBag.message = message;
                _staffList = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString(), 1, agencystaff.AgencyStaffId);
                ViewData["Title"] = "Pending  Verification";
                return View(_staffList);

                #endregion
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(_staffList);
        }
        public void UpdatependingverificationUser(AgencyStaff agencystaff, FormCollection collection, out string res, bool isEndOfYear=false)
        {
            res = "";
            try
            {
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
                //agencystaff.AccessDays = collection["DdlAccessType"].ToString() == "-1" ? null : collection["DdlAccessType"].ToString();
                agencystaff.HRCenter = collection["DdlHrCenter"] == null ? null : collection["DdlHrCenter"].ToString();
                agencystaff.Gender = collection["DdlGender"] == null ? null : collection["DdlGender"].ToString();
                agencystaff.PirRoleid = collection["DdlpirList"] == null ? null : collection["DdlpirList"].ToString();
                string DdlAgencyList, DdlRoleList, AvatarFile, AvatarHfile, AvatarSfile, AvatarTfile;
                // DdlAgencyList = collection["DdlAgencyList"].ToString();
                DdlAgencyList = Session["AgencyID"].ToString();
                //  DdlRoleList = collection["DdlRoleList"].ToString();
                DdlRoleList = collection["SelectedRoleId"].ToString();
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
                if (collection["ddlapprovereject"] != null && collection["ddlapprovereject"].ToString().Contains("0"))
                {
                    message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "3", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode, isEndOfYear);
                    //message = agencyData.approverejectrequestagencyHR(agencystaff.AgencyStaffId.ToString(), Convert.ToString(Session["UserID"]));
                }
                else
                {
                    message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "2", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode, isEndOfYear);
                }
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
        [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult editStaff(string id = "0", string ak="0")
        {
            AgencyStaff _staffList = null;
            try
            {
                //if (Session["RoleName"] != null && (Session["RoleName"].ToString().ToUpper().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5")))
                ViewData["Title"] = "Edit Staff";
                ViewBag.mode = 1;
                ViewBag.IsEndOfYear = "0";
                ViewBag.IsEndOfYear = ak;
                bool endYear = string.IsNullOrEmpty(ak) ? false : ak == "1" ? true : false;

                _staffList = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString(), 1, Guid.Parse(id),null, endYear);
                Session["oldemailid"] = _staffList.EmailAddress;

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(_staffList);

        }
        [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult editStaff(AgencyStaff agencystaff, FormCollection collection, FamilyHousehold.Center Centers, FamilyHousehold.Role Rolelist, List<PrimaryLanguages> PrimaryLanguages, string endOfYear="0")
        {
            AgencyStaff _staffList = new AgencyStaff();
            try
            {

                ViewBag.IsEndOfYear = endOfYear;

                bool isEndOfYear = string.IsNullOrEmpty(endOfYear) ? false : endOfYear == "1" ? true : false;

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
                #region verifying Agency user code here
                string message = "";
                ViewBag.mode = 1;
                agencystaff.LangList = PrimaryLanguages;
                Updatestaff(agencystaff, collection, out message, isEndOfYear);

                if (message == "1")
                {
                    TempData["message"] = "Record updated successfully. ";
                    if (Session["oldemailid"] != null)
                    {
                        if (Session["oldemailid"].ToString().ToUpper() != agencystaff.EmailAddress.ToUpper())
                        {
                            string oldemailid = Session["oldemailid"].ToString();
                            string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                            Thread thread = new Thread(delegate ()
                            {
                                sendMail(oldemailid, agencystaff.EmailAddress, char.ToUpper(agencystaff.FirstName[0]) + agencystaff.FirstName.Substring(1), oldemailid + "," + agencystaff.EmailAddress, Server.MapPath("~/MailTemplate"), imagepath);

                            });
                            thread.Start();
                            //SendMail.SendEmailoldnew(Session["oldemailid"].ToString(), agencyinfo.primaryEmail, char.ToUpper(agencyinfo.agencyName[0]) + agencyinfo.agencyName.Substring(1), Session["oldemailid"].ToString() + "," + agencyinfo.primaryEmail, Server.MapPath("~/MailTemplate"));
                            Session["oldemailid"] = agencystaff.EmailAddress;
                        }
                    }
                    return Redirect("~/Agency/viewagencystaff?ak="+endOfYear+"");
                    //return Redirect("~/AgencyUser/editStaff/" + agencystaff.AgencyStaffId);
                }
                else if (message == "2")
                    ViewBag.message = "Email already exist.";
                else if (message == "4")
                    ViewBag.message = "You don't have access to change the role of agency admin.";
                else if (message == "5")
                    ViewBag.message = "You don't have access to change your role.";
                else if (message == "8")
                    ViewBag.message = "User role already exists in center. Please select another center.";
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
        public void Updatestaff(AgencyStaff agencystaff, FormCollection collection, out string res, bool isEndOfYear=false)
        {
            res = "";
            try
            {
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
                //agencystaff.AccessDays = collection["DdlAccessType"].ToString() == "-1" ? null : collection["DdlAccessType"].ToString();
                agencystaff.HRCenter = collection["DdlHrCenter"] == null ? null : collection["DdlHrCenter"].ToString();
                agencystaff.Gender = collection["DdlGender"] == null ? null : collection["DdlGender"].ToString();
                agencystaff.PirRoleid = collection["DdlpirList"] == null ? null : collection["DdlpirList"].ToString();
                string DdlAgencyList, DdlRoleList, AvatarFile, AvatarHfile, AvatarSfile, AvatarTfile;
                DdlAgencyList = Session["AgencyID"].ToString();
                DdlRoleList = collection["DdlRoleList"].ToString();
                if (DdlRoleList == "a65bb7c2-e320-42a2-aed4-409a321c08a5")
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
                message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "1", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode,isEndOfYear);
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
        [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult staffReport()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View();
            }

        }
        [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [HttpPost]
        public ActionResult staffReport(string command)
        {
            try
            {
                Export export = new Export();
                Agencyreport agencyReport = agencyData.agencystaffreoprt(Session["AgencyID"].ToString());


                if (command.Contains("excel"))
                {

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Section B of the PIR " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                    MemoryStream ms = export.Exportexcel(agencyReport);
                    ms.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();

                    //
                    //MemoryStream ms = export.Exportexcel(agencyReport, strexcel);
                    //if (ms != null)
                    //{
                    //    ms.Seek(0, SeekOrigin.Begin);
                    //    return File(ms, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "Section B of the PIR " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                    //}
                }
                else
                {
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.pdf", "Section B of the PIR " + DateTime.Now.ToString("MM/dd/yyyy")));
                    Stream strpdf = Response.OutputStream;
                    export.Exportpdf(agencyReport, strpdf, Server.MapPath("~/Content/img/logo_email.png"));
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View();
        }
        public void sendMail(string emailold, string emailidnew, string name, string emailcombine, string path, string imagepath)
        {
            try
            {

                SendMail.SendEmailoldnew(emailold, emailidnew, name, emailcombine, path, imagepath);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }


        }
        public void sendMailVerificationEmail(string email, string name, string path, string template, string code, string imagepath)
        {

            SendMail.SendHRverificationemail(email, name, path, template, code, imagepath);
        }
        public void SendEmailVerificationMail(string email, string name, string path, string template)
        {
            string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
            SendMail.Sendverificationemail(email, name, path, template, imagepath);
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult rejectedList()
        {
            return View();
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult listrejectedList(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = agencyData.getrejectedList(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString()).ToList();
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        public ActionResult staffpersonalinformation()
        {
            AgencyStaff _staffList = new AgencyStaff();
            try
            {
                if (Request.QueryString["id"] != null)
                {
                    _staffList = agencyData.GetData_AllDropdown();
                    List<string> staffinfo = new List<string>();
                    staffinfo= agencyData.getEmail(Request.QueryString["id"].ToString());
                    if (staffinfo.Count>0 && staffinfo[5].ToString() == "1")
                    {
                        return Redirect("~/AgencyUser/staffpersonalinfo");
                    }
                    if (staffinfo.Count > 0)
                    {
                        _staffList.EmailAddress = staffinfo[0].ToString();
                        _staffList.FirstName = staffinfo[1].ToString();
                        _staffList.LastName = staffinfo[2].ToString();
                        _staffList.CellNumber = staffinfo[3].ToString();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(_staffList);
        }
        [HttpPost]
        public ActionResult staffpersonalinformation(AgencyStaff _staflist, FormCollection collection, List<PrimaryLanguages> PrimaryLanguages)
        {

            try
            {
                if (Request.QueryString["id"] != null)
                {
                    _staflist.Race = collection["DdlRaceList"] == null ? null : collection["DdlRaceList"].ToString();
                    _staflist.Natinality = collection["DdlNationList"] == null ? null : collection["DdlNationList"].ToString();
                    _staflist.HighestEducation = collection["DdlHighestEducation"] == null ? null : collection["DdlHighestEducation"].ToString();
                    _staflist.EarlyChildHood = collection["DdlEarlyChildHood"] == null ? null : collection["DdlEarlyChildHood"].ToString();
                    _staflist.GettingDegree = collection["DdlGettingDegree"] == null ? null : collection["DdlGettingDegree"].ToString();
                    _staflist.Gender = collection["DdlGender"] == null ? null : collection["DdlGender"].ToString();
                    string AvatarFile, AvatarHfile, AvatarSfile, AvatarTfile;
                    _staflist.LangList = PrimaryLanguages;
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
                    if (_staflist.Avatar != null)
                    {
                        AvatarFile = _staflist.Avatar.FileName;
                        string Fullpath = Uploadpath + "/" + AvatarFile;
                        #region Delete previous file if exist
                        try
                        {
                            if (!String.IsNullOrEmpty(_staflist.AvatarUrl))
                            {

                                FileInfo fin = new FileInfo(Server.MapPath(Uploadpath + "/" + _staflist.AvatarUrl.ToString()));
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
                            _staflist.Avatar.SaveAs(Server.MapPath(_newfilename));
                        }
                        catch (Exception ex)
                        {
                            clsError.WriteException(ex);
                            Server.ClearError();
                        }
                        _staflist.AvatarUrl = Path.GetFileName(_newfilename);

                    }
                    if (_staflist.AvatarH != null)
                    {
                        AvatarHfile = _staflist.AvatarH.FileName;
                        string Fullpath = Uploadpath + "/" + AvatarHfile;
                        #region Delete previous file if exist
                        try
                        {
                            if (!String.IsNullOrEmpty(_staflist.AvatarhUrl))
                            {
                                FileInfo fin = new FileInfo(Server.MapPath(Uploadpath + "/" + _staflist.AvatarhUrl.ToString()));
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
                            _staflist.AvatarH.SaveAs(Server.MapPath(_newfilename));
                        }
                        catch (Exception ex)
                        {
                            clsError.WriteException(ex);
                            Server.ClearError();
                        }
                        _staflist.AvatarhUrl = Path.GetFileName(_newfilename);
                    }

                    if (_staflist.AvatarS != null)
                    {
                        AvatarSfile = _staflist.AvatarS.FileName;
                        string Fullpath = Uploadpath + "/" + AvatarSfile;
                        #region Delete previous file if exist
                        try
                        {
                            if (!String.IsNullOrEmpty(_staflist.AvatarsUrl))
                            {
                                FileInfo fin = new FileInfo(Server.MapPath(Uploadpath + "/" + _staflist.AvatarsUrl.ToString()));
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
                            _staflist.AvatarS.SaveAs(Server.MapPath(_newfilename));
                        }
                        catch (Exception ex)
                        {
                            clsError.WriteException(ex);
                            Server.ClearError();
                        }
                        _staflist.AvatarsUrl = Path.GetFileName(_newfilename);
                    }
                    if (_staflist.AvatarT != null)
                    {
                        AvatarTfile = _staflist.AvatarT.FileName;
                        string Fullpath = Uploadpath + "/" + AvatarTfile;
                        #region Delete previous file if exist
                        try
                        {
                            if (!String.IsNullOrEmpty(_staflist.AvatartUrl))
                            {
                                FileInfo fin = new FileInfo(Server.MapPath(Uploadpath + "/" + _staflist.AvatartUrl.ToString()));
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
                            _staflist.AvatarT.SaveAs(Server.MapPath(_newfilename));
                        }
                        catch (Exception ex)
                        {
                            clsError.WriteException(ex);
                            Server.ClearError();
                        }
                        _staflist.AvatartUrl = Path.GetFileName(_newfilename);
                    }
                    #endregion
                    if (agencyData.emailVerification(_staflist, Request.QueryString["id"].ToString()) == "1")
                    {

                        return Redirect("~/AgencyUser/staffpersonalinfo");
                    }
                    else
                    {
                        ViewBag.message = "Please try again.";
                    }
                    _staflist = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString());
                    List<string> staffinfo = new List<string>();
                    staffinfo= agencyData.getEmail(Request.QueryString["id"].ToString());

                    if(staffinfo.Count>0)
                    {
                        _staflist.EmailAddress = staffinfo[0].ToString();
                        _staflist.FirstName = staffinfo[1].ToString();
                        _staflist.LastName = staffinfo[2].ToString();
                        _staflist.CellNumber = staffinfo[3].ToString();
                    }
                 
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(_staflist);
        }
        public ActionResult staffpersonalinfo()
        {

            return View();
        }
        public void sendMail(string email, string name, string path, string template, string imagepath)
        {

            SendMail.Sendverificationemail(email, name, path, template, imagepath);
        }


        [HttpGet]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult FamilyDetails(string id = "0")
        {
            int clintid = 0;
           
            if (Request.QueryString["ClientId"] != null)
            {

                ViewBag.ClientId = Request.QueryString["ClientId"].ToString();

            }
            if (Request.QueryString["ClientName"] != null)
            {
                ViewBag.ClintName = Request.QueryString["ClientName"].ToString();
            }

            if(id== "undefined")
            {
                id = TempData["HouseId"].ToString();
            }
            TempData["HouseId"] = id;
            id = EncryptDecrypt.Decrypt64(id);

            FamilyHousehold _familyinfo = familyData.GetData_AllDropdown(id, 0, Session["AgencyID"].ToString(), Session["UserID"].ToString(),Session["RoleID"].ToString());
            //FamilyHousehold _centerinfo = familyData.GetCenterData(Session["AgencyID"].ToString(), id);
            if (!string.IsNullOrEmpty(Request.QueryString["returned"]) || Convert.ToString(Request.QueryString["returned"]) == "1")
                _familyinfo.Returned = Request.QueryString["returned"].ToString();
            try
            {
                if (id == "0")
                {
                    ViewBag.mode = 0;
                    ViewBag.lang = _familyinfo.langList;
                    TempData["familyinfo"] = ViewBag.lang;
                    ViewBag.RaceSubCategory = _familyinfo.raceCategory;
                    TempData["RaceSubcategory"] = ViewBag.RaceSubCategory;
                    ViewBag.Race = _familyinfo.raceList;
                    TempData["Race"] = ViewBag.Race;
                    ViewBag.Relationship = _familyinfo.relationship;
                    TempData["Relationship"] = ViewBag.Relationship;
                    TempData["Programtype"] = _familyinfo.AvailableProgram;
                    _familyinfo.Income2 = GenerateIncomeList1();
                    _familyinfo.Income1 = GenerateIncomeList();
                    //Child Health History
                    TempData["AvailableDisease"] = _familyinfo.AvailableDisease;
                    TempData["AvailableDiagnosedDisease"] = _familyinfo.AvailableDiagnosedDisease;
                    TempData["AvailableDental"] = _familyinfo.AvailableDental;
                    //End
                    return View(_familyinfo);
                }
                else
                {
                    ViewBag.mode = 1;
                    FamilyHousehold obj = new FamilyHousehold();
                    obj = familyData.EditFamilyInfo(id, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Session["Roleid"].ToString());
                    Session["Docsstorage"] = obj.docstorage.ToString();
                    //foreach (var item in obj.QualifierRecords)
                    //{
                    //    if (item.clientid == clintid)
                    //    {
                    //        ViewBag.clientName = item.Name;
                    //        ViewBag.clientid  = EncryptDecrypt.Encrypt64(clintid.ToString());
                    //    }
                    //}
                    obj.AvailableProgram = _familyinfo.AvailableProgram;
                    obj.SchoolList = _familyinfo.SchoolList;
                    obj.ImmunizationRecords = _familyinfo.ImmunizationRecords;
                    obj.AvailableDental = _familyinfo.AvailableDental;
                    obj.AvailableDiagnosedDisease = _familyinfo.AvailableDiagnosedDisease;
                    obj.AvailableDisease = _familyinfo.AvailableDisease;
                    obj.AvailableEHS = _familyinfo.AvailableEHS;
                    obj.AvailableChildDrink = _familyinfo.AvailableChildDrink;//Changes
                    obj.AvailableChildDietFull = _familyinfo.AvailableChildDietFull;
                    obj.AvailableChildVitamin = _familyinfo.AvailableChildVitamin;
                    obj.AvailableService = _familyinfo.AvailableService;
                    obj.AvailablePrblms = _familyinfo.AvailablePrblms;
                    obj.Returned = _familyinfo.Returned;
                    if (obj.Income1 == null)
                        obj.Income1 = GenerateIncomeList();
                    if (obj.Income2 == null)
                        obj.Income2 = GenerateIncomeList1();
                    Session["HouseholdID"] = id;
                    ViewBag.tabpage = "1";
                    ViewBag.lang = _familyinfo.langList;
                    TempData["familyinfo"] = ViewBag.lang;
                    ViewBag.RaceSubCategory = _familyinfo.raceCategory;
                    TempData["RaceSubcategory"] = ViewBag.RaceSubCategory;
                    ViewBag.Race = _familyinfo.raceList;
                    TempData["Race"] = ViewBag.Race;
                    ViewBag.Relationship = _familyinfo.relationship;
                    TempData["Relationship"] = ViewBag.Relationship;
                    TempData["Programtype"] = _familyinfo.AvailableProgram;
                    TempData["Schooldistrict"] = _familyinfo.SchoolList;
                    TempData["ImmunizationRecords"] = _familyinfo.ImmunizationRecords;
                    //Child Health History
                    TempData["AvailableDisease"] = _familyinfo.AvailableDisease;
                    TempData["AvailableDiagnosedDisease"] = _familyinfo.AvailableDiagnosedDisease;
                    TempData["AvailableDental"] = _familyinfo.AvailableDental;
                    TempData["AvailableEHS"] = _familyinfo.AvailableEHS;
                    TempData["AvailableChildDrink"] = _familyinfo.AvailableChildDrink;
                    TempData["AvailableChildDietFull"] = _familyinfo.AvailableChildDietFull;
                    TempData["AvailableChildVitamin"] = _familyinfo.AvailableChildVitamin;
                    TempData["AvailableService"] = _familyinfo.AvailableService;
                    TempData["AvailablePrblms"] = _familyinfo.AvailablePrblms;
                    ViewBag.diet = _familyinfo.dietList;
                    TempData["DietInfo"] = ViewBag.diet;
                    ViewBag.food = _familyinfo.foodList;
                    TempData["foodInfo"] = ViewBag.food;
                    ViewBag.ChildFeed = _familyinfo.CFeedList;
                    TempData["ChildFeed"] = ViewBag.ChildFeed;
                    ViewBag.hungry = _familyinfo.ChungryList;
                    TempData["hungry"] = ViewBag.hungry;
                    ViewBag.ChildFormula = _familyinfo.CFormulaList;
                    TempData["ChildFormula"] = ViewBag.ChildFormula;
                    ViewBag.ChildCereal = _familyinfo.CFeedCerealList;
                    TempData["ChildCereal"] = ViewBag.ChildCereal;
                    ViewBag.ChildReferal = _familyinfo.CReferalCriteriaList;
                    TempData["ChildReferal"] = ViewBag.ChildReferal;
                    ViewBag.PMConditions = _familyinfo.PMCondtnList;
                    TempData["PMConditions"] = ViewBag.PMConditions;
                    //Changes ClientAssignedTo
                    ViewBag.ClientAssignedTo = _familyinfo.ClientAssignedTo;
                    TempData["ClientAssignedTo"] = ViewBag.ClientAssignedTo;
                    TempData["ScreeningQuestion"] = obj.customscreening;


                    obj.TabId = "1";
                    //Added on 26Dec2016
                    //ViewBag.Center = _centerinfo.Centers;
                    //TempData["Center"] = ViewBag.Center;
                    //obj.AvailableWorkshop = _centerinfo.AvailableWorkshop;//Changes
                    //obj.AvailableWorkshopDetails = _centerinfo.AvailableWorkshopDetails;
                    //obj.WorkshopDate = _centerinfo.WorkshopDate;
                    //obj.WorkshopId = _centerinfo.WorkshopId;
                    //obj.CenterDetails = _centerinfo.CenterDetails;
                    //Changes on 28Dec2016
                    ViewBag.Center = _familyinfo.Centers;
                    TempData["Center"] = ViewBag.Center;
                    obj.AvailableWorkshop = _familyinfo.AvailableWorkshop;//Changes
                    //obj.AvailableWorkshopDetails = _familyinfo.AvailableWorkshopDetails;
                    TempData["AvailableWorkshop"] = _familyinfo.AvailableWorkshop;
                    obj.WorkshopDate = _familyinfo.WorkshopDate;
                    obj.WorkshopId = _familyinfo.WorkshopId;
                    obj.CenterDetails = _familyinfo.CenterDetails;
                    //End
                    //End

                    if (!string.IsNullOrEmpty(Request.QueryString["Yakkrid"]))
                        obj.YakkrId = EncryptDecrypt.Decrypt64(Request.QueryString["Yakkrid"].ToString());
                    if (!string.IsNullOrEmpty(Request.QueryString["Staffid"]))
                        obj.StaffId = Request.QueryString["Staffid"].ToString();

                    if (!string.IsNullOrEmpty(obj.YakkrId) && !string.IsNullOrEmpty(obj.StaffId))
                    {
                        ViewBag.tabpage = "9";

                    }

                    if (TempData["DeleteParent"] != null && TempData["DeleteParent"].ToString() == "1")
                    {
                        ViewBag.tabpage = "3";
                        ViewBag.message = " Parent/Guardian deleted successfully.";
                    }
                    return View(obj);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View(_familyinfo);
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        [HttpPost]
        public ActionResult FamilyDetails(FamilyHousehold info, List<FamilyHousehold.calculateincome> Income1,
        List<FamilyHousehold.calculateincome1> Income2, string Command, FormCollection collection, List<FingerprintsModel.FamilyHousehold.phone> PhoneNos,
        List<FingerprintsModel.FamilyHousehold.Parentphone1> ParentPhone1, List<FingerprintsModel.FamilyHousehold.Parentphone2> ParentPhoneNos1,
        FamilyHousehold.PostedProgram PostedPostedPrograms, List<FamilyHousehold.ImmunizationRecord> Imminization, Screening _screen,
             FamilyHousehold.PostedPMService PostedPostedService, FamilyHousehold.PostedPMProblems PostedPostedPrblms, FamilyHousehold.PostedDisease PostedPostedDisease,
            FamilyHousehold.PostedDiagnosedDisease PostedPostedDiagnosedDisease, FamilyHousehold.PostedChildEHS PostedPostedMedicalEHS,
             FamilyHousehold.PostedChildEHS PostedPostedEHS, Nurse.PostedChildVitamin PostedPostedChildVitamin, Nurse.PostedChildDiet PostedPostedChildDietFull,
           Nurse.PostedChildDrink PostedPostedChildDrink)
        {

            info.IsIEP = Convert.ToBoolean(Request.Form["IsIEP"] != null ? true : false);
            info.IsIFSP = Convert.ToBoolean(Request.Form["IsIFSP"] != null ? true : false);
            info.IsExpired = Convert.ToBoolean(Request.Form["IsExpired"] != null ? true : false);


            StringBuilder _string = new StringBuilder();
            if (PostedPostedPrograms.ProgramID != null)
            {
                foreach (string str in PostedPostedPrograms.ProgramID)
                {
                    _string.Append(str + ",");
                }
                info.CProgramType = _string.ToString().Substring(0, _string.Length - 1);
            }
            StringBuilder _familychildinfo = new StringBuilder();
            if (PostedPostedService.PMServiceID != null)
            {
                foreach (string str in PostedPostedService.PMServiceID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherpmservices = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedService.PMServiceID1 != null)
            {
                foreach (string str in PostedPostedService.PMServiceID1)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherpmservices1 = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedPrblms.PMPrblmID != null)
            {
                foreach (string str in PostedPostedPrblms.PMPrblmID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherproblem = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();


            if (PostedPostedPrblms.PMPrblmID1 != null)
            {
                foreach (string str in PostedPostedPrblms.PMPrblmID1)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherproblem1 = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            if (info._childprogrefid == "1")
            {
                _familychildinfo.Clear();
                if (PostedPostedDisease.DiseaseID != null)
                {
                    foreach (string str in PostedPostedDisease.DiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDirectBloodRelativeEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDiagnosedConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedMedicalEHS.ChildEHSID != null)
                {
                    foreach (string str in PostedPostedMedicalEHS.ChildEHSID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedEHS.ChildEHSID != null)
                {
                    foreach (string str in PostedPostedEHS.ChildEHSID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditions1Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildMedicalTreatmentEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditions2Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
            }
            if (info._childprogrefid == "2")
            {
                _familychildinfo.Clear();
                if (PostedPostedDisease.DiseaseID != null)
                {
                    foreach (string str in PostedPostedDisease.DiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDirectBloodRelativeHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDiagnosedConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildMedicalTreatmentHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }

            }

            _familychildinfo.Clear();
            if (PostedPostedChildVitamin.CDietInfoID != null)
            {
                foreach (string str in PostedPostedChildVitamin.CDietInfoID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildChildVitaminSupplement = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedChildDietFull.CDietInfoID != null)
            {
                foreach (string str in PostedPostedChildDietFull.CDietInfoID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildDiet = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedChildDrink.CDrinkID != null)
            {
                foreach (string str in PostedPostedChildDrink.CDrinkID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildDrink = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }

            FamilyHousehold _familyinfo = new FamilyHousehold();
            _familyinfo = info;
            try
            {
                if (Command == "SaveFamilyDetails" || Command == "Saveincome" || Command == "Saveincome1" || Command == "SaveScreening")
                {
                    #region HouseHold
                    if (info.FileaddressAvatar != null)
                    {
                        info.HFileName = info.FileaddressAvatar.FileName;
                        info.HFileExtension = Path.GetExtension(info.FileaddressAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.FileaddressAvatar.InputStream);
                        info.HImageByte = b.ReadBytes(info.FileaddressAvatar.ContentLength);
                    }
                    else
                    {
                        info.HImageByte = info.HFileInString == null ? null : Convert.FromBase64String(info.HFileInString);
                    }
                    #endregion
                    #region AddParent
                    if (collection["DdlPRole"] != null)
                        info.PRole = (collection["DdlPRole"].ToString() == "") ? null : collection["DdlPRole"];
                    if (collection["DdlPDegreeEarned"] != null)
                        info.PDegreeEarned = (collection["DdlPDegreeEarned"].ToString() == "") ? null : collection["DdlPDegreeEarned"];
                    if (info.PAvatar != null)
                    {
                        info.PFileName = info.PAvatar.FileName;
                        info.PFileExtension = Path.GetExtension(info.PAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.PAvatar.InputStream);
                        info.PImageByte = b.ReadBytes(info.PAvatar.ContentLength);
                    }
                    else
                    {
                        info.PImageByte = info.PImagejson == null ? null : Convert.FromBase64String(info.PImagejson);

                    }
                    if ((!string.IsNullOrEmpty(info.P1firstname)) && (!string.IsNullOrEmpty(info.Pfirstname)))
                    {
                        info.Parentsecondexist = 1;
                        if (collection["DdlP1Role"] != null)
                            info.P1Role = (collection["DdlP1Role"].ToString() == "") ? null : collection["DdlP1Role"];
                        if (collection["DdlP1DegreeEarned"] != null)
                            info.P1DegreeEarned = (collection["DdlP1DegreeEarned"].ToString() == "-1") ? null : collection["DdlP1DegreeEarned"];
                        if (info.P1Avatar != null)
                        {
                            info.P1FileName = info.P1Avatar.FileName;
                            info.P1FileExtension = Path.GetExtension(info.P1Avatar.FileName);
                            BinaryReader b = new BinaryReader(info.P1Avatar.InputStream);
                            info.P1ImageByte = b.ReadBytes(info.P1Avatar.ContentLength);

                        }
                        else
                        {
                            info.P1ImageByte = info.P1Imagejson == null ? null : Convert.FromBase64String(info.P1Imagejson);

                        }
                    }
                    else
                    {
                        info.Parentsecondexist = 0;
                    }
                    #endregion
                    #region Add Child
                    if (info.CAvatar != null)
                    {
                        info.CFileName = info.CAvatar.FileName;
                        info.CFileExtension = Path.GetExtension(info.CAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.CAvatar.InputStream);
                        info.CImageByte = b.ReadBytes(info.CAvatar.ContentLength);
                    }
                    else
                    {
                        info.CImageByte = info.Imagejson == null ? null : Convert.FromBase64String(info.Imagejson);
                    }
                    if (info.FiledobRAvatar != null)
                    {
                        info.DobFileName = info.FiledobRAvatar.FileName;
                        info.DobFileExtension = Path.GetExtension(info.FiledobRAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.FiledobRAvatar.InputStream);
                        info.Dobaddressform = b.ReadBytes(info.FiledobRAvatar.ContentLength);
                    }
                    if (info.FileImmunization != null)
                    {
                        info.ImmunizationFileName = info.FileImmunization.FileName;
                        info.ImmunizationFileExtension = Path.GetExtension(info.FileImmunization.FileName);
                        BinaryReader b = new BinaryReader(info.FileImmunization.InputStream);
                        info.Immunizationfileinbytes = b.ReadBytes(info.FileImmunization.ContentLength);
                    }
                    #endregion
                    #region Parent income document
                    if (info.Income1 != null)
                    {
                        foreach (FamilyHousehold.calculateincome ParentIncome1 in Income1)
                        {
                            if (ParentIncome1.SalaryAvatar1 != null)
                            {
                                ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
                                ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
                                ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
                            }
                            if (ParentIncome1.SalaryAvatar2 != null)
                            {
                                ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
                                ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
                                ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
                            }
                            if (ParentIncome1.SalaryAvatar3 != null)
                            {
                                ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
                                ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
                                ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
                            }
                            if (ParentIncome1.SalaryAvatar4 != null)
                            {
                                ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
                                ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
                                ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
                            }
                            if (ParentIncome1.NoIncomeAvatar != null)
                            {
                                ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
                                ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
                                ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
                            }

                        }



                    }
                    if (info.Income2 != null)
                    {
                        foreach (FamilyHousehold.calculateincome1 ParentIncome1 in Income2)
                        {
                            if (ParentIncome1.SalaryAvatar1 != null)
                            {
                                ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
                                ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
                                ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
                            }
                            if (ParentIncome1.SalaryAvatar2 != null)
                            {
                                ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
                                ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
                                ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
                            }
                            if (ParentIncome1.SalaryAvatar3 != null)
                            {
                                ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
                                ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
                                ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
                            }
                            if (ParentIncome1.SalaryAvatar4 != null)
                            {
                                ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
                                ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
                                ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
                            }
                            if (ParentIncome1.NoIncomeAvatar != null)
                            {
                                ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
                                ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
                                BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
                                ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
                            }
                        }
                    }
                    #endregion
                    //if (info.EAvatar != null)
                    //{
                    //    info.EFileName = info.EAvatar.FileName;
                    //    info.EFileExtension = Path.GetExtension(info.EAvatar.FileName);
                    //    BinaryReader b = new BinaryReader(info.EAvatar.InputStream);
                    //    info.EImageByte = b.ReadBytes(info.EAvatar.ContentLength);
                    //}
                    //else
                    //{
                    //    info.EImageByte = info.EImagejson == null ? null : Convert.FromBase64String(info.EImagejson);
                    //}
                    //if (info.RAvatar != null)
                    //{
                    //    info.RFileName = info.RAvatar.FileName;
                    //    info.RFileExtension = Path.GetExtension(info.RAvatar.FileName);
                    //    BinaryReader b = new BinaryReader(info.RAvatar.InputStream);
                    //    info.RImageByte = b.ReadBytes(info.RAvatar.ContentLength);

                    //}
                    //else
                    //{
                    //    info.RImageByte = info.RImagejson == null ? null : Convert.FromBase64String(info.RImagejson);

                    //}


                    //#region Screening
                    //if (_screen.Physical != null)
                    //{
                    //    _screen.PhysicalFileName = _screen.Physical.FileName;
                    //    _screen.PhysicalFileExtension = Path.GetExtension(_screen.Physical.FileName);
                    //    BinaryReader b = new BinaryReader(_screen.Physical.InputStream);
                    //    _screen.PhysicalImageByte = b.ReadBytes(_screen.Physical.ContentLength);
                    //}
                    //else
                    //{
                    //    _screen.PhysicalImageByte = _screen.PhysicalImagejson == null ? null : Convert.FromBase64String(_screen.PhysicalImagejson);

                    //}
                    //if (_screen.Dental != null)
                    //{
                    //    _screen.DentalFileName = _screen.Dental.FileName;
                    //    _screen.DentalFileExtension = Path.GetExtension(_screen.Dental.FileName);
                    //    BinaryReader b = new BinaryReader(_screen.Dental.InputStream);
                    //    _screen.DentalImageByte = b.ReadBytes(_screen.Dental.ContentLength);
                    //}
                    //else
                    //{
                    //    _screen.DentalImageByte = _screen.DentalImagejson == null ? null : Convert.FromBase64String(_screen.DentalImagejson);

                    //}
                    //if (_screen.Vision != null)
                    //{
                    //    _screen.VisionFileName = _screen.Vision.FileName;
                    //    _screen.VisionFileExtension = Path.GetExtension(_screen.Vision.FileName);
                    //    BinaryReader b = new BinaryReader(_screen.Vision.InputStream);
                    //    _screen.VisionImageByte = b.ReadBytes(_screen.Vision.ContentLength);
                    //}
                    //else
                    //{
                    //    _screen.VisionImageByte = _screen.VisionImagejson == null ? null : Convert.FromBase64String(_screen.VisionImagejson);

                    //}
                    //if (_screen.Hearing != null)
                    //{
                    //    _screen.HearingFileName = _screen.Hearing.FileName;
                    //    _screen.HearingFileExtension = Path.GetExtension(_screen.Hearing.FileName);
                    //    BinaryReader b = new BinaryReader(_screen.Hearing.InputStream);
                    //    _screen.HearingImageByte = b.ReadBytes(_screen.Hearing.ContentLength);
                    //}
                    //else
                    //{
                    //    _screen.HearingImageByte = _screen.HearingImagejson == null ? null : Convert.FromBase64String(_screen.HearingImagejson);

                    //}
                    //if (_screen.Develop != null)
                    //{
                    //    _screen.DevelopFileName = _screen.Develop.FileName;
                    //    _screen.DevelopFileExtension = Path.GetExtension(_screen.Develop.FileName);
                    //    BinaryReader b = new BinaryReader(_screen.Develop.InputStream);
                    //    _screen.DevelopImageByte = b.ReadBytes(_screen.Develop.ContentLength);
                    //}
                    //else
                    //{
                    //    _screen.DevelopImageByte = _screen.DevelopImagejson == null ? null : Convert.FromBase64String(_screen.DevelopImagejson);

                    //}
                    //if (_screen.Speech != null)
                    //{
                    //    _screen.SpeechFileName = _screen.Speech.FileName;
                    //    _screen.SpeechFileExtension = Path.GetExtension(_screen.Speech.FileName);
                    //    BinaryReader b = new BinaryReader(_screen.Speech.InputStream);
                    //    _screen.SpeechImageByte = b.ReadBytes(_screen.Speech.ContentLength);
                    //}
                    //else
                    //{
                    //    _screen.SpeechImageByte = _screen.SpeechImagejson == null ? null : Convert.FromBase64String(_screen.SpeechImagejson);

                    //}
                    //if (_screen.ScreeningAccept != null)
                    //{
                    //    _screen.ScreeningAcceptFileName = _screen.ScreeningAccept.FileName;
                    //    _screen.ScreeningAcceptFileExtension = Path.GetExtension(_screen.ScreeningAccept.FileName);
                    //    BinaryReader b = new BinaryReader(_screen.ScreeningAccept.InputStream);
                    //    _screen.ScreeningAcceptImageByte = b.ReadBytes(_screen.ScreeningAccept.ContentLength);
                    //}
                    //else
                    //{
                    //    _screen.ScreeningAcceptImageByte = _screen.ScreeningAcceptImageByte == null ? null : Convert.FromBase64String(_screen.ScreeningAcceptImagejson);

                    //}


                    //#endregion




                    if (info.Releaseform != null)
                    {
                        info.ReleaseformFileName = info.Releaseform.FileName;
                        info.ReleaseformFileExtension = Path.GetExtension(info.Releaseform.FileName);
                        BinaryReader b = new BinaryReader(info.Releaseform.InputStream);
                        info.Releaseformfileinbytes = b.ReadBytes(info.Releaseform.ContentLength);
                    }
                    #region SaveFamilyDetails
                    string message = string.Empty;
                    if (info.HouseholdId == 0)
                        message = familyData.addParentInfo(ref info, 0, Guid.Parse(Session["UserID"].ToString()), ParentPhone1, ParentPhoneNos1, Income1, Income2, Imminization, PhoneNos, _screen, Session["Roleid"].ToString(), collection, Request.Files,false);
                    else
                    {
                        message = familyData.addParentInfo(ref info, 1, Guid.Parse(Session["UserID"].ToString()), ParentPhone1, ParentPhoneNos1, Income1, Income2, Imminization, PhoneNos, _screen, Session["Roleid"].ToString(), collection, Request.Files,false);
                        new TransportationData().InsertTransportationyakkrForEnrolledChild(info.ChildId.ToString(), Session["AgencyID"].ToString(), Session["UserID"].ToString());
                    }
                    _familyinfo = info;
                    Session["Docsstorage"] = _familyinfo.docstorage.ToString();
                    if (_familyinfo.Income1 == null)
                        _familyinfo.Income1 = GenerateIncomeList();
                    if (_familyinfo.Income2 == null)
                        _familyinfo.Income2 = GenerateIncomeList1();
                    if (message == "1")
                    {
                        ViewBag.tabpage = info.TabId;
                        ViewBag.message = "Record added successfully.";
                        ViewBag.result = "Sucess";
                    }
                    else if (message == "2")
                    {
                        ViewBag.tabpage = info.TabId;
                        if (Command == "Saveincome")
                        {
                            ViewBag.tabpage = info.TabId;

                        }
                        if (Command == "Saveincome1")
                        {
                            ViewBag.tabpage = info.TabId;
                        }
                        ViewBag.message = "Record updated successfully.";
                        ViewBag.result = "Sucess";

                    }
                    else if (message == "4")
                    {
                        ViewBag.message = "Address already exists.";
                    }
                    #endregion

                }

                if (Command == "SaveOthershousehold")
                {

                    if (info.HouseHoldAvatar != null)
                    {
                        info.HouseHoldFileName = info.HouseHoldAvatar.FileName;
                        info.HouseHoldFileExtension = Path.GetExtension(info.HouseHoldAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.HouseHoldAvatar.InputStream);
                        info.HouseHoldImageByte = b.ReadBytes(info.HouseHoldAvatar.ContentLength);
                    }
                    else
                    {
                        info.HouseHoldImageByte = info.HouseHoldImagejson == null ? null : Convert.FromBase64String(info.HouseHoldImagejson);
                    }


                    ViewBag.tabpage = info.TabId;
                    string message = string.Empty;
                    message = familyData.AddOthersSummary(info, Session["AgencyID"].ToString(), Session["UserID"].ToString());

                    if (info.OthersId == 0)
                        message = familyData.addOthersInfo(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                    else
                        message = familyData.addOthersInfo(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                    _familyinfo = info;
                    if (_familyinfo.Income1 == null)
                        _familyinfo.Income1 = GenerateIncomeList();
                    if (_familyinfo.Income2 == null)
                        _familyinfo.Income2 = GenerateIncomeList1();
                    if (message == "1")
                    {
                        ViewBag.tabpage = info.TabId;
                        ViewBag.message = "Record added successfully. ";
                        if (info.Oemergencycontact)
                        {
                            ViewBag.tabpage = info.TabId;
                            ViewBag.message = "Record added successfully. Please complete the emergency contact information. ";
                            _familyinfo.EmegencyId = info.EmegencyId;
                            _familyinfo.Efirstname = info.Ofirstname;
                            _familyinfo.Emiddlename = info.Omiddlename;
                            _familyinfo.Elastname = info.Olastname;
                            _familyinfo.EDOB = info.ODOB;
                            _familyinfo.EGender = info.OGender;
                        }
                        ViewBag.result = "SucessOther";
                        _familyinfo.OthersId = 0;
                    }
                    else if (message == "2")
                    {
                        ViewBag.tabpage = info.TabId;
                        ViewBag.message = "Record updated successfully. ";
                        ViewBag.result = "SucessOther";
                        if (info.Oemergencycontact && info.Alreadyemergencycontact == 0 && info.EmegencyId != 0)
                        {
                            ViewBag.tabpage = info.TabId;
                            ViewBag.message = "Record updated successfully. Please complete the emergency contact information.";
                            _familyinfo.EmegencyId = info.EmegencyId;
                            _familyinfo.Efirstname = info.Ofirstname;
                            _familyinfo.Emiddlename = info.Omiddlename;
                            _familyinfo.Elastname = info.Olastname;
                            _familyinfo.EDOB = info.ODOB;
                            _familyinfo.EGender = info.OGender;
                        }
                        _familyinfo.OthersId = 0;
                    }

                    else
                    {
                        ViewBag.message = "Error occurred please try again.";
                    }

                }

                if (Command == "SaveEmergencyDetail")
                {
                    ViewBag.tabpage = info.TabId;
                    #region save emergency
                    if (info.EAvatar != null)
                    {
                        info.EFileName = info.EAvatar.FileName;
                        info.EFileExtension = Path.GetExtension(info.EAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.EAvatar.InputStream);
                        info.EImageByte = b.ReadBytes(info.EAvatar.ContentLength);
                    }
                    else
                    {
                        info.EImageByte = info.EImagejson == null ? null : Convert.FromBase64String(info.EImagejson);
                    }

                    string message = string.Empty;
                    if (info.EmegencyId == 0)
                        message = familyData.addEmergencyInfo(info, 0, Guid.Parse(Session["UserID"].ToString()), PhoneNos, Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                    else
                        message = familyData.addEmergencyInfo(info, 1, Guid.Parse(Session["UserID"].ToString()), PhoneNos, Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                    _familyinfo = info;
                    if (_familyinfo.Income1 == null)
                        _familyinfo.Income1 = GenerateIncomeList();
                    if (_familyinfo.Income2 == null)
                        _familyinfo.Income2 = GenerateIncomeList1();
                    if (message == "1")
                    {
                        ViewBag.tabpage = info.TabId;
                        ViewBag.message = "Record added successfully. ";
                        ViewBag.result = "Sucessemergency";
                    }
                    if (message == "2")
                    {
                        ViewBag.tabpage = info.TabId;
                        ViewBag.message = "Record updated successfully. ";
                        ViewBag.result = "Sucessemergency";
                    }
                    if (message == "")
                    {
                        ViewBag.message = "Error occoured please try again. ";
                    }


                    #endregion
                }
                if (Command == "SaveRestrictedData")
                {
                    ViewBag.tabpage = info.TabId;
                    if (info.RAvatar != null)
                    {
                        info.RFileName = info.RAvatar.FileName;
                        info.RFileExtension = Path.GetExtension(info.RAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.RAvatar.InputStream);
                        info.RImageByte = b.ReadBytes(info.RAvatar.ContentLength);

                    }
                    else
                    {
                        info.RImageByte = info.RImagejson == null ? null : Convert.FromBase64String(info.RImagejson);

                    }
                    string message = string.Empty;
                    if (info.RestrictedId == 0)
                        message = familyData.addRestrictedInfo(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                    else
                        message = familyData.addRestrictedInfo(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                    _familyinfo = info;
                    if (_familyinfo.Income1 == null)
                        _familyinfo.Income1 = GenerateIncomeList();
                    if (_familyinfo.Income2 == null)
                        _familyinfo.Income2 = GenerateIncomeList1();
                    if (message == "1")
                    {
                        ViewBag.tabpage = info.TabId;
                        ViewBag.message = "Record added successfully. ";
                        ViewBag.result = "SucessRestricted";
                    }
                    else if (message == "2")
                    {
                        ViewBag.tabpage = info.TabId;
                        ViewBag.message = "Record updated successfully. ";
                        ViewBag.result = "SucessRestricted";
                        _familyinfo.RestrictedId = 0;
                    }
                    else
                    {
                        ViewBag.message = "Error occurred please try again. ";
                    }
                    _familyinfo.RestrictedId = Convert.ToInt32(info.HouseholdId);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            ViewBag.mode = 1;
            ViewBag.lang = TempData["familyinfo"];
            ViewBag.Race = TempData["Race"];
            ViewBag.RaceSubCategory = TempData["RaceSubcategory"];
            ViewBag.Relationship = TempData["Relationship"];
            _familyinfo.AvailableProgram = (List<FamilyHousehold.Programdetail>)TempData["Programtype"];
            _familyinfo.ImmunizationRecords = (List<FamilyHousehold.ImmunizationRecord>)TempData["ImmunizationRecords"];
            _familyinfo.AvailableDisease = (List<FamilyHousehold.ChildDirectBloodRelative>)TempData["AvailableDisease"];
            _familyinfo.AvailableDiagnosedDisease = (List<FamilyHousehold.ChildDiagnosedDisease>)TempData["AvailableDiagnosedDisease"];
            _familyinfo.AvailableDental = (List<FamilyHousehold.ChildDental>)TempData["AvailableDental"];
            _familyinfo.SchoolList = (List<SchoolDistrict>)TempData["Schooldistrict"];
            _familyinfo.AvailableChildDrink = (List<FamilyHousehold.ChildDrink>)TempData["AvailableChildDrink"];
            _familyinfo.AvailableChildDietFull = (List<Nurse.ChildDietFull>)TempData["AvailableChildDietFull"];
            _familyinfo.AvailableChildVitamin = (List<Nurse.ChildVitamin>)TempData["AvailableChildVitamin"];
            _familyinfo.AvailableService = (List<FamilyHousehold.PMService>)TempData["AvailableService"];
            _familyinfo.AvailablePrblms = (List<FamilyHousehold.PMProblems>)TempData["AvailablePrblms"];
            _familyinfo.AvailableEHS = (List<FamilyHousehold.ChildEHS>)TempData["AvailableEHS"];
            _familyinfo.AvailableWorkshop = (List<FamilyHousehold.WorkshopDetails>)TempData["AvailableWorkshop"];
            ViewBag.diet = TempData["DietInfo"];
            ViewBag.food = TempData["foodInfo"];
            ViewBag.hungry = TempData["hungry"];
            ViewBag.ChildFeed = TempData["ChildFeed"];
            ViewBag.hungry = TempData["hungry"];
            ViewBag.ChildFormula = TempData["ChildFormula"];
            ViewBag.ChildReferal = TempData["ChildReferal"];
            ViewBag.ChildCereal = TempData["ChildCereal"];
            ViewBag.PMConditions = TempData["PMConditions"];
            // changes ClientAssignedTo
            ViewBag.ClientAssignedTo = TempData["ClientAssignedTo"];
            //End
            _familyinfo.customscreening = (DataTable)TempData["ScreeningQuestion"];
            //Added on 23Dec2016
            ViewBag.Center = TempData["Center"];
            //End
            TempData.Keep();
            return View(_familyinfo);
        }

        [CustAuthFilter()]
        public ActionResult SaveCaseNoteFromFamilySummary(CaseNote casenote)
        {

            try
            {
                FamilyData obj = new FamilyData();
                //var list = obj.childDetails(Householdid, Session["AgencyID"].ToString()).ToList();
                //return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
            return Json(true);
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult listChildDetails(string Householdid = "0")
        {
            try
            {
                FamilyData obj = new FamilyData();
                var list = obj.childDetails(Householdid, Session["AgencyID"].ToString()).ToList();
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult listEmergencyContactDetails(string Householdid = "0")
        {
            try
            {
                FamilyData obj = new FamilyData();
                var listEmergencyContact = obj.EmergencyContactDetails(Householdid, Session["AgencyID"].ToString()).ToList();
                return Json(new { listEmergencyContact });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult listRestrictedDetails(string Householdid = "0")
        {
            try
            {
                FamilyData obj = new FamilyData();
                var listRestricted = obj.RestrictedDetails(Householdid, Session["AgencyID"].ToString()).ToList();
                return Json(new { listRestricted });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult viewfamilydetails(string id = "")
        {
            if (!string.IsNullOrEmpty(id))
                TempData["CenterFamily"] = id;
            return View();
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult listhousehold(string sortOrder, string sortDirection, string search, string search1, int pageSize, string clear, bool Applicationstatus, int requestedPage = 1,int programYearApplication=0)
        {
            try
            {
                int skip = pageSize * (requestedPage - 1);
                string totalrecord;
                string IncompleteApplication;
                string[] Center = null;
                int centerid = 0;
                int option = 0;
                int householdid = 0;
                if (TempData["CenterFamily"] != null)
                {
                    Center = TempData["CenterFamily"].ToString().Split(',');
                    if (Center != null)
                    {
                        if (Center.Length > 0 && !string.IsNullOrEmpty(Center[0]))
                        {
                            centerid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Center[0]));
                        }
                        if (Center.Length > 1 && !string.IsNullOrEmpty(Center[1]))
                            option = Convert.ToInt32(Center[1]);
                        if (Center.Length > 1 && !string.IsNullOrEmpty(Center[1]) && Center[1] == "4")
                        {
                            centerid = 0;
                            option = 0;
                            householdid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Center[0]));
                        }
                    }
                }
                var list = familyData.getHouseholdList(out totalrecord, out IncompleteApplication, sortOrder, sortDirection, search.TrimEnd().TrimStart(), search1.TrimEnd().TrimStart(), skip, pageSize, Convert.ToString(Session["UserID"]), centerid, option, householdid, Applicationstatus,programYearApplication);

                return Json(new { list, totalrecord, IncompleteApplication });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]


        public JsonResult Getchild(string ChildId = "0", string HouseHoldId = "0")
        {
            FamilyHousehold family = new FamilyHousehold();
            try
            {

                 family = familyData.Getchild(ChildId, HouseHoldId, Session["AgencyID"].ToString(), Server.MapPath("~//TempAttachment//"), Session["Roleid"].ToString());

                //    wellBabyList = family.WellBabyExamModelList;
                // if(wellBabyList.)
                //  family.PhysicalExamDates = new List<WellBabyExamModel>();
                // family.PhysicalExamDates = PhysicalExamDatesInScreening(family.CDOB,family.DateOfEnrollment);

                return Json(family);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //  return Json("Error occured please try again.");
                //return Json(Ex);
                return Json(family);
            }
        }

        private List<WellBabyExamModel> PhysicalExamDatesInScreening( string DOB,string DOE)
        {
            List<WellBabyExamModel> listOfDates = new List<WellBabyExamModel>();
            DateTime dob1 = DateTime.Parse(DOB);
            DateTime DOE1 = new DateTime();
            if (string.IsNullOrEmpty(DOE))
                DOE1 = DateTime.Now;
            else
                DOE1 = DateTime.Parse(DOE);

            listOfDates.Add(EachDate(dob1, DOE1, 7, false));
            listOfDates.Add(EachDate(dob1, DOE1, 2, true));
            listOfDates.Add(EachDate(dob1, DOE1, 4, true));
            listOfDates.Add(EachDate(dob1, DOE1, 6, true));
            listOfDates.Add(EachDate(dob1, DOE1, 12, true));
            listOfDates.Add(EachDate(dob1, DOE1, 15, true));
            listOfDates.Add(EachDate(dob1, DOE1, 18, true));
            listOfDates.Add(EachDate(dob1, DOE1, 24, true));
            listOfDates.Add(EachDate(dob1, DOE1 ,30, true));
            listOfDates.Add(EachDate(dob1, DOE1,36, true));

            return listOfDates;
        }
        private WellBabyExamModel EachDate(DateTime dob1, DateTime DOE1, int monthOrDays, bool isMonth)
        {
            WellBabyExamModel data = new WellBabyExamModel();
            DateTime newDate = new DateTime();
            newDate = (isMonth) ? dob1.AddMonths(monthOrDays) : dob1.AddDays(monthOrDays);
            data.Month = ((isMonth) ? (monthOrDays + " Months") : ("1 Week"));
            data.ScheduledExamDate = (newDate.ToShortDateString());
            data.EnrollmentDate = DOE1.ToShortDateString();
            DateTime scheduledDate = DateTime.ParseExact(data.ScheduledExamDate, @"MM/dd/yyyy",
            System.Globalization.CultureInfo.InvariantCulture);
            DateTime EnrollmentDate = DateTime.ParseExact(data.EnrollmentDate, @"MM/dd/yyyy",
           System.Globalization.CultureInfo.InvariantCulture);

            if (scheduledDate > EnrollmentDate)
            {
                foreach (var model in wellBabyList)
                {
                    if (data.Month == model.Month)//&& data.ExaminedDate==data.EnrollmentDate)
                    {
                        data.ExaminedDate = model.ExaminedDate;
                        data.Status = ("Completed");
                        break;
                    }
                }
                if (data.ExaminedDate == null && scheduledDate > DateTime.Now)
                {
                    data.ExaminedDate = "--";
                    data.Status = ("--");
                }
            }
            else
            {
                data.ExaminedDate = "--";
                data.Status = "Missing";
            }


            return data;
        }

        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Getchild1(string ChildId = "0", string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.Getchild1(ChildId, HouseHoldId, Session["AgencyID"].ToString(), Server.MapPath("~//TempAttachment//"), Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }



        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Deletechild(string ChildId = "0", string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.Deletechild(ChildId, HouseHoldId, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult listParentPhoneDetails1(string HouseHoldId = "0", string ParentID = "0")
        {
            try
            {

                FamilyData obj = new FamilyData();
                var listPhone = obj.PhoneDetails(HouseHoldId, ParentID, Session["AgencyID"].ToString()).ToList();
                return Json(new { listPhone });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult listParentPhoneDetails2(string HouseHoldId = "0", string ParentID = "0")
        {
            try
            {

                FamilyData obj = new FamilyData();
                var listPhone = obj.PhoneDetails(HouseHoldId, ParentID, Session["AgencyID"].ToString()).ToList();
                return Json(new { listPhone });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult GetRestrictedlist(string RestrictedId = "0", string HouseHoldId = "0")//string RestrictedId = "0",
        {
            try
            {
                return Json(familyData.GetRestricted(RestrictedId, HouseHoldId, Session["AgencyID"].ToString()));//RestrictedId,
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult deleterestricted(string RestrictedId = "0", string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.DeleteRestricted(RestrictedId, HouseHoldId, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        [JsonMaxLengthAttribute]
        public JsonResult listOtherDetails(string Householdid = "0")
        {
            try
            {
                FamilyData obj = new FamilyData();
                var listOther = obj.OtherDetails(Householdid, Session["AgencyID"].ToString()).ToList();
                return Json(new { listOther });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult GetOther(string OthersId = "0", string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.Getother(OthersId, HouseHoldId, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult DeleteOther(string OthersId = "0", string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.Deleteother(OthersId, HouseHoldId, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        [JsonMaxLengthAttribute]
        public JsonResult Getemergencycontact(string Emergencyid = "0", string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.Getemergencycontact(Emergencyid, HouseHoldId, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult Deleteemergencycontact(string EmergencyId = "0", string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.Deleteemergencycontact(EmergencyId, HouseHoldId, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult Deletecontact(string phoneId = "0", string HouseHoldId = "0", string EmergencyId = "0")
        {
            try
            {
                return Json(familyData.Deletecontact(phoneId, HouseHoldId, EmergencyId, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Deleteparent1(string parentid = "0", string HouseHoldId = "0")
        {
            try
            {
                if (familyData.DeleteParent1(parentid, HouseHoldId, Session["AgencyID"].ToString(), Session["UserID"].ToString()) == "1")
                {
                    TempData["DeleteParent"] = "1";
                    return Json("1");
                }
                return Json("Error occurred please try again.");
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Deleteparent2(string HouseHoldId = "0")
        {
            try
            {
                string ParentID = "1";
                return Json(familyData.DeleteParent2(ParentID, HouseHoldId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult DeleteParentContact(string phoneId = "0", string HouseHoldId = "0", string Parentid = "0")
        {
            try
            {
                return Json(familyData.DeleteParentContact(phoneId, HouseHoldId, Parentid, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult DeleteParent1Contact(string phoneId = "0", string HouseHoldId = "0", string Parentid1 = "0")
        {
            try
            {
                return Json(familyData.DeleteParentContact(phoneId, HouseHoldId, Parentid1, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]


        public JsonResult AutoCompleteDoctor(string term, string IsDeleted = "1")
        {
            var result = familyData.AutoCompleteDoctr(term, Session["AgencyID"].ToString(), IsDeleted);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]


        public JsonResult AutoCompleteDentist(string term, string IsDeleted = "1")
        {
            var result = familyData.AutoCompleteDentst(term, Session["AgencyID"].ToString(), IsDeleted);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Changes by Akansha on 14Dec2016
        // [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        [CustAuthFilter()]
        public ActionResult AddDoctor()
        {

            CommunityResource _communityinfo = communitydata.GetData_AllDropdown(Session["AgencyID"].ToString());

            ViewBag.doctor = _communityinfo.doctorList;
            TempData["community"] = ViewBag.doctor;
            return View(_communityinfo);
        }
        //Changes by Akansha on 16Dec2016
        //Changes on 1Feb2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult AdddoctorAjax(string Fname, string Lname, string CompanyName, string Zip, string Phoneno, string Address, string City
         , string State, string County, string Community, string DentalCenter = "0", string DentalNotes = "0", string MedicalNotes = "0", string MedicalCenter = "0", string CompanyNameden = "0", string DocCheck = "0", string DenCheck = "0")
        {
            CommunityResourceData obj = new CommunityResourceData();
            CommunityResource Info = new CommunityResource();
            try
            {
                string result = obj.AddCommunityAjax(Info, Fname, Lname, CompanyName, Address, Zip, Phoneno, City, State, County, Session["UserID"].ToString(), Session["AgencyID"].ToString(), "0", Community, DentalCenter, DentalNotes, MedicalNotes, MedicalCenter, CompanyNameden, DocCheck, DenCheck);
                return Json(new { Info, result });
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        [CustAuthFilter()]
        public ActionResult IncomeCalculator(List<FingerprintsModel.FamilyHousehold.calculateincome> Income)
        {
            if (Income == null || Income.Count <= 0)
            {
                Income = GenerateIncomeList();
            }

            return View(Income);
        }
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        [CustAuthFilter()]
        public ActionResult IncomeCalculator2(List<FingerprintsModel.FamilyHousehold.calculateincome1> Income1)
        {
            if (Income1 == null || Income1.Count <= 0)
            {
                Income1 = GenerateIncomeList1();
            }
            return View(Income1);
        }
        // [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        [CustAuthFilter()]
        public ActionResult IncomeCalculatorFamilySummarry(List<FingerprintsModel.FamilyHousehold.calculateincome> Income)
        {
            if (Income == null || Income.Count <= 0)
            {
                Income = GenerateIncomeList();
            }

            return View(Income);
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        [JsonMaxLengthAttribute]
        public JsonResult Checkaddress(int Zipcode, string Address = "", string HouseHoldId = "0")
        {
            try
            {
                string Result;
                var Zipcodelist = familyData.Checkaddress(out Result, Address, HouseHoldId, Zipcode);
                return Json(new { Zipcodelist, Result });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter()]
        private List<FamilyHousehold.calculateincome> GenerateIncomeList()
        {
            List<FamilyHousehold.calculateincome> IncomeList = new List<FamilyHousehold.calculateincome>();
            IncomeList.Add(new FamilyHousehold.calculateincome());
            return IncomeList;

        }
        [CustAuthFilter()]
        private List<FamilyHousehold.calculateincome1> GenerateIncomeList1()
        {
            List<FamilyHousehold.calculateincome1> IncomeList = new List<FamilyHousehold.calculateincome1>();
            IncomeList.Add(new FamilyHousehold.calculateincome1());
            return IncomeList;

        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult Immunizationquestion(List<FingerprintsModel.FamilyHousehold.ImmunizationRecord> Immunization)
        {


            return View(Immunization);
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public FileResult getpdfimage(string id = "0")
        {

            string[] name = id.Split(',');
            FamilyHousehold image1 = new FamilyData().getpdfimage(Session["AgencyID"].ToString(), name[0], name[1]);
            var FileName = image1.EFileName.Split('.');
            string contentType = "application/pdf";
            if (Convert.ToString(FileName[1]) == "pdf")
            {
                return File(image1.EImageByte, contentType, FileName[0] + "." + FileName[1]);// "image/jpeg");
            }
            else
            {
                return File(image1.EImageByte, "application/octet-stream", FileName[0] + "." + FileName[1]);// "image/jpeg");
            }



            //if (image1 != null)
            //{

            //    System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(image1.EImageByte));
            //    Document doc = new Document(PageSize.A4);
            //    PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~//TempAttachment//Income.pdf"), FileMode.Create, FileAccess.Write));
            //    doc.Open();
            //    iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    doc.Add(pdfImage);
            //    doc.Close();
            //}

            //return File(Server.MapPath("~//TempAttachment//Income.pdf"), contentType, "Income.pdf");

        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult DeleteParentincome(string incomeId = "0", string HouseHoldId = "0", string Parentid = "0")
        {
            try
            {
                return Json(familyData.DeleteParentincome(incomeId, HouseHoldId, Parentid, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult getsubcategory(string Raceid = "0")
        {
            try
            {
                return Json(familyData.getsubcategory(Raceid, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Getassociatefamily(string Firstname = "", string Lastname = "", string Address = "", string ZipCode = "", string City = "", string State = "", string mode = "")
        {
            try
            {
                return Json(familyData.Getassociatefamily(Firstname, Lastname, Address, ZipCode, City, State, Session["AgencyID"].ToString(), mode, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult AddClientDetails()
        {



            return View(familyData.GetFutureApplication());
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult AddClientDetailsAjax(string HouseholdId, string Street, string StreetName, string ZipCode, string City, string State, string County, string Pfirstname, string Plastname, string Cfirstname, string Clastname, string CDOB, string CGender, bool Enrollpregnantmother, string futureIntake = "")
        {
            FamilyData obj = new FamilyData();
            try
            {
                string result = obj.AddClientAjax(HouseholdId, Street, StreetName, ZipCode, City, State, County, Pfirstname, Plastname, Cfirstname, Clastname, CDOB, CGender, Session["UserID"].ToString(), Session["AgencyID"].ToString(), "0", Enrollpregnantmother, Session["Roleid"].ToString(), futureIntake);
                return Json(EncryptDecrypt.Encrypt64(result));
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult Qualifiertab()
        {
            return View();
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult SaveNotes(string HouseHoldId = "0", string Notes = "", string mode = "")
        {
            try
            {
                string result = "0";
                var noteslist = familyData.SaveNotes(ref result, Session["AgencyID"].ToString(), Session["UserID"].ToString(), HouseHoldId, Notes, mode);
                return Json(new { noteslist, result });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetPovertyCalculation(string HouseHoldId = "0", string PovertyPercentage = "", string Totalhousehold = "")
        {
            try
            {
                return Json(familyData.GetPovertyCalculation(HouseHoldId, PovertyPercentage, Totalhousehold));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        //  [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]
        [CustAuthFilter()]
        public ActionResult ScreeningIntake(DataTable CustomScreening)
        {
            CustomScreeningIntake screening = new CustomScreeningIntake();
            try
            {
                screening.CustomScreenings = CustomScreening;
                return View(screening);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View(screening);
            }
        }
        //  [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        [CustAuthFilter()]
        public JsonResult SavePovertyCalculation(string clientid, string HouseHoldId, string Parentid1, string Parentid2 = "", string Percentage1 = "",
        string Percentage2 = "", string Amount1 = "", string Amount2 = "",
        string ChildIncome = "", string PovertyPercentage = "", string mode = "")
        {
            try
            {
                string result = "0";
                var PovertyCalculation = familyData.SavePovertyCalculation(ref result, Session["AgencyID"].ToString(), Session["UserID"].ToString(), HouseHoldId, Parentid1,
                 Parentid2, Percentage1, Percentage2, Amount1, Amount2, ChildIncome, PovertyPercentage, mode, clientid);
                return Json(new { PovertyCalculation, result });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        // [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [CustAuthFilter()]
        public JsonResult getselectionpoints(string Programid, string clientid, string householdid)
        {
            try
            {
                return Json(familyData.getselectionpoints(Programid, Session["AgencyID"].ToString(), clientid, householdid));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult Deletepmmother(string ParentId = "0", string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.Deletepmmother(ParentId, HouseHoldId, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult SaveSelectionPoint(List<SelectPoints.CustomQuestion> custompoints, string Totalselectionpoint, string Totalcustompoint,
        string Grandpoint, string clientid, string householdid)
        {
            try
            {
                return Json(familyData.SaveSelectionPoint(custompoints, Totalselectionpoint, Totalcustompoint, Grandpoint, clientid, householdid, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        public PartialViewResult FamilyHouseHoldForFamilySummary(string tabName, int houseHoldId)
        {
            FamilyHousehold list = new FamilyHousehold();
            ViewBag.tabName = tabName;
            list.getList = familyData.GetFileCabinet(tabName, Session["AgencyID"].ToString(), houseHoldId);
            return PartialView(list);
        }

        public PartialViewResult FamilyHouseHold(string tabName, int houseHoldId)
        {
            FamilyHousehold list = new FamilyHousehold();
            ViewBag.tabName = tabName;
            list.getList = familyData.GetFileCabinet(tabName, Session["AgencyID"].ToString(), houseHoldId);
            return PartialView(list);
        }

        public FileResult getpdfimage1(string id = "0")
        {
            string[] name = id.Split(',');

            FamilyHousehold image1 = new FamilyData().getpdfimage1(Session["AgencyID"].ToString(), Convert.ToInt32(name[0]), name[1], name[2]);
            var FileName = image1.HFileName.Split('.');

            string contentType = "application/pdf";

            if (Convert.ToString(FileName[1]) == "pdf")
            {
                //Document document = new Document();
                //MemoryStream stream = new MemoryStream();
                //PdfWriter pdfWriter = PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~//TempAttachment//" + FileName[0] + ".pdf"), FileMode.Create, FileAccess.Write, FileShare.None));
                //document.Open();
                //document.Add(new PdfContents(image1.HFileName));
                //return File(FileName[0] + ".pdf", "application/pdf");

                // byte[] contents = image1.EImageByte; //GetFileContentsFromDatabase();
                return File(image1.EImageByte, contentType, FileName[0] + "." + FileName[1]);// "image/jpeg");

            }
            else
            {
                //if (image1 != null)
                //{
                return File(image1.EImageByte, "application/octet-stream", FileName[0] + "." + FileName[1]);// "image/jpeg");

                //System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(image1.EImageByte));
                //Document doc = new Document(PageSize.A4);
                //PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~//TempAttachment//" + FileName[0] + ".pdf"), FileMode.Create, FileAccess.Write));
                //doc.Open();
                //iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);
                //doc.Add(pdfImage);
                //doc.Close();
                //}
                //return File(Server.MapPath("~//TempAttachment//" + FileName[0] + ".pdf"), contentType, "" + FileName[0] + ".pdf");
            }
        }
        [JsonMaxLengthAttribute]
        public JsonResult getagencyid(string Agencyid = "0", string roleid = "",string programYear="")
        {
            try
            {
                return Json(familyData.getagencyid(Agencyid, roleid,programYear));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult GetNurse(string Centerid = "0")
        {
            try
            {
                return Json(familyData.GetNurse(Centerid, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult SaveMultipleAcceptanceprocess(List<String> ClientList, string Usernurseid, string householdid, string centerid, string Programid)
        {
            try
            {
                return Json(familyData.SaveMultipleAcceptanceprocess(ClientList, Usernurseid, householdid, centerid, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Programid));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult SaveAcceptanceprocess(string Clientid, string UserFSWId, string householdid, string centerid, string Programid)
        {
            try
            {

                // return Json(familyData.SaveAcceptanceprocess(Clientid, Usernurseid, householdid, centerid, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Programid));
                return Json(familyData.SaveHirarchyAcceptanceprocess(Clientid, UserFSWId, householdid, centerid, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Programid));

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Getallcenter(string mode,bool homebased)
        {
            try
            {
                return Json(familyData.Getallcenter(mode, Session["Roleid"].ToString(), Session["AgencyID"].ToString(), Session["UserID"].ToString(), homebased));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult SaveWaitingclient(List<Waitinginfo> waitinglist)
        {
            try
            {
                return Json(familyData.SaveWaitingclient(waitinglist, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult AutoCompleteFamily(string term, string Active = "0")
        {
            try
            {
                var result = familyData.AutoCompletefamilyList(term, Session["AgencyID"].ToString(), Session["UserID"].ToString(),Session["RoleID"].ToString(), Active);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        public JsonResult AutoCompleteWithdrawnList(string term, string trans,string progYear="")
        {
            try
            {
                var result = familyData.AutoCompleteWithdrawnList(term, Session["AgencyID"].ToString(), Session["UserID"].ToString(), trans,progYear);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult Yakkrlist(string id = "0")
        {

            try
            {

                int centerid = 0;
                int option = 0;
                string[] Center = null;
                if (!string.IsNullOrEmpty(id))
                {
                    Center = id.Split(',');
                    if (Center != null)
                    {
                        if (Center.Length > 0 && !string.IsNullOrEmpty(Center[0]))
                            centerid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Center[0]));
                        if (Center.Length > 1 && !string.IsNullOrEmpty(Center[1]))
                            option = Convert.ToInt32(Center[1]);

                    }
                    ViewBag.userlist = familyData.Getallyakkrclients(centerid, option, Session["AgencyID"].ToString(), Session["UserID"].ToString());

                }
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult YakkrAccepted()
        {
            try
            {

                ViewBag.Centerlist = familyData.Getyakkraccepted(Session["AgencyID"].ToString(), Session["UserID"].ToString());
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult Checkallmanadatoryfield(string Clientid, string Householid)
        {
            try
            {
                return Json(familyData.Checkallmanadatoryfield(Clientid, Householid, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        //     [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult ClientAssigned(string yakkrid, string Staffid, string HouseHoldId = "0")
        {
            try
            {
                string result = "0";
                return Json(familyData.SaveClientAssigned(ref result, Session["AgencyID"].ToString(), Session["UserID"].ToString(), HouseHoldId, Staffid, yakkrid));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public FileResult getpdfimageScreening(string id = "0")
        {

            string[] name = id.Split(',');
            FamilyHousehold image1 = new FamilyData().getpdfimageScreen(Session["AgencyID"].ToString(), name[0], name[1]);
            var FileName = image1.EFileName.Split('.');
            string contentType = "application/pdf";
            if (Convert.ToString(FileName[1]) == "pdf")
            {
                return File(image1.EImageByte, contentType, FileName[0] + "." + FileName[1]);// "image/jpeg");
            }
            else
            {
                return File(image1.EImageByte, "application/octet-stream", FileName[0] + "." + FileName[1]);// "image/jpeg");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public FileResult GetcustompdfimageScreening(string id = "0")
        {

            string[] name = id.Split(',');
            FamilyHousehold image1 = new FamilyData().GetCustompdfimageScreen(Session["AgencyID"].ToString(), name[0], name[1]);
            var FileName = image1.EFileName.Split('.');
            string contentType = "application/pdf";
            if (Convert.ToString(FileName[1]) == "pdf")
            {
                return File(image1.EImageByte, contentType, FileName[0] + "." + FileName[1]);// "image/jpeg");
            }
            else
            {
                return File(image1.EImageByte, "application/octet-stream", FileName[0] + "." + FileName[1]);// "image/jpeg");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914d792,a31b1716-b042-46b7-acc0-95794e378b26,a65bb7c2-e320-42a2-aed4-409a321c08a5,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult UserProfile()
        {
            return View();
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult SaveAcceptEnrollInfo(string Clientid, string ClassroomID, string centerid, string Reason, string StartDate, string Programid, string ChildTran, string ChildBMI, string ChildDis, string ChildFood)
        {
            try
            {
                string result = familyData.SaveAcceptanceenrollinfo(Clientid, ClassroomID, centerid, Reason, StartDate, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Programid, ChildTran, ChildBMI, ChildDis, ChildFood);

                if (result == "1")
                {
                    new TransportationData().InsertTransportationyakkr(Clientid, null, Session["AgencyID"].ToString(), Session["UserID"].ToString(), ClassroomID, centerid);
                }
                return Json(result);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult SaveHVAcceptEnrollInfo(string Clientid, string ClassroomID, string centerid, string Reason, string StartDate, string Programid, string ChildTran, string ChildBMI, string ChildDis, string ChildFood, Scheduler objsch)
        {
            FamilyData obj = new FamilyData();
            try
            {
                bool isFromYakkr600601 = false;
                string result1 = obj.AddSchedulerDetails(objsch.MeetingDescription, objsch.Date, objsch.StartTime, objsch.Duration, objsch.EndTime, "", Clientid, objsch.Description, "", Session["UserID"].ToString(), Session["AgencyID"].ToString(), isFromYakkr600601);//Date, "0", Community, DentalCenter, DentalNotes, MedicalNotes, MedicalCenter, CompanyNameden);
                                                                                                                                                                                                                                                        //return Json(new { result });
                string result2 = string.Empty;
                if (result1 == "1")
                {
                    result2 = familyData.SaveAcceptanceenrollinfo(Clientid, ClassroomID, centerid, Reason, StartDate, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Programid, ChildTran, ChildBMI, ChildDis, ChildFood);

                }
                string result = result2;
                return Json(new { result });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult SaveHealthreview(string Clientid, string Usernurseid, string householdid, string centerid, string Programid)
        {
            try
            {
                return Json(familyData.SaveHealthreview(Clientid, Usernurseid, householdid, centerid, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Programid));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult FamilySummary(string id = "0")
        {
            FamilyHousehold FamilyObject = new FamilyHousehold();
            try
            {
                familyData.FamilySummary(FamilyObject, id, Session["AgencyID"].ToString(), Session["UserID"].ToString(),Session["RoleID"].ToString());
                Session["Docsstorage"] = FamilyObject.docstorage.ToString();
                if (FamilyObject.Income1 == null)
                    FamilyObject.Income1 = GenerateIncomeList();
                TempData["ExistPmprogram"] = FamilyObject.ExistPmprogram;
                ViewBag.lang = FamilyObject.langList;
                TempData["language"] = ViewBag.lang;
                ViewBag.Relationship = FamilyObject.relationship;
                TempData["Relationship"] = FamilyObject.relationship;
                ViewBag.Race = FamilyObject.raceList;
                TempData["Race"] = ViewBag.Race;
                TempData["SchoolList"] = FamilyObject.SchoolList;
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(FamilyObject);
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]


        [HttpPost]
        public ActionResult FamilySummary(FamilyHousehold info, string Command, List<FingerprintsModel.FamilyHousehold.phone> PhoneNos)
        {
            string message = "";

            try
            {
                if (Command == "Householdbutton")
                {

                    if (info.FileaddressAvatar != null)
                    {
                        info.HFileName = info.FileaddressAvatar.FileName;
                        info.HFileExtension = Path.GetExtension(info.FileaddressAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.FileaddressAvatar.InputStream);
                        info.HImageByte = b.ReadBytes(info.FileaddressAvatar.ContentLength);
                    }
                    else
                    {
                        info.HImageByte = info.HFileInString == null ? null : Convert.FromBase64String(info.HFileInString);
                    }

                    message = familyData.SaveFamilySummary(info, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                    if (message == "1")
                    {
                        ViewBag.message = "Household summary updated successfully.";

                    }
                }
                else if (Command == "SaveOthershousehold")
                {


                    if (info.HouseHoldAvatar != null)
                    {
                        info.HouseHoldFileName = info.HouseHoldAvatar.FileName;
                        info.HouseHoldFileExtension = Path.GetExtension(info.HouseHoldAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.HouseHoldAvatar.InputStream);
                        info.HouseHoldImageByte = b.ReadBytes(info.HouseHoldAvatar.ContentLength);
                    }
                    else
                    {
                        info.HouseHoldImageByte = info.HouseHoldImagejson == null ? null : Convert.FromBase64String(info.HouseHoldImagejson);
                    }

                    message = familyData.AddOthersSummary(info, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                    if (message == "1")
                    {
                        ViewBag.message = "Others in household added successfully.";

                    }
                    else if (message == "2")
                    {
                        ViewBag.message = "Others in household updated successfully.";

                    }
                }
                else if (Command == "SaveEmergencyDetail")
                {

                    if (info.EAvatar != null)
                    {
                        info.EFileName = info.EAvatar.FileName;
                        info.EFileExtension = Path.GetExtension(info.EAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.EAvatar.InputStream);
                        info.EImageByte = b.ReadBytes(info.EAvatar.ContentLength);
                    }
                    else
                    {
                        info.EImageByte = info.EImagejson == null ? null : Convert.FromBase64String(info.EImagejson);
                    }
                    message = familyData.AddeContacts(info, Guid.Parse(Session["UserID"].ToString()), PhoneNos, Session["AgencyID"].ToString());
                    if (message == "1")
                    {
                        ViewBag.message = "Emergency Contact added successfully.";

                    }
                    if (message == "2")
                    {
                        ViewBag.message = "Emergency Contact updated successfully.";

                    }
                }
                else if (Command == "SaveRestrictedData")
                {

                    if (info.RAvatar != null)
                    {
                        info.RFileName = info.RAvatar.FileName;
                        info.RFileExtension = Path.GetExtension(info.RAvatar.FileName);
                        BinaryReader b = new BinaryReader(info.RAvatar.InputStream);
                        info.RImageByte = b.ReadBytes(info.RAvatar.ContentLength);
                    }
                    else
                    {
                        info.RImageByte = info.RImagejson == null ? null : Convert.FromBase64String(info.RImagejson);
                    }
                    message = familyData.addRestricted(info, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                    if (message == "1")
                    {
                        ViewBag.message = "Restricted contact added successfully.";
                    }
                    if (message == "2")
                    {
                        ViewBag.message = "Restricted contact updated successfully.";
                    }

                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            ViewBag.lang = TempData["language"];
            ViewBag.Race = TempData["Race"];
            info.relationship = (List<FamilyHousehold.Relationship>)TempData["Relationship"];
            info.SchoolList = (List<SchoolDistrict>)TempData["SchoolList"];
            ViewBag.Relationship = TempData["Relationship"];
            TempData.Keep();
            return View(info);
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]


        public ActionResult AddNewChild(string id = "0")
        {
            FamilyHousehold FamilyObject = new FamilyHousehold();
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["OthersId"]))
                    FamilyObject.OthersId = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["OthersId"].ToString()));
                FamilyObject = familyData.GetData_AllDropdownChild(Session["AgencyID"].ToString(), Session["UserID"].ToString(), FamilyObject.OthersId, Session["Roleid"].ToString());
                FamilyObject.Encrypthouseholid = id;
                if (id != "0")
                    FamilyObject.HouseholdId = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(id));
                if (!string.IsNullOrEmpty(Request.QueryString["Name"]))
                    FamilyObject.Pfirstname = EncryptDecrypt.Decrypt64(Request.QueryString["Name"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["OthersId"]))
                    FamilyObject.OthersId = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["OthersId"].ToString()));

                if (!string.IsNullOrEmpty(Request.QueryString["Clientid"]))
                    FamilyObject.ChildId = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["Clientid"].ToString()));

                ViewBag.lang = FamilyObject.langList;
                TempData["familyinfo"] = ViewBag.lang;
                ViewBag.RaceSubCategory = FamilyObject.raceCategory;
                TempData["RaceSubcategory"] = ViewBag.RaceSubCategory;
                ViewBag.Race = FamilyObject.raceList;
                TempData["Race"] = ViewBag.Race;
                ViewBag.Relationship = FamilyObject.relationship;
                TempData["Relationship"] = ViewBag.Relationship;
                TempData["Programtype"] = FamilyObject.AvailableProgram;
                TempData["Schooldistrict"] = FamilyObject.SchoolList;
                TempData["ImmunizationRecords"] = FamilyObject.ImmunizationRecords;
                //Child Health History
                TempData["AvailableDisease"] = FamilyObject.AvailableDisease;
                TempData["AvailableDiagnosedDisease"] = FamilyObject.AvailableDiagnosedDisease;
                TempData["AvailableDental"] = FamilyObject.AvailableDental;
                TempData["AvailableEHS"] = FamilyObject.AvailableEHS;
                TempData["AvailableChildDrink"] = FamilyObject.AvailableChildDrink;
                TempData["AvailableChildDietFull"] = FamilyObject.AvailableChildDietFull;
                TempData["AvailableChildVitamin"] = FamilyObject.AvailableChildVitamin;
                TempData["AvailableService"] = FamilyObject.AvailableService;
                TempData["AvailablePrblms"] = FamilyObject.AvailablePrblms;
                ViewBag.diet = FamilyObject.dietList;
                TempData["DietInfo"] = ViewBag.diet;
                ViewBag.food = FamilyObject.foodList;
                TempData["foodInfo"] = ViewBag.food;
                ViewBag.ChildFeed = FamilyObject.CFeedList;
                TempData["ChildFeed"] = ViewBag.ChildFeed;
                ViewBag.hungry = FamilyObject.ChungryList;
                TempData["hungry"] = ViewBag.hungry;
                ViewBag.ChildFormula = FamilyObject.CFormulaList;
                TempData["ChildFormula"] = ViewBag.ChildFormula;
                ViewBag.ChildCereal = FamilyObject.CFeedCerealList;
                TempData["ChildCereal"] = ViewBag.ChildCereal;
                ViewBag.ChildReferal = FamilyObject.CReferalCriteriaList;
                TempData["ChildReferal"] = ViewBag.ChildReferal;
                TempData["CustomScreening"] = FamilyObject.customscreening;

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            TempData["TransportNeeded"] = FamilyObject.CTransportNeeded;
            return View(FamilyObject);

        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]


        [HttpPost]
        public ActionResult AddNewChild(FamilyHousehold info, FamilyHousehold.PostedProgram PostedPostedPrograms, List<FamilyHousehold.ImmunizationRecord> Imminization,
            Screening _screen, FamilyHousehold.PostedDisease PostedPostedDisease,
            FamilyHousehold.PostedDiagnosedDisease PostedPostedDiagnosedDisease, FamilyHousehold.PostedChildEHS PostedPostedMedicalEHS,
             FamilyHousehold.PostedChildEHS PostedPostedEHS, Nurse.PostedChildVitamin PostedPostedChildVitamin, Nurse.PostedChildDiet PostedPostedChildDietFull,
           Nurse.PostedChildDrink PostedPostedChildDrink, FormCollection collection)
        {
            try
            {

                StringBuilder _string = new StringBuilder();
                if (PostedPostedPrograms.ProgramID != null)
                {
                    foreach (string str in PostedPostedPrograms.ProgramID)
                    {
                        _string.Append(str + ",");
                    }
                    info.CProgramType = _string.ToString().Substring(0, _string.Length - 1);
                }
                StringBuilder _familychildinfo = new StringBuilder();

                if (info._childprogrefid == "1")
                {
                    _familychildinfo.Clear();
                    if (PostedPostedDisease.DiseaseID != null)
                    {
                        foreach (string str in PostedPostedDisease.DiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildDirectBloodRelativeEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildDiagnosedConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedMedicalEHS.ChildEHSID != null)
                    {
                        foreach (string str in PostedPostedMedicalEHS.ChildEHSID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildChronicHealthConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedEHS.ChildEHSID != null)
                    {
                        foreach (string str in PostedPostedEHS.ChildEHSID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildChronicHealthConditions1Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildMedicalTreatmentEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildChronicHealthConditions2Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                }
                if (info._childprogrefid == "2")
                {
                    _familychildinfo.Clear();
                    if (PostedPostedDisease.DiseaseID != null)
                    {
                        foreach (string str in PostedPostedDisease.DiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildDirectBloodRelativeHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildDiagnosedConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildMedicalTreatmentHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildChronicHealthConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }

                }

                _familychildinfo.Clear();
                if (PostedPostedChildVitamin.CDietInfoID != null)
                {
                    foreach (string str in PostedPostedChildVitamin.CDietInfoID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChildVitaminSupplement = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedChildDietFull.CDietInfoID != null)
                {
                    foreach (string str in PostedPostedChildDietFull.CDietInfoID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDiet = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedChildDrink.CDrinkID != null)
                {
                    foreach (string str in PostedPostedChildDrink.CDrinkID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDrink = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                #region Add Child
                if (info.CAvatar != null)
                {
                    info.CFileName = info.CAvatar.FileName;
                    info.CFileExtension = Path.GetExtension(info.CAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.CAvatar.InputStream);
                    info.CImageByte = b.ReadBytes(info.CAvatar.ContentLength);
                }
                else
                {
                    info.CImageByte = info.Imagejson == null ? null : Convert.FromBase64String(info.Imagejson);
                }
                if (info.FiledobRAvatar != null)
                {
                    info.DobFileName = info.FiledobRAvatar.FileName;
                    info.DobFileExtension = Path.GetExtension(info.FiledobRAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.FiledobRAvatar.InputStream);
                    info.Dobaddressform = b.ReadBytes(info.FiledobRAvatar.ContentLength);
                }
                if (info.FiledFosterAvatar != null)
                {
                    info.FosterFileName = info.FiledFosterAvatar.FileName;
                    info.FosterFileExtension = Path.GetExtension(info.FiledFosterAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.FiledFosterAvatar.InputStream);
                    info.Fosterfileinbytes = b.ReadBytes(info.FiledFosterAvatar.ContentLength);
                }
                if (info.FileImmunization != null)
                {
                    info.ImmunizationFileName = info.FileImmunization.FileName;
                    info.ImmunizationFileExtension = Path.GetExtension(info.FileImmunization.FileName);
                    BinaryReader b = new BinaryReader(info.FileImmunization.InputStream);
                    info.Immunizationfileinbytes = b.ReadBytes(info.FileImmunization.ContentLength);
                }
                #endregion


                //#region Screening
                //if (_screen.Physical != null)
                //{
                //    _screen.PhysicalFileName = _screen.Physical.FileName;
                //    _screen.PhysicalFileExtension = Path.GetExtension(_screen.Physical.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Physical.InputStream);
                //    _screen.PhysicalImageByte = b.ReadBytes(_screen.Physical.ContentLength);
                //}
                //else
                //{
                //    _screen.PhysicalImageByte = _screen.PhysicalImagejson == null ? null : Convert.FromBase64String(_screen.PhysicalImagejson);

                //}
                //if (_screen.Dental != null)
                //{
                //    _screen.DentalFileName = _screen.Dental.FileName;
                //    _screen.DentalFileExtension = Path.GetExtension(_screen.Dental.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Dental.InputStream);
                //    _screen.DentalImageByte = b.ReadBytes(_screen.Dental.ContentLength);
                //}
                //else
                //{
                //    _screen.DentalImageByte = _screen.DentalImagejson == null ? null : Convert.FromBase64String(_screen.DentalImagejson);

                //}
                //if (_screen.Vision != null)
                //{
                //    _screen.VisionFileName = _screen.Vision.FileName;
                //    _screen.VisionFileExtension = Path.GetExtension(_screen.Vision.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Vision.InputStream);
                //    _screen.VisionImageByte = b.ReadBytes(_screen.Vision.ContentLength);
                //}
                //else
                //{
                //    _screen.VisionImageByte = _screen.VisionImagejson == null ? null : Convert.FromBase64String(_screen.VisionImagejson);

                //}
                //if (_screen.Hearing != null)
                //{
                //    _screen.HearingFileName = _screen.Hearing.FileName;
                //    _screen.HearingFileExtension = Path.GetExtension(_screen.Hearing.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Hearing.InputStream);
                //    _screen.HearingImageByte = b.ReadBytes(_screen.Hearing.ContentLength);
                //}
                //else
                //{
                //    _screen.HearingImageByte = _screen.HearingImagejson == null ? null : Convert.FromBase64String(_screen.HearingImagejson);

                //}
                //if (_screen.Develop != null)
                //{
                //    _screen.DevelopFileName = _screen.Develop.FileName;
                //    _screen.DevelopFileExtension = Path.GetExtension(_screen.Develop.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Develop.InputStream);
                //    _screen.DevelopImageByte = b.ReadBytes(_screen.Develop.ContentLength);
                //}
                //else
                //{
                //    _screen.DevelopImageByte = _screen.DevelopImagejson == null ? null : Convert.FromBase64String(_screen.DevelopImagejson);

                //}
                //if (_screen.Speech != null)
                //{
                //    _screen.SpeechFileName = _screen.Speech.FileName;
                //    _screen.SpeechFileExtension = Path.GetExtension(_screen.Speech.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Speech.InputStream);
                //    _screen.SpeechImageByte = b.ReadBytes(_screen.Speech.ContentLength);
                //}
                //else
                //{
                //    _screen.SpeechImageByte = _screen.SpeechImagejson == null ? null : Convert.FromBase64String(_screen.SpeechImagejson);

                //}
                //if (_screen.ScreeningAccept != null)
                //{
                //    _screen.ScreeningAcceptFileName = _screen.ScreeningAccept.FileName;
                //    _screen.ScreeningAcceptFileExtension = Path.GetExtension(_screen.ScreeningAccept.FileName);
                //    BinaryReader b = new BinaryReader(_screen.ScreeningAccept.InputStream);
                //    _screen.ScreeningAcceptImageByte = b.ReadBytes(_screen.ScreeningAccept.ContentLength);
                //}
                //else
                //{
                //    _screen.ScreeningAcceptImageByte = _screen.ScreeningAcceptImageByte == null ? null : Convert.FromBase64String(_screen.ScreeningAcceptImagejson);

                //}


                //#endregion


                if (info.Releaseform != null)
                {
                    info.ReleaseformFileName = info.Releaseform.FileName;
                    info.ReleaseformFileExtension = Path.GetExtension(info.Releaseform.FileName);
                    BinaryReader b = new BinaryReader(info.Releaseform.InputStream);
                    info.Releaseformfileinbytes = b.ReadBytes(info.Releaseform.ContentLength);
                }
                // if(TempData["TransportNeeded"]  as  info.CTransportNeeded)
                new TransportationData().InsertTransportationyakkrForEnrolledChild(info.ChildId.ToString(), Session["AgencyID"].ToString(), Session["UserID"].ToString());
                string message = familyData.Addnewchild(ref info, 0, Guid.Parse(Session["UserID"].ToString()), Imminization, _screen, collection, Request.Files);
                if (message == "1")
                {
                    TempData["message"] = "Record added successfully.";
                    return Redirect("~/AgencyUser/FamilySummary/" + info.Encrypthouseholid);

                }
                if (message == "2")
                {
                    TempData["message"] = "Record already added.";
                    return Redirect("~/AgencyUser/FamilySummary/" + info.Encrypthouseholid);

                }
                if (message == "3")
                {
                    TempData["message"] = "Record updated successfully.";
                    return Redirect("~/AgencyUser/FamilySummary/" + info.Encrypthouseholid);

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            ViewBag.lang = TempData["familyinfo"];
            ViewBag.Race = TempData["Race"];
            ViewBag.RaceSubCategory = TempData["RaceSubcategory"];
            ViewBag.Relationship = TempData["Relationship"];
            info.AvailableProgram = (List<FamilyHousehold.Programdetail>)TempData["Programtype"];
            info.ImmunizationRecords = (List<FamilyHousehold.ImmunizationRecord>)TempData["ImmunizationRecords"];
            info.AvailableDisease = (List<FamilyHousehold.ChildDirectBloodRelative>)TempData["AvailableDisease"];
            info.AvailableDiagnosedDisease = (List<FamilyHousehold.ChildDiagnosedDisease>)TempData["AvailableDiagnosedDisease"];
            info.AvailableDental = (List<FamilyHousehold.ChildDental>)TempData["AvailableDental"];
            info.SchoolList = (List<SchoolDistrict>)TempData["Schooldistrict"];
            info.AvailableChildDrink = (List<FamilyHousehold.ChildDrink>)TempData["AvailableChildDrink"];
            info.AvailableChildDietFull = (List<Nurse.ChildDietFull>)TempData["AvailableChildDietFull"];
            info.AvailableChildVitamin = (List<Nurse.ChildVitamin>)TempData["AvailableChildVitamin"];
            info.AvailableService = (List<FamilyHousehold.PMService>)TempData["AvailableService"];
            info.AvailablePrblms = (List<FamilyHousehold.PMProblems>)TempData["AvailablePrblms"];
            info.AvailableEHS = (List<FamilyHousehold.ChildEHS>)TempData["AvailableEHS"];
            ViewBag.diet = TempData["DietInfo"];
            ViewBag.food = TempData["foodInfo"];
            ViewBag.hungry = TempData["hungry"];
            ViewBag.ChildFeed = TempData["ChildFeed"];
            ViewBag.hungry = TempData["hungry"];
            ViewBag.ChildFormula = TempData["ChildFormula"];
            ViewBag.ChildReferal = TempData["ChildReferal"];
            ViewBag.ChildCereal = TempData["ChildCereal"];
            info.customscreening = (DataTable)TempData["CustomScreening"];


            TempData.Keep();
            return View(info);
        }
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc,e4c80fc2-8b64-447a-99b4-95d1510b01e9")] //added homevisitor on jan 04 2018
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]


        public ActionResult AddNewParent(string id = "0")
        {
            FamilyHousehold FamilyObject = new FamilyHousehold();
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Parentid"]))
                    FamilyObject.ParentID = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["Parentid"].ToString()));
                FamilyObject = familyData.GetData_AllDropdown(Convert.ToString(EncryptDecrypt.Decrypt64(id)), FamilyObject.ParentID, Session["AgencyID"].ToString(), Session["UserID"].ToString(),Session["RoleID"].ToString());
                if (id != "0")
                    FamilyObject.HouseholdId = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(id));

                if (TempData["ExistPmprogram"] != null)
                    FamilyObject.ExistPmprogram = Convert.ToInt32(TempData["ExistPmprogram"]);
                if (FamilyObject.Income1 == null)
                    FamilyObject.Income1 = GenerateIncomeList();
                FamilyObject.Encrypthouseholid = id;
                ViewBag.PMConditions = FamilyObject.PMCondtnList;
                TempData["PMConditions"] = ViewBag.PMConditions;
                TempData["AvailableService"] = FamilyObject.AvailableService;
                TempData["AvailablePrblms"] = FamilyObject.AvailablePrblms;
                TempData.Keep();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(FamilyObject);

        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        [HttpPost]
        public ActionResult AddNewParent(FamilyHousehold info, FormCollection collection, List<FingerprintsModel.FamilyHousehold.Parentphone1> ParentPhone1,
            FamilyHousehold.PostedPMService PostedPostedService, FamilyHousehold.PostedPMProblems PostedPostedPrblms, List<FamilyHousehold.calculateincome> Income1)
        {
            try
            {
                StringBuilder _familychildinfo = new StringBuilder();
                if (PostedPostedService.PMServiceID != null)
                {
                    foreach (string str in PostedPostedService.PMServiceID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._Pregnantmotherpmservices = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedPrblms.PMPrblmID != null)
                {
                    foreach (string str in PostedPostedPrblms.PMPrblmID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._Pregnantmotherproblem = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                if (collection["DdlPRole"] != null)
                    info.PRole = (collection["DdlPRole"].ToString() == "") ? null : collection["DdlPRole"];
                if (collection["DdlPDegreeEarned"] != null)
                    info.PDegreeEarned = (collection["DdlPDegreeEarned"].ToString() == "") ? null : collection["DdlPDegreeEarned"];
                if (info.PAvatar != null)
                {
                    info.PFileName = info.PAvatar.FileName;
                    info.PFileExtension = Path.GetExtension(info.PAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.PAvatar.InputStream);
                    info.PImageByte = b.ReadBytes(info.PAvatar.ContentLength);
                }
                else
                {
                    info.PImageByte = info.PImagejson == null ? null : Convert.FromBase64String(info.PImagejson);

                }
                if (info.Income1 != null)
                {
                    foreach (FamilyHousehold.calculateincome ParentIncome1 in Income1)
                    {
                        if (ParentIncome1.SalaryAvatar1 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
                            ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
                            ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar2 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
                            ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
                            ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar3 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
                            ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
                            ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar4 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
                            ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
                            ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
                        }
                        if (ParentIncome1.NoIncomeAvatar != null)
                        {
                            ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
                            ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
                            ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
                        }
                    }
                }
                string message = message = familyData.AddParent(ref info, 0, Guid.Parse(Session["UserID"].ToString()), ParentPhone1, Income1);
                //string UpdateParameter = "UPDATE";
                //int Mode = 4;
               // familyData.CheckByClient(UpdateParameter, Mode);

                if (message == "1")
                {
                    TempData["message"] = "Parent/Guardian added successfully.";
                    return Redirect("~/AgencyUser/FamilySummary/" + info.Encrypthouseholid);
                }
                if (message == "2")
                {
                    TempData["message"] = "Parent/Guardian updated successfully.";
                    return Redirect("~/AgencyUser/FamilySummary/" + info.Encrypthouseholid);
                }
                if (message == "3")
                {
                    TempData["message"] = "Parent/Guardian2 already exist.";
                    return Redirect("~/AgencyUser/FamilySummary/" + info.Encrypthouseholid);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            if (info.Income1 == null)
                info.Income1 = GenerateIncomeList();
            info.AvailableService = (List<FamilyHousehold.PMService>)TempData["AvailableService"];
            ViewBag.PMConditions = TempData["PMConditions"];
            info.AvailablePrblms = (List<FamilyHousehold.PMProblems>)TempData["AvailablePrblms"];
            TempData.Keep();
            return View(info);
        }
       // [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]
        [HttpPost]
        [ValidateInput(false)]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult DropClient(string Transition)
        {
            try
            {

                Transition transition = new Transition();


                List<RosterNew.Attachment> attach = new List<RosterNew.Attachment>();
                var ate = Request.Files;
                var ate2 = ate.AllKeys;

                for (int i = 0; i < ate2.Length; i++)
                {
                    RosterNew.Attachment aatt = new RosterNew.Attachment();
                    aatt.file = ate[i];
                    attach.Add(aatt);
                }


                JavaScriptSerializer serializer = new JavaScriptSerializer();

                transition = serializer.Deserialize<Transition>(Transition);
                transition.CaseNoteDetails.CaseNoteAttachmentList = attach;
                transition.ParentID = (transition.ParentID == "0" || transition.ParentID == "") ? "0" : EncryptDecrypt.Decrypt64(transition.ParentID);
                transition.ParentID2 = (transition.ParentID2 == "0" || transition.ParentID2 == "") ? "0" : EncryptDecrypt.Decrypt64(transition.ParentID2);
                var id = familyData.DropClient(transition);
                return Json(id, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc,e4c80fc2-8b64-447a-99b4-95d1510b01e9")] //added  e4c80fc2-8b64-447a-99b4-95d1510b01e9 on 01/02/2018
        [CustAuthFilter()]
        public JsonResult GetEnrollReason(string Status = "0", string clientId = "0")
        {
            try
            {


                return Json(new FamilyData().GetEnrollReason(Status, clientId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GeneratehouseholdeditRequest(string HouseHoldId = "0")
        {
            try
            {
                return Json(familyData.GeneratehouseholdeditRequest(HouseHoldId, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        public ActionResult BMIStatus()
        {
            return View();

        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult CalculateBmi(string Gender, string Input, string Dob, string AssessmentDate, string Height = "0",
            string Weight = "0", string Headcir = "0")
        {


            try
            {
                return Json(familyData.CalculateBmi(Gender, Input, Dob, AssessmentDate, Height, Weight, Headcir, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Deletefamily(string HouseHoldId = "0")
        {
            try
            {
                string message = familyData.Deletefamily(HouseHoldId, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                if (message == "1")
                {
                    TempData["message"] = "Family removed successfully.";
                }
                return Json(message);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        //Added by Akansha on 15Dec2016
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult FillCommunity(string Community = "0")//string RestrictedId = "0",
        {
            CommunityResourceData obj = new CommunityResourceData();
            try
            {
                return Json(obj.GetCommunityInfo(Community, Session["AgencyID"].ToString()));//RestrictedId,
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        //Changes on 28Dec2016 //update by atul 27-3-2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult AddWorkshopAjax(string Center, string value, string daypreference, string timepreference, string HouseholdId, string WorkshopId = "0")// , string Date,FamilyHousehold.PostedWorkshop PostedPostedWorkshop)
        {
            FamilyData obj = new FamilyData();
            FamilyHousehold info = new FamilyHousehold();
            StringBuilder _familyworkshopinfo = new StringBuilder();


            info.WorkshopInfo = value;
            try
            {
                string result = obj.AddWorkshopInfo(Center, info, HouseholdId, WorkshopId, daypreference, timepreference, Session["UserID"].ToString(), Session["AgencyID"].ToString());//Date, "0", Community, DentalCenter, DentalNotes, MedicalNotes, MedicalCenter, CompanyNameden);
                //string result = obj.AddWorkshopInfo(Center, info, HouseholdId, WorkshopId, Session["UserID"].ToString(), Session["AgencyID"].ToString());//Date, "0", Community, DentalCenter, DentalNotes, MedicalNotes, MedicalCenter, CompanyNameden);
                return Json(new { result });
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }
        //Added on 27Dec2016
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [JsonMaxLengthAttribute]
        public JsonResult GetWorkshopList(string HouseholdId = "0")
        {
            try
            {
                FamilyData obj = new FamilyData();
                return Json(obj.getWorkshopInfo(HouseholdId, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }

        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult GroupCaseNotes()
        {
            try
            {
                FamilyData obj = new FamilyData();
                List<SelectListItem> Centerlist = new List<SelectListItem>();
                List<FingerprintsModel.RosterNew.User> _userlist = new List<FingerprintsModel.RosterNew.User>();
                DataSet _dataset = obj.GetCenterCaseNote(Session["AgencyID"].ToString(), Session["UserID"].ToString());
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    SelectListItem info = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        info = new SelectListItem();
                        info.Value = dr["center"].ToString();
                        info.Text = dr["centername"].ToString();
                        Centerlist.Add(info);
                    }
                }
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    FingerprintsModel.RosterNew.User obj1 = null;
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        obj1 = new FingerprintsModel.RosterNew.User();
                        obj1.Id = (dr["UserId"]).ToString();
                        obj1.Name = dr["Name"].ToString();
                        _userlist.Add(obj1);
                    }
                }
                TempData["GroupCaseNotes"] = Centerlist;
                TempData["UserList"] = _userlist;

                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }

        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult GroupCaseNotes(RosterNew.CaseNote CaseNote, RosterNew.ClientUsers ClientIds, RosterNew.ClientUsers TeamIds, List<RosterNew.Attachment> Attachments)
        {
            try
            {

                StringBuilder _Ids = new StringBuilder();
                if (ClientIds.IDS != null)
                {
                    foreach (string str in ClientIds.IDS)
                    {
                        _Ids.Append(EncryptDecrypt.Decrypt64(str) + ",");
                    }
                    CaseNote.ClientIds = _Ids.ToString().Substring(0, _Ids.Length - 1);
                }
                _Ids.Clear();
                if (TeamIds.IDS != null)
                {
                    foreach (string str in TeamIds.IDS)
                    {
                        _Ids.Append(str + ",");
                    }
                    CaseNote.StaffIds = _Ids.ToString().Substring(0, _Ids.Length - 1);
                }
                CaseNote.CaseNotetags = CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Length - 1);
                string message = familyData.SaveGroupCaseNotes(CaseNote, Attachments, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Session["Roleid"].ToString());
                if (message == "1")
                {
                    TempData["message"] = "Record saved successfully.";
                    return Redirect("~/AgencyUser/GroupCaseNotes");
                }
                else
                    TempData["message"] = "Error occurred please try again later.";
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View();
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult ViewGroupCaseNotes()
        {
            try
            {
                FamilyData obj = new FamilyData();
                List<SelectListItem> Centerlist = new List<SelectListItem>();
                List<FingerprintsModel.RosterNew.User> _userlist = new List<FingerprintsModel.RosterNew.User>();
                DataSet _dataset = obj.GetCenterCaseNote(Session["AgencyID"].ToString(), Session["UserID"].ToString());
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    SelectListItem info = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        info = new SelectListItem();
                        info.Value = dr["center"].ToString();
                        info.Text = dr["centername"].ToString();
                        Centerlist.Add(info);
                    }
                }
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    FingerprintsModel.RosterNew.User obj1 = null;
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        obj1 = new FingerprintsModel.RosterNew.User();
                        obj1.Id = (dr["UserId"]).ToString();
                        obj1.Name = dr["Name"].ToString();
                        _userlist.Add(obj1);
                    }
                }
                TempData["GroupCaseNotes"] = Centerlist;
                TempData["UserList"] = _userlist;



                return View();

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ViewGroupCaseNotes(RosterNew.CaseNote CaseNote, RosterNew.ClientUsers ClientIds, RosterNew.ClientUsers TeamIds, List<RosterNew.Attachment> Attachments)
        {
            try
            {

                StringBuilder _Ids = new StringBuilder();
                if (ClientIds.IDS != null)
                {
                    foreach (string str in ClientIds.IDS)
                    {
                        _Ids.Append(str + ",");
                    }
                    CaseNote.ClientIds = _Ids.ToString().Substring(0, _Ids.Length - 1);
                }
                _Ids.Clear();
                if (TeamIds.IDS != null)
                {
                    foreach (string str in TeamIds.IDS)
                    {
                        _Ids.Append(str + ",");
                    }
                    CaseNote.StaffIds = _Ids.ToString().Substring(0, _Ids.Length - 1);
                }
                CaseNote.CaseNotetags = CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Length - 1);
                string message = familyData.SaveGroupCaseNotes(CaseNote, Attachments, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Session["Roleid"].ToString());
                if (message == "1")
                {
                    TempData["message"] = "Record saved successfully.";
                    return Redirect("~/AgencyUser/ViewGroupCaseNotes");
                }
                else
                    TempData["message"] = "Error occurred please try again later.";
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View();
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult HomeVisit()
        {
            return View();
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult HomeVisitFSW()
        {
            // var scheduler = new DHXScheduler(this);
            return View();
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult HomeVisitTest()
        {
            return View();
        }
        //Changes on 8Feb2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult HomeVisitFSWNew(Scheduler info)
        {

            if (info.MeetingId == 0)
            {
                ViewData["Title"] = "Add Appointment";
                ViewBag.mode = 0;
            }
            else
            {
                ViewData["Title"] = "Edit Appointment";
                ViewBag.mode = 0;
            }

            info.StaffRoleId = new Guid(Session["RoleID"].ToString());
            info.Enc_ClientId = Request.QueryString["id"].ToString();

            return View(info);
        }


        //Changes on 27 jan2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult AddSchedulerInfo(string Description, string Date, string StartTime, string Duration, string EndTime, string Day, string ClientId, string Notes, bool isyakkr600601, string MeetingId = "0")// , string Date,FamilyHousehold.PostedWorkshop PostedPostedWorkshop)
        {
            FamilyData obj = new FamilyData();
            DateTime seldate =DateTime.ParseExact(Date, @"MM/dd/yyyy",
              System.Globalization.CultureInfo.InvariantCulture);

         

            if (seldate.Date.ToShortDateString()==DateTime.Now.ToShortDateString() && (Session["Appointment"]) != null)
            {

                Session["Appointment"] = ((int)Session["Appointment"]) + 1;
            }
            //  FamilyHousehold info = new FamilyHousehold();
            if (MeetingId == "0")
            {
                ViewBag.mode = 0;
            }
            else
            {
                ViewBag.mode = 1;
            }
            try
            {
                string result = obj.AddSchedulerDetails(Description, Date, StartTime, Duration, EndTime, Day, EncryptDecrypt.Decrypt64(ClientId), Notes, MeetingId, Session["UserID"].ToString(), Session["AgencyID"].ToString(),isyakkr600601);//Date, "0", Community, DentalCenter, DentalNotes, MedicalNotes, MedicalCenter, CompanyNameden);
                return Json(new { result });
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult listSchedule(string sortOrder, string sortDirection, string ClientId, string Day = "0", int pageSize = 0, int requestedPage = 1)
        {
            try
            {
                int todayAppointmentCount = 0;
                FamilyData info = new FamilyData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                sortOrder = (sortOrder == "") ? "THCN" : sortOrder;
                sortDirection = (sortDirection == "") ? "DESC" : sortDirection;
                var list = info.ScheduleInfo(out totalrecord,out todayAppointmentCount, sortOrder, sortDirection, EncryptDecrypt.Decrypt64(ClientId), Day, skip, pageSize, Session["AgencyID"].ToString(),Session["UserId"].ToString());
                return Json(new { list, totalrecord,todayAppointmentCount });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Deleteschedule(string ID = "0")
        {
            FamilyData obj = new FamilyData();
            try
            {
                return Json(obj.Deletescheduleinfo(ID, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult getSchedule(string Id = "0")
        {
            FamilyData obj = new FamilyData();
            Scheduler info = new Scheduler();

            string userId = Session["UserId"].ToString();

            if (Id != "0")
            {
                info.MeetingId = Convert.ToInt32(Id);
                ViewData["Title"] = "Edit Appointment";

                ViewBag.mode = 1;
            }
            else
            {
                ViewData["Title"] = "Add Appointment";
            }
            try
            {
                return Json(obj.getscheduleinfo(Id, userId, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        //Changes on 18jan2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult FSWAppointment()
        {
            return View();
        }
        //Changes on 23Dec2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult listAppointment(string sortOrder, string sortDirection, string search, int pageSize = 0, int requestedPage = 1)
        {
            try
            {
                FamilyData info = new FamilyData();
                string totalrecord;
                string timeZoneDiff;
                int skip = pageSize * (requestedPage - 1);
                var list = info.AppointmentInfo(out totalrecord,out timeZoneDiff, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString(), Session["UserId"].ToString());
                Session["Appointment"] = list.Count();

                return Json(new { list, totalrecord , timeZoneDiff });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }


        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        //[HttpPost]
        //public JsonResult SaveIncome(FamilyHousehold info, List<FamilyHousehold.calculateincome> Income1)
        //{
        //    try
        //    {
        //        #region Parent income document
        //        if (info.Income1 != null)
        //        {
        //            foreach (FamilyHousehold.calculateincome ParentIncome1 in Income1)
        //            {
        //                if (ParentIncome1.SalaryAvatar1 != null)
        //                {
        //                    ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
        //                    ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
        //                    BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
        //                    ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
        //                }
        //                if (ParentIncome1.SalaryAvatar2 != null)
        //                {
        //                    ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
        //                    ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
        //                    BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
        //                    ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
        //                }
        //                if (ParentIncome1.SalaryAvatar3 != null)
        //                {
        //                    ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
        //                    ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
        //                    BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
        //                    ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
        //                }
        //                if (ParentIncome1.SalaryAvatar4 != null)
        //                {
        //                    ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
        //                    ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
        //                    BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
        //                    ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
        //                }
        //                if (ParentIncome1.NoIncomeAvatar != null)
        //                {
        //                    ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
        //                    ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
        //                    BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
        //                    ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
        //                }

        //            }



        //        }
        //        //if (info.Income2 != null)
        //        //{
        //        //    foreach (FamilyHousehold.calculateincome1 ParentIncome1 in Income2)
        //        //    {
        //        //        if (ParentIncome1.SalaryAvatar1 != null)
        //        //        {
        //        //            ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
        //        //            ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
        //        //            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
        //        //            ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
        //        //        }
        //        //        if (ParentIncome1.SalaryAvatar2 != null)
        //        //        {
        //        //            ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
        //        //            ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
        //        //            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
        //        //            ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
        //        //        }
        //        //        if (ParentIncome1.SalaryAvatar3 != null)
        //        //        {
        //        //            ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
        //        //            ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
        //        //            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
        //        //            ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
        //        //        }
        //        //        if (ParentIncome1.SalaryAvatar4 != null)
        //        //        {
        //        //            ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
        //        //            ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
        //        //            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
        //        //            ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
        //        //        }
        //        //        if (ParentIncome1.NoIncomeAvatar != null)
        //        //        {
        //        //            ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
        //        //            ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
        //        //            BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
        //        //            ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
        //        //        }
        //        //    }
        //        //}
        //        string message = string.Empty;
        //        message = familyData.SaveParentIncome1(ref info, Income1, Session["UserID"].ToString(), Session["Roleid"].ToString(), Session["AgencyID"].ToString());



        //        #endregion
        //            return Json("1");
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //        return Json(Ex.Message);
        //    }
        //    // return View();
        //}

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [HttpPost]
        [JsonMaxLengthAttribute]
        public JsonResult SaveIncomeall(FamilyHousehold info, List<FamilyHousehold.calculateincome> Income1,
        List<FamilyHousehold.calculateincome1> Income2, string Command, FormCollection collection, List<FingerprintsModel.FamilyHousehold.phone> PhoneNos,
        List<FingerprintsModel.FamilyHousehold.Parentphone1> ParentPhone1, List<FingerprintsModel.FamilyHousehold.Parentphone2> ParentPhoneNos1,
        FamilyHousehold.PostedProgram PostedPostedPrograms, List<FamilyHousehold.ImmunizationRecord> Imminization, Screening _screen,
             FamilyHousehold.PostedPMService PostedPostedService, FamilyHousehold.PostedPMProblems PostedPostedPrblms, FamilyHousehold.PostedDisease PostedPostedDisease,
            FamilyHousehold.PostedDiagnosedDisease PostedPostedDiagnosedDisease, FamilyHousehold.PostedChildEHS PostedPostedMedicalEHS,
             FamilyHousehold.PostedChildEHS PostedPostedEHS, Nurse.PostedChildVitamin PostedPostedChildVitamin, Nurse.PostedChildDiet PostedPostedChildDietFull,
           Nurse.PostedChildDrink PostedPostedChildDrink)
        {


            StringBuilder _string = new StringBuilder();
            if (PostedPostedPrograms.ProgramID != null)
            {
                foreach (string str in PostedPostedPrograms.ProgramID)
                {
                    _string.Append(str + ",");
                }
                info.CProgramType = _string.ToString().Substring(0, _string.Length - 1);
            }
            StringBuilder _familychildinfo = new StringBuilder();
            if (PostedPostedService.PMServiceID != null)
            {
                foreach (string str in PostedPostedService.PMServiceID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherpmservices = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedService.PMServiceID1 != null)
            {
                foreach (string str in PostedPostedService.PMServiceID1)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherpmservices1 = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedPrblms.PMPrblmID != null)
            {
                foreach (string str in PostedPostedPrblms.PMPrblmID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherproblem = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedPrblms.PMPrblmID1 != null)
            {
                foreach (string str in PostedPostedPrblms.PMPrblmID1)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherproblem1 = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            if (info._childprogrefid == "1")
            {
                _familychildinfo.Clear();
                if (PostedPostedDisease.DiseaseID != null)
                {
                    foreach (string str in PostedPostedDisease.DiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDirectBloodRelativeEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDiagnosedConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedMedicalEHS.ChildEHSID != null)
                {
                    foreach (string str in PostedPostedMedicalEHS.ChildEHSID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedEHS.ChildEHSID != null)
                {
                    foreach (string str in PostedPostedEHS.ChildEHSID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditions1Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildMedicalTreatmentEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditions2Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
            }
            if (info._childprogrefid == "2")
            {
                _familychildinfo.Clear();
                if (PostedPostedDisease.DiseaseID != null)
                {
                    foreach (string str in PostedPostedDisease.DiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDirectBloodRelativeHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDiagnosedConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildMedicalTreatmentHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
            }
            _familychildinfo.Clear();
            if (PostedPostedChildVitamin.CDietInfoID != null)
            {
                foreach (string str in PostedPostedChildVitamin.CDietInfoID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildChildVitaminSupplement = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedChildDietFull.CDietInfoID != null)
            {
                foreach (string str in PostedPostedChildDietFull.CDietInfoID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildDiet = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedChildDrink.CDrinkID != null)
            {
                foreach (string str in PostedPostedChildDrink.CDrinkID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildDrink = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }

            FamilyHousehold _familyinfo = new FamilyHousehold();
            _familyinfo = info;
            try
            {


                if (info.FileaddressAvatar != null)
                {
                    info.HFileName = info.FileaddressAvatar.FileName;
                    info.HFileExtension = Path.GetExtension(info.FileaddressAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.FileaddressAvatar.InputStream);
                    info.HImageByte = b.ReadBytes(info.FileaddressAvatar.ContentLength);
                }
                else
                {
                    info.HImageByte = info.HFileInString == null ? null : Convert.FromBase64String(info.HFileInString);
                }

                if (collection["DdlPRole"] != null)
                    info.PRole = (collection["DdlPRole"].ToString() == "") ? null : collection["DdlPRole"];
                if (collection["DdlPDegreeEarned"] != null)
                    info.PDegreeEarned = (collection["DdlPDegreeEarned"].ToString() == "") ? null : collection["DdlPDegreeEarned"];
                if (info.PAvatar != null)
                {
                    info.PFileName = info.PAvatar.FileName;
                    info.PFileExtension = Path.GetExtension(info.PAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.PAvatar.InputStream);
                    info.PImageByte = b.ReadBytes(info.PAvatar.ContentLength);
                }
                else
                {
                    info.PImageByte = info.PImagejson == null ? null : Convert.FromBase64String(info.PImagejson);

                }
                if ((!string.IsNullOrEmpty(info.P1firstname)) && (!string.IsNullOrEmpty(info.Pfirstname)))
                {
                    info.Parentsecondexist = 1;
                    if (collection["DdlP1Role"] != null)
                        info.P1Role = (collection["DdlP1Role"].ToString() == "") ? null : collection["DdlP1Role"];
                    if (collection["DdlP1DegreeEarned"] != null)
                        info.P1DegreeEarned = (collection["DdlP1DegreeEarned"].ToString() == "-1") ? null : collection["DdlP1DegreeEarned"];
                    if (info.P1Avatar != null)
                    {
                        info.P1FileName = info.P1Avatar.FileName;
                        info.P1FileExtension = Path.GetExtension(info.P1Avatar.FileName);
                        BinaryReader b = new BinaryReader(info.P1Avatar.InputStream);
                        info.P1ImageByte = b.ReadBytes(info.P1Avatar.ContentLength);

                    }
                    else
                    {
                        info.P1ImageByte = info.P1Imagejson == null ? null : Convert.FromBase64String(info.P1Imagejson);

                    }
                }
                else
                {
                    info.Parentsecondexist = 0;
                }

                if (info.CAvatar != null)
                {
                    info.CFileName = info.CAvatar.FileName;
                    info.CFileExtension = Path.GetExtension(info.CAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.CAvatar.InputStream);
                    info.CImageByte = b.ReadBytes(info.CAvatar.ContentLength);
                }
                else
                {
                    info.CImageByte = info.Imagejson == null ? null : Convert.FromBase64String(info.Imagejson);
                }
                if (info.FiledobRAvatar != null)
                {
                    info.DobFileName = info.FiledobRAvatar.FileName;
                    info.DobFileExtension = Path.GetExtension(info.FiledobRAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.FiledobRAvatar.InputStream);
                    info.Dobaddressform = b.ReadBytes(info.FiledobRAvatar.ContentLength);
                }
                if (info.FileImmunization != null)
                {
                    info.ImmunizationFileName = info.FileImmunization.FileName;
                    info.ImmunizationFileExtension = Path.GetExtension(info.FileImmunization.FileName);
                    BinaryReader b = new BinaryReader(info.FileImmunization.InputStream);
                    info.Immunizationfileinbytes = b.ReadBytes(info.FileImmunization.ContentLength);
                }

                if (info.Income1 != null)
                {
                    foreach (FamilyHousehold.calculateincome ParentIncome1 in Income1)
                    {
                        if (ParentIncome1.SalaryAvatar1 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
                            ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
                            ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar2 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
                            ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
                            ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar3 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
                            ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
                            ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar4 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
                            ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
                            ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
                        }
                        if (ParentIncome1.NoIncomeAvatar != null)
                        {
                            ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
                            ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
                            ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
                        }

                    }



                }
                if (info.Income2 != null)
                {
                    foreach (FamilyHousehold.calculateincome1 ParentIncome1 in Income2)
                    {
                        if (ParentIncome1.SalaryAvatar1 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
                            ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
                            ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar2 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
                            ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
                            ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar3 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
                            ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
                            ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar4 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
                            ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
                            ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
                        }
                        if (ParentIncome1.NoIncomeAvatar != null)
                        {
                            ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
                            ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
                            ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
                        }
                    }
                }

                if (_screen.Physical != null)
                {
                    _screen.PhysicalFileName = _screen.Physical.FileName;
                    _screen.PhysicalFileExtension = Path.GetExtension(_screen.Physical.FileName);
                    BinaryReader b = new BinaryReader(_screen.Physical.InputStream);
                    _screen.PhysicalImageByte = b.ReadBytes(_screen.Physical.ContentLength);
                }
                else
                {
                    _screen.PhysicalImageByte = _screen.PhysicalImagejson == null ? null : Convert.FromBase64String(_screen.PhysicalImagejson);

                }
                if (_screen.Dental != null)
                {
                    _screen.DentalFileName = _screen.Dental.FileName;
                    _screen.DentalFileExtension = Path.GetExtension(_screen.Dental.FileName);
                    BinaryReader b = new BinaryReader(_screen.Dental.InputStream);
                    _screen.DentalImageByte = b.ReadBytes(_screen.Dental.ContentLength);
                }
                else
                {
                    _screen.DentalImageByte = _screen.DentalImagejson == null ? null : Convert.FromBase64String(_screen.DentalImagejson);

                }
                if (_screen.Vision != null)
                {
                    _screen.VisionFileName = _screen.Vision.FileName;
                    _screen.VisionFileExtension = Path.GetExtension(_screen.Vision.FileName);
                    BinaryReader b = new BinaryReader(_screen.Vision.InputStream);
                    _screen.VisionImageByte = b.ReadBytes(_screen.Vision.ContentLength);
                }
                else
                {
                    _screen.VisionImageByte = _screen.VisionImagejson == null ? null : Convert.FromBase64String(_screen.VisionImagejson);

                }
                if (_screen.Hearing != null)
                {
                    _screen.HearingFileName = _screen.Hearing.FileName;
                    _screen.HearingFileExtension = Path.GetExtension(_screen.Hearing.FileName);
                    BinaryReader b = new BinaryReader(_screen.Hearing.InputStream);
                    _screen.HearingImageByte = b.ReadBytes(_screen.Hearing.ContentLength);
                }
                else
                {
                    _screen.HearingImageByte = _screen.HearingImagejson == null ? null : Convert.FromBase64String(_screen.HearingImagejson);

                }
                if (_screen.Develop != null)
                {
                    _screen.DevelopFileName = _screen.Develop.FileName;
                    _screen.DevelopFileExtension = Path.GetExtension(_screen.Develop.FileName);
                    BinaryReader b = new BinaryReader(_screen.Develop.InputStream);
                    _screen.DevelopImageByte = b.ReadBytes(_screen.Develop.ContentLength);
                }
                else
                {
                    _screen.DevelopImageByte = _screen.DevelopImagejson == null ? null : Convert.FromBase64String(_screen.DevelopImagejson);

                }
                if (_screen.Speech != null)
                {
                    _screen.SpeechFileName = _screen.Speech.FileName;
                    _screen.SpeechFileExtension = Path.GetExtension(_screen.Speech.FileName);
                    BinaryReader b = new BinaryReader(_screen.Speech.InputStream);
                    _screen.SpeechImageByte = b.ReadBytes(_screen.Speech.ContentLength);
                }
                else
                {
                    _screen.SpeechImageByte = _screen.SpeechImagejson == null ? null : Convert.FromBase64String(_screen.SpeechImagejson);

                }
                if (_screen.ScreeningAccept != null)
                {
                    _screen.ScreeningAcceptFileName = _screen.ScreeningAccept.FileName;
                    _screen.ScreeningAcceptFileExtension = Path.GetExtension(_screen.ScreeningAccept.FileName);
                    BinaryReader b = new BinaryReader(_screen.ScreeningAccept.InputStream);
                    _screen.ScreeningAcceptImageByte = b.ReadBytes(_screen.ScreeningAccept.ContentLength);
                }
                else
                {
                    _screen.ScreeningAcceptImageByte = _screen.ScreeningAcceptImageByte == null ? null : Convert.FromBase64String(_screen.ScreeningAcceptImagejson);

                }
                if (info.Releaseform != null)
                {
                    info.ReleaseformFileName = info.Releaseform.FileName;
                    info.ReleaseformFileExtension = Path.GetExtension(info.Releaseform.FileName);
                    BinaryReader b = new BinaryReader(info.Releaseform.InputStream);
                    info.Releaseformfileinbytes = b.ReadBytes(info.Releaseform.ContentLength);
                }
                string message = string.Empty;
                if (info.HouseholdId == 0)
                    message = familyData.addParentInfo(ref info, 0, Guid.Parse(Session["UserID"].ToString()), ParentPhone1, ParentPhoneNos1, Income1, Income2, Imminization, PhoneNos, _screen, Session["Roleid"].ToString(), collection, Request.Files,true);
                else
                    message = familyData.addParentInfo(ref info, 1, Guid.Parse(Session["UserID"].ToString()), ParentPhone1, ParentPhoneNos1, Income1, Income2, Imminization, PhoneNos, _screen, Session["Roleid"].ToString(), collection, Request.Files, true);

                var list = "";
                if (info.Income1 != null && info.Income1.Count > 0)
                    list = RenderRazorViewToString("~/Views/Partialviews/ParentIncome1.cshtml", info.Income1);
                var list1 = "";
                if (info.Income2 != null && info.Income2.Count > 0)
                    list1 = RenderRazorViewToString("~/Views/Partialviews/ParentIncom2.cshtml", info.Income2);
                return Json(new { message, list, list1 });

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json("");
        }
        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        //Changes on 1Feb2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult AddOthersHouseholdAjax(FormCollection obj, HttpPostedFileBase HouseHoldAvatar)
        {
            FamilyHousehold _familyinfo = new FamilyHousehold();

            _familyinfo.Ofirstname = obj["Fname"].ToString();
            _familyinfo.Omiddlename = obj["Mname"].ToString();
            _familyinfo.Olastname = obj["Lname"].ToString();
            _familyinfo.OGender = obj["OGender"].ToString();
            _familyinfo.ODOB = obj["ODOB"].ToString();
            _familyinfo.CSSN = obj["SSN"].ToString();
            _familyinfo.OthersId = Convert.ToInt32(obj["OthersId"].ToString());
            _familyinfo.HouseholdId = Convert.ToInt32(obj["HouseholdId"].ToString());
            _familyinfo.IsEmergency = Convert.ToBoolean( obj["IsEmer"].ToString());


            if (HouseHoldAvatar != null && HouseHoldAvatar.ContentLength > 0)
            {
                _familyinfo.HouseHoldFileName = HouseHoldAvatar.FileName;
                _familyinfo.HouseHoldFileExtension = Path.GetExtension(HouseHoldAvatar.FileName);
                BinaryReader b = new BinaryReader(HouseHoldAvatar.InputStream);
                _familyinfo.HouseHoldImageByte = b.ReadBytes(HouseHoldAvatar.ContentLength);
            }
            else
            {
                _familyinfo.HouseHoldImageByte = _familyinfo.HouseHoldImagejson == null ? null : Convert.FromBase64String(_familyinfo.HouseHoldImagejson);
            }
            try
            {
                string result = familyData.AddOthersHouseholdInfo(_familyinfo,  Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                if (_familyinfo.IsEmergency == true && (result == "1"))
                {

                    ViewBag.message = "Record added successfully. Please complete the emergency contact information. ";
                    _familyinfo.EmegencyId = _familyinfo.EmegencyId;
                 
                }
                if (result == "2" && _familyinfo.Oemergencycontact && _familyinfo.Alreadyemergencycontact == 0 && _familyinfo.EmegencyId != 0)
                {

                    ViewBag.message = "Record updated successfully. Please complete the emergency contact information.";
                    _familyinfo.EmegencyId = _familyinfo.EmegencyId;
              
                }

                return Json(new { result, _familyinfo.EmegencyId });
             
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }
        ////Changes on 2Feb2017
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26")]
        //public JsonResult AddEmergencyContactAjax(string Fname, string Mname, string Lname, string Gender, string DOB, string Email, string Relationwithchild, string Enotes, string EAvatar, string HouseholdId, string EmegencyId, List<FingerprintsModel.FamilyHousehold.phone> PhoneNos,FormCollection frm,HttpPostedFileBase file)
        //{

        //    FamilyHousehold _familyinfo = new FamilyHousehold();


        //    //if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
        //    //{
        //    //    _familyinfo.EAvatar = System.Web.HttpContext.Current.Request.Files["EAvatar"];
        //    //}

        //   // _familyinfo.EAvatar = EAvatar;
        //    if (_familyinfo.EAvatar != null)
        //    {
        //        _familyinfo.EFileName = _familyinfo.EAvatar.FileName;
        //        _familyinfo.EFileExtension = Path.GetExtension(_familyinfo.EAvatar.FileName);
        //        BinaryReader b = new BinaryReader(_familyinfo.EAvatar.InputStream);
        //        _familyinfo.EImageByte = b.ReadBytes(_familyinfo.EAvatar.ContentLength);
        //    }
        //    else
        //    {
        //        _familyinfo.EImageByte = _familyinfo.EImagejson == null ? null : Convert.FromBase64String(_familyinfo.EImagejson);
        //    }
        //    try
        //    {
        //        string result = familyData.AddEmergencyInfo(_familyinfo, Fname, Mname, Lname, Gender, DOB, Email, Relationwithchild, Enotes, HouseholdId,EmegencyId,PhoneNos, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
        //        return Json(new { result });
        //    }
        //    catch (Exception Ex)
        //    {

        //        return Json(Ex.Message);
        //    }
        //}


        //Changes on 3Feb2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult AddEmergencyContactAjax(FamilyHousehold _familyinfo, List<FingerprintsModel.FamilyHousehold.phone> PhoneNos, FormCollection frm, HttpPostedFileBase file)
        {

            //FamilyHousehold _familyinfo = new FamilyHousehold();


            //if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            //{
            //    _familyinfo.EAvatar = System.Web.HttpContext.Current.Request.Files["EAvatar"];
            //}

            // _familyinfo.EAvatar = EAvatar;
            if (_familyinfo.EAvatar != null)
            {
                _familyinfo.EFileName = _familyinfo.EAvatar.FileName;
                _familyinfo.EFileExtension = Path.GetExtension(_familyinfo.EAvatar.FileName);
                BinaryReader b = new BinaryReader(_familyinfo.EAvatar.InputStream);
                _familyinfo.EImageByte = b.ReadBytes(_familyinfo.EAvatar.ContentLength);
            }
            else
            {
                _familyinfo.EImageByte = _familyinfo.EImagejson == null ? null : Convert.FromBase64String(_familyinfo.EImagejson);
            }
            try
            {
                string result = string.Empty;
                if (_familyinfo.EmegencyId == 0)
                    result = familyData.addEmergencyInfo(_familyinfo, 0, Guid.Parse(Session["UserID"].ToString()), PhoneNos, Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                else
                    result = familyData.addEmergencyInfo(_familyinfo, 1, Guid.Parse(Session["UserID"].ToString()), PhoneNos, Session["AgencyID"].ToString(), Session["Roleid"].ToString());

                //  string result = familyData.AddEmergencyInfo(_familyinfo, Fname, Mname, Lname, Gender, DOB, Email, Relationwithchild, Enotes, HouseholdId, EmegencyId, PhoneNos, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                return Json(new { result });
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }
        //End




        ////Changes on 2Feb2017
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26")]
        //public JsonResult AddRestrictedAjax(string Fname, string Lname, string Desc, string RAvatar, string RestrictedId, string HouseholdId)
        //{
        //    FamilyHousehold _familyinfo = new FamilyHousehold();
        //    //_familyinfo.RAvatar.FileName = RAvatar;
        //    //if (info.RAvatar != null)
        //    //{
        //    //    info.RFileName = info.RAvatar.FileName;
        //    //    info.RFileExtension = Path.GetExtension(info.RAvatar.FileName);
        //    //    BinaryReader b = new BinaryReader(info.RAvatar.InputStream);
        //    //    info.RImageByte = b.ReadBytes(info.RAvatar.ContentLength);

        //    //}
        //    //else
        //    //{
        //    //    info.RImageByte = info.RImagejson == null ? null : Convert.FromBase64String(info.RImagejson);

        //    //}
        //    try
        //    {
        //        string result = familyData.AddRestrictedInfo(_familyinfo, Fname, Lname, Desc, RAvatar, RestrictedId, HouseholdId, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString());


        //        return Json(new { result});
        //    }
        //    catch (Exception Ex)
        //    {

        //        return Json(Ex.Message);
        //    }
        //}


        //Changes on 3Feb2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult AddRestrictedAjax(FamilyHousehold _familyinfo)
        {
            // FamilyHousehold _familyinfo = new FamilyHousehold();
            //_familyinfo.RAvatar.FileName = RAvatar;
            if (_familyinfo.RAvatar != null)
            {
                _familyinfo.RFileName = _familyinfo.RAvatar.FileName;
                _familyinfo.RFileExtension = Path.GetExtension(_familyinfo.RAvatar.FileName);
                BinaryReader b = new BinaryReader(_familyinfo.RAvatar.InputStream);
                _familyinfo.RImageByte = b.ReadBytes(_familyinfo.RAvatar.ContentLength);

            }
            else
            {
                _familyinfo.RImageByte = _familyinfo.RImagejson == null ? null : Convert.FromBase64String(_familyinfo.RImagejson);

            }
            try
            {
                string result = string.Empty;
                if (_familyinfo.RestrictedId == 0)
                    result = familyData.addRestrictedInfo(_familyinfo, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                else
                    result = familyData.addRestrictedInfo(_familyinfo, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                // _familyinfo = info;


                return Json(new { result });
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }





        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [HttpPost]
        [JsonMaxLengthAttribute]
        public JsonResult SaveScreening(FamilyHousehold info, List<FamilyHousehold.calculateincome> Income1,
        List<FamilyHousehold.calculateincome1> Income2, string Command, FormCollection collection, List<FingerprintsModel.FamilyHousehold.phone> PhoneNos,
        List<FingerprintsModel.FamilyHousehold.Parentphone1> ParentPhone1, List<FingerprintsModel.FamilyHousehold.Parentphone2> ParentPhoneNos1,
        FamilyHousehold.PostedProgram PostedPostedPrograms, List<FamilyHousehold.ImmunizationRecord> Imminization, Screening _screen,
             FamilyHousehold.PostedPMService PostedPostedService, FamilyHousehold.PostedPMProblems PostedPostedPrblms, FamilyHousehold.PostedDisease PostedPostedDisease,
            FamilyHousehold.PostedDiagnosedDisease PostedPostedDiagnosedDisease, FamilyHousehold.PostedChildEHS PostedPostedMedicalEHS,
             FamilyHousehold.PostedChildEHS PostedPostedEHS, Nurse.PostedChildVitamin PostedPostedChildVitamin, Nurse.PostedChildDiet PostedPostedChildDietFull,
           Nurse.PostedChildDrink PostedPostedChildDrink, int saveCommand)
        {


            StringBuilder _string = new StringBuilder();
            if (PostedPostedPrograms.ProgramID != null)
            {
                foreach (string str in PostedPostedPrograms.ProgramID)
                {
                    _string.Append(str + ",");
                }
                info.CProgramType = _string.ToString().Substring(0, _string.Length - 1);
            }
            StringBuilder _familychildinfo = new StringBuilder();
            if (PostedPostedService.PMServiceID != null)
            {
                foreach (string str in PostedPostedService.PMServiceID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherpmservices = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedService.PMServiceID1 != null)
            {
                foreach (string str in PostedPostedService.PMServiceID1)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherpmservices1 = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedPrblms.PMPrblmID != null)
            {
                foreach (string str in PostedPostedPrblms.PMPrblmID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherproblem = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedPrblms.PMPrblmID1 != null)
            {
                foreach (string str in PostedPostedPrblms.PMPrblmID1)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._Pregnantmotherproblem1 = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            if (info._childprogrefid == "1")
            {
                _familychildinfo.Clear();
                if (PostedPostedDisease.DiseaseID != null)
                {
                    foreach (string str in PostedPostedDisease.DiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDirectBloodRelativeEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDiagnosedConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedMedicalEHS.ChildEHSID != null)
                {
                    foreach (string str in PostedPostedMedicalEHS.ChildEHSID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedEHS.ChildEHSID != null)
                {
                    foreach (string str in PostedPostedEHS.ChildEHSID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditions1Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildMedicalTreatmentEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditions2Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
            }
            if (info._childprogrefid == "2")
            {
                _familychildinfo.Clear();
                if (PostedPostedDisease.DiseaseID != null)
                {
                    foreach (string str in PostedPostedDisease.DiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDirectBloodRelativeHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDiagnosedConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildMedicalTreatmentHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                {
                    foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChronicHealthConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
            }
            _familychildinfo.Clear();
            if (PostedPostedChildVitamin.CDietInfoID != null)
            {
                foreach (string str in PostedPostedChildVitamin.CDietInfoID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildChildVitaminSupplement = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedChildDietFull.CDietInfoID != null)
            {
                foreach (string str in PostedPostedChildDietFull.CDietInfoID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildDiet = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }
            _familychildinfo.Clear();
            if (PostedPostedChildDrink.CDrinkID != null)
            {
                foreach (string str in PostedPostedChildDrink.CDrinkID)
                {
                    _familychildinfo.Append(str + ",");
                }
                info._ChildDrink = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
            }

            FamilyHousehold _familyinfo = new FamilyHousehold();
            _familyinfo = info;
            try
            {


                if (info.FileaddressAvatar != null)
                {
                    info.HFileName = info.FileaddressAvatar.FileName;
                    info.HFileExtension = Path.GetExtension(info.FileaddressAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.FileaddressAvatar.InputStream);
                    info.HImageByte = b.ReadBytes(info.FileaddressAvatar.ContentLength);
                }
                else
                {
                    info.HImageByte = info.HFileInString == null ? null : Convert.FromBase64String(info.HFileInString);
                }

                if (collection["DdlPRole"] != null)
                    info.PRole = (collection["DdlPRole"].ToString() == "") ? null : collection["DdlPRole"];
                if (collection["DdlPDegreeEarned"] != null)
                    info.PDegreeEarned = (collection["DdlPDegreeEarned"].ToString() == "") ? null : collection["DdlPDegreeEarned"];
                if (info.PAvatar != null)
                {
                    info.PFileName = info.PAvatar.FileName;
                    info.PFileExtension = Path.GetExtension(info.PAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.PAvatar.InputStream);
                    info.PImageByte = b.ReadBytes(info.PAvatar.ContentLength);
                }
                else
                {
                    info.PImageByte = info.PImagejson == null ? null : Convert.FromBase64String(info.PImagejson);

                }
                if ((!string.IsNullOrEmpty(info.P1firstname)) && (!string.IsNullOrEmpty(info.Pfirstname)))
                {
                    info.Parentsecondexist = 1;
                    if (collection["DdlP1Role"] != null)
                        info.P1Role = (collection["DdlP1Role"].ToString() == "") ? null : collection["DdlP1Role"];
                    if (collection["DdlP1DegreeEarned"] != null)
                        info.P1DegreeEarned = (collection["DdlP1DegreeEarned"].ToString() == "-1") ? null : collection["DdlP1DegreeEarned"];
                    if (info.P1Avatar != null)
                    {
                        info.P1FileName = info.P1Avatar.FileName;
                        info.P1FileExtension = Path.GetExtension(info.P1Avatar.FileName);
                        BinaryReader b = new BinaryReader(info.P1Avatar.InputStream);
                        info.P1ImageByte = b.ReadBytes(info.P1Avatar.ContentLength);

                    }
                    else
                    {
                        info.P1ImageByte = info.P1Imagejson == null ? null : Convert.FromBase64String(info.P1Imagejson);

                    }
                }
                else
                {
                    info.Parentsecondexist = 0;
                }

                if (info.CAvatar != null)
                {
                    info.CFileName = info.CAvatar.FileName;
                    info.CFileExtension = Path.GetExtension(info.CAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.CAvatar.InputStream);
                    info.CImageByte = b.ReadBytes(info.CAvatar.ContentLength);
                }
                else
                {
                    info.CImageByte = info.Imagejson == null ? null : Convert.FromBase64String(info.Imagejson);
                }
                if (info.FiledobRAvatar != null)
                {
                    info.DobFileName = info.FiledobRAvatar.FileName;
                    info.DobFileExtension = Path.GetExtension(info.FiledobRAvatar.FileName);
                    BinaryReader b = new BinaryReader(info.FiledobRAvatar.InputStream);
                    info.Dobaddressform = b.ReadBytes(info.FiledobRAvatar.ContentLength);
                }
                if (info.FileImmunization != null)
                {
                    info.ImmunizationFileName = info.FileImmunization.FileName;
                    info.ImmunizationFileExtension = Path.GetExtension(info.FileImmunization.FileName);
                    BinaryReader b = new BinaryReader(info.FileImmunization.InputStream);
                    info.Immunizationfileinbytes = b.ReadBytes(info.FileImmunization.ContentLength);
                }

                if (info.Income1 != null)
                {
                    foreach (FamilyHousehold.calculateincome ParentIncome1 in Income1)
                    {
                        if (ParentIncome1.SalaryAvatar1 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
                            ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
                            ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar2 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
                            ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
                            ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar3 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
                            ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
                            ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar4 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
                            ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
                            ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
                        }
                        if (ParentIncome1.NoIncomeAvatar != null)
                        {
                            ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
                            ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
                            ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
                        }

                    }



                }
                if (info.Income2 != null)
                {
                    foreach (FamilyHousehold.calculateincome1 ParentIncome1 in Income2)
                    {
                        if (ParentIncome1.SalaryAvatar1 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename1 = ParentIncome1.SalaryAvatar1.FileName;
                            ParentIncome1.SalaryAvatarFileExtension1 = Path.GetExtension(ParentIncome1.SalaryAvatar1.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar1.InputStream);
                            ParentIncome1.SalaryAvatar1bytes = b.ReadBytes(ParentIncome1.SalaryAvatar1.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar2 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename2 = ParentIncome1.SalaryAvatar2.FileName;
                            ParentIncome1.SalaryAvatarFileExtension2 = Path.GetExtension(ParentIncome1.SalaryAvatar2.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar2.InputStream);
                            ParentIncome1.SalaryAvatar2bytes = b.ReadBytes(ParentIncome1.SalaryAvatar2.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar3 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename3 = ParentIncome1.SalaryAvatar3.FileName;
                            ParentIncome1.SalaryAvatarFileExtension3 = Path.GetExtension(ParentIncome1.SalaryAvatar3.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar3.InputStream);
                            ParentIncome1.SalaryAvatar3bytes = b.ReadBytes(ParentIncome1.SalaryAvatar3.ContentLength);
                        }
                        if (ParentIncome1.SalaryAvatar4 != null)
                        {
                            ParentIncome1.SalaryAvatarFilename4 = ParentIncome1.SalaryAvatar4.FileName;
                            ParentIncome1.SalaryAvatarFileExtension4 = Path.GetExtension(ParentIncome1.SalaryAvatar4.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.SalaryAvatar4.InputStream);
                            ParentIncome1.SalaryAvatar4bytes = b.ReadBytes(ParentIncome1.SalaryAvatar4.ContentLength);
                        }
                        if (ParentIncome1.NoIncomeAvatar != null)
                        {
                            ParentIncome1.NoIncomeFilename4 = ParentIncome1.NoIncomeAvatar.FileName;
                            ParentIncome1.NoIncomeFileExtension4 = Path.GetExtension(ParentIncome1.NoIncomeAvatar.FileName);
                            BinaryReader b = new BinaryReader(ParentIncome1.NoIncomeAvatar.InputStream);
                            ParentIncome1.NoIncomeAvatarbytes = b.ReadBytes(ParentIncome1.NoIncomeAvatar.ContentLength);
                        }
                    }
                }



                if (_screen.ScreeningList != null && _screen.ScreeningList.Count > 0)
                {

                    for (int i = 0; i < _screen.ScreeningList.Count; i++)
                    {
                        if (_screen.ScreeningList[i].ApprovedFile != null)
                        {
                            _screen.ScreeningList[i].ApprovedFileName = _screen.ScreeningList[i].ApprovedFile.FileName;
                            _screen.ScreeningList[i].ApprovedFileExtension = Path.GetExtension(_screen.ScreeningList[i].ApprovedFileName);
                            BinaryReader br = new BinaryReader(_screen.ScreeningList[i].ApprovedFile.InputStream);
                            _screen.ScreeningList[i].ApprovedImageByte = br.ReadBytes(_screen.ScreeningList[i].ApprovedFile.ContentLength);
                        }

                        else
                        {

                            _screen.ScreeningList[i].ApprovedImageByte = _screen.ScreeningList[i].ApprovedImageJson == null ? null : Convert.FromBase64String(_screen.ScreeningList[i].ApprovedImageJson);

                        }
                    }



                }


                //if (_screen.Physical != null)
                //{
                //    _screen.PhysicalFileName = _screen.Physical.FileName;
                //    _screen.PhysicalFileExtension = Path.GetExtension(_screen.Physical.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Physical.InputStream);
                //    _screen.PhysicalImageByte = b.ReadBytes(_screen.Physical.ContentLength);
                //}
                //else
                //{
                //    _screen.PhysicalImageByte = _screen.PhysicalImagejson == null ? null : Convert.FromBase64String(_screen.PhysicalImagejson);

                //}
                //if (_screen.Dental != null)
                //{
                //    _screen.DentalFileName = _screen.Dental.FileName;
                //    _screen.DentalFileExtension = Path.GetExtension(_screen.Dental.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Dental.InputStream);
                //    _screen.DentalImageByte = b.ReadBytes(_screen.Dental.ContentLength);
                //}
                //else
                //{
                //    _screen.DentalImageByte = _screen.DentalImagejson == null ? null : Convert.FromBase64String(_screen.DentalImagejson);

                //}
                //if (_screen.Vision != null)
                //{
                //    _screen.VisionFileName = _screen.Vision.FileName;
                //    _screen.VisionFileExtension = Path.GetExtension(_screen.Vision.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Vision.InputStream);
                //    _screen.VisionImageByte = b.ReadBytes(_screen.Vision.ContentLength);
                //}
                //else
                //{
                //    _screen.VisionImageByte = _screen.VisionImagejson == null ? null : Convert.FromBase64String(_screen.VisionImagejson);

                //}
                //if (_screen.Hearing != null)
                //{
                //    _screen.HearingFileName = _screen.Hearing.FileName;
                //    _screen.HearingFileExtension = Path.GetExtension(_screen.Hearing.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Hearing.InputStream);
                //    _screen.HearingImageByte = b.ReadBytes(_screen.Hearing.ContentLength);
                //}
                //else
                //{
                //    _screen.HearingImageByte = _screen.HearingImagejson == null ? null : Convert.FromBase64String(_screen.HearingImagejson);

                //}
                //if (_screen.Develop != null)
                //{
                //    _screen.DevelopFileName = _screen.Develop.FileName;
                //    _screen.DevelopFileExtension = Path.GetExtension(_screen.Develop.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Develop.InputStream);
                //    _screen.DevelopImageByte = b.ReadBytes(_screen.Develop.ContentLength);
                //}
                //else
                //{
                //    _screen.DevelopImageByte = _screen.DevelopImagejson == null ? null : Convert.FromBase64String(_screen.DevelopImagejson);

                //}
                //if (_screen.Speech != null)
                //{
                //    _screen.SpeechFileName = _screen.Speech.FileName;
                //    _screen.SpeechFileExtension = Path.GetExtension(_screen.Speech.FileName);
                //    BinaryReader b = new BinaryReader(_screen.Speech.InputStream);
                //    _screen.SpeechImageByte = b.ReadBytes(_screen.Speech.ContentLength);
                //}
                //else
                //{
                //    _screen.SpeechImageByte = _screen.SpeechImagejson == null ? null : Convert.FromBase64String(_screen.SpeechImagejson);

                //}


                if (_screen.ScreeningAccept != null)
                {
                    _screen.ScreeningAcceptFileName = _screen.ScreeningAccept.FileName;
                    _screen.ScreeningAcceptFileExtension = Path.GetExtension(_screen.ScreeningAccept.FileName);
                    BinaryReader b = new BinaryReader(_screen.ScreeningAccept.InputStream);
                    _screen.ScreeningAcceptImageByte = b.ReadBytes(_screen.ScreeningAccept.ContentLength);
                }
                else
                {
                    _screen.ScreeningAcceptImageByte = _screen.ScreeningAcceptImagejson == null ? null : Convert.FromBase64String(_screen.ScreeningAcceptImagejson);

                }





                if (info.Releaseform != null)
                {
                    info.ReleaseformFileName = info.Releaseform.FileName;
                    info.ReleaseformFileExtension = Path.GetExtension(info.Releaseform.FileName);
                    BinaryReader b = new BinaryReader(info.Releaseform.InputStream);
                    info.Releaseformfileinbytes = b.ReadBytes(info.Releaseform.ContentLength);
                }
                string message = string.Empty;
                if (info.HouseholdId == 0)
                    message = familyData.addScreeningInfo(ref info, 0, Guid.Parse(Session["UserID"].ToString()), ParentPhone1, ParentPhoneNos1, Income1, Income2, Imminization, PhoneNos, _screen, Session["Roleid"].ToString(), collection, Request.Files, Server.MapPath("~//TempAttachment//"), saveCommand);
                else
                    message = familyData.addScreeningInfo(ref info, 1, Guid.Parse(Session["UserID"].ToString()), ParentPhone1, ParentPhoneNos1, Income1, Income2, Imminization, PhoneNos, _screen, Session["Roleid"].ToString(), collection, Request.Files, Server.MapPath("~//TempAttachment//"), saveCommand);

                var list = info;

                return Json(new { message, list });

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json("");
        }

        [CustAuthFilter()]
        public ActionResult ClientFileReview()
        {
            if (Session["Roleid"] == null)
            {
                return RedirectToAction("Loginagency", "Login");
            }
            return View();
        }
        [CustAuthFilter()]
        public ActionResult DemographicPercentage()
        {
            if (Session["Roleid"] == null)
            {
                return RedirectToAction("Loginagency", "Login");
            }
            return View();
        }
        [CustAuthFilter()]
        public JsonResult GetClassess(string CurrentMonth)
        {

            List<Class_Center> centers = new List<Class_Center>();
            List<ClientDetails> clientsList = new List<ClientDetails>();
            List<ClientReviewStatus> clientStatus = new List<ClientReviewStatus>();
            try
            {
                Guid? AgencyId = new Guid(Session["AgencyID"].ToString());
                Guid UserId = new Guid(Session["UserID"].ToString());
                string query = "GETCENTER";
                centers = new ClientFileReviewData().GetClassCenter(AgencyId, query);
                Class_Center class_center = new Class_Center();
                class_center.AgencyId = AgencyId;
                string queryCommand = "SELECT";
                clientsList = new ClientFileReviewData().GetClientsForReview(out clientStatus, class_center, queryCommand);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(new { centers, clientsList, clientStatus }, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter()]
        public JsonResult GetClassByCenterID(long CenterID)
        {
            List<Class_Center> classess = new List<Class_Center>();
            try
            {

                Guid? AgencyId = new Guid(Session["AgencyID"].ToString());
                Guid UserId = new Guid(Session["UserID"].ToString());
                string query = "GETCLASSES";
                classess = new ClientFileReviewData().GetClassCenter(AgencyId, query, CenterID);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(classess, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter()]
        public JsonResult CheckIsHost(string currentMonth, long clientID)
        {


            DevelopmentMembers HostInfo = new DevelopmentMembers();
            DevelopmentMembers member = new DevelopmentMembers();
            try
            {
                Guid? AgencyId = new Guid(Session["AgencyID"].ToString());
                Guid UserId = new Guid(Session["UserID"].ToString());
                HostInfo = new ClientFileReviewData().CheckIsHost(currentMonth, AgencyId, UserId, clientID);
                member.AgencyId = AgencyId;
                member.UserId = UserId;
                member = new ClientFileReviewData().GetCurrentMember(member);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { HostInfo, member }, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter()]
        public JsonResult GetClients(Class_Center classCenter)
        {
            List<ClientDetails> clientDetails = new List<ClientDetails>();
            List<ClientReviewStatus> clientStatus = new List<ClientReviewStatus>();
            try
            {
                classCenter.AgencyId = new Guid(Session["AgencyID"].ToString());
                string queryCommand = (classCenter.CenterId == 0 && classCenter.ClassRoomId == 0) ? "SELECT" : "GETBY_CLASS_CENTER";
                clientDetails = new ClientFileReviewData().GetClientsForReview(out clientStatus, classCenter, queryCommand);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { clientDetails, clientStatus }, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter()]
        public JsonResult GetMembers(string ReviewMonth, long ClientId)
        {
            List<DevelopmentMembers> membersList = new List<DevelopmentMembers>();
            int contributorCount = 0;
            DevelopmentMembers members = new DevelopmentMembers();
            try
            {
                members.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                members.ReviewMonth = ReviewMonth;
                members.ClientId = ClientId;
                members.UserId = new Guid(Session["UserID"].ToString());

                membersList = new ClientFileReviewData().GetDevelopmentMembers(out contributorCount, members);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { membersList, contributorCount }, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter()]
        public JsonResult InsertMembersAttendance(string membersList)
        {
            bool IsRowAffected = false;
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                List<DevelopmentMembers> MembersList = js.Deserialize<List<DevelopmentMembers>>(membersList);

                Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                Guid UserId = new Guid(Session["UserID"].ToString());
                IsRowAffected = new ClientFileReviewData().InsertMembersAttendance(MembersList, AgencyId, UserId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(IsRowAffected, JsonRequestBehavior.AllowGet);

        }
        [CustAuthFilter()]
        public JsonResult GetReviewNotes(string MonthReviewed, long ClientID)
        {


            List<ClientReviewNotes> OpenNotesList = new List<ClientReviewNotes>();
            List<ClientReviewNotes> ClosedNotesList = new List<ClientReviewNotes>();
            try
            {
                List<ClientReviewNotes> reviewNotesList = new List<ClientReviewNotes>();
                ClientReviewNotes notes = new ClientReviewNotes();
                notes.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                notes.ClientId = ClientID;
                notes.ReviewMonth = MonthReviewed;
                Guid userId = new Guid(Session["UserID"].ToString());
                reviewNotesList = new ClientFileReviewData().GetReviewNotes(notes, userId);
                OpenNotesList = reviewNotesList.Where(x => x.ReviewStatus == true).ToList();
                ClosedNotesList = reviewNotesList.Where(x => x.ReviewStatus == false).ToList();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { OpenNotesList, ClosedNotesList }, JsonRequestBehavior.AllowGet);

        }
        [CustAuthFilter()]
        public JsonResult InsertReviewNotes(string reviewNotes)
        {

            bool isResult = false;
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                ClientReviewNotes notes = js.Deserialize<ClientReviewNotes>(reviewNotes);
                notes.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                notes.UserId = new Guid(Session["UserID"].ToString());
                isResult = new ClientFileReviewData().InsertReviewNotes(notes);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);

        }
        [CustAuthFilter()]
        public JsonResult UpdateReviewNotes(string ReviewNotes)
        {
            bool isResult = false;
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                ClientReviewNotes notes = js.Deserialize<ClientReviewNotes>(ReviewNotes);

                notes.UserId = new Guid(Session["UserID"].ToString());
                isResult = new ClientFileReviewData().UpdateReviewNotes(notes);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);

        }

        [CustAuthFilter()]
        public JsonResult GetClientProfile(long ClientId, string dateReview)
        {

            List<Clientprofile> clientProfile = new List<Clientprofile>();
            try
            {
                Guid? AgencyId = new Guid(Session["AgencyID"].ToString());
                DateTime MonthReview = Convert.ToDateTime(dateReview);
                clientProfile = new ClientFileReviewData().GetClientProfile(AgencyId, ClientId, MonthReview);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(clientProfile, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter()]
        public ActionResult CFRReport()

        {
            //  var userid = Session["UserID"].ToString();
            return View();
        }
        [CustAuthFilter()]
        public JsonResult GetCFRReport(long centerID = 0)
        {
            List<Guid?> useridList = new List<Guid?>();
            ArrayList anaList = new ArrayList();
            try
            {
                Guid agencyId = new Guid(Session["AgencyID"].ToString());
                Guid userId = new Guid(Session["UserID"].ToString());
                Guid roleId = new Guid(Session["RoleID"].ToString());
                List<List<CFRAnalysis>> analysisList = new List<List<CFRAnalysis>>();
                CFRAnalysis analysis = new CFRAnalysis();

                anaList = new ClientFileReviewData().GetCFRReportData(out useridList, agencyId, userId, roleId, centerID);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { useridList, anaList }, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public JsonResult GetCenters()
        {
            List<Class_Center> centers = new List<Class_Center>();
            try
            {
                string queryCom = "GETCENTER";
                Guid? agencyId = new Guid(Session["AgencyID"].ToString());
                centers = new ClientFileReviewData().GetClassCenter(agencyId, queryCom);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(centers, JsonRequestBehavior.AllowGet);

        }

        [CustAuthFilter("c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public ActionResult MatrixSummary()
        {
            return View();
        }

        [CustAuthFilter("c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public JsonResult GetMatrixReport(string Year)
        {
            List<MasterMatrixSummary> summarylist = new List<MasterMatrixSummary>();
            List<ChangePercentage> percentageList = new List<ChangePercentage>();
            string programYear = string.Empty;
            ArrayList obj = new ArrayList();
            try
            {
                Guid agencyId = new Guid(Session["AgencyID"].ToString());
                Guid userId = new Guid(Session["UserID"].ToString());
                Guid roleId = new Guid(Session["RoleID"].ToString());
                obj = new MatrixReportData().GetMatrixReportData(out summarylist, out percentageList, out programYear, agencyId, userId, roleId, Year);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { obj, summarylist, percentageList, programYear }, JsonRequestBehavior.AllowGet);
        }

        //For Matrix Summary//

        [CustAuthFilter("c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,2af7205e-87b4-4ca7-8ca8-95827c08564c")]

        public JsonResult GetActiveYear()
        {

            List<SelectListItem> yearList = new List<SelectListItem>();
            try
            {

                Guid agencyId = new Guid(Session["AgencyID"].ToString());
                yearList = new MatrixReportData().GetActiveProgramYear(agencyId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(yearList, JsonRequestBehavior.AllowGet);
        }


        //Summary Description//

        [HttpPost]
        [CustAuthFilter("c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,2af7205e-87b4-4ca7-8ca8-95827c08564c")]

        public JsonResult GetSummaryDescription(int groupId)
        {
            List<AssessmentResults> results = new List<AssessmentResults>();
            try
            {
                Guid agencyId = new Guid(Session["AgencyID"].ToString());

                results = new MatrixReportData().GetDescriptionsummary(groupId, agencyId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Get Summary Question
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter("c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,2af7205e-87b4-4ca7-8ca8-95827c08564c")]

        public JsonResult GetSummaryQuestions(long groupId)
        {

            List<QuestionsModel> questionlist = new List<QuestionsModel>();
            QuestionsModel question = new QuestionsModel();
            QuestionsModel questionmodel = null;
            question = new MatrixReportData().GetSummaryquestions(groupId);
            if (question != null)
            {

                int quesCount = question.AssessmentQuestion.Split('?').Count();
                for (int i = 0; i < quesCount; i++)
                {
                    if (!string.IsNullOrEmpty(question.AssessmentQuestion.Split('?')[i]))
                    {
                        questionmodel = new QuestionsModel();
                        questionmodel.AssessmentQuestion = question.AssessmentQuestion.Split('?')[i] + "?";
                        questionmodel.AssessmentQuestionId = question.AssessmentQuestionId;
                        questionmodel.AssessmentGroupId = question.AssessmentGroupId;
                        questionlist.Add(questionmodel);
                    }

                }
            }

            return Json(questionlist, JsonRequestBehavior.AllowGet);
        }


        //  public readonly string GoogleApiKey = System.Configuration.ConfigurationManager.AppSettings["GoogleMapKey"].ToString();

        public readonly string GoogleApiKey = "AIzaSyCbNxbfW0RSsXvvj6J7li2e3WDxlpD9xt8";
        [CustAuthFilter()]
        public ActionResult SaveTransportation(ChildTransportation objTransportation)
        {
            bool Result = false;
            string Address = "";
            var locationService = new GoogleLocationService(GoogleApiKey);
            try
            {

                if (objTransportation != null)
                {
                    int clientid;
                    var isNumeric = int.TryParse(objTransportation.ClientId, out clientid);
                    objTransportation.ClientId = isNumeric ? objTransportation.ClientId : EncryptDecrypt.Decrypt64(objTransportation.ClientId);

                    objTransportation.AgencyId = Session["AgencyID"].ToString();
                    if (objTransportation.PickupStatus == "1")
                    {
                        Address = new FamilyData().GetAddressByClientId(objTransportation.ClientId);

                        var point = locationService.GetLatLongFromAddress(Address);
                        if (point != null)
                        {
                            objTransportation.PickupLatitude = string.IsNullOrEmpty(point.Latitude.ToString()) ? 0 : Convert.ToDouble(point.Latitude.ToString());
                            objTransportation.PickupLongitude = string.IsNullOrEmpty(point.Longitude.ToString()) ? 0 : Convert.ToDouble(point.Longitude.ToString());
                        }
                        else
                        {
                            objTransportation.PickupLatitude = 0;
                            objTransportation.PickupLongitude = 0;
                        }

                    }
                    else if (objTransportation.PickupStatus == "0")
                    {
                        Address = objTransportation.PickupAddress + "," + objTransportation.PickupCity + "," + objTransportation.PickupState + "," + objTransportation.PickupZipcode;
                        var point = locationService.GetLatLongFromAddress(Address);
                        if (point != null)
                        {
                            objTransportation.PickupLatitude = string.IsNullOrEmpty(point.Latitude.ToString()) ? 0 : Convert.ToDouble(point.Latitude.ToString());
                            objTransportation.PickupLongitude = string.IsNullOrEmpty(point.Longitude.ToString()) ? 0 : Convert.ToDouble(point.Longitude.ToString());
                        }
                        else
                        {
                            objTransportation.PickupLatitude = 0;
                            objTransportation.PickupLongitude = 0;
                        }
                    }
                    if (objTransportation.DropStatus == "1")
                    {
                        Address = new FamilyData().GetAddressByClientId(objTransportation.ClientId);

                        var point = locationService.GetLatLongFromAddress(Address);
                        if (point != null)
                        {
                            objTransportation.DropLatitude = string.IsNullOrEmpty(point.Latitude.ToString()) ? 0 : Convert.ToDouble(point.Latitude.ToString());
                            objTransportation.DropLongitude = string.IsNullOrEmpty(point.Longitude.ToString()) ? 0 : Convert.ToDouble(point.Longitude.ToString());
                        }
                        else
                        {
                            objTransportation.DropLatitude = 0;
                            objTransportation.DropLongitude = 0;
                        }
                    }
                    else if (objTransportation.DropStatus == "0")
                    {
                        Address = objTransportation.DropAddress + "," + objTransportation.DropCity + "," + objTransportation.DropState + "," + objTransportation.DropZipcode;
                        var point = locationService.GetLatLongFromAddress(Address);
                        if (point != null)
                        {
                            objTransportation.DropLatitude = string.IsNullOrEmpty(point.Latitude.ToString()) ? 0 : Convert.ToDouble(point.Latitude.ToString());
                            objTransportation.DropLongitude = string.IsNullOrEmpty(point.Longitude.ToString()) ? 0 : Convert.ToDouble(point.Longitude.ToString());
                        }
                        else
                        {
                            objTransportation.DropLatitude = 0;
                            objTransportation.DropLongitude = 0;
                        }
                    }
                    Result = familyData.SaveTransportation(objTransportation, Session["UserID"].ToString());
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }
        [CustAuthFilter()]
        public ActionResult GetTransportationDetails(string ClientId)
        {
            string JSONString = string.Empty;
            try
            {
                int cid;
                var isNumeric = int.TryParse(ClientId, out cid);
                ClientId = isNumeric ? ClientId : EncryptDecrypt.Decrypt64(ClientId);

                DataTable dtTransportation = new DataTable();
                familyData.GetTranportationDetails(ref dtTransportation, Session["AgencyID"].ToString(), ClientId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtTransportation);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        [CustAuthFilter()]
        public ActionResult GetAddressByClientId(string ClientId)
        {
            string JSONString = string.Empty;
            try
            {
                JSONString = new FamilyData().GetAddressByClientId(ClientId);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
       [HttpPost]
        public ActionResult AddWellBabyExamDetails(Screening Screening, HttpPostedFileBase WellBabyDoc)
        {
          
            familyData = new FamilyData();
           Stream sf= WellBabyDoc.InputStream;


            if (WellBabyDoc!=null && WellBabyDoc.ContentLength>0)
            {
                Screening.PhysicalFileName = WellBabyDoc.FileName;
                Screening.PhysicalFileExtension = Path.GetExtension(WellBabyDoc.FileName);
                BinaryReader b = new BinaryReader(WellBabyDoc.InputStream);
                Screening.PhysicalImageByte = b.ReadBytes(WellBabyDoc.ContentLength);
            }
          


            return Json(familyData.AddWellBabyDetails(Screening,Session["AgencyId"].ToString(),Session["UserId"].ToString()));
        }
        [CustAuthFilter()]
        public ActionResult GetWellBabyExamDetails(Screening Screening)
        {
            familyData = new FamilyData();

            List<string> listValues = familyData.GetWellBabyDetail(Screening.Householdid, Session["AgencyId"].ToString(), Screening.Childid, Screening.WellBabyExamMonth, Server.MapPath("~//TempAttachment//"));
            return Json(listValues);
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

       public ActionResult UpdateDateOfBirth(string DOB, string Clientid)
        {
            bool status = false;
            familyData = new FamilyData();
            status = familyData.UpdateDateOfBirth(DOB, Clientid);
            return Json(status);
        }
        public ActionResult UpdateJobTraining(string Clientid, bool IsCompleted)
        {
            bool status = false;
            familyData = new FamilyData();
            status = familyData.UpdateJobTraining(IsCompleted, Clientid);
            return Json(status);
        }
	 [CustAuthFilter()]
        public ActionResult GetDemographicPercentage()
        {
            try
            {
                 return Json(new FamilyData().GetDemographicPercentage(Session["RoleId"].ToString(), Session["AgencyID"].ToString(), Session["UserID"].ToString()));
               //return Json("");
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter()]
        public ActionResult GetAcceptanceStatus(string ClientId)
        {
            try
            {
                return Json(new FamilyData().GetAcceptanceStatus(ClientId, Session["AgencyID"].ToString()));

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter()]
        public JsonResult SaveReviewAgainDetails(string ClientId, string RoleId)
        {

            return Json(new FamilyData().SaveReviewAgainDetails(ClientId, RoleId, Session["AgencyID"].ToString(), Session["Userid"].ToString()));
        }
        [CustAuthFilter()]
        public ActionResult AcceptanceProcess()
        {
            return View();
        }
        [CustAuthFilter()]
        public ActionResult Testing()
        {
            return View();
        }
        [CustAuthFilter()]
        public JsonResult GetProgramDatesByProgram(long programID)
        {
            //DataSet dateset = new DataSet();
            //JavaScriptSerializer serializer = new JavaScriptSerializer();


            Agency.ProgramType programType = new Agency.ProgramType();
            programType = new FamilyData().GetProgramByProgramID(programID);
            // serial = serializer.Serialize(dateset.Tables[0]);
            return Json(programType, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]
        [HttpGet]
        public ActionResult HouseholdDetails(string id = "0")
        {
            FamilyHouseless householdDetails = new FamilyHouseless();
            householdDetails.FamilyHousehold = new FamilyHousehold();
            householdDetails.FamilyHousehold.Encrypthouseholid = id;

            householdDetails = new FamilyData().GetHouseholdDetails(householdDetails);
            ViewBag.message = "";

            return View(householdDetails);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]
        public ActionResult HouseholdDetails(FamilyHouseless homeless)
        {
            string message = "";
            List<CaseNote> CaseNoteList = new List<CaseNote>();
            RosterNew.Users Users = new RosterNew.Users();


            if (homeless.HasNewAddress && homeless.FamilyHousehold.HomeType == 1)
            {
                homeless.FamilyHousehold.Street = homeless.NewAddressHousehold.Street;
                homeless.FamilyHousehold.StreetName = homeless.NewAddressHousehold.StreetName;
                homeless.FamilyHousehold.ZipCode = homeless.NewAddressHousehold.ZipCode;
                homeless.FamilyHousehold.City = homeless.NewAddressHousehold.City;
                homeless.FamilyHousehold.State = homeless.NewAddressHousehold.State;
                homeless.FamilyHousehold.County = homeless.NewAddressHousehold.County;
                homeless.FamilyHousehold.FileaddressAvatar = homeless.FamilyHousehold.FileaddressAvatar;

            }


            if (homeless.FamilyHousehold.FileaddressAvatar != null)
            {
                homeless.FamilyHousehold.HFileName = homeless.FamilyHousehold.FileaddressAvatar.FileName;
                homeless.FamilyHousehold.HFileExtension = Path.GetExtension(homeless.FamilyHousehold.FileaddressAvatar.FileName);
                BinaryReader b = new BinaryReader(homeless.FamilyHousehold.FileaddressAvatar.InputStream);
                homeless.FamilyHousehold.HImageByte = b.ReadBytes(homeless.FamilyHousehold.FileaddressAvatar.ContentLength);
            }
            else
            {
                homeless.FamilyHousehold.HImageByte = homeless.FamilyHousehold.HFileInString == null ? null : Convert.FromBase64String(homeless.FamilyHousehold.HFileInString);
            }

         //   message = familyData.SaveFamilySummary(homeless.FamilyHousehold, Session["AgencyID"].ToString(), Session["UserID"].ToString(), 2);
            if (message == "1")
            {

                if (homeless.HasCaseNoteDetails)
                {

                    homeless.CaseNoteDetails.CaseNotetags = string.IsNullOrEmpty(homeless.CaseNoteDetails.CaseNotetags) ? "" : homeless.CaseNoteDetails.CaseNotetags.Substring(0, homeless.CaseNoteDetails.CaseNotetags.Length - 1);
                    homeless.CaseNoteDetails.ClientIds = (homeless.UsersList.Clientlist != null && homeless.UsersList.Clientlist.Count > 0) ? string.Join(",", homeless.UsersList.Clientlist.Where(x => x.Id != "").Select(x => EncryptDecrypt.Decrypt64(x.Id)).ToArray()) : "";
                    homeless.CaseNoteDetails.StaffIds = (homeless.UsersList.UserList != null && homeless.UsersList.UserList.Count > 0) ? string.Join(",", homeless.UsersList.UserList.Where(x => x.Id != "").Select(x => x.Id).ToArray()) : "";
                    homeless.CaseNoteDetails.HouseHoldId = homeless.FamilyHousehold.HouseholdId.ToString();
                    homeless.CaseNoteDetails.IsLateArrival = false;
                    //homeless.CaseNoteDetails.ClientId = EncryptDecrypt.Decrypt64(homeless.FamilyHousehold.clientIdnew);
                    //homeless.CaseNoteDetails.ProgramId = homeless.FamilyHousehold.CProgramType;
                    //homeless.CaseNoteDetails.CenterId = homeless.FamilyHousehold.CenterId.ToString();
                    homeless.CaseNoteDetails.CaseNoteid = "0";
                    message = new RosterData().SaveCaseNotes(ref message, ref CaseNoteList, ref Users, homeless.CaseNoteDetails, homeless.CaseNoteAttachments, Session["AgencyID"].ToString(), Session["UserID"].ToString(), 2);
                }

                ViewBag.message = "Household summary updated successfully.";

            }





            var sre = homeless.FamilyHousehold.FamilyHasAddress;
            return View(homeless);

            //return RedirectToAction("HouseholdDetails", "AgencyUser", new { @id = homeless.FamilyHousehold.Encrypthouseholid });
        }


        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]
        public JsonResult GetClassroomWithSeats(dynamic centerId)

        {

            List<ClassRoom> classRoomList = new List<ClassRoom>();
            try
            {


                var id = centerId;
                if (centerId[0] != "" || centerId[0] != "0")
                {
                    centerId = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId[0]));
                    //centerId = Convert.ToInt64(centerId[0]);
                }
                else
                {
                    centerId = 0;
                }


                classRoomList = new FamilyData().GetClassRoomWithSeats(centerId, classRoomList);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(classRoomList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult GetAllowFutureForPM()
        {
            FamilyHousehold family = new FamilyHousehold();
            try
            {
                family = familyData.GetFutureApplication();
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(family, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult DeleteFromWaiting(ClientAcceptList clientAccepted)
        {

            bool isResult = false;
            try
            {
                isResult = new FamilyData().DeleteFromWaiting(clientAccepted);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(isResult, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter()]
        public JsonResult AgencyStaffDashboardAjax()
        {
            TempData["userrole"] = FingerprintsModel.EncryptDecrypt.Encrypt64(Session["Roleid"].ToString());
            var dec = FingerprintsModel.EncryptDecrypt.Decrypt64("ZTRjODBmYzItOGI2NC00NDdhLTk5YjQtOTVkMTUxMGIwMWU5");
            int yakkrcount = 0;
            int appointment = 0;
            string PYSDate = "";
            List<HrCenterInfo> hrCenter = new List<HrCenterInfo>();
            hrCenter = new FamilyData().Getcenters(out PYSDate, ref yakkrcount, ref appointment, Session["AgencyID"].ToString(), Session["UserID"].ToString());
            Session["Yakkrcount"] = yakkrcount;
            Session["Appointment"] = appointment;

            return Json(hrCenter, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult GetFollowUpScreenings(string clientId, int followup)
        {
            List<SelectListItem> followupList = new List<SelectListItem>();
            try
            {
                followupList = new RosterData().GetFollowUpScreenings(clientId, followup);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(followupList, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult GetScreeningDetailsByID(string clientId, string programId, string screeningId)
        {

            Screening _screening = new Screening();
            try
            {




                DataSet _data = new NurseData().GetScreeningsbyid(clientId, screeningId, Session["AgencyID"].ToString(), Session["UserID"].ToString(), programId, Session["Roleid"].ToString());

                DataRow documentrow = null;

                if (_data != null)
                {
                    if (_data.Tables.Count > 0 && _data.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.screenings = _data.Tables[0];
                        ViewBag.uploaddocument = Convert.ToBoolean(_data.Tables[0].Rows[0]["DocumentUpload"]);
                    }
                    if (_data.Tables.Count > 1 && _data.Tables[1].Rows.Count > 0)
                    {
                        ViewBag.screeningsdata = _data.Tables[1];
                        ViewBag.screeningdate = _data.Tables[1].Rows[0]["screeningdate"].ToString() != "" ? Convert.ToDateTime(_data.Tables[1].Rows[0]["screeningdate"]).ToString("MM/dd/yyyy") : "";

                    }
                    if (_data.Tables.Count > 2 && _data.Tables[2].Rows.Count > 0)
                    {

                        List<SelectListItem> _screeningList = new List<SelectListItem>();
                        SelectListItem obj = null;
                        foreach (DataRow dr in _data.Tables[2].Rows)
                        {
                            obj = new SelectListItem();
                            obj.Value = dr["ScreeningID"].ToString();
                            obj.Text = dr["ScreeningName"].ToString();
                            _screeningList.Add(obj);
                        }
                        if (_screeningList != null && _screeningList.Count < 1)
                            ViewBag.message = "No Screening found.";
                        ViewBag.screening = _screeningList;
                        TempData["Screen"] = _screeningList;
                    }
                    if (_data.Tables.Count > 3 && _data.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _data.Tables[3].Rows)
                        {
                            ViewBag.missingscreeningdate = dr["missingscreeningdate"].ToString() == "" ? "" : Convert.ToDateTime(dr["missingscreeningdate"]).ToString("MM/dd/yyyy");
                            ViewBag.ScreeningStatus = dr["MissingStatus"].ToString();

                        }


                    }



                }
                ViewBag.screeningid = screeningId;

                if (ViewBag.screeningid == "1" || ViewBag.screeningid == "2" || ViewBag.screeningid == "3" || ViewBag.screeningid == "4" || ViewBag.screeningid == "5" ||
              ViewBag.screeningid == "6")
                {
                    DataTable Screeningvalues = null;
                    if (ViewBag.screeningsdata != null && (DataTable)ViewBag.screeningsdata != null && ((DataTable)ViewBag.screeningsdata).Rows.Count > 0)
                    {
                        Screeningvalues = (DataTable)ViewBag.screeningsdata;
                        documentrow = Screeningvalues.AsEnumerable().FirstOrDefault(P => P.Field<string>("DocumentName") != "");

                        if (documentrow != null)
                        {
                            _screening.PhysicalFileExtension = documentrow["DocumentExtension"].ToString();
                            _screening.PhysicalFileName = documentrow["DocumentName"].ToString();
                            if (documentrow["Documentdata"].ToString() != "")
                            {
                                _screening.PhysicalImageByte = (byte[])documentrow["Documentdata"];
                            }
                        }

                        if (ViewBag.screeningid == "1")
                        {
                            _screening.F001physicalDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F001physicalDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F002physicalResults = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F002physicalResults").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F003physicallFOReason = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F003physicallFOReason").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F004medFollowup = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F004medFollowup").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F005MedFOComments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F005MedFOComments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F006bpResults = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F006bpResults").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F007hgDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F007hgDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F008hgStatus = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F008hgStatus").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F009hgResults = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F009hgResults").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F010hgReferralDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F010hgReferralDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F011hgComments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F011hgComments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F012hgDate2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F012hgDate2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F013hgResults2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F013hgResults2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F014hgFOStatus = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F014hgFOStatus").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F015leadDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F015leadDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F016leadResults = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F016leadResults").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F017leadReferDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F017leadReferDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F018leadComments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F018leadComments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F019leadDate2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F019leadDate2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F020leadResults2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F020leadResults2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.F021leadFOStatus = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "F021leadFOStatus").Select(m => m.Field<string>("Value")).FirstOrDefault();
                        }
                        if (ViewBag.screeningid == "2")
                        {
                            _screening.v022date = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v022date").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v023results = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v023results").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v024comments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v024comments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v025dateR1 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v025dateR1").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v026resultsR1 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v026resultsR1").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v027commentsR1 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v027commentsR1").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v028dateR2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v028dateR2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v029resultsR2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v029resultsR2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v030commentsR2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v030commentsR2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v031ReferralDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v031ReferralDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v032Treatment = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v032Treatment").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v033TreatmentComments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v033TreatmentComments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v034Completedate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v034Completedate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.v035ExamStatus = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "v035ExamStatus").Select(m => m.Field<string>("Value")).FirstOrDefault();
                        }
                        if (ViewBag.screeningid == "3")
                        {
                            _screening.h036Date = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h036Date").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h037Results = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h037Results").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h038Comments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h038Comments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h039DateR1 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h039DateR1").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h040ResultsR1 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h040ResultsR1").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h041CommentsR1 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h041CommentsR1").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h042DateR2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h042DateR2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h043ResultsR2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h043ResultsR2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h044CommentsR2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h044CommentsR2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h045ReferralDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h045ReferralDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h046Treatment = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h046Treatment").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h047TreatmentComments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h047TreatmentComments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h048CompleteDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h048CompleteDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.h049ExamStatus = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "h049ExamStatus").Select(m => m.Field<string>("Value")).FirstOrDefault();
                        }
                        if (ViewBag.screeningid == "4")
                        {
                            _screening.d050evDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d050evDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d051NameDEV = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d051NameDEV").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d052evResults = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d052evResults").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d053evResultsDetails = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d053evResultsDetails").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d054evDate2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d054evDate2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d055evResults2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d055evResults2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d056evReferral = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d056evReferral").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d057evFOStatus = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d057evFOStatus").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d058evComments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d058evComments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.d059evTool = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "d059evTool").Select(m => m.Field<string>("Value")).FirstOrDefault();

                        }
                        if (ViewBag.screeningid == "5")
                        {
                            _screening.E060denDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "E060denDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.E061denResults = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "E061denResults").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.E062denPrevent = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "E062denPrevent").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.E063denReferralDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "E063denReferralDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.E064denTreatment = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "E064denTreatment").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.E065denTreatmentComments = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "E065denTreatmentComments").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.E066denTreatmentReceive = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "E066denTreatmentReceive").Select(m => m.Field<string>("Value")).FirstOrDefault();
                        }
                        if (ViewBag.screeningid == "6")
                        {
                            _screening.s067Date = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s067Date").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s068NameTCR = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s068NameTCR").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s069Details = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s069Details").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s070Results = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s070Results").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s071RescreenTCR = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s071RescreenTCR").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s072RescreenTCRDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s072RescreenTCRDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s073RescreenTCRResults = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s073RescreenTCRResults").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s074ReferralDC = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s074ReferralDC").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s075ReferDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s075ReferDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s076DCDate = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s076DCDate").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s077NameDC = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s077NameDC").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s078DetailDC = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s078DetailDC").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s079DCDate2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s079DCDate2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s080DetailDC2 = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s080DetailDC2").Select(m => m.Field<string>("Value")).FirstOrDefault();
                            _screening.s081FOStatus = Screeningvalues.AsEnumerable().Where(m => m.Field<string>("QuestionAcronym") == "s081FOStatus").Select(m => m.Field<string>("Value")).FirstOrDefault();
                        }


                    }

                }








            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


            if (screeningId == "1")
            {

                return PartialView("~/Views/PartialScreening/PhysicalScreening.cshtml", _screening);
            }
            if (screeningId == "2")
            {

                return PartialView("~/Views/PartialScreening/VisionScreening.cshtml", _screening);
            }
            if (screeningId == "3")
            {

                return PartialView("~/Views/PartialScreening/HearingScreening.cshtml", _screening);
            }
            if (screeningId == "4")
            {

                return PartialView("~/Views/PartialScreening/DevelopmentalScreening.cshtml", _screening);
            }
            if (screeningId == "5")
            {

                return PartialView("~/Views/PartialScreening/DentalScreening.cshtml", _screening);
            }

            if (screeningId == "6")
            {

                return PartialView("~/Views/PartialScreening/SpeechScreening.cshtml", _screening);
            }

            return null;
        }


        [HttpPost]
        [CustAuthFilter()]
        public ActionResult SaveScreeningAjax(Screening _screening, FormCollection _Collections, string screeningdate, string pId, HttpPostedFileBase ScreeningDocument, string programId, string encClientID, string Status)
        {
            try
            {
                string message = "";
                _screening.ClientID = (encClientID == "" || encClientID == "0") ? 0 : Convert.ToInt32(EncryptDecrypt.Decrypt64(encClientID));

                DataSet _data = new NurseData().savecustomscreening(ref message, _screening, _Collections, screeningdate, Status, Session["AgencyID"].ToString(), Session["UserID"].ToString(), ScreeningDocument, programId, Session["Roleid"].ToString());
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return RedirectToAction("Roster", "Roster");
        }



        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]
        [HttpGet]
        public ActionResult WithdrawnClients()
        {

            TransitionWithdrawal transwithdrawal = TransitionWithdrawal.Instance;
            try
            {
                transwithdrawal.ProcessMode = Mode.Withdrawal;

               transwithdrawal = new agencyData().GetTransitionWithDrawalClients((int)transwithdrawal.ProcessMode);
                transwithdrawal.IsWithdrawal = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View(transwithdrawal);
        }

        [CustAuthFilter()]
        public ActionResult GetWithdrawnListByCenter(string CenterId, string ClassRoomID, string FSWId, string SearchText = "",string programYear="", int reqPage = 1, int pgSize = 10)
        {

            TransitionWithdrawal transwithdrawal = TransitionWithdrawal.Instance;
            try
            {
                int _centerid = 0;
                int _classroomid = 0;

                _centerid = string.IsNullOrEmpty(CenterId) || CenterId == "0" ? 0 : Convert.ToInt32(EncryptDecrypt.Decrypt64(CenterId));
                _classroomid = string.IsNullOrEmpty(ClassRoomID) || ClassRoomID == "0" ? 0 : Convert.ToInt32(EncryptDecrypt.Decrypt64(ClassRoomID));

                transwithdrawal.ProcessMode = Mode.Withdrawal;

                transwithdrawal = new agencyData().GetTransitionWithDrawalClients((int)transwithdrawal.ProcessMode, _centerid, _classroomid, FSWId, SearchText, programYear, reqPage, pgSize);
                transwithdrawal.IsWithdrawal = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return PartialView("~/Views/Partialviews/TransitionWithdrawalListPartial.cshtml", transwithdrawal);

        }

        [CustAuthFilter()]
        public ActionResult GetTransitionListByCenter(string CenterId, string ClassRoomID, string FSWId, string SearchText = "",string programYear="", int reqPage = 1, int pgSize = 10)
        {

            TransitionWithdrawal transwithdrawal = TransitionWithdrawal.Instance;
            try
            {
                int _centerid = 0;
                int _classroomid = 0;

                _centerid = string.IsNullOrEmpty(CenterId) || CenterId == "0" ? 0 : Convert.ToInt32(EncryptDecrypt.Decrypt64(CenterId));
                _classroomid = string.IsNullOrEmpty(ClassRoomID) || ClassRoomID == "0" ? 0 : Convert.ToInt32(EncryptDecrypt.Decrypt64(ClassRoomID));

                transwithdrawal.ProcessMode = Mode.Transition;

                transwithdrawal = new agencyData().GetTransitionWithDrawalClients((int)transwithdrawal.ProcessMode, _centerid, _classroomid, FSWId, SearchText,programYear, reqPage, pgSize);

                transwithdrawal.IsWithdrawal = false;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return PartialView("~/Views/Partialviews/TransitionWithdrawalListPartial.cshtml", transwithdrawal);

        }



        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]
        [HttpGet]
        public ActionResult TransitionClients()
        {
            TransitionWithdrawal transwithdrawal = TransitionWithdrawal.Instance;
            try
            {
                transwithdrawal.ProcessMode = Mode.Transition;
                transwithdrawal = new agencyData().GetTransitionWithDrawalClients((int)transwithdrawal.ProcessMode);
                transwithdrawal.IsWithdrawal = false;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View(transwithdrawal);
        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult GetClassroomsWithFSWHVByCenter(string centerId)
        {
            List<SelectListItem> stafflist = new List<SelectListItem>();
            List<ClassRoom> classroomList = new List<ClassRoom>();
            try
            {


                classroomList = new RosterData().GetClassroomsWithFSWHVByCenter(ref stafflist, centerId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(new { classroomList, stafflist }, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public JsonResult GetWithdrawnTranstionQuestionsByClient(string clientId)
        {
            TransitionWithdrawal withdrawntr = TransitionWithdrawal.Instance;
            try
            {
                clientId = EncryptDecrypt.Decrypt64(clientId);
                withdrawntr.ProcessMode = Mode.Withdrawal;
                withdrawntr = new agencyData().GetTransitionWithDrawalClients((int)withdrawntr.ProcessMode);
                withdrawntr.IsWithdrawal = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(withdrawntr, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]
        public JsonResult UpdatePregMomEnrollmentStatus(string Transition, string PregMomChilds)
        {

            List<SeatAvailability> results = new List<SeatAvailability>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            TransitionDetails transitionDetails = new TransitionDetails();

            List<PregMomChilds> pregChilds = new List<FingerprintsModel.PregMomChilds>();
            Transition transition = new FingerprintsModel.Transition();
            List<RosterNew.Attachment> attach = new List<RosterNew.Attachment>();
            var ate = Request.Files;
            var ate2 = ate.AllKeys;

            for (int i = 0; i < ate2.Length; i++)
            {
                RosterNew.Attachment aatt = new RosterNew.Attachment();
                aatt.file = ate[i];
                attach.Add(aatt);
            }


            transition = serializer.Deserialize<Transition>(Transition);
            pregChilds = serializer.Deserialize<List<PregMomChilds>>(PregMomChilds);
            transition.CaseNoteDetails.CaseNoteAttachmentList = new List<RosterNew.Attachment>();

            transition.CaseNoteDetails.CaseNoteAttachmentList = attach;
            transition.ParentID = (transition.ParentID == "0" || transition.ParentID == "") ? "0" : EncryptDecrypt.Decrypt64(transition.ParentID);
            transitionDetails.Transition = transition;
            transitionDetails.PregMomChilds = pregChilds;

            transitionDetails.Transition.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(transitionDetails.Transition.EClientID));
            transitionDetails.Transition.HouseholdId = EncryptDecrypt.Decrypt64(transitionDetails.Transition.HouseholdId);
            transitionDetails.Transition.ProgramTypeId = Convert.ToInt64(EncryptDecrypt.Decrypt64(transitionDetails.Transition.Enc_ProgID));

            results = new RosterData().SaveChildHeadStartTranstion(transitionDetails, Session["AgencyId"].ToString(), Session["UserID"].ToString(), Session["RoleID"].ToString(),true);

            return Json(results);
        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult UpdatePregMomDentalExam(long clientId,bool dentalAnswer)
        {

            bool isResult = false;
            try
            {

                isResult=  new FamilyData().UpdatePregMomDentalExam(clientId, dentalAnswer);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);

            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult GetPIRQuestionAnswer(string ClientId, string QuestionNumber, bool IsPregMom, string programTypeID,string prgYear="")
        {
            // string result = "";

            FamilyData fdata = new FamilyData();
            List<Tuple<int, string, int>> tuples = new List<Tuple<int, string, int>>();
            WithdrawalQuestions transWithdrawal = new WithdrawalQuestions();
            try
            {
                long _progID = 0;
                _progID = Int64.TryParse(programTypeID, out _progID) ? _progID : Convert.ToInt64(EncryptDecrypt.Decrypt64(programTypeID));


                transWithdrawal = fdata.GetAnswerForPIRQuestions(out tuples, ClientId, QuestionNumber, IsPregMom, _progID,prgYear);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
           //if(QuestionNumber=="9")
           // return Json( tuples ,JsonRequestBehavior.AllowGet);
           //else
                return Json(transWithdrawal, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        [CustAuthFilter()]

        public JsonResult SaveWithdrawnQuestion(string transition)
        {            
            bool isResult = false;
            try
            {
                Transition trans = new Transition();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                trans = serializer.Deserialize<Transition>(transition);
                trans.ClientId =Convert.ToInt64( EncryptDecrypt.Decrypt64(trans.EClientID));
                trans.ProgramTypeId = Convert.ToInt64(EncryptDecrypt.Decrypt64(trans.Enc_ProgID));
                trans.ParentID = (string.IsNullOrEmpty(trans.ParentID) || trans.ParentID == "0") ? "0" : EncryptDecrypt.Decrypt64(trans.ParentID);
                trans.ParentID2 = (string.IsNullOrEmpty(trans.ParentID2) || trans.ParentID2 == "0") ? "0" : EncryptDecrypt.Decrypt64(trans.ParentID2);
                isResult = new FamilyData().UpdateWithdrawQuestions(trans, Session["AgencyID"].ToString(),Session["UserID"].ToString());

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Gets the Screening based on the Program Reference Type -- added on 07/05/2018 for FSW Screening
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="refProg"></param>
        /// <returns></returns>
        [HttpPost]


        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d")]
        public PartialViewResult GetScreeningByRefProg(string clientID, string refProg)
        {
            Screening _scr = new Screening();
            try
            {
                _scr = new FamilyData().GetScreeningDataByRefProg(clientID, refProg, Server.MapPath("~//TempAttachment//"));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return PartialView("~/Views/Screening/AddScreening2.cshtml", _scr);
        }


        [CustAuthFilter()]
        public ActionResult ClassRoomAssignment()
        {


            Dictionary<string, int> seatsDictionary = new Dictionary<string, int>();



            try
            {
                new CenterData().GetSeatsCountByCenter(ref seatsDictionary, "0","0", true);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View(seatsDictionary);
        }


        public JsonResult GetSeatsCountByCenter(string centerid,string classroomId="0")
        {

            Dictionary<string, int> dictionarySeats = new Dictionary<string, int>();

           

            try
            {
                classroomId = EncryptDecrypt.Decrypt64(classroomId);

                new CenterData().GetSeatsCountByCenter(ref dictionarySeats, centerid, classroomId, true);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(dictionarySeats, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAcceptedFutureClients(string centerid, string age)
        {
            List<ClientWaitingList> acceptedClients = new List<ClientWaitingList>();
            List<ClassRoom> classRoomList = new List<ClassRoom>();
            Dictionary<string, int> seatsCountDictionary = new Dictionary<string, int>();
            try
            {
                acceptedClients = new FamilyData().GetAcceptedFutureClients(ref classRoomList, ref seatsCountDictionary,centerid, age);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }

            //  return PartialView("~/Views/Partialviews/FutureAcceptedClientsPartial.cshtml", acceptedClients);

            return Json(new { acceptedClients, classRoomList, seatsCountDictionary }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AssignClassroomFutureClients(string clientIds,string classroomId,string centerId,string clsStartDate)
        {
            string result = "0";
            Dictionary<string, Int32> dictionraySeats = new Dictionary<string, int>();
            try
            {
                clientIds = string.Join(",", clientIds.Split(',').ToArray().Select(x => EncryptDecrypt.Decrypt64(x)).ToArray());
                classroomId = EncryptDecrypt.Decrypt64(classroomId);
                result = new CenterData().AssignClassroomFutureClients(ref dictionraySeats,clientIds, classroomId,centerId, clsStartDate);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { result,dictionraySeats },JsonRequestBehavior.AllowGet);
        }

        public JsonResult AcceptClassroomAssignment(string clientIds, string classroomId, string centerId, string clsStartDate)
        {
            string result = "0";
            try
            {
                clientIds = string.Join(",", clientIds.Split(',').ToArray().Select(x => EncryptDecrypt.Decrypt64(x)).ToArray());
                classroomId =(classroomId=="0" || classroomId=="")?"0": EncryptDecrypt.Decrypt64(classroomId);
                result = new CenterData().AcceptClassroomAssignmentClients(clientIds, centerId,classroomId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public JsonResult GetAgeYearsBy(string currentDate = "", string clientId = "")
        {
            string ageString = "";
            ageString = new FamilyData().GetAgeYearsByDate(currentDate, clientId);

            return Json(ageString, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [CustAuthFilter()]
        public JsonResult SaveDocument(string clientID, string screeningID, string screeningPeriod, string customScreeningPeriod, string imageJson = "")
        {
            Screening _scr = new Screening();
            try
            {
                _scr.ClientID = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientID));
                _scr.Screeningid = Convert.ToInt32(screeningID);
                _scr.ScreeningPeriodIndex = screeningPeriod;
                _scr.CustomScreeningPeriod = int.Parse(customScreeningPeriod);
                _scr.ScreeningAcceptImagejson = "";
                var reqFiles = Request.Files;

                if (reqFiles.AllKeys.Length > 0)
                {
                    _scr.ScreeningAcceptFileName = reqFiles[0].FileName;
                    _scr.ScreeningAcceptFileExtension = System.IO.Path.GetExtension(reqFiles[0].FileName);
                    BinaryReader br = new BinaryReader(reqFiles[0].InputStream);
                    _scr.ScreeningAcceptImageByte = br.ReadBytes(reqFiles[0].ContentLength);
                }
                else
                {
                    _scr.ScreeningAcceptFileName = "ScreeningDocument";
                    _scr.ScreeningAcceptFileExtension = ".png";
                    _scr.ScreeningAcceptImageByte = string.IsNullOrEmpty(imageJson) ? null : Convert.FromBase64String(imageJson);

                }

                if (_scr.ScreeningAcceptImageByte == null && string.IsNullOrEmpty(imageJson))
                {
                    return Json(_scr, JsonRequestBehavior.AllowGet);
                }


                _scr = new FamilyData().SaveScreeningDocument(_scr, Server.MapPath("~//TempAttachment//"));
                _scr.ScreeningAcceptImageByte = new byte[0];
                return Json(_scr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                _scr = new Screening();
                return Json(_scr, JsonRequestBehavior.AllowGet);
            }


        }



        [CustAuthFilter()]

        public JsonResult GetAcceptanceClientsByUser(string centerid, string option)
        {
            List<ClientAcceptList> ClientList = new List<ClientAcceptList>();
            string centerName = string.Empty;
            try
            {
                ClientList = new FamilyData().GetAcceptanceClients(ref centerName,centerid, option);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(new { ClientList, centerName }, JsonRequestBehavior.AllowGet);

        }


        #region method to save the Screening Approval and initial screening entry
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [CustAuthFilter()]
        [HttpPost]
        [JsonMaxLengthAttribute]
        public JsonResult SaveScreeningApprovalByClient(FamilyHousehold info,string Command,Screening _screen,int saveCommand)

            
        {

            bool isResult = false;
            try
            { 

            if (_screen.ScreeningList != null && _screen.ScreeningList.Count > 0)
            {

                for (int i = 0; i < _screen.ScreeningList.Count; i++)
                {
                    if (_screen.ScreeningList[i].ApprovedFile != null)
                    {
                        _screen.ScreeningList[i].ApprovedFileName = _screen.ScreeningList[i].ApprovedFile.FileName;
                        _screen.ScreeningList[i].ApprovedFileExtension = Path.GetExtension(_screen.ScreeningList[i].ApprovedFileName);
                        BinaryReader br = new BinaryReader(_screen.ScreeningList[i].ApprovedFile.InputStream);
                        _screen.ScreeningList[i].ApprovedImageByte = br.ReadBytes(_screen.ScreeningList[i].ApprovedFile.ContentLength);
                    }

                    else
                    {

                        _screen.ScreeningList[i].ApprovedImageByte = _screen.ScreeningList[i].ApprovedImageJson == null ? null : Convert.FromBase64String(_screen.ScreeningList[i].ApprovedImageJson);

                    }
                }

            }




            if (_screen.ScreeningAccept != null)
            {
                _screen.ScreeningAcceptFileName = _screen.ScreeningAccept.FileName;
                _screen.ScreeningAcceptFileExtension = Path.GetExtension(_screen.ScreeningAccept.FileName);
                BinaryReader b = new BinaryReader(_screen.ScreeningAccept.InputStream);
                _screen.ScreeningAcceptImageByte = b.ReadBytes(_screen.ScreeningAccept.ContentLength);
            }
            else
            {
                _screen.ScreeningAcceptImageByte = _screen.ScreeningAcceptImagejson == null ? null : Convert.FromBase64String(_screen.ScreeningAcceptImagejson);

            }






                isResult = familyData.SaveScreeningApprovalByClient(ref info, _screen, saveCommand);




        } 
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(isResult, JsonRequestBehavior.AllowGet) ;
        }

        #endregion
    }
}
