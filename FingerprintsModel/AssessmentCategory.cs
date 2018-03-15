using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class AssessmentCategory
    {

        public long AssessmentCategoryId { get; set; }

        public string Category { get; set; }

        public Guid? AgencyId { get; set; }

        public Guid UserId { get; set; }
        public long CategoryPosition { get; set; }
    }
}
