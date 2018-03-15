using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class SurveyAnswers
    {

        public int SurveyAnswerId { get; set; }
        public int ReferralClientId { get; set; }
        public int QuestionId { get; set; }
        public int Answer { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


    }
}
