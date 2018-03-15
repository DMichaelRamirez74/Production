using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class AttendanceTypeModel
    {
        public List<AttendanceType> attendanceTypeList { get; set; }
    }

    public class AttendanceType
    {
        public long IndexId { get; set; }
        public long AttendanceTypeId { get; set; }
        public string Acronym { get; set; }
        public string Description { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid UserId { get; set; }

        public bool Status { get; set; }

        public bool HomeBased { get; set; }

    }
}
