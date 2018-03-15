using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class FPASteps
    {
        public Guid AgencyId { get; set; }
        public Guid StaffId { get; set; }
        public int StepID { get; set; }
        public Int64 ClientId { get; set; }
        public string EncyrptedClientId { get; set; }//Changes
        public long FPAID { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public string Assignment { get; set; }
        public string StepsCompletionDate { get; set; }
        public string ActualCompletionDate { get; set; }
        public string Name { get; set; }
        public int month { get; set; }
        public int week { get; set; }
        public bool Email { get; set; }
        public string maxCompletionDate { get; set; }
        public int Reminderdays { get; set; }
        public int Status { get; set; }
        public string ParentName { get; set; }
        public string ChildName { get; set; }
        public string FSWName { get; set; }
        public string ParentEmailId { get; set; }
        public List<StepsInfo> StepsData = new List<StepsInfo>();
        public class StepsInfo
        {
            public int StepsID { get; set; }
            public string Description { get; set; }
            public string Assignment { get; set; }
            public string StepsCompletionDate { get; set; }
            public bool Email { get; set; }
        }
    }
}
