using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class TeacherVisit:Scheduler
    {
        public string YakkrCode { get; set; }
        public int VisitCount { get; set; }
        public string YakkrID { get; set; }
        public bool Editable { get; set; }
    }
}
