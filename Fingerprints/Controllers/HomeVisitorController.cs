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
using System.Web.Script.Serialization;

namespace Fingerprints.Controllers
{
    public class HomeVisitorController : Controller
    {
        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
        role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
        role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
        role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
        role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
        role=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
        */
        agencyData _Data = new agencyData();
        HomevisitorData homeVisitorData = new HomevisitorData();
     
        [CustAuthFilter(RoleEnum.HomeVisitor )]
        public ActionResult Classrooms()
        {
            try
            {

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return View();
        }

        [CustAuthFilter(RoleEnum.HomeVisitor)]
        public ActionResult HomeBasedSocialization()
        {
            try
            {
                ViewBag.HomeBasedlist = new agencyData().HomeBasedsocialization(Convert.ToString(Session["UserID"]), Convert.ToString(Session["AgencyID"]));
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }

        [CustAuthFilter(RoleEnum.Teacher,RoleEnum.FamilyServiceWorker,RoleEnum.HomeVisitor)]
        public ActionResult Scheduler()
        {

            return View();
        }

        [CustAuthFilter(RoleEnum.HomeVisitor, RoleEnum.Teacher, RoleEnum.FamilyServiceWorker, RoleEnum.HealthNurse, RoleEnum.TeacherAssistant,
           RoleEnum.MentalHealthSpecialist)]
        public JsonResult getevents()
        {
            try
            {
                List<Scheduler> m = new List<Scheduler>();
                m = homeVisitorData.getUserEvents();
               
                return Json(m, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }

            //  return Json(m);
        }

        [CustAuthFilter(RoleEnum.HomeVisitor, RoleEnum.Teacher, RoleEnum.FamilyServiceWorker, RoleEnum.HealthNurse, RoleEnum.TeacherAssistant,
           RoleEnum.MentalHealthSpecialist)]
        public JsonResult getclients(int mode=1)
        {
            List<customclient> list = new List<customclient>();
            try
            {
                
                DataSet ds = homeVisitorData.getclients(mode);

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            customclient obj = new customclient();
                            if (mode == 1)
                            {
                                obj.clientid = FingerprintsModel.EncryptDecrypt.Encrypt64(item["ClientID"].ToString());
                            }
                            else {
                                obj.clientid = item["ClientID"].ToString();
                            }
                            obj.clientname = item["fullname"].ToString();
                            list.Add(obj);

                        }
                    }
                }
                return Json(new { list });
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json("");
            }

        }

