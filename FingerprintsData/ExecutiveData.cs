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
using System.Web;
using System.IO;
using System.Globalization;
using FingerprintsDataAccessHandler;


namespace FingerprintsData
{
    public class ExecutiveData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;

        public ExecutiveDashBoard GetExecutiveDetails(StaffDetails staff, string Command)
        {
            ExecutiveDashBoard executive = new ExecutiveDashBoard();



          
            try
            {


                executive.AcceptanceReason = new List<SelectListItem>();
               

                double doubCheck = 0;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@Command", Command));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                //   command.CommandText = "SP_GetEACDashboardDetails";
                command.CommandText = "USP_GetExecutiveDashboard";
                command.CommandTimeout = 120;
                Connection.Open();


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //Seats and Slots
                    if (reader.HasRows)
                    {


                        while (reader.Read())
                        {
                            executive.AvailablePercentage = reader["AvailablePercentage"].ToString();
                            executive.AvailableSeat = "0";
                            executive.YesterDayAttendance = reader["YesterDayAttendance"].ToString();
                            executive.ADA = "0";
                            // executive.WaitingList = double.TryParse(reader["WaitingListPercentage"].ToString(), out doubCheck) ? Math.Round(Convert.ToDouble(reader["WaitingListPercentage"].ToString()), 1).ToString("N1") : 0.ToString("N1");
                            // executive.WaitingListCount = double.TryParse(reader["WaitingListCount"].ToString(), out doubCheck) ? Math.Round(Convert.ToDouble(reader["WaitingListCount"].ToString()), 1).ToString() : 0.ToString("N1");
                        }
                    }

                    //Waiting List 
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.WaitingList = double.TryParse(reader["WaitingListPercentage"].ToString(), out doubCheck) ? Math.Round(Convert.ToDouble(reader["WaitingListPercentage"].ToString()), 1).ToString("N1") : 0.ToString("N1");
                            executive.WaitingListCount = double.TryParse(reader["WaitingListCount"].ToString(), out doubCheck) ? Math.Round(Convert.ToDouble(reader["WaitingListCount"].ToString()), 1).ToString() : 0.ToString("N1");

                        }
                    }

