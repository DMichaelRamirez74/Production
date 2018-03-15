using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public  class BillingRates
    {
        public string Id { get; set; }
        public string AgencyId { get; set; }
        public string UserId { get; set; }
        public string ProgramTypeId { get; set; }
        public string FixedAmount { get; set; }
        public string EarlyRate { get; set; }
        public string NormalRate { get; set; }
        public string LateRate { get; set; }
        public string BillDirectToFamily { get; set; }
        public bool AllowOverrideRate { get; set; }
        public string EarlyorLateTimes { get; set; }
    }
}
