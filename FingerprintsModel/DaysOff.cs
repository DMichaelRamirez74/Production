using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class DaysOff
    {

        public string DaysOffID { get; set; }
        public string ProgramYear { get; set; }
        public Guid AgencyId { get; set; }
        public int RecordType { get; set; }
        public string RecordName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string OffDayComments { get; set; }
        public long? CenterId { get; set; }
        public string CenterName { get; set; }
        public string Enc_CenterId { get; set; }
        public long? ClassRoomId { get; set; }

        public string ClassRoomName { get; set; }
        public string Enc_ClassRoomId { get; set; }
        public string CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool IsActive { get; set; }

        public bool IsStaff { get; set; }
        public Guid RoleId { get; set; }
        public List<ClassRoomModel> ClassRoomIdArray { get; set; }

    }
    public enum EnumDaysOff
    {
        [Display(Name = "Agency Wide Closed")]
        AgencyWideClosed = 1,

        [Display(Name = "Entire Center Closed")]
        EntireCenterClosed = 2,

        [Display(Name = "Classroom Closed")]
        ClassRoomClosed = 3

    }

    public class DaysOffModel
    {
        public List<DaysOff> DaysOffList { get; set; }
        public EnumDaysOff EnumDaysOffType { get; set; }
        public List<Center> CenterList { get; set; }
        public List<ClassRoomModel> ClassRoomList { get; set; }
       
        public List<string> DatesList { get; set; }

        public string CenterListString { get; set; }
        public string ClassRoomListString { get; set; }
        public string OffDaysString { get; set; }
    }

    public class ClassRoomModel
    {
        public string ClassRoomName { get; set; }
        public string ClassRoomId { get; set; }
        public string CenterId { get; set; }
        public string DaysOffId { get; set; }
        public string OffDayComments { get; set; }
        public bool Status { get; set; }
    }

    

   

   

   
}
