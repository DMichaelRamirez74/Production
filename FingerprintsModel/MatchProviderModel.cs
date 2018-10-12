using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class MatchProviderModel
    {
        public int? Id { get; set; }
        public string OrganizationName { get; set; }
        public int? ServiceId { get; set; }
        public string Services { get; set; }
        public string ParentName { get; set; }
        public string Address { get; set; }
        public int? ResourceId { get; set; }
        public string AgencyId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }        
        public long? CommunityId { get; set; }
        public long? ClientId { get; set; }
        public string Notes { get; set; }
        public bool? IsFunction { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ReferralDate { get; set; }
        public long? ReferralClientServiceId { get; set; }
        public List<MatchProviderModel> MPMList { get; set; }
        public List<SelectListItem> OrganizationList { get; set; }
        public string CRColorCode { get; set; }
    }
}
