using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    /// <summary>
    /// Daily Observation alias Daily Health Check
    /// </summary>
    public class DailyObservation:Pagination
    {
        public string ObservationID { get; set; }

        public Guid? AgencyID { get; set; }
        public string AgencyName { get; set; }
        public string Description { get; set; }
        public List<DailyObservation> DailyObservationList { get; set; }
        public string SearchTerm { get; set; }
        public string EnteredBy { get; set; }
        public bool Status { get; set; }
        public bool IsReported { get; set; }
        public bool Editable { get; set; }

    }
}
