using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using System.IO;
using Fingerprints.ViewModel;
using Fingerprints.CustomClasses;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Data;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System.Collections;

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
         roleid=825f6940-9973-42d2-b821-56c7c937bfe(Facilities Manager)
         roleid=c352f959-cfd5-4902-a529-71de1f4824cc (Social Service Manager)
         roleid=2af7205e-87b4-4ca7-8ca8-95827c08564c (Area Manager)
         roleid=2d9822cd-85a3-4269-9609-9aabb914D792 (HR Manager)
         roleid=6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba (Transportation Manager)
         roleid=7c2422ba-7bd4-4278-99af-b694dcab7367 (executive)
         roleid=b65759ba-4813-4906-9a69-e180156e42fc (ERSEA Manager)
         roleid=4b77aab6-eed1-4ac3-b498-f3e80cf129c0 (Education Manager)
          roleid=944d3851-75cc-41e9-b600-3fa904cf951f (Billing Manager)
          roleid=3b49b025-68eb-4059-8931-68a0577e5fa2 (Agency Admin)
         */
        RosterData RosterData = new RosterData();


        string[] managerRoleArray = { "A65BB7C2-E320-42A2-AED4-409A321C08A5","3B49B025-68EB-4059-8931-68A0577E5FA2",
                "944D3851-75CC-41E9-B600-3FA904CF951F", "047C02FE-B8F1-4A9B-B01F-539D6A238D80",
            "9AD1750E-2522-4717-A71B-5916A38730ED", "825F6940-9973-42D2-B821-5B6C7C937BFE",

        "B4D86D72-0B86-41B2-ADC4-5CCCE7E9775B", "C352F959-CFD5-4902-A529-71DE1F4824CC","2AF7205E-87B4-4CA7-8CA8-95827C08564C",
        "2D9822CD-85A3-4269-9609-9AABB914D792","6ED25F82-57CB-4C04-AC8F-A97C44BDB5BA","7C2422BA-7BD4-4278-99AF-B694DCAB7367",
            "B65759BA-4813-4906-9A69-E180156E42FC","4B77AAB6-EED1-4AC3-B498-F3E80CF129C0" };

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-56c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]

        public ActionResult Roster()
        {
            ViewBag.IsManager = false;

            ViewBag.IsManager = (Array.IndexOf(managerRoleArray, Session["RoleId"].ToString().ToUpper()) > -1);


            return View();
        }

        [HttpPost]
        // [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
        public ActionResult CheckIn(FormCollection collection)
        {
            try
            { int reasonid = 0;
                string NewReason = "";
                string childCode = collection.Get("childid");
                string childID =EncryptDecrypt.Decrypt64(childCode);

                string absentType = collection.Get("AbsentType");
                string Cnotes = collection.Get("CNotes");
                if (!string.IsNullOrEmpty(collection.Get("ReasonList") ))
                     NewReason = collection.Get("txtNewReason");

                if (collection.Get("ReasonList").ToString() != "")
                    reasonid = Convert.ToInt32(collection.Get("ReasonList"));
                string result = "";
                RosterData.MarkAbsent(ref result, childID, Session["UserID"].ToString(), absentType, Cnotes, Session["AgencyID"].ToString(), reasonid, NewReason);
                if (result == "1")
                    TempData["message"] = "Record saved successfully.";

                return Redirect("~/Roster/Roster");
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }




        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-56c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult listRoster(string sortOrder, string sortDirection, string Center, string Classroom, int pageSize, int requestedPage = 1)
        {
            try
            {

                int skip = pageSize * (requestedPage - 1);
                string totalrecord;
                var list = RosterData.GetrosterList(out totalrecord, sortOrder, sortDirection, Center, Classroom, skip, pageSize, Convert.ToString(Session["UserID"]), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,2d9822cd-85a3-4269-9609-9aabb914D792,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,2af7205e-87b4-4ca7-8ca8-95827c08564c,825f6940-9973-42d2-b821-56c7c937bfe,9ad1750e-2522-4717-a71b-5916a38730ed,047c02fe-b8f1-4a9b-b01f-539d6a238d80,944d3851-75cc-41e9-b600-3fa904cf951f,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc,7c2422ba-7bd4-4278-99af-b694dcab7367,6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba,b65759ba-4813-4906-9a69-e180156e42fc,4b77aab6-eed1-4ac3-b498-f3e80cf129c0,a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Getclassrooms(string Centerid = "0")
        {
            try
            {
                return Json(RosterData.Getclassrooms(Centerid, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        public JsonResult AutoCompleteSerType(string Services)
        {
            string agencyId = Session["AgencyId"].ToString();
            var result = RosterData.AutoCompleteSerType(Services, agencyId);
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


        [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a")]
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
                        new RosterData().GetNotesDetialByNoteId(ref objNotes, NoteId);
                    Int64 ClientId = !string.IsNullOrEmpty(EncryptDecrypt.Decrypt64(ChildId)) ? Convert.ToInt64(EncryptDecrypt.Decrypt64(ChildId)) : 0;
                    string ClientName = !string.IsNullOrEmpty(EncryptDecrypt.Decrypt64(ChildName)) ? EncryptDecrypt.Decrypt64(ChildName) : "";
                    objNotes.dictClientDetails.Add(ClientId, ClientName);
                }
                else
                {
                    ViewBag.HeaderName = "MULTI - CHILD ENTRY";
                    ViewBag.Child = "Child List";
                    new RosterData().GetChildlistByUserId(ref objNotes, Session["UserID"].ToString(), Session["AgencyID"].ToString());
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
                new RosterData().GetElementDetailsByDomainId(ref dtElements, DomainId);
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
                    Result = new RosterData().SaveObservatioNotes(objNotes);
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

        public ActionResult GetCaseNoteByTags(string Householdid, string centerid, string ClientId, string TagName)

        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtMaterial = new DataTable();
                new RosterData().GetCaseNoteByTags(ref dtMaterial, Convert.ToInt32(Householdid), Convert.ToInt32(centerid), ClientId, Session["AgencyID"].ToString(), Session["UserID"].ToString(), TagName);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterial);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        public ActionResult GetCaseNoteByNoteId(string NoteId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtNotes = new DataTable();
                new RosterData().GetCaseNoteByNoteId(ref dtNotes, NoteId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtNotes);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult SaveSubNotes(string CaseNoteId, string CaseNoteDate, string Householdid, string centerid, string ClassroomId, RosterNew.CaseNote CaseNote)
        //{
        //    bool isInserted =false;
        //    try
        //    {
        //        isInserted= new RosterData().SaveSubNotes(CaseNoteId, CaseNoteDate, Convert.ToInt32(Householdid), Convert.ToInt32(centerid), ClassroomId, Session["AgencyID"].ToString(), Session["UserID"].ToString(), CaseNote.Note, Session["Roleid"].ToString());                
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //    }
        //    return Json(isInserted);
        //}
        public ActionResult GetSubNotes(string CaseNoteId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtSubNotes = new DataTable();
                new RosterData().GetSubNotes(ref dtSubNotes, CaseNoteId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtSubNotes);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        public JsonResult UploadFiles(string monitor)
        {
            bool isInserted = false;
            string CaseNoteId = TempData["CaseNoteid"].ToString();
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
                            isInserted = new RosterData().SaveAttachmentsOnSubNote(Session["AgencyID"].ToString().ToString(), CaseNoteId, new BinaryReader(pic.InputStream).ReadBytes(pic.ContentLength), fileName, _ext, Session["UserID"].ToString());

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
        public ActionResult SaveSubNotes(RosterNew.CaseNote CaseNote)
        {
            bool isInserted = false;
            try
            {
                TempData["CaseNoteid"] = CaseNote.CaseNoteid;
                isInserted = new RosterData().SaveSubNotes(CaseNote.CaseNoteid, CaseNote.CaseNoteDate, Convert.ToInt32(CaseNote.HouseHoldId), Convert.ToInt32(CaseNote.CenterId), CaseNote.Classroomid, Session["AgencyID"].ToString(), Session["UserID"].ToString(), CaseNote.Note, Session["Roleid"].ToString());
                TempData.Keep();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isInserted);
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,047c02fe-b8f1-4a9b-b01f-539d6a238d80,c352f959-cfd5-4902-a529-71de1f4824cc")]

        [ValidateInput(false)]
        public ActionResult CaseNotesclient(string id = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    id = FingerprintsModel.EncryptDecrypt.Decrypt64(id);
                }
                else
                {
                    id = "0";
                }
                string Name = "";
                int centerid = 0;
                int Householdid = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["centerid"]))
                {
                    ViewBag.centerid = centerid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["centerid"].ToString()));
                }
                else
                    ViewBag.centerid = 0;
                FingerprintsModel.RosterNew.Users Userlist = new FingerprintsModel.RosterNew.Users();

                if (!string.IsNullOrEmpty(Request.QueryString["Householdid"]))
                {
                    if (Request.QueryString["Householdid"].ToString() == "0")
                        Householdid = 0;
                    else
                        Householdid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["Householdid"].ToString()));
                    ViewBag.Householdid = Request.QueryString["Householdid"].ToString();
                }
                else
                    ViewBag.Householdid = 0;

                ViewBag.CaseNotelist = RosterData.GetCaseNote(ref Name, ref Userlist, Householdid, centerid, id, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                ViewBag.Userlist = Userlist;
                ViewBag.Name = Name;
                ViewBag.Client = id;
                if (!string.IsNullOrEmpty(Request.QueryString["Programid"]))
                    ViewBag.Programid = Convert.ToInt32(EncryptDecrypt.Decrypt64(Request.QueryString["Programid"].ToString()));
                else
                    ViewBag.Programid = 0;
                ViewBag.User = Session["FullName"].ToString();
                ViewBag.Date = DateTime.Now.ToString("MM/dd/yyy");
                // if (Session["RoleList"] != null)                    
                List<FingerprintsModel.Role> listRole = Session["RoleList"] as List<FingerprintsModel.Role>;
                ViewBag.Role = listRole.Select(a => a.RoleName).FirstOrDefault();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View();
        }



        public ActionResult ReferralCategorycompany(string id, string clientName = "")
        {
            ViewBag.Id = id;
            ViewBag.clientName = clientName;
            int ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(id));
            REF obj_REF = new REF();
            var refList = new List<REF>();
            refList = RosterData.ReferralCategoryCompany(ClientId);
            obj_REF.refListData = refList;
            return View(obj_REF);
        }
        public ActionResult ReferralCategorycompanyPopUp(string id, string clientName = "")
        {

            int ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(id));
            REF obj_REF = new REF();
            var refList = new List<REF>();
            refList = RosterData.ReferralCategoryCompany(ClientId);
            obj_REF.refListData = refList;
            return Json(refList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetReferralServices()
        {

            List<SelectListItem> selectlist = new List<SelectListItem>();
            string agencyId = Session["AgencyId"].ToString();
            selectlist = new RosterData().GetServiceReference(agencyId);
            return Json(selectlist, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ReferralCategory(ReferralList ReferralCategory)
        {
            ViewBag.ClientName = ReferralCategory.clientName;
            ViewBag.Id = ReferralCategory.id;
            ViewBag.ReferalClientId = ReferralCategory.ReferralClientId;
            ViewBag.parentName = ReferralCategory.parentName;
            int ClientId = 0;
            if (ReferralCategory.ReferralClientId == null)
            {
                ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(ReferralCategory.id));
            }
            else
            {
                ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(ReferralCategory.id));
            }
            ViewBag.UniqueClientId = ReferralCategory.id;

            REF obj_REF = new REF();
            var refList = new List<REF>();
            refList = RosterData.ReferralCategory(ClientId, ReferralCategory.ReferralClientId, Convert.ToInt32(ReferralCategory.Step));
            obj_REF.refListData = refList;
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
            // ViewBag.UniqueClientId = ReferralCategory.id;

            REF obj_REF = new REF();
            var refList = new List<REF>();
            refList = RosterData.ReferralCategory(clientId, ReferralCategory.ReferralClientId, Convert.ToInt32(ReferralCategory.Step));
            obj_REF.refListData = refList;
            // TempData["tempClientId"] = ReferralCategory.id;
            // TempData.Keep("tempClientId");
            long refferralClientId = (string.IsNullOrEmpty(ReferralCategory.ReferralClientId.ToString())) ? 0 : (long)ReferralCategory.ReferralClientId;
            return Json(new { refList, refferralClientId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HouseHoldReferrals(long referralClientId)
        {
            List<SelectListItem> referrals = new List<SelectListItem>();
            referrals = RosterData.GetSelectedReferrals(referralClientId);
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
                foreach (var item in serviceID_Array)
                {
                    long referralId = RosterData.SaveReferralClient(Convert.ToInt32(item), CommanClientId_, new Guid(SaveReferral.AgencyId), Step, Status, CreatedBy, SaveReferral.referralClientId);
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
                            Success = RosterData.SaveHouseHold(Convert.ToInt32(item), CommanClientId_, Step, Status, Convert.ToInt32(SaveReferral.HouseHoldId), Convert.ToInt64(id), querycommand);
                            count++;
                        }
                    }
                }
                else
                {
                    Success = RosterData.SaveHouseHold(0, CommanClientId_, Step, Status, Convert.ToInt32(SaveReferral.HouseHoldId), SaveReferral.referralClientId, querycommand, SaveReferral.ClientId);
                }

                return Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveMatchProviders(ListRoster SaveProvider)
        {
            bool Success = false;
            try
            {
                TempData.Keep("tempClientId");
                string cId = (string.IsNullOrEmpty(SaveProvider.ClientId)) ? TempData["tempClientId"].ToString() : SaveProvider.ClientId;
                int ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(cId));
                SaveProvider.Description = (SaveProvider.Description == null) ? SaveProvider.Description = "" : SaveProvider.Description;

                string UserId = Session["UserID"].ToString();
                Success = RosterData.SaveMatchProviders(SaveProvider.ReferralDate, SaveProvider.Description, SaveProvider.ServiceResourceId, SaveProvider.AgencyId, UserId, ClientId, SaveProvider.CommunityId, SaveProvider.ReferralClientServiceId);
                RosterData.YakkarInsert(SaveProvider.AgencyId, UserId, ClientId);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Success;
        }

        public bool SaveReferral(ListRoster Savereferral)
        {
            bool Success = false;
            try
            {
                TempData.Keep("tempClientId");
                string cId = (string.IsNullOrEmpty(Savereferral.CommonClientId)) ? TempData["tempClientId"].ToString() : Savereferral.CommonClientId;
                // string cId = TempData["tempClientId"].ToString();
                int commanclient = Convert.ToInt32(EncryptDecrypt.Decrypt64(cId));
                int Step = 3;

                long referenceID = 0;
                bool Status = true;
                string UserId = Session["UserID"].ToString();
                referenceID = RosterData.SaveReferral(Savereferral.ReferralDate, Savereferral.Description == null ? "" : Savereferral.Description, Convert.ToInt32(Savereferral.ServiceResourceId), Savereferral.AgencyId, UserId, commanclient, Convert.ToInt32(Savereferral.CommunityId), Convert.ToInt32(Savereferral.ReferralClientServiceId));

                string[] ClientId_Array = Savereferral.ClientId.Split(',');
                int count = 0;
                foreach (var item in ClientId_Array)
                {
                    Success = RosterData.SaveHouseHold(Convert.ToInt32(item), commanclient, Step, Status, Convert.ToInt32(Savereferral.HouseHoldId), referenceID, "INSERT");
                    count++;
                }

                RosterData.YakkarInsert(Savereferral.AgencyId, UserId, commanclient);

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
            Success = RosterData.DeleteReferralService(ReferralClientServiceId);
            return true;
        }


        public ActionResult FamilyNeeds()
        {
            return View();
        }

        public ActionResult LoadSurveyOptions(long ReferralClientId)
        {
            string userId = Session["UserID"].ToString();
            var surveyList = RosterData.LoadSurveyOptions(ReferralClientId, userId);
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
            string userId = Session["UserID"].ToString();
            RosterData.InsertSurveyOptions(surveyOptions, ReferralClientId, userId, isUpdate);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MatchProviders(ListRoster MatchProvider, string CommunityIds, string stepId = "")
        {
            ViewBag.ReferralClientId = 0;
            if (MatchProvider.AgencyId != "" || MatchProvider.AgencyId != null)
            {
                ViewBag.ReferralClientId = MatchProvider.referralClientId;
                // MatchProvider.referralClientId = 0;
            }
            ViewBag.ClientName = MatchProvider.clientName;
            MatchProvider.AgencyId = Session["AgencyId"].ToString();
            ViewBag.Id = MatchProvider.id;
            ViewBag.ParentName = MatchProvider.parentName;
            ViewBag.StepId = stepId;

            TempData.Keep("tempClientId");
            MatchProviderModel obj_MPM = new MatchProviderModel();
            var matchProvidersList = new List<MatchProviderModel>();
            List<SelectListItem> OrganizationList = new List<SelectListItem>();
            matchProvidersList = RosterData.MatchProviders(MatchProvider.AgencyId, CommunityIds, MatchProvider.referralClientId);
            obj_MPM.ParentName = MatchProvider.parentName;
            obj_MPM.AgencyId = MatchProvider.AgencyId;
            obj_MPM.MPMList = matchProvidersList;
            if (matchProvidersList != null && matchProvidersList.Count() > 0)
            {
                //OrganizationList = RosterData.FamilyServiceList(Convert.ToInt32(matchProvidersList.FirstOrDefault().ServiceId), matchProvidersList.FirstOrDefault().AgencyId);
                OrganizationList = RosterData.FamilyServiceList(Convert.ToInt32(matchProvidersList.FirstOrDefault().ServiceId), MatchProvider.AgencyId);

            }


            obj_MPM.OrganizationList = OrganizationList;
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
            //if (MatchProvider.AgencyId != "" || MatchProvider.AgencyId != null)
            //{
            //    ViewBag.ReferralClientId = MatchProvider.referralClientId;
            //    // MatchProvider.referralClientId = 0;
            //}
            string AgencyId = Session["AgencyID"].ToString();
            MatchProviderModel obj_MPM = new MatchProviderModel();
            var matchProvidersList = new List<MatchProviderModel>();
            List<SelectListItem> OrganizationList = new List<SelectListItem>();
            matchProvidersList = RosterData.MatchProviders(AgencyId, CommunityIds, referralClientId);
            obj_MPM.ParentName = parentName;
            obj_MPM.AgencyId = AgencyId;
            obj_MPM.MPMList = matchProvidersList;
            if (matchProvidersList != null && matchProvidersList.Count() > 0)
            {
                //OrganizationList = RosterData.FamilyServiceList(Convert.ToInt32(matchProvidersList.FirstOrDefault().ServiceId), matchProvidersList.FirstOrDefault().AgencyId);
                OrganizationList = RosterData.FamilyServiceList(Convert.ToInt32(matchProvidersList.FirstOrDefault().ServiceId), AgencyId);

            }


            obj_MPM.OrganizationList = OrganizationList;
            return Json(obj_MPM, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReferralService(string id, string ClientName = "")
        {
            ViewBag.ClientName = ClientName;
            ViewBag.ClientID = id;
            long ID = Convert.ToInt32(EncryptDecrypt.Decrypt64(id));
            ReferralServiceModel referralService = new ReferralServiceModel();
            var ReferralList = new List<ReferralServiceModel>();
            ReferralList = RosterData.ReferralService(ID);
            referralService.referralserviceList = ReferralList;
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
            ReferralServiceModel referralService = new ReferralServiceModel();
            var ReferralList = new List<ReferralServiceModel>();
            long clientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(ClientId));
            ReferralList = RosterData.ReferralService(clientId);
            // referralService.referralserviceList = ReferralList;
            return Json(ReferralList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FamilyResourcesList(Int32 ServiceId, string AgencyId)
        {
            try
            {
                AgencyId = (AgencyId == "") ? Session["AgencyId"].ToString() : AgencyId;
                List<SelectListItem> listOrganization = new List<SelectListItem>();
                listOrganization = RosterData.FamilyServiceList(ServiceId, AgencyId);
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

        public ActionResult FamilyServiceListCompany(Int32 ServiceId, string AgencyId)
        {
            try
            {
                List<SelectListItem> listOrganization = new List<SelectListItem>();
                listOrganization = RosterData.FamilyServiceListCompany(ServiceId, AgencyId);
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
            string agencyId = Session["AgencyId"].ToString();
            referralType = RosterData.GetReferralType(communityId, agencyId);
            return Json(referralType, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrganization(Int32 CommunityId)
        {
            try
            {
                MatchProviderModel matchProviderModel = new MatchProviderModel();
                matchProviderModel = RosterData.GetOrganization(CommunityId);
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
                refOrg = RosterData.GetOrganizationCompany(CommunityId);
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
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,047c02fe-b8f1-4a9b-b01f-539d6a238d80,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CaseNotesclient(RosterNew.CaseNote CaseNote, RosterNew.ClientUsers ClientIds, RosterNew.ClientUsers TeamIds, List<RosterNew.Attachment> Attachments)
        {
            try
            {
                ViewBag.User = Session["FullName"].ToString();
                ViewBag.Date = DateTime.Now.ToString("MM/dd/yyy");
                // if (Session["RoleList"] != null)                    
                List<FingerprintsModel.Role> listRole = Session["RoleList"] as List<FingerprintsModel.Role>;
                ViewBag.Role = listRole.Select(a => a.RoleName).FirstOrDefault();
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
                CaseNote.CaseNotetags = CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Length - 1);
                List<CaseNote> CaseNoteList = new List<CaseNote>();
                FingerprintsModel.RosterNew.Users Userlist = new FingerprintsModel.RosterNew.Users();
                string Name = "";
                if (CaseNote.HouseHoldId != "0")
                {
                    CaseNote.HouseHoldId = EncryptDecrypt.Decrypt64(CaseNote.HouseHoldId);
                }
                CaseNote.IsLateArrival = false;
                string message = RosterData.SaveCaseNotes(ref Name, ref CaseNoteList, ref Userlist, CaseNote, Attachments, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                ViewBag.Name = Name;
                if (string.IsNullOrEmpty(CaseNote.ClientId))
                    ViewBag.Client = "0";
                else
                    ViewBag.Client = CaseNote.ClientId;

                ViewBag.CaseNotelist = CaseNoteList;
                ViewBag.Userlist = Userlist;
                ViewBag.centerid = CaseNote.CenterId;
                if (string.IsNullOrEmpty(CaseNote.ProgramId))
                    ViewBag.Programid = "0";
                else
                    ViewBag.Programid = CaseNote.ProgramId;
                if (string.IsNullOrEmpty(CaseNote.HouseHoldId) || CaseNote.HouseHoldId == "0")
                    ViewBag.Householdid = "0";
                else
                    ViewBag.Householdid = EncryptDecrypt.Encrypt64(CaseNote.HouseHoldId);
                if (message == "1")
                {
                    ViewBag.message = "Case Note saved successfully.";

                }
                else
                    ViewBag.message = "Please try again.";
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View();
        }
        //Changes on 29Dec2016
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult Getcasenotedetails(string Casenoteid, string ClientId)
        {
            try
            {
                return Json(RosterData.GetcaseNoteDetail(Casenoteid, ClientId, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetElement(string DomainId = "0")
        {
            try
            {
                return Json(RosterData.GetElementInfo(DomainId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
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
                return Json("Error occured please try again.");
            }
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult GetFPAInfo(string FPAID = "0")
        {
            try
            {
                ViewBag.mode = 1;
                return Json(RosterData.GetFPADetails(FPAID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult DeleteStepView(string StepId)
        {
            try
            {
                ViewBag.mode = 1;
                return Json(RosterData.DeleteStepView(StepId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again after some time.");
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
                object Delete = (RosterData.DeleteFPA(strfpaid));
                return Json(Delete, JsonRequestBehavior.AllowGet);


            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again after some time.");
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
                var list = RosterData.GetFPAGoalListForHousehold(out totalrecord, search, clientid, sortOrder, sortDirection, skip, pageSize);

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
                    obj = RosterData.GetFpa(Convert.ToInt64(obj.FPAID));
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
                var FPADATA = new RosterData().GetFpa(iFPAid);
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
            RosterData objdata = new RosterData();
            obj = objdata.GetData_AllDropdown();
            ViewBag.CateList = obj.cateList;
            TempData["CateList"] = ViewBag.CateList;
            ViewBag.DomList = obj.domList;
            // TempData["DomList"] = ViewBag.DomList;

            if (Request.QueryString.AllKeys.Contains("FPAid") && !string.IsNullOrEmpty(Request.QueryString["FPAid"].ToString()))
            {
                obj.FPAID = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["FPAid"].ToString()));
                obj = RosterData.GetFpa(obj.FPAID);
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
                    DataSet ds = RosterData.getParentNames(obj.ClientId);
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
                RosterData objdata = new RosterData();
                objNew = objdata.GetData_AllDropdown();
                DataSet ds = RosterData.getParentNames(obj.ClientId);
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
            RosterData objdata = new RosterData();
            FPA obj = new FPA();
            if (!string.IsNullOrEmpty(command))
            {
                try
                {
                    Export export = new Export();
                    obj = objdata.GetData_AllDropdown();
                    obj.GoalFor = Convert.ToInt32(Request.Form["GoalFor"].ToString());
                    ViewBag.DomList = obj.domList;
                    TempData["DomList"] = ViewBag.DomList;
                    ViewBag.CateList = obj.cateList;
                    TempData["CateList"] = ViewBag.CateList;

                    obj = info;
                    //  string fpaid = FingerprintsModel.EncryptDecrypt.Decrypt64(info.FPAID);
                    obj = RosterData.GetFpa(Convert.ToInt32(info.FPAID));
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

                obj = objdata.GetData_AllDropdown();
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
                int Mode = 2;
                try
                {
                    info.Category = (collection["DdlCateList"] == null) ? null : collection["DdlCateList"].ToString();
                    info.Domain = (collection["DdlDomList"] == null) ? null : collection["DdlDomList"].ToString();
                    if (info.FPAID > 0)
                    {
                        //Update Data
                        string UpdateParameter = "UPDATE";
                        message = objdata.AddFPA(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                        objdata.CheckByClient(UpdateParameter, Mode);
                    }
                    else
                    {
                        //Insert Data
                        string InsertParameter = "INSERT";
                        message = objdata.AddFPA(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                        objdata.CheckByClient(InsertParameter, Mode);
                    }
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
                            return Redirect("~/Roster/FPAList?id=" + EncryptDecrypt.Encrypt64(Convert.ToString(info.ClientId)));
                        }
                    }
                    else if (message == "2")
                    {
                        ViewBag.result = "Sucess";
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
            RosterData objdata = new RosterData();
            obj = objdata.GetData_AllDropdown();
            ViewBag.CateList = obj.cateList;
            TempData["CateList"] = ViewBag.CateList;
            ViewBag.DomList = obj.domList;

            if (Request.QueryString.AllKeys.Contains("FPAid") && !string.IsNullOrEmpty(Request.QueryString["FPAid"].ToString()))
            {
                obj.FPAID = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["FPAid"].ToString()));
                obj = RosterData.GetFpaforParents(obj.FPAID);
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
                DataSet ds = RosterData.getParentNames(obj.ClientId);
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
            RosterData objdata = new RosterData();
            FPA obj = new FPA();

            obj = objdata.GetData_AllDropdown();
            obj = RosterData.GetFpa(info.FPAID);
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
                info.Category = (collection["DdlCateList"] == null) ? null : collection["DdlCateList"].ToString();
                info.Domain = (collection["DdlDomList"] == null) ? null : collection["DdlDomList"].ToString();

                message = objdata.UpdateFPAParent(obj);//, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());

                message = objdata.AddFPA(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
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

                RosterData objdata = new RosterData();
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

                string message = objdata.AddFPA(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
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
                return Json(RosterData.GetgroupcaseNoteDetail(Casenoteid, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        public void CompleteServicePdf(string ServiceId, string AgencyID, string ClientID, string CommunityID, string Notes, string referralDate)
        {
            var ReferredName = Session["FullName"];
            string clientId = EncryptDecrypt.Decrypt64(ClientID).ToString();
            PDFGeneration obj_REF = new PDFGeneration();
            var refList = new List<PDFGeneration>();
            refList = RosterData.CompleteServicePdf(clientId);

            var refList1 = new List<CompanyDetails>();
            refList1 = RosterData.CompanyDetailsList(ServiceId);

            var refList2 = new List<CommunityDetails>();
            refList2 = RosterData.CommunityDetailsList(CommunityID);

            var refList3 = new List<BusinessHours>();
            refList3 = RosterData.BusinessHoursList(ServiceId, AgencyID, CommunityID);
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
            AgencyID = Session["AgencyId"].ToString();
            var refList3 = new List<BusinessHours>();
            refList3 = RosterData.BusinessHoursList(ServiceId, AgencyID, CommunityID);

            return Json(refList3, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult MatrixAnalysis(string id, string Householdid, string centerid, string Programid, string ClientName)
        {
            MatrixScore score = new MatrixScore();

            long householdID = Convert.ToInt64(EncryptDecrypt.Decrypt64(Householdid));
            long ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
            long programId = Convert.ToInt64(EncryptDecrypt.Decrypt64(Programid));
            Guid agencyId = new Guid(Session["AgencyID"].ToString());
            ViewBag.ClientName = ClientName;

            score = new RosterData().GetMatrixScoreList(householdID, agencyId, ClientId, programId);
            score.HouseHoldId = Householdid;
            score.CenterId = centerid;
            score.ClientId = id;
            score.ProgramId = Programid;
            if (score.MatrixScoreList.Count > 0)
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
            Guid agencyId = new Guid(Session["AgencyID"].ToString());
            List<ShowRecommendations> recommList = new List<ShowRecommendations>();
            matrixscore = new RosterData().GetClientDetails(out recommList, householdID, agencyId);
            return Json(new { matrixscore, recommList }, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetRecommendations(string HouseholdId, long assessmentNo, string activeProgramYear)
        {
            Guid agencyId = new Guid(Session["AgencyID"].ToString());
            long householdID = Convert.ToInt32(EncryptDecrypt.Decrypt64(HouseholdId));
            ArrayList recommList = new ArrayList();
            recommList = new RosterData().GetRecommendations(householdID, assessmentNo, agencyId, activeProgramYear);
            return Json(recommList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetDescripton(int groupId, string clientId)
        {
            long dec_ClientID = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientId));
            List<AssessmentResults> results = new List<AssessmentResults>();
            Guid agencyId = new Guid(Session["AgencyID"].ToString());

            results = new RosterData().GetDescription(groupId, dec_ClientID, agencyId);

            return Json(results, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetQuestions(long groupId, string clientId)
        {
            long dec_ClientID = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientId));
            List<QuestionsModel> questionlist = new List<QuestionsModel>();
            QuestionsModel question = new QuestionsModel();
            QuestionsModel questionmodel = null;
            question = new RosterData().GetQuestions(groupId, dec_ClientID);
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
                matrixscore.AgencyId = new Guid(Session["AgencyID"].ToString());
                matrixscore.UserId = new Guid(Session["UserID"].ToString());
                lastId = new RosterData().InsertMatrixScore(matrixscore, out isShow, out recommendationList);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { lastId, isShow, recommendationList }, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public JsonResult GetChartDetails(string houseHoldId, string date, string clientId)
        {
            List<MatrixScore> scoreList = new List<MatrixScore>();
            List<ChartDetails> chardetailsList = new List<ChartDetails>();
            long houseHold = Convert.ToInt64(EncryptDecrypt.Decrypt64(houseHoldId));
            long ClientID = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));
            AnnualAssessment assessment = new AnnualAssessment();
            int groupType = 0;
            Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            Guid? userID = (Session["UserID"] != null) ? new Guid(Session["UserID"].ToString()) : (Guid?)null;
            scoreList = new RosterData().GetChartDetails(out assessment, out chardetailsList, AgencyId, userID, houseHold, date, ClientID);
            long type = assessment.AnnualAssessmentType;
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;
            DateTime date3 = DateTime.Now;
            DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));

            if (type == 1)
            {
                date1 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment1To).ToString("MM/dd/yyyy"));
            }

            if (type == 2)
            {
                date1 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment1To).ToString("MM/dd/yyyy"));
                date2 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment2To).ToString("MM/dd/yyyy"));
            }
            if (type == 3)
            {
                date1 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment1To).ToString("MM/dd/yyyy"));
                date2 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment2To).ToString("MM/dd/yyyy"));
                date3 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment3To).ToString("MM/dd/yyyy"));
            }

            switch (type)
            {
                case 1:
                    groupType = (date1 >= currentDate) ? 1 : 0;
                    break;
                case 2:
                    groupType = (date1 >= currentDate) ? 1 : (date2 >= currentDate) ? 2 : 0;
                    break;
                case 3:
                    groupType = (date1 >= currentDate) ? 1 : (date2 >= currentDate) ? 2 : (date3 >= currentDate) ? 3 : 0;
                    break;
            }

            List<MatrixScore> matrixscorelist = null;
            List<long> categoryIdList = new List<long>();
            System.Collections.ArrayList arraylist = new System.Collections.ArrayList();
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

            return Json(new { scoreList, groupType, chardetailsList, arraylist }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetChart()
        {
            List<MatrixScore> scoreList = new List<MatrixScore>();
            Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            scoreList = new RosterData().SetChart(AgencyId);
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
                matrixscore.AgencyId = new Guid(Session["AgencyID"].ToString());
                matrixscore.UserId = new Guid(Session["UserID"].ToString());
                scoreList = new RosterData().GetName(matrixscore);

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
                isResult = new RosterData().InsertMatrixRecommendationData(matrixRec);

            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }


        [JsonMaxLengthAttribute]
        public JsonResult GetChildrenImage(string enc_clientId)
        {
            SelectListItem childImage = new SelectListItem();
            try
            {
                long clientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_clientId));

                childImage = new RosterData().GetChildrenImageData(clientId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childImage, JsonRequestBehavior.AllowGet);
        }
        [JsonMaxLengthAttribute]
        public JsonResult GetAttendenceByDate(string selectedDate, string clientId)
        {
            AttendenceDetailsByDate attendence = new AttendenceDetailsByDate();
            try
            {
                long de_clientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId));

                attendence = new RosterData().GetAttendenceDetailsByDate(selectedDate, de_clientId, Session["AgencyID"].ToString());

                attendence.SignedInTime = Convert.ToDateTime(attendence.SignedInTime).ToString("hh:mm tt");
                attendence.SignedOutTime = Convert.ToDateTime(attendence.SignedOutTime).ToString("hh:mm tt");
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(attendence, JsonRequestBehavior.AllowGet);
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
            CaseNote.CaseNotetitle = Convert.ToString(collection.Get("CaseNotetitle"));
            CaseNote.CaseNoteDate = Convert.ToString(collection.Get("CaseNoteDate"));

            CaseNote.IsLateArrival = true;
            string message = RosterData.SaveCaseNotes(ref Name, ref CaseNoteList, ref Userlist, CaseNote, Attachments, Session["AgencyID"].ToString(), Session["UserID"].ToString());

            return Redirect("~/Roster/Roster");
        }
    }
}
