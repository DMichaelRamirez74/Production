using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{



    public class SubstituteRole : Pagination, IManagerReport
    {

        public SubstituteRole()
        {

        }
        public SubstituteRole(bool intializeObject)
        {
            if(intializeObject)
            {
                this.StaffDetails = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>(false);
                this.SubsituteRoleList = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<List<SubstituteRole>>();
            }
        }

        public StaffDetails StaffDetails { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public bool Status { get; set; }
        public long SubstituteID { get; set; }

        public string CenterID { get; set; }


        public string CenterName { get; set; }

        public string ClassroomID { get; set; }

        public string ClassroomName { get; set; }

        public string StepUpToQualityStars { get; set; }

        public string SearchTerm { get; set; }

        public bool Editable { get; set; }



        public List<SubstituteRole> SubsituteRoleList { get; set; }
    }



   


}
