using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fingerprints.Filters;
using FingerprintsData;
using FingerprintsModel;
using System.Data;
using GoogleMaps.LocationServices;
using System.Web.Script.Serialization;

namespace Fingerprints.Controllers
{


    /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
     roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
     roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
     roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
     roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
     roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
     roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacher)
     roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
     roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
     roleid=825f6940-9973-42d2-b821-5b6c7c937bfe(Facilities Manager)
     roleid=c352f959-cfd5-4902-a529-71de1f4824cc (Social Service Manager)
     roleid=2af7205e-87b4-4ca7-8ca8-95827c08564c (Area Manager)
     roleid=2d9822cd-85a3-4269-9609-9aabb914D792 (HR Manager)
     roleid=6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba (Transportation Manager)
     roleid=7c2422ba-7bd4-4278-99af-b694dcab7367 (executive)
     roleid=b65759ba-4813-4906-9a69-e180156e42fc (ERSEA Manager)
     roleid=4b77aab6-eed1-4ac3-b498-f3e80cf129c0 (Education Manager)
      roleid=944d3851-75cc-41e9-b600-3fa904cf951f (Billing Manager)
      roleid=3b49b025-68eb-4059-8931-68a0577e5fa2 (Agency Admin)
      roleid=047c02fe-b8f1-4a9b-b01f-539d6a238d80 (Disabilities Manager)
     */

    //[CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
    public class TransportationController : Controller
    {




        // public readonly string GoogleApiKey = System.Configuration.ConfigurationManager.AppSettings["GoogleMapKey"].ToString();

        public readonly string GoogleApiKey ="AIzaSyCbNxbfW0RSsXvvj6J7li2e3WDxlpD9xt8";

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult Dashboard()
        {
            CenterTrasportationAnalysis transportanalysis = new CenterTrasportationAnalysis();
            try
            {
                transportanalysis = new TransportationData().GetTransportAnalysisData(new Guid(Session["AgencyId"].ToString()));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(transportanalysis);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult CreateRoute()
        {
            AssignRouteChildren childern = new AssignRouteChildren();
            List<AssignedRouteAll> allList = new List<AssignedRouteAll>();
            try
            {
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                allList = new TransportationData().GetChildrenRouteAssignedDetail(agencyId);
                List<CenterDetails> centerlist = new List<CenterDetails>();
                string address = string.Empty;
                childern.BusStaffDetails = new TransportationData().GetBusStaffData(out address, out centerlist,agencyId);
                childern.CenterAddress = address;
                childern.CenterList = centerlist;
               // allList = GetLatLongAll(allList);
                childern.AssignRouteList = allList;
                JavaScriptSerializer js = new JavaScriptSerializer();
                childern.TestString = js.Serialize(childern.AssignRouteList);
                List<AssignedRouteAll> pickupRouteNames = allList.Where(a => a.PickUpRouteName != null && a.PickUpRouteName != "Not Assigned").OrderBy(a => a.Latitude).OrderBy(a=>a.Longitude).ToList();
                List<AssignedRouteAll> dropRouteNames = allList.Where(a => a.DropRouteName != null && a.DropRouteName != "Not Assigned").OrderBy(a => a.Latitude).OrderBy(a => a.Longitude).ToList();

                string[] listPickUp = pickupRouteNames.Where(a => a.PickUpRouteName != null && a.PickUpRouteName != "Not Assigned").Select(a => a.PickUpRouteName).ToArray();
                string[] listPickDrop = dropRouteNames.Where(a => a.DropRouteName != null && a.DropRouteName != "Not Assigned").Select(a => a.DropRouteName).ToArray();
               var listPickUp1= listPickUp.Union(listPickDrop).ToList();
                // var listDrop = allList.Select(a => new { RoputeName = a.DropRouteName }).ToList();
                

                TempData["pickupRouteNames"] = js.Serialize(pickupRouteNames);
                TempData["dropRouteNames"] = js.Serialize(dropRouteNames);
                TempData["listPickUp1"] = listPickUp1;
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
         
            return View(childern);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult AddRoute(string routeDetailsString)
        {
            bool isResult = false;
            RouteDetails routeDetails = new RouteDetails();
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                routeDetails = js.Deserialize<RouteDetails>(routeDetailsString);
                routeDetails.CenterId = Convert.ToInt64(EncryptDecrypt.Decrypt64(routeDetails.Enc_CenterId));
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                string agency = Session["AgencyId"].ToString();
                routeDetails.AgencyId = agencyId;
                routeDetails.UserId = new Guid(Session["UserID"].ToString());
                isResult = new TransportationData().InsertRouteData(routeDetails);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult GetCreatedRoute()
        {
            List<AssignedRouteChildren> assignedChildList = new List<AssignedRouteChildren>();
            List<RouteDetails> routeDetailsList = new List<RouteDetails>();
            try
            {

                routeDetailsList=  new TransportationData().GetCreatedRouteData(new Guid(Session["AgencyId"].ToString()));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(routeDetailsList, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult GetCreatedRouteByCenter(string enc_CenterId)
        {
            List<AssignedRouteChildren> assignedChildList = new List<AssignedRouteChildren>();
            List<RouteDetails> routeDetailsList = new List<RouteDetails>();
            try
            {
                long centerId = Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_CenterId));

                routeDetailsList = new TransportationData().GetCreatedRouteDataByCenter(new Guid(Session["AgencyId"].ToString()),centerId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(routeDetailsList, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult AssignRoute(int id = 0,string Center="")
        {
            AssignRouteChildren childern = new AssignRouteChildren();
            childern.CenterId = 0;
            childern.CenterId = (Center==""|| Center=="0")?0:Convert.ToInt32(EncryptDecrypt.Decrypt64(Center));
            List<AssignedRouteAll> allList = new List<AssignedRouteAll>();
            try
            {


                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                switch (id)
                {

                    case 1:
                        {
                            allList = new TransportationData().GetPickUpChildrenData(agencyId, childern.CenterId);
                            break;
                        }
                    case 2:
                        {
                            allList = new TransportationData().GetDropUpChildrenData(agencyId, childern.CenterId);
                            break;
                        }
                    default:
                        {
                            allList = new TransportationData().GetChildrenRouteAssigned(agencyId, childern.CenterId);
                            break;
                        }
                }
                string PickupRoute = "", DropRoute = "";
                new TransportationData().GetRoutes(ref PickupRoute,ref DropRoute, agencyId.ToString(), childern.CenterId);
              //  allList = GetLatLongAll(allList);
                childern.AssignRouteList = allList;
                JavaScriptSerializer js = new JavaScriptSerializer();
                childern.TestString = js.Serialize(childern.AssignRouteList);
                childern.PickUpRotes = PickupRoute;
                childern.DropRotes = DropRoute;
                childern.SelectValue = id;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return View(childern);
            //return View();
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult AcceptRejectTransportationRequest(string ClientId, string YakkrID, string Status)
        {
            bool isUpdated = false;
            try
            {
                isUpdated = new TransportationData().AcceptRejectTransportRequest(ClientId, YakkrID, Status, Session["AgencyID"].ToString(), Session["UserID"].ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isUpdated);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult GetTransportationandHouseholdDetails(string ClientId)
        {
            string JSONString = string.Empty;
            try
            {
                DataSet dtTransportation = new DataSet();
                new TransportationData().GetTranportationandHouseholdDetails(ref dtTransportation, Session["AgencyID"].ToString(), ClientId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtTransportation);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult GetRiderChildren(string centerId)
        {
            List<RiderChildrens> riderChildrenData = new List<RiderChildrens>();
            try
            {
                long dec_centerId = Convert.ToInt64(EncryptDecrypt.Decrypt64(centerId));
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                riderChildrenData = new TransportationData().GetRiderChildrensData(agencyId, dec_centerId);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(riderChildrenData, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult GetChildrenToAssignRoute()
        {

            List<AssignedRouteAll> allList = new List<AssignedRouteAll>();
            try
            {
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                allList = new TransportationData().GetChildrenRouteAssigned(agencyId,0);
                allList = GetLatLongAll(allList);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(allList, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult GetPickUpChildren()
        {

            List<AssignedRouteAll> pickUpList = new List<AssignedRouteAll>();
            try
            {
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                pickUpList = new TransportationData().GetPickUpChildrenData(agencyId,0);
                pickUpList = GetLatLongAll(pickUpList);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(pickUpList, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult GetDropUpChildren()
        {
            AssignRouteChildren assignChild = new AssignRouteChildren();
            List<AssignedRouteAll> dropChildrenList = new List<AssignedRouteAll>();
            try
            {
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                dropChildrenList = new TransportationData().GetDropUpChildrenData(agencyId,0);
                dropChildrenList = GetLatLongAll(dropChildrenList);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(dropChildrenList, JsonRequestBehavior.AllowGet);
        }

        public List<AssignedRouteAll> GetLatLongAll(List<AssignedRouteAll> allChildren)
        {
            if (allChildren.Count() > 0)
            {
               
                var locationService = new GoogleLocationService(GoogleApiKey);
                foreach (var item in allChildren)
                {
                    var address = item.Address + ' ' + item.City + ',' + item.State + ' ' + item.ZipCode;
                    // var address = "75 Ninth Avenue 2nd and 4th Floors New York, NY 10011";

                    var point = locationService.GetLatLongFromAddress(address);
                    if (point != null)
                    {
                        item.Latitude = string.IsNullOrEmpty(point.Latitude.ToString()) ? 0 : Convert.ToDouble(point.Latitude.ToString());

                        item.Longitude = string.IsNullOrEmpty(point.Longitude.ToString()) ? 0 : Convert.ToDouble(point.Longitude.ToString());
                    }
                    else
                    {
                        item.Latitude = 0;
                        item.Longitude = 0;
                    }
                }
            }

            return allChildren;

        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult SaveAssignedRoute(string ClientId, string RouteId,string RouteType)
        {
            bool isUpdated = false;
            try
            {
                isUpdated = new TransportationData().SaveAssignedRoute(ClientId, RouteId, RouteType, Session["AgencyID"].ToString(), Session["UserID"].ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isUpdated);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult DeleteAssignedRoute(string ClientId, string RouteId)
        {
            bool isDeleted = false;
            try
            {
                isDeleted = new TransportationData().DeleteAssignedRoute(ClientId, RouteId, Session["AgencyID"].ToString(), Session["UserID"].ToString());
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isDeleted);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult UpdatePosition(List<string> listChild, string RouteId)
        {
            bool isUpdated = false;
            try
            {
                int i = 1;
                foreach(string childid in listChild)
                {
                    isUpdated = new TransportationData().UpdatePosition(childid, RouteId, Session["AgencyID"].ToString(), i);
                    i++;
                }
               
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isUpdated);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public ActionResult GetChildDetails(string ClientId)
        {
            string JSONString = string.Empty;
            try
            {
                DataSet dtTransportation = new DataSet();
                new TransportationData().GetChildDetails(ref dtTransportation, Session["AgencyID"].ToString(), ClientId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtTransportation);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult GetRouteAssignedByCenter(string centerID,int routeType)
        {
            List<AssignedStaff> staffList = new List<AssignedStaff>();
            try
            {
                long dec_CenterId =Convert.ToInt64(EncryptDecrypt.Decrypt64(centerID));
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                Guid userId = new Guid(Session["UserID"].ToString());
                staffList = new TransportationData().GetAssignedRouteInfo(dec_CenterId, agencyId, userId,routeType);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(staffList, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter("6ed25f82-57cb-4c04-ac8f-a97c44bdb5ba")]
        public JsonResult GetBusStaffByCenter(string enc_CenterId)
        {
            BusStaff staff = new BusStaff();
            List<AssignedRouteAll> allList = new List<AssignedRouteAll>();
            List<string> pickUpList = new List<string>();
            string pickUpRouteNames = string.Empty;
            string dropRouteName = string.Empty;
            string TestString = string.Empty;
            try
            {
                long centerId = (enc_CenterId=="0")?0:Convert.ToInt64(EncryptDecrypt.Decrypt64(enc_CenterId));
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                staff = new TransportationData().GetBusStaffByCenter(agencyId, centerId);
                allList = new TransportationData().GetChildrenRouteAssignedDetailByCenter(agencyId, centerId);
             //   allList = GetLatLongAll(allList);
                JavaScriptSerializer js = new JavaScriptSerializer();
                TestString = js.Serialize(allList);
                List<AssignedRouteAll> pickupRouteNames = allList.Where(a => a.PickUpRouteName != null && a.PickUpRouteName != "Not Assigned").OrderBy(a => a.Latitude).OrderBy(a => a.Longitude).ToList();
                List<AssignedRouteAll> dropRouteNames = allList.Where(a => a.DropRouteName != null && a.DropRouteName != "Not Assigned").OrderBy(a => a.Latitude).OrderBy(a => a.Longitude).ToList();

                string[] listPickUp = pickupRouteNames.Where(a => a.PickUpRouteName != null && a.PickUpRouteName != "Not Assigned").Select(a => a.PickUpRouteName).ToArray();
                string[] listPickDrop = dropRouteNames.Where(a => a.DropRouteName != null && a.DropRouteName != "Not Assigned").Select(a => a.DropRouteName).ToArray();
                var listPickUp1 = listPickUp.Union(listPickDrop).ToList();
                // var listDrop = allList.Select(a => new { RoputeName = a.DropRouteName }).ToList();


                 pickUpRouteNames = js.Serialize(pickupRouteNames);
                dropRouteName = js.Serialize(dropRouteNames);
                pickUpList = listPickUp1;
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);

            }
            return Json(new { staff, pickUpRouteNames, dropRouteName, pickUpList, TestString },JsonRequestBehavior.AllowGet);
        }
    }
}