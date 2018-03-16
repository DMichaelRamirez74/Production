using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsData
{
    public class ERSEAData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;


        public ERSEAData()
        {
            this.Connection = connection.returnConnection();
            this.command = new SqlCommand();
            if (this.Connection.State == ConnectionState.Open)
            {
                this.Connection.Close();
            }
        }
        public List<CenterAnalysis> GetCenterAnalysisList(out CenterAnalysisPercentage calcAnalysis, out List<SelectListItem> programList, Guid userId, Guid agencyId, long programId)
        {
            List<CenterAnalysis> centerAnalysisList = new List<CenterAnalysis>();
            CenterAnalysis centerAnalysis = null;
            programList = new List<SelectListItem>();
            calcAnalysis = new CenterAnalysisPercentage();
            try
            {

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@userId", userId));
                command.Parameters.Add(new SqlParameter("@ProgramId", programId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = (programId == 0) ? "USP_GetCenterAnalysis" : "USP_GetChildrenByProgramType";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            centerAnalysis = new CenterAnalysis();
                            centerAnalysis.CenterId = Convert.ToInt64(dr["CenterId"]);
                            centerAnalysis.Enc_CenterId = EncryptDecrypt.Encrypt64(centerAnalysis.CenterId.ToString());
                            centerAnalysis.CenterName = dr["CenterName"].ToString();
                            centerAnalysis.Seats = Convert.ToInt64(dr["TotalSeats"]);
                            centerAnalysis.Enrolled = Convert.ToInt64(dr["ClientsEnrolled"]);
                            centerAnalysis.PercentageFilled = Convert.ToDouble(dr["CenterPercentage"]);
                            centerAnalysis.Waiting = Convert.ToInt64(dr["WaitingList"]);
                            centerAnalysis.Returning = Convert.ToInt64(dr["Returning"]);
                            centerAnalysis.Graduating = Convert.ToInt64(dr["Graduating"]);
                            centerAnalysis.Foster = Convert.ToInt64(dr["Foster"]);
                            centerAnalysis.HomeLess = Convert.ToInt64(dr["HomeLess"]);
                            centerAnalysis.OverIncome = Convert.ToInt64(dr["OverIncome"]);
                            centerAnalysis.Leads = Convert.ToInt64(dr["Leads"]);
                            centerAnalysis.WithDrawn = Convert.ToInt64(dr["WithDrawn"]);
                            centerAnalysis.Dropped = Convert.ToInt64(dr["Dropped"]);
                            centerAnalysis.ProgramId = EncryptDecrypt.Encrypt64(dr["ProgramID"].ToString());
                            centerAnalysisList.Add(centerAnalysis);
                        }
                    }

                }

                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            programList.Add(
                                new SelectListItem
                                {
                                    Text = dr["ProgramType"].ToString(),
                                    Value = EncryptDecrypt.Encrypt64(dr["ProgramTypeID"].ToString())
                                }
                            );
                        }
                    }
                }
                CenterAnalysisPercentage centeranalyiscal = new CenterAnalysisPercentage();
                if (centerAnalysisList.Count > 0)
                {
                    calcAnalysis.TotalReturned = centerAnalysisList.Sum(x => x.Returning);
                    calcAnalysis.TotalEnrolled = centerAnalysisList.Sum(x => x.Enrolled);
                    calcAnalysis.TotalWithdrawn = centerAnalysisList.Sum(x => x.WithDrawn);
                    calcAnalysis.TotalDropped = centerAnalysisList.Sum(x => x.Dropped);
                    calcAnalysis.TotalLeads = centerAnalysisList.Sum(x => x.Leads);
                    calcAnalysis.TotalPercentageFilled = centerAnalysisList.Sum(x => x.Waiting);
                    calcAnalysis.TotalSeats = centerAnalysisList.Sum(x => x.Seats);
                    calcAnalysis.TotalWaiting = centerAnalysisList.Sum(x => x.Waiting);
                    calcAnalysis.TotalGraduating = centerAnalysisList.Sum(x => x.Graduating);
                    calcAnalysis.TotalHomeLess = centerAnalysisList.Sum(x => x.HomeLess);
                    calcAnalysis.TotalFoster = centerAnalysisList.Sum(x => x.Foster);
                    calcAnalysis.TotalOverIncome = centerAnalysisList.Sum(x => x.OverIncome);
                    calcAnalysis.TotalPercentageFilled = (calcAnalysis.TotalPercentageFilled / calcAnalysis.TotalSeats) * 100;
                }

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
            return centerAnalysisList;
        }



        public List<SelectListItem> GetGradChildDataByProgram(out long totalCount, long programId, Guid agencyId)
        {
            List<SelectListItem> ReturnChildList = new List<SelectListItem>();
            totalCount = 0;
            try
            {

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@ProgramId", programId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetGraduatingChildByProgram";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            ReturnChildList.Add(new SelectListItem
                            {
                                Text = (string.IsNullOrEmpty(dr["Graduating"].ToString())) ? "0" : dr["Graduating"].ToString(),
                                Value = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString())
                            });
                        }
                    }

                    if (ReturnChildList.Count() > 0)
                    {
                        totalCount = ReturnChildList.Sum(x => Convert.ToInt64(x.Text));
                    }

                }



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
            return ReturnChildList;
        }

        public CommunityAssessment GetChartDetailsDataByZip(string searchText)
        {
            CommunityAssessment communityAssessment = new CommunityAssessment();


            try
            {

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Zipcode", searchText));
                command.Parameters.Add(new SqlParameter("@State", "0"));
                command.Parameters.Add(new SqlParameter("@City", "0"));
                command.Parameters.Add(new SqlParameter("@mode", "0"));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_zipCommunityassessmentanalysis";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            communityAssessment.StatDemoGraphicsZip = new StatsDemographics
                            {
                                AreaLand = dr["area_land"].ToString().Replace("\"", string.Empty).Trim(),
                                AreaWater = dr["AreaWater"].ToString().Replace("\"", string.Empty).Trim(),
                                PopulationCount = dr["population_count"].ToString(),
                                HousingUnitCount = dr["HousingUnitCount"].ToString().Replace("\"", string.Empty).Trim(),
                                AreaLandCensus = dr["AreaLand"].ToString().Replace("\"", string.Empty).Trim(),
                                MedianHomeValue = dr["median_home_value"].ToString(),
                                total_households_occupied = dr["totalhouseholdsoccupied"].ToString().Replace("\"", string.Empty).Trim(),
                                MedianHouseholdIncome = dr["median_household_income"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString()
                            };
                            communityAssessment.CitiesInZipCode = new CitiesInZip
                            {
                                Acceptablecities = dr["acceptable_cities"].ToString(),
                                UnacceptableCities = dr["unacceptable_cities"].ToString(),
                                ZipCode = dr["Zipcode"].ToString(),
                                Primarycity = dr["primary_city"].ToString(),
                                State = dr["State"].ToString()
                            };
                            communityAssessment.PopulationOverTime = new EstimatedPopulationOverTime
                            {
                                EstimatedPopulation2005 = dr["estimated_population_2005"].ToString(),
                                EstimatedPopulation2006 = dr["estimated_population_2006"].ToString(),
                                EstimatedPopulation2007 = dr["estimated_population_2007"].ToString(),
                                EstimatedPopulation2008 = dr["estimated_population_2008"].ToString(),
                                EstimatedPopulation2009 = dr["estimated_population_2009"].ToString(),
                                EstimatedPopulation2010 = dr["estimated_population_2010"].ToString(),
                                EstimatedPopulation2011 = dr["estimated_population_2011"].ToString(),
                                EstimatedPopulation2012 = dr["estimated_population_2012"].ToString(),
                                EstimatedPopulation2013 = dr["estimated_population_2013"].ToString(),
                                ZipCode = dr["Zipcode"].ToString()
                            };

                            communityAssessment.HouseholdOverTime = new EstimatedHouseholdsOverTime
                            {
                                EstimatedHouseholds2005 = dr["estimated_households_2005"].ToString(),
                                EstimatedHouseholds2006 = dr["estimated_households_2006"].ToString(),
                                EstimatedHouseholds2007 = dr["estimated_households_2007"].ToString(),
                                EstimatedHouseholds2008 = dr["estimated_households_2008"].ToString(),
                                EstimatedHouseholds2009 = dr["estimated_households_2009"].ToString(),
                                EstimatedHouseholds2010 = dr["estimated_households_2010"].ToString(),
                                EstimatedHouseholds2011 = dr["estimated_households_2011"].ToString(),
                                EstimatedHouseholds2012 = dr["estimated_households_2012"].ToString(),
                                EstimatedHouseholds2013 = dr["estimated_households_2013"].ToString(),
                                ZipCode = dr["Zipcode"].ToString()
                            };
                            communityAssessment.ChildrenBasedOnAge = new ChildrenByAge
                            {
                                Male1 = dr["male1"].ToString().Replace("\"", string.Empty).Trim(),
                                Male2 = dr["male2"].ToString().Replace("\"", string.Empty).Trim(),
                                Male3 = dr["male3"].ToString().Replace("\"", string.Empty).Trim(),
                                Male4 = dr["male4"].ToString().Replace("\"", string.Empty).Trim(),
                                Male5 = dr["male5"].ToString().Replace("\"", string.Empty).Trim(),
                                Male6 = dr["male6"].ToString().Replace("\"", string.Empty).Trim(),
                                Male7 = dr["male7"].ToString().Replace("\"", string.Empty).Trim(),
                                Male8 = dr["male8"].ToString().Replace("\"", string.Empty).Trim(),
                                Male9 = dr["male9"].ToString().Replace("\"", string.Empty).Trim(),
                                Male10 = dr["male10"].ToString().Replace("\"", string.Empty).Trim(),
                                Male11 = dr["male11"].ToString().Replace("\"", string.Empty).Trim(),
                                Male12 = dr["male12"].ToString().Replace("\"", string.Empty).Trim(),
                                Male13 = dr["male13"].ToString().Replace("\"", string.Empty).Trim(),
                                Male14 = dr["male14"].ToString().Replace("\"", string.Empty).Trim(),
                                Male15 = dr["male15"].ToString().Replace("\"", string.Empty).Trim(),
                                Male16 = dr["male16"].ToString().Replace("\"", string.Empty).Trim(),
                                Male17 = dr["male17"].ToString().Replace("\"", string.Empty).Trim(),
                                Male18 = dr["male18"].ToString().Replace("\"", string.Empty).Trim(),
                                Male19 = dr["male19"].ToString().Replace("\"", string.Empty).Trim(),
                                Male20 = dr["Male20"].ToString().Replace("\"", string.Empty).Trim(),

                                Female1 = dr["female1"].ToString().Replace("\"", string.Empty).Trim(),
                                Female2 = dr["female2"].ToString().Replace("\"", string.Empty).Trim(),
                                Female3 = dr["female3"].ToString().Replace("\"", string.Empty).Trim(),
                                Female4 = dr["female4"].ToString().Replace("\"", string.Empty).Trim(),
                                Female5 = dr["female5"].ToString().Replace("\"", string.Empty).Trim(),
                                Female6 = dr["female6"].ToString().Replace("\"", string.Empty).Trim(),
                                Female7 = dr["female7"].ToString().Replace("\"", string.Empty).Trim(),
                                Female8 = dr["female8"].ToString().Replace("\"", string.Empty).Trim(),
                                Female9 = dr["female9"].ToString().Replace("\"", string.Empty).Trim(),
                                Female10 = dr["female10"].ToString().Replace("\"", string.Empty).Trim(),
                                Female11 = dr["female11"].ToString().Replace("\"", string.Empty).Trim(),
                                Female12 = dr["female12"].ToString().Replace("\"", string.Empty).Trim(),
                                Female13 = dr["female13"].ToString().Replace("\"", string.Empty).Trim(),
                                Female14 = dr["female14"].ToString().Replace("\"", string.Empty).Trim(),
                                Female15 = dr["female15"].ToString().Replace("\"", string.Empty).Trim(),
                                Female16 = dr["female16"].ToString().Replace("\"", string.Empty).Trim(),
                                Female17 = dr["female17"].ToString().Replace("\"", string.Empty).Trim(),
                                Female18 = dr["female18"].ToString().Replace("\"", string.Empty).Trim(),
                                Female19 = dr["female19"].ToString().Replace("\"", string.Empty).Trim(),
                                Female20 = dr["female20"].ToString().Replace("\"", string.Empty).Trim(),
                                ZipCode = dr["Zipcode"].ToString()
                            };

                            communityAssessment.HouseholdsKids = new HouseholdWithKids
                            {
                                TotalHouseholdsWKids = dr["totalhouseholdswkids"].ToString().Replace("\"", string.Empty).Trim(),
                                households_wo_kids = dr["householdswokids"].ToString().Replace("\"", string.Empty).Trim(),
                                AverageHouseHoldSize = dr["averagehouseholdsize"].ToString().Replace("\"", string.Empty).Trim(),
                                ZipCode = dr["Zipcode"].ToString()
                            };

                            communityAssessment.TotalPopulationBasedOnAge = new TotalPopulationByAge
                            {
                                MaleUnder5 = dr["MaleUnder5"].ToString().Replace("\"", string.Empty).Trim(),
                                Male5To9 = dr["Male5To9"].ToString().Replace("\"", string.Empty).Trim(),
                                Male10To14 = dr["Male10To14"].ToString().Replace("\"", string.Empty).Trim(),
                                Male15To17 = dr["Male15To17"].ToString().Replace("\"", string.Empty).Trim(),
                                Male18To19 = dr["Male18To19"].ToString().Replace("\"", string.Empty).Trim(),
                                Male20 = dr["Male20"].ToString().Replace("\"", string.Empty).Trim(),
                                Male21 = dr["Male21"].ToString().Replace("\"", string.Empty).Trim(),
                                Male22To24 = dr["Male22To24"].ToString().Replace("\"", string.Empty).Trim(),
                                Male25To29 = dr["Male25To29"].ToString().Replace("\"", string.Empty).Trim(),
                                Male30To34 = dr["Male30To34"].ToString().Replace("\"", string.Empty).Trim(),
                                Male35To39 = dr["Male35To39"].ToString().Replace("\"", string.Empty).Trim(),
                                Male40To44 = dr["Male40To44"].ToString().Replace("\"", string.Empty).Trim(),
                                Male45to49 = dr["Male45to49"].ToString().Replace("\"", string.Empty).Trim(),
                                Male50To54 = dr["Male50To54"].ToString().Replace("\"", string.Empty).Trim(),
                                Male55To59 = dr["Male55To59"].ToString().Replace("\"", string.Empty).Trim(),
                                Male60To61 = dr["Male60To61"].ToString().Replace("\"", string.Empty).Trim(),
                                Male62To64 = dr["Male62To64"].ToString().Replace("\"", string.Empty).Trim(),
                                Male65To66 = dr["Male65To66"].ToString().Replace("\"", string.Empty).Trim(),
                                Male67To69 = dr["Male67To69"].ToString().Replace("\"", string.Empty).Trim(),
                                MAle70To74 = dr["MAle70To74"].ToString().Replace("\"", string.Empty).Trim(),
                                Male75To79 = dr["Male75To79"].ToString().Replace("\"", string.Empty).Trim(),
                                Male80To84 = dr["Male80To84"].ToString().Replace("\"", string.Empty).Trim(),
                                Male85Plus = dr["Male85Plus"].ToString().Replace("\"", string.Empty).Trim(),
                                FemaleUnder5 = dr["FemaleUnder5"].ToString().Replace("\"", string.Empty).Trim(),
                                Female5to9 = dr["Female5to9"].ToString().Replace("\"", string.Empty).Trim(),
                                Female10To14 = dr["Female10To14"].ToString().Replace("\"", string.Empty).Trim(),
                                Female15To17 = dr["Female15To17"].ToString().Replace("\"", string.Empty).Trim(),
                                Female18to19 = dr["Female18to19"].ToString().Replace("\"", string.Empty).Trim(),
                                Female20 = dr["Female20"].ToString().Replace("\"", string.Empty).Trim(),
                                Female21 = dr["Female21"].ToString().Replace("\"", string.Empty).Trim(),
                                Female22To24 = dr["Female22To24"].ToString().Replace("\"", string.Empty).Trim(),
                                Female25To29 = dr["Female25To29"].ToString().Replace("\"", string.Empty).Trim(),
                                Female30To34 = dr["Female30To34"].ToString().Replace("\"", string.Empty).Trim(),
                                Female35To39 = dr["Female35To39"].ToString().Replace("\"", string.Empty).Trim(),
                                Female40To44 = dr["Female40To44"].ToString().Replace("\"", string.Empty).Trim(),
                                Female45To49 = dr["Female45To49"].ToString().Replace("\"", string.Empty).Trim(),
                                Female50To54 = dr["Female50To54"].ToString().Replace("\"", string.Empty).Trim(),
                                Female55To59 = dr["Female55To59"].ToString().Replace("\"", string.Empty).Trim(),
                                Female60To61 = dr["Female60To61"].ToString().Replace("\"", string.Empty).Trim(),
                                Female62to64 = dr["female_62_to_64"].ToString().Replace("\"", string.Empty).Trim(),
                                Female65To66 = dr["female_65_to_66"].ToString().Replace("\"", string.Empty).Trim(),
                                Female67To69 = dr["female67to69"].ToString().Replace("\"", string.Empty).Trim(),
                                Female70To74 = dr["female70to74"].ToString().Replace("\"", string.Empty).Trim(),
                                Female75To79 = dr["female75to79"].ToString().Replace("\"", string.Empty).Trim(),
                                Female80To84 = dr["female80to84"].ToString().Replace("\"", string.Empty).Trim(),
                                Female85Plus = dr["female85plus"].ToString().Replace("\"", string.Empty).Trim(),
                                MedianAge = dr["medianage"].ToString().Replace("\"", string.Empty).Trim(),
                                MAleMedianAge = dr["malemedianage"].ToString().Replace("\"", string.Empty).Trim(),
                                FemaleMedainAge = dr["femalemedianage"].ToString().Replace("\"", string.Empty).Trim(),
                                TotalPopulationBySexAndAge = dr["TotalPopulationBySexAndAge"].ToString().Replace("\"", string.Empty).Trim(),
                                ZipCode = dr["Zipcode"].ToString()
                            };

                            communityAssessment.PopulationByGender = new GenderPopulation
                            {
                                ZipCode = dr["Zipcode"].ToString(),
                                TotalMalePopulation = dr["MalePopulation"].ToString().Replace("\"", string.Empty).Trim(),
                                TotalFemalePopulation = dr["TotalFemalePopulation"].ToString().Replace("\"", string.Empty).Trim()
                            };
                            communityAssessment.PopulationByRace = new Race
                            {
                                Asian = dr["Asian"].ToString(),
                                White = dr["white"].ToString(),
                                BlackOrAfricanAmerican = dr["black_or_african_american"].ToString(),
                                AmericanIndian = dr["american_indian_or_alaskan_native"].ToString(),
                                NativeHawaiian = dr["native_hawaiian_and_other_pacific_islander"].ToString(),
                                OtherRace = dr["other_race"].ToString(),
                                TwoOrMoreRace = dr["two_or_more_races"].ToString(),
                                ZipCode = dr["Zipcode"].ToString()
                            };

                            communityAssessment.HouseholdByAge = new HouseholdHeadAge
                            {
                                owner_15_to_24 = dr["owner15to24"].ToString().Replace("\"", string.Empty).Trim(),
                                owner_25_to_34 = dr["owner25to34"].ToString().Replace("\"", string.Empty).Trim(),
                                owner_35_to_44 = dr["owner35to44"].ToString().Replace("\"", string.Empty).Trim(),
                                owner_45_to_54 = dr["owner45to54"].ToString().Replace("\"", string.Empty).Trim(),
                                owner_55_to_59 = dr["owner55to59"].ToString().Replace("\"", string.Empty).Trim(),
                                owner_60_to_64 = dr["owner60to64"].ToString().Replace("\"", string.Empty).Trim(),
                                owner_65_to_74 = dr["owner65to74"].ToString().Replace("\"", string.Empty).Trim(),
                                owner_75_to_84 = dr["owner75to84"].ToString().Replace("\"", string.Empty).Trim(),
                                owner_85_plus = dr["owner85plus"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_15_to_24 = dr["renter15to24"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_25_to_34 = dr["renter25to34"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_35_to_44 = dr["renter35to44"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_45_to_54 = dr["renter45to54"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_55_to_59 = dr["renter55to59"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_60_to_64 = dr["renter60to64"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_65_to_74 = dr["renter65to74"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_75_to_84 = dr["renter75to84"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_85_plus = dr["renter85plus"].ToString().Replace("\"", string.Empty).Trim(),
                                ZipCode = dr["Zipcode"].ToString()
                            };
                            communityAssessment.FamilyAndSingles = new FamilySingles
                            {
                                HousbandWifeFamilyHouseholds = dr["husbandwifefamilyhouseholds"].ToString().Replace("\"", string.Empty).Trim(),
                                OtherFamilyHouseholds = dr["otherfamilyhouseholds"].ToString().Replace("\"", string.Empty).Trim(),
                                LivingAlone = dr["livingalone"].ToString().Replace("\"", string.Empty).Trim(),
                                NotLivingAlone = dr["notlivingalone"].ToString().Replace("\"", string.Empty).Trim(),
                                ZipCode = dr["Zipcode"].ToString()
                            };
                            communityAssessment.PopulationCountOnAge = new PopulationBasedOnAge
                            {
                                PopulationUnder10 = dr["pop_under_10"].ToString(),
                                PopulationUnder10To19 = dr["pop_10_to_19"].ToString(),
                                Population20To29 = dr["pop_20_to_29"].ToString(),
                                Population30To39 = dr["pop_30_to_39"].ToString(),
                                Population40to49 = dr["pop_40_to_49"].ToString(),
                                Population50to59 = dr["pop_50_to_59"].ToString(),
                                Population60to69 = dr["pop_60_to_69"].ToString(),
                                Population70To79 = dr["pop_70_to_79"].ToString(),
                                Population80Plus = dr["pop_80_plus"].ToString()
                            };
                            communityAssessment.ZipCodeInfo = new ZipCodeDetails
                            {
                                AreaCodes = dr["area_codes"].ToString(),
                                Zipcode = dr["Zipcode"].ToString(),
                                County = dr["County"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                Location = dr["Location"].ToString(),
                                Type = dr["type"].ToString(),
                                Decommissioned = dr["decommissioned"].ToString(),
                                Primarycity = dr["primary_city"].ToString(),
                                WorldRegion = dr["world_region"].ToString(),
                                Latitude = dr["latitude"].ToString(),
                                Longitude = dr["longitude"].ToString(),
                                PreciseLatitute = dr["precise_latitude"].ToString(),
                                PreciseLongitude = dr["precise_longitude"].ToString(),
                                LatitudeMin = dr["latitude_min"].ToString(),
                                LatitudeMax = dr["latitude_max"].ToString(),
                                LongitudeMin = dr["longitude_min"].ToString(),
                                LongitudeMax = dr["longitude_max"].ToString()
                            };
                            if (communityAssessment.ZipCodeInfo.AreaCodes.Count() > 3)
                            {
                                var val = Split(communityAssessment.ZipCodeInfo.AreaCodes);
                                communityAssessment.ZipCodeInfo.AreaCodes = string.Join(",", val);
                            }
                            communityAssessment.Vacancy_Reasons = new VacancyReasons
                            {
                                ZipCode = dr["Zipcode"].ToString(),
                                vacant_housing_units_for_rent = dr["vacanthousingunitsforrent"].ToString().Replace("\"", string.Empty).Trim(),
                                vacant_housing_units_rented_and_unoccupied = dr["vacanthousingunitsrentedandunoccupied"].ToString().Replace("\"", string.Empty).Trim(),
                                vacant_housing_units_for_sale_only = dr["vacanthousingunitsforsaleonly"].ToString().Replace("\"", string.Empty).Trim(),
                                vacant_housing_units_sold_and_unoccupied = dr["vacanthousingunitssoldandunoccupied"].ToString().Replace("\"", string.Empty).Trim(),
                                vacant_housing_units_for_season_recreational_or_occasional_use = dr["vacanthousingunitsforseasonrecreationaloroccasionaluse"].ToString().Replace("\"", string.Empty).Trim(),
                                vacant_housing_units_for_migrant_workers = dr["vacanthousingunitsformigrantworkers"].ToString().Replace("\"", string.Empty).Trim(),
                                vacant_housing_units_vacant_for_other_reasons = dr["vacanthousingunitsvacantforotherreasons"].ToString().Replace("\"", string.Empty).Trim(),
                            };
                            communityAssessment.OccupancyByHousing = new HousingOccupancy
                            {
                                owned_households_with_a_mortgage_or_loan = dr["ownedhouseholdswithamortgageorloan"].ToString().Replace("\"", string.Empty).Trim(),
                                owned_households_free_and_clear = dr["ownedhouseholdsfreeandclear"].ToString().Replace("\"", string.Empty).Trim(),
                                total_households_by_vacancy_status = dr["totalhouseholdsbyvacancystatus"].ToString().Replace("\"", string.Empty).Trim(),
                                total_rented_households_by_age = dr["totalrentedhouseholdsbyage"].ToString().Replace("\"", string.Empty).Trim(),
                                renter_occupied_households = dr["renteroccupiedhouseholds"].ToString().Replace("\"", string.Empty).Trim(),
                                ZipCode = dr["Zipcode"].ToString()
                            };

                            communityAssessment.HousingTypeFacilities = new HousingType
                            {
                                correctional_facility_for_adults_population = dr["correctionalfacilityforadultspopulation"].ToString().Replace("\"", string.Empty).Trim(),
                                juvenile_facilities_population = dr["juvenilefacilitiespopulation"].ToString().Replace("\"", string.Empty).Trim(),
                                nursing_facilities_population = dr["nursingfacilitiespopulation"].ToString().Replace("\"", string.Empty).Trim(),
                                college_student_housing_population = dr["collegestudenthousingpopulation"].ToString().Replace("\"", string.Empty).Trim(),
                                military_quarters_population = dr["militaryquarterspopulation"].ToString().Replace("\"", string.Empty).Trim(),
                                other_noninstitutional_population = dr["othernoninstitutionalpopulation"].ToString().Replace("\"", string.Empty).Trim(),
                                population_in_occupied_housing_units = dr["populationinoccupiedhousingunits"].ToString().Replace("\"", string.Empty).Trim(),
                                other_institutional_population = dr["otherinstitutionalpopulation"].ToString().Replace("\"", string.Empty).Trim(),
                                ZipCode = dr["Zipcode"].ToString()
                            };



                            communityAssessment.TotalFemalePopulationCensus = dr["PopulationCount"].ToString().Replace("\"", string.Empty).Trim();




                            communityAssessment.PercentPopulationPoverty = dr["percent_population_in_poverty"].ToString();
                            communityAssessment.MedianEarningsPastYear = dr["median_earnings_past_year"].ToString();

                            communityAssessment.MedianGrossRent = dr["median_gross_rent"].ToString();

                            communityAssessment.PercentHighSchoolGrad = dr["percent_high_school_graduate"].ToString();
                            communityAssessment.PercentBachelorsDeg = dr["percent_bachelors_degree"].ToString();
                            communityAssessment.PercentGradDeg = dr["percent_graduate_degree"].ToString();



                            communityAssessment.Under20ByAge = dr["under20byage"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.MaleUnder20 = dr["maleunder20"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.MaleUnder1 = dr["maleunder1"].ToString().Replace("\"", string.Empty).Trim();


                            communityAssessment.FemaleUnder20 = dr["femaleunder20"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.FemaleUnder1 = dr["femaleunder1"].ToString().Replace("\"", string.Empty).Trim();


                            communityAssessment.TotalHouseholdsByType = dr["totalhouseholdsbytype"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.FamilyHouseholds = dr["familyhouseholds"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.MaleWoWifeHouseholds = dr["malewowifehouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.FemaleWoHusbandHousholds = dr["femalewohusbandhouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.NoFamilyHouseholds = dr["nonfamilyhouseholds"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.TotalHouseHoldsByKids = dr["totalhouseholdsbykids"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.FamilyHouseholdsWKids = dr["familyhouseholdswkids"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.HusbandWifeHouseholdsWKids = dr["husbandwifehouseholdswkids"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.HusbandWifeWkidsunder6Only = dr["husbandwifehouseholdswkids"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.husband_wife_households_w_kids_6_to_17_only = dr["husbandwifehouseholdswkids6to17only"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.husband_wife_households_w_kids_under_6_and_6_to_17 = dr["husbandwifehouseholdswkidsunder6and6to17"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.other_family_households_w_kids = dr["otherfamilyhouseholdswkids"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.male_wo_wife_w_kids_households = dr["malewowifewkidshouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.male_wo_wife_w_kids_under_6_only_households = dr["malewowifewkidsunder6onlyhouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.male_wo_wife_w_kids_under_6_and_6_to_17_households = dr["malewowifewkidsunder6and6to17households"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.male_wo_wife_w_kids_6_to_17_only_households = dr["malewowifewkids6to17onlyhouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_wo_husband_w_kids_households = dr["femalewohusbandwkidshouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_wo_husband_w_kids_under_6_only_households = dr["femalewohusbandwkidsunder6onlyhouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_wo_husband_w_kids_under_6_and_6_to_17_households = dr["femalewohusbandwkidsunder6and6to17households"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_wo_husband_w_kids_6_to_17_only_households = dr["femalewohusbandwkids6to17onlyhouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.nonfamily_households_w_kids = dr["nonfamilyhouseholdswkids"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.male_w_kids_households = dr["malewkidshouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.male_w_kids_under_6_only_households = dr["malewkidsunder6onlyhouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.male_w_kids_under_6_and_6_to_17_households = dr["malewkidsunder6and6to17households"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.male_w_kids_6_to_17_only_households = dr["malewkids6to17onlyhouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_w_kids_households = dr["femalewkidshouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_w_kids_under_6_only_households = dr["femalewkidsunder6onlyhouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_w_kids_under_6_and_6_to_17_households = dr["femalewkidsunder6and6to17households"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_w_kids_6_to_17_only_households = dr["femalewkids6to17onlyhouseholds"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.family_households_wo_kids = dr["familyhouseholdswokids"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.husband_wife_households_wo_kids = dr["husbandwifehouseholdswokids"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.other_family_households_wo_kids = dr["otherfamilyhouseholdswokids"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.male_wo_wife_wo_kids_households = dr["malewowifewokidshouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.female_wo_husband_wo_kids_households = dr["femalewohusbandwokidshouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.nonfamily_households_wo_kids = dr["nonfamilyhouseholdswokids"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_male_wo_kids_households = dr["nonfamilymalewokidshouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_female_wo_kids_households = dr["nonfamilyfemalewokidshouseholds"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.total_households_by_65_plus = dr["totalhouseholdsby65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.households_with_65_plus = dr["householdswith65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.one_person_household_with_65_plus = dr["onepersonhouseholdwith65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.two_plus_household_with_65_plus = dr["twoplushouseholdwith65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.family_household_with_65_plus = dr["twoplushouseholdwith65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_household_with_65_plus = dr["nonfamilyhouseholdwith65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.households_wo_65_plus = dr["householdswo65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.one_person_household_wo_65_plus = dr["onepersonhouseholdwo65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.two_plus_household_wo_65_plus = dr["twoplushouseholdwo65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.family_household_wo_65_plus = dr["familyhouseholdwo65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_household_wo_65_plus = dr["nonfamilyhouseholdwo65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.total_households_by_75_plus = dr["totalhouseholdsby75plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.total_65_plus = dr["total65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.in_households_65_plus = dr["inhouseholds65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.in_family_households_65_plus = dr["infamilyhouseholds65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.family_householder_is_65_plus = dr["familyhouseholderis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.family_householder_male_is_65_plus = dr["familyhouseholdermaleis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.spouse_of_householder_is_65_plus = dr["spouseofhouseholderis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.parent_of_householder_is_65_plus = dr["parentofhouseholderis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.parent_in_law_of_householder_is_65_plus = dr["parentinlawofhouseholderis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.other_relative_of_householder_is_65_plus = dr["otherrelativeofhouseholderis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_relative_of_householder_is_65_plus = dr["nonrelativeofhouseholderis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.in_non_family_households_65_plus = dr["innonfamilyhouseholds65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_householder_male_is_65_plus = dr["nonfamilyhouseholdermaleis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_householder_male_not_alone_is_65_plus = dr["nonfamilyhouseholdermalenotaloneis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_householder_female_is_65_plus = dr["nonfamilyhouseholderfemaleis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_householder_female_alone_is_65_plus = dr["nonfamilyhouseholderfemalealoneis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_householder_female_not_alone_is_65_plus = dr["nonfamilyhouseholderfemalenotaloneis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_family_household_non_relative_of_householder_is_65_plus = dr["nonfamilyhouseholdnonrelativeofhouseholderis65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.in_group_quarters_65_plus = dr["ingroupquarters65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.institutionalized_65_plus = dr["institutionalized65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.non_institutionalized_65_plus = dr["noninstitutionalized65plus"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.population_in_group_quarters = dr["populationingroupquarters"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.institutionalized_population = dr["institutionalizedpopulation"].ToString().Replace("\"", string.Empty).Trim();




                            communityAssessment.noninstitutionalized_population = dr["noninstitutionalizedpopulation"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.total_households_by_occupancy_status = dr["totalhouseholdsbyoccupancystatus"].ToString().Replace("\"", string.Empty).Trim();

                            communityAssessment.total_households_vacant = dr["totalhouseholdsvacant"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.total_households_by_tenure = dr["totalhouseholdsbytenure"].ToString().Replace("\"", string.Empty).Trim();





                            communityAssessment.total_households_by_age = dr["totalhouseholdsbyage"].ToString().Replace("\"", string.Empty).Trim();
                            communityAssessment.total_owned_households_by_age = dr["totalownedhouseholdsbyage"].ToString().Replace("\"", string.Empty).Trim();



                            break;
                        }
                        string acceptableCities = string.Empty;
                        if (_dataset.Tables[0].Rows.Count > 1)
                        {
                            foreach (DataRow dr1 in _dataset.Tables[0].Rows)
                            {

                                if (communityAssessment.ZipCodeInfo.Primarycity.ToUpper() != dr1["City"].ToString().ToUpper())
                                {
                                    if (!string.IsNullOrEmpty(dr1["City"].ToString()))
                                    {


                                        acceptableCities += "," + dr1["City"].ToString();
                                    }
                                }
                            }
                            communityAssessment.CitiesInZipCode.Acceptablecities = acceptableCities;
                        }
                        else
                        {
                            communityAssessment.CitiesInZipCode.Acceptablecities = communityAssessment.ZipCodeInfo.City;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                DataAdapter.Dispose();
                command.Dispose();
            }
            return communityAssessment;
        }

        static IEnumerable<string> Split(string str)
        {
            while (str.Length > 0)
            {
                yield return new string(str.Take(3).ToArray());
                str = new string(str.Skip(3).ToArray());
            }
        }

        public List<ZipcodebyState> zipcodebyState(string SearchText)
        {

            List<ZipcodebyState> zipStateList = new List<ZipcodebyState>();

            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@state", SearchText));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetZipcodesbyState";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            ZipcodebyState Zipcodelist = new ZipcodebyState();
                            Zipcodelist.zip = dr["zip"].ToString();
                            Zipcodelist.type = dr["type"].ToString();
                            Zipcodelist.area_codes = dr["area_codes"].ToString();
                            Zipcodelist.county = dr["county"].ToString();
                            Zipcodelist.City = dr["City"].ToString();
                            zipStateList.Add(Zipcodelist);
                        }
                    }

                    var list = from u in zipStateList
                               group u by u.zip into g
                               select new
                               {
                                   zip = g.First<ZipcodebyState>().zip,
                                   City = g.Select(u => u.City),
                                   type = g.Select(u => u.type),
                                   area_codes = g.Select(u => u.area_codes),
                                   county = g.Select(u => u.county),
                               };

                    zipStateList = new List<ZipcodebyState>();
                    foreach (var item in list)
                    {
                        var city = string.Join(",", item.City.Select(p => p));
                        string areaCode_ = "";
                        var areacodes = item.area_codes.Distinct().ToList();
                        if (areacodes.Count() == 1)
                        {
                            if (areacodes.FirstOrDefault().Count() > 3)
                            {
                                var val = Split(areacodes.FirstOrDefault().ToString());
                                areaCode_ = string.Join(",", val);
                            }
                            else
                            {
                                areaCode_ = areacodes.FirstOrDefault().ToString();
                            }
                        }
                        else
                        {
                            areaCode_ = string.Join(",", areacodes.Select(p => p));
                        }
                        var county = string.Join(",", item.county.Select(p => p)).Split(',')[0];
                        var Type = string.Join(",", item.type.Select(p => p)).Split(',')[0];
                        ZipcodebyState Zipcodelist = new ZipcodebyState();
                        Zipcodelist.zip = item.zip;
                        Zipcodelist.City = city.ToLower();
                        Zipcodelist.area_codes = areaCode_;
                        Zipcodelist.county = county;
                        Zipcodelist.type = Type.ToLower();

                        zipStateList.Add(Zipcodelist);
                    }
                }
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
            return zipStateList;
        }

        public List<ZipByStateandCity> zipcodebycityandState(out City_State cityState, string searchcity, string searchstate)
        {
            DataSet dt = new DataSet();
            List<ZipByStateandCity> zipStateCityList = new List<ZipByStateandCity>();
            cityState = new City_State();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@city", searchcity.Trim()));
                command.Parameters.Add(new SqlParameter("@state", searchstate.Trim()));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetzipcodebyCityandState";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            ZipByStateandCity Zipcodelist = new ZipByStateandCity();
                            Zipcodelist.Zipcode = dr["Zipcode"].ToString();
                            Zipcodelist.City = dr["City"].ToString().ToLower();
                            Zipcodelist.population_count = dr["population_count"].ToString().ToLower();
                            Zipcodelist.type = dr["type"].ToString().ToLower();
                            Zipcodelist.acceptable_cities = dr["acceptable_cities"].ToString().ToLower();
                            Zipcodelist.unacceptable_cities = dr["unacceptable_cities"].ToString().ToLower();
                            zipStateCityList.Add(Zipcodelist);
                        }
                    }

                }
                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                        {

                            cityState.AreaCode = dr1["area_codes"].ToString();
                            if (cityState.AreaCode.Count() > 3)
                            {
                                var splitArea = Split(cityState.AreaCode.ToString());
                                cityState.AreaCode = string.Join(",", splitArea);
                            }
                            cityState.CoOrdinates = dr1["latitude"].ToString() + ',' + dr1["longitude"].ToString();
                            cityState.County = dr1["county"].ToString();
                            break;
                        }

                    }
                }

                long TotalCount = zipStateCityList.Sum(x => Convert.ToInt32(x.population_count));
                if (TotalCount > 0)
                {
                    foreach (var item in zipStateCityList)
                    {
                        double popCount = (Convert.ToDouble(item.population_count));
                        item.population_count = ((popCount * 100) / TotalCount).ToString();
                    }

                }
                cityState.MostPopulatedZip = zipStateCityList.OrderByDescending(x => x.population_count).Select(x => x.Zipcode).FirstOrDefault();
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
            return zipStateCityList;
        }

        public List<WorkShopAnalysis> GetWorkshopAnalysisList(out List<SelectListItem> centerList, Guid agencyId, Guid roleId, Guid userId, long centerId)
        {
            List<WorkShopAnalysis> workshopAnalysisList = new List<WorkShopAnalysis>();
            WorkShopAnalysis workshopAnalysis = null;
            centerList = new List<SelectListItem>();
            SelectListItem selectedItem = null;
            try
            {

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetWorkshopAnalysis";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            workshopAnalysis = new WorkShopAnalysis();
                            workshopAnalysis.HouseholdEnrolled = Convert.ToInt64(dr["HouseholdId"]);
                            workshopAnalysis.WorkShopId = Convert.ToInt64(dr["ID"]);
                            workshopAnalysis.WorkShopName = dr["WorkshopName"].ToString();
                            workshopAnalysisList.Add(workshopAnalysis);
                        }
                    }

                }
                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                        {
                            selectedItem = new SelectListItem();
                            selectedItem.Text = dr1["centername"].ToString();
                            selectedItem.Value = dr1["centerid"].ToString();
                            centerList.Add(selectedItem);
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
                DataAdapter.Dispose();
                command.Dispose();
            }
            return workshopAnalysisList;
        }

        public void GetApplicationEnrollmentBasedonZips(ref DataSet dtTable, string AgencyId)
        {
            dtTable = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetApplicationEnrollmentByZipCode";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtTable);
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
        }

        public void GetApplicationEnrollmentBasedonZip(ref List<ApplicationEnrollment> lstAppliaction, string AgencyId)
        {
            lstAppliaction = new List<ApplicationEnrollment>();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetApplicationEnrollmentByZipCode";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            ApplicationEnrollment obj = new ApplicationEnrollment();
                            obj.ZipCode = dr["ZipCode"].ToString();
                            obj.Application = Convert.ToInt32(dr["AppCount"].ToString());
                            obj.Enrollment = Convert.ToInt32(dr["EnrollCount"].ToString());
                            obj.Withdrawn = Convert.ToInt32(dr["WithdrawnCount"].ToString());
                            obj.Dropped = Convert.ToInt32(dr["DroppedCount"].ToString());
                            lstAppliaction.Add(obj);
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
        }

        public void GetCityName(ref List<CityName> lstCityname)
        {
            lstCityname = new List<CityName>();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetCityNamewithZip";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            CityName obj = new CityName();
                            obj.Zipcode = dr["ZipCode"].ToString();
                            obj.City = dr["City"].ToString();
                            obj.PrimaryCity = dr["PrimaryCity"].ToString();
                            lstCityname.Add(obj);
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
        }

        public ERSEADashBoard GetADABasedonCenter(ref List<ADA> lstADA, ref int firstMonth, string AgencyId)
        {
            lstADA = new List<ADA>();
            ERSEADashBoard erseaDashboard = new ERSEADashBoard();
            try
            {


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetADAForMonth";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        //foreach (DataRow dr in _dataset.Tables[0].Rows)
                        //{
                        //    ADA obj = new ADA();
                        //    obj.CenterName = !string.IsNullOrEmpty(dr["CenterName"].ToString()) ? dr["CenterName"].ToString() : "";
                        //    obj.Jan = !string.IsNullOrEmpty(dr["Jan"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Jan"].ToString()), 0) : 0;
                        //    obj.Feb = !string.IsNullOrEmpty(dr["Feb"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Feb"].ToString()), 0) : 0;
                        //    obj.Mar = !string.IsNullOrEmpty(dr["Mar"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Mar"].ToString()), 0) : 0;
                        //    obj.Apr = !string.IsNullOrEmpty(dr["Apr"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Apr"].ToString()), 0) : 0;
                        //    obj.May = !string.IsNullOrEmpty(dr["May"].ToString()) ? Math.Round(Convert.ToDecimal(dr["May"].ToString()), 0) : 0;
                        //    obj.Jun = !string.IsNullOrEmpty(dr["Jun"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Jun"].ToString()), 0) : 0;
                        //    obj.Jul = !string.IsNullOrEmpty(dr["Jul"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Jul"].ToString()), 0) : 0;
                        //    obj.Aug = !string.IsNullOrEmpty(dr["Aug"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Aug"].ToString()), 0) : 0;
                        //    obj.Sep = !string.IsNullOrEmpty(dr["Sep"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Sep"].ToString()), 0) : 0;
                        //    obj.Oct = !string.IsNullOrEmpty(dr["Oct"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Oct"].ToString()), 0) : 0;
                        //    obj.Nov = !string.IsNullOrEmpty(dr["Nov"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Nov"].ToString()), 0) : 0;
                        //    obj.Dec = !string.IsNullOrEmpty(dr["Dec"].ToString()) ? Math.Round(Convert.ToDecimal(dr["Dec"].ToString()), 0) : 0;
                        //    lstADA.Add(obj);
                        //}

                        erseaDashboard.listADA = _dataset.Tables[0].AsEnumerable().Select(x => new ADA
                        {
                            CenterName = !string.IsNullOrEmpty(x.Field<string>("CenterName").ToString()) ? x.Field<string>("CenterName").ToString() : "",
                            Jan = !string.IsNullOrEmpty(x.Field<decimal>("Jan").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Jan"))) : 0,
                            Feb = !string.IsNullOrEmpty(x.Field<decimal>("Feb").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Feb"))) : 0,
                            Mar = !string.IsNullOrEmpty(x.Field<decimal>("Mar").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Mar"))) : 0,
                            Apr = !string.IsNullOrEmpty(x.Field<decimal>("Apr").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Apr"))) : 0,
                            May = !string.IsNullOrEmpty(x.Field<decimal>("May").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("May"))) : 0,
                            Jun = !string.IsNullOrEmpty(x.Field<decimal>("Jun").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Jun"))) : 0,
                            Jul = !string.IsNullOrEmpty(x.Field<decimal>("Jul").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Jul"))) : 0,
                            Aug = !string.IsNullOrEmpty(x.Field<decimal>("Aug").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Aug"))) : 0,
                            Sep = !string.IsNullOrEmpty(x.Field<decimal>("Sep").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Sep"))) : 0,
                            Oct = !string.IsNullOrEmpty(x.Field<decimal>("Oct").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Oct"))) : 0,
                            Nov = !string.IsNullOrEmpty(x.Field<decimal>("Nov").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Nov"))) : 0,
                            Dec = !string.IsNullOrEmpty(x.Field<decimal>("Dec").ToString()) ? Math.Round(Convert.ToDecimal(x.Field<decimal>("Dec"))) : 0,

                        }).ToList();



                    }

                    if (_dataset.Tables.Count > 0 && _dataset.Tables[1].Rows.Count > 0)
                    {
                        erseaDashboard.MonthOrdersList = _dataset.Tables[1].AsEnumerable().Select(x => new SelectListItem
                        {

                            Text = x.Field<string>("Month"),
                            Value = x.Field<int>("MonthNumber").ToString()

                        }).ToList();
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
            return erseaDashboard;
        }

        public ChildrenInfoClass GetChildrenByCenter(CenterAnalysisParameters centerParameters)
        {
            List<ChildrenInfo> childrenList = new List<ChildrenInfo>();
            List<ClassRoomDetails> ClassRoomList = new List<ClassRoomDetails>();

            ChildrenInfoClass childrenInfoClass = new ChildrenInfoClass();
            try
            {


                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetChildrenByCenter";
                    command.Parameters.AddWithValue("@AgencyId", centerParameters.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@CenterId", centerParameters.CenterId);
                    command.Parameters.AddWithValue("@ProgramId", centerParameters.ProgramId);
                    command.Parameters.AddWithValue("@Take", centerParameters.Take);
                    command.Parameters.AddWithValue("@Skip", centerParameters.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", centerParameters.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", centerParameters.SearchText);
                    command.Parameters.AddWithValue("@ClassRoomId", centerParameters.ClassRoomId);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childrenInfoClass.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);


                        childrenInfoClass.ChildrenList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                          select new ChildrenInfo
                                                          {
                                                              ClientName = dr1["name"].ToString(),
                                                              ClassStartDate = dr1["Dateofclassstartdate"].ToString(),
                                                              //Image = dr1["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr1["ProfilePic"]),
                                                              Foster = dr1["fosterChild"].ToString(),
                                                              Gender = dr1["gender"].ToString(),
                                                              CenterName = dr1["CenterName"].ToString(),
                                                              OverIncome = dr1["OverIncome"].ToString(),
                                                              AttendancePercentage = Math.Round(Convert.ToDouble(dr1["AttendancePercentage"])).ToString(),
                                                              ChildAttendance = dr1["AttendanceType"].ToString(),
                                                              Dob = dr1["Dob"].ToString(),
                                                              ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString())
                                                          }).ToList();
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        childrenInfoClass.ClassRoomInfoList = (from DataRow dr2 in _dataset.Tables[1].Rows

                                                               select new ClassRoomDetails
                                                               {

                                                                   EnC_ClassRoomId = EncryptDecrypt.Encrypt64(dr2["ClassroomID"].ToString()),
                                                                   ClassRoomId = Convert.ToInt64(dr2["ClassroomID"].ToString()),
                                                                   ClassRoomName = dr2["ClassroomName"].ToString()
                                                               }).ToList();

                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
                _dataset.Dispose();
            }
            return childrenInfoClass;
        }

        public ChildrenInfoClass GetChildrenByClassRoom(CenterAnalysisParameters getchildParameter)
        {
            List<ChildrenInfo> childrenList = new List<ChildrenInfo>();
            ChildrenInfo childrenInfo = new ChildrenInfo();
            ChildrenInfoClass childrenInfoClass = new ChildrenInfoClass();
            try
            {

                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetChildrenByClassRoom";
                command.Parameters.AddWithValue("@AgencyId", getchildParameter.StaffDetails.AgencyId);
                command.Parameters.AddWithValue("@CenterId", getchildParameter.CenterId);
                command.Parameters.AddWithValue("@ClassRoomId", getchildParameter.ClassRoomId);
                command.Parameters.AddWithValue("@ProgramId", getchildParameter.ProgramId);
                command.Parameters.AddWithValue("@Take", getchildParameter.Take);
                command.Parameters.AddWithValue("@Skip", getchildParameter.Skip);
                command.Parameters.AddWithValue("@RequestedPage", getchildParameter.RequestedPage);
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childrenInfoClass.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            childrenInfo = new ChildrenInfo();
                            childrenInfo.ClientName = dr["name"].ToString();
                            childrenInfo.ClassStartDate = dr["Dateofclassstartdate"].ToString();
                            childrenInfo.Image = dr["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePic"]);
                            childrenInfo.Foster = dr["fosterChild"].ToString();
                            childrenInfo.Gender = dr["gender"].ToString();
                            childrenInfo.OverIncome = dr["OverIncome"].ToString();
                            childrenInfo.AttendancePercentage = dr["AttendancePercentage"].ToString();
                            childrenInfo.ChildAttendance = dr["AttendanceType"].ToString();
                            childrenInfo.Dob = dr["Dob"].ToString();
                            childrenList.Add(childrenInfo);
                        }
                    }
                }

                childrenInfoClass.ChildrenList = childrenList;

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
            return childrenInfoClass;
        }

        public ChildrenInfoClass GetEnrolledChildrenData(CenterAnalysisParameters enrolledParameter)
        {

            ChildrenInfoClass childrenInfoClass = new ChildrenInfoClass();
            try
            {

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetEnrolledChildByCenter";
                    command.Parameters.AddWithValue("@AgencyId", enrolledParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", enrolledParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", enrolledParameter.CenterId);
                    command.Parameters.AddWithValue("@UserId", enrolledParameter.StaffDetails.UserId);
                    command.Parameters.AddWithValue("@RoleId", enrolledParameter.StaffDetails.RoleId);
                    command.Parameters.AddWithValue("@Take", enrolledParameter.Take);
                    command.Parameters.AddWithValue("@Skip", enrolledParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", enrolledParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", enrolledParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childrenInfoClass.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);


                        childrenInfoClass.ChildrenList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                          select new ChildrenInfo
                                                          {

                                                              ClientName = dr1["name"].ToString(),
                                                              ClassStartDate = dr1["Dateofclassstartdate"].ToString(),
                                                              Foster = dr1["fosterChild"].ToString(),
                                                              Gender = dr1["gender"].ToString(),
                                                              OverIncome = dr1["OverIncome"].ToString(),
                                                              AttendancePercentage = Math.Round(Convert.ToDouble(dr1["AttendancePercentage"])).ToString(),
                                                              ChildAttendance = dr1["AttendanceType"].ToString(),
                                                              Dob = dr1["Dob"].ToString(),
                                                              CenterName = dr1["CenterName"].ToString(),
                                                              Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["clientId"].ToString())
                                                          }

                                                        ).ToList();


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
                DataAdapter.Dispose();
                _dataset.Dispose();
            }


            return childrenInfoClass;
        }

        public ChildrenInfoClass GetReturningChildren(CenterAnalysisParameters returningParameter)
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {
                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetReturningChildByCenter";
                    command.Parameters.AddWithValue("@AgencyId", returningParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", returningParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", returningParameter.CenterId);
                    command.Parameters.AddWithValue("@Take", returningParameter.Take);
                    command.Parameters.AddWithValue("@Skip", returningParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", returningParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", returningParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        childInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        childInfo.ReturningList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                   select new ChildrenInfo
                                                   {
                                                       ClientName = dr1["name"].ToString(),
                                                       ClassStartDate = dr1["Dateofclassstartdate"].ToString(),
                                                       Foster = dr1["fosterChild"].ToString(),
                                                       Gender = dr1["gender"].ToString(),
                                                       OverIncome = dr1["OverIncome"].ToString(),
                                                       AttendancePercentage = dr1["AttendancePercentage"].ToString(),
                                                       ChildAttendance = dr1["AttendanceType"].ToString(),
                                                       Dob = dr1["Dob"].ToString(),
                                                       CenterName = dr1["CenterName"].ToString(),
                                                       ProgramType = dr1["ProgramType"].ToString(),
                                                       Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString())
                                                   }
                                                 ).ToList();
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
            return childInfo;
        }

        public ChildrenInfoClass GetGraduatingChildrenData(CenterAnalysisParameters gradParameter)
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection)
                {

                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetGraduatingChildByCenter";
                    command.Parameters.AddWithValue("@AgencyId", gradParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", gradParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", gradParameter.CenterId);
                    command.Parameters.AddWithValue("@Take", gradParameter.Take);
                    command.Parameters.AddWithValue("@Skip", gradParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", gradParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", gradParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);


                        childInfo.GraduatingList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                    select new ChildrenInfo
                                                    {
                                                        ClientName = dr1["name"].ToString(),
                                                        ClassStartDate = dr1["Dateofclassstartdate"].ToString(),
                                                        Foster = dr1["fosterChild"].ToString(),
                                                        Gender = dr1["gender"].ToString(),
                                                        OverIncome = dr1["OverIncome"].ToString(),
                                                        AttendancePercentage = dr1["AttendancePercentage"].ToString(),
                                                        ChildAttendance = dr1["AttendanceType"].ToString(),
                                                        Dob = dr1["Dob"].ToString(),
                                                        CenterName = dr1["CenterName"].ToString(),
                                                        ProgramType = dr1["ProgramType"].ToString(),
                                                        Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString())
                                                    }).ToList();

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
            return childInfo;
        }

        public ChildrenInfoClass GetWatitingChildrenData(CenterAnalysisParameters waitingParameter)
        {
            ChildrenInfoClass childrenInfo = new ChildrenInfoClass();
            try
            {
                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetWaitingChildByCenter";
                    command.Parameters.AddWithValue("@AgencyId", waitingParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", waitingParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", waitingParameter.CenterId);
                    command.Parameters.AddWithValue("@Take", waitingParameter.Take);
                    command.Parameters.AddWithValue("@Skip", waitingParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", waitingParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", waitingParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childrenInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        childrenInfo.WaitingChildrenList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                            select new WaitingChildren
                                                            {
                                                                ChildrenName = dr1["ChildrenName"].ToString(),
                                                                Gender = dr1["Gender"].ToString(),
                                                                CenterName = dr1["centername"].ToString(),
                                                                ProgramId = Convert.ToInt64(dr1["ProgramTypeId"].ToString()),
                                                                ProgramType = dr1["ProgramType"].ToString(),
                                                                Dob = dr1["dob"].ToString(),
                                                                DateOnList = dr1["DateEntered"].ToString(),
                                                                SelectionPoints = Convert.ToInt64(dr1["SelectionPoints"].ToString()),
                                                                CenterChoice = dr1["Centerchoice"].ToString(),
                                                                Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["clientId"].ToString())
                                                            }

                                                          ).ToList();

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
                command.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return childrenInfo;
        }

        public ChildrenInfoClass GetWithdrawnChildList(CenterAnalysisParameters withdrawnParameter)
        {
            ChildrenInfoClass childrenInfo = new ChildrenInfoClass();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_WithdrawnChildByCenter";
                    command.Parameters.AddWithValue("@AgencyId", withdrawnParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", withdrawnParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", withdrawnParameter.CenterId);
                    command.Parameters.AddWithValue("@UserId", withdrawnParameter.StaffDetails.UserId);
                    command.Parameters.AddWithValue("@RoleId", withdrawnParameter.StaffDetails.RoleId);
                    command.Parameters.AddWithValue("@Take", withdrawnParameter.Take);
                    command.Parameters.AddWithValue("@Skip", withdrawnParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", withdrawnParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", withdrawnParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childrenInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        childrenInfo.WithdrawnChildrenList = (from DataRow dr1 in _dataset.Tables[0].Rows

                                                              select new WaitingChildren
                                                              {

                                                                  ChildrenName = dr1["ChildrenName"].ToString(),
                                                                  Gender = dr1["Gender"].ToString(),
                                                                  CenterName = dr1["centername"].ToString(),
                                                                  ProgramId = Convert.ToInt64(dr1["ProgramTypeId"].ToString()),
                                                                  ProgramType = dr1["ProgramType"].ToString(),
                                                                  Dob = dr1["dob"].ToString(),
                                                                  DateOnList = dr1["DateEntered"].ToString(),
                                                                  SelectionPoints = Convert.ToInt64(dr1["SelectionPoints"]),
                                                                  Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["clientId"].ToString())

                                                              }).ToList();
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

                command.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return childrenInfo;
        }

        public ChildrenInfoClass GetDroppedChildList(CenterAnalysisParameters droppedParameter)
        {

            ChildrenInfoClass childrenInfo = new ChildrenInfoClass();
            try
            {

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_DroppedChildByCenter";
                    command.Parameters.AddWithValue("@AgencyId", droppedParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", droppedParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", droppedParameter.CenterId);
                    command.Parameters.AddWithValue("@UserId", droppedParameter.StaffDetails.UserId);
                    command.Parameters.AddWithValue("@RoleId", droppedParameter.StaffDetails.RoleId);
                    command.Parameters.AddWithValue("@Take", droppedParameter.Take);
                    command.Parameters.AddWithValue("@Skip", droppedParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", droppedParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", droppedParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        childrenInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);


                        childrenInfo.DroppedChildrenList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                            select new WaitingChildren
                                                            {
                                                                ChildrenName = dr1["ChildrenName"].ToString(),
                                                                Gender = dr1["Gender"].ToString(),
                                                                CenterName = dr1["centername"].ToString(),
                                                                ProgramId = Convert.ToInt64(dr1["ProgramTypeId"].ToString()),
                                                                ProgramType = dr1["ProgramType"].ToString(),
                                                                Dob = dr1["dob"].ToString(),
                                                                DateOnList = dr1["DateEntered"].ToString(),
                                                                SelectionPoints = Convert.ToInt64(dr1["SelectionPoints"]),
                                                                Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["clientId"].ToString())
                                                            }

                                                          ).ToList();
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

                Connection.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return childrenInfo;
        }

        public ChildrenInfoClass GetOverIncomeChildrenData(out List<SelectListItem> parentNameList, CenterAnalysisParameters oIParameter)
        {
            List<ChildrenInfo> overIncomChildList = new List<ChildrenInfo>();
            parentNameList = new List<SelectListItem>();
            SelectListItem parentInfo = null;
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            childInfo.OverIncomeChildrenList = new List<ChildrenInfo>();
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Open();

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetOverIncomeChildByCenter";
                    command.Parameters.AddWithValue("@AgencyId", oIParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", oIParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", oIParameter.CenterId);
                    command.Parameters.AddWithValue("@Take", oIParameter.Take);
                    command.Parameters.AddWithValue("@Skip", oIParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", oIParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", oIParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);


                        childInfo.OverIncomeChildrenList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                            select new ChildrenInfo
                                                            {
                                                                ClientName = dr1["name"].ToString(),
                                                                Gender = dr1["Gender"].ToString(),
                                                                CenterName = dr1["centername"].ToString(),
                                                                ProgramType = dr1["ProgramType"].ToString(),
                                                                Dob = dr1["dob"].ToString(),
                                                                ClassStartDate = dr1["Dateofclassstartdate"].ToString(),
                                                                ClassRoomName = dr1["ClassRoomName"].ToString(),
                                                                ChildIncome = (Convert.ToInt64(dr1["PovertyCalculated"].ToString()) > 100 && Convert.ToInt64(dr1["PovertyCalculated"].ToString()) < 130) ? "less than 130%" : "greater than 130%",
                                                                ClientId = dr1["ClientId"].ToString(),
                                                                Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString()),
                                                                ClientId1 = dr1["clientId1"].ToString(),
                                                                ClientId2 = dr1["clientId2"].ToString()

                                                            }).ToList();
                    }

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                        {
                            parentInfo = new SelectListItem
                            {
                                Text = dr1["ParentName"].ToString(),
                                Value = dr1["ClientId"].ToString()
                            };
                            parentNameList.Add(parentInfo);

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
                Connection.Dispose();
                command.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return childInfo;
        }



        public ChildrenInfoClass GetFosterChildData(CenterAnalysisParameters fosterParameter)
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", fosterParameter.StaffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@CenterId", fosterParameter.CenterId));
                    command.Parameters.Add(new SqlParameter("@ProgramId", fosterParameter.ProgramId));
                    command.Parameters.Add(new SqlParameter("@UserId", fosterParameter.StaffDetails.UserId));
                    command.Parameters.AddWithValue("@Take", fosterParameter.Take);
                    command.Parameters.AddWithValue("@Skip", fosterParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", fosterParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", fosterParameter.SearchText);
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetFosterChildByCenter";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        childInfo.FosterChildrenList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                        select new FosterChild
                                                        {
                                                            ClientName = dr1["Name"].ToString(),
                                                            ClientId = Convert.ToInt64(dr1["ClientID"]),
                                                            CenterId = Convert.ToInt64(dr1["CenterId"]),
                                                            FileAttached = dr1["FosterAttachment"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr1["FosterAttachment"]),
                                                            FileName = dr1["FosterFileName"].ToString(),
                                                            FileExtension = dr1["FosterFileExtension"].ToString(),
                                                            ClassStartDate = dr1["Dateofclassstartdate"].ToString(),
                                                            Gender = dr1["gender"].ToString(),
                                                            Dob = dr1["Dob"].ToString(),
                                                            CenterName = dr1["CenterName"].ToString(),
                                                            Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString())
                                                        }

                                                      ).ToList();

                    }

                }



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                Connection.Dispose();
            }
            return childInfo;
        }
        public ChildrenInfoClass GetHomelessChildrenData(CenterAnalysisParameters homelessParameter)
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GethomelessChildByCenter";
                    command.Parameters.AddWithValue("@AgencyId", homelessParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", homelessParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", homelessParameter.CenterId);
                    command.Parameters.AddWithValue("@Take", homelessParameter.Take);
                    command.Parameters.AddWithValue("@Skip", homelessParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", homelessParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", homelessParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);


                        childInfo.HomeLessChildrenList = (from DataRow dr in _dataset.Tables[0].Rows
                                                          select new HomelessChildren
                                                          {
                                                              ChildrenName = dr["name"].ToString(),
                                                              Gender = dr["Gender"].ToString(),
                                                              CenterName = dr["centername"].ToString(),
                                                              ProgramId = Convert.ToInt64(dr["ProgramId"].ToString()),
                                                              ProgramType = dr["ProgramType"].ToString(),
                                                              Dob = dr["dob"].ToString(),
                                                              ClassStartDate = dr["Dateofclassstartdate"].ToString(),
                                                              ClassRoomId = Convert.ToInt64(dr["ClassRoomId"].ToString()),
                                                              ClassRoomName = dr["ClassRoomName"].ToString(),
                                                              Enc_ClientId = EncryptDecrypt.Encrypt64(dr["ClientId"].ToString())
                                                          }

                                                        ).ToList();

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
            return childInfo;
        }



        public ChildrenInfoClass GetLeadsChildrenData(CenterAnalysisParameters leadsParameter)
        {
            ChildrenInfoClass childInfo = new ChildrenInfoClass();
            try
            {
                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetLeadsByChildCenter";
                    command.Parameters.AddWithValue("@AgencyId", leadsParameter.StaffDetails.AgencyId);
                    command.Parameters.AddWithValue("@ProgramId", leadsParameter.ProgramId);
                    command.Parameters.AddWithValue("@CenterId", leadsParameter.CenterId);
                    command.Parameters.AddWithValue("@Take", leadsParameter.Take);
                    command.Parameters.AddWithValue("@Skip", leadsParameter.Skip);
                    command.Parameters.AddWithValue("@RequestedPage", leadsParameter.RequestedPage);
                    command.Parameters.AddWithValue("@SearchText", leadsParameter.SearchText);
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        childInfo.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        childInfo.LeadsChildrenList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                       select new LeadsChildren
                                                       {
                                                           ChildrenName = dr1["ChildrenName"].ToString(),
                                                           ParentName = dr1["ParentName"].ToString(),
                                                           Disability = dr1["Disability"].ToString(),
                                                           Gender = dr1["Gender"].ToString(),
                                                           Dob = dr1["DateofBirth"].ToString(),
                                                           CenterName = dr1["CenterName"].ToString(),
                                                           ContactStatus = dr1["ContactStatus"].ToString(),
                                                           YakkrStatus = dr1["YakkrStatus"].ToString(),
                                                           RejectParentId = dr1["RejectParentId"].ToString(),
                                                           Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ChildId"].ToString()),
                                                           FSWName = dr1["FSWName"].ToString(),
                                                           StaffUserId = dr1["StaffUserId"].ToString()
                                                       }).ToList();
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

                command.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return childInfo;
        }

        public SelectListItem GetChildrenImageData(long ClientId)
        {
            SelectListItem child = new SelectListItem();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetChildrenImage";
                command.Parameters.AddWithValue("@ClientId", ClientId);
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            child.Text = dr["profilepic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["profilepic"]);
                            child.Value = dr["gender"].ToString();
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
            return child;
        }

        public string GetgeometryByZip(string zipcode)
        {
            string geometry = "";
            try
            {

                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetgeometryByZipcode";
                command.Parameters.AddWithValue("@Zipcode", zipcode);
                Object result = command.ExecuteScalar();
                if (result != null)
                    geometry = result.ToString();

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
            return geometry;
        }

        public string GetgeometryByState(string state)
        {
            string geometry = "";
            try
            {

                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetgeometryByState";
                command.Parameters.AddWithValue("@State", state);
                Object result = command.ExecuteScalar();
                if (result != null)
                    geometry = result.ToString();

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
            return geometry;
        }

        public string GetgeometryByCounty(string county)
        {
            string geometry = "";
            try
            {

                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetgeometryByCounty";
                command.Parameters.AddWithValue("@County", county);
                Object result = command.ExecuteScalar();
                if (result != null)
                    geometry = result.ToString();

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
            return geometry;
        }

        public bool SaveGeoJsonZipcode(string zipcode, string geometry)
        {
            bool isInserted = false;
            try
            {
                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SaveZipcodeBoundaryCoordinates";
                command.Parameters.AddWithValue("@Zipcode", zipcode);
                command.Parameters.AddWithValue("@Coordinates", geometry);
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

        public bool SaveGeoJsonCounty(string county, string geometry)
        {
            bool isInserted = false;
            try
            {

                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SaveCountyBoundaryCoordinates";
                command.Parameters.AddWithValue("@County", county.TrimStart(',').TrimEnd(','));
                command.Parameters.AddWithValue("@Coordinates", geometry);
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

        public bool SaveGeoJsonState(string state, string geometry)
        {
            bool isInserted = false;
            try
            {

                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SaveStateBoundaryCoordinates";
                command.Parameters.AddWithValue("@State", state.TrimStart(',').TrimEnd(','));
                command.Parameters.AddWithValue("@Coordinates", geometry);
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
    }
}

