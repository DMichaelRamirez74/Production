using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class ScreeningAccess
    {
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }

        public bool IsEnter { get; set; }
        public bool IsReview { get; set; }
        public bool IsViewOnly { get; set; }

        public int ScreeningAccessType { get; set; }

        public enum ScreeningAccessEnum
        {
            Enter=1,
            Review=2,
            ViewOnly=3
        }

       
    }
}

   
