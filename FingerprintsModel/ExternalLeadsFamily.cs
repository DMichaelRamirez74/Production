using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  class ExternalLeadsFamily
    {

            public Family family { get; set; }
            public List<Child> child { get; set; }
            public int CenterId { get; set; }
            public string PrimaryCenter { get; set; }
            public string SecondaryCenter { get; set; }
            public int ParentId { get; set; }
        }
    public class Family
    {
        public int ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Extension { get; set; }
        public Int64? PrimaryCenterId { get; set; }
        public Int64? SecondaryCenter { get; set; }
        public bool IsHomeBased { get; set; }
        public bool IsPartyDay { get; set; }
        public bool IsFullDay { get; set; }
        public bool? ChildTransport { get; set; }
        public string EmailAddress { get; set; }
        public bool IsSchoolDay { get; set; }
        public string LocationRequest { get; set; }
    }

    public class Child
    {
        public int ChildId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Disability { get; set; }
        public int ParentId { get; set; }
    }

    public class ExternalLeads
    {
        public string YakkrCode { get; set; }
        public string YakkrInfo { get; set; }
        public string TotalCount { get; set; }

    }
}

