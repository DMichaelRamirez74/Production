using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FingerprintsModel
{
    public class SupperAdmin
    {
        public Guid superadminId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsPrimary { get; set; }
        public Guid AgencyId { get; set; }
        public string RoleName { get; set; }  
        [Required]
        [EmailAddress]
        public string Emailid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressDetails { get; set; }
        public string MobileNumber { get; set; }
        public char status { get; set; }
        public string EncryptedId { get; set; }
        public string createdDate { get; set; }
        public string TimeZoneID { get; set; }
        public List<TimeZoneinfo> TimeZonelist = new List<TimeZoneinfo>();
    }

    public class ScreeningQ
    {
        public string ScreeningId { get; set; }
        public string ScreeningName { get; set; }
        public string AgencyId { get; set; }
        public string AgencyName { get; set; }
        public string ScreeningFor { get; set; }
        public string CreatedOn { get; set; }

        public string ScreeningOrder { get; set; }
        public string status { get; set; }
        public List<Questions> Questionlist { get; set; }

        public string ProgramTypes { get; set; }
    }
    public class Options
    {
        public int OptionId { get; set; }
        public string Option { get; set; }

        public bool IsChecked { get; set; }


    }
    public class Questions
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public bool Required { get; set; }
        public List<Options> OptionList { get; set; }

        public double QuestionOrder { get; set; }

        public string OptionValue { get; set; }

    }


}
