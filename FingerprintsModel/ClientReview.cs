using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ClientReview
    {
       
    }

    public class Class_Center
    {
        public long CenterId { get; set; }
        public long ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public string CenterName { get; set; }
        public Guid? AgencyId { get; set; }
    }

    public class ClientDetails
    {
        public long ClientId { get; set; }
        public string  ClientName { get; set; }
        public string Status { get; set; }

        public string ActiveProgramYear { get; set; }
    }

    public class ClientReviewStatus
    {
        public long ClientId { get; set; }
        public string AttendanceMonth { get; set; }
        public string Status { get; set; }
        public string ActiveProgramYear { get; set; }
    }

    public class ClientReviewNotes
    {
        public long NotesId { get; set; }

        public long ClientId { get; set; }
        public string OpenNotes { get; set; }
        public string CloseNotes { get; set; }
        public string ReviewedStaffName { get; set; }
        public string ReviewMonth { get; set; }
        public string ActiveProgramYear { get; set; }
        public bool ReviewStatus { get; set; }
        public string ReviewDate { get; set; }
        public string StaffUniqueColor { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public bool UserActiveStatus { get; set; }

        public bool IsEdit { get; set; }

    }
    public class DevelopmentMembers
    {
        public Guid? UserId { get; set; }
        public Guid RoleId { get; set; }
        public string UserColor { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        public Guid? AgencyId { get; set; }
        public bool IsPresent { get; set; }
        public long ClientId { get; set; }
        public bool Status { get; set; }
        public bool IsHost { get; set; }
        public string ReviewMonth { get; set; }
        public bool IsContributor { get; set; }
        public bool IsEdit { get; set; }

    }

    public class Clientprofile
    {
        public string ChildName { get; set; }

        public string ChildId { get;set;} //for encrypted id
        public string ChildAge { get; set; }
        public string DOB { get; set; }
        public string BMI { get; set; }
        public string Language { get; set; }
        public Double Attendance { get; set; }
        public string StartDate { get; set; }
        public int TotalEnrolled { get; set; }
        public string IEP { get; set; }
        public string BehaviorPlan { get; set; }
        public string Allergies { get; set; }
        public string Doctor { get; set; }
        public string Dentist { get; set; }
        public string MissingScreenings { get; set; }
        public string TransferRequested { get; set; }
        public string TransportationProvided { get; set; }
        public bool IsPregnantMother { get; set; }
        public string Trimester { get; set; }
        public string Mother { get; set; }
        public string Parent { get; set; }

        public string PregnantAddress { get; set; }
        public string MotherJobTraining { get; set; }

        public string FatherJobTraining { get; set; }
        public string ParentDOB { get; set; }

        public string Employed { get; set; }
        public string Profilepic { get; set; }
        public string BMIPercentage { get; set; }

        public string JobTraining { get; set; }
        public string TotalCasenote { get; set; }
        public string LastdateofCasenote { get; set; }
        public string Address { get; set; }
        public string CaseNotes { get; set; }
        public string FatherFigure { get; set; }

        public string ParentMaleName { get; set; }
        public string FatherDOB { get; set; }
        public string FatherIsEmployed { get; set; }
        public string ParenFemaleName { get; set; }
        public string MotherDOB { get; set; }
        public string MotherIsEmployed { get; set; }





    }

}
