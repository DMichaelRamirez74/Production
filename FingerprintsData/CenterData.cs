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

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Globalization;
//using System.Web.Script.Serialization;

namespace FingerprintsData
{
    public class CenterData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        //SqlDataReader dataReader = null;
        //SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable _dataTable = null;
        DataSet _dataset = null;
        public string addeditcenter(Center info, List<Center.ClassRoom> Classroom, bool isEndOfYear = false)
        {
            try
            {
                string result = string.Empty;
                command.Connection = Connection;
                command.CommandText = "SP_addcenter_withclass";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@CenterName", info.CenterName);
                command.Parameters.AddWithValue("@StepUpToQualityStars", info.StepUpToQualityStars);
                command.Parameters.AddWithValue("@NAEYCCertified", info.NAEYCCertified);
                command.Parameters.AddWithValue("@Address", info.Address);
                command.Parameters.AddWithValue("@City", info.City);
                command.Parameters.AddWithValue("@State", info.State);
                command.Parameters.AddWithValue("@Zip", info.Zip);
                command.Parameters.AddWithValue("@TimeZoneID", info.TimeZoneID);
                command.Parameters.AddWithValue("@AdminSite", info.AdminSite);
                command.Parameters.AddWithValue("@Homebased", info.HomeBased);
                command.Parameters.AddWithValue("@IsEndOfYear", isEndOfYear);
                if (Classroom != null && Classroom.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[29] {
                    new DataColumn("ClassroomName", typeof(string)),
                    new DataColumn("MaxCapacity",typeof(string)),
                     new DataColumn("ClassroomID",typeof(string)),
                    new DataColumn("DoubleSession",typeof(string)),
                     new DataColumn("ClassSession",typeof(string)),
                     new DataColumn("StartTime",typeof(string)),
                     new DataColumn("StopTime",typeof(string)),
                      new DataColumn("Dinner",typeof(string)),
                       new DataColumn("Lunch",typeof(string)),
                        new DataColumn("Breakfast",typeof(string)),
                        new DataColumn("Snack",typeof(string)),
                         new DataColumn("Monday",typeof(string)),
                         new DataColumn("Tuesday",typeof(string)),
                         new DataColumn("Wednesday",typeof(string)),
                         new DataColumn("Thursday",typeof(string)),
                         new DataColumn("Friday",typeof(string)),
                         new DataColumn("Saturday",typeof(string)),
                         new DataColumn("Sunday",typeof(string)),
                          new DataColumn("AvailSeats",typeof(string)),
                           new DataColumn("NoOfSeats",typeof(string)),
                           new DataColumn("BreakfastFromTime",typeof(string)),
                           new DataColumn("BreakfastToTime",typeof(string)),
                           new DataColumn("LunchFromTime",typeof(string)),
                           new DataColumn("LunchToTime",typeof(string)),
                           new DataColumn("SnackFromTime",typeof(string)),
                           new DataColumn("SnackToTime",typeof(string)),
                           new DataColumn("DinnerFromTime",typeof(string)),
                           new DataColumn("DinnerToTime",typeof(string)),
                           new DataColumn("TimeBetweenMeals",typeof(string))

                    });
                    foreach (Center.ClassRoom classname in Classroom)
                    {
                        if (classname.RoomName != null)
                        {
                            dt.Rows.Add(classname.RoomName,
                                classname.MaxCapacity,
                                classname.ClassroomID,
                                classname.DoubleSession,
                                classname.ClassSession,
                                classname.StartTime,
                                classname.StopTime,
                                classname.Dinner,
                                classname.Lunch,
                                classname.Breakfast,
                                classname.Snack,
                                //classname.Snack2,
                                classname.Monday,
                                classname.Tuesday,
                                classname.Wednesday,
                                classname.Thursday,
                                classname.Friday,
                                classname.Saturday,
                                classname.Sunday,
                                classname.ActualSeats,
                                classname.Noofseats,
                                classname.BreakfastFromTime,
                                classname.BreakfastToTime,
                                classname.LunchFromTime,
                                classname.LunchToTime,
                                classname.SnackFromTime,
                                classname.SnackToTime,
                                classname.DinnerFromTime,
                               classname.DinnerToTime,
                                classname.TimeBetweenMeals);

                        }
                    }
                    command.Parameters.Add(new SqlParameter("@tblphone", dt));

                }

                //info.AreaID = (info.AreaID == 0) ? 1 : info.AreaID;
                //info.DivisionID = (info.DivisionID == 0) ? 1 : info.DivisionID;

                command.Parameters.AddWithValue("@AreaID", info.AreaID);
                command.Parameters.AddWithValue("@DivisionID", info.DivisionID);
                command.Parameters.AddWithValue("@mode", info.mode);
                command.Parameters.AddWithValue("@CenterId", info.CenterId);
                command.Parameters.AddWithValue("@AgencyId", info.AgencyId);
                command.Parameters.AddWithValue("@CreatedBy", info.CreatedBy);
                command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
        }

