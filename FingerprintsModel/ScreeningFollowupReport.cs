using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class ScreeningFollowupReport:Pagination
    {
        public List<ScreeningFollowup> ScreeningFollowupList { get; set; }

        public List<ScreeningNew> ScreeningList { get; set; }
        public string[] CenterIDs { get; set; }
        public string[] ClassroomIDs { get; set; }
        public string[] ScreeningIDs { get; set; }
        public string SearchTerm { get; set; }


    }
}
