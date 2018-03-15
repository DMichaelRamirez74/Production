using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public  class QuestionsModel
    {
        public string AssessmentQuestion { get; set; }
        public long AssessmentQuestionId { get; set; }
        public string AssessmentGroupType { get; set; }
        public long AssessmentGroupId { get; set; }
        public long QuestionValue { get; set; }
        public string QuestionText { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; }

        public long CategoryPosition { get; set; }
    }
}
