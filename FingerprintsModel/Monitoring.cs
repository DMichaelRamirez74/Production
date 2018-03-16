using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class Monitoring
    {
        public Guid? Id { get; set; }
        public Guid AgencyID { get; set; }        
        public Guid ImageId { get; set; }
        public bool PassFailCode { get; set; }
        public Int64? CenterId { get; set; }
        public Int64 ClassRoomId { get; set; }
        public Guid StaffId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string ImageOfDamage { get; set; }
        public string ImageName { get; set; }
        public Guid? ToSataffId { get; set; }
        public Guid? UserID { get; set; }
        public string RouteCode { get; set; }
    }
}
