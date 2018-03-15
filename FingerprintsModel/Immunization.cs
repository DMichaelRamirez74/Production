using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class Immunization
    {
       public List<Immunization> getList { get; set; }

       public int ImmunizationId { get; set; }
       public string ImmunizationType { get; set; }
       public string AgencyId { get; set; }
       public string Dose1To { get; set; }
       public string Dose1From { get; set; }
       public string Dose1WaitPeriod { get; set; }
       public string Dose2To { get; set; }
       public string Dose2From { get; set; }
       public string Dose2WaitPeriod { get; set; }
       public string Dose3To { get; set; }
       public string Dose3From { get; set; }
       public string Dose3WaitPeriod { get; set; }
       public string Dose4To { get; set; }
       public string Dose4From { get; set; }
       public string Dose4WaitPeriod { get; set; }
       public string Dose5To { get; set; }
       public string Dose5From { get; set; }
       public string Dose5WaitPeriod { get; set; }
       public bool HighRiskGroup { get; set; }
       public bool Recurring { get; set; }
       public int NoofDoses { get; set; }
       public string Makeup1Wait { get; set; }
       public string Makeup2Wait { get; set; }
       public string Makeup3Wait { get; set; }
       public string Makeup4Wait { get; set; }
       public string Makeup5Wait { get; set; }
       public int Status { get; set; }
     //  public int Dose1To { get; set; }
       public string CreatedBy { get; set; }
       public string DateEntered { get; set; }
      

    }
}
