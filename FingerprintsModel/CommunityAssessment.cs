using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class CommunityAssessment
    {

        public StatsDemographics StatDemoGraphicsZip { get; set; }
        public CitiesInZip CitiesInZipCode { get; set; }
        public EstimatedPopulationOverTime PopulationOverTime { get; set; }
        public EstimatedHouseholdsOverTime HouseholdOverTime { get; set; }
        public ChildrenByAge ChildrenBasedOnAge { get; set; }
        public HouseholdWithKids HouseholdsKids { get; set; }

        public GenderPopulation PopulationByGender { get; set; }

        public TotalPopulationByAge TotalPopulationBasedOnAge { get; set; }

        public PopulationBasedOnAge PopulationCountOnAge { get; set; }

        public HouseholdHeadAge HouseholdByAge { get; set; }
        public FamilySingles FamilyAndSingles { get; set; }

        public Race PopulationByRace { get; set; }
        public ZipCodeDetails ZipCodeInfo { get; set; }
        public VacancyReasons Vacancy_Reasons { get; set; }

        public HousingOccupancy OccupancyByHousing { get; set; }

        public HousingType HousingTypeFacilities { get; set; }
        public string PercentPopulationPoverty { get; set; }
        public string MedianEarningsPastYear { get; set; }

        
        public string MedianGrossRent { get; set; }

        public string PercentHighSchoolGrad { get; set; }
        public string PercentBachelorsDeg { get; set; }
        public string PercentGradDeg { get; set; }
        public string ZipCodeCensus { get; set; }
        
        public string Under20ByAge { get; set; }
        public string MaleUnder20 { get; set; }
        public string MaleUnder1 { get; set; }


        public string FemaleUnder20 { get; set; }
        public string FemaleUnder1 { get; set; }



        public string TotalHouseholdsByType { get; set; }
        public string FamilyHouseholds { get; set; }

        
       
        public string MaleWoWifeHouseholds { get; set; }
        public string FemaleWoHusbandHousholds { get; set; }
        public string NoFamilyHouseholds { get; set; }
      
    
        public string TotalHouseHoldsByKids { get; set; }

        public string FamilyHouseholdsWKids { get; set; }
        public string HusbandWifeHouseholdsWKids { get; set; }
        public string HusbandWifeWkidsunder6Only { get; set; }

        public string husband_wife_households_w_kids_under_6_and_6_to_17 { get; set; }
        public string husband_wife_households_w_kids_6_to_17_only { get; set; }
        public string other_family_households_w_kids { get; set; }
        public string male_wo_wife_w_kids_households { get; set; }
        public string male_wo_wife_w_kids_under_6_only_households { get; set; }
        public string male_wo_wife_w_kids_under_6_and_6_to_17_households { get; set; }

        public string male_wo_wife_w_kids_6_to_17_only_households { get; set; }
        public string female_wo_husband_w_kids_households { get; set; }
        public string female_wo_husband_w_kids_under_6_only_households { get; set; }
        public string female_wo_husband_w_kids_under_6_and_6_to_17_households { get; set; }
        public string female_wo_husband_w_kids_6_to_17_only_households { get; set; }
        public string nonfamily_households_w_kids { get; set; }
        public string male_w_kids_households { get; set; }

        public string male_w_kids_under_6_only_households { get; set; }
        public string male_w_kids_under_6_and_6_to_17_households { get; set; }
        public string male_w_kids_6_to_17_only_households { get; set; }
        public string female_w_kids_households { get; set; }
        public string female_w_kids_under_6_only_households { get; set; }
        public string female_w_kids_under_6_and_6_to_17_households { get; set; }
        public string female_w_kids_6_to_17_only_households { get; set; }

        public string family_households_wo_kids { get; set; }
        public string husband_wife_households_wo_kids { get; set; }
        public string other_family_households_wo_kids { get; set; }
        public string male_wo_wife_wo_kids_households { get; set; }
        public string female_wo_husband_wo_kids_households { get; set; }
        public string nonfamily_households_wo_kids { get; set; }
        public string non_family_male_wo_kids_households { get; set; }
        public string non_family_female_wo_kids_households { get; set; }
        public string total_households_by_65_plus { get; set; }
        public string households_with_65_plus { get; set; }
        public string one_person_household_with_65_plus { get; set; }
        public string two_plus_household_with_65_plus { get; set; }
        public string family_household_with_65_plus { get; set; }
        public string non_family_household_with_65_plus { get; set; }
        public string households_wo_65_plus { get; set; }
        public string one_person_household_wo_65_plus { get; set; }
        public string two_plus_household_wo_65_plus { get; set; }
        public string family_household_wo_65_plus { get; set; }
        public string non_family_household_wo_65_plus { get; set; }
        public string total_households_by_75_plus { get; set; }
        public string total_65_plus { get; set; }
        public string in_households_65_plus { get; set; }
        public string in_family_households_65_plus { get; set; }
        public string family_householder_is_65_plus { get; set; }
        public string family_householder_male_is_65_plus { get; set; }
        public string family_householder_female_is_65_plus { get; set; }
        public string spouse_of_householder_is_65_plus { get; set; }
        public string parent_of_householder_is_65_plus { get; set; }
        public string parent_in_law_of_householder_is_65_plus { get; set; }
        public string other_relative_of_householder_is_65_plus { get; set; }
        public string non_relative_of_householder_is_65_plus { get; set; }
        public string in_non_family_households_65_plus { get; set; }
        public string non_family_householder_male_is_65_plus { get; set; }
        public string non_family_householder_male_alone_is_65_plus { get; set; }
        public string non_family_householder_male_not_alone_is_65_plus { get; set; }
        public string non_family_householder_female_is_65_plus { get; set; }
        public string non_family_householder_female_alone_is_65_plus { get; set; }
        public string non_family_householder_female_not_alone_is_65_plus { get; set; }
        public string non_family_household_non_relative_of_householder_is_65_plus { get; set; }
        public string in_group_quarters_65_plus { get; set; }
        public string institutionalized_65_plus { get; set; }
        public string non_institutionalized_65_plus { get; set; }
        public string population_in_group_quarters { get; set; }
        public string institutionalized_population { get; set; }
       
      
       
       
        public string noninstitutionalized_population { get; set; }
       
   
 
        public string total_households_by_occupancy_status { get; set; }

        public string total_households_vacant { get; set; }
        public string total_households_by_tenure { get; set; }
       
       
       
       
     
 
    
       
    
      
       
       
        public string total_households_by_age { get; set; }
        public string total_owned_households_by_age { get; set; }
      
      
       

        public string MalePopulation { get; set; }
        public string TotalFemalePopulationCensus { get; set; }

    }

    public class ZipcodebyState
    {
        public string zip { get; set; }
        public string type { get; set; }
        public string county { get; set; }
        public string area_codes { get; set; }
        public string City { get; set; }


    }
    public class ZipByStateandCity
    {
        public string Zipcode { get; set; }
        public string City { get; set; }

        public string type { get; set; }
        public string population_count { get; set; }
        public string acceptable_cities { get; set; }
        public string unacceptable_cities { get; set; }
    }
    public class CitiesInZip
    {
        public string ZipCode { get; set; }
        public string Acceptablecities { get; set; }
        public string UnacceptableCities { get; set; }
        public string Primarycity { get; set; }
        public string State { get; set; }
    }

    public class StatsDemographics
    {
        public string MedianHomeValue { get; set; }
        public string HousingUnitCount { get; set; }
        public string AreaLand { get; set; }
        public string AreaLandCensus { get; set; }
        public string total_households_occupied { get; set; }
        public string PopulationCount { get; set; }
        public string AreaWater { get; set; }
        public string MedianHouseholdIncome { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
    }

    public class EstimatedPopulationOverTime
    {
        public string EstimatedPopulation2005 { get; set; }
        public string EstimatedPopulation2006 { get; set; }
        public string EstimatedPopulation2007 { get; set; }
        public string EstimatedPopulation2008 { get; set; }
        public string EstimatedPopulation2009 { get; set; }
        public string EstimatedPopulation2010 { get; set; }
        public string EstimatedPopulation2011 { get; set; }
        public string EstimatedPopulation2012 { get; set; }
        public string EstimatedPopulation2013 { get; set; }

        public string ZipCode { get; set; }
    }

    public class EstimatedHouseholdsOverTime
    {
        public string ZipCode { get; set; }

        public string EstimatedHouseholds2005 { get; set; }
        public string EstimatedHouseholds2006 { get; set; }
        public string EstimatedHouseholds2007 { get; set; }
        public string EstimatedHouseholds2008 { get; set; }
        public string EstimatedHouseholds2009 { get; set; }
        public string EstimatedHouseholds2010 { get; set; }
        public string EstimatedHouseholds2011 { get; set; }
        public string EstimatedHouseholds2012 { get; set; }
        public string EstimatedHouseholds2013 { get; set; }

    }

    public class TotalPopulationByAge
    {
        public string TotalPopulationBySexAndAge { get; set; }
        public string MaleUnder5 { get; set; }
        public string Male5To9 { get; set; }
        public string Male10To14 { get; set; }
        public string Male15To17 { get; set; }
        public string Male18To19 { get; set; }
        public string Male20 { get; set; }
        public string Male21 { get; set; }
        public string Male22To24 { get; set; }
        public string Male25To29 { get; set; }
        public string Male30To34 { get; set; }
        public string Male35To39 { get; set; }
        public string Male40To44 { get; set; }
        public string Male45to49 { get; set; }
        public string Male50To54 { get; set; }
        public string Male55To59 { get; set; }
        public string Male60To61 { get; set; }
        public string Male62To64 { get; set; }
        public string Male65To66 { get; set; }
        public string Male67To69 { get; set; }
        public string MAle70To74 { get; set; }
        public string Male75To79 { get; set; }
        public string Male80To84 { get; set; }
        public string Male85Plus { get; set; }

        public string FemaleUnder5 { get; set; }
        public string Female5to9 { get; set; }
        public string Female10To14 { get; set; }
        public string Female15To17 { get; set; }
        public string Female18to19 { get; set; }
        public string Female20 { get; set; }
        public string Female21 { get; set; }
        public string Female22To24 { get; set; }
        public string Female25To29 { get; set; }
        public string Female30To34 { get; set; }
        public string Female35To39 { get; set; }
        public string Female40To44 { get; set; }
        public string Female45To49 { get; set; }
        public string Female50To54 { get; set; }
        public string Female55To59 { get; set; }
        public string Female60To61 { get; set; }
        public string Female62to64 { get; set; }
        public string Female65To66 { get; set; }
        public string Female67To69 { get; set; }
        public string Female70To74 { get; set; }
        public string Female75To79 { get; set; }
        public string Female80To84 { get; set; }
        public string Female85Plus { get; set; }
        public string MedianAge { get; set; }
        public string MAleMedianAge { get; set; }
        public string FemaleMedainAge { get; set; }

        public string ZipCode { get; set; }
    }
    public class ChildrenByAge
    {
        public string ZipCode { get; set; }
        public string Male1 { get; set; }
        public string Male2 { get; set; }
        public string Male3 { get; set; }
        public string Male4 { get; set; }
        public string Male5 { get; set; }
        public string Male6 { get; set; }
        public string Male7 { get; set; }
        public string Male8 { get; set; }
        public string Male9 { get; set; }
        public string Male10 { get; set; }
        public string Male11 { get; set; }
        public string Male12 { get; set; }
        public string Male13 { get; set; }
        public string Male14 { get; set; }
        public string Male15 { get; set; }
        public string Male16 { get; set; }
        public string Male17 { get; set; }
        public string Male18 { get; set; }
        public string Male19 { get; set; }
        public string Male20 { get; set; }

        public string Female1 { get; set; }
        public string Female2 { get; set; }
        public string Female3 { get; set; }
        public string Female4 { get; set; }
        public string Female5 { get; set; }
        public string Female6 { get; set; }
        public string Female7 { get; set; }
        public string Female8 { get; set; }
        public string Female9 { get; set; }
        public string Female10 { get; set; }
        public string Female11 { get; set; }
        public string Female12 { get; set; }
        public string Female13 { get; set; }
        public string Female14 { get; set; }
        public string Female15 { get; set; }
        public string Female16 { get; set; }
        public string Female17 { get; set; }
        public string Female18 { get; set; }
        public string Female19 { get; set; }
        public string Female20 { get; set; }
    }

    public class HouseholdWithKids
    {
        public string households_wo_kids { get; set; }
        public string TotalHouseholdsWKids { get; set; }
        public string ZipCode { get; set; }
        public string AverageHouseHoldSize { get; set; }

    }

    public class GenderPopulation
    {
        public string ZipCode { get; set; }
        public string TotalMalePopulation { get; set; }
        public string TotalFemalePopulation { get; set; }

    }

    public class Race
    {
        public string ZipCode { get; set; }
        public string White { get; set; }
        public string BlackOrAfricanAmerican { get; set; }
        public string AmericanIndian { get; set; }
        public string Asian { get; set; }
        public string NativeHawaiian { get; set; }
        public string OtherRace { get; set; }
        public string TwoOrMoreRace { get; set; }
    }

    public class HouseholdHeadAge
    {
        public string owner_15_to_24 { get; set; }
        public string owner_25_to_34 { get; set; }
        public string owner_35_to_44 { get; set; }
        public string owner_45_to_54 { get; set; }
        public string owner_55_to_59 { get; set; }
        public string owner_60_to_64 { get; set; }
        public string owner_65_to_74 { get; set; }
        public string owner_75_to_84 { get; set; }
        public string owner_85_plus { get; set; }
        public string renter_15_to_24 { get; set; }
        public string renter_25_to_34 { get; set; }
        public string renter_35_to_44 { get; set; }
        public string renter_45_to_54 { get; set; }
        public string renter_55_to_59 { get; set; }
        public string renter_60_to_64 { get; set; }
        public string renter_65_to_74 { get; set; }
        public string renter_75_to_84 { get; set; }
        public string renter_85_plus { get; set; }
        public string ZipCode { get; set; }
    }

    public class FamilySingles
    {
        public string HousbandWifeFamilyHouseholds { get; set; }
        public string OtherFamilyHouseholds { get; set; }
        public string LivingAlone { get; set; }
        public string NotLivingAlone { get; set; }
        public string ZipCode { get; set; }
    }
    public class PopulationBasedOnAge
    {
        public string PopulationUnder10 { get; set; }
        public string PopulationUnder10To19 { get; set; }
        public string Population20To29 { get; set; }
        public string Population30To39 { get; set; }

        public string Population40to49 { get; set; }
        public string Population50to59 { get; set; }
        public string Population60to69 { get; set; }
        public string Population70To79 { get; set; }
        public string Population80Plus { get; set; }
    }

    public class ZipCodeDetails
    {
        public string ZipID { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Decommissioned { get; set; }
        public string Primarycity { get; set; }
        public string AreaCodes { get; set; }
        public string WorldRegion { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PreciseLatitute { get; set; }
        public string PreciseLongitude { get; set; }
        public string LatitudeMin { get; set; }
        public string LongitudeMax { get; set; }
        public string LongitudeMin { get; set; }
        public string LatitudeMax { get; set; }

    }

    public class VacancyReasons
    {
        public string vacant_housing_units_for_rent { get; set; }
        public string vacant_housing_units_rented_and_unoccupied { get; set; }
        public string vacant_housing_units_for_sale_only { get; set; }
        public string vacant_housing_units_for_season_recreational_or_occasional_use { get; set; }
        public string vacant_housing_units_for_migrant_workers { get; set; }
        public string vacant_housing_units_vacant_for_other_reasons { get; set; }
        public string vacant_housing_units_sold_and_unoccupied { get; set; }
        public string ZipCode { get; set; }
    }

    public class HousingOccupancy
    {
        public string owned_households_with_a_mortgage_or_loan { get; set; }
        public string owned_households_free_and_clear { get; set; }
        public string total_rented_households_by_age { get; set; }
        public string total_households_by_vacancy_status { get; set; }
        public string renter_occupied_households { get; set; }
        public string ZipCode { get; set; }
    }

    public class HousingType
    {
        public string population_in_occupied_housing_units { get; set; }
        public string correctional_facility_for_adults_population { get; set; }
        public string juvenile_facilities_population { get; set; }
        public string nursing_facilities_population { get; set; }
        public string college_student_housing_population { get; set; }
        public string military_quarters_population { get; set; }
        public string other_noninstitutional_population { get; set; }
        public string other_institutional_population { get; set; }
        public string ZipCode { get; set; }
    }

    public class City_State
    {
        public string CityState { get; set; }
        public string County { get; set; }
        public string AreaCode { get; set; }
        public string CoOrdinates { get; set; }
        public string MostPopulatedZip { get; set; }
    }
}
