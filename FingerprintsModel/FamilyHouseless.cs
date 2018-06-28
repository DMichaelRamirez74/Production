using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class FamilyHouseless
    {
        public FamilyHouseless()
        {
            this.CaseNoteDetails = new RosterNew.CaseNote();
            this.FamilyHousehold = new FamilyHousehold();
            this.UsersList = new RosterNew.Users();
            this.CaseNoteAttachments = new List<RosterNew.Attachment>();
            this.UsersList.Clientlist = new List<RosterNew.User>();
            this.UsersList.UserList = new List<RosterNew.User>();
            this.NewAddressHousehold = new FamilyHousehold();
        }

        public RosterNew.CaseNote CaseNoteDetails { get;set;}

        public List<RosterNew.Attachment> CaseNoteAttachments { get; set; }
        //public RosterNew.ClientUsers DevelopmentTeamStaffs { get; set; }
        public RosterNew.Users UsersList { get; set; }
        public FamilyHousehold FamilyHousehold { get; set; }

        public FamilyHousehold NewAddressHousehold { get; set; }

        public bool HasCaseNoteDetails { get; set; }

        public bool HasNewAddress { get; set; }


        
    }
}
