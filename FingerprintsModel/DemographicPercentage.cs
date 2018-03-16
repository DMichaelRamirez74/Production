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
            BlackOrAfrican = "0";
            AmericanIndian = "0";
            Other = "0";
            NativeHawaiian = "0";
            BiracialOrMulti = "0";
            Asian = "0";
            Unspecified = "0";
            White = "0";
            StaffBlackOrAfrican = "0";
            StaffAmericanIndian = "0";
            StaffOther = "0";
            StaffNativeHawaiian = "0";
            StaffBiracialOrMulti = "0";
            StaffAsian = "0";
        }
        public string TotalParent { get; set; }
        public string TotalClient { get; set; }
        public string WorkingParent { get; set; }
        public string WorkingParentPercent { get; set; }
        public string Insurance { get; set; }
        public string InsurancePercent { get; set; }
        public string JobParent { get; set; }
        public string JobParentPercent { get; set; }

        public string ClientDoctor { get; set; }
        public string ClientDoctorPercent { get; set; }
        public string ClientDental { get; set; }

        public string ClientDentalPercent { get; set; }
        public string ClientDisability { get; set; }
        public string ClientDisablilyPercent { get; set; }
        public string AttendanceIssue { get; set; }
        public string AttendIssuePercent { get; set; }
        public string OtherLangSpeakers { get; set; }
        public string OtherLangSpeakersPercent { get; set; }
        public string EnglishLang { get; set; }
        public string AfricanLang { get; set; }
        public string CaribbeanLang { get; set; }
        public string EastAsianLang { get; set; }
        public string EuropeanLang { get; set; }
        public string MiddleLang { get; set; }
        public string NativeCenterLang { get; set; }
        public string NativeNorthLang { get; set; }
        public string PacificLang { get; set; }
        public string SpanisLang { get; set; }
        public string Ethnicity { get; set; }
        public string NonEthnicity { get; set; }
        public string BlackOrAfrican { get; set; }
        public string AmericanIndian { get; set; }
        public string Other { get; set; }
        public string NativeHawaiian { get; set; }
        public string BiracialOrMulti { get; set; }
        public string Asian { get; set; }
        public string Unspecified { get; set; }
        public string White { get; set; }

        public string StaffBlackOrAfrican { get; set; }
        public string StaffAmericanIndian { get; set; }
        public string StaffOther { get; set; }
        public string StaffNativeHawaiian { get; set; }
        public string StaffBiracialOrMulti { get; set; }
        public string StaffAsian { get; set; }
        public string StaffUnspecified { get; set; }
        public string StaffWhite { get; set; }

        public string FamilyAvgAge { get; set; }
        public string FSWAvgAge { get; set; }
        public string TeacherAvgAge { get; set; }
    }

    public class DemoGraphicPercentageAvgAge
    {

        public string TotalParent { get; set; }
        public string TotalClient { get; set; }
        public string WorkingParent { get; set; }
        public string WorkingParentPercent { get; set; }
        public string Insurance { get; set; }
        public string InsurancePercent { get; set; }
        public string JobParent { get; set; }
        public string JobParentPercent { get; set; }

        public string ClientDoctor { get; set; }
        public string ClientDoctorPercent { get; set; }
        public string ClientDental { get; set; }

        public string ClientDentalPercent { get; set; }
        public string ClientDisability { get; set; }
        public string ClientDisablilyPercent { get; set; }
        public string AttendanceIssue { get; set; }
        public string AttendIssuePercent { get; set; }
        public string OtherLangSpeakers { get; set; }
        public string OtherLangSpeakersPercent { get; set; }
        public string EnglishLang { get; set; }
        public string AfricanLang { get; set; }
        public string CaribbeanLang { get; set; }
        public string EastAsianLang { get; set; }
        public string EuropeanLang { get; set; }
        public string MiddleLang { get; set; }
        public string NativeCenterLang { get; set; }
        public string NativeNorthLang { get; set; }
        public string PacificLang { get; set; }
        public string SpanisLang { get; set; }
        public string Ethnicity { get; set; }
        public string NonEthnicity { get; set; }


    }
}
