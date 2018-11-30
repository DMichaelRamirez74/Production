using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using System.IO;
using Fingerprints.CustomClasses;
using System.Data;
using System.Web.Helpers;
using System.Web.Script.Serialization;

namespace Fingerprints.Controllers
{
    public class TeacherController : Controller
    {
        /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
       roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
       roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
       roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
       roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
       roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
       roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacher)
       roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
       roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
       roleid=7c2422ba-7bd4-4278-99af-b694dcab7367(Executive)
       */
        FamilyData _family = new FamilyData();
        TeacherData _Teacher = new TeacherData();
        string available = "3";
        [JsonMaxLengthAttribute]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public JsonResult Loadallcenterscreening(string Centerid = "0")
        {
            try
            {
                return Json(_Teacher.Getchildscreeningcenter(Centerid, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [JsonMaxLengthAttribute]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public JsonResult Loaddeclinedscreening(string Centerid = "0")
        {
            try
            {
                return Json(_Teacher.GetteacherDeclinedScreenings(Centerid, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public JsonResult Loadchildmissingscreening(string ClassRoom, string Centerid = "0")
        {
            try
            {
                return Json(_Teacher.Getallchildmissingscreening(Centerid, ClassRoom, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter(RoleEnum.Teacher,RoleEnum.TeacherAssistant)]
        public ActionResult DownloadScreeningMatrixExcel(string Centerid, string Classroom = "")
        {
            try
            {
                Export export = new Export();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Screening Status Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                MemoryStream ms = export.ExportExcelScreeningMatrix(_Teacher.Getallchildmissingscreening(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
                ms.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }


        /// <summary>
        /// Teacher code by shelly
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,4b77aab6-eed1-4ac3-b498-f3e80cf129c0")]
        //[CustAuthFilter(new string[] { Role.teacher,Role.educationManager})]

        [CustAuthFilter(RoleEnum.Teacher,RoleEnum.TeacherAssistant)]
        public ActionResult Roster(string id="")
        {
            try
            {
                int res = 0;
                bool notChecked = false;
                ViewBag.NotChecked = false;
                if(int.TryParse(id,out res) && res==1)
                {
                    ViewBag.NotChecked = true;
                    notChecked = true;
                }
                return View(new TeacherData().GetChildList(notChecked));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        //[CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        [CustAuthFilter(RoleEnum.Teacher)]
        public ActionResult GetChildDevelopmentTeamByChildId(string ClientId, string CenterId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtDevelopmentTeamDetails = new DataTable();
                new TeacherData().GetChildDevelopmentTeamByChildId(ref dtDevelopmentTeamDetails, ClientId, CenterId, Session["UserID"].ToString(), Session["AgencyID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtDevelopmentTeamDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        //[CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        [CustAuthFilter(RoleEnum.Teacher)]
        public ActionResult AddChildReferealNotes(List<YakkrRouting> yakkrdetails)
        {
            bool Result = false;
            try
            {
                if (yakkrdetails != null)
                {
                    foreach (YakkrRouting objYakkrRouting in yakkrdetails)
                    {
                        if (Session["UserID"] != null)
                            objYakkrRouting.UserID = new Guid(Session["UserID"].ToString());
                        if (Session["AgencyID"] != null)
                            objYakkrRouting.AgencyID = new Guid(Session["AgencyID"].ToString());
                        bool isAffected = _Teacher.ADDChildReferralNotes(objYakkrRouting);
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }
        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult Roster(FormCollection collection)
        {
            try
            {
                int reasonid = 0;string NewReason = "";
                string childID = collection.Get("childid");
                string absentType = collection.Get("AbsentType");
                string Cnotes = "";//= collection.Get("CNotes");
                if (!string.IsNullOrEmpty(collection.Get("txtNewReason")))
                     NewReason = collection.Get("txtNewReason");
                if (!string.IsNullOrEmpty(collection.Get("ReasonList")))
                   reasonid = Convert.ToInt32(collection.Get("ReasonList"));
                string result = "";
                new TeacherData().MarkAbsent(ref result, childID, Session["UserID"].ToString(), absentType, Cnotes, Session["AgencyID"].ToString(), reasonid, NewReason);
                if (result == "1")
                    TempData["message"] = "Record saved successfully.";
                return View(new TeacherData().GetChildList());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [HttpGet]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult ParentCheckIn_CheckOut(int available)
        {
            try
            {
                ViewData["Available"] = available;
                return View(new TeacherData().GetChildList());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [HttpGet]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult Meals()
        {
            try
            {
                return View(new TeacherData().GetMeals(Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult Meals(FormCollection collection)
        {
            try
            {
                string result = "";

                TeacherModel _teacher = new TeacherData().GetMeals(ref result, Session["UserID"].ToString(), Session["AgencyID"].ToString(), collection);
                if (result == "1")
                    TempData["message"] = "Record saved successfully.";
                return View(_teacher);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult CheckIn(FormCollection collection)
        {
            try
            {
                int reasonid = 0;
                string childID = collection.Get("childid");
                string absentType = collection.Get("AbsentType");
                string Cnotes = collection.Get("CNotes");
                string NewReason = collection.Get("NewReason");
                string result = "";
                if (!string.IsNullOrEmpty(collection.Get("ReasonList")))
                    reasonid = Convert.ToInt32(collection.Get("ReasonList"));
                //string result = "";
                TeacherModel _teacher = new TeacherData().MarkAbsent(ref result, childID, Session["UserID"].ToString(), absentType, Cnotes, Session["AgencyID"].ToString(), reasonid, NewReason);
                if (result == "1")
                    TempData["message"] = "Record saved successfully.";

                return Redirect("~/Teacher/Roster");
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        [HttpGet]
        public ActionResult ParentCheckIn(string clientid, int accesstype, string available)
        {
            try
            {
                ViewData["ActiveTabTeacher"] = 0;
                string result = "";
                return View(new TeacherData().GetParentList(ref result, clientid, 1, Session["UserID"].ToString(), Session["AgencyID"].ToString(), available));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        [HttpGet]
        public ActionResult CheckInCheckOutDetail(string clientid, int accesstype)
        {
            try
            {
                return View(new TeacherData().GetMainChildDisplay(clientid, 1, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        [HttpGet]
        public ActionResult ParentCheckOut(string clientid, int accesstype)
        {
            try
            {
                string result = "";
                return View(new TeacherData().GetParentList(ref result, clientid, 1, Session["UserID"].ToString(), Session["AgencyID"].ToString(), "0"));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [HttpGet]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult CheckOut()
        {
            try
            {
                return View(new TeacherData().GetChildList());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult ParentCheckIn(string clientid, FormCollection collection)
        {

            try
            {
                TeacherModel _teach = new TeacherModel();
                string available = collection.Get("Available");
                string Oavailable = collection.Get("OfficeAvailable");

                string result = "";
                if (available == "1" && Oavailable == "0")
                {
                    ViewData["ActiveTabTeacher"] = 1;
                     _teach = new TeacherData().GetParentList(ref result, clientid, Session["UserID"].ToString(), collection, 1, Session["AgencyID"].ToString());
                    if (result == "1")
                        TempData["message"] = "Record saved successfully.";
                    return View(_teach);

                }
                else if (available == "2")
                {
                     _teach = new TeacherData().GetParentList(ref result, clientid, Session["UserID"].ToString(), collection, 1, Session["AgencyID"].ToString());
                    if (result == "1")
                        TempData["message"] = "Record saved successfully.";
                    return Redirect("Roster");

                }

                else
                {
                     _teach = new TeacherData().GetParentList(ref result, clientid, Session["UserID"].ToString(), collection, 1, Session["AgencyID"].ToString());
                    if (result == "1")
                        TempData["message"] = "Record saved successfully.";
                    return Redirect("~/Teacher/ParentCheckIn_CheckOut?available=" + available + "");
                }

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }


        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult ParentCheckOut(string clientid, FormCollection collection)
        {
            try
            {
                string result = "";
                new TeacherData().GetParentList(ref result, clientid, Session["UserID"].ToString(), collection, 2, Session["AgencyID"].ToString());
                return Redirect("ParentCheckIn_CheckOut?available=" + available + "");

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }


        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult DomainObservationResults(string ChildId, string ChildName)
        {
            DomainObservationResults results = new DomainObservationResults();
            try
            {
                if (ChildId != null)
                    results = new TeacherData().GetDomainObservationResults(new Guid(Session["UserID"].ToString()), Convert.ToInt64(EncryptDecrypt.Decrypt64(ChildId)));
                else
                    results = new TeacherData().GetDomainObservationResults(new Guid(Session["UserID"].ToString()), null);
                if (!string.IsNullOrEmpty(ChildId))
                {
                    results.ChildId = !string.IsNullOrEmpty(EncryptDecrypt.Decrypt64(ChildId)) ? Convert.ToInt64(EncryptDecrypt.Decrypt64(ChildId)) : 0;
                    results.ChildName = !string.IsNullOrEmpty(EncryptDecrypt.Decrypt64(ChildName)) ? EncryptDecrypt.Decrypt64(ChildName) : "";
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(results);
        }

        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult GetAttachmentByNoteId(string NoteId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtAttachments = new DataTable();
                new TeacherData().GetAttachmentByNoteId(ref dtAttachments, NoteId, Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtAttachments);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }


        [JsonMaxLengthAttribute]
        [CustAuthFilter("7c2422ba-7bd4-4278-99af-b694dcab7367")]
        public JsonResult Loadallcenterscreeningexe(string Centerid = "0")
        {
            try
            {
                return Json(_Teacher.Getchildscreeningcenterexecutive(Centerid, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        //Task
        //[HttpGet]
        //[CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        //public ActionResult DailySafetyCheck()
        //{
        //    try
        //    {
        //        Guid? userId = null;
        //        if (Session["UserID"] != null)
        //            userId = new Guid(Session["UserID"].ToString());
        //        List<DailySaftyCheckImages> listImages = _Teacher.GetDailySaftyCheckImages(userId, Session["Roleid"].ToString());
        //        return View(listImages);
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //        return View();
        //    }
        //}

        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult InsertMonitoring(List<Monitoring> monitor, string Message, bool? isClosed, string CenterId)
        {
            bool Result = false;
            try
            {
                
                if (monitor != null)
                {
                    foreach (Monitoring obj_Monitor in monitor)
                    {
                        if (Session["UserID"] != null)
                            obj_Monitor.UserID = new Guid(Session["UserID"].ToString());
                        if (Session["AgencyID"] != null)
                            obj_Monitor.AgencyID = new Guid(Session["AgencyID"].ToString());
                        Guid? MonitorId = _Teacher.InsertMonitoringDetail(obj_Monitor);
                        if (MonitorId != null)
                        {
                            obj_Monitor.Id = MonitorId;
                            Result = true;
                        }
                        if (!obj_Monitor.PassFailCode && Result)
                        {
                            Result = _Teacher.InsertWorkOrderDetail(obj_Monitor);
                        }
                    }

                    isClosed = (isClosed == null) ? false : isClosed;
                    if (isClosed != null)
                    {
                        string Notes = Message;
                        if (Session["Roleid"].ToString().Contains("82b862e6-1a0f-46d2-aad4-34f89f72369a"))
                        {

                       
                            Guid userid = new Guid(Session["UserID"].ToString());
                            new TeacherData().AddDailySafetyCheckOpenCloseRequest(Notes, Convert.ToBoolean(isClosed), false, true, userid, monitor.ElementAt(0), null);
                        }
                        else
                        {
                            Guid userId = new Guid(Session["UserID"].ToString());
                            new TeacherData().AddDailySafetyCheckOpenCloseRequest(Notes, Convert.ToBoolean(isClosed), true, false,userId, monitor.ElementAt(0), monitor.ElementAt(0).CenterId.ToString());
                        }
                    }
                    else
                    {
                        if (Session["Roleid"].ToString().Contains("82b862e6-1a0f-46d2-aad4-34f89f72369a"))
                        {
                            Guid userId = new Guid(Session["UserID"].ToString());
                            new TeacherData().DeleteDailySafetyCheckOpenCloseRequest(userId, false, monitor.ElementAt(0));
                        }
                        else
                        {
                            Guid userId = new Guid(Session["UserID"].ToString());
                            new TeacherData().DeleteDailySafetyCheckOpenCloseRequest(userId, true, monitor.ElementAt(0));

                        }
                    }


                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }


        //public ActionResult CloseEntireCenter(string CenterId, string daysOffString, string yakkrArray, bool isStaff, bool? isClosed = true)
        //{
        //    bool Result = false;

        //    try
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        string[] yakkrStringArray = serializer.Deserialize<string[]>(yakkrArray);

        //        Monitoring monitor = new Monitoring();
        //        if (Session["UserID"] != null)
        //            monitor.UserID = new Guid(Session["UserID"].ToString());
        //        if (Session["AgencyID"] != null)
        //            monitor.AgencyID = new Guid(Session["AgencyID"].ToString());
        //        monitor.CenterId = Convert.ToInt64(CenterId);

        //        DaysOffModel model = new DaysOffModel();
        //        DaysOff daysOff = serializer.Deserialize<DaysOff>(daysOffString);

        //        daysOff.AgencyId = new Guid(Session["AgencyId"].ToString());
        //        daysOff.CreatedBy = new Guid(Session["UserID"].ToString());
        //        daysOff.RoleId = new Guid(Session["RoleId"].ToString());


        //        if (yakkrStringArray.Count() > 0)
        //        {
        //            foreach (string yakkr in yakkrStringArray)
        //            {
        //                Result = new TeacherData().AcceptRejectRequest(yakkr, monitor.UserID, monitor.AgencyID);

        //            }
        //        }
        //        List<Tuple<bool, string, string, string, long, string,string>> tupleEmail = new List<Tuple<bool, string, string, string, long, string,string>>();

        //        string Reason = daysOff.OffDayComments;

        //        bool StaffCome = daysOff.IsStaff;

        //        model = new CenterData().InsertDaysOff(daysOff);

        //        tupleEmail = new CenterData().GetParentAndManagementEmail(new Guid(Session["UserID"].ToString()), "2", isStaff, Convert.ToInt64(CenterId), 0);

        //        if (tupleEmail.Count() > 0)
        //        {
        //            string path = "";

        //            foreach (var tuple in tupleEmail)
        //            {
        //                using (var sr = new StreamReader(Server.MapPath("~/MailTemplate/ClassroomClosed.html")))
        //                {
        //                    path = sr.ReadToEnd();
        //                    if (tuple.Item1)
        //                    {
        //                        path = path.Replace("{Name}", tuple.Item2).Replace("{reason}", Reason).Replace("{reasonBody}", "all classes for the center: " + tuple.Item5 + " are closed today.").Replace("{reportbody}", "");
        //                    }
        //                    else
        //                    {
        //                        path = path.Replace("{Name}", tuple.Item2).Replace("{reason}", Reason).Replace("{reasonBody}", "all classes for the center: " + tuple.Item5 + " are closed today.").Replace("{reportbody}", "Do not report to office today.");

        //                    }
        //                    SendMail.SendEmailToParentAndTeacher(tuple.Item3, path, Session["EmailID"].ToString());
        //                   // for send sms to the parents and teacher 
        //                    if (Convert.ToString(tuple.Item7)!="0")
        //                    {
        //                        SendSMSToParentTeacher(tuple, "");
        //                    } 
        //                }
        //            }
        //        }



        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //    }
        //    return Json(Result);
        //}


        public ActionResult CloseEntireCenter(string CenterId, string daysOffString, string yakkrArray, bool isStaff, bool? isClosed = true)
        {
            bool Result = false;

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string[] yakkrStringArray = serializer.Deserialize<string[]>(yakkrArray);

                Monitoring monitor = new Monitoring();
                if (Session["UserID"] != null)
                    monitor.UserID = new Guid(Session["UserID"].ToString());
                if (Session["AgencyID"] != null)
                    monitor.AgencyID = new Guid(Session["AgencyID"].ToString());
                monitor.CenterId = Convert.ToInt64(CenterId);

                DaysOffModel model = new DaysOffModel();
                DaysOff daysOff = serializer.Deserialize<DaysOff>(daysOffString);

                daysOff.AgencyId = new Guid(Session["AgencyId"].ToString());
                daysOff.CreatedBy = new Guid(Session["UserID"].ToString());
                daysOff.RoleId = new Guid(Session["RoleId"].ToString());


                if (yakkrStringArray.Count() > 0)
                {
                    foreach (string yakkr in yakkrStringArray)
                    {
                        Result = new TeacherData().AcceptRejectRequest(yakkr, monitor.UserID, monitor.AgencyID);

                    }
                }
                List<Tuple<bool, string, string, string, long, string,string>> tupleEmail = new List<Tuple<bool, string, string, string, long, string,string>>();

                string Reason = daysOff.OffDayComments;

                bool StaffCome = daysOff.IsStaff;

                model = new CenterData().InsertDaysOff(daysOff);

                tupleEmail = new CenterData().GetParentAndManagementEmail(new Guid(Session["UserID"].ToString()), "2", isStaff, Convert.ToInt64(CenterId), 0);

                if (tupleEmail.Count() > 0)
                {
                    string path = "";

                    foreach (var tuple in tupleEmail)
                    {
                        using (var sr = new StreamReader(Server.MapPath("~/MailTemplate/ClassroomClosed.html")))
                        {
                            path = sr.ReadToEnd();
                            if (tuple.Item1)
                            {
                                path = path.Replace("{Name}", tuple.Item2).Replace("{reason}", Reason).Replace("{reasonBody}", "all classes for the center: " + tuple.Item5 + " are closed today.").Replace("{reportbody}", "");
                            }
                            else
                            {
                                path = path.Replace("{Name}", tuple.Item2).Replace("{reason}", Reason).Replace("{reasonBody}", "all classes for the center: " + tuple.Item5 + " are closed today.").Replace("{reportbody}", "Do not report to office today.");

                            }
                          SendMail.SendEmailToParentAndTeacher(tuple.Item3, path, Session["EmailID"].ToString());


                            // for send sms to the parents and teacher 
                            //if (Convert.ToString(tuple.Item7)!="0")
                            //{
                            //    SendSMSToParentTeacher(tuple, "");
                            //}
                        }
                    }
                }


            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }

        //public void SendSMSToParentTeacher(Tuple<bool, string, string, string, long, string, string> tupleSMS,String Reason)
        //{
        //     string accountSid = Convert.ToString(ConfigurationManager.AppSettings["TwilioAccountId"]);
        //     string authToken = Convert.ToString(ConfigurationManager.AppSettings["TwilioAuthToken"]);

        //     TwilioClient.Init(accountSid, "235426b5a1eda1e999e6e4f17584e107");

        //    var people = new Dictionary<string, string>() {
        //        {"+918668170944", "Test"},
               
        //    };

        //    foreach (var person in people)
        //    {
        //        MessageResource.Create(
        //            from: new PhoneNumber("+17402173945"), // From number, must be an SMS-enabled Twilio number
        //            to: new PhoneNumber(person.Key), // To number, if using Sandbox see note above
        //                                             // Message content
        //            body: $"Hey {person.Value} All classes for the center: " + tupleSMS.Item5 + " are closed today Because of "+Reason+". Do not report to office today.");

        //    }
        //}
     
        public ActionResult DownloadDocuments(string FilePath)
        {
            byte[] fileBytes = null;
            try
            {
                var _ext = Path.GetExtension(FilePath);
                _ext = _ext.Replace('.', ' ').Trim();
                if (FilePath.Contains("~"))
                    FilePath = FilePath.Replace('~', ' ').Trim();
                string absolute = Server.MapPath("~" + FilePath);
                string ContentType = "";
                if (_ext.ToLower() == "pdf")
                    ContentType = "application/" + _ext;
                else
                    ContentType = "image/" + _ext;
                return File(absolute, ContentType);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
            }

        }
        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        //public ActionResult AcceptRejectRequest(string daysOffString, string YakkrArray, string centerId, bool isStaff)
        //{
        //    bool Result = false;
        //    try
        //    {
        //        Guid? userId = new Guid(Session["UserID"].ToString());
        //        Guid? agencyId = new Guid(Session["AgencyId"].ToString());
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        List<SelectListItem> yakkrList = new List<SelectListItem>();
        //        yakkrList = serializer.Deserialize<List<SelectListItem>>(YakkrArray);
        //        DaysOffModel model = new DaysOffModel();
        //        DaysOff daysOff = serializer.Deserialize<DaysOff>(daysOffString);
        //        daysOff.AgencyId = new Guid(Session["AgencyId"].ToString());
        //        daysOff.CreatedBy = new Guid(Session["UserID"].ToString());
        //        daysOff.RoleId = new Guid(Session["RoleId"].ToString());
        //        if (yakkrList.Count() > 0)
        //        {
        //            foreach (var item in yakkrList)
        //            {
        //                if (item.Text != "0")
        //                {
        //                    Result = new TeacherData().AcceptRejectRequest(item.Text, userId, agencyId);
        //                }
        //            }
        //        }

        //        List<ClassRoomModel> classRoomIdArray = new List<ClassRoomModel>();
        //        classRoomIdArray = daysOff.ClassRoomIdArray;
        //        if (daysOff.ClassRoomIdArray.Count() > 0)
        //        {
        //            model = new CenterData().InsertDaysOff(daysOff);

        //        }

        //        List<Tuple<bool, string, string, string, long, string,string>> tupleEmail = new List<Tuple<bool, string, string, string, long, string,string>>();

        //        if (yakkrList.Count() > 0)
        //        {
        //            foreach (var item2 in yakkrList)
        //            {
        //                if (item2.Selected)
        //                {

        //                    tupleEmail = new CenterData().GetParentAndManagementEmail(new Guid(Session["UserID"].ToString()), "3", isStaff, Convert.ToInt64(centerId), Convert.ToInt64(item2.Value));

        //                    if (tupleEmail.Count() > 0)
        //                    {
        //                        string path = "";
        //                        using (var sr = new StreamReader(Server.MapPath("~/MailTemplate/ClassroomClosed.html")))
        //                        {
        //                            path = sr.ReadToEnd();
        //                        }

        //                        foreach (var tuple in tupleEmail)
        //                        {
        //                            string Reason = "";
        //                            Reason = classRoomIdArray.Where(x => Convert.ToInt64(x.CenterId) == Convert.ToInt64(centerId) && Convert.ToInt64(x.ClassRoomId) == tuple.Item5).Select(x => x.OffDayComments).FirstOrDefault();
        //                            path = path.Replace("{Name}", tuple.Item2).Replace("{reason}", Reason).Replace("{reasonBody}", "all classes of Classroom " + tuple.Item7 + " in center " + tuple.Item5 + " are closed today.").Replace("{reportbody}", "");
        //                            //}
        //                            //else
        //                            //{
        //                            // path = path.Replace("{Name}", tuple.Item2).Replace("{reason}", Reason).Replace("{reasonBody}", "all classes of Classroom " + tuple.Item7 + " in center " + tuple.Item5 + " are closed today.").Replace("{reportbody}", "");
        //                            //}
        //                            SendMail.SendEmailToParentAndTeacher(tuple.Item3, path, Session["EmailID"].ToString());
        //	 // for send sms to the parents and teacher 
        //                            if (Convert.ToString(tuple.Item7) != "0")
        //                            {
        //                                SendSMSToParentTeacher(tuple, Reason);
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }


        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //    }
        //    return Json(Result);
        //}

        public ActionResult AcceptRejectRequest(string daysOffString, string YakkrArray, string centerId, bool isStaff)
        {
            bool Result = false;
            try
            {
                Guid? userId = new Guid(Session["UserID"].ToString());
                Guid? agencyId = new Guid(Session["AgencyId"].ToString());
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<SelectListItem> yakkrList = new List<SelectListItem>();
                yakkrList = serializer.Deserialize<List<SelectListItem>>(YakkrArray);
                DaysOffModel model = new DaysOffModel();
                DaysOff daysOff = serializer.Deserialize<DaysOff>(daysOffString);
                daysOff.AgencyId = new Guid(Session["AgencyId"].ToString());
                daysOff.CreatedBy = new Guid(Session["UserID"].ToString());
                daysOff.RoleId = new Guid(Session["RoleId"].ToString());
                if (yakkrList.Count() > 0)
                {
                    foreach (var item in yakkrList)
                    {
                        if (item.Text != "0")
                        {
                            Result = new TeacherData().AcceptRejectRequest(item.Text, userId, agencyId);
                        }
                    }
                }

                List<ClassRoomModel> classRoomIdArray = new List<ClassRoomModel>();
                classRoomIdArray = daysOff.ClassRoomIdArray;
                if (daysOff.ClassRoomIdArray.Count() > 0)
                {
                    model = new CenterData().InsertDaysOff(daysOff);

                }

                List<Tuple<bool, string, string, string, long, string,string>> tupleEmail = new List<Tuple<bool, string, string, string, long, string,string>>();

                if (yakkrList.Count() > 0)
                {
                    foreach (var item2 in yakkrList)
                    {
                        if (item2.Selected)
                        {

                            tupleEmail = new CenterData().GetParentAndManagementEmail(new Guid(Session["UserID"].ToString()), "3", isStaff, Convert.ToInt64(centerId), Convert.ToInt64(item2.Value));

                            if (tupleEmail.Count() > 0)
                            {
                                string path = "";
                                using (var sr = new StreamReader(Server.MapPath("~/MailTemplate/ClassroomClosed.html")))
                                {
                                    path = sr.ReadToEnd();
                                }

                                foreach (var tuple in tupleEmail)
                                {
                                    string Reason = "";
                                    Reason = classRoomIdArray.Where(x => Convert.ToInt64(x.CenterId) == Convert.ToInt64(centerId) && Convert.ToInt64(x.ClassRoomId) == tuple.Item5).Select(x => x.OffDayComments).FirstOrDefault();
                                    path = path.Replace("{Name}", tuple.Item2).Replace("{reason}", Reason).Replace("{reasonBody}", "all classes of Classroom " + tuple.Item7 + " in center " + tuple.Item5 + " are closed today.").Replace("{reportbody}", "");
                                    SendMail.SendEmailToParentAndTeacher(tuple.Item3, path, Session["EmailID"].ToString());

                                    // for send sms to the parents and teacher 
                                    //if (Convert.ToString(tuple.Item7) != "0")
                                    //{
                                    //    SendSMSToParentTeacher(tuple, Reason);
                                    //}
                                   

                                }
                            }
                        }
                    }
                }


            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }

        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult ChangeRequest(string RequestId, string classroomId, string centerId)
        {
            bool Result = false;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string[] classRoomIdarr = serializer.Deserialize<string[]>(classroomId);

                new TeacherData().ChangeRequest(RequestId, classRoomIdarr, Session["UserId"].ToString(), Session["AgencyId"].ToString(), centerId);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }

        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult UploadFile(string monitor)
        {
            string _imgname = string.Empty;
            string _imgpath = string.Empty;
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                    if (pic.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(pic.FileName);
                        var _ext = Path.GetExtension(pic.FileName);

                        _imgname = Guid.NewGuid().ToString();
                        _imgname = "Damage_" + _imgname + _ext;
                        var _comPath = Server.MapPath("/Content/ImageDamage/") + _imgname ;
                        _imgpath = "/Content/ImageDamage/" + _imgname;
                        var path = _comPath;

                        // Saving Image in Original Mode
                        pic.SaveAs(path);

                        // resizing image
                        MemoryStream ms = new MemoryStream();
                        WebImage img = new WebImage(_comPath);

                        if (img.Width > 200)
                            img.Resize(200, 200);
                        img.Save(_comPath);
                        // end resize
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Convert.ToString(_imgpath), JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFireExpiration(string Role)
        {
            string Result = "";
            try
            {
                if (Session["UserID"] != null)
                    Result = _Teacher.GetFireExpirationDate(new Guid(Session["UserID"].ToString()), Session["Roleid"].ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }

        [HttpPost]
        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult UpdateFireExpiration(string Expire, string Role)
        {
            bool Result = false;
            try
            {
                if (Session["UserID"] != null)
                    Result = _Teacher.UpdateFireExpirationDate(new Guid(Session["UserID"].ToString()), Expire, Session["Roleid"].ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }

        [JsonMaxLengthAttribute]
        public JsonResult GetChildrenImage(string enc_clientId)
        {
            SelectListItem childImage = new SelectListItem();
            try
            {
                long clientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_clientId));

                childImage = _Teacher.GetChildrenImageData(clientId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childImage, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter()]
        public ActionResult WeeklyAttendance()
        {
            try
            {
                ViewBag.PageVersion = DateTime.Now.DayOfYear.ToString();

                TeacherModel model = new TeacherModel();
                model=new TeacherData().GetCenterBasedCenters(Session["UserID"].ToString(), Session["AgencyID"].ToString(),Session["RoleId"].ToString());
                model.AgencyId = Session["AgencyId"].ToString();
                model.RoleId = Session["RoleID"].ToString();
                return View(model);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }


        [CustAuthFilter()]


        public JsonResult GetChildListForCenterBased(string centerId,string classroomId,bool isHistorical,string attendanceDate="")
         {
            TeacherModel model = new TeacherModel();
            try
            {
               // attendanceDate = (isHistorical) ? "" : attendanceDate;
               long _centerId = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
              long  _classroomId = Convert.ToInt64(EncryptDecrypt.Decrypt64(classroomId));
                model = new TeacherData().GetChildListByUserIdByCenter(Session["UserID"].ToString(), Session["AgencyID"].ToString(),_centerId,_classroomId,isHistorical,attendanceDate);

                var jsonSerialiser = new JavaScriptSerializer();
                jsonSerialiser.MaxJsonLength = Int32.MaxValue;
                model.WeeklyAttendancestring = jsonSerialiser.Serialize(model.WeeklyAttendance);
                model.ChildInfoString = jsonSerialiser.Serialize(model.Itemlst);
                model.CenterString = jsonSerialiser.Serialize(model.CenterList);
                model.AbsenceReasonString = jsonSerialiser.Serialize(model.AbsenceReasonList);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter()]

        public JsonResult GetChildAttendanceDetailsByDate(string centerId,string classroomId,string attendaneDate,bool isHistorical)
        {
            TeacherModel model = new TeacherModel();
            try
            {
                long _centerId = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                long _classroomId = Convert.ToInt64(EncryptDecrypt.Decrypt64(classroomId));
                // model = new TeacherData().GetChildAttendanceDetailsByDate(Session["AgencyId"].ToString(),attendaneDate, isHistorical, _centerId, _classroomId);
              model = new TeacherData().GetChildListByUserIdByCenter(Session["UserID"].ToString(), Session["AgencyID"].ToString(), _centerId, _classroomId, isHistorical, attendaneDate);
                var jsonSerialiser = new JavaScriptSerializer();
                jsonSerialiser.MaxJsonLength = Int32.MaxValue;
                model.WeeklyAttendancestring = jsonSerialiser.Serialize(model.WeeklyAttendance);
                model.ChildInfoString = jsonSerialiser.Serialize(model.Itemlst);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]

        public JsonResult GetClassRoomsByCenterHistorical(string centerId)
        {
            TeacherModel model = new TeacherModel();

            try
            {
                model.Enc_CenterId = centerId;
                model.CenterID = Convert.ToString(EncryptDecrypt.Decrypt64(centerId));
                model.AgencyId = Session["AgencyId"].ToString();
                model.UserId = Session["UserID"].ToString();
             model=   new TeacherData().GetClassRoomsByCenterHistorical(model);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,82b862e6-1a0f-46d2-aad4-34f89f72369a")]

        public JsonResult GetFSWNameandTeacherName(string centerId,string classroomId)
        {
            TeacherModel model = new TeacherModel();

            try
            {
                model.Enc_CenterId = centerId;
                model.CenterID = Convert.ToString(EncryptDecrypt.Decrypt64(centerId));
                model.Enc_ClassRoomId = classroomId;
                model.ClassID = Convert.ToString(EncryptDecrypt.Decrypt64(classroomId));
                model.AgencyId = Session["AgencyId"].ToString();
                model.UserId = Session["UserID"].ToString();
                model = new TeacherData().GetFSWandTeacherByClassRoom(model);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        



        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5,3b49b025-68eb-4059-8931-68a0577e5fa2,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,82b862e6-1a0f-46d2-aad4-34f89f72369a")]

        public JsonResult InsertAttendanceData(string userId, string agencyId, string centerId, string classRoomId,bool historical, string WeeklyAttendString = "", string dailyMealsString = "",string dateString = "")
        {

            List<TeacherModel> teacherModel = new List<TeacherModel>();
            List<OfflineAttendance> offlineAttendanceList = new List<OfflineAttendance>();
            bool isResult = false;
            try
            {

                var jsonSerialiser = new JavaScriptSerializer();
                jsonSerialiser.MaxJsonLength = Int32.MaxValue;
                List<OfflineAttendance> offlineAttendance = jsonSerialiser.Deserialize<List<OfflineAttendance>>(WeeklyAttendString);
                List<DailyAttendMealsTotal> dailyMealsList = (dailyMealsString != "") ? jsonSerialiser.Deserialize<List<DailyAttendMealsTotal>>(dailyMealsString) : new List<DailyAttendMealsTotal>();

                offlineAttendance.ForEach(x =>
                 {
                     x.ClientID = EncryptDecrypt.Decrypt64(x.ClientID);
                     x.UserID = userId;
                     x.AgencyId = agencyId;
                     x.CenterID = EncryptDecrypt.Decrypt64(centerId);
                     x.ClassroomID = EncryptDecrypt.Decrypt64(classRoomId);
                 });

                if (dailyMealsList != null)
                {
                    dailyMealsList.ForEach(x =>
                    {
                        x.ClassroomID = EncryptDecrypt.Decrypt64(classRoomId);
                        x.CenterID = EncryptDecrypt.Decrypt64(centerId);
                        x.AgencyId = agencyId;
                        x.UserID = userId;

                    });
                }


                offlineAttendance = offlineAttendance.OrderBy(x => x.AttendanceDate).ThenBy(x => x.ClientID).ToList();




                List<TeacherModel> MealsList = new List<TeacherModel>();

                MealsList.AddRange(

                    offlineAttendance.Where(x => x.BreakFast == "1").Select(x => new TeacherModel
                    {
                        ClientID = x.ClientID,
                        AttendanceDate = x.AttendanceDate,
                        MealType = "1",
                        UserId = x.UserID,
                        AgencyId = x.AgencyId,
                        CenterID = x.CenterID,
                        ClassID = x.ClassroomID


                    }).OrderBy(x => x.AttendanceDate).ThenBy(x => x.ClientID).ToList()


                    );



                MealsList.AddRange(
                    offlineAttendance.Where(x => x.Lunch == "1").Select(x => new TeacherModel
                    {
                        ClientID = x.ClientID,
                        AttendanceDate = x.AttendanceDate,
                        MealType = "2",
                        UserId = x.UserID,
                        AgencyId = x.AgencyId,
                        CenterID = x.CenterID,
                        ClassID = x.ClassroomID

                    }).OrderBy(x => x.AttendanceDate).ThenBy(x => x.ClientID).ToList());

                MealsList.AddRange(
                offlineAttendance.Where(x => x.Snacks == "1").Select(x => new TeacherModel
                {
                    ClientID = x.ClientID,
                    AttendanceDate = x.AttendanceDate,
                    MealType = "3",
                    UserId = x.UserID,
                    AgencyId = x.AgencyId,
                    CenterID = x.CenterID,
                    ClassID = x.ClassroomID

                }).OrderBy(x => x.AttendanceDate).ThenBy(x => x.ClientID).ToList());

                MealsList.OrderBy(x => x.AttendanceDate).ThenBy(x => x.ClientID).ToList();

                List<TeacherModel> adultMealsList = dailyMealsList.Select(x => new TeacherModel
                {
                    ABreakfast = x.AdultBreakFast,
                    ALunch = x.AdultLunch,
                    ASnack = x.AdultSnacks,
                    AgencyId = x.AgencyId,
                    UserId = x.UserID,
                    CenterID = x.CenterID,
                    ClassID = x.ClassroomID,
                    AttendanceDate = x.AttendanceDate
                }
                   ).OrderBy(x => x.AttendanceDate).ToList();

                List<TeacherModel> adultMealsList2 = new List<TeacherModel>();

                if (dailyMealsList != null)
                {
                    adultMealsList2.AddRange(

                        dailyMealsList.Select(x => new TeacherModel
                        {
                            MealSelected = x.AdultBreakFast,
                            MealType = "1",
                            AgencyId = x.AgencyId,
                            UserId = x.UserID,
                            CenterID = x.CenterID,
                            ClassID = x.ClassroomID,
                            AttendanceDate = x.AttendanceDate
                        }
                       ).OrderBy(x => x.AttendanceDate).ToList()

                        );

                    adultMealsList2.AddRange(

                       dailyMealsList.Select(x => new TeacherModel
                       {
                           MealSelected = x.AdultLunch,
                           MealType = "2",
                           AgencyId = x.AgencyId,
                           UserId = x.UserID,
                           CenterID = x.CenterID,
                           ClassID = x.ClassroomID,
                           AttendanceDate = x.AttendanceDate
                       }
                      ).OrderBy(x => x.AttendanceDate).ToList()

                       );

                    adultMealsList2.AddRange(

                      dailyMealsList.Select(x => new TeacherModel
                      {
                          MealSelected = x.AdultSnacks,
                          MealType = "3",
                          AgencyId = x.AgencyId,
                          UserId = x.UserID,
                          CenterID = x.CenterID,
                          ClassID = x.ClassroomID,
                          AttendanceDate = x.AttendanceDate
                      }
                     ).OrderBy(x => x.AttendanceDate).ToList()

                      );
                    adultMealsList2.OrderBy(x => x.AttendanceDate).ToList();
                }
                //offlineAttendanceList = new TeacherData().InsertOfflineAttendanceData(offlineAttendance, MealsList, adultMealsList2, userId, agencyId,dateString);

                isResult = new TeacherData().InsertOfflineAttendanceData(offlineAttendance, MealsList, adultMealsList2, userId, agencyId, dateString, historical);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            //return Json(offlineAttendanceList, JsonRequestBehavior.AllowGet);
            return Json(isResult, JsonRequestBehavior.AllowGet);

        }
    }
}
