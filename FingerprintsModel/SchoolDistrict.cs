using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class SchoolDistrict
    {
       public Int32 SchoolDistrictID { get; set; }
        public string Acronym { get; set; }
        public string Description { get; set; }
        public string TransitionDate { get; set; }
        public bool FormalAgreement { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
