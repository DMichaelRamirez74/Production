using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class Inkind:Pagination
    {
        public string Id { get; set; }
        public string ProgramYear { get; set; }
        public string Hours { get; set; }
        public string Dollers { get; set; }

        public List<InkindDonors> InkindDonorsList { get; set; }

        public List<InkindActivity> InkindActivityList { get; set; }

        public List<InKindTransactions> InkindTransactionsList { get; set; }

        public InKindTransactions InKindTransactions { get; set; }
        public InKindDonarsContact InKindDonarsContact { get; set; }



        public int HomeActivityCount { get; set; }

      

        public long InkindPeriodID { get; set; }

       public List<InkindPeriods> InkindPeriodsList { get; set; }

        
    }
    public class InkindDonors
    {
        public string Name { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public List<FamilyHousehold.phone> PhoneNoList { get; set; }
        public string EmailAddress { get; set; }

        //    public InkindActivity InkindActivity { get; set; }

        public string InkindDonorId { get; set; }

        public int PastPresentParent { get; set; }

        public int AllowHomeBasedActivity { get; set; }
        public int EmergencyContact { get; set; }
    }

    public class InkindActivity
    {
        public InkindActivity()
        {
            this.StaffDetails = new StaffDetails();
            this.SubActivityList = new List<SubActivities>();
        }
        public string DateOfActivity { get; set; }
        public string ActivityType { get; set; }
        public string ActivityDescription { get; set; }
        public string ActivityHours { get; set; }
        public string ActivityMinutes { get; set; }
        public string ActivityAmount { get; set; }
        public string ActivityAmountRate { get; set; }
        public long CenterId { get; set; }
        public long ClassroomId { get; set; }
        public string ActivityAmountType { get; set; }

        public string SignatureDonor { get; set; }
        public string SignatureStaff { get; set; }

        public bool IsSignatureRequired { get; set; }
        public bool IsActive { get; set; }

        public int IsAllowDocumentUpload { get; set; }

        public string ActivityCode { get; set; }
        public bool Volunteer { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public StaffDetails StaffDetails { get; set; }

        public List<SubActivities> SubActivityList { get; set; }
    }


    public class SubActivities
    {
        public long SubActivityId { get; set; }
        public Guid AgencyId { get; set; }
        public int ActivityCode { get; set; }
        public string ActivityDescription { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }


    public class InKindTransactions:InkindPeriods
    {
        public string ClientID { get; set; }
        public string ParentID { get; set; }
        public Guid AgencyId { get; set; }
        public string ActivityDate { get; set; }
        public int CenterID { get; set; }
        public int ClassroomID { get; set; }
        public int FromNo { get; set; }
        public int ActivityID { get; set; }
        public int Hours { get; set; }
        public double Minutes { get; set; }
        public string ActivityNotes { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompany { get; set; }

        public bool IsEmergencyContact { get; set; }
        public string DonorSignature { get; set; }
      //  public string StaffSignature { get; set; }

        //public string SignatureCode
        //{
        //    get;set;
        //}

        public StaffSignature StaffSignature { get; set; }
        public decimal InKindAmount { get; set; }
        public decimal MilesDriven { get; set; }

        public bool ParentType{get;set;}
        public int InkindTransactionID { get; set; }
        public string Name { get; set; }
        public string Enc_CenterID { get; set; }

        public string Enc_ClassroomID { get; set; }
        public List<InkindAttachments> InkindAttachmentsList { get; set; }
    }


    public class InkindModel
    {
        public InKindTransactions transactions { get; set; }
        public InKindDonarsContact corporate { get; set; }
    }

    public class InKindDonarsContact
    {
        public long InKindDonarId { get; set; }
        public string CorporateName { get; set; }
        public string ContactName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        public string ApartmentNo { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string PhoneType { get; set; }
        public string County { get; set; }
        public string Gender { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public bool Status { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }

        public bool IsInsert { get; set; }

        public bool IsCompany { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool NoEmail { get; set; }
    }



    public class InkindAttachments
    {
        public long InkindAttachmentID { get; set; }
        public HttpPostedFileBase InkindAttachmentFile { get; set; }
        public string InkindAttachmentJson { get; set; }
        public string InkindAttachmentFileName { get; set; }
        public string InkindAttachmentFileExtension { get; set; }
        public string InkindAttachmentFileUrl { get; set; }
        public byte[] InkindAttachmentFileByte { get; set; }
        public long InkindTransactionID { get; set; }
        public bool InkindAttachmentStatus { get; set; }
    }

    public class InkindReportModel:Pagination,IInkindPeriod
    {

        public List<InkindReport> InkindReportList { get; set; }

        public InkindReportFilterEnum FilterTypeEnum { get; set; }
        public string SubFilterOption { get; set; }

        public string Centers { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string DateEntered { get; set; }

        public string SortOrder { get; set; }
        public string SortColumn { get; set; }

        public string TotalHours { get; set; }
        public string TotalMiles{get;set;}
        public string TotalAmount { get; set; }
        public string SearchTerm { get; set; }

        public string SearchTermType { get; set; }


        public List<InkindPeriods> InkindPeriodList { get; set; }
    }

    public class InkindReport : InKindTransactions
    {
       public string CenterName { get; set; }
        public string ActivityDescription { get; set; }

        public string ActivityType { get; set; }

        public string StaffEntered { get; set; }

      

    }

    public class InkindPeriods
    {
        public long InkindPeriodID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ActiveProgramYear { get; set; }
        public bool IsClosed { get; set; }
        public bool IsStatus { get; set; }

      
    }


    public interface IInkind
    {
        InkindActivity InkindActivity { get; set; }
        InKindTransactions InkindTransactions { get; set; }

    }

    public interface IInkindPeriod
    {
         List<InkindPeriods> InkindPeriodList { get; set; }
    }

   

    public enum InkindReportFilterEnum
    {
       

        [Description("Center")]
        Center = 1,

        [Description("Contributer")]
        Contributor = 2,

        [Description("Contribution Activity")]
        ContributionActivity =3,

        [Description("Date Entered")]
        DateEntered = 4,

        [Description("Entered By")]
        EnteredBy = 5


    }

    public enum InkindAmountTypeEnum
    {
        Miles=1,
        Hours=2,
        Fixed=3
    }

    public enum InkindActivityTypeEnum
    {
        [Description("Center")]
        Center=1,

        [Description("Home Based")]
        HomeBased =2
    }


}
