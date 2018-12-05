using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FingerprintsModel;
namespace FingerprintsData
{
    public class LoginData
    {

        SqlConnection Connection = connection.returnConnection();
        SqlCommand Command = new SqlCommand();
        SqlDataReader DataReader = null;
        SqlDataAdapter DataAdapter = null;
        DataTable UserDataTable = null;
        DataSet _Dataset = null;
        public FingerprintsModel.Login LoginUser(out string result, out List<Role> RoleList, string UserName, string Password, string IPaddress)
        {
     //  string Pwd = EncryptDecrypt.Decrypt(Password);
            Login Login = null;
            result = string.Empty;
            RoleList = new List<Role>();
           try
            {
                Command.Parameters.Add(new SqlParameter("@emailid", UserName));
                Command.Parameters.Add(new SqlParameter("@password", EncryptDecrypt.Encrypt(Password)));
                Command.Parameters.Add(new SqlParameter("@IPaddress", IPaddress));
                Command.Parameters.Add(new SqlParameter("@result", SqlDbType.VarChar, 100)).Direction = ParameterDirection.Output;
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "LOGINDETAILS";
                DataAdapter = new SqlDataAdapter(Command);
                _Dataset = new DataSet();
                DataAdapter.Fill(_Dataset);
                result = Command.Parameters["@result"].Value.ToString();
                if (result.ToLower().Contains("success"))
                {
                    if (_Dataset != null && _Dataset.Tables[0] != null && _Dataset.Tables[0].Rows.Count > 0)
                    {
                        Login = new Login();
                        Login.UserId = Guid.Parse(Convert.ToString(_Dataset.Tables[0].Rows[0]["userid"]));
                        Login.RoleName = Convert.ToString(_Dataset.Tables[0].Rows[0]["RoleName"]);
                        Login.Emailid = Convert.ToString(_Dataset.Tables[0].Rows[0]["Emailid"]);
                        Login.UserName = Convert.ToString(_Dataset.Tables[0].Rows[0]["name"]);
                        Login.roleId = Guid.Parse(_Dataset.Tables[0].Rows[0]["Roleid"].ToString());
                        Login.MenuEnable = Convert.ToBoolean(_Dataset.Tables[0].Rows[0]["MenuEnabled"].ToString());
                        if (!string.IsNullOrEmpty(_Dataset.Tables[0].Rows[0]["AgencyId"].ToString()))
                            Login.AgencyId = Guid.Parse(_Dataset.Tables[0].Rows[0]["AgencyId"].ToString());
                        else
                            Login.AgencyId = null;
                        Login.IsShowPIR = Convert.ToBoolean(_Dataset.Tables[0].Rows[0]["IsShowPIR"]);
                        Login.IsShowSectionB = Convert.ToBoolean(_Dataset.Tables[0].Rows[0]["IsShowSectionB"]);
                        Login.IsShowScreening = Convert.ToBoolean(_Dataset.Tables[0].Rows[0]["IsShowScreening"]);
                        Login.AllowCaseNoteTeacher = Convert.ToString(_Dataset.Tables[0].Rows[0]["AllowCaseNoteTeacher"]);
                    }
                    if (_Dataset != null && _Dataset.Tables.Count > 1 && _Dataset.Tables[1] != null && _Dataset.Tables[1].Rows.Count > 0)
                    {
                        Role info = new Role();
                        foreach (DataRow dr in _Dataset.Tables[1].Rows)
                        {
                            info = new Role();
                            info.RoleId = dr["Roleid"].ToString();
                            info.RoleName = dr["rolename"].ToString();
                            info.Defaultrole = Convert.ToBoolean(dr["defualtrole"]);
                            RoleList.Add(info);
                        }


                    }
                }
                return Login;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
               if (Connection != null)
                   Connection.Close();
               return Login;
            }
            finally
            {
               if (Connection != null)
                    Connection.Close();
           }

        }
        public FingerprintsModel.Login LoginUseragency(out string result, string UserName, string Password, string IPaddress)
        {
            Login Login = null;
            result = string.Empty;
            try
            {
                Command.Parameters.Add(new SqlParameter("@emailid", UserName));
                Command.Parameters.Add(new SqlParameter("@password", EncryptDecrypt.Encrypt(Password)));
                Command.Parameters.Add(new SqlParameter("@IPaddress", IPaddress));
                Command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = "Sp_LoginUser1";
                DataAdapter = new SqlDataAdapter(Command);
                UserDataTable = new DataTable();
                DataAdapter.Fill(UserDataTable);
                result = Command.Parameters["@result"].Value.ToString();
                if (UserDataTable != null && UserDataTable.Rows.Count > 0)
                {
                    Login = new Login();
                    Login.UserId = Guid.Parse(Convert.ToString(UserDataTable.Rows[0]["userid"]));
                    Login.RoleName = Convert.ToString(UserDataTable.Rows[0]["RoleName"]);
                    Login.Emailid = Convert.ToString(UserDataTable.Rows[0]["Emailid"]);
                    Login.UserName = Convert.ToString(UserDataTable.Rows[0]["name"]);
                    Login.AgencyName = Convert.ToString(UserDataTable.Rows[0]["AgencyName"]);
                    if (!string.IsNullOrEmpty(UserDataTable.Rows[0]["AgencyId"].ToString()))
                        Login.AgencyId = Guid.Parse(UserDataTable.Rows[0]["AgencyId"].ToString());
                    else
                        Login.AgencyId = null;
                    if (!string.IsNullOrEmpty(UserDataTable.Rows[0]["accessstart"].ToString()))
                        Login.AccessStart = UserDataTable.Rows[0]["accessstart"].ToString();
                    if (!string.IsNullOrEmpty(UserDataTable.Rows[0]["AccessStop"].ToString()))
                        Login.AccessStop = UserDataTable.Rows[0]["AccessStop"].ToString();
                }
                DataAdapter.Dispose();
                Command.Dispose();
                UserDataTable.Dispose();
                return Login;
            }
           catch (Exception ex)
           {
                clsError.WriteException(ex);
                return Login;
            }
            finally
            {
                DataAdapter.Dispose();
                Command.Dispose();
                UserDataTable.Dispose();
            }

        }

