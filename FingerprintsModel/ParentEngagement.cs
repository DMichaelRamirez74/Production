using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ParentEngagement
    {
        public Int64? ClientId { get; set; }
        public String Date { get; set; }
        public string Hours { get; set; }
        public string Minutes { get; set; }
        public string ActivityId { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public Int64? HouseHoldId { get; set; }
        public string RouteCode { get; set; }
    }
}
