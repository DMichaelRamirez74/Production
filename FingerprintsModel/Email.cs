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
            public Enums.EmailStatus EmailStatus { get; set; }
            public long ReferenceID { get; set; }
            public Enums.EmailType EmailType { get; set; }
            public StaffDetails staffDetails { get; set; }

        }


      
    }
}
