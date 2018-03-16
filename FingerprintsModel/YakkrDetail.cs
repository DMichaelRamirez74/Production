using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class YakkrDetail
    {
        public String YakkrCode { get; set; }
        public String Description { get; set; }
        public int Number { get; set; }
    }

    public class YakkrClientDetail
    {
        public string Slots { get; set; }
        public string YakkrID { get; set; }
        public string ClientId { get; set; }
        public string YakkrCode { get; set; }
        public String ClientName { get; set; }
        public String DOB { get; set; }
        public string CenterId { get; set; }
        public string CenterName { get; set; }
        public string FromUser { get; set; }
        public string Date { get; set; }
        public string HouseHoldId { get; set; }
        public string FromUserID { get; set; }
        public string _EncCenterId { get; set; }
    }
}
