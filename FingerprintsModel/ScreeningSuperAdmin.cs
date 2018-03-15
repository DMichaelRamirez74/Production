using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class ScreeningSuperAdmin
    {
       public string date { get; set; }
       public int Role { get; set; }
       public int Status { get; set; }
       public string Description { get; set; }
       public string UsersName { get; set; }
       public List<Users> UsersList = new List<Users>();
       public class Users
       {
           public string Name { get; set; }
           public string UserId { get; set; }
        
       }
    }
}
