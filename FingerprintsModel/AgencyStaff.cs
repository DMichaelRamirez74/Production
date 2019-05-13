using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
//using static FingerprintsModel.RosterNew;

namespace FingerprintsModel
{
    public class AgencyStaff
    {
        public Guid AgencyStaffId { get; set; }
        public string enrollid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Race { get; set; }
        public string Natinality { get; set; }
        public string GERole { get; set; }
        public string PIRRole { get; set; }
        public string HireDate { get; set; }
        public string TerDate { get; set; }
        public string EmailAddress { get; set; }
        public string CellNumber { get; set; }
        public string HRCenter { get; set; }
        public string HighestEducation { get; set; }
        public string EarlyChildHood { get; set; }
        public string GettingDegree { get; set; }
        public string HourlyRate { get; set; }
        public string Gender { get; set; }
        public string Salary { get; set; }
        public string Contractor { get; set; }
        public string AssociatedProgram { get; set; }
        public string Replacement { get; set; }
        public string DOB { get; set; }
        public string AccessStartDate { get; set; }
        public string AccessDays { get; set; }
        public string AccessStart { get; set; }
        public string AccessStop { get; set; }
        public HttpPostedFileBase Avatar { get; set; }
        public HttpPostedFileBase AvatarH { get; set; }
        public HttpPostedFileBase AvatarS { get; set; }
        public HttpPostedFileBase AvatarT { get; set; }
        public string AvatarUrl { get; set; }
        public string AvatarhUrl { get; set; }
        public string AvatarsUrl { get; set; }
        public string AvatartUrl { get; set; }
        public string LoginIdFk { get; set; }
        public string ApprovedBy { get; set; }
        public string VerifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string EnrollmentId { get; set; }
        public bool ByEnrollmentProcess { get; set; }
        public bool ISApproved { get; set; }
        public bool ISVerified { get; set; }
        public char ISActive { get; set; }
        public char AgencyStatus { get; set; }
        public Guid SelectedAgencyId { get; set; }
        public string SelectedRoleId { get; set; }
        public string ServiceYears { get; set; }

        [Display(Name = "Login allowed")]
        public bool LoginAllowed { get; set; }
        public List<Agency> agncylist = new List<Agency>();
        public List<Role> rolelist = new List<Role>();
        public List<Role> _rolelist = new List<Role>();
        public List<RaceInfo> raceList = new List<RaceInfo>();
        public List<NationalityInfo> nationList = new List<NationalityInfo>();
        public List<EducationLevelIno> EducationLevelList = new List<EducationLevelIno>();
        public List<HrCenterInfo> HrcenterList = new List<HrCenterInfo>();
        public List<PirInfo> Pirlist = new List<PirInfo>();
        public List<TimeZoneinfo> TimeZonelist = new List<TimeZoneinfo>();
        public List<ProgInfo> refList = new List<ProgInfo>();
        public List<RefInfo> progList = new List<RefInfo>();
        //Changes
        public string Classrooms { get; set; }
        public List<ClassRoom> Classroom = new List<ClassRoom>();
        public string Users { get; set; }
        public List<UserInfo> UserList { get; set; }
        public int ClassAssignID { get; set; } //29Aug2016
        public List<classAssign> UserAssignList { get; set; }//30
        public string UserAssign { get; set; }//30
        //End
        public string TimeZoneID { get; set; }
        public string roleName { get; set; }
        public string encryptedId { get; set; }
        public string createdDate { get; set; }
        public string PirRoleid { get; set; }
        public string centerlist { get; set; }
        public string Rolelist { get; set; }

        public List<PrimaryLanguages> LangList = new List<PrimaryLanguages>();



        public List<HrCenterInfo> centers = new List<HrCenterInfo>();
        public int Id
        {
            get;
            set;
        }
        public string AgencyName { get; set; }

        public string ProgramYear { get; set; }

        public List<SelectListItem> ProgramYearList { get; set; }

    }

    public class RefInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    
    public class ProgInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class RaceInfo
    {
        public string RaceId { get; set; }
        public string Name { get; set; }
    }

    public class NationalityInfo
    {
        public string NationId { get; set; }
        public string Name { get; set; }
    }

