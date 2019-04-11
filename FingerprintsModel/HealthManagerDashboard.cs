using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class HealthManagerDashboard:IAccessSectionDashboard
    {

        public IEnumerable<ScreeningMatrix> ScreeningMatrix { get; set; }

        public IEnumerable<NDaysScreeningReview> ScreeningReview { get; set; }

        public bool AccessScreeningMatrix { get; set; }
        public bool AccessScreeningReview { get; set; }

        

    }
}