        [CustAuthFilter(RoleEnum.HomeVisitor, RoleEnum.Teacher, RoleEnum.FamilyServiceWorker, RoleEnum.HealthNurse, RoleEnum.TeacherAssistant,
          RoleEnum.MentalHealthSpecialist)]
        public JsonResult saveEvent(Scheduler _event, FormCollection collection, Recurrence recurrence)
        {
            string result = string.Empty;
            try
            {
                _event.Recurrence = recurrence;
                _event.ClientId = Convert.ToInt64(FingerprintsModel.EncryptDecrypt.Decrypt64(_event.ClientName));
                _event.ClientName = "";
                _event.StaffId = new Guid(Session["UserId"].ToString());
                _event.AgencyId = new Guid(Session["AgencyID"].ToString());
                _event.MeetingDescription = _event.title;
                string h = homeVisitorData.saveEvent(_event);
                List<Scheduler> m = new List<Scheduler>();
                m = homeVisitorData.getUserEvents();
                return Json(m, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return Json(result);
        }


        [CustAuthFilter(RoleEnum.HomeVisitor, RoleEnum.Teacher, RoleEnum.FamilyServiceWorker, RoleEnum.HealthNurse, RoleEnum.TeacherAssistant,
          RoleEnum.MentalHealthSpecialist)]
        public ActionResult Delete(Scheduler _event)
        {
            string result = string.Empty;
            try
            {

                _event.ClientId = Convert.ToInt64(FingerprintsModel.EncryptDecrypt.Decrypt64(_event.ClientName));
                _event.ClientName = "";
                _event.StaffId = new Guid(Session["UserId"].ToString());
                _event.AgencyId = new Guid(Session["AgencyID"].ToString());
                _event.MeetingDescription = _event.title;
                string h = homeVisitorData.DeleteEvent(_event);
                List<Scheduler> m = new List<Scheduler>();
              
                m = homeVisitorData.getUserEvents();
                return Json(m, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return Json(result);
        }

        public JsonResult GetChildDetails(string clientId)
        {
            DataSet childData = new DataSet();

            try
            {

                homeVisitorData.GetChildDetails(ref childData, clientId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return Json(childData);
        }

        [CustAuthFilter()]
        public JsonResult UpdateScheduleAppointment(string scheduleString, string meetingStartTime, string meetingEndTime, string meetingDuration)
        {
            bool isResult = false;
            try
            {
                Scheduler scheduler = new FingerprintsModel.Scheduler();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                scheduler = serializer.Deserialize<Scheduler>(scheduleString);
                scheduler.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(scheduler.Enc_ClientId));
                isResult = homeVisitorData.UpdateScheduleAppointment(scheduler, meetingStartTime, meetingEndTime, meetingDuration);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult);
        }


        public JsonResult CheckAvailableAppointment(string startTime, string endTime, string meetingDate)
        {

            bool isResult = false;
            try
            {







                Scheduler schedular = new FingerprintsModel.Scheduler();
                schedular.StartTime = startTime;
                schedular.EndTime = endTime;
                schedular.MeetingDate = meetingDate;
                isResult = homeVisitorData.CheckAvailableAppointment(schedular);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFilteredDates(string dateString, string listString, string clientId)
        {

            List<string> dates = new List<string>();
            List<string> dates2 = new List<string>();
            try
            {

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dates = serializer.Deserialize<List<string>>(dateString);
                dates2 = serializer.Deserialize<List<string>>(listString);
                Scheduler schedular = new FingerprintsModel.Scheduler();
                schedular.ClientId = (clientId == null || clientId == "") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));
                dates = homeVisitorData.GetFilteredDates(dates, dates2, schedular);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(dates, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter()]
        public ActionResult HomeVisitsHistorical()
        {
            return View();
        }


       [CustAuthFilter()]
        public JsonResult GetFamiliesUnderUser(string userId, string roleId)
        {
            List<SelectListItem> familyList = new List<SelectListItem>();
            try
            {
            
                familyList = homeVisitorData.GetFamiliesUnderUserId(userId, roleId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(familyList, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public JsonResult GetInitialAppointmentByClientId(string clientId)
        {
            Scheduler schedule = new FingerprintsModel.Scheduler();
            List<Scheduler> scheduleList = new List<FingerprintsModel.Scheduler>();
            try
            {
                schedule.AgencyId = new Guid(Session["AgencyId"].ToString());
                schedule.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));
                schedule.MeetingDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
                schedule.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("MM/dd/yyyy");
                schedule = homeVisitorData.GetInitialAppointmentByClientId(ref scheduleList, schedule);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { schedule, scheduleList }, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter()]
        public JsonResult GetHomeVisitAttendanceByFromDate(string meetingStartdate, string meetingEndDate, string clientId)
        {
            List<Scheduler> schedularList = new List<FingerprintsModel.Scheduler>();
            try
            {
                Scheduler schedule = new FingerprintsModel.Scheduler();
                schedule.MeetingDate = meetingStartdate;
                schedule.EndDate = meetingEndDate;
                schedule.Enc_ClientId = clientId;
                schedule.ClientId = (clientId == "" || clientId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));
                schedule.AgencyId = new Guid(Session["AgencyId"].ToString());
                schedule.StaffId = new Guid(Session["UserID"].ToString());
                schedularList = homeVisitorData.GetHomeVisitAttendanceByFromDate(schedule);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(schedularList, JsonRequestBehavior.AllowGet);

        }

        [CustAuthFilter()]
        public JsonResult InsertHistoricalHomeVisit(string scheuleString, string homeVisitorId)
        {
            List<Scheduler> schedulerList = new List<FingerprintsModel.Scheduler>();
            bool isResult = false;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                schedulerList = serializer.Deserialize<List<Scheduler>>(scheuleString);
                Guid homevisitor = new Guid(homeVisitorId);
                isResult = homeVisitorData.InsertHistoricalHomeVisit(schedulerList, homevisitor);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);

        }

        #region TCR&FSW Scheduler

        [CustAuthFilter(RoleEnum.HomeVisitor, RoleEnum.Teacher, RoleEnum.FamilyServiceWorker, RoleEnum.HealthNurse, RoleEnum.TeacherAssistant,
          RoleEnum.MentalHealthSpecialist)]
        public ActionResult TeacherScheduler(string clientid,long yakkr,long Yakkrid) {

         var result= homeVisitorData.getVisitingDetailsByclient(clientid,yakkr, Yakkrid, 1);
            ViewBag.Details = result;
            ViewBag.YakkrId = Yakkrid;
            ViewBag.YakkrCode = yakkr;
            ViewBag.IsAllCSH = false; //hide client dropdown 
            return View();
        }

        [CustAuthFilter(RoleEnum.HomeVisitor, RoleEnum.Teacher, RoleEnum.FamilyServiceWorker, RoleEnum.HealthNurse, RoleEnum.TeacherAssistant,
          RoleEnum.MentalHealthSpecialist)]
        public ActionResult TCRScheduler()
        {
            ViewBag.Details = new { };
            ViewBag.YakkrId = 0;
            ViewBag.YakkrCode = 0;
            ViewBag.IsAllCSH = true; //show client dropdown

            return View("TeacherScheduler");
        }

       // [CustAuthFilter(new string[] { Role.teacher, Role.familyServiceWorker })]
        public JsonResult UpdateVisitingDetails(List<VisitDetail> data, string agencyid,int type)
        {
            var result = homeVisitorData.UpdateVisitingDetails(data, agencyid,type);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [CustAuthFilter(RoleEnum.HomeVisitor, RoleEnum.Teacher, RoleEnum.FamilyServiceWorker, RoleEnum.HealthNurse, RoleEnum.TeacherAssistant,
           RoleEnum.MentalHealthSpecialist)]
        public ActionResult SaveSchedulerEvent(Scheduler _event,int yakkrid=0)
        {
            List<Scheduler> m = new List<Scheduler>();
            bool YakkrRemoved = false;
            try
            {
                var StfInfo = StaffDetails.GetInstance();
                _event.StaffId = new Guid(Session["UserId"].ToString());
                _event.AgencyId = new Guid(Session["AgencyID"].ToString());
                _event.MeetingDescription = _event.title;

                string h = homeVisitorData.saveEvent(_event);
              
                m = homeVisitorData.getUserEvents();


                if (yakkrid > 0 && _event.ClientId > 0 && _event.MeetingId ==0) { 
                  YakkrRemoved=  homeVisitorData.ClearYakkr(yakkrid,_event.ClientId);
                }

                // return Json(m, JsonRequestBehavior.AllowGet);
                return Json(new { Events = m,YakkrRemoved=YakkrRemoved }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex) {
                clsError.WriteException(ex);
            }
            return Json(m, JsonRequestBehavior.AllowGet);
        }

        #endregion TCR&FSW Scheduler


    }
    public class customclient
    {
        public string clientid { get; set; }
        public string clientname { get; set; }
    }
}
