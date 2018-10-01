﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fingerprints.Filters;
using FingerprintsData;
using FingerprintsModel;
using System.Data;
using Fingerprints.CustomClasses;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.end;
using System.Text;
using System.Dynamic;
using System.Web.Script.Serialization;
using Fingerprints.ViewModel;
using Newtonsoft.Json.Linq;

namespace Fingerprints.Controllers
{
    public class HomeController : Controller
    {
        /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         roleid =3b49b025-68eb-4059-8931-68a0577e5fa2 (Agency Admin)
         roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
         roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacvher)
         roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
         roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
         roleid=7c2422ba-7bd4-4278-99af-b694dcab7367(executive)
         roleid=b65759ba-4813-4906-9a69-e180156e42fc (ERSEA Manager)
         roleid=047c02fe-b8f1-4a9b-b01f-539d6a238d80 (Disabilities Manager)
         roleid=9c34ec8E-2359-4704-be89-d9f4b7706e82 (Disability Staff)
         roleid=944d3851-75cc-41e9-b600-3fa904cf951f (Billing Manager)
         roleid=825f6940-9973-42d2-b821-5b6c7c937bfe(Facilities Manager)
         roleid=cb540cea-154c-482e-82a6-c1e0a189f611(Facility Worker)
         roleid=4b77aab6-eed1-4ac3-b498-f3e80cf129c0(Education Manager)
         */
        DisabilityManagerData _DisabilityManagerData = new DisabilityManagerData();
        SuperAdminData superAdmin = new SuperAdminData();
        agencyData agencydata = new agencyData();
        FamilyData _family = new FamilyData();

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d")]
        public ActionResult SuperAdminDashboard()
        {
            ViewBag.superadmindashboard = superAdmin.GetSuperAdmindashboard();
            return View();
        }
        //   [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]

       [CustAuthFilter(userTypeArray: new string[] {Role.agencyAdmin,Role.gEarthAdministrator, Role.superAdmin})]

     

