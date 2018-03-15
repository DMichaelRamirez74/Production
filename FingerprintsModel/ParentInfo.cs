using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{

    public class ParentInfoModel
    {
        public List<ParentInfo> ParentInfoList { get; set; }

        public List<Center> CenterList { get; set; }
    }
    public class ParentInfo
    {

        public long ClientId { get; set; }
        public string ChildName { get; set; }
        public string ParentName { get; set; }

        public bool NoEmail { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }

        public int PhoneType { get; set; }

        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }

        public bool IsSmsHomePhone { get; set; }
        public bool IsSmsWorkPhone { get; set; }
        public bool IsSmsMobilePhone { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsSms { get; set; }

        public long CenterId { get; set; }
        public long ClassRoomId { get; set; }

        public string CenterName { get; set; }
        public string ClassRoomName { get; set; }

        public string SearchText { get; set; }

        public long FilterType { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }


    }
}
