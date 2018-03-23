using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class DemographicPercentage
    {

        public DemographicPercentage()
        {
            //BlackOrAfrican = "0";
            //AmericanIndian = "0";
            //Other = "0";
            //NativeHawaiian = "0";
            //BiracialOrMulti = "0";
            //Asian = "0";
            //Unspecified = "0";
            //White = "0";
            //StaffBlackOrAfrican = "0";
            //StaffAmericanIndian = "0";
            //StaffOther = "0";
            //StaffNativeHawaiian = "0";
            //StaffBiracialOrMulti = "0";
            //StaffAsian = "0";
        }
        public int TotalParent { get; set; }
        public int TotalClient { get; set; }
        public int WorkingParent { get; set; }
        public decimal WorkingParentPercent { get; set; }
        public int Insurance { get; set; }
        public decimal InsurancePercent { get; set; }
        public int JobParent { get; set; }
        public decimal JobParentPercent { get; set; }

        public int ClientDoctor { get; set; }
        public decimal ClientDoctorPercent { get; set; }
        public int ClientDental { get; set; }

        public decimal ClientDentalPercent { get; set; }
        public int ClientDisability { get; set; }
        public decimal ClientDisablilyPercent { get; set; }
        public int AttendanceIssue { get; set; }
        public decimal AttendIssuePercent { get; set; }
        public int OtherLangSpeakers { get; set; }
        public decimal OtherLangSpeakersPercent { get; set; }
        public int EnglishLang { get; set; }
        public int AfricanLang { get; set; }
        public int CaribbeanLang { get; set; }
        public int EastAsianLang { get; set; }
        public int EuropeanLang { get; set; }
        public int MiddleLang { get; set; }
        public int NativeCenterLang { get; set; }
        public int NativeNorthLang { get; set; }
        public int PacificLang { get; set; }
        public int SpanisLang { get; set; }
        public int OtherLang { get; set; }
        public int Ethnicity { get; set; }
        public int NonEthnicity { get; set; }
        public int BlackOrAfrican { get; set; }
        public int AmericanIndian { get; set; }
        public int Other { get; set; }
        public int NativeHawaiian { get; set; }
        public int BiracialOrMulti { get; set; }
        public int Asian { get; set; }
        public int Unspecified { get; set; }
        public int White { get; set; }

        public int StaffBlackOrAfrican { get; set; }
        public int StaffAmericanIndian { get; set; }
        public int StaffOther { get; set; }
        public int StaffNativeHawaiian { get; set; }
        public int StaffBiracialOrMulti { get; set; }
        public int StaffAsian { get; set; }
        public int StaffUnspecified { get; set; }
        public int StaffWhite { get; set; }

        public decimal FamilyAvgAge { get; set; }
        public decimal FSWAvgAge { get; set; }
        public decimal TeacherAvgAge { get; set; }

        public int StaffEnglishLang { get; set; }
        public int StaffAfricanLang { get; set; }
        public int StaffCaribbeanLang { get; set; }
        public int StaffEastAsianLang { get; set; }
        public int StaffEuropeanLang { get; set; }
        public int StaffMiddleLang { get; set; }
        public int StaffNativeCenterLang { get; set; }
        public int StaffNativeNorthLang { get; set; }
        public int StaffPacificLang { get; set; }
        public int StaffSpanisLang { get; set; }
        public int StaffOtherLang { get; set; }

    }

   
}
