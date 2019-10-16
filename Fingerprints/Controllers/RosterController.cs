using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Fingerprints.CustomClasses;
using Fingerprints.Filters;
using Fingerprints.ViewModel;
using FingerprintsData;
using FingerprintsModel;
using FingerprintsModel.Enums;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Fingerprints.Controllers
{

    public class RosterController : Controller
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
         roleid=825f6940-9973-42d2-b821-5b6c7c937bfe(Facilities Manager)
         roleid=c352f959-cfd5-4902-a529-71de1f4824cc (Social Service Manager)
         roleid=2af7205e-87b4-4ca7-8ca8-95827c08564c (Area Manager)
         roleid=2d9822cd-85a3-4269-9609-9aabb914D792 (HR Manager)
         roleid=6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba (Transportation Manager)
         roleid=7c2422ba-7bd4-4278-99af-b694dcab7367 (executive)
         roleid=b65759ba-4813-4906-9a69-e180156e42fc (ERSEA Manager)
         roleid=4b77aab6-eed1-4ac3-b498-f3e80cf129c0 (Education Manager)
          roleid=944d3851-75cc-41e9-b600-3fa904cf951f (Billing Manager)
          roleid=3b49b025-68eb-4059-8931-68a0577e5fa2 (Agency Admin)
          roleid=047c02fe-b8f1-4a9b-b01f-539d6a238d80 (Disabilities Manager)
          roleid=9c34ec8e-2359-4704-be89-d9f4b7706e82 (Disability Staff)
         */


        private readonly StaffDetails staff = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();
        readonly RosterData rosterData;

        //   public RosterController() => rosterData = new RosterData(staff);


        public RosterController()
        {
            rosterData = new RosterData(staff);
        }



        private readonly string[] managerRoleArray = { "A65BB7C2-E320-42A2-AED4-409A321C08A5","3B49B025-68EB-4059-8931-68A0577E5FA2",
                "944D3851-75CC-41E9-B600-3FA904CF951F", "047C02FE-B8F1-4A9B-B01F-539D6A238D80",
            "9AD1750E-2522-4717-A71B-5916A38730ED", "825F6940-9973-42D2-B821-5B6C7C937BFE",

        "B4D86D72-0B86-41B2-ADC4-5CCCE7E9775B", "C352F959-CFD5-4902-A529-71DE1F4824CC","2AF7205E-87B4-4CA7-8CA8-95827C08564C",
        "2D9822CD-85A3-4269-9609-9AABB914D792","6ED25F82-57CB-4C04-AC8F-A97C44BDB5BA","7C2422BA-7BD4-4278-99AF-B694DCAB7367",
            "B65759BA-4813-4906-9A69-E180156E42FC","4B77AAB6-EED1-4AC3-B498-F3E80CF129C0","A31B1716-B042-46B7-ACC0-95794E378B26" };



        [CustAuthFilter()]
        public ActionResult Roster()
        {
            ViewBag.IsManager = false;

            ViewBag.IsManager = (Array.IndexOf(managerRoleArray, Session["RoleId"].ToString().ToUpper()) > -1);




            Roster roster = new FingerprintsModel.Roster();

            Parallel.Invoke(() =>
            {
                List<ClosedInfo> closedList = new CenterData().CheckForTodayClosure(staff);

                if (closedList.Count() > 0)
                {
                    roster.ClosedDetails = new ClosedInfo
                    {
                        ClosedToday = closedList.Select(x => x.ClosedToday).FirstOrDefault(),
                        CenterName = string.Join(",", closedList.Select(x => x.CenterName).Distinct().ToArray()),
                        ClassRoomName = string.Join(",", closedList.Select(x => x.ClassRoomName).Distinct().ToArray()),
                        AgencyName = closedList.Select(x => x.AgencyName).FirstOrDefault()
                    };
                }
                else
                {
                    roster.ClosedDetails = new ClosedInfo();
                }

            },
           () =>
           {

               roster.AbsenceReasonList = new TeacherData().GetAbsenceReason(staff.AgencyId, 3).ToList();
               roster.AbsenceReasonList.Add(new AbsenceReason
               {

                   ReasonId = 0,
                   Reason = "Choose Reason"
               });

           },
           () =>
           {
               roster.AbsenceTypeList = new TeacherData().GetAttendanceType(staff.AgencyId.ToString(), staff.UserId.ToString(), false).Where(x => x.Value == "2" || x.Value == "3").ToList();

               roster.AbsenceTypeList.Add(new SelectListItem
               {
                   Text = "Select Type",
                   Value = "0"
               });
           },
           () =>
           {
               if (Role.RolesDictionary[(int)RoleEnum.HealthNurse].ToLowerInvariant() == Convert.ToString(Session["RoleID"]).ToUpper())
               {
                   Session["YakkrCountPending"] = new YakkrData().GetYakkrCountPending();
               }

           }


            );


            return View(roster);
        }


        // [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        [CustAuthFilter()]
        public ActionResult RedirectToRoster()
        {
            //ViewBag.IsManager = false;

            //  ViewBag.IsManager = (Array.IndexOf(managerRoleArray, Session["RoleId"].ToString().ToUpper()) > -1);

            Session["_RosterCenter"] = "";
            Session["_RosterClassroom"] = "";
            Session["_RosterPageSize"] = 0;
            Session["_RosterRequestedPage"] = 1;
            Session["_RosterSearchText"] = "";
            Session["_RosterFilter"] = 0;

            return RedirectToAction("Roster", "Roster");
        }


        [HttpPost]
        // [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        [CustAuthFilter()]
        public ActionResult CheckIn(FormCollection collection)
        {
            try
            {
                int reasonid = 0;
                string NewReason = "";
                string childCode = collection.Get("childid");
                string childID = EncryptDecrypt.Decrypt64(childCode);

                string absentType = collection.Get("AbsentType");
                string Cnotes = (collection.Get("CNotes") == null) ? "" : collection.Get("CNotes");
                if (!string.IsNullOrEmpty(collection.Get("ReasonList")))
                {
                    NewReason = collection.Get("txtNewReason");
                }

                if (collection.Get("ReasonList").ToString() != "")
                {
                    reasonid = Convert.ToInt32(collection.Get("ReasonList"));
                }

                string result = "";
                rosterData.MarkAbsent(ref result, childID, absentType, Cnotes, reasonid, NewReason);
                if (result == "1")
                {
                    TempData["message"] = "Record saved successfully.";
                }

                return Redirect("~/Roster/Roster");
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }




        [JsonMaxLengthAttribute]
        [CustAuthFilter()]
        [HttpPost]
        public JsonResult GetRosterList(RosterViewModel rosterViewModel)

        {
            try
            {

                Session["_RosterCenter"] = rosterViewModel.CenterId;
                Session["_RosterClassroom"] = rosterViewModel.ClassroomId;
                Session["_RosterPageSize"] = rosterViewModel.PageSize;
                Session["_RosterRequestedPage"] = rosterViewModel.RequestedPage;
                Session["_RosterSearchText"] = rosterViewModel.SearchTerm;
                Session["_RosterFilter"] = rosterViewModel.FilterOption;
                Session["_RosterSortOrder"] = rosterViewModel.SortColumn;
                Session["_RosterSortDirection"] = rosterViewModel.SortOrder;

                rosterViewModel.SkipRows = rosterViewModel.GetSkipRows();
                int totalrecord = 0;
                var list = rosterData.GetrosterList(out totalrecord, rosterViewModel);
                return Json(new { totalrecord, list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("");
            }
        }

        [CustAuthFilter(RoleEnum.Teacher, RoleEnum.BillingManager, RoleEnum.GenesisEarthAdministrator,
            RoleEnum.DisabilitiesManager, RoleEnum.HealthManager, RoleEnum.FacilitiesManager, RoleEnum.CenterManager,
            RoleEnum.SocialServiceManager, RoleEnum.HealthNurse, RoleEnum.AreaManager, RoleEnum.HomeVisitor,
            RoleEnum.HRManager, RoleEnum.TransportManager, RoleEnum.Executive, RoleEnum.FamilyServiceWorker,
            RoleEnum.TeacherAssistant, RoleEnum.ERSEAManager, RoleEnum.EducationManager)]

        public JsonResult Getclassrooms(string Centerid = "0")
        {
            try
            {
                return Json(rosterData.Getclassrooms(Centerid));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        public JsonResult AutoCompleteSerType(string Services)
        {

            var result = rosterData.AutoCompleteSerType(Services, staff.AgencyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,047c02fe-b8f1-4a9b-b01f-539d6a238d80,c352f959-cfd5-4902-a529-71de1f4824cc")]
        //[ValidateInput(false)]
        //public ActionResult ObservationNotesClient(string id = "")
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(id))
        //        {
        //            id = FingerprintsModel.EncryptDecrypt.Decrypt64(id);
        //        }
        //        else
        //        {
        //            id = "0";
        //        }
        //        string Name = "";
        //        int centerid = 0;
        //        int Householdid = 0;
        //        if (!string.IsNullOrEmpty(Request.QueryString["centerid"]))
        //        {
        //            ViewBag.centerid = centerid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["centerid"].ToString()));
        //        }
        //        else
        //            ViewBag.centerid = 0;
        //        FingerprintsModel.RosterNew.Users Userlist = new FingerprintsModel.RosterNew.Users();

        //        if (!string.IsNullOrEmpty(Request.QueryString["Householdid"]))
        //        {
        //            if (Request.QueryString["Householdid"].ToString() == "0")
        //                Householdid = 0;
        //            else
        //                Householdid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["Householdid"].ToString()));
        //            ViewBag.Householdid = Request.QueryString["Householdid"].ToString();
        //        }
        //        else
        //            ViewBag.Householdid = 0;

        //        ViewBag.CaseNotelist = RosterData.GetCaseNote(ref Name, ref Userlist, Householdid, centerid, id, Session["AgencyID"].ToString(), Session["UserID"].ToString());
        //        ViewBag.Userlist = Userlist;
        //        ViewBag.Name = Name;
        //        ViewBag.Client = id;
        //        if (!string.IsNullOrEmpty(Request.QueryString["Programid"]))
        //            ViewBag.Programid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["Programid"].ToString()));
        //        else
        //            ViewBag.Programid = 0;

        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //    }
        //    return View();
        //}


        [CustAuthFilter(RoleEnum.Teacher, RoleEnum.TeacherAssistant)]
        public ActionResult ObservationNote(string ChildId, string ChildName, string NoteId)
        {
            ObservationNotes objNotes = new ObservationNotes();
            try
            {
                if (!string.IsNullOrEmpty(ChildId))
                {
                    ViewBag.HeaderName = "SINGLE CHILD ENTRY";
                    ViewBag.Child = "Single Child";
                    objNotes.NoteId = "";
                    if (NoteId != null)
                    {
                        rosterData.GetNotesDetialByNoteId(ref objNotes, NoteId);
                    }

                    Int64 ClientId = !string.IsNullOrEmpty(EncryptDecrypt.Decrypt64(ChildId)) ? Convert.ToInt64(EncryptDecrypt.Decrypt64(ChildId)) : 0;
                    string ClientName = !string.IsNullOrEmpty(EncryptDecrypt.Decrypt64(ChildName)) ? EncryptDecrypt.Decrypt64(ChildName) : "";
                    objNotes.dictClientDetails.Add(ClientId, ClientName);
                }
                else
                {
                    ViewBag.HeaderName = "MULTI - CHILD ENTRY";
                    ViewBag.Child = "Child List";
                    rosterData.GetChildlistByUserId(ref objNotes);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View(objNotes);
        }

        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult GetElementDetailsByDomainId(string DomainId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtElements = new DataTable();
                rosterData.GetElementDetailsByDomainId(ref dtElements, DomainId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtElements);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult SaveObservationNotes(ObservationNotes objNotes)
        {
            bool Result = false;
            try
            {
                if (objNotes != null)
                {
                    objNotes.UserId = Session["UserID"].ToString();
                    Result = rosterData.SaveObservatioNotes(objNotes);
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }

        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public JsonResult UploadFile(string monitor)
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
                            var _comPath = Server.MapPath("/Content/NotesAttachment/") + _imgname;
                            _imgpath[i] = "/Content/NotesAttachment/" + _imgname;
                            i++;
                            var path = _comPath;

                            // Saving Image in Original Mode
                            pic.SaveAs(path);

                            // resizing image
                            //MemoryStream ms = new MemoryStream();
                            //WebImage img = new WebImage(_comPath);

                            //if (img.Width > 200)
                            //    img.Resize(200, 200);
                            //img.Save(_comPath);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(_imgpath, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCaseNoteByTags(CaseNoteByClientID caseNotes, int IsFromFamilySummary = 0)

        {
            try
            {

                caseNotes = rosterData.GetCaseNoteByTags(caseNotes, IsFromFamilySummary);


            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }

            return PartialView("~/Views/Partialviews/_CaseNoteListPartial.cshtml", caseNotes);

        }
        public ActionResult GetCaseNoteByClientId(CaseNoteByClientID caseNoteByClient)

        {
            CaseNoteByClientID casenote = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<CaseNoteByClientID>();

            try
            {

                casenote = rosterData.GetCaseNoteByClientId(caseNoteByClient);
                List<FingerprintsModel.Role> listRole = Session["RoleList"] as List<FingerprintsModel.Role>;
                casenote.Role = listRole.Select(a => a.RoleName).FirstOrDefault();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(casenote, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCaseNoteByNoteId(string NoteId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtNotes = new DataTable();
                rosterData.GetCaseNoteByNoteId(ref dtNotes, NoteId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtNotes);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);

        }



        public ActionResult GetSubNotes(string CaseNoteId)
        {
            List<SubCaseNote> subList = new List<SubCaseNote>();
            try
            {
                DataTable dtSubNotes = new DataTable();
                subList = rosterData.GetSubNotes(CaseNoteId);

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(subList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadFiles(string monitor)
        {
            bool isInserted = false;
            string CaseNoteId = TempData["CaseNoteid"].ToString();
            string Subcasenoteid = TempData["Subcasenoteid"].ToString();
            string[] _imgpath = new string[System.Web.HttpContext.Current.Request.Files.Count];
            try
            {
                // int i = 0;
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
                            isInserted = rosterData.SaveAttachmentsOnSubNote(CaseNoteId, new BinaryReader(pic.InputStream).ReadBytes(pic.ContentLength), fileName, _ext, Subcasenoteid);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isInserted, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        [JsonMaxLength]
        public ActionResult SaveSubNotes(RosterNew.CaseNote CaseNote)
        {

            bool isResult = false;
            try
            {


                int _householdID = 0;
                int _centerid = 0;
                int _classroomid = 0;

                CaseNote.HouseHoldId = int.TryParse(CaseNote.HouseHoldId, out _householdID) ? CaseNote.HouseHoldId : EncryptDecrypt.Decrypt64(CaseNote.HouseHoldId);
                CaseNote.CenterId = int.TryParse(CaseNote.CenterId, out _centerid) ? _centerid.ToString() : EncryptDecrypt.Decrypt64(CaseNote.CenterId);
                CaseNote.Classroomid = int.TryParse(CaseNote.Classroomid, out _classroomid) ? _classroomid.ToString() : EncryptDecrypt.Decrypt64(CaseNote.Classroomid);

                if (CaseNote.CaseNoteAttachmentList != null)
                {
                    CaseNote.CaseNoteAttachmentList.ForEach(x =>
                    {
                        x.AttachmentFileByte = (!string.IsNullOrEmpty(x.AttachmentJson) ? Convert.FromBase64String(x.AttachmentJson) : new byte[0]);
                    });
                }

                isResult = rosterData.SaveSubNotes(CaseNote);

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }
        [CustAuthFilter()]

        [ValidateInput(false)]
        public ActionResult CaseNotesclient(string id = "")
        {
            CaseNoteByClientID caseNoteByClientID = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<CaseNoteByClientID>();

            try
            {
                string Name = "";
                caseNoteByClientID.Enc_ClientID = !string.IsNullOrEmpty(id) && id != "0" ? id : EncryptDecrypt.Encrypt64("0");
                RosterNew.Users Userlist = new RosterNew.Users();
                caseNoteByClientID.CenterID = !string.IsNullOrEmpty(Request.QueryString["centerid"]) && Request.QueryString["centerid"].ToString() != "0" ? Request.QueryString["centerid"].ToString() : EncryptDecrypt.Encrypt64("0");
                caseNoteByClientID.Enc_HouseholdID = !string.IsNullOrEmpty(Request.QueryString["Householdid"]) && Request.QueryString["Householdid"].ToString() != "0" ? Request.QueryString["Householdid"].ToString() : EncryptDecrypt.Encrypt64("0");
                caseNoteByClientID.RequestedPage = 1;
                caseNoteByClientID.PageSize = 10;
                caseNoteByClientID.SkipRows = caseNoteByClientID.GetSkipRows();
                caseNoteByClientID.SortOrder = "DESC";
                caseNoteByClientID.SortColumn = "th1";
                caseNoteByClientID.IsCaseNoteManager = true;
                caseNoteByClientID = rosterData.GetCaseNote(ref Name, caseNoteByClientID);
                ViewBag.Name = Name;

                if (!string.IsNullOrEmpty(Request.QueryString["Programid"]))
                {
                    caseNoteByClientID.ProgramId = Convert.ToString(EncryptDecrypt.Decrypt64(Request.QueryString["Programid"].ToString()));
                }
                else
                {
                    caseNoteByClientID.ProgramId = "0";
                }

                ViewBag.User = Session["FullName"].ToString();

                List<FingerprintsModel.Role> listRole = Session["RoleList"] as List<FingerprintsModel.Role>;
                ViewBag.Role = listRole.Select(a => a.RoleName).FirstOrDefault();


            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(caseNoteByClientID);
        }


        [HttpPost]
        [JsonMaxLength]
        [CustAuthFilter()]
        [ValidateInput(false)]
        public ActionResult SaveCaseNote(RosterNew.CaseNote caseNote)
        {


            string Name = "";

            string message = rosterData.SaveCaseNotes(ref Name, caseNote);

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetCaseNotes(CaseNoteByClientID caseNotes)
        {

            string Name = "";
            caseNotes = rosterData.GetCaseNote(ref Name, caseNotes);
            return PartialView("~/Views/Partialviews/_CaseNoteListPartial.cshtml", caseNotes);
        }


        [CustAuthFilter()]
        public ActionResult ReferralCategorycompany(string id, string clientName = "")
        {
            try
            {

                //ViewBag.Id = id;
                //ViewBag.clientName = clientName;
                //ViewBag.ScreeningReferralYakkr = String.IsNullOrEmpty(Request.QueryString["ScreeningReferralYakkr"].ToString()) ? EncryptDecrypt.Encrypt64("0") : Request.QueryString["ScreeningReferralYakkr"].ToString();

                REF obj_REF = new REF();


                GetReferralCategoryCompanyList(ref obj_REF, id, clientName);

                return View(obj_REF);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return View(new REF());
            }

        }

        public ActionResult ReferralCategorycompanyPopUp(string id, string clientName = "")
        {

            //int ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(id));
            REF obj_REF = new REF();
            //var refList = new List<REF>();
            //refList = RosterData.ReferralCategoryCompany(ClientId);
            //obj_REF.refListData = refList;
            GetReferralCategoryCompanyList(ref obj_REF, id, clientName);

            return Json(obj_REF.refListData, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult GetReferralCategoryCompanyPartial(string id, string clientName = "")
        {
            REF obj_REF = new REF();
            GetReferralCategoryCompanyList(ref obj_REF, id, clientName);
            return PartialView("~/Views/Partialviews/Referral/_ReferralCategoryCompanyPartial.cshtml", obj_REF);
        }


        public void GetReferralCategoryCompanyList(ref REF _objRef, string _id, string _clientName)
        {
            try
            {
                int ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(_id));

                _objRef.id = _id;
                _objRef.clientName = _clientName;
                _objRef.ScreeningReferralYakkr = Request.QueryString["ScreeningReferralYakkr"] != null && !String.IsNullOrEmpty(Request.QueryString["ScreeningReferralYakkr"].ToString()) ? Request.QueryString["ScreeningReferralYakkr"].ToString() : EncryptDecrypt.Encrypt64("0");
                //var refList = new List<REF>();
                _objRef.refListData = rosterData.ReferralCategoryCompany(ClientId);
                // _objRef.refListData = refList;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
        }

        public ActionResult GetReferralServices()
        {

            List<SelectListItem> selectlist = new List<SelectListItem>();
            selectlist = rosterData.GetServiceReference(staff.AgencyId);
            return Json(selectlist, JsonRequestBehavior.AllowGet);


        }


        [CustAuthFilter()]
        public ActionResult ReferralCategory(ReferralList ReferralCategory)
        {
            REF obj_REF = new REF();

            GetReferralCategoryList(ref obj_REF, ReferralCategory);

            TempData["tempClientId"] = ReferralCategory.id;
            TempData.Keep("tempClientId");
            return View(obj_REF);
        }

        /// <summary>
        /// Referral Catetory Popup
        /// </summary>
        /// <param name="referralClientId"></param>
        /// <returns></returns>

        public JsonResult ReferralCategoryPopup(ReferralList ReferralCategory)
        {
            int clientId = (ReferralCategory.ReferralClientId == null) ? Convert.ToInt32(EncryptDecrypt.Decrypt64(ReferralCategory.id)) : Convert.ToInt32(EncryptDecrypt.Decrypt64(ReferralCategory.id));

            REF obj_REF = new REF();
            var refList = new List<REF>();
            refList = rosterData.ReferralCategory(clientId, ReferralCategory.ReferralClientId, Convert.ToInt32(ReferralCategory.Step));
            obj_REF.refListData = refList;

            long refferralClientId = (string.IsNullOrEmpty(ReferralCategory.ReferralClientId.ToString())) ? 0 : (long)ReferralCategory.ReferralClientId;
            return Json(new { refList, refferralClientId }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult ReferralCategoryPartial(ReferralList ReferralCategory)
        {
            REF referral = new REF();

            GetReferralCategoryList(ref referral, ReferralCategory);

            return PartialView("~/Views/Partialviews/Referral/_ReferralCategoryPartial.cshtml", referral);
        }


        public void GetReferralCategoryList(ref REF referral, ReferralList referralList)
        {


            try
            {
                referral.ParentName = referralList.parentName;
                referral.ReferralClientId = referralList.ReferralClientId;
                referral.id = referralList.id;
                referral.clientName = referralList.clientName;
                referral.ScreeningReferralYakkr = referralList.ScreeningReferralYakkr;

                int ClientId = 0;

                if (referralList.ReferralClientId == null)
                {
                    ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(referralList.id));
                }
                else
                {
                    ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(referralList.id));
                }


                referral.refListData = rosterData.ReferralCategory(ClientId, referralList.ReferralClientId, Convert.ToInt32(referralList.Step));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

        }


        public ActionResult HouseHoldReferrals(long referralClientId)
        {
            List<SelectListItem> referrals = new List<SelectListItem>();
            referrals = rosterData.GetSelectedReferrals(referralClientId);
            return Json(referrals, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool SaveReferralClient(ListRoster SaveReferral)
        {
            try
            {
                bool Success = false;
                long CommanClientId_ = Convert.ToInt32(EncryptDecrypt.Decrypt64(SaveReferral.CommonClientId));
                Int32 Step = 2;
                bool Status = true;
                int CreatedBy = Convert.ToInt32(CommanClientId_);
                string[] serviceID_Array = SaveReferral.ServiceId.Split(',');

                List<long> referralIdentity = new List<long>();

                long screeningReferral = 0;
                long.TryParse(string.IsNullOrEmpty(SaveReferral.ScreeningReferralYakkr) ? "0" : EncryptDecrypt.Decrypt64(SaveReferral.ScreeningReferralYakkr), out screeningReferral);



                foreach (var item in serviceID_Array)
                {
                    long referralId = rosterData.SaveReferralClient(Convert.ToInt32(item), CommanClientId_, new Guid(SaveReferral.AgencyId), Step, Status, CreatedBy, SaveReferral.referralClientId, screeningReferralYakkr: screeningReferral);
                    referralIdentity.Add(referralId);

                }
                string querycommand = (SaveReferral.referralClientId > 0) ? "UPDATE" : "INSERT";

                if (SaveReferral.referralClientId == 0)
                {


                    string[] ClientId_Array = SaveReferral.ClientId.Split(',');

                    foreach (var id in referralIdentity)
                    {
                        int count = 0;
                        foreach (var item in ClientId_Array)
                        {
                            Success = rosterData.SaveHouseHold(Convert.ToInt32(item), CommanClientId_, Step, Status, Convert.ToInt32(SaveReferral.HouseHoldId), Convert.ToInt64(id), querycommand);
                            count++;
                        }
                    }
                }
                else
                {
                    Success = rosterData.SaveHouseHold(0, CommanClientId_, Step, Status, Convert.ToInt32(SaveReferral.HouseHoldId), SaveReferral.referralClientId, querycommand, SaveReferral.ClientId);
                }

                return Success;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                //  throw ex;
                return false;
            }
        }

        [CustAuthFilter()]
        public bool SaveMatchProviders(ListRoster SaveProvider)
        {
            bool Success = false;
            try
            {
                TempData.Keep("tempClientId");
                string cId = (string.IsNullOrEmpty(SaveProvider.ClientId)) ? TempData["tempClientId"].ToString() : SaveProvider.ClientId;
                SaveProvider.Description = (SaveProvider.Description == null) ? SaveProvider.Description = "" : SaveProvider.Description;

                SaveProvider.ClientId = EncryptDecrypt.Decrypt64<long>(cId).ToString();

                Success = rosterData.SaveMatchProviders(SaveProvider);

                if (Success)
                {

                    // RosterData.YakkarInsert(SaveProvider.AgencyId, UserId, ClientId);

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Success;
        }

        [CustAuthFilter()]
        public bool SaveReferral(ListRoster Savereferral)
        {
            bool Success = false;
            try
            {
                TempData.Keep("tempClientId");
                string cId = (string.IsNullOrEmpty(Savereferral.CommonClientId)) ? TempData["tempClientId"].ToString() : Savereferral.CommonClientId;
                int commanclient = Convert.ToInt32(EncryptDecrypt.Decrypt64(cId));
                Savereferral.CommonClientId = EncryptDecrypt.Decrypt64(cId);

                int Step = 3;

                long referenceID = 0;

                referenceID = rosterData.SaveReferral(Savereferral);



                string[] ClientId_Array = Savereferral.ClientId.Split(',');
                int count = 0;

                if (referenceID > 0)
                {
                    foreach (var item in ClientId_Array)
                    {
                        Success = rosterData.SaveHouseHold(Convert.ToInt32(item), commanclient, Step, true, Convert.ToInt32(Savereferral.HouseHoldId), referenceID, "INSERT");
                        count++;
                    }
                }
                else
                {
                    Success = false;
                }


                //yakkr450 insert moved to sp_ReferalOperations SP
                //  RosterData.YakkarInsert(Savereferral.AgencyId, UserId, commanclient);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Success;
        }

        public bool DeleteReferralService(long ReferralClientServiceId)
        {
            bool Success = false;
            Success = rosterData.DeleteReferralService(ReferralClientServiceId);
            return true;
        }


        public ActionResult FamilyNeeds()
        {
            return View();
        }

        public ActionResult LoadSurveyOptions(long ReferralClientId)
        {
            string userId = Session["UserID"].ToString();
            var surveyList = rosterData.LoadSurveyOptions(ReferralClientId);
            if (surveyList != null && surveyList.Count > 0)
            {
                return Json(surveyList, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertSurveyOptions(long ReferralClientId, bool isUpdate, string surveyoptions = "")
        {
            dynamic distanceJsonResult = surveyoptions;
            List<SurveyOptions> surveyOptions = JsonConvert.DeserializeObject<List<SurveyOptions>>(distanceJsonResult);
            rosterData.InsertSurveyOptions(surveyOptions, ReferralClientId, isUpdate);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]

        public ActionResult MatchProviders(ListRoster MatchProvider, string CommunityIds, string stepId = "")
        {
            MatchProviderModel obj_MPM = new MatchProviderModel();
            try
            {




                GetMatchProvidersList(ref obj_MPM, MatchProvider, CommunityIds);

                int _step = 0;
                int.TryParse(stepId, out _step);

                obj_MPM.Step = _step;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(obj_MPM);
        }


        /// <summary>
        /// Match Providers Pop-Up
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ClientName"></param>
        /// <returns></returns>

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult MatchProvidersPopUp(string id, string parentName, int referralClientId, string clientName, string CommunityIds, string stepId = "")
        {
            ViewBag.ReferralClientId = 0;

            string AgencyId = Session["AgencyID"].ToString();
            MatchProviderModel obj_MPM = new MatchProviderModel();
            var matchProvidersList = new List<MatchProviderModel>();
            List<SelectListItem> OrganizationList = new List<SelectListItem>();
            matchProvidersList = rosterData.MatchProviders(CommunityIds, referralClientId);
            obj_MPM.ParentName = parentName;
            obj_MPM.AgencyId = AgencyId;
            obj_MPM.MPMList = matchProvidersList;
            if (matchProvidersList != null && matchProvidersList.Count() > 0)
            {
                OrganizationList = rosterData.FamilyServiceList(Convert.ToInt64(matchProvidersList.FirstOrDefault().ServiceId));

            }

            obj_MPM.OrganizationList = OrganizationList;
            return Json(obj_MPM, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult GetMatchProvidersPartial(ListRoster MatchProvider, string CommunityIds, string stepId = "")
        {
            MatchProviderModel objModel = new MatchProviderModel();

            GetMatchProvidersList(ref objModel, MatchProvider, CommunityIds);
            int stepid = 0;
            objModel.Step = int.TryParse(stepId, out stepid) ? stepid : stepid;

            return PartialView("~/Views/Partialviews/Referral/_MatchProvidersPartial.cshtml", objModel);
        }


        public void GetMatchProvidersList(ref MatchProviderModel matchProvidersModel, ListRoster matchProvider, string communityIds)
        {
            try
            {

                if (matchProvider.AgencyId != "" || matchProvider.AgencyId != null)
                {

                    matchProvidersModel.ReferralClientId = matchProvider.referralClientId;
                }

                matchProvidersModel.clientName = matchProvider.clientName;
                matchProvidersModel.id = matchProvider.id;
                matchProvidersModel.ScreeningReferralYakkr = matchProvider.ScreeningReferralYakkr;

                TempData.Keep("tempClientId");

                var matchProvidersList = new List<MatchProviderModel>();
                List<SelectListItem> OrganizationList = new List<SelectListItem>();
                matchProvidersList = rosterData.MatchProviders(communityIds, matchProvider.referralClientId);
                matchProvidersModel.ParentName = matchProvider.parentName;
                matchProvidersModel.AgencyId = matchProvider.AgencyId;
                matchProvidersModel.MPMList = matchProvidersList;
                matchProvidersModel.ScreeningReferralYakkr = matchProvider.ScreeningReferralYakkr;
                if (matchProvidersList != null && matchProvidersList.Count() > 0)
                {
                    OrganizationList = rosterData.FamilyServiceList(Convert.ToInt32(matchProvidersList.FirstOrDefault().ServiceId));

                }

                matchProvidersModel.OrganizationList = OrganizationList;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
        }

        [CustAuthFilter()]
        public ActionResult ReferralService(string id, string ClientName = "")
        {


            ViewBag.ScreeningReferralYakkr = Request.QueryString["scrYakkr"] != null && !string.IsNullOrEmpty(Request.QueryString["scrYakkr"].ToString()) ? Request.QueryString["scrYakkr"].ToString() : EncryptDecrypt.Encrypt64("0");
            int takeRecords = 10;
            int skipRecords = 0;
            ReferralServiceModel referralService = GetReferralServiceList(id, takeRecords, skipRecords, _yakkrCode: 0);
            referralService.ClientName = ClientName;
            referralService._EncClientId = id;
            TempData["tempClientId"] = id;
            TempData.Keep("tempClientId");
            return View(referralService);

        }

        /// <summary>
        /// Referral Service for Matrix Recommendations-Popup
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="AgencyId"></param>
        /// <returns></returns>
        /// 
        public JsonResult ReferralServicePopUp(string ClientId, string ClientName = "")
        {

            var ReferralList = new List<ReferralServiceModel>();
            long clientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(ClientId));
            ReferralServiceModel referralService = GetReferralServiceList(ClientId, _takeRecords: 1000, _skipRecords: 0, _yakkrCode: 0);
            ReferralList = referralService.referralserviceList;
            return Json(ReferralList, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Referral Service for Matrix Recommendations-Popup
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="AgencyId"></param>
        /// <returns></returns>
        /// 
        public PartialViewResult GetReferralServiceListPartial(string clientId, int pageSize, int requestedPage, int yakkrCode = 0)
        {

            var ReferralList = new List<ReferralServiceModel>();
            long _clientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientId));
            int skip = pageSize * (requestedPage - 1);
            skip = (skip < 0) ? 0 : skip;

            ReferralServiceModel referralService = GetReferralServiceList(clientId, pageSize, skip, _yakkrCode: yakkrCode);
            return PartialView("~/Views/Partialviews/Referral/_ReferralServiceListPartial.cshtml", referralService);
        }

        /// <summary>
        /// Gets Referral Service Partial View Page
        /// </summary>
        /// <param name="_clientId"></param>
        /// <param name="_takeRecords"></param>
        /// <param name="_skipRecords"></param>
        /// <returns></returns>

        [CustAuthFilter()]
        [HttpPost]
        public PartialViewResult GetReferralServicePartial(string clientId, int pageSize, int requestedPage)
        {

            var ReferralList = new List<ReferralServiceModel>();

            int skip = pageSize * (requestedPage - 1);
            skip = (skip < 0) ? 0 : skip;

            ReferralServiceModel referralService = GetReferralServiceList(clientId, pageSize, skip, _yakkrCode: 601);
            return PartialView("~/Views/Partialviews/Referral/_ReferralServicePartial.cshtml", referralService);
        }


        public ReferralServiceModel GetReferralServiceList(string _encClientId, int _takeRecords, int _skipRecords, int _yakkrCode)
        {

            int _totalRecords = 0;
            ReferralServiceModel referralServiceModel = new ReferralServiceModel();
            long _clientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(_encClientId));
            referralServiceModel.referralserviceList = rosterData.ReferralService(ref _totalRecords, _clientId, _takeRecords, _skipRecords, _yakkrCode);
            referralServiceModel.TotalRecords = _takeRecords;
            referralServiceModel._EncClientId = _encClientId;
            referralServiceModel.ClientId = _clientId;
            return referralServiceModel;
        }





        /// <summary>
        /// Gets the 
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="AgencyId"></param>
        /// <returns></returns>
        public ActionResult FamilyResourcesList(int serviceId)
        {
            try
            {

                List<SelectListItem> listOrganization = new List<SelectListItem>();
                listOrganization = rosterData.FamilyServiceList(serviceId);
                return Json(new { listOrganization }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }
        }

        public ActionResult QualityOfReferral()
        {
            return View();
        }

        public ActionResult FamilyServiceListCompany(Int32 ServiceId)
        {
            try
            {
                List<SelectListItem> listOrganization = new List<SelectListItem>();
                listOrganization = rosterData.FamilyServiceListCompany(ServiceId);
                return Json(new { listOrganization }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }
        }

        public ActionResult GetReferralType(int communityId)
        {
            List<SelectListItem> referralType = new List<SelectListItem>();

            referralType = rosterData.GetReferralType(communityId);
            return Json(referralType, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrganization(Int32 CommunityId)
        {
            try
            {
                MatchProviderModel matchProviderModel = new MatchProviderModel();
                matchProviderModel = rosterData.GetOrganization(CommunityId);
                var jsonResult = Json(matchProviderModel, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }
        }

        public ActionResult GetOrganizationCompany(Int32 CommunityId)
        {
            try
            {
                REF refOrg = new REF();
                refOrg = rosterData.GetOrganizationCompany(CommunityId);
                var jsonResult = Json(refOrg, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }
        }
        //Changes on 29Dec2016
        [CustAuthFilter()]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CaseNotesclient(RosterNew.CaseNote CaseNote, RosterNew.ClientUsers ClientIds, RosterNew.ClientUsers TeamIds, List<RosterNew.Attachment> Attachments, int Mode = 1)
        {
            try
            {


                List<FingerprintsModel.Role> listRole = Session["RoleList"] as List<FingerprintsModel.Role>;

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

                CaseNote.CaseNoteAttachmentList = new List<RosterNew.Attachment>();
                CaseNote.CaseNoteAttachmentList = Attachments;

                CaseNote.CaseNotetags = (CaseNote != null && !string.IsNullOrEmpty(CaseNote.CaseNotetags)) ? CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Length - 1) : "";

                string Name = "";
                if (CaseNote.HouseHoldId != "0")
                {
                    CaseNote.HouseHoldId = EncryptDecrypt.Decrypt64(CaseNote.HouseHoldId);
                }
                CaseNote.IsLateArrival = false;


                if (CaseNote.CaseNoteAttachmentList != null && CaseNote.CaseNoteAttachmentList.Count > 0)
                {
                    CaseNote.CaseNoteAttachmentList.ForEach(x =>
                    {
                        x.AttachmentFileByte = x.AttachmentJson != null && x.AttachmentJson != "" ? Convert.FromBase64String(x.AttachmentJson.Replace("data:image/png;base64,", string.Empty)) : new byte[0];
                        x.AttachmentFileExtension = ".png";
                        x.AttachmentFileName = "CaseNoteAttachment";

                    });
                }

                string message = rosterData.SaveCaseNotes(ref Name, CaseNote, Mode);

                if (message == "1")
                {
                    TempData["CaseNoteMessage"] = "Case Note saved successfully.";
                }
                else
                {
                    TempData["CaseNoteMessage"] = "Please try again.";
                }

                if (Mode == 3)
                {
                    return Redirect("/AgencyUser/FamilySummary/" + ViewBag.Householdid);
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return RedirectToAction("CaseNotesclient", "Roster", new { @id = EncryptDecrypt.Encrypt64(CaseNote.ClientId).Trim(), @Programid = EncryptDecrypt.Encrypt64(CaseNote.ProgramId), @centerid = EncryptDecrypt.Encrypt64(CaseNote.CenterId), @Householdid = CaseNote.HouseHoldId });
        }
        //Changes on 29Dec2016
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [CustAuthFilter()]
        public ActionResult Getcasenotedetails(string caseNoteId, string householdId = "", string clientId = "")
        {
            //CaseNoteByClientID caseNoteByClientId = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<CaseNoteByClientID>();
            CaseNoteByClientID casenotebyClientId = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<CaseNoteByClientID>();

            try
            {
                long _tryTest = 0;


                // var caseNoteList = RosterData.GetcaseNoteDetail(Casenoteid, ClientId, staff);

                //caseNoteByClientId = RosterData.GetDevelopmentalMembersByClientID(Convert.ToInt64(ClientId), staff);

                //caseNoteByClientId.CaseNoteList = caseNoteList;

                // return Json(RosterData.GetcaseNoteDetail(Casenoteid, ClientId, staff));

                casenotebyClientId.CaseNote = new CaseNote
                {
                    CaseNoteid = long.TryParse(caseNoteId, out _tryTest) ? caseNoteId : EncryptDecrypt.Decrypt64(caseNoteId),
                    ClientId = long.TryParse(clientId, out _tryTest) ? clientId : EncryptDecrypt.Decrypt64(clientId),
                    HouseHoldId = long.TryParse(householdId, out _tryTest) ? householdId : EncryptDecrypt.Decrypt64(householdId)
                };


                casenotebyClientId = rosterData.GetCaseNoteByCaseNoteId(casenotebyClientId);

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);

            }
            return PartialView("~/Views/partialviews/_CaseNotePopup.cshtml", casenotebyClientId);


            //  return PartialView("~/Views/Partialviews/_CaseNote.cshtml", caseNoteByClientId);
        }


        [CustAuthFilter()]
        public ActionResult GetcasenotedetailByCaseNoteId(string Casenoteid, string ClientId = "")
        {

            try
            {

                return Json(rosterData.GetcaseNoteDetail(Casenoteid, ClientId));



            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred. Please, try again later.", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [CustAuthFilter()]
        public ActionResult GetCaseNoteSectionPartial(string caseNoteId, string householdId = "", string clientId = "")
        {
            CaseNoteByClientID casenotebyClientId = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<CaseNoteByClientID>();
            try
            {



                casenotebyClientId.CaseNote = new CaseNote();
                casenotebyClientId.CaseNote.CaseNoteid = EncryptDecrypt.Decrypt64<long>(caseNoteId).ToString();
                casenotebyClientId.CaseNote.ClientId = EncryptDecrypt.Decrypt64<long>(clientId).ToString();

                casenotebyClientId.CaseNote.HouseHoldId = EncryptDecrypt.Decrypt64<long>(householdId).ToString();


                casenotebyClientId = rosterData.GetCaseNoteByCaseNoteId(casenotebyClientId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return PartialView("~/Views/partialviews/_CaseNote.cshtml", casenotebyClientId);
        }






        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetElement(string DomainId = "0")
        {
            try
            {
                return Json(rosterData.GetElementInfo(DomainId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult SendEmail(string FPAID, string ClientId, string ParentName = "0", string ChildName = "0", string FSWName = "0", string ParentEmail = "0", string Goal = "0")
        {
            try
            {
                // string link
                FSWName = Session["FullName"].ToString();
                string link = UrlExtensions.LinkToRegistrationProcess("/Roster/FPAParent?id=" + ClientId + "&FPAid=" + FPAID);
                return Json(SendMail.SendFPAStepsEmail(ParentName, ChildName, FSWName, ParentEmail, Goal, Server.MapPath("~/MailTemplate"), link));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetFPAInfo(string FPAID = "0")
        {
            try
            {
                ViewBag.mode = 1;
                return Json(rosterData.GetFPADetails(FPAID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult DeleteStepView(string StepId)
        {
            try
            {
                ViewBag.mode = 1;
                return Json(rosterData.DeleteStepView(StepId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again after some time.");
            }
        }
        // delFPAInfo
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [HttpPost]
        public JsonResult delFPAInfo(string FPAID)
        {
            try
            {
                string strfpaid = FingerprintsModel.EncryptDecrypt.Decrypt64(FPAID);
                ViewBag.mode = 1;
                object Delete = (rosterData.DeleteFPA(strfpaid));
                return Json(Delete, JsonRequestBehavior.AllowGet);


            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again after some time.");
            }
        }
        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult listFPA(string search, String ClientId, string sortOrder, string sortDirection, int pageSize, int requestedPage = 1)
        {
            try
            {
                //  int skip = pageSize * (requestedPage - 1);
                //string totalrecord="Totalrecord";
                // List<FPA> list = RosterData.GetFPAGoalListForHousehold(ClientId);
                string clientid = FingerprintsModel.EncryptDecrypt.Decrypt64(ClientId);
                int skip = pageSize * (requestedPage - 1);
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.TrimEnd().TrimStart();
                }
                else { }
                string totalrecord;
                var list = rosterData.GetFPAGoalListForHousehold(out totalrecord, search, clientid, sortOrder, sortDirection, skip, pageSize);

                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult FPAList()
        {
            FPA obj = new FingerprintsModel.FPA();
            // string Householdid = ""; string centerid = ""; string Programid = "";
            if (Request.QueryString["id"] != null && !string.IsNullOrEmpty(Request.QueryString["id"].ToString()))
            {
                TempData["ClientId"] = Request.QueryString["id"].ToString();
                obj.ClientId = Convert.ToInt64(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["id"].ToString()));
                if (Request.QueryString.AllKeys.Contains("ClientName"))
                {
                    TempData["clientName"] = Request.QueryString["ClientName"].ToString();
                }
                // ViewBag.ChildName = Request.QueryString["ForClient"].ToString();
                obj.EncyrptedClientId = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.ClientId.ToString());
            }
            else if (TempData["ClientId"] != null)
            {
                TempData["ClientId"] = TempData["ClientId"].ToString();
                obj.ClientId = Convert.ToInt64(FingerprintsModel.EncryptDecrypt.Decrypt64(TempData["ClientId"].ToString()));
                if (TempData["clientName"] != null)
                {
                    TempData["clientName"] = Request.QueryString["ClientName"].ToString();
                }
                // ViewBag.ChildName = Request.QueryString["ForClient"].ToString();
                obj.EncyrptedClientId = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.ClientId.ToString());
            }
            if (Request.QueryString["FPA"] != null && !string.IsNullOrEmpty(Request.QueryString["FPA"].ToString()))
            {
                try
                {
                    Export export = new Export();

                    obj.FPAID = Convert.ToInt64(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["FPA"].ToString()));
                    obj = rosterData.GetFpa(Convert.ToInt64(obj.FPAID));
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.pdf",
                        "Family Partnership Agreement " + DateTime.Now.ToString("MM/dd/yyyy")));
                    string encriptedclid = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.ClientId.ToString());
                    string encriptedfpaid = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.FPAID.ToString());
                    Stream strpdf = Response.OutputStream;
                    export.ExportPdf2(obj, strpdf, Server.MapPath("~/Content/img/logo_email.png"));
                    // Response.Redirect("~/Roster/FPA/?id=" + encriptedclid + "&FPAid=" + encriptedfpaid);
                    Response.End();
                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }
                finally
                {
                }
            }
            return View(obj);
        }




        public FPA GetFPA()
        {
            try
            {
                string fpaId = FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["FPAid"].ToString());
                string clientId = FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["id"].ToString());
                int iFPAid = Convert.ToInt32(fpaId);
                var FPADATA = rosterData.GetFpa(iFPAid);
                return FPADATA;
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return null;
            }
        }



        //Added by santosh for getting goal and  steps both
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult FPA()
        {
            FingerprintsModel.FPA obj = new FingerprintsModel.FPA();
            obj = rosterData.GetData_AllDropdown();
            ViewBag.CateList = obj.cateList;
            TempData["CateList"] = ViewBag.CateList;
            ViewBag.DomList = obj.domList;
            // TempData["DomList"] = ViewBag.DomList;

            if (Request.QueryString.AllKeys.Contains("FPAid") && !string.IsNullOrEmpty(Request.QueryString["FPAid"].ToString()))
            {
                obj.FPAID = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["FPAid"].ToString()));
                obj = rosterData.GetFpa(obj.FPAID);
                obj.EncriptedFPAID = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.FPAID.ToString());
            }
            else
            {
                obj.GoalFor = 0;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["id"].ToString()))
            {
                obj.ClientId = Convert.ToInt64(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["id"].ToString()));
                obj.EncyrptedClientId = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.ClientId.ToString());
                if (obj.FPAID <= 0)
                {
                    DataSet ds = rosterData.getParentNames(obj.ClientId);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            obj.ParentName1 = ds.Tables[0].Rows[0]["parentName"].ToString();
                            obj.ParentName2 = ds.Tables[0].Rows[1]["parentName"].ToString();
                        }
                        else
                        {
                            obj.ParentName1 = ds.Tables[0].Rows[0]["parentName"].ToString();
                        }

                        obj.ParentName1 = ds.Tables[1].Rows[0]["Parentname1"].ToString();
                        obj.ParentEmailId1 = ds.Tables[1].Rows[0]["parentEmail1"].ToString();
                        obj.ParentName2 = ds.Tables[1].Rows[0]["Parentname2"].ToString();
                        obj.ParentEmailId2 = ds.Tables[1].Rows[0]["parentEmail2"].ToString();
                        obj.ChildName = ds.Tables[1].Rows[0]["childName"].ToString();
                        obj.IsEmail1 = ds.Tables[1].Rows[0]["noEmail1"].ToString().TrimEnd().TrimStart() == "True" ? false : true;
                        obj.IsEmail2 = ds.Tables[1].Rows[0]["noEmail2"].ToString().TrimEnd().TrimStart() == "True" ? false : true;
                        obj.IsSingleParent = ds.Tables[1].Rows[0]["IsSingleParent"].ToString().TrimEnd().TrimStart() == "1" ? true : false;
                    }
                }
            }
            if (obj.FPAID == 0)
            {
                ViewBag.mode = 0;
            }
            else
            {
                ViewBag.mode = 1;
            }
            return View(obj);
        }

        public object GetFPAForMatrix(string clientId)
        {
            FPA obj = new FPA();
            FPA objNew = new FingerprintsModel.FPA();

            try
            {
                obj.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));
                obj.EncyrptedClientId = EncryptDecrypt.Encrypt64(obj.ClientId.ToString());

                objNew = rosterData.GetData_AllDropdown();
                DataSet ds = rosterData.getParentNames(obj.ClientId);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        obj.ParentName1 = ds.Tables[0].Rows[0]["parentName"].ToString();
                        obj.ParentName2 = ds.Tables[0].Rows[1]["parentName"].ToString();
                    }
                    else
                    {
                        obj.ParentName1 = ds.Tables[0].Rows[0]["parentName"].ToString();
                    }

                    obj.ParentName1 = ds.Tables[1].Rows[0]["Parentname1"].ToString();
                    obj.ParentEmailId1 = ds.Tables[1].Rows[0]["parentEmail1"].ToString();
                    obj.ParentName2 = ds.Tables[1].Rows[0]["Parentname2"].ToString();
                    obj.ParentEmailId2 = ds.Tables[1].Rows[0]["parentEmail2"].ToString();
                    obj.ChildName = ds.Tables[1].Rows[0]["childName"].ToString();
                    obj.IsEmail1 = ds.Tables[1].Rows[0]["noEmail1"].ToString().TrimEnd().TrimStart() == "True" ? false : true;
                    obj.IsEmail2 = ds.Tables[1].Rows[0]["noEmail2"].ToString().TrimEnd().TrimStart() == "True" ? false : true;
                    obj.IsSingleParent = ds.Tables[1].Rows[0]["IsSingleParent"].ToString().TrimEnd().TrimStart() == "1" ? true : false;
                }
                obj.cateList = objNew.cateList;
                obj.domList = objNew.domList;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
            // return obj;
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [HttpPost]
        public ActionResult FPA(string command, FingerprintsModel.FPA info, FormCollection collection, List<FingerprintsModel.FPASteps> GoalSteps)
        {
            FPA obj = new FPA();
            if (!string.IsNullOrEmpty(command))
            {
                try
                {
                    Export export = new Export();
                    obj = rosterData.GetData_AllDropdown();
                    obj.GoalFor = Convert.ToInt32(Request.Form["GoalFor"].ToString());
                    ViewBag.DomList = obj.domList;
                    TempData["DomList"] = ViewBag.DomList;
                    ViewBag.CateList = obj.cateList;
                    TempData["CateList"] = ViewBag.CateList;

                    obj = info;
                    //  string fpaid = FingerprintsModel.EncryptDecrypt.Decrypt64(info.FPAID);
                    obj = rosterData.GetFpa(Convert.ToInt32(info.FPAID));
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", String.Format(@"attachment;filename={0}.pdf",
                        "Family Partnership Agreement " + DateTime.Now.ToString("MM/dd/yyyy")));
                    string encriptedclid = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.ClientId.ToString());
                    string encriptedfpaid = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.FPAID.ToString());
                    Stream strpdf = Response.OutputStream;
                    export.ExportPdf2(obj, strpdf, Server.MapPath("~/Content/img/logo_email.png"));
                    // Response.Redirect("~/Roster/FPA/?id=" + encriptedclid + "&FPAid=" + encriptedfpaid);
                    Response.End();
                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }
                finally
                {
                }
            }
            else
            {

                obj = rosterData.GetData_AllDropdown();
                obj.GoalFor = Convert.ToInt32(Request.Form["GoalFor"].ToString());
                ViewBag.DomList = obj.domList;
                TempData["DomList"] = ViewBag.DomList;
                ViewBag.CateList = obj.cateList;
                TempData["CateList"] = ViewBag.CateList;

                obj = info;
                if (GoalSteps != null && GoalSteps.Count > 0)
                {

                    foreach (var item in GoalSteps)
                    {
                        if (!string.IsNullOrEmpty(item.Description))
                        {
                            obj.GoalSteps.Add(item);
                        }
                    }
                }
                if (obj.GoalFor == 0)
                {
                    obj.GoalFor = 1;
                }
                if (obj.IsSingleParent)
                {
                    obj.GoalFor = 1;
                }
                string message = "";
                // int Mode = 2;
                try
                {
                    info.Category = (collection["DdlCateList"] == null) ? null : collection["DdlCateList"].ToString();
                    info.Domain = (collection["DdlDomList"] == null) ? null : collection["DdlDomList"].ToString();
                    if (info.FPAID > 0)
                    {
                        //Update Data
                        // string UpdateParameter = "UPDATE";
                        message = rosterData.AddFPA(info, 1);
                        // objdata.CheckByClient(UpdateParameter, Mode);
                    }
                    else
                    {
                        //Insert Data
                        //    string InsertParameter = "INSERT";
                        message = rosterData.AddFPA(info, 0);
                        // objdata.CheckByClient(InsertParameter, Mode);
                    }
                    ViewBag.result = "Success";
                    if (message.Contains("1_"))
                    {
                        ViewBag.result = "Success";
                        TempData["message"] = "Record added successfully.";
                        string[] arr = message.Split('_');
                        string ClientId = EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId));
                        if (arr.Length > 1)
                        {
                            string FPAId = Convert.ToString(arr[1]);
                            return Redirect("~/Roster/FPAList?id=" + EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId)));
                        }
                    }
                    else if (message == "2")
                    {
                        ViewBag.result = "Success";
                        TempData["message"] = "Record updated successfully.";
                        string ClientId = EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId));
                        string FPAId = Convert.ToString(info.FPAID);
                        return Redirect("~/Roster/FPAList?id=" + EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId)));
                    }
                    else if (message == "3")
                    {
                        ViewBag.message = "FPA already exist.";
                    }
                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }
            }
            return View(obj);
        }

        //Added on 27Dec2016

        public ActionResult FPAParent()
        {
            FingerprintsModel.FPA obj = new FingerprintsModel.FPA();
            obj = rosterData.GetData_AllDropdown();
            ViewBag.CateList = obj.cateList;
            TempData["CateList"] = ViewBag.CateList;
            ViewBag.DomList = obj.domList;

            if (Request.QueryString.AllKeys.Contains("FPAid") && !string.IsNullOrEmpty(Request.QueryString["FPAid"].ToString()))
            {
                obj.FPAID = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["FPAid"].ToString()));
                obj = rosterData.GetFpaforParents(obj.FPAID);
                obj.EncriptedFPAID = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.FPAID.ToString());
            }
            else
            {
                obj.GoalFor = 0;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["id"].ToString()))
            {
                obj.ClientId = Convert.ToInt64(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["id"].ToString()));
                obj.EncyrptedClientId = FingerprintsModel.EncryptDecrypt.Encrypt64(obj.ClientId.ToString());
                DataSet ds = rosterData.getParentNames(obj.ClientId);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        obj.ParentName1 = ds.Tables[0].Rows[0]["parentName"].ToString();
                        obj.ParentName2 = ds.Tables[0].Rows[1]["parentName"].ToString();
                    }
                    else
                    {
                        obj.ParentName1 = ds.Tables[0].Rows[0]["parentName"].ToString();
                    }
                }
            }
            if (obj.FPAID == 0)
            {
                ViewBag.mode = 0;
            }
            else
            {
                ViewBag.mode = 1;
            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult FPAParent(FingerprintsModel.FPA info, FormCollection collection, List<FingerprintsModel.FPASteps> GoalSteps)
        {

            FPA obj = new FPA();

            obj = rosterData.GetData_AllDropdown();
            obj = rosterData.GetFpa(info.FPAID);
            // obj.GoalFor = Convert.ToInt32(Request.Form["GoalFor"].ToString());
            obj.GoalSteps.Clear();
            ViewBag.DomList = obj.domList;
            TempData["DomList"] = ViewBag.DomList;
            ViewBag.CateList = obj.cateList;
            TempData["CateList"] = ViewBag.CateList;
            obj.GoalStatus = info.GoalStatus;
            obj.CompletionDate = info.CompletionDate;
            obj.GoalStatus = info.GoalStatus;
            obj.ActualGoalCompletionDate = info.ActualGoalCompletionDate;
            if (GoalSteps != null && GoalSteps.Count > 0)
            {

                foreach (var item in GoalSteps)
                {
                    if (item.StepID > 0)
                    {
                        obj.GoalSteps.Add(item);
                    }
                }
            }

            string message = "";
            try
            {
                info.Category = collection["DdlCateList"]?.ToString();
                info.Domain = collection["DdlDomList"]?.ToString();

                message = rosterData.UpdateFPAParent(obj);//, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());

                message = rosterData.AddFPA(info, 1);
                ViewBag.result = "Success";

                if (message.Contains("1_"))
                {
                    ViewBag.result = "Sucess";
                    TempData["message"] = "Record added successfully.";
                    string[] arr = message.Split('_');
                    string ClientId = EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId));
                    if (arr.Length > 1)
                    {
                        string FPAId = Convert.ToString(arr[1]);
                        return Redirect("~/Login/Loginagency");//?id=" + EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId)));
                    }
                }
                else if (message == "2")
                {
                    ViewBag.result = "Sucess";
                    TempData["message"] = "Record updated successfully.";
                    string ClientId = EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId));
                    string FPAId = Convert.ToString(info.FPAID);
                    return Redirect("~/Login/Loginagency");//?id=" + EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId)));
                }
                else if (message == "3")
                {
                    ViewBag.message = "FPA already exist.";
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(obj);
        }


        /// <summary>
        /// Save FPA PopUp 
        /// </summary>
        /// <param name="Casenoteid"></param>
        /// <returns></returns>
        [HttpPost]
        // public JsonResult SaveFbaForParentMatrix(string ClientId, string Goal, int Category, string GoalDate, int GoalStatus,int GoalFor, string CompletionDate,string stepsList)
        public JsonResult SaveFbaForParentMatrix(string ClientId, string infoString)
        {
            try
            {
                FPA fpaConv = JsonConvert.DeserializeObject<FPA>(infoString);


                FPA info = new FPA();
                long clientId = Convert.ToInt64(FingerprintsModel.EncryptDecrypt.Decrypt64(ClientId));
                info = fpaConv;
                info.ClientId = clientId;
                //info.FPAID = 0;
                //info.Goal = Goal;
                //info.GoalDate = GoalDate;
                //info.ClientId = clientId;
                //info.CompletionDate = CompletionDate;
                //info.Element = null;
                //info.Domain = null;
                //info.Category = Category.ToString();
                //info.GoalStatus = GoalStatus;
                //info.GoalFor = (GoalFor==0)?1:GoalFor;

                //FPASteps fbaStep = new FPASteps();
                //fbaStep.Description = DynamicDesc;
                //fbaStep.Status = DynamicStatus;
                //fbaStep.StepsCompletionDate = DynamicCompletionDate;
                //fbaStep.Email = false;
                //fbaStep.StepID = 0;
                //fbaStep.Reminderdays = DynamicRemainder;
                //info.GoalSteps.Add(fbaStep);
                //if (fpaSteps != null && fpaSteps.Count > 0)
                //{

                //    foreach (var item in fpaSteps)
                //    {
                //        if (!string.IsNullOrEmpty(item.Description))
                //        {
                //            info.GoalSteps.Add(item);
                //        }
                //    }
                //}

                string message = rosterData.AddFPA(info, 0);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json("Record saved successfully.");
        }

        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetGroupCaseNoteDetails(string Casenoteid)
        {
            try
            {
                return Json(rosterData.GetgroupcaseNoteDetail(Casenoteid));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        public void CompleteServicePdf(string ServiceId, string AgencyID, string ClientID, string CommunityID, string Notes, string referralDate)
        {
            var ReferredName = Session["FullName"];
            string clientId = EncryptDecrypt.Decrypt64(ClientID).ToString();
            PDFGeneration obj_REF = new PDFGeneration();
            var refList = new List<PDFGeneration>();
            refList = rosterData.CompleteServicePdf(clientId);

            var refList1 = new List<CompanyDetails>();
            refList1 = rosterData.CompanyDetailsList(ServiceId);

            var refList2 = new List<CommunityDetails>();
            refList2 = rosterData.CommunityDetailsList(CommunityID);

            var refList3 = new List<BusinessHours>();
            refList3 = rosterData.BusinessHoursList(ServiceId, CommunityID);
            string RefdataBody = string.Empty;
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/ServicePdf/ServicePdf.html"));

            RefdataBody = reader.ReadToEnd();

            RefdataBody = RefdataBody.Replace("[[Date]]", referralDate);
            RefdataBody = RefdataBody.Replace("[[DOB]]", refList[0].DOB.ToString());
            RefdataBody = RefdataBody.Replace("[[Parent/GuardianName]]", refList[0].FirstName.ToString());
            string businessdynamicRow = "";
            foreach (var item in refList3)
            {

                string mon = string.IsNullOrEmpty(item.MonFrom) ? "n/a" : (item.MonFrom.Substring(0, 5) + " To " + item.MonTo.Substring(0, 5));
                string tue = string.IsNullOrEmpty(item.TueFrom) ? "n/a" : (item.TueFrom.Substring(0, 5) + " To " + item.TueTo.Substring(0, 5));
                string wed = string.IsNullOrEmpty(item.WedFrom) ? "n/a" : (item.WedFrom.Substring(0, 5) + " To " + item.WedTo.Substring(0, 5));
                string thu = string.IsNullOrEmpty(item.ThursFrom) ? "n/a" : (item.ThursFrom.Substring(0, 5) + " To " + item.ThursTo.Substring(0, 5));
                string fri = string.IsNullOrEmpty(item.FriFrom) ? "n/a" : (item.FriFrom.Substring(0, 5) + " To " + item.FriTo.Substring(0, 5));
                string sat = string.IsNullOrEmpty(item.SatFrom) ? "n/a" : (item.SatFrom.Substring(0, 5) + " To " + item.SatTo.Substring(0, 5));
                string sun = string.IsNullOrEmpty(item.SunFrom) ? "n/a" : (item.SunFrom.Substring(0, 5) + " To " + item.SunTo.Substring(0, 5));


                businessdynamicRow += "<tr>" +
                                    "<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'>" + mon + "</td>" +
                                    "<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'>" + tue + "</td>" +
                                    "<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'>" + wed + "</td>" +
                                    "<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'>" + thu + "</td>" +
                                    "<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'>" + fri + "</td>" +
                                    "<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'>" + sat + "</td>" +
                                    "<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'>" + sun + "</td>" +
                                "</tr>";
            }
            RefdataBody = RefdataBody.Replace("[[BusinessDynamicRow]]", businessdynamicRow);


            if (refList1.Count > 0)
            {
                RefdataBody = RefdataBody.Replace("[[ReferralServices]]", !string.IsNullOrEmpty(refList1[0].Services) ? refList1[0].Services.ToString() : "");
            }
            else
            {
                RefdataBody = RefdataBody.Replace("[[ReferralServices]]", "n/a");
            }




            RefdataBody = RefdataBody.Replace("[[Referredto]]", !string.IsNullOrEmpty(refList2[0].CompanyName) ? refList2[0].CompanyName.ToString() : "");
            RefdataBody = RefdataBody.Replace("[[Address]]", !string.IsNullOrEmpty(refList2[0].Address) ? refList2[0].Address.ToString() : "");
            RefdataBody = RefdataBody.Replace("[[PhoneNo]]", !string.IsNullOrEmpty(refList2[0].Phoneno) ? refList2[0].Phoneno.ToString() : "");
            RefdataBody = RefdataBody.Replace("[[Email]]", !string.IsNullOrEmpty(refList2[0].Email) ? refList2[0].Email.ToString() : "");
            RefdataBody = RefdataBody.Replace("[[Referredby]]", !string.IsNullOrEmpty(ReferredName.ToString()) ? ReferredName.ToString() : "");
            RefdataBody = RefdataBody.Replace("[[Notes]]", Notes);

            var bytes = System.Text.Encoding.UTF8.GetBytes(RefdataBody);

            var input = new MemoryStream(bytes);
            var output = new MemoryStream(); // this MemoryStream is closed by FileStreamResult
            var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
            var writer = PdfWriter.GetInstance(document, output);
            writer.CloseStream = false;
            document.Open();
            MemoryStream stream = new MemoryStream();
            XMLWorkerHelper xmlWorker = XMLWorkerHelper.GetInstance();
            xmlWorker.ParseXHtml(writer, document, input, stream);

            writer.PageEvent = new Footer();
            Paragraph welcomeParagraph = new Paragraph("");
            document.Add(welcomeParagraph);
            //writer.PageEvent = new Footer();

            //Paragraph welcomeParagraph = new Paragraph("");

            //document.Add(welcomeParagraph);
            document.Close();
            output.Position = 0;
            var fileBytes = output.ToArray();
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            Response.AddHeader("content-disposition", "attachment;filename= OrderDetails.pdf");
            Response.ContentType = "application/octectstream";
            Response.BinaryWrite(fileBytes);
            Response.End();
        }


        public ActionResult GetBusinessHours(string ServiceId, string AgencyID, string CommunityID)
        {
            ;
            var refList3 = new List<BusinessHours>();
            refList3 = rosterData.BusinessHoursList(ServiceId, CommunityID);

            return Json(refList3, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult MatrixAnalysis(string id, string Householdid, string centerid, string Programid, string ClientName)
        {
            MatrixScore score = new MatrixScore();

            long householdID = Convert.ToInt64(EncryptDecrypt.Decrypt64(Householdid));
            long ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
            long programId = Convert.ToInt64(EncryptDecrypt.Decrypt64(Programid));
            ViewBag.ClientName = ClientName;

            score = rosterData.GetMatrixScoreList(householdID, ClientId, programId);
            score.HouseHoldId = Householdid;
            score.CenterId = centerid;
            score.ClientId = id;
            score.ProgramId = Programid;
            if (score.MatrixScoreList != null && score.MatrixScoreList.Count > 0)
            {
                score.ClassRoomId = score.MatrixScoreList[0].ClassRoomId;
                score.ProgramType = score.MatrixScoreList[0].ProgramType;
                score.ActiveYear = score.MatrixScoreList[0].ActiveYear;

            }

            return View(score);
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetClientStatus(string HouseHoldID)
        {
            MatrixScore matrixscore = new MatrixScore();
            long householdID = Convert.ToInt32(EncryptDecrypt.Decrypt64(HouseHoldID));
            List<ShowRecommendations> recommList = new List<ShowRecommendations>();
            matrixscore = rosterData.GetClientDetails(out recommList, householdID);
            return Json(new { matrixscore, recommList }, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetRecommendations(string HouseholdId, long assessmentNo, string activeProgramYear)
        {

            long householdID = Convert.ToInt32(EncryptDecrypt.Decrypt64(HouseholdId));
            ArrayList recommList = new ArrayList();
            recommList = rosterData.GetRecommendations(householdID, assessmentNo, activeProgramYear);
            return Json(recommList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetDescripton(int groupId, string clientId)
        {
            long dec_ClientID = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientId));
            List<AssessmentResults> results = new List<AssessmentResults>();


            results = rosterData.GetDescription(groupId, dec_ClientID);

            return Json(results, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetQuestions(long groupId, string clientId)
        {
            long dec_ClientID = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientId));
            List<QuestionsModel> questionlist = new List<QuestionsModel>();
            QuestionsModel question = new QuestionsModel();
            QuestionsModel questionmodel;
            question = rosterData.GetQuestions(groupId, dec_ClientID);
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

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult InsertMatrixScore(MatrixScore matrixscore)
        {
            long lastId = matrixscore.MatrixScoreId;
            bool isShow = false;
            ArrayList recommendationList = new ArrayList();
            try
            {


                matrixscore.Dec_HouseHoldId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.HouseHoldId));
                matrixscore.Dec_ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.ClientId));
                matrixscore.Dec_ProgramId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.ProgramId));
                matrixscore.Dec_CenterId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.CenterId));
                lastId = rosterData.InsertMatrixScore(matrixscore, out isShow, out recommendationList);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { lastId, isShow, recommendationList }, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        //public JsonResult GetChartDetails(string houseHoldId, string date, string clientId)
        //{


        //    List<MatrixScore> scoreList = new List<MatrixScore>();
        //    List<ChartDetails> chardetailsList = new List<ChartDetails>();
        //    System.Collections.ArrayList arraylist = new System.Collections.ArrayList();

        //    int groupType = 0;

        //    try
        //    {
        //        AnnualAssessment assessment = new AnnualAssessment();
        //        long houseHold = Convert.ToInt64(EncryptDecrypt.Decrypt64(houseHoldId));
        //        long ClientID = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));


        //        Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
        //        Guid? userID = (Session["UserID"] != null) ? new Guid(Session["UserID"].ToString()) : (Guid?)null;
        //        scoreList = rosterData.GetChartDetails(out assessment, out chardetailsList, AgencyId, userID, houseHold, date, ClientID);
        //        long type = assessment.AnnualAssessmentType;
        //        DateTime date1 = DateTime.Now;
        //        DateTime date2 = DateTime.Now;
        //        DateTime date3 = DateTime.Now;
        //        DateTime currentDate = DateTime.Parse(DateTime.Now.ToString(), new CultureInfo("en-US", true));


        //        if (type == 1)
        //        {

        //            date1 = DateTime.Parse(assessment.Assessment1To.ToString(), new CultureInfo("en-US", true));
        //        }

        //        if (type == 2)
        //        {
        //            date1 = DateTime.Parse(assessment.Assessment1To.ToString(), new CultureInfo("en-US", true));
        //            date2 = DateTime.Parse(assessment.Assessment2To.ToString(), new CultureInfo("en-US", true));
        //        }
        //        if (type == 3)
        //        {
        //            date1 = DateTime.Parse(assessment.Assessment1To.ToString(), new CultureInfo("en-US", true));
        //            date2 = DateTime.Parse(assessment.Assessment2To.ToString(), new CultureInfo("en-US", true));
        //            date3 = DateTime.Parse(assessment.Assessment3To.ToString(), new CultureInfo("en-US", true));
        //        }

        //        switch (type)
        //        {
        //            case 1:
        //                groupType = (date1 >= currentDate) ? 1 : 0;
        //                break;
        //            case 2:
        //                groupType = (date1 >= currentDate) ? 1 : (date2 >= currentDate) ? 2 : 0;
        //                break;
        //            case 3:
        //                groupType = (date1 >= currentDate) ? 1 : (date2 >= currentDate) ? 2 : (date3 >= currentDate) ? 3 : 0;
        //                break;
        //        }

        //        List<MatrixScore> matrixscorelist = null;
        //        List<long> categoryIdList = new List<long>();

        //        categoryIdList = scoreList.Select(x => x.AssessmentCategoryId).Distinct().ToList();
        //        if (categoryIdList != null && categoryIdList.Count > 0)
        //        {
        //            foreach (int categoryId in categoryIdList)
        //            {
        //                matrixscorelist = new List<MatrixScore>();
        //                matrixscorelist = scoreList.OrderBy(x => x.AnnualAssessmentType).Where(x => x.AssessmentCategoryId == categoryId).ToList();
        //                arraylist.Add(matrixscorelist);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    return Json(new { scoreList, groupType, chardetailsList, arraylist }, JsonRequestBehavior.AllowGet);
        //}



        [CustAuthFilter()]
        public JsonResult GetChartDetails(string houseHoldId, string date, string clientId)
        {


            List<MatrixScore> scoreList = new List<MatrixScore>();
            List<ChartDetails> chardetailsList = new List<ChartDetails>();
            System.Collections.ArrayList arraylist = new System.Collections.ArrayList();

            int groupType = 0;

            try
            {
                AnnualAssessment assessment = new AnnualAssessment();
                long houseHold = Convert.ToInt64(EncryptDecrypt.Decrypt64(houseHoldId));
                long ClientID = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));
                scoreList = rosterData.GetChartDetails(out assessment, out chardetailsList, houseHold, date, ClientID);
                long type = assessment.AnnualAssessmentType;


                if (type == 1 && Convert.ToInt32(assessment.EnrollmentDays) > 0)
                {
                    if (Convert.ToInt32(assessment.EnrollmentDays) <= Convert.ToInt32(assessment.Assessment1To)

                            && Convert.ToInt32(assessment.EnrollmentDays) >= Convert.ToInt32(assessment.Assessment1From))
                    {
                        groupType = 1;
                    }
                }

                if ((type == 2 || type == 3) && Convert.ToInt32(assessment.EnrollmentDays) > 0)
                {

                    if (Convert.ToInt32(assessment.EnrollmentDays) <= Convert.ToInt32(assessment.Assessment1To)

                           && Convert.ToInt32(assessment.EnrollmentDays) >= Convert.ToInt32(assessment.Assessment1From))
                    {
                        groupType = 1;
                    }

                    else if (Convert.ToInt32(assessment.EnrollmentDays) >= Convert.ToInt32(assessment.Assessment2From)

                         && Convert.ToInt32(assessment.EnrollmentDays) <= Convert.ToInt32(assessment.Assessment2To)

                        )
                    {
                        groupType = 2;
                    }

                    if (type == 3 && (Convert.ToInt32(assessment.EnrollmentDays) >= Convert.ToInt32(assessment.Assessment3From)

                         && Convert.ToInt32(assessment.EnrollmentDays) <= Convert.ToInt32(assessment.Assessment3To)

                        ))
                    {
                        groupType = 3;
                    }
                }



                List<MatrixScore> matrixscorelist = null;
                List<long> categoryIdList = new List<long>();

                categoryIdList = scoreList.Select(x => x.AssessmentCategoryId).Distinct().ToList();
                if (categoryIdList != null && categoryIdList.Count > 0)
                {
                    foreach (int categoryId in categoryIdList)
                    {
                        matrixscorelist = new List<MatrixScore>();
                        matrixscorelist = scoreList.OrderBy(x => x.AnnualAssessmentType).Where(x => x.AssessmentCategoryId == categoryId).ToList();
                        arraylist.Add(matrixscorelist);
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { scoreList, groupType, chardetailsList, arraylist }, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter()]
        public JsonResult SetChart()
        {
            List<MatrixScore> scoreList = new List<MatrixScore>();
            scoreList = rosterData.SetChart();
            List<long> categoryList = new List<long>();
            categoryList = scoreList.Select(x => x.AssessmentCategoryId).Distinct().ToList();
            System.Collections.ArrayList arraylist = new System.Collections.ArrayList();
            List<MatrixScore> score = null;
            foreach (int id in categoryList)
            {
                score = new List<MatrixScore>();
                score = scoreList.OrderBy(x => x.AssessmentNumber).Where(x => x.AssessmentCategoryId == id).ToList();
                arraylist.Add(score);
            }
            return Json(arraylist, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetName(MatrixScore matrixscore)
        {
            List<MatrixScore> scoreList = new List<MatrixScore>();
            try
            {
                matrixscore.Dec_HouseHoldId = (string.IsNullOrEmpty(matrixscore.HouseHoldId)) ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.HouseHoldId));
                //matrixscore.AgencyId = new Guid(Session["AgencyID"].ToString());
                //matrixscore.UserId = new Guid(Session["UserID"].ToString());
                scoreList = rosterData.GetName(matrixscore);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(scoreList, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult InsertMatrixRecommendation(string matrixRecString)
        {
            bool isResult = false;
            try
            {
                MatrixRecommendations matrixRec = JsonConvert.DeserializeObject<MatrixRecommendations>(matrixRecString);
                matrixRec.Dec_ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixRec.CientId));
                matrixRec.Dec_HouseHoldId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixRec.HouseHoldId));
                matrixRec.AgencyId = new Guid(Session["AgencyId"].ToString());
                matrixRec.UserId = new Guid(Session["UserID"].ToString());
                isResult = rosterData.InsertMatrixRecommendationData(matrixRec);

            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }


        [JsonMaxLengthAttribute]
        [HttpPost]
        public JsonResult GetChildrenImage(string enc_clientId, int mode = 1)
        {
            SelectListItem childImage = new SelectListItem();
            try
            {

                long clientId = 0;
                clientId = long.TryParse(enc_clientId, out clientId) ? clientId : Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_clientId));
                childImage = rosterData.GetChildrenImageData(clientId, mode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childImage, JsonRequestBehavior.AllowGet);
        }

        //[JsonMaxLengthAttribute]
        [HttpPost]
        [CustAuthFilter()]
        public PartialViewResult GetAttendenceByDate(AttendanceMealAuditReport report)
        {
            List<AttendenceDetailsByDate> attendence;
            try
            {

                int totalRecord;
                report.SkipRows = report.GetSkipRows();
                attendence = rosterData.GetAttendenceDetailsByDate(out totalRecord, report, staff);
                ViewBag.TotalRecord = totalRecord;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                attendence = new List<AttendenceDetailsByDate>();
                ViewBag.TotalRecord = 0;
            }



            return PartialView("~/Views/Partialviews/_AttendanceDetail.cshtml", attendence);
            // return Json(attendence, JsonRequestBehavior.AllowGet);
        }

        public partial class Footer : PdfPageEventHelper

        {

            public override void OnEndPage(PdfWriter writer, Document doc)
            {
                Paragraph footer = new Paragraph("THANK YOU", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL));
                string imageFilePath = System.Web.HttpContext.Current.Server.MapPath("/Images/fingerprint-footer.png");
                PdfPTable footerTbl = new PdfPTable(1);
                footerTbl.TotalWidth = doc.PageSize.Width;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imageFilePath);
                logo.ScaleToFit(500, 800);
                PdfPCell cell = new PdfPCell(logo);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                footerTbl.AddCell(cell);
                footerTbl.WriteSelectedRows(0, -1, -47, 47, writer.DirectContent);


            }

        }



        [ValidateInput(false)]
        public ActionResult SaveCaseNotes(FormCollection collection)
        {
            string id = collection.Get("childid");
            RosterNew.CaseNote CaseNote = new RosterNew.CaseNote();

            //CaseNote.CaseNotetags = CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Length - 1);
            List<CaseNote> CaseNoteList = new List<CaseNote>();
            FingerprintsModel.RosterNew.Users Userlist = new FingerprintsModel.RosterNew.Users();
            List<RosterNew.Attachment> Attachments = new List<RosterNew.Attachment>();
            string Name = "";

            CaseNote.ClientId = EncryptDecrypt.Decrypt64(Convert.ToString(collection.Get("CaseNoteClientId")));
            CaseNote.CenterId = Convert.ToString(collection.Get("CenterId"));
            CaseNote.CaseNoteid = "0";
            CaseNote.ProgramId = Convert.ToString(collection.Get("ProgramId"));
            CaseNote.HouseHoldId = EncryptDecrypt.Decrypt64(Convert.ToString(collection.Get("CaseNoteHouseHoldId")));
            CaseNote.Note = Convert.ToString(collection.Get("Note"));
            CaseNote.CaseNotetags = Convert.ToString(collection.Get("CaseNoteTags"));
            CaseNote.CaseNotetags = CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Trim().Length - 1);
            CaseNote.CaseNoteTitle = Convert.ToString(collection.Get("CaseNoteTitle"));
            CaseNote.CaseNoteDate = Convert.ToString(collection.Get("CaseNoteDate"));
            CaseNote.ClientIds = CaseNote.ClientId;
            CaseNote.IsLateArrival = true;
            CaseNote.CaseNoteAttachmentList = new List<RosterNew.Attachment>();
            string message = rosterData.SaveCaseNotes(ref Name, CaseNote);

            return Redirect("~/Roster/Roster");
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult Transition()
        {
            return View();
        }
        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult GetPregnantList(string sortOrder, string sortDirection, string Center, string Classroom, int pageSize, int requestedPage = 1)
        {
            try
            {

                int skip = pageSize * (requestedPage - 1);
                string totalrecord;
                var list = rosterData.GetPregnantMomList(out totalrecord, sortOrder, sortDirection, Center, Classroom, skip, pageSize, Convert.ToString(Session["UserID"]), Session["AgencyID"].ToString());


                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("");
            }
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult ChildEarlyHeadStartTransition(string Id, string ProgramId)
        {
            Transition transition = new Transition();

            ViewBag.ClientId = Id;
            ViewBag.ProgramId = ProgramId;
            transition = new FamilyData().GetEnrollReason("0", Id,staff);
            return View(transition);
        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult SaveChildEarlyHeadStartTranstion(Transition transition, List<PregMomChilds> pregChilds)
        {

            //List<SeatAvailability> results = new List<SeatAvailability>();
            //string AgencyId = Session["AgencyId"].ToString();
            //string UserId = Session["UserID"].ToString();
            //string RoleId = Session["Roleid"].ToString();




            List<SeatAvailability> results = new List<SeatAvailability>();
            TransitionDetails transitionDetails = new TransitionDetails();

            transition.ClientId = EncryptDecrypt.Decrypt64<long>(transition.EClientID);
            transition.ProgramTypeId = EncryptDecrypt.Decrypt64<long>(transition.Enc_ProgID);


          
            transition.ParentID = EncryptDecrypt.Decrypt64<long>(transition.EClientID).ToString();
            transitionDetails.Transition = transition;

            transitionDetails.PregMomChilds = pregChilds;

            transitionDetails.Transition.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(transitionDetails.Transition.EClientID));
            transitionDetails.Transition.HouseholdId = EncryptDecrypt.Decrypt64(transitionDetails.Transition.HouseholdId);
            transitionDetails.Transition.ProgramTypeId = Convert.ToInt64(EncryptDecrypt.Decrypt64(transitionDetails.Transition.Enc_ProgID));

            results = rosterData.SaveChildHeadStartTranstion(transitionDetails, false);

            return Json(results);
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult HeadStartTransition(string Id, string ProgramId, string st)
        {
            Transition trans = new FingerprintsModel.Transition();
            try
            {

                trans = new FamilyData().GetEnrollReason(st, Id,staff);
                trans.Enc_ProgID = ProgramId;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }


            return View(trans);



        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public JsonResult GetAvailablitySeatsByClass(string CenterId, string ClassRoomId, string ClientID)
        {
            try
            {
                int result = 0;
                //bool isIntCenter = int.TryParse(CenterId, out result);
                //  bool isIntclass = int.TryParse(ClassRoomId, out result);

                CenterId = int.TryParse(CenterId, out result) ? CenterId : (CenterId != "0") ? EncryptDecrypt.Decrypt64(CenterId) : CenterId;
                ClassRoomId = int.TryParse(ClassRoomId, out result) ? ClassRoomId : (ClassRoomId != "0") ? EncryptDecrypt.Decrypt64(ClassRoomId) : ClassRoomId;

                result = rosterData.GetAvailablitySeatsByClass(CenterId, ClassRoomId, Session["AgencyID"].ToString(), ClientID);
                return Json(result.ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }


        [HttpPost]
        public ActionResult GetCenterByAgency(string programYear = "")
        {
            var list = rosterData.GetCenterList(programYear);
            return Json(new { list }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        [JsonMaxLength]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult SaveHeadStartTransition(Transition transition, RosterNew.CaseNote caseNote)
        {

            int result = 0;



            transition.CenterId = EncryptDecrypt.Decrypt64<int>(transition.Enc_CenterID);
            transition.ClassRoomId = EncryptDecrypt.Decrypt64<int>(transition.Enc_ClassroomID);

            transition.ClientId = EncryptDecrypt.Decrypt64<long>(transition.EClientID);
            transition.ProgramTypeId = EncryptDecrypt.Decrypt64<long>(transition.Enc_ProgID);
            transition.TransProgramTypeID = EncryptDecrypt.Decrypt64<long>(transition.TransProgramTypeID).ToString();


            transition.ParentID = EncryptDecrypt.Decrypt64<long>(transition.ParentID).ToString();
            transition.ParentID2 = EncryptDecrypt.Decrypt64<long>(transition.ParentID2).ToString(); ;

            transition.HouseholdId = EncryptDecrypt.Decrypt64<long>(transition.HouseholdId).ToString();


            result = rosterData.SaveHeadStartTranstion(transition, staff);
            if (result == 1)
            {
                string name = "";


                name = rosterData.SaveCaseNotes(ref name, caseNote, 0);
                result = 3;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult GetCenterAndClassRoomsByCenter(string centerid, string Classroom, string householdid, string ChildId)
        {
            var list = rosterData.GetCenterAndClassRoomsByCenter(centerid, Classroom, householdid, ChildId, Convert.ToString(Session["UserID"]), Session["AgencyID"].ToString());
            return Json(new { list });
        }




        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-5b6c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,a31b1716-b042-46b7-acc0-95794e378b26")]

        public ActionResult SaveCenterOrClassroomChange(RosterNew.CaseNote caseNote, string dateOfTransition, int? reasonId, string newReason)
        {
            caseNote.Note = System.Uri.UnescapeDataString(caseNote.Note);
            bool result = rosterData.SaveCenterOrClassroomChange(caseNote.ClientId, dateOfTransition, caseNote.CenterId, caseNote.Classroomid, staff, reasonId, newReason);
            StringBuilder _Ids = new StringBuilder();
            string Name = "";

            caseNote.CaseNotetags = caseNote.CaseNotetags.Substring(0, caseNote.CaseNotetags.Length - 1);
            caseNote.ProgramId = null;
            caseNote.CaseNoteid = "0";
            caseNote.HouseHoldId = "0";
            caseNote.CaseNoteSecurity = true;
            caseNote.CaseNoteAttachmentList = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<List<RosterNew.Attachment>>();
            string message = rosterData.SaveCaseNotes(ref Name, caseNote);
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        public ActionResult SaveProgramInformationReport(MatrixScore matrixScore)
        {
            matrixScore.Dec_HouseHoldId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixScore.HouseHoldId));
            matrixScore.Dec_ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixScore.ClientId));
            matrixScore.Dec_ProgramId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixScore.ProgramId));
            rosterData.InsertParentDetailsMatrixScore(matrixScore);
            return null;
        }

        /// <summary>
        /// JsonResult Method to get the Tags Inputs while entering the Tag Name.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        /// ***Method depreciated. Please use GetCaseNoteTag **
        [CustAuthFilter()]
        public JsonResult GetCaseNoteTagonInput(string searchText, string term = "")
        {
            List<SelectListItem> tagsList = new List<SelectListItem>();
            try
            {

                tagsList = rosterData.GetCaseNoteTagsonInput(searchText);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(tagsList, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public JsonResult GetCaseNoteTag(string term = "")
        {
            List<SelectListItem> tagsList = new List<SelectListItem>();
            try
            {

                tagsList = rosterData.GetCaseNoteTagsonInput(term);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            var list = new List<ExtendSelectList>();
            foreach (var item in tagsList)
            {

                list.Add(new ExtendSelectList() { id = item.Value, label = item.Text, value = item.Text });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter()]
        public JsonResult ClearRosterSession(int requestedPage, int pageSize, string centerId, string classroomId, string filter, string searchText, string sortOrder, string sortDirection)
        {

            Session["_RosterCenter"] = centerId;
            Session["_RosterClassroom"] = classroomId;
            Session["_RosterPageSize"] = pageSize;
            Session["_RosterRequestedPage"] = requestedPage;
            Session["_RosterSearchText"] = searchText;
            Session["_RosterFilter"] = filter;
            Session["_RosterSortOrder"] = sortOrder;
            Session["_RosterSortDirection"] = sortDirection;

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHouselessClient(string householdId, string clientId)
        {
            FamilyHouseless houseless = new FamilyHouseless();

            houseless = rosterData.GetHouselessClient(householdId, clientId);

            return Json(houseless, JsonRequestBehavior.AllowGet);
        }







        //[HttpPost]
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,047c02fe-b8f1-4a9b-b01f-539d6a238d80,c352f959-cfd5-4902-a529-71de1f4824cc")]

        //public JsonResult NextProgramYearTransition(string clientId)
        //{

        //    bool isResult = false;

        //    isResult = rosterData.NextProgramYearTransition(clientId);

        //    return Json(isResult);
        //}


        [HttpPost]
        [CustAuthFilter(RoleEnum.Teacher, RoleEnum.DisabilitiesManager, RoleEnum.SocialServiceManager, RoleEnum.HealthNurse, RoleEnum.HomeVisitor, RoleEnum.FamilyServiceWorker)]

        public JsonResult UpdateReturningTransitionClient(int returnValue, string clientId)
        {
            bool isResult = false;

            isResult = rosterData.UpdateReturningTransitionClient(returnValue, clientId);

            return Json(isResult, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult GetCaseNoteByClient(string clientId = "", string householdId = "")
        {

            string JSONString = string.Empty;
            try
            {
                DataTable dtCaseNote = new DataTable();
                householdId = (householdId != "" && householdId != "0") ? EncryptDecrypt.Decrypt64(householdId) : "0";
                rosterData.GetCaseNotesByClient(ref dtCaseNote, clientId, householdId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtCaseNote);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult GetHouseholdCasenoteTags(string HouseholdId)
        {
            List<HouseholdTags> result = null;
            try
            {
                RosterData rd = rosterData;
                result = rd.GetHouseholdCasenoteTags(HouseholdId);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [CustAuthFilter()]
        public JsonResult SaveFamilyAdvocate(string HouseholdID, string FamilyAdvocate)
        {

            AgencyStaff staff = null;
            try
            {
                staff = new FamilyData().SaveFamilyAdvocate(HouseholdID, FamilyAdvocate);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return Json(staff, JsonRequestBehavior.AllowGet);
        }


        #region ReferalReviewList

        public JsonResult GetOrganizationList(int ServiceId, string AgencyId)
        {
            var result = rosterData.GetOrganizationListWithCount(ServiceId, AgencyId, 1);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetOrganizationListByAgency(string AgencyId)
        {
            var result = rosterData.GetOrganizationListWithCount(0, AgencyId, 3);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetReviewList(int id)
        {
            var result = rosterData.GetReviewList(id, 2);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion ReferalReviewList

        [CustAuthFilter()]
        public ActionResult DeleteCaseNote(int casenoteid, int[] appendcid, bool deletemain)
        {
            var result = rosterData.DeleteCaseNote(casenoteid, appendcid, deletemain, 1);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region CaseNote_Tag_Report
        [CustAuthFilter()]
        public ActionResult CaseNoteTagReport()
        {

            if (new agencyData().GetSingleAccessStatus(18))
            {
                return View();
            }
            else
            {
                return new RedirectResult("~/login/Loginagency");
            }
        }
        [CustAuthFilter()]
        public ActionResult GetCaseNoteTagReport(long pno = 1, long psize = 10, string searchtxt = "", string sortclmn = "", string sortdir = "")
        {
            if (new agencyData().GetSingleAccessStatus(18))
            {
                var result = rosterData.GetCaseNoteTagReport(pno, psize, searchtxt, sortclmn, sortdir, 1);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new RedirectResult("~/login/Loginagency");
            }
        }
        [CustAuthFilter()]
        public ActionResult GetCaseNotesByTagId(long tagid)
        {

            var result = rosterData.GetCaseNotesByTagId(tagid, 2);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public ActionResult DownloadCaseNoteByTagId(long tagid, long total, string tname)
        {
            try
            {
                Export export = new Export();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Case Note Report for " + tname + "-" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                var _cnlist = rosterData.GetCaseNotesByTagId(tagid, 2);
                MemoryStream ms = export.ExportCaseNoteByTagId(_cnlist, tagid, total, tname);

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

        [CustAuthFilter()]
        public ActionResult EditCaseNote(long tagid, string tagname, int mode = 1, long ExistsTagId = 0)
        {
            long availableTag = 0;
            var Result = rosterData.EditDeleteCNTag(tagid, tagname, mode, ExistsTagId, ref availableTag);

            return Json(new { Success = Result, AvailableTagId = availableTag }, JsonRequestBehavior.AllowGet);
        }


        #endregion CaseNote_Tag_Report


        #region TimeLine

        [CustAuthFilter()]
        public ActionResult TimeLine(string clientid = "0")
        {
            //if (dev) {
            //    clientid = EncryptDecrypt.Encrypt64(clientid);
            //}

            long cid = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientid));

            ViewBag.ClientId = clientid; //id must encrypted
            var result = rosterData.GetClientDetails(cid);
            var ActiveSteps = rosterData.GetTimelineMaster();
            // ViewBag.Client = result;
            ViewBag.ProfilePic = result.Profilepic;
            result.Profilepic = string.Empty;
            ViewBag.Client = result;
            ViewBag.ActiveSteps = ActiveSteps;
            return View();
        }

        [CustAuthFilter()]
        public ActionResult GetTimeLine(string clientid, string stepIds)
        {

            long cid = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientid));

            var result = rosterData.GetClientTimeLine(cid, stepIds);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #endregion TimeLine

        /// <summary>
        /// Gets the Developmental Team, Household members and default case notes for Attendance issue popup
        /// </summary>

        [CustAuthFilter()]
        [HttpPost]
        public JsonResult GetDevelopmentalTeamByClient(string clientID, int yakrkcode)
        {

            CaseNoteByClientID caseNoteClient = new CaseNoteByClientID();
            try
            {
                long _clientId = 0;
                _clientId = long.TryParse(clientID, out _clientId) ? _clientId : Convert.ToInt64(EncryptDecrypt.Decrypt64(clientID));
                StaffDetails staff = StaffDetails.GetInstance();

                caseNoteClient = rosterData.GetDevelopmentalMembersByClientID(_clientId, staff, yakrkcode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(caseNoteClient, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        [HttpPost]
        public JsonResult GetParentInformationByClient(string clientId)
        {

            List<ParentInfo> parentInfo = new List<ParentInfo>();

            ChildrenInfo childInfo;

            parentInfo = rosterData.GetParentInformationByClient(out childInfo, clientId, staff);
            return Json(new { parentInfo, childInfo }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Gets the Developmental Team, Household members and default case notes for Attendance issue popup
        /// </summary>

        [CustAuthFilter()]
        [HttpPost]
        public ActionResult GetDevelopmentalTeamPartial(string clientID, int yakkrCode = 0)
        {

            CaseNoteByClientID caseNoteClient = new CaseNoteByClientID();
            try
            {
                long _clientId = 0;
                _clientId = long.TryParse(clientID, out _clientId) ? _clientId : Convert.ToInt64(EncryptDecrypt.Decrypt64(clientID));
                StaffDetails staff = StaffDetails.GetInstance();

                caseNoteClient = rosterData.GetDevelopmentalMembersByClientID(_clientId, staff, yakkrCode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            // return Json(caseNoteClient, JsonRequestBehavior.AllowGet);
            return PartialView("~/Views/partialviews/_CaseNote.cshtml", caseNoteClient);
        }




        #region Gets Case Note List of Attendance Issue

        [HttpPost]
        public ActionResult GetAttendanceIssueCaseNoteList(string clientId, int pageSize, int requestedPage)
        {

            List<CaseNote> caseNoteList = new List<CaseNote>();

            CaseNoteByClientID caseNoteClientId = new CaseNoteByClientID();

            try
            {
                caseNoteClientId.PageSize = pageSize;
                caseNoteClientId.RequestedPage = requestedPage;
                caseNoteClientId.SkipRows = caseNoteClientId.GetSkipRows();

                rosterData.GetAttendanceIssueCaseNoteList(ref caseNoteClientId, StaffDetails.GetInstance(), clientId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return PartialView("~/Views/Partialviews/_CaseNoteListPartial.cshtml", caseNoteClientId);

        }

        #endregion

        [HttpPost]
        [ValidateInput(false)]
        [CustAuthFilter()]
        public ActionResult SaveAttendanceIssueCaseNote(RosterNew.CaseNote caseNote, string[] fileInput)
        {


            var files = Request.Files;
            var keys = files.AllKeys;
            caseNote.CaseNoteAttachmentList = new List<RosterNew.Attachment>();

            for (int i = 0; i < keys.Length; i++)
            {
                RosterNew.Attachment aatt = new RosterNew.Attachment();
                aatt.file = files[i];
                caseNote.CaseNoteAttachmentList.Add(aatt);
            }

            caseNote.CaseNoteid = caseNote.CaseNoteid != null && caseNote.CaseNoteid != "" ? "0" : caseNote.CaseNoteid;
            caseNote.ClientId = caseNote.ClientId != null && caseNote.ClientId != "" && caseNote.ClientId != "0" ? EncryptDecrypt.Decrypt64(caseNote.ClientId) : "0";
            caseNote.ProgramId = caseNote.ProgramId != null && caseNote.ProgramId != "" && caseNote.ProgramId != "0" ? EncryptDecrypt.Decrypt64(caseNote.ProgramId) : "0";
            caseNote.CenterId = caseNote.CenterId != null && caseNote.CenterId != "" && caseNote.CenterId != "0" ? EncryptDecrypt.Decrypt64(caseNote.CenterId) : "0";
            caseNote.Classroomid = caseNote.Classroomid != null && caseNote.Classroomid != "" && caseNote.Classroomid != "0" ? EncryptDecrypt.Decrypt64(caseNote.Classroomid) : "0";
            caseNote.HouseHoldId = caseNote.HouseHoldId != null && caseNote.HouseHoldId != "" && caseNote.HouseHoldId != "0" ? EncryptDecrypt.Decrypt64(caseNote.HouseHoldId) : "0";

            caseNote.CaseNotetags = caseNote.CaseNotetags.Trim(',');

            string Name = "";


            string message = rosterData.SaveCaseNotes(ref Name, caseNote, (int)FingerprintsModel.Enums.TransitionMode.Others);

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult ResetExcessiveAbsence(string _encClientId, string _encYakkrId)
        {

            try
            {
                int _clientID = int.TryParse(_encClientId, out _clientID) ? _clientID : Convert.ToInt32(EncryptDecrypt.Decrypt64(_encClientId));
                int _yakkrId = int.TryParse(_encYakkrId, out _yakkrId) ? _yakkrId : Convert.ToInt32(EncryptDecrypt.Decrypt64(_encYakkrId));


                return Json(rosterData.ResetExcessiveAbsence(staff, _clientID, _yakkrId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetAppendCaseNotesPopup()
        {
            return PartialView("~/Views/Partialviews/_AppendCaseNotePopup.cshtml");
        }


        [HttpPost]
        public ActionResult GetAppendCaseNoteSection()
        {
            return PartialView("~/Views/Partialviews/_AppendCaseNote.cshtml");
        }


        [HttpPost]
        [CustAuthFilter()]
        public ActionResult GetAdditionalStaffCenterClassroomChange(string clientId, string centerId, string classroomId)
        {
            RosterNew.Users users = new RosterNew.Users();

            users = rosterData.GetAdditionalStaffCenterClassroomChange(clientId, centerId, classroomId, staff);

            return Json(users, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetTransitionTypeDetailBy(string clientId,string yakkr)
        {


            TransitionTypeModel model = new TransitionTypeModel();
            Transition transition = new FingerprintsModel.Transition();

            Parallel.Invoke(() =>
            {
                model = rosterData.GetTransitionTypeDetailsByClientId(clientId);
            },
            () =>
            {
                transition=  new FamilyData().GetEnrollReason(Status: yakkr, clientId: clientId,staff:staff);
            });
           


            return Json(new { model, transition }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult SaveTransitionTypeDetail(TransitionTypeDetail transitionTypeDetail,Transition transition)
        {

            bool isResult = rosterData.SaveTransitionTypeDetail(transitionTypeDetail,transition);


            return Json(isResult, JsonRequestBehavior.AllowGet);
        }






    }
}
