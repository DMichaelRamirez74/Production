using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  interface IAccessSectionDashboard
    {
         bool AccessScreeningMatrix { get; set; }
         bool AccessScreeningReview { get; set; }
    }
}
