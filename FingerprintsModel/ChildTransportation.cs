using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ChildTransportation
    {
        public string Id { get; set; }
        public string AgencyId { get; set; }
        public string ClientId { get; set; }
        public string PickupStatus { get; set; }
        public string PickupAddress { get; set; }
        public string PickupZipcode { get; set; }
        public string PickupCity { get; set; }
        public string PickupState { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string DropStatus { get; set; }
        public string DropAddress { get; set; }
        public string DropZipcode { get; set; }
        public string DropCity { get; set; }
        public string DropState { get; set; }
        public double DropLatitude { get; set; }
        public double DropLongitude { get; set; }
    }
}
