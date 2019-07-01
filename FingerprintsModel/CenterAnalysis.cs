using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class CenterAnalysis
    {
        public long CenterId { get; set; }
        public string Enc_CenterId { get; set; }
        public string CenterName { get; set; }
        public long Seats { get; set; }
        public long Enrolled { get; set; }
        public long Waiting { get; set; }

        public double PercentageFilled { get; set; }
        public long Returning { get; set; }
        public long Graduating { get; set; }
        public long OverIncome { get; set; }
        public long Foster { get; set; }
        public long HomeLess { get; set; }
        public long Leads { get; set; }
        public string ProgramId { get; set; }
        public long WithDrawn { get; set; }
        public long Dropped { get; set; }
    }

    public class CenterAnalysisPercentage
    {
        public long TotalSeats { get; set; }
        public long TotalEnrolled { get; set; }
        public long TotalWaiting { get; set; }

        public double TotalPercentageFilled { get; set; }
        public long TotalReturned { get; set; }
        public long TotalGraduating { get; set; }
        public long TotalOverIncome { get; set; }
        public long TotalFoster { get; set; }
        public long TotalHomeLess { get; set; }
        public long TotalWithdrawn { get; set; }
        public long TotalDropped { get; set; }
        public long TotalLeads { get; set; }
    }

    public class CenterAnalysisModule
    {
        public List<CenterAnalysis> CenterAnalysisList { get; set; }

        public CenterAnalysisPercentage CenterPercentage { get; set; }
    }

    public class FosterChild
    {
        public long ClientId { get; set; }
        public string Enc_ClientId { get; set; }
        public string ClientName { get; set; }
        public string FileAttached { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long CenterId { get; set; }
        public long ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public string CenterName { get; set; }
        public string ProgramType { get; set; }
        public long ProgramId { get; set; }
        public string Dob { get; set; }
        public string ClassStartDate { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }

    }

    public class HomelessChildren
    {
        public string ChildrenName { get; set; }
        public long ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public string CenterName { get; set; }
        public string ProgramType { get; set; }
        public long ProgramId { get; set; }
        public string Dob { get; set; }
        public string ClassStartDate { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }
        public string Enc_ClientId { get; set; }
    }
    public class ChildrenInfoClass
    {

        public ChildrenInfoClass()
        {
            this.ChildrenList = new List<ChildrenInfo>();
            this.ClassRoomInfoList = new List<ClassRoomDetails>();
            this.WaitingChildrenList = new List<WaitingChildren>();
            this.WithdrawnChildrenList = new List<WaitingChildren>();
            this.DroppedChildrenList = new List<WaitingChildren>();
            this.ReturningList = new List<ChildrenInfo>();
            this.GraduatingList = new List<ChildrenInfo>();
            this.OverIncomeChildrenList = new List<ChildrenInfo>();
            this.FosterChildrenList = new List<FosterChild>();
            this.HomeLessChildrenList = new List<HomelessChildren>();
            this.LeadsChildrenList = new List<LeadsChildren>();
            this.TotalRecord = 0;
        }

        public List<ChildrenInfo> ChildrenList { get; set; }
        public List<ClassRoomDetails> ClassRoomInfoList { get; set; }

        public List<WaitingChildren> WaitingChildrenList { get; set; }

        public List<WaitingChildren> WithdrawnChildrenList { get; set; }
        public List<WaitingChildren> DroppedChildrenList { get; set; }

        public List<ChildrenInfo> ReturningList { get; set; }

        public List<ChildrenInfo> GraduatingList { get; set; }

        public List<ChildrenInfo> OverIncomeChildrenList { get; set; }

        public List<FosterChild> FosterChildrenList { get; set; }


        public List<HomelessChildren> HomeLessChildrenList { get; set; }

        public List<LeadsChildren> LeadsChildrenList { get; set; }
        public int TotalRecord { get; set; }
    }
    public class ChildrenInfo
    {
        public string ClassRoomName { get; set; }
        public string ClassRoomID { get; set; }
        public string ClientName { get; set; }

        public string DateOfFirstService { get; set; }
        public string ClassStartDate { get; set; }
        public string OverIncome { get; set; }
        public string Foster { get; set; }
        public string Image { get; set; }
        public string ChildAttendance { get; set; }
        public string Gender { get; set; }
        public string AttendancePercentage { get; set; }
        public string Dob { get; set; }

        public string CenterID { get; set; }
        public string CenterName { get; set; }
        public string ProgramType { get; set; }
        public string Amount1 { get; set; }
        public string Amount2 { get; set; }
        public string ChildIncome { get; set; }
        public string ClientId1 { get; set; }
        public string ClientId2 { get; set; }
        public string ClientId { get; set; }
        public string ParentName { get; set; }
        public string Enc_ClientId { get; set; }

        public string Age { get; set; }
        public string AgeInWords { get; set; }
        public int AgeInMonths { get; set; }

    }

    public class EnrolledChildren
    {
        public string ChildrenName { get; set; }
        public long ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public string CenterName { get; set; }
        public long CenterId { get; set; }
        public string ProgramType { get; set; }
        public long ProgramId { get; set; }
        public string Dob { get; set; }
        public string ClassStartDate { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }
        public long ClientId { get; set; }

    }

    public class LeadsChildren
    {
        public string ChildrenName { get; set; }
        public string ParentName { get; set; }
        public string Dob { get; set; }
        public string phoneNumber { get; set; }
        public string Gender { get; set; }
        public string Disability { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string Extension { get; set; }
        public string IsPartyDay { get; set; }
        public string IsHomebased { get; set; }
        public string Isfullday { get; set; }
        public string EmailAddress { get; set; }
        public string IsSchoolDay { get; set; }
        public string childTransport { get; set; }
        public string CenterId { get; set; }
        public string CenterName { get; set; }
        public string ContactStatus { get; set; }
        public string RejectParentId { get; set; }
        public string ParentId { get; set; }
        public string YakkrStatus { get; set; }

        public string Enc_ClientId { get; set; }
        public string ClientId { get; set; }
        public string StaffUserId { get; set; }
        public string FSWName { get; set; }
    }

    public class ClassRoomDetails
    {
        public string ClassRoomName { get; set; }
        public string EnC_ClassRoomId { get; set; }
        public long ClassRoomId { get; set; }
    }

    public class WaitingChildren
    {
        public string ChildrenName { get; set; }
        // public long ClassRoomId { get; set; }
        // public string ClassRoomName { get; set; }
        public string CenterName { get; set; }
        public string ProgramType { get; set; }
        public long ProgramId { get; set; }
        public string Dob { get; set; }
        public string DateOnList { get; set; }
        public string ProfileImage { get; set; }
        public string Gender { get; set; }
        public long SelectionPoints { get; set; }
        public string CenterChoice { get; set; }
        public string Enc_ClientId { get; set; }


    }

    public class CenterAnalysisParameters
    {

        public CenterAnalysisParameters()
        {
            this.StaffDetails = StaffDetails.GetInstance();
            this.SearchText = string.Empty;
        }
        public StaffDetails StaffDetails { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string SearchText { get; set; }

        public long ProgramId { get; set; }

        public long CenterId { get; set; }

        public int RequestedPage { get; set; }

        public long ClassRoomId { get; set; }


    }

}
