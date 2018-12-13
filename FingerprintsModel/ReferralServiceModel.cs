using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  class ReferralServiceModel
    {
        public long? ClientId { get; set; }

        public string _EncClientId { get; set; }

        public string ClientName { get; set; }
        public long? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int? Step { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ReferralDate { get; set; }

        public string ConvCreatedDate { get; set; }
        public string ConvReferralDate { get; set; }
        public Guid? AgencyId { get; set; }
        public long? ReferralClientServiceId { get; set; }
        public string ParentName { get; set; }
        public List<ReferralServiceModel> referralserviceList { get; set; }
        public string ScreeningReferralYakkr { get; set; }

        public int TotalRecords { get; set; }
    }
}
