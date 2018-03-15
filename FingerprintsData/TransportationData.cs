using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using FingerprintsModel;

namespace FingerprintsData
{
    public class TransportationData
    {

        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;

        public TransportationData()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
            }
        }

        public CenterTrasportationAnalysis GetTransportAnalysisData(Guid agencyId)
        {

            List<TransportationAnalysis> transportList = new List<TransportationAnalysis>();
            TransportationAnalysis transportData = null;
            TransportationAnalysisTotal totalCount = new TransportationAnalysisTotal();
            CenterTrasportationAnalysis transportAnalysis = new CenterTrasportationAnalysis();

            try
            {

                using (Connection)
                {

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetTransportationAnalysis";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                transportData = new TransportationAnalysis();
                                transportData.CenterId = Convert.ToInt64(dr["CenterId"]);
                                transportData.CenterName = dr["CenterName"].ToString();
                                transportData.Enc_CenterId = EncryptDecrypt.Encrypt64(transportData.CenterId.ToString());
                                transportData.PickUp = Convert.ToInt64(dr["PickUpCount"]);
                                transportData.DropOff = Convert.ToInt64(dr["DropCount"]);
                                transportData.Riders = Convert.ToInt64(dr["RidersCount"]);
                                transportData.AgencyId = new Guid(dr["AgencyId"].ToString());
                                transportList.Add(transportData);
                            }
                        }

                        if (transportList.Count() > 0)
                        {
                            totalCount.RidersTotal = transportList.Sum(x => x.Riders);
                            totalCount.PickUpTotal = transportList.Sum(x => x.PickUp);
                            totalCount.DropOffTotal = transportList.Sum(x => x.DropOff);
                        }
                        else
                        {
                            totalCount.RidersTotal = 0;
                            totalCount.PickUpTotal = 0;
                            totalCount.DropOffTotal = 0;
                        }

                        transportAnalysis.TransportationAnalysisList = transportList;
                        transportAnalysis.AnalysisTotal = totalCount;
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
                Connection.Dispose();
            }
            return transportAnalysis;

        }

        public List<RiderChildrens> GetRiderChildrensData(Guid agencyId, long centerId)
        {
            List<RiderChildrens> childrenList = new List<RiderChildrens>();
            List<RiderChildrens> childrenList2 = new List<RiderChildrens>();
            RiderChildrens childrens = null;
            try
            {
                using (Connection)
                {

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetRidersChildren";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                childrens = new RiderChildrens();

                                childrens.Address = dr["HouseHoldAddress"].ToString();
                                childrens.PickUpAddress = dr["PickUpAddress"].ToString();
                                childrens.DropAddress = dr["DropAddress"].ToString();
                                childrens.Disability = (Convert.ToInt64(dr["Disability"].ToString()) == 1) ? "Yes" : "No";
                                childrens.DisabilityDescription = dr["DisabilityDescription"].ToString();
                                childrens.PhoneNo = dr["Phoneno"].ToString();
                                childrens.PhoneType = dr["PhoneType"].ToString();
                                childrens.Allergy = dr["FoodAllergiescomment"].ToString();
                                childrens.ClientName = dr["ChildName"].ToString();
                                childrens.ClientId = Convert.ToInt64(dr["ClientId"].ToString());
                                childrenList.Add(childrens);

                            }
                            RiderChildrens children2 = null;
                            var list2 = childrenList.Select(x => x.ClientId).Distinct();
                            foreach (var item in list2)
                            {
                                children2 = new RiderChildrens();
                                var list3 = childrenList.Where(x => x.ClientId == item).ToList();
                                children2.Address = list3.Select(x => x.Address).FirstOrDefault();
                                children2.PickUpAddress = list3.Select(x => x.PickUpAddress).FirstOrDefault();
                                children2.DropAddress = list3.Select(x => x.DropAddress).FirstOrDefault();
                                children2.ClientName = list3.Select(x => x.ClientName).FirstOrDefault();
                                children2.Disability = list3.Select(x => x.Disability).FirstOrDefault();
                                children2.DisabilityDescription = list3.Select(x => x.DisabilityDescription).FirstOrDefault();
                                children2.Allergy = list3.Select(x => x.Allergy).FirstOrDefault();
                                children2.ClientId = list3.Select(x => x.ClientId).FirstOrDefault();
                                children2.PhoneList = new List<SelectListItem>();
                                foreach (var item2 in list3)
                                {
                                    SelectListItem itemselect = new SelectListItem
                                    {
                                        Text = item2.PhoneType,
                                        Value = item2.PhoneNo
                                    };
                                    children2.PhoneList.Add(itemselect);
                                }

                                childrenList2.Add(children2);


                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
                Connection.Dispose();
            }
            return childrenList2;

        }

        public List<AssignedRouteAll> GetChildrenRouteAssigned(Guid agencyId, int CenterId)
        {

            AssignedRouteAll allChildren = null;
            List<AssignedRouteAll> allChildrenRoute = new List<AssignedRouteAll>();
            try
            {

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetPickUpDropUpChildren";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                allChildren = new AssignedRouteAll
                                {
                                    PickUpStatus = dr["PickUpStatus"].ToString(),
                                    Address = dr["PickUpAddress"].ToString(),
                                    City = dr["PickUpCity"].ToString(),
                                    ZipCode = dr["PickUpZipCode"].ToString(),
                                    State = dr["PickUpSate"].ToString(),
                                    ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                    ChildrenName = dr["ChildName"].ToString(),
                                    PinLarge = "/images/pointer_green_big.png",
                                    PinSmall = "/images/pointer_green_small.png",
                                    Latitude = Convert.ToDouble(dr["PickupLatitude"].ToString()),
                                    Longitude = Convert.ToDouble(dr["PickupLongitude"].ToString()),
                                    RouteId = Convert.ToInt32(dr["PickUpRouteId"]),
                                    RouteType = Convert.ToInt32(dr["PickUpRouteType"])

                                };
                                allChildrenRoute.Add(allChildren);
                                allChildren = new AssignedRouteAll
                                {
                                    DropStatus = dr["DropStatus"].ToString(),
                                    Address = dr["DropAddress"].ToString(),
                                    City = dr["DropCity"].ToString(),
                                    ZipCode = dr["DropZipCode"].ToString(),
                                    State = dr["DropState"].ToString(),
                                    ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                    ChildrenName = dr["ChildName"].ToString(),
                                    PinLarge = "/images/pointer_red_big.png",
                                    PinSmall = "/images/pointer_red_small.png",
                                    Latitude = Convert.ToDouble(dr["DropLatitude"].ToString()),
                                    Longitude = Convert.ToDouble(dr["DropLongitude"].ToString()),
                                    RouteId = Convert.ToInt32(dr["DropUpRouteId"]),
                                    RouteType = Convert.ToInt32(dr["DropUpRouteType"])

                                };
                                allChildrenRoute.Add(allChildren);
                            }

                        }

                    }

                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
                if (Connection != null)
                    Connection.Dispose();
            }
            return allChildrenRoute;

        }

        public List<AssignedRouteAll> GetChildrenRouteAssignedDetail(Guid agencyId)
        {

            AssignedRouteAll allChildren = null;
            List<AssignedRouteAll> allChildrenRoute = new List<AssignedRouteAll>();
            try
            {

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetPickUpDropUpChildrenWithAssignedRoute";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                allChildren = new AssignedRouteAll
                                {
                                    PickUpStatus = dr["PickUpStatus"].ToString(),
                                    Address = dr["PickUpAddress"].ToString(),
                                    City = dr["PickUpCity"].ToString(),
                                    ZipCode = dr["PickUpZipCode"].ToString(),
                                    State = dr["PickUpSate"].ToString(),
                                    ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                    ChildrenName = dr["ChildName"].ToString(),
                                    PinLarge = "/images/pointer_green_big.png",
                                    PinSmall = "/images/pointer_green_small.png",
                                    PickUpRouteName = dr["PickUpRouteName"].ToString(),
                                    Latitude = Convert.ToDouble(dr["PickupLatitude"].ToString()),
                                    Longitude = Convert.ToDouble(dr["PickupLongitude"].ToString())
                                };
                                allChildrenRoute.Add(allChildren);
                                allChildren = new AssignedRouteAll
                                {
                                    DropStatus = dr["DropStatus"].ToString(),
                                    Address = dr["DropAddress"].ToString(),
                                    City = dr["DropCity"].ToString(),
                                    ZipCode = dr["DropZipCode"].ToString(),
                                    State = dr["DropState"].ToString(),
                                    ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                    ChildrenName = dr["ChildName"].ToString(),
                                    PinLarge = "/images/pointer_red_big.png",
                                    PinSmall = "/images/pointer_red_small.png",
                                    DropRouteName = dr["DropRouteName"].ToString(),
                                    Latitude = Convert.ToDouble(dr["DropLatitude"].ToString()),
                                    Longitude = Convert.ToDouble(dr["DropLongitude"].ToString())

                                };
                                allChildrenRoute.Add(allChildren);



                            }

                        }

                    }

                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
                Connection.Dispose();
            }
            return allChildrenRoute;

        }
        public BusStaff GetBusStaffData(out string CenterAddress, out List<CenterDetails> centerList, Guid agencyId)
        {

            List<BusDriver> busDriverlist = new List<BusDriver>();
            List<BusMonitor> busMonitorlist = new List<BusMonitor>();
            centerList = new List<CenterDetails>();
            BusDriver _busDriver = new BusDriver();
            BusMonitor _busMonitor = new BusMonitor();
            BusStaff _busStaff = new BusStaff();
            CenterDetails _centerDetails = new CenterDetails();
            CenterAddress = string.Empty;

            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetBusDriver_BusMonitor";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                _busDriver = new BusDriver
                                {
                                    BusDriverId = new Guid(dr["BusDriverId"].ToString()),
                                    BusDriverName = dr["BusDriverName"].ToString(),
                                    Enc_BusDriverId = EncryptDecrypt.Encrypt64(dr["BusDriverId"].ToString()),
                                    CenterId = Convert.ToInt64(dr["Center"]),

                                };
                                busDriverlist.Add(_busDriver);
                                CenterAddress = (CenterAddress == "") ? dr["CenterAddress"].ToString() : CenterAddress;
                            }

                        }

                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                            {
                                _busMonitor = new BusMonitor
                                {
                                    BusMonitorId = new Guid(dr1["BusMonitorId"].ToString()),
                                    BusMonitorName = dr1["BusMonitorName"].ToString(),
                                    Enc_BusMonitorId = EncryptDecrypt.Encrypt64(dr1["BusMonitorId"].ToString()),
                                    CenterId = Convert.ToInt64(dr1["Center"]),

                                };
                                busMonitorlist.Add(_busMonitor);
                            }

                        }
                        if (_dataset.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr2 in _dataset.Tables[2].Rows)
                            {
                                _centerDetails = new CenterDetails
                                {
                                    CenterId = Convert.ToInt64(dr2["CenterId"]),
                                    Enc_CenterId = EncryptDecrypt.Encrypt64(dr2["CenterId"].ToString()),
                                    CenterName = dr2["CenterName"].ToString()
                                };
                                centerList.Add(_centerDetails);

                            }
                        }

                    }


                    _busStaff.BusDriverList = busDriverlist;
                    _busStaff.BusMonitorList = busMonitorlist;
                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                Connection.Dispose();
            }
            return _busStaff;
        }

        public void GetRoutes(ref string PickUpRoutes, ref string DropRoutes, string AgencyId, int CenterId)
        {
            try
            {
                PickUpRoutes = "<option value='0'>Pickup Route</option>";
                DropRoutes = "<option value='0'>Drop Route</option>";
                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                    command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetCreateRouteDetails";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                   // Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                string optiontag = "<option value='##RouteId##'>##RouteName##</option>";
                                int RouteType = !string.IsNullOrEmpty(dr["RouteType"].ToString()) ? Convert.ToInt32(dr["RouteType"].ToString()) : 1;
                                string RouteId = !string.IsNullOrEmpty(dr["RouteId"].ToString()) ? dr["RouteId"].ToString() : "0";
                                string RouteName = !string.IsNullOrEmpty(dr["RouteName"].ToString()) ? dr["RouteName"].ToString() : "0";
                                optiontag = optiontag.Replace("##RouteId##", RouteId);
                                optiontag = optiontag.Replace("##RouteName##", RouteName);
                                if (RouteType == 1)
                                    PickUpRoutes += optiontag;
                                else
                                    DropRoutes += optiontag;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            finally
            {
                DataAdapter.Dispose();
                _dataset.Dispose();
                Connection.Dispose();
            }
        }

        public bool InsertRouteData(RouteDetails routeDetails)
        {
            bool isRowAffected = false;

            try
            {
                string TST = EncryptDecrypt.Decrypt64(routeDetails.BusDriverId);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@AgencyId", routeDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@BusDriverId", EncryptDecrypt.Decrypt64(routeDetails.BusDriverId)));
                command.Parameters.Add(new SqlParameter("@BusMonitorId", EncryptDecrypt.Decrypt64(routeDetails.BusMonitorId)));
                command.Parameters.Add(new SqlParameter("@RouteType", routeDetails.RouteType));
                command.Parameters.Add(new SqlParameter("@RouteName", routeDetails.RouteName));
                command.Parameters.Add(new SqlParameter("@BusName", routeDetails.BusName));
                command.Parameters.Add(new SqlParameter("@UserId", routeDetails.UserId));
                command.Parameters.Add(new SqlParameter("@RouteAddress", routeDetails.RouteAddress));
                command.Parameters.Add(new SqlParameter("@CenterId", routeDetails.CenterId));
                if (!string.IsNullOrEmpty(routeDetails.Enc_RouteId) && routeDetails.Enc_RouteId != "0")
                {
                    long RouteId = Convert.ToInt64(EncryptDecrypt.Decrypt64(routeDetails.Enc_RouteId));
                    command.Parameters.Add(new SqlParameter("@RouteId", RouteId));
                }
                command.CommandText = "USP_InsertRouteDetails";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);

            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return isRowAffected;

        }

        public List<RouteDetails> GetCreatedRouteData(Guid agencyId)
        {
            List<CreatedRoute> createdRoutList = new List<CreatedRoute>();

            // List<RouteDetails> listRouteDetails = new List<RouteDetails>();
            AssignedRouteChildren children = new AssignedRouteChildren();
            List<RouteDetails> routeDetailsList = new List<RouteDetails>();

            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetCreatedRouteInformation";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        //Route Creation
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr1 in _dataset.Tables[0].Rows)
                            {
                                RouteDetails routeDetails = new RouteDetails();
                                routeDetails.RouteName = dr1["RouteName"].ToString();
                                routeDetails.RouteAddress = dr1["RouteAddress"].ToString();
                                routeDetails.BusDriverName = dr1["BusDriverName"].ToString();
                                routeDetails.BusMonitorName = dr1["BusMonitorName"].ToString();
                                routeDetails.BusName = dr1["BusName"].ToString();
                                routeDetails.RouteId = Convert.ToInt64(dr1["RouteId"]);
                                routeDetails.RouteType = Convert.ToInt32(dr1["RouteType"]);
                                routeDetails.Enc_BusDriverId = EncryptDecrypt.Encrypt64(dr1["BusDriverId"].ToString());
                                routeDetails.Enc_BusMonitorId = EncryptDecrypt.Encrypt64(dr1["BusMonitorId"].ToString());
                                routeDetails.Enc_RouteId = EncryptDecrypt.Encrypt64(dr1["RouteId"].ToString());
                                List<AssignedRouteChildren> listassignedChildList = new List<AssignedRouteChildren>();
                                if (_dataset.Tables[1].Rows.Count > 0)
                                {

                                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                                    {
                                        if (Convert.ToInt64(dr["RouteId"]) == routeDetails.RouteId)
                                        {
                                            children = new AssignedRouteChildren();
                                            children.ChildrenName = dr["ChildrenName"].ToString();
                                            children.RouteAddress = dr["ClientAddress"].ToString();
                                            children.StopPosition = dr["Position"].ToString();
                                            children.ClientId = Convert.ToInt64(dr["ClientId"]);
                                            children.Enc_ClientId = EncryptDecrypt.Encrypt64(dr["ClientId"].ToString());
                                            listassignedChildList.Add(children);
                                        }

                                    }
                                }
                                routeDetails.ChildrenList = listassignedChildList;
                                routeDetailsList.Add(routeDetails);

                            }

                        }
                        //Route Assigned

                    }


                }

            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                Connection.Dispose();
            }
            return routeDetailsList;


        }


        public List<RouteDetails> GetCreatedRouteDataByCenter(Guid agencyId, long centerId)
        {
            List<CreatedRoute> createdRoutList = new List<CreatedRoute>();

            // List<RouteDetails> listRouteDetails = new List<RouteDetails>();
            AssignedRouteChildren children = new AssignedRouteChildren();
            List<RouteDetails> routeDetailsList = new List<RouteDetails>();

            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetCreatedRouteByCenter";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        //Route Creation
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr1 in _dataset.Tables[0].Rows)
                            {
                                RouteDetails routeDetails = new RouteDetails();
                                routeDetails.RouteName = dr1["RouteName"].ToString();
                                routeDetails.RouteAddress = dr1["RouteAddress"].ToString();
                                routeDetails.BusDriverName = dr1["BusDriverName"].ToString();
                                routeDetails.BusMonitorName = dr1["BusMonitorName"].ToString();
                                routeDetails.BusName = dr1["BusName"].ToString();
                                routeDetails.RouteId = Convert.ToInt64(dr1["RouteId"]);
                                routeDetails.RouteType = Convert.ToInt32(dr1["RouteType"]);
                                routeDetails.Enc_BusDriverId = EncryptDecrypt.Encrypt64(dr1["BusDriverId"].ToString());
                                routeDetails.Enc_BusMonitorId = EncryptDecrypt.Encrypt64(dr1["BusMonitorId"].ToString());
                                routeDetails.Enc_RouteId = EncryptDecrypt.Encrypt64(dr1["RouteId"].ToString());
                                List<AssignedRouteChildren> listassignedChildList = new List<AssignedRouteChildren>();
                                if (_dataset.Tables[1].Rows.Count > 0)
                                {

                                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                                    {
                                        if (Convert.ToInt64(dr["RouteId"]) == routeDetails.RouteId)
                                        {
                                            children = new AssignedRouteChildren();
                                            children.ChildrenName = dr["ChildrenName"].ToString();
                                            children.RouteAddress = dr["ClientAddress"].ToString();
                                            children.StopPosition = dr["Position"].ToString();
                                            children.ClientId = Convert.ToInt64(dr["ClientId"]);
                                            children.Enc_ClientId = EncryptDecrypt.Encrypt64(dr["ClientId"].ToString());
                                            listassignedChildList.Add(children);
                                        }

                                    }
                                }
                                routeDetails.ChildrenList = listassignedChildList;
                                routeDetailsList.Add(routeDetails);

                            }

                        }
                        //Route Assigned

                    }

                }


            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                Connection.Dispose();
            }
            return routeDetailsList;


        }

        public bool SaveAssignedRoute(string Clientid, string RouteId, string RouteType, string AgencyId, string UserId)
        {
            bool isUpdated = false;
            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();


                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_SaveAssignedRoute";
                    command.Parameters.AddWithValue("@ClientId", Clientid);
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@RouteId", RouteId);
                    command.Parameters.AddWithValue("@RouteType", RouteType);
                    Connection.Open();
                    int RowsAffected = command.ExecuteNonQuery();
                    Connection.Close();
                    if (RowsAffected > 0)
                        isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
            }
            return isUpdated;
        }

        public bool UpdatePosition(string Clientid, string RouteId, string AgencyId, int position)
        {
            bool isUpdated = false;
            try
            {
                using (Connection)
                {
                    command.Parameters.Clear();

                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_UpdatePosition";
                    command.Parameters.AddWithValue("@ClientId", EncryptDecrypt.Decrypt64(Clientid));
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@RouteId", EncryptDecrypt.Decrypt64(RouteId));
                    command.Parameters.AddWithValue("@Position", position);
                    Connection.Open();
                    int RowsAffected = command.ExecuteNonQuery();
                    Connection.Close();
                    if (RowsAffected > 0)
                        isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
            }
            return isUpdated;
        }
        public bool DeleteAssignedRoute(string Clientid, string RouteId, string AgencyId, string UserId)
        {
            bool isUpdated = false;
            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();


                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_DeleteAssignedRoute";
                    command.Parameters.AddWithValue("@ClientId", EncryptDecrypt.Decrypt64(Clientid));
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@RouteId", EncryptDecrypt.Decrypt64(RouteId));
                    Connection.Open();
                    int RowsAffected = command.ExecuteNonQuery();
                    Connection.Close();
                    if (RowsAffected > 0)
                        isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
            }
            return isUpdated;
        }
        public List<AssignedRouteAll> GetPickUpChildrenData(Guid agencyId, int CenterId)
        {
            AssignedRouteAll allChildren = null;
            List<AssignedRouteAll> allChildrenRoute = new List<AssignedRouteAll>();
            try
            {
                using (Connection)
                {

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetPickUpChildren";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            allChildrenRoute = (from DataRow dr in _dataset.Tables[0].Rows
                                                select new AssignedRouteAll
                                                {
                                                    PickUpStatus = dr["PickUpStatus"].ToString(),
                                                    Address = dr["PickUpAddress"].ToString(),
                                                    City = dr["PickUpCity"].ToString(),
                                                    ZipCode = dr["PickUpZipCode"].ToString(),
                                                    State = dr["PickUpSate"].ToString(),
                                                    ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                                    ChildrenName = dr["ChildName"].ToString(),
                                                    PinLarge = "/images/pointer_green_big.png",
                                                    PinSmall = "/images/pointer_green_small.png",
                                                    Latitude = Convert.ToDouble(dr["PickupLatitude"].ToString()),
                                                    Longitude = Convert.ToDouble(dr["PickupLongitude"].ToString()),
                                                    RouteId = Convert.ToInt32(dr["RouteId"]),
                                                    RouteType = Convert.ToInt32(dr["RouteType"])
                                                }
                                              ).ToList();

                        }

                    }

                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
                if (Connection != null)
                    Connection.Close();
                Connection.Dispose();
            }
            return allChildrenRoute;

        }

        public List<AssignedRouteAll> GetDropUpChildrenData(Guid agencyId, int CenterId)
        {
            AssignedRouteAll allChildren = null;
            List<AssignedRouteAll> dropChildrenList = new List<AssignedRouteAll>();
            try
            {
                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetDropUpChildren";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {

                                allChildren = new AssignedRouteAll
                                {
                                    DropStatus = dr["DropStatus"].ToString(),
                                    Address = dr["DropAddress"].ToString(),
                                    City = dr["DropCity"].ToString(),
                                    ZipCode = dr["DropZipCode"].ToString(),
                                    State = dr["DropState"].ToString(),
                                    ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                    ChildrenName = dr["ChildName"].ToString(),
                                    PinLarge = "/images/pointer_red_big.png",
                                    PinSmall = "/images/pointer_red_small.png",
                                    Latitude = Convert.ToDouble(dr["DropLatitude"].ToString()),
                                    Longitude = Convert.ToDouble(dr["DropLongitude"].ToString()),
                                    RouteId = Convert.ToInt32(dr["RouteId"]),
                                    RouteType = Convert.ToInt32(dr["RouteType"])

                                };
                                dropChildrenList.Add(allChildren);

                            }

                        }

                    }
                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                Connection.Dispose();
            }
            return dropChildrenList;

        }


        public List<AssignedRouteAll> GetChildrenRouteAssignedALL(Guid agencyId)
        {
            List<AssignedRouteAll> allList = new List<AssignedRouteAll>();
            AssignedRouteAll allRoute = new AssignedRouteAll();
            HouseholdContactAddress householdAddress = null;
            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetPickUpDropUpChildren";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                householdAddress = new HouseholdContactAddress
                                {
                                    HouseholdCity = dr["HouseholdCity"].ToString(),
                                    HouseholdAddress = dr["HouseholdAddress"].ToString(),
                                    HouseholdCounty = dr["HouseholdCounty"].ToString(),
                                    HouseholdState = dr["HouseholdState"].ToString(),
                                    HouseholdZipCode = dr["HouseHoldZipCode"].ToString(),
                                };

                                string dropUpStatus = dr["DropStatus"].ToString();
                                if (dropUpStatus == "1")
                                {
                                    allRoute = new AssignedRouteAll
                                    {
                                        DropStatus = dropUpStatus,
                                        Address = householdAddress.HouseholdAddress,
                                        City = householdAddress.HouseholdCity,
                                        ZipCode = householdAddress.HouseholdZipCode,
                                        State = householdAddress.HouseholdState,
                                        ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                        ChildrenName = dr["ChildName"].ToString(),
                                        PinLarge = "/images/pointer_red_big.png",
                                        PinSmall = "/images/pointer_red_small.png"
                                    };
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                Connection.Dispose();
            }
            return allList;
        }
        public bool AcceptRejectTransportRequest(string Clientid, string yakkrId, string Status, string AgencyId, string UserId)
        {
            bool isUpdated = false;
            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_AcceptRejectTransportationRequest";
                    command.Parameters.AddWithValue("@ChildId", Clientid);
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@YakkrId", yakkrId);
                    command.Parameters.AddWithValue("@Status", Status);
                    Connection.Open();

                    int RowsAffected = command.ExecuteNonQuery();
                    Connection.Close();

                    if (RowsAffected > 0)
                        isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
            }
            return isUpdated;
        }


        public bool InsertTransportationyakkr(string Clientid, string yakkrId, string AgencyId, string UserId, string ClassRoomId, string CenterId)
        {
            bool isUpdated = false;
            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();


                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_SaveTransportationYakkr";
                    command.Parameters.AddWithValue("@ClientId", Clientid);
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@ClassroomId", ClassRoomId);
                    command.Parameters.AddWithValue("@CenterId", EncryptDecrypt.Decrypt64(CenterId));
                    Connection.Open();
                    int RowsAffected = command.ExecuteNonQuery();
                    Connection.Close();
                    if (RowsAffected > 0)
                        isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
            }
            return isUpdated;
        }

        public bool InsertTransportationyakkrForEnrolledChild(string Clientid, string AgencyId, string UserId)
        {
            bool isUpdated = false;
            try
            {
                using (Connection)
                {

                    command.Parameters.Clear();


                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_SaveTransportationYakkrForEnrolledChild";
                    command.Parameters.AddWithValue("@ClientId", Clientid);
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    Connection.Open();
                    int RowsAffected = command.ExecuteNonQuery();
                    Connection.Close();
                    if (RowsAffected > 0)
                        isUpdated = true;
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return isUpdated;
        }
        public void GetTranportationandHouseholdDetails(ref DataSet dtTransportation, string AgencyId, string ClientId)
        {
            dtTransportation = new DataSet();
            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                    command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetTransportationandHouseholdDetails";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    DataAdapter.Fill(dtTransportation);
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
            }
        }
        public void GetChildDetails(ref DataSet dtTransportation, string AgencyId, string ClientId)
        {
            dtTransportation = new DataSet();
            try
            {
                using (Connection)
                {

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64(ClientId)));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetChildDetails";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    DataAdapter.Fill(dtTransportation);
                    Connection.Close();


                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
            }
        }

        public List<AssignedStaff> GetAssignedRouteInfo(long centerId, Guid agencyId, Guid userId, int routeType)
        {
            List<AssignedStaff> stafflist = new List<AssignedStaff>();
            List<AssignedStaff> stafflist2 = new List<AssignedStaff>();
            try
            {
                AssignedStaff staff = new AssignedStaff();

                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    command.Parameters.Add(new SqlParameter("@RouteType", routeType));
                    command.Parameters.Add(new SqlParameter("@CenterId", centerId));

                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetAssignedRouteByCenter";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                staff = new AssignedStaff();
                                staff.BusDriverName = dr["BusDriveName"].ToString();
                                staff.BusMonitorName = dr["BusMonitorName"].ToString();
                                staff.ClientId = dr["ClientId"].ToString();
                                staff.RouteId = Convert.ToInt32(dr["RouteId"]);
                                staff.RouteName = dr["RouteName"].ToString();
                                stafflist.Add(staff);
                            }
                        }

                        var routIdlist = stafflist.Select(x => x.RouteId).Distinct().ToList();
                        foreach (var item in routIdlist)
                        {
                            staff = new AssignedStaff();
                            var countId = stafflist.Where(x => x.RouteId == item).Count();
                            staff = stafflist.Where(x => x.RouteId == item).ToList()[0];
                            staff.NoOfStops = countId.ToString();
                            stafflist2.Add(staff);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
            }
            return stafflist2;
        }


        public BusStaff GetBusStaffByCenter(Guid agencyId, long centerId)
        {

            List<BusDriver> busDriverlist = new List<BusDriver>();
            List<BusMonitor> busMonitorlist = new List<BusMonitor>();

            BusDriver _busDriver = new BusDriver();
            BusMonitor _busMonitor = new BusMonitor();
            BusStaff _busStaff = new BusStaff();
            CenterDetails _centerDetails = new CenterDetails();


            try
            {
                using (Connection)
                {

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetBusStaffByCenter";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                _busDriver = new BusDriver
                                {
                                    BusDriverId = new Guid(dr["BusDriverId"].ToString()),
                                    BusDriverName = dr["BusDriverName"].ToString(),
                                    Enc_BusDriverId = EncryptDecrypt.Encrypt64(dr["BusDriverId"].ToString()),
                                    CenterId = Convert.ToInt64(dr["Center"]),

                                };
                                busDriverlist.Add(_busDriver);

                            }

                        }

                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                            {
                                _busMonitor = new BusMonitor
                                {
                                    BusMonitorId = new Guid(dr1["BusMonitorId"].ToString()),
                                    BusMonitorName = dr1["BusMonitorName"].ToString(),
                                    Enc_BusMonitorId = EncryptDecrypt.Encrypt64(dr1["BusMonitorId"].ToString()),
                                    CenterId = Convert.ToInt64(dr1["Center"]),

                                };
                                busMonitorlist.Add(_busMonitor);
                            }

                        }
                        if (_dataset.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr2 in _dataset.Tables[2].Rows)
                            {
                                _centerDetails = new CenterDetails
                                {
                                    CenterId = Convert.ToInt64(dr2["CenterId"]),
                                    Enc_CenterId = EncryptDecrypt.Encrypt64(dr2["CenterId"].ToString()),
                                    CenterName = dr2["CenterName"].ToString(),
                                    CenterAddress = dr2["CenterAddress"].ToString()
                                };

                            }
                        }

                    }
                    _busStaff.BusDriverList = busDriverlist;
                    _busStaff.BusMonitorList = busMonitorlist;
                    _busStaff.Center = _centerDetails;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                Connection.Dispose();
            }
            return _busStaff;
        }


        public List<AssignedRouteAll> GetChildrenRouteAssignedDetailByCenter(Guid agencyId, long centerId)
        {

            AssignedRouteAll allChildren = null;
            List<AssignedRouteAll> allChildrenRoute = new List<AssignedRouteAll>();
            try
            {
                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    if (centerId != 0)
                    {
                        command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                        command.CommandText = "USP_GetPickUpDropUpChildrenWithAssignedRoute_Center";

                    }
                    else
                    {
                        command.CommandText = "USP_GetPickUpDropUpChildrenWithAssignedRoute";
                    }
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                allChildren = new AssignedRouteAll
                                {
                                    PickUpStatus = dr["PickUpStatus"].ToString(),
                                    Address = dr["PickUpAddress"].ToString(),
                                    City = dr["PickUpCity"].ToString(),
                                    ZipCode = dr["PickUpZipCode"].ToString(),
                                    State = dr["PickUpSate"].ToString(),
                                    ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                    ChildrenName = dr["ChildName"].ToString(),
                                    PinLarge = "/images/pointer_green_big.png",
                                    PinSmall = "/images/pointer_green_small.png",
                                    PickUpRouteName = dr["PickUpRouteName"].ToString(),
                                    Latitude = Convert.ToDouble(dr["PickupLatitude"].ToString()),
                                    Longitude = Convert.ToDouble(dr["PickupLongitude"].ToString())
                                };
                                allChildrenRoute.Add(allChildren);
                                allChildren = new AssignedRouteAll
                                {
                                    DropStatus = dr["DropStatus"].ToString(),
                                    Address = dr["DropAddress"].ToString(),
                                    City = dr["DropCity"].ToString(),
                                    ZipCode = dr["DropZipCode"].ToString(),
                                    State = dr["DropState"].ToString(),
                                    ClientId = Convert.ToInt64(dr["ClientId"].ToString()),
                                    ChildrenName = dr["ChildName"].ToString(),
                                    PinLarge = "/images/pointer_red_big.png",
                                    PinSmall = "/images/pointer_red_small.png",
                                    DropRouteName = dr["DropRouteName"].ToString(),
                                    Latitude = Convert.ToDouble(dr["DropLatitude"].ToString()),
                                    Longitude = Convert.ToDouble(dr["DropLongitude"].ToString())

                                };
                                allChildrenRoute.Add(allChildren);

                            }

                        }

                    }
                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                Connection.Dispose();
            }
            return allChildrenRoute;

        }
    }
}
