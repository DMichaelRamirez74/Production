using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class OrganizatonDetail
    {
        public string Id { get; set; }
        public string OrganizationName { get; set; } 
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ContactName { get; set; }
        public string AccountNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string ContactNumber { get; set; }
        public string Extension { get; set; }
        public string UserId { get; set; }
        public string AgencyId { get; set; }
        public string ProgramTypeId { get; set; }
    }
}
