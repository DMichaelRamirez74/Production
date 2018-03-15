using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class DomainObservationResults
    {
        public Int64 ChildId { get; set; }
        public string ChildName { get; set; }        

        public List<Domain> lstDomain = new List<Domain>();

        public List<Notes> lstNotes = new List<Notes>();
    }

    public class Domain
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Count { get; set; }
    }

    public class Notes
    {
        public string NoteId { get; set; }
        public string Date { get; set; }
        public string Note { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Element { get; set; }
        public string Attchment { get; set; }
    }
}
