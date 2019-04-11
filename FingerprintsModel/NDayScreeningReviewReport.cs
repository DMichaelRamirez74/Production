using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class NDayScreeningReviewReport : Pagination
    {

        public string ScreeningReportPeriodID { get; set; }
        public string ScreeningID { get; set; }
        public string CenterID { get; set; }
        public string ClassroomID { get; set; }
        public string EnrollmentStatus { get; set; }

        public List<NDaysScreeningReview> NDayScreeningReviewList { get; set; }
        public List<ScreeningReportPeriods> ScreeningReportPeriodsList { get; set; }
        public string SearchTerm { get; set; }


    }
}
