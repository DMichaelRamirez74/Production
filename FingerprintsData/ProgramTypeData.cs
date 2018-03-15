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

namespace FingerprintsData
{
   public class ProgramTypeData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataReader dataReader = null;
        SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable programdataTable = null;
        public string AddProg(ProgramType info, int mode, Guid userId)//, string AgencyId
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandText = "SP_addprogtype";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@ProgramID", info.ProgramID);
                if(info.AgencyId==null)
                    command.Parameters.AddWithValue("@AgencyId", null);
                else
                command.Parameters.AddWithValue("@AgencyId", info.AgencyId);
                command.Parameters.AddWithValue("@ProgramTypes", (info.ProgramTypes).Trim());
                command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                command.Parameters.AddWithValue("@PIRReport", info.PIRReport);
                command.Parameters.AddWithValue("@Slots", (info.Slots));
                command.Parameters.AddWithValue("@GranteeNumber", (info.GranteeNumber));
                command.Parameters.AddWithValue("@ReferenceProg", (info.ReferenceProg));
               command.Parameters.AddWithValue("@Area", (info.Area));
                command.Parameters.AddWithValue("@MinAge", (info.MinAge));
                command.Parameters.AddWithValue("@MaxAge", (info.MaxAge));
               command.Parameters.AddWithValue("@StartTime", (info.StartTime));
               command.Parameters.AddWithValue("@StopTime", (info.StopTime));
                command.Parameters.AddWithValue("@ProgYear", (info.ProgYear));
                command.Parameters.AddWithValue("@ProgEndYear", (info.ProgEndYear));
                //Changes
                command.Parameters.AddWithValue("@HealthReview", (info.HealthReview));
                //End
                command.Parameters.AddWithValue("@CreatedBy", userId);
                command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                //Connection.Open();
                command.ExecuteNonQuery();
               // Connection.Close();
                return command.Parameters["@result"].Value.ToString();
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
            return command.Parameters["@result"].Value.ToString();
        }


        public List<ProgramType> ProgInfo(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            List<ProgramType> _proglist = new List<ProgramType>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@agencyid", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Prog_list";
                DataAdapter = new SqlDataAdapter(command);
                programdataTable = new DataTable();
                DataAdapter.Fill(programdataTable);
                if (programdataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < programdataTable.Rows.Count; i++)
                    {
                        ProgramType addProgRow = new ProgramType();
                        addProgRow.ProgramID = Convert.ToInt32(programdataTable.Rows[i]["ProgramTypeID"]);
                        addProgRow.ProgramTypes = Convert.ToString(programdataTable.Rows[i]["ProgramType"]);
                        addProgRow.ReferenceProg = Convert.ToString(programdataTable.Rows[i]["ReferenceProg"]);
                        addProgRow.Slots = Convert.ToString(programdataTable.Rows[i]["Slots"]);
                        addProgRow.GranteeNumber = Convert.ToString(programdataTable.Rows[i]["GranteeNumber"]);
                        addProgRow.Description = Convert.ToString(programdataTable.Rows[i]["Description"]);
                       // addProgRow.Area = Convert.ToString(programdataTable.Rows[i]["AreaID"]);
                        addProgRow.MaxAge = Convert.ToInt32(programdataTable.Rows[i]["MaxAge"]);
                        addProgRow.MinAge = Convert.ToInt32(programdataTable.Rows[i]["MinAge"]);
                        addProgRow.Status = Convert.ToInt32(programdataTable.Rows[i]["Status"]);
                        addProgRow.PIRReport = Convert.ToString(programdataTable.Rows[i]["PIRReport"]);
                       // addProgRow.StartTime = Convert.ToString(programdataTable.Rows[i]["StartTime"]);
                        //addProgRow.StopTime = Convert.ToString(programdataTable.Rows[i]["EndTime"]);
                       // addProgRow.CreatedDate = Convert.ToDateTime(programdataTable.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");

                        _proglist.Add(addProgRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _proglist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _proglist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                programdataTable.Dispose();
            }
        }


        public ProgramType Getproginfo(string ProgramID, string UserId)
        {
            ProgramType obj = new ProgramType();
            try
            {
                command.Parameters.Add(new SqlParameter("@ProgramID", ProgramID));
                if(UserId==null)
                    command.Parameters.Add(new SqlParameter("@AgencyId", null));
                else
                command.Parameters.Add(new SqlParameter("@AgencyId", UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_proginfo";
                DataAdapter = new SqlDataAdapter(command);
                programdataTable = new DataTable();
                DataAdapter.Fill(programdataTable);
                if (programdataTable != null && programdataTable.Rows.Count > 0)
                {
                    obj.ProgramID = Convert.ToInt32(programdataTable.Rows[0]["ProgramTypeID"]);
                    
                   // obj.Description = programdataTable.Rows[0]["Description"].ToString();
                   // obj.CreatedDate = Convert.ToDateTime(programdataTable.Rows[0]["DateEntered"]).ToString("MM/dd/yyyy");
                   
                    obj.ProgramTypes = Convert.ToString(programdataTable.Rows[0]["ProgramType"]);
                    obj.ReferenceProg = Convert.ToString(programdataTable.Rows[0]["ReferenceProg"]);
                    obj.Slots = Convert.ToString(programdataTable.Rows[0]["Slots"]);
                    obj.GranteeNumber = Convert.ToString(programdataTable.Rows[0]["GranteeNumber"]);
                    obj.Description = Convert.ToString(programdataTable.Rows[0]["Description"]);
                   // obj.Area = Convert.ToString(programdataTable.Rows[0]["AreaID"]);
                    obj.MaxAge = Convert.ToInt32(programdataTable.Rows[0]["MaxAge"]);
                    obj.MinAge = Convert.ToInt32(programdataTable.Rows[0]["MinAge"]);
                    obj.Status = Convert.ToInt32(programdataTable.Rows[0]["Status"]);
                    obj.PIRReport = Convert.ToString(programdataTable.Rows[0]["PIRReport"]);
                  //  obj.StartTime = Convert.ToString(programdataTable.Rows[0]["StartTime"]);
                  //  obj.StopTime = Convert.ToString(programdataTable.Rows[0]["EndTime"]);
                  //  obj.ProgYear = Convert.ToInt32(programdataTable.Rows[0]["ProgramStartYear"]);
                    if (!DBNull.Value.Equals((programdataTable.Rows[0]["StartTime"])))
                    {
                        obj.StartTime = Convert.ToString(programdataTable.Rows[0]["StartTime"]);
                    }
                    else
                    {
                        obj.StartTime = string.Empty;// Convert.ToInt32(string.Empty);
                    }
                    if (!DBNull.Value.Equals((programdataTable.Rows[0]["EndTime"])))
                    {
                        obj.StopTime = Convert.ToString(programdataTable.Rows[0]["EndTime"]);
                    }
                    else
                    {
                        obj.StopTime = string.Empty;// Convert.ToInt32(string.Empty);
                    }
                    if (!DBNull.Value.Equals((programdataTable.Rows[0]["AreaID"])))
                    {
                        obj.Area = Convert.ToString(programdataTable.Rows[0]["AreaID"]);
                    }
                    else
                    {
                        obj.Area = string.Empty;// Convert.ToInt32(string.Empty);
                    }
                    if (!DBNull.Value.Equals((programdataTable.Rows[0]["ProgramStartYear"])))
                    {
                        obj.ProgYear = Convert.ToInt32(programdataTable.Rows[0]["ProgramStartYear"]);
                    }
                    else
                    {
                        obj.ProgYear = 0;// Convert.ToInt32(string.Empty);
                    }
                    if (!DBNull.Value.Equals((programdataTable.Rows[0]["ProgramendYear"])))
                    {
                        obj.ProgEndYear = Convert.ToInt32(programdataTable.Rows[0]["ProgramendYear"]);
                    }
                    else
                    {
                        obj.ProgEndYear = 0;// Convert.ToInt32(string.Empty);
                    }
                   // obj.ProgEndYear = Convert.ToInt32(programdataTable.Rows[0]["ProgramendYear"]);

                    if (!DBNull.Value.Equals((programdataTable.Rows[0]["HealthReview"])))//Changes
                    {
                        obj.HealthReview = Convert.ToBoolean(programdataTable.Rows[0]["HealthReview"]);
                    }
                    else
                    {
                        obj.HealthReview = false;// Convert.ToInt32(string.Empty);
                    }


                }
                DataAdapter.Dispose();
                command.Dispose();
                programdataTable.Dispose();
                return obj;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return obj;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                programdataTable.Dispose();
            }
        }


        public int updatestatus(string ProgramID, int mode, Guid userId)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Update_ProgramStatus";
                command.Parameters.Add(new SqlParameter("@ProgramID", ProgramID));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@userid", userId));
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

        public ProgramType GetData_AllDropdown(int i = 0, Guid id = new Guid(), ProgramType prog = null)
        {
            ProgramType _prog = new ProgramType();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_RefProg_Dropdowndata";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        List<ProgramType.ReferenceProgInfo> _proglist = new List<ProgramType.ReferenceProgInfo>();
                        //_staff.myList
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProgramType.ReferenceProgInfo obj = new ProgramType.ReferenceProgInfo();
                            obj.Id = dr["ReferenceId"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _proglist.Add(obj);
                        }

                        _prog.refList = _proglist;
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
            return _prog;
        }


        public List<ProgramType> ProgInfoSuperAdmin(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            List<ProgramType> _proglist = new List<ProgramType>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                //command.Parameters.Add(new SqlParameter("@agencyid", null));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Prog_list_SuperAdmin";
                DataAdapter = new SqlDataAdapter(command);
                programdataTable = new DataTable();
                DataAdapter.Fill(programdataTable);
                if (programdataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < programdataTable.Rows.Count; i++)
                    {
                        ProgramType addProgRow = new ProgramType();
                        addProgRow.ProgramID = Convert.ToInt32(programdataTable.Rows[i]["ProgramTypeID"]);
                        addProgRow.ProgramTypes = Convert.ToString(programdataTable.Rows[i]["ProgramType"]);
                        addProgRow.ReferenceProg = Convert.ToString(programdataTable.Rows[i]["ReferenceProg"]);
                        addProgRow.Slots = Convert.ToString(programdataTable.Rows[i]["Slots"]);
                        addProgRow.GranteeNumber = Convert.ToString(programdataTable.Rows[i]["GranteeNumber"]);
                        addProgRow.Description = Convert.ToString(programdataTable.Rows[i]["Description"]);
                        addProgRow.Area = Convert.ToString(programdataTable.Rows[i]["AreaID"]);
                        addProgRow.MaxAge = Convert.ToInt32(programdataTable.Rows[i]["MaxAge"]);
                        addProgRow.MinAge = Convert.ToInt32(programdataTable.Rows[i]["MinAge"]);
                        addProgRow.Status = Convert.ToInt32(programdataTable.Rows[i]["Status"]);
                        addProgRow.PIRReport = Convert.ToString(programdataTable.Rows[i]["PIRReport"]);
                        addProgRow.StartTime = Convert.ToString(programdataTable.Rows[i]["StartTime"]);
                        addProgRow.StopTime = Convert.ToString(programdataTable.Rows[i]["EndTime"]);
                        // addProgRow.CreatedDate = Convert.ToDateTime(programdataTable.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");

                        _proglist.Add(addProgRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _proglist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _proglist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                programdataTable.Dispose();
            }
        }


        public string CopyToAgency(Guid UserID)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_ProgType_Agencies";
                command.Parameters.Add(new SqlParameter("@CreatedBy", UserID));
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
                Connection.Close();
                command.Dispose();
            }
        }
    }
}
