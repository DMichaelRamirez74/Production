using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class StatusChange
    {
        public Int64? ClientId { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public Int64? HouseHoldId { get; set; }

        public string RouteCode { get; set; }
        public string Time { get; set; }
        public string NewReason { get; set; }

        public bool IsLateArrival { get; set; }
    }
}
