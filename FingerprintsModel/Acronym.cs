using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class Acronym
    {
        public Guid? AgencyId { get; set; }
        public Guid UserId { get; set; }
        public string AcronymName { get; set; }
        public long AcronymId { get; set; }
        public bool Status { get; set; }
    }
}
