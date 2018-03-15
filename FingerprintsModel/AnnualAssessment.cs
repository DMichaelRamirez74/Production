using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  class AnnualAssessment
    {

        public long AnnualAssessmentId { get; set; }
        public long AnnualAssessmentType { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid UserId { get; set; }

        public string UserName { get; set; }
        public string AgencyName { get; set; }

        public string Assessment1From { get; set; }
        public string Assessment1To { get; set; }
        public string Assessment2From { get; set; }
        public string Assessment2To { get; set; }
        public string Assessment3From { get; set; }
        public string Assessment3To { get; set; }

    }
}
