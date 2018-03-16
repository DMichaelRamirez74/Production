using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using System.Threading;
using System.Globalization;
using Fingerprints.ViewModel;
using System.Data.SqlClient;
using Fingerprints.CustomClasses;
namespace Fingerprints.Controllers
{
    public class SuperAdminController : Controller
    {

        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         */
        SuperAdminData datalayer = new SuperAdminData();
        agencyData agencyData = new agencyData();
        CategoryData categoryData = new CategoryData();
        Center _center = new Center();
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult SuperAdminList(string id = "0")
        {
            TempData["status"] = id;
            ViewBag.id = id;
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult AddSuperAdmin(string id = "0")
        {
            SupperAdmin superAdmin = new SupperAdmin();
            AgencyStaff _staff = agencyData.GetData_AllDropdown();
            try
            {
                if (id == "0")
                {
                    ViewBag.mode = 0;
                    ViewData["Title"] = "Add Internal Staff";
                }
                else
                {
                    ViewBag.mode = 1;
                    ViewData["Title"] = "Edit Internal Staff";
                    superAdmin = datalayer.GetSuperAdminInfoById(Guid.Parse(id));

                    Session["oldemailid"] = superAdmin.Emailid;

                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            superAdmin.TimeZonelist = _staff.TimeZonelist;
            TempData["TimeZonelist"] = _staff.TimeZonelist;
            return View(superAdmin);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult listSuperAdmin(string sortOrder, string sortDirection, string search, int pageSize, string clear, int requestedPage = 1)
        {
            try
            {
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                //if (clear.Contains("0"))
                TempData["status"] = clear;
                var list = datalayer.GetSuperAdminList(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, TempData["status"].ToString(), Convert.ToString(Session["UserID"])).ToList();
                TempData.Keep();
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Activate_Deactivate_SuperAdmin(Guid id, string mode)
        {

            try
            {
                return Json(datalayer.Activate_DeactivateSuperAdmin(id, mode, Guid.Parse(Session["UserId"].ToString())));

            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }

        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [HttpPost]
        public ActionResult AddSuperAdmin(SupperAdmin obj)
        {
            obj.TimeZonelist = (List<TimeZoneinfo>)TempData["TimeZonelist"];
            try
            {
                if (obj.superadminId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    ViewData["Title"] = "Add Internal Staff";
                    ViewBag.mode = 0;
                    string RandomPassword = GenerateRandomPassword.GenerateRandomCode(10);
                    obj.UserId = Guid.Parse(Convert.ToString(Session["UserId"].ToString()));
                    string message = datalayer.Add_Edit_SuperAdmininfo(obj, RandomPassword, "0");
                    if (message == "1")
                    {
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                        string Name = textInfo.ToTitleCase(obj.FirstName);
                        string link = UrlExtensions.LinkToRegistrationProcess("/login/Login");
                        string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                        SendMail.SendEmailToSuperAdmin(obj.Emailid, RandomPassword, Name, Server.MapPath("~/MailTemplate"), imagepath, link);
                        TempData["message"] = "Record added successfully. ";
                        return Redirect("~/SuperAdmin/SuperAdminList");
                    }
                    else if (message == "2")
                    {
                        ViewBag.message = "Email already exist.";
                        return View(obj);
                    }
                    //else if (message == "3")
                    //    ViewBag.message = "User name already exist.";
                    else
                    {
                        ViewBag.message = message;
                        return View(obj);
                    }


                }
                else
                {
                    string message = "";
                    ViewBag.mode = 1;
                    UpdateSuperAdminInfo(obj, out message);
                    if (message == "1")
                    {
                        if (Session["oldemailid"] != null)
                        {
                            if (Session["oldemailid"].ToString().ToUpper() != obj.Emailid.ToUpper())
                            {
                                string oldemailid = Session["oldemailid"].ToString();
                                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                                string Name = textInfo.ToTitleCase(obj.FirstName);
                                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                                Thread thread = new Thread(delegate()
                                {
                                    sendMail(oldemailid, obj.Emailid, Name, oldemailid + "," + obj.Emailid, Server.MapPath("~/MailTemplate"), imagepath);

                                });
                                thread.Start();
                                Session["oldemailid"] = obj.Emailid;
                            }
                        }
                        TempData["message"] = "Record updated successfully. ";
                        return Redirect("~/SuperAdmin/SuperAdminList");
                    }
                    else if (message == "2")
                        ViewBag.message = "Email already exist";
                    else
                        ViewBag.message = message;

                    ViewData["Title"] = "Edit Internal Staff";
                    return View(obj);
                }

            }

            catch (Exception ex)
            {
                clsError.WriteException(ex); Server.ClearError();
                ViewBag.message = ex.Message;
                return View();
            }
        }
        //[CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        //public ActionResult EditSuperAdmin(Guid id)
        //{
        //    try
        //    {
        //        SupperAdmin superAdmin = datalayer.GetSuperAdminInfoById(id);
        //        return View(superAdmin);
        //    }
        //    catch (Exception Ex)
        //    {

        //        ViewBag.message = Ex.Message;
        //        return View();
        //    }
        //}
        //[CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        //[HttpPost]
        //public ActionResult EditSuperAdmin(SupperAdmin obj)
        //{
        //    try
        //    {
        //        obj.UserId = Guid.Parse(Convert.ToString(Session["UserId"].ToString()));
        //        string message = datalayer.Add_Edit_SuperAdmininfo(obj, string.Empty, "1");
        //        if (message == "1")
        //        {
        //            ViewBag.message = "Record updated successfully.";
        //        }
        //        else if (message == "2")
        //            ViewBag.message = "Email already exist.";
        //        else
        //            ViewBag.message = message;
        //        return View();
        //    }
        //    catch (Exception Ex)
        //    {
        //        ViewBag.message = Ex.Message;
        //        return View();
        //    }
        //}
        [NonAction]
        void UpdateSuperAdminInfo(SupperAdmin obj, out string res)
        {
            res = "";
            try
            {
                obj.UserId = Guid.Parse(Convert.ToString(Session["UserId"].ToString()));
                string message = datalayer.Add_Edit_SuperAdmininfo(obj, string.Empty, "1");
                res = message;
            }
            catch (Exception Ex)
            {
                ViewBag.message = Ex.Message;
                clsError.WriteException(Ex);
                Server.ClearError();
            }
        }
        public void sendMail(string emailold, string emailidnew, string name, string emailcombine, string path, string imagepath)
        {

            SendMail.SendEmailoldnew(emailold, emailidnew, name, emailcombine, path, imagepath);

        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult ProgramType()
        {
            //ProgramType _prog = (new ProgramTypeData()).GetData_AllDropdown();
            ProgramTypeData progData = new ProgramTypeData();
            ProgramType _prog = progData.GetData_AllDropdown();
            ViewBag.RefList = _prog.refList;
            TempData["RefList"] = ViewBag.RefList;
            return View(_prog);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [HttpPost]
        public ActionResult ProgramType(ProgramType info, FormCollection collection)
        {
            try
            {

                TempData.Keep();
                string message = "";
                ProgramTypeData obj = new ProgramTypeData();
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
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Programdetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                ProgramTypeData info = new ProgramTypeData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.ProgInfoSuperAdmin(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["UserID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Getprogdetails(string ProgramID = "0")
        {
            ProgramTypeData obj = new ProgramTypeData();
            try
            {
                return Json(obj.Getproginfo(ProgramID, null));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult updateprogstatus(string ProgramID, int mode)
        {
            ProgramTypeData obj = new ProgramTypeData();
            try
            {
                return Json(obj.updatestatus(ProgramID, mode, Guid.Parse(Convert.ToString(Session["UserID"]))));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult CopyProgAgency()
        {
            ProgramTypeData obj = new ProgramTypeData();
            try
            {
                return Json(obj.CopyToAgency(Guid.Parse(Convert.ToString(Session["UserID"]))));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        #region  Immunization

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Immunization()
        {
            //if (Session["AgencyID"].ToString() != null)
            //{
            //    string AgencyId = Session["AgencyID"].ToString();
            //}
            //else
            //    string AgencyId=null;
            Immunization immunizations = new Immunization();
            immunizations.getList = (new SuperAdminData()).GetListImmunizationInfo(Convert.ToString(Session["AgencyID"]));
            ViewBag.immunizationlist = immunizations.getList;
            TempData["immunizationlist"] = ViewBag.immunizationlist;
            return View(immunizations);
        }


        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult Immunization(List<Immunization> immunizations)
        {
            Guid UserId = Guid.Parse(Convert.ToString(Session["UserId"].ToString()));
            List<Immunization> _Immulist = null;
            //string AgencyId = Session["AgencyID"].ToString();
            try
            {
                // var Savedata = (new SuperAdminData()).Add_Update_ImmunizationInfo(immunizations, UserId);
                var Savedata = (new SuperAdminData()).Add_Update_ImmunizationInfo(immunizations, UserId, Convert.ToString(Session["AgencyID"]), out _Immulist);
                // TempData["immunizationlist"] = _Immulist;
                // return View(_Immulist);
                if (Savedata == "Data saved successfully.")
                    TempData["message"] = "Data saved successfully.";
                return Json(Savedata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

                if (ex is SqlException)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult ImmunizationDelete(int ImmunizationId)
        {
            try
            {
                var Deletedata = (new SuperAdminData()).Delete_ImmunizationInfo(ImmunizationId, Convert.ToString(Session["AgencyID"]), Convert.ToString(Session["UserId"]));
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult SelectionPoints()
        {
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult Category(string id = "0")
        {
            CategoryData obj = new CategoryData();
            Categoryinfo _categoryinfo = obj.GetCategoryList();//Session["AgencyID"].ToString()
            TempData["ServiceInfo"] = _categoryinfo.categoryList;

            if (id == "0")
            {

                TempData["ServiceInfo"] = _categoryinfo.categoryList;
                ViewBag.mode = 0;
            }
            else
            {
                Categoryinfo objinfo = new Categoryinfo();
                TempData["ServiceInfo"] = _categoryinfo.categoryList;
                _categoryinfo = categoryData.EditServices(id);
                objinfo.ServiceData = _categoryinfo.ServiceData;
                ViewBag.ServiceData = _categoryinfo.ServiceData;
                TempData["ServiceData"] = _categoryinfo.ServiceData;
                ViewBag.mode = 1;
            }





            return View(_categoryinfo);
            // return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult AutoCompleteCategory(string term, string IsDeleted = "0")
        {
            var result = categoryData.AutoCompleteCategoryInfo(term, IsDeleted);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult AddCategoryAjax(string Categoryname, string CategoryId)
        {
            string result;
            Categoryinfo Info = new Categoryinfo();
            try
            {
                if (CategoryId == "0")
                {
                    result = categoryData.AddCategoryInfoAjax(Info, Categoryname, CategoryId, Session["UserID"].ToString(), "0");
                }
                else
                {
                    result = categoryData.AddCategoryInfoAjax(Info, Categoryname, CategoryId, Session["UserID"].ToString(), "1");
                }
                return Json(new { Info, result });
            }
            catch (Exception Ex)
            {

                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Deletecategory(string CategoryID = "0")
        {

            try
            {
                return Json(categoryData.Deletecategoryinfo(CategoryID));//, Session["UserId"].ToString())
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [HttpPost]
        public ActionResult Category(Categoryinfo info, FormCollection collection, List<FingerprintsModel.Categoryinfo.ServiceInfo> ServiceData)
        {

            string message = "";
            try
            {


                if (ViewBag.mode == 0)
                {
                    message = categoryData.addeditCategory(info, 0, Guid.Parse(Convert.ToString(Session["UserID"])), ServiceData);
                }
                else
                {
                    message = categoryData.addeditCategory(info, 1, Guid.Parse(Convert.ToString(Session["UserID"])), ServiceData);
                }
                // string message = categoryData.addeditCategory(info, 0, Guid.Parse(Convert.ToString(Session["UserID"])),ServiceData);
                if (message == "1")
                {
                    ViewBag.message = "Record added successfully. ";
                    ViewBag.result = "Success";
                }
                else if (message == "2")
                {
                    ViewBag.message = "Record updated successfully. ";
                    ViewBag.result = "Success";
                }
                else if (message == "3")
                {
                    ViewBag.message = "Record already exist.";
                }
                else
                {
                    ViewBag.message = message;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            info.categoryList = (List<Categoryinfo.CategoryDetail>)TempData["ServiceInfo"];
            TempData.Keep();
            return View(info);
            // return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Categorydetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                //SchoolDistrictData info = new SchoolDistrictData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = categoryData.CategoryInfo(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["UserId"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Getcategorydetails(string CategoryID = "0")
        {
            try
            {
                ViewBag.mode = 1;
                return Json(categoryData.EditServices(CategoryID));

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Deleteservice(string ServiceID = "0")
        {

            try
            {
                return Json(categoryData.Deleteserviceinfo(ServiceID));//, Session["UserId"].ToString())
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult Center(string id = "0")
        {
            CenterData centerData = new CenterData();
            //Center _center = centerData.GetData_AllDropdown();

            //return View(_center);
            string agencyId = string.Empty;
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
                _center = new CenterData().editcentre(id, agencyId);
                ViewBag.Classroom = _center.Classroom;
                TempData["Classroom"] = _center.Classroom;
                TempData["timezonelist"] = _center.TimeZonelist;

                return View(_center);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View(_center);
            }


        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult AutoCompleteAgency(string term, string Active = "0")
        {
            CenterData centerData = new CenterData();
            var result = centerData.AutoCompleteAgencyList(term, Active);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [HttpPost]
        public ActionResult Center(Center info, FormCollection collection, List<FingerprintsModel.Center.ClassRoom> Classroom)
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
                string DdlAgencyList;
                // DdlAgencyList = collection["DdlAgencyList"].ToString();
                DdlAgencyList = collection["AgencyId"].ToString();
                info.AgencyId = DdlAgencyList;
                info.CreatedBy = Session["UserID"].ToString();
                string message = new CenterData().addeditcenter(info, Classroom);
                if (message == "1")
                {
                    TempData["message"] = "Record added successfully.";
                    return Redirect("~/SuperAdmin/centerlist");
                }
                else if (message == "2")
                {
                    TempData["message"] = "Record updated successfully.";
                    return Redirect("~/SuperAdmin/centerlist");

                }
                else if (message == "3")
                {
                    ViewBag.message = "Center already exist.";
                }
                else
                {
                    ViewBag.message = "An error occurred while adding data.";
                }
                return View(info);
                //info = null;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View(info);

            }

        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
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
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult listcenter(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                string totalrecord, agencyId = string.Empty;
                int skip = pageSize * (requestedPage - 1);
                var list = new CenterData().centerList(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, agencyId);
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        [JsonMaxLengthAttribute]
        public JsonResult listClassroomDetails(string CenterId = "0")
        {
            try
            {
                string agencyId = string.Empty;
                CenterData obj = new CenterData();
                var listClassroom = obj.ClassDetails(CenterId, agencyId).ToList();
                return Json(new { listClassroom });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult GetClassroom(string ClassroomID = "0", string CenterID = "0")
        {
            CenterData obj = new CenterData();
            try
            {
                string agencyId = string.Empty;
                return Json(obj.GetClassroominfo(ClassroomID, CenterID, agencyId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
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
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DeleteClassroomInfo(string ClassroomID = "0", string CenterId = "0")
        {
            CenterData obj = new CenterData();
            try
            {
                string agencyId = string.Empty;
                return Json(obj.DeleteClassroomdetails(ClassroomID, CenterId, agencyId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Deleteclassroom(string CenterId = "0")
        {
            CenterData obj = new CenterData();
            try
            {
                string agencyId = string.Empty;
                return Json(obj.DeleteClassroominfo(CenterId, agencyId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Classroom(string id = "0")
        {
            AgencyStaff _staffList = datalayer.GetData_AllDropdown();
            try
            {

                if (id == "0")
                {
                    ViewBag.mode = 0;
                }
                else
                {
                    ViewBag.mode = 1;
                    AgencyStaff obj = new AgencyStaff();
                    // obj = datalayer.EditClass(id);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View(_staffList);
            //return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult Classroom(AgencyStaff info, FormCollection collection, List<FingerprintsModel.UserInfo> UserList, List<FingerprintsModel.ClassRoom> Classroom, List<FingerprintsModel.classAssign> UserAssignList)
        {
            try
            {
                string DdlAgencyList;
                string message = string.Empty;
                int mode;
                DdlAgencyList = collection["HiddenAgencyId"].ToString();
                info.SelectedAgencyId = Guid.Parse(DdlAgencyList);
                info.centerlist = collection["DdlCentersList"] == null ? null : collection["DdlCentersList"].ToString();

                //   info.Classrooms = collection["Classroom"].ToString();
                //info.Users = collection["UserList"] == null ? null : collection["UserList"].ToString();
                if (UserAssignList != null)
                {
                    mode = 1;
                }
                else
                {
                    mode = 0;
                }


                //if (info.ClassAssignID == 0)
                //    message = datalayer.AssignClass(info, mode, (Convert.ToString(Session["UserID"])),UserList);
                //else
                message = datalayer.AssignClass(info, mode, (Convert.ToString(Session["UserID"])), UserList);
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


            // info.SelectedAgencyId = collection["DdlAccessType"].ToString() == null ? null : collection["DdlAccessType"].ToString();
            return View(info);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult YakkrInfo()
        {
            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        [HttpPost]
        public ActionResult YakkrInfo(Yakkr info)
        {
            try
            {
                string message = "";

                if (info.YakkrRoleID == 0)
                {

                    message = datalayer.AddYakkrDetails(info, 0, Guid.Parse(Session["UserID"].ToString()));
                    ViewBag.result = "Sucess";
                }
                else
                {

                    message = datalayer.AddYakkrDetails(info, 1, Guid.Parse(Session["UserID"].ToString()));
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


            return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult YakkrDetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {

                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = datalayer.YakkrInfoDetails(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["UserId"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Getyakkrdetails(string YakkrRoleID = "0")
        {

            try
            {
                return Json(datalayer.Getyakkrinfo(YakkrRoleID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Deleteyakkr(string YakrrId = "0")
        {

            try
            {
                return Json(datalayer.Deleteyakkrinfo(YakrrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult Slots()
        {
            try
            {
                Slots _Slots = new Slots();
                return View(_Slots);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }


        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult AddSlots(string AgencyId, int Slot, string ProgramYear, int SlotId)
        {
            try
            {


                return Json(datalayer.AddSlots(AgencyId, Slot, ProgramYear, SlotId, Session["UserID"].ToString()));

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json("");
            }



        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult SlotsDetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = datalayer.SlotDetails(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["UserID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DeleteSlot(string SlotId = "0")
        {

            try
            {
                return Json(datalayer.DeleteSlot(SlotId, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult DeleteFund(string FundId, string Agencyid)
        {
            try
            {
                return Json(datalayer.DeleteFund(FundId, Agencyid, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult DeleteFundA(string FundId, string Agencyid)
        {
            try
            {
                return Json(datalayer.DeleteFundA(FundId, Agencyid, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DeleteProg(string progId, string Agencyid)
        {
            try
            {
                return Json(datalayer.DeleteProg(progId, Agencyid, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult DeleteProgA(string progId, string Agencyid)
        {
            try
            {
                return Json(datalayer.DeleteProgA(progId, Agencyid, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult ScreeningSuperAdmin(string id)
        {
            try
            {
                ViewBag.mode = 0;
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.mode = 1;
                    ViewBag.screening = datalayer.EditScreening(EncryptDecrypt.Decrypt64(id), Session["UserID"].ToString());
                }
                return View();

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult Screening(string ScreeningId, string ScreeningName, List<Questions> Questionlist, string AgencyId, string Screeningfor,
            string Programtype, bool Document, string ScreeningDate, bool Inintake)
        {
            try
            {
                string message = datalayer.AddScreeningquestion(ScreeningId, ScreeningName, Questionlist, AgencyId, Session["UserID"].ToString(), Screeningfor, Programtype, Document, ScreeningDate, Inintake);
                if (message == "1")
                {
                    TempData["message"] = "Record saved successfully.";
                }
                return Json(message);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult Screeninglist()
        {
            try
            {
                return View();

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }

        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult listScreening(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                string totalrecord, agencyId = string.Empty;
                int skip = pageSize * (requestedPage - 1);
                var list = datalayer.ScreeningList(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize);
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DeleteScreening(string id)
        {
            try
            {
                return Json(datalayer.DeleteScreening(id, Convert.ToString(Session["UserID"])));
            }
            catch (Exception Ex)
            {
                return Json(Ex.Message);
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DeleteQuestion(string Questionid, string Screeningid)
        {
            try
            {
                return Json(datalayer.DeleteQuestion(Questionid, Screeningid, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DeleteOption(string Optionid)
        {
            try
            {
                return Json(datalayer.DeleteOption(Optionid, Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        //Added by Akansha
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult FPACategory()
        {
            return View();
        }
        [HttpPost]
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult FPACategory(FPACategory info)  //Changes
        {
            try
            {

                TempData.Keep();
                string message = "";
                SuperAdminData obj = new SuperAdminData();
                if (info.FPACategoryID == 0)
                {

                    message = obj.AddFPACategory(info, 0, Guid.Parse(Session["UserID"].ToString()));

                }
                else
                {

                    message = obj.AddFPACategory(info, 1, Guid.Parse(Session["UserID"].ToString()));

                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
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
                    ViewBag.message = "Record already exist.";
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(info);

        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult FPACategorydetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                SuperAdminData info = new SuperAdminData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.FPACateInfo(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["UserID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult GetFPAdetails(string FPACategoryID = "0")
        {
            SuperAdminData obj = new SuperAdminData();
            try
            {
                return Json(obj.GetFPACateinfo(FPACategoryID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DeleteFPACate(string FPACategoryID = "0")
        {
            SuperAdminData obj = new SuperAdminData();
            try
            {
                return Json(obj.DeleteFPAinfo(FPACategoryID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Workshop()
        {
            return View();
        }
        [HttpPost]
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Workshop(Workshop info)  //Changes
        {
            try
            {

                TempData.Keep();
                string message = "";
                SuperAdminData obj = new SuperAdminData();
                if (info.WorkshopID == 0)
                {

                    message = obj.AddWorkshop(info, 0, Convert.ToString(Session["AgencyID"]), Guid.Parse(Session["UserID"].ToString()));

                }
                else
                {

                    message = obj.AddWorkshop(info, 1, Convert.ToString(Session["AgencyID"]), Guid.Parse(Session["UserID"].ToString()));

                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
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
                    ViewBag.message = "Record already exist.";
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(info);

        }

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Workshopdetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                SuperAdminData info = new SuperAdminData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.WorkshopInfo(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["UserID"].ToString(), Convert.ToString(Session["AgencyID"]));
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }

        public ActionResult GetCategoryBySearchText(string SearchText)
        {
            SuperAdminData obj = new SuperAdminData();
            List<SelectListItem> lstItems = new List<SelectListItem>();
            try
            {
                // if (string.IsNullOrEmpty(FamilyId))
                obj.GetCategoryBySearchText(ref lstItems,SearchText);
                //else
                //    new BillingData().GetClientByFamilyId(ref lstItems, FamilyId, Session["AgencyID"].ToString(), ProgramId, CenterId, SearchText);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(lstItems);
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult GetWorkshopdetails(string WorkshopID = "0")
        {
            SuperAdminData obj = new SuperAdminData();
            try
            {
                return Json(obj.GetWorkshopinfo(WorkshopID, Convert.ToString(Session["AgencyID"])));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult DeleteWorkshop(string WorkshopID = "0")
        {
            SuperAdminData obj = new SuperAdminData();
            try
            {
                return Json(obj.DeleteWorkshopinfo(WorkshopID, Convert.ToString(Session["AgencyID"])));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }


        //Added on 7Feb2017
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult HolidayList()
        {
            return View();
        }
        [HttpPost]
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult HolidayList(Holiday info)  //Changes
        {
            try
            {

                TempData.Keep();
                string message = "";
                SuperAdminData obj = new SuperAdminData();
                if (info.HolidayID == 0)
                {

                    message = obj.AddHoliday(info, 0, Convert.ToString(Session["AgencyID"]), Guid.Parse(Session["UserID"].ToString()));

                }
                else
                {

                    message = obj.AddHoliday(info, 1, Convert.ToString(Session["AgencyID"]), Guid.Parse(Session["UserID"].ToString()));

                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
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
                    ViewBag.message = "Record already exist.";
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(info);

        }


        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Holidaydetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                SuperAdminData info = new SuperAdminData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.HolidayInfo(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["UserID"].ToString(), Convert.ToString(Session["AgencyID"]));
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult GetHolidaydetails(string HolidayID = "0")
        {
            SuperAdminData obj = new SuperAdminData();
            try
            {
                return Json(obj.GetHolidayinfo(HolidayID, Convert.ToString(Session["AgencyID"])));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult DeleteHoliday(string HolidayID = "0")
        {
            SuperAdminData obj = new SuperAdminData();
            try
            {
                return Json(obj.DeleteHolidayinfo(HolidayID, Convert.ToString(Session["AgencyID"])));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        //End
        //Add by atul
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult DisablitiesType()
        {
            return View();
        }

        [HttpPost]
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult DisablitiesType(Disablities_Type info)  //Changes
        {
            try
            {

                TempData.Keep();
                string message = "";
                SuperAdminData obj = new SuperAdminData();
                if (info.DisablitiesID == 0)
                {

                    message = obj.AddDisablitiesType(info, 0, Guid.Parse(Session["UserID"].ToString()));

                }
                else
                {

                    message = obj.AddDisablitiesType(info, 1, Guid.Parse(Session["UserID"].ToString()));

                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
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
                    ViewBag.message = "Record already exist.";
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(info);

        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DisablitiesTypedetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                SuperAdminData info = new SuperAdminData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.DisablitiesTypeInfo(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["UserID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult GetDisablitiesTypedetails(string DisablitiesID = "0")
        {
            SuperAdminData obj = new SuperAdminData();
            try
            {
                return Json(obj.GetDisablitiesTypeinfo(DisablitiesID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public JsonResult DeleteDisablitiesType(string DisablitiesID = "0")
        {
            SuperAdminData obj = new SuperAdminData();
            try
            {
                return Json(obj.DeleteDisablitiesTypeinfo(DisablitiesID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        //End

        public ActionResult ImportBoundaryCoordinates()
        {
            return View();
        }

        public ActionResult InsertGeoJsonZipcode(string zipcode, string geometry)
        {
            bool Result = false;
            try
            {
                Result = new ERSEAData().SaveGeoJsonZipcode(zipcode, geometry);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }
        public ActionResult InsertGeoJsonCounty(string county, string geometry)
        {
            bool Result = false;
            try
            {
                Result = new ERSEAData().SaveGeoJsonCounty(county, geometry);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }
        public ActionResult InsertGeoJsonState(string state, string geometry)
        {
            bool Result = false;
            try
            {
                Result = new ERSEAData().SaveGeoJsonState(state, geometry);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }
    }
}