        //public FingerprintsModel.Login LoginUseragency(out string result, string UserName, string Password,string agencycode ,string IPaddress)
        //{
        //    Login Login = null;
        //    result = string.Empty;
        //    try
        //    {
        //        Command.Parameters.Add(new SqlParameter("@emailid", UserName));
        //        Command.Parameters.Add(new SqlParameter("@agencycode", agencycode));
        //        Command.Parameters.Add(new SqlParameter("@password", EncryptDecrypt.EncryptData(Password)));
        //        Command.Parameters.Add(new SqlParameter("@IPaddress", IPaddress));
        //        Command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
        //        Command.Connection = Connection;
        //        Command.CommandType = CommandType.StoredProcedure;
        //        Command.CommandText = "Sp_LoginUser1";
        //        DataAdapter = new SqlDataAdapter(Command);
        //        UserDataTable = new DataTable();
        //        DataAdapter.Fill(UserDataTable);
        //        result = Command.Parameters["@result"].Value.ToString();
        //        if (UserDataTable != null && UserDataTable.Rows.Count > 0)
        //        {
        //            Login = new Login();
        //            Login.UserId = Guid.Parse(Convert.ToString(UserDataTable.Rows[0]["userid"]));
        //            Login.RoleName = Convert.ToString(UserDataTable.Rows[0]["RoleName"]);
        //            Login.Emailid = Convert.ToString(UserDataTable.Rows[0]["Emailid"]);
        //            Login.UserName = Convert.ToString(UserDataTable.Rows[0]["name"]);
        //            Login.AgencyName = Convert.ToString(UserDataTable.Rows[0]["AgencyName"]);
        //            if (!string.IsNullOrEmpty(UserDataTable.Rows[0]["AgencyId"].ToString()))
        //                Login.AgencyId = Guid.Parse(UserDataTable.Rows[0]["AgencyId"].ToString());
        //            else
        //                Login.AgencyId = null;
        //            if (!string.IsNullOrEmpty(UserDataTable.Rows[0]["accessstart"].ToString()))
        //                Login.AccessStart = UserDataTable.Rows[0]["accessstart"].ToString();
        //            if (!string.IsNullOrEmpty(UserDataTable.Rows[0]["AccessStop"].ToString()))
        //                Login.AccessStop = UserDataTable.Rows[0]["AccessStop"].ToString();
        //        }
        //        DataAdapter.Dispose();
        //        Command.Dispose();
        //        UserDataTable.Dispose();
        //        return Login;
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //        return Login;
        //    }
        //    finally
        //    {
        //        DataAdapter.Dispose();
        //        Command.Dispose();
        //        UserDataTable.Dispose();
        //    }

