using Fingerprints.Filters;
using FingerprintsData;
using FingerprintsModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Fingerprints.Utilities
{
     public abstract class  Helper
    {

        public static int GetYakkrCountByUserId(string UserId, string AgencyId, string Status = "1")
        {
            int Count = 0;
            try
            {
                Count = (new YakkrData().GetYakkrCountByUserId(new Guid(AgencyId), new Guid(UserId), Status));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Count;
        }

        public static List<SelectListItem> GetCentersByUserId(string UserId, string AgencyId, string RoleId, bool isReqAdminSite = false, bool isCenterBasedOnly = false, bool isHomebasedonly = false,bool isEndOfYear=false,bool allCenters=false)
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtCenters = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "-Select Center-", Value = "0" });
                new CenterData().GetCentersByUserId(ref dtCenters, UserId, AgencyId, RoleId,isReqAdminSite,isCenterBasedOnly,isHomebasedonly,isEndOfYear,allCenters);
                if (dtCenters != null)
                {
                    if (dtCenters.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCenters.Rows)
                        {
                            lstCenters.Add(new SelectListItem { Text = dr["CenterName"].ToString(), Value = dr["Center"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstCenters;
        }


        public static List<SelectListItem> GetCenterBasedCentersByAgencyID(string UserId, string AgencyId, string RoleId)
        {

            string type = "3"; //Only CenterBased
            List<HrCenterInfo> hrCenterList = new List<HrCenterInfo>();
            List<SelectListItem> centerList = new List<SelectListItem>();

            hrCenterList = new agencyData().getagencyid(AgencyId, type);

            centerList.Add(new SelectListItem
            {
                Text = "--Select Center--",
                Value = "0"
            });
            centerList.AddRange((from HrCenterInfo hrc in hrCenterList
                                 select new SelectListItem
                                 {
                                     Text = hrc.Name,
                                     Value = EncryptDecrypt.Encrypt64(hrc.CenterId)
                                 }

                                 ));
            return centerList;

        }

        public static List<SelectListItem> GetMinutes()
        {
            List<SelectListItem> Duration = new List<SelectListItem>();
            try
            {
                Duration.Add(new SelectListItem { Value = "0", Text = "Minutes" });
                Duration.Add(new SelectListItem { Value = "15", Text = "15" });
                Duration.Add(new SelectListItem { Value = "30", Text = "30" });
                Duration.Add(new SelectListItem { Value = "45", Text = "45" });
                Duration.Add(new SelectListItem { Value = "60", Text = "60" });

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Duration;
        }

        public static List<SelectListItem> GetCentersByUserIdSearch(string UserId, string AgencyId, string RoleId)
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtCenters = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "All Center", Value = "0" });
                new CenterData().GetCentersByUserId(ref dtCenters, UserId, AgencyId, RoleId);
                if (dtCenters != null)
                {
                    if (dtCenters.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCenters.Rows)
                        {
                            lstCenters.Add(new SelectListItem { Text = dr["CenterName"].ToString(), Value = dr["Center"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstCenters;
        }
        public static List<SelectListItem> GetWorkShop()
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtWorkshop = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "All Workshop", Value = "0" });
                new EventsData().GetWorkshop(ref dtWorkshop);
                if (dtWorkshop != null)
                {
                    if (dtWorkshop.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtWorkshop.Rows)
                        {
                            lstCenters.Add(new SelectListItem { Text = dr["WorkshopName"].ToString(), Value = dr["Id"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstCenters;
        }


        public static List<SelectListItem> GetUsersByRoleId(string targetRoleId, string roleId, string userId, string agencyId)
        {
            List<SelectListItem> staffDetails = new List<SelectListItem>();
            try
            {
                staffDetails.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                staffDetails.AddRange(new HomevisitorData().GetUsersByRoleId(targetRoleId, roleId, userId, agencyId));

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return staffDetails;
        }

        public static List<SelectListItem> GetAttendanceType(string agencyId, string userId, bool homeBased)
        {
            List<SelectListItem> attendanceTypeList = new List<SelectListItem>();

            try
            {
                attendanceTypeList.Add(new SelectListItem
                {
                    Text = "--Select--",
                    Value = "0"
                });

                attendanceTypeList.AddRange(new TeacherData().GetAttendanceType(agencyId, userId, homeBased));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return attendanceTypeList;
        }

        /// <summary>
        /// method to get, whether Logged in user having access to PIR and Section B
        /// </summary>
        /// <returns></returns>
        public static bool GetUserAccessPIR(string mode="1")
        {
            bool isAccess = false;
            try
            {
                isAccess = new agencyData().GetUserAccessPIR(mode);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isAccess;
        }
      

        //public static List<SelectListItem> GetChildDetails(string AgencyId)
        //{
        //    List<SelectListItem> lstCenters = new List<SelectListItem>();
        //    try
        //    {
        //        DataTable dtCenters = new DataTable();
        //        lstCenters.Add(new SelectListItem { Text = "Choose", Value = "0" });
        //        new BillingData().GetChildDetails(ref dtCenters, AgencyId);
        //        if (dtCenters != null)
        //        {
        //            if (dtCenters.Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in dtCenters.Rows)
        //                {
        //                    lstCenters.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["ClientID"].ToString() });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        clsError.WriteException(Ex);
        //    }
        //    return lstCenters;
        //}

        public static List<SelectListItem> GetAgencyDetails()
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtCenters = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "Choose", Value = "0" });
                new BillingData().GetAgencyDetails(ref dtCenters);
                if (dtCenters != null)
                {
                    if (dtCenters.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCenters.Rows)
                        {
                            lstCenters.Add(new SelectListItem { Text = dr["AgencyName"].ToString(), Value = dr["AgencyId"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstCenters;
        }

        public static List<SelectListItem> GetProgramTypeDetails(string AgencyId)
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtCenters = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "Program Type", Value = "0" });
                new BillingData().GetProgramTypeDetails(ref dtCenters, AgencyId);
                if (dtCenters != null)
                {
                    if (dtCenters.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCenters.Rows)
                        {
                            lstCenters.Add(new SelectListItem { Text = dr["ProgramType"].ToString(), Value = dr["ProgramTypeID"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstCenters;
        }

        public static List<SelectListItem> GetMaterialGroup(string RoleId)
        {
            List<SelectListItem> lstOptions = new List<SelectListItem>();
            try
            {
                lstOptions.Add(new SelectListItem { Text = "Choose", Value = "0" });
                lstOptions.Add(new SelectListItem { Text = "General", Value = "General" });
                if (RoleId.ToUpper() == "82B862E6-1A0F-46D2-AAD4-34F89F72369A" || RoleId.ToUpper() == "E4C80FC2-8B64-447A-99B4-95D1510B01E9")
                    lstOptions.Add(new SelectListItem { Text = "Education", Value = "Education" });
                if (RoleId.ToUpper() == "047C02FE-B8F1-4A9B-B01F-539D6A238D80")
                    lstOptions.Add(new SelectListItem { Text = "Disabilites", Value = "Disabilites" });
                if (RoleId.ToUpper() == "699168AC-AD2D-48AC-B9DE-9855D5DC9AF8")
                    lstOptions.Add(new SelectListItem { Text = "Mental Health", Value = "Mental Health" });
                if (RoleId.ToUpper() == "CE744500-7CA2-4122-B15F-686C44811A51")
                    lstOptions.Add(new SelectListItem { Text = "Nutrition", Value = "Nutrition" });
                if (RoleId.ToUpper() == "94CDF8A2-8D81-4B80-A2C6-CDBDC5894B6D" || RoleId.ToUpper() == "E4C80FC2-8B64-447A-99B4-95D1510B01E9")
                    lstOptions.Add(new SelectListItem { Text = "Social Services", Value = "Social Services" });
                if (RoleId.ToUpper() == "AE148380-F94E-4F7A-A378-897C106F1A52")
                    lstOptions.Add(new SelectListItem { Text = "Transportation", Value = "Transportation" });
                if (RoleId.ToUpper() == "A31B1716-B042-46B7-ACC0-95794E378B26")
                    lstOptions.Add(new SelectListItem { Text = "Nursing", Value = "Nursing" });

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstOptions;
        }

        public static List<SelectListItem> GetCenterDetails(string AgencyId)
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtCenters = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "All", Value = "0" });
                new BillingData().GetCenterDetails(ref dtCenters, AgencyId);
                if (dtCenters != null)
                {
                    if (dtCenters.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCenters.Rows)
                        {
                            lstCenters.Add(new SelectListItem { Text = dr["CenterName"].ToString(), Value = dr["CenterId"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstCenters;
        }

        public static List<SelectListItem> GetUserCenterDetails(string AgencyId)
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtCenters = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "Centers", Value = "0" });
                new BillingData().GetCenterDetails(ref dtCenters, AgencyId);
                if (dtCenters != null)
                {
                    if (dtCenters.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCenters.Rows)
                        {
                            lstCenters.Add(new SelectListItem { Text = dr["CenterName"].ToString(), Value = dr["CenterId"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstCenters;
        }

        public static List<SelectListItem> GetClassroomDetails(string AgencyId)
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtCenters = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "Classrooms", Value = "0" });
                new BillingData().GetClassroomDetails(ref dtCenters, AgencyId);
                if (dtCenters != null)
                {
                    if (dtCenters.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCenters.Rows)
                        {
                            lstCenters.Add(new SelectListItem { Text = dr["ClassroomName"].ToString(), Value = dr["ClassroomID"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstCenters;
        }

        public static List<SelectListItem> GetActiveAndFutureYears(string AgencyId)
        {
            List<SelectListItem> listYears = new List<SelectListItem>();
            try
            {
                listYears.Add(new SelectListItem { Text = "Choose", Value = "0" });
                //listYears.AddRange(GetActiveProgramYear(AgencyId));
                //listYears.AddRange(GetCurrentAndFutureYear());
                listYears.AddRange(GetCurrentandFutureYearsByAgency(AgencyId));
                listYears = listYears.Distinct().ToList();
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return listYears;
        }


        public static List<SelectListItem>GetAcademicMonths(StaffDetails staff)
        {
            List<SelectListItem> academicMonths;
            try
            {

                academicMonths= Fingerprints.Common.FactoryInstance.Instance.CreateInstance<agencyData>().GetAcademicMonths(staff);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
                academicMonths = new List<SelectListItem>();
            }
            return academicMonths;
        }

        public static List<SelectListItem> GetCurrentandFutureYearsByAgency(string agencyId)
        {
            List<SelectListItem> futureYears = new List<SelectListItem>();
            try
            {
                futureYears.AddRange(GetActiveProgramYear(agencyId));

                if (futureYears.Count > 0)
                {
                    string currentyear = futureYears[0].Text;
                    string nextText = "";
                    string nextValue = "";
                    nextText = (Convert.ToInt32(currentyear.Split('-')[0]) - 1).ToString() + "-" + (Convert.ToInt32(currentyear.Split('-')[0])).ToString();
                    nextValue = (nextText).ToString().Split('-')[0].Substring(2, 2) + "-" + (nextText).ToString().Split('-')[1].Substring(2, 2);

                    futureYears.Clear();
                    futureYears.Add(new SelectListItem
                    {
                        Text = nextText,
                        Value = nextValue
                    });

                    futureYears.Add(new SelectListItem
                    {
                        Text = currentyear,
                        Value = (currentyear).ToString().Split('-')[0].Substring(2, 2) + "-" + (currentyear).ToString().Split('-')[1].Substring(2, 2)
                    });

                    nextText = (Convert.ToInt32(currentyear.Split('-')[1])).ToString() + "-" + (Convert.ToInt32(currentyear.Split('-')[1]) + 1).ToString();
                    nextValue = (nextText).ToString().Split('-')[0].Substring(2, 2) + "-" + (nextText).ToString().Split('-')[1].Substring(2, 2);
                    futureYears.Add(new SelectListItem
                    {
                        Text = nextText,
                        Value = nextValue

                    });


                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return futureYears;

        }

        public static List<SelectListItem> GetActiveProgramYear(string AgencyId)
        {
            List<SelectListItem> lstActiveYears = new List<SelectListItem>();
            try
            {
                DataTable dtActiveProgramYear = new DataTable();
                new ExecutiveData().GetActiveProgramYear(ref dtActiveProgramYear, AgencyId);
                if (dtActiveProgramYear != null)
                {
                    if (dtActiveProgramYear.Rows.Count > 0)
                    {
                        string CurrentYear = DateTime.Now.Year.ToString().Substring(0, 2);
                        foreach (DataRow dr in dtActiveProgramYear.Rows)
                        {
                            string[] year = dr["ActiveProgramYear"].ToString().Split('-');
                            string activeyear = CurrentYear + year[0] + "-" + CurrentYear + year[1];
                            lstActiveYears.Add(new SelectListItem { Text = activeyear, Value = dr["ActiveProgramYear"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }

            return lstActiveYears;
        }

        public static List<SelectListItem> GetCurrentAndFutureYear()
        {
            List<SelectListItem> lstActiveYears = new List<SelectListItem>();
            try
            {
                string CurrentYear = DateTime.Now.Year.ToString().Substring(0, 2);
                int currentyear = DateTime.Now.Year;
                int Month = DateTime.Now.Month;
                string CurrentYearText = "", CurrentYearValue = "";
                string FutureYearText = "", FutureYearValue = "";
                if (Month >= 8)
                {
                    CurrentYearText = (currentyear + 1) + "-" + (currentyear + 2);
                    CurrentYearValue = (currentyear).ToString().Substring(2, 2) + "-" + (currentyear + 1).ToString().Substring(2, 2);
                    FutureYearText = (currentyear + 2) + "-" + (currentyear + 3);
                    FutureYearValue = (currentyear + 2).ToString().Substring(2, 2) + "-" + (currentyear + 3).ToString().Substring(2, 2);
                }
                else
                {
                    CurrentYearText = (currentyear) + "-" + (currentyear + 1);
                    CurrentYearValue = (currentyear).ToString().Substring(2, 2) + "-" + (currentyear + 1).ToString().Substring(2, 2);
                    FutureYearText = (currentyear + 1) + "-" + (currentyear + 2);
                    FutureYearValue = (currentyear + 1).ToString().Substring(2, 2) + "-" + (currentyear + 2).ToString().Substring(2, 2);
                }
                lstActiveYears.Add(new SelectListItem { Text = CurrentYearText, Value = CurrentYearValue });
                lstActiveYears.Add(new SelectListItem { Text = FutureYearText, Value = FutureYearValue });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstActiveYears;
        }

        public static List<SelectListItem> GetDomainDetails()
        {
            List<SelectListItem> lstDomain = new List<SelectListItem>();
            try
            {
                DataTable dtDomains = new DataTable();
                lstDomain.Add(new SelectListItem { Text = "Choose", Value = "0" });
                new RosterData().GetDomainDetails(ref dtDomains);
                if (dtDomains != null)
                {
                    if (dtDomains.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDomains.Rows)
                        {
                            lstDomain.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["DomainId"].ToString() });
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return lstDomain;
        }

        public static List<SelectListItem> states()
        {
            List<SelectListItem> items = new List<SelectListItem>() { 
            //List<State> los = new List<State> {
            new SelectListItem { Value="0",Text= "Select State" },
            new SelectListItem {Value= "AL", Text="AL-Alabama" },
            new SelectListItem {Value="AK", Text="AK-Alaska" },new SelectListItem { Value="AB",Text= "AB-Alberta" },
            new SelectListItem {Value="AZ", Text="AZ-Arizona" },new SelectListItem { Value="AR",Text= "AR-Arakansas"},
            new SelectListItem {Value="BC", Text="BC-British Columbia" },new SelectListItem{ Value="CA",Text= "CA-California"},
            new SelectListItem {Value="CO", Text= "CO-Colorado" },new SelectListItem{ Value="CT",Text= "CT-Connecticut"},
            new SelectListItem {Value="DE",Text= "DE-Delaware" },new SelectListItem{ Value="FL",Text= "FL-Florida"},
            new SelectListItem{Value="GA", Text="GA-Georiga" },new SelectListItem{ Value="GU",Text= "GU-Guam"},
            new SelectListItem {Value= "HI",Text= "HI-Hawali" },new SelectListItem{ Value="ID",Text= "ID-Idaho"},
            new SelectListItem {Value= "IL",Text= "IL-Illinois" },new SelectListItem{ Value="UB",Text= "UB-Indiana"},
            new SelectListItem {Value="IA",Text= "IA-Iowa" },new SelectListItem{ Value="KS",Text= "KS-Kansas"},
            new SelectListItem {Value="KY",Text= "KY-Kentucky" },new SelectListItem{ Value="LA",Text= "LA-Louisiana"},
            new SelectListItem {Value="ME",Text= "ME-Maine" },new SelectListItem{ Value="MB",Text= "MB-Manitoba"},
            new SelectListItem {Value= "MD",Text= "MD-Maryland" },new SelectListItem{ Value="MA",Text= "MA-Massachusetts"},
            new SelectListItem { Value= "MI",Text= "MI-Michigan" },new SelectListItem{ Value="MN",Text= "MN-Minnesota"},
            new SelectListItem {Value= "MS", Text="MS-Mississippi" },new SelectListItem{ Value="MO",Text= "MO-Missouri"},
            new SelectListItem {Value= "MT",Text= "MT-Montana" },new SelectListItem{ Value="NE",Text= "NE-Nebraska"},
            new SelectListItem {Value= "NV",Text= "NV-Nevada" },new SelectListItem{ Value="NB",Text= "NB-New Brunswick"},
            new SelectListItem { Value="NH",Text= "NH-New Hampshire" },new SelectListItem{ Value="NJ",Text= "NJ-New Jersey"},
            new SelectListItem { Value= "NM",Text= "NM-New Mexico" },new SelectListItem{ Value="NY",Text= "NY-New York"},
            new SelectListItem { Value= "NF",Text= "NF-Newfoundland" },new SelectListItem{ Value="NC",Text= "NC-North Carolina"},
            new SelectListItem{  Value="ND", Text="ND-North Dakota" },new SelectListItem{ Value="NT",Text= "NT-Northwest Territories"},
            new SelectListItem{ Value="NS",Text= "NS-Nova Scotia" },new SelectListItem{ Value="NU",Text= "NU-Nunavut"},
            new SelectListItem { Value = "OH", Text = "OH-Ohio" },new SelectListItem{ Value="OK",Text= "OK-Oklahoma"},
            new SelectListItem { Value = "ON", Text = "ON-Ontario" },new SelectListItem{ Value="OR",Text= "OR-Oregon"},
            new SelectListItem { Value ="PA",Text = "PA-Pennsylvania" },new SelectListItem{ Value="PE",Text= "PE-Prince Edward Island"},
            new SelectListItem { Value ="PR",Text = "PR-Puerto Rico" },new SelectListItem{ Value="QC",Text= "QC-Quebec"},new SelectListItem { Value= "RI",Text= "RI-Rhode Island"},
            new SelectListItem { Value ="SK",Text = "SK-Saskatchewan" },new SelectListItem{ Value="SC", Text = "SC-South Carolina" },
            new SelectListItem { Value ="SD",Text = "SD-South Dakota" },new SelectListItem{ Value="TN", Text = "TN-Tennessee" },
            new SelectListItem { Value ="TX",Text = "TX-Texas" },new SelectListItem{ Value="UT", Text = "UT-Itah" },
            new SelectListItem { Value ="VT",Text = "VT-Vermont" },new SelectListItem{ Value="VI", Text = "VI-Virgin Islands" },
            new SelectListItem { Value ="VA",Text = "VA-Virginia" },new SelectListItem{ Value="WA", Text = "WA-Washington" },
            new SelectListItem { Value ="WV",Text = "WV-West Virginia" },new SelectListItem{ Value="WI", Text = "WI-Wisconsin" },
            new SelectListItem { Value ="WY", Text ="WY-Wyoming" },new SelectListItem{ Value="YT", Text = "YT-Yukon Territory" }
            };
            return items;
        }

        public static List<ClosedInfo> CheckForTodayClosure(Guid? agencyId, Guid userId)
        {


            List<ClosedInfo> infoList = new List<ClosedInfo>();
            try
            {
                //Guid? agencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                //Guid userId = new Guid(Session["UserId"].ToString());
                infoList = new CenterData().CheckForTodayClosure(agencyId, userId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return infoList;
        }

        public static bool CheckUserHasHomeBasedCenter(string userId, string agencyId, string roleId)
        {
            bool result = false;
            try
            {

                StaffDetails details = StaffDetails.GetInstance();
                //details.AgencyId = new Guid(agencyId);
                //details.UserId = new Guid(userId);
                //details.RoleId = new Guid(roleId);


                result = new TeacherData().CheckUserHasHomeBased(details);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return result;
        }

        public static string GetDocumentName(long id,string str = "")
        {
            //string str = "Other";
            switch (id)
            {
                case 1:
                    str = "Income Tax Form 1040";
                    break;
                case 2:
                    str = "Written Statements form employers";
                    break;
                case 3:
                    str = "W-2";
                    break;
                case 4:
                    str = "Foster care reimbursement";
                    break;
                case 5:
                    str = "TANF documentation";
                    break;
                case 6:
                    str = "SSI Documentation";
                    break;
                case 7:
                    str = "Pay stub or pay enevelopes";
                    break;
                case 8:
                    str = "Unemployment";
                    break;
            }

            return str;
        }


        public static string Getbase64str(string path)
        {
            //   string b64 = @"data:image/png;base64,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LyIgeGFwUmlnaHRzOk1hcmtlZD0iVHJ1ZSIgeGFwUmlnaHRzOldlYlN0YXRlbWVudD0iaHR0cDovL3Byby5jb3JiaXMuY29tL3NlYXJjaC9zZWFyY2hyZXN1bHRzLmFzcD90eHQ9NDItMTU1NjQ5NzgmYW1wO29wZW5JbWFnZT00Mi0xNTU2NDk3OCI+DQoJCQk8ZGM6cmlnaHRzPg0KCQkJCTxyZGY6QWx0Pg0KCQkJCQk8cmRmOmxpIHhtbDpsYW5nPSJ4LWRlZmF1bHQiPsKpIENvcmJpcy4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuPC9yZGY6bGk+DQoJCQkJPC9yZGY6QWx0Pg0KCQkJPC9kYzpyaWdodHM+DQoJCQk8ZGM6Y3JlYXRvcj48cmRmOlNlcSB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPjxyZGY6bGk+Q29yYmlzPC9yZGY6bGk+PC9yZGY6U2VxPg0KCQkJPC9kYzpjcmVhdG9yPjwvcmRmOkRlc2NyaXB0aW9uPg0KCQk8cmRmOkRlc2NyaXB0aW9uIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyI+PHhtcDpSYXRpbmc+NDwveG1wOlJhdGluZz48eG1wOkNyZWF0ZURhdGU+MjAwOC0wMi0xMVQxOTozMjo0My4xNzNaPC94bXA6Q3JlYXRlRGF0ZT48L3JkZjpEZXNjcmlwdGlvbj48cmRmOkRlc2NyaXB0aW9uIHhtbG5zOk1pY3Jvc29mdFBob3RvPSJodHRwOi8vbnMubWljcm9zb2Z0LmNvbS9waG90by8xLjAvIj48TWljcm9zb2Z0UGhvdG86UmF0aW5nPjYzPC9NaWNyb3NvZnRQaG90bzpSYXRpbmc+PC9yZGY6RGVzY3JpcHRpb24+PC9yZGY6UkRGPg0KPC94OnhtcG1ldGE+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8P3hwYWNrZXQgZW5kPSd3Jz8+/9sAQwACAQECAQECAgICAgICAgMFAwMDAwMGBAQDBQcGBwcHBgcHCAkLCQgICggHBwoNCgoLDAwMDAcJDg8NDA4LDAwM/9sAQwECAgIDAwMGAwMGDAgHCAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwM/8AAEQgAXQB7AwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEB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";
            string b64 = "data:image/png;base64,";
            var filepath = Path.GetFullPath(path);
            Byte[] bytes = File.ReadAllBytes(path);
            String file = Convert.ToBase64String(bytes);
            b64 = b64 + file;
            return b64;
        }

        public static string RenderActionResultToString(ActionResult result,Controller ctrl)
        {
            // Create memory writer.
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            // Create fake http context to render the view.
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request,
                fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                ctrl.ControllerContext.RouteData,
                ctrl.ControllerContext.Controller);
            var oldContext = System.Web.HttpContext.Current;
            System.Web.HttpContext.Current = fakeContext;

            // Render the view.
            result.ExecuteResult(fakeControllerContext);

            // Restore old context.
            System.Web.HttpContext.Current = oldContext;

            // Flush memory and return output.
            memWriter.Flush();
            return sb.ToString();
        }

        public static  byte[]  GetPDFBytesFromHTML(string htmlMarkup, bool pnoEnable=true) {

            var output = new MemoryStream();
            using (var doc = new Document(PageSize.A4))
            {
                var writer = PdfWriter.GetInstance(doc, output);

                if (pnoEnable)
                {
                    PDFBackgroundHelper pageEventHelper = new PDFBackgroundHelper();
                    writer.PageEvent = pageEventHelper;
                }
                writer.CloseStream = false;
                doc.Open();


                var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
                tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor()); // use our new processor

                CssFilesImpl cssFiles = new CssFilesImpl();
                cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                var cssResolver = new StyleAttrCSSResolver(cssFiles);
                cssResolver.AddCss(@"code { padding: 2px 4px; }", "utf-8", true);
                var charset = Encoding.UTF8;
                var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(doc, writer));
                var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                var worker = new XMLWorker(pipeline, true);
                var xmlParser = new XMLParser(true, worker, charset);
                xmlParser.Parse(new StringReader(htmlMarkup));

            }
            output.Position = 0;
            

            return output.ToArray();
        }

    }
}