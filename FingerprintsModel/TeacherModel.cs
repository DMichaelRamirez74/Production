using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class TeacherModel:IInkindPeriod
    {
       
        public string Tdate { get; set; }
        public String ClientID { get; set; }
        public String CName { get; set; }
        public String CDOB { get; set; }
        public string Enc_CenterId { get; set; }
        public string Enc_ClassRoomId { get; set; }
        public List<TeacherModel> Itemlst { get; set; }
        public List<TeacherModel> Observationlst { get; set; }

        public List<InkindActivity> Activitylst { get; set; }
        public List<TeacherModel> ObservationlstChecked { get; set; }
        public List<TeacherModel> Meallst { get; set; }

        public List<FamilyHousehold> EmergencyContactList { get; set; }

        public string AccessDateString { get; set; }

        public string RestrictedDateString { get; set; }

        public String CImage { get; set; }
        public string CIFileName { get; set; }
        public string CIFileExtension { get; set; }
        public byte[] CIFileData { get; set; }
        public decimal PercentAbsent { get; set; }
        public string EnrollmentDays { get; set; }
        public string AttendanceType { get; set; }
        public string CNotes { get; set; }
        public string Parent1ID { get; set; }
        public string Parent1Name { get; set; }
        public string Parent2ID { get; set; }
        public string Parent2Name { get; set; }
        public string ParentSig { get; set; }
        public string ParentSigOut { get; set; }
        public string ParentCheckedIn { get; set; }
        public string ParentCheckedOut { get; set; }
        public string ParentSig2 { get; set; }
        public string ParentSigOut2 { get; set; }
        public string ParentCheckedIn2 { get; set; }
        public string ParentCheckedOut2 { get; set; }
        public string OtherName { get; set; }
        public string OtherNameOut { get; set; }
        public string OtherNameIn2 { get; set; }
        public string OtherNameOut2 { get; set; }
        public string OtherNameTeacher { get; set; }
        public string TeacherName { get; set; }

        public int TeacherTimeZoneDiff { get; set; }
        public int FSWTimeZoneDiff { get; set; }
        public string FSWName { get; set; }
        public string TeacherCheckedIn { get; set; }
        public string ObservationID { get; set; }
        public string ObservationDescription { get; set; }
        public string ActivityCode { get; set; }
        public string ActivityDescription { get; set; }
        public string TeacherCheckInSig { get; set; }
        public string ObservationIDChecked { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Snack { get; set; }
        public bool Dinner { get; set; }
        public bool Snack2 { get; set; }
        public string MealID { get; set; }
        public string MealType { get; set; }
        public string ClassID { get; set; }
        public string CenterID { get; set; }
        public string TimeIn { get; set; }
        public string TimeIn2 { get; set; }
        public string TimeOut { get; set; }
        public string TimeOut2 { get; set; }
        public string ABreakfast { get; set; }
        public string ALunch { get; set; }
        public string ASnack { get; set; }
        public string ADinner { get; set; }
        public string ASnack2 { get; set; }
        public string Available { get; set; } 
        public bool ObservationChecked { get; set; }
        public bool TeacherObservationActive { get; set; }
        public string MealSelected { get; set; }
        public string DisabilityDescription { get; set; }
        public string Disability { get; set; }
        public List<TeacherModel> Hours { get; set; }
        public List<TeacherModel> Minutes { get; set; }
        public string hourID { get; set; }
        public string hourDes { get; set; }
        public string minID { get; set; }
        public string minDes { get; set; }
        public string Programid { get; set; }
        public string Enc_ClientId { get; set; }

       public string Enc_ProgramId { get; set; }
        public string Enc_HouseholdId { get; set; }

        public string Dateofclassstartdate { get; set; }

        public string AttendanceDate { get; set; }
        public List<OfflineAttendance> WeeklyAttendance { get; set; }
        public string  WeeklyAttendancestring { get; set; }
       
        public string ChildInfoString { get; set; }
        
        public string CenterString { get; set; }
        public List<Center> CenterList { get; set; }
       
        public string UserId { get; set; }
        public string AgencyId { get; set; }

        public string AbsenceReason { get; set; }
        public int AbsenceReasonId { get; set; }
        public int AttendanceTypeId { get; set; }
        public List<SelectListItem> AbsenceReasonList { get; set; }
        public List<SelectListItem> AttendanceTypeList { get; set; }
        public string AbsenceReasonString { get; set; }
        public bool IsLateArrival { get; set; }

        public ClosedInfo ClosedDetails { get; set; }

        public int NotCheckedCount { get; set; }

        public string RosterCount { get; set; }
        public int IsCaseNoteEntered { get; set; }
        public string RoleId { get; set; }

        public string CenterName { get; set; }
        public string ClassroomName { get; set; }

        public string AllowCaseNoteTeacher { get; set; }

        public int Appointment { get; set; }

        public bool HasHomeVisit { get; set; }

        public bool HasCenterVisit { get; set; }

        public  List<InkindPeriods> InkindPeriodList {get;set;}
    }

    public class OfflineAttendance
    {
        public string ClientID { get; set; }
        public string AttendanceType { get; set; }
        public string AttendanceDate { get; set; }
        public string SignedInBy { get; set;}
        public string PSignatureIn { get; set; }
        public string PSignatureOut { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string SignedOutBy { get; set; }
        public string BreakFast { get; set; }
        public string Lunch { get; set; }
        public string Snacks { get; set; }
        public string TSignatureIn { get; set; }
        public string TSignatureOut { get; set; }
        public string UserID { get; set; }
        //public string _DecClientId { get; set; }
        public string AgencyId { get; set; }
        public string CenterID { get; set; }
        public string ClassroomID { get; set; }

        public string AbsenceReasonId { get; set; }

        public string AdultBreakFast { get; set; }
        public string AdultLunch { get; set; }

        public string AdultSnacks { get; set; }


    }

    public class DailyAttendMealsTotal
    {
        public string AdultBreakFast { get; set; }
        public string AdultLunch { get; set; }
        public string AdultSnacks { get; set; }
        public string AttendanceDate { get; set; }
        public string ChildBreakFast { get; set; }
        public string ChildExcused { get; set; }
        public string ChildLunch { get; set; }
        public string ChildPresent { get; set; }
        public string ChildSnacks { get; set; }
        public string ChildUnExcused { get; set; }
        public string DailyID { get; set; }
        public string AgencyId { get; set; }
        public string CenterID { get; set; }
        public string UserID { get; set; }
        public string ClassroomID { get; set; }

    }

    public class ClosedInfo
    {
        public int ClosedToday { get; set; }
        public string CenterName { get; set; }
        public string ClassRoomName { get; set; }
        public string AgencyName { get; set; }

        public long CenterId { get; set; }
        public long ClassRoomId { get; set; }
    }


    public class ClientGrowth
    {
        public long IndexId { get; set; }
        public string ClientID { get; set; }
       // public string EClientId { get; set; }
        public string Name { get; set; }
        public string DOB { get; set; }
        public string AssessmentDate { get; set; }

        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal BMI { get; set; }
        public decimal HeadCirc { get;set;}
        public string createdby { get; set; }

        public long InputType { get; set; }
        public decimal Age { get; set; }

      //  public DateTime CreatedDate { get; set; }
    }


    public class GrowthChart {

        public List<ClientGrowth> ChildGrowth { get; set; }

        //  public List<STDTable> STDTables { get; set; }
        public List<STDTable> DTHeadCircuGrowth { get; set; }
        public List<STDTable> DTLengthGrowth { get; set; }

        public List<STDTable> DTWeightGrowth { get; set; }

        public List<STDTable> DTWeightLengthGrowth { get; set; }

        public List<STDTable> DTBMIGrowth { get; set; }
        // public STDTable STDTables { get; set; }
    }


    public class STDTable {

        public string AgeInMonth { get; set; }
        // public string Percentiles { get; set; }
        public decimal Length { get; set; }
        public string P3 { get; set; }
        public string P5 { get; set; }
        public string P10 { get; set; }
        public string P25 { get; set; }
        public string P50 { get; set; }
        public string P75 { get; set; }
        public string P85 { get; set; }
        public string P90 { get; set; }
        public string P95 { get; set; }
        public string P97 { get; set; }
        public string P98 { get; set; }
    }

}