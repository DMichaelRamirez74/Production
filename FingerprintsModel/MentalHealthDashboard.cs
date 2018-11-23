using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  class MentalHealthDashboard
    {
        public string CenterId { get; set; }
        public string Name { get; set; }
        public string CenterName { get; set; }
        public string Mode { get; set; }
        public int Routecode310 { get; set; }
        public int Routecode311 { get; set; }
        public int Routecode312 { get; set; }
        public int Routecode313 { get; set; }
        public string TotalChildren { get; set; }
        public string DisabilityPercentage { get; set; }
        public string Indicated { get; set; }
        public string Pending { get; set; }
        public string Qualified { get; set; }
        public string Released { get; set; }
        public List<MentalHealthClientList> ClientList { get; set; }

    }

    public class MentalHealthClientList
    {

        public string YakkrId { get; set; }
        public string Householid { get; set; }
        public string Eclientid { get; set; }
        public string EHouseholid { get; set; }
        public string Name { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string CenterName { get; set; }
        public string CenterId { get; set; }
        public string ProgramId { get; set; }
        public string ClassroomName { get; set; }
        public string FSW { get; set; }
        public string Picture { get; set; }
        public string classroomid { get; set; }

    }
    public class MentalHealthCaseNote
    {
        public string Mode { get; set; }
        public string ClientId { get; set; }
        public string CenterId { get; set; }
        public string YakkrId { get; set; }
        public string ClientName { get; set; }
        public string TimeSpent { get; set; }
        public RosterNew.CaseNote CaseNote { get; set; }
        public string ReferredBy { get; set; }
        public string CenterName { get; set; }
        public string ClassroomName { get; set; }
        public string RoleName { get; set; }
        public string MHStatus { get; set; }
        public bool ConsultParent { get; set; }
        public bool ConsultStaff { get; set; }
        public bool ProvideAssessment { get; set; }
        public bool ProvideService { get; set; }
        //for case note

    


        public string Date { get; set; }
        public string Title { get; set; }
        public string MHcasenote { get; set; }
        public string Tags { get; set; }
  


        public List<int> ClientIds { get; set; }
        public string CaseProgramId { get; set; }
    }
}
