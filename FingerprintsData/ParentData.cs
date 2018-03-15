using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsData
{
    public class ParentData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataTable _dataTable = null;
        DataSet _dataset = null;

        //public void GetParentDetails(ref DataTable dtParentAndChildDetails, string EmailId, ref string ProfilePic)
        //{
        //    try
        //    {
        //        dtParentAndChildDetails = new DataTable();
        //        command.Connection = Connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new SqlParameter("@EmailId", EmailId));
        //        command.CommandText = "SP_GetParentAndChildDetailByEmail";
        //        DataAdapter = new SqlDataAdapter(command);
        //        DataAdapter.Fill(dtParentAndChildDetails);
        //        if (dtParentAndChildDetails.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtParentAndChildDetails.Rows)
        //            {
        //                if (dr["ParentId"].ToString() == "False")
        //                    ProfilePic = dr["ProfilePicture"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePicture"]);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    finally
        //    {
        //        command.Dispose();
        //    }
        //}

        //public void GetChildDetails(ref DataSet dsChildDetails, string ClientId, ref string ProfilePic)
        //{
        //    dsChildDetails = new DataSet();
        //    try
        //    {
        //        command.Connection = Connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new SqlParameter("@ClientId", Convert.ToInt64(ClientId)));
        //        command.CommandText = "SP_GetChildDetailByClientId";
        //        DataAdapter = new SqlDataAdapter(command);
        //        DataAdapter.Fill(dsChildDetails);
        //        if (dsChildDetails.Tables[0].Rows.Count > 0)
        //        {
        //            ProfilePic = dsChildDetails.Tables[0].Rows[0]["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dsChildDetails.Tables[0].Rows[0]["ProfilePic"]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    finally
        //    {
        //        command.Dispose();
        //    }
        //}


        public void GetParentDetails(ref DataTable dtParentAndChildDetails, string EmailId, ref string ProfilePic)
        {

            try
            {
                dtParentAndChildDetails = new DataTable();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@EmailId", EmailId));
                command.CommandText = "SP_GetParentAndChildDetailByEmail";
                DataAdapter = new SqlDataAdapter(command);
                DataSet _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                // DataAdapter.Fill(dtParentAndChildDetails);

                if (_dataset.Tables[0] != null)
                {
                    dtParentAndChildDetails = _dataset.Tables[0];
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            if (dr["ParentId"].ToString() == "False")
                                ProfilePic = dr["ProfilePicture"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePicture"]);
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
                command.Dispose();
            }

        }

        public List<SelectListItem> GetChildDetails(ref DataSet dsChildDetails, string ClientId, ref string ProfilePic, string agencyid,ref int IsMarkAbsent,ref bool IsLateArrival)
        {
            dsChildDetails = new DataSet();
            List<SelectListItem> AbsenceReasonList = new List<SelectListItem>();
            try
            {
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ClientId", Convert.ToInt64(ClientId)));
                command.Parameters.Add(new SqlParameter("@AgencyID", agencyid));
                command.CommandText = "SP_GetChildDetailByClientId";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dsChildDetails);
                if (dsChildDetails.Tables[0].Rows.Count > 0)
                {
                    ProfilePic = dsChildDetails.Tables[0].Rows[0]["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dsChildDetails.Tables[0].Rows[0]["ProfilePic"]);
                    IsMarkAbsent= dsChildDetails.Tables[0].Rows[0]["IsMarkedAbsent"].ToString() == "" ? 0 : Convert.ToInt32(dsChildDetails.Tables[0].Rows[0]["IsMarkedAbsent"]);
                    IsLateArrival = dsChildDetails.Tables[0].Rows[0]["IsLateArrival"].ToString() == "" ? false: Convert.ToBoolean(dsChildDetails.Tables[0].Rows[0]["IsLateArrival"]);
                }
                if (dsChildDetails.Tables[1] != null)
                {
                    if (dsChildDetails.Tables[1].Rows.Count > 0)
                    {
                        AbsenceReasonList = (from DataRow dr5 in dsChildDetails.Tables[1].Rows
                                             select new SelectListItem
                                             {
                                                 Text = dr5["absenseReason"].ToString(),
                                                 Value = dr5["reasonid"].ToString()
                                             }).ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
            }
            return AbsenceReasonList;
        }

        public void GetScreenResults(ref DataTable dtChildDetails, string ClientId, string UserId, string AgencyId)
        {
            dtChildDetails = new DataTable();
            try
            {
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.Parameters.Add(new SqlParameter("@userid", new Guid(UserId)));
                command.Parameters.Add(new SqlParameter("@agencyid", new Guid(AgencyId)));
                command.CommandText = "SP_GetScreeningDetails";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtChildDetails);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {

                command.Dispose();
            }
        }

        public bool AddParentAddressChange(ParentAddressChange AddressDetails, YakkrRouting yakkr, string Email)
        {
            bool isRowAffected = false;
            try
            {
                DateTime dt = Convert.ToDateTime(AddressDetails.DateOfChange);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Address", AddressDetails.Address));
                command.Parameters.Add(new SqlParameter("@City", AddressDetails.City));
                command.Parameters.Add(new SqlParameter("@State", AddressDetails.State));
                command.Parameters.Add(new SqlParameter("@Zipcode", AddressDetails.ZipCode));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", AddressDetails.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@DateOfChange", dt));
                command.Parameters.Add(new SqlParameter("@AgencyID", yakkr.AgencyID));
                command.Parameters.Add(new SqlParameter("@UserId", yakkr.UserID));
                command.Parameters.Add(new SqlParameter("@Email", Email));
                command.CommandText = "SP_ParentAddressChange";
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


        public bool AddParentMessage(ParentStatus message, string AgencyId, string ToStaffId)
        {
            bool isRowAffected = false;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", message.UserId));
                command.Parameters.Add(new SqlParameter("@Message", message.Status));
                command.Parameters.Add(new SqlParameter("@ClientId", message.ClientId));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", message.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@RouteCode", message.RouteCode));
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyId));
                command.Parameters.Add(new SqlParameter("@ToStaffId", ToStaffId));
                command.CommandText = "USP_AddMessage";
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

        public bool EducationStatusChange(StatusChange status, string AgencyId)
        {
            bool isRowAffected = false;
            try
            {
                DateTime dt = Convert.ToDateTime(status.Date);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                command.Parameters.Add(new SqlParameter("@MilitaryStatus", status.Status));
                command.Parameters.Add(new SqlParameter("@Notes", status.Notes));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@DateOfChange", dt));
                command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyId));
                command.CommandText = "USP_EducationStatusChange";
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
        
        public bool ParentStatusChange(ParentStatus status)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                if (status.days != null)
                {
                    foreach (var day in status.days.TrimStart(',').TrimEnd(',').Split(','))
                    {
                        command = new SqlCommand();
                        command.Connection = Connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                        command.Parameters.Add(new SqlParameter("@Status", Convert.ToBoolean(status.Status)));
                        command.Parameters.Add(new SqlParameter("@DayOfVolunteer", day));
                        command.Parameters.Add(new SqlParameter("@Hours", status.Hours));
                        command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                        command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                        command.Parameters.Add(new SqlParameter("@Command", status.RouteCode));
                        command.CommandText = "SP_ParentStatusChange";
                        int RowsAffected = command.ExecuteNonQuery();
                        if (RowsAffected > 0)
                            isRowAffected = true;
                    }
                }
                else
                {
                    command = new SqlCommand();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                    command.Parameters.Add(new SqlParameter("@Status", status.Status));
                    command.Parameters.Add(new SqlParameter("@Hours", status.Hours));
                    command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                    command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                    command.Parameters.Add(new SqlParameter("@Command", status.RouteCode));
                    if (status.RouteCode == "70")
                        command.Parameters.Add(new SqlParameter("@AbsentStatus", status.Status));
                    command.CommandText = "SP_ParentStatusChange";
                    int RowsAffected = command.ExecuteNonQuery();
                    if (RowsAffected > 0)
                        isRowAffected = true;
                }

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

        public bool MilitaryStatusChange(StatusChange status, string AgencyId)
        {
            bool isRowAffected = false;
            try
            {
                DateTime dt = Convert.ToDateTime(status.Date);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                command.Parameters.Add(new SqlParameter("@MilitaryStatus", status.Status));
                command.Parameters.Add(new SqlParameter("@Notes", status.Notes));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@DateOfChange", dt));
                command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyId));
                command.CommandText = "USP_MilitaryStatusChange";
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

        public bool HomelessStatusChange(StatusChange status, string AgencyId, string Email)
        {
            bool isRowAffected = false;
            try
            {
                DateTime dt = Convert.ToDateTime(status.Date);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                command.Parameters.Add(new SqlParameter("@HomelessStatus", status.Status));
                command.Parameters.Add(new SqlParameter("@Notes", status.Notes));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@DateOfChange", dt));
                command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyId));
                command.Parameters.Add(new SqlParameter("@Email", Email));
                command.CommandText = "USP_HomelessStatusChange";
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

        public bool MarkAbsentStatus(StatusChange status, string AgencyId, string Email)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                command.Parameters.Add(new SqlParameter("@NewReason", status.NewReason));
                command.Parameters.Add(new SqlParameter("@AbsentStatus", status.Status));
                command.Parameters.Add(new SqlParameter("@Notes", status.Notes));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyId));
                command.Parameters.Add(new SqlParameter("@LateTime", status.Time));
                command.Parameters.Add(new SqlParameter("@IsLateArrival", status.IsLateArrival));
                command.CommandText = "USP_MarkAbsentStatus";
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

        public bool EmploymentStatusChange(StatusChange status, string AgencyId)
        {
            bool isRowAffected = false;
            try
            {
                DateTime dt = Convert.ToDateTime(status.Date);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                command.Parameters.Add(new SqlParameter("@EmploymentStatus", status.Status));
                command.Parameters.Add(new SqlParameter("@Notes", status.Notes));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@DateOfChange", dt));
                command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyId));
                command.CommandText = "USP_EmploymentStatusChange";
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

        public bool StatusChange(StatusChange status)
        {
            bool isRowAffected = false;
            try
            {
                DateTime dt = Convert.ToDateTime(status.Date);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                command.Parameters.Add(new SqlParameter("@EmploymentStatus", status.Status));
                command.Parameters.Add(new SqlParameter("@Notes", status.Notes));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@DateOfChange", dt));
                command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                command.Parameters.Add(new SqlParameter("@Command", status.RouteCode));
                command.CommandText = "SP_StatusChange";
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

        public bool VolunteerRequest(Volunteer objVol, string AgencyId, string Email)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", objVol.UserId));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", objVol.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@ClientId", objVol.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyId));
                command.Parameters.Add(new SqlParameter("@Email", Email));
                command.CommandText = "USP_AddRouting_VolunteerRequest";
                object yakkr = command.ExecuteScalar();
                if (yakkr != null)
                {
                    Int64? yakkrid = Convert.ToInt64(yakkr);

                    if (objVol.days != null)
                    {
                        if (objVol.days.Count() > 0)
                        {
                            foreach (var day in objVol.days.TrimStart(',').TrimEnd(',').Split(','))
                            {
                                command.CommandText = "USP_VolunteerRequest";
                                command.Parameters.Clear();
                                command.Parameters.Add(new SqlParameter("@UserId", objVol.UserId));
                                command.Parameters.Add(new SqlParameter("@HouseHoldId", objVol.HouseHoldId));
                                command.Parameters.Add(new SqlParameter("@ClientId", objVol.ClientId));
                                command.Parameters.Add(new SqlParameter("@VolunteerStatus", Convert.ToBoolean(objVol.Status)));
                                command.Parameters.Add(new SqlParameter("@DayOfVolunteer", day));
                                command.Parameters.Add(new SqlParameter("@HoursOfVolunteer", objVol.Hours));
                                command.Parameters.Add(new SqlParameter("@YakkrId", yakkrid));
                                command.Parameters.Add(new SqlParameter("@Email", Email));
                                int RowsAffected = command.ExecuteNonQuery();
                                if (RowsAffected > 0)
                                    isRowAffected = true;
                            }
                        }


                    }
                    else
                    {
                        command.CommandText = "USP_VolunteerRequest";
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@UserId", objVol.UserId));
                        command.Parameters.Add(new SqlParameter("@HouseHoldId", objVol.HouseHoldId));
                        command.Parameters.Add(new SqlParameter("@ClientId", objVol.ClientId));
                        command.Parameters.Add(new SqlParameter("@VolunteerStatus", Convert.ToBoolean(objVol.Status)));
                        command.Parameters.Add(new SqlParameter("@YakkrId", yakkrid));
                        command.Parameters.Add(new SqlParameter("@Email", Email));
                        int RowsAffected = command.ExecuteNonQuery();
                        if (RowsAffected > 0)
                            isRowAffected = true;
                    }
                }

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


        public bool ParentEngagementRequest(ParentEngagement status, string AgencyId)
        {
            bool isRowAffected = false;
            try
            {
                DateTime dt = Convert.ToDateTime(status.Date);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyId));
                command.CommandText = "USP_ADDRouing_ParentEngagementStatus";
                object yakkr = command.ExecuteScalar();
                if (yakkr != null)
                {
                    Int64 yakkrid = Convert.ToInt64(yakkr);
                    foreach (var actId in status.ActivityId.TrimStart(',').TrimEnd(',').Split(','))
                    {
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@UserId", status.UserId));
                        command.Parameters.Add(new SqlParameter("@AgenctyId", new Guid(AgencyId)));
                        command.Parameters.Add(new SqlParameter("@ClientId", status.ClientId));
                        command.Parameters.Add(new SqlParameter("@EngagementDate", dt));
                        command.Parameters.Add(new SqlParameter("@ActivityId", actId));
                        command.Parameters.Add(new SqlParameter("@Hours", status.Hours));
                        command.Parameters.Add(new SqlParameter("@Minutes", status.Minutes));
                        command.Parameters.Add(new SqlParameter("@Notes", status.Notes));
                        command.Parameters.Add(new SqlParameter("@HouseHoldId", status.HouseHoldId));
                        command.Parameters.Add(new SqlParameter("@YakkrId", yakkrid));
                        command.CommandText = "USP_ParentEngagementStatus";
                        int RowsAffected = command.ExecuteNonQuery();
                        if (RowsAffected > 0)
                            isRowAffected = true;
                    }
                }

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
        public string GetHouseHoldByEmail(string EmailId)
        {
            string HouseHold = "";
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@EmailId", EmailId));
                command.CommandText = "SP_GetHouseHoldIdByEmailId";
                Object HouseHoldId = command.ExecuteScalar();
                if (HouseHoldId != null)
                    HouseHold = HouseHoldId.ToString();
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
            return HouseHold;
        }
        public bool UpdateAddressChange(string HouseHoldId, string UserId, string YakkrId)
        {
            bool isRowAffected = false;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@UserId", new Guid(UserId)));
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.CommandText = "SP_UpdateParentAddress";
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

        public bool UpdateStatusChange(string HouseHoldId, string UserId, string RouteCode, string ClientId, string YakkrID)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@RoutCode", Convert.ToInt32(RouteCode)));
                command.Parameters.Add(new SqlParameter("@UserId", new Guid(UserId)));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                if (!string.IsNullOrEmpty(YakkrID))
                    command.Parameters.Add(new SqlParameter("@YakkrID", Convert.ToInt64(YakkrID)));
                command.CommandText = "SP_UpdateParentStatus";
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

        public bool ApproveEmploymentRequest(string YakkrID)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@YakkrID", YakkrID));
                command.CommandText = "USP_ApproveEmploymentStaus";
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

        public bool ApproveMilitaryRequest(string YakkrID)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@YakkrID", YakkrID));
                command.CommandText = "USP_ApproveMilitrayStaus";
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

        public bool ApproveHomelessRequest(string YakkrID)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@YakkrID", YakkrID));
                command.CommandText = "USP_ApproveHomelessStaus";
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

        public bool ApproveAbsentRequest(string YakkrID)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@YakkrID", YakkrID));
                command.CommandText = "USP_ApproveAbsentRequest";
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

        public bool ApproveParentEngagemnetRequest(string YakkrID)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@YakkrID", YakkrID));
                command.CommandText = "USP_ApproveParentEngagementRequest";
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

        public bool ApproveVolunteerRequest(string YakkrID)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@YakkrID", YakkrID));
                command.CommandText = "USP_ApproveVolunteerStaus";
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
        public bool ApproveEducationRequest(string YakkrID)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@YakkrID", YakkrID));
                command.CommandText = "USP_ApproveEducationStaus";
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

        public bool ClassroomOpenrequestApproval(string UserId, string RouteCode, string Notes, string YakkrID)
        {
            bool isRowAffected = false;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RoutCode", Convert.ToInt32(RouteCode)));
                command.Parameters.Add(new SqlParameter("@UserId", new Guid(UserId)));
                command.Parameters.Add(new SqlParameter("@Notes", Notes));
                command.Parameters.Add(new SqlParameter("@YakkrID", Convert.ToInt64(YakkrID)));
                command.CommandText = "SP_ClassroomOpenApproval";
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

        public string[] GetAddressByHouseHold(string YakkrId, string HouseHoldId)
        {
            string[] Address = new string[2];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@YakkrId", Convert.ToInt64(YakkrId)));
                command.CommandText = "GetAddressByHouseHoldId";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Address[0] = _dataset.Tables[0].Rows[0][0].ToString();
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        Address[1] = _dataset.Tables[1].Rows[0][0].ToString();
                    }
                }
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
            return Address;
        }
        public string[] GetEmploymentStatus(string HouseHoldId, string ClientId, string YakkrId)
        {
            string[] EmploymentStatus = new string[3];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.CommandText = "USP_GetEmployementStatus";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        EmploymentStatus[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Working" : "Not Working";
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        EmploymentStatus[1] = _dataset.Tables[1].Rows[0][0].ToString() == "1" ? "Working" : "Not Working";
                        EmploymentStatus[2] = !string.IsNullOrEmpty(_dataset.Tables[1].Rows[0][1].ToString()) ? _dataset.Tables[1].Rows[0][1].ToString() : "";
                    }
                }
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
            return EmploymentStatus;
        }

        public string[] GetMilitaryStatus(string HouseHoldId, string ClientId, string YakkrId)
        {
            string[] MilitaryStatus = new string[3];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.CommandText = "USP_GetMilitaryStatus";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        if (_dataset.Tables[0].Rows[0][0].ToString() == "1")
                        {
                            MilitaryStatus[0] = "None";
                        }
                        else if (_dataset.Tables[0].Rows[0][0].ToString() == "2")
                        {
                            MilitaryStatus[0] = "Active";
                        }
                        else if (_dataset.Tables[0].Rows[0][0].ToString() == "3")
                        {
                            MilitaryStatus[0] = "Veteran";
                        }
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        if (_dataset.Tables[1].Rows[0][0].ToString() == "1")
                        {
                            MilitaryStatus[1] = "None";
                        }
                        else if (_dataset.Tables[1].Rows[0][0].ToString() == "2")
                        {
                            MilitaryStatus[1] = "Active";
                        }
                        else if (_dataset.Tables[1].Rows[0][0].ToString() == "3")
                        {
                            MilitaryStatus[1] = "Veteran";
                        }
                        MilitaryStatus[2] = !string.IsNullOrEmpty(_dataset.Tables[1].Rows[0][1].ToString()) ? _dataset.Tables[1].Rows[0][1].ToString() : "";
                    }
                }
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
            return MilitaryStatus;
        }

        public string[] GetHomelessStatus(string HouseHoldId, string ClientId, string YakkrId)
        {
            string[] HomelessStatus = new string[3];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.CommandText = "USP_GetHomelessStatus";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        HomelessStatus[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Found Housing" : "Homeless";
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        HomelessStatus[1] = _dataset.Tables[1].Rows[0][0].ToString() == "1" ? "Found Housing" : "Homeless";
                        HomelessStatus[2] = !string.IsNullOrEmpty(_dataset.Tables[1].Rows[0][1].ToString()) ? _dataset.Tables[1].Rows[0][1].ToString() : "";
                    }
                }
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
            return HomelessStatus;
        }

        public string[] GetAbsentRequest(string YakkrId)
        {
            string[] AbsentStatus = new string[2];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.CommandText = "USP_GetAbsentRequest";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                       // AbsentStatus[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Sick" : _dataset.Tables[0].Rows[0][0].ToString() == "2" ? "Vacation" : _dataset.Tables[0].Rows[0][0].ToString() == "3" ? "Funeral" : "";
                        AbsentStatus[0] = _dataset.Tables[0].Rows[0][0].ToString();
                    }
                }
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
            return AbsentStatus;
        }

        public string[] GetParentEngagementRequest(string YakkrId)
        {
            string[] Details = new string[6];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.CommandText = "USP_GetParentEngagementRequest";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {

                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Details[0] = _dataset.Tables[0].Rows[0]["EngagementDate"].ToString();
                        Details[1] = _dataset.Tables[0].Rows[0]["Hours"].ToString();
                        Details[2] = _dataset.Tables[0].Rows[0]["Minutes"].ToString();
                        Details[3] = _dataset.Tables[0].Rows[0]["Notes"].ToString();
                        Details[4] = _dataset.Tables[0].Rows[0]["ActivityId"].ToString() == "1" ? "Reading" : _dataset.Tables[0].Rows[0]["ActivityId"].ToString() == "2" ? "Writing" : "";
                    }
                    if (_dataset.Tables[0].Rows.Count == 2)
                    {
                        string activity = Details[4].ToString();
                        string activity_1 = _dataset.Tables[0].Rows[1]["ActivityId"].ToString() == "1" ? "Reading" : _dataset.Tables[0].Rows[1]["ActivityId"].ToString() == "2" ? "Writing" : "";
                        activity = activity + "," + activity_1;
                        Details[4] = activity;
                    }
                }
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
            return Details;
        }
        public string[] GetParentMessages(string YakkrId)
        {
            string[] Messages = new string[1];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.CommandText = "SP_GetParentMessage";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Messages[0] = _dataset.Tables[0].Rows[0]["Message"].ToString();
                    }
                }
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
            return Messages;
        }


        public string[] GetEducationStatus(string HouseHoldId, string ClientId, string YakkrId)
        {
            string[] EducationStatus = new string[3];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.CommandText = "USP_GetEducationStatus";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        EducationStatus[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Advanced Degree or Baccalaureate" : _dataset.Tables[0].Rows[0][0].ToString() == "2" ? "Associate, Vocational, or some College" : _dataset.Tables[0].Rows[0][0].ToString() == "3" ? "High School Graduate or GED" : _dataset.Tables[0].Rows[0][0].ToString() == "4" ? "Less than High school" : "";
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        EducationStatus[1] = _dataset.Tables[1].Rows[0][0].ToString() == "1" ? "Advanced Degree or Baccalaureate" : _dataset.Tables[1].Rows[0][0].ToString() == "2" ? "Associate, Vocational, or some College" : _dataset.Tables[1].Rows[0][0].ToString() == "3" ? "High School Graduate or GED" : _dataset.Tables[1].Rows[0][0].ToString() == "4" ? "Less than High school" : "";
                        EducationStatus[2] = !string.IsNullOrEmpty(_dataset.Tables[1].Rows[0][1].ToString()) ? _dataset.Tables[1].Rows[0][1].ToString() : "";
                    }
                }
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
            return EducationStatus;
        }

        public string[] GetVolunteerRequest(string HouseHoldId, string ClientId, string YakkrId)
        {
            string[] VolunteerRequest = new string[4];
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.CommandText = "USP_GetVolunteerRequest";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        VolunteerRequest[0] = _dataset.Tables[0].Rows[0][0].ToString() == "True" ? "Stopped" : "Started";
                        VolunteerRequest[1] = _dataset.Tables[0].Rows[0][0].ToString() == "True" ? "Started" : "Stopped";
                        string Days = "";
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            string day = dr[1].ToString() == "1" ? "Monday" : dr[1].ToString() == "2" ? "Tuesday" : dr[1].ToString() == "3" ? "Wednesday" : dr[1].ToString() == "4" ? "Thursday" : dr[1].ToString() == "5" ? "Friday" : dr[1].ToString() == "6" ? "Saturday" : "";
                            Days = Days + "," + day;
                        }
                        VolunteerRequest[2] = Days.TrimStart(',').TrimEnd(',');
                        VolunteerRequest[3] = _dataset.Tables[0].Rows[0][2].ToString();
                    }
                }
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
            return VolunteerRequest;
        }

        public string[] GetStatus(string HouseHoldId, string RoutCode, string Email, string ClientId, string UserId)
        {
            string[] Address = new string[5];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@Command", RoutCode));
                command.Parameters.Add(new SqlParameter("@EmailId", Email));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.CommandText = "SP_GetStatus";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (RoutCode == "091")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            Address[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Working" : "Not Working";
                        }
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            Address[1] = _dataset.Tables[1].Rows[0][0].ToString() == "1" ? "Working" : "Not Working";
                        }
                    }
                    if (RoutCode == "092")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            if (_dataset.Tables[0].Rows[0][0].ToString() == "1")
                            {
                                Address[0] = "None";
                            }
                            else if (_dataset.Tables[0].Rows[0][0].ToString() == "2")
                            {
                                Address[0] = "Active";
                            }
                            else if (_dataset.Tables[0].Rows[0][0].ToString() == "3")
                            {
                                Address[0] = "Veteran";
                            }

                        }
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            if (_dataset.Tables[1].Rows[0][0].ToString() == "1")
                            {
                                Address[1] = "None";
                            }
                            else if (_dataset.Tables[1].Rows[0][0].ToString() == "2")
                            {
                                Address[1] = "Active";
                            }
                            else if (_dataset.Tables[1].Rows[0][0].ToString() == "3")
                            {
                                Address[1] = "Veteran";
                            }
                        }
                    }
                    if (RoutCode == "093")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            Address[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Found Housing" : "Homeless";
                        }
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            Address[1] = _dataset.Tables[1].Rows[0][0].ToString() == "1" ? "Found Housing" : "Homeless";
                        }
                    }
                    if (RoutCode == "094")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            Address[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Advanced Degree or Baccalaureate" : _dataset.Tables[0].Rows[0][0].ToString() == "2" ? "Associate, Vocational, or some College" : _dataset.Tables[0].Rows[0][0].ToString() == "3" ? "High School Graduate or GED" : _dataset.Tables[0].Rows[0][0].ToString() == "4" ? "Less than High school" : "";
                        }
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            Address[1] = _dataset.Tables[1].Rows[0][0].ToString() == "1" ? "Advanced Degree or Baccalaureate" : _dataset.Tables[1].Rows[0][0].ToString() == "2" ? "Associate, Vocational, or some College" : _dataset.Tables[1].Rows[0][0].ToString() == "3" ? "High School Graduate or GED" : _dataset.Tables[1].Rows[0][0].ToString() == "4" ? "Less than High school" : "";
                        }
                    }
                    if (RoutCode == "080")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            Address[0] = _dataset.Tables[0].Rows[0][0].ToString() == "True" ? "Stopped" : "Started";
                            Address[1] = _dataset.Tables[0].Rows[0][0].ToString() == "True" ? "Started" : "Stopped";
                        }

                    }
                    if (RoutCode == "264")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            Address[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Sick" : _dataset.Tables[0].Rows[0][0].ToString() == "2" ? "Vacation" : _dataset.Tables[0].Rows[0][0].ToString() == "3" ? "Funeral" : "";
                            Address[1] = _dataset.Tables[0].Rows[0][1].ToString();
                        }

                    }
                    if (RoutCode == "265")
                    {
                        string[] Details = new string[6];
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            Details[0] = _dataset.Tables[0].Rows[0]["EngagementDate"].ToString();
                            Details[1] = _dataset.Tables[0].Rows[0]["Hours"].ToString();
                            Details[2] = _dataset.Tables[0].Rows[0]["Minutes"].ToString();
                            Details[3] = _dataset.Tables[0].Rows[0]["Notes"].ToString();
                            Details[4] = _dataset.Tables[0].Rows[0]["ActivityId"].ToString() == "1" ? "Reading" : _dataset.Tables[0].Rows[0]["ActivityId"].ToString() == "2" ? "Writing" : "";
                        }
                        if (_dataset.Tables[0].Rows.Count == 2)
                        {
                            string activity = Details[4].ToString();
                            string activity_1 = _dataset.Tables[0].Rows[1]["ActivityId"].ToString() == "1" ? "Reading" : _dataset.Tables[0].Rows[1]["ActivityId"].ToString() == "2" ? "Writing" : "";
                            activity = activity + "," + activity_1;
                            Details[4] = activity;
                        }
                        return Details;
                    }
                    if (RoutCode == "41" || RoutCode == "42" || RoutCode == "43")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            Address[0] = _dataset.Tables[0].Rows[0]["Message"].ToString();

                        }

                    }
                    if (RoutCode == "267")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            Address[0] = _dataset.Tables[0].Rows[0]["AlertMessage"].ToString();

                        }
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            Address[1] = _dataset.Tables[1].Rows[0]["Teacher"].ToString();
                            Address[2] = _dataset.Tables[1].Rows[0]["CenterID"].ToString()+"$" + _dataset.Tables[1].Rows[0]["ClassroomID"].ToString()+"$"+ _dataset.Tables[1].Rows[0]["Classroom"].ToString();
                            Address[4] = _dataset.Tables[1].Rows[0]["Reason"].ToString().Trim();
                        }
                        Address[3] = UserId;
                    }
                    if (RoutCode == "292")
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            Address[0] = _dataset.Tables[0].Rows[0]["Name"].ToString();
                            Address[1] = _dataset.Tables[0].Rows[0]["Notes"].ToString();
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
                Connection.Close();
                command.Dispose();
            }
            return Address;
        }


        public void GetClassRoomReqStatus(ref DataTable dtCenters, string userId,string agencyId,string yakkId,string routecode,string date)
        {
            dtCenters = new DataTable();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@YakkrId", yakkId));
                command.Parameters.Add(new SqlParameter("@Routecode", routecode));
                command.Parameters.Add(new SqlParameter("@Date", date));
                command.CommandText = "USP_GetStatusForClassRoomClose";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(dtCenters);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
        }
        public string[] GetParentStatus(string HouseHoldId, String Email)
        {
            string[] Status = new string[3];

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@EmailId", Email));
                command.CommandText = "SP_GetParentStatus";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Status[0] = _dataset.Tables[0].Rows[0][0].ToString() == "1" ? "Working" : "Not Working";
                        if (_dataset.Tables[0].Rows[0][1].ToString() == "1")
                            Status[1] = "None";
                        else if (_dataset.Tables[0].Rows[0][1].ToString() == "2")
                            Status[1] = "Active";
                        else if (_dataset.Tables[0].Rows[0][1].ToString() == "3")
                            Status[1] = "Veteran";
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        Status[2] = _dataset.Tables[1].Rows[0][0].ToString() == "1" ? "Found Housing" : "Homeless";
                    }
                }
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
            return Status;
        }


        public DisabilityChildren GetDisabilityChildrenData(string yakkrId, string HouseHoldId, string ClientId, string agencyId, string userId, int mode, string isAccepted)
        {

            DisabilityChildren children = new DisabilityChildren();
            DisabilityChildren.DisbilityChild child = new DisabilityChildren.DisbilityChild();
            List<DisabilityChildren.DisbilityChild> childList = new List<DisabilityChildren.DisbilityChild>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@HouseHoldId", Convert.ToInt64(HouseHoldId)));
                command.Parameters.Add(new SqlParameter("@YakkrId", Convert.ToInt64(yakkrId)));
                command.Parameters.Add(new SqlParameter("@ClientId", Convert.ToInt64(ClientId)));
                command.Parameters.Add(new SqlParameter("@AgencyId", new Guid(agencyId)));
                command.Parameters.Add(new SqlParameter("@UserId", new Guid(userId)));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@IsAccepted", (isAccepted == "1") ? true : false));
                command.CommandText = "USP_DisabilityChildrenProcess";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (mode == 1) //yakkr code=300
                {
                    if (_dataset != null)
                    {
                        if (_dataset.Tables[0] != null)
                        {
                            if (_dataset.Tables[0].Rows.Count > 0)
                            {

                                children.DisabilityChildInfo = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                                select new DisabilityChildren.DisbilityChild
                                                                {
                                                                    ClientId = Convert.ToInt64(dr1["ClientId"]),
                                                                    ChildName = dr1["ChildName"].ToString(),
                                                                    Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString()),
                                                                    RouteCode = dr1["RouteCode"].ToString(),
                                                                    YakkrId = Convert.ToInt64(dr1["YakkrId"].ToString()),
                                                                    DateOfBirth = Convert.ToDateTime(dr1["DOB"]).ToString("MM/dd/yyyy"),
                                                                    CenterName = dr1["CenterName"].ToString(),
                                                                    CenterId = EncryptDecrypt.Encrypt64(dr1["CenterId"].ToString()),
                                                                    ClassRoomId = dr1["ClassRoomId"].ToString(),
                                                                    ProgramId = EncryptDecrypt.Encrypt64(dr1["ProgramId"].ToString()),
                                                                    FswName = dr1["fswname"].ToString(),
                                                                    ClassRoomName = dr1["classRoomName"].ToString(),
                                                                    SchoolDistrict = dr1["district"].ToString(),
                                                                    Gender = dr1["Gender"].ToString() == "1" ? "Male" : dr1["Gender"].ToString() == "2" ? "Female" : "Others"

                                                                }).Distinct().ToList()[0];
                            }
                        }
                    }
                }
                if (mode == 2)//yakkr code=301 
                {
                    if (_dataset != null)
                    {
                        if (_dataset.Tables[0] != null)
                        {
                            if (_dataset.Tables[0].Rows.Count > 0)
                            {

                                childList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                             select new DisabilityChildren.DisbilityChild
                                             {
                                                 ClientId = Convert.ToInt64(dr1["ClientId"]),
                                                 ChildName = dr1["ChildName"].ToString(),
                                                 Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString()),
                                                 RouteCode = dr1["RouteCode"].ToString(),
                                                 YakkrId = Convert.ToInt64(dr1["YakkrId"].ToString()),
                                                 DateOfBirth = Convert.ToDateTime(dr1["DOB"]).ToString("MM/dd/yyyy"),
                                                 CenterName = dr1["CenterName"].ToString(),
                                                 DisableNotesId = dr1["DisableNotesId"].ToString(),
                                                 DisableAttachment = dr1["DisableDocumentName"].ToString(),
                                                 CenterId = EncryptDecrypt.Encrypt64(dr1["CenterId"].ToString()),
                                                 ClassRoomId = dr1["ClassRoomId"].ToString(),
                                                 ProgramId = EncryptDecrypt.Encrypt64(dr1["ProgramId"].ToString()),
                                                 FswName = dr1["fswname"].ToString(),
                                                 ClassRoomName = dr1["classRoomName"].ToString(),
                                                 SchoolDistrict = dr1["district"].ToString(),
                                                 Gender = dr1["Gender"].ToString() == "1" ? "Male" : dr1["Gender"].ToString() == "2" ? "Female" : "Others",

                                                 AttachmentPath = (from DataRow dr2 in _dataset.Tables[0].Rows
                                                                   where dr2["DisableDocumentName"].ToString() != ""
                                                                   select new SelectListItem
                                                                   {
                                                                       Text = dr2["DisableDocumentName"].ToString(),
                                                                       Value = dr2["DisableNotesId"].ToString()
                                                                   }).ToList()

                                             }).Distinct().ToList();
                                children.DisabilityChildInfo = childList.FirstOrDefault();

                            }
                        }
                    }
                }

                if (mode == 3 || mode == 4)//YAKKR CODE 302 OR 303
                {
                    if (_dataset != null)
                    {
                        if (_dataset.Tables[0] != null)
                        {
                            if (_dataset.Tables[0].Rows.Count > 0)
                            {
                                List<SelectListItem> arraystring = new List<SelectListItem>();

                                arraystring = (from DataRow dr0 in _dataset.Tables[0].Rows
                                               select new SelectListItem
                                               {
                                                   Text = dr0["DisableDocumentName"].ToString(),
                                                   Value = dr0["DisableNotesId"].ToString()
                                               }).Where(x => x.Text != "").Distinct().ToList();

                                childList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                             select new DisabilityChildren.DisbilityChild
                                             {
                                                 ClientId = Convert.ToInt64(dr1["ClientId"]),
                                                 ChildName = dr1["ChildName"].ToString(),
                                                 Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString()),
                                                 RouteCode = dr1["RouteCode"].ToString(),
                                                 YakkrId = Convert.ToInt64(dr1["YakkrId"].ToString()),
                                                 DateOfBirth = Convert.ToDateTime(dr1["DOB"]).ToString("MM/dd/yyyy"),
                                                 CenterName = dr1["CenterName"].ToString(),
                                                 DisableNotesId = dr1["DisableNotesId"].ToString(),
                                                 DisableAttachment = dr1["DisableDocumentName"].ToString(),
                                                 CenterId = EncryptDecrypt.Encrypt64(dr1["CenterId"].ToString()),
                                                 ClassRoomId = dr1["ClassRoomId"].ToString(),
                                                 ProgramId = EncryptDecrypt.Encrypt64(dr1["ProgramId"].ToString()),
                                                 DisabilityTypeId = dr1["DisablitiesTypeID"].ToString(),
                                                 SpecialServices = dr1["specialservices"].ToString(),
                                                 FswName = dr1["fswname"].ToString(),
                                                 ClassRoomName = dr1["classRoomName"].ToString(),
                                                 SchoolDistrict = dr1["district"].ToString(),
                                                 Gender = dr1["Gender"].ToString() == "1" ? "Male" : dr1["Gender"].ToString() == "2" ? "Female" : "Others",
                                                 AttachmentPath = arraystring
                                             }).Distinct().ToList();
                                string disabilityId = "";
                                var list = childList.Where(x => x.DisabilityTypeId != "").Select(x => x.DisabilityTypeId).Distinct().ToList();
                                if (list.Count > 0)
                                {
                                    disabilityId = list[0];
                                }
                                childList.ForEach(x => x.DisabilityTypeId = disabilityId);

                                children.DisabilityChildInfo = childList[0];
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return children;
        }

    }
}
