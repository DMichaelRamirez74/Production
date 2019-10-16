using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class TransitionTypeModel
    {
        public List<TransitionType> TransitionTypeList { get; set; }

      public IEnumerable<TransitionTypeDetail> TransitionTypeDetail { get; set; }
    }
}
