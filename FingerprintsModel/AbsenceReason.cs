using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FingerprintsModel
{

   
    public class AbsenceReasonModel
    {
        public List<AbsenceReason> absenceReasonList { get; set; }
    }
  public  class AbsenceReason
    {
        public int ReasonId { get; set; }
        public string Reason { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public bool Status { get; set; }
    }
}