    public class EducationLevelIno
    {
        public string EducationLevel { get; set; }
        public string Name { get; set; }
    }
    public class HrCenterInfo
    {
        public string CenterId { get; set; }
        public string Enc_CenterID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string SeatsAvailable { get; set; }
        public string yakkrcount { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string FutureWaiting { get; set; }
        public string Routecode100 { get; set; }
        public string Routecode101 { get; set; }
        public string Routecode102 { get; set; }
        public string Routecode106 { get; set; }
        public string Routecode107 { get; set; }
        public string TotalWaitingList { get; set; }
        public string Attendance { get; set; }
        public bool Homebased { get; set; }
        public List<HrCenterInfo> AllCentersList { get; set; }

        public int HomeVisitAppointment { get; set; }
        public int OverIncome { get; set; }
        public int Disability { get; set; }
        public decimal ClosingRate { get; set; }
    }

    public class Roster
    {
        public string CenterId { get; set; }
        public string Householid { get; set; }
        public string EHouseholid { get; set; }
        public string ProgramId { get; set; }
        public string ProgramType { get; set; }
        [Required()]
        public string Eclientid { get; set; }
        public string Name { get; set; }
        public string CenterName { get; set; }
        public string ClassroomName { get; set; }
        public string classroomid { get; set; }
        public string clientenrolled { get; set; }
        public string TotalScreening { get; set; }
        public string ExpectedScreening { get; set; }
        public string MissingScreening { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public int IsWithdrawn { get; set; }
        public string DOB { get; set; }
        public string MissingScreeningdate { get; set; }
        public string MissingScreeningstatus { get; set; }
        public string FSW { get; set; }
        public string Teacher { get; set; }
        public string Dayscount { get; set; }
        public string Screeningid { get; set; }
        public string ScreeningName { get; set; }
        public string RosterYakkr { get; set; }
        // changed by shambhu 21 march
        public string totalday { get; set; }
        // changes by shambhu 16 march 17
        public string recordCreated { get; set; }
        public int IsPresent { get; set; }//Added on 30Dec2016
        public string District { get; set; }
        public List<ClassRoom> ClassroomsNurse { get; set; }//Added on 28Dec2016
        public List<Roster> Rosters { get; set; }
        public List<HrCenterInfo> Centers { get; set; }
        public List<DisableNotes> disablenotes { get; set; }

        public string Acronym { get; set; }
        public string SpecialService { get; set; }
        public int  Yakkr600 { get; set; }
        public int Yakkr601 { get; set; }

        public decimal PresentCount { get; set; }
        public decimal TotalCount { get; set; }
        public int IssuePercentage { get; set; }
        public ClosedInfo ClosedDetails { get; set; }
		 public int AbsenceReasonId { get; set; }
        public string MarkAbsenseReason { get; set; }

        public string LastCaseNoteDate { get; set; }
        public string LastFPADate { get; set; }
        public string LastReferralDate { get; set; }
        public string LateArivalNotes { get; set; }
        public string LateArrivalDuration { get; set; }
        public bool IsLateArrival { get; set; }
        public bool IsCaseNoteEntered { get; set; }
        public int IsHomeBased { get; set; }
		public int IsAppointMentYakkr600601 { get; set; }
        public List<SelectListItem> AbsenceReasonList { get; set; }
        public List<SelectListItem> AbsenceTypeList { get; set; }

        public bool IsScreeningFollowUpReq { get; set; }

        public bool IsScreeningFollowUpComplete { get; set; }

        public int ReferenceProg { get; set; }

        public bool IsFutureWithdrawal { get; set; }

        public decimal Age { get; set; }

        public int IsPreg { get; set; }
        public int IsClassStarted { get; set; }
        public int PrimaryInsurance { get; set; }

        public List<CenterAndClassRoom> CenterAndClassRoom { get; set; }
        public List<CaseNoteDetails> CaseNoteDetails { get; set; }
        public int FamilyHomeless { get; set; }

        public bool IsShowTransition { get; set; }
        public string TransitionColor{get;set;}

        public int TransitionType { get; set; }

        public string Returning { get; set; }

        public bool IsShowScreeningFollowUp { get; set; }

        public string FamilyAdvocate { get; set; }
        public bool IsAllowAttendanceIssueReview { get; set; }

    }
  public class CenterAndClassRoom
    {
        public string Name { get; set; }
        public string clientenrolled { get; set; }
        public string Eclientid { get; set; }
    }

