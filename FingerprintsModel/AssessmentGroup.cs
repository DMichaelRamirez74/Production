using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  class AssessmentGroup
    {
        public long AssessmentGroupId { get; set; }
        public long AssessmentCategoryId { get; set; }
        public string AssessmentGroupType { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid UserId { get; set; }

        public string Category { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public long CategoryPosition { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
