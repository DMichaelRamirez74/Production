using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.ViewModel;
using Fingerprints.Filters;
using System.Text;
using FingerprintsModel.Enums;

namespace Fingerprints.Controllers
{
    public class YakkrController : Controller
    {
        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         role=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
         */

        StaffDetails staff = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();

        [CustAuthFilter(RoleEnum.GenesisEarthAdministrator,RoleEnum.AgencyAdmin)]
       //[CustAuthFilter()]
        public ActionResult Yakkr()
        {
            //if (!String.IsNullOrWhiteSpace(Convert.ToString(Session["AgencyID"])))
            //{
            ViewBag.mode = 0;
            Yakkr _Yakkr = (new YakkrData()).GetData_YakkrData(Session["AgencyID"].ToString());
            ViewBag.YakkrList = _Yakkr.YakkrList;
            TempData["YakkrList"] = ViewBag.YakkrList;
            ViewBag._YakkrRolesList = _Yakkr._YakkrRolesList;
            TempData["_YakkrRolesList"] = ViewBag._YakkrRolesList;
            ViewBag._YakkrAgencyCodes = _Yakkr._YakkrAgencyCodes;
            TempData["_YakkrAgencyCodes"] = ViewBag._YakkrAgencyCodes;
            return View(_Yakkr);
            //}


        }
        [HttpPost]
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public ActionResult Yakkr(Yakkr info, FormCollection collection)
        {
            try
            {

                TempData.Keep();
                string message = "";
                YakkrData obj = new YakkrData();
                if (info.YakkrRoleID == 0)
                {
                    info.YakkrID = collection["DdlYakkrList"].ToString() == "0" ? null : collection["DdlYakkrList"].ToString();
                    info.StaffRoleID = collection["DdlStaffList"].ToString() == "0" ? null : collection["DdlStaffList"].ToString();

                    info.SecondaryRoleID = collection["DdlStaffSecondaryList"] == null ? null : collection["DdlStaffSecondaryList"].ToString();
                    message = obj.AddYakkrInfo(info, 0, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                    ViewBag.result = "Sucess";
                }
                else
                {
                    info.YakkrID = collection["DdlYakkrList"].ToString() == "0" ? null : collection["DdlYakkrList"].ToString();
                    info.StaffRoleID = collection["DdlStaffList"].ToString() == "0" ? null : collection["DdlStaffList"].ToString();

                    info.SecondaryRoleID = collection["DdlStaffSecondaryList"] == null ? null : collection["DdlStaffSecondaryList"].ToString();
                    message = obj.AddYakkrInfo(info, 1, Guid.Parse(Session["UserID"].ToString()), Session["AgencyID"].ToString());
                    ViewBag.result = "Sucess";
                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
                    ViewBag.message = "Record added successfully.";
                }
                else if (message == "2")
                {
                    ViewBag.message = "Record updated successfully.";
                }
                else if (message == "3")
                {
                    ViewBag.message = "Record already exist.";
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            info.YakkrList = (List<Yakkr.YakkrCode>)TempData["YakkrList"];
            info._YakkrRolesList = (List<Yakkr.YakkrRoles>)TempData["_YakkrRolesList"];
            info._YakkrAgencyCodes = (List<Yakkr.YakkrAgencyCodes>)TempData["_YakkrAgencyCodes"];
            return View(info);

        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult YakkrDetails(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                YakkrData info = new YakkrData();
                string totalrecord;
                int skip = pageSize * (requestedPage - 1);
                var list = info.YakkrInfoDetails(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Session["AgencyID"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
            // return View();
        }

        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Getyakkrdetails(string YakkrRoleID = "0")
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(obj.Getyakkrinfo(YakkrRoleID, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult Deleteyakkr(string YakkrRoleID = "0")
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(obj.Deleteyakkrinfo(YakkrRoleID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
        public JsonResult GetYakkrDetailslist(string YakkrId = "0")
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(obj.Getyakkrdetailinfo(YakkrId, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult YakkrDetail()
        {
            try
            {
                int yakkrcount = 0;
                ViewBag.YakkrDetail = new YakkrData().YakkrDetail(ref yakkrcount, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                Session["Yakkrcount"] = yakkrcount;
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }

        }

        [HttpGet]
        [CustAuthFilter()]
        public ActionResult YakkrDetails()
        {

            List<YakkrDetail> listYakkr = new List<FingerprintsModel.YakkrDetail>();
            try
            {
                string Status = "1";

                listYakkr = new YakkrData().GetYakkrDetail(new Guid(Session["AgencyID"].ToString()), new Guid(Session["UserID"].ToString()), Status);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(listYakkr);
        }


        [HttpGet]

        [CustAuthFilter()]
        public ActionResult YakkrList(string YakkrCode)
        {
            List<YakkrClientDetail> listYakkr = new List<FingerprintsModel.YakkrClientDetail>();
            try
            {

                string yakkrDescription;

                ViewBag.YakkrCode = YakkrCode;
              
                string Status = "1";

                listYakkr = new YakkrData().GetYakkrListByCode(out yakkrDescription,YakkrCode.ToString(), Status);
                ViewBag.YakkrDescription = yakkrDescription;
                if (listYakkr.Count() == 0)
                {
                    return RedirectToAction("YakkrDetails");
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(listYakkr);
        }


        //[HttpPost]
        //public ActionResult SenMailToParentsAndTeachers(string YakkrID, string recordType = "0")
        //{
        //    bool Result = false;
        //    try
        //    {
        //        Result = new CenterData().UpdateDaysOffByYakkr(YakkrID, Session["UserID"].ToString(), Session["AgencyId"].ToString(), Session["RoleId"].ToString(), recordType);
        //        if (Result)
        //        {
        //            Result = false;

        //            new ParentData().UpdateStatusChange("0", Session["UserID"].ToString(), "73", "", YakkrID);
        //            Dictionary<String, String> dictEmail = new Dictionary<string, string>();
        //            if (Session["UserID"] != null)
        //                dictEmail = new YakkrData().SenMailToParentsAndTeachers(new Guid(Session["UserID"].ToString()), YakkrID, recordType);
        //            if (dictEmail.Count() > 0)
        //            {
        //                //string approvalMessage=(recordType=="2")?
        //                foreach (KeyValuePair<string, string> pair in dictEmail)
        //                {
        //                    SendMail.SendEmailToParentAndTeacher(pair.Value.ToString(), "Classroom closed approval mail", Session["EmailID"].ToString());
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


        [HttpPost]
        public ActionResult SenMailToParentsAndTeachers(string YakkrID)
        {
            bool Result = false;
            try
            {
                new ParentData().UpdateStatusChange("0", Session["UserID"].ToString(), "73", "", YakkrID);
                Dictionary<String, String> dictEmail = new Dictionary<string, string>();
                if (Session["UserID"] != null)
                    dictEmail = new YakkrData().SenMailToParentsAndTeachers(new Guid(Session["UserID"].ToString()));
                if (dictEmail.Count() > 0)
                {
                    foreach (KeyValuePair<string, string> pair in dictEmail)
                    {
                        SendMail.SendEmailToParentAndTeacher(pair.Value.ToString(), "Classroom closed approval mail", Session["EmailID"].ToString());
                    }
                }

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }



        public JsonResult GetCaseNoteDetailsByYakkr(string householdid,string clientId, string yakkrId)
        {
            //  InternalRefferalCaseNote caseNote = new InternalRefferalCaseNote();
            CaseNoteByClientID casnote = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<CaseNoteByClientID>();
            try
            {
                var caesNoteID = new YakkrData().GetCaseNoteByYakkr(yakkrId);

                casnote.CaseNote = new CaseNote();
                casnote.CaseNote.ClientId = clientId;
                casnote.CaseNote.CaseNoteid =Convert.ToString(caesNoteID);
                casnote.CaseNote.HouseHoldId = householdid;

                casnote= new RosterData().GetCaseNoteByCaseNoteId(casnote, staff);

            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(casnote, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteYakkrByYakkrId(long clientId, long yakkrId)
        {
            bool isResult = false;
            try
            {

                isResult = new YakkrData().DeleteYakkrRoutingById(clientId, yakkrId);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }


        #region yakkr451&453

        [HttpGet]
        [CustAuthFilter()]
        public ActionResult QuestionnaireForm(int yakkrid,string cn, int cid,int center=0, int hid=0)
        {
            try
            {
                //  int centerid = 75;
                // string id = "0";
                //int Householdid = 0;
                //string Name = "";
                ViewBag.Name = cn;
                ViewBag.YakkarId = yakkrid;
                ViewBag.HouseHoldId = hid;
                ViewBag.ClientId = cid;

                ViewBag.QSDetails = new YakkrData().GetQuestionaireByYakkrId(yakkrid, 2);

                CaseNoteByClientID caseNotebyClientID = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<CaseNoteByClientID>();

                //RosterNew.Users Userlist = new RosterNew.Users();
                //var Rd = new RosterData();
                //    caseNotebyClientID.Enc_HouseholdID = EncryptDecrypt.Encrypt64(hid.ToString());
                //    caseNotebyClientID.Enc_ClientID = EncryptDecrypt.Encrypt64(cid.ToString());
                //    Rd.GetCaseNote(ref Name, caseNotebyClientID,staff);
                //ViewBag.Userlist = Userlist.UserList;

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View();
        }

        [CustAuthFilter()]
        public JsonResult GetYakkr451DetailsById(int id)
        {

           var result =  new YakkrData().GetQuestionaireByYakkrId(id,3);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustAuthFilter()]
        //[ValidateInput(false)]
        public ActionResult InsertQuestionnaireForm(Questionaire qsform, RosterNew.CaseNote CaseNote)
        {

            bool caseNoteResult = true;
            bool questionnaireResult = false;

            if (qsform.AppointmentMaked == 0)
            {

                string message = "";
                string Name = "";

                //insert casenote

                CaseNote.CaseNoteAttachmentList.ForEach(x =>
                {
                    x.AttachmentFileName = string.IsNullOrEmpty(x.AttachmentFileName) ? "CaseNoteAttachment" : x.AttachmentFileName;

                    if (!string.IsNullOrEmpty(x.AttachmentJson))
                    {
                        x.AttachmentFileByte = Convert.FromBase64String(x.AttachmentJson);
                    }

                });

                message = new RosterData().SaveCaseNotes(ref Name, CaseNote, staff, 2);

                if (message != "1")
                    caseNoteResult = false;


                if (!string.IsNullOrEmpty(Name))
                {
                    qsform.CaseNoteId = Convert.ToInt32(Name);
                }


            }

            if (caseNoteResult)
            {
                questionnaireResult = new YakkrData().InsertQuestionaireForm(qsform, staff);

                if (!questionnaireResult)
                {
                    new RosterData().DeleteCaseNote(casenoteid:Convert.ToInt32(CaseNote.CaseNoteid), appendcid:new int[] { }, deletemain: true, mode:1);
                }

            }





            ////return View();
            //if (qsform.AppointmentMaked == 0 && qsform.ReceiveAnotherReferral)
            //{

            //    string ID = EncryptDecrypt.Encrypt64(CaseNote.ClientId);
            //    return new RedirectResult("~/Roster/ReferralService?id=" + ID + "&ClientName=" + CaseNote.ClientName + "");
            //}
            //else
            //{
            //    return new RedirectResult("~/Yakkr/YakkrList?YakkrCode=450");
            //}


            return Json(new { result= questionnaireResult, encClientId = EncryptDecrypt.Encrypt64(CaseNote.ClientId) }, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public ActionResult Organizationalissue(int id, string clientid, string center, string household, string clientname)
        {

            var result = new YakkrData().GetQuestionaireByYakkrId(id, 3);

          

            ViewBag.ClientName = clientname;
            ViewBag.ClientId = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientid));


            //CaseNoteByClientID caseNotebyClientId = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<CaseNoteByClientID>();
            //string Name = "";
            //caseNotebyClientId.Enc_ClientID = clientid;
            //caseNotebyClientId.Enc_HouseholdID = household;
            //caseNotebyClientId.CenterID = center;
            //RosterNew.Users Userlist = new RosterNew.Users();
            //var Rd = new RosterData();
            //var stf = StaffDetails.GetInstance();
            //Rd.GetCaseNote(ref Name, caseNotebyClientId, stf);
            //ViewBag.Userlist = Userlist.UserList;

            return View(result);
        }

        [HttpPost]
        [CustAuthFilter()]
       // [ValidateInput(false)]
        //public ActionResult InsertOrganizationalissue(int ProblemOn, int? CRColorCode, int QuestionaireID, int CommunityId, int Yakkrid,
        //   string MgNotes, RosterNew.CaseNote CaseNote, List<RosterNew.Attachment> Attachments, RosterNew.ClientUsers TeamIds)
        public ActionResult InsertOrganizationalIssue(ReferalDetails referralDetails, RosterNew.CaseNote caseNote, int yakkrId)
        {

            bool caseNoteResult = true;
            bool questionnaireResult = false;
            int caseNoteId = 0;

            if (referralDetails.ProblemOn==1)
            {
                string message = "";
                string Name = "";

                //insert casenote


                if(caseNote != null )
                {

                    if(caseNote.CaseNoteAttachmentList != null)
                    {
                        caseNote.CaseNoteAttachmentList.ForEach(x =>
                        {
                            x.AttachmentFileName = string.IsNullOrEmpty(x.AttachmentFileName) ? "OrganizationalIssue_CaseNoteAttachment" : x.AttachmentFileName;

                            if (!string.IsNullOrEmpty(x.AttachmentJson))
                            {
                                x.AttachmentFileByte = Convert.FromBase64String(x.AttachmentJson);
                            }

                        });

                    }
                   

                    message = new RosterData().SaveCaseNotes(ref Name, caseNote, staff, 2);

                }


                if (message != "1")
                    caseNoteResult = false;


                if (!string.IsNullOrEmpty(Name))
                {
                    caseNoteId = Convert.ToInt32(Name);
                }

            }



            if (caseNoteResult)
            {
                questionnaireResult = new YakkrData().SubmitFeedBack453(4, referralDetails, staff, yakkrId, caseNoteId);

                if (!questionnaireResult)
                {
                    new RosterData().DeleteCaseNote(casenoteid: caseNoteId, appendcid: new int[] { }, deletemain: true, mode: 1);
                }

            }

            //if (ProblemOn == 1)
            //{
            //    StringBuilder _Ids = new StringBuilder();
            //    if (TeamIds.IDS != null)
            //    {
            //        foreach (string str in TeamIds.IDS)
            //        {
            //            _Ids.Append(str + ",");
            //        }
            //        CaseNote.StaffIds = _Ids.ToString().Substring(0, _Ids.Length - 1);
            //    }
            //    CaseNote.CaseNotetags = (CaseNote != null && !string.IsNullOrEmpty(CaseNote.CaseNotetags)) ? CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Length - 1) : "";
            //}
            //CaseNote.CaseNoteAttachmentList = Attachments;
            //var result = new YakkrData().SubmitFeedBack453(4, ProblemOn, CRColorCode, CommunityId, QuestionaireID, Yakkrid, MgNotes, CaseNote);


            return Json(new { result=questionnaireResult }, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public PartialViewResult ReferralIssueSummary(int yakkrid,string clientname)
        {

            var result = new YakkrData().GetQuestionaireByYakkrId(yakkrid,5);

            ViewBag.CName = clientname;
            ViewBag.Yakkr453 = yakkrid;

            return PartialView("~/Views/Partialviews/_ReferralIssueSummaryPartial.cshtml", result);
        }

        [CustAuthFilter()]
        public ActionResult ClearYakkrOfReferralIssueSummary(int yakkrid)
        {
            var result = false;
            try
            {
                result = new YakkrData().ClearYakkrOfReferralIssueSummary(yakkrid);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            // return new RedirectResult("~/Yakkr/YakkrList?YakkrCode=453");
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        #endregion  yakkr451&453


        #region Get Yakkr Count by User



        [HttpPost]
        [CustAuthFilter()]
        public JsonResult GetYakkrCountByUser()
        {
            int yakkrCount = 0;
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();
                yakkrCount= new YakkrData().GetYakkrCountByUserId((Guid)staff.AgencyId, (Guid)staff.UserId, Status:"1");
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);

            }

            return Json(yakkrCount, JsonRequestBehavior.AllowGet);
        }

        #endregion Yakkr Count by User


    }
}
