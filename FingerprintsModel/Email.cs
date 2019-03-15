using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
 public   class Email
    {

    
    public  static string ConnectionId { get; set; }
     public   static ConcurrentQueue<int> conque { get; set; }
        public static int TotalEmails { get; set; }

        public class ClientEmailReport
        {
            public long ClientID { get; set; }
            public long ParentID { get; set; }
            public EmailStatusEnum EmailStatus { get; set; }
            public long ReferenceID { get; set; }
            public EmailTypeEnum EmailType { get; set; }
            public StaffDetails staffDetails { get; set; }

        }


        public enum EmailTypeEnum
        {
            General=1,
            CenterClosure =2,
            ClassroomClosure=3,
            UnscheduledSchoolDay=4
        }

        public enum EmailStatusEnum
        {
            All = 0,
            SentEmails =1,
            BouncedEmails=2,
            NoEmails=3,
           
        }

    }
}
