using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class AssessmentResults
    {
        public long AssessmentResultId { get; set; }
        public long MatrixId { get; set; }
        public string MatrixType { get; set; }
        public long MatrixValue { get; set; }
        public long AssessmentGroupId { get; set; }

        public string AssessmentGroupType { get; set; }
        public bool ReferralSuggested { get; set; }

        public bool FPASuggested { get; set; }
        public string Description { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid UserId { get; set; }
        public long CategoryPosition { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
