using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class MatrixScore
    {

        public string ClientId { get; set; }
        public Guid AgencyId { get; set; }
        public long Dec_ClientId { get; set; }
        public string HouseHoldId { get; set; }
        public long Dec_HouseHoldId { get; set; }
        public string ActiveYear { get; set; }
        public string ProgramId { get; set; }
        public long Dec_ProgramId { get; set; }
        public string CenterId { get; set; }
        public long Dec_CenterId { get; set; }
        public long ClassRoomId { get; set; }
        public string ProgramType { get; set; }
        public string ProfilePic { get; set; }
        public long AssessmentCategoryId { get; set; }
        public long Testvalue { get; set; }
        public string ParentName { get; set; }
        public string AssessmentCategory { get; set; }
        public bool IsChecked { get; set; }
        public long ParentId{ get; set; }
        public Guid UserId { get; set; }
        public long AssessmentGroupId { get; set; }
        public long MatrixScoreId { get; set; }
        public string AssessmentGroupType { get; set; }
        public long  AnnualAssessmentType { get; set; }

        public int TotalCount { get; set; }

        public long AssessmentNumber { get; set; }

        public string StaffName { get; set; }

        public string Date { get; set; }
        public AnnualAssessment annualassessment { get; set; }
        public List<MatrixScore> MatrixScoreList { get; set; }

        public List<long> CategoryIdList { get; set; }
        public List<System.Web.Mvc.SelectListItem> ActiveYearList { get; set; }

        public List<ParentNames> ParentList = new List<ParentNames>();


        public int Id { get; set; }
    }

    public class ChartDetails
    {
        public long TestValueSum { get; set; }
        public double ResultPercentage { get; set; }
        public long AssessementCategoryId { get; set; }
        public long AssessmentNumber { get; set; }
        public long GroupIdCount { get; set; }
        public double ChartHeight { get; set; }

        public long MaximumMatrixValue { get; set; }

    }

    public class MatrixRecommendations
    {
        public long AssessmentCategoryId { get; set; }
        public long AssessmentGroupId { get; set; }
        public string AssessmentGroupType { get; set; }
        public string Category { get; set; }
        public bool FPASuggested { get; set; }
        public bool ReferralSuggested { get; set; }
        public long CategoryPosition { get; set; }
        public long AssessmentNumber { get; set; }
        public long TestValue { get; set; }
        public string Description { get; set; }
        public string CientId { get; set; }
        public long Dec_ClientId { get; set; }
        public string HouseHoldId { get; set; }
        public long Dec_HouseHoldId { get; set; }
        public string ActiveProgramYear { get; set; }
        public Guid UserId { get; set; }
        public Guid AgencyId { get; set; }
        public bool Status { get; set; }
        public long MatrixRecommendationId { get; set; }
    }
    public class ParentNames
    {
        public int ParentID { get; set; }
        public string ParentName { get; set; }

        public int ParentInvolved { get; set; }

    }
    public class ShowRecommendations
    {
        public bool ShowPopup { get; set; }
        public long AssessmentNumber { get; set; }
        public Guid AgencyId { get; set; }
       

    }
    
}
