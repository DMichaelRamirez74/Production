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
using System.Text;
using Fingerprints.CustomClasses;
using FingerprintsModel.Enums;

namespace Fingerprints.Controllers
{
    public class MyProfileController : Controller
    {

        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
           role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
           role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
           role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
           role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
           role=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor )
           roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacvher)
           roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
           roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
          roleid=7c2422ba-7bd4-4278-99af-b694dcab7367(executive)
           */
        //    [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,a31b1716-b042-46b7-acc0-95794e378b26,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,7c2422ba-7bd4-4278-99af-b694dcab7367,c352f959-cfd5-4902-a529-71de1f4824cc")]

        [CustAuthFilter()] //allows all users if the session is not expired//
        public ActionResult editProfile(string id = "0")
        {
            try
            { 

                ViewBag.UserID = id;

                return View(new MyProfileData().Getprofile(id,Session["AgencyId"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }

        }
      //  [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,a31b1716-b042-46b7-acc0-95794e378b26,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,7c2422ba-7bd4-4278-99af-b694dcab7367,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [HttpPost]
        [CustAuthFilter()]
        public ActionResult editProfile(string id, FingerprintsModel.MyProfile _profile)
        {
            try

            {


                if (_profile.StaffEducation != null && _profile.StaffEducation.Certificates != null && _profile.StaffEducation.Certificates.Count > 0)
                {
                    _profile.StaffEducation.Certificates.ForEach(x =>
                    {
                        x.AttachmentFileByte = !string.IsNullOrEmpty(x.AttachmentJson) ? Convert.FromBase64String(x.AttachmentJson) : x.AttachmentFileByte;
                        x.AttachmentFileName = !string.IsNullOrEmpty(x.AttachmentJson) ? "EducationalAttachment" : x.AttachmentFileName;
                        x.AttachmentFileExtension = !string.IsNullOrEmpty(x.AttachmentJson) ? ".png" : x.AttachmentFileExtension;
                    });
                }

                if (new MyProfileData().SaveProfile(id, _profile) == "1")
                    TempData["message"] = "Record saved successfully.";
                else
                    TempData["message"] = "Error occurred. Please try again.";

                ViewBag.UserID = id;

               

                return View(_profile);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }


        [HttpPost]
        [CustAuthFilter()]
        public JsonResult CheckSignatureCode(string signatureCode,string id)
        {

            string result = "0";

            result = new MyProfileData().CheckSignatureCodeData(signatureCode, id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,a31b1716-b042-46b7-acc0-95794e378b26,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,7c2422ba-7bd4-4278-99af-b694dcab7367,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult Role()
        {
            try
            {
                return View(new MyProfileData().Getallroles(Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }

        }
        //[CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,a31b1716-b042-46b7-acc0-95794e378b26,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,7c2422ba-7bd4-4278-99af-b694dcab7367,c352f959-cfd5-4902-a529-71de1f4824cc")]
        [CustAuthFilter()]
        public ActionResult ChangeRole(string id,string subId="0")
        {
            string newLocation = string.Empty;
            try
            {
                TempData["SubId"] = subId;

                if(!string.IsNullOrEmpty(id))
                {
                    Session["Roleid"] = id;
                    Session["SubstituteID"] = !string.IsNullOrEmpty(subId.Trim()) && subId!="0" ?EncryptDecrypt.Decrypt64(subId):"0";

                   List<Tuple<string, string, int,bool>> AccessList = new List<Tuple<string, string, int,bool>>();
                    bool isAcceptanceProcess = false;

                        AccessList = new LoginData().GetAccessPageByUserId(ref isAcceptanceProcess, new Guid(Session["UserId"].ToString()), new Guid(Session["AgencyID"].ToString()), new Guid(Session["RoleId"].ToString()));

                    Session["AccessList"] = AccessList;
                    //Genesis Earth Administrator alias Agency Admin- Menu Enable is false.
                    if (Session["Roleid"].ToString().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5") && Session["MenuEnable"] != null && Convert.ToBoolean(Session["MenuEnable"]))
                        newLocation = "~/Home/AgencyAdminDashboard";

                    //Genesis Earth Administrator alias Agency Admin- Menu Enable is true.
                    else if (Session["Roleid"].ToString().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5") && Session["MenuEnable"] != null && !Convert.ToBoolean(Session["MenuEnable"]))
                        newLocation = "~/Agency/AgencyProfile/" + Session["AgencyID"];

                    //HR manager Dashboard
                    else if (Session["Roleid"].ToString().Contains("2d9822cd-85a3-4269-9609-9aabb914d792"))
                        newLocation = "~/Home/AgencyHRDashboard";

                    //Family Service Worker Dashboard
                    else if (Session["Roleid"].ToString().Contains("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d"))
                        newLocation = "~/Home/AgencystaffDashboard";

                    //Health/ Nurse Dashboard
                    else if (Session["Roleid"].ToString() == "a31b1716-b042-46b7-acc0-95794e378b26")
                        newLocation = "~/Home/ApplicationApprovalDashboard";

                    //Home Visitor Dashboard
                    else if (Session["Roleid"].ToString() == "e4c80fc2-8b64-447a-99b4-95d1510b01e9")
                        newLocation = "~/Home/AgencystaffDashboard";

                    //Teacher Dashboard
                    else if (Session["Roleid"].ToString() == "82b862e6-1a0f-46d2-aad4-34f89f72369a")
                        newLocation = "~/Teacher/Roster";

                    //Social Service Manager Dashboard
                    else if (Session["Roleid"].ToString().Contains("c352f959-cfd5-4902-a529-71de1f4824cc"))
                        newLocation = "~/Home/AgencystaffDashboard";

                    // Health Manager Dashboard
                    else if (Session["Roleid"].ToString() == "9ad1750e-2522-4717-a71b-5916a38730ed")
                        //newLocation = "~/Home/HealthManager";

                        newLocation = "~/HealthManager/Dashboard";

                    // Executive Manager Dashboard
                    else if (Session["Roleid"].ToString() == "7c2422ba-7bd4-4278-99af-b694dcab7367")
                        newLocation = "~/Home/Dashboard";

                    else if (Session["Roleid"].ToString().ToLowerInvariant() == FingerprintsModel.Role.RolesDictionary[(int)RoleEnum.CenterManager].ToLowerInvariant())
                        newLocation = "~/Home/Dashboard";

                    //Disabilities Manager Dashboard
                    else if (Session["Roleid"].ToString() == "047c02fe-b8f1-4a9b-b01f-539d6a238d80")
                        newLocation = "~/Home/AgencyDisabilityManagerDashboard";

                    // Mental Health Specialist Dashboard
                    else if (Session["Roleid"].ToString() == "699168ac-ad2d-48ac-b9de-9855d5dc9af8")
                        newLocation = "~/MentalHealth/MentalHealthDashboard";

                    //Disability Staff Dashboard
                    else if (Session["Roleid"].ToString() == "9c34ec8e-2359-4704-be89-d9f4b7706e82")
                        newLocation = "~/Home/DisabilityStaffDashboard";

                    // Parent Portal Dashboard
                    else if (Session["Roleid"].ToString().Contains("5ac211b2-7d4a-4e54-bd61-5c39d67a1106"))
                        newLocation = "~/Parent/ParentInfo";

                    // Billing Manager Dashboard
                    else if (Session["Roleid"].ToString().ToUpper().Contains("944D3851-75CC-41E9-B600-3FA904CF951F"))
                        newLocation = "~/Billing/FamilyOverride";

                    // ERSEA Manager Dashboard
                    else if (Session["Roleid"].ToString().ToUpper().Contains("B65759BA-4813-4906-9A69-E180156E42FC"))
                        newLocation = "~/ERSEA/ERSEADashboard";

                    // Transportation Manager Dashboard
                    else if (Session["Roleid"].ToString() == "6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")
                        newLocation = "~/Transportation/Dashboard";

                    // Facilities Manager Dashboard
                    else if (Session["Roleid"].ToString() == "825f6940-9973-42d2-b821-5b6c7c937bfe")
                        newLocation = "~/Home/AgencyFacilitiesManagerDashboard";

                    // Facilities Worker Dashboard
                    else if (Session["Roleid"].ToString() == "cb540cea-154c-482e-82a6-c1e0a189f611")
                        newLocation = "~/Home/FacilityWorkerDashboard";

                    // Education Manager Dashboard
                    else if (Session["Roleid"].ToString() == "4b77aab6-eed1-4ac3-b498-f3e80cf129c0")
                        newLocation = "~/EducationManager/EducationManagerDashboard";


                    else
                        newLocation = "~/Home/Agencyuserdashboard";
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                
            }
            return Redirect(newLocation);
        }
         [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,a31b1716-b042-46b7-acc0-95794e378b26,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,7c2422ba-7bd4-4278-99af-b694dcab7367,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult editEMP(int emp, FingerprintsModel.MyProfile _profile)
        {
            try
            {
                _profile.hidtab = "#addEmployment";
                if (!string.IsNullOrEmpty(Request.QueryString["Userid"]))
                    _profile.UserID = Request.QueryString["Userid"].ToString();
                if (new MyProfileData().deleteEmployment(_profile.UserID, emp, _profile) == "1")
                    TempData["message"] = "File deleted successfully.";
                else
                    TempData["message"] = "Error occured. Please try again.";

                return Redirect("~/MyProfile/editProfile/" + _profile.UserID + _profile.hidtab);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }


        }
         [CustAuthFilter("2d9822cd-85a3-4269-9609-9aabb914d792,a65bb7c2-e320-42a2-aed4-409a321c08a5,a31b1716-b042-46b7-acc0-95794e378b26,94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,7c2422ba-7bd4-4278-99af-b694dcab7367,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public ActionResult editEDU(int edu, FingerprintsModel.MyProfile _profile)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["Userid"]))
                    _profile.UserID = Request.QueryString["Userid"].ToString();


                if (new MyProfileData().deleteEducation(edu, _profile) == "1")
                    TempData["message"] = "Record deleted successfully.";
                else
                    TempData["message"] = "Error occurred. Please try again.";

                return Redirect("~/MyProfile/editProfile/" + _profile.UserID + _profile.hidtab);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }

        }
        



    }
}
