using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  class CFRAnalysis
    {  
        public long PresentCount { get; set; }
        public long ContributorCount { get; set; }
        public long AbsentMembersCount { get; set; }

        public string AttendanceMonth { get; set; }

        public Guid? FSWUserId { get; set; }

        public string FSWUserName { get; set; }

        public long TotalFamilies { get; set; }

        public List<CFRAnalysis> CfrList { get; set; }


    }

    public class ListAnalysis
    {
        public List<CFRAnalysis> CfrList { get; set; }
    }
}