    public class CaseNoteDetails
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class HouseholdTags
    {
        public string TagName { get; set; }
        public string TagId { get; set; }
        public int TagCount { get; set; }
        public int CasenoteIdByTags { get; set; }
        public List<AssociatedTags> AssociatedTags { get; set; }

    }

    public class AssociatedTags
    {
        public string TagName { get; set; }
        public string TagId { get; set; }
        public string CaseNoteId { get; set; }

    }

    public class CaseNote :RosterNew.CaseNote
    {
        public string Householid { get; set; }
        public string clientid { get; set; }
        public string Staffid { get; set; }
        //public string CaseNoteid { get; set; }
        public string WrittenBy { get; set; }
        public string Name { get; set; }
        public string BY { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string Attachment { get; set; }
        public string References { get; set; }
        public string Programyear { get; set; }
        //public string Note { get; set; }
        public string Tagname { get; set; }
        public bool SecurityLevel { get; set; }
        public bool IsAllowSecurityCN { get; set; }
        public string GroupCaseNote  { get; set; }
        //public string SecurityRoles { get; set; }
        public bool IsEditable { get; set; }
        public string ProgramID { get; set; }
        public string ProgramType { get; set; }
    }


    public class SubCaseNote
    {

        public  string Name { get; set; }
        public string SubCasenoteid { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Attachment { get; set; }

        public string Notes { get; set; }
        public string WrittenBy { get; set; }
        public string WrittenDate { get; set; }

    }

    public class CaseNoteByClientID : Pagination
    {
        public string Role { get; set; }
        public List<CaseNote> CaseNoteList { get; set; }
        public List<RosterNew.User> SecurityRolesList { get; set; }
        public List<RosterNew.User> Clientlist { get; set; }
        public List<RosterNew.User> UserList { get; set; }
     
    
        public List<ParentInfo> ParentContactInfoList { get; set; }

        public string ReferenceYakkrID { get; set; }
    }


    public class Waitinginfo
    {
        public string CenterId { get; set; }
        public string Householid { get; set; }
        public string Clientid { get; set; }
        public string Programid { get; set; }
        public string Options { get; set; }
        public string Notes { get; set; }
    }

    public class Fswuserapproval :YakkrClientDetail
    {
        public string StaffName { get; set; }
        public string routecode { get; set; }
        public string Status { get; set; }

    }


    public class YakkrDetails
    {
        public string CenterId { get; set; }
        public string ClientId { get; set; }
        public string HouseholdId { get; set; }
        public string ClientName { get; set; }
        public string Date { get; set; }
        public string CenterName { get; set; }
        public string StaffName { get; set; }
        public string StaffId { get; set; }
        public string routecode { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Yakkrid { get; set; }

    }


    public class PirInfo
    {
        public string PirId { get; set; }
        public string Name { get; set; }

    }
    public class UserInfo
    {
        public string userId { get; set; }
        public string Name { get; set; }
        public int ClassroomID { get; set; }//
        public int ClassAssignId { get; set; }
        public bool IsSelected { get; set; }

    }

    public class CoreTeam
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsCoreTean { get; set; }
        public string UserColor { get; set; }
    }


    public class PIRAccessRoles
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAccessPIR { get; set; }


    }

    public class IncomeReviewRoles
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsReviewIncome { get; set; }
    }

    

    public class AcceptanceProcess
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAcceptance { get; set; }
     


    }
    public class Demographic
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsDemographic { get; set; }
        public string UserColor { get; set; }


    }
    //public class DemographicPercentage
    //{
    //    public string WorkingParent { get; set; }
    //    public string TotalChild { get; set; }
    //    public string Disability { get; set; }
    //    public string HadDoctor { get; set; }
    //    public string HadDental { get; set; }


    //}
    

    public class TimeZoneinfo
    {
        public string TimZoneId { get; set; }
        public string TimZoneName { get; set; }

    }
    public class Zipcodes
    {
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
       
    }
    
    public class Attendance
    {
        public string TodayAttendance { get; set; }
        public string WeeklyAttendance { get; set; }
        public string MonthlyAttendance { get; set; }
        public string YearlyAttendance { get; set; }

    }



