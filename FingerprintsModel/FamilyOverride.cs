using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class FamilyOverride
    {
        public string Id { get; set; }
        public string AgencyId { get; set; }
        public string UserId { get; set; }
        public string FamilyId { get; set; }
        public string ChildId { get; set; }
        public string ProgramTypeId { get; set; }
        public string FixedAmount { get; set; }
        public string NeverLessThan { get; set; }
        public string NeverMoreThan { get; set; }
        public string OverrideEarlyRate { get; set; }
        public string OverrideNormalRate { get; set; }
        public string OverrideLateRate { get; set; }
        public bool Email { get; set; }
        public bool Print { get; set; }
        public string BillDirectToFamily { get; set; }

    }
}
