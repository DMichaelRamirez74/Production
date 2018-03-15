using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ExecutiveDashBoard
    {
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
    }
}
