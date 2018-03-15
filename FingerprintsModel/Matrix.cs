using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  class Matrix
    {

        public long MatrixId { get; set; }

        public long MatrixValue { get; set; }

        public string MatrixType { get; set; }
        public Guid? AgencyId { get; set; }

        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        public bool Status { get; set; }
    }
}
