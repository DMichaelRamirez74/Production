using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class FacilitesModel
    {
        public List<FacilitiesManagerDashboard> FacilitiesDashboardList { get; set; }
        public FacilitesModel()
        {
            //this.FacilitiesDashboardList = new List<FacilitiesManagerDashboard>();
            //new FacilitesModel();
            this.FacilitiesDashboardList = new List<FacilitiesManagerDashboard>();
        }
        public static FacilitesModel GetInstance()
        {
            return new FacilitesModel();
        }

        public List<SelectListItem> StaffList = new List<SelectListItem>();
        public string Userid { get; set; }
    }

    public class AssignFacilityStaff
    {
        public int WorkId { get; set; }
        public string CenterName { get; set; }

        public string ImageCount { get; set; }
        public string StaffName { get; set; }
        public string ClassroomName { get; set; }
        public string UserDescription { get; set; }
        public string UserDescrption { get; set; }
        public string EstimatedDate { get; set; }
        public string EstimatedTime { get; set; }
        public string WorkOrderNumber { get; set; }
        public string workorderSequence { get; set; }
    
        public bool Request { get; set; }
        public string CenterAddress { get; set; }
        public string yakkrdescription { get; set; }
        public string RoleName { get; set; }
        public string AssignedTo { get; set; }
        public string StaffName1 { get; set; }

        public string WorkOrderDate { get; set; }
        public string RequestedDate { get; set; }
        public string YakkrCode { get; set; }
       
        public string DURL { get; set; }
        public List<SelectListItem> StaffList = new List<SelectListItem>();
        public string StaffContact { get; set; }
        public string StaffEmailaddress { get; set; }
        public string InternalAssignTo { get; set; }
        public string Userid { get; set; }
        public bool IsInternal { get; set; }
        public string ExternCompanyName { get; set; }
        public string YakkrId { get; set; }
        public string ExternalContactName { get; set; }
        public string ExternalContactNo { get; set; }
        public string ExternalAddress { get; set; }
        public string ExternalEmailId { get; set; }
        public string FacilityId { get; set; }
        public string IsTemporaryFix { get; set; }
        public string LaborHours { get; set; }
        public string MilesDriven { get; set; }
        public string Notes { get; set; }
       
        public List<PartDetails> PartDetails { get; set; }
        public List<AssignFacilityStaff> workOrderList { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public string SenderName { get; set; }
        public string SenderRole { get; set; }
        public string SenderPhone { get; set; }

    }
    public class DamageFixedImages
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Imagejson { get; set; }
        public byte[] ImageByte { get; set; }
        public string ImageURL { get; set; }
    }
    
    public class PartDetails
    {
        public string PartNumber { get; set; }
        public string Quantity { get; set; }
        public string UnitCost { get; set; }
        public string TotalCost { get; set; }
        public string PartDescription { get; set; }
    }

    public class FacilitiesManagerDashboard
    {
        public string CenterName { get; set; }
        public string Enc_CenterId { get; set; }
        public long CenterId { get; set; }
        public long OpenedWorkOrders { get; set; }
        public long AssignedWorkOrders { get; set; }
        public long InternalAssigned { get; set; }
        public long ExternalAssigned { get; set; }
        public long CompletedWorkOrders { get; set; }
        public long TemporarilyFixedWorkOrders { get; set; }
        public long AssignedHimself { get; set; }
      

    }

   
}

