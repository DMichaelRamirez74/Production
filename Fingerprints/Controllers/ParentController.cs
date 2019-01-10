using Fingerprints.Filters;
using FingerprintsData;
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Fingerprints.Controllers
{
    public class ParentController : Controller
    {

        // GET: Parent
        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public ActionResult ParentInfo(string ClientId)
        {
            if (Session["ChildDetails"] == null)
            {
                List<BillingReview> list = new List<BillingReview>();
                string ProfilePic = string.Empty;
                string ParentName = string.Empty;
                DataTable dtParentAndChildDetails = new DataTable();
                if (Session["EmailID"] != null)
                    new ParentData().GetParentDetails(ref dtParentAndChildDetails, Session["EmailID"].ToString(), ref ProfilePic);
                if (dtParentAndChildDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtParentAndChildDetails.Rows)
                    {
                        if (!Convert.ToBoolean(dr["Isfamily"].ToString()))
                        {
                            list.Add(new BillingReview() { ClientId = dr["ClientID"].ToString(), ClientName = dr["Name"].ToString() });
                        }
                        else
                        {
                            if (!Convert.ToBoolean(dr["ParentId"].ToString()))
                            {
                                ParentName = dr["Name"].ToString();
                            }
                        }

                    }

                }

                Session["ChildDetails"] = list;
                Session["ParentName"] = ParentName;
                ProfilePic = ProfilePic == "" ? ("/Images/prof-image.png") : ("data:image/jpg;base64," + ProfilePic);
                Session["ProfilePic"] = ProfilePic;
            }

            ViewBag.ClientId = ClientId;
            return View();
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public ActionResult SchoolCalendar()
        {

            return View();
        }

        [HttpPost]
        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult GetParentDetails()
        {
            string JSONString = string.Empty;
            string ProfilePic = string.Empty;
            try
            {
                DataTable dtParentAndChildDetails = new DataTable();
                if (Session["EmailID"] != null)
                    new ParentData().GetParentDetails(ref dtParentAndChildDetails, Session["EmailID"].ToString(), ref ProfilePic);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtParentAndChildDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(new { JSONString, ProfilePic });
        }

        [HttpPost]
        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult GetChildDetails(string ClientId)
        {
            List<SelectListItem> absenceReasonList = null;
            string JSONString = string.Empty;
            string ProfilePic = "";
            int IsMarkAbsent = 0;
            bool IsLateArrival = false;
            try
            {

                DataSet dsChildDetails = new DataSet();
                absenceReasonList = new ParentData().GetChildDetails(ref dsChildDetails, ClientId, ref ProfilePic,Session["AgencyID"].ToString(),ref IsMarkAbsent,ref IsLateArrival);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dsChildDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
            return Json(new { JSONString, ProfilePic, absenceReasonList, IsMarkAbsent , IsLateArrival });
        }

        [HttpPost]
        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult GetScreenResults(string ClientId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtChildDetails = new DataTable();
                new ParentData().GetScreenResults(ref dtChildDetails, ClientId, Session["UserID"].ToString(), Session["AgencyID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtChildDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
            return Json(JSONString);
        }

        [HttpPost]
        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult ChangeParentAddressRequest(ParentAddressChange AddressDetails)
        {
            bool isResult = false;
            try
            {
                AddressDetails.UserId = Session["UserID"].ToString();
                YakkrRouting yakkr = new YakkrRouting();
                yakkr.AgencyID = new Guid(Session["AgencyID"].ToString());
                yakkr.UserID = new Guid(Session["UserID"].ToString());
                isResult = new ParentData().AddParentAddressChange(AddressDetails, yakkr, Session["EmailID"].ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [HttpPost]
        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult AddParentMessage(ParentStatus statuschange, string ToStaffId)
        {
            bool isResult = false;
            try
            {
                statuschange.UserId = Session["UserID"].ToString();
                //statuschange.HouseHoldId = Convert.ToInt64(Session["HouseHoldId"].ToString());
                isResult = new ParentData().AddParentMessage(statuschange, Session["AgencyID"].ToString(), ToStaffId);
                //if (isResult)
                //{
                //    isResult = new TeacherData().InsertYakkrRouting(
                //        new YakkrRouting
                //        {
                //            AgencyID = new Guid(Session["AgencyID"].ToString()),
                //            UserID = new Guid(Session["UserID"].ToString()),
                //            RouteCode = statuschange.RouteCode,
                //            ToSataffId = new Guid(ToStaffId),
                //            HouseHoldId = Convert.ToInt64(statuschange.HouseHoldId),
                //            Email = Session["EmailID"].ToString(),
                //            ClientId = statuschange.ClientId
                //        }, "");
                //}

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult EmployemntStatusChange(List<StatusChange> statuschange)
        {
            bool isResult = false;
            try
            {
                foreach (StatusChange status in statuschange)
                {
                    status.UserId = Session["UserID"].ToString();
                    isResult = new ParentData().EmploymentStatusChange(status, Session["AgencyID"].ToString());
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult MilitaryStatusChange(List<StatusChange> statuschange)
        {
            bool isResult = false;
            try
            {
                foreach (StatusChange status in statuschange)
                {
                    status.UserId = Session["UserID"].ToString();
                    isResult = new ParentData().MilitaryStatusChange(status, Session["AgencyID"].ToString());
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult HomelessStatusChange(List<StatusChange> statuschange)
        {
            bool isResult = false;
            try
            {
                foreach (StatusChange status in statuschange)
                {
                    status.UserId = Session["UserID"].ToString();
                    isResult = new ParentData().HomelessStatusChange(status, Session["AgencyID"].ToString(), Session["EmailID"].ToString());
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult MarkAbsentStatus(List<StatusChange> statuschange)
        {
            bool isResult = false;
            try
            {
               string str= RouteData.Values["ClientId"] + Request.Url.Query;
                foreach (StatusChange status in statuschange)
                {
                    status.UserId = Session["UserID"].ToString();
                    isResult = new ParentData().MarkAbsentStatus(status, Session["AgencyID"].ToString(), Session["EmailID"].ToString());
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult EducationStatusChange(List<StatusChange> statuschange)
        {
            bool isResult = false;
            try
            {
                foreach (StatusChange status in statuschange)
                {
                    status.UserId = Session["UserID"].ToString();
                    isResult = new ParentData().EducationStatusChange(status, Session["AgencyID"].ToString());
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult VolunteerRequest(List<Volunteer> volunteer)
        {
            bool isResult = false;
            try
            {
                foreach (Volunteer objVol in volunteer)
                {
                    objVol.UserId = Session["UserID"].ToString();
                    isResult = new ParentData().VolunteerRequest(objVol, Session["AgencyID"].ToString(), Session["EmailID"].ToString());
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public ActionResult BillingAttachments()
        {
            return View();
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public JsonResult ParentEngagement(List<ParentEngagement> statuschange)
        {
            bool isResult = false;
            try
            {
                foreach (ParentEngagement status in statuschange)
                {
                    status.UserId = Session["UserID"].ToString();

                    isResult = new ParentData().ParentEngagementRequest(status, Session["AgencyID"].ToString());
                    //if (isResult)
                    //{
                    //    isResult = new TeacherData().InsertYakkrRouting(
                    //        new YakkrRouting
                    //        {
                    //            AgencyID = new Guid(Session["AgencyID"].ToString()),
                    //            UserID = new Guid(Session["UserID"].ToString()),
                    //            RouteCode = status.RouteCode,
                    //            ToSataffId = new Guid("EBF675DF-9468-4CD4-92B0-D97B7D49888D"),
                    //            HouseHoldId = Convert.ToInt64(status.HouseHoldId),
                    //            Email = Session["EmailID"].ToString(),
                    //            ClientId = status.ClientId
                    //        }, ""
                    //        );
                    //}
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        [CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106")]
        public ActionResult Library()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UpdateAddressChange(string HouseHoldId, string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().UpdateAddressChange(HouseHoldId, Session["UserID"].ToString(), YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult UpdateStatusChange(string HouseHoldId, string RouteCode, string ClientId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().UpdateStatusChange(HouseHoldId, Session["UserID"].ToString(), RouteCode, ClientId, null));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult ApproveEmploymentRequest(String YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().ApproveEmploymentRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult ApproveMilitaryRequest(String YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().ApproveMilitaryRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult ApproveHomelessRequest(String YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().ApproveHomelessRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult ApproveAbsentRequest(String YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().ApproveAbsentRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult ApproveParentEngagementRequest(String YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().ApproveParentEngagemnetRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult ApproveVolunteerRequest(String YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().ApproveVolunteerRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult ApproveEducationRequest(String YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().ApproveEducationRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult ClassroomOpenrequestApproval(string RouteCode, string Notes, string YakkrID)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().ClassroomOpenrequestApproval(Session["UserID"].ToString(), RouteCode, Notes, YakkrID));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetAddressByHouseHoldId(string YakkrID, string HouseHoldId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().GetAddressByHouseHold(YakkrID, HouseHoldId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetStatus(string HouseHoldId, string RouteCode, string ClientId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetStatus(HouseHoldId, RouteCode, Session["EmailID"].ToString(), ClientId, UserId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetEmploymentStatus(string HouseHoldId, string ClientId, string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetEmploymentStatus(HouseHoldId, ClientId, YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetMilitaryStatus(string HouseHoldId, string ClientId, string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetMilitaryStatus(HouseHoldId, ClientId, YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetHomelessStatus(string HouseHoldId, string ClientId, string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetHomelessStatus(HouseHoldId, ClientId, YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetEducationStatus(string HouseHoldId, string ClientId, string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetEducationStatus(HouseHoldId, ClientId, YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetAbsentRequest(string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetAbsentRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [HttpPost]
        public JsonResult GetParentEngagementRequest(string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetParentEngagementRequest(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetParentMessages(string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetParentMessages(YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetVolunteerRequest(string HouseHoldId, string ClientId, string YakkrId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetVolunteerRequest(HouseHoldId, ClientId, YakkrId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [HttpPost]
        public JsonResult GetAlert(string HouseHoldId, string RouteCode, string Date)
        {
            YakkrData obj = new YakkrData();
            try
            {
                string UserId = Session["UserID"].ToString();
                return Json(new ParentData().GetStatus(HouseHoldId, RouteCode, Date, "", UserId));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }


        public JsonResult GetAlertForCenterClose(string YakkrId, string RouteCode, string Date)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtClassRoomsDetails = new DataTable();
                string UserId = Session["UserID"].ToString();
                new ParentData().GetClassRoomReqStatus(ref dtClassRoomsDetails, UserId, Session["AgencyId"].ToString(), YakkrId, RouteCode, Date);
            
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtClassRoomsDetails);

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
            return Json(JSONString);
        }

        [HttpPost]
        public JsonResult GetParentStatus(string HouseHoldId)
        {
            YakkrData obj = new YakkrData();
            try
            {
                return Json(new ParentData().GetParentStatus(Session["HouseHoldId"].ToString(), Session["EmailID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        public JsonResult StatusChange(List<StatusChange> statuschange)
        {
            bool isResult = false;
            try
            {
                foreach (StatusChange status in statuschange)
                {
                    status.UserId = Session["UserID"].ToString();
                    isResult = new ParentData().StatusChange(status);
                    YakkrRouting yakkr = new YakkrRouting();
                    if (isResult)
                    {
                        isResult = new TeacherData().InsertYakkrRouting(
                            new YakkrRouting
                            {
                                AgencyID = new Guid(Session["AgencyID"].ToString()),
                                UserID = new Guid(Session["UserID"].ToString()),
                                RouteCode = status.RouteCode,
                                ToSataffId = new Guid("EBF675DF-9468-4CD4-92B0-D97B7D49888D"),
                                HouseHoldId = Convert.ToInt64(status.HouseHoldId),
                                Email = Session["EmailID"].ToString(),
                                ClientId = status.ClientId
                            }, ""
                            );
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        public JsonResult ParentStatusChange(List<ParentStatus> statuschange)
        {
            bool isResult = false;
            try
            {
                foreach (ParentStatus status in statuschange)
                {
                    status.UserId = Session["UserID"].ToString();
                    isResult = new ParentData().ParentStatusChange(status);
                    if (isResult)
                    {
                        isResult = new TeacherData().InsertYakkrRouting(
                            new YakkrRouting
                            {
                                AgencyID = new Guid(Session["AgencyID"].ToString()),
                                UserID = new Guid(Session["UserID"].ToString()),
                                RouteCode = status.RouteCode,
                                ToSataffId = new Guid("EBF675DF-9468-4CD4-92B0-D97B7D49888D"),
                                HouseHoldId = Convert.ToInt64(status.HouseHoldId),
                                Email = Session["EmailID"].ToString(),
                                ClientId = status.ClientId
                            }, ""
                            );
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult);
        }

        public JsonResult GetDisabilityChildren(string YakkrID, string HouseHoldId, string ClientId, int mode,string IsAccepted="0")
        {
            DisabilityChildren disbilityChildren = new DisabilityChildren();
            try
            {
                disbilityChildren = new ParentData().GetDisabilityChildrenData(YakkrID, HouseHoldId, ClientId, Session["AgencyId"].ToString(), Session["UserID"].ToString(), mode,IsAccepted);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(disbilityChildren, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UploadDisabilityFile(string monitor)
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
                            var _comPath = Server.MapPath("/Content/DisabilityAttachment/") + _imgname;
                            _imgpath[i] = "/Content/DisabilityAttachment/" + _imgname;
                            i++;
                            var path = _comPath;
                            pic.SaveAs(path);
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


        public ActionResult ClientGrowthChart(string client)
        {

            ViewBag.eClientId = client;

            try
            {
                long cid = 0;
                Clientprofile cp = new Clientprofile();
                if (!string.IsNullOrEmpty(client))
                {
                    cid = Convert.ToInt64(EncryptDecrypt.Decrypt(client));
                    cp = new RosterData().GetClientDetails(cid);

                }
                ViewBag.ClientDetail = cp;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View();
        }

    }
}