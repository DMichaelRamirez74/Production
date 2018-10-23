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
    public class FacilitiesData
    {
        SqlConnection _connection { get; set; }
        SqlCommand command { get; set; }
        SqlDataAdapter DataAdapter { get; set; }
      //  SqlTransaction transaction { get; set; }
        SqlDataAdapter dataAdapter { get; set; }
        DataSet _dataset { get; set; }

        public FacilitiesData()
        {
            this._connection = connection.returnConnection();
            this.command = new SqlCommand();
            //  this.transaction = null;
            this.dataAdapter = new SqlDataAdapter();
            this. _dataset = new DataSet();
        }
        public AssignFacilityStaff GetFacilityStaffList(int yakkrid,string Agencyid="")
        {

            AssignFacilityStaff facilitiesModel = new AssignFacilityStaff();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", Agencyid));
                    command.Parameters.Add(new SqlParameter("@YakkrId", yakkrid));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_GetFacilityStaffList";
                    dataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    dataAdapter.Fill(_dataset);
                    if(_dataset.Tables[0]!=null&& _dataset.Tables[0].Rows.Count>0)
                    {
                      
                        facilitiesModel.CenterName = _dataset.Tables[0].Rows[0]["CenterName"].ToString();
                        facilitiesModel.CenterAddress = _dataset.Tables[0].Rows[0]["Address"].ToString();
                        facilitiesModel.yakkrdescription = _dataset.Tables[0].Rows[0]["yakkrdescription"].ToString();
                        facilitiesModel.DURL = _dataset.Tables[0].Rows[0]["ImageOfDamage"].ToString() == "" ? "" : _dataset.Tables[0].Rows[0]["ImageOfDamage"].ToString();
                        facilitiesModel.YakkrCode = _dataset.Tables[0].Rows[0]["yakkrcode"].ToString();
                        facilitiesModel.StaffName = _dataset.Tables[0].Rows[0]["staffname"].ToString();
                        facilitiesModel.UserDescrption= _dataset.Tables[0].Rows[0]["UserDescription"].ToString();
                        facilitiesModel.RoleName = _dataset.Tables[0].Rows[0]["RoleName"].ToString();
                        //facilitiesModel.RoleName1 = _dataset.Tables[0].Rows[0]["RoleName1"].ToString();
                        facilitiesModel.StaffName1 = _dataset.Tables[0].Rows[0]["StaffName1"].ToString();
                        facilitiesModel.workorderSequence = _dataset.Tables[0].Rows[0]["workorderSequence"].ToString();
                        facilitiesModel.WorkOrderNumber = _dataset.Tables[0].Rows[0]["WorkOrderNumber"].ToString();
                        facilitiesModel.ClassroomName = _dataset.Tables[0].Rows[0]["ClassroomName"].ToString();
                        facilitiesModel.WorkOrderDate = (DBNull.Value == _dataset.Tables[0].Rows[0]["WorkOrderDate"]) ? "--" : Convert.ToDateTime(_dataset.Tables[0].Rows[0]["WorkOrderDate"]).ToString("MM/dd/yyyy");
                        facilitiesModel.RequestedDate = (DBNull.Value == _dataset.Tables[0].Rows[0]["RequestedDate"]) ? "--" : Convert.ToDateTime(_dataset.Tables[0].Rows[0]["RequestedDate"]).ToString("MM/dd/yyyy");

                    }
                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {

                            facilitiesModel.StaffList = (from DataRow dr5 in _dataset.Tables[1].Rows
                                                         select new SelectListItem
                                                         {
                                                             Text = dr5["name"].ToString(),
                                                             Value = dr5["userid"].ToString()
                                                         }).ToList();
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
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return facilitiesModel;
        }


        public AssignFacilityStaff AssignToFaciltyStaff( AssignFacilityStaff AssignWork , string yakkrid, string Agencyid ,string userid)
        {

            AssignFacilityStaff facilitiesModel = new AssignFacilityStaff();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                    _connection.Open();
                    command.Parameters.Clear();
                     command.Parameters.Add(new SqlParameter("@AgencyId", Agencyid));
                      command.Parameters.Add(new SqlParameter("@UserID", userid));
                    command.Parameters.Add(new SqlParameter("@ExternalAddress", AssignWork.ExternalAddress));
                    command.Parameters.Add(new SqlParameter("@ExternalEmailId", AssignWork.ExternalEmailId));
                    command.Parameters.Add(new SqlParameter("@ExternCompanyName", AssignWork.ExternCompanyName));
                    command.Parameters.Add(new SqlParameter("@ExternalContactName", AssignWork.ExternalContactName));
                    command.Parameters.Add(new SqlParameter("@ExternalContactNo", AssignWork.ExternalContactNo));
                    command.Parameters.Add(new SqlParameter("@IsInternal", AssignWork.IsInternal));
                    command.Parameters.Add(new SqlParameter("@InternalAssignTo", AssignWork.InternalAssignTo));
                    command.Parameters.Add(new SqlParameter("@yakkrid", yakkrid));
                    command.Parameters.Add(new SqlParameter("@EmailRequest", AssignWork.Request));
                    command.Parameters.Add(new SqlParameter("@workordernumber", AssignWork.workorderSequence));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_AssignWorkToFaciltyStaff";
                    // int res=command.ExecuteNonQuery();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            
                            facilitiesModel.StaffContact = _dataset.Tables[0].Rows[0]["cellnumber"].ToString();
                            facilitiesModel.StaffEmailaddress = _dataset.Tables[0].Rows[0]["emailaddress"].ToString();
                            facilitiesModel.StaffName = _dataset.Tables[0].Rows[0]["name"].ToString();
                            facilitiesModel.RoleName = _dataset.Tables[0].Rows[0]["RoleName"].ToString();
                            facilitiesModel.UserDescrption= _dataset.Tables[0].Rows[0]["UserDescrption"].ToString();
                            facilitiesModel.CenterName = _dataset.Tables[0].Rows[0]["CenterName"].ToString();
                            facilitiesModel.CenterAddress = _dataset.Tables[0].Rows[0]["Address"].ToString();
                            facilitiesModel.ClassroomName = _dataset.Tables[0].Rows[0]["ClassroomName"].ToString();
                        }
                    }
                    //if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    //{

                    //    facilitiesModel.Subject = (_dataset.Tables[1].Rows[0]["subject"] == DBNull.Value) ? "" : _dataset.Tables[1].Rows[0]["subject"].ToString();
                    //    facilitiesModel.Body = (_dataset.Tables[1].Rows[0]["emailbody"] == DBNull.Value) ? "" : _dataset.Tables[1].Rows[0]["emailbody"].ToString();

                    //    facilitiesModel.SenderName = (_dataset.Tables[1].Rows[0]["SenderName"] == DBNull.Value) ? "" : _dataset.Tables[1].Rows[0]["SenderName"].ToString();
                    //    facilitiesModel.SenderRole = (_dataset.Tables[1].Rows[0]["SenderRole"] == DBNull.Value) ? "" : _dataset.Tables[1].Rows[0]["SenderRole"].ToString();
                    //    facilitiesModel.SenderPhone = (_dataset.Tables[1].Rows[0]["SenderPhone"] == DBNull.Value) ? "" : _dataset.Tables[1].Rows[0]["SenderPhone"].ToString();

                    //}

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return facilitiesModel;
        }
        public List<PartDetails> AutoCompletePartDetails(string term)
        {
            List<PartDetails> PartList = new List<PartDetails>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "AutoComplete_PartDetailsList";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PartName", term);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PartDetails obj = new PartDetails();
                            obj.PartNumber = Convert.ToString(dr["PartNumber"].ToString());
                            obj.PartDescription = dr["PartDescription"].ToString();
                            obj.UnitCost = dr["UnitCost"].ToString();
                            PartList.Add(obj);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return PartList;
        }

        public AssignFacilityStaff SaveFacilityStaff(AssignFacilityStaff AssignWork, string yakkrid, string Agencyid, string userid)
        {

            AssignFacilityStaff facilitiesModel = new AssignFacilityStaff();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                    _connection.Open();
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", Agencyid));
                    command.Parameters.Add(new SqlParameter("@UserID", userid));
                    command.Parameters.Add(new SqlParameter("@IsTemp", AssignWork.IsTemporaryFix));
                    command.Parameters.Add(new SqlParameter("@LabourHours", AssignWork.LaborHours));
                    command.Parameters.Add(new SqlParameter("@MilesDriven", AssignWork.MilesDriven));
                    command.Parameters.Add(new SqlParameter("@Notes", AssignWork.Notes));

                    command.Parameters.Add(new SqlParameter("@yakkrid", yakkrid));

                    if (AssignWork.PartDetails != null && AssignWork.PartDetails.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("PartNumber", typeof(string)),
                    new DataColumn("Quantity",typeof(string)),
                    new DataColumn("UnitCost",typeof(string)),
                    new DataColumn("TotalCost",typeof(string)),
                      new DataColumn("PartDescription",typeof(string)),
                    });
                        foreach (PartDetails part in AssignWork.PartDetails)
                        {
                            if (part.PartNumber != null && part.Quantity != null)
                            {
                                dt.Rows.Add(part.PartNumber, part.Quantity, part.UnitCost, part.TotalCost, part.PartDescription);
                            }
                        }
                        command.Parameters.Add(new SqlParameter("@PartDetails", dt));

                    }

                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_SaveFacilityStaffWorkStatus";
                     int res=command.ExecuteNonQuery();
                   

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return facilitiesModel;
        }
        public FacilitesModel GetFacilitiesModelDashboard(StaffDetails details,bool iscenterMgr=false)
        {

            FacilitesModel facilitiesModel = new FacilitesModel();
            facilitiesModel.FacilitiesDashboardList = new List<FacilitiesManagerDashboard>();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@userId", details.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@isCenterManager", iscenterMgr));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    // command.CommandText = "USP_FacilitiesManagerDashboard";
                    command.CommandText = "USP_FacilitiesManagerDashboardCount";
                    dataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    dataAdapter.Fill(_dataset);
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            facilitiesModel.FacilitiesDashboardList = (from DataRow dr in _dataset.Tables[0].Rows

                                                                       select new FacilitiesManagerDashboard
                                                                       {
                                                                           CenterId = Convert.ToInt64(dr["CenterId"]),
                                                                           Enc_CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                                                                           CenterName = dr["CenterName"].ToString(),
                                                                           OpenedWorkOrders = Convert.ToInt64(dr["OpenedWorkOrders"]),
                                                                           InternalAssigned = Convert.ToInt64(dr["InternalAssignedWorkOrders"]),
                                                                           ExternalAssigned = Convert.ToInt64(dr["ExternalAssignedWorkOrders"]),
                                                                           CompletedWorkOrders = Convert.ToInt64(dr["CompletedWorkOrders"]),
                                                                           TemporarilyFixedWorkOrders = Convert.ToInt64(dr["TemporarilyFixed"]),
                                                                           AssignedHimself = Convert.ToInt64(dr["AssignedHimself"])

                                                                       }).ToList();

                        }

                    }
                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {

                            facilitiesModel.StaffList = (from DataRow dr5 in _dataset.Tables[1].Rows
                                                         select new SelectListItem
                                                         {
                                                             Text = dr5["name"].ToString(),
                                                             Value = dr5["userid"].ToString()
                                                         }).ToList();
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
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return facilitiesModel;
        }
        public FacilitesModel GetFacilitiesStaffDashboard(StaffDetails details)
        {

            FacilitesModel facilitiesModel = new FacilitesModel();
            facilitiesModel.FacilitiesDashboardList = new List<FacilitiesManagerDashboard>();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@userId", details.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    // command.CommandText = "USP_FacilitiesManagerDashboard";
                    command.CommandText = "USP_FacilitiesStaffDashboardCount";
                    dataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    dataAdapter.Fill(_dataset);
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            facilitiesModel.FacilitiesDashboardList = (from DataRow dr in _dataset.Tables[0].Rows

                                                                       select new FacilitiesManagerDashboard
                                                                       {
                                                                           CenterId = Convert.ToInt64(dr["CenterId"]),
                                                                           Enc_CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                                                                           CenterName = dr["CenterName"].ToString(),
                                                                           OpenedWorkOrders = Convert.ToInt64(dr["OpenedWorkOrders"]),

                                                                           CompletedWorkOrders = Convert.ToInt64(dr["CompletedWorkOrders"]),
                                                                           TemporarilyFixedWorkOrders = Convert.ToInt64(dr["TemporarilyFixed"])                                                                 
                                                                       }).ToList();

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
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return facilitiesModel;
        }

        public List<AssignFacilityStaff> GetWorkOrderStatusList(string CenterId, string Type, string AgencyId, string UserId,bool IsCenterManager)
        {

            List<AssignFacilityStaff> facilitiesModelList = new List<AssignFacilityStaff>();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@Type", Type));
                command.Parameters.Add(new SqlParameter("@CenterId", (CenterId)));
                    command.Parameters.Add(new SqlParameter("@IsCenterManager", (IsCenterManager)));
                    command.Connection = _connection;
                command.CommandType = CommandType.StoredProcedure;               
                command.CommandText = "SP_GetWorkOrderStatusList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                AssignFacilityStaff facilitiesModel = new AssignFacilityStaff();
                                facilitiesModel.YakkrId = dr["YakkrId"].ToString();
                               facilitiesModel.FacilityId = dr["FacilityId"].ToString();
                                facilitiesModel.yakkrdescription = dr["Description"].ToString();
                                facilitiesModel.UserDescription = dr["UserDescription"].ToString();
                                 facilitiesModel.YakkrCode = dr["yakkrcode"].ToString();
                                facilitiesModel.StaffName = dr["staffname"].ToString();
                                facilitiesModel.workorderSequence= dr["WorkOrderNumber"].ToString();
                                facilitiesModel.ImageCount = dr["ImageCount"].ToString();
                                //  facilitiesModel.RequestedDate = (DBNull.Value == dr["RequestedDate"]) ? "--" : Convert.ToDateTime(_dataset.Tables[0].Rows[0]["RequestedDate"]).ToString("MM/dd/yyyy");
                                facilitiesModel.RequestedDate = (DBNull.Value == dr["RequestedDate"]) ? "--" : Convert.ToDateTime(dr["RequestedDate"]).ToString("MM/dd/yyyy");

                                if (Type=="3")
                                {
                                    //                                    facilitiesModel.WorkOrderDate = (DBNull.Value == dr["WorkOrderDate"]) ? "--" : Convert.ToDateTime(_dataset.Tables[0].Rows[0]["WorkOrderDate"]).ToString("MM/dd/yyyy");\
                                    facilitiesModel.WorkOrderDate = (DBNull.Value == dr["WorkOrderDate"]) ? "--" : Convert.ToDateTime(dr["WorkOrderDate"]).ToString("MM/dd/yyyy");

                                    facilitiesModel.StaffName1 = _dataset.Tables[0].Rows[0]["staffname1"].ToString();

                                }
                                if (Type=="2")
                                {
                                    facilitiesModel.ExternCompanyName = dr["ExternalCompanyName"].ToString();
                                    facilitiesModel.ExternalEmailId = dr["ExternalCompanyEmailID"].ToString();
                                    facilitiesModel.ExternalAddress = dr["ExternalCompanyAddress"].ToString();

                                    facilitiesModel.ExternalContactNo = dr["ExternalContactNo"].ToString();
                                    // facilitiesModel.EstimatedDate = (DBNull.Value == dr["EstimatedDate"]) ? "--" : Convert.ToDateTime(_dataset.Tables[0].Rows[0]["EstimatedDate"]).ToString("MM/dd/yyyy");
                                    facilitiesModel.EstimatedDate = (DBNull.Value == dr["EstimatedDate"]) ? "--" : Convert.ToDateTime(dr["EstimatedDate"]).ToString("MM/dd/yyyy");
                                    facilitiesModel.EstimatedTime = dr["EstimatedTime"].ToString();

                                }
                      
                                facilitiesModelList.Add(facilitiesModel);
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
            }
            return facilitiesModelList;
        }

        public List<AssignFacilityStaff> GetStaffWorkOrderStatusList(string CenterId, string Type, string AgencyId, string UserId)
        {

            List<AssignFacilityStaff> facilitiesModelList = new List<AssignFacilityStaff>();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                    command.Parameters.Add(new SqlParameter("@userid", UserId));
                    command.Parameters.Add(new SqlParameter("@Type", Type));
                    command.Parameters.Add(new SqlParameter("@CenterId", (CenterId)));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_GetStaffWorkOrderStatusList";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                AssignFacilityStaff facilitiesModel = new AssignFacilityStaff();
                               facilitiesModel.workorderSequence= dr["WorkOrderNumber"].ToString();
                                facilitiesModel.YakkrId = dr["YakkrId"].ToString();
                                facilitiesModel.FacilityId = dr["FacilityId"].ToString();
                                facilitiesModel.yakkrdescription = dr["Description"].ToString();
                                facilitiesModel.YakkrCode = dr["yakkrcode"].ToString();
                                facilitiesModel.StaffName = dr["staffname"].ToString();
                                //facilitiesModel.RoleName = dr["RoleName"].ToString();
                                facilitiesModel.RequestedDate = (DBNull.Value == dr["RequestedDate"]) ? "--" : Convert.ToDateTime(_dataset.Tables[0].Rows[0]["RequestedDate"]).ToString("MM/dd/yyyy");
                               // facilitiesModel.Notes = dr["notes"].ToString();
                                facilitiesModelList.Add(facilitiesModel);
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
            }
            return facilitiesModelList;
        }

        public List<AssignFacilityStaff> AutoCompleteExternalFacility(string term)
        {
            List<AssignFacilityStaff> staffList = new List<AssignFacilityStaff>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "AutoComplete_ExternalFacility";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CmpyName", term);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AssignFacilityStaff obj = new AssignFacilityStaff();
                            obj.ExternCompanyName = Convert.ToString(dr["ExternalCompanyName"]);
                            obj.ExternalEmailId = dr["ExternalCompanyemailid"].ToString();
                            obj.ExternalAddress = dr["ExternalCompanyAddress"].ToString();
                            obj.ExternalContactName = dr["ExternalContactName"].ToString();
                            obj.ExternalContactNo = dr["ExternalContactNo"].ToString();

                            staffList.Add(obj);
                        }
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return staffList;
        }

        public bool UpdateEstimatedTime(string facilityid, string EstimateDate, string EstimatedHours,string userid)
        {
            bool result = false;
            AssignFacilityStaff facilitiesModel = new AssignFacilityStaff();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                    _connection.Open();
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@facilityid", facilityid));
                    command.Parameters.Add(new SqlParameter("@UserID", userid));
                    command.Parameters.Add(new SqlParameter("@EstimateDate", EstimateDate));
                    command.Parameters.Add(new SqlParameter("@EstimatedHours", EstimatedHours));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_UpdateEstimatedDateTime";
                    int res = command.ExecuteNonQuery();
                    if (res >= 1)
                        result = true;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return result;
        }


        public bool AddDamageFixedImage(string yakkrid,List<DamageFixedImages> imageList, string userid)
        {
            bool result = false;
            AssignFacilityStaff facilitiesModel = new AssignFacilityStaff();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                    _connection.Open();
                    command.Parameters.Add(new SqlParameter("@UserID", userid));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_AddDamageFixedImage";

                    command.Parameters.Add(new SqlParameter("@yakkrid", yakkrid));

                    if (imageList != null && imageList.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("FileName", typeof(string)),
                    new DataColumn("FileExtension",typeof(string)),
                    new DataColumn("ImageByte",typeof(byte[])),
                    
                    });
                        foreach (DamageFixedImages image in imageList)
                        {
                            if (image.FileExtension != null && image.ImageByte != null)
                            {
                                dt.Rows.Add(image.FileName,image.FileExtension,image.ImageByte);
                            }
                        }
                        command.Parameters.Add(new SqlParameter("@ImageList", dt));

                    }

                    int res = command.ExecuteNonQuery();
                    if (res >= 1)
                        result = true;


                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return result;
        }

        public List<string> LoadCarouselImages(string workid)
        {
            List<string> ImageList = new List<string>();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                    _connection.Open();
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_GetDamageFixedImageByWorkID";
                    command.Parameters.Add(new SqlParameter("@workid", workid));
                  
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                string picture = "";
                                picture = dr["ImageByteArray"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ImageByteArray"]);
                                ImageList.Add(picture);
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
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return ImageList;
        }

        public List<PartDetails> PartDetailsList(string workid)
        {
            List<PartDetails> PartDetailsList = new List<PartDetails>();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                    _connection.Open();
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.CommandText = "SP_GetWorkOrderParts";
                    command.Parameters.Add(new SqlParameter("@workid", workid));
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                PartDetails part = new FingerprintsModel.PartDetails();
                                part.PartNumber= dr["PartNumber"].ToString();
                                part.Quantity= dr["Quantity"].ToString();
                                part.TotalCost ="$"+ dr["TotalCost"].ToString();
                                part.UnitCost = "$" + dr["unitcost"].ToString();
                                part.PartDescription = dr["PartDescription"].ToString();
                                  PartDetailsList.Add(part);
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
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return PartDetailsList;
        }
        public AssignFacilityStaff GetWorkOrderDetail(string yakkrid,string orderid)
        {

            AssignFacilityStaff facilitiesModel = new AssignFacilityStaff();
            try
            {
                using (_connection)
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@orderid", orderid));
                    command.Parameters.Add(new SqlParameter("@yakkrid", yakkrid));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_GetWorkOrderDetail";
                    dataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    dataAdapter.Fill(_dataset);
                    if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                    {

                        facilitiesModel.CenterName = _dataset.Tables[0].Rows[0]["CenterName"].ToString();
                        facilitiesModel.StaffName = _dataset.Tables[0].Rows[0]["Staffname"].ToString();
                       facilitiesModel.workorderSequence = _dataset.Tables[0].Rows[0]["WorkOrderNumber"].ToString();
                        facilitiesModel.RequestedDate = (DBNull.Value == _dataset.Tables[0].Rows[0]["RequestedDate"]) ? "--" : Convert.ToDateTime(_dataset.Tables[0].Rows[0]["RequestedDate"]).ToString("MM/dd/yyyy");
                          facilitiesModel.UserDescription = _dataset.Tables[0].Rows[0]["UserDescription"].ToString();
                        facilitiesModel.IsTemporaryFix = _dataset.Tables[0].Rows[0]["FixType"].ToString();
                        facilitiesModel.AssignedTo = _dataset.Tables[0].Rows[0]["AssignedTo"].ToString();
                                         
                    }
                    facilitiesModel.workOrderList = new List<AssignFacilityStaff>();
                    if (_dataset.Tables[1] != null)
                    {
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[1].Rows)
                            {
                                AssignFacilityStaff work = new AssignFacilityStaff();
                                work.IsTemporaryFix = dr["FixType"].ToString();
                                work.AssignedTo =dr["AssignedTo"].ToString();
                                work.WorkId =Convert.ToInt32( dr["WorkId"]);
                                work.Notes= dr["Notes"].ToString();
                                work.StaffName= dr["Staffname"].ToString();
                                work.RequestedDate = (DBNull.Value == dr["RequestedDate"]) ? "--" : Convert.ToDateTime(dr["RequestedDate"]).ToString("MM/dd/yyyy");
                                work.PartDetails = new List<PartDetails>();
                                work.PartDetails = PartDetailsList(dr["WorkId"].ToString());
                                work.ImageCount= dr["ImageCount"].ToString();
                                facilitiesModel.workOrderList.Add(work);
                          
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
                dataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return facilitiesModel;
        }
    }
}
