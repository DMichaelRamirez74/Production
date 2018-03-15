using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class FamilyDetail
    {
        public Int64 ClientId { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string Address { get; set; }
        public bool Isfamily { get; set; }
        public bool IsChild { get; set; }
        public string ParentId { get; set; }
        public string FamilyHomeless { get; set; }
        public string Employed { get; set; }
        public string CurrentMilitary { get; set; }
        public string EducationLevel { get; set; }
        

    }
}
