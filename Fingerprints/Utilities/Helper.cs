using Fingerprints.Filters;
using FingerprintsData;
using FingerprintsModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.html;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fingerprints.Utilities
{
    public static class Helper
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

        public static List<SelectListItem> GetCentersByUserId(string UserId, string AgencyId, string RoleId, bool isReqAdminSite = false, bool isCenterBasedOnly = false, bool isHomebasedonly = false)
        {
            List<SelectListItem> lstCenters = new List<SelectListItem>();
            try
            {
                DataTable dtCenters = new DataTable();
                lstCenters.Add(new SelectListItem { Text = "-Select Center-", Value = "0" });
                new CenterData().GetCentersByUserId(ref dtCenters, UserId, AgencyId, RoleId,isReqAdminSite,isCenterBasedOnly,isHomebasedonly);
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
        public static bool GetUserAccessPIR(string mode = "1")
        {
            bool isAccess = false;
            try
            {
                isAccess = new agencyData().GetUserAccessPIR(mode);
            }
            catch (Exception ex)
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
    }
}