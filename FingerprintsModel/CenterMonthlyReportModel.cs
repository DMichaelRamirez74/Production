using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
   public class CenterMonthlyReportModel : IManagerReport
    {
        public string CenterID { get; set; }

        public string CenterName{ get; set; }
        public string ClassroomID{ get; set; }

        public string ClassroomName{ get; set; }

        public string StepUpToQualityStars{ get; set; }

        public List<SelectListItem> CenterCordinators { get; set; }
        public List<SelectListItem> FamilyServiceWorkers { get; set; }

        public List<SelectListItem> FPA { get; set; }

        public List<SelectListItem> Referral { get; set; }
      
        public string Month { get; set; }
       public List<SelectListItem> FSWHomeVisit { get; set; }
       
       public List<RecruitmentActivities> RecruitmentActivitiesList { get; set; }
        
        public ParentMeeting ParentMeeting { get; set; }
        public decimal ADA { get; set; }    

        public string ExplanationADAUnderPercentage { get; set; }
       
       public SelectListItem ChildFamilyReview { get; set; }
       public List<SelectListItem> TeacherHomeVisit { get; set; }


      public List<SelectListItem> ParentTeacherConference { get; set; }
        public DateTime? MonthLastDate { get; set; }
    }
    
    public class ParentMeeting:Workshop
    {
        public string EducationComponentDescription { get; set; }

        public int AttendanceCount { get; set; }
    }
}
