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
using FingerprintsModel.Enums;
using Fingerprints.Common;
using Fingerprints.Utilities;

namespace Fingerprints.Controllers
{
    // [CustAuthFilter("b65759ba-4813-4906-9a69-e180156e42fc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
    [CustAuthFilter()]
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
   roleid=b65759ba-4813-4906-9a69-e180156e42fc (ERSEA Manager)
   */
    
        [CustAuthFilter(RoleEnum.ERSEAManager)]
        public ActionResult ERSEADashboard()
        {
            var result = this.GetERSEADashboard();
            return View(result);
        }

        [CustAuthFilter()]
        public ActionResult ADAByCenter()
        {

            var result = this.GetERSEADashboard();
            return View(result);
        }

        [CustAuthFilter()]
        public ActionResult DownloadADAByCenter()
        {

            var result = this.GetERSEADashboard();
            var fileBytes = new byte[0];
            // Render the view xml to a string, then parse that string into an XML dom.
            //string html = this.RenderActionResultToString(this.View("_ADAByCenterPartial", result,this));
            string html = Helper.RenderActionResultToString(this.View("ADAByCenterPDFView", result), this);
            fileBytes = Helper.GetPDFBytesFromHTML(html,false);


            // Send the binary data to the browser.
            //  return new BinaryContentResult(fileBytes, "application/pdf");

            return File(fileBytes, "application/pdf", "ADA By Center.pdf");
    }


