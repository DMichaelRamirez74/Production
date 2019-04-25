using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class FamilyActivityReport:Pagination
    {

        public List<FamilyActivityModel> FamilyActivityList { get; set; }

        
        public string[] CenterIDs { get; set; }
        public string[] ClassroomIDs { get; set; }
        public int[] Months { get; set; }

        public string SearchTerm { get; set; }
    }
}
