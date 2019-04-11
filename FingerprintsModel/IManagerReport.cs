using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  interface IManagerReport
    {
        string CenterName { get; set; }
        string CenterID { get; set; }

        string ClassroomName { get; set; }
        string ClassroomID { get; set; }

        string StepUpToQualityStars { get; set; }

        
    }
}
