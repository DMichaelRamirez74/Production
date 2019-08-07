using System;
using System.Web.Mvc;
using FingerprintsModel;
using FingerprintsData;
using Fingerprints.CustomClasses;
using Fingerprints.Filters;
using System.Linq;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using FingerprintsModel.Enums;

namespace Fingerprints.Controllers
{

    /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         roleid =3b49b025-68eb-4059-8931-68a0577e5fa2 (Agency Admin)
         roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
         roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacher)
         roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
         roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
         roleid=7c2422ba-7bd4-4278-99af-b694dcab7367(executive)
         roleid=b65759ba-4813-4906-9a69-e180156e42fc (ERSEA Manager)
         roleid=047c02fe-b8f1-4a9b-b01f-539d6a238d80 (Disabilities Manager)
         roleid=9c34ec8E-2359-4704-be89-d9f4b7706e82 (Disability Staff)
         roleid=944d3851-75cc-41e9-b600-3fa904cf951f (Billing Manager)
         roleid=825f6940-9973-42d2-b821-5b6c7c937bfe(Facilities Manager)
         */
    public class InKindController : Controller
    {
        //
        // GET: /InKind/
        /// <summary>
        /// Get the PublicAssestEntry View.
        /// allows the Users: Teacher, Agency Admin,GenesisEarth Administrator, Center Manager,ERSEA Manager
        /// </summary>
        /// <returns></returns>
        // [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,b65759ba-4813-4906-9a69-e180156e42fc")]



        StaffDetails staff = StaffDetails.GetInstance();

        [CustAuthFilter()]
        public ActionResult InKindEntry()
        {
            Inkind inkind = new Inkind();

            try
            {

                inkind = GetInkindActivityFromTempData();


                if(inkind!=null && inkind.InkindPeriodsList!=null && inkind.InkindPeriodsList.Count>0)
                {
                    inkind.InkindPeriodsList = inkind.InkindPeriodsList.Where(x => x.IsClosed == false).ToList();
                }

                //StaffDetails details = StaffDetails.GetInstance();

                //inkind = new InKindData().GetInkindActivities(details);

                //TempData["Inkind"] = inkind;

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(inkind);
        }

        /// <summary>
        /// Get the list of Parent or Company based In-kind Donors available in the database.
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns>JsonResult list</returns>

        // [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,b65759ba-4813-4906-9a69-e180156e42fc")]
        [CustAuthFilter()]
        public JsonResult GetParentCompanyDonorsBySearch(int requestedPage,int pageSize,string searchName = "" )
        {
               Inkind inkind;
            
                inkind = new InKindData().GetInkindParentCompanyDonors(staff, searchName,requestedPage,pageSize);
                return Json(inkind, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the List Classrooms based on Center Id for Inkind Process. 
        /// </summary>
        /// <param name="Centerid"></param>
        /// <returns></returns>
        //[CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,b65759ba-4813-4906-9a69-e180156e42fc")]
        [CustAuthFilter()]
        public JsonResult GetClassRoomsForInkind(string Centerid = "0")
        {
            try
            {
                return Json(new RosterData().Getclassrooms(Centerid,staff, isEndOfYear: false, isInkind: true));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        /// <summary>
        /// Gets the InKind Activities Page.
        /// </summary>
        /// <returns></returns>
        //[CustAuthFilter("3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5")]

        [CustAuthFilter()]
        public ActionResult InKindActivities()
        {
            try
            {

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View();
        }


        /// <summary>
        /// JsonResult method to get the Inkind Activity Details list
        /// </summary>
        /// <returns>Inkind</returns>
        [CustAuthFilter()]
        public JsonResult GetInkindActivities()
        {
            Inkind inkind = new Inkind();

            try
            {
              


                inkind = new InKindData().GetInkindActivities(staff);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(inkind, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  Method to insert the In-kind Activities from View Page. 
        /// </summary>
        /// <param name="inkindActivity"></param>
        /// <returns>Json Result</returns>

        [CustAuthFilter()]
        public JsonResult InsertInkindActivity(string inkindActivity)
        {
            bool isResut = false;
            string returnResult = string.Empty;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                InkindActivity activity = new InkindActivity();
                activity = serializer.Deserialize<InkindActivity>(inkindActivity);

                returnResult = CheckActivityExists(activity);

                if (returnResult == "0")
                {
                    isResut = new InKindData().InsertInkindActivity(activity);
                    returnResult = (isResut) ? "1" : "4";
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the Inkind Activity.Returns,Boolean JSON Result.
        /// </summary>
        /// <param name="activityCode"></param>
        /// <returns></returns>

        [CustAuthFilter()]
        public JsonResult DeleteInkindActivity(string activityCode)
        {
            bool isResult = false;
            try
            {
               
    

                isResult = new InKindData().DeleteInkindActivity(staff, activityCode);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        /// <summary>
        /// method to check, whether Activity already exists in database.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public string CheckActivityExists(InkindActivity activity)
        {

            string existsCode = "";

            existsCode = new InKindData().CheckActivityExists(activity);
            return existsCode;

        }

        /// <summary>
        /// Gets the required details
        /// </summary>
        /// <returns></returns>

        // [CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,b65759ba-4813-4906-9a69-e180156e42fc")]
        [CustAuthFilter()]
        public JsonResult GetDetailsByActivityType(string activityCode, int reqDetails, string hours = "0", string minutes = "0", string miles = "0")
        {
            string returnDetails = string.Empty;
            double dblMiles = 0;
            double dblhours = 0;
            double dblMinutes = 0;
            Inkind inkind = new Inkind();
            InkindActivity activity = new InkindActivity();
            List<InkindActivity> activtyList = new List<InkindActivity>();
            List<string> activityCodeList = new List<string>();
            try
            {

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                activityCodeList = serializer.Deserialize<List<string>>(activityCode);

                activtyList = GetInkindActivityFromTempData().InkindActivityList;

                // inkind=new InKindData().GetInkindActivities(new StaffDetails(),)

                activityCodeList = activityCodeList.Distinct().ToList();
                if (activityCodeList.Count() > 0)
                {

                    activity.ActivityAmount = "0";

                    string activityAmount = "0";
                    foreach (string actCode in activityCodeList)
                    {
                        if (reqDetails == 1) //for Amount Type
                        {
                            activity = activtyList.Where(x => x.ActivityCode == actCode).FirstOrDefault();
                        }

                        else if (reqDetails == 2)//Amount Rate Calculation
                        {
                            activity = activtyList.Where(x => x.ActivityCode == actCode).FirstOrDefault();

                            double.TryParse(miles, out dblMiles);
                            double.TryParse(hours, out dblhours);
                            double.TryParse(minutes, out dblMinutes);



                            if (dblMiles > 0 && activity.ActivityAmountType == "1")
                            {
                                activityAmount = (Convert.ToDouble(activityAmount) + (Convert.ToDouble(activity.ActivityAmountRate) * dblMiles)).ToString("F", CultureInfo.InvariantCulture);
                            }
                            else if ((dblhours > 0 || dblMinutes > 0) && activity.ActivityAmountType == "2")
                            {

                                activityAmount = (Convert.ToDouble(activityAmount) + (Convert.ToDouble(activity.ActivityAmountRate) * (dblhours + (dblMinutes / 60)))).ToString("F", CultureInfo.InvariantCulture);
                            }

                            //else if (activity.ActivityAmountType == "3")
                            //{
                            //    // activity.ActivityAmount = (activity.ActivityAmount == "0") ? "" : activity.ActivityAmount;
                            //    activityAmount = activity.ActivityAmount;
                            //}

                        }

                    }

                    activity.ActivityAmount = activityAmount;

                }



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(activity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to insert the Inkind Activity Transactions by Parent or Corporate.
        /// </summary>
        /// <param name="InKindTransactions"></param>
        /// <returns></returns>
        /// 
        [JsonMaxLength]
        [ValidateInput(false)]
        //[CustAuthFilter("5ac211b2-7d4a-4e54-bd61-5c39d67a1106,82b862e6-1a0f-46d2-aad4-34f89f72369a,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,b65759ba-4813-4906-9a69-e180156e42fc")]
        [CustAuthFilter()]
        public JsonResult InsertInkindTransactions(string modelString = "", string cameraUploads=null)
        //public JsonResult InsertInkindTransactions(Inkind _inkind)
        {
            int returnResult = 0;
            long identityRet = 0;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                Inkind model = new Inkind();
                model = serializer.Deserialize<Inkind>(modelString);


                var fileKeys = Request.Files.AllKeys;


                model.InkindTransactionsList[0].InkindAttachmentsList = new List<InkindAttachments>();
                var count = model.InkindTransactionsList.Count;
                if (count == 1)
                {
                    for (int i = 0; i < fileKeys.Length; i++)
                    {
                        model.InkindTransactionsList[0].InkindAttachmentsList.Add(new InkindAttachments
                        {

                            InkindAttachmentFile = Request.Files[i],
                            InkindAttachmentFileName = Request.Files[i].FileName,
                            InkindAttachmentFileExtension = Path.GetExtension(Request.Files[i].FileName),
                            InkindAttachmentFileByte = new BinaryReader(Request.Files[i].InputStream).ReadBytes(Request.Files[i].ContentLength)
                        });
                    }

                    if(!string.IsNullOrEmpty(cameraUploads))
                    {
                        List<SelectListItem> cameraUplodList = serializer.Deserialize<List<SelectListItem>>(cameraUploads);

                        foreach(var item in cameraUplodList)
                        {
                            model.InkindTransactionsList[0].InkindAttachmentsList.Add(new InkindAttachments
                            {
                                //InkindAttachmentFile = Convert.FromBase64String(item.Value),
                                InkindAttachmentFileName = item.Text,
                                InkindAttachmentFileExtension = ".png",
                                InkindAttachmentFileByte = Convert.FromBase64String(item.Value)
                            });
                        }
                    }

                }






                if (Session["UserID"] == null)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }

                if (model.InKindDonarsContact.IsInsert)
                {
                    identityRet = new InKindData().InsertInKindDonors(model.InKindDonarsContact);
                    if (identityRet > 0)
                    {


                        foreach (var item in model.InkindTransactionsList)
                        {
                            item.ParentID = identityRet.ToString();

                            item.InKindAmount = GetAmountByInkindType(item);

                            item.CenterID = (item.CenterID > 0) ? item.CenterID : !string.IsNullOrEmpty(item.Enc_CenterID) ? Convert.ToInt32(EncryptDecrypt.Decrypt64(item.Enc_CenterID)) : item.CenterID;
                            item.ClassroomID = (item.ClassroomID > 0) ? item.ClassroomID : !string.IsNullOrEmpty(item.Enc_ClassroomID) ? Convert.ToInt32(EncryptDecrypt.Decrypt64(item.Enc_ClassroomID)) : item.ClassroomID;
                            item.IsActive = true;
                            item.StaffSignature = item.StaffSignature ?? new StaffSignature();
                            returnResult = new InKindData().InsertInkindTransactions(item);
                        }

                    }
                }
                else
                {

                    foreach (var item in model.InkindTransactionsList)
                    {
                        item.InKindAmount = GetAmountByInkindType(item);


                        item.CenterID = (item.CenterID > 0) ? item.CenterID : !string.IsNullOrEmpty(item.Enc_CenterID) ? Convert.ToInt32(EncryptDecrypt.Decrypt64(item.Enc_CenterID)) : item.CenterID;
                        item.ClassroomID = (item.ClassroomID > 0) ? item.ClassroomID : !string.IsNullOrEmpty(item.Enc_ClassroomID) ? Convert.ToInt32(EncryptDecrypt.Decrypt64(item.Enc_ClassroomID)) : item.ClassroomID;

                        item.IsActive = true;
                        item.StaffSignature = item.StaffSignature ?? new StaffSignature();
                        returnResult = new InKindData().InsertInkindTransactions(item);
                    }
                }
                returnResult = (returnResult > 0) ? 1 : returnResult;
                return Json(returnResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(returnResult, JsonRequestBehavior.AllowGet);
            }
        }

        [CustAuthFilter()]
        public ActionResult ParentParticipation()
        {
            ParentParticipation parentParticipation;
                parentParticipation = new InKindData().GetParentParticipationInkind(staff);
           
            return View(parentParticipation);
        }

        //[CustAuthFilter("82b862e6-1a0f-46d2-aad4-34f89f72369a,3b49b025-68eb-4059-8931-68a0577e5fa2,a65bb7c2-e320-42a2-aed4-409a321c08a5,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,b65759ba-4813-4906-9a69-e180156e42fc")]
        [CustAuthFilter()]
        public JsonResult CheckaddressForInKind(int Zipcode, string Address = "", string HouseHoldId = "0")
        {
            try
            {
                string Result;
                var Zipcodelist = new FamilyData().Checkaddress(out Result, Address, HouseHoldId, Zipcode);
                return Json(new { Zipcodelist, Result });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }

        /// <summary>
        /// Json Result to insert the In-kind activties from Parent Portal
        /// </summary>
        /// <param name="transactionString"></param>
        /// <returns>boolean</returns>
        [CustAuthFilter()]
        public JsonResult InsertParentParticipation(string transactionString = "", string parentID = "")
        {
            bool returnResult = false;
            try
            {
                ParentParticipation participation = new FingerprintsModel.ParentParticipation();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                participation.InKindTransactionsList = serializer.Deserialize<List<InKindTransactions>>(transactionString);


                returnResult = new InKindData().InsertParentParticipation(participation.InKindTransactionsList);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// JsonResult method to delete the In-Kind Sub-Activity based on SubActivityId
        /// </summary>
        /// <param name="subID"></param>
        /// <returns>boolean</returns>
        [CustAuthFilter()]
        public JsonResult DeleteInKindSubActivity(int subID)
        {
            bool returnResult = false;
            try
            {
                returnResult = new InKindData().DeleteInKindSubActivity(subID);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public JsonResult GetActivityByActivityType(int activitytype)
        {
            Inkind inKindData = new Inkind();
            try
            {

                inKindData = GetInkindActivityFromTempData();

                //if (inKindData != null)
                //{
                //    if (inKindData.InkindActivityList.Count() > 0)
                //    {
                //        inKindData.InkindActivityList = inKindData.InkindActivityList.Where(x => x.ActivityType == activitytype.ToString()).ToList();
                //    }
                //}
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(inKindData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the In-Kind Activities from TempData() and Gets from database in case Tempdata["InKind"] is null.
        /// </summary>
        /// <returns></returns>
        [CustAuthFilter()]
        public Inkind GetInkindActivityFromTempData()
        {
            Inkind _tempinkindDetails = new Inkind();


            try
            {
                if (Session["Inkind"] != null)
                {
                    _tempinkindDetails = (Inkind)Session["Inkind"];

                }
                else
                {
                 
                    _tempinkindDetails = new InKindData().GetInkindActivities(staff);
                    Session["Inkind"] = _tempinkindDetails;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return _tempinkindDetails;
        }

        [CustAuthFilter()]
        public decimal GetAmountByInkindType(InKindTransactions transactions)
        {
            decimal inkindAmount = 0;
            try
            {
                //Inkind inkindAMt = new Inkind();

                List<InkindActivity> activityListInkind = new List<InkindActivity>();

                activityListInkind = GetInkindActivityFromTempData().InkindActivityList;

                if (activityListInkind.Count() > 0)
                {
                    activityListInkind = activityListInkind.Where(x => x.ActivityCode == transactions.ActivityID.ToString()).ToList();

                    var sess = Session["Inkind"];

                    if (activityListInkind.Count() > 0)
                    {
                        foreach (var item in activityListInkind)
                        {
                            if (item.ActivityAmountType == "1") //Miles
                            {
                                inkindAmount = Convert.ToDecimal((Convert.ToDouble(inkindAmount) + (Convert.ToDouble(item.ActivityAmountRate) * Convert.ToDouble(transactions.MilesDriven))).ToString("F", CultureInfo.InvariantCulture));
                            }
                            else if (item.ActivityAmountType == "2")//Hours
                            {
                                inkindAmount = Convert.ToDecimal((Convert.ToDouble(inkindAmount) + (Convert.ToDouble(item.ActivityAmountRate) * (transactions.Hours + (transactions.Minutes / 60)))).ToString("F", CultureInfo.InvariantCulture));
                            }

                            else if (item.ActivityAmountType == "3")//fixed
                            {
                                //inkindAmount = Convert.ToDecimal(item.ActivityAmountRate);
                                inkindAmount = Convert.ToDecimal(transactions.InKindAmount);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return inkindAmount;

        }


        [CustAuthFilter()]
        public ActionResult InkindReport()
        {

            InkindReportModel inkindReportModel = new FingerprintsModel.InkindReportModel();
            inkindReportModel.TotalAmount = "$ 0.00";
            inkindReportModel.TotalHours = "0.00";
            inkindReportModel.TotalMiles = "0.00";

            inkindReportModel.InkindPeriodList = new InKindData().GetInkindPeriodsDate(staff, staff.AgencyId);

            return View(inkindReportModel);
        }


        [CustAuthFilter()]
        public JsonResult GetInkindOptionByFilterType(int filterType)
        {
            List<SelectListItem> inKindOptionList = new List<SelectListItem>();

         
            inKindOptionList = new InKindData().GetInkindOptionByFilter(staff, filterType);

            return Json(inKindOptionList, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Returns the In-Kind Activities List as Partial View, based on the filter criteria
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="requestedPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [CustAuthFilter()]
        public PartialViewResult GetInkindReportPartial(int filterType, string selectedOption,string centers, string fromDate, string toDate,string dateEntered, int requestedPage, int pageSize, string sortOrder, string sortColumn, string searchTerm, string searchTermType)
        {
            InkindReportModel inkindReportmodel = new InkindReportModel();

            try
            {

                inkindReportmodel.RequestedPage = requestedPage;
                inkindReportmodel.PageSize = pageSize;
                inkindReportmodel.SkipRows = inkindReportmodel.GetSkipRows();
                inkindReportmodel.FilterTypeEnum = EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.InkindReportFilter>(filterType.ToString());
                inkindReportmodel.FromDate = fromDate;
                inkindReportmodel.ToDate = toDate;
                inkindReportmodel.DateEntered = dateEntered;
                inkindReportmodel.SortColumn = string.IsNullOrEmpty(sortColumn) ||sortColumn=="null" ? string.Empty : sortColumn.ToUpperInvariant();
                inkindReportmodel.SortOrder =string.IsNullOrEmpty(sortOrder) ||sortOrder=="null"?string.Empty: sortOrder;

                inkindReportmodel.SubFilterOption = selectedOption;
                inkindReportmodel.Centers = centers;

                inkindReportmodel.SearchTerm = searchTerm;
                inkindReportmodel.SearchTermType = string.IsNullOrEmpty(searchTermType)||searchTermType=="null" ? string.Empty : searchTermType;



                new InKindData().GetInkindReportData(ref inkindReportmodel, staff);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return PartialView("~/Views/InKind/_InkindReportListPartial.cshtml", inkindReportmodel);
        }





        [HttpPost]
        [CustAuthFilter()]
        public JsonResult GetInkindTransactions(string transactionId)
        {


            Inkind inkind = new Inkind();

            transactionId = EncryptDecrypt.Decrypt64(transactionId);

            inkind = new InKindData().GetInkindTransactions(staff, transactionId);


            bool isCenterBased = false;
            bool isHomeBased = false;
            //if (inkind != null && inkind.InkindActivityList != null && inkind.InkindActivityList.Count > 0)
            //{
            //    isCenterBased = Convert.ToInt32(inkind.InkindActivityList[0].ActivityType) == (int)FingerprintsModel.Enums.InkindActivityType.Center;
            //    isHomeBased = Convert.ToInt32(inkind.InkindActivityList[0].ActivityType) == (int)FingerprintsModel.Enums.InkindActivityType.HomeBased;

            //}

            List<SelectListItem> centerList = Utilities.Helper.GetCentersByUserId(staff.UserId.ToString(), staff.AgencyId.ToString(), staff.RoleId.ToString(), isCenterBasedOnly: isCenterBased, isHomebasedonly: isHomeBased);

            centerList.ForEach(x => x.Value = EncryptDecrypt.Encrypt64(x.Value));

            return Json(new { inkind, centerList }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Post method to delete the in-kind transaction record by the InkindTransactionID
        /// </summary>
        /// <param name="inkindTransactionId"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter()]
        public JsonResult DeleteInkindTransactions(string inkindTransactionId)
        {

            bool isResult = false;
            try
            {
                InKindTransactions inkindTransactions = new InKindTransactions();

                inkindTransactions.IsActive = false;
                inkindTransactions.InkindTransactionID = Convert.ToInt32(EncryptDecrypt.Decrypt64(inkindTransactionId));

                inkindTransactions.StaffSignature = inkindTransactions.StaffSignature ?? new StaffSignature();

                isResult = new InKindData().InsertInkindTransactions(inkindTransactions) > 0;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }



            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult AutoCompleteInkindTransactions(int filterType, string selectedOption,string centers, string fromDate, string toDate, int requestedPage, int pageSize, string sortOrder, string sortColumn, string searchTerm, string searchTermType)
        {

            InkindReportModel inkindReportmodel = new InkindReportModel();
            inkindReportmodel.RequestedPage = requestedPage;
            inkindReportmodel.PageSize = pageSize;
            inkindReportmodel.SkipRows = inkindReportmodel.GetSkipRows();
            inkindReportmodel.FilterTypeEnum = EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.InkindReportFilter>(filterType.ToString());
            inkindReportmodel.FromDate = fromDate;
            inkindReportmodel.ToDate = toDate;
            inkindReportmodel.SortColumn = sortColumn.ToUpperInvariant();
            inkindReportmodel.SortOrder = sortOrder;
            inkindReportmodel.SubFilterOption = selectedOption;
            inkindReportmodel.Centers = centers;
            inkindReportmodel.SearchTerm = searchTerm;
            //inkindReportmodel.SearchFilterType = searchFilterType;


            List<SelectListItem> inkindSearchList = new InKindData().AutoCompleteInkindTransactionData(inkindReportmodel, staff);





            return Json(inkindSearchList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult GetInkindPeriods(Guid? targetAgencyId)
        {
             List<InkindPeriods> inkindPeriodList = new InKindData().GetInkindPeriodsDate(staff, targetAgencyId);
            return Json(inkindPeriodList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult ModifyInkindEntryPeriods(List<InkindPeriods> inkindPeriodList, Guid? targetAgencyId)
        {

            bool isResult = false;
            try
            {
                isResult = new InKindData().ModifyInkindEntryPeriodData(staff, inkindPeriodList, targetAgencyId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(isResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustAuthFilter()]
        public JsonResult CheckInKindRecordExistsByDate(string startDate, string endDate, Guid? targetAgencyId, long inkindPeriodID)
        {
            bool isExists = new InKindData().CheckInKindRecordExistsData(staff, startDate, endDate, targetAgencyId, inkindPeriodID);

            return Json(isExists, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustAuthFilter()]
        public FileResult GetInkindAttachment(int attachmentId, int inkindTransactionId)
        {

            try
            {
                InkindAttachments inkindAttachments = new InKindData().GetInkindAttachmentData(staff, attachmentId, inkindTransactionId);

                string attachmentFormat = "";

                switch (inkindAttachments.InkindAttachmentFileExtension)
                {
                    case ".xlsx":
                        attachmentFormat = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".pdf":
                        attachmentFormat = "application/pdf";
                        break;
                    case ".jpg":
                        attachmentFormat = "image/jpeg";
                        break;

                    case ".png":
                        attachmentFormat = "image/png";
                        break;
                    case ".docx":
                        attachmentFormat = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".txt":
                        attachmentFormat = "text/plain";
                        break;
                    default:
                        attachmentFormat = "application/octet-stream";
                        break;
                }

                return File(inkindAttachments.InkindAttachmentFileByte, attachmentFormat);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return File(new byte[0], "");
            }

        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult DeleteInkindAttachments(int attachmentId, int inkindTransactionId)
        {
            bool isResult = false;
            try
            {
                isResult = new InKindData().DeleteInkindAttachments(staff, attachmentId, inkindTransactionId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(isResult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [CustAuthFilter()]
        public void ExportInkindReport(InkindReportModel inkindReportmodel,int filterType,int reportFormatType )
        {

            try
            {

                var reportFormat = EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.ReportFormatType>(reportFormatType.ToString());
                inkindReportmodel.SkipRows = inkindReportmodel.GetSkipRows();
                inkindReportmodel.FilterTypeEnum = EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.InkindReportFilter>(filterType.ToString());
        
                inkindReportmodel.SortColumn = string.IsNullOrEmpty(inkindReportmodel.SortColumn) || inkindReportmodel.SortColumn == "null" ? string.Empty : inkindReportmodel.SortColumn.ToUpperInvariant();
                inkindReportmodel.SortOrder = string.IsNullOrEmpty(inkindReportmodel.SortOrder) || inkindReportmodel.SortOrder == "null"?string.Empty : inkindReportmodel.SortOrder;

                inkindReportmodel.SearchTermType = string.IsNullOrEmpty(inkindReportmodel.SearchTermType) || inkindReportmodel.SearchTermType == "null" ? string.Empty : inkindReportmodel.SearchTermType;

                new InKindData().GetInkindReportData(ref inkindReportmodel, staff);

                string imagePath = Server.MapPath("~/Images/");

                MemoryStream memoryStream = new MemoryStream();

                if (inkindReportmodel != null && inkindReportmodel.InkindReportList != null && inkindReportmodel.InkindReportList.Count > 0)
                {

                    memoryStream = new Export().ExportInkindReport(inkindReportmodel,imagePath, reportFormat);
                  
                }

                string reportName = "In-Kind Report";

                DownloadReport(memoryStream, reportFormat, reportName);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

        }


        [HttpPost]
        [CustAuthFilter()]

        public JsonResult GetInkindPeriodYakkrMappings(string yakkrId)
        {

                List<InkindPeriods> inkindPeriodList;
                inkindPeriodList= new InKindData().GetInkindPeriodYakkrMappingData(staff, yakkrId);
               return Json(inkindPeriodList,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public JsonResult ActivateNewInkindPeriod(string yakkrId)
        {
            bool isResult = false;

            isResult= new InKindData().ActivateNewInkindPeriodData(staff, yakkrId);
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        public void DownloadReport(MemoryStream memoryStream, ReportFormatType reportFormat, string reportName, params object[] args)
        {
            Response.Clear();
            Response.Buffer = true;

            switch (reportFormat)
            {
                case FingerprintsModel.Enums.ReportFormatType.Pdf:

                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + reportName + "" + DateTime.Now.ToString("MM/dd/yyyy") + ".pdf");

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bytes);

                    break;

                case FingerprintsModel.Enums.ReportFormatType.Xls:

                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + reportName + "" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                    memoryStream.WriteTo(Response.OutputStream);

                    break;

            }


            Response.End();
            Response.Close();
        }

    }
}
