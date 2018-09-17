using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FingerprintsModel;
using System.Web.Mvc;
using System.IO;
using System.Web;


namespace FingerprintsData
{
    public class HomevisitorData
    {

        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;


        public DataSet getclients(string client, string agency, string roleId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlConnection con = connection.returnConnection();
                cmd.Connection = con;
                cmd.CommandText = "USP_getClientsForHomeVisiter";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@userid", client);
                cmd.Parameters.AddWithValue("@agencyid", agency);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return ds;
        }

        public string saveEvent(Scheduler obj)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand();
                SqlConnection con = connection.returnConnection();
                cmd.Connection = con;
                cmd.CommandText = "USP_insertSchedule";
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@userid", obj.ClientId);
                //cmd.Parameters.AddWithValue("@agencyid", obj.AgencyId);
                cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                cmd.Parameters.AddWithValue("@MeetingId", obj.MeetingId);
                cmd.Parameters.AddWithValue("@ClientId", obj.ClientId);
                cmd.Parameters.AddWithValue("@AgencyId", obj.AgencyId);
                cmd.Parameters.AddWithValue("@MeetingDescription", obj.MeetingDescription);
                cmd.Parameters.AddWithValue("@Description ", obj.Description);
                cmd.Parameters.AddWithValue("@StartTime ", obj.StartTime);
                cmd.Parameters.AddWithValue("@Duration", obj.Duration);
                cmd.Parameters.AddWithValue("@IsRecurring", obj.IsRecurrence);
                cmd.Parameters.AddWithValue("@EndTime ", obj.EndTime);
                cmd.Parameters.AddWithValue("@Date ", obj.MeetingDate);
                cmd.Parameters.AddWithValue("@CreatedBy", obj.StaffId);
                cmd.Parameters.AddWithValue("@EndDate", obj.EndDate);
                cmd.Parameters.AddWithValue("@Parentid", obj.ParentId);
                cmd.Parameters.AddWithValue("@InstanceId", obj.InstanceId);
                cmd.Parameters.AddWithValue("@result", string.Empty).Direction = ParameterDirection.Output;
                cmd.Connection.Open();
                int i = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                if (i > 0)
                    return "sucess_" + cmd.Parameters["@result"].Value.ToString();
                else
                    return "error";
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "server error";
            }

        }

        public List<Scheduler> getUserEvents(string userid, string agencyid)
        {
            List<Scheduler> li = new List<Scheduler>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlConnection con = connection.returnConnection();
                cmd.Connection = con;
                cmd.CommandText = "USP_Appointment_listby_user";
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@userid", obj.ClientId);
                //cmd.Parameters.AddWithValue("@agencyid", obj.AgencyId);
                cmd.Parameters.AddWithValue("@agencyid", agencyid);
                cmd.Parameters.AddWithValue("@userId", userid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds != null && ds.Tables != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            Scheduler addSchedulerRow = new Scheduler();
                            addSchedulerRow.MeetingId = Convert.ToInt32(item["ID"]);
                            addSchedulerRow.MeetingDescription = Convert.ToString(item["Description"]);
                            addSchedulerRow.title = Convert.ToString(item["Description"]);
                            addSchedulerRow.ClientName = EncryptDecrypt.Encrypt64(Convert.ToString(item["ClientID"]));
                            addSchedulerRow.ClientId = Convert.ToInt32(item["ClientID"]);
                            addSchedulerRow.StartTime = Convert.ToString(item["StartTime"]);//Changes on 23Jan2017
                            addSchedulerRow.EndTime = Convert.ToString(item["EndTime"]);//Changes on 23Jan2017
                            addSchedulerRow.MeetingNotes = Convert.ToString(item["Notes"]);
                            addSchedulerRow.Description = Convert.ToString(item["Notes"]);
                            addSchedulerRow.MeetingDate = Convert.ToDateTime(item["Date"]).ToString("MM/dd/yyyy");
                            addSchedulerRow.StartTime = Convert.ToString(item["StartTime"]);//Changes on 23Jan2017
                            addSchedulerRow.start = Convert.ToDateTime(item["Date"]).ToString("MM/dd/yyyy") + " " + Convert.ToString(item["StartTime"]);
                            addSchedulerRow.end = Convert.ToDateTime(item["Date"]).ToString("MM/dd/yyyy") + " " + Convert.ToString(item["EndTime"]);
                            addSchedulerRow.Duration = Convert.ToString(item["Duration"]);
                            addSchedulerRow.EndDate = Convert.ToString(item["EndDate"]);
                            addSchedulerRow.ParentId = Convert.ToString(item["Parentid"]);
                            addSchedulerRow.InstanceId = Convert.ToString(item["InstanceId"]);

                            if (item["IsDeletedInstance"] != DBNull.Value)
                            {
                                addSchedulerRow.IsDeletedInstance = Convert.ToBoolean(item["IsDeletedInstance"]);
                            }

                            if (item["IsRecurring"] != DBNull.Value)
                            {
                                addSchedulerRow.IsRecurrence = Convert.ToBoolean(item["IsRecurring"]);
                                addSchedulerRow.IsParentEvent = true;
                            }
                            li.Add(addSchedulerRow);
                        }
                    }

                }
                return li;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }
        }

        public string DeleteEvent(Scheduler obj)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlConnection con = connection.returnConnection();
                cmd.Connection = con;
                cmd.CommandText = "USP_DeleteSchedule";
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@userid", obj.ClientId);
                //cmd.Parameters.AddWithValue("@agencyid", obj.AgencyId);
                cmd.Parameters.AddWithValue("@Mode", obj.Mode);
                cmd.Parameters.AddWithValue("@MeetingId", obj.MeetingId);
                cmd.Parameters.AddWithValue("@ClientId", obj.ClientId);
                cmd.Parameters.AddWithValue("@AgencyId", obj.AgencyId);
                cmd.Parameters.AddWithValue("@MeetingDescription", obj.MeetingDescription);
                cmd.Parameters.AddWithValue("@Description ", obj.Description);
                cmd.Parameters.AddWithValue("@StartTime ", obj.StartTime);
                cmd.Parameters.AddWithValue("@Duration", obj.Duration);
                cmd.Parameters.AddWithValue("@IsRecurring", obj.IsRecurrence);
                cmd.Parameters.AddWithValue("@EndTime ", obj.EndTime);
                cmd.Parameters.AddWithValue("@Date ", obj.MeetingDate);
                cmd.Parameters.AddWithValue("@CreatedBy", obj.StaffId);
                cmd.Parameters.AddWithValue("@EndDate", obj.EndDate);
                cmd.Parameters.AddWithValue("@Parentid", obj.ParentId);
                cmd.Parameters.AddWithValue("@InstanceId", obj.InstanceId);
                cmd.Parameters.AddWithValue("@result", string.Empty).Direction = ParameterDirection.Output;

                cmd.Connection.Open();
                int i = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                if (i > 0)
                    return "sucess_" + cmd.Parameters["@result"].Value.ToString();
                else
                    return "error";
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "server error";
            }
        }


        public void GetChildDetails(ref DataSet dtTransportation, string AgencyId, string ClientId)
        {
            dtTransportation = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64(ClientId)));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEnrolledChildDetailsByClient";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtTransportation);
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
        }

        public bool UpdateScheduleAppointment(Scheduler scheduler, string meetingStartTime, string meetingEndTime, string meetingDuration)
        {

            bool isRowAffected = false;
            try
            {
                //DataTable parentDt = new DataTable();
                //parentDt.Columns.AddRange(new DataColumn[2] {
                //         new DataColumn("Id", typeof(int)),
                //    new DataColumn("ClientId",typeof(int)) });

                //if (scheduler.ParentDetailsList.Count() > 0)
                //{
                //    for (int i = 0; i < scheduler.ParentDetailsList.Count(); i++)
                //    {

                //        parentDt.Rows.Add(i, Convert.ToInt32(scheduler.ParentDetailsList[i].ClientId));
                //    }
                //}

                command = new SqlCommand();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyID", scheduler.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", scheduler.StaffRoleId));
                command.Parameters.Add(new SqlParameter("@UserId", scheduler.StaffId));
                command.Parameters.Add(new SqlParameter("@MeetingId", scheduler.MeetingId));
                command.Parameters.Add(new SqlParameter("@ClientId", scheduler.ClientId));
                command.Parameters.Add(new SqlParameter("@AttendanceTypeId", scheduler.AttendanceTypeId));
                command.Parameters.Add(new SqlParameter("@IsUpdateEnroll", scheduler.IsUpdateEnrollment));
                command.Parameters.Add(new SqlParameter("@IsReschedule", scheduler.IsReSchedule));
                command.Parameters.Add(new SqlParameter("@MeetingDate", scheduler.MeetingDate));
                command.Parameters.Add(new SqlParameter("@StartTime", scheduler.StartTime));
                command.Parameters.Add(new SqlParameter("@EndTime", scheduler.EndTime));
                command.Parameters.Add(new SqlParameter("@Duration", scheduler.Duration));
                // command.Parameters.Add(new SqlParameter("@ParentCheckTable", parentDt));
                command.Parameters.Add(new SqlParameter("@ParentId1", scheduler.ParentId1));
                command.Parameters.Add(new SqlParameter("@ParentId2", scheduler.ParentId2));
                command.Parameters.Add(new SqlParameter("@PSignature1", scheduler.PSignature1));
                command.Parameters.Add(new SqlParameter("@PSignature2", scheduler.PSignature2));
                command.Parameters.Add(new SqlParameter("@Day", scheduler.Day));
                command.Parameters.Add(new SqlParameter("@MeetingStartTime", meetingStartTime));
                command.Parameters.Add(new SqlParameter("@MeetingEndTime", meetingEndTime));
                command.Parameters.Add(new SqlParameter("@MeetingDuration", meetingDuration));
                //command.Parameters.Add(new SqlParameter("@result", string.Empty).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_UpdateScheduleHVAppointment";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = (int)command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isRowAffected;
        }

        public bool CheckAvailableAppointment(Scheduler schedular)
        {
            bool isResult = false;
            List<TimeSpan> span = new List<TimeSpan>();
            List<TimeSpan> spanList = new List<TimeSpan>();
            double interval = 30;

            try
            {
                spanList = GetTimeSpanBetweenTimes(schedular.MeetingDate, schedular.StartTime, schedular.EndTime, interval);

                command = new SqlCommand();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyID", schedular.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", schedular.StaffId));
                command.Parameters.Add(new SqlParameter("@StartTime", schedular.StartTime));
                command.Parameters.Add(new SqlParameter("@EndTime", schedular.EndTime));
                command.Parameters.Add(new SqlParameter("@MeetingDate", schedular.MeetingDate));
                command.Parameters.Add(new SqlParameter("@ClientId", schedular.ClientId));
                command.Parameters.Add(new SqlParameter("@mode", 1));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CheckAppointmentSchedule";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                _dataset = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        isResult = (from DataRow dr in _dataset.Tables[0].Rows
                                    select DateTime.Parse(dr["TimeSpan"].ToString()).TimeOfDay
                              ).ToList().Where(x => spanList.Contains(x)).Any();
                    }

                    //if(span.Count()>0)
                    //{
                    //    isResult = span.Where(x => spanList.Contains(x)).Any();
                    //}
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isResult;
        }

        public List<string> GetFilteredDates(List<string> dates, List<string> dates2, Scheduler schedular)
        {
            try
            {

                string datelist = string.Join(",", dates.ToArray());

                command = new SqlCommand();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyID", schedular.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", schedular.StaffId));
                command.Parameters.Add(new SqlParameter("@StartTime", schedular.StartTime));
                command.Parameters.Add(new SqlParameter("@EndTime", schedular.EndTime));
                command.Parameters.Add(new SqlParameter("@MeetingDate", datelist));
                command.Parameters.Add(new SqlParameter("@ClientId", schedular.ClientId));
                command.Parameters.Add(new SqlParameter("@mode", 2));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CheckAppointmentSchedule";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                _dataset = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);

                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        var tempDates = dates;
                        dates = (from DataRow dr in _dataset.Tables[0].Rows
                                 select dr["ScheduledDates"].ToString()).ToList();
                        if (dates.Count() > 0)
                        {
                            dates = dates2.Except(dates).ToList();
                            dates = dates2.Except(tempDates).ToList();
                        }
                        else
                        {
                            dates = dates2;
                        }

                    }
                    else
                    {
                        dates = new List<string>();
                        dates = dates2;
                    }

                }
                else
                {
                    dates = dates2;
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return dates;
        }



        public List<TimeSpan> GetTimeSpanBetweenTimes(string date, string startTime, string endTime, double interval)
        {
            List<TimeSpan> spanList = new List<TimeSpan>();
            try
            {


                //TimeSpan start = TimeSpan.Parse("23:55");
                //TimeSpan end = TimeSpan.Parse("00:10");

                DateTime starting = Convert.ToDateTime(date + " " + startTime);
                DateTime ending = Convert.ToDateTime(date + " " + endTime);
                for (var ts = starting; ts <= ending; ts = ts.AddMinutes(interval))
                {
                    spanList.Add(ts.TimeOfDay);
                }

                // spanList=spanList.AddRange(starting.AddMinutes(interval).TimeOfDay)

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return spanList;

        }


        public Scheduler GetInitialAppointmentByClientId(Scheduler schedular)
        {

            ParentDetails parentDetails = new ParentDetails();

            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyID", schedular.AgencyId));
                command.Parameters.Add(new SqlParameter("@ClientId", schedular.ClientId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetInitialAppointmentByClient";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                _dataset = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                Connection.Close();
                schedular = new Scheduler();
                schedular.ScheduledAppointment = new Scheduler();
                schedular.ParentDetailsList = new List<ParentDetails>();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        schedular = (from DataRow dr in _dataset.Tables[0].Rows
                                     select new Scheduler
                                     {
                                         ClientId = Convert.ToInt64(dr["ClientId"]),
                                         MeetingDate = Convert.ToString(dr["MeetingDate"]),
                                         StartTime = Convert.ToString(dr["StartTime"]),
                                         EndTime = Convert.ToString(dr["EndTime"]),
                                         Duration = Convert.ToString(dr["Duration"]),
                                         IsRepeat = Convert.ToBoolean(dr["IsRecurring"]),
                                         Day = Convert.ToString(dr["Day"]),
                                         MeetingId = Convert.ToInt64(dr["ID"])
                                     }
                                   ).ToList()[0];
                    }

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        schedular.ParentDetailsList = (from DataRow dr1 in _dataset.Tables[1].Rows
                                                       select new ParentDetails
                                                       {
                                                           ParentName = dr1["ParentName"].ToString(),
                                                           ParentRole = (dr1["ParentRole"].ToString() == "1") ? "Father" :
                                                                        (dr1["ParentRole"].ToString() == "2") ?"Mother":
                                                                        (dr1["ParentRole"].ToString() == "3") ? "Grandparents" :
                                                                        (dr1["ParentRole"].ToString() == "4") ? "Relatives other than grandparents" :
                                                                          (dr1["ParentRole"].ToString() == "5")? "Foster Parent - Not including relative":"Other",

                                                           ClientId = dr1["ClientId"].ToString()
                                                       }
                                                     ).ToList();
                    }

                    if(_dataset.Tables.Count>2 && _dataset.Tables[2].Rows.Count>0)
                    {
                        schedular.ScheduledAppointment = (from DataRow dr2 in _dataset.Tables[2].Rows
                                     select new Scheduler
                                     {
                                         ClientId = Convert.ToInt64(dr2["ClientId"]),
                                         MeetingDate = Convert.ToString(dr2["MeetingDate"]),
                                         StartTime = Convert.ToString(dr2["StartTime"]),
                                         EndTime = Convert.ToString(dr2["EndTime"]),
                                         Duration = Convert.ToString(dr2["Duration"]),
                                         IsRepeat = Convert.ToBoolean(dr2["IsRecurring"]),
                                         Day = Convert.ToString(dr2["Day"]),
                                         MeetingId = Convert.ToInt64(dr2["ID"])
                                     }
                                   ).ToList()[0];
                    }

                    if(_dataset.Tables.Count>3 && _dataset.Tables[3].Rows.Count>0)
                    {
                        schedular.ProgramYearStartDate = Convert.ToString(_dataset.Tables[3].Rows[0]["ProgramYearStartDate"]);
                    }
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

            return schedular;
        }


        public List<SelectListItem> GetFamiliesUnderUserId(string userId, string agencyId, string roleId)
        {
            List<SelectListItem> usersList = new List<SelectListItem>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetFamiliesUnderUserId";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", agencyId));
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@RoleId", roleId));

                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        usersList = (from DataRow dr in _dataset.Tables[0].Rows
                                     select new SelectListItem
                                     {
                                         Text = dr["clientFamily"].ToString(),
                                         Value = EncryptDecrypt.Encrypt64(dr["ClientId"].ToString())
                                     }
                                   ).ToList();
                    }
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
                command.Dispose();
            }
            return usersList;
        }

        public bool InsertHistoricalHomeVisit(List<Scheduler> schedulerList, Guid agencyId, Guid homeVisitorId, Guid userId, string meetingId)
        {
            int rowsAffected = 0;
            bool isResult = false;
            try
            {
                DataTable table = new DataTable();

                schedulerList.ForEach(x =>
                {
                    x.ClientId = Convert.ToInt64(EncryptDecrypt.Decrypt64(x.Enc_ClientId));
                    x.StaffId = homeVisitorId;
                    x.Status = true;
                    x.CreatedDate = x.MeetingDate;
                    x.AgencyId = agencyId;

                });

                schedulerList = schedulerList.Distinct().ToList();

                table = GetHistoricalEntryTable(schedulerList);
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyID", agencyId));
                command.Parameters.Add(new SqlParameter("@HomeVisitorId", homeVisitorId));
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@ScheduleTable", table));
                command.Parameters.Add(new SqlParameter("@MeetingId", meetingId));


                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertHistoricalHomeVisit";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                rowsAffected = command.ExecuteNonQuery();
                Connection.Close();

                if (rowsAffected > 0)
                {
                    isResult = true;
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
            return isResult;
        }


        public List<Scheduler> GetHomeVisitAttendanceByFromDate(Scheduler scheduler)
        {
            List<Scheduler> schedulerList = new List<Scheduler>();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", scheduler.AgencyId));
                command.Parameters.Add(new SqlParameter("@ClientId", scheduler.ClientId));
                command.Parameters.Add(new SqlParameter("@AttendanceDate", scheduler.MeetingDate));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_HomeVisitsAttendanceDetails";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                _dataset = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        schedulerList = (from DataRow dr in _dataset.Tables[0].Rows
                                         select new Scheduler
                                         {
                                             MeetingDate = dr["AttendanceDate"].ToString(),
                                             ClientId = Convert.ToInt64(dr["ClientId"]),
                                             AttendanceTypeId = Convert.ToInt64(dr["AttendanceType"]),
                                             ParentId1 = Convert.ToString(dr["ParentId1"]),
                                             ParentId2 = Convert.ToString(dr["ParentId2"])
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

            }

            return schedulerList;

        }


        public DataTable GetHistoricalEntryTable(List<Scheduler> schedulerList)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.AddRange(new DataColumn[16] {
                    new DataColumn("ID",typeof(long)),
                    new DataColumn("ClientId",typeof(long)),
                    new DataColumn("StartTime",typeof(string)),
                    new DataColumn("EndTime",typeof(string)),
                    new DataColumn("Date ", typeof(string)),
                    new DataColumn("Day",typeof(string)),
                    new DataColumn("Status",typeof(bool)),
                    new DataColumn("CreatedBy",typeof(Guid)),
                    new DataColumn("CreatedDate",typeof(string)),
                    new DataColumn("AgencyId",typeof(Guid)),
                    new DataColumn("Duration",typeof(string)),
                    new DataColumn("IsRecurring ", typeof(bool)),
                    new DataColumn("ParentId1",typeof(string)),
                    new DataColumn("ParentId2",typeof(string)),
                    new DataColumn("IsVisited",typeof(bool)),
                    new DataColumn("AttendanceType",typeof(int))

                });


                foreach (var item in schedulerList)
                {

                    dt.Rows.Add(
                        0,
                          item.ClientId
                    , item.StartTime
                    , item.EndTime
                    , item.MeetingDate
                    , item.Day
                    , true
                    , item.StaffId
                    , item.Date
                    , item.AgencyId
                    , item.Duration
                    , item.IsRepeat
                    , item.ParentId1
                    , item.ParentId2
                    , 1
                    , item.AttendanceTypeId

                        );

                }

                return dt;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return dt;
            }

        }


        public List<SelectListItem> GetUsersByRoleId(string targetRoleId, string roleId, string userId, string agencyId)
        {
            List<SelectListItem> usersList = new List<SelectListItem>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetUsersByRoleId";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", agencyId));
                command.Parameters.Add(new SqlParameter("@TargetRoleId", targetRoleId));
                command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                Connection.Open();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        usersList = (from DataRow dr in _dataset.Tables[0].Rows
                                     select new SelectListItem
                                     {
                                         Text = dr["StaffName"].ToString(),
                                         Value = dr["UserID"].ToString()
                                     }
                                   ).ToList();
                    }
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
                command.Dispose();
            }
            return usersList;
        }

    }
}
