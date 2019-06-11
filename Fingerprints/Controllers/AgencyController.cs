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
using System.Text;
using Fingerprints.CustomClasses;
using FingerprintsModel.Enums;

namespace Fingerprints.Controllers
{
    public class AgencyController : Controller
    {
        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         */
        agencyData agencyData = new agencyData();
        [CustAuthFilter(RoleEnum.SuperAdmin)]
        public ActionResult viewAgency(string id = "0")
        {
            TempData["status"] = id;
            ViewBag.id = id;
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult addAgency(string id = "0", string SuperAdmin = "")
        {
            Agency agencyDetail = new Agency();
            AgencyStaff _staff = agencyData.GetData_AllDropdown();
            try
            {
                if (id == "0")
                {
                    ViewBag.mode = 0;
                    ViewData["Title"] = "Add Agency";
                    ViewBag.country = _staff.nationList;
                    TempData["nationality"] = ViewBag.country;
                    agencyDetail.agencyCode = agencyData.agencycode();
                    agencyDetail.TimeZonelist = _staff.TimeZonelist;
                    TempData["Timezonelist"] = _staff.TimeZonelist;
                    ViewBag.RefList = _staff.refList;
                    TempData["RefList"] = ViewBag.RefList;
                    Session["LeftMenu"] = "SuperAdmin";
                    agencyDetail.ProgramTypeList = new List<Agency.ProgramType>();
                    agencyDetail.FundSourcedata = new List<Agency.FundSource>();
                    agencyDetail.ProgramYearList = new List<SelectListItem>();
                    agencyDetail._FundedEnrollment = new Agency.FundedEnrollment();
                    agencyDetail.DivisionsList = new List<SelectListItem>();
                    agencyDetail.DivisionsFullList = new List<Divisions>();
                    agencyDetail.AreasFullList = new List<FingerprintsModel.Areas>();
                    agencyDetail.Areabreakdown = "";
                    agencyDetail.DivisionBreakDown = "";
                    agencyDetail.nationality = _staff.nationList.Where(x => x.Name.Replace(" ", "").ToLower().Trim() == "unitedstates").Select(x => x.NationId).FirstOrDefault();
                    int currentYear = DateTime.Now.Year;
                    agencyDetail.ProgramYearList.Add(new SelectListItem
                    {

                        Text = (currentYear - 1).ToString().Substring(2, 2) + "-" + currentYear.ToString().Substring(2, 2),
                        Value = (currentYear - 1).ToString().Substring(2, 2) + "-" + currentYear.ToString().Substring(2, 2)
                    });

                    agencyDetail.ProgramYearList.Add(new SelectListItem
                    {

                        Text = (currentYear).ToString().Substring(2, 2) + "-" + (currentYear + 1).ToString().Substring(2, 2),
                        Value = (currentYear).ToString().Substring(2, 2) + "-" + (currentYear + 1).ToString().Substring(2, 2)
                    });

                }
                else
                {
                    ViewBag.country = _staff.nationList;
                    TempData["nationality"] = ViewBag.country;
                    agencyDetail = agencyData.editAgency(id);
                    ViewBag.FundSourceData = agencyDetail.FundSourcedata;
                    TempData["FundSourceData"] = agencyDetail.FundSourcedata;

                    if(agencyDetail.ProgramTypeList.Count==0)
                    {
                        agencyDetail.ProgramTypeList.Add(new Agency.ProgramType
                        {
                            DivisionID = "1",
                            programstartDate="",
                            programendDate="",
                            DateFutureApplication="",
                            LastDateCurrentApplication="",
                            TransitionDate="",
                       
                        });
                    }

                    agencyDetail.TimeZonelist = _staff.TimeZonelist;
                    TempData["Timezonelist"] = _staff.TimeZonelist;
                    ViewBag.RefList = _staff.refList;
                    TempData["RefList"] = ViewBag.RefList;
                    Session["oldemailid"] = agencyDetail.primaryEmail;
                    ViewBag.mode = 1;
                    var calling_view = "";
                    if (Request.UrlReferrer != null)
                    {
                        calling_view = Request.UrlReferrer.ToString();
                    }
                    else
                    {
                        calling_view = Request.Url.ToString();
                    }
                    if (Session["AgencyID"] != null)
                    {
                        if (calling_view.Contains("viewAgency"))
                        {
                            Session["LeftMenu"] = "SuperAdmin";
                            ViewData["Title"] = "Edit Agency";
                        }
                        else
                        {
                            if (Request.QueryString["SuperAdmin"] == null)
                            {
                                Session["LeftMenu"] = "AgencyAdmin";
                                ViewData["Title"] = "Agency Profile";
                            }
                            else
                            {
                                Session["LeftMenu"] = "SuperAdmin";
                                ViewData["Title"] = "Edit Agency";
                            }
                        }
                    }
                    else
                    {
                        Session["LeftMenu"] = "SuperAdmin";
                        ViewData["Title"] = "Edit Agency";
                    }


                }
            }
            catch (Exception Ex)
            {

                ViewBag.message = Ex.Message;

            }
            return View(agencyDetail);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult addAgency(Agency agencyinfo, FormCollection collection)
        {
            try
            {
                agencyinfo.AccessDays = collection["DdlAccessType"].ToString() == "-1" ? null : collection["DdlAccessType"].ToString();
                if (agencyinfo.agencyId == null)
                {
                    ViewBag.mode = 0;
                    string RandomPassword = GenerateRandomPassword.GenerateRandomCode(10);
                    agencyinfo.agencyId = "00000000-0000-0000-0000-000000000000";
                    agencyinfo._FundedEnrollment = new Agency.FundedEnrollment();
                    string Agency_Code = "";
                    string message = agencyData.addeditAgency(agencyinfo, 0, RandomPassword, Guid.Parse(Convert.ToString(Session["UserID"])), out Agency_Code, agencyinfo.FundSourcedata);
                    if (message == "1")
                    {
                        string agency = agencyinfo.agencyName;
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                        agency = textInfo.ToTitleCase(agency);
                        string link = UrlExtensions.LinkToRegistrationProcess("/login/Loginagency");
                        string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                        string email = SendMail.SendEmail(agencyinfo.primaryEmail, RandomPassword, agency, Server.MapPath("~/MailTemplate"), imagepath, link, Agency_Code);
                        TempData["message"] = "Record added successfully. ";
                        return Redirect("~/Agency/viewAgency");
                    }
                    else if (message == "2")
                        ViewBag.message = "Email already exist.";
                    else if (message == "3")
                        ViewBag.message = "User name already exist.";
                    else
                        ViewBag.message = message;
                }
                else
                {
                    string Agency_Code = "";
                    string message = agencyData.addeditAgency(agencyinfo, 1, string.Empty, Guid.Parse(Convert.ToString(Session["UserID"])), out Agency_Code, agencyinfo.FundSourcedata);
                    ViewBag.mode = 1;
                    if (message == "1")
                    {
                        TempData["message"] = "Record updated successfully. ";
                        if (Session["oldemailid"] != null)
                        {
                            if (Session["oldemailid"].ToString().ToUpper() != agencyinfo.primaryEmail.ToUpper())
                            {
                                string agency = agencyinfo.agencyName;
                                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                                agency = textInfo.ToTitleCase(agency);
                                string oldemailid = Session["oldemailid"].ToString();
                                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                                Thread thread = new Thread(delegate ()
                                {
                                    sendMail(oldemailid, agencyinfo.primaryEmail, agency, oldemailid + "," + agencyinfo.primaryEmail, Server.MapPath("~/MailTemplate"), imagepath);

                                });
                                thread.Start();

                                Session["oldemailid"] = agencyinfo.primaryEmail;
                            }
                        }
                        if (Session["Roleid"].ToString().Equals("a65bb7c2-e320-42a2-aed4-409a321c08a5"))
                        {
                            return Redirect("~/Agency/addAgency/" + agencyinfo.agencyId);
                        }
                        if (Session["Roleid"].ToString().Equals("f87b4a71-f0a8-43c3-aea7-267e5e37a59d"))
                        {
                            return Redirect("~/Agency/viewAgency");
                        }
                    }
                    else if (message == "2")
                        ViewBag.message = "Email already exist.";
                    else
                        ViewBag.message = message;
                }
                ViewBag.country = TempData["nationality"];
                agencyinfo.TimeZonelist = (List<TimeZoneinfo>)TempData["Timezonelist"];
                agencyinfo.FundSourcedata = (List<Agency.FundSource>)TempData["FundSourceData"];
                if (Session["AgencyID"] != null)
                    ViewData["Title"] = "Agency Profile";
                else
                    if (agencyinfo.agencyId == "00000000-0000-0000-0000-000000000000")
                        ViewData["Title"] = "Add Agency";
                    else
                        ViewData["Title"] = "Edit Agency";
                ViewBag.RefList = TempData["RefList"];
             
                TempData.Keep();
                return View(agencyinfo);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                ViewBag.message = "Error Occured";
                return View();
            }
        }




        [CustAuthFilter(RoleEnum.SuperAdmin)]
        public JsonResult listAgency(string sortOrder, string sortDirection, string search, int pageSize, string clear, int requestedPage = 1)
        {
            try
            {
                int skip = pageSize * (requestedPage - 1);
                string totalrecord;
                //if (clear.Contains("0"))
                TempData["status"] = clear;
                var list = agencyData.getagencyList(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, TempData["status"].ToString(), Convert.ToString(Session["UserID"]));
                TempData.Keep();
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult updateAgency(Guid id, int mode)
        {
            try
            {
                return Json(agencyData.updateAgency(id, mode, Guid.Parse(Convert.ToString(Session["UserID"]))));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult viewagencyuser(string id = "0")
        {
            TempData["status"] = id;
            ViewBag.id = id;
            List<Agency> list = agencyData.AgencyList();
            IEnumerable<SelectListItem> items = list
           .Select(c => new SelectListItem
           {
               Value = c.agencyId.ToString(),
               Text = c.agencyName
           });
            ViewBag.Agency = items;
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult listagencyUser(Guid? agencyId, string sortOrder, bool agencyuserAll, string sortDirection, string search, int pageSize, string clear, int requestedPage = 1)
        {
            try
            {
                int skip = pageSize * (requestedPage - 1);
                string totalrecord;
                //if (clear.Contains("0"))
                TempData["status"] = clear;
                var list = agencyData.getagencyuserList(out totalrecord, agencyuserAll, agencyId, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, TempData["status"].ToString());
                TempData.Keep();
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult updateagency_User(Guid id, int mode)
        {
            try
            {
                return Json(agencyData.updateagencyUser(id, mode, Guid.Parse(Convert.ToString(Session["UserID"]))));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult addAgencyuser(string id = "0")
        {
            if (id.Equals("0"))
            {
                ViewBag.mode = 0;
                ViewData["Title"] = "Add Agency User";
                AgencyStaff _staffList = agencyData.GetData_AllDropdown();
                _staffList.LoginAllowed = true;
                return View(_staffList);
            }
            else
            {
                ViewBag.mode = 1;
                ViewData["Title"] = "Edit Agency User";
                AgencyStaff _staffList = agencyData.GetData_AllDropdown("",1, Guid.Parse(id));
                Session["oldemailid"] = _staffList.EmailAddress;
                _staffList.LoginAllowed = true;
                return View(_staffList);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [HttpPost]
        public ActionResult addAgencyuser(AgencyStaff agencystaff, FormCollection collection, FamilyHousehold.Center Centers, FamilyHousehold.Role Rolelist)
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
            AgencyStaff _staffList = new AgencyStaff();
            agencystaff.AgencyStaffId = Guid.Parse(collection["AgencyStaffId"].ToString());
            #region Add Agency User Code
            if (agencystaff.AgencyStaffId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                try
                {
                    ViewData["Title"] = "Add Agency User";
                    ViewBag.mode = 0;
                    agencystaff.UpdatedBy = agencystaff.CreatedBy = Session["UserId"].ToString();
                    agencystaff.Race = collection["DdlRaceList"] == null ? null : collection["DdlRaceList"].ToString();
                    agencystaff.Natinality = collection["DdlNationList"] == null  ? null : collection["DdlNationList"].ToString();
                    agencystaff.HighestEducation = collection["DdlHighestEducation"] == null ? null : collection["DdlHighestEducation"].ToString();
                    agencystaff.EarlyChildHood = collection["DdlEarlyChildHood"] == null ? null : collection["DdlEarlyChildHood"].ToString();
                    agencystaff.GettingDegree = collection["DdlGettingDegree"] == null ? null : collection["DdlGettingDegree"].ToString();
                    agencystaff.Contractor = collection["DdlContractor"] == null ? null : collection["DdlContractor"].ToString();
                    agencystaff.AssociatedProgram = collection["DdlAssociatedProgram"] == null ? null : collection["DdlAssociatedProgram"].ToString();
                    agencystaff.Replacement = collection["DdlReplacement"] == null ? null : collection["DdlReplacement"].ToString();
                    agencystaff.AccessDays = collection["DdlAccessType"] == null ? null : collection["DdlAccessType"].ToString();
                    agencystaff.HRCenter = collection["DdlHrCenter"] == null ? null : collection["DdlHrCenter"].ToString();
                    agencystaff.Gender = collection["DdlGender"] == null ? null : collection["DdlGender"].ToString();
                   // agencystaff.Classrooms = collection["DdlClassList"] == null ? null : collection["DdlClassList"].ToString();//Changes

                    agencystaff.PirRoleid = collection["DdlpirList"] == null ? null : collection["DdlpirList"].ToString();

                    string DdlAgencyList, DdlRoleList, AvatarFile, AvatarHfile, AvatarSfile, AvatarTfile;
                    // DdlAgencyList = collection["DdlAgencyList"].ToString();
                    DdlAgencyList = collection["HiddenAgencyId"].ToString();
                    DdlRoleList = collection["DdlRoleList"].ToString();
                    agencystaff.SelectedAgencyId = Guid.Parse(DdlAgencyList);
                    agencystaff.SelectedRoleId = DdlRoleList;
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
                    string message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "0", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode);
                    #region Bind page data
                    //if (!String.IsNullOrEmpty(StaffID) && StaffID != "0")
                    //{
                    //    _staffList = agencyData.GetData_AllDropdown(1, Guid.Parse(StaffID));
                    //}

                    #endregion
                    if (message == "1")
                    {
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                        string userName = agencystaff.FirstName;
                        userName = textInfo.ToTitleCase(userName);
                        string link = UrlExtensions.LinkToRegistrationProcess("/login/Loginagency");
                        string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                        Dictionary<string, string> userinfo = agencyData.getuserroleagencyname(StaffID);
                        SendMail.SendEmailagencyuser(agencystaff.EmailAddress, agencystaff.Password, userName, Server.MapPath("~/MailTemplate"), userinfo["agencyname"].ToString(), userinfo["Rolename"].ToString(), imagepath, link, AgencyCode);
                        // ViewBag.message = "Record added successfully.";
                        ModelState.Clear();
                        TempData["message"] = "Record added successfully. ";
                        return Redirect("~/Agency/viewagencyuser");
                        // _staffList = agencyData.GetData_AllDropdown();

                        // return View(_staffList);
                    }
                    else if (message == "2")
                    {

                        ViewBag.message = "Email already exist.";
                        _staffList = agencyData.GetData_AllDropdown(DdlAgencyList, 0, Guid.NewGuid(), agencystaff);
                        return View(_staffList);
                    }

                    else if (message == "3")
                    {
                        _staffList = agencyData.GetData_AllDropdown(DdlAgencyList, 0, Guid.NewGuid(), agencystaff);
                        ViewBag.message = "User name already exist.";
                        return View(_staffList);
                    }
                    else if (message == "4")
                    {

                        ViewBag.message = "Selected agency might be disabled. Please select another agency.";
                        _staffList = agencyData.GetData_AllDropdown(DdlAgencyList, 0, Guid.NewGuid(), agencystaff);
                        return View(_staffList);
                    }
                    else if (message == "8")
                    {

                        ViewBag.message = "User role already exists in center. Please select another center.";
                        _staffList = agencyData.GetData_AllDropdown(DdlAgencyList, 0, Guid.NewGuid(), agencystaff);
                        return View(_staffList);
                    }

                    else
                    {
                        _staffList = agencyData.GetData_AllDropdown(DdlAgencyList, 0, Guid.NewGuid(), agencystaff);
                        ViewBag.message = message;
                        return View(_staffList);


                    }
                    //  return View(_staffList);
                }

                catch (Exception ex)
                {
                    clsError.WriteException(ex); Server.ClearError();
                    return View(_staffList);

                }

            }
            #endregion

            #region Updating Agency user code here
            else
            {
                string message = "";
                ViewBag.mode = 1;
                UpdateAgencyUser(agencystaff, collection, out message);
                if (message == "1")
                {
                    if (Session["oldemailid"] != null)
                    {
                        if (Session["oldemailid"].ToString().ToUpper() != agencystaff.EmailAddress.ToUpper())
                        {
                            string oldemailid = Session["oldemailid"].ToString();
                            string agency = agencystaff.FirstName;
                            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                            agency = textInfo.ToTitleCase(agency);
                            string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                            Thread thread = new Thread(delegate()
                            {
                                sendMail(oldemailid, agencystaff.EmailAddress, agency, oldemailid + "," + agencystaff.EmailAddress, Server.MapPath("~/MailTemplate"), imagepath);

                            });
                            thread.Start();
                            Session["oldemailid"] = agencystaff.EmailAddress;
                        }
                    }
                    TempData["message"] = "Record updated successfully. ";
                    return Redirect("~/Agency/viewagencyuser");
                    // return View(_staffList);
                }
                else if (message == "2")
                    ViewBag.message = "Email already exist.";
               else if (message == "4")
                        ViewBag.message = "Selected agency might be disabled. Please select another agency.";
               else if (message == "8")
                        ViewBag.message = "User role already exists in center. Please select another center.";
                else
                    ViewBag.message = message;
                _staffList = agencyData.GetData_AllDropdown("",1, agencystaff.AgencyStaffId);
                ViewData["Title"] = "Edit Agency User";
                return View(_staffList);
            }

            #endregion
        }
        public void UpdateAgencyUser(AgencyStaff agencystaff, FormCollection collection, out string res)
        {
            res = "";
            try
            {
                agencystaff.AgencyStaffId = Guid.Parse(collection["AgencyStaffId"].ToString());
                agencystaff.UpdatedBy = agencystaff.CreatedBy = Session["UserId"].ToString();
                agencystaff.Race = collection["DdlRaceList"] ==null ? null : collection["DdlRaceList"].ToString();
                agencystaff.Natinality = collection["DdlNationList"] == null ? null : collection["DdlNationList"].ToString();
                agencystaff.HighestEducation = collection["DdlHighestEducation"] == null ? null : collection["DdlHighestEducation"].ToString();
                agencystaff.EarlyChildHood = collection["DdlEarlyChildHood"] == null ? null : collection["DdlEarlyChildHood"].ToString();
                agencystaff.GettingDegree = collection["DdlGettingDegree"] == null ? null : collection["DdlGettingDegree"].ToString();
                agencystaff.Contractor = collection["DdlContractor"] == null ? null : collection["DdlContractor"].ToString();
                agencystaff.AssociatedProgram = collection["DdlAssociatedProgram"] == null ? null : collection["DdlAssociatedProgram"].ToString();
                agencystaff.Replacement = collection["DdlReplacement"] == null ? null : collection["DdlReplacement"].ToString();
                agencystaff.AccessDays = collection["DdlAccessType"] == null ? null : collection["DdlAccessType"].ToString();
                agencystaff.HRCenter = collection["DdlHrCenter"] == null ? null : collection["DdlHrCenter"].ToString();
                agencystaff.Gender = collection["DdlGender"] == null ? null : collection["DdlGender"].ToString();
                agencystaff.PirRoleid = collection["DdlpirList"] == null ? null : collection["DdlpirList"].ToString();
                string DdlAgencyList, DdlRoleList, AvatarFile, AvatarHfile, AvatarSfile, AvatarTfile;
                // DdlAgencyList = collection["DdlAgencyList"].ToString();
                DdlAgencyList = collection["HiddenAgencyId"].ToString();
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



                string StaffID = "", AgencyCode = ""; ;
                string message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "1", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode);
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
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult editAgencyUser(Guid id)
        {

            try
            {
                AgencyStaff _staffList = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString(),1, id);
                return View(_staffList);
            }
            catch (Exception Ex)
            {

                ViewBag.message = Ex.Message;
                return View();
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [HttpPost]
        public ActionResult editAgencyUser(AgencyStaff agencystaff, FormCollection collection)
        {

            try
            {
                agencystaff.AgencyStaffId = Guid.Parse(collection["AgencyStaffId"].ToString());
                agencystaff.UpdatedBy = agencystaff.CreatedBy = Session["UserId"].ToString();
                agencystaff.Race = collection["DdlRaceList"].ToString() == "0" ? null : collection["DdlRaceList"].ToString();
                agencystaff.Natinality = collection["DdlNationList"].ToString() == "0" ? null : collection["DdlNationList"].ToString();
                agencystaff.HighestEducation = collection["DdlHighestEducation"].ToString() == "-1" ? null : collection["DdlHighestEducation"].ToString();
                agencystaff.EarlyChildHood = collection["DdlEarlyChildHood"].ToString() == "-1" ? null : collection["DdlEarlyChildHood"].ToString();
                agencystaff.GettingDegree = collection["DdlGettingDegree"].ToString() == "-1" ? null : collection["DdlGettingDegree"].ToString();
                agencystaff.Contractor = collection["DdlContractor"].ToString() == "-1" ? null : collection["DdlGettingDegree"].ToString();
                agencystaff.AssociatedProgram = collection["DdlAssociatedProgram"].ToString() == "-1" ? null : collection["DdlAssociatedProgram"].ToString();
                agencystaff.Replacement = collection["DdlReplacement"].ToString() == "-1" ? null : collection["DdlReplacement"].ToString();
                agencystaff.AccessDays = collection["DdlAccessType"].ToString();
                agencystaff.HRCenter = collection["DdlHrCenter"].ToString() == "0" ? null : collection["DdlHrCenter"].ToString();
                string DdlAgencyList, DdlRoleList, AvatarFile, AvatarHfile, AvatarSfile, AvatarTfile;
                DdlAgencyList = collection["DdlAgencyList"].ToString();
                DdlRoleList = collection["DdlRoleList"].ToString();

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
                string message = agencyData.Add_Edit_AgencyStaffInfo(agencystaff, "1", DdlAgencyList, DdlRoleList, out StaffID, out AgencyCode);
                if (message == "1")
                {
                    ViewBag.message = "Record updated successfully.";
                }
                else if (message == "2")
                    ViewBag.message = "Email already exist.";
                else
                    ViewBag.message = message;

                // return View(agencystaff);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                Server.ClearError();
                ViewBag.message = Ex.Message;

            }
            AgencyStaff _staffList = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString(), 1, agencystaff.AgencyStaffId);
            return View(_staffList);
        }


        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult LoadCenterByAgency(string AgncyID,string isEndOfYear="0")
        {

            bool isEndYear = string.IsNullOrEmpty(isEndOfYear) ? false : isEndOfYear == "1" ? true : false;
            var CenterList = agencyData.CenterListByAgency(AgncyID,isEndYear);
            return Json(CenterList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutoCompleteAgency(string term, string Active = "0")
        {
            var result = agencyData.AutoCompleteAgencyList(term, Active);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult viewagencystaff(string id = "0",string ak="0")
        {
            if (id == "1")
            {
                TempData["status"] = "1";
                ViewBag.id = "1";
            }
            else if (id == "2")
            {
                TempData["status"] = "2";
                ViewBag.id = "2";
            }
            else if (id == "3")
            {
                TempData["status"] = "3";
                ViewBag.id = "3";
            }
            else if (id == "4")
            {
                TempData["status"] = "4";
                ViewBag.id = "4";

            }
            else
            {
                TempData["status"] = "0";
                ViewBag.id = "0";

            }

            ViewBag.IsEndOfYear = string.IsNullOrEmpty(ak)?"0":ak=="1"?"1":"0";

            


            return View();
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult listagencystaff(string sortOrder, string sortDirection, string search, int pageSize, string clear, int requestedPage = 1, string isEndOfYear="0")
        {
            try
            {
                int skip = pageSize * (requestedPage - 1);
                string totalrecord;
                //if (clear.Contains("0"))
                TempData["status"] = clear;
                bool isEndYear = string.IsNullOrEmpty(isEndOfYear) ? false : isEndOfYear == "1" ? true : false;

                var list = agencyData.getagencystaffList(out totalrecord, Session["AgencyID"].ToString(), sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, TempData["status"].ToString(), isEndYear);
                TempData.Keep();
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,2d9822cd-85a3-4269-9609-9aabb914d792")]
        public JsonResult updateagencyStaff(Guid id, int mode,string isEndOfYear="0")
        {
            try
            {
                bool isEndYear = string.IsNullOrEmpty(isEndOfYear) ? false : isEndOfYear == "1" ? true : false;
                return Json(agencyData.updateagencystaff(id, mode, Guid.Parse(Convert.ToString(Session["UserID"])), isEndYear));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        public void sendMail(string emailold, string emailidnew, string name, string emailcombine, string path, string imagepath)
        {

            SendMail.SendEmailoldnew(emailold, emailidnew, name, emailcombine, path, imagepath);

        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult AgencyProfile(string id = "0", string SuperAdmin = "")
        {
            Agency agencyDetail = new Agency();
            AgencyStaff _staff = agencyData.GetData_AllDropdown(Session["AgencyID"].ToString());
            try
            {
                if (id == "0")
                {
                    //Session["AgencyID"] = null;
                    ViewBag.mode = 0;
                    ViewData["Title"] = "Add Agency";
                    ViewBag.country = _staff.nationList;
                    TempData["nationality"] = ViewBag.country;
                    agencyDetail.agencyCode = agencyData.agencycode();
                    agencyDetail.TimeZonelist = _staff.TimeZonelist;
                    TempData["Timezonelist"] = _staff.TimeZonelist;
                    ViewBag.RefList = _staff.refList;

                    Session["LeftMenu"] = "SuperAdmin";
                }
                else
                {
                    ViewBag.country = _staff.nationList;
                    TempData["nationality"] = ViewBag.country;
                    agencyDetail = agencyData.editAgency(id);
                    ViewBag.FundSourceData = agencyDetail.FundSourcedata;
                    TempData["FundSourceData"] = agencyDetail.FundSourcedata;
                    agencyDetail.TimeZonelist = _staff.TimeZonelist;
                    TempData["Timezonelist"] = _staff.TimeZonelist;
                    Session["oldemailid"] = agencyDetail.primaryEmail;
                    ViewBag.RefList = _staff.refList;
                    TempData["RefList"] = ViewBag.RefList;
                    ViewBag.mode = 1;
                    var calling_view = "";
                    if (Request.UrlReferrer != null)
                    {
                        calling_view = Request.UrlReferrer.ToString();
                    }
                    else
                    {
                        calling_view = Request.Url.ToString();
                    }
                    if (Session["AgencyID"] != null)
                    {
                        if (calling_view.Contains("viewAgency"))
                        {
                            Session["LeftMenu"] = "SuperAdmin";
                            ViewData["Title"] = "Edit Agency";
                        }
                        else
                        {
                            if (Request.QueryString["SuperAdmin"] == null)
                            {
                                Session["LeftMenu"] = "AgencyAdmin";
                                ViewData["Title"] = "Agency Profile";
                            }
                            else
                            {
                                Session["LeftMenu"] = "SuperAdmin";
                                ViewData["Title"] = "Edit Agency";
                            }
                        }
                    }
                    else
                    {
                        Session["LeftMenu"] = "SuperAdmin";
                        ViewData["Title"] = "Edit Agency";
                    }


                }
            }
            catch (Exception Ex)
            {

                ViewBag.message = Ex.Message;

            }
            return View(agencyDetail);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult AgencyProfile(Agency agencyinfo, FormCollection collection)
        {
            try
            {

                agencyinfo.AccessDays = collection["DdlAccessType"].ToString() == "-1" ? null : collection["DdlAccessType"].ToString();
                if (agencyinfo.agencyId == null)
                {
                    if (agencyinfo.programstartDate != null && agencyinfo.programendDate != null)
                    {
                    }
                    ViewBag.mode = 0;
                    string RandomPassword = GenerateRandomPassword.GenerateRandomCode(10);
                    agencyinfo.agencyId = "00000000-0000-0000-0000-000000000000";
                    string Agency_Code = "";
                    string message = agencyData.addeditAgency(agencyinfo, 0, RandomPassword, Guid.Parse(Convert.ToString(Session["UserID"])), out Agency_Code, agencyinfo.FundSourcedata);
                    if (message == "1")
                    {
                        string agency = agencyinfo.agencyName;
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                        agency = textInfo.ToTitleCase(agency);
                        string link = UrlExtensions.LinkToRegistrationProcess("/login/Loginagency");
                        string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                        string email = SendMail.SendEmail(agencyinfo.primaryEmail, RandomPassword, agency, Server.MapPath("~/MailTemplate"), imagepath, link, Agency_Code);
                        TempData["message"] = "Record added successfully. ";
                        return Redirect("~/Agency/viewAgency");
                    }
                    else if (message == "2")
                        ViewBag.message = "Email already exist.";
                    else if (message == "3")
                        ViewBag.message = "User name already exist.";
                    else
                        ViewBag.message = message;
                }
                else
                {
                    string Agency_Code = "";
                    string message = agencyData.addeditAgency(agencyinfo, 1, string.Empty, Guid.Parse(Convert.ToString(Session["UserID"])), out Agency_Code, agencyinfo.FundSourcedata);
                    ViewBag.mode = 1;
                    if (message == "1")
                    {
                        TempData["message"] = "Record updated successfully. ";
                        if (Session["oldemailid"] != null)
                        {
                            if (Session["oldemailid"].ToString().ToUpper() != agencyinfo.primaryEmail.ToUpper())
                            {
                                string agency = agencyinfo.agencyName;
                                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                                agency = textInfo.ToTitleCase(agency);
                                string oldemailid = Session["oldemailid"].ToString();
                                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                                Thread thread = new Thread(delegate()
                                {
                                    sendMail(oldemailid, agencyinfo.primaryEmail, agency, oldemailid + "," + agencyinfo.primaryEmail, Server.MapPath("~/MailTemplate"), imagepath);

                                });
                                thread.Start();

                                Session["oldemailid"] = agencyinfo.primaryEmail;
                            }
                        }
                        if (Session["Roleid"].ToString().Equals("a65bb7c2-e320-42a2-aed4-409a321c08a5"))
                        {
                            return Redirect("~/Agency/AgencyProfile/" + agencyinfo.agencyId);
                        }
                        if (Session["Roleid"].ToString().Equals("f87b4a71-f0a8-43c3-aea7-267e5e37a59d"))
                        {
                            return Redirect("~/Agency/viewAgency");
                        }
                    }
                    else if (message == "2")
                        ViewBag.message = "Email already exist.";
                    else
                        ViewBag.message = message;
                }
                ViewBag.country = TempData["nationality"];
                agencyinfo.TimeZonelist = (List<TimeZoneinfo>)TempData["Timezonelist"];
                agencyinfo.FundSourcedata = (List<Agency.FundSource>)TempData["FundSourceData"];
                ViewBag.RefList = TempData["RefList"];
                if (Session["AgencyID"] != null)
                    ViewData["Title"] = "Agency Profile";
                else
                    if (agencyinfo.agencyId == "00000000-0000-0000-0000-000000000000")
                        ViewData["Title"] = "Add Agency";
                    else
                        ViewData["Title"] = "Edit Agency";
                TempData.Keep();
                return View(agencyinfo);
            }
            catch (Exception Ex)
            {
                ViewBag.message = Ex.Message;
                return View();
            }
        }

         [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult listProgDetails(string FundId = "0")//string ProgramID = "0",
        {
            try
            {

                agencyData obj = new agencyData();
                var listProg = obj.ProgDetails(FundId, Session["AgencyID"].ToString()).ToList();//ProgramID,
                return Json(new { listProg });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [JsonMaxLengthAttribute]
        public JsonResult getClassroom(string CenterId = "0", string AgencyId = "0",string programYear="")
        {
            try
            {
                if (string.IsNullOrEmpty(AgencyId))
                {
                    if (Session["AgencyID"] != null)
                        AgencyId = Session["AgencyID"].ToString();
                }

                return Json(agencyData.getclassroominfo(CenterId, AgencyId,programYear));//Convert.ToString(Session["AgencyID"])
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [JsonMaxLengthAttribute]
        public JsonResult getManager(string RoleId = "0")
        {
            try
            {
                return Json(agencyData.getmanagerinfo(RoleId, Convert.ToString(Session["AgencyID"])));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [JsonMaxLengthAttribute]
        public JsonResult getFSWUsers(string AgencyId = "0")
        {
            try
            {
                return Json(agencyData.getusersinfo(AgencyId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [JsonMaxLengthAttribute]
        public JsonResult getagencyid(string AgencyId, string Type)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(AgencyId))
                {
                    AgencyId = Convert.ToString(Session["AgencyID"]);
                }
                return Json(agencyData.getagencyid(AgencyId, Type));//UserId,AgencyId
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [JsonMaxLengthAttribute]
        public JsonResult getClassroomCenter(string CenterId = "0")
        {
            try
            {
                return Json(agencyData.getclassroomdetails(CenterId));//Convert.ToString(Session["AgencyID"])
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        //29Aug2016
         [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
         [JsonMaxLengthAttribute]
         public JsonResult getClassroomCenterAssign(string Type, string CenterId = "0", string Agencyid="")
         {
             try
             {

                if (String.IsNullOrEmpty(Agencyid))
                {
                    if (Session["AgencyID"] != null)
                        Agencyid = Convert.ToString(Session["AgencyID"]);
                }
                return Json(agencyData.getclassroomdetailsassign(CenterId, Type, Agencyid));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }



         [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
         public JsonResult ResendEmail(string emailid, string agencyname, string Username)
         {
             try
             {
                 string agency = agencyname;
                 TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                 agency = textInfo.ToTitleCase(agency);
                 string link = UrlExtensions.LinkToRegistrationProcess("/login/Loginagency");
                 string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                 Thread thread = new Thread(delegate()
                 {
                     SendMail.SendEmail(emailid,Username, agencyname, Server.MapPath("~/MailTemplate"), imagepath, link);

                 });
                 thread.Start();
                 return Json("1");
             }
             catch (Exception Ex)
             {
                 return Json(Ex.Message);
             }
         }

        public ActionResult AdditionalSlots(string YakkrID = "")
        {

            TempData["YakkrID"] = YakkrID;
            AgencyAdditionalSlots res = agencyData.GetAdditionalSlotDetails(Session["AgencyId"].ToString(), Session["UserID"].ToString(), YakkrID);

            return View(res);
        }

        public JsonResult SaveAdditionalSeats(List<AgencyAdditionalSlots> Seats)
        {

            var YakkrID = Convert.ToInt32(TempData["YakkrID"]);
            return Json(agencyData.SaveAdditionalSeats(Session["AgencyId"].ToString(), Session["UserID"].ToString(), Seats, YakkrID));

        }


        //[CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2")]
        //public JsonResult GetFamiliesUnderUser(string userId, string roleId)
        //{
        //    List<SelectListItem> familyList = new List<SelectListItem>();
        //    try
        //    {
        //        string agencyId = Session["AgencyId"].ToString();
        //        familyList = new agencyData().GetFamiliesUnderUserId(userId, agencyId, roleId);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }

        //    return Json(familyList, JsonRequestBehavior.AllowGet);
        //}



        /// <summary>
        /// Gets the Area breakdowns list from the table.
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult GetAreas(string agencyId)
        {
            List<FingerprintsModel.Areas> _areabreakDownList = new List<FingerprintsModel.Areas>();

            if (!string.IsNullOrEmpty(agencyId))
            {
                _areabreakDownList.Add(new FingerprintsModel.Areas
                {
                    AgencyID = new Guid(agencyId)
                });

                _areabreakDownList = new agencyData().GetAreas(_areabreakDownList);
            }


            return Json(_areabreakDownList, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// POST method to insert the Broken Areas to the Table
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]

        public JsonResult AddAreas(List<FingerprintsModel.Areas> areasList)
        {
            bool isResult = false;
            isResult = new agencyData().AddAreas(areasList);
            return Json(new { isResult }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Gets the Division break down list from the table.
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult GetDivisions(string agencyId)
        {


            List<Divisions> _divisionbreakDownList = new List<Divisions>();

            if(!string.IsNullOrEmpty(agencyId))
            {
                _divisionbreakDownList.Add(new Divisions
                {
                    AgencyID = new Guid(agencyId)
                });


                _divisionbreakDownList = new agencyData().GetDivisons(_divisionbreakDownList);
            }
            
            return Json(_divisionbreakDownList, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// POST method to insert the Broken Divisions to the Table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult AddDivisions(List<Divisions> divisionsList)
        {
            bool isResult = false;
            divisionsList = new agencyData().AddDivisions(out isResult, divisionsList);
            return Json(new { isResult, divisionsList }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// POST method to get the City,County and State by zipcode
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        
        public JsonResult GetAddressByZipCode(int Zipcode)
        {
            List<Zipcodes> Zipcodelist = new List<Zipcodes>();
            try
            {
                Zipcodelist = new BillingData().Checkaddress(Zipcode);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(new { Zipcodelist }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Http Post method to Add the Funds 
        /// </summary>
        /// <param name="fundSource"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFunds(Agency.FundSource fundSource)
        {
            bool isResult = false;

           isResult= new agencyData().AddFunds(fundSource,0);
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public ActionResult AddFundsNextProgramYear(Agency.FundSource fundSource)
        {
            bool isResult = false;

            isResult = new agencyData().AddFunds(fundSource,1);

            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]

        public JsonResult SetNextProgramYearDate(string programDate)
        {
            bool isResult = false;
            try
            {
                isResult = agencyData.SetNextProgramYearDate(programDate);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        #region Staff-Mang Mapping


        [CustAuthFilter("3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,f87b4a71-f0a8-43c3-aea7-267e5e37a59d")] //Agency Admin,GenesisEarth Admin,Super Admin
        public ActionResult StaffRoleMapping() {

            StaffDetails stf = StaffDetails.GetInstance();
            var result = new StaffRoleMapping();

            //  if (stf.AgencyId.ToString() == "3b49b025-68eb-4059-8931-68a0577e5fa2") //for Agency Admin
            // {
            string cmd = String.IsNullOrEmpty(stf.AgencyId.ToString()) ? "SuperAdmin" : "AgencyAdmin";

            result = agencyData.GetStaffRoleMappingDetails(stf.AgencyId.ToString(), cmd);
           // }

            return View(result);
        }

        public JsonResult GetStaffRoleList(string mRoleId, string agencyId) {
            string cmd = "StaffRoleListBymId";
            var result = agencyData.GetStaffRoleMappingDetails(agencyId,cmd, null,mRoleId);
                return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateStaffRoleMapping(string mRoleId, List<string> staffRoleIds,string agencyId)
        {
            string cmd = "Update";
             var result = agencyData.GetStaffRoleMappingDetails(agencyId,cmd, staffRoleIds,mRoleId);
             return Json(result, JsonRequestBehavior.AllowGet);
           
        }

        public JsonResult GetManagerRoleMapList(string agencyId) {

           var result =  agencyData.GetStaffRoleMappingDetails(agencyId);

            return Json(result.ManagerRoleTableList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetManagementRole(string stfRole)
        {

            var stf = StaffDetails.GetInstance();
            var result = agencyData.GetStaffRoleMappingDetails(stf.AgencyId.ToString(), "mgrole",null,null, stfRole);

            return Json(result.RolesList, JsonRequestBehavior.AllowGet);
        }

        #endregion Staff-Mang Mapping


        [HttpPost]
        public JsonResult GetUsersListByRoleId(string id)
        {
            try
            {
                var stf = new StaffDetails();
                var result = new HomevisitorData().GetUsersByRoleId(id, stf.RoleId.ToString(), stf.UserId.ToString(), stf.AgencyId.ToString());

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #region Daily Health Check

        [HttpGet]
        [CustAuthFilter(RoleEnum.AgencyAdmin,RoleEnum.GenesisEarthAdministrator,RoleEnum.SuperAdmin)]
        public ActionResult DailyHealthCheck()
        {

          
            return View();
        }


        #region Get Daily Observation Lookup

        [HttpPost]
        [CustAuthFilter(RoleEnum.AgencyAdmin,RoleEnum.GenesisEarthAdministrator,RoleEnum.SuperAdmin)]
        public JsonResult GetDailyObservationLookup(DailyObservation dailyObservation,int mode)
        {
            var staffDetails = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();
            dailyObservation.SkipRows = dailyObservation.GetSkipRows();
            var model = agencyData.GetDailyObservationLookup(staffDetails,dailyObservation, mode);

            return Json(model,JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region Check Daily Observation Lookup already Exists

        [HttpPost]
        [CustAuthFilter(RoleEnum.AgencyAdmin,RoleEnum.GenesisEarthAdministrator,RoleEnum.SuperAdmin)]
        public JsonResult CheckDailyObservationLookupExists(DailyObservation dailyObservation)
        {
            var staffDetails = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();
            int result = 0;
             dailyObservation = agencyData.CheckDailyObservationLookup(out result,staffDetails, dailyObservation);
            return Json(new { Result = result, Model = dailyObservation }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Add /Edit/ Daily Observation Lookup
        [HttpPost]
        [CustAuthFilter(RoleEnum.AgencyAdmin,RoleEnum.GenesisEarthAdministrator,RoleEnum.SuperAdmin)]
        public JsonResult UpsertDailyObservationLookup(DailyObservation dailyObservation, int mode)
        {
            var staffDetails = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();

            int statusResult;
            var result = agencyData.UpsertDailyObservationLookup(out statusResult, staffDetails, dailyObservation);
            dailyObservation.SkipRows = dailyObservation.GetSkipRows();

            if (result)
                dailyObservation = agencyData.GetDailyObservationLookup(staffDetails,dailyObservation, mode);

            return Json(new { Result = result, StatusResult=statusResult, Model = dailyObservation }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Make the selected description will be available for all Agencies
        [HttpPost]
        [CustAuthFilter(RoleEnum.AgencyAdmin, RoleEnum.GenesisEarthAdministrator, RoleEnum.SuperAdmin)]
        public JsonResult AvailDailyObservationLookupAllAgencies(DailyObservation dailyObservation, int mode)
        {
            var staffDetails = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();

            var result = agencyData.AvailDailyObservationAllAgencies(staffDetails, dailyObservation);

            dailyObservation.SkipRows = dailyObservation.GetSkipRows();

            if (result)
                dailyObservation = agencyData.GetDailyObservationLookup(staffDetails, dailyObservation,mode);

            return Json(new { Result = result, Model = dailyObservation }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Remove Daily Observation Lookup

        [HttpPost]
        [CustAuthFilter(RoleEnum.AgencyAdmin, RoleEnum.GenesisEarthAdministrator, RoleEnum.SuperAdmin)]
        public JsonResult RemoveDailyObservationLookup(DailyObservation observation)
        {
            //DailyObservation
            return Json(null);
        }

        #endregion



        #endregion
    }

}