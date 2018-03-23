using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class PIRAccessStaffs
    {

        public PIRAccessStaffs()
        {
            this.PIRStaffsList = new List<PIRStaffs>();
            this.StaffDetails = StaffDetails.GetInstance();
        }
        public List<PIRStaffs> PIRStaffsList { get; set; }
        public StaffDetails StaffDetails { get; set; }

        public int TotalRecord { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }
        public string SearchText { get; set; }
    
        public int RequestedPage { get; set; }
    }

    public class PIRStaffs
    {

        public Guid UserId { get; set; }
        public string StaffName { get; set; }
        public bool IsShowPIR { get; set; }
        public Guid RoleId { get; set; }
        public bool IsShowSectionB { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
    }
}
