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
                if (new MyProfileData().SaveProfile(id, _profile) == "1")
                    TempData["message"] = "Record saved successfully.";
                else
                    TempData["message"] = "Error occured. Please try again.";
                return View(_profile);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
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
        public ActionResult ChangeRole(string id)
        {
            string newLocation = string.Empty;
            try
            {
                if(!string.IsNullOrEmpty(id))
                {
                    Session["Roleid"] = id;

                   List<Tuple<string, string>> AccessList = new List<Tuple<string, string>>();
                        AccessList = new LoginData().GetAccessPageByUserId(new Guid(Session["UserId"].ToString()), new Guid(Session["AgencyID"].ToString()), new Guid(Session["RoleId"].ToString()));


                    if (Session["Roleid"].ToString().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5") && Session["MenuEnable"] != null && Convert.ToBoolean(Session["MenuEnable"]))
                        newLocation = "~/Home/AgencyAdminDashboard";
                    else if (Session["Roleid"].ToString().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5") && Session["MenuEnable"] != null && !Convert.ToBoolean(Session["MenuEnable"]))
                        newLocation = "~/Agency/AgencyProfile/" + Session["AgencyID"];
                    else if (Session["Roleid"].ToString().Contains("2d9822cd-85a3-4269-9609-9aabb914d792"))
                        newLocation = "~/Home/AgencyHRDashboard";
                    else if (Session["Roleid"].ToString().Contains("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d"))
                        newLocation = "~/Home/AgencystaffDashboard";
                    else if (Session["Roleid"].ToString() == "a31b1716-b042-46b7-acc0-95794e378b26")
                        newLocation = "~/Home/ApplicationApprovalDashboard";
                    else if (Session["Roleid"].ToString() == "e4c80fc2-8b64-447a-99b4-95d1510b01e9")
                        newLocation = "~/Home/AgencystaffDashboard";
                    else if (Session["Roleid"].ToString() == "82b862e6-1a0f-46d2-aad4-34f89f72369a")
                        newLocation = "~/Home/TeacherDashBoard";
                    else if (Session["Roleid"].ToString().Contains("c352f959-cfd5-4902-a529-71de1f4824cc"))
                        newLocation = "~/Home/AgencystaffDashboard";
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
                    TempData["message"] = "Error occured. Please try again.";

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
