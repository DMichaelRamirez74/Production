using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
  public class CaseNoteViewModel :Pagination,IManagerReport
    {
        public string Enc_CaseNoteId { get; set; }

        public string Date { get; set; }
        public string Title { get; set; }

        public string Notes { get; set; }

        public List<SelectListItem> Tags { get; set; }

        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> DevelopmentalTeam { get; set; }

        public bool SecureNoteLevel { get; set; }

        public List<SelectListItem> Attachments { get; set; }

        public string CenterName { get; set; }
       
        public string CenterID { get; set; }
      
        public string ClassroomName { get; set; }
        

        public string ClassroomID { get; set; }
        
        public string StepUpToQualityStars { get; set; }


        public IEnumerable<CaseNoteViewModel> CaseNotes { get; set; }
        
    }
}
