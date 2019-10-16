using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class RosterViewModel : Pagination
    {
        public string CenterId { get; set; }
        public string ClassroomId { get; set; }
        public string SearchTerm { get; set; }
        public string FilterOption { get; set; }
        public List<Roster> RosterList { get; set; }

      
    }
}
