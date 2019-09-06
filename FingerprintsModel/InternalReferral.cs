using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class InternalReferral
    {

        public string RoleId { get; set; }
        //public string Note { get; set; }
        public string YakkrCode { get; set; }
        //public string Date { get; set; }
        //public string Title { get; set; }
        //public string CaseNote { get; set; }
        //public string Tags { get; set; }
        public string ClientId { get; set; }
        public string CenterId { get; set; }
        public string HouseholdId { get; set; }
        public string ClassroomId { get; set; }
        //public List<int> ClientIds { get; set; }
        //public List<int> StaffIds { get; set; }
        //public bool IsSecurity { get; set; }
        public string ProgramId { get; set; }

        public string CaseNoteId { get; set; }
    }
}
