using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FingerprintsModel
{
    public class RaceSubcategory
    {
        [Required(ErrorMessage = "Please enter Race Sub-category name")]
        public string SubCategoryName { get; set; }
        public string RaceDescription { get; set; }
        public List<RaceInfo> raceList = new List<RaceInfo>();
        public string RaceID { get; set; }
        public string RaceCategoryName { get; set; }
        public Int32 RaceSubCategoryID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public Boolean _isactive = true;
        public Boolean IsActive
        {
            get
            {
                return _isactive;
            }
            set
            {
                _isactive = value;
            }
        }

        public class RaceInfo
        {
            public string RaceId { get; set; }
            public string Name { get; set; }
        }

    }

    
}
