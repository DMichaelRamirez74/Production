using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class MatrixSummary
    {
        public long TotalClients { get; set; }
        public long TotalAnswered { get; set; }
        public long TotalPoints { get; set; }

        public double TotalRatio { get; set; }

        public long AssessmentGroupId { get; set; }
        public long AssessmentCategoryId { get; set; }
        public string AssessmentGroupType { get; set; }

        public string Category { get; set; }
        public long AssessmentNumber { get; set; }
        public string ProgramYear { get; set; }

        public Guid AgencyId { get; set; }
        public long CategoryPosition { get; set; }

        public long MaxMatrixValue { get; set; }

      
    }

    public class MasterMatrixSummary
    {
        public long FamiliesEntered { get; set; }
        public long TotalFamilies { get; set; }
        public long AssessmentNumberMaster { get; set; }

        public double PercentFamilyEntered { get; set; }
    }

    public class ChangePercentage
    {
        public double ChangePercent { get; set; }
        public string FontColor { get; set; }
        public string ArrowType { get; set; }
        public long AssessmentCategoryId { get; set; }
        public long AssessmentGroupId { get; set; }

        public string AssessmentGroupType { get; set; }
        public string Category { get; set; }

        public long CategoryPosition { get; set; }


    }
}
