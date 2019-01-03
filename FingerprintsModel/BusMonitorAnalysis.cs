using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class BusMonitorAnalysisList
    {
        public List<BusMonitorAnalysis> ListBusMonitor { get; set; }
        public BusMonitorAnalysisTotal AnalysisTotal { get; set; }
    }
    public class BusMonitorAnalysis
    {
        public long RouteId { get; set; }
        public string RouteName { get; set; }
        public string BusName { get; set; }
        public long CenterId { get; set; }
        public int Riders { get; set; }
        public int PickUp { get; set; }
        public int Drop { get; set; }
        public int RouteType { get; set; }
        public long ClientId { get; set; }
        public Guid AgencyId { get; set; }
        public Guid UserId { get; set; }
    }

    public class BusMonitorAnalysisTotal
    {
        public long RidersTotal { get; set; }
        public long PickUpTotal { get; set; }
        public long DropOffTotal { get; set; }
    }
    public class BusRiderChildrens
    {
        public string ClientName { get; set; }
        public long ClientId { get; set; }
        public string Address { get; set; }
        public string PickUpAddress { get; set; }
        public string DropAddress { get; set; }
        public string Disability { get; set; }
        public string Allergy { get; set; }
        public string PhoneNo { get; set; }
        public string PhoneType { get; set; }
        public string DisabilityDescription { get; set; }


        public List<SelectListItem> PhoneList { get; set; }
    }
    public class AssignedStaffBusMonitor
    {
        public string BusDriverName { get; set; }
        public string BusMonitorName { get; set; }
        public string RouteName { get; set; }
        public string NoOfStops { get; set; }
        public string ClientId { get; set; }
        public int RouteId { get; set; }
    }
}