                    //Waiting List Chart Data //

                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.AcceptanceReason.Add(new SelectListItem
                            {
                                Text = reader["Description"]==DBNull.Value?string.Empty:Convert.ToString(reader["Description"]),
                                Value =reader["Count"]==DBNull.Value?"0":Convert.ToString(reader["Count"])
                            });

                        }
                    }





                    //Employee Birthday
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["Staff"] != null && reader["Staff"].ToString() != "")
                            {
                                executive.EmployeeBirthdayList.Add(new ExecutiveDashBoard.EmployeeBirthday
                                {
                                    Staff = reader["Staff"].ToString(),
                                    DateOfBirth = DateTime.Parse(reader["DOB"].ToString(), new CultureInfo("en-US", true)).ToString("MMM, dd")
                                });
                            }
                        }
                    }


                    //Enrolled By Program
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.EnrolledProgramList.Add(new ExecutiveDashBoard.EnrolledProgram
                            {
                                ProgramType = reader["ProgramType"].ToString(),
                                Total = reader["Total"].ToString(),
                                Available = reader["Available"].ToString()
                            });
                        }

                    }

                    //ClassRoomType
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            executive.NDayScreeningReviewList.Add(new NDaysScreeningReview
                            {


                                ScreeningID = Convert.ToInt32(reader["ScreeningID"]),
                                ScreeningName = Convert.ToString(reader["ScreeningName"]),
                                Completed = reader["Completed"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Completed"]),
                                CompletedButLate = reader["CompletedButLate"]==DBNull.Value?0:Convert.ToInt64(reader["CompletedButLate"]),
                                NotExpired=reader["NotExpired"]==DBNull.Value?0:Convert.ToInt64(reader["NotExpired"]),
                                NotCompletedandLate=reader["NotCompletedandLate"]==DBNull.Value?0:Convert.ToInt64(reader["NotCompletedandLate"])
                            });
                        }
                    }

                    //MissingScreen
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //executive.MissingScreenList.Add(new ExecutiveDashBoard.MissingScreen
                            //{
                            //    Name = reader["Name"].ToString(),
                            //    Screen = reader["MissingScreen"].ToString()
                            //});


                            executive.ScreeningMatrixList.Add(new ScreeningMatrix
                            {
                                ScreeningName = reader["ScreeningName"] == DBNull.Value ? string.Empty: Convert.ToString(reader["ScreeningName"]),
                                UptoDate= reader["UptoDate"]==DBNull.Value?0: Convert.ToInt64(reader["UptoDate"]),
                                Missing= reader["Missing"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Missing"]),
                                Expired= reader["Expired"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Expired"]),
                                Expiring= reader["Expiring"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Expiring"])
                            });
                        }
                    }

                    //FamilyOverIncome
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            decimal IncomePercentage = !string.IsNullOrEmpty(reader[0].ToString()) ? Convert.ToDecimal(reader[0].ToString()) : 0;
                            executive.FamilyOverIncome = Math.Round(IncomePercentage, 1).ToString();
                        }

                    }


                    if (string.IsNullOrEmpty(executive.FamilyOverIncome))
                        executive.FamilyOverIncome = "0";

                    //Disability
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.DisabilityPercentage = reader["DisabilityPercentage"].ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(executive.DisabilityPercentage))
                        executive.DisabilityPercentage = "0";

                    //ThermHoursAndDollars
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.ThermHours = reader["TotalHours"].ToString();
                            executive.ThermDollars = Convert.ToDouble(reader["Dollars"]).ToString();
                        }

                    }





                    //Total Hours and Dollers
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.TotalHours = reader["Hours"].ToString();
                            executive.TotalDollars = reader["Budget"].ToString();
                        }
                    }


                    //Get Center Based and Home Based Enrollment
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.EnrollmentTypeList.Add(new ExecutiveDashBoard.EnrolledByCenterType
                            {
                                Total = Convert.ToString(reader["EnrollmentCount"]),
                                CenterType = ExecutiveDashBoard.GetDescription((FingerprintsModel.Enums.CenterType)Convert.ToInt32(reader["HomeBased"]))
                            });
                        }

                    }


                    executive.EnrollmentTypeList = executive.EnrollmentTypeList.OrderBy(x => x.CenterType).ToList();


                    // Case Note by Month //
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.listCaseNote.Add(new ExecutiveDashBoard.CaseNote
                            {
                                Percentage = Convert.ToDecimal(reader["Percentage"]) == Convert.ToInt32(reader["Percentage"]) ? Convert.ToInt32(reader["Percentage"]).ToString() : Convert.ToString(reader["Percentage"]),
                                Month = Convert.ToString(reader["Month"])
                            });
                        }
                    }

                    // ADA Month Order  and has explanation for ADA less than 85% //
                    if(reader.NextResult() && reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            executive.ADAList.Add(new ExecutiveDashBoard.ADAChart
                            {
                                // Month= FingerprintsModel.EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.Month>(reader["MonthNumber"].ToString()).ToString(),
                                Month=Convert.ToString(reader["Month"]),
                                MonthOrder =Convert.ToInt32(reader["MonthOrder"]),
                                Percentage= Math.Round(Convert.ToDecimal(reader["ADA"]), 1, MidpointRounding.ToEven),
                                MonthNumber=Convert.ToInt32(reader["MonthNumber"]),
                                ExplanationUnderPercentage=Convert.ToString(reader["ExplanationUnderPercentage"]).Trim()
                            });
                        }
                    }


                    /// shows demographic menu for Access roles if the returns 1.

                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            HttpContext.Current.Session["IsDemographic"] = (string.IsNullOrEmpty(reader["ShowDemographic"].ToString())) ? false :
                            Convert.ToString(reader["ShowDemographic"]) == "1" ? true : false;
                        }
                    }

                    /// gets the Program Year Start Date for the Executive Dashboard///
                    if (reader.NextResult() && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            executive.ProgramYearStartDate = Convert.ToString(reader["ProgramYearStartDate"]);
                        }
                    }

                    // gets the Access Section for the Roles

                   if(reader.NextResult() && reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            executive.AccessScreeningMatrix = reader["AccessScreeningMatrix"] == DBNull.Value ? false : Convert.ToBoolean(reader["AccessScreeningMatrix"]);
                            executive.AccessScreeningReview = reader["AccessScreeningReview"] == DBNull.Value ? false : Convert.ToBoolean(reader["AccessScreeningReview"]);
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
                if (Connection != null)
                    Connection.Close();
            }
            return executive;
        }


        public List<ExecutiveDashBoard.CaseNote> CaseNoteChartData(StaffDetails staff)
        {

            List<ExecutiveDashBoard.CaseNote> caseNoteList = new List<ExecutiveDashBoard.CaseNote>();

            try
            {
                using (Connection = connection.returnConnection())
                {

                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@Command", staff.RoleId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetCaseNoteReport";
                    command.CommandTimeout = 120;
                    Connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                caseNoteList.Add(new ExecutiveDashBoard.CaseNote
                                {
                                    Month = Convert.ToString(reader["Month"]),
                                    Percentage = Convert.ToString(reader["Percentage"])

                                });
                            }
                        }
                    }

                    


                }
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
            }

            return caseNoteList;
        }
        public string GetClassSession(string Session)
        {
            string ClassRoomType = string.Empty;
            switch (Session)
            {
                case "0":
                    ClassRoomType = "Morning";
                    break;
                case "1":
                    ClassRoomType = "Afternoon";
                    break;
                case "2":
                    ClassRoomType = "FullDay > 6 hours";
                    break;
                case "3":
                    ClassRoomType = "FullDay > 10 hours";
                    break;
                case "4":
                    ClassRoomType = "Home Based";
                    break;
            }
            return ClassRoomType;
        }

        public void GetSlotsDetailByDate(ref DataTable dtSeatDetails, string Agencyid, string date)
        {
            dtSeatDetails = new DataTable();
            try
            {
             
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@Date", date));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetSlotDetailsByDate";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtSeatDetails);
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

        public void GetActiveProgramYear(ref DataTable dtActiveProgramYear, string AgencyId)
        {
            dtActiveProgramYear = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetActiveProgrmYear";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtActiveProgramYear);
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

        public void GetSeatsDetailByDate(ref DataTable dtSeatDetails, string Agencyid, string date, string Role, string UserId)
        {
            dtSeatDetails = new DataTable();
            try
            {
               
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                    command.Parameters.Add(new SqlParameter("@Date", date));
                    command.Parameters.Add(new SqlParameter("@RoleId", new Guid(Role)));
                    command.Parameters.Add(new SqlParameter("@userid", UserId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_GetSeatsDetailsByDate";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    DataAdapter.Fill(dtSeatDetails);
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
                    Connection.Close();
            }
        }

        public bool SaveInkind(Inkind inkind, string AgencyId, string UserId)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                if (!string.IsNullOrEmpty(inkind.Id))
                    command.Parameters.Add(new SqlParameter("@Id", inkind.Id));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                //command.Parameters.Add(new SqlParameter("@ProgramYear", inkind.ProgramYear)); ;
                command.Parameters.Add(new SqlParameter("@Hours", inkind.Hours));
                command.Parameters.Add(new SqlParameter("@Budget", inkind.Dollers));
                command.Parameters.Add(new SqlParameter("@InkindPeriodID", inkind.InkindPeriodID));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AddInkindBudget";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isInserted = true;
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
            return isInserted;
        }
        public bool DeleteInkind(string Id)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@Id", Id));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DeleteInkindBudget";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isInserted = true;
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
            return isInserted;
        }

        public void GetInkindDetailsByUserId(ref DataTable dtInkind, string UserId, string AgencyId,int inkindPeriodId=0)
        {
            dtInkind = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetInkindBudget";
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@InkindPeriodID", inkindPeriodId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtInkind);
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



        public ExecutiveDashBoard GetAbsenceReport(int? centerid, int? classid, int? clientid, string search = "")
        {


            ExecutiveDashBoard executive = new ExecutiveDashBoard();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                var stf = StaffDetails.GetInstance();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));

                command.Parameters.Add(new SqlParameter("@CenterId", centerid));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", classid));
                command.Parameters.Add(new SqlParameter("@ClientId", clientid));
                command.Parameters.Add(new SqlParameter("@Command", "Report"));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AbsenceReport";
                //command.CommandTimeout = 120;
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    // absence report
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        executive.AbsenceReport = new List<ExecutiveDashBoard.AbsenceByWeek>();
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            //  executive.AbsenceReport.Add( dr["Percentage"].ToString());
                            executive.AbsenceReport.Add(new ExecutiveDashBoard.AbsenceByWeek()
                            {

                                week = dr["weekdate"].ToString(),
                                value = dr["Percentage"].ToString()
                            });

                        }
                    }

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        executive.AttendanceIssuePercentage = _dataset.Tables[1].Rows[0]["attendanceIssuePercentage"].ToString();

                    }


                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }

            return executive;

            }



        public List<SelectObject> GetClientByCenterAndClass(int? centerid, int? classid, string search = "")
        {


            var data = new List<SelectObject>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                var stf = StaffDetails.GetInstance();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));

                command.Parameters.Add(new SqlParameter("@CenterId", centerid));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", classid));
                command.Parameters.Add(new SqlParameter("@Command", "search"));
                command.Parameters.Add(new SqlParameter("@Search", search));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AbsenceReport";
                command.CommandTimeout = 120;
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    // absence report
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                       
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            data.Add(new SelectObject()
                            {
                                id = dr["Id"].ToString(),
                                text = dr["Text"].ToString()
                            });
                          
                           

                        }

                    }
                }
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }

            return data;
        }

        #region Get ADA Daily Attendance percentage

        public void GetADASeatsDaily(ref string adaPercentage,ref string todaySeats)
        {
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();
                DataTable _dataTable = new DataTable();
                if (Connection.State==ConnectionState.Open)
                {
                    Connection.Close();
                }

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetADAPercentageDaily";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    DataAdapter.Fill(_dataTable);
                    Connection.Close();
                }

                if(_dataTable!=null && _dataTable.Rows.Count>0)
                {
                    adaPercentage = Convert.ToString(_dataTable.Rows[0]["ADADailyPercentage"]);
                    todaySeats = Convert.ToString(_dataTable.Rows[0]["AvailableSeats"]);
                }

            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
            }

        }
        #endregion








        #region Refresh Executive Dashboard Section


        public bool RefershExecutiveDashboardBySection(int sectionType, StaffDetails staff)
        {

            bool isRowsAffected = false;
            var dbManager = new DBManager(connection.ConnectionString);
            try
            {


                //using (Connection = connection.returnConnection())
                //{
                // var rowsAffected=   Connection.Execute("USP_RefreshExecutiveDashboard", new { AgencyID = staff.AgencyId, UserID = staff.UserId, RoleID = staff.RoleId, SectionType = sectionType }, commandType: CommandType.StoredProcedure);
                //    isRowsAffected = (rowsAffected > 0);
                //}


                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@SectionType", sectionType));
                    command.Connection = Connection;
                    command.CommandText = "USP_RefreshExecutiveDashboard";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    isRowsAffected = command.ExecuteNonQuery() > 0;

                }



                    //var parameters = new IDbDataParameter[] {

                    //    dbManager.CreateParameter("@AgencyID", staff.AgencyId,DbType.Guid),
                    //    dbManager.CreateParameter("@UserID", staff.UserId, DbType.Guid),
                    //    dbManager.CreateParameter("@RoleID", staff.RoleId, DbType.Guid),
                    //    dbManager.CreateParameter("@SectionType", sectionType, DbType.Int32)
                    //    };

                    //isRowsAffected = Convert.ToBoolean(dbManager.Insert("USP_RefreshExecutiveDashboard", CommandType.StoredProcedure, parameters));

                }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
            }
            return isRowsAffected;
        }


        #endregion


        #region Get the Executive Dashboard by Section

        /// <summary>
        /// method to get the executive dashboard by section
        /// </summary>
        /// <param name="sectionType"></param>
        /// <param name="staff"></param>
        /// <returns></returns>
        public ExecutiveDashBoard GetExecuteDashboardBySection(int sectionType, StaffDetails staff)
        {

            ExecutiveDashBoard dashboard = new ExecutiveDashBoard();
            double doubCheck = 0;
           // IDbConnection dBconnection = null;
            var dBManager = new DBManager(connection.ConnectionString);
            //  IDataReader reader = null;
            try
            {



                //using (Connection = connection.returnConnection())
                //{
                //    var result = Connection.QueryMultiple("USP_GetExecutiveDashboardBySection", new { AgencyID = staff.AgencyId, RoleID = staff.RoleId, UserID = staff.UserId,SectionType=sectionType}, commandType: CommandType.StoredProcedure);
                //    result.ReadFirst();
                //}


                //   var parameters = new IDbDataParameter[]
                //   {
                //   dBManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                //   dBManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                //   dBManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                //   dBManager.CreateParameter("@SectionType",sectionType,DbType.Int32)


                //   };

                //reader = dBManager.GetDataReader("USP_GetExecutiveDashboardBySection", CommandType.StoredProcedure, parameters, out dBconnection);


                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@SectionType", sectionType));
                    command.Connection = Connection;
                    command.CommandText = "USP_GetExecutiveDashboardBySection";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        switch (EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.DashboardSectionType>(sectionType.ToString()))
                        {
                            case FingerprintsModel.Enums.DashboardSectionType.CurrentEnrollment:

                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        dashboard.EnrollmentTypeList.Add(new ExecutiveDashBoard.EnrolledByCenterType
                                        {
                                            Total = Convert.ToString(reader["EnrollmentCount"]),
                                            CenterType = ExecutiveDashBoard.GetDescription((FingerprintsModel.Enums.CenterType)Convert.ToInt32(reader["HomeBased"]))
                                        });
                                    }

                                }



                                dashboard.EnrollmentTypeList = dashboard.EnrollmentTypeList.OrderBy(x => x.CenterType).ToList();




                                break;


                            //Enrolled By Program
                            case FingerprintsModel.Enums.DashboardSectionType.EnrolledByProgram:


                                if(reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        dashboard.EnrolledProgramList.Add(new ExecutiveDashBoard.EnrolledProgram
                                        {
                                            ProgramType = reader["ProgramType"].ToString(),
                                            Total = reader["Total"].ToString(),
                                            Available = reader["Available"].ToString()
                                        });
                                    }
                                }
                              


                                break;

                            //MissingScreen
                            case FingerprintsModel.Enums.DashboardSectionType.MissingScreening:


                                if(reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        //dashboard.MissingScreenList.Add(new ExecutiveDashBoard.MissingScreen
                                        //{
                                        //    Name = reader["Name"].ToString(),
                                        //    Screen = reader["MissingScreen"].ToString()
                                        //});

                                        dashboard.ScreeningMatrixList.Add(new ScreeningMatrix
                                        {

                                            ScreeningName=Convert.ToString(reader["Name"]),
                                            UptoDate=Convert.ToInt64(reader["UptoDate"]),
                                            Missing=Convert.ToInt64(reader["Missing"]),
                                            Expired=Convert.ToInt64(reader["Expired"]),
                                            Expiring=Convert.ToInt64(reader["Expiring"])
                                        });
                                    }
                                }



                                break;

                            //ClassRoomType

                            case FingerprintsModel.Enums.DashboardSectionType.ClassroomType:


                                if(reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        dashboard.ClassRoomTypeList.Add(new ExecutiveDashBoard.ClassRoomType
                                        {
                                            ClassSession = GetClassSession(reader["ClassSession"].ToString()),
                                            Total = reader["Total"].ToString(),
                                            Available = reader["Available"].ToString()
                                        });
                                    }
                                }

                                break;

                            // Case Note Analysis //

                            case FingerprintsModel.Enums.DashboardSectionType.CaseNoteAnalysis:



                                while (reader.Read())
                                {
                                    dashboard.listCaseNote.Add(new ExecutiveDashBoard.CaseNote
                                    {
                                        Month = Convert.ToString(reader["Month"]),
                                        Percentage = Convert.ToDecimal(reader["Percentage"]) == Convert.ToInt32(reader["Percentage"]) ? Convert.ToInt32(reader["Percentage"]).ToString() : Convert.ToString(reader["Percentage"]),


                                    });
                                }


                                break;

                            // In-Kind Hours and Dollars //

                            case FingerprintsModel.Enums.DashboardSectionType.InKindHoursDollars:


                                while (reader.Read())
                                {
                                    dashboard.ThermHours = reader["TotalHours"].ToString();
                                    dashboard.ThermDollars = Convert.ToDouble(reader["Dollars"]).ToString();
                                }




                                //Total Hours and Dollars//

                                if (reader.NextResult())
                                {
                                    while (reader.Read())
                                    {
                                        dashboard.TotalHours = reader["Hours"].ToString();
                                        dashboard.TotalDollars = reader["Budget"].ToString();
                                    }
                                }

                                break;

                            // Disability
                            case FingerprintsModel.Enums.DashboardSectionType.Disabilities:


                                while (reader.Read())
                                {
                                    dashboard.DisabilityPercentage = reader["DisabilityPercentage"].ToString();
                                }


                                if (string.IsNullOrEmpty(dashboard.DisabilityPercentage))
                                    dashboard.DisabilityPercentage = "0";

                                break;

                            // Family Over Income

                            case FingerprintsModel.Enums.DashboardSectionType.OverIncome:




                                while (reader.Read())
                                {
                                    decimal IncomePercentage = !string.IsNullOrEmpty(reader[0].ToString()) ? Convert.ToDecimal(reader[0].ToString()) : 0;
                                    dashboard.FamilyOverIncome = Math.Round(IncomePercentage, 1).ToString();
                                }




                                if (string.IsNullOrEmpty(dashboard.FamilyOverIncome))
                                    dashboard.FamilyOverIncome = "0";


                                break;

                            // Waiting List //

                            case FingerprintsModel.Enums.DashboardSectionType.WaitingList:


                                while (reader.Read())
                                {
                                    dashboard.WaitingList = double.TryParse(reader["WaitingListPercentage"].ToString(), out doubCheck) ? Math.Round(Convert.ToDouble(reader["WaitingListPercentage"].ToString()), 1).ToString("N1") : 0.ToString("N1");
                                    dashboard.WaitingListCount = double.TryParse(reader["WaitingListCount"].ToString(), out doubCheck) ? Math.Round(Convert.ToDouble(reader["WaitingListCount"].ToString()), 1).ToString() : 0.ToString("N1");

                                }
                                dashboard.AcceptanceReason = new List<SelectListItem>();

                                //Waiting List Chart Data //

                                if (reader.NextResult() && reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        dashboard.AcceptanceReason.Add(new SelectListItem
                                        {
                                            Text = reader["Description"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Description"]),
                                            Value = reader["Count"] == DBNull.Value ? "0" : Convert.ToString(reader["Count"])
                                        });

                                    }
                                }


                                break;


                            // Screening review //
                            case FingerprintsModel.Enums.DashboardSectionType.ScreeningReview:

                                while(reader.Read())
                                {
                                    dashboard.NDayScreeningReviewList.Add(new NDaysScreeningReview
                                    {


                                        ScreeningID = Convert.ToInt32(reader["ScreeningID"]),
                                        ScreeningName = Convert.ToString(reader["ScreeningName"]),
                                        Completed = reader["Completed"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Completed"]),
                                        CompletedButLate = reader["CompletedButLate"] == DBNull.Value ? 0 : Convert.ToInt64(reader["CompletedButLate"]),
                                        NotExpired = reader["NotExpired"] == DBNull.Value ? 0 : Convert.ToInt64(reader["NotExpired"]),
                                        NotCompletedandLate = reader["NotCompletedandLate"] == DBNull.Value ? 0 : Convert.ToInt64(reader["NotCompletedandLate"])
                                    });
                                }


                                break;


                            //ADA//
                            case FingerprintsModel.Enums.DashboardSectionType.ADA:

                                // ADA Month Order  and has explanation for ADA less than 85% //
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        dashboard.ADAList.Add(new ExecutiveDashBoard.ADAChart
                                        {
                                            // Month = FingerprintsModel.EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.Month>(reader["MonthNumber"].ToString()).ToString(),
                                            Month=Convert.ToString(reader["Month"]),
                                            MonthOrder = Convert.ToInt32(reader["MonthOrder"]),
                                            Percentage = Math.Round(Convert.ToDecimal(reader["ADA"]), 1, MidpointRounding.ToEven),
                                            MonthNumber = Convert.ToInt32(reader["MonthNumber"]),
                                            ExplanationUnderPercentage = Convert.ToString(reader["ExplanationUnderPercentage"]).Trim()
                                        });
                                    }
                                }
                                break;

                        }
                    }
                }


                //switch (EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.DashboardSectionType>(sectionType.ToString()))
                //{
                //    case FingerprintsModel.Enums.DashboardSectionType.CurrentEnrollment:



                //        while (reader.Read())
                //        {
                //            dashboard.EnrollmentTypeList.Add(new ExecutiveDashBoard.EnrolledByCenterType
                //            {
                //                Total = Convert.ToString(reader["EnrollmentCount"]),
                //                CenterType = ExecutiveDashBoard.GetDescription((ExecutiveDashBoard.CenterTypeEnum)Convert.ToInt32(reader["HomeBased"]))
                //            });
                //        }


                //        dashboard.EnrollmentTypeList = dashboard.EnrollmentTypeList.OrderBy(x => x.CenterType).ToList();




                //        break;


                //    //Enrolled By Program
                //    case FingerprintsModel.Enums.DashboardSectionType.EnrolledByProgram:



                //        while (reader.Read())
                //        {
                //            dashboard.EnrolledProgramList.Add(new ExecutiveDashBoard.EnrolledProgram
                //            {
                //                ProgramType = reader["ProgramType"].ToString(),
                //                Total = reader["Total"].ToString(),
                //                Available = reader["Available"].ToString()
                //            });
                //        }


                //        break;

                //    //MissingScreen
                //    case FingerprintsModel.Enums.DashboardSectionType.MissingScreening:




                //        while (reader.Read())
                //        {
                //            dashboard.MissingScreenList.Add(new ExecutiveDashBoard.MissingScreen
                //            {
                //                Name = reader["Name"].ToString(),
                //                Screen = reader["MissingScreen"].ToString()
                //            });
                //        }


                //        break;

                //    //ClassRoomType

                //    case FingerprintsModel.Enums.DashboardSectionType.ClassroomType:



                //        while (reader.Read())
                //        {
                //            dashboard.ClassRoomTypeList.Add(new ExecutiveDashBoard.ClassRoomType
                //            {
                //                ClassSession = GetClassSession(reader["ClassSession"].ToString()),
                //                Total = reader["Total"].ToString(),
                //                Available = reader["Available"].ToString()
                //            });
                //        }


                //        break;

                //    // Case Note Analysis //

                //    case FingerprintsModel.Enums.DashboardSectionType.CaseNoteAnalysis:



                //        while (reader.Read())
                //        {
                //            dashboard.listCaseNote.Add(new ExecutiveDashBoard.CaseNote
                //            {
                //                Month = Convert.ToString(reader["Month"]),
                //                Percentage = Convert.ToDecimal(reader["Percentage"]) == Convert.ToInt32(reader["Percentage"]) ? Convert.ToInt32(reader["Percentage"]).ToString() : Convert.ToString(reader["Percentage"]),


                //            });
                //        }


                //        break;

                //    // In-Kind Hours and Dollars //

                //    case FingerprintsModel.Enums.DashboardSectionType.InKindHoursDollars:


                //        while (reader.Read())
                //        {
                //            dashboard.ThermHours = reader["TotalHours"].ToString();
                //            dashboard.ThermDollars = Convert.ToDouble(reader["Dollars"]).ToString();
                //        }




                //        //Total Hours and Dollars//

                //        if (reader.NextResult())
                //        {
                //            while (reader.Read())
                //            {
                //                dashboard.TotalHours = reader["Hours"].ToString();
                //                dashboard.TotalDollars = reader["Budget"].ToString();
                //            }
                //        }

                //        break;

                //    // Disability
                //    case FingerprintsModel.Enums.DashboardSectionType.Disabilities:


                //        while (reader.Read())
                //        {
                //            dashboard.DisabilityPercentage = reader["DisabilityPercentage"].ToString();
                //        }


                //        if (string.IsNullOrEmpty(dashboard.DisabilityPercentage))
                //            dashboard.DisabilityPercentage = "0";

                //        break;

                //    // Family Over Income

                //    case FingerprintsModel.Enums.DashboardSectionType.OverIncome:




                //        while (reader.Read())
                //        {
                //            decimal IncomePercentage = !string.IsNullOrEmpty(reader[0].ToString()) ? Convert.ToDecimal(reader[0].ToString()) : 0;
                //            dashboard.FamilyOverIncome = Math.Round(IncomePercentage, 1).ToString();
                //        }




                //        if (string.IsNullOrEmpty(dashboard.FamilyOverIncome))
                //            dashboard.FamilyOverIncome = "0";


                //        break;

                //    // Waiting List //

                //    case FingerprintsModel.Enums.DashboardSectionType.WaitingList:


                //        while (reader.Read())
                //        {
                //            dashboard.WaitingList = double.TryParse(reader["WaitingListPercentage"].ToString(), out doubCheck) ? Math.Round(Convert.ToDouble(reader["WaitingListPercentage"].ToString()), 1).ToString("N1") : 0.ToString("N1");
                //            dashboard.WaitingListCount = double.TryParse(reader["WaitingListCount"].ToString(), out doubCheck) ? Math.Round(Convert.ToDouble(reader["WaitingListCount"].ToString()), 1).ToString() : 0.ToString("N1");

                //        }


                //        break;

                //}
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
           {
                //    dBManager.CloseConnection(dBconnection);

                //    if (reader != null)
                //        reader.Close();

                Connection.Dispose();
                command.Dispose();
            
            }

            return dashboard;
        }

        #endregion


        #region Save ADA Explanation for below 85%


        public bool SaveADAExplanation(StaffDetails staff,int month,string explanation)

        {
            bool isRowsAffected = false;
            try
            {
                var dbManager = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);

                var parameters = new IDbDataParameter[]
                {
                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@MonthNumber",month,DbType.Int32),
                    dbManager.CreateParameter("@Explanation",explanation,DbType.String)

                };

                isRowsAffected = dbManager.ExecuteWithNonQuery<bool>("USP_AddADAExplanation", CommandType.StoredProcedure, parameters);


            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isRowsAffected;
        }
        #endregion
    }


}
