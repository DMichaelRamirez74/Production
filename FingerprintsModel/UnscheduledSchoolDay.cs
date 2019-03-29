using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{


    public class UnscheduledSchoolDayModal:Pagination
    {
      
        public List<UnscheduledSchoolDay> UnscheduledSchoolDayList { get; set; }
        public List<System.Web.Mvc.SelectListItem> ReasonList { get; set; }
    }

    public class UnscheduledSchoolDay
    {

        public long UnscheduledSchoolDayID { get; set; }
        public string CenterID { get; set; }
        public string[] ClassroomID { get; set; }
        public string CenterName { get; set; }
        public string ClassroomName { get; set; }

        public string ClassDate { get; set; }
        public string UnscheduledSchoolDayReason { get; set; }
        public long UnscheduledSchoolDayReasonID { get; set; }

        
    }

   

    


   
}
