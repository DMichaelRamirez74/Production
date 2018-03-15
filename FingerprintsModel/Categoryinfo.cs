using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public class Categoryinfo
    {
        public Int32 CategoryID { get; set; }
        public Int32 CategoryIDpopup { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string mode { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public List<CategoryDetail> categoryList = new List<CategoryDetail>();
        public class CategoryDetail
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        public int ServiceID { get; set; }
        public int Category { get; set; }
        public bool Status { get; set; }
        public string ServiceDescription { get; set; }
        public string Year { get; set; }
        public string CommonTag { get; set; }
        public string PIRMatch { get; set; }
        public List<ServiceInfo> ServiceData = new List<ServiceInfo>();
        public class ServiceInfo
        {
            public int ServiceID { get; set; }
            public int Category { get; set; }
            public bool Status { get; set; }
            public string Description { get; set; }
            public string Year { get; set; }
            public string CommonTag { get; set; }
            public string PIRMatch { get; set; }
            public bool IsDeletedService { get; set; }
        }
    }
}
