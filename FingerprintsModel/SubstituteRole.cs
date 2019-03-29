using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class SubstituteRoleModel : Pagination
    {
     
        public List<SubstituteRole> SubstituteRoleList { get; set; }
    }

    public class SubstituteRole:StaffDetails
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string ClassroomName { get; set; }
        public string CenterName { get; set; }
        public long SubstituteID { get; set;}
    }



   


}