        public ClientAttendancePercentage GetDailyAttendance(string CenterId, string AgencyId, string UserId)
        {
            ClientAttendancePercentage per = new ClientAttendancePercentage();

            try
            {
                var centerid = EncryptDecrypt.Decrypt64(CenterId);

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                DataSet ds = new DataSet();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", centerid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetClientAttendancePercentage";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        per.CurrentDayPercentage = Convert.ToInt32(dr["OneDayPercentage"]);
                        per.MonthlyPercentage = Convert.ToInt32(dr["OneMonthPercentage"]);
                        per.WeeklyPercentage = Convert.ToInt32(dr["OneWeekPercentage"]);
                        per.YearlyPercentage = Convert.ToInt32(dr["OneYearPercentage"]);
                        per.DailyClientCount = Convert.ToInt32(dr["OneDayCount"]);
                        per.MonthlyClientCount = Convert.ToInt32(dr["OneMonthCount"]);
                        per.WeeklyClientCount = Convert.ToInt32(dr["OneWeekCount"]);
                        per.YearlyClientCount = Convert.ToInt32(dr["OneYearCount"]);
                        per.DayTotalEnroll = Convert.ToInt32(dr["DayTotEnroll"]);
                        per.MonthTotalEnroll = Convert.ToInt32(dr["MonthTotEnroll"]);
                        per.WeekTotalEnroll = Convert.ToInt32(dr["WeekTotEnroll"]);
                        per.YearTotalEnroll = Convert.ToInt32(dr["YearTotEnroll"]);
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
            return per;
        }

        public Center editcentre(string id, string agencyid, bool isEndOfYear = false)
        {

            Center _centre = new Center();

            _centre.DivisionsList = new List<Divisions>();
            _centre.AreasList = new List<Areas>();
            try
            {
                if (!string.IsNullOrEmpty(agencyid))
                {

                }
                else
                {
                    agencyid = null;
                }
                command.Parameters.Add(new SqlParameter("@centerid", id));
                if (!string.IsNullOrEmpty(agencyid))
                    command.Parameters.Add(new SqlParameter("@agencyID", agencyid));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getcentreinfo";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables.Count > 1)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        List<TimeZoneinfo> TimeZonelist = new List<TimeZoneinfo>();
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            TimeZoneinfo obj = new TimeZoneinfo();
                            obj.TimZoneId = dr["TimeZone_ID"].ToString();
                            obj.TimZoneName = dr["TIMEZONENAME"].ToString();
                            TimeZonelist.Add(obj);
                        }

                        _centre.TimeZonelist = TimeZonelist;
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        _centre.Address = _dataset.Tables[1].Rows[0]["Address"].ToString();
                        _centre.AdminSite = _dataset.Tables[1].Rows[0]["AdminSite"].ToString();
                        _centre.CenterId = Convert.ToInt32(_dataset.Tables[1].Rows[0]["CenterId"]);
                        _centre.CenterName = _dataset.Tables[1].Rows[0]["CenterName"].ToString();
                        _centre.City = _dataset.Tables[1].Rows[0]["City"].ToString();
                        _centre.NAEYCCertified = _dataset.Tables[1].Rows[0]["NAEYCCertified"].ToString();
                        _centre.StepUpToQualityStars = _dataset.Tables[1].Rows[0]["StepUpToQualityStars"].ToString();
                        _centre.TimeZoneID = _dataset.Tables[1].Rows[0]["TimeZone"].ToString();
                        _centre.Zip = _dataset.Tables[1].Rows[0]["Zip"].ToString();
                        _centre.State = _dataset.Tables[1].Rows[0]["State"].ToString();
                        _centre.HomeBased = Convert.ToBoolean(_dataset.Tables[1].Rows[0]["HomeBased"]);
                        _centre.AgencyName = _dataset.Tables[1].Rows[0]["AgencyName"].ToString();
                        _centre.AgencyId = _dataset.Tables[1].Rows[0]["AgencyId"].ToString();
                        _centre.AreaID = string.IsNullOrEmpty(_dataset.Tables[1].Rows[0]["AreaID"].ToString()) ? 0 : (long)_dataset.Tables[1].Rows[0]["AreaID"];
                        _centre.DivisionID = string.IsNullOrEmpty(_dataset.Tables[1].Rows[0]["DivisionID"].ToString()) ? 0 : (long)_dataset.Tables[1].Rows[0]["DivisionID"];
                        _centre.ProgramYear = String.IsNullOrEmpty(_dataset.Tables[1].Rows[0]["ActiveProgramYear"].ToString()) ? string.Empty : Convert.ToString(_dataset.Tables[1].Rows[0]["ActiveProgramYear"]);
                    }
                    // if (_dataset.Tables[2].Rows.Count > 0)
                    if ((_dataset.Tables[2].Rows.Count > 0) && (Convert.ToString(_centre.CenterId) != "0"))
                    {
                        for (int i = 0; i < _dataset.Tables[2].Rows.Count; i++)
                        {
                            List<FingerprintsModel.Center.ClassRoom> _categorylist = new List<FingerprintsModel.Center.ClassRoom>();
                            FingerprintsModel.Center.ClassRoom obj = new FingerprintsModel.Center.ClassRoom();
                            obj.ClassroomID = Convert.ToInt32(_dataset.Tables[2].Rows[i]["ClassroomID"].ToString());
                            obj.RoomName = _dataset.Tables[2].Rows[i]["ClassroomName"].ToString();
                            //obj.ClassSession = _dataset.Tables[1].Rows[i]["ClassSession"].ToString();
                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["ClassSession"].ToString()))
                            {
                                obj.ClassSession = _dataset.Tables[2].Rows[i]["ClassSession"].ToString();
                            }
                            else
                            {
                                obj.ClassSession = string.Empty;
                            }


                            obj.DoubleSession = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["DoubleSession"].ToString());
                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["StartTime"].ToString()))
                            {
                                obj.StartTime = _dataset.Tables[2].Rows[i]["StartTime"].ToString();
                            }
                            else
                            {
                                obj.StartTime = string.Empty;
                            }
                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["EndTime"].ToString()))
                            {
                                obj.StopTime = _dataset.Tables[2].Rows[i]["EndTime"].ToString();
                            }
                            else
                            {
                                obj.StopTime = string.Empty;
                            }


                            //  obj.StartTime = _dataset.Tables[1].Rows[i]["StartTime"].ToString();
                            // obj.StopTime = _dataset.Tables[1].Rows[i]["StopTime"].ToString();
                            obj.Dinner = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Dinner"].ToString());
                            obj.Breakfast = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Breakfast"].ToString());
                            obj.Lunch = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Lunch"].ToString());
                            obj.Snack = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Snack"].ToString());
                            obj.Monday = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Monday"].ToString());
                            obj.Tuesday = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Tuesday"].ToString());
                            obj.Wednesday = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Wednesday"].ToString());
                            obj.Thursday = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Thursday"].ToString());
                            obj.Friday = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Friday"].ToString());
                            obj.Saturday = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Saturday"].ToString());
                            obj.Sunday = Convert.ToBoolean(_dataset.Tables[2].Rows[i]["Sunday"].ToString());
                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["NoOfSeats"].ToString()))
                            {
                                obj.Noofseats = _dataset.Tables[2].Rows[i]["NoOfSeats"].ToString();
                            }
                            else
                            {
                                obj.Noofseats = string.Empty;
                            }

                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["BreakfastFromTime"].ToString()))
                            {
                                obj.BreakfastFromTime = _dataset.Tables[2].Rows[i]["BreakfastFromTime"].ToString();
                            }
                            else
                            {
                                obj.BreakfastFromTime = string.Empty;
                            }



                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["LunchFromTime"].ToString()))
                            {
                                obj.LunchFromTime = _dataset.Tables[2].Rows[i]["LunchFromTime"].ToString();
                            }
                            else
                            {
                                obj.LunchFromTime = string.Empty;
                            }


                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["SnackFromTime"].ToString()))
                            {
                                obj.SnackFromTime = _dataset.Tables[2].Rows[i]["SnackFromTime"].ToString();
                            }
                            else
                            {
                                obj.SnackFromTime = string.Empty;
                            }



                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["DinnerFromTime"].ToString()))
                            {
                                obj.DinnerFromTime = _dataset.Tables[2].Rows[i]["DinnerFromTime"].ToString();
                            }
                            else
                            {
                                obj.DinnerFromTime = string.Empty;
                            }
                            //if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["Snack2FromTime"].ToString()))
                            //{
                            //    obj.Snack2FromTime = _dataset.Tables[2].Rows[i]["SnackFromTime"].ToString();
                            //}
                            //else
                            //{
                            //    obj.Snack2FromTime = string.Empty;
                            //}
                            obj.Snack2FromTime = string.Empty;
                            if (_dataset.Tables[2].Rows[i]["TimeBetweenMeals"] != DBNull.Value)
                            {
                                obj.TimeBetweenMeals = Convert.ToInt32(_dataset.Tables[2].Rows[i]["TimeBetweenMeals"].ToString());
                            }
                            else
                            {
                                obj.TimeBetweenMeals = 0;
                            }

                            if (!string.IsNullOrEmpty(_dataset.Tables[2].Rows[i]["ActualSeats"].ToString()))
                            {
                                obj.ActualSeats = _dataset.Tables[2].Rows[i]["ActualSeats"].ToString();
                            }
                            else
                            {
                                obj.ActualSeats = string.Empty;
                            }


                            _centre.Classroom.Add(obj);

                        }

                    }



                    if (_dataset.Tables[3].Rows.Count > 0)
                    {
                        _centre.AreasList = _dataset.Tables[3].AsEnumerable().Select(x => new Areas
                        {
                            AreaIndexID = x.Field<long>("AreaIndexID"),
                            AreaID = x.Field<long>("AreaID"),
                            Description = x.Field<string>("Description")
                        }).ToList();
                    }

                    if (_dataset.Tables[4].Rows.Count > 0)
                    {
                        _centre.DivisionsList = _dataset.Tables[4].AsEnumerable().Select(x => new Divisions
                        {
                            DivisionIndexID = x.Field<long>("DivisionIndexID"),
                            DivisionID = x.Field<long>("DivisionID"),
                            Description = x.Field<string>("Description")
                        }).ToList();
                    }

                    if (_dataset.Tables[5].Rows.Count > 0)
                    {
                        _centre.IsShowArea = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["IsShowArea"]);
                        _centre.IsShowDivision = Convert.ToBoolean(_dataset.Tables[5].Rows[0]["IsShowDivision"]);
                    }

                    if (isEndOfYear == true && id == "0")
                    {
                        _centre.ProgramYear = Convert.ToString(_dataset.Tables[6].Rows[0]["ActiveProgramYear"]);
                    }

                }
                return _centre;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _centre;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
        }
        public List<Center> centerList(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string agencyid, bool isEndOfYear = false)
        {
            List<Center> _centerlist = new List<Center>();
            try
            {
                totalrecord = string.Empty;
                string searchcenter = string.Empty;
                string AgencyId = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchcenter = string.Empty;
                else
                    searchcenter = search;
                if (string.IsNullOrEmpty(agencyid.Trim()))
                    AgencyId = string.Empty;
                else
                    AgencyId = agencyid;
                command.Parameters.Add(new SqlParameter("@Search", searchcenter));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                if (!string.IsNullOrEmpty(agencyid))
                    command.Parameters.Add(new SqlParameter("@agencyID", agencyid));
                // command.Parameters.Add(new SqlParameter("@agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_getcenterlist";
                DataAdapter = new SqlDataAdapter(command);
                _dataTable = new DataTable();
                DataAdapter.Fill(_dataTable);
                if (_dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < _dataTable.Rows.Count; i++)
                    {
                        Center addCenter = new Center();
                        addCenter.CenterId = Convert.ToInt32(_dataTable.Rows[i]["CenterId"]);
                        addCenter.CenterName = _dataTable.Rows[i]["CenterName"].ToString();
                        addCenter.City = _dataTable.Rows[i]["City"].ToString();
                        addCenter.State = _dataTable.Rows[i]["State"].ToString();
                        addCenter.DateEntered = Convert.ToString(_dataTable.Rows[i]["DateEntered"]);
                        addCenter.status = _dataTable.Rows[i]["Status"].ToString();
                        addCenter.ProgramYear = Convert.ToString(_dataTable.Rows[i]["ActiveProgramYear"]);
                        if (!string.IsNullOrEmpty(_dataTable.Rows[i]["AgencyName"].ToString()))
                        {
                            addCenter.AgencyName = _dataTable.Rows[i]["AgencyName"].ToString();
                        }
                        else
                        {
                            addCenter.AgencyName = string.Empty;
                        }
                        _centerlist.Add(addCenter);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _centerlist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _centerlist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataTable.Dispose();
            }
        }
        public int UpdateCenter(string id, int mode, Guid userId, bool isEndOfYear = false)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@CenterId", id));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@userid", userId));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                command.CommandText = "Sp_Update_center";
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return 0;
            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
        }
        public List<Center.ClassRoom> ClassDetails(string CenterId, string Agencyid)
        {
            List<Center.ClassRoom> _classroom = new List<Center.ClassRoom>();
            try
            {
                // command.Parameters.Add(new SqlParameter("@householdid", householdid));
                command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                if (!string.IsNullOrEmpty(Agencyid))
                    command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                // command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_classlist";
                DataAdapter = new SqlDataAdapter(command);
                _dataTable = new DataTable();
                DataAdapter.Fill(_dataTable);
                if (_dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in _dataTable.Rows)
                    {
                        Center.ClassRoom _classadd = new Center.ClassRoom();
                        _classadd.ClassroomID = Convert.ToInt32(row["ClassroomID"]);
                        _classadd.RoomName = row["ClassroomName"].ToString();
                        _classadd.ClassSession = (row["ClassSession"]).ToString();
                        _classadd.DoubleSession = Convert.ToBoolean(row["DoubleSession"]);
                        _classadd.StartTime = (row["StartTime"]).ToString();
                        _classadd.StopTime = (row["EndTime"]).ToString();
                        _classadd.Dinner = Convert.ToBoolean(row["DoubleSession"]);
                        _classadd.Breakfast = Convert.ToBoolean(row["Breakfast"]);
                        _classadd.Lunch = Convert.ToBoolean(row["Lunch"]);
                        _classadd.Snack = Convert.ToBoolean(row["Snack"]);
                        _classadd.Monday = Convert.ToBoolean(row["Monday"]);
                        _classadd.Tuesday = Convert.ToBoolean(row["Tuesday"]);
                        _classadd.Wednesday = Convert.ToBoolean(row["Wednesday"]);
                        _classadd.Thursday = Convert.ToBoolean(row["Thursday"]);
                        _classadd.Friday = Convert.ToBoolean(row["Friday"]);
                        _classadd.Saturday = Convert.ToBoolean(row["Saturday"]);
                        _classadd.Sunday = Convert.ToBoolean(row["Sunday"]);
                        _classadd.BreakfastFromTime = (row["BreakfastFromTime"]).ToString();
                        if (!string.IsNullOrEmpty(row["NoOfSeats"].ToString()))
                        {
                            _classadd.Noofseats = row["NoOfSeats"].ToString();
                        }
                        else
                        {
                            _classadd.Noofseats = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(row["AvailSeats"].ToString()))
                        {
                            _classadd.ActualSeats = row["AvailSeats"].ToString();
                        }
                        else
                        {
                            _classadd.ActualSeats = string.Empty;
                        }



                        _classroom.Add(_classadd);
                    }
                }
                return _classroom;
            }
            catch (Exception ex)
            {
                //  totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _classroom;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataTable.Dispose();
            }
        }
        public string DeleteClassroomdetails(string ClassroomID, string CenterId, string Agencyid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteclassroominfo";
                command.Parameters.Add(new SqlParameter("@ClassroomID", ClassroomID));
                command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                if (!string.IsNullOrEmpty(Agencyid))
                    command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                //   command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
            finally
            {
                command.Dispose();

            }
        }
        public List<Center.ClassRoom> GetClassroominfo(string ClassroomID, string CenterId, string Agencyid)
        {
            List<Center.ClassRoom> _classroom = new List<Center.ClassRoom>();
            try
            {
                command.Parameters.Add(new SqlParameter("@ClassroomID", ClassroomID));
                command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                if (!string.IsNullOrEmpty(Agencyid))
                    command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                // command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Get_classinfo";
                DataAdapter = new SqlDataAdapter(command);
                _dataTable = new DataTable();
                DataAdapter.Fill(_dataTable);
                if (_dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in _dataTable.Rows)
                    {
                        Center.ClassRoom _classadd = new Center.ClassRoom();
                        _classadd.ClassroomID = Convert.ToInt32(row["ClassroomID"]);
                        _classadd.RoomName = row["ClassroomName"].ToString();
                        _classadd.ClassSession = (row["ClassSession"]).ToString();
                        _classadd.DoubleSession = Convert.ToBoolean(row["DoubleSession"]);
                        _classadd.StartTime = (row["StartTime"]).ToString();
                        _classadd.StopTime = (row["EndTime"]).ToString();
                        _classadd.Dinner = Convert.ToBoolean(row["DoubleSession"]);
                        _classadd.Breakfast = Convert.ToBoolean(row["Breakfast"]);
                        _classadd.Lunch = Convert.ToBoolean(row["Lunch"]);
                        _classadd.Snack = Convert.ToBoolean(row["Snack"]);
                        _classadd.Monday = Convert.ToBoolean(row["Monday"]);
                        _classadd.Tuesday = Convert.ToBoolean(row["Tuesday"]);
                        _classadd.Wednesday = Convert.ToBoolean(row["Wednesday"]);
                        _classadd.Thursday = Convert.ToBoolean(row["Thursday"]);
                        _classadd.Friday = Convert.ToBoolean(row["Friday"]);
                        _classadd.Saturday = Convert.ToBoolean(row["Saturday"]);
                        _classadd.Sunday = Convert.ToBoolean(row["Sunday"]);
                        _classadd.BreakfastFromTime = (row["BreakfastFromTime"]).ToString();
                        if (!string.IsNullOrEmpty(row["NoOfSeats"].ToString()))
                        {
                            _classadd.Noofseats = row["NoOfSeats"].ToString();
                        }
                        else
                        {
                            _classadd.Noofseats = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(row["AvailSeats"].ToString()))
                        {
                            _classadd.ActualSeats = row["AvailSeats"].ToString();
                        }
                        else
                        {
                            _classadd.ActualSeats = string.Empty;
                        }
                        // _classadd.Noofseats = row["NoOfSeats"].ToString();
                        //  _classadd.ActualSeats = row["AvailSeats"].ToString();


                        _classroom.Add(_classadd);
                    }
                }
                return _classroom;
            }
            catch (Exception ex)
            {
                //  totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _classroom;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataTable.Dispose();
            }
        }
        public List<Center> AutoCompleteAgencyList(string term, string active = "0")
        {
            List<Center> AgencyList = new List<Center>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "AutoComplete_AgencyList";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AgnyName", term);
                        command.Parameters.AddWithValue("@Active", active);
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
                            Center obj = new Center();
                            obj.AgencyId = Convert.ToString(dr["AgencyId"].ToString());
                            obj.AgencyName = dr["AgencyName"].ToString();
                            //obj.AccessDays = dr["Accesstype"].ToString();
                            //obj.AccessStartDate = Convert.ToDateTime(dr["AccessStartDate"]).ToString("MM/dd/yyyy");
                            //obj.AccessStart = dr["AccessStart"].ToString();
                            //obj.AccessStop = dr["AccessStop"].ToString();
                            //obj.TimeZoneID = dr["TimeZone_ID"].ToString();

                            AgencyList.Add(obj);
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
            return AgencyList;
        }
        public Center GetData_AllDropdown()
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            Center _center = new Center();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_AgencyUser_Dropdowndata";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }


                if (ds.Tables[7].Rows.Count > 0)
                {

                    List<TimeZoneinfo> TimeZonelist = new List<TimeZoneinfo>();
                    foreach (DataRow dr in ds.Tables[7].Rows)
                    {
                        TimeZoneinfo obj = new TimeZoneinfo();
                        obj.TimZoneId = dr["TimeZone_ID"].ToString();
                        obj.TimZoneName = dr["TIMEZONENAME"].ToString();
                        TimeZonelist.Add(obj);
                    }
                    //TimeZonelist.Insert(0, new TimeZoneinfo() { TimZoneId = "0", TimZoneName = "Select" });
                    _center.TimeZonelist = TimeZonelist;
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            //  _agencyStafflist.Add(_staff);
            return _center;
        }
        public string DeleteClassroominfo(string CenterId, string Agencyid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteclassroomdetails";
                //command.Parameters.Add(new SqlParameter("@ClassroomID", ClassroomID));
                command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                if (!string.IsNullOrEmpty(Agencyid))
                    command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                //   command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
            finally
            {
                command.Dispose();

            }
        }
        //Changes
        public string DeleteClassroom(string classId, string Agencyid, bool isEndOfYear = false)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteclassinfo";
                //command.Parameters.Add(new SqlParameter("@ClassroomID", ClassroomID));
                command.Parameters.Add(new SqlParameter("@ClassId", classId));
                if (!string.IsNullOrEmpty(Agencyid))
                    command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
            finally
            {
                command.Dispose();

            }
        }
        //public Center GetClassroominfo(string ClassroomID, string CenterID, string agencyid)
        //{
        //    Center obj = new Center();
        //    try
        //    {
        //        command.Parameters.Add(new SqlParameter("@ClassroomID", ClassroomID));
        //        command.Parameters.Add(new SqlParameter("@CenterID", CenterID));
        //        command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
        //        command.Connection = Connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "SP_otherinfo";
        //        DataAdapter = new SqlDataAdapter(command);
        //        _dataTable = new DataTable();
        //        DataAdapter.Fill(_dataTable);
        //        if (_dataTable != null && _dataTable.Rows.Count > 0)
        //        {
        //            obj.Classroom = Convert.ToInt32(_dataTable.Rows[0]["ID"]);
        //            obj.Ofirstname = _dataTable.Rows[0]["Firstname"].ToString();
        //            obj.Omiddlename = _dataTable.Rows[0]["Middlename"].ToString();
        //            obj.Olastname = _dataTable.Rows[0]["Lastname"].ToString();
        //            obj.ODOB = Convert.ToDateTime(_dataTable.Rows[0]["DOB"]).ToString("MM/dd/yyyy");
        //            obj.OGender = _dataTable.Rows[0]["Gender"].ToString();
        //            obj.Oemergencycontact = _dataTable.Rows[0]["isemergency"].ToString() == "" ? false : Convert.ToBoolean(_dataTable.Rows[0]["isemergency"]);
        //        }
        //        DataAdapter.Dispose();
        //        command.Dispose();
        //        _dataTable.Dispose();
        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //        return obj;
        //    }
        //    finally
        //    {
        //        DataAdapter.Dispose();
        //        command.Dispose();
        //        familydataTable.Dispose();
        //    }
        //}



        public string NoOfSeats(string Seats, string Classid, string Agencyid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Getseats";
                command.Parameters.Add(new SqlParameter("@Seats", Seats));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@Classid", Classid));
                command.Parameters.Add(new SqlParameter("@result", ""));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();

            }
        }

        public void GetCentersByUserId(ref DataTable dtCenters, string UserID, string Agencyid, string RoleId, bool isreqAdminSite = false, bool isCenterBasedOnly = false, bool isHomeBasedOnly = false, bool isEndOfYear = false,bool allCenters=false)
        {
            dtCenters = new DataTable();
            try
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                    command.Parameters.Add(new SqlParameter("@UserId", UserID));
                    command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
                    command.Parameters.Add(new SqlParameter("@ReqAdminSite", isreqAdminSite));
                    command.Parameters.Add(new SqlParameter("@ReqCenterBasedOnly", isCenterBasedOnly));
                    command.Parameters.Add(new SqlParameter("@Homebased", isHomeBasedOnly));
                    command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                    command.Parameters.Add(new SqlParameter("@AllAgencyCenters", allCenters));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetCentersByuserId";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    DataAdapter.Fill(dtCenters);
                    Connection.Close();
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Close();
                }

                DataAdapter.Dispose();
                command.Dispose();
            }
        }

        public void GetClassRoomsByCenterId(ref DataTable dtCenters, string CenterId, string agencyId, string userId)
        {
            dtCenters = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@CenterId", Convert.ToInt64(CenterId)));
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetClassRoomsByCenterId";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtCenters);
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

        //public void GetClassRoomsByCenterId(ref DataTable dtCenters, string CenterId)
        //{
        //    dtCenters = new DataTable();
        //    try
        //    {
        //        command.Parameters.Add(new SqlParameter("@CenterId", Convert.ToInt64(CenterId)));
        //        command.Connection = Connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "USP_GetClassRoomsByCenterId";
        //        DataAdapter = new SqlDataAdapter(command);
        //        DataAdapter.Fill(dtCenters);
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
        //}


        public DaysOffModel GetDaysOffByUser(Guid agencyId, Guid userId, Guid RoleId)
        {
            List<DaysOff> offDaysList = new List<DaysOff>();
            DaysOffModel model = new DaysOffModel();
            DaysOff offDays = new DaysOff();
            _dataset = new DataSet();
            //var jsonSerialiser = new JavaScriptSerializer();
            //jsonSerialiser.MaxJsonLength = Int32.MaxValue;
            model.DaysOffList = new List<DaysOff>();
            model.CenterList = new List<Center>();
            model.ClassRoomList = new List<ClassRoomModel>();
            model.DatesList = new List<string>();
            List<string> datesList = new List<string>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
                command.Parameters.Add(new SqlParameter("@mode", 1));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DaysOff";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        model.DaysOffList = (from DataRow dr in _dataset.Tables[0].Rows
                                             select new DaysOff
                                             {
                                                 AgencyId = new Guid(dr["AgencyId"].ToString()),
                                                 CenterId = Convert.ToInt64(dr["CenterId"].ToString()),
                                                 ClassRoomId = Convert.ToInt64(dr["ClassRoomId"]),
                                                 Enc_CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                                                 Enc_ClassRoomId = EncryptDecrypt.Encrypt64(dr["ClassRoomId"].ToString()),
                                                 RecordType = Convert.ToInt32(dr["RecordType"]),
                                                 //Enum.GetName(typeof(EnumDaysOff), Convert.ToInt64(dr["RecordType"]))
                                                 RecordName = (dr["RecordType"].ToString() == "1") ? "Agency Wide Closure" : (dr["RecordType"].ToString() == "2") ? "Entire Center Closure" : "Classroom Closure",
                                               // FromDate = Convert.ToDateTime(dr["FromDate"]).ToString("MM/dd/yyyy"),
                                                // ToDate = Convert.ToDateTime(dr["ToDate"]).ToString("MM/dd/yyyy"),
                                                  FromDate = dr["FromDate"].ToString(),
                                                 ToDate = dr["ToDate"].ToString(),
                                                 OffDayComments = string.IsNullOrEmpty(dr["OffDayComments"].ToString()) ? "" : dr["OffDayComments"].ToString(),
                                                 DaysOffID = dr["DaysOffID"].ToString(),
                                                 CenterName = string.IsNullOrEmpty(dr["CenterName"].ToString()) ? "" : dr["CenterName"].ToString(),
                                                 ClassRoomName = string.IsNullOrEmpty(dr["ClassRoomName"].ToString()) ? "" : dr["ClassRoomName"].ToString()
                                             }

                                     ).ToList();

                        var list2 = model.DaysOffList.Where(x => x.RecordType == 3).ToList();

                        model.DaysOffList = model.DaysOffList.Select(x => new DaysOff
                        {
                            AgencyId = x.AgencyId,
                            CenterId = x.CenterId,
                            ClassRoomIdArray = model.DaysOffList.Where(a => a.RecordType != 3 && a.RecordType == x.RecordType && a.CenterId == x.CenterId && a.FromDate == x.FromDate && a.ToDate == x.ToDate).Select(a => new ClassRoomModel
                            {
                                CenterId = a.CenterId.ToString(),
                                ClassRoomId = a.ClassRoomId.ToString(),
                                DaysOffId = a.DaysOffID.ToString(),
                                ClassRoomName = a.ClassRoomName
                            }).ToList(),
                            ClassRoomName = x.ClassRoomName,
                            FromDate = x.FromDate,
                            ToDate = x.ToDate,
                            OffDayComments = x.OffDayComments,
                            CenterName = x.CenterName,
                            Enc_CenterId = x.Enc_CenterId,
                            RecordName = x.RecordName,
                            RecordType = x.RecordType,
                            DaysOffID = x.DaysOffID
                        }).Where(x => x.RecordType != 3).ToList();

                        if (list2.Count() > 0)
                        {
                            foreach (var item in list2)
                            {
                                var list3 = list2.Where(x => x.FromDate == item.FromDate && x.ToDate == item.ToDate).ToList();
                                if (list3.Count() > 0)
                                {
                                    string classroomName = string.Join(",", list3.Select(x => x.ClassRoomName).ToArray());
                                    string daysOffId = string.Join(",", list3.Select(x => x.DaysOffID).ToArray());
                                    model.DaysOffList.AddRange((from a in list3
                                                                select new DaysOff
                                                                {
                                                                    AgencyId = a.AgencyId,
                                                                    CenterId = a.CenterId,
                                                                    Enc_CenterId = a.Enc_CenterId,
                                                                    RecordType = a.RecordType,
                                                                    RecordName = a.RecordName,
                                                                    FromDate = a.FromDate,
                                                                    ToDate = a.ToDate,
                                                                    OffDayComments = a.OffDayComments,
                                                                    CenterName = a.CenterName,
                                                                    ClassRoomName = classroomName,
                                                                    ClassRoomIdArray = (from b in list3
                                                                                        select new ClassRoomModel
                                                                                        {
                                                                                            DaysOffId = b.DaysOffID.ToString(),
                                                                                            ClassRoomId = b.ClassRoomId.ToString(),
                                                                                            ClassRoomName = b.ClassRoomName
                                                                                        }).ToList(),
                                                                    DaysOffID = daysOffId
                                                                }).ToList().Take(1));

                                    list2 = list2.Except(list3.ToList()).ToList();

                                }

                            }

                        }

                        model.DaysOffList = model.DaysOffList.Select(a => new DaysOff
                        {
                            AgencyId = a.AgencyId,
                            CenterId = a.CenterId,
                            Enc_CenterId = a.Enc_CenterId,
                            RecordType = a.RecordType,
                            RecordName = a.RecordName,
                            FromDate = a.FromDate,
                            ToDate = a.ToDate,
                            OffDayComments = a.OffDayComments,
                            CenterName = a.CenterName,
                            ClassRoomName = a.ClassRoomName,
                            ClassRoomIdArray = a.ClassRoomIdArray,
                            DaysOffID = a.DaysOffID
                        }).Where(x => x.FromDate == x.FromDate && x.ToDate == x.ToDate).Distinct().ToList();

                        var list = model.DaysOffList.Select(x => new SelectListItem
                        {
                            Text = x.FromDate,
                            Value = x.ToDate
                        }).ToList();



                        if (list.Count() > 0)
                        {


                            foreach (var item in list)
                            {
                                // model.DatesList.AddRange(GetDatesBetween(Convert.ToDateTime(item.Text), Convert.ToDateTime(item.Value)));
                                model.DatesList.AddRange(GetDatesBetween( DateTime.Parse(item.Text, new CultureInfo("en-US", true)), DateTime.Parse(item.Value, new CultureInfo("en-US", true))));
                            }
                            //model.OffDaysString = jsonSerialiser.Serialize(datesList);
                        }

                    }

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        model.CenterList = (from DataRow dr1 in _dataset.Tables[1].Rows
                                            select new Center
                                            {
                                                CenterName = dr1["CenterName"].ToString(),
                                                CenterId = Convert.ToInt32(dr1["CenterId"].ToString())
                                            }).ToList();

                        // model.CenterListString = jsonSerialiser.Serialize(model.CenterList);

                    }
                    if (_dataset.Tables[2].Rows.Count > 0)
                    {

                        model.ClassRoomList = (from DataRow dr2 in _dataset.Tables[2].Rows
                                               select new ClassRoomModel
                                               {
                                                   ClassRoomName = dr2["ClassRoomName"].ToString(),
                                                   ClassRoomId = dr2["ClassRoomId"].ToString(),
                                                   CenterId = dr2["CenterID"].ToString()
                                               }).ToList();
                        // model.ClassRoomListString = jsonSerialiser.Serialize(model.ClassRoomList);
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return model;
        }

        public DaysOffModel InsertDaysOff(DaysOff daysOff)
        {
            DaysOffModel model = new DaysOffModel();
            int rowsAffected = 0;
            try
            {
                DataTable classRoomDt = new DataTable();
                classRoomDt = GetClassRoomDataTable(daysOff.ClassRoomIdArray);

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", daysOff.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", daysOff.CreatedBy));
                command.Parameters.Add(new SqlParameter("@RoleId", daysOff.RoleId));
                command.Parameters.Add(new SqlParameter("@RecordType", daysOff.RecordType));
                command.Parameters.Add(new SqlParameter("@FromDate", daysOff.FromDate));
                command.Parameters.Add(new SqlParameter("@ToDate", daysOff.ToDate));
                command.Parameters.Add(new SqlParameter("@OffDayComments", daysOff.OffDayComments));
                command.Parameters.Add(new SqlParameter("@DaysOffTable", classRoomDt));
                //  command.Parameters.Add(new SqlParameter("@ClassRoomId", (item.ClassRoomId == "0") ? null : item.ClassRoomId));
                command.Parameters.Add(new SqlParameter("@CenterId", (daysOff.CenterId == 0) ? null : daysOff.CenterId));
                command.Parameters.Add(new SqlParameter("@DaysOffId", daysOff.DaysOffID));
                command.Parameters.Add(new SqlParameter("@mode", 2));
                command.Parameters.Add(new SqlParameter("@IsActive", daysOff.IsActive));
                command.Parameters.Add(new SqlParameter("@IsStaffReport", daysOff.IsStaff));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DaysOff";
                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    model = GetDaysOffByUser(daysOff.AgencyId, daysOff.CreatedBy, daysOff.RoleId);
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return model;
        }

        public DaysOffModel DeleteDaysOff(DaysOff daysOff, string[] DaysOffId)
        {
            DaysOffModel model = new DaysOffModel();
            int rowsAffected = 0;
            try
            {
                if (DaysOffId.Count() > 0)
                {
                    foreach (var item in DaysOffId)
                    {
                        if (Connection.State == ConnectionState.Open)
                            Connection.Close();
                        Connection.Open();
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@AgencyId", daysOff.AgencyId));
                        command.Parameters.Add(new SqlParameter("@UserId", daysOff.CreatedBy));
                        command.Parameters.Add(new SqlParameter("@RoleId", daysOff.RoleId));
                        command.Parameters.Add(new SqlParameter("@DaysOffId", item));
                        command.Parameters.Add(new SqlParameter("@mode", 3));
                        command.Connection = Connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "USP_DaysOff";
                        rowsAffected = command.ExecuteNonQuery();
                    }

                    if (rowsAffected > 0)
                    {
                        model = GetDaysOffByUser(daysOff.AgencyId, daysOff.CreatedBy, daysOff.RoleId);
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return model;
        }


        public bool GetOffDayValidation(DaysOff daysOff)
        {
            bool isResult = false;
            try
            {
                DataTable classRoomDayOffdt = new DataTable();
                classRoomDayOffdt = GetClassRoomDataTable(daysOff.ClassRoomIdArray);
                string classroomId = "";
                if (daysOff.ClassRoomIdArray.Count() > 0)
                {
                    classroomId = string.Join(",", daysOff.ClassRoomIdArray.Select(x => x.ClassRoomId).ToArray());
                }

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", daysOff.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", daysOff.CreatedBy));
                command.Parameters.Add(new SqlParameter("@RoleId", daysOff.RoleId));
                command.Parameters.Add(new SqlParameter("@RecordType", daysOff.RecordType));
                command.Parameters.Add(new SqlParameter("@FromDate", daysOff.FromDate));
                command.Parameters.Add(new SqlParameter("@ToDate", daysOff.ToDate));
                command.Parameters.Add(new SqlParameter("@CenterId", daysOff.CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", classroomId));
                command.Parameters.Add(new SqlParameter("@DaysOffId", daysOff.DaysOffID));
                command.Parameters.Add(new SqlParameter("@DaysOffTable", classRoomDayOffdt));
                command.Parameters.Add(new SqlParameter("@mode", 4));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DaysOff";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {

                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        isResult = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["Result"]);
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isResult;
        }


        public List<string> GetDatesBetween(DateTime startDate, DateTime endDate)
        {

            List<string> dates = new List<string>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date.Date.ToString("MM/dd/yyyy"));
            }

            return dates;
        }



        public DataTable GetClassRoomDataTable(List<ClassRoomModel> classRoomList)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("DaysOffId ", typeof(int)),
                    new DataColumn("CenterId",typeof(long)),
                     new DataColumn("ClassRoomId",typeof(long)),
                      new DataColumn("OffDayComments",typeof(string)),
                    new DataColumn("IsActive",typeof(bool))

                });

                if (classRoomList.Count() > 0)
                {
                    foreach (ClassRoomModel classRoom in classRoomList)
                    {

                        dt.Rows.Add(
                            classRoom.DaysOffId,
                            classRoom.CenterId,
                            classRoom.ClassRoomId,
                            classRoom.OffDayComments,
                            !classRoom.Status
                            );
                    }
                }


                return dt;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return dt;
            }

        }


        public bool UpdateDaysOffByYakkr(string yakkrId, string userId, string agencyId, string roleId, string recordType)
        {
            int rowsAffected = 0;
            bool isResult = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                command.Parameters.Add(new SqlParameter("@YakkrId", yakkrId));
                command.Parameters.Add(new SqlParameter("@RecordType", recordType));
                command.Parameters.Add(new SqlParameter("@mode", 5));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DaysOff";
                rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isResult = true;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isResult;
        }


        public ParentInfoModel ParentContactInfo(ParentInfo info)
        {
            ParentInfoModel model = new ParentInfoModel();

            List<ParentInfo> parentInfoList = new List<ParentInfo>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", info.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", info.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", info.RoleId));
                command.Parameters.Add(new SqlParameter("@CenterId", info.CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", info.ClassRoomId));
                command.Parameters.Add(new SqlParameter("@FilterType", info.FilterType));
                command.Parameters.Add(new SqlParameter("@SearchText", string.IsNullOrEmpty(info.SearchText) ? "" : info.SearchText));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEnrolledChildParentInfo";
                _dataset = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        parentInfoList = (from DataRow dr in _dataset.Tables[0].Rows
                                          select new ParentInfo
                                          {
                                              ClientId = dr["ClientId"] == DBNull.Value ? 0 : Convert.ToInt64(dr["ClientId"]),
                                              ChildName = dr["ChildName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ChildName"]),
                                              CenterId = dr["CenterId"]==DBNull.Value?0: Convert.ToInt64(dr["CenterId"]),
                                              ClassRoomId = dr["ClassroomID"]==DBNull.Value?0: Convert.ToInt64(dr["ClassroomID"]),
                                              CenterName = dr["CenterName"]==DBNull.Value?string.Empty:Convert.ToString(dr["CenterName"]),
                                              ClassRoomName = dr["CenterName"] == DBNull.Value ? string.Empty:Convert.ToString(dr["ClassRoomName"]),
                                              PhoneType = dr["PhoneType"] == DBNull.Value ?0:Convert.ToInt32(dr["PhoneType"]),
                                              PhoneNo = dr["Phoneno"]==DBNull.Value?string.Empty:Convert.ToString(dr["Phoneno"]),
                                              IsSms = dr["Sms"]==DBNull.Value?false: Convert.ToBoolean(dr["Sms"]),
                                              ParentName = dr["ParentName"]==DBNull.Value?string.Empty: dr["ParentName"].ToString().Trim(',').Replace(",", "<br>"),
                                              EmailId = dr["EmailId"]==DBNull.Value?string.Empty: dr["EmailId"].ToString().Trim(',').Replace(",", Environment.NewLine),
                                              NoEmail =dr["NoEmail"]==DBNull.Value?false:  Convert.ToBoolean(dr["NoEmail"]),
                                              IsPrimary =dr["IsPrimaryContact"]==DBNull.Value?false: Convert.ToBoolean(dr["IsPrimaryContact"])
                                          }

                                        ).ToList();
                    }

                    if (_dataset.Tables.Count == 2)
                    {

                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            model.CenterList = (from DataRow dr1 in _dataset.Tables[1].Rows
                                                select new Center
                                                {
                                                    CenterId = dr1["CenterId"] == DBNull.Value ? 0 : Convert.ToInt32(dr1["CenterId"]),
                                                    CenterName = dr1["CenterName"] == DBNull.Value ? string.Empty : Convert.ToString(dr1["CenterName"]),
                                                    Enc_CenterId = EncryptDecrypt.Encrypt64(dr1["CenterId"] == DBNull.Value ? "0" : Convert.ToString(dr1["CenterId"]))
                                                }
                                              ).ToList();
                        }
                    }

                    List<ParentInfo> infoList2 = new List<ParentInfo>();
                    List<long> clientIDList = new List<long>();
                    clientIDList = parentInfoList.Select(x => x.ClientId).Distinct().ToList();

                    foreach (var clientId in clientIDList)
                    {
                        infoList2.AddRange(parentInfoList.Select(x => new ParentInfo
                        {
                            ClientId = x.ClientId,
                            ChildName = x.ChildName,
                            CenterId = x.CenterId,
                            ClassRoomId = x.ClassRoomId,
                            CenterName = x.CenterName,
                            ClassRoomName = x.ClassRoomName,
                            HomePhone = string.Join("<br>", parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 1).Select(c => c.PhoneNo).Distinct().ToArray()).Replace("<br>", Environment.NewLine).Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine),
                            MobilePhone = string.Join("<br>", parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 2).Select(c => c.PhoneNo).Distinct().ToArray()).Replace("<br>", Environment.NewLine).Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine),
                            WorkPhone = string.Join("<br>", parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 3).Select(c => c.PhoneNo).Distinct().ToArray()).Replace("<br>", Environment.NewLine).Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine),
                            IsSmsHomePhone = parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 1).Select(c => c.IsSms).Distinct().FirstOrDefault(),
                            IsSmsMobilePhone = parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 2).Select(c => c.IsSms).Distinct().FirstOrDefault(),
                            IsSmsWorkPhone = parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 3).Select(c => c.IsSms).Distinct().FirstOrDefault(),
                            ParentName = string.Join(",", parentInfoList.Where(c => c.ClientId == clientId).Select(c => c.ParentName).Distinct().ToArray()).Replace(",", Environment.NewLine),
                            EmailId = x.EmailId,
                            NoEmail = x.NoEmail,
                            IsPrimary = x.IsPrimary
                        }).Where(x => x.ClientId == clientId).Distinct().Take(1).ToList());
                    }

                    infoList2 = infoList2.Distinct().ToList();
                    model.ParentInfoList = infoList2;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return model;
        }



        public ParentInfoModel GetParentInfoBySearch(ParentInfo info)
        {
            ParentInfoModel model = new ParentInfoModel();

            List<ParentInfo> parentInfoList = new List<ParentInfo>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", info.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", info.UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", info.CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", info.ClassRoomId));
                command.Parameters.Add(new SqlParameter("@FilterType", info.FilterType));
                command.Parameters.Add(new SqlParameter("@SearchText", info.SearchText));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEnrolledChildParentInfo";
                _dataset = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        parentInfoList = (from DataRow dr in _dataset.Tables[0].Rows
                                          select new ParentInfo
                                          {
                                              ClientId = Convert.ToInt64(dr["ClientId"]),
                                              ChildName = dr["ChildName"].ToString(),
                                              CenterId = Convert.ToInt64(dr["CenterId"]),
                                              ClassRoomId = Convert.ToInt64(dr["ClassroomID"]),
                                              CenterName = dr["CenterName"].ToString(),
                                              ClassRoomName = dr["ClassRoomName"].ToString(),
                                              PhoneType = Convert.ToInt32(dr["PhoneType"]),
                                              PhoneNo = dr["Phoneno"].ToString(),
                                              IsSms = Convert.ToBoolean(dr["Sms"]),
                                              ParentName = dr["ParentName"].ToString(),
                                              EmailId = dr["EmailId"].ToString(),
                                              NoEmail = (dr["FatherEmailId"].ToString() == "" && dr["MotherEmailId"].ToString() == "") ? true : false,
                                              IsPrimary = Convert.ToBoolean(dr["IsPrimaryContact"])
                                          }

                                        ).ToList();
                    }

                    List<ParentInfo> infoList2 = new List<ParentInfo>();
                    List<long> clientIDList = new List<long>();
                    clientIDList = parentInfoList.Select(x => x.ClientId).Distinct().ToList();

                    var list2 = parentInfoList.Where(c => c.ClientId == 15023).Select(c => c.PhoneNo).Distinct().ToArray();
                    foreach (var clientId in clientIDList)
                    {
                        //ParentInfo info = new ParentInfo {

                        //    ClientId = parentInfoList.Where(x => x.ClientId == clientId).Select(x => x.ClientId).Distinct().FirstOrDefault(),
                        //    ChildName= parentInfoList.Where(x => x.ClientId == clientId).Select(x => x.ChildName).Distinct().FirstOrDefault(),
                        //    CenterId=
                        //};
                        infoList2.AddRange(parentInfoList.Select(x => new ParentInfo
                        {
                            ClientId = x.ClientId,
                            ChildName = x.ChildName,
                            CenterId = x.CenterId,
                            ClassRoomId = x.ClassRoomId,
                            CenterName = x.CenterName,
                            ClassRoomName = x.ClassRoomName,
                            //  PhoneType = x.PhoneType,
                            HomePhone = string.Join("<br>", parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 1).Select(c => c.PhoneNo).Distinct().ToArray()).Replace("<br>", Environment.NewLine).Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine),
                            MobilePhone = string.Join("<br>", parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 2).Select(c => c.PhoneNo).Distinct().ToArray()).Replace("<br>", Environment.NewLine).Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine),
                            WorkPhone = string.Join("<br>", parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 3).Select(c => c.PhoneNo).Distinct().ToArray()).Replace("<br>", Environment.NewLine).Replace(Environment.NewLine, Environment.NewLine + Environment.NewLine),
                            IsSmsHomePhone = parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 1).Select(c => c.IsSms).Distinct().FirstOrDefault(),
                            IsSmsMobilePhone = parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 2).Select(c => c.IsSms).Distinct().FirstOrDefault(),
                            IsSmsWorkPhone = parentInfoList.Where(c => c.ClientId == clientId && c.PhoneType == 3).Select(c => c.IsSms).Distinct().FirstOrDefault(),
                            // IsSms = x.IsSms,
                            ParentName = string.Join(",", parentInfoList.Where(c => c.ClientId == clientId).Select(c => c.ParentName).Distinct().ToArray()).Replace(",", Environment.NewLine),
                            EmailId = x.EmailId,
                            NoEmail = x.NoEmail,
                            IsPrimary = x.IsPrimary
                        }).Where(x => x.ClientId == clientId).Distinct().Take(1).ToList());
                    }

                    infoList2 = infoList2.Distinct().ToList();
                    model.ParentInfoList = infoList2;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return model;
        }

        public List<Tuple<bool, string, string, string, long, string, string>> GetParentAndManagementEmail(StaffDetails staff, int RecordType, bool isStaff, long centerId, long classRoomId)
        {
            Dictionary<String, String> dictEmail = new Dictionary<string, string>();
            List<Tuple<bool, string, string, string, long, string, string>> list = new List<Tuple<bool, string, string, string, long, string, string>>();

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetParentsAndManagementEmail";
                command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", classRoomId));
                command.Parameters.Add(new SqlParameter("@RecordType", RecordType));
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            string email = !string.IsNullOrEmpty(dr["Email"].ToString()) ? dr["Email"].ToString() : "";
                            list.Add(new Tuple<bool, string, string, string, long, string, string>
                            (true, dr["ParentName"].ToString(), email, dr["CenterName"].ToString(), Convert.ToInt64(dr["ClassRoomID"]), dr["ClassRoomName"].ToString(), dr["PhoneNumber"].ToString()));
                            // dictEmail.Add("Parent" + i.ToString(), !string.IsNullOrEmpty(dr["Email"].ToString()) ? dr["Email"].ToString() : "");
                            i++;
                        }

                    }

                    if (_dataset.Tables[1].Rows.Count > 0 && isStaff == false && RecordType != 3)
                    {
                        int i = 0;
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            string email = !string.IsNullOrEmpty(dr["Email"].ToString()) ? dr["Email"].ToString() : "";
                            list.Add(new Tuple<bool, string, string, string, long, string, string>
                            (false, dr["TeacherName"].ToString(), email, dr["CenterName"].ToString(), Convert.ToInt64(dr["ClassRoomID"]), dr["ClassRoomName"].ToString(), dr["PhoneNumber"].ToString()));

                            //dictEmail.Add("Teacher" + i.ToString(), !string.IsNullOrEmpty(dr["Email"].ToString()) ? dr["Email"].ToString() : "");
                            i++;
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
                Connection.Close();
            }
            return list;
        }




        public List<ClosedInfo> CheckForTodayClosure(Guid? agencyId, Guid userId)
        {
            ClosedInfo info = new ClosedInfo();
            List<ClosedInfo> closedList = new List<ClosedInfo>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Add(new SqlParameter("@Agencyid", agencyId));
                command.Parameters.Add(new SqlParameter("@userid", userId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CheckForTodayCenterClosure";
                SqlDataAdapter da = new SqlDataAdapter(command);
                _dataset = new DataSet();

                da.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        closedList = (from DataRow dr in _dataset.Tables[0].Rows
                                      select new ClosedInfo
                                      {
                                          ClosedToday = Convert.ToInt32(dr["TodayClosed"]),
                                          CenterName = dr["CenterName"].ToString(),
                                          ClassRoomName = dr["ClassRoomName"].ToString(),
                                          AgencyName = dr["AgencyName"].ToString(),
                                          CenterId = Convert.ToInt64(dr["CenterId"]),
                                          ClassRoomId = Convert.ToInt64(dr["ClassRoomId"])
                                      }
                         ).Where(x => x.ClosedToday > 0).ToList();

                    }
                }


                //  closedList = closedList.Where(x => x.ClosedToday > 0).ToList();

                List<ClosedInfo> infoList2 = new List<ClosedInfo>();

                if (closedList.Count() > 0)
                {
                    var centerlist = closedList.Select(x => x.CenterId).Distinct().ToList();

                    if (centerlist.Count() > 0)
                    {
                        foreach (var item in centerlist)
                        {
                            info = new ClosedInfo
                            {
                                ClosedToday = closedList.Where(x => x.CenterId == item).Select(x => x.ClosedToday).FirstOrDefault(),
                                CenterName = string.Join(",", closedList.Where(x => x.CenterId == item).Select(x => x.CenterName).Distinct().ToArray()),
                                ClassRoomName = string.Join(",", closedList.Where(x => x.CenterId == item).Select(x => x.ClassRoomName).Distinct().ToArray()),
                                AgencyName = closedList.Where(x => x.CenterId == item).Select(x => x.AgencyName).FirstOrDefault()
                            };
                            infoList2.Add(info);
                        }

                        closedList = infoList2;
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
                Connection.Close();
            }
            return closedList;
        }


        public void GetSeatsCountByCenter(ref Dictionary<string, int> centerSeatsDictionary, string centerId, string classroomID, bool isEndOfYear)
        {
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();



                using (Connection = connection.returnConnection())
                {
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();

                    Connection.Open();
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetCenterSeatsCountByProgramYear";
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                    command.Parameters.Add(new SqlParameter("@ClassroomID", classroomID));
                    command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    if (_dataset != null && _dataset.Tables.Count>0 && _dataset.Tables[0].Rows.Count > 0)
                    {

                        centerSeatsDictionary.Add("TotalSlots", Convert.ToInt32(_dataset.Tables[0].Rows[0]["SlotPurchased"]));
                        centerSeatsDictionary.Add("ClientsReturningAgency", Convert.ToInt32(_dataset.Tables[0].Rows[0]["ClientsReturningAgency"]));
                        centerSeatsDictionary.Add("ClientsReturningCenter", Convert.ToInt32(_dataset.Tables[0].Rows[0]["ClientsReturningCenter"]));
                        centerSeatsDictionary.Add("AvailableSeats", Convert.ToInt32(_dataset.Tables[0].Rows[0]["AvailableSeats"]));
                        centerSeatsDictionary.Add("OpenSeats", Convert.ToInt32(_dataset.Tables[0].Rows[0]["OpenSeats"]));
                        centerSeatsDictionary.Add("CenterID", Convert.ToInt32(_dataset.Tables[0].Rows[0]["CenterID"]));
                        centerSeatsDictionary.Add("ClassroomID", Convert.ToInt32(_dataset.Tables[0].Rows[0]["ClassroomID"]));
                    }
                    else
                    {
                        centerSeatsDictionary.Add("TotalSlots", 0);
                        centerSeatsDictionary.Add("ClientsReturningAgency", 0);
                        centerSeatsDictionary.Add("ClientsReturningCenter", 0);
                        centerSeatsDictionary.Add("AvailableSeats", 0);
                        centerSeatsDictionary.Add("OpenSeats", 0);
                        centerSeatsDictionary.Add("CenterID", 0);
                        centerSeatsDictionary.Add("ClassroomID", 0);
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
                command.Dispose();
            }
        }


        public string AssignClassroomFutureClients(ref Dictionary<string, Int32> seatsDictionary, string clientIds, string classroomId, string centerId, string classStartDate)
        {
            string result = "0";
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@Clientids", clientIds));
                    command.Parameters.Add(new SqlParameter("@ClassroomID", classroomId));
                    command.Parameters.Add(new SqlParameter("@IsEndOfYear", true));
                    command.Parameters.Add(new SqlParameter("@CenterID", centerId));
                    command.Parameters.Add(new SqlParameter("@ClassStartDate", classStartDate));
                    command.Parameters.Add(new SqlParameter("@result", 0)).Direction = ParameterDirection.Output;
                    command.Connection = Connection;
                    command.CommandText = "USP_AssignClassroomByProgramYear";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    command.ExecuteNonQuery();
                    result = command.Parameters["@result"].Value.ToString();
                    Connection.Close();
                }

                this.GetSeatsCountByCenter(ref seatsDictionary, centerId, classroomId, true);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
            }
            return result;
        }

        public string AcceptClassroomAssignmentClients(string clientIds, string centerId, string classroomID)
        {
            string result = "0";

            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@Clientids", clientIds));
                    command.Parameters.Add(new SqlParameter("@ClassroomID", classroomID));
                    command.Parameters.Add(new SqlParameter("@IsEndOfYear", true));
                    command.Parameters.Add(new SqlParameter("@CenterID", centerId));
                    command.Parameters.Add(new SqlParameter("@result", 0)).Direction = ParameterDirection.Output;
                    command.Connection = Connection;
                    command.CommandText = "USP_ConfirmClassroomAssignment";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    command.ExecuteNonQuery();
                    result = command.Parameters["@result"].Value.ToString();
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
                command.Dispose();
            }
            return result;

        }



        #region check un-scheduled school days exists


        public List<SelectListItem> CheckUnscheduledSchoolDay(StaffDetails staff, UnscheduledSchoolDay unscheduledSchoolDay)
        {

            List<SelectListItem> classList = new List<SelectListItem>();
            try
            {
                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@CenterID", EncryptDecrypt.Decrypt64(unscheduledSchoolDay.CenterID)));
                    command.Parameters.Add(new SqlParameter("@Classrooms", string.Join(",", unscheduledSchoolDay.ClassroomID)));
                    command.Parameters.Add(new SqlParameter("@ClassDate", unscheduledSchoolDay.ClassDate));
                    //  command.Parameters.Add(new SqlParameter("@ReasonID", optionaClassDays.UnscheduledSchoolDayReasonID));
                    //  command.Parameters.Add(new SqlParameter("@ReasonText", optionaClassDays.UnscheduledSchoolDayReason));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_CheckUnscheduledSchoolDayExists";
                    Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                classList.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(reader["ClassroomName"])
                                });
                            }
                        }
                    }


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
                command.Dispose();

            }
            return classList;
        }

        #endregion

        #region Add UN Scheduled School Days
        public bool AddunScheduledSchoolDay(out IEnumerable<UnscheduledSchoolDay> unscheduledSchoolDayList, StaffDetails staff, UnscheduledSchoolDay optionaClassDays, int mode)
        {

            bool isRowsAffected = false;
            unscheduledSchoolDayList = null;
            try
            {
                long centerId = 0;



                centerId = long.TryParse(optionaClassDays.CenterID, out centerId) ? centerId : Convert.ToInt64(EncryptDecrypt.Decrypt64(optionaClassDays.CenterID));

                var dBManager = new FingerprintsDataAccessHandler.DBManager(connection.ConnectionString);

                IDbDataParameter[] parameterArray =
                {

                dBManager.CreateParameter("@AgencyID", staff.AgencyId, DbType.Guid),
                dBManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                dBManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                dBManager.CreateParameter("@CenterID",centerId,DbType.Int64),
                dBManager.CreateParameter("@classrooms",string.Join(",", optionaClassDays.ClassroomID),DbType.AnsiString),
                dBManager.CreateParameter("@ClassDate",optionaClassDays.ClassDate, DbType.String),
                dBManager.CreateParameter("@ReasonID",optionaClassDays.UnscheduledSchoolDayReasonID,DbType.Int64),
                dBManager.CreateParameter("@ReasonText",optionaClassDays.UnscheduledSchoolDayReason,DbType.AnsiString),
                dBManager.CreateParameter("@UnscheduledSchoolDayID",optionaClassDays.UnscheduledSchoolDayID,DbType.Int64),
                dBManager.CreateParameter("@Mode",mode,DbType.Int32),
                dBManager.CreateParameter("@Result",int.MaxValue,0,DbType.Int32, System.Data.ParameterDirection.Output)


            };


                _dataTable = dBManager.GetDataTable("USP_AddUnscheduledSchoolDays", CommandType.StoredProcedure, parameterArray);


                isRowsAffected = Convert.ToInt32(parameterArray.
                                                    Where(x => x.Direction == ParameterDirection.Output && x.ParameterName == "@Result").First().Value) > 0;





                if (_dataTable.Rows.Count > 0)
                {
                    unscheduledSchoolDayList = _dataTable.AsEnumerable().Select(x => new UnscheduledSchoolDay
                    {

                        UnscheduledSchoolDayID = x.Field<Int64>("UnscheduledSchoolDayID"),
                        CenterID = Convert.ToString(x.Field<long>("CenterID")),
                        ClassroomID = new string[] { x.Field<long>("ClassroomID").ToString() }

                    });

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return isRowsAffected;
        }

        #endregion


        #region Gets the Unscheduled School Days List

        public  Task<UnscheduledSchoolDayModal> GetUnScheduledSchoolDays(UnscheduledSchoolDayModal classDaysModel, StaffDetails staff)
        {


            try
            {

                classDaysModel.UnscheduledSchoolDayList = new List<UnscheduledSchoolDay>();
                classDaysModel.ReasonList = new List<SelectListItem>();



                var dbManager = new FingerprintsDataAccessHandler.DBManager(connection.ConnectionString);




                var parameters = new IDbDataParameter[]
                  {
                   dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                   dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                   dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                   dbManager.CreateParameter("@Skip",classDaysModel.SkipRows,DbType.Int32),
                   dbManager.CreateParameter("@Take",classDaysModel.PageSize,DbType.Int32),
                   dbManager.CreateParameter("@SortOrder",classDaysModel.SortOrder,DbType.AnsiString),
                   dbManager.CreateParameter("@SortColumn",classDaysModel.SortColumn,DbType.AnsiString),
                   dbManager.CreateParameter("@TotalRecord",int.MaxValue, classDaysModel.TotalRecord,DbType.Int32,ParameterDirection.Output)

              };


                IDbConnection dbConnection;
                IDataReader reader;
                reader =  dbManager.GetDataReader("USP_GetUnscheduledSchoolDays", CommandType.StoredProcedure, parameters, out dbConnection);

                try
                {


                    while (reader.Read())
                    {
                        classDaysModel.UnscheduledSchoolDayList.Add(new UnscheduledSchoolDay
                        {
                            CenterName = Convert.ToString(reader["CenterName"]),
                            ClassroomName = Convert.ToString(reader["ClassroomName"]),
                            UnscheduledSchoolDayReason = Convert.ToString(reader["UnscheduledSchoolDayReason"]),
                            ClassDate = Convert.ToString(reader["ClassDate"]),
                            UnscheduledSchoolDayID = Convert.ToInt64(reader["UnscheduledSchoolDayID"])
                        });
                    }




                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            classDaysModel.ReasonList.Add(new SelectListItem
                            {
                                Text = Convert.ToString(reader["UnscheduledSchoolDayReason"]),
                                Value = Convert.ToString(reader["UnscheduledSchoolDayReasonID"])
                            });
                        }
                    }

                    classDaysModel.ReasonList.Add(new SelectListItem
                    {
                        Text= "Other",
                        Value= "Other"
                    });

                    classDaysModel.ReasonList.Insert(0, new SelectListItem
                    {
                        Text = "--Select--",
                        Value = "0"
                    });





                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }
                finally
                {
                    reader.Close();
                    dbManager.CloseConnection(dbConnection);
                }

                classDaysModel.TotalRecord = Convert.ToInt32(parameters.Where(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output).First().Value);




            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {

            }

            return Task.FromResult( classDaysModel);
        }

        #endregion


        #region Gets the Email Report for based on selected Unscheduled School Day

        public async Task<Dictionary<int, object>> GetClientEmailReportBy(Email.ClientEmailReport emailReport)
        {

            Dictionary<int, object> dictionary = new Dictionary<int, object>();

            dictionary.Add((int)FingerprintsModel.Enums.EmailStatus.SentEmails, new List<ParentInfo>());
            dictionary.Add((int)FingerprintsModel.Enums.EmailStatus.BouncedEmails, new List<ParentInfo>());
            dictionary.Add((int)FingerprintsModel.Enums.EmailStatus.NoEmails, new List<ParentInfo>());
            dictionary.Add(4, new List<UnscheduledSchoolDay>());



            List<ParentInfo> ChildList = new List<ParentInfo>();
            List<ParentInfo> parentList = new List<ParentInfo>();
            List<ParentInfo> PhoneTypeList = new List<ParentInfo>();
            List<SelectListItem> clientEmailReport = new List<SelectListItem>();


            try
            {
                using (Connection = connection.returnConnection())
                {

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", emailReport.staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", emailReport.staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", emailReport.staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@ReferenceID", emailReport.ReferenceID));
                    command.Parameters.Add(new SqlParameter("@EmailType", (int)FingerprintsModel.Enums.EmailType.UnscheduledSchoolDay));
                    command.Connection = Connection;
                    command.CommandText = "USP_GetClientEmailReport";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {



                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                ChildList.Add(new ParentInfo
                                {
                                    ClientId = Convert.ToInt64(reader["ClientID"]),
                                    ChildName = Convert.ToString(reader["ChildName"]),
                                    HouseholdId = Convert.ToInt64(reader["HouseholdID"])
                                });
                            }

                        }


                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                parentList.Add(new ParentInfo
                                {
                                    ParentId = Convert.ToInt64(reader["ClientID"]),
                                    NoEmail = DBNull.Value == reader["NoEmail"] ? false : Convert.ToBoolean(reader["NoEmail"]),
                                    EmailId = DBNull.Value == reader["EmailID"] ? string.Empty : Convert.ToString(reader["EmailId"]),
                                    ParentName = Convert.ToString(reader["ParentName"]),
                                    HouseholdId = Convert.ToInt64(reader["HouseholdID"])
                                });
                            }
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                PhoneTypeList.Add(new ParentInfo
                                {
                                    ParentId = Convert.ToInt64(reader["ParentID"]),
                                    PhoneNo = DBNull.Value == reader["phoneNo"] ? string.Empty : Convert.ToString(reader["phoneNo"]),
                                    PhoneType = DBNull.Value == reader["PhoneType"] ? 0 : Convert.ToInt32(reader["PhoneType"]),
                                });
                            }
                        }



                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                clientEmailReport.Add(new SelectListItem
                                {
                                    Text = Convert.ToString(reader["ParentID"]),

                                    Value = Convert.ToString(reader["EmailStatus"])
                                });
                            }
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                dictionary[4] = new UnscheduledSchoolDay
                                {
                                    CenterID = Convert.ToString(reader["CenterID"]),
                                    ClassroomID = new string[] { Convert.ToString(reader["ClassroomID"]) },
                                    ClassDate = Convert.ToString(reader["ClassDate"]),
                                    CenterName = Convert.ToString(reader["CenterName"]),
                                    ClassroomName = Convert.ToString(reader["ClassroomName"])

                                };
                            }
                        }










                        var totalList = ChildList.GroupJoin(parentList, c => c.HouseholdId, p => p.HouseholdId, (c, p) => new { c, p })
      .SelectMany(c => c.p.DefaultIfEmpty(), (c, p) => new
      {
          c.c.ClientId,
          c.c.ChildName,
          ParentId = Convert.ToString(p.ParentId)
                                                                  ,
          p.ParentName

                                            ,
          p.EmailId
                                                                  ,
          p.NoEmail
      })
    .GroupJoin(clientEmailReport, cpoo => cpoo.ParentId, ce => ce.Text, (cpoo, ce) => new { cpoo, ce })
                                                                  .SelectMany(cpooc => cpooc.ce.DefaultIfEmpty(), (cpooc, ce) => new
                                                                  {
                                                                      cpooc.cpoo.ClientId,
                                                                      cpooc.cpoo.ChildName,
                                                                      cpooc.cpoo.ParentId,
                                                                      cpooc.cpoo.ParentName,
                                                                      cpooc.cpoo.NoEmail,
                                                                      cpooc.cpoo.EmailId,
                                                                      HomePhone = string.Join("<br>", PhoneTypeList.Where(x => x.PhoneType == 1 && Convert.ToString(x.ParentId) == cpooc.cpoo.ParentId).Select(x => x.PhoneNo).ToArray()),
                                                                      MobilePhone = string.Join("<br>", PhoneTypeList.Where(x => x.PhoneType == 2 && Convert.ToString(x.ParentId) == cpooc.cpoo.ParentId).Select(x => x.PhoneNo).ToArray()),
                                                                      WorkPhone = string.Join("<br>", PhoneTypeList.Where(x => x.PhoneType == 3 && Convert.ToString(x.ParentId) == cpooc.cpoo.ParentId).Select(x => x.PhoneNo).ToArray()),
                                                                      EmailStatus = cpooc.cpoo.NoEmail ? (int)FingerprintsModel.Enums.EmailStatus.NoEmails : ce != null ? Convert.ToInt32(ce.Value) : (int)FingerprintsModel.Enums.EmailStatus.BouncedEmails

                                                                  }).Distinct().ToList();








                        dictionary[(int)FingerprintsModel.Enums.EmailStatus.SentEmails] = totalList.Where(x => x.EmailStatus == (int)FingerprintsModel.Enums.EmailStatus.SentEmails).Select(y => new ParentInfo
                        {
                            ClientId = y.ClientId,
                            ParentId = Convert.ToInt64(y.ParentId),
                            ChildName = y.ChildName,
                            ParentName = y.ParentName,
                            EmailId = y.EmailId,
                            HomePhone = y.HomePhone,
                            MobilePhone = y.MobilePhone,
                            WorkPhone = y.WorkPhone


                        }).Distinct().ToList();



                        dictionary[(int)FingerprintsModel.Enums.EmailStatus.BouncedEmails] = totalList.Where(x => x.EmailStatus == (int)FingerprintsModel.Enums.EmailStatus.BouncedEmails).Select(y => new ParentInfo
                        {
                            ClientId = y.ClientId,
                            ParentId = Convert.ToInt64(y.ParentId),
                            ChildName = y.ChildName,
                            ParentName = y.ParentName,
                            EmailId = y.EmailId,
                            HomePhone = y.HomePhone,
                            MobilePhone = y.MobilePhone,
                            WorkPhone = y.WorkPhone
                        }).Distinct().ToList();


                        dictionary[(int)FingerprintsModel.Enums.EmailStatus.NoEmails] = totalList.Where(x => x.EmailStatus == (int)FingerprintsModel.Enums.EmailStatus.NoEmails).Select(y => new ParentInfo
                        {
                            ClientId = y.ClientId,
                            ParentId = Convert.ToInt64(y.ParentId),
                            ChildName = y.ChildName,
                            ParentName = y.ParentName,
                            EmailId = y.EmailId,
                            HomePhone = y.HomePhone,
                            MobilePhone = y.MobilePhone,
                            WorkPhone = y.WorkPhone
                        }).Distinct().ToList();








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
                command.Dispose();
            }
            return await Task.FromResult(dictionary);


        }

        #endregion

        #region Method to change parent's email address from the email report
        public Task<int> ChangeParentEmail(StaffDetails staff, string parentId, string emailId)
        {

            int referId = 0;
            try
            {

                var dBManager = new FingerprintsDataAccessHandler.DBManager(connection.ConnectionString);


                var parameters = new IDbDataParameter[]
                {
                   dBManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                   dBManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                   dBManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                   dBManager.CreateParameter("@ParentID",parentId,DbType.Int64),
                   dBManager.CreateParameter("@EmailID",emailId,DbType.String)

                };

                referId = dBManager.ExecuteWithScalar<int>("USP_ChangeEmailAddress", CommandType.StoredProcedure, parameters);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            finally
            {

            }

            return Task.FromResult(referId);


        }

        #endregion


        #region Method to check the existence of unsent email parents based one email type



        public bool CheckForUnsentEmailClients(Email.ClientEmailReport emailReport)
        {
            bool isAvailable = true;

            try
            {
                var dbManager = new FingerprintsDataAccessHandler.DBManager(connection.ConnectionString);


                var parameters = new IDbDataParameter[]
             {
                   dbManager.CreateParameter("@AgencyID",emailReport.staffDetails.AgencyId,DbType.Guid),
                   dbManager.CreateParameter("@RoleID",emailReport.staffDetails.RoleId,DbType.Guid),
                   dbManager.CreateParameter("@UserID",emailReport.staffDetails.UserId,DbType.Guid),
                   dbManager.CreateParameter("@EmailType",(int)emailReport.EmailType ,DbType.Int32),
                   dbManager.CreateParameter("@ReferenceId",emailReport.ReferenceID,DbType.String)

             };

              isAvailable=  dbManager.ExecuteWithScalar<bool>("USP_CheckUnsentEmailParentExists", CommandType.StoredProcedure, parameters);



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return isAvailable;
        }

        #endregion

    }
}
