using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class ERSEADashBoard
    {
        public List<ApplicationEnrollment> lstApplication = new List<ApplicationEnrollment>();

        public List<ADA> listADA = new List<ADA>();
    }

    public class ApplicationEnrollment
    {
        public string ZipCode { get; set; }
        public string CityName { get; set; }
        public int Application { get; set; }
        public int Enrollment { get; set; }
        public int Withdrawn { get; set; }
        public int Dropped { get; set; }
    }

    public class CityName
    {
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string PrimaryCity { get; set; }
    }
    public class ADA
    {
        public string CenterName { get; set; }
        public decimal Jan { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Apr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Aug { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dec { get; set; }

    }
}
