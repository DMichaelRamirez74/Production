using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class Volunteer
    {
        public Int64? ClientId { get; set; }
        public String Status { get; set; }
        public string days { get; set; }
        public string Hours { get; set; }
        public string UserId { get; set; }
        public Int64? HouseHoldId { get; set; }
        public string RouteCode { get; set; }
    }

    public class Days
    {
        public string Day { get; set; }
    }
}
