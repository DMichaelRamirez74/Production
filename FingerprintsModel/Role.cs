using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FingerprintsModel
{
    public class Role
    {

        public Role()
        {
            this.UserList = new List<UserDetails>();
        }
        public List<Role> RoleList { get; set; }
        public List<FingerprintsModel.RosterNew.User> ClientList { get; set; }
        public string AssignedRoles { get; set; }
        public string RoleId { get; set; }
        public bool IsAllow { get; set; }
        public string RoleName { get; set; }
        public bool Defaultrole { get; set; }
        public string ColorCode { get; set; }
        public bool ToView { get; set; }
        public bool ToFollowUp { get; set; }
        public bool ToEnter { get; set; }
        public List<UserDetails> UserList { get; set; }
    }

    public class UserDetails
    {
        public bool ToView { get; set; }
        public bool ToFollowUp { get; set; }
        public bool ToEnter { get; set; }

        public string ColorCode { get; set; }
        public string UserId { get; set; }
        public string StaffName { get; set; }
        public bool IsAllow { get; set; }
        public string RoleId { get; set; }

    }

    public class AcceptanceRole
    {
        public List<Role> RoleList = new List<Role>();
        public int Priority { get; set; }
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public bool isAllowIncome { get; set; }
    }
}
