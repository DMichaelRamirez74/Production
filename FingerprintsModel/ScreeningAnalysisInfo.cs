using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{

    public class ScreeningAnalysisInfoModel
    {
        public List<ScreeningAnalysisInfo> ScreeningAnalysisInfoList { get; set; }

        public List<Center> CenterList { get; set; }
    }
    public class ScreeningAnalysisInfo
    {

        public long ClientId { get; set; }
        public string ChildName { get; set; }
        public string CenterName { get; set; }
        public string ClassroomName { get; set; }
        public string NumofDaysScreening { get; set; }
        public string ScreeningDate { get; set; }
        public string ScreeningResults { get;set; }
        public string RescreenNumofDaysScreening { get; set; }
        public string dob { get; set; }
        public string RescreenDate { get; set; }
        public string RescreenResults { get; set; }
        public string DateofFirstService { get; set; }
        public long CenterId { get; set; }
        public long ClassRoomId { get; set; }

        public string InitialParentDecaDate { get; set; }
        public string FamilyCompleted { get; set; }
        public string Relationship { get; set; }
        public string AreaofNeed1 { get; set; }
        public string AreaofNeed2 { get; set; }
        public string InitialStaffDecaDate { get; set; }
        public string StaffCompleted { get; set; }
        public string MentalHealthDate { get; set; }
        public string ReferralBasedOn { get; set; }

        public string SearchText { get; set; }

        public long FilterType { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }


    }
}
