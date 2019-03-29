
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ScreeningMatrixReport:Pagination
    {
      //  public IEnumerable<ScreeningMatrix> ScreeningMatrix { get; set; }

        public List<ScreeningNew> ScreeningList { get; set; }
        public List<ScreeningMatrix> ScreeningMatrix { get; set; }
        public string CenterID { get; set; }

        public string ClassroomID { get; set; }
        public string SearchTerm { get; set; }


        public string ScreeningID { get; set; }
    }
}
