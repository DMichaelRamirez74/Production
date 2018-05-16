using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class AcceptanceProcessStatus
    {

      public string RoleName { get; set; }
        public string IsAccepted { get; set; }
        public string Reason { get; set; }
        public string IsReviewAgain { get; set; }

        public string RoleId { get; set; }
        public string IsPending { get; set; }

        public string PriorityLevel { get; set; }
        public string AcceptanceStartDate { get; set; }
        public List<ApplicationStatusnotes> AppNotes = new List<ApplicationStatusnotes>();

      
    }
    public class ApplicationStatusnotes
    {
        public string Name { get; set; }
        public string notes { get; set; }
        public string CreatedOn { get; set; }
        public string AppStatus { get; set; }
        public int ReviewAgain { get; set; }
    }
}
