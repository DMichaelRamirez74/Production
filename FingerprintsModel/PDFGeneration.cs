using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class PDFGeneration
    {

        public int? Id { get; set; }

        public string DOB { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? clientId { get; set; }


        public string MonFrom { get; set; }
        public string MonTo { get; set; }
        public string TueFrom { get; set; }
        public string TueTo { get; set; }
        public string WedFrom { get; set; }
        public string WedTo { get; set; }
        public string ThursFrom { get; set; }
        public string ThursTo { get; set; }
        public string FriFrom { get; set; }
        public string FriTo { get; set; }
        public string SatFrom { get; set; }
        public string SatTo { get; set; }
        public string SunFrom { get; set; }
        public string SunTo { get; set; }
        public string CompanyName { get; set; }
        public int? ServiceId { get; set; }
        public string Services { get; set; }
        public string ParentName { get; set; }
        public string Address { get; set; }
        public string Phoneno { get; set; }

        public string Email { get; set; }

        public string CommunityID { get; set; }
        public int? ResourceId { get; set; }
        public string AgencyId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public List<MatchProviderModel> MPMList { get; set; }
        public List<BusinessHours> BusinessHoursList { get; set; }
    }
}
