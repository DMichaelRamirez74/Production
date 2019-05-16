using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
  public class RecruitmentActivities:IManagerReport
    {
        public string CenterID { get; set; }

        public string CenterName { get; set; }


        public string ClassroomID { get; set; }


        public string ClassroomName { get; set; }


        public string StepUpToQualityStars { get; set; }

        public long RecruitmentActivityID { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
        public string EnteredBy { get; set; }

        public bool Status { get; set; }

        public bool IsReported { get; set; }

     



    }
}