    public class ClientWaitingList:ClientAcceptList
    {
        public string Id { get; set; }
        public bool IsHomeBased { get; set; }
        //public string ClientId { get; set; }
        //public string HouseholdId { get; set; }
        public string HouseholdIdencrypted { get; set; }
        public string Programid { get; set; }
        //public string CenterId { get; set; }
        //public string Name { get; set; }
        //public string Choice { get; set; }
        public string SelectionPoints { get; set; }
        //public string DateOnList { get; set; }
        //public string ProgramType { get; set; }
        //public string Gender { get; set; }
        //public string DOB { get; set; }
        //public string TotalChoice { get; set; }
        public string Option { get; set; }
        public string Notes { get; set; }
        public string IsReviewed { get; set; }
        public string Age { get; set; }
        //public List<UserInfo> UserList { get; set; }
        //public string ClassroomName { get; set; }
        //public List<FamilyHousehold.Programdetail> ProgramsList { get; set; }

        public bool ClassroomConfirmed { get; set; }

        //public string IsFutureApplication { get; set; }

        //public int AcceptanceType { get; set; }



    }
    //Changes 19Aug2016
    public class ClientAcceptList :YakkrClientDetail
    {

      

        public string Name { get; set; }
        public string Choice { get; set; }
      
        public string DateOnList { get; set; }
        public string ProgramType { get; set; }
        public string Gender { get; set; }
      
        public string TotalChoice { get; set; }
        public List<ClassRoom> Classroom = new List<ClassRoom>();

        public List<FamilyHousehold.Programdetail> ProgramsList { get; set; }
        //22Aug2016
      //  public string Description { get; set; }
        public string CTransport { get; set; }
        public Nullable<Boolean> ChildDisability { get; set; }
        public string ChildWeight { get; set; }
        public bool Income { get; set; }
        public bool Homeless { get; set; }
        public bool Foster { get; set; }
        public bool TANF { get; set; }
        public bool SNAP { get; set; }
        public bool SSI { get; set; }
        public string FoodAllergies { get; set; }
        //23Aug2016
        //
        public bool IsIEP { get; set; }
        public bool IsIFSP { get; set; }
        public bool IsExpired { get; set; }
        //
        public string CParentDisable { get; set; }
        public List<UserInfo> UserList { get; set; }
        public List<UserInfo> FSWList { get; set; }
        public List<UserInfo> UserListNutrition { get; set; }

        public int AcceptanceType { get; set; }

        public bool IsFutureApplication { get; set; }

    }
    public class ClassRoom
    {
        public string ClassName { get; set; }
        public int ClassroomID { get; set; }
        public int ActualSeats { get; set; }
        public int ProgramId { get; set; }

        public string Enc_ClassRoomId { get; set; }
    }
    
    //End
    //End
    //30Aug2016
    public class classAssign
    {
        public int ClassAssignID { get; set; }
        public int ClassroomID { get; set; }
        public string userId { get; set; }
    }
    //14-dec-2017
    public class ClientAttendancePercentage
    {

        public int CurrentDayPercentage { get; set; }
        public int WeeklyPercentage { get; set; }
        public int MonthlyPercentage { get; set; }
        public int YearlyPercentage { get; set; }
        public int DailyClientCount { get; set; }
        public int WeeklyClientCount { get; set; }
        public int MonthlyClientCount { get; set; }
        public int YearlyClientCount { get; set; }
        public int DayTotalEnroll { get; set; }
        public int WeekTotalEnroll { get; set; }
        public int MonthTotalEnroll { get; set; }
        public int YearTotalEnroll { get; set; }
    }
    

    public interface IPagination
    {
        int GetSkipRows();
    }

    public abstract class Pagination:IPagination
    {
        public  int TotalRecord { get; set; }
        public  int PageSize { get; set; }
        public  int SkipRows { get; set; }
        public int RequestedPage { get; set; }

        public string SortOrder { get; set; }
        public string SortColumn { get; set; }

        public virtual int GetSkipRows()
        {
            return (this.RequestedPage - 1) * PageSize;
        }
    }


    public  class GridParams : Pagination
    {
        public GridParams() {
            Search = "";
            SortOrder = "";
            SortColumn = "";
            RequestedPage = 1;
            PageSize = 10;
        }

        public string Search { get; set; }
       
    }

  






}
