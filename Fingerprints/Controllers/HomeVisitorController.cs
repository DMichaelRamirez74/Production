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
        HomevisitorData hvdata = new HomevisitorData();
        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
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

        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult HomeBasedSocialization()
        {
            try
            {
                ViewBag.HomeBasedlist = _Data.HomeBasedsocialization(Convert.ToString(Session["UserID"]), Convert.ToString(Session["AgencyID"]));
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult Scheduler()
        {

            return View();
        }

        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult getevents()
        {
            try
            {
                List<Scheduler> m = new List<Scheduler>();
                string userid = Session["UserID"].ToString();
                string agencyid = Session["AgencyID"].ToString();
                m = new HomevisitorData().getUserEvents(userid, agencyid);
                // {
                // new Scheduler(){title="All Day Event",start="2016-12-07",allDay=true},
                //new Scheduler(){title="Long Event",start="2017-01-07",end= "2017-01-10"},
                // new Scheduler(){title="Normal Event",start="2017-01-10",end= "2017-01-10"}
                // };
                return Json(m, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }

            //  return Json(m);
        }

        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult getclients()
        {
            List<customclient> list = new List<customclient>();
            try
            {
                string userid = Session["UserID"].ToString();
                string agencyid = Session["AgencyID"].ToString();
                string roleId = Session["RoleId"].ToString();
                DataSet ds = new HomevisitorData().getclients(userid, agencyid, roleId);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            customclient obj = new customclient();
                            obj.clientid = FingerprintsModel.EncryptDecrypt.Encrypt64(item["ClientID"].ToString());
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

        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
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
                string h = new HomevisitorData().saveEvent(_event);
                List<Scheduler> m = new List<Scheduler>();
                string userid = Session["UserID"].ToString();
                string agencyid = Session["AgencyID"].ToString();
                m = new HomevisitorData().getUserEvents(userid, agencyid);
                // {
                // new Scheduler(){title="All Day Event",start="2016-12-07",allDay=true},
                //new Scheduler(){title="Long Event",start="2017-01-07",end= "2017-01-10"},
                // new Scheduler(){title="Normal Event",start="2017-01-10",end= "2017-01-10"}
                // };
                return Json(m, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return Json(result);
        }


        [CustAuthFilter("e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
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
                string h = new HomevisitorData().DeleteEvent(_event);
                List<Scheduler> m = new List<Scheduler>();
                string userid = Session["UserID"].ToString();
                string agencyid = Session["AgencyID"].ToString();
                m = new HomevisitorData().getUserEvents(userid, agencyid);
                // {
                // new Scheduler(){title="All Day Event",start="2016-12-07",allDay=true},
                //new Scheduler(){title="Long Event",start="2017-01-07",end= "2017-01-10"},
                // new Scheduler(){title="Normal Event",start="2017-01-10",end= "2017-01-10"}
                // };
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
                string agencyId = Session["AgencyId"].ToString();
                new HomevisitorData().GetChildDetails(ref childData, agencyId, clientId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return Json(childData);
        }


        public JsonResult UpdateScheduleAppointment(string scheduleString, string meetingStartTime, string meetingEndTime, string meetingDuration)
        {
            bool isResult = false;
            try
            {
                Scheduler scheduler = new FingerprintsModel.Scheduler();
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                scheduler = serializer.Deserialize<Scheduler>(scheduleString);
                scheduler.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(scheduler.Enc_ClientId));
                scheduler.AgencyId = new Guid(Session["AgencyId"].ToString());
                scheduler.StaffId = new Guid(Session["UserId"].ToString());
                isResult = new HomevisitorData().UpdateScheduleAppointment(scheduler, meetingStartTime, meetingEndTime, meetingDuration);

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
                schedular.StaffId = new Guid(Session["UserID"].ToString());
                schedular.AgencyId = new Guid(Session["AgencyID"].ToString());
                isResult = new HomevisitorData().CheckAvailableAppointment(schedular);
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
                schedular.StaffId = new Guid(Session["UserID"].ToString());
                schedular.AgencyId = new Guid(Session["AgencyID"].ToString());

                dates = new HomevisitorData().GetFilteredDates(dates, dates2, schedular);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(dates, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult HomeVisitsHistorical()
        {
            return View();
        }


        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetFamiliesUnderUser(string userId, string roleId)
        {
            List<SelectListItem> familyList = new List<SelectListItem>();
            try
            {
                string agencyId = Session["AgencyId"].ToString();
                familyList = new HomevisitorData().GetFamiliesUnderUserId(userId, agencyId, roleId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(familyList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInitialAppointmentByClientId(string clientId)
        {
            Scheduler schedule = new FingerprintsModel.Scheduler();
            try
            {
                schedule.AgencyId = new Guid(Session["AgencyId"].ToString());
                schedule.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));
                schedule = new HomevisitorData().GetInitialAppointmentByClientId(schedule);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(schedule, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetHomeVisitAttendanceByFromDate(string date, string clientId)
        {
            List<Scheduler> schedularList = new List<FingerprintsModel.Scheduler>();
            try
            {
                Scheduler schedule = new FingerprintsModel.Scheduler();
                schedule.MeetingDate = date;
                schedule.Enc_ClientId = clientId;
                schedule.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));
                schedule.AgencyId = new Guid(Session["AgencyId"].ToString());
                schedule.StaffId = new Guid(Session["UserID"].ToString());
                schedularList = new HomevisitorData().GetHomeVisitAttendanceByFromDate(schedule);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(schedularList, JsonRequestBehavior.AllowGet);

        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult InsertHistoricalHomeVisit(string scheuleString, string id, string homeVisitorId)
        {
            List<Scheduler> schedulerList = new List<FingerprintsModel.Scheduler>();
            bool isResult = false;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                schedulerList = serializer.Deserialize<List<Scheduler>>(scheuleString);
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                Guid userId = new Guid(Session["UserID"].ToString());
                Guid homevisitor = new Guid(homeVisitorId);

                isResult = new HomevisitorData().InsertHistoricalHomeVisit(schedulerList, agencyId, homevisitor, userId, id);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);

        }

    }
    public class customclient
    {
        public string clientid { get; set; }
        public string clientname { get; set; }
    }
}
