using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ScreeningFollowup : ChildrenInfo, IManagerReport
    {
        public ScreeningFollowup()
        {
            //this.ScreeningReportPeriods =Fingerprints.Common.FactoryInstance.Instance.CreateInstance<ScreeningReportPeriods>();
            //this.ScreeningQuestion = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<ScreeningQ>();
        }
        public string CenterID{get;set;}

        public new string CenterName { get; set; }

        public string ClassroomID{get;set;}

        public string ClassroomName{get;set;}

        public string StepUpToQualityStars{get;set;}

        public string ScreeningDate { get; set; }
        public ScreeningPeriods ScreeningPeriods { get; set; }

        public ScreeningQ ScreeningQuestion { get; set; }

    }


   
}
