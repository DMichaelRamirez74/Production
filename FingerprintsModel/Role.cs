using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FingerprintsModel
{
    public class Role
    {
        public string RoleId { get; set; }        
        public string RoleName { get; set; }
        public bool Defaultrole { get; set; }
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
