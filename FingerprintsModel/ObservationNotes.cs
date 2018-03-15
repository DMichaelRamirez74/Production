using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class ObservationNotes
    {
        public string NoteId { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }        
        public Int64 DomainId { get; set; }
        public Int64 ElementId { get; set; }
        public string[] lstClientid { get; set; }
        public string[] AttachmentPath { get; set; }

        public Dictionary<Int64, string> dictClientDetails = new Dictionary<Int64, string>();

        public string UserId { get; set; }
        
    }


}
