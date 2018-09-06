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
        [CustAuthFilter()]
        public ActionResult PublicAssetEntry()
        {
            Inkind inkind = new Inkind();

            try
            {

                inkind = GetInkindActivityFromTempData();

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

        [CustAuthFilter()]

        public JsonResult GetParentCompanyDonorsBySearch(string searchName = "")
        {
            Inkind inkind = new Inkind();
            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
                inkind = new InKindData().GetInkindParentCompanyDonors(staffDetails, searchName);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return Json(inkind, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Gets the List Classrooms based on Center Id for Inkind Process. 
        /// </summary>
        /// <param name="Centerid"></param>
        /// <returns></returns>
       [CustAuthFilter()]

        public JsonResult GetClassRoomsForInkind(string Centerid = "0")
        {
            try
            {
                return Json(new RosterData().Getclassrooms(Centerid, Session["AgencyID"].ToString(), isEndOfYear: false, isInkind: true));
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
                StaffDetails details = new StaffDetails();
                //details.AgencyId = (Session["AgencyId"] == null) ? (Guid?)null : new Guid(Session["AgencyID"].ToString());
                //details.UserId = new Guid(Session["UserID"].ToString());
                //details.RoleId = new Guid(Session["RoleID"].ToString());

                inkind = new InKindData().GetInkindActivities(details);
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

                //activity.StaffDetails = new StaffDetails
                //{
                //    UserId = new Guid(Session["UserID"].ToString()),
                //    RoleId = new Guid(Session["RoleID"].ToString()),
                //    AgencyId = (Session["AgencyId"] == null) ? (Guid?)null : new Guid(Session["AgencyID"].ToString())
                //};


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
                //StaffDetails details = new StaffDetails
                //{
                //    UserId = new Guid(Session["UserID"].ToString()),
                //    RoleId = new Guid(Session["RoleID"].ToString()),
                //    AgencyId = (Session["AgencyId"] == null) ? (Guid?)null : new Guid(Session["AgencyID"].ToString())
                //};
                StaffDetails details = StaffDetails.GetInstance();

                isResult = new InKindData().DeleteInkindActivity(details, activityCode);
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
        [CustAuthFilter()]

        public JsonResult InsertInkindTransactions(string modelString = "")
            //public JsonResult InsertInkindTransactions(Inkind _inkind)
        {
            int returnResult = 0;
            long identityRet = 0;
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Inkind model = new Inkind();
                model = serializer.Deserialize<Inkind>(modelString);


                var fileKeys = Request.Files.AllKeys;


                model.InkindTransactionsList[0].InkindAttachmentsList = new List<InkindAttachments>();
                var count =model.InkindTransactionsList.Count;
                if(count==1)
                {
                    for (int i = 0; i < fileKeys.Length; i++)
                    {
                        model.InkindTransactionsList[0].InkindAttachmentsList.Add(new InkindAttachments
                        {

                            InkindAttachmentFile = Request.Files[i],
                            InkindAttachmentFileName = Request.Files[i].FileName,
                            InkindAttachmentFileExtension=Path.GetExtension(Request.Files[i].FileName),
                            InkindAttachmentFileByte= new BinaryReader(Request.Files[i].InputStream).ReadBytes(Request.Files[i].ContentLength)
                        });
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

                            returnResult = new InKindData().InsertInkindTransactions(item);
                        }

                    }
                }
                else
                {

                    foreach (var item in model.InkindTransactionsList)
                    {
                        item.InKindAmount = GetAmountByInkindType(item);

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
            ParentParticipation parentParticipation = new FingerprintsModel.ParentParticipation();

            try
            {
                parentParticipation = new InKindData().GetParentParticipationInkind(StaffDetails.GetInstance());
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(parentParticipation);
        }

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
                    StaffDetails details = StaffDetails.GetInstance();
                    _tempinkindDetails = new InKindData().GetInkindActivities(details);
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
    }
}
