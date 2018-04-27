using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ProgramType
    {
        public Int32 ProgramID { get; set; }
        public string AgencyId { get; set; }
        public string ProgramTypes { get; set; }
        public string Description { get; set; }
        public string PIRReport { get; set; }
        public string Slots { get; set; }
         public string GranteeNumber { get; set; }
         public string Area { get; set; }
         public string StartTime { get; set; }
         public string StopTime { get; set; }
         public int MinAge { get; set; }
         public int MaxAge { get; set; }
         public int Status { get; set; }
         public int ProgYear { get; set; }
         public int ProgEndYear { get; set; }
         public string ReferenceProg { get; set; }
         public int WorkingDays { get; set; }
         public string CreatedBy { get; set; }
         public string CreatedDate { get; set; }
         public List<ReferenceProgInfo> refList = new List<ReferenceProgInfo>();
         public bool HealthReview { get; set; }
         public class ReferenceProgInfo
         {
             public string Id { get; set; }
             public string Name { get; set; }
         }

        public long TotalSeats { get; set; }
        public long OccupiedSeats { get; set; }
        public long AvailableSeats { get; set; }
    }
}
