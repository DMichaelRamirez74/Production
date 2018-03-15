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
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System.Web.Helpers;

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
         */
        RosterData RosterData = new RosterData();
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult Roster()
        {
            return View();
        }
        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult listRoster(string sortOrder, string sortDirection, string Center, string Classroom, int pageSize, int requestedPage = 1)
        {
            try
            {
                int skip = pageSize * (requestedPage - 1);
                string totalrecord;
                var list = RosterData.GetrosterList(out totalrecord, sortOrder, sortDirection, Center, Classroom, skip, pageSize, Convert.ToString(Session["UserID"]), Session["AgencyID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("");
            }
        }
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
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

            }
            return Json(_imgpath, JsonRequestBehavior.AllowGet);
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
            try
            {
                TempData.Keep("tempClientId");
                string cId = TempData["tempClientId"].ToString();
                int ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(cId));
                SaveProvider.Description = (SaveProvider.Description == null) ? SaveProvider.Description = "" : SaveProvider.Description;
                bool Success = false;
                string UserId = Session["UserID"].ToString();
                Success = RosterData.SaveMatchProviders(SaveProvider.ReferralDate, SaveProvider.Description, SaveProvider.ServiceResourceId, SaveProvider.AgencyId, UserId, ClientId, SaveProvider.CommunityId, SaveProvider.ReferralClientServiceId);
                RosterData.YakkarInsert(SaveProvider.AgencyId, UserId, ClientId);
                return Success;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveReferral(ListRoster Savereferral)
        {
            try
            {
                TempData.Keep("tempClientId");
                string cId = TempData["tempClientId"].ToString();
                int commanclient = Convert.ToInt32(EncryptDecrypt.Decrypt64(cId));
                int Step = 3;
                bool Success = false;
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
                return Success;
            }
            catch (Exception ex)
            {
                return false;
            }
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

                StringBuilder _Ids = new StringBuilder();
                string Name = "";
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

                if (CaseNote.HouseHoldId != "0")
                {
                    CaseNote.HouseHoldId = EncryptDecrypt.Decrypt64(CaseNote.HouseHoldId);
                }
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
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
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
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [HttpPost]
        public JsonResult delFPAInfo(string FPAID)
        {
            try
            {
                string strfpaid = FingerprintsModel.EncryptDecrypt.Decrypt64(FPAID);
                ViewBag.mode = 1;
                return Json(RosterData.DeleteFPA(strfpaid));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again after some time.");
            }
        }
        [JsonMaxLengthAttribute]
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
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
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult FPAList()
        {
            FPA obj = new FingerprintsModel.FPA();
            string Householdid = ""; string centerid = ""; string Programid = "";
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
        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
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
                try
                {
                    info.Category = (collection["DdlCateList"] == null) ? null : collection["DdlCateList"].ToString();
                    info.Domain = (collection["DdlDomList"] == null) ? null : collection["DdlDomList"].ToString();
                    if (info.FPAID > 0)
                    {
                        message = objdata.AddFPA(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                    }
                    else
                    {

                        message = objdata.AddFPA(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
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

        public ActionResult MatrixAnalysis(string id, string Householdid, string centerid, string Programid)
        {
            MatrixScore score = new MatrixScore();

            long householdID = Convert.ToInt64(EncryptDecrypt.Decrypt64(Householdid));
            long ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
            long programId = Convert.ToInt64(EncryptDecrypt.Decrypt64(Programid));
            Guid agencyId = new Guid(Session["AgencyID"].ToString());

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

        public JsonResult GetClientStatus(string HouseHoldID)
        {
            MatrixScore matrixscore = new MatrixScore();
            long householdID = Convert.ToInt32(EncryptDecrypt.Decrypt64(HouseHoldID));
            Guid agencyId = new Guid(Session["AgencyID"].ToString());
            matrixscore = new RosterData().GetClientDetails(householdID, agencyId);
            return Json(matrixscore, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDescripton(int groupId, string clientId)
        {
            long dec_ClientID = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientId));
            List<AssessmentResults> results = new List<AssessmentResults>();
            Guid agencyId = new Guid(Session["AgencyID"].ToString());

            results = new RosterData().GetDescription(groupId, dec_ClientID, agencyId);

            return Json(results, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
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
        public JsonResult InsertMatrixScore(MatrixScore matrixscore)
        {
            long lastId = matrixscore.MatrixScoreId;
            try
            {


                matrixscore.Dec_HouseHoldId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.HouseHoldId));
                matrixscore.Dec_ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.ClientId));
                matrixscore.Dec_ProgramId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.ProgramId));
                matrixscore.Dec_CenterId = Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.CenterId));
                matrixscore.AgencyId = new Guid(Session["AgencyID"].ToString());
                matrixscore.UserId = new Guid(Session["UserID"].ToString());
                lastId = new RosterData().InsertMatrixScore(matrixscore);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(lastId, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetChartDetails(string houseHoldId, string date)
        {
           List<MatrixScore>scoreList = new List<MatrixScore>();
            List<ChartDetails> chardetailsList = new List<ChartDetails>();
            long houseHold= Convert.ToInt64(EncryptDecrypt.Decrypt64(houseHoldId));
            AnnualAssessment assessment = new AnnualAssessment();
            int groupType = 0;
              Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            Guid? userID = (Session["UserID"] != null) ? new Guid(Session["UserID"].ToString()) : (Guid?)null;
            scoreList = new RosterData().GetChartDetails(out assessment,out chardetailsList, AgencyId,userID,houseHold,date);
            long type = assessment.AnnualAssessmentType;
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now;
            DateTime date3 = DateTime.Now;
            DateTime currentDate= Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));

            if (type==1)
            {
                date1=  Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment1To).ToString("MM/dd/yyyy"));
            }

            if (type == 2)
            {
                date1 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment1To).ToString("MM/dd/yyyy"));
                 date2 =Convert.ToDateTime( Convert.ToDateTime(assessment.Assessment2To).ToString("MM/dd/yyyy"));
            }
            if (type == 3)
            {
               date1 =Convert.ToDateTime( Convert.ToDateTime(assessment.Assessment1To).ToString("MM/dd/yyyy"));
                date2 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment2To).ToString("MM/dd/yyyy"));
                 date3 = Convert.ToDateTime(Convert.ToDateTime(assessment.Assessment3To).ToString("MM/dd/yyyy"));
            }

            switch (type)
            {
                case 1:
                    groupType = (date1 >= currentDate) ? 1 : 0;
                    break;
                case 2:
                  groupType=  (date1 >= currentDate) ?1 : (date2 >= currentDate) ?  2:0;
                    break;
                case 3:
                    groupType = (date1 >= currentDate) ? 1 : (date2 >= currentDate) ? 2 : (date3 >= currentDate) ? 3 : 0;
                    break;
            }

            List<MatrixScore> matrixscorelist = null;
            List<long> categoryIdList = new List<long>();
            System.Collections.ArrayList arraylist = new System.Collections.ArrayList();
            categoryIdList = scoreList.Select(x => x.AssessmentCategoryId).Distinct().ToList();
            if(categoryIdList != null && categoryIdList.Count>0)
            {
                foreach(int categoryId in categoryIdList)
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
          categoryList= scoreList.Select(x => x.AssessmentCategoryId).Distinct().ToList();
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

        public JsonResult GetName(MatrixScore matrixscore)
        {
            List<MatrixScore> scoreList = new List<MatrixScore>();
            try
            {
                matrixscore.Dec_HouseHoldId=(string.IsNullOrEmpty(matrixscore.HouseHoldId))?0: Convert.ToInt64(EncryptDecrypt.Decrypt64(matrixscore.HouseHoldId));
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
    }
}
