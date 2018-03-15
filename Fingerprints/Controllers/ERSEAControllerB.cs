using Fingerprints.CustomClasses;
using FingerprintsData;
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fingerprints.Filters;
using System.Web.Script.Serialization;

namespace Fingerprints.Controllers
{
    [CustAuthFilter("b65759ba-4813-4906-9a69-e180156e42fc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
    public class ERSEAController : Controller
    {
        //
        // GET: /ERSEA/

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

        public ActionResult ERSEADashboard()
        {
            ERSEADashBoard obj = new ERSEADashBoard();
            try
            {
                //get the Json filepath  
                //string file = Server.MapPath("~/Content/JSON/us_census_zipcode.json");
                //deserialize JSON from file  
                //var Json = System.IO.File.ReadAllText(file);

                List<ApplicationEnrollment> lstApplication = new List<ApplicationEnrollment>();
                List<ADA> lstADA = new List<ADA>();
                List<CityName> lstCityName = new List<CityName>();
                new ERSEAData().GetApplicationEnrollmentBasedonZip(ref lstApplication, Session["AgencyID"].ToString());
                new ERSEAData().GetADABasedonCenter(ref lstADA, Session["AgencyID"].ToString());
                new ERSEAData().GetCityName(ref lstCityName);
                if (lstApplication.Count() > 0)
                {
                    foreach (ApplicationEnrollment objEnroll in lstApplication)
                    {
                        Object city = lstCityName.Where(a => a.Zipcode.Contains(objEnroll.ZipCode)).Select(a => a.City).FirstOrDefault();
                        Object primarycity = lstCityName.Where(a => a.Zipcode.Contains(objEnroll.ZipCode)).Select(a => a.PrimaryCity).FirstOrDefault();
                        if (city != null || primarycity != null)
                        {
                            if (primarycity != null)
                                objEnroll.CityName = primarycity.ToString();
                            else
                                objEnroll.CityName = city.ToString();
                        }

                    }
                }
                obj.lstApplication = lstApplication;
                obj.listADA = lstADA;
                ADA totalADA = new ADA();
                int count = lstADA.Count() * 100;

                totalADA.Jan = Math.Round((lstADA.Sum(a => a.Jan) / count) * 100, 0);
                totalADA.Feb = Math.Round((lstADA.Sum(a => a.Feb) / count) * 100, 0);
                totalADA.Mar = Math.Round((lstADA.Sum(a => a.Mar) / count) * 100, 0);
                totalADA.Apr = Math.Round((lstADA.Sum(a => a.Apr) / count) * 100, 0);
                totalADA.May = Math.Round((lstADA.Sum(a => a.May) / count) * 100, 0);
                totalADA.Jun = Math.Round((lstADA.Sum(a => a.Jun) / count) * 100, 0);
                totalADA.Jul = Math.Round((lstADA.Sum(a => a.Jul) / count) * 100, 0);
                totalADA.Aug = Math.Round((lstADA.Sum(a => a.Aug) / count) * 100, 0);
                totalADA.Sep = Math.Round((lstADA.Sum(a => a.Sep) / count) * 100, 0);
                totalADA.Oct = Math.Round((lstADA.Sum(a => a.Oct) / count) * 100, 0);
                totalADA.Nov = Math.Round((lstADA.Sum(a => a.Nov) / count) * 100, 0);
                totalADA.Dec = Math.Round((lstADA.Sum(a => a.Dec) / count) * 100, 0);
                ViewBag.TotalPercentage = totalADA;
                decimal total = (totalADA.Jan + totalADA.Feb + totalADA.May + totalADA.Apr + totalADA.May + totalADA.Jun + totalADA.Jul + totalADA.Aug + totalADA.Sep + totalADA.Oct + totalADA.Nov + totalADA.Dec);
                //decimal total = (lstADA.Sum(a => a.Jan) + lstADA.Sum(a => a.Feb) + lstADA.Sum(a => a.Mar) + lstADA.Sum(a => a.Apr) + lstADA.Sum(a => a.May) + lstADA.Sum(a => a.Jun) + lstADA.Sum(a => a.Jul) + lstADA.Sum(a => a.Aug) + lstADA.Sum(a => a.Sep) + lstADA.Sum(a => a.Oct) + lstADA.Sum(a => a.Nov) + lstADA.Sum(a => a.Dec));
                ViewBag.OverAllPercentage = Math.Round((total / (300)) * 100, 0);
                ViewBag.OverAllApplication = lstApplication.Sum(a => a.Application);
                ViewBag.OverAllEnroll = lstApplication.Sum(a => a.Enrollment);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(obj);
        }

        //[HttpPost]
        //public ActionResult GetApplicationEnrollmentBasedonZip()
        //{
        //    string JSONString = string.Empty;
        //    try
        //    {
        //        DataSet ds= new DataSet();
        //        new ERSEAData().GetApplicationEnrollmentBasedonZip(ref ds, Session["AgencyID"].ToString());
        //        JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(ds);
        //        var JSONRESULT= Json(JSONString, JsonRequestBehavior.AllowGet);
        //        JSONRESULT.MaxJsonLength = int.MaxValue;
        //        return JSONRESULT
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //    }

        //    return Json(JSONString);
        //}
        //GET Selection SelectionCriteriaUpdate Page.

        public ActionResult SelectionCriteriaUpdate()
        {
            SelectPointsData progData = new SelectPointsData();
            SelectPoints _prog = null;
            _prog = progData.GetData_AllDropdown(Session["AgencyID"].ToString());
            ViewBag.RefList = _prog.refList;
            TempData["RefList"] = ViewBag.RefList;
            return View(_prog);
        }

        [HttpPost]
        public ActionResult SelectionCriteriaUpdate(SelectPoints info, string Command, FormCollection collection, List<FingerprintsModel.SelectPoints.CustomQuestion> CustomQues)
        {
            try
            {

                TempData.Keep();
                string message = "";
                SelectPointsData obj = new SelectPointsData();
                info.AgencyID = (Session["AgencyID"].ToString());
                if (info.SPID == 0)
                {
                    if (Command == "SaveLockSelectDetail")
                    {
                        info.IsLocked = true;
                        info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                        message = obj.AddEditSelectPoint(info, 0, Guid.Parse(Session["UserID"].ToString()), CustomQues);

                    }
                    else
                    {
                        info.IsLocked = false;
                        info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                        message = obj.AddEditSelectPoint(info, 0, Guid.Parse(Session["UserID"].ToString()), CustomQues);
                    }
                    // info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                    //  message = obj.AddEditSelectPoint(info, 0, Guid.Parse(Session["UserID"].ToString()), CustomQues);

                }
                else
                {
                    if (info.IsLocked == false)
                    {
                        if (Command == "SaveLockSelectDetail")
                        {
                            info.IsLocked = true;
                            info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                            message = obj.AddEditSelectPoint(info, 1, Guid.Parse(Session["UserID"].ToString()), CustomQues);
                        }
                        else
                        {
                            info.IsLocked = false;
                            info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                            message = obj.AddEditSelectPoint(info, 1, Guid.Parse(Session["UserID"].ToString()), CustomQues);

                        }
                    }
                    else
                    {
                        ViewBag.message = "Record already locked.";
                        ViewBag.result = "Sucess";
                    }
                    //info.ReferenceProg = (collection["DdlProgRefList"].ToString() == "-1") ? null : collection["DdlProgRefList"];
                    //message = obj.AddEditSelectPoint(info, 1, Guid.Parse(Session["UserID"].ToString()), CustomQues);

                }
                if (message == "1")
                {
                    // TempData["message"] = "Record added successfully.";
                    ViewBag.message = "Record added successfully.";
                    ViewBag.result = "Sucess";
                }
                else if (message == "2")
                {
                    ViewBag.message = "Record updated successfully.";
                    ViewBag.result = "Sucess";
                }
                else if (message == "3")
                {
                    ViewBag.message = "Record already locked.";
                    ViewBag.result = "Sucess";
                }
                ViewBag.RefList = TempData["RefList"];
                TempData.Keep();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            ViewBag.RefList = TempData["RefList"];
            TempData.Keep();
            return View(info);


        }
        //GET  CommunityAssessment page.

        [JsonMaxLength]
        public JsonResult listQuesDetails(string ProgramId = "0")//string CQID = "0",
        {
            try
            {

                SelectPointsData obj = new SelectPointsData();
                var listQues = obj.QuesDetails(ProgramId, Session["AgencyID"].ToString()).ToList();
                return Json(new { listQues });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }

        public JsonResult GetSelectPointlist(string ProgramId = "0")//string RestrictedId = "0",
        {
            SelectPointsData obj = new SelectPointsData();
            try
            {
                return Json(obj.EditSelectPointInfo(ProgramId, Session["AgencyID"].ToString()));//RestrictedId,
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }


        public JsonResult GetRefProglist(string ProgramId = "0")//string RestrictedId = "0",
        {
            SelectPointsData obj = new SelectPointsData();
            try
            {
                return Json(obj.GetRefProglistInfo(ProgramId, Session["AgencyID"].ToString()));//RestrictedId,
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        public JsonResult Deletecustomques(string CQID = "0")
        {
            SelectPointsData obj = new SelectPointsData();
            try
            {
                return Json(obj.DeleteQues(CQID, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        public ActionResult CommunityAssessment()
        {
            return View();
        }

        //GET Center Analysis page;
        public ActionResult CenterAnalysis()
        {
            return View();
        }

        //GET Workshop Analysis By Center page.
        [CustAuthFilter("b65759ba-4813-4906-9a69-e180156e42fc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult WorkshopAnalysis()
        {
            return View();
        }

        [JsonMaxLength]
        public JsonResult GetCenterAnalysisList(string programId)
        {
            List<SelectListItem> programList = new List<SelectListItem>();
            List<CenterAnalysis> centerAnalysisList = new List<FingerprintsModel.CenterAnalysis>();
            CenterAnalysisPercentage calcAnalysis = new CenterAnalysisPercentage();
            try
            {
                Guid userId = new Guid(Session["UserID"].ToString());
                Guid AgencyId = new Guid(Session["AgencyId"].ToString());
                long dec_ProgramId = programId == "0" ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));
                centerAnalysisList = new ERSEAData().GetCenterAnalysisList(out calcAnalysis, out programList, userId, AgencyId, dec_ProgramId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { centerAnalysisList, calcAnalysis, programList }, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetWorkShopAnalysis(long centerId)
        {
            List<WorkShopAnalysis> workshopList = new List<WorkShopAnalysis>();
            List<SelectListItem> centerList = new List<SelectListItem>();
            bool isLink = false;
            try
            {
                Guid AgencyId = new Guid(Session["AgencyId"].ToString());
                Guid userId = new Guid(Session["UserID"].ToString());
                Guid roleId = new Guid(Session["RoleId"].ToString());
                workshopList = new ERSEAData().GetWorkshopAnalysisList(out centerList, AgencyId, roleId, userId, centerId);
                isLink = (Session["RoleId"].ToString() == "b4d86d72-0b86-41b2-adc4-5ccce7e9775b");
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { workshopList, centerList,isLink  }, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetFosterChild(string centerId, string programID)
        {
            List<FosterChild> fosterchildList = new List<FosterChild>();
            try
            {
                long centerID = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                long programId = (programID == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programID));
                Guid userId = new Guid(Session["UserID"].ToString());
                Guid AgencyId = new Guid(Session["AgencyId"].ToString());
                fosterchildList = new ERSEAData().GetFosterChildData(centerID, programId, userId, AgencyId);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(fosterchildList, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetGraduatingChildByProgram(string programTypeID)
        {
            List<SelectListItem> gradChildList = new List<SelectListItem>();
            long totalGradcount = 0;
            try
            {
                long dec_ProgramId = programTypeID == "0" ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programTypeID));
                Guid agencyId = new Guid(Session["AgencyId"].ToString());

                gradChildList = new ERSEAData().GetGradChildDataByProgram(out totalGradcount, dec_ProgramId, agencyId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { gradChildList, totalGradcount }, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetChartDataByZipCode(string zipcode)
        {
            CommunityAssessment communityassment = new FingerprintsModel.CommunityAssessment();
            try
            {
                communityassment = new ERSEAData().GetChartDetailsDataByZip(zipcode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(communityassment, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public ActionResult GetZipcodesbystate(string searchText)
        {

            List<ZipcodebyState> Zipcodestatelist = new List<ZipcodebyState>();
            ERSEAData zipcodeList = new ERSEAData();
            try
            {

                Zipcodestatelist = zipcodeList.zipcodebyState(searchText);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { Zipcodestatelist }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetZipcodesbystateandCity(string searchcity, string searchstate)
        {

            List<ZipByStateandCity> ZipcodestateCitylist = new List<ZipByStateandCity>();
            City_State cityState = new City_State();
            try
            {

                ZipcodestateCitylist = new ERSEAData().zipcodebycityandState(out cityState, searchcity, searchstate);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { ZipcodestateCitylist, cityState }, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public ActionResult GETChildrenByCenter(string CenterID, string programId)
        {
            ChildrenInfoClass childrenInfo = new ChildrenInfoClass();
            try
            {
                var centerId = EncryptDecrypt.Decrypt64(CenterID);
                var AgencyId = Session["AgencyID"].ToString();
                ERSEAData centerlist = new ERSEAData();
                long programID = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString()));
                childrenInfo = centerlist.GetChildrenByCenter(centerId, AgencyId, programID);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childrenInfo, JsonRequestBehavior.AllowGet);
          
        }

        [JsonMaxLength]
        public ActionResult GetChildrenByClassRoom(string CenterId, string classroomId, string programId)
        {
            List<ChildrenInfo> children_List = new List<ChildrenInfo>();
            try
            {

                long dec_CenterId = Convert.ToInt64(EncryptDecrypt.Decrypt64(CenterId));
                long dec_ClassRoomId = (classroomId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(classroomId));
                long dec_ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));
                Guid AgencyId = new Guid(Session["AgencyID"].ToString());

                children_List = new ERSEAData().GetChildrenByClassRoom(dec_CenterId, AgencyId, dec_ClassRoomId, dec_ProgramId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
             return Json(children_List, JsonRequestBehavior.AllowGet);
           
          
        }

        [JsonMaxLength]
        public JsonResult GetEnrolledChildren(string centerId, string programId)
        {
            List<ChildrenInfo> enrolledList = new List<ChildrenInfo>();
          
            try
            {
                long centerID = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                long programID = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));
                enrolledList = new ERSEAData().GetEnrolledChildrenData(centerID, programID, new Guid(Session["AgencyId"].ToString()));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
          
            return Json(enrolledList, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetReturningChildren(string centerId, string programId)
        {
            List<ChildrenInfo> returningList = new List<ChildrenInfo>();
            try
            {
                long centerID = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                long programID = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));
                returningList = new ERSEAData().GetReturningChildren(centerID, programID, new Guid(Session["AgencyId"].ToString()));

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(returningList, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetGraduatingChildren(string centerId, string programId)
        {
            List<ChildrenInfo> graduatingList = new List<ChildrenInfo>();
            try
            {
                long centerID = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                long programID = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));
                graduatingList = new ERSEAData().GetGraduatingChildrenData(centerID, programID, new Guid(Session["AgencyId"].ToString()));

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(graduatingList, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetWaitingChildren(string centerId, string programId)
        {
            List<WaitingChildren> waitingList = new List<WaitingChildren>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = string.Empty;
            try
            {
                long centerID = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                long programID = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));
                waitingList = new ERSEAData().GetWatitingChildrenData(centerID, programID, new Guid(Session["AgencyId"].ToString()));
                // jsonData = js.Serialize(waitingList);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(waitingList, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetHomelessChildren(string centerId, string programId)
        {
            List<HomelessChildren> homeLessChilren = new List<HomelessChildren>();
            try
            {

                long programID = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));
                long centerID = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));

                ERSEAData homelessList = new ERSEAData();
                homeLessChilren = new ERSEAData().GetHomelessChildrenData(centerID, programID, new Guid(Session["AgencyId"].ToString()));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(homeLessChilren, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetOverIncomeChildren(string centerId, string programId)
        {
            List<ChildrenInfo> overIncomeChildList = new List<ChildrenInfo>();
            List<SelectListItem> ParentList = new List<SelectListItem>();
            try
            {

                long programID = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));
                long centerID = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));

                overIncomeChildList = new ERSEAData().GetOverIncomeChildrenData(out ParentList, centerID, programID, new Guid(Session["AgencyId"].ToString()));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { overIncomeChildList, ParentList }, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetLeadschildren(string centerID)
        {
            List<LeadsChildren> leadsChildrenlist = new List<LeadsChildren>();
            try
            {

                long CenterID = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerID));
                leadsChildrenlist = new ERSEAData().GetLeadsChildrenData(CenterID, new Guid(Session["AgencyId"].ToString()));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(leadsChildrenlist, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLengthAttribute]
        public JsonResult GetChildrenImage(string enc_clientId)
        {
            SelectListItem item = new SelectListItem();
            try
            {
                long clientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_clientId));

                item = new ERSEAData().GetChildrenImageData(clientId);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(item, JsonRequestBehavior.AllowGet);
        }     
        public JsonResult GetgeometryByZip(string Zipcode)
        {
            string geometry = string.Empty;
            try
            {
                geometry = new ERSEAData().GetgeometryByZip(Zipcode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(geometry, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetgeometryByState(string State)
        {
            string geometry = string.Empty;
            try
            {
                geometry = new ERSEAData().GetgeometryByState(State);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(geometry, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetgeometryByCounty(string County)
        {
            string geometry = string.Empty;
            try
            {
                geometry = new ERSEAData().GetgeometryByCounty(County);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(geometry, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertGeoJsonZipcode(string zipcode, string geometry)
        {
            bool Result = false;
            try
            {
                  Result = new ERSEAData().SaveGeoJsonZipcode(zipcode, geometry);                
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }
        
    }
}