        //}
        public bool CheckEmailIdExist(string Emailid, string Password)
        {
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                Command.CommandType = CommandType.Text;
                Command.Parameters.Add(new SqlParameter("@Email", Emailid));
                Command.CommandText = "Select isnull(emailid,'') as EmailId from Logininfo where emailid is not null and emailid=@Email";
                Command.Connection = Connection;
                DataReader = Command.ExecuteReader();
                if (DataReader.Read() && DataReader.HasRows)
                {
                    DataReader.Close();
                    Command.Parameters.Add(new SqlParameter("@Password", EncryptDecrypt.Encrypt(Password)));
                    Command.CommandText = "update Logininfo  set Password=@Password ,DateModified=getdate() where emailid=@Email";
                    if (Command.ExecuteNonQuery() > 0)
                        return true;
                    else
                        return false;
                }

                return false;
            }
            catch (Exception ex)
            {
               clsError.WriteException(ex);
                return false;
            }
            finally
            {
                DataReader.Close();
                Connection.Close();
                Command.Dispose();

            }
        }
        public string ChangePassword(string currentPassword, string newPassword, string userId)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                Command.CommandType = CommandType.Text;
                Command.Parameters.Add(new SqlParameter("@currentPassword", EncryptDecrypt.Encrypt(currentPassword)));
                Command.Parameters.Add(new SqlParameter("@newPassword", EncryptDecrypt.Encrypt(newPassword)));
                Command.Parameters.Add(new SqlParameter("@userId", userId));
                Command.CommandText = "Select Password from Logininfo where password=@currentPassword and userid=@userId  and status=1";
                Command.Connection = Connection;
                DataReader = Command.ExecuteReader();
                if (DataReader.Read() && DataReader.HasRows)
                {
                    DataReader.Close();
                    Command.CommandText = "update Logininfo  set Password=@newPassword,DateModified=getdate(),UpdatedBy=@userId where  password=@currentPassword and userid=@userId  and status=1";
                    if (Command.ExecuteNonQuery() > 0)
                        return "1";
                    else
                        return "0";
                }
                return "-1";
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return ex.Message;
            }
            finally
            {
                DataReader.Close();
                Connection.Close();
                Command.Dispose();
                Command.Dispose();
            }
        }

        public bool IsDevelopmentTeam(Guid userId,Guid? AgencyId,Guid RoleId)
        {
            bool isRowAffected = false;
            try
            {
                if(Connection.State==ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Open();
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.Clear();
                Command.Parameters.Add(new SqlParameter("@AgencyId", (AgencyId)));
                Command.Parameters.Add(new SqlParameter("@UserId", (userId)));
                Command.Parameters.Add(new SqlParameter("@RoleId", (RoleId)));
                Command.CommandText = "USP_CheckDevelopmentTeam";
                var obj = Command.ExecuteScalar();
                if(string.IsNullOrEmpty(obj.ToString()))
                {
                    isRowAffected = false;
                }
                else
                {
                    
                    if(Convert.ToBoolean(obj))
                    {
                        isRowAffected = true;
                    }
                  
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
              //  return ex.Message;
            }
            finally
            {
                Connection.Close();
                Command.Dispose();
                Command.Dispose();
            }
            return isRowAffected;
        }

        public bool IsDemographic(Guid userId, Guid? AgencyId, Guid RoleId)
        {
            bool isRowAffected = false;
         
            try
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Open();
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.Clear();
                Command.Parameters.Add(new SqlParameter("@AgencyId", (AgencyId)));
                Command.Parameters.Add(new SqlParameter("@UserId", (userId)));
                Command.Parameters.Add(new SqlParameter("@RoleId", (RoleId)));
               
                Command.CommandText = "USP_CheckDemographic";
                var obj = Command.ExecuteScalar();
              
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    isRowAffected = false;
                }
                else
                {

                    if (Convert.ToBoolean(obj))
                    {
                        isRowAffected = true;
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                //  return ex.Message;
            }
            finally
            {
                Connection.Close();
                Command.Dispose();
                Command.Dispose();
            }
            return isRowAffected;
        }

        public List<Tuple<string, string>> GetAccessPageByUserId(ref bool isAcceptanceProcess, Guid userId, Guid? AgencyId, Guid RoleId)
        {
           
            List<Tuple<string, string>> AccessList = new List<Tuple<string, string>>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Open();
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.Clear();
                Command.Parameters.Add(new SqlParameter("@AgencyId", (AgencyId)));
                Command.Parameters.Add(new SqlParameter("@UserId", (userId)));
                Command.Parameters.Add(new SqlParameter("@RoleId", (RoleId)));

                Command.CommandText = "SP_GetAccessPageByUserId";
                DataAdapter = new SqlDataAdapter(Command);
                _Dataset = new DataSet();
                DataAdapter.Fill(_Dataset);
               if(_Dataset != null )
                {

                    if(_Dataset.Tables.Count > 0 && _Dataset.Tables[0].Rows.Count > 0)
                    {

                   

                    foreach (DataRow dr in _Dataset.Tables[0].Rows)
                    {

                        AccessList.Add(new Tuple<string, string>(dr["LayoutName"].ToString(), dr["URL"].ToString()));
                    }
                    }

                    if(_Dataset.Tables.Count>1 && _Dataset.Tables[1].Rows.Count>0)
                    {
                        isAcceptanceProcess = Convert.ToBoolean(_Dataset.Tables[1].Rows[0]["IsInAcceptanceProcess"]);
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
                Command.Dispose();
                Command.Dispose();
            }
            return AccessList;
        }

   

        //Changes on 4jan2017
        public string CheckPassword(string Email, string Password)
        {
            try
            {
                //if (Connection.State == ConnectionState.Open)
                //    Connection.Close();
                // Connection.Open();
                Command.Connection = Connection;
                Command.CommandType = CommandType.Text;
                Command.Parameters.Add(new SqlParameter("@Email", (Email)));
                Command.Parameters.Add(new SqlParameter("@Password", EncryptDecrypt.Encrypt(Password)));
                Command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                Command.CommandText = "SP_checklogin_info";
                Command.Connection = Connection;
                Command.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                Command.ExecuteNonQuery();
                Connection.Close();
                return Command.Parameters["@result"].Value.ToString();

                // return "-1";
            }
            catch (Exception ex)
           {
                clsError.WriteException(ex);
                return ex.Message;
            }
            finally
            {

                Connection.Close();
                Command.Dispose();
                Command.Dispose();
            }
        }

        //End
    }
}
