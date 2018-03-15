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

namespace FingerprintsData
{
    public class ExecutiveData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;

        public ExecutiveDashBoard GetExecutiveDetails(string Agencyid, string userid, string Command)
        {
            ExecutiveDashBoard executive = new ExecutiveDashBoard();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@Command", Command));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetEACDashboardDetails";
                command.CommandTimeout = 120;
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    //SeatsandSlots
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        executive.AvailablePercentage = _dataset.Tables[0].Rows[0]["AvailablePercentage"].ToString();
                        executive.AvailableSeat = _dataset.Tables[0].Rows[0]["AvailableSeat"].ToString();
                        executive.YesterDayAttendance = _dataset.Tables[0].Rows[0]["YesterDayAttendance"].ToString();
                        executive.ADA = _dataset.Tables[0].Rows[0]["ADA"].ToString();
                        executive.WaitingList = Math.Round(Convert.ToDouble(_dataset.Tables[0].Rows[0]["WaitingListPercentage"].ToString()), 1).ToString("N1");
                        executive.WaitingListCount = Math.Round(Convert.ToDouble(_dataset.Tables[0].Rows[0]["WaitingListCount"].ToString()), 1).ToString();
                    }
                    //EmployeeBirhday
                    if (_dataset != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            if (dr["Staff"] != null && dr["Staff"].ToString() != "")
                            {
                                executive.EmployeeBirthdayList.Add(new ExecutiveDashBoard.EmployeeBirthday
                                {
                                    Staff = dr["Staff"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(dr["DOB"].ToString()).ToString("MMM-dd")
                                });
                            }
                        }
                    }
                    //EnrolledByProgram
                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[2].Rows)
                        {
                            executive.EnrolledProgramList.Add(new ExecutiveDashBoard.EnrolledProgram
                            {
                                ProgramType = dr["ProgramType"].ToString(),
                                Total = dr["Total"].ToString(),
                                Available = dr["Available"].ToString()
                            });
                        }
                    }
                    //ClassRoomType
                    if (_dataset.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[3].Rows)
                        {
                            executive.ClassRoomTypeList.Add(new ExecutiveDashBoard.ClassRoomType
                            {
                                ClassSession = GetClassSession(dr["ClassSession"].ToString()),
                                Total = dr["Total"].ToString(),
                                Available = dr["Available"].ToString()
                            });
                        }
                    }
                    //MissingScreen
                    if (_dataset != null && _dataset.Tables[4].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[4].Rows)
                        {
                            executive.MissingScreenList.Add(new ExecutiveDashBoard.MissingScreen
                            {
                                Name = dr["Name"].ToString(),
                                Screen = dr["MissingScreen"].ToString()
                            });
                        }
                    }

                    //FamilyOverIncome
                    if (_dataset.Tables[5].Rows.Count > 0)
                    {
                        decimal IncomePercentage = !string.IsNullOrEmpty(_dataset.Tables[5].Rows[0][0].ToString()) ? Convert.ToDecimal(_dataset.Tables[5].Rows[0][0].ToString()) : 0;
                        //decimal Count = _dataset.Tables[5].Rows.Count;
                        //decimal OverIncomeCount = 0;
                        //foreach (DataRow dr in _dataset.Tables[5].Rows)
                        //{
                        //    decimal IncomePercentage = Convert.ToInt32(dr["IncomePercentage"].ToString());
                        //    if (IncomePercentage > 100)
                        //        OverIncomeCount++;
                        //}
                        //decimal Remaining = OverIncomeCount;
                        //Count = (Remaining * 100) / Count;
                        executive.FamilyOverIncome = Math.Round(IncomePercentage, 1).ToString();
                    }
                    if (string.IsNullOrEmpty(executive.FamilyOverIncome))
                        executive.FamilyOverIncome = "0";

                    //Disability
                    if (_dataset.Tables[6].Rows.Count > 0)
                    {
                        //decimal Count = _dataset.Tables[6].Rows.Count;
                        //decimal Disabilities = 0;

                        //foreach (DataRow dr in _dataset.Tables[6].Rows)
                        //{
                        //    int NoOfDisable = Convert.ToInt32(dr["ParentDisable"].ToString());
                        //    if (NoOfDisable == 1)
                        //        Disabilities++;
                        //}
                        //decimal Remaining = Disabilities;
                        //Count = (Remaining * 100) / Count;
                        //executive.DisabilityPercentage = Math.Round(Count, 1).ToString();

                        executive.DisabilityPercentage = _dataset.Tables[6].Rows[0]["DisabilityPercentage"].ToString();
                    }
                    if (string.IsNullOrEmpty(executive.DisabilityPercentage))
                        executive.DisabilityPercentage = "0";

                    //ThermHoursAndDollars
                    if (_dataset.Tables[7].Rows.Count > 0)
                    {
                        decimal Count = _dataset.Tables[7].Rows.Count;
                        executive.ThermHours = _dataset.Tables[7].Rows[0]["TotalHours"].ToString();
                        executive.ThermDollars = _dataset.Tables[7].Rows[0]["Dollars"].ToString();
                    }

                    //CaseNote
                    if (_dataset.Tables[8].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[8].Rows)
                        {
                            executive.listCaseNote.Add(new ExecutiveDashBoard.CaseNote
                            {
                                Month = dr["Month"].ToString(),
                                Percentage = dr["Percentage"].ToString()
                            });
                        }
                    }
                    //Total Hours and Dollers
                    if (_dataset.Tables[9].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[9].Rows)
                        {
                            executive.TotalHours = dr["Hours"].ToString();
                            executive.TotalDollars = dr["Budget"].ToString();
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
                    ClassRoomType = "FullDay>6hours";
                    break;
                case "3":
                    ClassRoomType = "FullDay>10 hours";
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
                DateTime Date = Convert.ToDateTime(date);
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@Date", Date));
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
                DateTime Date = Convert.ToDateTime(date);
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@Date", Date));
                command.Parameters.Add(new SqlParameter("@RoleId", new Guid(Role)));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetSeatsDetailsByDate";
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
                command.Parameters.Add(new SqlParameter("@ProgramYear", inkind.ProgramYear)); ;
                command.Parameters.Add(new SqlParameter("@Hours", inkind.Hours));
                command.Parameters.Add(new SqlParameter("@Budget", inkind.Dollers));
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

        public void GetInkindDetailsByUserId(ref DataTable dtInkind, string UserId, string AgencyId)
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
    }
}
