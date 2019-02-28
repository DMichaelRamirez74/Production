using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ExecutiveDashBoard:NewProgramYearTransitionDashboard
    {
        public List<AbsenceByWeek> AbsenceReport { get; set; }
        public string AttendanceIssuePercentage { get; set; }
        public class AbsenceByWeek {
            public string week { get; set; }
            public string value { get; set; }
        }
        public string AvailablePercentage { get; set; }
        public string AvailableSeat { get; set; }
        public string YesterDayAttendance { get; set; }
        public string ADA { get; set; }
        public string FamilyOverIncome { get; set; }
        public string DisabilityPercentage { get; set; }
        public string ThermHours { get; set; }
        public string ThermDollars { get; set; }

        public string TotalHours { get; set; }
        public string TotalDollars { get; set; }
        public string WaitingList { get; set; }
        public string WaitingListCount { get; set; }

        public List<EmployeeBirthday> EmployeeBirthdayList = new List<EmployeeBirthday>();
        public class EmployeeBirthday
        {
            public string Staff { get; set; }
            public string DateOfBirth { get; set; }
        }

        public List<CaseNote> listCaseNote = new List<CaseNote>();
        public class CaseNote
        {
            public string Month { get; set; }
            public string Percentage { get; set; }
        }
        public List<EnrolledProgram> EnrolledProgramList = new List<EnrolledProgram>();
        public class EnrolledProgram
        {
            public string ProgramType { get; set; }
            public string Total { get; set; }
            public string Available { get; set; }
        }

        public List<ClassRoomType> ClassRoomTypeList = new List<ClassRoomType>();
        public class ClassRoomType
        {
            public string ClassSession { get; set; }
            public string Total { get; set; }
            public string Available { get; set; }
        }

        public List<MissingScreen> MissingScreenList = new List<MissingScreen>();
        public class MissingScreen
        {
            public string Name { get; set; }
            public string Screen { get; set; }
        }


        public List<EnrolledByCenterType> EnrollmentTypeList = new List<EnrolledByCenterType>();

        public class EnrolledByCenterType
        {
            public string CenterType { get; set; }
            public string Total { get; set; }
            public string Available { get; set; }
        }


        public enum CenterTypeEnum
        {
            [Description("Center Based")]
            CenterBased=0,
            [Description("Home Based")]
            HomeBased =1

        }

        /// <summary>
        /// Enumeration for different sections in the Executive Dashboard
        /// </summary>
        public enum DashboardSectionType
        {
            CurrentEnrollment=1,
            EnrolledByProgram=2,
            MissingScreening=3,
            ClassroomType=4,
            CaseNoteAnalysis=5,
            InKindHoursDollars=6,
            Disabilities=7,
            OverIncome=8,
            WaitingList=9
        }

        //public static string GetDescription<T>(this T e) where T : IConvertible
        //{
        //    if (e is Enum)
        //    {
        //        Type type = e.GetType();
        //        Array values = System.Enum.GetValues(type);

        //        foreach (int val in values)
        //        {
        //            if (val == e.ToInt32(CultureInfo.InvariantCulture))
        //            {
        //                var memInfo = type.GetMember(type.GetEnumName(val));
        //                var descriptionAttribute = memInfo[0]
        //                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
        //                    .FirstOrDefault() as DescriptionAttribute;

        //                if (descriptionAttribute != null)
        //                {
        //                    return descriptionAttribute.Description;
        //                }
        //            }
        //        }
        //    }

        //    return null; // could also return string.Empty
        //}

        public static string GetDescription(ExecutiveDashBoard.CenterTypeEnum Band)
        {
            System.Reflection.FieldInfo oFieldInfo = Band.GetType().GetField(Band.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])oFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return Band.ToString();
            }
        }


    }
}
