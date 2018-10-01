using Fingerprints.Filters;
using FingerprintsData;
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fingerprints.Controllers
{
    [CustAuthFilter()]
    public class EducationManagerController : Controller
    {
        //
        // GET: /EducationManager/
         
        EducationManagerData EduData = new EducationManagerData();
        public ActionResult EducationManagerDashboard()
        {

            return View();
        }

        public JsonResult GetEducationManagerDashboard()
        {
            EduData = new EducationManagerData();
            EducationManager manager = new EducationManager();
            try
            {
                manager = EduData.GetEducationDashboard();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(manager, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StaffEventCreation(string Eventid = "")
        {
            EduData = new EducationManagerData();
            StaffEventCreation events = new StaffEventCreation();
            try
            {
                events = Eventid == "" ? EduData.GetStaffEventCreation((int)StaffEventListType.Initial, Eventid) : EduData.GetStaffEventCreation((int)StaffEventListType.ByEventId, Eventid);
                events.Heading = Eventid == "" ? "Event Creation" : "Edit Event Details";
                events.IsEditMode = Eventid == "" ? false : true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(events);
        }

        [HttpPost]
        public ActionResult StaffEventInfo(string RSVP, string EventId, string YakkrId)
        {

            EduData = new EducationManagerData();
            bool res = false;
            try
            {
                res = EduData.SaveRSVP(RSVP, EventId, YakkrId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return RedirectToAction("YakkrDetails", "Yakkr");

        }

        public ActionResult StaffEventInfo(string Yakkrid)
        {
            EduData = new EducationManagerData();
            StaffEventCreation events = new StaffEventCreation();
            try
            {
                events = EduData.StaffEventInfo(Yakkrid);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(events);
        }
        [HttpPost]
        public ActionResult StaffEventCreation(StaffEventCreation staffEvent)
        {
            EduData = new EducationManagerData();
            bool res = false;
            try
            {
                if (staffEvent.Eventid != 0)
                {
                    if (staffEvent.InitialEventDate != staffEvent.EventDate && staffEvent.InitialEventTime == staffEvent.StartTime)
                    {
                        staffEvent.EventChangesOn = 1;
                    }
                    else if (staffEvent.InitialEventDate == staffEvent.EventDate && staffEvent.InitialEventTime != staffEvent.StartTime)
                    {
                        staffEvent.EventChangesOn = 2;
                    }
                    else if (staffEvent.InitialEventDate != staffEvent.EventDate && staffEvent.InitialEventTime != staffEvent.StartTime)
                    {
                        staffEvent.EventChangesOn = 3;
                    }
                    else
                    {
                        staffEvent.EventChangesOn = 0;
                    }
                }
                res = EduData.SaveStaffEventDetails(staffEvent);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return RedirectToAction("EventsList");

        }


        public ActionResult EventsList()
        {

            ViewBag.EventType = "future";
            EduData = new EducationManagerData();
            StaffEventCreation evt = new StaffEventCreation();

            try
            {
                evt.events = EduData.GetStaffEventList((int)StaffEventListType.UpcomingEvents);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(evt);


        }


        public ActionResult CancelledEvents()
        {


            EduData = new EducationManagerData();
            StaffEventCreation evt = new StaffEventCreation();
            ViewBag.EventType = "cancel";
            try
            {
                evt.events = EduData.GetStaffEventList((int)StaffEventListType.CancelledEvents);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View("EventsList", evt);


        }

        public ActionResult CompletedEvents()
        {


            EduData = new EducationManagerData();
            StaffEventCreation evt = new StaffEventCreation();
            ViewBag.EventType = "completed";
            try
            {
                evt.events = EduData.GetStaffEventList((int)StaffEventListType.CompletedEvents);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View("EventsList", evt);


        }

        public ActionResult StaffEventReports()

        {


            EduData = new EducationManagerData();
            StaffEventCreation evt = new StaffEventCreation();
            ViewBag.EventType = "completed";
            try
            {
                evt.events = EduData.GetStaffEventList(5);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(evt);


        }
        //public ActionResult GetEventByYakkrid(string YakkrId)
        //{
        //    StaffEventCreation eventdata = new StaffEventCreation();
        //    EduData = new EducationManagerData();
        //    try
        //    {
        //        eventdata = EduData.GetEventByYakkrId(YakkrId);

        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    return Json(eventdata, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetEventListByEventType(string EventType)
        {

            EduData = new EducationManagerData();
            StaffEventCreation evt = new StaffEventCreation();
            try
            {
                evt.events = EventType =="1"?EduData.GetStaffEventList((int)StaffEventListType.OpenEvents): EduData.GetStaffEventList((int)StaffEventListType.CompletedEvents);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(evt.events,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEventReportByID(int eventId,int eventType) {

            EventReportDetails eventdetails = new EventReportDetails();
            EduData = new EducationManagerData();
            try {
                eventdetails = EduData.GetEventReportByID(eventType,eventId, 1);
            } catch (Exception ex) {
                clsError.WriteException(ex);
            }
            return Json(eventdetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsersforMultipleRoles(List<string> RolesList, int Id=0)
        {

            List<UserDetails> userDetails = new List<UserDetails>();
            EduData = new EducationManagerData();
            try
            {
                userDetails = EduData.GetUsersforMultipleRoles(RolesList);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OnSpotEventCheckIn(int EventId) {


            var EventDetails=   EduData.GetEventReportByID(0,EventId,2,"",null);

            return View(EventDetails);
        }

        public Tuple<string, string> GetManagerDetails() {


            StaffDetails staffDetails = StaffDetails.GetInstance();
            string AgencyId = ""; string UserId = "";
            if (staffDetails.AgencyId != null)
            {
                AgencyId = staffDetails.AgencyId.ToString();
                UserId = staffDetails.UserId.ToString();
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
                //  Session["MangAgencyId"] = AgencyId;
                // Session["MangId"] = UserId;
              //  HttpCookie Emailid = new HttpCookie("Emailid");
                //Emailid.Expires = DateTime.Now.AddDays(-1d);

            }
            else { 
            //if (Session["MangAgencyId"] != null && Session["MangId"] != null) {
            //Redirected to 
                AgencyId = Session["MangAgencyId"].ToString();
                UserId = Session["MangId"].ToString();
            }

            var MgDetails = Tuple.Create(AgencyId, UserId);
         

            return MgDetails;

        }

        public JsonResult GetUserListForEventChekin(int eventid,string search) {
            var EventDetails = EduData.GetEventReportByID(0, eventid, 2,search);

            return Json(EventDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StaffEventCheckin(string userid, int eventid) {

            var StaffDetails = EduData.GetEventReportByID(0, eventid, 3,"",userid);

            return View(StaffDetails);
        }

        [HttpPost]
        public ActionResult SubmitEventSignature(string UserId,string Signature, int EventId)
        {
            var userid = new List<string>(); userid.Add(UserId);
            var sign = new List<string>(); sign.Add(Signature);


            EduData.InsertEventCheckIn(EventId, userid, sign);

            return RedirectToAction("OnSpotEventCheckIn", new { EventId = EventId });  
        }

        //void
       [HttpPost]
        public ActionResult OnSpotEventCheckIn(int EventId, List<string> UserId, List<string> Signature)
        {

            //  var EventDetails = EduData.GetEventReportByID(EventId, 2);
           var result= EduData.InsertEventCheckIn(EventId, UserId, Signature);

            return RedirectToAction("EventsList");
        }

        public JsonResult GetUserBasedEventReport() {

            var result = EduData.GetUserBasedEventReport("list",null,null,null,null);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStaffListForEventReport(int? Center, Guid? role)
        {
            var result = EduData.GetUserBasedEventReport("StaffList",Center,role,null,null);

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetStaffsEventReport(List<string> UserIds)
        {
            var result = EduData.GetUserBasedEventReport("StaffReport", null, null, UserIds,null);

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetStaffBasedEventsSummary(Guid? UserId)
        {
            var result = EduData.GetUserBasedEventReport("EventSummary", null, null, null, UserId);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ScreeningAnalysis()
        {

            return View();
        }

        public JsonResult GetScreeningInfoBySearch(long centerId, long classRoomId, long filterType)
        {
            ScreeningAnalysisInfo info = new ScreeningAnalysisInfo();
            ScreeningAnalysisInfoModel model = new ScreeningAnalysisInfoModel();
         //   try
          //  {
                info.CenterId = centerId;
                info.ClassRoomId = classRoomId;
                info.FilterType = filterType;
                info.AgencyId = new Guid(Session["AgencyId"].ToString());
                info.UserId = new Guid(Session["UserID"].ToString());
                info.RoleId = new Guid(Session["RoleId"].ToString());
                // model = new CenterData().GetParentInfoBySearch(info);
                model = new EducationManagerData().ScreeningInfo(info);

          //  }
           // catch (Exception ex)
          //  {

               // clsError.WriteException(ex);
           // }
            return Json(model);

        }

    }
}
