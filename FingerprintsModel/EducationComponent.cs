using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{


    public class EducationComponent:Pagination
    {
        public long EducationComponentID { get; set; }
        public string Description { get; set; }
        public string EnteredBy { get; set; }

        public bool Status { get; set; }
        public bool IsReported { get; set; }

        public List<EducationComponent> EducationComponentList{get;set;}
    }
}
