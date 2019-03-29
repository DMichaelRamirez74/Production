using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FingerprintsModel;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Globalization;
using FingerprintsDataAccessHandler;

namespace FingerprintsData
{
    public class NurseData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataTable familydataTable = null;
        DataSet _dataset = null;
        DataTable _dataTable = null;

        StaffDetails staff = StaffDetails.GetInstance();

        public Nurse GetData_AllDropdown(string agencyid, string userid, int i = 0, Nurse familyInfo = null)
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            Nurse Info = new Nurse();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_NurseInfo_Dropdowndata";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@AgencyId", agencyid));
                        command.Parameters.Add(new SqlParameter("@userid", userid));
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                    try
                    {
                        List<FingerprintsModel.Nurse.PrimarylangInfo> listlang = new List<FingerprintsModel.Nurse.PrimarylangInfo>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FingerprintsModel.Nurse.PrimarylangInfo obj = new FingerprintsModel.Nurse.PrimarylangInfo();
                            obj.LangId = Convert.ToString(dr["Id"].ToString());
                            obj.Name = dr["Name"].ToString();
                            listlang.Add(obj);
                        }

                        Info.langList = listlang;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        try
                        {
                            List<FingerprintsModel.Nurse.RaceSubCategory> _racelist = new List<FingerprintsModel.Nurse.RaceSubCategory>();
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                FingerprintsModel.Nurse.RaceSubCategory obj = new FingerprintsModel.Nurse.RaceSubCategory();
                                obj.RaceSubCategoryID = dr["Id"].ToString();
                                obj.Name = dr["Name"].ToString();
                                _racelist.Add(obj);
                            }
                            Info.raceCategory = _racelist;
                        }
                        catch (Exception ex)
                        {
                            clsError.WriteException(ex);
                        }
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        try
                        {
                            List<FingerprintsModel.Nurse.Relationship> _relationlist = new List<FingerprintsModel.Nurse.Relationship>();
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                FingerprintsModel.Nurse.Relationship obj = new FingerprintsModel.Nurse.Relationship();
                                obj.Id = dr["Id"].ToString();
                                obj.Name = dr["Name"].ToString();
                                _relationlist.Add(obj);
                            }
                            //  _rolelist.Insert(0, new Role() { RoleId = "0", RoleName = "Select" });
                            Info.relationship = _relationlist;
                        }
                        catch (Exception ex)
                        {
                            clsError.WriteException(ex);
                        }
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        try
                        {
                            List<Nurse.RaceInfo> _racelist = new List<Nurse.RaceInfo>();
                            //_staff.myList
                            foreach (DataRow dr in ds.Tables[3].Rows)
                            {
                                Nurse.RaceInfo obj = new Nurse.RaceInfo();
                                obj.RaceId = dr["Id"].ToString();
                                obj.Name = dr["Name"].ToString();
                                _racelist.Add(obj);

                            }
                            //_racelist.Insert(0, new RaceInfo() { RaceId = "0", Name = "Select" });
                            Info.raceList = _racelist;
                        }
                        catch (Exception ex)
                        {
                            clsError.WriteException(ex);
                        }
                    }

                    if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                    {
                        List<Nurse.Programdetail> Programs = new List<Nurse.Programdetail>();
                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            Nurse.Programdetail obj = new Nurse.Programdetail();
                            obj.Id = Convert.ToInt32(dr["programtypeid"]);
                            obj.Name = dr["programtype"].ToString();
                            obj.ReferenceId = dr["ReferenceId"].ToString();
                            Programs.Add(obj);
                        }
                        Info.AvailableProgram = Programs;
                    }
                    if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                    {
                        List<FamilyHousehold.ImmunizationRecord> ImmunizationRecords = new List<FamilyHousehold.ImmunizationRecord>();
                        foreach (DataRow dr in ds.Tables[5].Rows)
                        {
                            FamilyHousehold.ImmunizationRecord obj = new FamilyHousehold.ImmunizationRecord();
                            obj.Dose = dr["Immunization"].ToString();
                            obj.ImmunizationmasterId = Convert.ToInt32(dr["Immunizationid"]);
                            ImmunizationRecords.Add(obj);
                        }

                        Info.ImmunizationRecords = ImmunizationRecords;
                    }
                    if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                    {
                        List<Nurse.ChildDirectBloodRelative> ChildHealth = new List<Nurse.ChildDirectBloodRelative>();
                        foreach (DataRow dr in ds.Tables[6].Rows)
                        {
                            Nurse.ChildDirectBloodRelative obj = new Nurse.ChildDirectBloodRelative();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            ChildHealth.Add(obj);
                        }
                        Info.AvailableDisease = ChildHealth;
                    }
                    if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                    {
                        List<Nurse.ChildDiagnosedDisease> ChildDiagnosedHealth = new List<Nurse.ChildDiagnosedDisease>();
                        foreach (DataRow dr in ds.Tables[7].Rows)
                        {
                            Nurse.ChildDiagnosedDisease obj = new Nurse.ChildDiagnosedDisease();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            ChildDiagnosedHealth.Add(obj);
                        }
                        Info.AvailableDiagnosedDisease = ChildDiagnosedHealth;
                    }
                    if (ds.Tables[8] != null && ds.Tables[8].Rows.Count > 0)
                    {
                        List<Nurse.ChildDental> ChildDental = new List<Nurse.ChildDental>();
                        foreach (DataRow dr in ds.Tables[8].Rows)
                        {
                            Nurse.ChildDental obj = new Nurse.ChildDental();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            ChildDental.Add(obj);
                        }
                        Info.AvailableDental = ChildDental;
                    }

                    if (ds.Tables[9] != null && ds.Tables[9].Rows.Count > 0)
                    {
                        List<Nurse.ChildEHS> ChildEHSInfo = new List<Nurse.ChildEHS>();
                        foreach (DataRow dr in ds.Tables[9].Rows)
                        {
                            Nurse.ChildEHS obj = new Nurse.ChildEHS();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            ChildEHSInfo.Add(obj);
                        }
                        Info.AvailableEHS = ChildEHSInfo;
                    }
                    if (ds.Tables[10] != null && ds.Tables[10].Rows.Count > 0)
                    {
                        List<Nurse.ChildDietInfo> ChildDietDetails = new List<Nurse.ChildDietInfo>();
                        foreach (DataRow dr in ds.Tables[10].Rows)
                        {
                            Nurse.ChildDietInfo obj = new Nurse.ChildDietInfo();
                            obj.Id = (dr["ID"]).ToString();
                            obj.Name = dr["DietInfo"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            ChildDietDetails.Add(obj);
                        }
                        Info.dietList = ChildDietDetails;
                    }
                    if (ds.Tables[11] != null && ds.Tables[11].Rows.Count > 0)
                    {
                        List<Nurse.ChildDrink> ChildDrinkDetails = new List<Nurse.ChildDrink>();
                        foreach (DataRow dr in ds.Tables[11].Rows)
                        {
                            Nurse.ChildDrink obj = new Nurse.ChildDrink();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            ChildDrinkDetails.Add(obj);
                        }
                        Info.AvailableChildDrink = ChildDrinkDetails;
                    }
                    if (ds.Tables[12] != null && ds.Tables[12].Rows.Count > 0)
                    {
                        List<Nurse.ChildFoodInfo> ChildFoodDetails = new List<Nurse.ChildFoodInfo>();
                        foreach (DataRow dr in ds.Tables[12].Rows)
                        {
                            Nurse.ChildFoodInfo obj = new Nurse.ChildFoodInfo();
                            obj.Id = (dr["ID"]).ToString();
                            obj.Name = dr["Category"].ToString();
                            ChildFoodDetails.Add(obj);
                        }
                        Info.foodList = ChildFoodDetails;
                    }



                    if (ds.Tables[13] != null && ds.Tables[13].Rows.Count > 0)
                    {
                        List<SchoolDistrict> schooldistrict = new List<SchoolDistrict>();
                        foreach (DataRow dr in ds.Tables[13].Rows)
                        {
                            SchoolDistrict info = new SchoolDistrict();
                            info.SchoolDistrictID = Convert.ToInt32(dr["SchoolDistrictID"]);
                            info.Acronym = dr["Acronym"].ToString();
                            schooldistrict.Add(info);
                        }
                        Info.SchoolList = schooldistrict;
                    }
                    if (ds.Tables[14] != null && ds.Tables[14].Rows.Count > 0)
                    {
                        List<Nurse.ChildFeedCerealInfo> ChildCerealDetails = new List<Nurse.ChildFeedCerealInfo>();
                        foreach (DataRow dr in ds.Tables[14].Rows)
                        {
                            Nurse.ChildFeedCerealInfo obj = new Nurse.ChildFeedCerealInfo();
                            obj.Id = (dr["ID"]).ToString();
                            obj.Name = dr["Description"].ToString();
                            ChildCerealDetails.Add(obj);
                        }
                        Info.CFeedCerealList = ChildCerealDetails;
                    }
                    if (ds.Tables[15] != null && ds.Tables[15].Rows.Count > 0)
                    {
                        List<Nurse.ChildReferalCriteriaInfo> ChildReferalDetails = new List<Nurse.ChildReferalCriteriaInfo>();
                        foreach (DataRow dr in ds.Tables[15].Rows)
                        {
                            Nurse.ChildReferalCriteriaInfo obj = new Nurse.ChildReferalCriteriaInfo();
                            obj.Id = (dr["ID"]).ToString();
                            obj.Name = dr["Description"].ToString();
                            ChildReferalDetails.Add(obj);
                        }
                        Info.CReferalCriteriaList = ChildReferalDetails;
                    }
                    if (ds.Tables[16] != null && ds.Tables[16].Rows.Count > 0)
                    {
                        List<Nurse.ChildFormulaInfo> ChildFormulaDetails = new List<Nurse.ChildFormulaInfo>();
                        foreach (DataRow dr in ds.Tables[16].Rows)
                        {
                            Nurse.ChildFormulaInfo obj = new Nurse.ChildFormulaInfo();
                            obj.Id = (dr["ID"]).ToString();
                            obj.Name = dr["Description"].ToString();
                            ChildFormulaDetails.Add(obj);
                        }
                        Info.CFormulaList = ChildFormulaDetails;
                    }
                    if (ds.Tables[17] != null && ds.Tables[17].Rows.Count > 0)
                    {
                        List<Nurse.ChildFeedInfo> ChildFedDetails = new List<Nurse.ChildFeedInfo>();
                        foreach (DataRow dr in ds.Tables[17].Rows)
                        {
                            Nurse.ChildFeedInfo obj = new Nurse.ChildFeedInfo();
                            obj.Id = (dr["ID"]).ToString();
                            obj.Name = dr["Description"].ToString();
                            ChildFedDetails.Add(obj);
                        }
                        Info.CFeedList = ChildFedDetails;
                    }
                    if (ds.Tables[18] != null && ds.Tables[18].Rows.Count > 0)
                    {
                        List<Nurse.ChildDietFull> ChildDietFullDetails = new List<Nurse.ChildDietFull>();
                        foreach (DataRow dr in ds.Tables[18].Rows)
                        {
                            Nurse.ChildDietFull obj = new Nurse.ChildDietFull();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            ChildDietFullDetails.Add(obj);
                        }
                        Info.AvailableChildDietFull = ChildDietFullDetails;
                    }
                    if (ds.Tables[19] != null && ds.Tables[19].Rows.Count > 0)
                    {
                        List<Nurse.ChildVitamin> ChildVitaminDetails = new List<Nurse.ChildVitamin>();
                        foreach (DataRow dr in ds.Tables[19].Rows)
                        {
                            Nurse.ChildVitamin obj = new Nurse.ChildVitamin();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            ChildVitaminDetails.Add(obj);
                        }
                        Info.AvailableChildVitamin = ChildVitaminDetails;
                    }
                    if (ds.Tables[20] != null && ds.Tables[20].Rows.Count > 0)
                    {
                        List<Nurse.ChildHungryInfo> ChildHungryDetails = new List<Nurse.ChildHungryInfo>();
                        foreach (DataRow dr in ds.Tables[20].Rows)
                        {
                            Nurse.ChildHungryInfo obj = new Nurse.ChildHungryInfo();
                            obj.Id = (dr["ID"]).ToString();
                            obj.Name = dr["Description"].ToString();
                            ChildHungryDetails.Add(obj);
                        }
                        Info.ChungryList = ChildHungryDetails;
                    }
                    if (ds.Tables[21] != null && ds.Tables[21].Rows.Count > 0)
                    {
                        List<Nurse.PMConditionsInfo> PMCondtnDetails = new List<Nurse.PMConditionsInfo>();
                        foreach (DataRow dr in ds.Tables[21].Rows)
                        {
                            Nurse.PMConditionsInfo obj = new Nurse.PMConditionsInfo();
                            obj.Id = (dr["ID"]).ToString();
                            obj.Name = dr["Description"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            PMCondtnDetails.Add(obj);
                        }
                        Info.PMCondtnList = PMCondtnDetails;
                    }
                    if (ds.Tables[22] != null && ds.Tables[22].Rows.Count > 0)
                    {
                        List<Nurse.PMProblems> PMPrblmDetails = new List<Nurse.PMProblems>();
                        foreach (DataRow dr in ds.Tables[22].Rows)
                        {
                            Nurse.PMProblems obj = new Nurse.PMProblems();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            PMPrblmDetails.Add(obj);
                        }
                        Info.AvailablePrblms = PMPrblmDetails;
                    }
                    if (ds.Tables[23] != null && ds.Tables[23].Rows.Count > 0)
                    {
                        List<Nurse.PMService> PMServiceDetails = new List<Nurse.PMService>();
                        foreach (DataRow dr in ds.Tables[23].Rows)
                        {
                            Nurse.PMService obj = new Nurse.PMService();
                            obj.Id = Convert.ToInt32(dr["ID"]);
                            obj.Name = dr["Description"].ToString();
                            // obj.ReferenceId = dr["Description"].ToString();
                            PMServiceDetails.Add(obj);
                        }
                        Info.AvailableService = PMServiceDetails;
                    }





                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Info;
        }
        public Nurse EditFamilyInfo(string id, int yakkrid, string Agencyid, string userid, string RoleID)
        {
            Nurse obj = new Nurse();
            obj.DictionarySlotsSeats = new Dictionary<string, int>();
        
            //obj.income1 = new FamilyHousehold.calculateincome();

            try
            {
                command.Parameters.Add(new SqlParameter("@client", id));
                command.Parameters.Add(new SqlParameter("@RoleID", RoleID));
                command.Parameters.Add(new SqlParameter("@yakkrid", yakkrid));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Nursehouseholdinfo";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                obj= GetallHouseholdinfo(obj, _dataset);
                DataAdapter.Dispose();
                command.Dispose();
                return obj;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return obj;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
        }
        public Nurse GetallHouseholdinfo(Nurse obj, DataSet _dataset)
        {
            if (_dataset != null && _dataset.Tables.Count > 0)
            {
                if (_dataset.Tables[0].Rows.Count > 0)
                {
                    //house hold details
                    obj.HouseholdId = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ID"]);
                    obj.FamilyHouseholdID = Convert.ToInt32(_dataset.Tables[0].Rows[0]["familyhouseholdid"]);
                    obj.Street = Convert.ToString(_dataset.Tables[0].Rows[0]["Street"]);
                    obj.StreetName = Convert.ToString(_dataset.Tables[0].Rows[0]["StreetName"]);
                    obj.City = Convert.ToString(_dataset.Tables[0].Rows[0]["City"]);
                    obj.ZipCode = Convert.ToString(_dataset.Tables[0].Rows[0]["ZipCode"]);
                    obj.State = Convert.ToString(_dataset.Tables[0].Rows[0]["State"]);
                    obj.County = Convert.ToString(_dataset.Tables[0].Rows[0]["County"]);
                    obj.HFileName = Convert.ToString(_dataset.Tables[0].Rows[0]["FileName"]);
                    obj.HFileExtension = Convert.ToString(_dataset.Tables[0].Rows[0]["FileExtension"]);
                    if (_dataset.Tables[0].Rows[0]["Hpaper"].ToString() != "")
                        obj.AdresssverificationinPaper = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["Hpaper"]);
                    //Family householddetails
                    //
                    if (_dataset.Tables[0].Rows[0]["RentType"].ToString() != "")
                        obj.RentType = Convert.ToInt32(_dataset.Tables[0].Rows[0]["RentType"]);
                    if (_dataset.Tables[0].Rows[0]["TANF"].ToString() != "")
                        obj.TANF = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["TANF"]);
                    if (_dataset.Tables[0].Rows[0]["HouseType"].ToString() != "")
                        obj.HomeType = Convert.ToInt32(_dataset.Tables[0].Rows[0]["HouseType"]);
                    if (_dataset.Tables[0].Rows[0]["FamilyType"].ToString() != "")
                        obj.FamilyType = Convert.ToInt32(_dataset.Tables[0].Rows[0]["FamilyType"]);
                    if (_dataset.Tables[0].Rows[0]["SSI"].ToString() != "")
                        obj.SSI = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["SSI"]);
                    if (_dataset.Tables[0].Rows[0]["WIC"].ToString() != "")
                        obj.WIC = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["WIC"]);
                    if (_dataset.Tables[0].Rows[0]["SNAP"].ToString() != "")
                        obj.SNAP = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["SNAP"]);
                    if (_dataset.Tables[0].Rows[0]["NONE"].ToString() != "")
                        obj.NONE = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["NONE"]);
                    if (_dataset.Tables[0].Rows[0]["Interpretor"].ToString() != "")
                        obj.Interpretor = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Interpretor"]);
                    if (_dataset.Tables[0].Rows[0]["ParentRelatioship"].ToString() != "")

                        obj.ParentRelatioship = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ParentRelatioship"]);
                    if (_dataset.Tables[0].Rows[0]["OtherRelationship"].ToString() != "")
                        obj.ParentRelatioshipOther = _dataset.Tables[0].Rows[0]["OtherRelationship"].ToString();
                    if (_dataset.Tables[0].Rows[0]["OtherDesc"].ToString() != "")
                        obj.OtherLanguageDetail = _dataset.Tables[0].Rows[0]["OtherDesc"].ToString();
                    obj.PrimaryLanguauge = Convert.ToString(_dataset.Tables[0].Rows[0]["PrimaryLanguauge"]);
                    obj.docstorage = Convert.ToInt32(_dataset.Tables[0].Rows[0]["docsStorage"]);
                    //Parent1 details
                    obj.ExistPmprogram = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Pmprogram"]);
                    obj.Pfirstname = _dataset.Tables[0].Rows[0]["Pfirstname"].ToString();
                    obj.Plastname = _dataset.Tables[0].Rows[0]["PLastName"].ToString();
                    obj.Pmidddlename = _dataset.Tables[0].Rows[0]["PMiddlename"].ToString();
                    if (_dataset.Tables[0].Rows[0]["PDOB"].ToString() != "")
                        obj.PDOB = Convert.ToDateTime(_dataset.Tables[0].Rows[0]["PDOB"]).ToString("MM/dd/yyyy");
                    obj.PGender = _dataset.Tables[0].Rows[0]["PGender"].ToString();
                    obj.Pemailid = _dataset.Tables[0].Rows[0]["PEmailid"].ToString();
                    if (_dataset.Tables[0].Rows[0]["PMilitaryStatus"].ToString() != "")
                        obj.PMilitaryStatus = Convert.ToInt32(_dataset.Tables[0].Rows[0]["PMilitaryStatus"]);
                    obj.PEnrollment = _dataset.Tables[0].Rows[0]["PEnrollment"].ToString();
                    if (_dataset.Tables[0].Rows[0]["PDegreeEarned"].ToString() != "")
                        obj.PDegreeEarned = Convert.ToString(_dataset.Tables[0].Rows[0]["PDegreeEarned"]);
                    obj.Pnotesother = _dataset.Tables[0].Rows[0]["PNotes"].ToString();
                    obj.PRole = _dataset.Tables[0].Rows[0]["ParentRole"].ToString();
                    if (_dataset.Tables[0].Rows[0]["PCurrentlyWorking"].ToString() != "")
                        obj.PCurrentlyWorking = _dataset.Tables[0].Rows[0]["PCurrentlyWorking"].ToString();
                    if (_dataset.Tables[0].Rows[0]["ParentId"].ToString() != "")
                        obj.ParentID = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ParentId"]);
                    obj.HealthReview = Convert.ToInt32(_dataset.Tables[0].Rows[0]["HealthReview"]);




                    //Changes
                    if (_dataset.Tables[0].Rows[0]["ParentOriginalId"].ToString() != "")
                        obj.ParentOriginalId = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ParentOriginalId"]);
                    //End
                    if (_dataset.Tables[0].Rows[0]["IsPreg"].ToString() != "")
                    {
                        obj.PQuestion = _dataset.Tables[0].Rows[0]["IsPreg"].ToString();
                        //if (obj.PQuestion == "1")
                        //    obj.ChildId = Convert.ToInt32(obj.ParentID);
                    }
                       
                    if (_dataset.Tables[0].Rows[0]["EnrollforPregnant"].ToString() != "")
                        obj.Pregnantmotherenrolled = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["EnrollforPregnant"]);
                    if (_dataset.Tables[0].Rows[0]["motherinsurance"].ToString() != "")
                        obj.Pregnantmotherprimaryinsurance = Convert.ToInt32(_dataset.Tables[0].Rows[0]["motherinsurance"]);
                    if (_dataset.Tables[0].Rows[0]["insurancenotemother"].ToString() != "")
                        obj.Pregnantmotherprimaryinsurancenotes = _dataset.Tables[0].Rows[0]["insurancenotemother"].ToString();
                    if (_dataset.Tables[0].Rows[0]["TrimesterEnrolled"].ToString() != "")
                        obj.TrimesterEnrolled = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TrimesterEnrolled"]);
                    if (_dataset.Tables[0].Rows[0]["PMVisitDoc"].ToString() != "")
                        obj.PMVisitDoc = Convert.ToString(_dataset.Tables[0].Rows[0]["PMVisitDoc"]);
                    //if (_dataset.Tables[0].Rows[0]["PMIssues"].ToString() != "")//pm issues
                    //obj.PMOtherIssues = _dataset.Tables[0].Rows[0]["PMIssues"].ToString();



                    if (_dataset.Tables[0].Rows[0]["PMConditionID"].ToString() != "")
                        obj.PMConditions = Convert.ToString(_dataset.Tables[0].Rows[0]["PMConditionID"]);
                    if (_dataset.Tables[0].Rows[0]["PMConditionDescID"].ToString() != "")
                        obj.PMCondtnDesc = Convert.ToString(_dataset.Tables[0].Rows[0]["PMConditionDescID"]);
                    if (_dataset.Tables[0].Rows[0]["PMDentalExam"].ToString() != "")
                        obj.PMDentalExam = Convert.ToInt32(_dataset.Tables[0].Rows[0]["PMDentalExam"]);
                    if (_dataset.Tables[0].Rows[0]["PMRisk"].ToString() != "")
                        obj.PMRisk = Convert.ToInt32(_dataset.Tables[0].Rows[0]["PMRisk"]);
                    if (_dataset.Tables[0].Rows[0]["PMNeedDental"].ToString() != "")
                        obj.PMNeedDental = Convert.ToInt32(_dataset.Tables[0].Rows[0]["PMNeedDental"]);
                    if (_dataset.Tables[0].Rows[0]["PMRecieveDental"].ToString() != "")
                        obj.PMRecieveDental = Convert.ToInt32(_dataset.Tables[0].Rows[0]["PMRecieveDental"]);
                    obj.ParentSSN1 = _dataset.Tables[0].Rows[0]["SSN"].ToString() == "" ? "" : EncryptDecrypt.Decrypt(_dataset.Tables[0].Rows[0]["SSN"].ToString());
                    if (_dataset.Tables[0].Rows[0]["Medicalhome"].ToString() != "")
                        obj.MedicalhomePreg1 = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Medicalhome"]);
                    if (_dataset.Tables[0].Rows[0]["Doctorvalue"].ToString() != "")
                        obj.P1Doctor = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Doctorvalue"]);
                    obj.CDoctorP1 = _dataset.Tables[0].Rows[0]["doctorname"].ToString();
                    if (_dataset.Tables[0].Rows[0]["NoEmail"].ToString() != "")
                        obj.Noemail1 = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["NoEmail"]);
                    obj.PImagejson = _dataset.Tables[0].Rows[0]["PProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])_dataset.Tables[0].Rows[0]["PProfilePic"]);
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        List<FamilyHousehold.calculateincome> IncomeList = new List<FamilyHousehold.calculateincome>();

                        DataRow[] dr = _dataset.Tables[1].Select("ParentId=" + Convert.ToInt32(_dataset.Tables[0].Rows[0]["ParentId"]));
                        foreach (DataRow dr1 in dr)
                        {
                            FamilyHousehold.calculateincome _income = new FamilyHousehold.calculateincome();
                            _income.newincomeid = Convert.ToInt32(dr1["IncomeId"]);

                            if (dr1["Income"].ToString() != "")
                                _income.Income = Convert.ToInt32(dr1["Income"]);
                            if (dr1["IncomeSource1"].ToString() != "")
                                _income.IncomeSource1 = dr1["IncomeSource1"].ToString();
                            //if (dr1["IncomeSource2"].ToString() != "")
                            //    _income.IncomeSource2 = dr1["IncomeSource2"].ToString();
                            //if (dr1["IncomeSource3"].ToString() != "")
                            //    _income.IncomeSource3 = dr1["IncomeSource3"].ToString();
                            //if (dr1["IncomeSource4"].ToString() != "")
                            //    _income.IncomeSource4 = dr1["IncomeSource4"].ToString();
                            if (dr1["IncomeAmt1"].ToString() != "")
                                _income.AmountVocher1 = Convert.ToDecimal(dr1["IncomeAmt1"]);
                            if (dr1["IncomeAmt2"].ToString() != "")
                                _income.AmountVocher2 = Convert.ToDecimal(dr1["IncomeAmt2"]);
                            if (dr1["IncomeAmt3"].ToString() != "")
                                _income.AmountVocher3 = Convert.ToDecimal(dr1["IncomeAmt3"]);
                            if (dr1["IncomeAmt4"].ToString() != "")
                                _income.AmountVocher4 = Convert.ToDecimal(dr1["IncomeAmt4"]);
                            if (dr1["PayFrequency"].ToString() != "")
                                _income.Payfrequency = Convert.ToInt32(dr1["PayFrequency"]);
                            if (dr1["WorkingPeriod"].ToString() != "")
                                _income.Working = Convert.ToInt32(dr1["WorkingPeriod"]);
                            if (dr1["TotalIncome"].ToString() != "")
                                _income.IncomeCalculated = Convert.ToDecimal(dr1["TotalIncome"]);
                            if (dr1["Image1"].ToString() != "")
                                _income.SalaryAvatar1bytes = (byte[])dr1["Image1"];
                            if (dr1["Image2"].ToString() != "")
                                _income.SalaryAvatar2bytes = (byte[])dr1["Image2"];
                            if (dr1["Image3"].ToString() != "")
                                _income.SalaryAvatar3bytes = (byte[])dr1["Image3"];
                            if (dr1["Image4"].ToString() != "")
                                _income.SalaryAvatar1bytes = (byte[])dr1["Image4"];
                            if (dr1["Image1FileName"].ToString() != "")
                                _income.SalaryAvatarFilename1 = dr1["Image1FileName"].ToString();
                            if (dr1["Image2FileName"].ToString() != "")
                                _income.SalaryAvatarFilename2 = dr1["Image2FileName"].ToString();
                            if (dr1["Image3FileName"].ToString() != "")
                                _income.SalaryAvatarFilename3 = dr1["Image3FileName"].ToString();
                            if (dr1["Image4FileName"].ToString() != "")
                                _income.SalaryAvatarFilename4 = dr1["Image4FileName"].ToString();
                            if (dr1["Image1FileExt"].ToString() != "")
                                _income.SalaryAvatarFileExtension1 = dr1["Image1FileExt"].ToString();
                            if (dr1["Image2FileExt"].ToString() != "")
                                _income.SalaryAvatarFileExtension2 = dr1["Image2FileExt"].ToString();
                            if (dr1["Image3FileExt"].ToString() != "")
                                _income.SalaryAvatarFileExtension3 = dr1["Image3FileExt"].ToString();
                            if (dr1["Image4FileExt"].ToString() != "")
                                _income.SalaryAvatarFileExtension4 = dr1["Image4FileExt"].ToString();
                            if (dr1["NoIncomeImage"].ToString() != "")
                                _income.NoIncomeAvatarbytes = (byte[])dr1["NoIncomeImage"];
                            if (dr1["NoIncomeImageFileName"].ToString() != "")
                                _income.NoIncomeFilename4 = dr1["NoIncomeImageFileName"].ToString();
                            if (dr1["NoIncomeImageFileExt"].ToString() != "")
                                _income.NoIncomeFileExtension4 = dr1["NoIncomeImageFileExt"].ToString();
                            if (dr1["IncomePaper1"].ToString() != "")
                                _income.incomePaper1 = Convert.ToBoolean(dr1["IncomePaper1"]);
                            if (dr1["IncomePaper2"].ToString() != "")
                                _income.incomePaper2 = Convert.ToBoolean(dr1["IncomePaper2"]);
                            if (dr1["IncomePaper3"].ToString() != "")
                                _income.incomePaper3 = Convert.ToBoolean(dr1["IncomePaper3"]);
                            if (dr1["IncomePaper4"].ToString() != "")
                                _income.incomePaper4 = Convert.ToBoolean(dr1["IncomePaper4"]);
                            if (dr1["NoincomePaper"].ToString() != "")
                                _income.noincomepaper = Convert.ToBoolean(dr1["NoincomePaper"]);

                            IncomeList.Add(_income);
                        }
                        obj.FamilyHousehold.Income1 = IncomeList;
                    }
                }

                //Parent2 detail
                if (_dataset.Tables[0].Rows.Count == 2)
                {
                    obj.P1firstname = _dataset.Tables[0].Rows[1]["Pfirstname"].ToString();
                    obj.P1lastname = _dataset.Tables[0].Rows[1]["PLastName"].ToString();
                    obj.P1midddlename = _dataset.Tables[0].Rows[1]["PMiddlename"].ToString();
                    obj.P1DOB = _dataset.Tables[0].Rows[1]["PDOB"].ToString() == "" ? "" : Convert.ToDateTime(_dataset.Tables[0].Rows[1]["PDOB"]).ToString("MM/dd/yyyy");
                    if (_dataset.Tables[0].Rows[1]["PGender"].ToString() != "")
                        obj.P1Gender = Convert.ToInt32(_dataset.Tables[0].Rows[1]["PGender"]);
                    obj.P1emailid = _dataset.Tables[0].Rows[1]["PEmailid"].ToString();
                    if (_dataset.Tables[0].Rows[1]["PMilitaryStatus"].ToString() != "")
                        obj.P1MilitaryStatus = Convert.ToInt32(_dataset.Tables[0].Rows[1]["PMilitaryStatus"]);
                    if (_dataset.Tables[0].Rows[1]["PEnrollment"].ToString() != "")
                        obj.P1Enrollment = Convert.ToString(_dataset.Tables[0].Rows[1]["PEnrollment"]);
                    if (_dataset.Tables[0].Rows[1]["PDegreeEarned"].ToString() != "")
                        obj.P1DegreeEarned = Convert.ToString(_dataset.Tables[0].Rows[1]["PDegreeEarned"]);
                    if (_dataset.Tables[0].Rows[1]["PCurrentlyWorking"].ToString() != "")
                        obj.P1CurrentlyWorking = _dataset.Tables[0].Rows[1]["PCurrentlyWorking"].ToString();
                    obj.P1Role = _dataset.Tables[0].Rows[1]["ParentRole"].ToString();
                    obj.P1notesother = _dataset.Tables[0].Rows[1]["PNotes"].ToString();
                    if (_dataset.Tables[0].Rows[1]["ParentId"].ToString() != "")
                        obj.ParentID1 = Convert.ToInt32(_dataset.Tables[0].Rows[1]["ParentId"]);
                    if (_dataset.Tables[0].Rows[1]["IsPreg"].ToString() != "")
                        obj.P1Question = _dataset.Tables[0].Rows[1]["IsPreg"].ToString();
                    if (_dataset.Tables[0].Rows[1]["EnrollforPregnant"].ToString() != "")
                        obj.PregnantmotherenrolledP1 = Convert.ToBoolean(_dataset.Tables[0].Rows[1]["EnrollforPregnant"]);
                    if (_dataset.Tables[0].Rows[1]["motherinsurance"].ToString() != "")
                        obj.Pregnantmotherprimaryinsurance1 = Convert.ToInt32(_dataset.Tables[0].Rows[1]["motherinsurance"]);
                    if (_dataset.Tables[0].Rows[1]["insurancenotemother"].ToString() != "")
                        obj.Pregnantmotherprimaryinsurancenotes1 = _dataset.Tables[0].Rows[1]["insurancenotemother"].ToString();
                    if (_dataset.Tables[0].Rows[1]["TrimesterEnrolled"].ToString() != "")
                        obj.TrimesterEnrolled1 = Convert.ToInt32(_dataset.Tables[0].Rows[1]["TrimesterEnrolled"]);
                    obj.ParentSSN2 = _dataset.Tables[0].Rows[1]["SSN"].ToString() == "" ? "" : EncryptDecrypt.Decrypt(_dataset.Tables[0].Rows[1]["SSN"].ToString());
                    if (_dataset.Tables[0].Rows[1]["Medicalhome"].ToString() != "")
                        obj.MedicalhomePreg2 = Convert.ToInt32(_dataset.Tables[0].Rows[1]["Medicalhome"]);
                    if (_dataset.Tables[0].Rows[1]["Doctorvalue"].ToString() != "")
                        obj.P2Doctor = Convert.ToInt32(_dataset.Tables[0].Rows[1]["Doctorvalue"]);
                    obj.CDoctorP2 = _dataset.Tables[0].Rows[1]["doctorname"].ToString();
                    if (_dataset.Tables[0].Rows[1]["NoEmail"].ToString() != "")
                        obj.Noemail2 = Convert.ToBoolean(_dataset.Tables[0].Rows[1]["NoEmail"]);
                    if (obj.HealthReview == 0 || obj.HealthReview == 2)
                        obj.HealthReview = Convert.ToInt32(_dataset.Tables[0].Rows[1]["HealthReview"]);
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        List<FamilyHousehold.calculateincome1> IncomeList = new List<FamilyHousehold.calculateincome1>();

                        DataRow[] dr = _dataset.Tables[1].Select("ParentId=" + Convert.ToInt32(_dataset.Tables[0].Rows[1]["ParentId"]));
                        foreach (DataRow dr1 in dr)
                        {
                            FamilyHousehold.calculateincome1 _income = new FamilyHousehold.calculateincome1();
                            _income.IncomeID = Convert.ToInt32(dr1["IncomeId"]);
                            if (dr1["Income"].ToString() != "")
                                _income.Income = Convert.ToInt32(dr1["Income"]);
                            if (dr1["IncomeSource1"].ToString() != "")
                                _income.IncomeSource1 = dr1["IncomeSource1"].ToString();
                            //if (dr1["IncomeSource2"].ToString() != "")
                            //    _income.IncomeSource2 = dr1["IncomeSource2"].ToString();
                            //if (dr1["IncomeSource3"].ToString() != "")
                            //    _income.IncomeSource3 = dr1["IncomeSource3"].ToString();
                            //if (dr1["IncomeSource4"].ToString() != "")
                            //    _income.IncomeSource4 = dr1["IncomeSource4"].ToString();
                            if (dr1["IncomeAmt1"].ToString() != "")
                                _income.AmountVocher1 = Convert.ToDecimal(dr1["IncomeAmt1"]);
                            if (dr1["IncomeAmt2"].ToString() != "")
                                _income.AmountVocher2 = Convert.ToDecimal(dr1["IncomeAmt2"]);
                            if (dr1["IncomeAmt3"].ToString() != "")
                                _income.AmountVocher3 = Convert.ToDecimal(dr1["IncomeAmt3"]);
                            if (dr1["IncomeAmt4"].ToString() != "")
                                _income.AmountVocher4 = Convert.ToDecimal(dr1["IncomeAmt4"]);
                            if (dr1["PayFrequency"].ToString() != "")
                                _income.Payfrequency = Convert.ToInt32(dr1["PayFrequency"]);
                            if (dr1["WorkingPeriod"].ToString() != "")
                                _income.Working = Convert.ToInt32(dr1["WorkingPeriod"]);
                            if (dr1["TotalIncome"].ToString() != "")
                                _income.IncomeCalculated = Convert.ToDecimal(dr1["TotalIncome"]);
                            if (dr1["Image1"].ToString() != "")
                                _income.SalaryAvatar1bytes = (byte[])dr1["Image1"];
                            if (dr1["Image2"].ToString() != "")
                                _income.SalaryAvatar2bytes = (byte[])dr1["Image2"];
                            if (dr1["Image3"].ToString() != "")
                                _income.SalaryAvatar3bytes = (byte[])dr1["Image3"];
                            if (dr1["Image4"].ToString() != "")
                                _income.SalaryAvatar1bytes = (byte[])dr1["Image4"];
                            if (dr1["Image1FileName"].ToString() != "")
                                _income.SalaryAvatarFilename1 = dr1["Image1FileName"].ToString();
                            if (dr1["Image2FileName"].ToString() != "")
                                _income.SalaryAvatarFilename2 = dr1["Image2FileName"].ToString();
                            if (dr1["Image3FileName"].ToString() != "")
                                _income.SalaryAvatarFilename3 = dr1["Image3FileName"].ToString();
                            if (dr1["Image4FileName"].ToString() != "")
                                _income.SalaryAvatarFilename4 = dr1["Image4FileName"].ToString();
                            if (dr1["Image1FileExt"].ToString() != "")
                                _income.SalaryAvatarFileExtension1 = dr1["Image1FileExt"].ToString();
                            if (dr1["Image2FileExt"].ToString() != "")
                                _income.SalaryAvatarFileExtension2 = dr1["Image2FileExt"].ToString();
                            if (dr1["Image3FileExt"].ToString() != "")
                                _income.SalaryAvatarFileExtension3 = dr1["Image3FileExt"].ToString();
                            if (dr1["Image4FileExt"].ToString() != "")
                                _income.SalaryAvatarFileExtension4 = dr1["Image4FileExt"].ToString();
                            if (dr1["NoIncomeImage"].ToString() != "")
                                _income.NoIncomeAvatarbytes = (byte[])dr1["NoIncomeImage"];
                            if (dr1["NoIncomeImageFileName"].ToString() != "")
                                _income.NoIncomeFilename4 = dr1["NoIncomeImageFileName"].ToString();
                            if (dr1["NoIncomeImageFileExt"].ToString() != "")
                                _income.NoIncomeFileExtension4 = dr1["NoIncomeImageFileExt"].ToString();
                            if (dr1["IncomePaper1"].ToString() != "")
                                _income.incomePaper1 = Convert.ToBoolean(dr1["IncomePaper1"]);
                            if (dr1["IncomePaper2"].ToString() != "")
                                _income.incomePaper2 = Convert.ToBoolean(dr1["IncomePaper2"]);
                            if (dr1["IncomePaper3"].ToString() != "")
                                _income.incomePaper3 = Convert.ToBoolean(dr1["IncomePaper3"]);
                            if (dr1["IncomePaper4"].ToString() != "")
                                _income.incomePaper4 = Convert.ToBoolean(dr1["IncomePaper4"]);
                            if (dr1["NoincomePaper"].ToString() != "")
                                _income.noincomepaper = Convert.ToBoolean(dr1["NoincomePaper"]);

                            IncomeList.Add(_income);
                        }
                        obj.FamilyHousehold.Income2 = IncomeList;

                    }
                }
                if (_dataset.Tables[2].Rows.Count > 0)
                {
                    List<Nurse.Qualifier> Qualifier = new List<Nurse.Qualifier>();
                    Nurse.Qualifier _Qualifier = null;
                    foreach (DataRow dr in _dataset.Tables[2].Rows)
                    {
                        _Qualifier = new Nurse.Qualifier();
                        _Qualifier.Name = dr["name"].ToString();
                        _Qualifier.Programtype = dr["ProgramType"].ToString();
                        _Qualifier.Dob = dr["DOB"].ToString() == "" ? "" : Convert.ToDateTime(dr["DOB"]).ToString("MM/dd/yyyy");
                        _Qualifier.clientid = Convert.ToInt32(dr["clientid"]);
                        _Qualifier.Programid = Convert.ToInt32(dr["programid"]);
                        _Qualifier.PovertyPercentage = dr["PovertyPercentage"].ToString();
                        _Qualifier.Selectionpoint = dr["Selectionpoint"].ToString();
                        _Qualifier.ClientType = dr["Clienttype"].ToString();
                        obj.totalhousehold = Convert.ToInt32(dr["totalhousehold"]);
                        _Qualifier.IsAccepted = dr["IsAccepted"].ToString() == "" ? false : Convert.ToBoolean(dr["IsAccepted"]);
                        Qualifier.Add(_Qualifier);
                        _Qualifier = null;
                    }
                    obj.QualifierRecords = Qualifier;
                }


                if (_dataset.Tables[3] != null && _dataset.Tables[3].Rows.Count > 0)
                {
                    List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                    foreach (DataRow dr in _dataset.Tables[3].Rows)
                    {
                        HrCenterInfo info = new HrCenterInfo();
                        info.CenterId = dr["center"].ToString();
                        info.Name = dr["centername"].ToString();
                        info.Address = dr["address"].ToString();
                        info.Zip = dr["Zip"].ToString();
                        info.SeatsAvailable = dr["AvailSeats"].ToString();
                        centerList.Add(info);
                    }
                    obj.HrcenterList = centerList;
                }

                if (_dataset.Tables[5].Rows.Count > 0)
                {
                    //house hold details
                    obj.Cfirstname = Convert.ToString(_dataset.Tables[5].Rows[0]["Firstname"]);
                    obj.Clastname = Convert.ToString(_dataset.Tables[5].Rows[0]["Lastname"]);
                    obj.Cmiddlename = Convert.ToString(_dataset.Tables[5].Rows[0]["Middlename"]);
                    obj.CDOB = Convert.ToDateTime(_dataset.Tables[5].Rows[0]["DOB"]).ToString("MM/dd/yyyy");
                    obj.CSSN = _dataset.Tables[5].Rows[0]["SSN"].ToString() == "" ? "" : EncryptDecrypt.Decrypt(_dataset.Tables[5].Rows[0]["SSN"].ToString());
                    obj.CRace = _dataset.Tables[5].Rows[0]["RaceID"].ToString();
                    obj.CRaceSubCategory = _dataset.Tables[5].Rows[0]["RaceSubCategoryID"].ToString();
                    if (_dataset.Tables[5].Rows[0]["DentalHome"].ToString() != "")
                        obj.CDentalhome = Convert.ToInt32(_dataset.Tables[5].Rows[0]["DentalHome"].ToString());
                    obj.ChildId = Convert.ToInt32(_dataset.Tables[5].Rows[0]["ID"]);

                    obj.DOBverifiedBy = _dataset.Tables[5].Rows[0]["Dobverifiedby"].ToString();
                    obj.CRace = _dataset.Tables[5].Rows[0]["RaceID"].ToString();
                    if (_dataset.Tables[5].Rows[0]["Ethnicity"].ToString() != "")
                        obj.CEthnicity = Convert.ToInt32(_dataset.Tables[5].Rows[0]["Ethnicity"]);
                    obj.CRaceSubCategory = _dataset.Tables[5].Rows[0]["RaceSubCategoryID"].ToString();
                    if (_dataset.Tables[5].Rows[0]["SchoolDistrict"].ToString() != "")
                        obj.SchoolDistrict = Convert.ToInt32(_dataset.Tables[5].Rows[0]["SchoolDistrict"]);
                    if (_dataset.Tables[5].Rows[0]["Medicalhome"].ToString() != "")
                        obj.Medicalhome = Convert.ToInt32(_dataset.Tables[5].Rows[0]["Medicalhome"]);
                    if (_dataset.Tables[5].Rows[0]["DentalHome"].ToString() != "")
                        obj.CDentalhome = Convert.ToInt32(_dataset.Tables[5].Rows[0]["DentalHome"]);
                    if (_dataset.Tables[5].Rows[0]["PrimaryInsurance"].ToString() != "")
                        obj.InsuranceOption = _dataset.Tables[5].Rows[0]["PrimaryInsurance"].ToString();
                    if (_dataset.Tables[5].Rows[0]["ImmunizationServiceType"].ToString() != "")
                        obj.ImmunizationService = Convert.ToInt32(_dataset.Tables[5].Rows[0]["ImmunizationServiceType"]);
                    if (_dataset.Tables[5].Rows[0]["BMIStatus"].ToString() != "")
                        obj.BMIStatus = Convert.ToInt32(_dataset.Tables[5].Rows[0]["BMIStatus"]);


                    obj.HWInput = _dataset.Tables[5].Rows[0]["HWInput"].ToString();
                    obj.AssessmentDate = _dataset.Tables[5].Rows[0]["AssessmentDate"].ToString() != "" ? Convert.ToDateTime(_dataset.Tables[5].Rows[0]["AssessmentDate"]).ToString("MM/dd/yyyy") : "";
                    obj.AHeight = _dataset.Tables[5].Rows[0]["BHeight"].ToString();
                    obj.AWeight = _dataset.Tables[5].Rows[0]["BWeight"].ToString();
                    obj.HeadCircle = _dataset.Tables[5].Rows[0]["HeadCirc"].ToString();





                    obj.CGender = Convert.ToString(_dataset.Tables[5].Rows[0]["Gender"]);
                    obj.Imagejson = _dataset.Tables[5].Rows[0]["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])_dataset.Tables[5].Rows[0]["ProfilePic"]);
                    if (_dataset.Tables[5].Rows[0]["ImmunizationServiceType"].ToString() != "")
                        obj.ImmunizationService = Convert.ToInt32(_dataset.Tables[5].Rows[0]["ImmunizationServiceType"]);
                    if (_dataset.Tables[5].Rows[0]["MedicalService"].ToString() != "")
                        obj.MedicalService = Convert.ToInt32(_dataset.Tables[5].Rows[0]["MedicalService"]);
                    if (_dataset.Tables[5].Rows[0]["Medicalhome"].ToString() != "")
                        obj.Medicalhome = Convert.ToInt32(_dataset.Tables[5].Rows[0]["Medicalhome"]);
                    if (_dataset.Tables[5].Rows[0]["ParentDisable"].ToString() != "")
                        obj.CParentdisable = Convert.ToInt32(_dataset.Tables[5].Rows[0]["ParentDisable"]);
                    if (_dataset.Tables[5].Rows[0]["BMIStatus"].ToString() != "")
                        obj.BMIStatus = Convert.ToInt32(_dataset.Tables[5].Rows[0]["BMIStatus"]);
                    if (_dataset.Tables[5].Rows[0]["DentalHome"].ToString() != "")
                        obj.CDentalhome = Convert.ToInt32(_dataset.Tables[5].Rows[0]["DentalHome"]);
                    if (_dataset.Tables[5].Rows[0]["Ethnicity"].ToString() != "")
                        obj.CEthnicity = Convert.ToInt32(_dataset.Tables[5].Rows[0]["Ethnicity"]);
                    obj.CDoctor = _dataset.Tables[5].Rows[0]["doctorname"].ToString();
                    obj.CDentist = _dataset.Tables[5].Rows[0]["dentistname"].ToString();
                    if (_dataset.Tables[5].Rows[0]["Doctorvalue"].ToString() != "")
                        obj.Doctor = Convert.ToInt32(_dataset.Tables[5].Rows[0]["Doctorvalue"]);
                    if (_dataset.Tables[5].Rows[0]["Dentistvalue"].ToString() != "")
                        obj.Dentist = Convert.ToInt32(_dataset.Tables[5].Rows[0]["Dentistvalue"]);
                    if (_dataset.Tables[5].Rows[0]["SchoolDistrict"].ToString() != "")
                        obj.SchoolDistrict = Convert.ToInt32(_dataset.Tables[5].Rows[0]["SchoolDistrict"]);
                    if (_dataset.Tables[5].Rows[0]["DobPaper"].ToString() != "")
                        obj.DobverificationinPaper = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["DobPaper"]);
                    if (_dataset.Tables[5].Rows[0]["FosterChild"].ToString() != "")
                        obj.IsFoster = Convert.ToInt32(_dataset.Tables[5].Rows[0]["FosterChild"]);
                    if (_dataset.Tables[5].Rows[0]["WelfareAgency"].ToString() != "")
                        obj.Inwalfareagency = Convert.ToInt32(_dataset.Tables[5].Rows[0]["WelfareAgency"]);
                    if (_dataset.Tables[5].Rows[0]["DualCustodyChild"].ToString() != "")
                        obj.InDualcustody = Convert.ToInt32(_dataset.Tables[5].Rows[0]["DualCustodyChild"]);
                    if (_dataset.Tables[5].Rows[0]["PrimaryInsurance"].ToString() != "")
                        obj.InsuranceOption = _dataset.Tables[5].Rows[0]["PrimaryInsurance"].ToString();
                    if (_dataset.Tables[5].Rows[0]["InsuranceNotes"].ToString() != "")
                        obj.MedicalNote = _dataset.Tables[5].Rows[0]["InsuranceNotes"].ToString();
                    if (_dataset.Tables[5].Rows[0]["ImmunizationinPaper"].ToString() != "")
                        obj.ImmunizationinPaper = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["ImmunizationinPaper"]);
                    if (_dataset.Tables[5].Rows[0]["CHQuestionID"].ToString() != "")
                        obj.HealthQuesId = Convert.ToInt32(_dataset.Tables[5].Rows[0]["CHQuestionID"]);
                    if (_dataset.Tables[5].Rows[0]["NutriQuesID"].ToString() != "")
                        obj.NutritionQuesId = Convert.ToInt32(_dataset.Tables[5].Rows[0]["NutriQuesID"]);
                    obj.HealthReview = Convert.ToInt32(_dataset.Tables[5].Rows[0]["HealthReview"]);
                    obj.EHSAllergy = _dataset.Tables[5].Rows[0]["EHSAllergy"].ToString();
                    obj.EHSEpiPen = Convert.ToInt32(_dataset.Tables[5].Rows[0]["EHSEpiPen"]);
                    //Commented on 21Dec2016
                    //if (_dataset.Tables[5].Rows[0]["CurrentNausea"].ToString() != "")
                    //    obj.PersistentNausea = Convert.ToString(_dataset.Tables[5].Rows[0]["CurrentNausea"]);
                    //if (_dataset.Tables[5].Rows[0]["CurrentConstipation"].ToString() != "")
                    //    obj.PersistentConstipation = Convert.ToString(_dataset.Tables[5].Rows[0]["CurrentConstipation"]);
                    //if (_dataset.Tables[5].Rows[0]["Currentdiarrhea"].ToString() != "")
                    //    obj.PersistentDiarrhea = Convert.ToString(_dataset.Tables[5].Rows[0]["Currentdiarrhea"]);
                    //if (_dataset.Tables[5].Rows[0]["weightchange"].ToString() != "")
                    //    obj.DramaticWeight = Convert.ToString(_dataset.Tables[5].Rows[0]["weightchange"]);
                    //if (_dataset.Tables[5].Rows[0]["Recentsurgery"].ToString() != "")
                    //    obj.RecentSurgery = Convert.ToString(_dataset.Tables[5].Rows[0]["Recentsurgery"]);
                    //if (_dataset.Tables[5].Rows[0]["Recenthospitalization"].ToString() != "")
                    //    obj.RecentHospitalization = Convert.ToString(_dataset.Tables[5].Rows[0]["Recenthospitalization"]);
                    //if (_dataset.Tables[5].Rows[0]["specialdiet"].ToString() != "")
                    //    obj.ChildSpecialDiet = Convert.ToString(_dataset.Tables[5].Rows[0]["specialdiet"]);
                    //if (_dataset.Tables[5].Rows[0]["foodallergies"].ToString() != "")
                    //    obj.FoodAllergies = Convert.ToString(_dataset.Tables[5].Rows[0]["foodallergies"]);
                    //if (_dataset.Tables[5].Rows[0]["nutritionalconcerns"].ToString() != "")
                    //    obj.NutritionalConcern = Convert.ToString(_dataset.Tables[5].Rows[0]["nutritionalconcerns"]);
                    //if (_dataset.Tables[5].Rows[0]["WICNutrition"].ToString() != "")
                    //    obj.WICNutrition = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["WICNutrition"]);
                    //if (_dataset.Tables[5].Rows[0]["FoodStamps"].ToString() != "")
                    //    obj.FoodStamps = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["FoodStamps"]);
                    //if (_dataset.Tables[5].Rows[0]["NoNutritionProg"].ToString() != "")
                    //    obj.NoNutritionProg = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["NoNutritionProg"]);
                    //if (_dataset.Tables[5].Rows[0]["foodpantry"].ToString() != "")
                    //    obj.FoodPantory = Convert.ToString(_dataset.Tables[5].Rows[0]["foodpantry"]);
                    //if (_dataset.Tables[5].Rows[0]["troublechewing"].ToString() != "")
                    //    obj.childTrouble = Convert.ToString(_dataset.Tables[5].Rows[0]["troublechewing"]);
                    //if (_dataset.Tables[5].Rows[0]["childusespoon"].ToString() != "")
                    //    obj.spoon = Convert.ToString(_dataset.Tables[5].Rows[0]["childusespoon"]);
                    //if (_dataset.Tables[5].Rows[0]["childusefeedingtube"].ToString() != "")
                    //    obj.feedingtube = Convert.ToString(_dataset.Tables[5].Rows[0]["childusefeedingtube"]);
                    //if (_dataset.Tables[5].Rows[0]["childhealth"].ToString() != "")
                    //    obj.childThin = Convert.ToString(_dataset.Tables[5].Rows[0]["childhealth"]);
                    //if (_dataset.Tables[5].Rows[0]["childtakebottle"].ToString() != "")
                    //    obj.Takebottle = Convert.ToString(_dataset.Tables[5].Rows[0]["childtakebottle"]);
                    //if (_dataset.Tables[5].Rows[0]["Childeatchew"].ToString() != "")
                    //    obj.chewanything = Convert.ToString(_dataset.Tables[5].Rows[0]["Childeatchew"]);
                    //if (_dataset.Tables[5].Rows[0]["childappetite"].ToString() != "")
                    //    obj.ChangeinAppetite = Convert.ToString(_dataset.Tables[5].Rows[0]["childappetite"]);
                    //if (_dataset.Tables[5].Rows[0]["childhungry"].ToString() != "")
                    //    obj.ChildHungry = Convert.ToString(_dataset.Tables[5].Rows[0]["childhungry"]);
                    //if (_dataset.Tables[5].Rows[0]["ChildFeed"].ToString() != "")
                    //    obj.ChildFeed = Convert.ToString(_dataset.Tables[5].Rows[0]["ChildFeed"]);
                    //if (_dataset.Tables[5].Rows[0]["ChildFormula"].ToString() != "")
                    //    obj.ChildFormula = Convert.ToString(_dataset.Tables[5].Rows[0]["ChildFormula"]);
                    //if (_dataset.Tables[5].Rows[0]["childmashedfoods"].ToString() != "")
                    //    obj.ChildFeedMarshfood = Convert.ToString(_dataset.Tables[5].Rows[0]["childmashedfoods"]);
                    //if (_dataset.Tables[5].Rows[0]["childchoppedfoods"].ToString() != "")
                    //    obj.ChildFeedChopedfood = Convert.ToString(_dataset.Tables[5].Rows[0]["childchoppedfoods"]);
                    //if (_dataset.Tables[5].Rows[0]["childfingerfoods"].ToString() != "")
                    //    obj.ChildFingerFood = Convert.ToString(_dataset.Tables[5].Rows[0]["childfingerfoods"]);
                    //if (_dataset.Tables[5].Rows[0]["childfedfingerfoods"].ToString() != "")
                    //    obj.ChildFingerFEDFood = Convert.ToString(_dataset.Tables[5].Rows[0]["childfedfingerfoods"]);
                    //if (_dataset.Tables[5].Rows[0]["childcereal"].ToString() != "")
                    //    obj.ChildFeedCereal = Convert.ToString(_dataset.Tables[5].Rows[0]["childcereal"]);
                    //if (_dataset.Tables[5].Rows[0]["childfruitjiuce"].ToString() != "")
                    //    obj.ChildFruitJuice = Convert.ToString(_dataset.Tables[5].Rows[0]["childfruitjiuce"]);
                    //if (_dataset.Tables[5].Rows[0]["childfruitjiuce"].ToString() != "")
                    //    obj.ChildFedJuice = Convert.ToString(_dataset.Tables[5].Rows[0]["childfruitjiuce"]);
                    //if (_dataset.Tables[5].Rows[0]["childfedVitamin"].ToString() != "")
                    //    obj.ChildFruitJuicevitaminc = Convert.ToString(_dataset.Tables[5].Rows[0]["childfedVitamin"]);
                    //if (_dataset.Tables[5].Rows[0]["childdrinkwater"].ToString() != "")
                    //    obj.ChildWater = Convert.ToString(_dataset.Tables[5].Rows[0]["childdrinkwater"]);
                    //if (_dataset.Tables[5].Rows[0]["Breakfast"].ToString() != "")
                    //    obj.Breakfast = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Breakfast"]);
                    //if (_dataset.Tables[5].Rows[0]["Lunch"].ToString() != "")
                    //    obj.Lunch = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Lunch"]);
                    //if (_dataset.Tables[5].Rows[0]["Snack"].ToString() != "")
                    //    obj.Snack = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Snack"]);
                    //if (_dataset.Tables[5].Rows[0]["Dinner"].ToString() != "")
                    //    obj.Dinner = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Dinner"]);
                    //if (_dataset.Tables[5].Rows[0]["NA"].ToString() != "")
                    //    obj.NA = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["NA"]);
                    //if (_dataset.Tables[5].Rows[0]["CriteriaforReferral"].ToString() != "")
                    //    obj.ChildReferalCriteria = Convert.ToString(_dataset.Tables[5].Rows[0]["CriteriaforReferral"]);
                    //obj.NauseaorVomitingcomment = _dataset.Tables[5].Rows[0]["NauseaorVomitingcomment"].ToString();
                    //obj.DiarrheaComment = _dataset.Tables[5].Rows[0]["DiarrheaComment"].ToString();
                    //obj.Constipationcomment = _dataset.Tables[5].Rows[0]["Constipationcomment"].ToString();
                    //obj.DramaticWeightchangecomment = _dataset.Tables[5].Rows[0]["DramaticWeightchangecomment"].ToString();
                    //obj.RecentSurgerycomment = _dataset.Tables[5].Rows[0]["RecentSurgerycomment"].ToString();
                    //obj.RecentHospitalizationComment = _dataset.Tables[5].Rows[0]["RecentHospitalizationComment"].ToString();
                    //obj.SpecialDietComment = _dataset.Tables[5].Rows[0]["SpecialDietComment"].ToString();
                    //obj.FoodAllergiesComment = _dataset.Tables[5].Rows[0]["FoodAllergiesComment"].ToString();
                    //obj.NutritionAlconcernsComment = _dataset.Tables[5].Rows[0]["NutritionAlconcernsComment"].ToString();
                    //obj.ChewingorSwallowingcomment = _dataset.Tables[5].Rows[0]["ChewingorSwallowingcomment"].ToString();
                    //obj.SpoonorForkComment = _dataset.Tables[5].Rows[0]["SpoonorForkComment"].ToString();
                    //obj.SpecialFeedingComment = _dataset.Tables[5].Rows[0]["SpecialFeedingComment"].ToString();
                    //obj.BottleComment = _dataset.Tables[5].Rows[0]["BottleComment"].ToString();
                    //obj.EatOrChewComment = _dataset.Tables[5].Rows[0]["EatOrChewComment"].ToString();



                    obj.EHSBabyOrMotherProblems = _dataset.Tables[5].Rows[0]["BabyMotherProblemComment"].ToString();
                    obj.HSBabyOrMotherProblems = _dataset.Tables[5].Rows[0]["BabyMotherProblemComment"].ToString();
                    obj.HsMedicationName = _dataset.Tables[5].Rows[0]["HsMedicationName"].ToString();
                    obj.HsDosage = _dataset.Tables[5].Rows[0]["HsDosage"].ToString();
                    obj.HSChildMedication = _dataset.Tables[5].Rows[0]["MedicationComment"].ToString();
                    obj.HSPreventativeDentalCare = _dataset.Tables[5].Rows[0]["PreventativeDentalCareComment"].ToString();
                    obj.HSProfessionalDentalExam = _dataset.Tables[5].Rows[0]["ProfessionalDentalExamComment"].ToString();
                    obj.HSNeedingDentalTreatment = _dataset.Tables[5].Rows[0]["DiagnosedDentalTreatmentComment"].ToString();
                    obj.HSChildReceivedDentalTreatment = _dataset.Tables[5].Rows[0]["ChildReceivedDentalTreatmentComment"].ToString();






                    obj.ImmunizationFileName = _dataset.Tables[5].Rows[0]["ImmunizationFileName"].ToString();
                    obj.Raceother = _dataset.Tables[5].Rows[0]["OtherRace"].ToString();
                    obj.IsFutureIntake = Convert.ToString(_dataset.Tables[5].Rows[0]["IsFutureIntake"]);


                }
                if (_dataset.Tables[6].Rows.Count > 0)
                {
                    List<Nurse.Programdetail> ProgramdetailRecords = new List<Nurse.Programdetail>();
                    Nurse.Programdetail obj1;
                    foreach (DataRow dr in _dataset.Tables[6].Rows)
                    {
                        obj1 = new Nurse.Programdetail();
                        obj1.Id = Convert.ToInt32(dr["programid"]);
                        obj1.ReferenceId = dr["ReferenceId"].ToString();
                        obj1.Name = dr["ProgramType"].ToString();
                        obj1.IsSelected = true;
                        //   obj.CProgramType = Convert.ToString(obj1.Id);
                        ProgramdetailRecords.Add(obj1);
                    }
                    obj.AvailableProgram = ProgramdetailRecords;
                    // obj.CProgramType = Convert.ToString(obj.AvailableProgram);
                    // obj.PostedPostedPrograms.ProgramID = ProgramdetailRecords;
                    if (obj.AvailableProgram.Count > 0)
                    {
                        foreach (var item1 in obj.AvailableProgram)
                        {
                            // string ReferenceID = item1.ReferenceId;
                            if (item1.ReferenceId == "1")//EHS
                            {
                                obj.ChildReferenceProgramID = item1.ReferenceId;
                            }
                            if (item1.ReferenceId == "2")//HS
                            {
                                obj.ChildReferenceProgramID = item1.ReferenceId;
                            }
                        }
                    }//end
                }
                if (obj.ChildReferenceProgramID == "1")//EHS
                {
                    if (_dataset.Tables[5].Rows[0]["ChildBirthWt"].ToString() != "")
                        obj.EhsChildBirthWt = Convert.ToString(_dataset.Tables[5].Rows[0]["ChildBirthWt"]);
                    if (_dataset.Tables[5].Rows[0]["ChildBorn"].ToString() != "")
                        obj.EhsChildBorn = Convert.ToString(_dataset.Tables[5].Rows[0]["ChildBorn"]);
                    if (_dataset.Tables[5].Rows[0]["ChildLength"].ToString() != "")
                        obj.EhsChildLength = Convert.ToString(_dataset.Tables[5].Rows[0]["ChildLength"]);
                    if (_dataset.Tables[5].Rows[0]["ChildPrblm"].ToString() != "")
                        obj.EhsChildProblm = Convert.ToInt32(_dataset.Tables[5].Rows[0]["ChildPrblm"]);
                    if (_dataset.Tables[5].Rows[0]["Medication"].ToString() != "")
                        obj.EhsMedication = Convert.ToString(_dataset.Tables[5].Rows[0]["Medication"]);
                    if (_dataset.Tables[5].Rows[0]["M_PCarePlan"].ToString() != "")
                        obj.EHSmpplan = Convert.ToString(_dataset.Tables[5].Rows[0]["M_PCarePlan"]);
                    if (_dataset.Tables[5].Rows[0]["Comment"].ToString() != "")
                        obj.EhsComment = Convert.ToString(_dataset.Tables[5].Rows[0]["Comment"]);
                    if (_dataset.Tables[5].Rows[0]["MedicationName"].ToString() != "")
                        obj.EhsMedicationName = Convert.ToString(_dataset.Tables[5].Rows[0]["MedicationName"]);
                    if (_dataset.Tables[5].Rows[0]["Dosage"].ToString() != "")
                        obj.EhsDosage = Convert.ToString(_dataset.Tables[5].Rows[0]["Dosage"]);
                    if (_dataset.Tables[5].Rows[0]["DentalCare"].ToString() != "")
                        obj.EhsChildDentalCare = Convert.ToString(_dataset.Tables[5].Rows[0]["DentalCare"]);
                    if (_dataset.Tables[5].Rows[0]["CurrentDentalexam"].ToString() != "")
                        obj.EhsDentalExam = Convert.ToString(_dataset.Tables[5].Rows[0]["CurrentDentalexam"]);
                    if (_dataset.Tables[5].Rows[0]["RecentDentalExam"].ToString() != "")
                        obj.EhsRecentDentalExam = Convert.ToString(_dataset.Tables[5].Rows[0]["RecentDentalExam"]);
                    if (_dataset.Tables[5].Rows[0]["NeedDentalTreatment"].ToString() != "")
                        obj.EhsChildNeedDentalTreatment = Convert.ToString(_dataset.Tables[5].Rows[0]["NeedDentalTreatment"]);
                    if (_dataset.Tables[5].Rows[0]["RecievedDentalTreatment"].ToString() != "")
                        obj.EhsChildRecievedDentalTreatment = Convert.ToString(_dataset.Tables[5].Rows[0]["RecievedDentalTreatment"]);


                    //Ehs Nutrition Qusetion
                    obj.EhsRestrictFood = _dataset.Tables[5].Rows[0]["ChildRestrictFood"].ToString();
                    obj.EhsChildVitaminSupplment = _dataset.Tables[5].Rows[0]["ChildVitaminSupplement"].ToString();
                    obj.EhsPersistentNausea = _dataset.Tables[5].Rows[0]["CurrentNausea"].ToString();
                    obj.EhsPersistentDiarrhea = _dataset.Tables[5].Rows[0]["Currentdiarrhea"].ToString();
                    obj.EhsPersistentConstipation = _dataset.Tables[5].Rows[0]["CurrentConstipation"].ToString();
                    obj.EhsDramaticWeight = _dataset.Tables[5].Rows[0]["weightchange"].ToString();
                    obj.EhsRecentSurgery = _dataset.Tables[5].Rows[0]["Recentsurgery"].ToString();
                    obj.EhsChildSpecialDiet = _dataset.Tables[5].Rows[0]["specialdiet"].ToString();
                    obj.EhsFoodAllergies = _dataset.Tables[5].Rows[0]["foodallergies"].ToString();
                    obj.EhsNutritionalConcern = _dataset.Tables[5].Rows[0]["nutritionalconcerns"].ToString();
                    obj.EhsNutritionalConcern = _dataset.Tables[5].Rows[0]["nutritionalconcerns"].ToString();
                    obj.EhsRecentHospitalization = _dataset.Tables[5].Rows[0]["Recenthospitalization"].ToString();
                    //if (_dataset.Tables[0].Rows[0]["WICNutrition"].ToString() != "")
                    //    obj.EhsWICNutrition = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["WICNutrition"]);
                    //if (_dataset.Tables[0].Rows[0]["FoodStamps"].ToString() != "")
                    //    obj.EhsFoodStamps = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["FoodStamps"]);
                    //if (_dataset.Tables[0].Rows[0]["NoNutritionProg"].ToString() != "")
                    //    obj.NoNutritionProg = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["NoNutritionProg"]);
                    obj.EhsFoodPantory = _dataset.Tables[5].Rows[0]["foodpantry"].ToString();
                    obj.EhschildTrouble = _dataset.Tables[5].Rows[0]["troublechewing"].ToString();
                    //obj.EhsChildFormula = _dataset.Tables[0].Rows[0]["ChildFormula"].ToString();
                    obj.Ehsspoon = _dataset.Tables[5].Rows[0]["childusespoon"].ToString();
                    obj.Ehsfeedingtube = _dataset.Tables[5].Rows[0]["childusefeedingtube"].ToString();
                    obj.EhschildThin = Convert.ToInt32(_dataset.Tables[5].Rows[0]["childhealth"].ToString());
                    obj.EhsTakebottle = _dataset.Tables[5].Rows[0]["childtakebottle"].ToString();
                    obj.Ehschewanything = _dataset.Tables[5].Rows[0]["Childeatchew"].ToString();
                    obj.EhsChangeinAppetite = _dataset.Tables[5].Rows[0]["childappetite"].ToString();
                    obj.EhsChildHungry = _dataset.Tables[5].Rows[0]["childhungry"].ToString();
                    obj.ChildFeed = _dataset.Tables[5].Rows[0]["ChildFeed"].ToString();
                    obj.ChildFeedCereal = _dataset.Tables[5].Rows[0]["childcereal"].ToString();
                    obj.ChildFeedMarshfood = _dataset.Tables[5].Rows[0]["childmashedfoods"].ToString();
                    obj.ChildFeedChopedfood = _dataset.Tables[5].Rows[0]["childchoppedfoods"].ToString();
                    obj.ChildFingerFood = _dataset.Tables[5].Rows[0]["childfingerfoods"].ToString();
                    obj.ChildFingerFEDFood = _dataset.Tables[5].Rows[0]["childfedfingerfoods"].ToString();
                    obj.ChildFruitJuice = _dataset.Tables[5].Rows[0]["childfruitjiuce"].ToString();
                    obj.EhsChildFruitJuicevitaminc = _dataset.Tables[5].Rows[0]["childfedVitamin"].ToString();
                    obj.EhsChildWater = _dataset.Tables[5].Rows[0]["childdrinkwater"].ToString();
                    if (_dataset.Tables[5].Rows[0]["Breakfast"].ToString() != "")
                        obj.EhsBreakfast = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Breakfast"]);
                    if (_dataset.Tables[5].Rows[0]["lunch"].ToString() != "")
                        obj.EhsLunch = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["lunch"]);
                    if (_dataset.Tables[5].Rows[0]["Snack"].ToString() != "")
                        obj.EhsSnack = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Snack"]);
                    if (_dataset.Tables[5].Rows[0]["Dinner"].ToString() != "")
                        obj.EhsDinner = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Dinner"]);
                    if (_dataset.Tables[5].Rows[0]["NA"].ToString() != "")
                        obj.EhsNA = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["NA"]);
                    // obj.ChildReferalCriteria = _dataset.Tables[0].Rows[0]["CriteriaforReferral"].ToString();
                    obj.EhsNauseaorVomitingcomment = _dataset.Tables[5].Rows[0]["NauseaorVomitingcomment"].ToString();
                    obj.EhsDiarrheaComment = _dataset.Tables[5].Rows[0]["DiarrheaComment"].ToString();
                    obj.EhsConstipationcomment = _dataset.Tables[5].Rows[0]["Constipationcomment"].ToString();
                    obj.EhsDramaticWeightchangecomment = _dataset.Tables[5].Rows[0]["DramaticWeightchangecomment"].ToString();
                    obj.EhsRecentSurgerycomment = _dataset.Tables[5].Rows[0]["RecentSurgerycomment"].ToString();
                    obj.EhsRecentHospitalizationComment = _dataset.Tables[5].Rows[0]["RecentHospitalizationComment"].ToString();
                    obj.EhsSpecialDietComment = _dataset.Tables[5].Rows[0]["SpecialDietComment"].ToString();
                    obj.EhsFoodAllergiesComment = _dataset.Tables[5].Rows[0]["FoodAllergiesComment"].ToString();
                    obj.EhsNutritionAlconcernsComment = _dataset.Tables[5].Rows[0]["NutritionAlconcernsComment"].ToString();
                    obj.EhsChewingorSwallowingcomment = _dataset.Tables[5].Rows[0]["ChewingorSwallowingcomment"].ToString();
                    obj.EhsSpoonorForkComment = _dataset.Tables[5].Rows[0]["SpoonorForkComment"].ToString();
                    obj.EhsSpecialFeedingComment = _dataset.Tables[5].Rows[0]["SpecialFeedingComment"].ToString();
                    obj.EhsBottleComment = _dataset.Tables[5].Rows[0]["BottleComment"].ToString();
                    obj.EhsEatOrChewComment = _dataset.Tables[5].Rows[0]["EatOrChewComment"].ToString();




                }
                if (obj.ChildReferenceProgramID == "2")//HS
                {
                    if (_dataset.Tables[5].Rows[0]["ChildBirthWt"].ToString() != "")
                        obj.HsChildBirthWt = Convert.ToString(_dataset.Tables[5].Rows[0]["ChildBirthWt"]);
                    if (_dataset.Tables[5].Rows[0]["ChildBorn"].ToString() != "")
                        obj.HsChildBorn = Convert.ToString(_dataset.Tables[5].Rows[0]["ChildBorn"]);
                    if (_dataset.Tables[5].Rows[0]["ChildLength"].ToString() != "")
                        obj.HsChildLength = Convert.ToString(_dataset.Tables[5].Rows[0]["ChildLength"]);
                    if (_dataset.Tables[5].Rows[0]["ChildPrblm"].ToString() != "")
                        obj.HsChildProblm = Convert.ToInt32(_dataset.Tables[5].Rows[0]["ChildPrblm"]);
                    if (_dataset.Tables[5].Rows[0]["Medication"].ToString() != "")
                        obj.HsMedication = Convert.ToString(_dataset.Tables[5].Rows[0]["Medication"]);
                    if (_dataset.Tables[5].Rows[0]["M_PCarePlan"].ToString() != "")
                        obj.HSmpplan = Convert.ToString(_dataset.Tables[5].Rows[0]["M_PCarePlan"]);
                    if (_dataset.Tables[5].Rows[0]["Comment"].ToString() != "")
                        obj.HsComment = Convert.ToString(_dataset.Tables[5].Rows[0]["Comment"]);
                    if (_dataset.Tables[5].Rows[0]["MedicationName"].ToString() != "")
                        obj.HsMedicationName = Convert.ToString(_dataset.Tables[5].Rows[0]["MedicationName"]);
                    if (_dataset.Tables[5].Rows[0]["Dosage"].ToString() != "")
                        obj.HsDosage = Convert.ToString(_dataset.Tables[5].Rows[0]["Dosage"]);
                    if (_dataset.Tables[5].Rows[0]["DentalCare"].ToString() != "")
                        obj.HsChildDentalCare = Convert.ToString(_dataset.Tables[5].Rows[0]["DentalCare"]);
                    if (_dataset.Tables[5].Rows[0]["CurrentDentalexam"].ToString() != "")
                        obj.HsDentalExam = Convert.ToString(_dataset.Tables[5].Rows[0]["CurrentDentalexam"]);
                    if (_dataset.Tables[5].Rows[0]["RecentDentalExam"].ToString() != "")
                        obj.HsRecentDentalExam = Convert.ToString(_dataset.Tables[5].Rows[0]["RecentDentalExam"]);
                    if (_dataset.Tables[5].Rows[0]["NeedDentalTreatment"].ToString() != "")
                        obj.HsChildNeedDentalTreatment = Convert.ToString(_dataset.Tables[5].Rows[0]["NeedDentalTreatment"]);
                    if (_dataset.Tables[5].Rows[0]["RecievedDentalTreatment"].ToString() != "")
                        obj.HsChildRecievedDentalTreatment = Convert.ToString(_dataset.Tables[5].Rows[0]["RecievedDentalTreatment"]);
                    if (_dataset.Tables[5].Rows[0]["ChildEverHadProfExam"].ToString() != "")
                        obj.ChildProfessionalDentalExam = _dataset.Tables[5].Rows[0]["ChildEverHadProfExam"].ToString();

                    //HS Nutrition Questions
                    obj.RestrictFood = _dataset.Tables[5].Rows[0]["ChildRestrictFood"].ToString();
                    obj.ChildVitaminSupplment = _dataset.Tables[5].Rows[0]["ChildVitaminSupplement"].ToString();
                    obj.PersistentNausea = _dataset.Tables[5].Rows[0]["CurrentNausea"].ToString();
                    obj.PersistentDiarrhea = _dataset.Tables[5].Rows[0]["Currentdiarrhea"].ToString();
                    obj.PersistentConstipation = _dataset.Tables[5].Rows[0]["CurrentConstipation"].ToString();
                    obj.DramaticWeight = _dataset.Tables[5].Rows[0]["weightchange"].ToString();
                    obj.RecentSurgery = _dataset.Tables[5].Rows[0]["Recentsurgery"].ToString();
                    obj.ChildSpecialDiet = _dataset.Tables[5].Rows[0]["specialdiet"].ToString();
                    obj.FoodAllergies = _dataset.Tables[5].Rows[0]["foodallergies"].ToString();
                    obj.NutritionalConcern = _dataset.Tables[5].Rows[0]["nutritionalconcerns"].ToString();
                    obj.NutritionalConcern = _dataset.Tables[5].Rows[0]["nutritionalconcerns"].ToString();
                    obj.RecentHospitalization = _dataset.Tables[5].Rows[0]["Recenthospitalization"].ToString();
                    if (_dataset.Tables[5].Rows[0]["WICNutrition"].ToString() != "")
                        obj.WICNutrition = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["WICNutrition"]);
                    if (_dataset.Tables[5].Rows[0]["FoodStamps"].ToString() != "")
                        obj.FoodStamps = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["FoodStamps"]);
                    if (_dataset.Tables[5].Rows[0]["NoNutritionProg"].ToString() != "")
                        obj.NoNutritionProg = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["NoNutritionProg"]);
                    obj.FoodPantory = _dataset.Tables[5].Rows[0]["foodpantry"].ToString();
                    obj.childTrouble = _dataset.Tables[5].Rows[0]["troublechewing"].ToString();
                    obj.ChildFormula = _dataset.Tables[5].Rows[0]["ChildFormula"].ToString();
                    obj.spoon = _dataset.Tables[5].Rows[0]["childusespoon"].ToString();
                    obj.feedingtube = _dataset.Tables[5].Rows[0]["childusefeedingtube"].ToString();
                    obj.childThin = _dataset.Tables[5].Rows[0]["childhealth"].ToString();
                    obj.Takebottle = _dataset.Tables[5].Rows[0]["childtakebottle"].ToString();
                    obj.chewanything = _dataset.Tables[5].Rows[0]["Childeatchew"].ToString();
                    obj.ChangeinAppetite = _dataset.Tables[5].Rows[0]["childappetite"].ToString();
                    obj.ChildHungry = _dataset.Tables[5].Rows[0]["childhungry"].ToString();
                    obj.ChildFeed = _dataset.Tables[5].Rows[0]["ChildFeed"].ToString();
                    obj.ChildFeedCereal = _dataset.Tables[5].Rows[0]["childcereal"].ToString();
                    obj.ChildFeedMarshfood = _dataset.Tables[5].Rows[0]["childmashedfoods"].ToString();
                    obj.ChildFeedChopedfood = _dataset.Tables[5].Rows[0]["childchoppedfoods"].ToString();
                    obj.ChildFingerFood = _dataset.Tables[5].Rows[0]["childfingerfoods"].ToString();
                    obj.ChildFingerFEDFood = _dataset.Tables[5].Rows[0]["childfedfingerfoods"].ToString();
                    obj.ChildFruitJuice = _dataset.Tables[5].Rows[0]["childfruitjiuce"].ToString();
                    obj.ChildFruitJuicevitaminc = _dataset.Tables[5].Rows[0]["childfedVitamin"].ToString();
                    obj.ChildWater = _dataset.Tables[5].Rows[0]["childdrinkwater"].ToString();
                    if (_dataset.Tables[5].Rows[0]["Breakfast"].ToString() != "")
                        obj.Breakfast = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Breakfast"]);
                    if (_dataset.Tables[5].Rows[0]["lunch"].ToString() != "")
                        obj.Lunch = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["lunch"]);
                    if (_dataset.Tables[5].Rows[0]["Snack"].ToString() != "")
                        obj.Snack = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Snack"]);
                    if (_dataset.Tables[5].Rows[0]["Dinner"].ToString() != "")
                        obj.Dinner = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["Dinner"]);
                    if (_dataset.Tables[5].Rows[0]["NA"].ToString() != "")
                        obj.NA = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["NA"]);
                    obj.ChildReferalCriteria = _dataset.Tables[5].Rows[0]["CriteriaforReferral"].ToString();
                    obj.NauseaorVomitingcomment = _dataset.Tables[5].Rows[0]["NauseaorVomitingcomment"].ToString();
                    obj.DiarrheaComment = _dataset.Tables[5].Rows[0]["DiarrheaComment"].ToString();
                    obj.Constipationcomment = _dataset.Tables[5].Rows[0]["Constipationcomment"].ToString();
                    obj.DramaticWeightchangecomment = _dataset.Tables[5].Rows[0]["DramaticWeightchangecomment"].ToString();
                    obj.RecentSurgerycomment = _dataset.Tables[5].Rows[0]["RecentSurgerycomment"].ToString();
                    obj.RecentHospitalizationComment = _dataset.Tables[5].Rows[0]["RecentHospitalizationComment"].ToString();
                    obj.SpecialDietComment = _dataset.Tables[5].Rows[0]["SpecialDietComment"].ToString();
                    obj.FoodAllergiesComment = _dataset.Tables[5].Rows[0]["FoodAllergiesComment"].ToString();
                    obj.NutritionAlconcernsComment = _dataset.Tables[5].Rows[0]["NutritionAlconcernsComment"].ToString();
                    obj.ChewingorSwallowingcomment = _dataset.Tables[5].Rows[0]["ChewingorSwallowingcomment"].ToString();
                    obj.SpoonorForkComment = _dataset.Tables[5].Rows[0]["SpoonorForkComment"].ToString();
                    obj.SpecialFeedingComment = _dataset.Tables[5].Rows[0]["SpecialFeedingComment"].ToString();
                    obj.BottleComment = _dataset.Tables[5].Rows[0]["BottleComment"].ToString();
                    obj.EatOrChewComment = _dataset.Tables[5].Rows[0]["EatOrChewComment"].ToString();
                    //End
                }

                if (_dataset.Tables[7].Rows.Count > 0)
                {
                    Screening _Screening = new Screening();
                    //Screening Parent Approval
                    if (_dataset.Tables[9].Rows.Count > 0)
                    {
                        if (_dataset.Tables[9].Rows[0]["ID"].ToString() != "")
                            _Screening.ParentAppID = Convert.ToInt32(_dataset.Tables[9].Rows[0]["ID"]);
                        //if (_dataset.Tables[9].Rows[0]["IsAccepted"].ToString() != "")
                        //    obj.AcceptApplicant = Convert.ToString(_dataset.Tables[8].Rows[0]["IsAccepted"]);
                    }

                    obj._Screening = _Screening;
                }

                //}


                if (_dataset.Tables[8].Rows.Count > 0)
                {
                    if (_dataset.Tables[8].Rows[0]["Notes"].ToString() != "")
                        obj.RejectDesc = Convert.ToString(_dataset.Tables[8].Rows[0]["Notes"]);
                    if (_dataset.Tables[8].Rows[0]["IsAccepted"].ToString() != "")
                        obj.AcceptApplicant = Convert.ToString(_dataset.Tables[8].Rows[0]["IsAccepted"]);
                }
                if (_dataset.Tables[10].Rows.Count > 0)
                {


                    List<Nurse.Childhealthnutrition> _childhealthnutrition = new List<Nurse.Childhealthnutrition>();
                    Nurse.Childhealthnutrition info = null;
                    foreach (DataRow dr in _dataset.Tables[10].Rows)
                    {
                        info = new Nurse.Childhealthnutrition();
                        info.Id = dr["ID"].ToString();
                        info.MasterId = dr["ChildRecieveTreatment"].ToString();
                        info.Description = dr["Description"].ToString();
                        info.Questionid = dr["Questionid"].ToString();
                        info.Programid = dr["Programid"].ToString();
                        _childhealthnutrition.Add(info);
                    }
                    obj._Childhealthnutrition = _childhealthnutrition;
                }
                if (_dataset.Tables[11] != null && _dataset.Tables[11].Rows.Count > 0)
                {
                    List<Nurse.PMproblemandservices> _PMproblemandservicesList = new List<Nurse.PMproblemandservices>();
                    Nurse.PMproblemandservices info = null;
                    foreach (DataRow dr in _dataset.Tables[11].Rows)
                    {
                        info = new Nurse.PMproblemandservices();
                        info.Id = dr["ID"].ToString();
                        info.MasterId = dr["PMPrblmID"].ToString();
                        info.Description = dr["PmDecription"].ToString();
                        info.Parentid = dr["ParentID"].ToString();
                        _PMproblemandservicesList.Add(info);
                    }
                    obj._PMproblem = _PMproblemandservicesList;
                }
                if (_dataset.Tables[12] != null && _dataset.Tables[12].Rows.Count > 0)
                {
                    List<Nurse.PMproblemandservices> _PMproblemandservicesList = new List<Nurse.PMproblemandservices>();
                    Nurse.PMproblemandservices info = null;
                    foreach (DataRow dr in _dataset.Tables[12].Rows)
                    {
                        info = new Nurse.PMproblemandservices();
                        info.Id = dr["ID"].ToString();
                        info.MasterId = dr["PMServiceID"].ToString();
                        info.Description = dr["PMDescription"].ToString();
                        info.Parentid = dr["ParentID"].ToString();
                        _PMproblemandservicesList.Add(info);
                    }
                    obj._PMservices = _PMproblemandservicesList;
                }
                if (_dataset.Tables[15] != null && _dataset.Tables[15].Rows.Count > 0)
                {
                    obj.customscreening = _dataset.Tables[15];
                }
                if ( _dataset.Tables[18]!=null && _dataset.Tables[18].Rows.Count > 0)
                {
                    if (_dataset.Tables[18].Rows[0]["isallowincome"].ToString() != "")
                        obj.IsAllowIncome = Convert.ToBoolean(_dataset.Tables[18].Rows[0]["isallowincome"]);
                    if (_dataset.Tables[18].Rows[0]["IsFinalReviwer"].ToString() != "")
                        obj.IsFinalReviwer = Convert.ToString(_dataset.Tables[18].Rows[0]["IsFinalReviwer"]);

                }

                //if (_dataset.Tables.Count > 19 && _dataset.Tables[19] != null && _dataset.Tables[19].Rows.Count > 0)
                //{
                //    obj.DictionarySlotsSeats.Add("TotalSlots", Convert.ToInt32(_dataset.Tables[19].Rows[0]["SlotPurchased"]));
                //    obj.DictionarySlotsSeats.Add("ClientsReturningAgency", Convert.ToInt32(_dataset.Tables[19].Rows[0]["ClientsReturningAgency"]));
                //    obj.DictionarySlotsSeats.Add("ClientsReturningCenter", Convert.ToInt32(_dataset.Tables[19].Rows[0]["ClientsReturningCenter"]));
                //    obj.DictionarySlotsSeats.Add("AvailableSeats", Convert.ToInt32(_dataset.Tables[19].Rows[0]["AvailableSeats"]));
                //    obj.DictionarySlotsSeats.Add("OpenSeats", Convert.ToInt32(_dataset.Tables[19].Rows[0]["OpenSeats"]));
                //    obj.DictionarySlotsSeats.Add("CenterID", Convert.ToInt32(_dataset.Tables[19].Rows[0]["CenterID"]));
                //}
                //else
                //{

                //    obj.DictionarySlotsSeats.Add("TotalSlots", 0);
                //    obj.DictionarySlotsSeats.Add("ClientsReturningAgency", 0);
                //    obj.DictionarySlotsSeats.Add("ClientsReturningCenter", 0);
                //    obj.DictionarySlotsSeats.Add("AvailableSeats", 0);
                //    obj.DictionarySlotsSeats.Add("OpenSeats", 0);
                //    obj.DictionarySlotsSeats.Add("CenterID", 0);
                //}
                //obj.CenterList = new List<Center>();
                //if (_dataset.Tables.Count > 20 && _dataset.Tables[20] != null && _dataset.Tables[20].Rows.Count > 0)
                //{
                //    obj.CenterList = (from DataRow dr in _dataset.Tables[20].Rows
                //                      select new Center
                //                      {
                //                          CenterId = Convert.ToInt32(dr["CenterID"]),
                //                          CenterName = Convert.ToString(dr["CenterName"])
                //                      }
                //                    ).ToList();
                //}



                if (_dataset.Tables.Count > 19 && _dataset.Tables[19] != null && _dataset.Tables[19].Rows.Count > 0)
                {   //parent1 income documents

                    var _docList = new List<FamilyHousehold.IncomeDocument>();
                    //var _tempIncome1 = obj.Income1;
                    foreach (DataRow dr in _dataset.Tables[19].Rows)
                    {
                        var _docIncomeId = dr["IncomeId"] != DBNull.Value ? Convert.ToInt32(dr["IncomeId"].ToString()) : 0;
                        var _docid = dr["DocumentId"] != DBNull.Value ? Convert.ToInt32(dr["DocumentId"].ToString()) : 0;
                        var _docName = dr["DocumentName"].ToString(); //for others only
                        for (int i = 0; i < obj.FamilyHousehold.Income1.Count; i++)
                        {
                            if (_docIncomeId == obj.FamilyHousehold.Income1[i].newincomeid)
                            {
                                if (obj.FamilyHousehold.Income1[i].Documents == null)
                                {
                                    obj.FamilyHousehold.Income1[i].Documents = new List<FamilyHousehold.IncomeDocument>();
                                }
                                obj.FamilyHousehold.Income1[i].Documents.Add(new FamilyHousehold.IncomeDocument()
                                {
                                    DocumentId = _docid,
                                    DocumentName = _docName,
                                    //  ParentName =obj.Pfirstname + ' '+ obj.Plastname
                                });
                            }


                        }
                    }
                }



                //parent2 income documents
                if (_dataset.Tables.Count > 20 && _dataset.Tables[20] != null && _dataset.Tables[20].Rows.Count > 0)
                {

                    var _docList = new List<FamilyHousehold.IncomeDocument>();
                    //var _tempIncome1 = obj.Income1;
                    foreach (DataRow dr in _dataset.Tables[20].Rows)
                    {
                        var _docIncomeId = dr["IncomeId"] != DBNull.Value ? Convert.ToInt32(dr["IncomeId"].ToString()) : 0;
                        var _docid = dr["DocumentId"] != DBNull.Value ? Convert.ToInt32(dr["DocumentId"].ToString()) : 0;
                        var _docName = dr["DocumentName"].ToString(); //for others only
                        for (int i = 0; i < obj.FamilyHousehold.Income2.Count; i++)
                        {
                            if (_docIncomeId == obj.Income2[i].IncomeID)
                            {
                                if (obj.FamilyHousehold.Income2[i].Documents == null)
                                {
                                    obj.FamilyHousehold.Income2[i].Documents = new List<FamilyHousehold.IncomeDocument>();
                                }
                                obj.FamilyHousehold.Income2[i].Documents.Add(new FamilyHousehold.IncomeDocument()
                                {
                                    DocumentId = _docid,
                                    DocumentName = _docName,
                                    // ParentName = obj.Pfirstname + ' ' + obj.Plastname
                                });
                            }


                        }
                    }

                }



            }
            return obj;


        }

        public List<Nurse.Parentphone1> PhoneDetails(string householdid, string Parentid, string Agencyid)
        {
            List<Nurse.Parentphone1> _Parentphone1phone = new List<Nurse.Parentphone1>();
            try
            {
                command.Parameters.Add(new SqlParameter("@householdid", householdid));
                command.Parameters.Add(new SqlParameter("@Parentid", Parentid));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_phonelist";
                DataAdapter = new SqlDataAdapter(command);
                familydataTable = new DataTable();
                DataAdapter.Fill(familydataTable);
                if (familydataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in familydataTable.Rows)
                    {
                        Nurse.Parentphone1 _phoneadd = new Nurse.Parentphone1();
                        _phoneadd.PPhoneId = Convert.ToInt32(row["Id"]);
                        _phoneadd.PhoneTypeP = row["PhoneType"].ToString();
                        _phoneadd.phonenoP = row["Phoneno"].ToString();
                        _phoneadd.StateP = row["IsPrimaryContact"].ToString() == "" ? false : Convert.ToBoolean(row["IsPrimaryContact"]);
                        _phoneadd.SmsP = row["Sms"].ToString() == "" ? false : Convert.ToBoolean(row["Sms"]);
                        _phoneadd.notesP = row["Notes"].ToString();
                        _Parentphone1phone.Add(_phoneadd);
                    }
                }
                return _Parentphone1phone;
            }
            catch (Exception ex)
            {
                //  totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _Parentphone1phone;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                familydataTable.Dispose();
            }
        }
        public string addParentInfo(Nurse obj, int mode, Guid ID, string agencyid)
        {
            string result = string.Empty;
            try
            {
                string HouseholdId = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                if (mode == 1 && obj.Pregnantmotherenrolled == true)
                {
                    command.Parameters.Add(new SqlParameter("@ParentID", obj.ParentOriginalId));
                    command.Parameters.Add(new SqlParameter("@HouseholdId", obj.HouseholdId));
                    command.Parameters.Add(new SqlParameter("@ClientID", obj.ParentID));
                    command.Parameters.Add(new SqlParameter("@PQuestion", obj.PQuestion));
                    command.Parameters.Add(new SqlParameter("@PMQuestionID", obj.PMQuestionID));
                    //PM Questions
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherenrolled", obj.Pregnantmotherenrolled));
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherprimaryinsurance", obj.Pregnantmotherprimaryinsurance));
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherprimaryinsurancenotes", obj.Pregnantmotherprimaryinsurancenotes));
                    command.Parameters.Add(new SqlParameter("@PMPrblmsPosted", obj.PMPrblmsPosted));
                    command.Parameters.Add(new SqlParameter("@PMServicePosted", obj.PMServicePosted));
                    command.Parameters.Add(new SqlParameter("@TrimesterEnrolled", obj.TrimesterEnrolled));
                    command.Parameters.Add(new SqlParameter("@MedicalhomePreg1", obj.MedicalhomePreg1));
                    command.Parameters.Add(new SqlParameter("@PMVisitDoc", obj.PMVisitDoc));
                    command.Parameters.Add(new SqlParameter("@PMProblem", obj.PMProblem));
                    command.Parameters.Add(new SqlParameter("@PMOtherIssues", obj.PMOtherIssues));
                    command.Parameters.Add(new SqlParameter("@PMConditions", obj.PMConditions));
                    command.Parameters.Add(new SqlParameter("@PMCondtnDesc", obj.PMCondtnDesc));
                    command.Parameters.Add(new SqlParameter("@PMRisk", obj.PMRisk));
                    command.Parameters.Add(new SqlParameter("@PMDentalExam", obj.PMDentalExam));
                    command.Parameters.Add(new SqlParameter("@PMDentalExamDate", obj.PMDentalExamDate));
                    command.Parameters.Add(new SqlParameter("@PMNeedDental", obj.PMNeedDental));
                    command.Parameters.Add(new SqlParameter("@PMRecieveDental", obj.PMRecieveDental));
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherpmservices", obj._Pregnantmotherpmservices));
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherproblem", obj._Pregnantmotherproblem));
                    //End
                    command.Parameters.Add(new SqlParameter("@CreatedBy", ID));
                    command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                    command.Parameters.Add(new SqlParameter("@mode", mode));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters["@result"].Direction = ParameterDirection.Output;
                }

                if (mode == 1 && obj.PregnantmotherenrolledP1 == true)
                {
                    command.Parameters.Add(new SqlParameter("@ParentID1", obj.ParentOriginalId));
                    command.Parameters.Add(new SqlParameter("@HouseholdId", obj.HouseholdId));
                    command.Parameters.Add(new SqlParameter("@ClientID", obj.ParentID));
                    command.Parameters.Add(new SqlParameter("@P1Question", obj.P1Question));
                    command.Parameters.Add(new SqlParameter("@PMQuestionID1", obj.PMQuestionID1));

                    //PM Question
                    command.Parameters.Add(new SqlParameter("@TrimesterEnrolled1", obj.TrimesterEnrolled1));
                    command.Parameters.Add(new SqlParameter("@PregnantmotherenrolledP1", obj.PregnantmotherenrolledP1));
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherprimaryinsurance1", obj.Pregnantmotherprimaryinsurance1));
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherprimaryinsurancenotes1", obj.Pregnantmotherprimaryinsurancenotes1));
                    command.Parameters.Add(new SqlParameter("@MedicalhomePreg1", obj.MedicalhomePreg2));
                    command.Parameters.Add(new SqlParameter("@PMVisitDoc1", obj.PMVisitDoc1));
                    command.Parameters.Add(new SqlParameter("@PMProblem1", obj.PMProblem1));
                    command.Parameters.Add(new SqlParameter("@PMOtherIssues1", obj.PMOtherIssues1));
                    command.Parameters.Add(new SqlParameter("@PMConditions1", obj.PMConditions1));
                    command.Parameters.Add(new SqlParameter("@PMCondtnDesc1", obj.PMCondtnDesc1));
                    command.Parameters.Add(new SqlParameter("@PMRisk1", obj.PMRisk1));
                    command.Parameters.Add(new SqlParameter("@PMDentalExam1", obj.PMDentalExam1));
                    command.Parameters.Add(new SqlParameter("@PMDentalExamDate1", obj.PMDentalExamDate1));
                    command.Parameters.Add(new SqlParameter("@PMNeedDental1", obj.PMNeedDental1));
                    command.Parameters.Add(new SqlParameter("@PMRecieveDental1", obj.PMRecieveDental1));
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherpmservices1", obj._Pregnantmotherpmservices1));
                    command.Parameters.Add(new SqlParameter("@Pregnantmotherproblem1", obj._Pregnantmotherproblem1));
                    //End
                    command.Parameters.Add(new SqlParameter("@CreatedBy", ID));
                    command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                    command.Parameters.Add(new SqlParameter("@mode", mode));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters["@result"].Direction = ParameterDirection.Output;
                }
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_addPMQuestions";
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;

        }
        public string addHealthInfo(Nurse obj, int mode, Guid ID, string agencyid, Screening _screen, List<FamilyHousehold.ImmunizationRecord> Imminization,string RoleID)
        {
            string result = string.Empty;
            try
            {
                string HouseholdId = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                if ((mode == 0 && obj.ChildReferenceProgramID == "2") || (mode == 1 && obj.ChildReferenceProgramID == "2"))
                {
                    command.Parameters.Add(new SqlParameter("@HealthQuesId", obj.HealthQuesId));
                    command.Parameters.Add(new SqlParameter("@HouseholdId", obj.HouseholdId));
                    command.Parameters.Add(new SqlParameter("@ClientID", obj.ChildId));
                    command.Parameters.Add(new SqlParameter("@ChildReferenceProgramID", obj.ChildReferenceProgramID));
                    command.Parameters.Add(new SqlParameter("@HsChildBorn", obj.HsChildBorn));
                    command.Parameters.Add(new SqlParameter("@HsChildBirthWt", obj.HsChildBirthWt));
                    command.Parameters.Add(new SqlParameter("@HsChildLength", obj.HsChildLength));
                    command.Parameters.Add(new SqlParameter("@HsChildProblm", obj.HsChildProblm));
                    command.Parameters.Add(new SqlParameter("@HsMedication", obj.HsMedication));
                    command.Parameters.Add(new SqlParameter("@HSmpplan", obj.HSmpplan));
                    command.Parameters.Add(new SqlParameter("@HsDentalExam", obj.HsDentalExam));
                    command.Parameters.Add(new SqlParameter("@HsComment", obj.HsComment));
                    command.Parameters.Add(new SqlParameter("@HsChildDentalCare", obj.HsChildDentalCare));
                    command.Parameters.Add(new SqlParameter("@HsRecentDentalExam", obj.HsRecentDentalExam));
                    command.Parameters.Add(new SqlParameter("@HsChildNeedDentalTreatment", obj.HsChildNeedDentalTreatment));
                    command.Parameters.Add(new SqlParameter("@HsChildRecievedDentalTreatment", obj.HsChildRecievedDentalTreatment));
                    command.Parameters.Add(new SqlParameter("@ChildDirectBloodRelativeHs", obj._ChildDirectBloodRelativeHs));
                    command.Parameters.Add(new SqlParameter("@ChildDiagnosedConditionsHs", obj._ChildDiagnosedConditionsHs));
                    command.Parameters.Add(new SqlParameter("@ChildMedicalTreatmentHs", obj._ChildMedicalTreatmentHs));
                    command.Parameters.Add(new SqlParameter("@ChildreceiveChronicHealthConditionsHs", obj._ChildChronicHealthConditionsHs));
                    command.Parameters.Add(new SqlParameter("@CreatedBy", ID));
                    command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                    command.Parameters.Add(new SqlParameter("@mode", mode));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters["@result"].Direction = ParameterDirection.Output;
                }
                else if ((mode == 0 && obj.ChildReferenceProgramID == "1") || (mode == 1 && obj.ChildReferenceProgramID == "1"))
                {
                    command.Parameters.Add(new SqlParameter("@HealthQuesId", obj.HealthQuesId));
                    command.Parameters.Add(new SqlParameter("@HouseholdId", obj.HouseholdId));
                    command.Parameters.Add(new SqlParameter("@ClientID", obj.ChildId));
                    command.Parameters.Add(new SqlParameter("@ChildReferenceProgramID", obj.ChildReferenceProgramID));
                    command.Parameters.Add(new SqlParameter("@EHsChildBorn", obj.EhsChildBorn));
                    command.Parameters.Add(new SqlParameter("@EhsChildBirthWt", obj.EhsChildBirthWt));
                    command.Parameters.Add(new SqlParameter("@EhsChildLength", obj.EhsChildLength));
                    command.Parameters.Add(new SqlParameter("@EhsChildProblm", obj.EhsChildProblm));
                    command.Parameters.Add(new SqlParameter("@EhsMedication", obj.EhsMedication));
                    command.Parameters.Add(new SqlParameter("@EHSmpplan", obj.EHSmpplan));
                    command.Parameters.Add(new SqlParameter("@EhsComment", obj.EhsComment));
                    command.Parameters.Add(new SqlParameter("@EHSAllergy", obj.EHSAllergy));
                    command.Parameters.Add(new SqlParameter("@EHSEpiPen", obj.EHSEpiPen));
                    command.Parameters.Add(new SqlParameter("@ChildDirectBloodRelativeEhs", obj._ChildDirectBloodRelativeEhs));
                    command.Parameters.Add(new SqlParameter("@ChildDiagnosedConditionsEhs", obj._ChildDiagnosedConditionsEhs));
                    command.Parameters.Add(new SqlParameter("@ChildreceiveChronicHealthConditionsEhs", obj._ChildChronicHealthConditions2Ehs));
                    command.Parameters.Add(new SqlParameter("@ChildreceivedChronicHealthConditionsEhs", obj._ChildChronicHealthConditionsEhs));
                    command.Parameters.Add(new SqlParameter("@ChildreceivingChronicHealthConditionsEhs", obj._ChildChronicHealthConditions1Ehs));
                    command.Parameters.Add(new SqlParameter("@ChildMedicalTreatmentEhs", obj._ChildMedicalTreatmentEhs));




                    //command.Parameters.Add(new SqlParameter("@HSBabyOrMotherProblems", obj.HSBabyOrMotherProblems));
                    //command.Parameters.Add(new SqlParameter("@HsMedicationName", obj.HsMedicationName));
                    //command.Parameters.Add(new SqlParameter("@HsDosage", obj.HsDosage));
                    //command.Parameters.Add(new SqlParameter("@HSChildMedication", obj.HSChildMedication));
                    //command.Parameters.Add(new SqlParameter("@HSPreventativeDentalCare", obj.HSPreventativeDentalCare));
                    //command.Parameters.Add(new SqlParameter("@HSProfessionalDentalExam", obj.HSProfessionalDentalExam));
                    //command.Parameters.Add(new SqlParameter("@HSNeedingDentalTreatment", obj.HSNeedingDentalTreatment));
                    //command.Parameters.Add(new SqlParameter("@HSChildReceivedDentalTreatment", obj.HSChildReceivedDentalTreatment));









                    command.Parameters.Add(new SqlParameter("@CreatedBy", ID));
                    command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                    command.Parameters.Add(new SqlParameter("@mode", mode));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters["@result"].Direction = ParameterDirection.Output;
                }



                //Added by Akansha on 19Dec2016
                // child nutrition with HS/Ehs
                if (obj.ChildReferenceProgramID == "1")  //Ehs Questions
                {
                    #region child nutrition

                    command.Parameters.Add(new SqlParameter("@ChildVitaminSupplment", obj.EhsChildVitaminSupplment));//new field added
                    command.Parameters.Add(new SqlParameter("@PersistentNausea", obj.EhsPersistentNausea));
                    command.Parameters.Add(new SqlParameter("@PersistentDiarrhea", obj.EhsPersistentDiarrhea));
                    command.Parameters.Add(new SqlParameter("@PersistentConstipation", obj.EhsPersistentConstipation));
                    command.Parameters.Add(new SqlParameter("@DramaticWeight", obj.EhsDramaticWeight));
                    command.Parameters.Add(new SqlParameter("@RecentSurgery", obj.EhsRecentSurgery));
                    command.Parameters.Add(new SqlParameter("@RecentHospitalization", obj.EhsRecentHospitalization));
                    command.Parameters.Add(new SqlParameter("@ChildSpecialDiet", obj.EhsChildSpecialDiet));
                    command.Parameters.Add(new SqlParameter("@FoodAllergies", obj.EhsFoodAllergies));
                    command.Parameters.Add(new SqlParameter("@NutritionalConcern", obj.EhsNutritionalConcern));
                    command.Parameters.Add(new SqlParameter("@WICNutrition", obj.WICNutrition));
                    command.Parameters.Add(new SqlParameter("@FoodStamps", obj.FoodStamps));
                    command.Parameters.Add(new SqlParameter("@NoNutritionProg", obj.NoNutritionProg));
                    command.Parameters.Add(new SqlParameter("@foodpantry", obj.EhsFoodPantory));
                    command.Parameters.Add(new SqlParameter("@childTrouble", obj.EhschildTrouble));
                    command.Parameters.Add(new SqlParameter("@spoon", obj.Ehsspoon));
                    command.Parameters.Add(new SqlParameter("@feedingtube", obj.Ehsfeedingtube));
                    command.Parameters.Add(new SqlParameter("@childThin", obj.EhschildThin));
                    command.Parameters.Add(new SqlParameter("@Takebottle", obj.EhsTakebottle));
                    command.Parameters.Add(new SqlParameter("@chewanything", obj.Ehschewanything));
                    command.Parameters.Add(new SqlParameter("@ChangeinAppetite", obj.EhsChangeinAppetite));
                    command.Parameters.Add(new SqlParameter("@ChildHungry", obj.EhsChildHungry));
                    command.Parameters.Add(new SqlParameter("@MilkComment", obj.EhsMilkComment));
                    command.Parameters.Add(new SqlParameter("@ChildFeed", obj.ChildFeed));//Differ
                    command.Parameters.Add(new SqlParameter("@ChildFormula", obj.ChildFormula));
                    command.Parameters.Add(new SqlParameter("@ChildFeedCereal", obj.ChildFeedCereal));
                    command.Parameters.Add(new SqlParameter("@ChildFeedMarshfood", obj.ChildFeedMarshfood));
                    command.Parameters.Add(new SqlParameter("@ChildFeedChopedfood", obj.ChildFeedChopedfood));
                    command.Parameters.Add(new SqlParameter("@ChildFingerFood", obj.ChildFingerFood));
                    command.Parameters.Add(new SqlParameter("@ChildFingerFEDFood", obj.ChildFingerFEDFood));
                    command.Parameters.Add(new SqlParameter("@ChildFruitJuice", obj.ChildFruitJuice));//End
                    command.Parameters.Add(new SqlParameter("@ChildFruitJuicevitaminc", obj.EhsChildFruitJuicevitaminc));
                    command.Parameters.Add(new SqlParameter("@ChildWater", obj.EhsChildWater));
                    command.Parameters.Add(new SqlParameter("@Breakfast", obj.EhsBreakfast));
                    command.Parameters.Add(new SqlParameter("@Lunch", obj.EhsLunch));
                    command.Parameters.Add(new SqlParameter("@Snack", obj.EhsSnack));
                    command.Parameters.Add(new SqlParameter("@Dinner", obj.EhsDinner));
                    command.Parameters.Add(new SqlParameter("@NA", obj.EhsNA));
                    command.Parameters.Add(new SqlParameter("@RestrictFood", obj.EhsRestrictFood));//New ques added

                    command.Parameters.Add(new SqlParameter("@ChildReferalCriteria", obj.ChildReferalCriteria));
                    command.Parameters.Add(new SqlParameter("@ChildChildVitaminSupplement", obj._ChildChildVitaminSupplement));
                    command.Parameters.Add(new SqlParameter("@ChildDiet", obj._ChildDiet));
                    command.Parameters.Add(new SqlParameter("@ChildDrink", obj._ChildDrink));

                    //command.Parameters.Add(new SqlParameter("@NotHealthStaff", obj.NotHealthStaff));



                    command.Parameters.Add(new SqlParameter("@NutritionQuesId", obj.NutritionQuesId));
                    command.Parameters.Add(new SqlParameter("@NauseaorVomitingcomment", obj.EhsNauseaorVomitingcomment));
                    command.Parameters.Add(new SqlParameter("@DiarrheaComment", obj.EhsDiarrheaComment));
                    command.Parameters.Add(new SqlParameter("@Constipationcomment", obj.EhsConstipationcomment));
                    command.Parameters.Add(new SqlParameter("@DramaticWeightchangecomment", obj.EhsDramaticWeightchangecomment));
                    command.Parameters.Add(new SqlParameter("@RecentSurgerycomment", obj.EhsRecentSurgerycomment));
                    command.Parameters.Add(new SqlParameter("@RecentHospitalizationComment", obj.EhsRecentHospitalizationComment));
                    command.Parameters.Add(new SqlParameter("@SpecialDietComment", obj.EhsSpecialDietComment));
                    command.Parameters.Add(new SqlParameter("@FoodAllergiesComment", obj.EhsFoodAllergiesComment));
                    command.Parameters.Add(new SqlParameter("@NutritionAlconcernsComment", obj.EhsNutritionAlconcernsComment));
                    command.Parameters.Add(new SqlParameter("@ChewingorSwallowingcomment", obj.EhsChewingorSwallowingcomment));
                    command.Parameters.Add(new SqlParameter("@SpoonorForkComment", obj.EhsSpoonorForkComment));
                    command.Parameters.Add(new SqlParameter("@SpecialFeedingComment", obj.EhsSpecialFeedingComment));
                    command.Parameters.Add(new SqlParameter("@BottleComment", obj.EhsBottleComment));
                    command.Parameters.Add(new SqlParameter("@EatOrChewComment", obj.EhsEatOrChewComment));
                    // command.Parameters.Add(new SqlParameter("@ChildReferenceProgramID", obj.ChildReferenceProgramID));
                    #endregion
                }
                if (obj.ChildReferenceProgramID == "2")  //hs Questions
                {
                    #region child nutrition
                    command.Parameters.Add(new SqlParameter("@NutritionQuesId", obj.NutritionQuesId));
                    command.Parameters.Add(new SqlParameter("@ChildVitaminSupplment", obj.ChildVitaminSupplment));//new field added
                    command.Parameters.Add(new SqlParameter("@RestrictFood", obj.RestrictFood));//New ques added
                    command.Parameters.Add(new SqlParameter("@PersistentNausea", obj.PersistentNausea));
                    command.Parameters.Add(new SqlParameter("@PersistentDiarrhea", obj.PersistentDiarrhea));
                    command.Parameters.Add(new SqlParameter("@PersistentConstipation", obj.PersistentConstipation));
                    command.Parameters.Add(new SqlParameter("@DramaticWeight", obj.DramaticWeight));
                    command.Parameters.Add(new SqlParameter("@RecentSurgery", obj.RecentSurgery));
                    command.Parameters.Add(new SqlParameter("@RecentHospitalization", obj.RecentHospitalization));
                    command.Parameters.Add(new SqlParameter("@ChildSpecialDiet", obj.ChildSpecialDiet));
                    command.Parameters.Add(new SqlParameter("@FoodAllergies", obj.FoodAllergies));
                    command.Parameters.Add(new SqlParameter("@NutritionalConcern", obj.NutritionalConcern));
                    command.Parameters.Add(new SqlParameter("@WICNutrition", obj.WICNutrition));
                    command.Parameters.Add(new SqlParameter("@FoodStamps", obj.FoodStamps));
                    command.Parameters.Add(new SqlParameter("@NoNutritionProg", obj.NoNutritionProg));
                    command.Parameters.Add(new SqlParameter("@foodpantry", obj.FoodPantory));
                    command.Parameters.Add(new SqlParameter("@childTrouble", obj.childTrouble));
                    command.Parameters.Add(new SqlParameter("@spoon", obj.spoon));
                    command.Parameters.Add(new SqlParameter("@feedingtube", obj.feedingtube));
                    command.Parameters.Add(new SqlParameter("@childThin", obj.childThin));
                    command.Parameters.Add(new SqlParameter("@Takebottle", obj.Takebottle));
                    command.Parameters.Add(new SqlParameter("@chewanything", obj.chewanything));
                    command.Parameters.Add(new SqlParameter("@ChangeinAppetite", obj.ChangeinAppetite));
                    command.Parameters.Add(new SqlParameter("@ChildHungry", obj.ChildHungry));
                    command.Parameters.Add(new SqlParameter("@MilkComment", obj.MilkComment));
                    command.Parameters.Add(new SqlParameter("@ChildFeed", obj.ChildFeed));
                    command.Parameters.Add(new SqlParameter("@ChildFormula", obj.ChildFormula));
                    command.Parameters.Add(new SqlParameter("@ChildFeedCereal", obj.ChildFeedCereal));
                    command.Parameters.Add(new SqlParameter("@ChildFeedMarshfood", obj.ChildFeedMarshfood));
                    command.Parameters.Add(new SqlParameter("@ChildFeedChopedfood", obj.ChildFeedChopedfood));
                    command.Parameters.Add(new SqlParameter("@ChildFingerFood", obj.ChildFingerFood));
                    command.Parameters.Add(new SqlParameter("@ChildFingerFEDFood", obj.ChildFingerFEDFood));
                    command.Parameters.Add(new SqlParameter("@ChildFruitJuice", obj.ChildFruitJuice));
                    command.Parameters.Add(new SqlParameter("@ChildFruitJuicevitaminc", obj.ChildFruitJuicevitaminc));
                    command.Parameters.Add(new SqlParameter("@ChildWater", obj.ChildWater));
                    command.Parameters.Add(new SqlParameter("@Breakfast", obj.Breakfast));
                    command.Parameters.Add(new SqlParameter("@Lunch", obj.Lunch));
                    command.Parameters.Add(new SqlParameter("@Snack", obj.Snack));
                    command.Parameters.Add(new SqlParameter("@Dinner", obj.Dinner));
                    command.Parameters.Add(new SqlParameter("@NA", obj.NA));
                    command.Parameters.Add(new SqlParameter("@ChildReferalCriteria", obj.ChildReferalCriteria));
                    command.Parameters.Add(new SqlParameter("@ChildChildVitaminSupplement", obj._ChildChildVitaminSupplement));
                    command.Parameters.Add(new SqlParameter("@ChildDiet", obj._ChildDiet));
                    command.Parameters.Add(new SqlParameter("@ChildDrink", obj._ChildDrink));

                    //command.Parameters.Add(new SqlParameter("@NotHealthStaff", obj.NotHealthStaff));




                    command.Parameters.Add(new SqlParameter("@NauseaorVomitingcomment", obj.NauseaorVomitingcomment));
                    command.Parameters.Add(new SqlParameter("@DiarrheaComment", obj.DiarrheaComment));
                    command.Parameters.Add(new SqlParameter("@Constipationcomment", obj.Constipationcomment));
                    command.Parameters.Add(new SqlParameter("@DramaticWeightchangecomment", obj.DramaticWeightchangecomment));
                    command.Parameters.Add(new SqlParameter("@RecentSurgerycomment", obj.RecentSurgerycomment));
                    command.Parameters.Add(new SqlParameter("@RecentHospitalizationComment", obj.RecentHospitalizationComment));
                    command.Parameters.Add(new SqlParameter("@SpecialDietComment", obj.SpecialDietComment));
                    command.Parameters.Add(new SqlParameter("@FoodAllergiesComment", obj.FoodAllergiesComment));
                    command.Parameters.Add(new SqlParameter("@NutritionAlconcernsComment", obj.NutritionAlconcernsComment));
                    command.Parameters.Add(new SqlParameter("@ChewingorSwallowingcomment", obj.ChewingorSwallowingcomment));
                    command.Parameters.Add(new SqlParameter("@SpoonorForkComment", obj.SpoonorForkComment));
                    command.Parameters.Add(new SqlParameter("@SpecialFeedingComment", obj.SpecialFeedingComment));
                    command.Parameters.Add(new SqlParameter("@BottleComment", obj.BottleComment));
                    command.Parameters.Add(new SqlParameter("@EatOrChewComment", obj.EatOrChewComment));
                    // command.Parameters.Add(new SqlParameter("@ChildReferenceProgramID", obj.ChildReferenceProgramID));
                    #endregion
                }











                //Commented on 21Dec2016

                //command.Parameters.Add(new SqlParameter("@NutritionQuesId", obj.NutritionQuesId));
                //command.Parameters.Add(new SqlParameter("@PersistentNausea", obj.PersistentNausea));
                //command.Parameters.Add(new SqlParameter("@PersistentDiarrhea", obj.PersistentDiarrhea));
                //command.Parameters.Add(new SqlParameter("@PersistentConstipation", obj.PersistentConstipation));
                //command.Parameters.Add(new SqlParameter("@DramaticWeight", obj.DramaticWeight));
                //command.Parameters.Add(new SqlParameter("@RecentSurgery", obj.RecentSurgery));
                //command.Parameters.Add(new SqlParameter("@RecentHospitalization", obj.RecentHospitalization));
                //command.Parameters.Add(new SqlParameter("@ChildSpecialDiet", obj.ChildSpecialDiet));
                //command.Parameters.Add(new SqlParameter("@FoodAllergies", obj.FoodAllergies));
                //command.Parameters.Add(new SqlParameter("@NutritionalConcern", obj.NutritionalConcern));
                //command.Parameters.Add(new SqlParameter("@WICNutrition", obj.WICNutrition));
                //command.Parameters.Add(new SqlParameter("@FoodStamps", obj.FoodStamps));
                //command.Parameters.Add(new SqlParameter("@NoNutritionProg", obj.NoNutritionProg));
                //command.Parameters.Add(new SqlParameter("@foodpantry", obj.FoodPantory));
                //command.Parameters.Add(new SqlParameter("@childTrouble", obj.childTrouble));
                //command.Parameters.Add(new SqlParameter("@spoon", obj.spoon));
                //command.Parameters.Add(new SqlParameter("@feedingtube", obj.feedingtube));
                //command.Parameters.Add(new SqlParameter("@childThin", obj.childThin));
                //command.Parameters.Add(new SqlParameter("@Takebottle", obj.Takebottle));
                //command.Parameters.Add(new SqlParameter("@chewanything", obj.chewanything));
                //command.Parameters.Add(new SqlParameter("@ChangeinAppetite", obj.ChangeinAppetite));
                //command.Parameters.Add(new SqlParameter("@ChildHungry", obj.ChildHungry));
                //command.Parameters.Add(new SqlParameter("@MilkComment", obj.MilkComment));
                //command.Parameters.Add(new SqlParameter("@ChildFeed", obj.ChildFeed));
                //command.Parameters.Add(new SqlParameter("@ChildFormula", obj.ChildFormula));
                //command.Parameters.Add(new SqlParameter("@ChildFeedCereal", obj.ChildFeedCereal));
                //command.Parameters.Add(new SqlParameter("@ChildFeedMarshfood", obj.ChildFeedMarshfood));
                //command.Parameters.Add(new SqlParameter("@ChildFeedChopedfood", obj.ChildFeedChopedfood));
                //command.Parameters.Add(new SqlParameter("@ChildFingerFood", obj.ChildFingerFood));
                //command.Parameters.Add(new SqlParameter("@ChildFingerFEDFood", obj.ChildFingerFEDFood));
                //command.Parameters.Add(new SqlParameter("@ChildFruitJuice", obj.ChildFruitJuice));
                //command.Parameters.Add(new SqlParameter("@ChildFruitJuicevitaminc", obj.ChildFruitJuicevitaminc));
                //command.Parameters.Add(new SqlParameter("@ChildWater", obj.ChildWater));
                //command.Parameters.Add(new SqlParameter("@Breakfast", obj.Breakfast));
                //command.Parameters.Add(new SqlParameter("@Lunch", obj.Lunch));
                //command.Parameters.Add(new SqlParameter("@Snack", obj.Snack));
                //command.Parameters.Add(new SqlParameter("@Dinner", obj.Dinner));
                //command.Parameters.Add(new SqlParameter("@NA", obj.NA));
                //command.Parameters.Add(new SqlParameter("@ChildReferalCriteria", obj.ChildReferalCriteria));
                //command.Parameters.Add(new SqlParameter("@ChildChildVitaminSupplement", obj._ChildChildVitaminSupplement));
                //command.Parameters.Add(new SqlParameter("@ChildDiet", obj._ChildDiet));
                //command.Parameters.Add(new SqlParameter("@ChildDrink", obj._ChildDrink));
                //command.Parameters.Add(new SqlParameter("@NauseaorVomitingcomment", obj.NauseaorVomitingcomment));
                //command.Parameters.Add(new SqlParameter("@DiarrheaComment", obj.DiarrheaComment));
                //command.Parameters.Add(new SqlParameter("@Constipationcomment", obj.Constipationcomment));
                //command.Parameters.Add(new SqlParameter("@DramaticWeightchangecomment", obj.DramaticWeightchangecomment));
                //command.Parameters.Add(new SqlParameter("@RecentSurgerycomment", obj.RecentSurgerycomment));
                //command.Parameters.Add(new SqlParameter("@RecentHospitalizationComment", obj.RecentHospitalizationComment));
                //command.Parameters.Add(new SqlParameter("@SpecialDietComment", obj.SpecialDietComment));
                //command.Parameters.Add(new SqlParameter("@FoodAllergiesComment", obj.FoodAllergiesComment));
                //command.Parameters.Add(new SqlParameter("@NutritionAlconcernsComment", obj.NutritionAlconcernsComment));
                //command.Parameters.Add(new SqlParameter("@ChewingorSwallowingcomment", obj.ChewingorSwallowingcomment));
                //command.Parameters.Add(new SqlParameter("@SpoonorForkComment", obj.SpoonorForkComment));
                //command.Parameters.Add(new SqlParameter("@SpecialFeedingComment", obj.SpecialFeedingComment));
                //command.Parameters.Add(new SqlParameter("@BottleComment", obj.BottleComment));
                //command.Parameters.Add(new SqlParameter("@EatOrChewComment", obj.EatOrChewComment));






                //Screening

                //Changes
                #region screening
                command.Parameters.Add(new SqlParameter("@Physical", _screen.AddPhysical));
                command.Parameters.Add(new SqlParameter("@Vision", _screen.AddVision));
                command.Parameters.Add(new SqlParameter("@Dental", _screen.AddDental));
                command.Parameters.Add(new SqlParameter("@Hearing", _screen.AddHearing));
                command.Parameters.Add(new SqlParameter("@Develop", _screen.AddDevelop));
                command.Parameters.Add(new SqlParameter("@Speech", _screen.AddSpeech));
                command.Parameters.Add(new SqlParameter("@PhysicalFileName", _screen.PhysicalFileName));
                command.Parameters.Add(new SqlParameter("@PhysicalFileExtension", _screen.PhysicalFileExtension));
                command.Parameters.Add(new SqlParameter("@PhysicalImageByte", _screen.PhysicalImageByte));
                command.Parameters.Add(new SqlParameter("@VisionFileName", _screen.VisionFileName));
                command.Parameters.Add(new SqlParameter("@VisionFileExtension", _screen.VisionFileExtension));
                command.Parameters.Add(new SqlParameter("@VisionImageByte", _screen.VisionImageByte));
                command.Parameters.Add(new SqlParameter("@DevelopFileName", _screen.DevelopFileName));
                command.Parameters.Add(new SqlParameter("@DevelopFileExtension", _screen.DevelopFileExtension));
                command.Parameters.Add(new SqlParameter("@DevelopImageByte", _screen.DevelopImageByte));
                command.Parameters.Add(new SqlParameter("@DentalFileExtension", _screen.DentalFileExtension));
                command.Parameters.Add(new SqlParameter("@DentalFileName", _screen.DentalFileName));
                command.Parameters.Add(new SqlParameter("@DentalImageByte", _screen.DentalImageByte));
                command.Parameters.Add(new SqlParameter("@HearingFileName", _screen.HearingFileName));
                command.Parameters.Add(new SqlParameter("@HearingFileExtension", _screen.HearingFileExtension));
                command.Parameters.Add(new SqlParameter("@HearingImageByte", _screen.HearingImageByte));
                command.Parameters.Add(new SqlParameter("@SpeechFileName", _screen.SpeechFileName));
                command.Parameters.Add(new SqlParameter("@SpeechFileExtension", _screen.SpeechFileExtension));
                command.Parameters.Add(new SqlParameter("@SpeechImageByte", _screen.SpeechImageByte));
                command.Parameters.Add(new SqlParameter("@ScreeningAcceptFileExtension", _screen.ScreeningAcceptFileExtension));
                command.Parameters.Add(new SqlParameter("@ScreeningAcceptFileName", _screen.ScreeningAcceptFileName));
                command.Parameters.Add(new SqlParameter("@ScreeningAcceptImageByte", _screen.ScreeningAcceptImageByte));
                DataTable dt6 = new DataTable();
                dt6.Columns.AddRange(new DataColumn[3] { 
                    new DataColumn("ScreeningID",typeof(Int32)), 
                    new DataColumn("QuestionID",typeof(Int32)), 
                    new DataColumn("Value",typeof(string)),
                    });

                foreach (var s in _screen.GetType().GetProperties())
                {
                    int screeningid = 0;
                    int questionid = 0;
                    if (s.Name.Substring(0, 1) == "F")
                        screeningid = 1;
                    if (s.Name.Substring(0, 1) == "v")
                        screeningid = 2;
                    if (s.Name.Substring(0, 1) == "h")
                        screeningid = 3;
                    if (s.Name.Substring(0, 1) == "d")
                        screeningid = 4;
                    if (s.Name.Substring(0, 1) == "E")
                        screeningid = 5;
                    if (s.Name.Substring(0, 1) == "s")
                        screeningid = 6;
                    if (screeningid == 1 || screeningid == 2 || screeningid == 3 || screeningid == 4 || screeningid == 5 || screeningid == 6)
                    {
                        questionid = Convert.ToInt32(s.Name.Substring(1, 3));
                        dt6.Rows.Add(screeningid, questionid, s.GetValue(_screen));
                    }
                }
                DataTable dt5 = new DataTable();
                dt5.Columns.AddRange(new DataColumn[18] { 
                    new DataColumn("Immunizationmasterid", typeof(Int32)),
                    new DataColumn("ImmunizationId", typeof(Int32)),
                    new DataColumn("Dose",typeof(string)), 
                    new DataColumn("Dose1",typeof(string)), 
                    new DataColumn("Preempt1",typeof(bool)), 
                    new DataColumn("Exempt1",typeof(bool)),
                    new DataColumn("Dose2",typeof(string)), 
                    new DataColumn("Preempt2",typeof(bool)), 
                    new DataColumn("Exempt2",typeof(bool)),
                    new DataColumn("Dose3",typeof(string)), 
                    new DataColumn("Preempt3",typeof(bool)), 
                    new DataColumn("Exempt3",typeof(bool)),
                     new DataColumn("Dose4",typeof(string)), 
                    new DataColumn("Preempt4",typeof(bool)), 
                    new DataColumn("Exempt4",typeof(bool)),
                     new DataColumn("Dose5",typeof(string)), 
                    new DataColumn("Preempt5",typeof(bool)), 
                    new DataColumn("Exempt5",typeof(bool)),

                    });
                if (Imminization != null)
                {
                    foreach (FamilyHousehold.ImmunizationRecord _Imminization in Imminization)
                    {
                        dt5.Rows.Add(_Imminization.ImmunizationmasterId, _Imminization.ImmunizationId, _Imminization.Dose, _Imminization.Dose1, _Imminization.Preempt1, _Imminization.Exempt1
                            , _Imminization.Dose2, _Imminization.Preempt2, _Imminization.Exempt2, _Imminization.Dose3, _Imminization.Preempt3, _Imminization.Exempt3
                            , _Imminization.Dose4, _Imminization.Preempt4, _Imminization.Exempt4, _Imminization.Dose5, _Imminization.Preempt5, _Imminization.Exempt5);
                    }
                }
                #endregion
                //End
                command.Parameters.Add(new SqlParameter("@tblImminization", dt5));
                command.Parameters.Add(new SqlParameter("@tblscreening", dt6));
                command.Parameters.Add(new SqlParameter("@BMIStatus2", obj.BMIStatus2));
                command.Parameters.Add(new SqlParameter("@HWInput", obj.HWInput));
                command.Parameters.Add(new SqlParameter("@AssessmentDate", obj.AssessmentDate));
                command.Parameters.Add(new SqlParameter("@BHeight", obj.AHeight));
                command.Parameters.Add(new SqlParameter("@bWeight", obj.AWeight));
                command.Parameters.Add(new SqlParameter("@HeadCircle", obj.HeadCircle));
                command.Parameters.Add(new SqlParameter("@Yakkrid", obj.Yakkrid));
                command.Parameters.Add(new SqlParameter("@RoleID", RoleID));
                //End
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_addHealthQuestions";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                GetallHouseholdinfo(obj, _dataset);

                // command.ExecuteNonQuery();

                result = command.Parameters["@result"].Value.ToString();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;

        }

        public bool InsertAcceptReason(Nurse obj,string AgencyId,string RoleID)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.AddWithValue("@ClienTId", obj.ClientID);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Reason", obj.AcceptReason);
                command.Parameters.AddWithValue("@RoleId", RoleID);
                command.Parameters.AddWithValue("@UserId", obj.UserId);
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_ApplicationAcceptReason";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return isInserted;
        }
        public string addAcceptInfo(out int pendingcount, Nurse obj, int mode, StaffDetails staffDetails)
        {
            string result = string.Empty;
            pendingcount = 0;
            try
            {
                string center = EncryptDecrypt.Decrypt64(obj.CenterID);
                string HouseholdId = string.Empty;





                var dbManager = new DBManager(connection.ConnectionString);

                var parameters = new IDbDataParameter[]
                {
                    dbManager.CreateParameter("@HouseholdId",obj.HouseholdId,DbType.Int64),
                    dbManager.CreateParameter("@ClientID",obj.ClientID,DbType.Int64),
                    dbManager.CreateParameter("@YakkrID",obj.Yakkrid,DbType.Int64),
                    dbManager.CreateParameter("@CenterID", EncryptDecrypt.Decrypt64(obj.CenterID),DbType.Int64),
                    dbManager.CreateParameter("@AcceptApplicant",obj.AcceptApplicant,DbType.String),
                    dbManager.CreateParameter("@RejectDesc",obj.RejectDesc,DbType.AnsiString),
                    dbManager.CreateParameter("@AcceptDesc",obj.AcceptDesc,DbType.AnsiString),
                    dbManager.CreateParameter("@CreatedBy",staffDetails.UserId,DbType.Guid),
                    dbManager.CreateParameter("@RoleId",staffDetails.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@Reason",obj.AcceptReason,DbType.AnsiString),
                    dbManager.CreateParameter("@AgencyID",staffDetails.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@result",2, string.Empty,DbType.String,ParameterDirection.Output),
                    dbManager.CreateParameter("@pendingcount",10,0,DbType.Int32,ParameterDirection.Output)




                };


                dbManager.ExecuteWithNonQuery<bool>("SP_addYakkrAcceptInfo", CommandType.StoredProcedure, parameters);


                result = parameters.Where(x => x.ParameterName == "@result" && x.Direction == ParameterDirection.Output).Select(x => x.Value).First().ToString();
                pendingcount = Convert.ToInt32(parameters.Where(x => x.ParameterName == "@pendingcount" && x.Direction == ParameterDirection.Output).Select(x => x.Value).First());



                //if (Connection.State == ConnectionState.Open)
                //    Connection.Close();
                //Connection.Open();
                //command.Connection = Connection;
                //command.Parameters.Add(new SqlParameter("@HouseholdId", obj.HouseholdId));
                //command.Parameters.Add(new SqlParameter("@ClientID", obj.ClientID));
                //command.Parameters.Add(new SqlParameter("@YakkrID", obj.Yakkrid));
                //command.Parameters.Add(new SqlParameter("@CenterID", EncryptDecrypt.Decrypt64(obj.CenterID)));
                //command.Parameters.Add(new SqlParameter("@AcceptApplicant", obj.AcceptApplicant));
                //command.Parameters.Add(new SqlParameter("@RejectDesc", obj.RejectDesc));
                //command.Parameters.Add(new SqlParameter("@AcceptDesc", obj.AcceptDesc));
                //command.Parameters.Add(new SqlParameter("@CreatedBy", ID));
                //command.Parameters.Add(new SqlParameter("@RoleId", RoleID));
                //command.Parameters.Add(new SqlParameter("@Reason", obj.AcceptReason));
                //command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                //command.Parameters.Add(new SqlParameter("@result", string.Empty));
                //command.Parameters["@result"].Direction = ParameterDirection.Output;
                //command.Parameters.Add(new SqlParameter("@pendingcount", 0)).Direction = ParameterDirection.Output;
                //command.Parameters["@pendingcount"].Size = 10;
                //command.CommandType = CommandType.StoredProcedure;
                //command.CommandText = "SP_addYakkrAcceptInfo";
                //command.ExecuteNonQuery();
                //result = command.Parameters["@result"].Value.ToString();
                //pendingcount = Convert.ToInt32(command.Parameters["@pendingcount"].Value);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                //Connection.Close();
                //command.Dispose();
            }
            return result;

        }
        public string HealthReview(Nurse obj, int mode, Guid ID, string agencyid)
        {
            string result = string.Empty;
            try
            {
                string HouseholdId = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@HouseholdId", obj.HouseholdId));
                command.Parameters.Add(new SqlParameter("@ClientID", obj.ClientID));
                command.Parameters.Add(new SqlParameter("@yakkrid", obj.Yakkrid));
                command.Parameters.Add(new SqlParameter("@CreatedBy", ID));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_addHealthreview";
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;

        }
        public List<FingerprintsModel.Nurse> GetFileCabinet(string tabName, string Agencyid, int houseHoldId)
        {
            List<FingerprintsModel.Nurse> _list = new List<FingerprintsModel.Nurse>();
            try
            {
                command.Parameters.Add(new SqlParameter("@tabName", tabName));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@houseHoldId", @houseHoldId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_Get_FamilyCabinet";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        FingerprintsModel.Nurse obj = new FingerprintsModel.Nurse();
                        obj.FamilyHouseholdID = Convert.ToInt32(dr["ID"]);
                        obj.HFileName = dr["FileName"].ToString();
                        obj.DocumentDesc = dr["Description"].ToString();
                        obj.AmountNo = dr["IncomeAmount"].ToString();
                        obj.CreatedOn = dr["DateEntered"].ToString();
                        _list.Add(obj);
                    }
                }
                DataAdapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return _list;
        }
        public List<Nurse.Applicationnotes> SaveNotes(ref string result, string agencyid, string userid, string HouseHoldId, string Notes, string mode)
        {
            List<Nurse.Applicationnotes> Applicationnotes = new List<Nurse.Applicationnotes>();
            try
            {

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_savenotes";
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", HouseHoldId));
                command.Parameters.Add(new SqlParameter("@Notes", Notes));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);

                if (_dataset != null && _dataset.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        Nurse.Applicationnotes _Applicationnotes = new Nurse.Applicationnotes();
                        _Applicationnotes.Name = dr["name"].ToString();
                        _Applicationnotes.CreatedOn = Convert.ToDateTime(dr["DateEntered"]).ToString("MM/dd/yyyy");
                        _Applicationnotes.notes = dr["Notes"].ToString();
                        Applicationnotes.Add(_Applicationnotes);
                    }
                }
                result = command.Parameters["@result"].Value.ToString();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                command.Dispose();

            }
            return Applicationnotes;
        }
        public Nurse getHsChild(string ChildId, string Agencyid, string userid,string serverpath)
        {
            Nurse obj = new Nurse();
            List<FingerprintsModel.Nurse> _list = new List<FingerprintsModel.Nurse>();
            try
            {
                command.Parameters.Add(new SqlParameter("@client", ChildId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Nursehouseholdinfo";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    if (_dataset.Tables[10].Rows.Count > 0)
                    {
                        List<Nurse.Childhealthnutrition> _childhealthnutrition = new List<Nurse.Childhealthnutrition>();
                        Nurse.Childhealthnutrition info = null;
                        foreach (DataRow dr in _dataset.Tables[10].Rows)
                        {
                            info = new Nurse.Childhealthnutrition();
                            info.Id = dr["ID"].ToString();
                            info.MasterId = dr["ChildRecieveTreatment"].ToString();
                            info.Description = dr["Description"].ToString();
                            info.Questionid = dr["Questionid"].ToString();
                            info.Programid = dr["Programid"].ToString();
                            _childhealthnutrition.Add(info);
                        }
                        obj._Childhealthnutrition = _childhealthnutrition;
                    }
                }
                if (_dataset.Tables[7].Rows.Count > 0)
                {
                    Screening _Screening = new Screening();
                    foreach (DataRow dr in _dataset.Tables[7].Rows)
                    {
                        _Screening.F001physicalDate = dr["F001physicalDate"].ToString();
                        _Screening.F002physicalResults = dr["F002physicalResults"].ToString();
                        _Screening.F003physicallFOReason = dr["F003physicallFOReason"].ToString();
                        _Screening.F004medFollowup = dr["F004medFollowup"].ToString();
                        _Screening.F005MedFOComments = dr["F005MedFOComments"].ToString();
                        _Screening.F006bpResults = dr["F006bpResults"].ToString();
                        _Screening.F007hgDate = dr["F007hgDate"].ToString();
                        _Screening.F008hgStatus = dr["F008hgStatus"].ToString();
                        _Screening.F009hgResults = dr["F009hgResults"].ToString();
                        _Screening.F010hgReferralDate = dr["F010hgReferralDate"].ToString();
                        _Screening.F011hgComments = dr["F011hgComments"].ToString();
                        _Screening.F012hgDate2 = dr["F012hgDate2"].ToString();
                        _Screening.F013hgResults2 = dr["F013hgResults2"].ToString();
                        _Screening.F014hgFOStatus = dr["F014hgFOStatus"].ToString();
                        _Screening.F015leadDate = dr["F015leadDate"].ToString();
                        _Screening.F016leadResults = dr["F016leadResults"].ToString();
                        _Screening.F017leadReferDate = dr["F017leadReferDate"].ToString();
                        _Screening.F018leadComments = dr["F018leadComments"].ToString();
                        _Screening.F019leadDate2 = dr["F019leadDate2"].ToString();
                        _Screening.F020leadResults2 = dr["F020leadResults2"].ToString();
                        _Screening.F021leadFOStatus = dr["F021leadFOStatus"].ToString();
                        _Screening.v022date = dr["v022date"].ToString();
                        _Screening.v023results = dr["v023results"].ToString();
                        _Screening.v024comments = dr["v024comments"].ToString();
                        _Screening.v025dateR1 = dr["v025dateR1"].ToString();
                        _Screening.v026resultsR1 = dr["v026resultsR1"].ToString();
                        _Screening.v027commentsR1 = dr["v027commentsR1"].ToString();
                        _Screening.v028dateR2 = dr["v028dateR2"].ToString();
                        _Screening.v029resultsR2 = dr["v029resultsR2"].ToString();
                        _Screening.v030commentsR2 = dr["v030commentsR2"].ToString();
                        _Screening.v031ReferralDate = dr["v031ReferralDate"].ToString();
                        _Screening.v032Treatment = dr["v032Treatment"].ToString();
                        _Screening.v033TreatmentComments = dr["v033TreatmentComments"].ToString();
                        _Screening.v034Completedate = dr["v034Completedate"].ToString();
                        _Screening.v035ExamStatus = dr["v035ExamStatus"].ToString();
                        _Screening.h036Date = dr["h036Date"].ToString();
                        _Screening.h037Results = dr["h037Results"].ToString();
                        _Screening.h038Comments = dr["h038Comments"].ToString();
                        _Screening.h039DateR1 = dr["h039DateR1"].ToString();
                        _Screening.h040ResultsR1 = dr["h040ResultsR1"].ToString();
                        _Screening.h041CommentsR1 = dr["h041CommentsR1"].ToString();
                        _Screening.h042DateR2 = dr["h042DateR2"].ToString();
                        _Screening.h043ResultsR2 = dr["h043ResultsR2"].ToString();
                        _Screening.h044CommentsR2 = dr["h044CommentsR2"].ToString();
                        _Screening.h045ReferralDate = dr["h045ReferralDate"].ToString();
                        _Screening.h046Treatment = dr["h046Treatment"].ToString();
                        _Screening.h047TreatmentComments = dr["h047TreatmentComments"].ToString();
                        _Screening.h048CompleteDate = dr["h048CompleteDate"].ToString();
                        _Screening.h049ExamStatus = dr["h049ExamStatus"].ToString();
                        _Screening.d050evDate = dr["d050evDate"].ToString();
                        _Screening.d051NameDEV = dr["d051NameDEV"].ToString();
                        _Screening.d052evResults = dr["d052evResults"].ToString();
                        _Screening.d053evResultsDetails = dr["d053evResultsDetails"].ToString();
                        _Screening.d054evDate2 = dr["d054evDate2"].ToString();
                        _Screening.d055evResults2 = dr["d055evResults2"].ToString();
                        _Screening.d056evReferral = dr["d056evReferral"].ToString();
                        _Screening.d057evFOStatus = dr["d057evFOStatus"].ToString();
                        _Screening.d058evComments = dr["d058evComments"].ToString();
                        _Screening.d059evTool = dr["d059evTool"].ToString();
                        _Screening.E060denDate = dr["E060denDate"].ToString();
                        _Screening.E061denResults = dr["E061denResults"].ToString();
                        _Screening.E062denPrevent = dr["E062denPrevent"].ToString();
                        _Screening.E063denReferralDate = dr["E063denReferralDate"].ToString();
                        _Screening.E064denTreatment = dr["E064denTreatment"].ToString();
                        _Screening.E065denTreatmentComments = dr["E065denTreatmentComments"].ToString();
                        _Screening.E066denTreatmentReceive = dr["E066denTreatmentReceive"].ToString();
                        _Screening.s067Date = dr["s067Date"].ToString();
                        _Screening.s068NameTCR = dr["s068NameTCR"].ToString();
                        _Screening.s069Details = dr["s069Details"].ToString();
                        _Screening.s070Results = dr["s070Results"].ToString();
                        _Screening.s071RescreenTCR = dr["s071RescreenTCR"].ToString();
                        _Screening.s072RescreenTCRDate = dr["s072RescreenTCRDate"].ToString();
                        _Screening.s073RescreenTCRResults = dr["s073RescreenTCRResults"].ToString();
                        _Screening.s074ReferralDC = dr["s074ReferralDC"].ToString();
                        _Screening.s075ReferDate = dr["s075ReferDate"].ToString();
                        _Screening.s076DCDate = dr["s076DCDate"].ToString();
                        _Screening.s077NameDC = dr["s077NameDC"].ToString();
                        _Screening.s078DetailDC = dr["s078DetailDC"].ToString();
                        _Screening.s079DCDate2 = dr["s079DCDate2"].ToString();
                        _Screening.s080DetailDC2 = dr["s080DetailDC2"].ToString();
                        _Screening.s081FOStatus = dr["s081FOStatus"].ToString();
                    }
                    //Screening changes
                    if (_dataset.Tables[14].Rows.Count > 0)
                    {
                        _Screening.AddPhysical = _dataset.Tables[14].Rows[0]["PhysicalScreening"].ToString();
                        _Screening.AddVision = _dataset.Tables[14].Rows[0]["Vision"].ToString();
                        _Screening.AddHearing = _dataset.Tables[14].Rows[0]["Hearing"].ToString();
                        _Screening.AddDental = _dataset.Tables[14].Rows[0]["Dental"].ToString();
                        _Screening.AddDevelop = _dataset.Tables[14].Rows[0]["Developmental"].ToString();
                        _Screening.AddSpeech = _dataset.Tables[14].Rows[0]["Speech"].ToString();
                        _Screening.ScreeningAcceptFileName = _dataset.Tables[14].Rows[0]["AcceptFileUl"].ToString();
                        _Screening.PhysicalFileName = _dataset.Tables[14].Rows[0]["PhyImageUl"].ToString();
                        _Screening.HearingFileName = _dataset.Tables[14].Rows[0]["HearingPicUl"].ToString();
                        _Screening.DentalFileName = _dataset.Tables[14].Rows[0]["DentalPicUl"].ToString();
                        _Screening.DevelopFileName = _dataset.Tables[14].Rows[0]["DevePicUl"].ToString();
                        _Screening.VisionFileName = _dataset.Tables[14].Rows[0]["VisionPicUl"].ToString();
                        _Screening.SpeechFileName = _dataset.Tables[14].Rows[0]["SpeechPicUl"].ToString();
                        _Screening.ParentAppID = Convert.ToInt32(_dataset.Tables[14].Rows[0]["ID"].ToString());
                        _Screening.Parentname = _dataset.Tables[14].Rows[0]["ParentName"].ToString();
                        _Screening.Consolidated = Convert.ToInt32(_dataset.Tables[14].Rows[0]["Consolidated"].ToString());

                        //Get screening scan document
                        _Screening.PhysicalImagejson = _dataset.Tables[14].Rows[0]["PhyImage"].ToString() == "" ? "" : Convert.ToBase64String((byte[])_dataset.Tables[14].Rows[0]["PhyImage"]);
                        _Screening.PhysicalFileExtension = _dataset.Tables[14].Rows[0]["PhyFileExtension"].ToString();
                        string Url = Guid.NewGuid().ToString();
                        if (_Screening.PhysicalFileName != "" && _Screening.PhysicalFileExtension == ".pdf")
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverpath + "//" + Url + ".pdf");
                            file.Write((byte[])_dataset.Tables[14].Rows[0]["PhyImage"], 0, ((byte[])_dataset.Tables[14].Rows[0]["PhyImage"]).Length);
                            file.Close();
                            _Screening.PhysicalImagejson = "/TempAttachment/" + Url + ".pdf";

                        }
                        Url = "";
                        _Screening.VisionImagejson = _dataset.Tables[14].Rows[0]["VisionPic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])_dataset.Tables[14].Rows[0]["VisionPic"]);
                        _Screening.VisionFileExtension = _dataset.Tables[14].Rows[0]["VisionFileExtension"].ToString();
                        Url = Guid.NewGuid().ToString();
                        if (_Screening.VisionFileName != "" && _Screening.VisionFileExtension == ".pdf")
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverpath + "//" + Url + ".pdf");
                            file.Write((byte[])_dataset.Tables[14].Rows[0]["VisionPic"], 0, ((byte[])_dataset.Tables[14].Rows[0]["VisionPic"]).Length);
                            file.Close();
                            _Screening.VisionImagejson = "/TempAttachment/" + Url + ".pdf";

                        }
                        Url = "";
                        _Screening.HearingImagejson = _dataset.Tables[14].Rows[0]["HearingPic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])_dataset.Tables[14].Rows[0]["HearingPic"]);
                        _Screening.HearingFileExtension = _dataset.Tables[14].Rows[0]["HearingFileExtension"].ToString();
                        Url = Guid.NewGuid().ToString();
                        if (_Screening.HearingFileName != "" && _Screening.HearingFileExtension == ".pdf")
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverpath + "//" + Url + ".pdf");
                            file.Write((byte[])_dataset.Tables[14].Rows[0]["HearingPic"], 0, ((byte[])_dataset.Tables[14].Rows[0]["HearingPic"]).Length);
                            file.Close();
                            _Screening.HearingImagejson = "/TempAttachment/" + Url + ".pdf";

                        }
                        Url = "";
                        _Screening.DevelopImagejson = _dataset.Tables[14].Rows[0]["DevePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])_dataset.Tables[14].Rows[0]["DevePic"]);
                        _Screening.DevelopFileExtension = _dataset.Tables[14].Rows[0]["DeveFileExtension"].ToString();
                        Url = Guid.NewGuid().ToString();
                        if (_Screening.DevelopFileName != "" && _Screening.DevelopFileExtension == ".pdf")
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverpath + "//" + Url + ".pdf");
                            file.Write((byte[])_dataset.Tables[14].Rows[0]["DevePic"], 0, ((byte[])_dataset.Tables[14].Rows[0]["DevePic"]).Length);
                            file.Close();
                            _Screening.DevelopImagejson = "/TempAttachment/" + Url + ".pdf";

                        }
                        Url = "";
                        _Screening.DentalImagejson = _dataset.Tables[14].Rows[0]["DentalPic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])_dataset.Tables[14].Rows[0]["DentalPic"]);
                        _Screening.DentalFileExtension = _dataset.Tables[14].Rows[0]["DentalPicExtension"].ToString();
                        Url = Guid.NewGuid().ToString();
                        if (_Screening.DentalFileName != "" && _Screening.DentalFileExtension == ".pdf")
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverpath + "//" + Url + ".pdf");
                            file.Write((byte[])_dataset.Tables[14].Rows[0]["DentalPic"], 0, ((byte[])_dataset.Tables[14].Rows[0]["DentalPic"]).Length);
                            file.Close();
                            _Screening.DentalImagejson = "/TempAttachment/" + Url + ".pdf";

                        }
                        Url = "";
                        _Screening.SpeechImagejson = _dataset.Tables[14].Rows[0]["SpeechPic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])_dataset.Tables[14].Rows[0]["SpeechPic"]);
                        _Screening.SpeechFileExtension = _dataset.Tables[14].Rows[0]["SpeechFileExtension"].ToString();

                        Url = Guid.NewGuid().ToString();
                        if (_Screening.SpeechFileName != "" && _Screening.SpeechFileExtension == ".pdf")
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverpath + "//" + Url + ".pdf");
                            file.Write((byte[])_dataset.Tables[14].Rows[0]["SpeechPic"], 0, ((byte[])_dataset.Tables[14].Rows[0]["SpeechPic"]).Length);
                            file.Close();
                            _Screening.SpeechImagejson = "/TempAttachment/" + Url + ".pdf";

                        }




                        //END




                    }
                    obj._Screening = _Screening;
                }
                if (_dataset.Tables[13].Rows.Count > 0)
                {
                    List<FamilyHousehold.ImmunizationRecord> ImmunizationRecords = new List<FamilyHousehold.ImmunizationRecord>();
                    FamilyHousehold.ImmunizationRecord obj1;
                    foreach (DataRow dr in _dataset.Tables[13].Rows)
                    {
                        obj1 = new FamilyHousehold.ImmunizationRecord();
                        obj1.ImmunizationId = Convert.ToInt32(dr["Immunization_ID"]);
                        obj1.ImmunizationmasterId = Convert.ToInt32(dr["Immunizationmasterid"]);
                        obj1.Dose = dr["Dose"].ToString();
                        if (dr["Dose1"].ToString() != "")
                            obj1.Dose1 = Convert.ToDateTime(dr["Dose1"]).ToString("MM/dd/yyyy");
                        else
                            obj1.Dose1 = dr["Dose1"].ToString();
                        if (dr["Dose2"].ToString() != "")
                            obj1.Dose2 = Convert.ToDateTime(dr["Dose2"]).ToString("MM/dd/yyyy");
                        else
                            obj1.Dose2 = dr["Dose2"].ToString();
                        if (dr["Dose3"].ToString() != "")
                            obj1.Dose3 = Convert.ToDateTime(dr["Dose3"]).ToString("MM/dd/yyyy");
                        else
                            obj1.Dose3 = dr["Dose3"].ToString();
                        if (dr["Dose4"].ToString() != "")
                            obj1.Dose4 = Convert.ToDateTime(dr["Dose4"]).ToString("MM/dd/yyyy");
                        else
                            obj1.Dose4 = dr["Dose4"].ToString();
                        if (dr["Dose5"].ToString() != "")
                            obj1.Dose5 = Convert.ToDateTime(dr["Dose5"]).ToString("MM/dd/yyyy");
                        else
                            obj1.Dose5 = dr["Dose5"].ToString();
                        if (dr["Exempt1"].ToString() != "")
                            obj1.Exempt1 = Convert.ToBoolean(dr["Exempt1"]);
                        if (dr["Exempt2"].ToString() != "")
                            obj1.Exempt2 = Convert.ToBoolean(dr["Exempt2"]);
                        if (dr["Exempt3"].ToString() != "")
                            obj1.Exempt3 = Convert.ToBoolean(dr["Exempt3"]);
                        if (dr["Exempt4"].ToString() != "")
                            obj1.Exempt4 = Convert.ToBoolean(dr["Exempt4"]);
                        if (dr["Exempt5"].ToString() != "")
                            obj1.Exempt5 = Convert.ToBoolean(dr["Exempt5"]);
                        if (dr["Preemptive1"].ToString() != "")
                            obj1.Preempt1 = Convert.ToBoolean(dr["Preemptive1"]);
                        if (dr["Preemptive2"].ToString() != "")
                            obj1.Preempt2 = Convert.ToBoolean(dr["Preemptive2"]);
                        if (dr["Preemptive3"].ToString() != "")
                            obj1.Preempt3 = Convert.ToBoolean(dr["Preemptive3"]);
                        if (dr["Preemptive4"].ToString() != "")
                            obj1.Preempt4 = Convert.ToBoolean(dr["Preemptive4"]);
                        if (dr["Preemptive5"].ToString() != "")
                            obj1.Preempt5 = Convert.ToBoolean(dr["Preemptive5"]);
                        ImmunizationRecords.Add(obj1);
                        obj1 = null;
                    }
                    obj.ImmunizationRecords = ImmunizationRecords;
                }







                if (_dataset.Tables[16] != null && _dataset.Tables[16].Rows.Count > 0)
                {
                    List<Nurse.Childcustomscreening> _Childcustomscreenings = new List<Nurse.Childcustomscreening>();
                    Nurse.Childcustomscreening info = null;
                    foreach (DataRow dr in _dataset.Tables[16].Rows)
                    {
                        info = new Nurse.Childcustomscreening();
                        info.QuestionID = dr["QuestionID"].ToString();
                        info.Screeningid = dr["Screeningid"].ToString();
                        info.Value = dr["Value"].ToString();
                        info.QuestionAcronym = dr["QuestionAcronym"].ToString();
                        info.optionid = dr["optionid"].ToString();
                        info.ScreeningDate = dr["ScreeningDate"].ToString() != "" ? Convert.ToDateTime(dr["ScreeningDate"]).ToString("MM/dd/yyyy") : "";
                        string Url = Guid.NewGuid().ToString();
                        if (dr["DocumentName"].ToString() != "" && dr["DocumentExtension"].ToString() == ".pdf")
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverpath + "//" + Url + ".pdf");
                            file.Write((byte[])dr["Documentdata"], 0, ((byte[])dr["Documentdata"]).Length);
                            file.Close();
                            info.pdfpath = "/TempAttachment/" + Url + ".pdf";
                        }
                        else
                        {
                            info.pdfpath = "";
                            info.Documentdata = dr["Documentdata"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["Documentdata"]);
                        }
                        _Childcustomscreenings.Add(info);
                    }
                    obj._childscreenings = _Childcustomscreenings;
                }

                if (_dataset.Tables[17] != null && _dataset.Tables[17].Rows.Count > 0)
                {
                    List<CustomScreeningAllowed> _CustomScreeningAllowed = new List<CustomScreeningAllowed>();
                    CustomScreeningAllowed info = null;
                    foreach (DataRow dr in _dataset.Tables[17].Rows)
                    {
                        info = new CustomScreeningAllowed();
                        info.ScreeningAllowed = dr["Screeningallowed"].ToString();
                        info.Screeningid = dr["Screeningid"].ToString();
                        info.ScreeningName = dr["screeningname"].ToString();
                        _CustomScreeningAllowed.Add(info);
                    }
                    obj._CustomScreeningAlloweds = _CustomScreeningAllowed;
                }




                DataAdapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return obj;
        }


        public List<Roster> ClientLists(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize)
        {

            List<Roster> RosterList = new List<Roster>();
            try
            {
                totalrecord = string.Empty;
                command.Parameters.Add(new SqlParameter("@Search", search));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@Roleid", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_clientscreeningList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            info = new Roster();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.CenterName = dr["CenterName"].ToString();
                            info.ProgramType = dr["ProgramType"].ToString();
                            info.ProgramId = dr["ReferenceProg"].ToString();
                            // info.ProgramId = "";
                            info.ClassroomName = dr["ClassroomName"].ToString();
                            info.DOB = Convert.ToString(dr["DOB"]);
                            RosterList.Add(info);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }
        //public List<SelectListItem> GetScreening(string clientid, string programid, string agencyid, string Userid,string roleid)
        //{
        //    List<SelectListItem> _screening = new List<SelectListItem>();
        //    try
        //    {
        //        command.Connection = Connection;
        //        command.Parameters.Add(new SqlParameter("@programid", programid));
        //        command.Parameters.Add(new SqlParameter("@UserId", Userid));
        //        command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
        //        command.Parameters.Add(new SqlParameter("@roleid", roleid));
        //        command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64(clientid)));
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "SP_GetScreeningClient";
        //        DataAdapter = new SqlDataAdapter(command);
        //        _dataTable = new DataTable();
        //        DataAdapter.Fill(_dataTable);
        //        if (_dataTable != null && _dataTable.Rows.Count > 0)
        //        {
        //            SelectListItem obj = null;
        //            foreach (DataRow dr in _dataTable.Rows)
        //            {
        //                obj = new SelectListItem();
        //                obj.Value = dr["ScreeningID"].ToString();
        //                obj.Text = dr["ScreeningName"].ToString();
        //                _screening.Add(obj);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //        return _screening;

        //    }
        //    finally
        //    {
        //        if (Connection != null)
        //            Connection.Close();
        //    }
        //    return _screening;
        //}


        public Screening GetScreening(string clientid, string programid)
        {
            Screening _screening = new Screening();
            try
            {


                _screening.ScreeningList = new List<ScreeningNew>();
                _screening.ScreeningPeriodsList = new List<ScreeningPeriods>();
                _screening.ChildInfo = new ChildrenInfo();

                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }


                using (Connection = connection.returnConnection())
                {


                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@programid", programid));
                    command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@roleid", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64(clientid)));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_GetScreeningClient";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                }

                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                    {
                        _screening.ScreeningList = (from DataRow dr in _dataset.Tables[0].Rows
                                                    select new ScreeningNew
                                                    {
                                                        ScreeningID = Convert.ToInt32(dr["ScreeningID"]),
                                                        ScreeningName = Convert.ToString(dr["ScreeningName"]),
                                                        Approved = Convert.ToInt32(dr["Approved"])

                                                    }
                                                  ).ToList();



                    }

                    //if(_dataset.Tables[1]!=null && _dataset.Tables[1].Rows.Count>0)
                    //{
                    //    _screening.ScreeningPeriodsList = (from DataRow dr2 in _dataset.Tables[1].Rows
                    //                                       select new ScreeningPeriods
                    //                                       {
                    //                                           ScreeningPeriod = Convert.ToInt32(dr2["ScreeningPeriod"]),
                    //                                           ScreeningPeriodType = Convert.ToInt32(dr2["ScreeningPeriodType"]),
                    //                                           Description = Convert.ToString(dr2["Description"])
                    //                                       }

                    //                                    ).OrderByDescending(x=>x.ScreeningPeriod).ToList();

                    //}

                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        _screening.ChildInfo.ClientId = Convert.ToString(_dataset.Tables[1].Rows[0]["ClientID"]);
                        _screening.ChildInfo.ClientName = Convert.ToString(_dataset.Tables[1].Rows[0]["ClientName"]);
                        _screening.ChildInfo.Enc_ClientId = EncryptDecrypt.Encrypt64(Convert.ToString(_dataset.Tables[1].Rows[0]["ClientID"]));
                        _screening.ChildInfo.CenterName = Convert.ToString(_dataset.Tables[1].Rows[0]["CenterName"]);
                        _screening.ChildInfo.ClassRoomName = Convert.ToString(_dataset.Tables[1].Rows[0]["ClassRoomName"]);
                        _screening.ChildInfo.ProgramType = Convert.ToString(_dataset.Tables[1].Rows[0]["ProgRef"]);
                        _screening.ChildInfo.Dob = Convert.ToString(_dataset.Tables[1].Rows[0]["DOB"]);
                        _screening.ChildInfo.Age = Convert.ToString(_dataset.Tables[1].Rows[0]["AgeByCurrentDate"]);
                        _screening.ChildInfo.AgeInWords = Convert.ToString(_dataset.Tables[1].Rows[0]["LDAAgeInWordsCurrentDate"]);
                        _screening.ChildInfo.AgeInMonths = Convert.ToInt32(_dataset.Tables[1].Rows[0]["AgeInMonths"]);
                    }

                }



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _screening;

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _screening;
        }



        public DataTable GetScreeningTemplate(string ScreeningId, string agencyid, string Userid)
        {
            try
            {
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@ScreeningId", ScreeningId));
                command.Parameters.Add(new SqlParameter("@UserId", Userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetScreeningtemplate";
                DataAdapter = new SqlDataAdapter(command);
                _dataTable = new DataTable();
                DataAdapter.Fill(_dataTable);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _dataTable;

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _dataTable;

        }
        public DataSet GetScreeningsbyid(string clientid, string screeningid, string agencyid, string Userid, string programid, string roleid)
        {
            
            try
            {
                command.Connection = Connection;

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@screeningid", screeningid));
                command.Parameters.Add(new SqlParameter("@clientid", EncryptDecrypt.Decrypt64(clientid)));
                command.Parameters.Add(new SqlParameter("@UserId", Userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@programid", programid));
                command.Parameters.Add(new SqlParameter("@roleid", roleid));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Screenings";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _dataset;

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _dataset;
        }



        public Screening GetScreeningsbyid2(string clientid, string screeningid, string agencyid, string Userid, string programid, string roleid, string serverPath)
        {

            Screening screening = new Screening();
            try
            {
            
                screening.ScreeningList = new List<ScreeningNew>();
                screening.ScreeningPeriodsList = new List<ScreeningPeriods>();
                screening.ChildInfo = new ChildrenInfo();
                StringBuilder urlString = new StringBuilder();
                screening.ScreeningAccessInfo = new ScreeningAccess();

                urlString.Clear();


                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }


                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@screeningid", screeningid));
                    command.Parameters.Add(new SqlParameter("@clientid", EncryptDecrypt.Decrypt64(clientid)));
                    command.Parameters.Add(new SqlParameter("@UserId", Userid));
                    command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                    command.Parameters.Add(new SqlParameter("@programid", programid));
                    command.Parameters.Add(new SqlParameter("@roleid", roleid));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_Screenings";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null && _dataset.Tables.Count > 0)
                {


                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        screening.ClientID = Convert.ToInt16(_dataset.Tables[0].Rows[0]["ClientID"]);
                        // screening.Parentname = Convert.ToString(_dataset.Tables[0].Rows[0]["ParentName"]);
                        screening.Consolidated = Convert.ToInt16(_dataset.Tables[0].Rows[0]["Consolidated"]);
                        screening.TypeScreening = Convert.ToInt16(programid);

                        screening.ParentSignature = String.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["ParentSignature"].ToString()) ? "" : Convert.ToString(_dataset.Tables[0].Rows[0]["ParentSignature"]);
                        screening.ScreeningAcceptImagejson = string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["AcceptanceForm"].ToString()) ? "" : Convert.ToBase64String((byte[])_dataset.Tables[0].Rows[0]["AcceptanceForm"]);
                        screening.ScreeningAcceptFileExtension = Convert.ToString(_dataset.Tables[0].Rows[0]["AcceptFileExtension"]);
                        screening.ScreeningAcceptFileName = Convert.ToString(_dataset.Tables[0].Rows[0]["AcceptFileUl"]);
                        urlString.Clear().Append(Guid.NewGuid().ToString());

                        if (!string.IsNullOrEmpty(screening.ScreeningAcceptFileName))
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverPath + "//" + urlString + screening.ScreeningAcceptFileExtension);
                            file.Write((byte[])_dataset.Tables[0].Rows[0]["AcceptanceForm"], 0, ((byte[])_dataset.Tables[0].Rows[0]["AcceptanceForm"]).Length);
                            file.Close();
                            screening.ScreeningAcceptImageUrl = "/TempAttachment/" + urlString + screening.ScreeningAcceptFileExtension;

                        }
                        screening.ParentAppID = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ID"]);


                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            ScreeningNew newScreening = new ScreeningNew();
                            newScreening.ScreeningID = Convert.ToInt16(dr["ScreeningID"]);
                            newScreening.ScreeningName = Convert.ToString(dr["ScreeningName"]);
                            newScreening.ScreeningsPerYear = Convert.ToInt32(dr["ScreeningsPerYear"]);
                            newScreening.ParentAppID = Convert.ToInt32(dr["ID"]);
                            newScreening.ApprovedFileName = Convert.ToString(dr["ApprovalFileUl"]);
                            newScreening.ApprovedFileExtension = Convert.ToString(dr["ApprovalFileExtension"]);
                            newScreening.Approved = Convert.ToInt16(dr["Approved"]);
                            urlString.Clear().Append(Guid.NewGuid().ToString());

                            newScreening.ApprovedImageJson = string.IsNullOrEmpty(dr["ApprovalForm"].ToString()) ? "" : Convert.ToBase64String((byte[])dr["ApprovalForm"]);

                            if (!string.IsNullOrEmpty(newScreening.ApprovedFileName))
                            {
                                System.IO.FileStream file = System.IO.File.Create(serverPath + "//" + urlString + newScreening.ApprovedFileExtension);
                                file.Write((byte[])dr["ApprovalForm"], 0, ((byte[])dr["ApprovalForm"]).Length);
                                file.Close();
                                newScreening.ApprovedImageUrl = "/TempAttachment/" + urlString + newScreening.ApprovedFileExtension;
                            }
                            newScreening.NoDocument = Convert.ToBoolean(dr["NoDocument"]);
                            newScreening.LastScreeningCompleted = Convert.ToInt32(dr["LastScreeningCompleted"]);


                            screening.ScreeningList.Add(newScreening);

                        }



                    }


                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {

                        int lastScreening = 0;
                        if (screening.ScreeningList.Count > 0)
                        {
                            lastScreening = screening.ScreeningList.Where(x => Convert.ToString(x.ScreeningID) == screeningid).Select(x => x.LastScreeningCompleted).FirstOrDefault();
                        }

                        screening.ScreeningPeriodsList = (from DataRow dr2 in _dataset.Tables[1].Rows
                                                          select new ScreeningPeriods
                                                          {
                                                              ScreeningPeriodIndex = Convert.ToInt32(dr2["ScreeningPeriodIndex"]),
                                                              ScreeningPeriod = Convert.ToDouble(dr2["ScreeningPeriod"]),
                                                              ScreeningPeriodType = Convert.ToInt32(dr2["ScreeningPeriodType"]),
                                                              Description = Convert.ToString(dr2["Description"]),
                                                              ScreeningPeriodFor = Convert.ToInt32(dr2["ScreeningPeriodFor"]),
                                                              ScreeningAge = Convert.ToDouble(dr2["ScreeningAge"]),
                                                              //ScreeningFocusType = Convert.ToInt32(dr2["ScreeningFocusType"]),
                                                              CustomScreeningPeriod = Convert.ToInt32(dr2["CustomScreeningPeriod"])
                                                          }

                                                      ).ToList();




                    }


                    screening.ChildInfo = new ChildrenInfo();

                    if (_dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
                    {
                        screening.ChildInfo.AgeInMonths = Convert.ToInt32(_dataset.Tables[2].Rows[0]["AgeInMonths"]);
                        screening.ChildInfo.Age = Convert.ToString(_dataset.Tables[2].Rows[0]["AgeInYears"]);
                        screening.ChildInfo.Dob = Convert.ToString(_dataset.Tables[2].Rows[0]["ClientDOB"]);
                    }

                    if (_dataset.Tables.Count > 3 && _dataset.Tables[3] != null && _dataset.Tables[3].Rows.Count > 0)
                    {
                        screening.ScreeningAccessInfo = (from DataRow dr4 in _dataset.Tables[3].Rows
                                                         select new ScreeningAccess
                                                         {
                                                             IsEnter = Convert.ToBoolean(dr4["EnterScreening"]),
                                                             IsReview = Convert.ToBoolean(dr4["ReviewScreening"]),
                                                             IsViewOnly = Convert.ToBoolean(dr4["ViewScreening"])
                                                         }
                                                       ).FirstOrDefault();
                    }



                    if (screening.ScreeningPeriodsList.Count > 0)
                    {
                        var clientAge = Convert.ToDouble(_dataset.Tables[1].Rows[0]["AgeClient"]);
                        var screeningAge = 0.0;
                        if (screening.ScreeningPeriodsList.Where(x => x.ScreeningAge <= clientAge).Any())
                        {
                             screeningAge = screening.ScreeningPeriodsList.Where(x => x.ScreeningAge <= clientAge).Max(x => x.ScreeningAge);
                        }
                       

                        DateTimeFormatInfo usDtfi = new CultureInfo("en-US", false).DateTimeFormat;
                        var convDob = Convert.ToDateTime(screening.ChildInfo.Dob, usDtfi);
                        var clientAgeModel = new Age(convDob, DateTime.Now);
                        var expirInPeriod =(programid=="1")?2: Convert.ToInt32(_dataset.Tables[0].Rows[0]["ExpireIn"]);
                        var expiredPeriod = (programid == "1") ? 3: Convert.ToInt32(_dataset.Tables[0].Rows[0]["ExpiredPeriod"]);
                        var nextScreeningAge = screening.ScreeningPeriodsList.OrderBy(x=>x.ScreeningAge).Where(x => x.ScreeningAge > screeningAge).Select(x=>x.ScreeningAge).FirstOrDefault();
                        nextScreeningAge = string.IsNullOrEmpty(nextScreeningAge.ToString()) ? 0 : nextScreeningAge;
                        var expireInMonths = 0.0;
                        

                        switch(expirInPeriod)
                        {
                            case 1:
                                expireInMonths = 0.5;
                                break;
                            case 2:
                                expireInMonths = 1.0;
                                break;
                            case 3:
                                expireInMonths = 2.0;
                                break;
                            default:
                                expireInMonths = 0.0;
                                break;

                        }

                        var expiredPeriodMonths = 0.0;

                        switch (expiredPeriod)
                        {
                            case 1:
                                expiredPeriodMonths = 3.0;
                                break;
                            case 2:
                                expiredPeriodMonths = 6.0;
                                break;
                            case 3:
                                expiredPeriodMonths = 12.0;
                                break;
                            case 4:
                                expiredPeriodMonths = 13.0;
                                break;
                            case 5:
                                expiredPeriodMonths = 0.0;
                                break;
                            default:
                                expiredPeriodMonths = 0.0;
                                break;

                        }


                       
                            for (int i = 0; i < screening.ScreeningPeriodsList.Count; i++)
                            {
                                if (screening.ScreeningPeriodsList[i].ScreeningAge < screeningAge)
                                {
                                    screening.ScreeningPeriodsList[i].ScreeningFocusType = 1;
                                    screening.ScreeningPeriodsList[i].ScreeningExpirationType = (int)FingerprintsModel.Enums.ScreeningExpiration.Expired;
                                }

                                if (screening.ScreeningPeriodsList[i].ScreeningAge == screeningAge)
                                {
                                    screening.ScreeningPeriodsList[i].ScreeningFocusType = 2;

                                    if((double)clientAgeModel.Months>= expiredPeriodMonths)
                                    {
                                        screening.ScreeningPeriodsList[i].ScreeningExpirationType = (int)FingerprintsModel.Enums.ScreeningExpiration.Expired;
                                    }
                                //else if((double)clientAgeModel.Months < expiredPeriodMonths && (Math.Ceiling(Convert.ToDouble(screening.ChildInfo.Age)) -Convert.ToDouble(screening.ChildInfo.Age))/12<=expireInMonths )
                                //{
                                //    screening.ScreeningPeriodsList[i].ScreeningExpirationType = (int)FingerprintsModel.Enums.ScreeningExpiration.Expiring;

                                //}

                                else if ((double)clientAgeModel.Months < expiredPeriodMonths && (nextScreeningAge - clientAge) * 12 <= expireInMonths)
                                {
                                    screening.ScreeningPeriodsList[i].ScreeningExpirationType = (int)FingerprintsModel.Enums.ScreeningExpiration.Expiring;

                                }

                                else
                                    {
                                        screening.ScreeningPeriodsList[i].ScreeningExpirationType = (int)FingerprintsModel.Enums.ScreeningExpiration.Current;
                                    }

                                   
                                   
                                }

                                if (screening.ScreeningPeriodsList[i].ScreeningAge > screeningAge)
                                {
                                    screening.ScreeningPeriodsList[i].ScreeningFocusType = 3;
                                    screening.ScreeningPeriodsList[i].ScreeningExpirationType = (int)FingerprintsModel.Enums.ScreeningExpiration.Current;
                                }
                            }
                       

                        

                    }


                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return screening;

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return screening;
        }


        public DataSet savecustomscreening(ref string message, Screening _screen, FormCollection _Collections, string ScreeningDate, string Status, string agencyid, string Userid, HttpPostedFileBase ScreeningDocument, string Programid, string roleid)
        {
            try
            {
                string HouseholdId = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                
                #region screening
                DataTable dt1 = new DataTable();
                dt1.Columns.AddRange(new DataColumn[4] { 
                    new DataColumn("ScreeningID",typeof(Int32)), 
                    new DataColumn("QuestionID",typeof(Int32)), 
                    new DataColumn("Value",typeof(string)),
                    new DataColumn("OptionID",typeof(Int32)), 
                    });
                #endregion

                //Fetching data for fixed screening
                //physical screening
                if(_screen.Screeningid==1)
                {
                    int questionid = 0;
                    foreach (var s in _screen.GetType().GetProperties().Where(s => s.Name.Substring(0,1) == "F"))
                    {
                        questionid = Convert.ToInt32(s.Name.Substring(1, 3));
                        dt1.Rows.Add(_screen.Screeningid, questionid, s.GetValue(_screen),0);
                    }
                    ScreeningDate = _screen.F001physicalDate;

                }
                if (_screen.Screeningid == 2)
                {
                    int questionid = 0;
                    foreach (var s in _screen.GetType().GetProperties().Where(s => s.Name.Substring(0, 1) == "v"))
                    {
                        questionid = Convert.ToInt32(s.Name.Substring(1, 3));
                        dt1.Rows.Add(_screen.Screeningid, questionid, s.GetValue(_screen),DBNull.Value);
                    }
                    ScreeningDate = _screen.v022date;
                }
                if (_screen.Screeningid == 3)
                {
                    int questionid = 0;
                    foreach (var s in _screen.GetType().GetProperties().Where(s => s.Name.Substring(0, 1) == "h"))
                    {
                        questionid = Convert.ToInt32(s.Name.Substring(1, 3));
                        dt1.Rows.Add(_screen.Screeningid, questionid, s.GetValue(_screen), DBNull.Value);
                    }
                    ScreeningDate = _screen.h036Date;
                }
                if (_screen.Screeningid == 4)
                {
                    int questionid = 0;
                    foreach (var s in _screen.GetType().GetProperties().Where(s => s.Name.Substring(0, 1) == "d"))
                    {
                        questionid = Convert.ToInt32(s.Name.Substring(1, 3));
                        dt1.Rows.Add(_screen.Screeningid, questionid, s.GetValue(_screen), DBNull.Value);
                    }
                    ScreeningDate = _screen.d050evDate;
                }
                if (_screen.Screeningid == 5)
                {
                    int questionid = 0;
                    foreach (var s in _screen.GetType().GetProperties().Where(s => s.Name.Substring(0, 1) == "E"))
                    {
                        questionid = Convert.ToInt32(s.Name.Substring(1, 3));
                        dt1.Rows.Add(_screen.Screeningid, questionid, s.GetValue(_screen), DBNull.Value);
                    }
                    ScreeningDate = _screen.E060denDate;
                }
                if (_screen.Screeningid == 6)
                {
                    int questionid = 0;
                    foreach (var s in _screen.GetType().GetProperties().Where(s => s.Name.Substring(0, 1) == "s"))
                    {
                        questionid = Convert.ToInt32(s.Name.Substring(1, 3));
                        dt1.Rows.Add(_screen.Screeningid, questionid, s.GetValue(_screen), DBNull.Value);
                    }
                    ScreeningDate = _screen.s067Date;
                }
                if (_screen.Screeningid >6)
                {
                    if(_Collections !=null)
                    {
                        foreach (var key in _Collections.AllKeys)
                        {
                            string questionid = string.Empty;
                            string optionid = string.Empty;
                            if (key.ToString() != "screeningdate" && key.ToString() != "Status")
                            {
                                if (key.ToString().Contains("o") || key.ToString().Contains("k"))
                                questionid = key.ToString().Split('k', 'k')[2];
                                if(key.ToString().Contains("o"))
                                {
                                    optionid = key.ToString().Split('o', 'o')[1];
                                    questionid = key.ToString().Split('k', 'k')[2];
                                }
                                if (key.ToString().Contains("R"))
                                {
                                    optionid = _Collections[key].ToString().Split('o', 'o')[1];
                                    questionid = _Collections[key].ToString().Split('k', 'k')[2];
                                }
                                if(string.IsNullOrEmpty(optionid))
                                    dt1.Rows.Add(_screen.Screeningid, questionid, _Collections[key].ToString(), DBNull.Value);
                                else
                                    dt1.Rows.Add(_screen.Screeningid, questionid, DBNull.Value, optionid);
                                optionid = "";
                                questionid = "";
                            }
                            if (key.ToString().Contains("ScreeningDate"))
                                ScreeningDate = _Collections[key].ToString();
                        }

                       
                    }
                }
                //End
                string filename = null;
                string fileextension = null;
                byte[] filedata = null;
                if(ScreeningDocument!=null)
                {
                    filename = ScreeningDocument.FileName;
                    fileextension = Path.GetExtension(ScreeningDocument.FileName);
                    filedata = new BinaryReader(ScreeningDocument.InputStream).ReadBytes(ScreeningDocument.ContentLength);
                }
                command.Parameters.Add(new SqlParameter("@ClientID", _screen.ClientID));
                command.Parameters.Add(new SqlParameter("@Screeningid", _screen.Screeningid));
                command.Parameters.Add(new SqlParameter("@ScreeningDate", ScreeningDate));
                command.Parameters.Add(new SqlParameter("@Status", Status));
                command.Parameters.Add(new SqlParameter("@tblscreening", dt1));
                command.Parameters.Add(new SqlParameter("@filename", filename));
                command.Parameters.Add(new SqlParameter("@fileextension", fileextension));
                command.Parameters.Add(new SqlParameter("@filedata", filedata));
                command.Parameters.Add(new SqlParameter("@Userid", Userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@Programid", Programid));
                command.Parameters.Add(new SqlParameter("@roleid", roleid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_CustScreeningQuestion";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                DataAdapter.Dispose();
                command.Dispose();
                message = command.Parameters["@result"].Value.ToString();
            

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                if(Connection!=null)
                Connection.Close();
                command.Dispose();
            }
            return _dataset;

        }
        //Changes on 30Dec2016
        public Roster Getchildscreeningroster(string centerid, string Classroom, string userid, string agencyid,string RoleId)
        {
            List<Roster> RosterList = new List<Roster>();
            Roster _roster = new Roster();
            List<ClassRoom> classList = new List<ClassRoom>();
            try
            {
                int center=0;
                if(int.TryParse(centerid,out center))
                {
                }
                else
                {
                    center=Convert.ToInt32( EncryptDecrypt.Decrypt64(centerid));
                }
                command.Parameters.Add(new SqlParameter("@Classroom", Classroom));
                command.Parameters.Add(new SqlParameter("@Center", center));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleId", RoleId));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getchildscreeningroster";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Roster();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.DOB = Convert.ToString(dr["dob"]);
                            info.Gender = dr["gender"].ToString();
                            info.CenterName = dr["CenterName"].ToString();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString());
                            info.ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString());
                            info.ClassroomName = dr["ClassroomName"].ToString();
                            info.FSW = dr["fswname"].ToString();
                            info.Teacher = dr["teacher"].ToString();//DBNull.Value.Equals(dr["ChildTransport"])
                            info.IsPresent = DBNull.Value.Equals(dr["IsPresent"]) ? 0 : Convert.ToInt32(dr["IsPresent"]);//.ToString() //Added on 30Dec2016
                            info.Dayscount = dr["dayscount"].ToString();
                           // info.Picture = dr["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePic"]);
                            RosterList.Add(info);


                        }
                        _roster.Rosters = RosterList;//Changes on 29Dec2016
                    }
                }
                //Changes on 29Dec2016
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    ClassRoom info = null;
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        info = new ClassRoom();
                        info.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                        info.ClassName = dr["ClassroomName"].ToString();
                        classList.Add(info);
                    }
                    _roster.ClassroomsNurse = classList;
                }

                //End
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _roster;

        }
        public List<Nurse.NurseScreening> Getchildscreeningcenter(StaffDetails staff, string centerid)
        {
            List<Nurse.NurseScreening> List = new List<Nurse.NurseScreening>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getchildscreeningcenter";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Nurse.NurseScreening info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Nurse.NurseScreening();
                            info.Screeningid = dr["screeningid"].ToString()!=""?EncryptDecrypt.Encrypt64(dr["screeningid"].ToString()):"";
                            info.Screeningname = dr["ScreeningName"].ToString();
                            info.Missingcount = dr["missingscreening"].ToString();
                            List.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return List;

        }
        public ScreeningMatrix Getallchildmissingscreening(StaffDetails staff, string centerid, string ClassRoom)
        {
            List<List<string>> List = new List<List<string>>();
            ScreeningMatrix ScreeningMatrix = new ScreeningMatrix();
            List<ClassRoom> Classlist = new List<ClassRoom>();
            List<Roster> Rosterlist = new List<Roster>();
            try
            {

                if(Connection.State==ConnectionState.Open)
                {
                    Connection.Close();
                }

                using (Connection = connection.returnConnection())
                {

               
                    command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                if (!string.IsNullOrEmpty(ClassRoom))
                    command.Parameters.Add(new SqlParameter("@ClassRoom", ClassRoom));
                else
                    command.Parameters.Add(new SqlParameter("@ClassRoom", DBNull.Value));
                    command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getchildmissingscreeningcenter";
                    Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                }


                if (_dataset!=null  && _dataset.Tables.Count>0)
                {
                    if (_dataset.Tables[0]!=null && _dataset.Tables[0].Rows.Count > 0)
                    {
                        List<string> column = new List<string>();
                        foreach(DataColumn dc in _dataset.Tables[0].Columns)
                        {
                            column.Add(dc.ColumnName);
                        }
                        List.Add(column);
                        for(int i=0;i<_dataset.Tables[0].Rows.Count;i++)
                        {
                            List<string> row = new List<string>();
                            for (int j = 0; j < _dataset.Tables[0].Columns.Count; j++)
                            {
                                row.Add(_dataset.Tables[0].Rows[i][j].ToString());
                            }
                            List.Add(row);
                        }
                        ScreeningMatrix.Screenings = List;
                    }


                    if (_dataset.Tables.Count>1 && _dataset.Tables[1]!=null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        ClassRoom Class = null;
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {

                         Class= new ClassRoom();
                         Class.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                         Class.ClassName = dr["ClassroomName"].ToString();
                         Classlist.Add(Class);
                        }
                        ScreeningMatrix.Classroom = Classlist;
                    }
                    //if (_dataset.Tables[2].Rows.Count > 0)
                    //{
                    //    Roster Roster = null;
                    //    foreach (DataRow dr in _dataset.Tables[2].Rows)
                    //    {
                    //        Roster = new Roster();
                    //        Roster.Eclientid = dr["clientid"].ToString();
                    //        Roster.Name = dr["name"].ToString();
                    //        Roster.CenterName = dr["centername"].ToString();
                    //        Roster.ClassroomName = dr["ClassroomName"].ToString();
                    //        Roster.ScreeningName = dr["ScreeningName"].ToString();
                    //        Rosterlist.Add(Roster);
                    //    }
                    //    ScreeningMatrix.ClientsClassroom = Rosterlist;
                    //}
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return ScreeningMatrix;

        }
        public DataSet GetmultiScreening(string ScreeningId,string centerid ,string agencyid, string Userid,string RoleId)
        {
            try
            {
                
                command.Connection = Connection;
                string[] screenings = null;
                StringBuilder _stringnew = new StringBuilder();
                if(!string.IsNullOrEmpty(ScreeningId))
                {
                   screenings=  ScreeningId.Split(',');
                   foreach (string str in screenings)
                   {
                       _stringnew.Append(EncryptDecrypt.Decrypt64(str) + ",");
                   }
                   ScreeningId = _stringnew.ToString().Substring(0, _stringnew.Length - 1);
                }
                command.Parameters.Add(new SqlParameter("@ScreeningId", ScreeningId));
                command.Parameters.Add(new SqlParameter("@centerid", centerid));
                command.Parameters.Add(new SqlParameter("@UserId", Userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetmultiScreening";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _dataset;

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _dataset;

        }
        public List<Nurse.clients> Loadallclientscreening(string Classroomid, string Centerid, string Screeningid, string userid, string agencyid,string Roleid)
        {
            List<Nurse.clients> List = new List<Nurse.clients>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Classroomid", Classroomid));
                command.Parameters.Add(new SqlParameter("@Centerid", Centerid));
                command.Parameters.Add(new SqlParameter("@Screeningid", Screeningid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@Roleid", Roleid));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetExeptionclientScreening";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Nurse.clients info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Nurse.clients();
                            List<Nurse.ScreeningStatus> ListStatus = new List<Nurse.ScreeningStatus>();
                            info.clientid = dr["ClientID"].ToString();
                            info.Screeningid = dr["screeningid"].ToString();
                            info.name = dr["ClientName"].ToString();
                            info.ScreeningDateQuestionID = dr["ScreeningDateQuestionID"].ToString();
                            info.DOB = dr["DOB"].ToString();
                            if (_dataset.Tables.Count >1 &&  _dataset.Tables[1] !=null)
                            {
                                Nurse.ScreeningStatus ScreeningStatus = null;
                                foreach (DataRow statusrows in _dataset.Tables[1].Rows)
                                {
                                    ScreeningStatus = new Nurse.ScreeningStatus();
                                    ScreeningStatus.Optionid = statusrows["optionid"].ToString();
                                    ScreeningStatus.Optionname = statusrows["optionname"].ToString();
                                    ScreeningStatus.OptionValue = statusrows["OptionValue"].ToString();
                                    ListStatus.Add(ScreeningStatus);
                                }
                                info._ScreeningStatus = ListStatus;


                            }
                            List.Add(info);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return List;

        }
        public string Saveclientscreening(List<Nurse.clients> ClientScreenings, string userid, string agencyid)
        {
           
            try
            {
                string screeningDateQnID = (ClientScreenings.Count > 0) ? ClientScreenings.Select(x => x.ScreeningDateQuestionID).FirstOrDefault():"0";
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[8] { 
                    new DataColumn("Screeningid", typeof(int)),
                    new DataColumn("classid",typeof(int)),
                    new DataColumn("centerid",typeof(int) ),
                    new DataColumn("clientid",typeof(int)),
                    new DataColumn("ScreeningDate",typeof(string)),
                    new DataColumn("Status",typeof(int)),
                    new DataColumn("Notes",typeof(string)),
                    new DataColumn("Exception",typeof(string))
                  
                  
                });
                foreach (Nurse.clients clients in ClientScreenings)
                {
                    dt.Rows.Add(clients.Screeningid,DBNull.Value, DBNull.Value ,clients.clientid,clients.ScreeningDate,clients.Status,clients.Notes,DBNull.Value);
                }
                command.Parameters.Add(new SqlParameter("@clientmissing", dt));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters.Add(new SqlParameter("@ScreeningDateQuestionID", screeningDateQnID));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveClientMissingScreening";
                Connection.Open();
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
           

        }
        public string Savemultiscreening(List<Nurse.clients> multiscreenings, string userid,string roleId, string agencyid)
        {

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[7] { 
                    new DataColumn("Screeningid", typeof(int)),
                    new DataColumn("classid",typeof(int)),
                    new DataColumn("centerid",typeof(int) ),
                    new DataColumn("clientid",typeof(int) ),
                    new DataColumn("ScreeningDate",typeof(string)),
                    new DataColumn("Status",typeof(int)),
                    new DataColumn("Notes",typeof(string) ),

                });
                foreach (Nurse.clients clients in multiscreenings)
                {
                    dt.Rows.Add(clients.Screeningid, clients.classid,clients.centerid,DBNull.Value ,clients.ScreeningDate, clients.Status,DBNull.Value );
                }
                command.Parameters.Add(new SqlParameter("@MissingScreening", dt));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleID", roleId));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveMissingScreenings";
                Connection.Open();
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }


        }
        public List<Roster> Loadmissingclient(StaffDetails staff, string Screeningid, string Centerid)
        {
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Centerid", Centerid));
                command.Parameters.Add(new SqlParameter("@Screeningid", EncryptDecrypt.Decrypt64(Screeningid)));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getmissingclients";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Roster();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["ClientID"].ToString());
                            info.Name = dr["ClientName"].ToString();
                            info.ProgramId = dr["ReferenceProg"].ToString();
                        info.DOB = Convert.ToString(dr["DOB"]);
                            info.Gender = dr["gender"].ToString();
                            info.Screeningid = dr["screeningid"].ToString();
                            RosterList.Add(info);
                        }
                    
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }
        public List<Roster> Loadclients(StaffDetails staff, string Classroomid, string Centerid, string Screeningid)
        {
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Classroomid", Classroomid));
                command.Parameters.Add(new SqlParameter("@Centerid", Centerid));
                command.Parameters.Add(new SqlParameter("@Screeningid", Screeningid));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@Roleid", staff.RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetmissingclasssclientScreening";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Roster();
                            info.Eclientid =dr["ClientID"].ToString();
                            info.Name = dr["ClientName"].ToString();
                            info.DOB = Convert.ToString(dr["dob"]);
                            info.Gender = dr["gender"].ToString();
                            info.Screeningid = dr["screeningid"].ToString();
                            RosterList.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }
        public string Saveclientclassscreening(List<Nurse.clients> ClientScreenings,string screeningDateQuestionId, string userid, string agencyid)
        {

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[8] { 
                    new DataColumn("Screeningid", typeof(int)),
                    new DataColumn("classid",typeof(int)),
                    new DataColumn("centerid",typeof(int) ),
                    new DataColumn("clientid",typeof(int)),
                    new DataColumn("ScreeningDate",typeof(string)),
                    new DataColumn("Status",typeof(int)),
                    new DataColumn("Notes",typeof(string)),
                    new DataColumn("Exception",typeof(string)),
                  
                });
                foreach (Nurse.clients clients in ClientScreenings)
                {
                    dt.Rows.Add(clients.Screeningid, DBNull.Value, DBNull.Value,clients.clientid, clients.ScreeningDate, clients.Status, DBNull.Value, clients.Exception);
                }
                command.Parameters.Add(new SqlParameter("@clientmissing", dt));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters.Add(new SqlParameter("@ScreeningDateQuestionID", screeningDateQuestionId));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveClientclassMissingScreening";
                Connection.Open();
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }


        }
        public List<Roster> Loadsavedmissingclient(string Classroomid, string Centerid, string Screeningid, string userid, string agencyid,string Roleid)
        {
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Classroomid", Classroomid));
                command.Parameters.Add(new SqlParameter("@Centerid", Centerid));
                command.Parameters.Add(new SqlParameter("@Screeningid", Screeningid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@Roleid", Roleid));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Loadsavedmissingclient";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {

                    Roster info = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        info = new Roster();
                        info.Eclientid = EncryptDecrypt.Encrypt64(dr["ClientID"].ToString());
                        info.Name = dr["ClientName"].ToString();
                        info.ProgramId = dr["ReferenceProg"].ToString();
                        info.MissingScreeningdate = Convert.ToString(dr["MissingScreeningdate"]);
                        info.MissingScreeningstatus = dr["MissingStatus"].ToString();
                        info.Screeningid = dr["screeningid"].ToString();
                        info.DOB = Convert.ToString(dr["dob"]);
                        RosterList.Add(info);
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }
        public List<Roster> GetDeclinedScreeningList(string centerid, string userid, string agencyid, string RoleID)
        {
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleID", RoleID));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetDeclinedScreeningList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Roster();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.DOB = Convert.ToString(dr["dob"]);
                            info.Gender = dr["gender"].ToString();
                            info.ScreeningName = dr["screeningname"].ToString();
                            info.CenterName = dr["centername"].ToString();

                            RosterList.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }
        public List<Roster> LoadGroupCaseNoteClient(string centerid, string Classroom, string userid, string agencyid)
        {
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Classroom", Classroom));
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_LoadGroupCaseNoteClient";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Roster();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.DOB = Convert.ToString(dr["dob"]);
                            info.Gender = dr["gender"].ToString();
                            RosterList.Add(info);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }
        public List<CaseNote> ViewGroupCaseNoteClient(string centerid, string Classroom, string userid, string agencyid)
        {
            List<CaseNote> CaseNoteList = new List<CaseNote>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Classroom", Classroom));
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_ViewGroupCaseNoteClient";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CaseNote info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new CaseNote();
                            info.CaseNoteid = dr["casenoteid"].ToString();
                            info.BY = dr["By"].ToString();
                            info.Title = dr["Title"].ToString();
                            info.Attachment = dr["Attachment"].ToString();
                            info.References = dr["References"].ToString();
                            info.Date = Convert.ToDateTime(dr["casenotedate"]).ToString("MM/dd/yyyy");
                            CaseNoteList.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return CaseNoteList;

        }
        public List<Roster> GetReScreeningList(string centerid, string userid, string agencyid,string Roleid)
        {
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@Roleid", Roleid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetRescreenList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Roster();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.DOB = Convert.ToDateTime(dr["dob"]).ToString("MM/dd/yyyy");
                            info.Gender = dr["gender"].ToString();
                            info.ScreeningName = dr["screeningname"].ToString();
                            info.CenterName = dr["centername"].ToString();
                            info.Screeningid = dr["screeningid"].ToString();
                            info.ProgramId = dr["ProgramID"].ToString();

                            RosterList.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }
        public List<Roster> GetWithdrawnList(string centerid, string userid, string agencyid, string Roleid)
        {
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@Roleid", Roleid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetWithDrawnList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Roster();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.DOB = Convert.ToString(dr["dob"]);
                            info.Gender = dr["gender"].ToString();
                            info.ScreeningName = dr["screeningname"].ToString();
                            info.CenterName = dr["centername"].ToString();
                            info.Screeningid = dr["screeningid"].ToString();
                            //info.ProgramId = dr["ProgramID"].ToString();
                            RosterList.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }


        public Screening GetScreeningByScreeningPeriod(Screening screening, string serverPath)
        {
            try
            {
                //StaffDetails staff = StaffDetails.GetInstance();
                screening.ScreeningList = new List<ScreeningNew>();
                screening.ScreeningPeriodsList = new List<ScreeningPeriods>();

                StringBuilder urlString = new StringBuilder();
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();

                }

                using (Connection = connection.returnConnection())
                {

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@ScreeningID", screening.Screeningid));
                    command.Parameters.Add(new SqlParameter("@ProgramRefID", screening.TypeScreening));
                    command.Parameters.Add(new SqlParameter("@ScreeingPeriodIndex", screening.ScreeningPeriodIndex));
                    command.Parameters.Add(new SqlParameter("@CustomScreeningPeriod", screening.CustomScreeningPeriod));
                    command.Parameters.Add(new SqlParameter("@ClientID", screening.ClientID));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetScreeningQuestionsByScreeningPeriod";
                    command.Connection = Connection;
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }

                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    ScreeningNew newScreening = new ScreeningNew();

                    newScreening.ParentAppID = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ID"]);

                    if(_dataset.Tables.Count>3 && _dataset.Tables[3].Rows.Count>0)
                    {

                        newScreening.ApprovedFileName = Convert.ToString(_dataset.Tables[3].Rows[0]["ApprovalFileUl"]);
                        newScreening.ApprovedFileExtension = Convert.ToString(_dataset.Tables[3].Rows[0]["ApprovalFileExtension"]);
                        urlString.Clear().Append(Guid.NewGuid().ToString());

                        newScreening.ApprovedImageJson = string.IsNullOrEmpty(_dataset.Tables[3].Rows[0]["ApprovalForm"].ToString()) ? "" : Convert.ToBase64String((byte[])_dataset.Tables[3].Rows[0]["ApprovalForm"]);

                        if (!string.IsNullOrEmpty(newScreening.ApprovedFileName))
                        {
                            System.IO.FileStream file = System.IO.File.Create(serverPath + "//" + urlString + newScreening.ApprovedFileExtension);
                            file.Write((byte[])(_dataset.Tables[3].Rows[0]["ApprovalForm"]), 0, ((byte[])(_dataset.Tables[3].Rows[0]["ApprovalForm"])).Length);
                            file.Close();
                            newScreening.ApprovedImageUrl = "/TempAttachment/" + urlString + newScreening.ApprovedFileExtension;
                        }

                    }

                    newScreening.ScreeningID = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ScreeningID"]);
                    screening.ScreeningList.Add(newScreening);


                    if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0 && screening != null && screening.ScreeningList.Count > 0)
                    {

                        var screeningIDList = screening.ScreeningList.Select(x => x.ScreeningID).Distinct().ToList();

                        if (screeningIDList != null && screeningIDList.Count > 0)
                        {
                            foreach (var item in screeningIDList)
                            {



                                var qnIDList = _dataset.Tables[0].AsEnumerable().Where(x => x.Field<int>("ScreeningID") == item).Select(x => x.Field<int>("QuestionID")).Distinct().ToList();

                                List<Questions> questions = new List<Questions>();

                                foreach (var qn in qnIDList)
                                {

                                    var qnList = _dataset.Tables[0].AsEnumerable().Where(x => x.Field<int>("QuestionID") == qn).ToList();



                                    Questions qns = new Questions
                                    {
                                        Question = qnList[0].Field<string>("Question"),
                                        QuestionId = qnList[0].Field<int>("QuestionID"),
                                        QuestionType = qnList[0].Field<int>("QuestionType").ToString(),
                                        QuestionOrder = Convert.ToDouble(qnList[0].Field<string>("QuestionOrder")),
                                        Required = Convert.ToBoolean(qnList[0].Field<bool>("Required")),

                                        OptionList = qnList[0].Field<int>("QuestionType") == 1 || qnList[0].Field<int>("QuestionType") == 3 || qnList[0].Field<int>("QuestionType") == 4 ? qnList.Select(x => new Options
                                        {
                                            Option = x.Field<string>("OptionName"),
                                            OptionId = Convert.ToInt32(x.Field<long>("OptionID")),
                                            OptionValue = Convert.ToInt32(x.Field<int>("OptionValue")),
                                            IsChecked = x.Field<string>("Value") == Convert.ToString(x.Field<long>("OptionID")),
                                            OptionDescription = x.Field<string>("OptionDescription"),
                                        }).OrderBy(x => x.OptionId).Where(x => x.OptionId != 0).ToList() : new List<Options>(),

                                        OptionValue = qnList[0].Field<string>("Value"),

                                        CheckboxValue = qnList[0].Field<int>("QuestionType") == 1 ? qnList[0].Field<string>("Value").Split(',').ToArray() :new string[0],



                                    };


                                    if (qns.QuestionType == "3" )
                                    {
                                        qns.OptionList.Insert(0, new Options
                                        {

                                            Option = string.Empty,
                                            OptionId = 0,
                                            OptionValue = 0,
                                            OptionDescription="",
                                            IsChecked = false

                                        });
                                    }



                                    questions.Add(qns);



                                }

                              questions=  questions.OrderBy(x => x.QuestionOrder).ToList();
                                screening.ScreeningList[screening.ScreeningList.FindIndex(x => x.ScreeningID == item)].Questionlist = questions;

                            }
                        }


                    }

                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        screening.ScreeningPeriodsList = (from DataRow dr2 in _dataset.Tables[1].Rows
                                                          select new ScreeningPeriods
                                                          {
                                                              ScreeningPeriod = Convert.ToDouble(dr2["ScreeningPeriod"]),
                                                              Description = Convert.ToString(dr2["Description"]),
                                                              ScreeningPeriodFor = Convert.ToInt32(dr2["ScreeningPeriodFor"]),
                                                              CustomScreeningPeriod=Convert.ToInt32(dr2["CustomScreeningPeriod"])
                                                          }
                                                        ).ToList();

                    }

                    if(_dataset.Tables.Count>2 && _dataset.Tables[2]!=null && _dataset.Tables[2].Rows.Count>0)
                    {
                        screening.ScreeningAccessInfo = (from DataRow dr3 in _dataset.Tables[2].Rows
                                                         select new ScreeningAccess
                                                         {
                                                             IsEnter = Convert.ToBoolean(dr3["EnterScreening"]),
                                                             IsReview = Convert.ToBoolean(dr3["ReviewScreening"]),
                                                             IsViewOnly = Convert.ToBoolean(dr3["ViewScreening"])
                                                         }
                                                       ).FirstOrDefault();
                    }



                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {

                Connection.Dispose();
                command.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return screening;
        }


        public bool SaveScreeningByScreeningPeriod(ref int screeningPeriod,Screening _scr)
        {
            bool isRowsAffected = false;
            try
            {
                //StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                #region screening
                DataTable dtScr = new DataTable();
                dtScr.Columns.AddRange(new DataColumn[4] {
                    new DataColumn("ScreeningID",typeof(Int32)),
                    new DataColumn("QuestionID",typeof(Int32)),
                    new DataColumn("Value",typeof(string)),
                    new DataColumn("OptionID",typeof(long))
                    });
                #endregion

                foreach (var s in _scr.ScreeningList)
                {

                    foreach (var qn in s.Questionlist)
                    {


                        if(qn.QuestionType!="1")
                        {
                            if (qn.OptionValue != null && qn.OptionValue != "" && qn.OptionValue != "0")
                            {
                                dtScr.Rows.Add(s.ScreeningID, qn.QuestionId, qn.OptionValue, 0);

                            }
                        }
                        else if(qn.QuestionType=="1")
                        {
                            if(qn.CheckboxValue!=null && qn.CheckboxValue.Length>0)
                            {
                                foreach(var chk in qn.CheckboxValue)
                                {
                                    dtScr.Rows.Add(s.ScreeningID, qn.QuestionId, chk, 0);
                                }
                            }

                        }
                       

                    }
                }

                #region allowed screening
                DataTable dtDocument = new DataTable();
                dtDocument.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("ScreeningID",typeof(Int32)),
                    new DataColumn("Allowed",typeof(Int32)),
                    new DataColumn("FileName",typeof(string)),
                    new DataColumn("FileExtension",typeof(string)),
                    new DataColumn("FileBytes",typeof(byte[]))
                    });
                #endregion

                foreach (var s in _scr.ScreeningList)
                {
                    dtDocument.Rows.Add(s.ScreeningID, 1, s.ApprovedFileName, s.ApprovedFileExtension, s.ApprovedImageByte);
                }



                _scr.Screeningid = _scr.ScreeningList.Select(x => x.ScreeningID).FirstOrDefault();

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@ScreeningID", _scr.Screeningid));
                    command.Parameters.Add(new SqlParameter("@ScreeningQnTable", dtScr));
                    command.Parameters.Add(new SqlParameter("@ScreeningDocTable", dtDocument));
                    command.Parameters.Add(new SqlParameter("@ClientID", _scr.ClientID));
                    command.Parameters.Add(new SqlParameter("@ScreeningPeriod", int.Parse(_scr.ScreeningPeriodIndex))).Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add(new SqlParameter("@CustomScreeningPeriod", _scr.CustomScreeningPeriod));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_SaveScreeningByScreeningPeriod";
                    command.Connection = Connection;
                    Connection.Open();
                    isRowsAffected = Convert.ToInt32(command.ExecuteNonQuery()) > 0;

                    screeningPeriod = int.Parse(command.Parameters["@ScreeningPeriod"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();

            }
            return isRowsAffected;
        }









    }
}
