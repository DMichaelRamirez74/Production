using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class MonthlyRecruitmentActivities:Pagination
    {

        public string CenterID { get; set; }
        public int Month { get; set; }

        public bool IsCreate { get; set; }

        public bool IsReview { get; set; }
       
        public List<RecruitmentActivities> RecruitmentActivityList { get; set; }
        public long ActivityTransactionID { get; set; }
        public string OtherDescription { get; set; }
       

    }
}
