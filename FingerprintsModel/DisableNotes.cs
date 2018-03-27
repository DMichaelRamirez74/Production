using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class DisableNotes
    {
        public string noteid { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string DisableDocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public string Createdon { get; set; }
        public string DisabilityTypeID { get; set; }
        public int YakkrId { get; set; }
        public string SpecialServiceDisability { get; set; }
        public List<DocumentInformation> DocumentList { get; set; }

        public List<DisableNotes> NotesList { get; set; }
        public int PrimaryDisability { get; set; }


    }

    public class DocumentInformation
    {
        public string DisableDocumentName { get; set; }
        public string DocumentDescription { get; set; }
        public string NotesId { get; set; }
        public string CreatedOn { get; set; }
    }
}
