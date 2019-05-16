using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class CenterMonthlyReport:Pagination
    {
        public List<CenterMonthlyReportModel> CenterMonthlyReportList { get; set; }

        public string[] CenterIDs { get; set; }
        public string[] ClassroomIDs { get; set; }
        public int[] Months { get; set; }

       
    }
}
