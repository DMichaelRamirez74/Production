using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  interface IManagerReport
    {
        [Display(Name ="Center")]
        string CenterName { get; set; }
        string CenterID { get; set; }

        [Display(Name ="Classroom")]
        string ClassroomName { get; set; }
        string ClassroomID { get; set; }

        string StepUpToQualityStars { get; set; }

        
    }
}
