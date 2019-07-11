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
    public class SuperAdminData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataReader dataReader = null;
        SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable agencydataTable = null;
        DataTable _dataTable = null;
        DataSet _dataset = null;
        public string Add_Edit_SuperAdmininfo(SupperAdmin obj, string password, string mode)
        {
            string res = "";
            try
            {
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = Connection;
                        command.CommandText = "Sp_Insert_Update_SuperAdmin";
                        command.Parameters.AddWithValue("@Email", obj.Emailid);
                        //command.Parameters.AddWithValue("@Usrname", DBNull.Value);
                        command.Parameters.AddWithValue("@Fname", obj.FirstName);
                        command.Parameters.AddWithValue("@Lname", obj.LastName);
                        command.Parameters.AddWithValue("@Mob", obj.MobileNumber);
                        command.Parameters.AddWithValue("@CreatBy", obj.UserId);
                        command.Parameters.AddWithValue("@password", EncryptDecrypt.Encrypt(password));
                        command.Parameters.AddWithValue("@mode", mode);
                        command.Parameters.AddWithValue("@SuprAdminId", obj.superadminId);
                        command.Parameters.AddWithValue("@timezoneid", obj.TimeZoneID);
                        command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;

                        //command.Parameters.AddWithValue("@SprAdminId", "0000s0000-0000-0000-0000-000000000000").Direction = ParameterDirection.Output;
                        command.CommandType = CommandType.StoredProcedure;
                        Connection.Open();
                        command.ExecuteNonQuery();
                        Connection.Close();
                        res = command.Parameters["@result"].Value.ToString();
                        //  obj.superadminId = Guid.Parse(Convert.ToString(command.Parameters["@SprAdminId"].Value.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return res;
        }
        public SupperAdmin GetSuperAdminInfoById(Guid id)
        {
            SupperAdmin obj = new SupperAdmin();
            try
            {
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_Single_SuperAdminInfo";
                        command.Parameters.AddWithValue("@SuperAdminId", id);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    obj.FirstName = dt.Rows[0]["FirstName"].ToString();
                    obj.LastName = dt.Rows[0]["LastName"].ToString();
                    obj.superadminId = Guid.Parse(Convert.ToString(dt.Rows[0]["Id"].ToString()));
                    obj.MobileNumber = dt.Rows[0]["MobileNumber"].ToString();
                    obj.Emailid = dt.Rows[0]["emailid"].ToString();
                    //obj.UserName = dt.Rows[0]["userName"].ToString();
                    obj.TimeZoneID = dt.Rows[0]["TimeZone_ID"].ToString();

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return obj;
        }
        public List<SupperAdmin> GetSuperAdminList(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string status, string userid)
        {
            List<SupperAdmin> _Adminlist = new List<SupperAdmin>();
            totalrecord = "";
            try
            {
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_All_SuperAdminInfo";
                        command.Parameters.AddWithValue("@Search", search);
                        command.Parameters.AddWithValue("@take", pageSize);
                        command.Parameters.AddWithValue("@skip", skip);
                        command.Parameters.AddWithValue("@sortcolumn", sortOrder);
                        command.Parameters.AddWithValue("@sortorder", sortDirection);
                        command.Parameters.AddWithValue("@userid", userid);
                        if (status.Contains("0") || status.Contains("1"))
                            command.Parameters.Add(new SqlParameter("@status", DBNull.Value));
                        if (status.Contains("2"))
                            command.Parameters.Add(new SqlParameter("@status", "1"));
                        if (status.Contains("3"))
                            command.Parameters.Add(new SqlParameter("@status", "0"));
                        command.Parameters.AddWithValue("@totalRecord", 0).Direction = ParameterDirection.Output; ;
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                        int rows = dt.Rows.Count;
                        totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SupperAdmin obj = new SupperAdmin();
                        obj.FirstName = dr["Name"].ToString();
                        obj.superadminId = Guid.Parse(Convert.ToString(dr["Id"].ToString()));
                        obj.MobileNumber = Convert.ToString(dr["MobileNumber"]);
                        obj.Emailid = Convert.ToString(dr["emailid"]);
                        obj.status = Convert.ToChar(dr["Status"]);
                        obj.createdDate = Convert.ToDateTime(dr["DateEntered"]).ToString("MM/dd/yyyy");
                        _Adminlist.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return _Adminlist;
        }
        public string Activate_DeactivateSuperAdmin(Guid id, string mode, Guid Loginid)
        {
            string res = "";
            try
            {
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = Connection;
                        command.CommandText = "Sp_Activate_Deactivate_SuperAdmin";
                        command.Parameters.AddWithValue("@LoginID", Loginid);
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@mode", Convert.ToInt32(mode));
                        command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                        command.CommandType = CommandType.StoredProcedure;
                        Connection.Open();
                        command.ExecuteNonQuery();
                        Connection.Close();
                        res = command.Parameters["@result"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return res;
        }
        public Dictionary<string, string> GetSuperAdmindashboard()
        {
            Dictionary<string, string> superadmindashboard = null;
            try
            {
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "SP_superadmindashboard";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    superadmindashboard = new Dictionary<string, string>();
                    superadmindashboard.Add("Totalagency", dt.Rows[0]["totalagency"].ToString());
                    superadmindashboard.Add("Totalactiveagency", dt.Rows[0]["totalactiveagency"].ToString());
                    superadmindashboard.Add("totaldeactiveagency", dt.Rows[0]["totaldeactiveagency"].ToString());
                    superadmindashboard.Add("totalstaff", dt.Rows[0]["totalstaff"].ToString());
                    superadmindashboard.Add("totalactivestaff", dt.Rows[0]["totalactivestaff"].ToString());
                    superadmindashboard.Add("totaldeactivestaff", dt.Rows[0]["totaldeactivestaff"].ToString());
                    superadmindashboard.Add("totalagencyuser", dt.Rows[0]["totalagencyuser"].ToString());
                    superadmindashboard.Add("totalactiveagencyuser", dt.Rows[0]["totalactiveagencyuser"].ToString());
                    superadmindashboard.Add("totaldeactiveagencyuser", dt.Rows[0]["totaldeactiveagencyuser"].ToString());
                    superadmindashboard.Add("totalsuspendedagencyuser", dt.Rows[0]["totalsuspendedagencyuser"].ToString());

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return superadmindashboard;
        }
        #region Immunization
        public string Add_Update_ImmunizationInfo(List<Immunization> obj, Guid UserId, string AgencyId, out List<Immunization> _Immulist)
        {
            string res = "";
            _Immulist = new List<Immunization>();
            try
            {
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = Connection;
                        command.CommandText = "Add_Update_Immunization";
                        command.Parameters.AddWithValue("@LoginId", UserId);
                        if (!String.IsNullOrWhiteSpace(AgencyId))
                        {
                            command.Parameters.AddWithValue("@agencyId", AgencyId);
                        }

                        //command.Parameters.AddWithValue("@Mode", Mode);

                        if (obj != null && obj.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.AddRange(new DataColumn[25]
                                {
                                    new DataColumn("ImmunizationId",typeof(int)),
                                    new DataColumn("ImmunizationType", typeof(string)),
                                    new DataColumn("Number_of_Doses",typeof(int)),
                                    new DataColumn("High_Risk_Group",typeof(bool)),
                                    new DataColumn("Recurring",typeof(bool)), 
                                    new DataColumn("Dose1To",typeof(int)),
                                    new DataColumn("Dose1From",typeof(int)),
                                    new DataColumn("Dose1_Wait_Period",typeof(int)),
                                    new DataColumn("Makeup1_Wait",typeof(int)),
                                    new DataColumn("Dose2To",typeof(int)),
                                    new DataColumn("Dose2From",typeof(int)),
                                    new DataColumn("Dose2_Wait_Period",typeof(int)),
                                    new DataColumn("Makeup2_Wait",typeof(int)),
                                     new DataColumn("Dose3To",typeof(int)),
                                     new DataColumn("Dose3From",typeof(int)),
                                     new DataColumn("Dose3_Wait_Period",typeof(int)),
                                     new DataColumn("Makeup3_Wait",typeof(int)),
                                     new DataColumn("Dose4To",typeof(int)),
                                     new DataColumn("Dose4From",typeof(int)),
                                     new DataColumn("Dose4_Wait_Period",typeof(int)),
                                     new DataColumn("Makeup4_Wait",typeof(int)),
                                     new DataColumn("Dose5To",typeof(int)),
                                     new DataColumn("Dose5From",typeof(int)),
                                     new DataColumn("Dose5_Wait_Period",typeof(int)),
                                     new DataColumn("Makeup5_Wait",typeof(int)),
                    });

                            foreach (Immunization data in obj)
                            {
                                dt.Rows.Add(data.ImmunizationId, data.ImmunizationType, data.NoofDoses, data.HighRiskGroup, data.Recurring, data.Dose1To, data.Dose1From, data.Dose1WaitPeriod, data.Makeup1Wait,
                                        data.Dose2To, data.Dose2From, data.Dose2WaitPeriod, data.Makeup2Wait, data.Dose3To, data.Dose3From, data.Dose3WaitPeriod, data.Makeup3Wait,
                                         data.Dose4To, data.Dose4From, data.Dose4WaitPeriod, data.Makeup4Wait, data.Dose5To, data.Dose5From, data.Dose5WaitPeriod, data.Makeup5Wait);
                            }
                            command.Parameters.Add(new SqlParameter("@tblImmunization", dt));
                        }

                        command.Parameters.Add("@RETURNRESULT", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                        //command.Parameters.AddWithValue("@RETURNRESULT", "").Direction = ParameterDirection.Output;
                        command.CommandType = CommandType.StoredProcedure;
                        Connection.Open();
                        // command.ExecuteNonQuery();
                        DataTable dt1 = new DataTable();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt1);

                        if (dt1.Rows.Count > 0)
                        {
                            Immunization obj1 = null;
                            foreach (DataRow dr in dt1.Rows)
                            {
                                obj1 = new Immunization();
                                obj1.ImmunizationId = Convert.ToInt32(dr["Immunizationid"]);
                                obj1.ImmunizationType = Convert.ToString(dr["Immunization"].ToString());
                                obj1.HighRiskGroup = Convert.ToBoolean(dr["High_Risk_Group"]);
                                obj1.Recurring = Convert.ToBoolean(dr["Recurring"]);
                                obj1.NoofDoses = Convert.ToInt32(dr["Number_of_Doses"]);

                                obj1.Dose1To = Convert.ToString(dr["Dose1To"]);
                                obj1.Dose1From = Convert.ToString(dr["Dose1From"]);
                                obj1.Dose1WaitPeriod = Convert.ToString(dr["Dose1_Wait_Period"]);
                                obj1.Makeup1Wait = Convert.ToString(dr["Makeup1_Wait"]);

                                obj1.Dose2To = Convert.ToString(dr["Dose2To"]);
                                obj1.Dose2From = Convert.ToString(dr["Dose2From"]);
                                obj1.Dose2WaitPeriod = Convert.ToString(dr["Dose2_Wait_Period"]);
                                obj1.Makeup2Wait = Convert.ToString(dr["Makeup2_Wait"]);

                                obj1.Dose3To = Convert.ToString(dr["Dose3To"]);
                                obj1.Dose3From = Convert.ToString(dr["Dose3From"]);
                                obj1.Dose3WaitPeriod = Convert.ToString(dr["Dose3_Wait_Period"]);
                                obj1.Makeup3Wait = Convert.ToString(dr["Makeup3_Wait"]);

                                obj1.Dose4To = Convert.ToString(dr["Dose4To"]);
                                obj1.Dose4From = Convert.ToString(dr["Dose4From"]);
                                obj1.Dose4WaitPeriod = Convert.ToString(dr["Dose4_Wait_Period"]);
                                obj1.Makeup4Wait = Convert.ToString(dr["Makeup4_Wait"]);

                                obj1.Dose5To = Convert.ToString(dr["Dose5To"]);
                                obj1.Dose5From = Convert.ToString(dr["Dose5From"]);
                                obj1.Dose5WaitPeriod = Convert.ToString(dr["Dose5_Wait_Period"]);
                                obj1.Makeup5Wait = Convert.ToString(dr["Makeup5_Wait"]);
                                //  obj.DateEntered = Convert.ToDateTime(dr["DateEntered"]).ToString("MM/dd/yyyy");
                                _Immulist.Add(obj1);
                            }
                        }
                        Connection.Close();
                        res = command.Parameters["@RETURNRESULT"].Value.ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                clsError.WriteException(ex);
            }
            return res;
        }

        public List<Immunization> GetListImmunizationInfo(string AgencyID)
        {
            List<Immunization> _Immulist = new List<Immunization>();
            try
            {
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        dt = new DataTable();
                        command.Connection = Connection;

                        command.CommandText = "SP_Get_ImmunizationList";
                        command.CommandType = CommandType.StoredProcedure;
                        if (!String.IsNullOrWhiteSpace(AgencyID))
                        {
                            command.Parameters.AddWithValue("@agencyId", AgencyID);
                        }
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                        int rows = dt.Rows.Count;
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Immunization obj = new Immunization();
                        obj.ImmunizationId = Convert.ToInt32(dr["Immunizationid"]);
                        obj.ImmunizationType = Convert.ToString(dr["Immunization"].ToString());
                        obj.HighRiskGroup = Convert.ToBoolean(dr["High_Risk_Group"]);
                        obj.Recurring = Convert.ToBoolean(dr["Recurring"]);
                        obj.NoofDoses = Convert.ToInt32(dr["Number_of_Doses"]);

                        obj.Dose1To = Convert.ToString(dr["Dose1To"]);
                        obj.Dose1From = Convert.ToString(dr["Dose1From"]);
                        obj.Dose1WaitPeriod = Convert.ToString(dr["Dose1_Wait_Period"]);
                        obj.Makeup1Wait = Convert.ToString(dr["Makeup1_Wait"]);

                        obj.Dose2To = Convert.ToString(dr["Dose2To"]);
                        obj.Dose2From = Convert.ToString(dr["Dose2From"]);
                        obj.Dose2WaitPeriod = Convert.ToString(dr["Dose2_Wait_Period"]);
                        obj.Makeup2Wait = Convert.ToString(dr["Makeup2_Wait"]);

                        obj.Dose3To = Convert.ToString(dr["Dose3To"]);
                        obj.Dose3From = Convert.ToString(dr["Dose3From"]);
                        obj.Dose3WaitPeriod = Convert.ToString(dr["Dose3_Wait_Period"]);
                        obj.Makeup3Wait = Convert.ToString(dr["Makeup3_Wait"]);

                        obj.Dose4To = Convert.ToString(dr["Dose4To"]);
                        obj.Dose4From = Convert.ToString(dr["Dose4From"]);
                        obj.Dose4WaitPeriod = Convert.ToString(dr["Dose4_Wait_Period"]);
                        obj.Makeup4Wait = Convert.ToString(dr["Makeup4_Wait"]);

                        obj.Dose5To = Convert.ToString(dr["Dose5To"]);
                        obj.Dose5From = Convert.ToString(dr["Dose5From"]);
                        obj.Dose5WaitPeriod = Convert.ToString(dr["Dose5_Wait_Period"]);
                        obj.Makeup5Wait = Convert.ToString(dr["Makeup5_Wait"]);
                        //  obj.DateEntered = Convert.ToDateTime(dr["DateEntered"]).ToString("MM/dd/yyyy");
                        _Immulist.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return _Immulist;
        }

        public bool Delete_ImmunizationInfo(int ImmunizationId, string AgencyId, string userId)
        {
            try
            {
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = Connection;
                        command.CommandText = "sp_delete_Immunization";
                        command.Parameters.AddWithValue("@Immunization", ImmunizationId);
                        if (!String.IsNullOrWhiteSpace(AgencyId))
                        {
                            command.Parameters.AddWithValue("@agencyId", AgencyId);
                        }
                        command.Parameters.AddWithValue("@CreatedBy", userId);
                        command.CommandType = CommandType.StoredProcedure;
                        Connection.Open();
                        command.ExecuteNonQuery();
                        Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return true;
        }

        #endregion
        public AgencyStaff GetData_AllDropdown(string agencyid = "", int i = 0, Guid id = new Guid(), AgencyStaff staff = null)
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            AgencyStaff _staff = new AgencyStaff();

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
                        if (!string.IsNullOrEmpty(agencyid))
                            command.Parameters.Add(new SqlParameter("@agencyID", agencyid));
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }



                if (ds.Tables[11] != null && ds.Tables[11].Rows.Count > 0)
                {

                    List<UserInfo> userlist = new List<UserInfo>();
                    foreach (DataRow dr in ds.Tables[11].Rows)
                    {
                        UserInfo obj = new UserInfo();
                        obj.userId = (dr["ID"]).ToString();
                        obj.Name = dr["Name"].ToString();
                        userlist.Add(obj);
                    }
                    _staff.UserList = userlist;
                }

                //End


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            //  _agencyStafflist.Add(_staff);
            return _staff;
        }
        public string AssignClass(AgencyStaff info, int mode, string userId, List<FingerprintsModel.UserInfo> UserList)//, List<Agency.FundSource.ProgramType> Prog, Guid userId
        {
            string result = string.Empty;
            try
            {
                SqlCommand commandAK = new SqlCommand();
                string agencyCode = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                commandAK.Connection = Connection;
                tranSaction = Connection.BeginTransaction();
                command.Connection = Connection;
                command.Transaction = tranSaction;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_assignClass";
                command.Parameters.Add(new SqlParameter("@ClassroomID", info.Classrooms));
                command.Parameters.Add(new SqlParameter("@ClassAssignID", info.ClassAssignID));//Changes
                command.Parameters.Add(new SqlParameter("@AgencyId", info.SelectedAgencyId));
                command.Parameters.Add(new SqlParameter("@centers", info.centerlist));
                command.Parameters.Add(new SqlParameter("@Users", info.Users));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@createdBy", userId));
                DataTable dt2 = new DataTable();
                dt2.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("ClassroomID", typeof(string)),
                    new DataColumn("Users",typeof(string)) 
                   //   new DataColumn("Id",typeof(string))
                    });
                foreach (FingerprintsModel.UserInfo users in UserList)
                {
                    if (users.userId != null)
                    {
                        dt2.Rows.Add(users.ClassroomID, users.userId);//,users.ClassAssignId
                    }
                }
                command.Parameters.Add(new SqlParameter("@tblusers", dt2));
                command.ExecuteNonQuery();
                tranSaction.Commit();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                if (tranSaction != null)
                    tranSaction.Rollback();
                clsError.WriteException(ex);
                return ex.Message;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
                Connection.Close();
                command.Dispose();
            }
        }
        //Changes
        public string AddYakkrDetails(Yakkr info, int mode, Guid userId)
        {
            try
            {
                command.Connection = Connection;
                command.CommandText = "SP_addyakkrdetails";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@YakkrRoleID", info.YakkrRoleID);
                command.Parameters.AddWithValue("@YakkrID", info.YakkrID);
                //  command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Value", (info.Value));
                command.Parameters.AddWithValue("@Description", info.Description == null ? null : info.Description.Trim());
                //command.Parameters.AddWithValue("@StaffRoleID", info.StaffRoleID);
                command.Parameters.AddWithValue("@IsEnableEmail", info.IsEnableEmail);
                command.Parameters.AddWithValue("@CreatedBy", userId);
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
        public List<Yakkr> YakkrInfoDetails(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            List<Yakkr> _yakkrlist = new List<Yakkr>();
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
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_yakkr_details";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        Yakkr addYakkrRow = new Yakkr();
                        addYakkrRow.YakkrRoleID = Convert.ToInt32(agencydataTable.Rows[i]["Yakkrid"]);
                        //addYakkrRow.YakkrID = Convert.ToString(yakkrdataTable.Rows[i]["YakkrID"]);
                        addYakkrRow.YakkrID = Convert.ToString(agencydataTable.Rows[i]["YakkrCode"]);
                        addYakkrRow.Description = Convert.ToString(agencydataTable.Rows[i]["Description"]);
                        addYakkrRow.Value = Convert.ToString(agencydataTable.Rows[i]["Value"]);
                        addYakkrRow.IsEnableEmail = Convert.ToBoolean(agencydataTable.Rows[i]["IsEnableEmail"]);
                        //addYakkrRow.SecondaryRoleID = Convert.ToString(agencydataTable.Rows[i]["OptionalRoleID"]);
                        //addYakkrRow.StaffRoleName = Convert.ToString(agencydataTable.Rows[i]["StaffRoleName"]);
                        //if (Convert.ToString(agencydataTable.Rows[i]["OptionalRole"]) != null)
                        //    addYakkrRow.OptionalRoleName = Convert.ToString(agencydataTable.Rows[i]["OptionalRole"]);
                        //else
                        //    addYakkrRow.OptionalRoleName = string.Empty;
                        //addYakkrRow.DateEntered = Convert.ToDateTime(agencydataTable.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");

                        _yakkrlist.Add(addYakkrRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _yakkrlist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _yakkrlist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public Yakkr Getyakkrinfo(string YakkrRoleID)
        {
            Yakkr obj = new Yakkr();
            try
            {
                command.Parameters.Add(new SqlParameter("@YakkrRoleID", YakkrRoleID));
                // command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getyakkrdetails";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable != null && agencydataTable.Rows.Count > 0)
                {
                    obj.YakkrRoleID = Convert.ToInt32(agencydataTable.Rows[0]["Yakkrid"]);
                    obj.Value = agencydataTable.Rows[0]["Value"].ToString();
                    obj.IsEnableEmail = Convert.ToBoolean(agencydataTable.Rows[0]["IsEnableEmail"]);
                    obj.Description = agencydataTable.Rows[0]["Description"].ToString();
                    // obj.DateEntered = Convert.ToDateTime(agencydataTable.Rows[0]["DateEntered"]).ToString("MM/dd/yyyy");
                    //  obj.SecondaryRoleID = agencydataTable.Rows[0]["OptionalRoleID"].ToString();
                    obj.YakkrID = agencydataTable.Rows[0]["YakkrCode"].ToString();
                    // obj.FormalAgreement = Convert.ToBoolean(yakkrdataTable.Rows[0]["FormalAgreement"].ToString());



                }
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
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
                agencydataTable.Dispose();
            }
        }
        public string Deleteyakkrinfo(string YakkrId)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteYakkrinfo";
                command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
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
        public string AddSlots(string AgencyId, int Slot, string ProgramYear, int SlotId, string userId, out string UEmail,out string UName, out List<string> EmailList )
        {
            UEmail = ""; UName = "";
            string email = string.Empty;
            EmailList = new List<string>();
            try
            {
              
                command.Connection = Connection;
                Connection.Open();
                command.CommandText = "SP_AddSlots";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@SlotId", SlotId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);

                command.Parameters.AddWithValue("@Slot", Slot);
                command.Parameters.AddWithValue("@ProgramYear", ProgramYear);
                command.Parameters.AddWithValue("@CreatedBy", userId); 
                command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                command.Parameters.AddWithValue("@UEmail", "").Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                                         
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                string email1 = "", firstname = "", lastname = "";
                if(_dataset.Tables[0] !=null && _dataset.Tables[0].Rows.Count>0)
                {

                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        // lstItems.Add(new SelectListItem { Text = dr["Category"].ToString(), Value = dr["Category"].ToString() });

                        email1 = (dr["EmailAddress"] == DBNull.Value) ? "" : dr["EmailAddress"].ToString();
                        firstname = (dr["FirstName"] == DBNull.Value) ? "" : dr["FirstName"].ToString();
                        lastname = (dr["LastName"] == DBNull.Value) ? "" : dr["LastName"].ToString();
                        string str = " " + email1 + " (" + firstname + " " + lastname + ")";
                        EmailList.Add(str);
                    }
                   
                }
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        email1 = (dr["EmailAddress"] == DBNull.Value) ? "" : dr["EmailAddress"].ToString();
                        firstname = (dr["FirstName"] == DBNull.Value) ? "" : dr["FirstName"].ToString();
                        lastname = (dr["LastName"] == DBNull.Value) ? "" : dr["LastName"].ToString();
                        UName = (dr["AgencyName"] == DBNull.Value) ? "" : dr["AgencyName"].ToString();

                        string str = " " + email1 +" (" + firstname + " " + lastname + ")"   ;
                        EmailList.Add(str);
                    }
                }

                UEmail = String.Join(",", EmailList.ToArray());

               

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
        public LSlots SlotDetails(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            LSlots slot = new LSlots();
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
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_SlotList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                bindSlotgrid(_dataset, slot);
                totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                return slot;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return slot;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();

            }
        }
        private void bindSlotgrid(DataSet _ds, LSlots Slots)
        {
            List<Slots> _slots = new List<Slots>();
            List<FamilyHousehold.Programdetail> Programs = new List<FamilyHousehold.Programdetail>();
            if (_dataset != null && _dataset.Tables.Count > 0)
            {

                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    Slots obj = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        obj = new Slots();
                        obj.SlotId = Convert.ToInt32(dr["slotid"]);
                        obj.AgencyName = dr["AgencyName"].ToString();
                        obj.AgencyId = dr["agencyid"].ToString();
                        obj.ProgramYear = dr["Programyear"].ToString();
                        obj.Slot = dr["SlotNumber"].ToString();
                        obj.Createddate = Convert.ToDateTime(dr["DateEntered"]).ToString("MM/dd/yyyy");
                        _slots.Add(obj);
                    }
                    Slots.Slots = _slots;
                }
            }


        }
        public string DeleteSlot(string SlotId, string UserId)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteSlot";
                command.Parameters.Add(new SqlParameter("@SlotId", SlotId));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
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
        public string DeleteFund(string FundId, string Agencyid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deletefund";
                command.Parameters.Add(new SqlParameter("@FundId", FundId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
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
                if (Connection != null)
                    Connection.Close();
                command.Dispose();

            }
        }
        public string DeleteFundA(string FundId, string Agencyid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deletefundA";
                command.Parameters.Add(new SqlParameter("@FundId", FundId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
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
                if (Connection != null)
                    Connection.Close();
                command.Dispose();

            }
        }
        public string DeleteProg(string progId, string Agencyid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteProgram";
                command.Parameters.Add(new SqlParameter("@progId", progId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
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
                if (Connection != null)
                    Connection.Close();
                command.Dispose();

            }
        }
        public string DeleteProgA(string progId, string Agencyid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteProgramA";
                command.Parameters.Add(new SqlParameter("@progId", progId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
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
                if (Connection != null)
                    Connection.Close();
                command.Dispose();

            }
        }
        public string AddScreeningquestion(string ScreeningId, string ScreeningName, List<Questions> Questionlist, string AgencyId, string Userid, string Programtype, bool Document, string ScreeningDate, bool Inintake, string expiredPeriod, string expireIn, string screeningsYear
          //  , List<ScreeningAccess> _screeningAccessList
         ,int screeningReportPeriodID,bool report   )
        {
            string result = string.Empty;
            try
            {




                Questionlist = Questionlist.OrderBy(x => x.QuestionOrder).ToList();

                double lastvalue = 0;
                foreach (var item in Questionlist)
                {
                    var or = item.QuestionOrder;

                    if (lastvalue == 0 && Math.Floor(item.QuestionOrder) < or)
                    {
                        lastvalue = Math.Ceiling(item.QuestionOrder);
                    }

                    else if (lastvalue == 0 && Math.Floor(item.QuestionOrder) == or)
                    {
                        lastvalue = item.QuestionOrder;
                    }
                    else if (lastvalue > 0 && ((lastvalue - item.QuestionOrder) == 0 ||

                        (Math.Abs(lastvalue - item.QuestionOrder) == 1) || (Math.Abs(lastvalue - item.QuestionOrder) == 0.5)))
                    {
                        lastvalue = (lastvalue + 1);
                    }

                    else if (lastvalue > 0 && Math.Abs(lastvalue - item.QuestionOrder) > 1)
                    {
                        lastvalue = (lastvalue + 2);
                    }

                    item.QuestionOrder = lastvalue;
                }





                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@UserId", Userid));
                command.Parameters.Add(new SqlParameter("@agencyid", string.IsNullOrEmpty(AgencyId)? (Guid?)null:new Guid(AgencyId)));
                command.Parameters.Add(new SqlParameter("@ScreeningId", ScreeningId));
                command.Parameters.Add(new SqlParameter("@ScreeningName", ScreeningName));
                command.Parameters.Add(new SqlParameter("@Programtype", Programtype));
                command.Parameters.Add(new SqlParameter("@Document", Document == true ? 1 : 0));
                command.Parameters.Add(new SqlParameter("@ScreeningDate", ScreeningDate));
                command.Parameters.Add(new SqlParameter("@Inintake", Inintake == true ? 1 : 0));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters.Add(new SqlParameter("@Expiredperiod", expiredPeriod));
                command.Parameters.Add(new SqlParameter("@ExpireIn", expireIn));
                command.Parameters.Add(new SqlParameter("@ScreeningsYear", screeningsYear));
                command.Parameters.Add(new SqlParameter("@ScreeningReportPeriodID", screeningReportPeriodID));
                command.Parameters.Add(new SqlParameter("@Report", report));


                command.Parameters["@result"].Direction = ParameterDirection.Output;

                //DataTable dtAccess = new DataTable();

                //dtAccess.Columns.AddRange(new DataColumn[3]
                //{
                //    new DataColumn("RoleId",typeof(Guid)),
                //    new DataColumn("ScreeningAccessType",typeof(int)),
                //    new DataColumn("Status",typeof(bool))
                //});


                //if (_screeningAccessList != null && _screeningAccessList.Count > 0)
                //{
                //    //foreach(var item2 in _screeningAccessList)
                //    //{
                //    //    dtAccess.Rows.Add(item2.RoleID, FingerprintsModel.Enums.ScreeningAccess.Enter, item2.ScreeningAccessType);
                //    //    dtAccess.Rows.Add(item2.RoleID, FingerprintsModel.Enums.ScreeningAccess.Review, item2.ScreeningAccessType);
                //    //    dtAccess.Rows.Add(item2.RoleID, FingerprintsModel.Enums.ScreeningAccess.ViewOnly, item2.ScreeningAccessType);
                //    //}

                //    foreach (var item2 in _screeningAccessList)
                //    {
                //        dtAccess.Rows.Add(item2.RoleID, item2.ScreeningAccessType, true);
                //    }
                //}


                if (Questionlist != null && Questionlist.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[10] {
                    new DataColumn("QuestionId", typeof(int)),
                    new DataColumn("Question",typeof(string)),
                    new DataColumn("QuestionType",typeof(int)),
                    new DataColumn("Option",typeof(string)),
                    new DataColumn("Optionid",typeof(string)),
                    new DataColumn("OptionDescription",typeof(string)),
                    new DataColumn("OptionValue",typeof(string)),
                    new DataColumn("Required",typeof(bool)),
                    new DataColumn("QuestionOrder",typeof(int)),
                    new DataColumn("StatusQuestion",typeof(bool))
                    });

                    foreach (var item in Questionlist)
                    {
                        StringBuilder _optionsid = new StringBuilder();
                        StringBuilder _optionsname = new StringBuilder();
                        StringBuilder _optionsDescription = new StringBuilder();
                        StringBuilder _optionValue = new StringBuilder();
                        if (item.OptionList != null)
                        {

                            foreach (var options in item.OptionList)
                            {
                                _optionsid.Append(options.OptionId + "$");
                                _optionsname.Append(options.Option + "$");
                                _optionsDescription.Append(options.OptionDescription + "$");
                                _optionValue.Append(options.OptionValue + "$");

                            }
                            dt.Rows.Add(item.QuestionId,
                                         item.Question,
                                         item.QuestionType,
                                         _optionsname.ToString().Substring(0, _optionsname.Length - 1),
                                         _optionsid.ToString().Substring(0, _optionsid.Length - 1),
                                         _optionsDescription.ToString().Substring(0, _optionsDescription.Length - 1),
                                         _optionValue.ToString().Substring(0, _optionValue.Length - 1),
                                         item.Required, Convert.ToInt32(item.QuestionOrder),
                                         item.IsStatusQuestion
                                         );
                        }
                        else
                            dt.Rows.Add(item.QuestionId,
                                         item.Question,
                                         item.QuestionType,
                                         DBNull.Value,
                                         DBNull.Value,
                                         DBNull.Value,
                                         DBNull.Value,
                                         item.Required,
                                         Convert.ToInt32(item.QuestionOrder),
                                         item.IsStatusQuestion);

                    }
                    command.Parameters.Add(new SqlParameter("@tblSscreening", dt));
                }

              //  command.Parameters.Add(new SqlParameter("@tblScreeningRoles", dtAccess));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveScreeningQuestion";
                Connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                result = "";

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();

            }
            return result;

        }
        public List<ScreeningQ> ScreeningList(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize)
        {
            List<ScreeningQ> _ScreeningQlist = new List<ScreeningQ>();
            try
            {
                totalrecord = string.Empty;
                string searchcenter = string.Empty;
                string AgencyId = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchcenter = string.Empty;
                else
                    searchcenter = search;
                command.Parameters.Add(new SqlParameter("@Search", searchcenter));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@AgencyID", (Guid?)null));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_getscreeninglist";
                DataAdapter = new SqlDataAdapter(command);
                _dataTable = new DataTable();
                DataAdapter.Fill(_dataTable);
                if (_dataTable.Rows.Count > 0)
                {
                    ScreeningQ addScreeningQ = null;
                    for (int i = 0; i < _dataTable.Rows.Count; i++)
                    {
                        addScreeningQ = new ScreeningQ();
                        addScreeningQ.ScreeningId = EncryptDecrypt.Encrypt64(_dataTable.Rows[i]["ScreeningID"].ToString());
                        addScreeningQ.ScreeningName = Convert.ToString(_dataTable.Rows[i]["ScreeningName"]);
                        addScreeningQ.AgencyName = Convert.ToString(_dataTable.Rows[i]["AgencyName"]);
                        addScreeningQ.CreatedOn = Convert.ToString(_dataTable.Rows[i]["DateEntered"]);
                        addScreeningQ.ScreeningOrder = Convert.ToString(_dataTable.Rows[i]["ScreeningOrder"]);
                        addScreeningQ.ProgramTypes = Convert.ToString(_dataTable.Rows[i]["ProgramType"]);
                        _ScreeningQlist.Add(addScreeningQ);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _ScreeningQlist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _ScreeningQlist;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }
        public string DeleteScreening(string id, string userId)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@id", EncryptDecrypt.Decrypt64(id)));
                command.Parameters.Add(new SqlParameter("@userid", userId));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.CommandText = "Sp_Deletescreening";
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "0";
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();

            }
        }


        //public DataTable EditScreening(string ScreeningId, string Userid)
        //{
        //    try
        //    {
        //        command.Connection = Connection;
        //        command.Parameters.Add(new SqlParameter("@UserId", Userid));
        //        command.Parameters.Add(new SqlParameter("@ScreeningId", ScreeningId));
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "SP_GetScreeningQuestion";
        //        DataAdapter = new SqlDataAdapter(command);
        //        _dataTable = new DataTable();
        //        DataAdapter.Fill(_dataTable);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //        return _dataTable;

        //    }
        //    finally
        //    {
        //        if (Connection != null)
        //            Connection.Close();
        //    }
        //    return _dataTable;

        //}



        public DataSet EditScreening(string ScreeningId, StaffDetails _staff)
        {
            try
            {
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@UserId", _staff.UserId));
                command.Parameters.Add(new SqlParameter("@ScreeningId", ScreeningId));
                command.Parameters.Add(new SqlParameter("@AgencyID", _staff.AgencyId));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetScreeningQuestion";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _dataset;

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }


            return _dataset;

        }


        public DataTable GetScreeningAccessRoles(string ScreeningId = "0")
        {
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection)
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@ScreeningId", ScreeningId));
                    command.Parameters.Add(new SqlParameter("@AgencyId", staff.AgencyId));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetScreeningAccessRoles";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataTable = new DataTable();
                    DataAdapter.Fill(_dataTable);
                    Connection.Close();
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _dataTable;

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _dataTable;

        }


        public string DeleteQuestion(string Questionid, string Screeningid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deletescrreningquestion";
                command.Parameters.Add(new SqlParameter("@Questionid", Questionid));
                command.Parameters.Add(new SqlParameter("@Screeningid", Screeningid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
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
                if (Connection != null)
                    Connection.Close();
            }
        }
        public string DeleteOption(string Optionid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deletescrreningOption";
                command.Parameters.Add(new SqlParameter("@Optionid", Optionid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
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
                if (Connection != null)
                    Connection.Close();
            }
        }
        //Added by Akansha
        public string AddFPACategory(FPACategory info, int mode, Guid userId)
        {
            try
            {
                command.Connection = Connection;
                command.CommandText = "SP_addFPACategory";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@FPACategoryID", info.FPACategoryID);
                // command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@CategryName", (info.CategryName).Trim());
                //Changes on 22dec2016
                if (!String.IsNullOrWhiteSpace(info.Description))
                {
                    command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                }
                //command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                command.Parameters.AddWithValue("@CreatedBy", userId);
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
            }
            return command.Parameters["@result"].Value.ToString();
        }
        public List<FPACategory> FPACateInfo(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            List<FPACategory> _FPAlist = new List<FPACategory>();
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
                command.Parameters.Add(new SqlParameter("@userId", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_FPACategory_list";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        FPACategory addFPARow = new FPACategory();
                        addFPARow.FPACategoryID = Convert.ToInt32(agencydataTable.Rows[i]["ID"]);
                        addFPARow.CategryName = Convert.ToString(agencydataTable.Rows[i]["CategoryName"]);
                        addFPARow.Description = Convert.ToString(agencydataTable.Rows[i]["Description"]);
                        addFPARow.CreatedDate = Convert.ToDateTime(agencydataTable.Rows[i]["CreatedDate"]).ToString("MM/dd/yyyy");

                        _FPAlist.Add(addFPARow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _FPAlist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _FPAlist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public FPACategory GetFPACateinfo(string FPACategoryID)
        {
            FPACategory obj = new FPACategory();
            try
            {
                command.Parameters.Add(new SqlParameter("@FPACategoryID", FPACategoryID));
                // command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_FPACategoryinfo";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable != null && agencydataTable.Rows.Count > 0)
                {
                    obj.FPACategoryID = Convert.ToInt32(agencydataTable.Rows[0]["ID"]);
                    obj.CategryName = agencydataTable.Rows[0]["CategoryName"].ToString();
                    //   obj.TransitionDate = agencydataTable.Rows[0]["TransitionDate"].ToString();
                    obj.Description = agencydataTable.Rows[0]["Description"].ToString();
                    obj.CreatedDate = Convert.ToDateTime(agencydataTable.Rows[0]["CreatedDate"]).ToString("MM/dd/yyyy");
                    //  obj.FormalAgreement = Convert.ToBoolean(agencydataTable.Rows[0]["FormalAgreement"].ToString());



                }
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
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
                agencydataTable.Dispose();
            }
        }
        public string DeleteFPAinfo(string FPACategoryID)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteFPAinfo";
                command.Parameters.Add(new SqlParameter("@FPACategoryID", FPACategoryID));
                //   command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
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

        //Added on 21Dec2016
        public string AddWorkshop(Workshop info, int mode, string agencyId, Guid userId)
        {
            try
            {
                command.Connection = Connection;
                command.CommandText = "SP_addWorkshop";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@WorkshopID", info.WorkshopID);
                // command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@WorkshopName", (info.WorkshopName).Trim());
                if (!String.IsNullOrWhiteSpace(info.Description))
                {
                    command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                }
                //  command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                if (!String.IsNullOrWhiteSpace(agencyId))
                {
                    command.Parameters.AddWithValue("@agencyId", agencyId);
                }
                command.Parameters.AddWithValue("@Category", info.Category);
                command.Parameters.AddWithValue("@CreatedBy", userId);
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
            }
            return command.Parameters["@result"].Value.ToString();
        }
        //Added on 22Dec2016
        public List<Workshop> WorkshopInfo(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid, string agencyId)
        {
            List<Workshop> _Workshoplist = new List<Workshop>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                if (!String.IsNullOrWhiteSpace(agencyId))
                {
                    command.Parameters.AddWithValue("@agencyId", agencyId);
                }
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@userId", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Workshop_list";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        Workshop addFPARow = new Workshop();
                        addFPARow.WorkshopID = Convert.ToInt32(agencydataTable.Rows[i]["ID"]);
                        addFPARow.WorkshopName = Convert.ToString(agencydataTable.Rows[i]["WorkshopName"]);
                        addFPARow.Description = Convert.ToString(agencydataTable.Rows[i]["Description"]);
                        addFPARow.CreatedDate = Convert.ToDateTime(agencydataTable.Rows[i]["CreatedDate"]).ToString("MM/dd/yyyy");
                        addFPARow.Category = Convert.ToString(agencydataTable.Rows[i]["Category"]);
                        _Workshoplist.Add(addFPARow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _Workshoplist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _Workshoplist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }

        public void GetCategoryBySearchText(ref List<SelectListItem> lstItems, string SearchText)
        {
            lstItems = new List<SelectListItem>();
            _dataset = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SearchCategory";
                command.Parameters.AddWithValue("@SearchText", SearchText);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            lstItems.Add(new SelectListItem { Text = dr["Category"].ToString(), Value = dr["Category"].ToString() });
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
        }
        public Workshop GetWorkshopinfo(string WorkshopID, string agencyId)
        {
            Workshop obj = new Workshop();
            try
            {
                if (!String.IsNullOrWhiteSpace(agencyId))
                {
                    command.Parameters.AddWithValue("@agencyId", agencyId);
                }
                command.Parameters.Add(new SqlParameter("@WorkshopId", WorkshopID));
                // command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Workshopinfo";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable != null && agencydataTable.Rows.Count > 0)
                {
                    obj.WorkshopID = Convert.ToInt32(agencydataTable.Rows[0]["ID"]);
                    obj.WorkshopName = agencydataTable.Rows[0]["WorkshopName"].ToString();
                    obj.Category = agencydataTable.Rows[0]["Category"].ToString();
                    //   obj.TransitionDate = agencydataTable.Rows[0]["TransitionDate"].ToString();
                    obj.Description = agencydataTable.Rows[0]["Description"].ToString();
                    obj.CreatedDate = Convert.ToDateTime(agencydataTable.Rows[0]["CreatedDate"]).ToString("MM/dd/yyyy");
                    //  obj.FormalAgreement = Convert.ToBoolean(agencydataTable.Rows[0]["FormalAgreement"].ToString());



                }
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
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
                agencydataTable.Dispose();
            }
        }
        public string DeleteWorkshopinfo(string WorkshopID, string agencyId)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteWorkshopinfo";
                if (!String.IsNullOrWhiteSpace(agencyId))
                {
                    command.Parameters.AddWithValue("@agencyId", agencyId);
                }
                command.Parameters.Add(new SqlParameter("@WorkshopID", WorkshopID));
                //   command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
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


        //Added on 7Feb2017

        public string AddHoliday(Holiday info, int mode, string agencyId, Guid userId)
        {
            try
            {
                command.Connection = Connection;
                command.CommandText = "SP_addHoliday";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@HolidayID", info.HolidayID);
                // command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@HolidayName", (info.HolidayName).Trim());
                command.Parameters.AddWithValue("@HolidayDate", (info.HolidayDate));
                if (!String.IsNullOrWhiteSpace(info.Description))
                {
                    command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                }
                //  command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                if (!String.IsNullOrWhiteSpace(agencyId))
                {
                    command.Parameters.AddWithValue("@agencyId", agencyId);
                }
                command.Parameters.AddWithValue("@CreatedBy", userId);
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
            }
            return command.Parameters["@result"].Value.ToString();
        }



        public List<Holiday> HolidayInfo(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid, string agencyId)
        {
            List<Holiday> _Holidaylist = new List<Holiday>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                if (!String.IsNullOrWhiteSpace(agencyId))
                {
                    command.Parameters.AddWithValue("@agencyId", agencyId);
                }
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@userId", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Holiday_list";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        Holiday addHolidayRow = new Holiday();
                        addHolidayRow.HolidayID = Convert.ToInt32(agencydataTable.Rows[i]["HolidayID"]);
                        addHolidayRow.HolidayName = Convert.ToString(agencydataTable.Rows[i]["HolidayName"]);
                        addHolidayRow.Description = Convert.ToString(agencydataTable.Rows[i]["Description"]);
                        addHolidayRow.HolidayDate = Convert.ToDateTime(agencydataTable.Rows[i]["HolidayDate"]).ToString("MM/dd/yyyy");
                        addHolidayRow.CreatedDate = Convert.ToDateTime(agencydataTable.Rows[i]["CreatedDate"]).ToString("MM/dd/yyyy");

                        _Holidaylist.Add(addHolidayRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _Holidaylist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _Holidaylist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }



        public Holiday GetHolidayinfo(string HolidayID, string agencyId)
        {
            Holiday obj = new Holiday();
            try
            {
                if (!String.IsNullOrWhiteSpace(agencyId))
                {
                    command.Parameters.AddWithValue("@agencyId", agencyId);
                }
                command.Parameters.Add(new SqlParameter("@HolidayID", HolidayID));
                // command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Holidayinfo";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable != null && agencydataTable.Rows.Count > 0)
                {
                    obj.HolidayID = Convert.ToInt32(agencydataTable.Rows[0]["HolidayID"]);
                    obj.HolidayName = agencydataTable.Rows[0]["HolidayName"].ToString();
                    obj.HolidayDate = Convert.ToDateTime(agencydataTable.Rows[0]["HolidayDate"]).ToString("MM/dd/yyyy");
                    obj.Description = agencydataTable.Rows[0]["Description"].ToString();
                    obj.CreatedDate = Convert.ToDateTime(agencydataTable.Rows[0]["CreatedDate"]).ToString("MM/dd/yyyy");
                    //  obj.FormalAgreement = Convert.ToBoolean(agencydataTable.Rows[0]["FormalAgreement"].ToString());



                }
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
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
                agencydataTable.Dispose();
            }
        }

        public string DeleteHolidayinfo(string HolidayID, string agencyId)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteHolidayinfo";
                if (!String.IsNullOrWhiteSpace(agencyId))
                {
                    command.Parameters.AddWithValue("@agencyId", agencyId);
                }
                command.Parameters.Add(new SqlParameter("@HolidayID", HolidayID));
                //   command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
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

        //public DataSet AddScreeningdetails(string Userid)
        //{
        //    try
        //    {
        //        command.Connection = Connection;
        //        command.Parameters.Add(new SqlParameter("@UserId", Userid));
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "SP_AddScreening";
        //        DataAdapter = new SqlDataAdapter(command);
        //        _dataset = new DataSet();
        //        DataAdapter.Fill(_dataset);
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //        return _dataset;

        //    }
        //    finally
        //    {
        //        if (Connection != null)
        //            Connection.Close();
        //    }
        //    return _dataset;

        //} //add by atul
        public string AddDisablitiesType(Disablities_Type info, int mode, Guid userId)
        {
            try
            {
                command.Connection = Connection;
                command.CommandText = "SP_addDisablitiesType";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@DisablitiesID", info.DisablitiesID);
                // command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@DisablitiesType", (info.DisablitiesType).Trim());
                //Changes on 22dec2016
                if (!String.IsNullOrWhiteSpace(info.Description))
                {
                    command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                }
                //command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                command.Parameters.AddWithValue("@CreatedBy", userId);
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
            }
            return command.Parameters["@result"].Value.ToString();
        }
        public List<Disablities_Type> DisablitiesTypeInfo(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            List<Disablities_Type> _DisablitiesTypelist = new List<Disablities_Type>();
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
                command.Parameters.Add(new SqlParameter("@userId", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_DisablitiesType_list";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        Disablities_Type add_DisablitiesTypeRow = new Disablities_Type();
                        add_DisablitiesTypeRow.DisablitiesID = Convert.ToInt32(agencydataTable.Rows[i]["ID"]);
                        add_DisablitiesTypeRow.DisablitiesType = Convert.ToString(agencydataTable.Rows[i]["DisablitiesType"]);
                        add_DisablitiesTypeRow.Description = Convert.ToString(agencydataTable.Rows[i]["Description"]);
                        add_DisablitiesTypeRow.CreatedDate = Convert.ToDateTime(agencydataTable.Rows[i]["CreatedDate"]).ToString("MM/dd/yyyy");

                        _DisablitiesTypelist.Add(add_DisablitiesTypeRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _DisablitiesTypelist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _DisablitiesTypelist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public Disablities_Type GetDisablitiesTypeinfo(string DisablitiesID)
        {
            Disablities_Type obj = new Disablities_Type();
            try
            {
                command.Parameters.Add(new SqlParameter("@DisablitiesID", DisablitiesID));
                // command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_DisablitiesTypeinfo";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable != null && agencydataTable.Rows.Count > 0)
                {
                    obj.DisablitiesID = Convert.ToInt32(agencydataTable.Rows[0]["ID"]);
                    obj.DisablitiesType = agencydataTable.Rows[0]["DisablitiesType"].ToString();
                    //   obj.TransitionDate = agencydataTable.Rows[0]["TransitionDate"].ToString();
                    obj.Description = agencydataTable.Rows[0]["Description"].ToString();
                    obj.CreatedDate = Convert.ToDateTime(agencydataTable.Rows[0]["CreatedDate"]).ToString("MM/dd/yyyy");
                    //  obj.FormalAgreement = Convert.ToBoolean(agencydataTable.Rows[0]["FormalAgreement"].ToString());



                }
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
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
                agencydataTable.Dispose();
            }
        }
        public string DeleteDisablitiesTypeinfo(string DisablitiesID)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteDisablitiesTypeinfo";
                command.Parameters.Add(new SqlParameter("@DisablitiesID", DisablitiesID));
                //   command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
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

    }
}
