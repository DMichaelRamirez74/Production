using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsData
{
   public class MentalHealthData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;
        public List<MentalHealthDashboard> GetMentalHealthDashboard( string Agencyid, string userid)
        {
            List<MentalHealthDashboard> centerList = new List<MentalHealthDashboard>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[SP_MentalHealthDashboard]";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            MentalHealthDashboard info = new MentalHealthDashboard();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["center"].ToString());
                            info.Name = dr["centername"].ToString();
                            info.TotalChildren = dr["totalchildren"].ToString();
                            info.DisabilityPercentage = dr["TotalDisablePercentage"].ToString();
                            info.Indicated = dr["Indicated"].ToString();
                           
                            info.Qualified = dr["Qualified"].ToString();
                            info.Released = dr["Released"].ToString();
                            centerList.Add(info);
                        }
                    }

                }
                DataAdapter.Dispose();
                command.Dispose();
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
            return centerList;
        }

        public List<MentalHealthClientList> LoadMentalHealthDashboardList(string centerid,string mode)
        {
            List<MentalHealthClientList> clientList = new List<MentalHealthClientList>();
            Roster _roster = new Roster();
            try
            {
                StaffDetails inst = StaffDetails.GetInstance();
                command.Parameters.Clear();             
                command.Parameters.Add(new SqlParameter("@CenterId", EncryptDecrypt.Decrypt64(centerid)));
                command.Parameters.Add(new SqlParameter("@userid", inst.UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", inst.AgencyId));
                command.Parameters.Add(new SqlParameter("@Mode", mode));        
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_LoadMentalHealthList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        MentalHealthClientList info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new MentalHealthClientList();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.DOB = Convert.ToString(dr["dob"]);
                            info.Gender = dr["gender"].ToString();
                            info.CenterName = dr["CenterName"].ToString();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString());
                            info.YakkrId = EncryptDecrypt.Encrypt64(dr["YakkrId"].ToString());
                            info.ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString());
                            info.ClassroomName = dr["ClassroomName"].ToString();
                            info.FSW = dr["fswname"].ToString();
                            info.Picture = dr["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePic"]);
                            info.classroomid = dr["classroomid"].ToString();
                            clientList.Add(info);
                        }
                       
                    }
                }
       

                //End
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
            return clientList;

        }

        public List<MentalHealthClientList> GetMentalHealthClientDetails(string clientid, string centerid,string householdid, string mode)
        {
            List<MentalHealthClientList> clientList = new List<MentalHealthClientList>();
            Roster _roster = new Roster();
            try
            {
                StaffDetails inst = StaffDetails.GetInstance();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@clientid", EncryptDecrypt.Decrypt64(clientid)));
                command.Parameters.Add(new SqlParameter("@centerid", EncryptDecrypt.Decrypt64(centerid)));
                command.Parameters.Add(new SqlParameter("@householdid", EncryptDecrypt.Decrypt64(householdid)));

                command.Parameters.Add(new SqlParameter("@userid", inst.UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", inst.AgencyId));
                command.Parameters.Add(new SqlParameter("@Mode", mode));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetMentalHealthClientDetails";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        MentalHealthClientList info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new MentalHealthClientList();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.DOB = Convert.ToString(dr["dob"]);
                            info.Gender = dr["gender"].ToString();
                            info.CenterName = dr["CenterName"].ToString();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString());
                            info.ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString());
                            info.ClassroomName = dr["ClassroomName"].ToString();
                            info.FSW = dr["fswname"].ToString();
                            info.Picture = dr["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePic"]);
                            info.classroomid = dr["classroomid"].ToString();
                            clientList.Add(info);
                        }

                    }
                }


                //End
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
            return clientList;

        }


        public bool SaveMentalHealthClient(MentalHealthCaseNote mhObj,string casenoteid)
        {
            bool result = false;
            try
            {
                StaffDetails staff = new StaffDetails();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveMentalHealthClient";
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@centerid", EncryptDecrypt.Decrypt64(mhObj.CenterId)));
                command.Parameters.Add(new SqlParameter("@clientid", EncryptDecrypt.Decrypt64(mhObj.ClientId)));
                command.Parameters.Add(new SqlParameter("@yakkrid", EncryptDecrypt.Decrypt64((mhObj.YakkrId))));
                command.Parameters.Add(new SqlParameter("@casenoteid", casenoteid));

                command.Parameters.Add(new SqlParameter("@MHConsultStaff", mhObj.ConsultStaff));
                command.Parameters.Add(new SqlParameter("@MHConsultParent", mhObj.ConsultParent));
                command.Parameters.Add(new SqlParameter("@MHAssessment", mhObj.ProvideAssessment));
                command.Parameters.Add(new SqlParameter("@MHService", mhObj.ProvideService));
                command.Parameters.Add(new SqlParameter("@MHStatus", mhObj.MHStatus));
                command.Parameters.Add(new SqlParameter("@TimeSpentInHours", mhObj.TimeSpent));
                command.Parameters.Add(new SqlParameter("@mode", mhObj.Mode));
                command.Parameters.Add(new SqlParameter("@AgencyId", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
               
                int res = command.ExecuteNonQuery();
                if (res > 1)
                    result = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null && Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                    command.Dispose();
                }
            }
            return result;
        }


        public Role GetMentalHealthDetailsByClientId(string ClientId)
        {
            Role role = new Role();
         
            role.ClientList = new List<RosterNew.User>();
            StaffDetails staff = StaffDetails.GetInstance();
            try
            {
                command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64(ClientId)));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[SP_GetMentalHealthDetailsByClientId]";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
            
                if (_dataset != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    FingerprintsModel.RosterNew.User obj = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        obj = new FingerprintsModel.RosterNew.User();
                        obj.Id = dr["clientid"].ToString();
                        obj.Name = dr["Name"].ToString();
                        role.ClientList.Add(obj);
                    }

                }
                DataAdapter.Dispose();
                command.Dispose();

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
            return role;
        }

    }
}
