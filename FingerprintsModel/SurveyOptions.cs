using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class SurveyOptions
    {
        public int QuestionsId { get; set; }
        public string Questions { get; set; }
        public string Answer { get; set; }
        public string Explanation { get; set; }
        public int AnswerId { get; set; }
        public string CreatedDate { get; set; }

    }
}
