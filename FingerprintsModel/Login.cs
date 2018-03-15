using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FingerprintsModel
{
    public class Login
    {
        public Guid UserId { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsPrimary { get; set; }
        public bool MenuEnable { get; set; }
        public Guid? AgencyId { get; set; }
        public string RoleName { get; set; }
        public string Emailid { get; set; }
        public Guid roleId { get; set; }
        public string IPAddress { get; set; }
        public string AccessStart { get; set; }
        public string AccessStop { get; set; }
        
    }
}