        [CustAuthFilter(RoleEnum.ERSEAManager)]
        public ERSEADashBoard GetERSEADashboard()
        {
            ERSEADashBoard obj = new ERSEADashBoard();
            try
            {
                //get the Json filepath  
                //string file = Server.MapPath("~/Content/JSON/us_census_zipcode.json");E:\Pandian\Git Repository\Fingerprints-Production\Fingerprints\Views\ERSEA\ADAByCenter.cshtml
                //deserialize JSON from file  
                //var Json = System.IO.File.ReadAllText(file);

                List<ApplicationEnrollment> lstApplication = new List<ApplicationEnrollment>();
                List<ADA> lstADA = new List<ADA>();
                List<CityName> lstCityName = new List<CityName>();
                int firstMonth = 0;
                new ERSEAData().GetApplicationEnrollmentBasedonZip(ref lstApplication, Session["AgencyID"].ToString());
                // obj = new ERSEAData().GetADABasedonCenter(ref lstADA, ref firstMonth, Session["AgencyID"].ToString());
                obj = new ERSEAData().GetADABasedonCenter(ref lstADA, ref firstMonth);
                new ERSEAData().GetCityName(ref lstCityName);

                // var firstnM = Enum.GetName(typeof(Month), 0);
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
         
                int count = (obj.listADA.Count() > 0) ? obj.listADA.Count() * 100 : 0;

                if (count > 0)
                {
                    obj.TotalADA.Jan = Math.Round((obj.listADA.Sum(a => a.Jan) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Feb = Math.Round((obj.listADA.Sum(a => a.Feb) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Mar = Math.Round((obj.listADA.Sum(a => a.Mar) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Apr = Math.Round((obj.listADA.Sum(a => a.Apr) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.May = Math.Round((obj.listADA.Sum(a => a.May) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Jun = Math.Round((obj.listADA.Sum(a => a.Jun) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Jul = Math.Round((obj.listADA.Sum(a => a.Jul) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Aug = Math.Round((obj.listADA.Sum(a => a.Aug) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Sep = Math.Round((obj.listADA.Sum(a => a.Sep) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Oct = Math.Round((obj.listADA.Sum(a => a.Oct) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Nov = Math.Round((obj.listADA.Sum(a => a.Nov) / count) * 100, 1,MidpointRounding.ToEven);
                    obj.TotalADA.Dec = Math.Round((obj.listADA.Sum(a => a.Dec) / count) * 100, 1,MidpointRounding.ToEven);
                }
              //  decimal total = (obj.TotalADA.Jan + obj.TotalADA.Feb + obj.TotalADA.Mar + obj.TotalADA.Apr + obj.TotalADA.May + obj.TotalADA.Jun + obj.TotalADA.Jul + obj.TotalADA.Aug + obj.TotalADA.Sep + obj.TotalADA.Oct + obj.TotalADA.Nov + obj.TotalADA.Dec); //0643
                //decimal total = (lstADA.Sum(a => a.Jan) + lstADA.Sum(a => a.Feb) + lstADA.Sum(a => a.Mar) + lstADA.Sum(a => a.Apr) + lstADA.Sum(a => a.May) + lstADA.Sum(a => a.Jun) + lstADA.Sum(a => a.Jul) + lstADA.Sum(a => a.Aug) + lstADA.Sum(a => a.Sep) + lstADA.Sum(a => a.Oct) + lstADA.Sum(a => a.Nov) + lstADA.Sum(a => a.Dec));
                if (count == 0)
                {

                }
                // ViewBag.OverAllPercentage = Math.Round((total / (300)) * 100, 0);
                //  obj.OverAllPercentage = Math.Round((total / (12)), 0);  //0643
                obj.OverAllApplication = lstApplication.Sum(a => a.Application);
                obj.OverAllEnrollment = lstApplication.Sum(a => a.Enrollment);
                obj.OverAllWithdrawn = lstApplication.Sum(a => a.Withdrawn);
                obj.OverAllDropped = lstApplication.Sum(a => a.Dropped);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            // return View(obj);
            return obj;
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


       

        [CustAuthFilter(RoleEnum.ERSEAManager)]
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
        [CustAuthFilter(RoleEnum.ERSEAManager)]
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
        [CustAuthFilter()]
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
        [CustAuthFilter()]
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

        [CustAuthFilter()]
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
        [CustAuthFilter()]
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

        [CustAuthFilter()]
        public ActionResult CommunityAssessment()
        {
            return View();
        }

        //GET Center Analysis page;
        [CustAuthFilter(RoleEnum.ERSEAManager)]
        public ActionResult CenterAnalysis()
        {
            return View();
        }

        //GET Workshop Analysis By Center page.
        [CustAuthFilter(RoleEnum.ERSEAManager,RoleEnum.CenterManager,RoleEnum.EducationManager)]
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
        [CustAuthFilter()]
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
            return Json(new { workshopList, centerList, isLink }, JsonRequestBehavior.AllowGet);
        }



        [JsonMaxLength]
        [CustAuthFilter()]
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
        [CustAuthFilter()]
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
        [CustAuthFilter()]
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

        [CustAuthFilter()]
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
        [CustAuthFilter()]
        public ActionResult GETChildrenByCenter(string centerId,string classRoomId, string programId,  int reqPage = 0, int pgSize = 0, int skipRow = 0, string searchText = "")
        {
            ChildrenInfoClass childrenInfo = new ChildrenInfoClass();
            try
            {

                CenterAnalysisParameters CenterAnalysisParameters = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    ClassRoomId=(classRoomId=="0" || classRoomId=="")?0:Convert.ToInt64(EncryptDecrypt.Decrypt64(classRoomId)),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };

                childrenInfo = new ERSEAData().GetChildrenByCenter(CenterAnalysisParameters);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childrenInfo, JsonRequestBehavior.AllowGet);

        }

        [JsonMaxLength]
        [CustAuthFilter()]
        public ActionResult GetChildrenByClassRoom(string centerId, string classroomId, string programId, int reqPage = 0, int pgSize = 0, int skipRow = 0)
        {
            List<ChildrenInfo> children_List = new List<ChildrenInfo>();
            ChildrenInfoClass childInfoClass = new ChildrenInfoClass();
            try
            {

                CenterAnalysisParameters getchildClassParameter = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    ClassRoomId= (classroomId == "0" || classroomId == "") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(classroomId))
            };
                childInfoClass = new ERSEAData().GetChildrenByClassRoom(getchildClassParameter);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfoClass, JsonRequestBehavior.AllowGet);


        }

        [JsonMaxLength]
        public JsonResult GetEnrolledChildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            List<ChildrenInfo> enrolledList = new List<ChildrenInfo>();

            ChildrenInfoClass childInfo = new ChildrenInfoClass();

            try
            {
                // long centerID =(centerId=="0")?0: Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                // long programID = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId));

                CenterAnalysisParameters CenterAnalysisParameters = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };
                childInfo = new ERSEAData().GetEnrolledChildrenData(CenterAnalysisParameters);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(childInfo, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetReturningChildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {
                CenterAnalysisParameters returningParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };
                childInfo = new ERSEAData().GetReturningChildren(returningParam);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfo, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetGraduatingChildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {
                CenterAnalysisParameters gradParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };
                childInfo = new ERSEAData().GetGraduatingChildrenData(gradParam);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfo, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetWaitingChildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {

                CenterAnalysisParameters waitingParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };

                childInfo = new ERSEAData().GetWatitingChildrenData(waitingParam);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfo, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetWithdrawnChildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {

            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {
                CenterAnalysisParameters withdramParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };

                childInfo = new ERSEAData().GetWithdrawnChildList(withdramParam);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfo, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetDroppedChildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {
                CenterAnalysisParameters droppedParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };

                childInfo = new ERSEAData().GetDroppedChildList(droppedParam);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfo, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetHomelessChildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {

                CenterAnalysisParameters homeLessParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };

                childInfo = new ERSEAData().GetHomelessChildrenData(homeLessParam);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfo, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetOverIncomeChildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            List<SelectListItem> ParentList = new List<SelectListItem>();
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {

                CenterAnalysisParameters overIncomeParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText

                };

                childInfo = new ERSEAData().GetOverIncomeChildrenData(out ParentList, overIncomeParam);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { childInfo, ParentList }, JsonRequestBehavior.AllowGet);
        }


        [JsonMaxLength]
        public JsonResult GetFosterChild(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {

                CenterAnalysisParameters fosterParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText
                };
                childInfo = new ERSEAData().GetFosterChildData(fosterParam);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfo, JsonRequestBehavior.AllowGet);
        }

        [JsonMaxLength]
        public JsonResult GetLeadschildren(string centerId, string programId, int reqPage, int pgSize, int skipRow, string searchText = "")
        {
            List<LeadsChildren> leadsChildrenlist = new List<LeadsChildren>();
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {

                CenterAnalysisParameters leadsParam = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    Skip = skipRow,
                    Take = pgSize,
                    RequestedPage = reqPage,
                    SearchText = searchText
                };

                childInfo = new ERSEAData().GetLeadsChildrenData(leadsParam);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(childInfo, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
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


        public ActionResult DownloadERSEAReport(string centerId, string programId, string reportFor, bool isAllCenter)
        {
            try
            {
                ChildrenInfoClass infoModel = new ChildrenInfoClass();
                List<SelectListItem> parentNameList = new List<SelectListItem>();
                Export export = new Export();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename=ERSEA Status Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                CenterAnalysisParameters parameters = new CenterAnalysisParameters
                {
                    CenterId = (centerId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId)),
                    ProgramId = (programId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(programId.ToString())),
                    RequestedPage = 1,
                    SearchText = "",
                    Take = 500,
                    Skip = 0

                };

                switch (reportFor)
                {
                    case "#FosterDisplaymodal":
                        Response.AddHeader("content-disposition", "attachment;filename=Enrolled Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                        infoModel = new ERSEAData().GetChildrenByCenter(parameters);
                        break;

                    case "#EnrolledModal":
                        Response.AddHeader("content-disposition", "attachment;filename=Enrolled Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                        infoModel = new ERSEAData().GetEnrolledChildrenData(parameters);
                        break;
                    case "#WithdrawnModal":
                        Response.AddHeader("content-disposition", "attachment;filename=Withdrawn Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                        infoModel = new ERSEAData().GetWithdrawnChildList(parameters);
                        break;
                    case "#DroppedModal":
                        Response.AddHeader("content-disposition", "attachment;filename=Dropped Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                        infoModel = new ERSEAData().GetDroppedChildList(parameters);
                        break;

                    case "#WaitingModal":
                        Response.AddHeader("content-disposition", "attachment;filename=Waiting Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                        infoModel = new ERSEAData().GetWatitingChildrenData(parameters);
                        break;
                    case "#ReturningModal":

                        Response.AddHeader("content-disposition", "attachment;filename=Returning Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                        infoModel = new ERSEAData().GetReturningChildren(parameters);
                        break;
                    case "#GraduatingModal":
                        Response.AddHeader("content-disposition", "attachment;filename=Graduating Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                        infoModel = new ERSEAData().GetGraduatingChildrenData(parameters);
                        break;
                    case "#OverIncomeModal":
                        Response.AddHeader("content-disposition", "attachment;filename=Over Income Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                        infoModel = new ERSEAData().GetOverIncomeChildrenData(out parentNameList, parameters);
                        break;
                    case "#FosterModal":
                        Response.AddHeader("content-disposition", "attachment;filename=Foster Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                        infoModel = new ERSEAData().GetFosterChildData(parameters);
                        break;
                    case "#HomeLessModal":
                        Response.AddHeader("content-disposition", "attachment;filename=Homeless Client Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                        infoModel = new ERSEAData().GetHomelessChildrenData(parameters);
                        break;
                    case "#ExternalLeadsModal":
                        Response.AddHeader("content-disposition", "attachment;filename=External Leads Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                        infoModel = new ERSEAData().GetLeadsChildrenData(parameters);
                        break;
                }


                System.IO.MemoryStream ms = export.ExportERSEACenterAnalysisReport(reportFor, infoModel, parentNameList, isAllCenter);
                ms.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                return View();
                // return File(fullPath, "application/vnd.ms-excel", file);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

    }
}
