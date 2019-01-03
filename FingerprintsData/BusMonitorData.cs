using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FingerprintsModel;
using System.Web.Mvc;

namespace FingerprintsData
{
    public class BusMonitorData
    {

        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;

        public BusMonitorAnalysisList GetBusMonitorData(BusMonitorAnalysis busmonitor)
        {

            List<BusMonitorAnalysis> busmonitorList = new List<BusMonitorAnalysis>();
            BusMonitorAnalysisList getbusmonitorList = new BusMonitorAnalysisList();
            BusMonitorAnalysis busmonitorData = null;
            BusMonitorAnalysisTotal objTotal = new BusMonitorAnalysisTotal();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", busmonitor.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", busmonitor.UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetBusMonitor";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            busmonitorData = new BusMonitorAnalysis();                         
                            busmonitorData.RouteName = dr["RouteName"].ToString();
                            busmonitorData.CenterId= Convert.ToInt64(dr["CenterId"]);
                           // busmonitorData.Riders = Convert.ToInt32(dr["RidersCount"]);
                            busmonitorData.PickUp = Convert.ToInt32(dr["PickUpCount"]);
                            busmonitorData.Drop = Convert.ToInt32(dr["DropCount"]);
                            busmonitorData.RouteType = Convert.ToInt32(dr["RouteType"]);                         
                            busmonitorList.Add(busmonitorData);
                        }
                    }

                    if (busmonitorList.Count() > 0)
                    {
                        
                      //  objTotal.RidersTotal = busmonitorList.Sum(x => x.Riders);
                        objTotal.PickUpTotal = busmonitorList.Sum(x => x.PickUp);
                        objTotal.DropOffTotal = busmonitorList.Sum(x => x.Drop);
                    }
                    else
                    {
                        
                      //  objTotal.RidersTotal = 0;
                        objTotal.PickUpTotal = 0;
                        objTotal.DropOffTotal = 0;
                    }

                    getbusmonitorList.ListBusMonitor = busmonitorList;
                    getbusmonitorList.AnalysisTotal = objTotal;

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
            }
            return getbusmonitorList;

        }

        public List<BusRiderChildrens> GetBusRiderChildrensData(Guid agencyId, long centerId,string routetype)
        {
            List<BusRiderChildrens> childrenList = new List<BusRiderChildrens>();
            List<BusRiderChildrens> childrenList2 = new List<BusRiderChildrens>();
            BusRiderChildrens childrens = null;
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                command.Parameters.Add(new SqlParameter("@RouteType",Convert.ToInt32(routetype)));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetBusRidersChildren";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            childrens = new BusRiderChildrens();

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
                        BusRiderChildrens children2 = null;
                        var list2 = childrenList.Select(x => x.ClientId).Distinct();
                        foreach (var item in list2)
                        {
                            children2 = new BusRiderChildrens();
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

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return childrenList2;

        }
        //public List<AssignedStaffBusMonitor> GetBusAssignedRouteInfo(long centerId, Guid agencyId, Guid userId, int routeType)
        //{
        //    List<AssignedStaffBusMonitor> stafflist = new List<AssignedStaffBusMonitor>();
        //    List<AssignedStaffBusMonitor> stafflist2 = new List<AssignedStaffBusMonitor>();
        //    try
        //    {
        //        AssignedStaffBusMonitor staff = new AssignedStaffBusMonitor();

        //        command.Parameters.Clear();
        //        command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
        //        command.Parameters.Add(new SqlParameter("@UserId", userId));
        //        command.Parameters.Add(new SqlParameter("@RouteType", routeType));
        //        command.Parameters.Add(new SqlParameter("@CenterId", centerId));

        //        command.Connection = Connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "USP_GetBusAssignedRouteByCenter";
        //        DataAdapter = new SqlDataAdapter(command);
        //        _dataset = new DataSet();
        //        DataAdapter.Fill(_dataset);
        //        if (_dataset.Tables[0] != null)
        //        {
        //            if (_dataset.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in _dataset.Tables[0].Rows)
        //                {
        //                    staff = new AssignedStaffBusMonitor();
        //                    staff.BusDriverName = dr["BusDriveName"].ToString();
        //                    staff.BusMonitorName = dr["BusMonitorName"].ToString();
        //                    staff.ClientId = dr["ClientId"].ToString();
        //                    staff.RouteId = Convert.ToInt32(dr["RouteId"]);
        //                    staff.RouteName = dr["RouteName"].ToString();
        //                    stafflist.Add(staff);
        //                }
        //            }

        //            var routIdlist = stafflist.Select(x => x.RouteId).Distinct().ToList();
        //            foreach (var item in routIdlist)
        //            {
        //                staff = new AssignedStaffBusMonitor();
        //                var countId = stafflist.Where(x => x.RouteId == item).Count();
        //                staff = stafflist.Where(x => x.RouteId == item).ToList()[0];
        //                staff.NoOfStops = countId.ToString();
        //                stafflist2.Add(staff);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    finally
        //    {
        //        if (Connection != null)
        //            Connection.Close();
        //    }
        //    return stafflist2;
        //}
    }
}
