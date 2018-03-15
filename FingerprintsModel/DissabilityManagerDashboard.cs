using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class DissabilityManagerDashboard
    {
        public string CenterId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string SeatsAvailable { get; set; }
        public string yakkrcount { get; set; }
        public string option1 { get; set; }
        public string option2 { get; set; }
        public string option3 { get; set; }
        public string Routecode100 { get; set; }
        public string Routecode101 { get; set; }
        public string Routecode102 { get; set; }
        public string TotalWaitingList { get; set; }
        public string Attendance { get; set; }
        public bool Homebased { get; set; }
        public string TotalChildren { get; set; }
        public string DisabilityPercentage { get; set; }
        public string Indicated { get; set; }
        public string Pending { get; set; }
        public string Qualified { get; set; }
        public string Released { get; set; }
        public List<DisablilityType> disabilitytype { get; set; }
        public string EFileName { get; set; }
        public string EFileExtension { get; set; }
        public string EImagejson { get; set; }
        public byte[] EImageByte { get; set; }



    }

    public class DisablilityType
    {
        public int Id { get; set; }
        public string DisabilityType { get; set; }
    }
}
