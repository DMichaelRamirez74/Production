using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class NDaysScreeningReview : ScreeningNew,IManagerReport
    {
        [Display(Name ="Completed")]
        [DisplayName("Completed")]
        public long Completed { get; set; }

        [Display(Name ="Completed but Late")]
        public long CompletedButLate { get; set; }

        [Display(Name ="Not Expired")]
        public long NotExpired { get; set; }

        [Display(Name ="Not Completed and Late")]
        public long NotCompletedandLate { get; set; }

        [Display(Name ="Center")]
        public string CenterName { get; set; }
        public string CenterID { get; set; }

        [Display(Name ="Classroom")]
        public string ClassroomName { get; set; }
        public string ClassroomID { get; set; }
        public string StepUpToQualityStars { get; set; }
        public string EnrollmentStatus { get; set; }

    }


   
}
