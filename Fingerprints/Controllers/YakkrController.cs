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
        [CustAuthFilter("a65bb7c2-e320-42a2-aed4-409a321c08a5")]
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

                ViewBag.YakkrCode = YakkrCode;
                string Status = "1";

                listYakkr = new YakkrData().GetYakkrListByCode(YakkrCode.ToString(), Status);

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



        public JsonResult GetCaseNoteDetailsByYakkr(string clientId, string yakkrId)
        {
            InternalRefferalCaseNote caseNote = new InternalRefferalCaseNote();
            try
            {
                caseNote = new YakkrData().GetCaseNoteByYakkr(clientId, yakkrId);

            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(caseNote,JsonRequestBehavior.AllowGet);
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


        #region yakkr451

        [HttpGet]
        [CustAuthFilter()]
        public ActionResult QuestionnaireForm(int yakkrid,string cn, int cid,int center=0, int hid=0)
        {

          //  int centerid = 75;
           // string id = "0";
            //int Householdid = 0;
            string Name = "";
            ViewBag.Name = cn;
            ViewBag.YakkarId = yakkrid;
            ViewBag.HouseHoldId = hid;
            ViewBag.ClientId = cid;

            ViewBag.QSDetails = new YakkrData().GetQuestionaireByYakkrId(yakkrid,2);
            RosterNew.Users Userlist = new RosterNew.Users();
            var Rd = new RosterData();
               
                Rd.GetCaseNote(ref Name, ref Userlist, hid, center, cid.ToString(), Session["AgencyID"].ToString(), Session["UserID"].ToString());
            ViewBag.Userlist = Userlist.UserList;



            return View();
        }

        [CustAuthFilter()]
        public JsonResult GetYakkr451DetailsById(int id) {

           var result =  new YakkrData().GetQuestionaireByYakkrId(id,3);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustAuthFilter()]
        [ValidateInput(false)]
        public ActionResult QuestionnaireForm(Questionaire qsform, RosterNew.CaseNote CaseNote, List<RosterNew.Attachment> Attachments, RosterNew.ClientUsers TeamIds, bool anotherref=false)
        {
            if (qsform.AppointmentMaked == 0)
            {
                StringBuilder _Ids = new StringBuilder();
                if (TeamIds.IDS != null)
                {
                    foreach (string str in TeamIds.IDS)
                    {
                        _Ids.Append(str + ",");
                    }
                    CaseNote.StaffIds = _Ids.ToString().Substring(0, _Ids.Length - 1);
                }

                CaseNote.CaseNotetags = (CaseNote != null && !string.IsNullOrEmpty(CaseNote.CaseNotetags)) ? CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Length - 1) : "";
            }
            var result = new YakkrData().InsertQuestionaireForm(qsform,CaseNote,Attachments);
            //return View();
            if (qsform.AppointmentMaked == 0 && anotherref) {

                string ID = EncryptDecrypt.Encrypt64(CaseNote.ClientId);
                return new RedirectResult("~/Roster/ReferralService?id="+ID+"&ClientName="+CaseNote.ClientName+"");
            }
            else
            {
                return new RedirectResult("~/Yakkr/YakkrList?YakkrCode=450");
            }
        }

        [CustAuthFilter()]
        public ActionResult Organizationalissue(int id, string clientid, string center, string household,string clientname) {

            var result = new YakkrData().GetQuestionaireByYakkrId(id, 3);

            string Name = "";
            long Client = Convert.ToInt32(EncryptDecrypt.Decrypt64(clientid));
            int CenterId = Convert.ToInt32(EncryptDecrypt.Decrypt64(center));
            int HouseHoldId = Convert.ToInt32(EncryptDecrypt.Decrypt64(household));

            ViewBag.ClientName = clientname;
            ViewBag.ClientId = Client;
            RosterNew.Users Userlist = new RosterNew.Users();
            var Rd = new RosterData();
            var stf = StaffDetails.GetInstance();

            Rd.GetCaseNote(ref Name, ref Userlist, HouseHoldId, CenterId, Client.ToString(), stf.AgencyId.ToString(), stf.UserId.ToString());
            ViewBag.Userlist = Userlist.UserList;

            return View(result);
        }

        [HttpPost]
        [CustAuthFilter()]
        [ValidateInput(false)]
        public ActionResult Organizationalissue(int ProblemOn, int? CRColorCode, int QuestionaireID, int CommunityId, int Yakkrid,
           string MgNotes, RosterNew.CaseNote CaseNote, List<RosterNew.Attachment> Attachments, RosterNew.ClientUsers TeamIds)
        {

            if (ProblemOn == 1) {
               
                    StringBuilder _Ids = new StringBuilder();
                    if (TeamIds.IDS != null)
                    {
                        foreach (string str in TeamIds.IDS)
                        {
                            _Ids.Append(str + ",");
                        }
                        CaseNote.StaffIds = _Ids.ToString().Substring(0, _Ids.Length - 1);
                    }

                    CaseNote.CaseNotetags = (CaseNote != null && !string.IsNullOrEmpty(CaseNote.CaseNotetags)) ? CaseNote.CaseNotetags.Substring(0, CaseNote.CaseNotetags.Length - 1) : "";
                
            }

            var result = new YakkrData().SubmitFeedBack453(4, ProblemOn, CRColorCode, CommunityId, QuestionaireID, Yakkrid,MgNotes,CaseNote, Attachments);

            return new RedirectResult("~/Yakkr/YakkrList?YakkrCode=453");
        }


        public PartialViewResult ReferralIssueSummary(int yakkrid,string clientname)
        {

            var result = new YakkrData().GetQuestionaireByYakkrId(yakkrid,5);

            ViewBag.CName = clientname;

            return PartialView("~/Views/Partialviews/_ReferralIssueSummaryPartial.cshtml", result);
        }


        //public ActionResult SubmitManagerFeedBack(int ProblemOn, string CaseNotetags, string FeedBackTOClient, int? CRColorCode,int QuestionaireID, int CommunityId,int Yakkrid)
        //{
        //    var result = new YakkrData().SubmitFeedBack453(4, ProblemOn, FeedBackTOClient,CRColorCode, CaseNotetags, CommunityId, QuestionaireID, Yakkrid);

        //    return RedirectToAction("YakkrList", new { YakkrCode = 451 });
        //}

        #endregion  yakkr451


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
