using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class YakkrRouting
    {
        public Guid AgencyID { get; set; }
        public Int64 CenterId { get; set; }
        public Int64 ClassRoomId { get; set; }
        public Guid? UserID { get; set; }
        public Int64? ClientId { get; set; }
        public Guid? ToSataffId { get; set; }
        public string RouteCode { get; set; }
        public Int64? HouseHoldId { get; set; }
        public string Email { get; set; }
        public Guid? Imageid { get; set; }


        public string MonitorId { get; set; }
        public string Message { get; set; }
    }
}
