using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class EductionMaterial
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string URLNote { get; set; }
        public string[] AttachmentPath { get; set; }
        public string UserId { get; set; }
        public string AgencyId { get; set; }
    }
}
