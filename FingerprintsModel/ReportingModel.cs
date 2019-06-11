using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class ReportingModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DOB { get; set; }
        public string Status { get; set; }
        public string Insurance { get; set; }
        public string ColumnName { get; set; }
        public string CenterName { get; set; }
        public string ClassroomName { get; set; }
        public int reporttype { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Guardian { get; set; }
        public string ProgramType { get; set; }
        public string DaysEnrolled { get; set; }
        public string ReasonForAcceptance { get; set; }
        public List<ReportingModel> Reportlst { get; set; }
        public List<ReportingModel> Meallst { get; set; }
        public List<ReportingModel> Monthlst { get; set; }
      //  public string CenterName { get; set; }
        public string MonthName { get; set; }
        public string MealCount { get; set; }
        public string AmealCount { get; set; }
        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Snack { get; set; }
        public string BreakfastTotal { get; set; }
        public string LunchTotal { get; set; }
        public string SnackTotal { get; set; }
        public string BreakfastTotalMonth { get; set; }
        public string LunchTotalMonth { get; set; }
        public string SnackTotalMonth { get; set; }
        public string ABreakfast { get; set; }
        public string ALunch { get; set; }
        public string ASnack { get; set; }
        public string MealType { get; set; }
        public string CenterID { get; set; }
        public string AttendanceMonth { get; set; }
        public string AttendanceDate { get; set; }

         public string AttendanceDateMonth { get; set; }
         public string CenterIDCenter { get; set; }
         public string AttendanceMonthCenter { get; set; }
         public string MonthNameCenter { get; set; }
         public string CenterNameCenter { get; set; }
        
            
    }


    #region CLASReport
    public class CLASReview
    {

        public long ReviewId { get; set; }
        public long Center { get; set; }
        public string CenterName { get; set; }
        public long ClassRoom { get; set; }
        public string ClassRoomName { get; set; }
        public string CommentNote { get; set; }
        public string DateofReview { get; set; }
        public string TimeofReview { get; set; }
        public decimal Score { get; set; }
        public string EnterByName { get; set; }

        public List<Attachments> CLASReviewAttachment { get; set; }
    }

    public class Attachments
    {
        public long MainTableId { get; set; }
        public long AttachmentID { get; set; }
        public HttpPostedFileBase AttachmentFile { get; set; }
        public string AttachmentJson { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentFileExtension { get; set; }
        public string AttachmentFileUrl { get; set; }
        public byte[] AttachmentFileByte { get; set; }
        // public long TransactionID { get; set; }
        public bool AttachmentStatus { get; set; }
    }


    #endregion CLASReport


    #region MDTReport


    public class MDTReport
    {

        public long MDTId { get; set; }
        public long ClientId { get; set; }
        public string DOB { get; set; }
        public string Name { get; set; }
        public string Goal { get; set; }
        public string Summary { get; set; }
        public bool IsDisability { get; set; }
        public bool IsMentalIssue { get; set; }
        /// <summary>
        /// Parent details
        /// </summary>
        public long ParentId { get; set; }
        public string ParentSign { get; set; }
        public int ParentSignType { get; set; }
        /// <summary>
        /// Facilitator details
        /// </summary>
       public string FacilitatorId { get; set; }
        public string FacilitatorSign { get; set; }
        public int FacilitatorSignType { get; set; }

        /// <summary>
        /// FamilyAdvocate Details
        /// </summary>
        public string FamilyAdvocateId { get; set; }
        public string FamilyAdvocateSign { get; set; }
        public int FASignType { get; set; }

        public bool Status { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public List<MDTAction> MDTActions { get; set; }

        public string CenterName { get; set; }
        public Agency AgnecyInfo { get; set; }
        public string AgencyId { get; set; }
        public string AgencyName { get; set; }

        public bool IsCompleted { get; set; }
        public bool HaveAttachment { get; set; }
      //  public string Address1 { get; set; }
      // public string Address2 { get; set; }
      //  public
    }

    public class MDTAction
    {

        public long ActionId { get; set; }
        public long MDTId { get; set; }
        public int ActionFor { get; set; }
        public string ActionNotes { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }


    #endregion MDTReport


    #region UFCReport




    public class UFCReport: Pagination,IManagerReport
    {
       
        public string CenterName { get; set; }

        public long HouseholdId { get; set; }

        public List<UFCClientDetails> Children { get; set; }

        public string Parents { get; set; }
       
        public string LastCaseNoteDate { get; set; }

        public long Month { get; set; }

        public string MonthType { get; set; }

        public DateTime? MonthLastDate { get; set; }
        public string EnrollmentStatus { get; set; }

        public string CenterID { get; set; }

        public string ClassroomName { get; set; }
        
        public string ClassroomID { get; set; }
        
        public string StepUpToQualityStars { get; set; }

        public string SearchTerm { get; set; }

        public List<UFCReport> UFCReportList { get; set; }

        public List<SelectListItem> CenterList { get; set; }

        public FingerprintsModel.Enums.UFCReportMode ReportMode { get; set; }


    }

    public class UFCClientDetails
    {
        public string ClientName { get; set; }
        public int EnrollmentStatus { get; set; }

            
    }

    #endregion UFCReport


}