using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class BillingReview
    {
        public string ClientId { get; set; }
        public string EmailId { get; set; }
        public string ClientName { get; set; }
        public string ProgramTypeId { get; set; }
        public string CenterId { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public bool Email { get; set; }
        public bool Print { get; set; }
    }

    public class FamilyRate
    {
        public string ClientId { get; set; }
        public decimal OverrideEarlyRate { get; set; }
        public decimal OverrideNormalRate { get; set; }
        public decimal OverrideLateRate { get; set; }
        public decimal FixedAmount { get; set; }
        public double EarlyorLateTimes { get; set; }
        public decimal NeverLessThan { get; set; }
        public decimal NeverMoreThan { get; set; }
    }

    public class ENLHours
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ProgramTypeId { get; set; }
        public string CenterId { get; set; }
        public string Month { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan EarlyRate { get; set; }
        public TimeSpan NormalRate { get; set; }
        public TimeSpan LateRate { get; set; }
        public TimeSpan TotalHours { get; set; }
        public decimal TotalBiling { get; set; }
    }
}
