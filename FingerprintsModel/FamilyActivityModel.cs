using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class FamilyActivityModel : IManagerReport
    {
        public string CenterID { get; set; }
       
        [Display(Name="Center")]
        public string CenterName { get; set; }
      

        public string ClassroomID { get; set; }
       

        [Display(Name ="Classroom")]
        public string ClassroomName { get; set; }
       

        public string StepUpToQualityStars { get; set; }

        [Display(Name ="FPA")]
        public long FPA { get; set; }

        [Display(Name ="Referrals")]
        public long Referral { get; set; }

        [Display(Name ="Internal Referrals")]
        public long InternalReferral { get; set; }

        [Display(Name ="Quality of Referrals")]
        public long QualityOfReferral { get; set; }

        [Display(Name = "Month")]
        public string Month { get; set; }

        public DateTime? MonthLastDate { get; set; }
       
    }
}
