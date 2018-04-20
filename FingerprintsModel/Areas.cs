using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class Areas
    {


        public Areas()
        {
            this._StaffDetails = StaffDetails.GetInstance();
        }

        private StaffDetails _StaffDetails;
        public StaffDetails StaffDetails
        {
            get { return _StaffDetails; }
            set
            {
                _StaffDetails = value;
            }
        }
        public long AreaIndexID { get; set; }
        public long AreaID { get; set; }
        public Guid AgencyID { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public string DateEnterd { get; set; }
        public string DateModified { get; set; }
        public bool Status { get; set; }
        public bool IsAreaReferred { get; set; }

    }
}
