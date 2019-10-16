using System;
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
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Fingerprints.ViewModel;
using System.Resources;
using System.Globalization;
using System.Collections;
using FingerprintsModel.Enums;
using Fingerprints.Common;

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
        StaffDetails staffDetails = StaffDetails.GetInstance();

        [CustAuthFilter(RoleEnum.SuperAdmin)]
        public ActionResult SuperAdminDashboard()
        {
            ViewBag.superadmindashboard = superAdmin.GetSuperAdmindashboard();
            return View();
        }
        //   [CustAuthFilter("f87b4a71-f0a8-43c3-aea7-267e5e37a59d,a65bb7c2-e320-42a2-aed4-409a321c08a5")]

        [CustAuthFilter(RoleEnum.SuperAdmin,RoleEnum.AgencyAdmin,RoleEnum.GenesisEarthAdministrator)]

        public ActionResult AgencyAdminDashboard(string id = "0")
        {
            try
            {
                if (!id.Equals("0"))
                {

                    Session["AgencyID"] = id;
                    staffDetails = StaffDetails.GetInstance();
                }
                ViewBag.agencyadmindashboard = agencydata.GetagencyAdmindashboard(staffDetails.AgencyId.ToString());
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
        [CustAuthFilter(RoleEnum.HRManager)]
        public ActionResult AgencyHRDashboard()
        {
            try
            {
                ViewBag.agencyhrdashboard = agencydata.GetagencyAdmindashboard(Convert.ToString(staffDetails.AgencyId));
                return View();

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        //Changes on 18Jan2017
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [CustAuthFilter(RoleEnum.SocialServiceManager, RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor,RoleEnum.Executive,RoleEnum.SocialServiceManager,RoleEnum.AreaManager,RoleEnum.CenterManager)]
        public ActionResult AgencystaffDashboard()
        {
            try
            {

                TempData["userrole"] = FingerprintsModel.EncryptDecrypt.Encrypt64(staffDetails.RoleId.ToString());
                var dec = FingerprintsModel.EncryptDecrypt.Decrypt64("ZTRjODBmYzItOGI2NC00NDdhLTk5YjQtOTVkMTUxMGIwMWU5");
                int yakkrcount = 0;
                int appointment = 0;
                string PYSDate = "";
             



                Parallel.Invoke(() =>
                {
                    ViewBag.Centerlist = _family.Getcenters(out PYSDate, ref yakkrcount, ref appointment, staffDetails);

                }
                //,
                //() =>
                //{
                //    var report = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<ScreeningMatrixReport>();
                //    report.CenterID = "0";
                //    report.RequestedPage = 1;
                //    report.PageSize = 10;
                //    report.SkipRows = report.GetSkipRows();
                //    report.SortOrder = "ASC";
                //    report.SortColumn = "Center Name";

                //    ViewBag.ScreeningMatrixReport = Common.FactoryInstance.Instance.CreateInstance<ScreeningData>().GetScreeningMatrixReport(report, staffDetails);
                //}


                );


                Session["Yakkrcount"] = yakkrcount;
                Session["Appointment"] = appointment;
                ViewBag.PYStartDate = PYSDate;

                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter(RoleEnum.HealthNurse)]
        public ActionResult AgencyHealthNurse()
        {
            try
            {
                int yakkrcount = 0;
                ViewBag.Centerlist = _family.GetcentersFSW(ref yakkrcount, Convert.ToString(staffDetails.AgencyId),Convert.ToString(staffDetails.RoleId).ToString(), Convert.ToString(staffDetails.UserId));
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


        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [CustAuthFilter(RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.Executive, RoleEnum.SocialServiceManager, RoleEnum.AreaManager, RoleEnum.CenterManager)]

        public JsonResult GetclientWaitingList(string Centerid, string Option, string Programtype)
        {
            try
            {
                List<ClientWaitingList> _clientWaitingList = new List<ClientWaitingList>();
                Dictionary<string, Int32> slotsDictionary = new Dictionary<string, int>();
                _clientWaitingList = _family.GetclientWaitingList(ref slotsDictionary,staffDetails, Centerid, Option, Programtype);
                return Json(new { _clientWaitingList, slotsDictionary }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred. Please try again.");
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
                _family.GetScreeningStatistics(ref yakkrcount, ref Screeninglist);
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
        [CustAuthFilter(RoleEnum.HomeVisitor)]
        public ActionResult HomeVisitorDashboard()
        {
            try
            {
                int yakkrcount = 0;
                int appointment = 0;
                string PYSDate = "";
                ViewBag.Centerlist = _family.Getcenters(out PYSDate, ref yakkrcount, ref appointment, staffDetails);
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
        // [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [CustAuthFilter(RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.Executive, RoleEnum.SocialServiceManager, RoleEnum.AreaManager, RoleEnum.CenterManager)]

        public JsonResult LoadClientPendinglist(string Centerid, string Type, GridParams Gparam)
        {
            try
            {
                //Pagination page = new Pagination;
                long TotalCount = 0;
                var result = _family.LoadClientPendinglist(Centerid, Type, Convert.ToString(staffDetails.AgencyId),
                    Convert.ToString(staffDetails.UserId), Gparam, ref  TotalCount );

                return new JsonResult { Data = new { data = result, TotalRecords = TotalCount } };
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter(RoleEnum.FamilyServiceWorker,RoleEnum.SocialServiceManager)]
        public JsonResult DeletePendingClient(string Id, string centerid, string Clientid, string householdid, string Programid)
        {
            try
            {
                return Json(_family.DeletePendingClient(Id, centerid, Clientid, householdid, Programid, Convert.ToString(staffDetails.AgencyId),Convert.ToString(staffDetails.UserId)));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        // [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]

        [CustAuthFilter(RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.Executive, RoleEnum.SocialServiceManager, RoleEnum.AreaManager, RoleEnum.CenterManager)]
        public JsonResult GetclientAcceptedList(string Centerid, string Option,GridParams Gparam)
        {
            try
            {
                long total = 0;
                //return Json(_family.GetclientAcceptList(Centerid, Option, Convert.ToString(staffDetails.AgencyId), Convert.ToString(staffDetails.UserId)));
                var result = _family.GetclientAcceptList(Centerid, Option,staffDetails,ref total,Gparam);

                return new JsonResult { Data=new { Data=result, TotalRecord=total }};
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }



        [HttpPost]
        [CustAuthFilter(RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.SocialServiceManager)]
        public JsonResult GetFSWOrHVList(string ClientId, string Centerid, int ListType)
        {
            try
            {
                return Json(_family.GetFSWListByClient(ClientId, Centerid, Convert.ToString(staffDetails.AgencyId), ListType));
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
        //        return Json("Error occurred please try again.");
        //    }
        //}

        
        [CustAuthFilter(RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.SocialServiceManager)]
        public ActionResult DisableYakkr(string cid,string hid)
        {


            var result = _family.DeleteClientYakkr("", cid, hid,staffDetails);

            var houseidencrypt = EncryptDecrypt.Encrypt64(hid);

            return RedirectToAction("FamilyDetails", "AgencyUser", new { id = houseidencrypt });


           }


        [HttpPost]
        [CustAuthFilter(RoleEnum.FamilyServiceWorker,RoleEnum.HomeVisitor,RoleEnum.SocialServiceManager)]
        public JsonResult DeleteRejectedRecord(string Id, string ClientId, string HouseholdId)
        {
            try
            {

                return Json(_family.DeleteClientYakkr(Id, ClientId, HouseholdId,staffDetails),JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }


        [HttpPost]

        [CustAuthFilter(RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.SocialServiceManager)]

        public JsonResult DeletePendingRecord(string Id,string Clientid,string HouseholdId)
        {
            return Json(_family.DeleteClientYakkr(Id, Clientid, HouseholdId,staffDetails),JsonRequestBehavior.AllowGet);

        }

        //[CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
       [CustAuthFilter(RoleEnum.Teacher)]
        public async Task<ActionResult> TeacherDashBoard()
        {

            try
            {
                DataTable Screeninglist = new DataTable();
                Screeninglist=await new TeacherData().TeacherDashboard();
                ViewBag.Screeninglists = Screeninglist;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }


        //[CustAuthFilter("b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        //[CustAuthFilter(new string[] {Role.centerManager }) ]
        [CustAuthFilter()]

        public ActionResult CentralManagerDashboard()
        {

            try
            {
               
                ViewBag.Centerlist = _family.GetApplicationApprovalDashboard();
              
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }



        //public ActionResult AreaManagerDashboard()
        //{
        //    try
        //    {
        //        //string AgencyId = Session["AgencyID"].ToString();
        //        ExecutiveDashBoard executive;
        //        executive = new ExecutiveData().GetExecutiveDetails(staffDetails,"AreaManager");
        //        TempData["CaseNote"] = executive.listCaseNote;
        //        return View(executive);
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //        return View();
        //    }
        //}




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

                StaffDetails staff = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();

             
                ViewBag.CenterId = CenterId;
                List<DailySaftyCheckImages> listImages = new TeacherData().GetDailySaftyCheckImages(staff,CenterId);
                return View(listImages);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }


        [CustAuthFilter(RoleEnum.AreaManager,RoleEnum.CenterManager,RoleEnum.ERSEAManager,RoleEnum.Executive,RoleEnum.SocialServiceManager)]
        public ActionResult CenterClosure()
        {
            return View();
        }

        [CustAuthFilter(RoleEnum.AreaManager, RoleEnum.CenterManager,RoleEnum.GenesisEarthAdministrator,RoleEnum.AgencyAdmin, RoleEnum.ERSEAManager, RoleEnum.Executive, RoleEnum.SocialServiceManager)]
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
        [CustAuthFilter()]
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
        [CustAuthFilter()]
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

        [CustAuthFilter()]
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


        [CustAuthFilter(RoleEnum.AreaManager,RoleEnum.CenterManager,RoleEnum.ERSEAManager,RoleEnum.Executive)]
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


        [CustAuthFilter(RoleEnum.CenterManager,RoleEnum.AreaManager,RoleEnum.Executive,RoleEnum.SocialServiceManager)]
        public ActionResult Dashboard()
        {
            try
            {
                Session["HasHomeBased"] = false;

                StaffDetails details = StaffDetails.GetInstance();
                bool hasHomeBased = false;


                if (staffDetails.RoleId.ToString().ToLowerInvariant() == EnumHelper.GetEnumDescription(RoleEnum.CenterManager).ToLowerInvariant())
                {

                    hasHomeBased = new TeacherData().CheckUserHasHomeBased(details);

                    Session["HasHomeBased"] = hasHomeBased;

                    ViewBag.RoleName = "Center Coordinator/Manager"; ViewBag.ViewType = "Center";
                }
                else if (staffDetails.RoleId.ToString().ToLowerInvariant() == EnumHelper.GetEnumDescription(RoleEnum.Executive).ToLowerInvariant())
                {
                    ViewBag.RoleName = "Executive"; ViewBag.ViewType = "Agency";
                }
                else if (staffDetails.RoleId.ToString().ToLowerInvariant() == EnumHelper.GetEnumDescription(RoleEnum.AreaManager).ToLowerInvariant())
                {
                    ViewBag.RoleName = "Area Manager"; ViewBag.ViewType = "Center";
                }

                else if (staffDetails.RoleId.ToString().ToLowerInvariant() == EnumHelper.GetEnumDescription(RoleEnum.SocialServiceManager).ToLowerInvariant())
                {
                    ViewBag.RoleName = "Social Service Manager"; ViewBag.ViewType = "Agency";
                }

                //ExecutiveDashBoard executive = new ExecutiveDashBoard();
              ExecutiveDashBoard executive = new ExecutiveData().GetExecutiveDetails(staffDetails,staffDetails.RoleId.ToString());


                return View(executive);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }

        [CustAuthFilter(RoleEnum.Executive)]
        public ActionResult GetSlotsDetailByDate(string Date)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtSlotsDetails = new DataTable();
                new ExecutiveData().GetSlotsDetailByDate(ref dtSlotsDetails, Convert.ToString(staffDetails.AgencyId), Date);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtSlotsDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }


        //[CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        [CustAuthFilter(RoleEnum.Executive,RoleEnum.CenterManager,RoleEnum.AreaManager)]
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

                List<ExecutiveDashBoard.CaseNote> listCaseNote = new ExecutiveData().CaseNoteChartData(staffDetails);
                    

             
                if (listCaseNote != null && listCaseNote.Count == 3)
                {
                    var data = new[] {new { Name = listCaseNote.ElementAt(0).Month, Value = Convert.ToDouble(listCaseNote.ElementAt(0).Percentage) },
                              new { Name = listCaseNote.ElementAt(1).Month, Value = Convert.ToDouble(listCaseNote.ElementAt(1).Percentage) },
                              new { Name = listCaseNote.ElementAt(2).Month, Value = Convert.ToDouble(listCaseNote.ElementAt(2).Percentage)}};
                   
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


        [CustAuthFilter(RoleEnum.SocialServiceManager, RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.Executive, RoleEnum.SocialServiceManager, RoleEnum.AreaManager, RoleEnum.CenterManager)]

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
        //[CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]

        [CustAuthFilter(RoleEnum.DisabilitiesManager, RoleEnum.DisabilityStaff)]
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
      //  [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        [CustAuthFilter(RoleEnum.DisabilitiesManager, RoleEnum.DisabilityStaff, RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.Executive, RoleEnum.SocialServiceManager, RoleEnum.AreaManager, RoleEnum.CenterManager)]

      
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
        // [CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        [CustAuthFilter(RoleEnum.DisabilitiesManager, RoleEnum.DisabilityStaff, RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.Executive, RoleEnum.SocialServiceManager, RoleEnum.AreaManager, RoleEnum.CenterManager)]

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
        //[CustAuthFilter("047c02fe-b8f1-4a9b-b01f-539d6a238d80,9c34ec8e-2359-4704-be89-d9f4b7706e82")]
        [CustAuthFilter(RoleEnum.DisabilitiesManager, RoleEnum.DisabilityStaff, RoleEnum.FamilyServiceWorker, RoleEnum.HomeVisitor, RoleEnum.Executive, RoleEnum.SocialServiceManager, RoleEnum.AreaManager, RoleEnum.CenterManager)]

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
        

        [CustAuthFilter(RoleEnum.AreaManager,RoleEnum.CenterManager,RoleEnum.DisabilitiesManager,RoleEnum.DisabilityStaff, RoleEnum.Executive,RoleEnum.SocialServiceManager,RoleEnum.FamilyServiceWorker,RoleEnum.HomeVisitor)]
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

        [CustAuthFilter()]
        [HttpGet]
        public ActionResult VolunteerBudget()
        {
            
          ViewBag.InkindPeriodList=  new InKindData().GetInkindPeriodsDate(staffDetails, staffDetails.AgencyId).Where(x=>!x.IsClosed).ToList();

            return View();
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult SaveInkind(Inkind inkind)
        {
            bool isResult = false;
            try
            {
                isResult = new ExecutiveData().SaveInkind(inkind, staffDetails.AgencyId.ToString(), staffDetails.UserId.ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }

        [HttpPost]
        [CustAuthFilter()]
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

        [CustAuthFilter()]
        public ActionResult GetInkindByUserId(int inkindPeriodId=0)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtInkind = new DataTable();
                new ExecutiveData().GetInkindDetailsByUserId(ref dtInkind, Session["UserID"].ToString(), Session["AgencyID"].ToString(), inkindPeriodId);
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
                reasonList = reasonList = new TeacherData().GetAbsenceReason(agencyId,1);
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

         [CustAuthFilter()]
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


        [CustAuthFilter()]

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

        [CustAuthFilter()]
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
        // [CustAuthFilter()]
        public ActionResult SaveInternalReferral(InternalReferral internalReferral, RosterNew.CaseNote caseNote)
        {
            var result = false;
            DisabilityManagerData fd = new DisabilityManagerData();
            try
            {
                string message = "";
                string casenoteid = "";

                if (caseNote != null)
                {
                    //if (caseNote.CaseNoteAttachmentList != null && caseNote.CaseNoteAttachmentList.Count > 0)
                    //{
                    //    caseNote.CaseNoteAttachmentList.ForEach(x =>
                    //    {
                    //        x.AttachmentFileByte = Convert.FromBase64String(x.AttachmentJson);
                    //    });
                    //}

                    message = new RosterData(staffDetails).SaveCaseNotes(ref casenoteid, caseNote, 2);

                    internalReferral.CaseNoteId = casenoteid;
                }


              


                if (message == "1")
                {
                    result = fd.SaveInternalReferral(internalReferral,staffDetails);
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }




            //if (Role.RolesDictionary[(int)RoleEnum.DisabilitiesManager].ToLowerInvariant() == staffDetails.RoleId.ToString().ToLowerInvariant())
            //{
            //    return RedirectToAction("AgencyDisabilityManagerDashboard");
            //}
            //else if (Role.RolesDictionary[(int)RoleEnum.DisabilityStaff].ToLowerInvariant() == staffDetails.RoleId.ToString().ToLowerInvariant())
            //{
            //    return RedirectToAction("DisabilityStaffDashboard");
            //}
            //else if (Role.RolesDictionary[(int)RoleEnum.Teacher].ToLowerInvariant() == staffDetails.RoleId.ToString().ToLowerInvariant() || Role.RolesDictionary[(int)RoleEnum.TeacherAssistant].ToLowerInvariant() == staffDetails.RoleId.ToString().ToLowerInvariant())
            //{
            //    return RedirectToAction("Roster", "Teacher");
            //}
            //else
            //{
            //    return RedirectToAction("Roster", "Roster");
            //}

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #region Absence Report
        //executive,edu Mg, Agency Admin,GenesisEarth Admin,FSW, Center Mg , Area Mg 
        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public ActionResult AbsenceReport()
        {
            if (agencydata.GetSingleAccessStatus(14))
            {
                return View();
            }
            else
            {
                return new RedirectResult("~/login/Loginagency");
            }
        }

        //executive,edu Mg, Agency Admin,GenesisEarth Admin,FSW, Center Mg , Area Mg 
        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,2af7205e-87b4-4ca7-8ca8-95827c08564c")]
        public JsonResult GetAbsenceReport(int? centerid, int? classid,int? clientid, string search = "")
        {

             var result = new ExecutiveData().GetAbsenceReport(centerid, classid, clientid, search);
           // result.AbsenceReport = new List<ExecutiveDashBoard.AbsenceByWeek>();

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


        //#region Get ADA Daily Percentage

        //[CustAuthFilter()]
        //public JsonResult GetADASeatsDaily()
        //{

        //    string adaPercentage = string.Empty;
        //    string todaySeats = string.Empty;
        //    try
        //    {
        //        new ExecutiveData().GetADASeatsDaily(ref adaPercentage, ref todaySeats,staffDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }

        //    return Json(new { adaPercentage, todaySeats }, JsonRequestBehavior.AllowGet);
        //}

        //#endregion


        public ActionResult SetLanguage(string id = "en", string returnurl = "")
        {
            Session["CurrentCluture"] = id;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(id);

            return Redirect(returnurl);
        }


        public  string GetAllLocalResoure()
        {
            var _jsonStr = "";
            try
            {
                ResourceSet resourceSet = LocalResource.Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

               var dict= resourceSet.Cast<DictionaryEntry>()
                       .ToDictionary(x => x.Key.ToString(),
                                     x => x.Value.ToString());

              //  var dict = JsonConvert.SerializeObject(resourceSet);

                var entries = dict.Select(d =>
        string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));

                _jsonStr =  "{" + string.Join(",", entries) + "}";

                return _jsonStr;
               // return entries.ToString() ;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return _jsonStr;
        }


        [HttpPost]
        [CustAuthFilter()]
        // public JsonResult GetOverIncomeClient(string encCenterId)
        public JsonResult GetOverIncomeClient(string encCenterId, GridParams gparam)
        {
            long  TotalCount = 0;
            List<SelectListItem> parentNameList = new List<SelectListItem>();
            ChildrenInfoClass childInfo= new FamilyData().GetOverIncomeChildrenData(out parentNameList,
                EncryptDecrypt.Decrypt64(encCenterId), gparam, ref TotalCount);

            return Json(new { childInfo, parentNameList,TotalCount }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Inserts the existing data to the table and gets the dashboard by section
        /// </summary>
        /// <param name="sectionType"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter()]
        public JsonResult RefreshExecutiveDashboardBySection(int sectionType)
        {
            if (FingerprintsModel.EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.DashboardSectionType>(sectionType.ToString()) != FingerprintsModel.Enums.DashboardSectionType.CaseNoteAnalysis &&
                FingerprintsModel.EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.DashboardSectionType>(sectionType.ToString()) != FingerprintsModel.Enums.DashboardSectionType.ADA
                )
            {
                new ExecutiveData().RefershExecutiveDashboardBySection(sectionType, staffDetails);
            }

            return Json(new ExecutiveData().GetExecuteDashboardBySection(sectionType, staffDetails), JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        [CustAuthFilter()]
        public JsonResult GetExecutiveDashboardBySection(int sectionType)
        {
            return Json(new ExecutiveData().GetExecuteDashboardBySection(sectionType, staffDetails), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustAuthFilter()]

        public JsonResult SaveADAExplanation(int month,string explanation)
        {
            bool isResult = FactoryInstance.Instance.CreateInstance<ExecutiveData>().SaveADAExplanation(staffDetails,month,explanation);

            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

    }
}