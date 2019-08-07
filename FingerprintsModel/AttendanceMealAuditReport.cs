using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
   public class AttendanceMealAuditReport:Pagination, IManagerReport
    {

       

        public List<SelectListItem> CenterList { get; set; }

        public string ClientName { get; set; }
        public string Enc_ClientID { get; set; }
        public string Dob { get; set; }
        public string DateOfFirstService {get;set;}

        public int EnrollmentStatus { get; set; }

        public int EnrollmentDays { get; set; }

        public double ADA { get; set; }

        public string CenterID{get;set;}

        public string CenterName{get;set;}

        public string ClassroomID{get;set;}

        public string ClassroomName{get;set;}

        public string StepUpToQualityStars{get;set;}

        public string SearchTerm { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<AttendanceMealAuditReport> AttendanceMealAuditReportList { get; set; }

    }
}
