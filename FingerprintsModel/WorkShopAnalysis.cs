using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class WorkShopAnalysis
    {
        public string WorkShopName { get; set; }
        public long WorkShopId { get; set; }
        public Guid AgencyId { get; set; }
        public long HouseholdEnrolled { get; set; }
    }
}
