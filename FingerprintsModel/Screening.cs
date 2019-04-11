using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FingerprintsModel
{


    public class ScreeningNew
    {

       
        public int ScreeningID { get; set; }

        [Display(Name ="Screening Type")]
        public string ScreeningName { get; set; }

     

        public HttpPostedFileBase ApprovedFile { get; set; }
        public string ApprovedFileName { get; set; }
        public string ApprovedFileExtension { get; set; }
        public int ParentAppID { get; set; }
        public int Approved { get; set; }
        public string ApprovedImageJson { get; set; }
       
        public string ApprovedImageUrl { get; set; }
         
        public byte[] ApprovedImageByte { get; set; }

        public bool NoDocument { get; set; }
        public List<Questions> Questionlist { get; set; }

        public int LastScreeningCompleted { get; set; }

        public bool IsExpired { get; set; }
        public bool IsExpiring { get; set; }
        public int ScreeningsPerYear { get; set; }

        public int ScreeningReportPeriodID{get;set;}

        public bool Report { get; set; }
    }

    public class ScreeningPeriods 
    {
        public double ScreeningPeriod { get; set; }
        public int ScreeningPeriodType { get; set; }
        public string Description { get; set; }
        public int ScreeningPeriodFor { get; set; }

        public int ScreeningFocusType { get; set; }

        public double ScreeningAge { get; set; }

        public long ScreeningPeriodIndex { get; set; }

        public int CustomScreeningPeriod { get; set; }

        public int ScreeningExpirationType { get; set; }
    }



    public class Screening
    {

        public Screening _Screening { get; set; }

        //Changes do not update this file
        public int Consolidated { get; set; }
        public string Parentname { get; set; }
        public DataTable CustomScreening { get; set; }
        //End

        public int CustomScreeningPeriod { get; set; }
        public string WellBabyExamMonth { get; set; }

        public string ScreeningPeriodIndex { get; set; }
        public string ClientName {get; set;}
          public int ParentAppID { get; set; }
          public int TypeScreening { get; set; }
          public int AgencyID { get; set; }
          public int ClientID { get; set; }

          public int Screeningid { get; set; }

        public List<SelectListItem> ParentList { get; set; }

        public List<ScreeningPeriods> ScreeningPeriodsList { get; set; }

        public string ScreeningParentID { get; set; }
        public string ParentSignature { get; set; }
        public List<ScreeningNew> ScreeningList
        {
            get;set;
        }

        public List<ScreeningQ> ScreeningQuestions { get; set; }

          public string F001physicalDate { get; set; }
          public string F002physicalResults { get; set; }
          public string F003physicallFOReason { get; set; }
          public string F004medFollowup { get; set; } 
          public string F005MedFOComments { get; set; }
          public string F006bpResults { get; set; }
          public string F007hgDate { get; set; }
          public string F008hgStatus { get; set; }
          public string F009hgResults { get; set; }
          public string F010hgReferralDate { get; set; }
          public string F011hgComments { get; set; }
          public string F012hgDate2 { get; set; }
          public string F013hgResults2 { get; set; }
          public string F014hgFOStatus { get; set; }
          public string F015leadDate { get; set; }
          public string F016leadResults { get; set; }
          public string F017leadReferDate { get; set; }
          public string F018leadComments { get; set; }
          public string F019leadDate2 { get; set; }
          public string F020leadResults2 { get; set; }
          public string F021leadFOStatus { get; set; }
          public string v022date { get; set; }
          public string v023results { get; set; }
          public string v024comments { get; set; }
          public string v025dateR1 { get; set; }
          public string v026resultsR1 { get; set; }
          public string v027commentsR1 { get; set; }
          public string v028dateR2 { get; set; }
          public string v029resultsR2 { get; set; }
          public string v030commentsR2 { get; set; }
          public string v031ReferralDate { get; set; }
          public string v032Treatment { get; set; }
          public string v033TreatmentComments { get; set; }
          public string v034Completedate { get; set; }
          public string v035ExamStatus { get; set; }
          public string h036Date { get; set; }
          public string h037Results { get; set; }
          public string h038Comments { get; set; }
          public string h039DateR1 { get; set; }
          public string h040ResultsR1 { get; set; }
          public string h041CommentsR1 { get; set; }
          public string h042DateR2 { get; set; }
          public string h043ResultsR2 { get; set; }
          public string h044CommentsR2 { get; set; }
          public string h045ReferralDate { get; set; }
          public string h046Treatment { get; set; }
          public string h047TreatmentComments { get; set; }
          public string h048CompleteDate { get; set; }
          public string h049ExamStatus { get; set; }
          public string d050evDate { get; set; }
          public string d051NameDEV { get; set; }
          public string d052evResults { get; set; }
          public string d053evResultsDetails { get; set; }
          public string d054evDate2 { get; set; }
          public string d055evResults2 { get; set; }
          public string d056evReferral { get; set; }
          public string d057evFOStatus { get; set; }
          public string d058evComments { get; set; }
          public string d059evTool { get; set; }
          public string E060denDate { get; set; }
          public string E061denResults { get; set; }
          public string E062denPrevent { get; set; }
          public string E063denReferralDate { get; set; }
          public string E064denTreatment { get; set; }
          public string E065denTreatmentComments { get; set; }
          public string E066denTreatmentReceive { get; set; }
          public string s067Date { get; set; }
          public string s068NameTCR { get; set; }
          public string s069Details { get; set; }
          public string s070Results { get; set; }
          public string s071RescreenTCR { get; set; }
          public string s072RescreenTCRDate { get; set; }
          public string s073RescreenTCRResults { get; set; }
          public string s074ReferralDC { get; set; }
          public string s075ReferDate { get; set; }
          public string s076DCDate { get; set; }
          public string s077NameDC { get; set; }
          public string s078DetailDC { get; set; }
          public string s079DCDate2 { get; set; }
          public string s080DetailDC2 { get; set; }
          public string s081FOStatus { get; set; }
          public string AddPhysical { get; set; }
          public string WellBabyExam { get; set; }
          public string AddVision { get; set; }
          public string AddHearing { get; set; }
          public string AddDevelop { get; set; }
          public string AddDental { get; set; }
          public string AddSpeech { get; set; }
          public HttpPostedFileBase Physical { get; set; }
          public HttpPostedFileBase Vision { get; set; }
          public HttpPostedFileBase Hearing { get; set; }
          public HttpPostedFileBase Develop { get; set; }
          public HttpPostedFileBase Dental { get; set; }
          public HttpPostedFileBase Speech { get; set; }

         public HttpPostedFileBase WellBabyDoc { get; set; }

          public HttpPostedFileBase ScreeningAccept { get; set; }

          public string PhysicalFileName { get; set; }
          public string PhysicalFileExtension { get; set; }
          public string PhysicalImagejson { get; set; }
          public byte[] PhysicalImageByte { get; set; }

          public string VisionFileName { get; set; }
          public string VisionFileExtension { get; set; }
          public string VisionImagejson { get; set; }
          public byte[] VisionImageByte { get; set; }


          public string HearingFileName { get; set; }
          public string HearingFileExtension { get; set; }
          public string HearingImagejson { get; set; }
          public byte[] HearingImageByte { get; set; }

          public string DevelopFileName { get; set; }
          public string DevelopFileExtension { get; set; }
          public string DevelopImagejson { get; set; }
          public byte[] DevelopImageByte { get; set; }

          public string DentalFileName { get; set; }
          public string DentalFileExtension { get; set; }
          public string DentalImagejson { get; set; }
          public byte[] DentalImageByte { get; set; }

          public string SpeechFileName { get; set; }
          public string SpeechFileExtension { get; set; }
          public string SpeechImagejson { get; set; }
          public byte[] SpeechImageByte { get; set; }

          public string ScreeningAcceptFileName { get; set; }
          public string ScreeningAcceptFileExtension { get; set; }
          public string ScreeningAcceptImagejson { get; set; }
          public byte[] ScreeningAcceptImageByte { get; set; }

        public string ScreeningAcceptImageUrl { get; set; }

        public string Householdid { get; set; }
        public string Childid { get; set; }

        public  ChildrenInfo ChildInfo { get; set; }

        public ScreeningAccess ScreeningAccessInfo { get; set; }

    }


    ///// <summary>
    ///// Enumaration for determining expiration period
    ///// </summary>
    //public enum ScreeningExpirationEnum
    //{
    //    [Description(null)]
    //    Current=0,
    //    [Description("Expiring")]
    //    Expiring =1,
    //    [Description("Expired")]
    //    Expired =2
    //}

   


    
}
