using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class DisabilityChildren
    {

        public DisbilityChild DisabilityChildInfo { get; set; }
        public class DisbilityChild
        {
            public string ChildName { get; set; }
            public string Enc_ClientId { get; set; }
            public long ClientId { get; set; }
            public long YakkrId { get; set; }
            public string RouteCode { get; set; }
            public string CenterName { get; set; }
            public string DateOfBirth { get; set; }
            public string CenterId { get; set; }
            public string ClassRoomId { get; set; }
            public string DisableNotesId { get; set; }
            public string DisableAttachment
            {
                get;set;
            }
            public string ProgramId { get; set; }

            public string DisabilityTypeId { get; set; }
            public List<SelectListItem> AttachmentPath { get; set; }
            public string SpecialServices { get; set; }
            public string ClassRoomName { get; set; }
            public string Gender { get; set; }
            public string FswName { get; set; }
            public string SchoolDistrict { get; set; }
        }

        

    }

    public class DisabilityDocument
    {
        public string[] AttachmentPath { get; set; }
        public string ClientId { get; set; }
        public string YakkrId { get; set; }
        public string RouteCode { get; set; }
        public int mode { get; set; }
        public bool IsAccepted { get; set; }
    }
}
