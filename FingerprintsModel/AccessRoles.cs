using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class AccessRoles
    {
      public int TitleId { get; set; }

        public string ColorCode { get; set; }
        public string TitleDescription { get; set; }

        public List<AccessRoles> TitleList { get; set; }
        public List<Role> RoleList { get; set; }

        public List<ScreeningNew> ScreeningList { get; set; }

        public int SelectedScreeningID { get; set; }
    }


    public class AccessStaffs
    {

        public Guid UserId { get; set; }
        public string StaffName { get; set; }
        public bool IsShowPIR { get; set; }
        public Guid RoleId { get; set; }
        public bool IsShowSectionB { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public bool ISDevelopmentTeam { get; set; }
    }
}
