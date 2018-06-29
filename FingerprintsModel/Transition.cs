using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class Transition
    {
        public long ClientId { get; set; }
        public string EClientID { get; set; }
        public long ProgramTypeId { get; set; }
        public string DateOfTransition { get; set; }

        public string TransProgramTypeID { get; set; }

        public int FamilyType { get; set; }
        public string Enc_ProgID { get; set; }
        public string ParentID { get; set; }
        public string Name { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string HouseholdId { get; set; }
        public string Status { get; set; }
        public string StatusText { get; set; }
        public string Reason { get; set; }
        public string ddlreason { get; set; }
        public string ddlreasontext { get; set; }
        public string IsWaiting { get; set; }
        public int? EnrollmentStatus { get; set; }
        public int? InsuranceType { get; set; }
     
        public int? BirthType { get; set; }
        public bool IsEHS { get; set; }
        public bool IsHS { get; set; }
        public string ActiveRecCode { get; set; }

        //public int? EHS_MedicaidCHIP_S { get; set; }
        //public int? EHS_MedicaidCHIP_E { get; set; }
        //public int? EHS_StateFunded_S { get; set; }
        //public int? EHS_StateFunded_E { get; set; }
        //public int? EHS_PrivateIns_S { get; set; }
        //public int? EHS_PrivateIns_E { get; set; }
        //public int? EHS_OtherIns_S { get; set; }
        //public int? EHS_OtherIns_E { get; set; }
        //public string EHS_Description_S { get; set; }
        //public string EHS_Description_E { get; set; }
        //public int? EHS_NoIns_S { get; set; }
        //public int? EHS_NoIns_E { get; set; }

        //public int? HS_MedicaidCHIP_S { get; set; }
        //public int? HS_MedicaidCHIP_E { get; set; }
        //public int? HS_StateFunded_S { get; set; }
        //public int? HS_StateFunded_E { get; set; }
        //public int? HS_PrivateIns_S { get; set; }
        //public int? HS_PrivateIns_E { get; set; }
        //public int? HS_OtherIns_S { get; set; }
        //public int? HS_OtherIns_E { get; set; }
        //public string HS_Description_S { get; set; }
        //public string HS_Description_E { get; set; }
        //public int HS_NoIns_S { get; set; }
        //public int HS_NoIns_E { get; set; }


        public int? MedicaidCHIP_S { get; set; }
        public int? MedicaidCHIP_E { get; set; }
        public int? StateFunded_S { get; set; }
        public int? StateFunded_E { get; set; }
        public int? PrivateIns_S { get; set; }
        public int? PrivateIns_E { get; set; }
        public int? OtherIns_S { get; set; }
        public int? OtherIns_E { get; set; }
        public string Description_S { get; set; }
        public string Description_E { get; set; }
        public int?  NoIns_S { get; set; }
        public int? NoIns_E { get; set; }

        public bool? JobTrainingSchool   { get;set; }

        public int? ParentRole { get; set; }

        public string ParentID2 { get; set; }

        public int? ParentRole2 { get; set; }
        public string ParentName { get; set; }
        public string ParentName2 { get; set; }

        public bool? JobTrainingSchool2 { get; set; }
        public bool? JobTrainingFinished { get; set; }
        public bool? JobTrainingFinished2 { get; set; }


        public int QuestionNumber { get; set; }
        public bool AcceptJobTrainingFinished { get; set; }
        public bool AcceptJobTrainingFinished2 { get; set; }
        public int? ShoolAchievement { get; set; }

        public int? ShoolAchievement2 { get; set; }

        public bool? NONE { get; set; }
        public bool? TANF { get; set; }
        public bool? SSI { get; set; }
        public bool? WIC { get; set; }
        public bool? SNAP { get; set; }
        public bool? HighSchool { get; set; }
        public bool? HighSchoolGraduate { get; set; }
        public bool? AssociateDegree { get; set; }
        public bool? AdvancedDegree { get; set; }

        public bool? HighSchool2 { get; set; }
        public bool? HighSchoolGraduate2 { get; set; }
        public bool? AssociateDegree2 { get; set; }
        public bool? AdvancedDegree2 { get; set; }


        public int? ImmunizationService { get; set; }
        public bool? MedicalHome { get; set; }
        public bool? DentalHome { get; set; }
        public bool? DentalServices { get; set; }

        public bool? NewProgramYearTransition { get; set; }
      
        public bool? MedicalServices { get; set; }
        public int? MedicalServiceTypes { get; set; }
        public int? DentalCare { get; set; }
        public bool PMDental { get; set; }
        public string TrnsInsuranceType { get; set; }
        public int? CenterId { get; set; }
        public string Enc_CenterID { get; set; }
        public string Enc_ClassroomID { get; set; }
        public string ClassStartDate { get; set; }
        public int? ClassRoomId { get; set; }
        public string OtherInsuranceTypeDesc { get; set; }
        public string ChildOtherInsuranceTypeDesc { get; set; }
        public string ChildInsuranceType { get; set; }
        public int? TransitioningType { get; set; }

        public string DateOfWithdrawn { get; set; }

        public string PregnantMotherInsurance { get; set; }
      

        public bool? PregnantMotherEnrollment { get; set; }

        public bool? IsShowJobTrainingQuestion { get; set; }

        public bool IsPreg { get; set; }
        public RosterNew.CaseNote CaseNoteDetails { get; set; }


        public RosterNew.Users Users { get; set; }
        //public List<RosterNew.Attachment> CaseNoteAttachments { get; set; }

        public List<FamilyHousehold.EnrollmentChangeReason> EnrollmentChangeReason { get; set; }


        public string LDAAge { get; set; }

        public string ReferenceProgram { get; set; } 
        public int TypeOfTransition { get; set; }

        public int Returning { get; set; }

        public int PregMomTransitionReady{get;set;}
        
        public string SchoolDistrictDate { get; set; }

        public int EHSHSEnrolled { get; set; }


        public List<SelectListItem> EHSPrograms { get; set; }

        public List<SelectListItem> HSPrograms { get; set; }
    }

    public class TransitionDetails {
        public Transition Transition { get; set; }
        public List<PregMomChilds> PregMomChilds { get; set; }
    }
    public class PregMomChilds {

        public Transition Transition { get; set; }
        public string DateOfTransition { get; set; }

        public string Name { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }


        
        public int InsuranceType { get; set; }
        public bool IsEHS { get; set; }
    }


 
    public class SeatAvailability
    {
        public int SloatAvailable { get; set; }
        public int SeatAvailable { get; set; }
        public int Result { get; set; }
        public string ChildName { get; set; }
    }





    /// <summary>
    /// Represents a file that has uploaded by a client via multipart/form-data. 
    /// </summary>
    //public class HttpPostedFileMultipart : HttpPostedFileBase
    //{
    //    private readonly MemoryStream _fileContents;

    //    public override int ContentLength => (int)_fileContents.Length;
    //    public override string ContentType { get; }
    //    public override string FileName { get; }
    //    public override Stream InputStream => _fileContents;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="HttpPostedFileMultipart"/> class. 
    //    /// </summary>
    //    /// <param name="fileName">The fully qualified name of the file on the client</param>
    //    /// <param name="contentType">The MIME content type of an uploaded file</param>
    //    /// <param name="fileContents">The contents of the uploaded file.</param>
    //    public HttpPostedFileMultipart(string fileName, string contentType, byte[] fileContents)
    //    {
    //        FileName = fileName;
    //        ContentType = contentType;
    //        _fileContents = new MemoryStream(fileContents);
    //    }
    //}
}
