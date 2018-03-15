using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ParentParticipation
    {

        public ParentParticipation()
        {
            this.InKindTransactionsList = new List<InKindTransactions>();
            this.InKindActivityList = new List<InkindActivity>();
            this.ChildDetailsList = new List<EnrolledChildren>();
            this.ParentDetails = new StaffDetails();
        }
        public List<InKindTransactions> InKindTransactionsList { get; set; }

        public List<InkindActivity> InKindActivityList { get; set; }

        public List<EnrolledChildren> ChildDetailsList { get; set; }

        public StaffDetails ParentDetails { get; set; }

        public string ParentId { get;set; }
        
            
    }

    


}
