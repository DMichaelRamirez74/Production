using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class InvoiceDetails
    {
        public string Id { get; set; }
        public string AgencyId { get; set; }
        public string ProgramTypeId { get; set; }
        public string Month { get; set; }
        public string Invoicedate { get; set; }
        public string ChildId { get; set; }
        public string Amount { get; set; }
        public string DueDate { get; set; }
        public string UserId { get; set; }
        public string CenterId { get; set; }
        public string FamilyId { get; set; }
        public string FamilyName { get; set; }
        public string ChildName { get; set; }
    }
}
