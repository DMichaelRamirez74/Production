using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class CenterTrasportationAnalysis
    {
        public List<TransportationAnalysis> TransportationAnalysisList { get; set; }
        public TransportationAnalysisTotal AnalysisTotal { get; set; }
    }


    public class TransportationAnalysis
    {
        public long CenterId { get; set; }
        public string Enc_CenterId { get; set; }
        public long Riders { get; set; }
        public long PickUp { get; set; }
        public long DropOff { get; set; }
        public string CenterName { get; set; }
        public Guid AgencyId { get; set; }
    }
    public class TransportationAnalysisTotal
    {
        public long RidersTotal { get; set; }
        public long PickUpTotal { get; set; }
        public long DropOffTotal { get; set; }
    }


    public class CenterDetails
    {
        public string CenterName { get; set; }
        public long CenterId { get; set; }
        public string Enc_CenterId { get; set; }
        public string CenterAddress { get; set; }
        
    }
    public class RiderChildrens
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




    public class AssignRouteChildren
    {
       //public HouseholdContactAddress HouseholdAddressChild { get; set; }
        //public List<PickUpContactAddress> PickUpAddressChildList { get; set; }
        //public List<DropUpContactAddress> DropUpAddressChildList { get; set; }
        public List<AssignedRouteAll> AssignRouteList { get; set; }
        public int SelectValue { get; set; }
        public string TestString { get; set; }
        public BusStaff BusStaffDetails { get; set; }
        public string CenterAddress { get; set; }
        public string PickUpRotes { get; set; }
        public string DropRotes { get; set; }
        public List<CenterDetails> CenterList { get; set; }

        public int CenterId { get; set; }
    }

    public class PickUpContactAddress
    {


        public long ClientId { get; set; }
        public string ChildrenName { get; set; }
        public string PickUpStatus { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PinLarge { get; set; }
        public string PinSmall { get; set; }
    }
    public class DropUpContactAddress
    {
        public long ClientId { get; set; }
        public string ChildrenName { get; set; }
        public string DropStatus { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PinLarge { get; set; }
        public string PinSmall { get; set; }
    }

    public class HouseholdContactAddress
    {
        public long ClientId { get; set; }
        public string HouseholdAddress { get; set; }
        public string HouseholdCity { get; set; }
        public string HouseholdState { get; set; }
        public string HouseholdCounty { get; set; }
        public string HouseholdZipCode { get; set; }
    }

    public class AssignedRouteAll
    {
        public long ClientId { get; set; }
        public string ChildrenName { get; set; }
        public string PickUpStatus { get; set; }
        public string DropStatus { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PinLarge { get; set; }
        public string PinSmall { get; set; }
        public string PickUpRouteName { get; set; }
        public string DropRouteName { get; set; }
        public int RouteType { get; set; }
        public int RouteId { get; set; }
    }

    public class BusStaff
    {
        public List<BusDriver> BusDriverList { get; set; }
        public List<BusMonitor> BusMonitorList { get; set; }
        public CenterDetails Center { get; set; }
    }

    public class BusDriver
    {
        public string BusDriverName { get; set; }
        public Guid BusDriverId { get; set; }
        public string Enc_BusDriverId { get; set; }
        public long CenterId { get; set; }
    }

    public class BusMonitor
    {
        public string BusMonitorName { get; set; }
        public Guid BusMonitorId { get; set; }
        public string Enc_BusMonitorId { get; set; }
        public long CenterId { get; set; }
    }

    public class RouteDetails
    {
        public string BusDriverId { get; set; }
        public string BusMonitorId { get; set; }
        public string Enc_BusMonitorId { get; set; }
        public string Enc_BusDriverId { get; set; }
        public string BusName { get; set; }
        public string RouteName { get; set; }
        public int RouteType { get; set; }
        public Guid AgencyId { get; set; }
        public Guid UserId { get; set; }
        public string RouteAddress { get; set; }
        public string BusMonitorName { get; set; }
        public string BusDriverName { get; set; }
        public long RouteId { get; set; }
        public int Mode { get; set; }
        public string Enc_RouteId { get; set; }
        public string Enc_CenterId { get; set; }
        public long CenterId { get; set; }
        public List<AssignedRouteChildren> ChildrenList { get; set; }
     
        
    }

    public class CreatedRoute
    {
        public RouteDetails RouteInfomation { get; set; }

        public List<AssignedRouteChildren> ChildrenList { get; set; }
    }

    

    public class AssignedRouteChildren
    {
        public long ClientId { get; set; }
        public string Enc_ClientId { get; set; }
        public string ChildrenName { get; set; }
        public string RouteAddress { get; set; }
        public string StopPosition { get; set; }
    }

    public class AssignedStaff
    {
        public string BusDriverName { get; set; }
        public string BusMonitorName { get; set; }
        public string RouteName { get; set; }
        public string NoOfStops { get; set; }
        public string ClientId { get; set; }
        public int RouteId { get; set; }
    }

}
