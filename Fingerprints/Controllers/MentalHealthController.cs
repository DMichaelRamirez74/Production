﻿using FingerprintsData;
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fingerprints.Controllers
{
    public class MentalHealthController : Controller
    {
        //
        // GET: /MentalHealth/
        MentalHealthData mHealth = null;
        public ActionResult MentalHealthDashboard()
        {
            MentalHealthDashboard dash = new MentalHealthDashboard();
            mHealth = new MentalHealthData();
            dash.ClientList = new List<MentalHealthClientList>();
            try
            {
                ViewBag.Centerlist = mHealth.GetMentalHealthDashboard( Session["AgencyID"].ToString(), Session["UserID"].ToString());
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(dash);
        }
        public ActionResult LoadMentalHealthDashboardList(string centerId,string mode)
        {
            MentalHealthDashboard dash = new MentalHealthDashboard();
         
            dash.ClientList = new List<MentalHealthClientList>();
            try
            {
                mHealth = new MentalHealthData();
                dash.Mode = mode;
                dash.ClientList = mHealth.LoadMentalHealthDashboardList(centerId, mode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
          
            return PartialView("~/Views/Partialviews/MentalHealthClientList.cshtml", dash);
        }

        public ActionResult GetMentalHealthClientDetails(string clientId,string centerId,string householdId)
        {
            MentalHealthDashboard dash = new MentalHealthDashboard();
            dash.ClientList = new List<MentalHealthClientList>();
            try
            {
                mHealth = new MentalHealthData();
                
                // dash.ClientList = mHealth.LoadMentalHealthDashboardList(centerId, mode);
            }
            catch (Exception ex)
            {
              clsError.WriteException(ex);
            }
          
            return PartialView("~/Views/Partialviews/MentalHealthClientDetails.cshtml", dash);
        }

        [HttpPost]
        [ValidateInput(false)]
        
        public ActionResult SaveMentalHealthClient(MentalHealthCaseNote MentalHealthCaseNote)
        {
            bool res = false;
            try
            {
                mHealth = new MentalHealthData();
                MentalHealthDashboard dash = new MentalHealthDashboard();
                dash.ClientList = new List<MentalHealthClientList>();

                // for casenote

                string name = "";
                string casenoteid = "";

                List<RosterNew.Attachment> Attachments = new List<RosterNew.Attachment>();
                var ate = Request.Files;
                var ate2 = ate.AllKeys;
                for (int i = 0; i < ate2.Length; i++)
                {
                    RosterNew.Attachment aatt = new RosterNew.Attachment();
                    aatt.file = ate[i];
                    if (aatt.file.ContentLength > 0)
                        Attachments.Add(aatt);
                }
                RosterNew.CaseNote _caseNote = new RosterNew.CaseNote();
                List<CaseNote> caseNote = new List<CaseNote>();
                RosterNew.Users _users = new RosterNew.Users();
                _caseNote.CenterId = EncryptDecrypt.Decrypt64(MentalHealthCaseNote.CenterId);
               // _caseNote.Classroomid = MentalHealthCaseNote.CaseClassroomId.ToString();
                _caseNote.ClientId = EncryptDecrypt.Decrypt64(MentalHealthCaseNote.ClientId.ToString());
                _caseNote.CaseNotetags = MentalHealthCaseNote.Tags.Trim(',');
                _caseNote.CaseNotetitle = MentalHealthCaseNote.Title;
                _caseNote.CaseNoteDate = MentalHealthCaseNote.Date;
                _caseNote.Note = MentalHealthCaseNote.MHcasenote;
                _caseNote.ClientIds = string.Join(",", MentalHealthCaseNote.ClientIds.ToArray());
              //  _caseNote.ProgramId = EncryptDecrypt.Decrypt64(MentalHealthCaseNote.CaseProgramId);
                _caseNote.CaseNoteAttachmentList = Attachments;

                casenoteid = new RosterData().SaveCaseNotes(ref name, ref caseNote, ref _users, _caseNote, Attachments, Session["AgencyId"].ToString(),Session["RoleID"].ToString(), Session["UserId"].ToString(), 2);


                 res = mHealth.SaveMentalHealthClient(MentalHealthCaseNote, name);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return RedirectToAction("MentalHealthDashboard");

        }
        public JsonResult GetMentalHealthDetailsByClientId(string ClientId)
        {
            Role result = new Role();
            try
            {
                mHealth = new MentalHealthData();
                result = mHealth.GetMentalHealthDetailsByClientId(ClientId);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

    }
}
