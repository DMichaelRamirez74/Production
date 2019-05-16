using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Data;
using Fingerprints.CustomClasses;
using Fingerprints.ViewModel;
using System.Text;
using FingerprintsModel.Enums;

namespace Fingerprints.Controllers
{
    public class EventsController : Controller
    {
        //
        // GET: /Events/
        /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
        roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
        roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
        roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
        roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
        roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
        roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacher)
        roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
        roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
        roleid=5ac211b2-7d4a-4e54-bd61-5c39d67a1106 (Parent)
        */

        StaffDetails staff = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();

        [CustAuthFilter(RoleEnum.CenterManager)]

        public ActionResult CreateEvent(string Id, string Name,string cId="")
        {
            Events objEvents = new Events();
            objEvents.Workshopid = Convert.ToInt32(Id);
            objEvents.Workshopname = Name;
            objEvents.CenterId = cId;
            new EventsData().GetWorkshopDetails(ref objEvents, Id, staff);
            List<SelectListItem> objCIFFOB = new List<SelectListItem>();


            switch (objEvents.EventStatus)
            {
                case null:
                   
                    objCIFFOB.Add(new SelectListItem { Value = "1", Text = "Open" });
                    objCIFFOB.Add(new SelectListItem { Value = "2", Text = "Pending", Selected = true });
                    break;

                case "1":

                    if (objEvents.MinutesDiff >= 0 && objEvents.MinutesDiff <= 30)
                    {
                       
                        objCIFFOB.Add(new SelectListItem { Value = "1", Text = "Open", Selected = true });
                        objCIFFOB.Add(new SelectListItem { Value = "3", Text = "Canceled" });
                        objCIFFOB.Add(new SelectListItem { Value = "0", Text = "Closed" });
                      
                    }
                    else
                    {
                       
                        objCIFFOB.Add(new SelectListItem { Value = "1", Text = "Open", Selected = true });
                        objCIFFOB.Add(new SelectListItem { Value = "3", Text = "Canceled" });
                       
                    }
                    break;
                case "2":
                   
                    objCIFFOB.Add(new SelectListItem { Value = "2", Text = "Pending", Selected = true });
                    objCIFFOB.Add(new SelectListItem { Value = "3", Text = "Canceled" });
                    break;

                case "3":
                    objCIFFOB.Add(new SelectListItem { Value = "1", Text = "Open" });
                    objCIFFOB.Add(new SelectListItem { Value = "3", Text = "Canceled", Selected = true });
                    break;

                case "0":
                   
                    objCIFFOB.Add(new SelectListItem { Value = "0", Text = "Closed", Selected = true });
                    break;

                default:
                    objCIFFOB.Add(new SelectListItem { Value = "-1", Text = "Choose" });
                    objCIFFOB.Add(new SelectListItem { Value = "2", Text = "Pending" });
                    objCIFFOB.Add(new SelectListItem { Value = "1", Text = "Open" });
                    objCIFFOB.Add(new SelectListItem { Value = "3", Text = "Canceled" });
                    objCIFFOB.Add(new SelectListItem { Value = "0", Text = "Closed" });
                    break;

            }
            objEvents.ListReason = LoadReasons();
            ViewBag.EventsStatus = objCIFFOB;
            ViewBag.CenterId = cId;
            return View(objEvents);
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult SaveEvents(Events objEvents)
        {
            bool isResult = false;
            try
            {
                objEvents.AgencyId = Session["AgencyID"].ToString();
                objEvents.UserId = Session["UserID"].ToString();
                isResult = new EventsData().SaveEvents(objEvents);
                if (objEvents.IsUpdate)
                {
                    List<ReasonList> reasonList = new List<ReasonList>();
                    GetRegisteredParentAndMail(objEvents);
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }

        [CustAuthFilter(RoleEnum.CenterManager,RoleEnum.Parent)]
        public JsonResult GetCancelledEventInfo(int mode)
        {
            EventsList events = new EventsList();
            List<EventsList> eventsList = new List<EventsList>();
            List<ReasonList> reasonList = new List<ReasonList>();
            bool isResult = false;

            try
            {
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                if (mode == 1)
                {
                    isResult = new EventsData().GetUpdatedReasons(events, mode);

                }
                else
                {
                    eventsList = new EventsData().GetChangedEventsForParent(events, mode);
                }

                return Json(new { isResult, eventsList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(new { isResult, eventsList }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public void GetRegisteredParentAndMail(Events events)
        {

            try
            {
                int CheckMode = events.CheckMode;
                string DefaultStatus = events.DefaultStatus;
                string ChangeStatus = events.ChangeStatus;
                string DefaultDate = events.DefaultDate;
                string ChangeDate = events.ChangeDate;
                string DefaultTime = events.DefaultTime;
                string ChangeTime = events.ChangeTime;
                string Enc_EventId = EncryptDecrypt.Encrypt64(events.Eventid.ToString());
                events = new EventsData().GetRegisteredParentEmailData(events);
                string path;
                using (var sr = new StreamReader(Server.MapPath("~/MailTemplate/EmailCancel.html")))
                {
                    path = sr.ReadToEnd();
                }
                string link = UrlExtensions.LinkToRegistrationProcess("/Login/loginagency");
                string imagepath = UrlExtensions.LinkToRegistrationProcess("Content/img/logo_email.png");
                string yesPath = UrlExtensions.LinkToRegistrationProcess("/Events/MailMessage");
                string noPath = UrlExtensions.LinkToRegistrationProcess("/Events/ConfirmMail");
              //  string mode = "3";
                string[] array = { events.EventStatusDescription, events.EventDateDescription, events.EventTimeDescription };
                string FromEmail = Session["EmailID"].ToString();
                string reason = string.Join(",", array.Where(s => !string.IsNullOrEmpty(s)));
                if (events.RegisteredMembers != null)
                {


                    if (events.RegisteredMembers.Count() > 0)
                    {

                        foreach (var item in events.RegisteredMembers)
                        {

                            System.Threading.Thread thread = new System.Threading.Thread(delegate ()
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                if (CheckMode == 1)//status
                                {
                                    
                                    path = path.Replace("{Name}", item.Text).Replace("{WorkShopName}", events.Workshopname).
                                    Replace("{EventDate}", events.EventDate).
                                    Replace("{EventTime}", events.EventTime).
                                    Replace("{CenterName}", events.CenterName)
                                    .Replace("{url}", link).
                                    Replace("{Path}", imagepath).
                                    Replace("{ChangeReason}", reason).
                                    Replace("{YesPath}", yesPath).
                                    Replace("{NoPath}", noPath).
                                    Replace("{Email}", item.Value).
                                    Replace("{EventId}", Enc_EventId).
                                    Replace("{DefaultStatus}", DefaultStatus).
                                    Replace("{ChangeStatus}", ChangeStatus).
                                    Replace("{StatusContent}", "changed to").
                                    Replace("{DefaultDate}", "").Replace("{ChangeDate}", "").
                                    Replace("{DefaultTime}", "").Replace("{ChangeTime}", "").
                                    Replace("{DateContent}", "").Replace("{TimeContent}", "");
                                    path = path.Replace("{Name}", item.Text).
                                    Replace("{WorkShopName}", events.Workshopname).
                                    Replace("{EventDate}", events.EventDate).
                                    Replace("{EventTime}", events.EventTime).
                                    Replace("{CenterName}", events.CenterName).
                                    Replace("{url}", link).Replace("{Path}", imagepath).
                                    Replace("{ChangeReason}", reason).Replace("{YesPath}", yesPath).
                                    Replace("{NoPath}", noPath).Replace("{Email}", item.Value).
                                    Replace("{EventId}", Enc_EventId).Replace("{DefaultStatus}", DefaultStatus).
                                    Replace("{ChangeStatus}", ChangeStatus).Replace("{StatusContent}", "changed to").
                                    Replace("{DefaultDate}", DefaultDate).Replace("{ChangeDate}", "").Replace("{DefaultTime}", DefaultTime).
                                   Replace("{ChangeTime}", "").Replace("{DateContent}", "").Replace("{TimeContent}", "");

                                }
                                else if (CheckMode == 2)//date
                                {
                                    path = path.Replace("{Name}", item.Text).
                                    Replace("{WorkShopName}", events.Workshopname).
                                    Replace("{EventDate}", events.EventDate).
                                    Replace("{EventTime}", events.EventTime).
                                    Replace("{CenterName}", events.CenterName).
                                    Replace("{url}", link).
                                    Replace("{Path}", imagepath).
                                    Replace("{ChangeReason}", reason).
                                    Replace("{YesPath}", yesPath).
                                    Replace("{NoPath}", noPath).
                                    Replace("{Email}", item.Value).
                                    Replace("{EventId}", Enc_EventId).
                                    Replace("{DefaultDate}", DefaultDate).
                                    Replace("{ChangeDate}", ChangeDate).
                                    Replace("{DateContent}", "changed to").
                                    Replace("{DefaultStatus}", DefaultStatus).
                                    Replace("{ChangeStatus}", "").
                                    Replace("{DefaultTime}", DefaultTime).
                                    Replace("{ChangeTime}", "").
                                    Replace("{StatusContent}", "").
                                    Replace("{TimeContent}", "");

                                }
                                else if (CheckMode == 3)//time
                                {
                                    path = path.Replace("{Name}", item.Text).
                                    Replace("{WorkShopName}", events.Workshopname).
                                    Replace("{EventDate}", events.EventDate).
                                    Replace("{EventTime}", events.EventTime).
                                    Replace("{CenterName}", events.CenterName).
                                    Replace("{url}", link).
                                    Replace("{Path}", imagepath).
                                    Replace("{ChangeReason}", reason).
                                    Replace("{YesPath}", yesPath).
                                    Replace("{NoPath}", noPath).
                                    Replace("{Email}", item.Value).
                                    Replace("{EventId}", Enc_EventId).
                                    Replace("{DefaultTime}", DefaultTime).
                                    Replace("{ChangeTime}", ChangeTime).
                                    Replace("{TimeContent}", "changed to").
                                    Replace("{DefaultStatus}", DefaultStatus).
                                    Replace("{ChangeStatus}", "").
                                    Replace("{DefaultDate}", DefaultDate).
                                    Replace("{ChangeDate}", "").
                                    Replace("{StatusContent}", "").
                                    Replace("{DateContent}", "");
                                }
                                SendMail.SendEventChangedEmail(item.Value, path, FromEmail);
                            });
                            thread.Start();

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
        }

        public ActionResult MailMessage(string id = "", string e = "", string y = "0")
        {
            bool Result = false;
            Result = InsertResponseEventChange(id, e, y);
            ViewBag.IsResult =
            ViewBag.Confirm = "Your registered workshop has been modified Successfully";
            return View();
        }



        public bool InsertResponseEventChange(string id = "", string email = "", string yes = "0")
        {
            bool isResult = false;
            bool attend = false;
            try
            {
                EventsList events = new EventsList();

                events.EventId = Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
                events.EmailId = email;
                attend = (yes == "1");
                isResult = new EventsData().InsertResponseEventChangeData(events, attend);
                return isResult;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return isResult;
            }

        }

        public JsonResult GetCenterAddress(string Id)
        {
            string CenterAddress = "";
            try
            {
                CenterAddress = new EventsData().GetCenterAddress(Id);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(CenterAddress);
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public JsonResult UploadFile(string monitor)
        {
            string[] _imgpath = new string[System.Web.HttpContext.Current.Request.Files.Count];
            try
            {

                _imgpath = SaveImageToSource();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(_imgpath, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public JsonResult ChangeImage(string monitor, string eventId)
        {
            string[] _imgpath = new string[System.Web.HttpContext.Current.Request.Files.Count];

            bool isResult = false;
            EventsList events = new EventsList();
            try
            {

                _imgpath = SaveImageToSource();
                if (_imgpath.Count() > 0)
                {
                    foreach (var image in _imgpath)
                    {
                        events.EventId = Convert.ToInt64(EncryptDecrypt.Decrypt64(eventId));
                        events.AgencyId = new Guid(Session["AgencyID"].ToString());
                        events.UserId = new Guid(Session["UserID"].ToString());
                        events.ImagePath = image;
                        isResult = new EventsData().UpdateEventsImage(events);
                    }
                }



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { isResult, _imgpath }, JsonRequestBehavior.AllowGet);
        }


        public string[] SaveImageToSource()
        {
            string _imgname = string.Empty;
            string[] _imgpath = new string[System.Web.HttpContext.Current.Request.Files.Count];
            try
            {
                int i = 0;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    foreach (var key in System.Web.HttpContext.Current.Request.Files)
                    {

                        var pic = System.Web.HttpContext.Current.Request.Files[key.ToString()];
                        if (pic.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(pic.FileName);
                            var _ext = Path.GetExtension(pic.FileName);
                            string name = Path.GetFileNameWithoutExtension(fileName);
                            _imgname = Guid.NewGuid().ToString();
                            _imgname = name + "_" + _imgname + _ext;
                            var _comPath = Server.MapPath("/Content/EventsImages/") + _imgname;
                            _imgpath[i] = "/Content/EventsImages/" + _imgname;
                            i++;
                            var path = _comPath;

                            // Saving Image in Original Mode
                            pic.SaveAs(path);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return _imgpath;
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult EventsReport()
        {
            try
            {
                EventsList events = new EventsList();
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View();
        }
        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult Events()
        {
            EventsModal eventsModal = new EventsModal();
            try
            {

                EventsList events = new EventsList();
                events.AgencyId = new Guid(Session["AgencyID"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.CenterId = 0;
                eventsModal = GetAllEvents(events);

                return View(eventsModal);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View(eventsModal);
            }


        }

        [CustAuthFilter(RoleEnum.CenterManager)]

        public JsonResult GetEventsByCenter(string centerId)
        {
            EventsModal eventsModal = new EventsModal();
            try
            {
                EventsList events = new EventsList();
                events.AgencyId = new Guid(Session["AgencyID"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                eventsModal = GetAllEvents(events);
                return Json(eventsModal, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(eventsModal, JsonRequestBehavior.AllowGet);
            }
        }

        public EventsModal GetAllEvents(EventsList list)
        {
            EventsModal modal = new EventsModal();
            modal = new EventsData().GetEventsListData(list);
            return modal;
        }


        public JsonResult GetRegisteredEvent()
        {
            EventsList events = new EventsList();
            List<EventsList> eventsList = new List<EventsList>();
            try
            {
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                eventsList = new EventsData().GetRegisteredEvent(events);
                return Json(eventsList);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(eventsList);

            }
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult EventDetails(string id)
        {

            ShowEventDetails details = new ShowEventDetails();
            try
            {
                details = new EventsData().GetEventDetails(id, new Guid(Session["AgencyId"].ToString()));


                return View(details);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View();
            }
        }

        [CustAuthFilter(RoleEnum.Parent)]
        public ActionResult ParentEventSelection()
        {
            ParentSelectionEvent modal = new ParentSelectionEvent();
            try
            {
                EventsList events = new EventsList();
                List<EventsCenter> centerList = new List<EventsCenter>();
                List<EventsList> eventsList = new List<EventsList>();
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                modal = new EventsData().GetParentEventSelectionData(events, Session["EmailId"].ToString());

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(modal);
        }
        [CustAuthFilter(RoleEnum.Parent)]
        public ActionResult ParentEventRegistration(string id = "0", string m = "0")
        {

            ParentRegisterEvent registerEvent = new ParentRegisterEvent();
            EventsList events = new EventsList();

            string emailId = string.Empty;
            try
            {
                ViewBag.UpdateMode = m;
                events.EventId = Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                emailId = Session["EmailId"].ToString();
                registerEvent = new EventsData().GetEventsForRegisterData(events, emailId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(registerEvent);
        }


        public List<SelectListItem> LoadReasons()
        {
            List<SelectListItem> reasonList = new List<SelectListItem>();
            Events events = new Events();


            string emailId = string.Empty;
            try
            {
                events.AgencyId = Session["AgencyID"].ToString();
                reasonList = new EventsData().LoadReasons(events);
                events.ListReason = reasonList;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return events.ListReason;
        }

        [CustAuthFilter(RoleEnum.Parent)]
        public ActionResult EventInformation(string id = "0", int mode = 0)
        {
            ParentRegisterEvent registerEvent = new ParentRegisterEvent();
            EventsList events = new EventsList();
            try
            {
                events.EventId = (id == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                registerEvent = new EventsData().GetCancelledWorkShopInfo(events, mode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(registerEvent);
        }

        public ActionResult ConfirmMail(string id = "0", int mode = 0)
        {
            ParentRegisterEvent registerEvent = new ParentRegisterEvent();
            EventsList events = new EventsList();
            try
            {
                ViewBag.Confirm = "Your registered workshop has been cancelled";
                events.EventId = (id == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                registerEvent = new EventsData().GetCancelledWorkShopInfo(events, mode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View("/Views/Events/MailMessage.cshtml");
        }


        [CustAuthFilter(RoleEnum.Parent)]
        public JsonResult RegisterEvent(string parentEventString = "", int mode = 0)
        {


            RegisteredFeeback feedBack = new RegisteredFeeback();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                ParentRegisterEvent eventregsister = json_serializer.Deserialize<ParentRegisterEvent>(parentEventString);
                eventregsister.Events.AgencyId = new Guid(Session["AgencyId"].ToString());
                eventregsister.Events.UserId = new Guid(Session["UserID"].ToString());
                eventregsister.Events.EmailId = Session["EmailId"].ToString();
                feedBack = new EventsData().RegisterForEventData(eventregsister, mode);
                return Json(feedBack, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(feedBack, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter(RoleEnum.Parent)]
        public JsonResult CancelEventRegistration(string enc_EventId)
        {
            bool isResult = false;
            EventsList events = new EventsList();
            try
            {
                events.EventId = Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_EventId));
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                events.UserId = new Guid(Session["UserID"].ToString());
                isResult = new EventsData().CancelEventRegistrationData(events);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult CancelledEvents()
        {
            return View();
        }

        [CustAuthFilter(RoleEnum.Parent)]
        public ActionResult CancelledEventsForParent()
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
        [CustAuthFilter(RoleEnum.CenterManager)]

        public ActionResult RegisteredParentsForEvent()
        {


            return View();
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public JsonResult GetRegParentsByWorkShop(string workShopId, string centerId, int mode, string searchText = "")
        {
            ParentRegisterEvent parentEvent = new ParentRegisterEvent();
            EventsList events = new EventsList();
            try
            {
                events.AgencyId = new Guid(Session["AgencyID"].ToString());
                events.UserId = new Guid(Session["UserId"].ToString());
                events.CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                events.EventId = (workShopId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(workShopId));
                parentEvent = new EventsData().GetRegisteredParentsForEvents(events, mode, searchText);
                return Json(parentEvent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(parentEvent, JsonRequestBehavior.AllowGet);
            }
        }
        [CustAuthFilter(RoleEnum.Parent)]
        public JsonResult GetParentEventCalender()
        {
            EventsList events = new EventsList();
            List<EventsList> eventsList = new List<EventsList>();

            try
            {
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                eventsList = new EventsData().GetRegisteredEvent(events);
                return Json(eventsList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(eventsList, JsonRequestBehavior.AllowGet);
            }

        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult ParentCheckInEvent(string id = "0", string enc_id = "0")
        {

            ParentRegisterEvent registerEvent = new ParentRegisterEvent();
            EventsList events = new EventsList();
            string emailId = string.Empty;
            try
            {

                events.EventId = Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
                events.ClientId = (enc_id == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_id));
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                emailId = string.Empty;
                registerEvent = new EventsData().GetEventsForRegisterData(events, emailId);
                registerEvent.Events.Enc_ClientId = enc_id;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(registerEvent);
        }

        

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult NewRegistration(string WorkShopName, string CenterName, string Enc_Centerid = "0", string WorkShopId = "0")
        {

            List<NewRegistration> list = new List<NewRegistration>();
            NewRegistration list1 = new NewRegistration();
            WalkinRegistraton registration = new WalkinRegistraton();
            try
            {

                EventsList events = new EventsList
                {
                    WorkShopName = WorkShopName,
                    CenterName = CenterName,
                    CenterId = (Enc_Centerid == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(Enc_Centerid)),
                    WokShopId = (WorkShopId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(WorkShopId)),
                    UserId = new Guid(Session["UserID"].ToString()),
                    AgencyId = new Guid(Session["AgencyId"].ToString())

                };
                List<EventsCenter> centerList = new List<EventsCenter>();
                list = new EventsData().GetClientForNewRegistration(ref centerList, events);
                ViewBag.WorkShopName = WorkShopName;
                ViewBag.CenterName = CenterName;

                ViewBag.CenterID = Enc_Centerid;
                ViewBag.workShopId = WorkShopId;
                
                registration.NewRegistrationList = list;
                registration.CenterList = centerList;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(registration);

        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public JsonResult GetNewClientBySearch(string Enc_Centerid = "0", string WorkShopId = "0", string searchText = "", string centerId2 = "0", int mode = 0)
        {
            List<NewRegistration> list = new List<NewRegistration>();
            try
            {
                EventsList events = new EventsList
                {

                    CenterId = (Enc_Centerid == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(Enc_Centerid)),
                    WokShopId = (WorkShopId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(WorkShopId)),
                    UserId = new Guid(Session["UserID"].ToString()),
                    AgencyId = new Guid(Session["AgencyId"].ToString()),

                };
                long ConvCenterID = (centerId2 == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId2));
                List<EventsCenter> centerList = new List<EventsCenter>();
                list = new EventsData().GetClientForNewRegistration(ref centerList, events, searchText, ConvCenterID, mode);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult NewParentCheckInEvent(string householdid = "0", string centerid = "0", string clientid = "0", string WorkShopId = "0")
        {
            ParentRegisterEvent registerEvent = new ParentRegisterEvent();
            EventsList events = new EventsList();
            string emailId = string.Empty;
            try
            {
                events.ClientId = (clientid == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(clientid));
                events.CenterId = (centerid == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerid));
                events.WokShopId = (WorkShopId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(WorkShopId));
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                emailId = string.Empty;
                registerEvent = new EventsData().GetEventsForNewRegisterData(events, emailId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(registerEvent);

        }



        public ActionResult GetEventDetails(string workshopid, string centerid)
        {

            string JSONString = string.Empty;
            try
            {
                DataTable dtDevelopmentTeamDetails = new DataTable();
                new EventsData().GetEventDetails(ref dtDevelopmentTeamDetails, Session["AgencyId"].ToString(), workshopid, centerid);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtDevelopmentTeamDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        [CustAuthFilter(RoleEnum.CenterManager)]

        public JsonResult AcceptParentSigForEvent(string parentEventString = "")
        {
            RegisteredFeeback feedBack = new RegisteredFeeback();
            try
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                ParentRegisterEvent eventregsister = json_serializer.Deserialize<ParentRegisterEvent>(parentEventString);
                eventregsister.Events.AgencyId = new Guid(Session["AgencyId"].ToString());
                eventregsister.Events.UserId = new Guid(Session["UserID"].ToString());
                eventregsister.Events.ClientId = (eventregsister.Events.Enc_ClientId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(eventregsister.Events.Enc_ClientId));
                feedBack = new EventsData().AcceptSignatureParent(eventregsister);
                return Json(feedBack, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(feedBack, JsonRequestBehavior.AllowGet);

            }

        }

        [CustAuthFilter(RoleEnum.Parent)]
        public JsonResult GetRemainingSeats(string enc_Id = "0")
        {
            EventsList events = new EventsList();
            long remainingCount = 0;
            try
            {
                events.EventId = (enc_Id == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_Id));
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                remainingCount = new EventsData().GetRemainingSeats(events);
                return Json(remainingCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(remainingCount, JsonRequestBehavior.AllowGet);
            }
        }
        [CustAuthFilter(RoleEnum.Parent)]
        public JsonResult GetEventsCountCalender(bool isRender = false)
        {
            EventsList events = new EventsList();
            List<EventsCalender> calenderEvents = new List<EventsCalender>();

            try
            {
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                if (isRender)
                {
                    calenderEvents = new EventsData().GetRegisteredEventCalendar(events);
                }
                else
                {
                    calenderEvents = new EventsData().GetRegisteredEventCount(events);

                }
                var arr = calenderEvents.ToArray();
                return Json(arr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                var arr = calenderEvents.ToArray();
                return Json(arr, JsonRequestBehavior.AllowGet);
            }

        }

        [CustAuthFilter(RoleEnum.Parent)]
        public JsonResult GetEventsForCalender()
        {
            EventsList events = new EventsList();
            List<EventsCalender> calenderEvents = new List<EventsCalender>();

            try
            {
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.UserId = new Guid(Session["UserID"].ToString());
                events.EmailId = Session["EmailId"].ToString();
                calenderEvents = new EventsData().GetRegisteredEventCalendar(events);
                var arr = calenderEvents.ToArray();
                return Json(arr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                var arr = calenderEvents.ToArray();
                return Json(arr, JsonRequestBehavior.AllowGet);
            }
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult GetWorkshopReport(string Workshopid, string CenterId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtWorkshop = new DataTable();
                new EventsData().WorkshopReport(ref dtWorkshop,staff,  Workshopid, CenterId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtWorkshop);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        public ActionResult GetCancelledWorkshop(string CenterId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtWorkshop = new DataTable();
                new EventsData().GetCancelledWorkshop(ref dtWorkshop, CenterId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtWorkshop);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        [CustAuthFilter(RoleEnum.CenterManager)]
        [JsonMaxLengthAttribute]
        public JsonResult GetChildrenImage(string enc_clientId)
        {
            SelectListItem childImage = new SelectListItem();
            try
            {
                long clientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_clientId));

                childImage = new EventsData().GetChildrenImageData(clientId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childImage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckDate(string EventId)
        {
            // int DateCount = new int();
            EventsList events = new EventsList();
            try
            {
                events.AgencyId = new Guid(Session["AgencyId"].ToString());
                events.EventId = Convert.ToInt64(EncryptDecrypt.Decrypt64(EventId).ToString());
                events = new EventsData().CheckDate(events);

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            // return Json(JSONString);

            return Json(events, JsonRequestBehavior.AllowGet);
        }
        




    }


}
