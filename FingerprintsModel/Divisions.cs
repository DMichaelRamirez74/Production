using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class Divisions
    {

        private static readonly Divisions instance=new Divisions();

        public static Divisions Instance
        {
            get
            {
                return instance;
            }

        }

        public Divisions()
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
        public long DivisionIndexID { get; set; }
        public long DivisionID { get; set; }
        public Guid AgencyID { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public string DateEnterd { get; set; }
        public string DateModified { get; set; }
        public bool Status { get; set; }

        public bool IsDivisionReferred { get; set; }

    }
}
