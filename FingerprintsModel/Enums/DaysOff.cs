using System.ComponentModel.DataAnnotations;

namespace FingerprintsModel.Enums
{


    public class DaysOffEnumClass
    {
        public enum DaysOffType
        {
            [Display(Name = "Agency Wide Closed")]
            AgencyWideClosed = 1,

            [Display(Name = "Entire Center Closed")]
            EntireCenterClosed = 2,

            [Display(Name = "Classroom Closed")]
            ClassRoomClosed = 3

        }
    }
        
    
}