        public ActionResult AgencyAdminDashboard(string id = "0")
        {
            try
            {
                if (!id.Equals("0"))
                {

                    Session["AgencyID"] = id;
                }
                ViewBag.agencyadmindashboard = agencydata.GetagencyAdmindashboard(Session["AgencyID"].ToString());
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View();
        }
        public ActionResult DisableJavascript()
        {
            return View();
        }
        [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792")]
        public ActionResult AgencyHRDashboard()
        {
            try
            {
                ViewBag.agencyhrdashboard = agencydata.GetagencyAdmindashboard(Session["AgencyID"].ToString());
                return View();

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        //Changes on 18Jan2017
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult AgencystaffDashboard()
        {
            try
            {
                
                TempData["userrole"] = FingerprintsModel.EncryptDecrypt.Encrypt64(Session["Roleid"].ToString());
                var dec = FingerprintsModel.EncryptDecrypt.Decrypt64("ZTRjODBmYzItOGI2NC00NDdhLTk5YjQtOTVkMTUxMGIwMWU5");
                int yakkrcount = 0;
                int appointment = 0;
                ViewBag.Centerlist = _family.Getcenters(ref yakkrcount, ref appointment, Session["AgencyID"].ToString(), Session["UserID"].ToString());




                Session["Yakkrcount"] = yakkrcount;
                Session["Appointment"] = appointment;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26")]
        public ActionResult AgencyHealthNurse()
        {
            try
            {
                int yakkrcount = 0;
                ViewBag.Centerlist = _family.GetcentersFSW(ref yakkrcount, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                Session["YakkrCountPending"] = yakkrcount;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        public ActionResult Agencyuserdashboard()
        {

            return View();
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetclientWaitingList(string Centerid, string Option, string Programtype)
        {
            try
            {
                List<ClientWaitingList> _clientWaitingList = new List<ClientWaitingList>();
                Dictionary<string, Int32> slotsDictionary = new Dictionary<string, int>();
                _clientWaitingList= _family.GetclientWaitingList(ref slotsDictionary,  Centerid, Option, Programtype, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                return Json(new { _clientWaitingList, slotsDictionary }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        //[CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26")]

        [CustAuthFilter()]
        public ActionResult ApplicationApprovalDashboard()
        {
            try
            {
                int yakkrcount = 0;
                DataTable Screeninglist = new DataTable(); ;
                ViewBag.Centerlist = _family.GetApplicationApprovalDashboard(ref yakkrcount, ref Screeninglist, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                Session["YakkrCountPending"] = yakkrcount;
                ViewBag.Screeninglists = Screeninglist;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        //Changes on 18Jan2017
        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult HomeVisitorDashboard()
        {
            try
            {
                int yakkrcount = 0;
                int appointment = 0;
                ViewBag.Centerlist = _family.Getcenters(ref yakkrcount, ref appointment, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                Session["Yakkrcount"] = yakkrcount;
                Session["Appointment"] = appointment;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult LoadClientPendinglist(string Centerid, string Type)
        {
            try
            {
                return Json(_family.LoadClientPendinglist(Centerid, Type, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult DeletePendingClient(string Id, string centerid, string Clientid, string householdid, string Programid)
        {
            try
            {
                return Json(_family.DeletePendingClient(Id, centerid, Clientid, householdid, Programid, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetclientAcceptedList(string Centerid, string Option)
        {
            try
            {
                return Json(_family.GetclientAcceptList(Centerid, Option, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetFSWOrHVList(string ClientId,string Centerid, int ListType)
        {
            try
            {
                return Json(_family.GetFSWListByClient(ClientId,Centerid, Session["AgencyID"].ToString(), ListType));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d")]
        //public JsonResult DeleteRejectedRecord(string Id)
        //{
        //    try
        //    {

        //        return Json(_family.DeleteRejectedRecord(Id, Session["UserID"].ToString()));
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //        return Json("Error occured please try again.");
        //    }
        //}
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult DeleteRejectedRecord(string Id, string ClientId, string HouseholdId)
        {
            try
            {

                return Json(_family.DeleteRejectedRecord(Id, ClientId, HouseholdId, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult TeacherDashBoard()
        {

            try
            {

                DataTable Screeninglist = new DataTable();
                new TeacherData().TeacherDashboard(ref Screeninglist, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                ViewBag.Screeninglists = Screeninglist;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult CentralManagerDashboard()
        {

            try
            {
                int yakkrcount = 0;
                DataTable Screeninglist = new DataTable(); ;
                ViewBag.Centerlist = _family.GetApplicationApprovalDashboard(ref yakkrcount, ref Screeninglist, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                Session["YakkrCountPending"] = yakkrcount;
                ViewBag.Screeninglists = Screeninglist;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }



        public ActionResult AreaManagerDashboard()
        {
            try
            {
                string AgencyId = Session["AgencyID"].ToString();
                ExecutiveDashBoard executive;
                executive = new ExecutiveData().GetExecutiveDetails(Session["AgencyID"].ToString(), Session["UserID"].ToString(), "AreaManager");
                TempData["CaseNote"] = executive.listCaseNote;
                return View(executive);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("9ad1750e-2522-4717-a71b-5916a38730ed")]
        public ActionResult HealthManager()
        {

            return View();
        }


        [HttpGet]
        public ActionResult DailySafetyCheckCenterSelection()
        {
            return View();
        }

        //Task
        [HttpGet]

        [CustAuthFilter("b4d86d72-0b86-41b2-adc4-5ccce7e9775b,82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult DailySafetyCheck(Int64? CenterId)
        {
            try
            {
                Guid? userId = null;
                if (Session["UserID"] != null)
                    userId = new Guid(Session["UserID"].ToString());
                ViewBag.CenterId = CenterId;
                List<DailySaftyCheckImages> listImages = new TeacherData().GetDailySaftyCheckImages(userId, Session["Roleid"].ToString(), CenterId);
                return View(listImages);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }


        [CustAuthFilter("b65759ba-4813-4906-9a69-e180156e42fc,7c2422ba-7bd4-4278-99af-b694dcab7367,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public ActionResult CenterClosure()
        {
            return View();
        }

        [CustAuthFilter("b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a65bb7c2-e320-42a2-aed4-409a321c08a5,b65759ba-4813-4906-9a69-e180156e42fc")]
        public ActionResult DaysOff()
        {
            List<DaysOff> offList = new List<DaysOff>();
            DaysOffModel model = new DaysOffModel();
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Guid AgencyId = new Guid(Session["AgencyId"].ToString());
                Guid UserId = new Guid(Session["UserID"].ToString());
                Guid RoleId = new Guid(Session["RoleId"].ToString());
                model = new CenterData().GetDaysOffByUser(AgencyId, UserId, RoleId);
                model.OffDaysString = serializer.Serialize(model.DatesList);
                model.CenterListString = serializer.Serialize(model.CenterList);
                model.ClassRoomListString = serializer.Serialize(model.ClassRoomList);
                //model.DaysOffList = offList;

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(model);
        }


        [HttpPost]
        public JsonResult InsertOffDays(string daysOffString = "")
        {
            DaysOffModel model = new DaysOffModel();
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                DaysOff daysOff = serializer.Deserialize<DaysOff>(daysOffString);

                daysOff.AgencyId = new Guid(Session["AgencyId"].ToString());
                daysOff.CreatedBy = new Guid(Session["UserID"].ToString());
                daysOff.RoleId = new Guid(Session["RoleId"].ToString());
                model = new CenterData().InsertDaysOff(daysOff);
                model.OffDaysString = serializer.Serialize(model.DatesList);
                model.CenterListString = serializer.Serialize(model.CenterList);
                model.ClassRoomListString = serializer.Serialize(model.ClassRoomList);
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(model, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]

        public JsonResult DeleteOffDays(string dayOffIdString)
        {
            DaysOffModel model = new DaysOffModel();
            DaysOff daysOff = new FingerprintsModel.DaysOff();
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;

                string[] dayOffId = serializer.Deserialize<string[]>(dayOffIdString);
                daysOff.AgencyId = new Guid(Session["AgencyId"].ToString());
                daysOff.CreatedBy = new Guid(Session["UserID"].ToString());
                daysOff.RoleId = new Guid(Session["RoleId"].ToString());
                model = new CenterData().DeleteDaysOff(daysOff, dayOffId);
                model.OffDaysString = serializer.Serialize(model.DatesList);
                model.CenterListString = serializer.Serialize(model.CenterList);
                model.ClassRoomListString = serializer.Serialize(model.ClassRoomList);

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(model, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetOffDayValidation(string fromDate, string toDate, string centerId, string classroomArray, string daysOffType, string daysOffId)
        {
            bool isResult = false;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;

                List<ClassRoomModel> classRommArray = serializer.Deserialize<List<ClassRoomModel>>(classroomArray);
                DaysOff daysOff = new FingerprintsModel.DaysOff();
                daysOff.AgencyId = new Guid(Session["AgencyId"].ToString());
                daysOff.CreatedBy = new Guid(Session["UserID"].ToString());
                daysOff.RoleId = new Guid(Session["RoleId"].ToString());
                daysOff.FromDate = fromDate;
                daysOff.ToDate = toDate;
                daysOff.CenterId = Convert.ToInt32(centerId);
                daysOff.RecordType = Convert.ToInt32(daysOffType);
                daysOff.ClassRoomIdArray = classRommArray;
                daysOff.DaysOffID = daysOffId;
                isResult = new CenterData().GetOffDayValidation(daysOff);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter("b65759ba-4813-4906-9a69-e180156e42fc,7c2422ba-7bd4-4278-99af-b694dcab7367,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public ActionResult GetClassroomsByCenterId(string CenterId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtClassRoomsDetails = new DataTable();
                new CenterData().GetClassRoomsByCenterId(ref dtClassRoomsDetails, CenterId, Session["AgencyId"].ToString(), Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtClassRoomsDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }


        //[CustAuthFilter("b65759ba-4813-4906-9a69-e180156e42fc,7c2422ba-7bd4-4278-99af-b694dcab7367,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        //public ActionResult GetClassroomsByCenterId(string CenterId)
        //{
        //    string JSONString = string.Empty;
        //    try
        //    {
        //        DataTable dtClassRoomsDetails = new DataTable();
        //        new CenterData().GetClassRoomsByCenterId(ref dtClassRoomsDetails, CenterId);
        //        JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtClassRoomsDetails);
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //    }
        //    return Json(JSONString);
        //}


        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public ActionResult Dashboard()
        {
            try
            {
                Session["HasHomeBased"] = false;

                StaffDetails details = StaffDetails.GetInstance();
                bool hasHomeBased = false;

                if (Session["Roleid"].ToString().Contains("b4d86d72-0b86-41b2-adc4-5ccce7e9775b"))
                {
                    details.AgencyId = new Guid(Session["AgencyID"].ToString());
                    details.RoleId = new Guid(Session["RoleID"].ToString());
                    details.UserId = new Guid(Session["UserID"].ToString());

                    hasHomeBased = new TeacherData().CheckUserHasHomeBased(details);

                    Session["HasHomeBased"] = hasHomeBased;

                    ViewBag.RoleName = "Center Manager"; ViewBag.ViewType = "Center";
                }
                else if (Session["Roleid"].ToString().Contains("7c2422ba-7bd4-4278-99af-b694dcab7367"))
                {
                    ViewBag.RoleName = "Executive"; ViewBag.ViewType = "Agency";
                }
                else if (Session["Roleid"].ToString().Contains("2af7205e-87b4-4ca7-8ca8-95827c08564c"))
                {
                    ViewBag.RoleName = "Area Manager"; ViewBag.ViewType = "Center";
                }
                ExecutiveDashBoard executive=new ExecutiveDashBoard();
                executive = new ExecutiveData().GetExecutiveDetails(Session["AgencyID"].ToString(), Session["UserID"].ToString(), Session["Roleid"].ToString());
                TempData["CaseNote"] = executive.listCaseNote;

                return View(executive);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }

        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367")]
        public ActionResult GetSlotsDetailByDate(string Date)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtSlotsDetails = new DataTable();
                new ExecutiveData().GetSlotsDetailByDate(ref dtSlotsDetails, Session["AgencyID"].ToString(), Date);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtSlotsDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }


        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public ActionResult GetSeatsDetailByDate(string Date)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtSeatsDetails = new DataTable();
                new ExecutiveData().GetSeatsDetailByDate(ref dtSeatsDetails, Session["AgencyID"].ToString(), Date, Session["Roleid"].ToString(), Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtSeatsDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

      [CustAuthFilter()]
        public ActionResult GetCentersByUserId(string Date)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtSeatsDetails = new DataTable();
                new ExecutiveData().GetSeatsDetailByDate(ref dtSeatsDetails, Session["AgencyID"].ToString(), Date, Session["Roleid"].ToString(), Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtSeatsDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public JsonResult Barchart()
        {
            try
            {
                List<ExecutiveDashBoard.CaseNote> listCaseNote = TempData["CaseNote"] as List<ExecutiveDashBoard.CaseNote>;
                if (listCaseNote != null && listCaseNote.Count == 3)
                {
                    var data = new[] {new { Name = listCaseNote.ElementAt(0).Month, Value = Convert.ToInt32(listCaseNote.ElementAt(0).Percentage) },
                              new { Name = listCaseNote.ElementAt(1).Month, Value = Convert.ToInt32(listCaseNote.ElementAt(1).Percentage) },
                              new { Name = listCaseNote.ElementAt(2).Month, Value = Convert.ToInt32(listCaseNote.ElementAt(2).Percentage)}};
                    TempData.Remove("CaseNote");
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error");
                }
                // returning list from here.
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error");
            }
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetDailyAttendance(string Centerid)
        {
            try
            {
                return Json(new CenterData().GetDailyAttendance(Centerid, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        // shambhu changes 21 Feb
        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80")]

        public ActionResult AgencyDisabilityManagerDashboard(string a)
        {
            try
            {
                TempData["userrole"] = FingerprintsModel.EncryptDecrypt.Encrypt64(Session["Roleid"].ToString());
                int yakkrcount = 0;
                int appointment = 0;
                DisabilityCumulative DisabilityCumulative=new DisabilityCumulative();
                ViewBag.Centerlist = _DisabilityManagerData.GetDissabilityManagerDashboard(ref DisabilityCumulative,ref yakkrcount, ref appointment, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                ViewBag.CumulativeList = DisabilityCumulative;
                Session["Yakkrcount"] = yakkrcount;
                Session["Appointment"] = appointment;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [HttpPost]
        public ActionResult AgencyDisabilityManagerDashboard(HttpPostedFileBase FileUpload1, string ClientId, string classroomid, string ddlDisabilityType, string ddlqualifiedreleased, string DocumentDate, string txtdocdesc)
        {
            try
            {
                string DISABLETYPEID = "";
                if (Request.Form["disabilitytype"] != null)
                {
                    DISABLETYPEID = Request.Form["disabilitytype"].ToString();
                }



                DataTable dt = new DataTable();
                try
                {
                    dt.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("DisableDocument ", typeof(byte)),
                    new DataColumn("DisableDocumentName",typeof(string))

                        });

                    dt.Rows.Add(
                        new BinaryReader(FileUpload1.InputStream).ReadBytes(FileUpload1.ContentLength),
                        FileUpload1.FileName
                  );

                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);

                }
                string result = _DisabilityManagerData.SavePendingDisableUseInfo(ClientId, classroomid, Request.Form["CenterID"].ToString(), "", Session["AgencyID"].ToString(), Session["UserID"].ToString(), Request.Form["Programid"].ToString(), Request.Form["txtappnotes"].ToString(), Request.Form["hdnModeType"].ToString(), dt, DISABLETYPEID, ddlqualifiedreleased, DocumentDate, txtdocdesc);
                TempData["message"] = "Record added successfully.";
                return RedirectToAction("AgencyDisabilityManagerDashboard", "Home");
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [JsonMaxLengthAttribute]
        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public JsonResult Loadallenrolled(string Centerid = "0", string Classroom = "0")
        {
            try
            {
                var list = _DisabilityManagerData.GetAllRoster(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [JsonMaxLengthAttribute]
        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public JsonResult LoadPendingDisableRoster(string Centerid = "0", string Classroom = "0", string Mode = "", string sortOrder = "", string sortDirection = "DESC", string clientId = "0")
        {
            try
            {

                var list = _DisabilityManagerData.GetPendingDisableRoster(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString(), Mode, sortOrder, sortDirection, clientId);
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [JsonMaxLengthAttribute]
        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public JsonResult LoadNotes(string ClientId)
        {
            try
            {
                var list = _DisabilityManagerData.GetDisableNotesList(Session["AgencyID"].ToString(), ClientId);
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [JsonMaxLengthAttribute]
        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public JsonResult LoadDisableDocuments(string ClientId)
        {
            try
            {

                var list = _DisabilityManagerData.GetDisableNotesList(Session["AgencyID"].ToString(), ClientId);
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public JsonResult SavePendingDisableUser(string ClientId, string classroomid, string ddlDisabilityType, string ddlqualifiedreleased, string DocumentDate, string txtdocdesc, string receivedService = "0")
        {
            try
            {
                bool IsCompleted = false;

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("DisableDocument ", typeof(byte[])),
                    new DataColumn("DisableDocumentName",typeof(string))
                          });

                string DISABLETYPEID = "", IsWithdrawn="";
                string result = "";
                string primaryDisabilityType = "";

                if (Request.Form["disabilitytype"] != null)
                {
                    DISABLETYPEID = Request.Form["disabilitytype"].ToString();
                    primaryDisabilityType = Request.Form["primarydisabilitytype"].ToString();
                }
                if (Request.Form["IsCompleted"] != null)
                {
                     IsCompleted = Convert.ToBoolean(Request.Form["IsCompleted"].ToString());
                   
                }
                if (Request.Form["IsWithdrawn"] != null)
                {
                    IsWithdrawn = Convert.ToString(Request.Form["IsWithdrawn"].ToString());

                }
                if (Request.Files.Count > 0)
                {


                    try
                    {

                        foreach (var key in Request.Files)
                        {
                            var file = Request.Files[key.ToString()];

                            dt.Rows.Add(
                                new BinaryReader(file.InputStream).ReadBytes(file.ContentLength),
                                file.FileName
                          );

                        }
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);

                    }

                    result = _DisabilityManagerData.SavePendingDisableUseInfo(ClientId, classroomid, Request.Form["CenterID"].ToString(), "", Session["AgencyID"].ToString(), Session["UserID"].ToString(), Request.Form["Programid"].ToString(), Request.Form["Notes"].ToString(), Request.Form["Mode"].ToString(), dt, DISABLETYPEID, ddlqualifiedreleased, DocumentDate, txtdocdesc,"", primaryDisabilityType, IsCompleted, IsWithdrawn);
                }

                else
                {
                    result = _DisabilityManagerData.SavePendingDisableUseInfo(ClientId, classroomid, Request.Form["CenterID"].ToString(), "", Session["AgencyID"].ToString(), Session["UserID"].ToString(), Request.Form["Programid"].ToString(), Request.Form["Notes"].ToString(), Request.Form["Mode"].ToString(), dt, DISABLETYPEID, ddlqualifiedreleased, DocumentDate, txtdocdesc, receivedService, primaryDisabilityType, IsCompleted, IsWithdrawn);
                }

                return Json(result);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }




        public ActionResult Test()
        {
            return View();
        }




        [CustAuthFilter("9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public ActionResult DisabilityStaffDashboard()
        {
            try
            {
                TempData["userrole"] = FingerprintsModel.EncryptDecrypt.Encrypt64(Session["Roleid"].ToString());
                int yakkrcount = 0;
                int appointment = 0;
                DisabilityCumulative DisabilityCumulative = new DisabilityCumulative();
                ViewBag.Centerlist = _DisabilityManagerData.GetDissabilityStaffDashboard(ref DisabilityCumulative,ref yakkrcount, ref appointment, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                ViewBag.CumulativeList = DisabilityCumulative;
                Session["Yakkrcount"] = yakkrcount;
                Session["Appointment"] = appointment;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }

        [JsonMaxLengthAttribute]
        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public JsonResult BindDisabilityType()
        {
            try
            {
                var list = _DisabilityManagerData.BindDisableType();
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [JsonMaxLengthAttribute]
        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public JsonResult BindDisableTypeByClient(string Clientid)
        {
            try
            {
                var list = _DisabilityManagerData.BindDisableTypeByClient(Clientid);
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [JsonMaxLengthAttribute]
        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public JsonResult SaveDisablityTypes(string ClientId, string DisabilityTypeId,string PrimaryDisablity)
        {
            try
            {
                var list = _DisabilityManagerData.SaveDisabilityTypes(ClientId,DisabilityTypeId,PrimaryDisablity);
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        

        [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        public FileResult getDisableDocument(string id = "0")
        {

            string[] name = id.Split(',');
            DissabilityManagerDashboard image1 = new DisabilityManagerData().getDisableDocument(id);
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

        public ActionResult VolunteerBudget()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveInkind(Inkind inkind)
        {
            bool isResult = false;
            try
            {
                isResult = new ExecutiveData().SaveInkind(inkind, Session["AgencyID"].ToString(), Session["UserID"].ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }

        [HttpPost]
        public JsonResult DeleteInkind(string Id)
        {
            bool isResult = false;
            try
            {

                isResult = new ExecutiveData().DeleteInkind(Id);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }

        public ActionResult GetInkindByUserId()
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtInkind = new DataTable();
                new ExecutiveData().GetInkindDetailsByUserId(ref dtInkind, Session["UserID"].ToString(), Session["AgencyID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtInkind);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]

        public ActionResult AbsenceReason()
        {

            AbsenceReasonModel model = new AbsenceReasonModel();

            try
            {
                List<AbsenceReason> reasonList = new List<FingerprintsModel.AbsenceReason>();
                Guid? agencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                reasonList = reasonList = new TeacherData().GetAbsenceReason(agencyId);
                model.absenceReasonList = reasonList;
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(model);
        }

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]

        public JsonResult SaveAbsenceReason(string reasonId, string reason, bool reasonStatus)
        {
            bool isResult = false;
            List<AbsenceReason> reasonList = new List<FingerprintsModel.AbsenceReason>();
            try
            {
                AbsenceReason model = new FingerprintsModel.AbsenceReason
                {
                    Reason = reason,
                    ReasonId = Convert.ToInt32(reasonId),
                    AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null,
                    CreatedBy = new Guid(Session["UserId"].ToString()),
                    Status = reasonStatus,
                    ModifiedBy = new Guid(Session["UserId"].ToString())
                };

                reasonList = new TeacherData().SaveAbsenceReason(out isResult, model);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { isResult, reasonList }, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult AttendanceType()
        {
            AttendanceTypeModel model = new AttendanceTypeModel();
            try
            {
                model.attendanceTypeList = new List<FingerprintsModel.AttendanceType>();
                Guid? agencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                model.attendanceTypeList = new TeacherData().GetAttendanceType(agencyId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(model);
        }

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult InsertAttendanceType(string attendanceType, string description, string acronym, string attendanceTypeId, string status, string indexId)
        {
            AttendanceTypeModel model = new AttendanceTypeModel();
            AttendanceType _attendType = new FingerprintsModel.AttendanceType();
            bool isResult = false;
            try
            {
                _attendType.Status = Convert.ToBoolean(Convert.ToInt32(status));
                _attendType.IndexId = (indexId == "" || indexId == "0") ? 0 : Convert.ToInt64(indexId);
                _attendType.Acronym = (_attendType.Status) ? acronym.ToUpper() : null;
                _attendType.Description = (_attendType.Status) ? description : null;
                _attendType.AttendanceTypeId = Convert.ToInt64(attendanceTypeId);
                _attendType.HomeBased = Convert.ToBoolean(Convert.ToInt32(attendanceType));
                _attendType.UserId = new Guid(Session["UserId"].ToString());
                _attendType.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;

                model = new TeacherData().InsertAttendanceType(out isResult, _attendType);

            }
            catch (Exception ex)
            {
                bool ids = Convert.ToBoolean(0);
                clsError.WriteException(ex);
            }
            return Json(new { model, isResult }, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult GetAvailableAttendanceType(string homeBased, string description, string acronym, string attendanceTypeId)
        {
            AttendanceType _attendType = new FingerprintsModel.AttendanceType();
            int availCount = 0;
            int result = 0;
            try
            {
                _attendType.HomeBased = Convert.ToBoolean(Convert.ToInt32(homeBased));
                _attendType.Description = description;
                _attendType.Acronym = (string.IsNullOrEmpty(acronym)) ? "" : acronym.ToUpper();
                _attendType.AttendanceTypeId = Convert.ToInt32(attendanceTypeId);
                _attendType.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                _attendType.UserId = new Guid(Session["UserId"].ToString());
                availCount = new TeacherData().GetAvailableAttendanceType(out result, _attendType);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { availCount, result }, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,a31b1716-b042-46b7-acc0-95794e378b26,b65759ba-4813-4906-9a69-e180156e42fc,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult ParentContactInformation()
        {

          


            //ParentInfoModel model = new ParentInfoModel();
            //ParentInfo info = new ParentInfo();
            try
            {
                

                //info.AgencyId = new Guid(Session["AgencyId"].ToString());
                //info.UserId = new Guid(Session["UserID"].ToString());
                //model = new CenterData().ParentContactInfo(info);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            //return View(model);
            return View();
        }


        [CustAuthFilter("b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,a31b1716-b042-46b7-acc0-95794e378b26,b65759ba-4813-4906-9a69-e180156e42fc,a65bb7c2-e320-42a2-aed4-409a321c08a5")]

        public JsonResult GetParentInfoBySearch(long centerId, long classRoomId, long filterType, string searchText = "")
        {
            ParentInfo info = new ParentInfo();
            ParentInfoModel model = new ParentInfoModel();
            try
            {
                info.CenterId = centerId;
                info.ClassRoomId = classRoomId;
                info.SearchText = searchText;
                info.FilterType = filterType;
                info.AgencyId = new Guid(Session["AgencyId"].ToString());
                info.UserId = new Guid(Session["UserID"].ToString());
                info.RoleId = new Guid(Session["RoleId"].ToString());
               // model = new CenterData().GetParentInfoBySearch(info);
               model=new CenterData().ParentContactInfo(info);

            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return Json(model);

        }


        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]
        public ActionResult AgencyFacilitiesManagerDashboard()
        {
            FacilitesModel model = new FacilitesModel();
            StaffDetails details = StaffDetails.GetInstance();
            try
            {

                model = new FacilitiesData().GetFacilitiesModelDashboard(details);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(model);
        }

        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public ActionResult FacilityWorks(int YakkrID = 0)
        {
            AssignFacilityStaff staff = new AssignFacilityStaff();
            try
            {
                TempData["StaffYakkrid"] = YakkrID;
                FacilitiesData fd = new FacilitiesData();
                staff = fd.GetFacilityStaffList(YakkrID, Session["AgencyID"].ToString());
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(staff);
        }

        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public ActionResult SaveFacilityWorks(AssignFacilityStaff AssignWork)
        {
            var result = true;
            try
            {
                string yakkrid;
                if (TempData["StaffYakkrid"] == null)
                {
                    yakkrid = AssignWork.YakkrId;
                }
                else
                {
                    yakkrid = TempData["StaffYakkrid"].ToString();
                }
                FacilitiesData fd = new FacilitiesData();
                AssignFacilityStaff ToAddressDetails = new AssignFacilityStaff();
                ToAddressDetails = fd.AssignToFaciltyStaff(AssignWork, yakkrid, Session["AgencyID"].ToString(), Session["UserID"].ToString());

                if (AssignWork.IsInternal == false)
                {
                    string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                    ToAddressDetails.ExternalEmailId = AssignWork.ExternalEmailId;
                    if (AssignWork.Request == true)
                        SendMail.SendMailForQuotation(Server.MapPath("~/MailTemplate"), AssignWork, ToAddressDetails);
                    else
                        SendMail.SendMailForFacilityIssue(Server.MapPath("~/MailTemplate"), AssignWork, ToAddressDetails);

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public ActionResult FacilityStaffWorks(int YakkrID = 0)
        {
            AssignFacilityStaff staff = new AssignFacilityStaff();
            try
            {
                TempData["StaffYakkrid"] = YakkrID;
                FacilitiesData fd = new FacilitiesData();
                staff = fd.GetFacilityStaffList(YakkrID, Session["AgencyID"].ToString());
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(staff);

        }
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public ActionResult FacilityWorkerDashboard()
        {
            FacilitesModel model = new FacilitesModel();
            StaffDetails staffDetails = StaffDetails.GetInstance();
            try
            {
                model = new FacilitiesData().GetFacilitiesStaffDashboard(staffDetails);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(model);
        }
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public JsonResult AutoCompleteExternalFacility(string term)
        {
            List<AssignFacilityStaff> result = new List<AssignFacilityStaff>();
            try
            {
                result = new FacilitiesData().AutoCompleteExternalFacility(term);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public ActionResult GetWorkOrderStatusList(string Centerid, string Type, bool IsCenterManager = false)
        {
            List<AssignFacilityStaff> stafflist = new List<AssignFacilityStaff>();
            try
            {
                FacilitiesData fd = new FacilitiesData();
                stafflist = fd.GetWorkOrderStatusList(Centerid, Type, Session["AgencyId"].ToString(), Session["UserId"].ToString(), IsCenterManager);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(stafflist);
        }
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public ActionResult GetStaffWorkOrderStatusList(string Centerid, string Type)
        {
            List<AssignFacilityStaff> stafflist = new List<AssignFacilityStaff>();

            try
            {
                FacilitiesData fd = new FacilitiesData();
                stafflist = fd.GetStaffWorkOrderStatusList(Centerid, Type, Session["AgencyId"].ToString(), Session["UserId"].ToString());

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(stafflist);
        }
        [HttpPost]
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public JsonResult SaveFacilityStaffWorks(AssignFacilityStaff AssignWork)
        {
            var result = true;
            try
            {
                string yakkrid;
                if (TempData["StaffYakkrid"] == null)
                {
                    yakkrid = AssignWork.YakkrId;
                }
                else
                {
                    yakkrid = TempData["StaffYakkrid"].ToString();
                }

                FacilitiesData fd = new FacilitiesData();
                AssignFacilityStaff ToAddressDetails = new AssignFacilityStaff();
                ToAddressDetails = fd.SaveFacilityStaff(AssignWork, yakkrid, Session["AgencyID"].ToString(), Session["UserID"].ToString());

            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public JsonResult AutoCompletePartDetails(string term)
        {
            var result = new List<PartDetails>();
            try
            {
                FacilitiesData fd = new FacilitiesData();
                result = fd.AutoCompletePartDetails(term);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public JsonResult UpdateEstimatedTime(string facilityid, string EstimateDate, string EstimatedHours)
        {
            var result = false;
            try
            {
                FacilitiesData fd = new FacilitiesData();
                result = fd.UpdateEstimatedTime(facilityid, EstimateDate, EstimatedHours, Session["UserID"].ToString());

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result);
        }
        [HttpPost]
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public JsonResult UploadFacilityImages(FormCollection fc, List<HttpPostedFileBase> ImageList)
        {
            var result = false;
            try
            {
                if (ImageList != null && ImageList.Count > 0)
                {
                    string yakkrid = fc["YakkrId"].ToString();
                    List<DamageFixedImages> Images = new List<DamageFixedImages>();
                    foreach (var image in ImageList)
                    {
                        if (image != null && image.ContentLength > 0)
                        {
                            DamageFixedImages fixImage = new DamageFixedImages();
                            fixImage.FileName = image.FileName;
                            fixImage.FileExtension = Path.GetExtension(image.FileName);
                            BinaryReader b = new BinaryReader(image.InputStream);
                            fixImage.ImageByte = b.ReadBytes(image.ContentLength);
                            Images.Add(fixImage);
                        }
                    }
                    FacilitiesData fd = new FacilitiesData();
                     result = fd.AddDamageFixedImage(yakkrid, Images, Session["UserID"].ToString());
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public JsonResult LoadCarouselImages(string workid)
        {
            var result =new  List<string>();
            try
            {
                FacilitiesData fd = new FacilitiesData();
                 result = fd.LoadCarouselImages(workid);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter("825f6940-9973-42d2-b821-5b6c7c937bfe,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,cb540cea-154c-482e-82a6-c1e0a189f611")]

        public JsonResult GetWorkOrderDetail(string Yakkrid, string OrderId)
        {
            AssignFacilityStaff result = new AssignFacilityStaff();
            try
            {
                FacilitiesData fd = new FacilitiesData();
                 result = fd.GetWorkOrderDetail(Yakkrid, OrderId);
               
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInternalRefDetails(string ClientId)
        {
           Role result =new Role();
            try
            {
                DisabilityManagerData fd = new DisabilityManagerData();
                 result = fd.GetInternalRefDetails(ClientId);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveInternalReferral(InternalReferral internRef)
        {
            var result = false;
            DisabilityManagerData fd = new DisabilityManagerData();
            try
            {
                string name = "";
                string casenoteid = "";

                List<RosterNew.Attachment> Attachments = new List<RosterNew.Attachment>();
                var ate = Request.Files;
                var ate2 = ate.AllKeys;
                for (int i = 0; i < ate2.Length; i++)
                {
                    RosterNew.Attachment aatt = new RosterNew.Attachment();
                    aatt.file = ate[i];
                    if(aatt.file.ContentLength>0)
                       Attachments.Add(aatt);
                }
                 RosterNew.CaseNote _caseNote = new RosterNew.CaseNote();
                 List<CaseNote> caseNote = new List<CaseNote>();            
                 RosterNew.Users _users = new RosterNew.Users();
                _caseNote.CenterId =EncryptDecrypt.Decrypt64( internRef.CaseCenterId);
                _caseNote.Classroomid = internRef.CaseClassroomId.ToString();
                _caseNote.ClientId = EncryptDecrypt.Decrypt64(internRef.CaseClientId.ToString());             
                _caseNote.CaseNotetags = internRef.Tags.Trim(',');
                _caseNote.CaseNotetitle = internRef.Title;
                _caseNote.CaseNoteDate = internRef.Date;
                _caseNote.Note = internRef.Note;
                _caseNote.ClientIds= string.Join(",", internRef.ClientIds.ToArray());
                _caseNote.ProgramId = EncryptDecrypt.Decrypt64(internRef.CaseProgramId);
                _caseNote.CaseNoteAttachmentList = Attachments;

                casenoteid = new RosterData().SaveCaseNotes(ref name, ref caseNote, ref _users, _caseNote, Attachments, Session["AgencyId"].ToString(), Session["UserId"].ToString(), 2);
                result = fd.SaveInternalReferral(internRef, name);                       
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return RedirectToAction("AgencyDisabilityManagerDashboard");

        }

        #region Absence Report
        //executive,edu Mg, Agency Admin,GenesisEarth Admin,FSW, Center Mg , Area Mg 
        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public ActionResult AbsenceReport() {
            if (agencydata.GetSingleAccessStatus(14))
            {
                return View();
            }
            else {
                return new RedirectResult("~/login/Loginagency");
            }
        }

        //executive,edu Mg, Agency Admin,GenesisEarth Admin,FSW, Center Mg , Area Mg 
        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public JsonResult GetAbsenceReport(int? centerid, int? classid,int? clientid, string search = "")
        {

            var result = new ExecutiveData().GetAbsenceReport(centerid, classid, clientid, search);

            return Json(new { AbsenceReport= result.AbsenceReport, AttendanceIssuePercentage=result.AttendanceIssuePercentage },JsonRequestBehavior.AllowGet);
        }

        //executive,edu Mg, Agency Admin,GenesisEarth Admin,FSW, Center Mg , Area Mg 
        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public JsonResult GetClientByCenterAndClass(int? centerid, int? classid, string search = "")
        {

            var result = new ExecutiveData().GetClientByCenterAndClass(centerid, classid, search);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Absence Report


        #region Get ADA Daily Percentage

        [CustAuthFilter()]
        public JsonResult GetADASeatsDaily()
        {

            string adaPercentage = string.Empty;
            string todaySeats = string.Empty;
            try
            {
                 new ExecutiveData().GetADASeatsDaily(ref adaPercentage,ref todaySeats);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(new { adaPercentage, todaySeats }, JsonRequestBehavior.AllowGet);
        }

        #endregion





    }
}