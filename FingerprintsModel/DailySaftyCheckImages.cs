using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class DailySaftyCheckImages
    {
        public Guid Id { get; set; }
        public string ImageDescription { get; set; }
        public string ImagePath { get; set; }
        public bool? PassFailCode { get; set; }
        public Guid? ToStaffId { get; set; }
        public string RouteCode { get; set; }
        public string ImageOfDamage { get; set; }
        public string WorkOrderDescription { get; set; }
    }
}
