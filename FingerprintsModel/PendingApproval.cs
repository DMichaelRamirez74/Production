using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class PendingApproval
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string name { get; set; }
        public string EnrollmentCode { get; set; }
        public string MobileNo { get; set; }
        public string IsResend { get; set; }
        public string IsApprove { get; set; }
        public string RoleId { get; set; }
        public string DateEntered { get; set; }
        public string datemodified { get; set; }
        public string rolename { get; set; }

        public bool IsEndOfYear { get; set; }

        public string ActiveProgramYear { get; set; }
    }

    public class Enrolementcode
    {
        public string AuthorisationCode { get; set; }
        public string Description { get; set; }
        public string DateEntered { get; set; }
        public string ValidUpto { get; set; }
        public string status { get; set; }
    }
}
