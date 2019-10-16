using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public  class TransitionTypeDetail
    {
        public string TransitionTypeDetailId { get; set; }

        public string FK_TransitionTypeId { get; set; }
        public string FK_ClientId {get;set;}

        public string ClientName { get; set; }

        public string FK_EnrollmentTransactionId { get; set; }
    }
}
