using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FingerprintsModel;
using System.Collections;

namespace FingerprintsData
{
  public  class ClientFileReviewData
    {

        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        //SqlDataReader dataReader = null;
        //SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;



       public List<Class_Center>GetClassCenter(Guid? AgencyId,string queryCommand,long centerId=0)
        {
            Class_Center center_Class = new Class_Center();
            List<Class_Center> centersList = new List<Class_Center>();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetCenters_Class";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Class_Center class_center = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            class_center = new Class_Center();
                            class_center.CenterId = Convert.ToInt64(dr["CenterId"]);
                            class_center.ClassRoomName = dr["ClassroomName"].ToString();
                            class_center.ClassRoomId = Convert.ToInt64(dr["ClassRoomId"]);
                            class_center.CenterName = dr["CenterName"].ToString();
                            centersList.Add(class_center);
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
            return centersList;
        }

        public List<ClientDetails>GetClientsForReview( out List<ClientReviewStatus> clientStatusList, Class_Center classCenter,string queryCommand)
        {
            List<ClientDetails> clientDetailsList = new List<ClientDetails>();
            ClientDetails clientDetails = null;
            ClientReviewStatus clientReviewStatus = null;
            clientStatusList = new List<ClientReviewStatus>();
        //    List<ClientReviewStatus> clientStatusList = new List<ClientReviewStatus>();


            try
            {

                StaffDetails details = new StaffDetails();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
               
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", classCenter.AgencyId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", classCenter.ClassRoomId));
                command.Parameters.Add(new SqlParameter("@CenterId", classCenter.CenterId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Parameters.Add(new SqlParameter("@UserId", details.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId)); 
                command.CommandText = "USP_GetClients_ForReview";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if((_dataset.Tables[0]!=null) &&(_dataset.Tables[0].Rows.Count>0) )
                {
                    foreach(DataRow dr in _dataset.Tables[0].Rows)
                    {
                        clientDetails = new ClientDetails();
                        clientDetails.ClientId = Convert.ToInt64(dr["ClientId"]);
                        clientDetails.ClientName = dr["ClientName"].ToString();
                       // clientDetails.ActiveProgramYear = dr["ActiveProgramYear"].ToString();
                        clientDetailsList.Add(clientDetails);
                    }
                }

                if ((_dataset.Tables[1] != null) && (_dataset.Tables[1].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        clientReviewStatus = new ClientReviewStatus ();
                        clientReviewStatus.ClientId = Convert.ToInt64(dr["ClientId"]);
                        clientReviewStatus.AttendanceMonth = dr["AttendanceMonth"].ToString().ToLower();
                        clientReviewStatus.ActiveProgramYear = dr["ActiveProgramYear"].ToString();
                        clientReviewStatus.Status = (string.IsNullOrEmpty(dr["Status"].ToString())) ? "Empty" : (Convert.ToBoolean(dr["Status"])) ? "Open" : "Closed";
                        clientStatusList.Add(clientReviewStatus);
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
            return clientDetailsList;
        }

        public DevelopmentMembers CheckIsHost(string month,Guid? AgencyId,Guid userID,long clientId)
        {
           // bool isRowAffected = false;
            DevelopmentMembers devMembers = new DevelopmentMembers();
            
            try
            {
                string queryCommand = "CHECKHOST";
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@ClientId", clientId));
                command.Parameters.Add(new SqlParameter("@AttendanceMonth", month));
                command.Parameters.Add(new SqlParameter("@UserId", userID));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "USP_MembersForReview";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if ((_dataset.Tables[0] != null) && (_dataset.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                       devMembers.UserId = (!string.IsNullOrEmpty(dr["HostUserId"].ToString())) ? new Guid(dr["HostUserId"].ToString()) : (Guid?)null;
                       devMembers.FullName = dr["HostName"].ToString();
                       devMembers.UserColor = dr["HostUserColor"].ToString();
                       devMembers.RoleId = new Guid(dr["HostRoleId"].ToString());
                       devMembers.RoleName = dr["HostRoleName"].ToString();
                       devMembers.IsHost = Convert.ToBoolean(dr["IsHost"]);
                    }
                    // membersList = membersList.Where(x => x.UserId != null).ToList();
                }
                else
                {
                    devMembers.UserId = (Guid?)null;
                    devMembers.FullName = "";
                    devMembers.UserColor = "";
                    devMembers.RoleName = "";
                    devMembers.IsHost = true;
                }

                //var obj = command.ExecuteScalar();
                //isRowAffected = Convert.ToBoolean(obj);
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
            return devMembers;
        }
  

        public List<DevelopmentMembers>GetDevelopmentMembers(out int ContributorCount, DevelopmentMembers members)
        {
            List<DevelopmentMembers> membersList = new List<DevelopmentMembers>();
            DevelopmentMembers dev_members = null;
            string queryCommand = "SELECT";
             ContributorCount = 0;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", members.AgencyId));
                command.Parameters.Add(new SqlParameter("@ClientId", members.ClientId));
                command.Parameters.Add(new SqlParameter("@AttendanceMonth", members.ReviewMonth));
                command.Parameters.Add(new SqlParameter("@UserId", members.UserId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "USP_MembersForReview";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if ((_dataset.Tables[0] != null) && (_dataset.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        dev_members = new DevelopmentMembers();
                        dev_members.UserId = (!string.IsNullOrEmpty(dr["UserId"].ToString())) ? new Guid(dr["UserId"].ToString()) : (Guid?)null;
                        dev_members.FullName = dr["MemberName"].ToString();
                        dev_members.UserColor = dr["UserColor"].ToString();
                        dev_members.RoleId = new Guid(dr["RoleId"].ToString());
                        dev_members.RoleName = dr["RoleName"].ToString();
                        dev_members.IsContributor = Convert.ToBoolean(dr["IsContributor"]);

                        if ((dev_members.UserId == members.UserId)&&(!dev_members.IsContributor))
                        {
                            dev_members.IsPresent = true;
                            dev_members.IsEdit = false;
                        }
                        else
                        {
                            dev_members.IsEdit = true;
                            dev_members.IsPresent =(string.IsNullOrEmpty(dr["IsPresent"].ToString()))?false: Convert.ToBoolean(dr["IsPresent"]);
                        }
                        membersList.Add(dev_members);
                    }
                   // membersList = membersList.Where(x => x.UserId != null).ToList();
                }

                if ((_dataset.Tables[1] != null) && (_dataset.Tables[1].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        ContributorCount = (string.IsNullOrEmpty(dr["ContributorCount"].ToString())) ? 0 : Convert.ToInt32(dr["ContributorCount"]);
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
            return membersList;
        }

        public bool InsertMembersAttendance(List<DevelopmentMembers> members,Guid? agencyId,Guid userId)
        {
            bool isRowAffected = false;
            string queryCommand = "INSERT";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                if((members!=null) && (members.Count>0))
                {
                    foreach(var item in members)
                    {
                        Connection.Open();
                        command.Connection = Connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@MembersUserId", item.UserId));
                        command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                        command.Parameters.Add(new SqlParameter("@HostUserId", userId));
                        command.Parameters.Add(new SqlParameter("@ClientId", item.ClientId));
                        command.Parameters.Add(new SqlParameter("@Status", item.Status));
                        command.Parameters.Add(new SqlParameter("@IsPresent", item.IsPresent));
                        command.Parameters.Add(new SqlParameter("@AttendanceMonth", item.ReviewMonth));
                        command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                        command.CommandText = "USP_MembersForReview";
                        int RowsAffected = command.ExecuteNonQuery();
                        if (RowsAffected > 0)
                            isRowAffected = true;
                        Connection.Close();
                    }
                }
                
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                   command.Dispose();
                }
            }
            return isRowAffected;
        }

        public List<ClientReviewNotes> GetReviewNotes(ClientReviewNotes reviewNotes,Guid userID)
        {
            string queryCommand = "SELECT";
            List<ClientReviewNotes> notesList = new List<ClientReviewNotes>();
            
            
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", reviewNotes.AgencyId));
                command.Parameters.Add(new SqlParameter("@ClientId", reviewNotes.ClientId));
                command.Parameters.Add(new SqlParameter("@ReviewMonth", reviewNotes.ReviewMonth));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "USP_ClientReviewNotes";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if ((_dataset.Tables[0] != null) && (_dataset.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        reviewNotes = new ClientReviewNotes();
                        reviewNotes.ClientId = Convert.ToInt64(dr["ClientId"].ToString());
                        reviewNotes.ReviewedStaffName = dr["StaffName"].ToString();
                        reviewNotes.ReviewStatus = Convert.ToBoolean(dr["Status"]);
                        reviewNotes.NotesId = Convert.ToInt64(dr["NotesId"]);
                        reviewNotes.OpenNotes = dr["OpenNotes"].ToString();
                        reviewNotes.CloseNotes = dr["CloseNotes"].ToString();
                        reviewNotes.ReviewDate = Convert.ToDateTime(dr["DateReviewed"]).ToString("MM/dd/yyyy");
                        reviewNotes.UserActiveStatus = (dr["UserActiveStatus"].ToString() == "1") ? true : (dr["UserActiveStatus"].ToString() == "2") ? false : false;
                        reviewNotes.StaffUniqueColor = dr["UserColor"].ToString();
                        reviewNotes.UserId = new Guid(dr["CreatedBy"].ToString());
                        if(reviewNotes.UserId != userID)
                        {
                            if(!reviewNotes.UserActiveStatus)
                            {
                                reviewNotes.IsEdit = true;
                            }
                        }
                        else
                        {
                            reviewNotes.IsEdit = true;
                        }
                        notesList.Add(reviewNotes);
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
            return notesList;
        }

        public bool InsertReviewNotes(ClientReviewNotes Notes)
        {
            bool isRowAffected = false;
            try
            {
                string queryCommand = "INSERT";
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", Notes.AgencyId));
                command.Parameters.Add(new SqlParameter("@ClientId", Notes.ClientId));
                command.Parameters.Add(new SqlParameter("@ReviewMonth", Notes.ReviewMonth));
                command.Parameters.Add(new SqlParameter("@OpenNotes", Notes.OpenNotes));
                command.Parameters.Add(new SqlParameter("@CloseNotes", Notes.CloseNotes));
                command.Parameters.Add(new SqlParameter("@UserId", Notes.UserId));
                command.Parameters.Add(new SqlParameter("@StaffName", Notes.ReviewedStaffName));
                command.Parameters.Add(new SqlParameter("@Status", Notes.ReviewStatus));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "USP_ClientReviewNotes";
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

        public bool UpdateReviewNotes(ClientReviewNotes Notes)
        {
            bool isRowAffected = false;
            try
            {
                string queryCommand = "UPDATE";
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@OpenNotes", Notes.OpenNotes));
                command.Parameters.Add(new SqlParameter("@CloseNotes", Notes.CloseNotes));
                command.Parameters.Add(new SqlParameter("@UserId", Notes.UserId));
                command.Parameters.Add(new SqlParameter("@NotesId", Notes.NotesId));
                command.Parameters.Add(new SqlParameter("@Status", Notes.ReviewStatus));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "USP_ClientReviewNotes";
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


        public List<Clientprofile> GetClientProfile(Guid? AgencyId, long ClientId,DateTime monthReview)
        {
            List<Clientprofile> ClientprofileList = new List<Clientprofile>();
            Clientprofile clientProfile = new Clientprofile(); ;
            string queryCommand = "SELECT";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.Parameters.Add(new SqlParameter("@AttendanceDate", monthReview));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "USP_ClientGetProfile";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if ((_dataset.Tables[0] != null) && (_dataset.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                      
                        clientProfile.ChildName = dr["ChildName"].ToString();
                        clientProfile.ChildId =  EncryptDecrypt.Encrypt64( dr["ClientId"].ToString());
                        clientProfile.ChildAge = "(" + dr["ChildYear"].ToString() + "Y  " + dr["ChildMonth"].ToString() + "M " + dr["ChildDay"].ToString()+"Days )";

                        clientProfile.DOB = (!string.IsNullOrEmpty(Convert.ToDateTime(dr["DOB"]).ToString("MM/dd/yyyy"))) ? (Convert.ToDateTime(dr["DOB"]).ToString("MM/dd/yyyy")) : "";
                        clientProfile.BMI = dr["BMI"].ToString();
                        clientProfile.BMIPercentage = dr["BMIPercentage"].ToString();
                        //if(string.IsNullOrEmpty(clientProfile.BMIPercentage))
                        //{
                        //    clientProfile.BMI = "-";
                        //}
                        //else
                        //{
                        //    clientProfile.BMI = Math.Round(Convert.ToDecimal(clientProfile.BMIPercentage),2) + " " + "( " + clientProfile.BMI + " )";
                        //}
                        clientProfile.Doctor = dr["Doctor"].ToString();
                        clientProfile.Dentist = dr["Dentist"].ToString();
                        clientProfile.TransportationProvided = dr["TransportationProvided"].ToString();
                        // Clientprofile.PregnantMother = (string.IsNullOrEmpty(dr["PregnantMother"].ToString())) ? dr["PregnantMother"].ToString() : "";
                        // members.IsHomeBased = Convert.ToBoolean(dr["HomeBased"]);
                        clientProfile.ParentDOB = Convert.ToDateTime(dr["ParentDob"]).ToString("MM/dd/yyyy");
                        clientProfile.Mother = dr["ParentName"].ToString();
                        clientProfile.Language = dr["Language"].ToString();
                        int iep = 0;
                        int ifsp = 0;
                        if (!string.IsNullOrEmpty(dr["IEP"].ToString()))
                        {
                             iep = Convert.ToInt32(dr["IEP"]);
                        }
                        if (!string.IsNullOrEmpty(dr["IFSP"].ToString()))
                        {
                            ifsp = Convert.ToInt32(dr["IFSP"]);
                        }

                        if((iep==1 )|| (ifsp==1))
                        {
                            clientProfile.IEP = "Yes";
                         }
                       else if ((iep == 0) || (ifsp == 0))
                        {
                            clientProfile.IEP = "No";
                        }
                        else 
                        {
                            clientProfile.IEP = "-";
                        }
                        clientProfile.Employed = dr["Employed"].ToString();
                        clientProfile.BehaviorPlan = dr["BehaviorPlan"].ToString();
                        //clientProfile.StartDate= Convert.ToDateTime(dr["StartDate"]).ToString("MM/dd/yyyy");
                        clientProfile.StartDate = dr["StartDate"].ToString();
                        clientProfile.TotalEnrolled = string.IsNullOrEmpty(dr["TotalEnrollment"].ToString())?0:Convert.ToInt32(dr["TotalEnrollment"]);
                        clientProfile.Profilepic = dr["ProfilePicture"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePicture"]);
                        clientProfile.IsPregnantMother = string.IsNullOrEmpty(dr["PregnantMother"].ToString()) ? false : (Convert.ToInt32(dr["PregnantMother"]) == 1) ? true : false;
                    }

                }
                if ((_dataset.Tables[1] != null) && (_dataset.Tables[1].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        clientProfile.MissingScreenings =(dr["MissingScreening"].ToString().ToLower()=="yes")?"Yes":"No";
                    }
                }
                //if ((_dataset.Tables[2] != null) && (_dataset.Tables[2].Rows.Count > 0))
                //{
                //    foreach (DataRow dr in _dataset.Tables[2].Rows)
                //    {
                //        clientProfile.LastdateofCasenote =(string.IsNullOrEmpty(dr["LastCaseNote"].ToString())) ?"": Convert.ToDateTime( dr["LastCaseNote"]).ToString("MM/dd/yyyy");
                //    }
                //}
                if ((_dataset.Tables[2] != null) && (_dataset.Tables[2].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[2].Rows)
                    {

                        clientProfile.Address += (string.IsNullOrEmpty(dr["Street"].ToString())) ? "" : dr["Street"].ToString().ToLower() + "+";
                       clientProfile.Address+= (string.IsNullOrEmpty(dr["StreetName"].ToString())) ? "" : dr["StreetName"].ToString().ToLower() + ", ";
                       // clientProfile.Address+= (string.IsNullOrEmpty(dr["ApartmentNo"].ToString())) ? "+" : dr["ApartmentNo"].ToString().ToLower()+ "+";

                        clientProfile.Address+= (string.IsNullOrEmpty(dr["City"].ToString())) ? "" : dr["City"].ToString().ToLower() + ", ";
                       // clientProfile.Address += (string.IsNullOrEmpty(dr["County"].ToString())) ? "" : dr["County"].ToString() + "+";

                        clientProfile.Address+= (string.IsNullOrEmpty(dr["State"].ToString())) ? "" : dr["State"].ToString().ToUpper() + " ";
                        clientProfile.Address += (string.IsNullOrEmpty(dr["Zipcode"].ToString())) ? "" : dr["Zipcode"].ToString().ToLower() ;

                        clientProfile.Address.Trim().ToLower();


                       // clientProfile.Address = dr["StreetName"].ToString() + "," + dr["City"].ToString() + "," + dr["State"].ToString() + "," + dr["Zipcode"].ToString();
                    }
                }
                if ((_dataset.Tables[3] != null) && (_dataset.Tables[3].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[3].Rows)
                    {
                        clientProfile.Trimester = dr["Trimester"].ToString();
                    }
                }

                if ((_dataset.Tables[4] != null) && (_dataset.Tables[4].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[4].Rows)
                    {
                        clientProfile.TotalCasenote =string.IsNullOrEmpty(dr["TotalCaseNoteWritten"].ToString())?"0": dr["TotalCaseNoteWritten"].ToString();
                        clientProfile.LastdateofCasenote = string.IsNullOrEmpty(dr["LastCaseNote"].ToString())?"-": Convert.ToDateTime(dr["LastCaseNote"]).ToString("MM/dd/yyyy");
                    }
                }
                if ((_dataset.Tables[6] != null) && (_dataset.Tables[6].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[6].Rows)
                    {
                        clientProfile.TransferRequested = dr["TransferRequested"].ToString();
                    }
                }


                if ((_dataset.Tables[7] != null) && (_dataset.Tables[7].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[7].Rows)
                    {
                       
                        clientProfile.Parent = dr["ParentType"].ToString();

                    }
                }

                if ((_dataset.Tables[8] != null) && (_dataset.Tables[8].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[8].Rows)
                    {
                        clientProfile.ParentMaleName = dr["FatherName"].ToString();
                        clientProfile.FatherDOB = (dr["FatherDOB"].ToString() == "") ? "" : Convert.ToDateTime(dr["FatherDOB"]).ToString("MM/dd/yyyy");
                        clientProfile.FatherIsEmployed = dr["FatherIsEmployed"].ToString();
                        clientProfile.FatherJobTraining = dr["FatherJobTraining"].ToString();
                       

                    }
                }

                if ((_dataset.Tables[9] != null) && (_dataset.Tables[9].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[9].Rows)
                    {
                        
                        clientProfile.ParenFemaleName = dr["MotherName"].ToString();
                        clientProfile.MotherDOB = (dr["MotherDob"].ToString() == "") ? "" : Convert.ToDateTime(dr["MotherDob"]).ToString("MM/dd/yyyy");
                        clientProfile.MotherIsEmployed = dr["MotherIsEmployed"].ToString();
                        clientProfile.MotherJobTraining = dr["MotherJobTraining"].ToString();
                     

                    }
                }

                if ((_dataset.Tables[10] != null) && (_dataset.Tables[10].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[10].Rows)
                    {
                        clientProfile.Allergies = dr["Allergies"].ToString();
                    }
                }
              
                if ((_dataset.Tables[11] != null) && (_dataset.Tables[11].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[11].Rows)
                    {
                        clientProfile.Attendance =Convert.ToDouble(dr["AttendancePercentage"].ToString());
                    }
                }
                ClientprofileList.Add(clientProfile);
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
            return ClientprofileList;
        }

        public DevelopmentMembers GetCurrentMember(DevelopmentMembers member)
        {
            string queryCommand = "GETUSER";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", member.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", member.UserId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "USP_MembersForReview";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if ((_dataset.Tables[0] != null) && (_dataset.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        member = new DevelopmentMembers();
                        member.UserId = (!string.IsNullOrEmpty(dr["UserId"].ToString())) ? new Guid(dr["UserId"].ToString()) : (Guid?)null;
                        member.FullName = dr["MemberName"].ToString();
                        member.UserColor = dr["UserColor"].ToString();
                        member.RoleId = new Guid(dr["RoleId"].ToString());
                        member.UserColor = dr["UserColor"].ToString();
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
            return member;
        }


        public ArrayList GetCFRReportData(out List<Guid?> useridList, Guid agencyId,Guid UserId,Guid roleId,long centerID)
        {
            string queryCommand = "SELECT";
            List<CFRAnalysis> cfrAnalysisList = new List<CFRAnalysis>();
            CFRAnalysis analysis = null;
            useridList = new List<Guid?>();
            ListAnalysis ana = new ListAnalysis();
            ArrayList list = new ArrayList();
            string commandText = (centerID == 0) ? "USP_ClientFileReviewAnalysis" : "USP_GetCFRByCenter";

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                command.Parameters.Add(new SqlParameter("@CenterId", centerID));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = commandText;
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if ((_dataset.Tables[0] != null) && (_dataset.Tables[0].Rows.Count > 0))
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        analysis = new CFRAnalysis();
                        analysis.AbsentMembersCount = (string.IsNullOrEmpty(dr["AbsentMembersCount"].ToString())) ? 0 : Convert.ToInt64(dr["AbsentMembersCount"]);
                        analysis.AttendanceMonth = (string.IsNullOrEmpty(dr["AttendanceMonth"].ToString())) ? "N/A" : dr["AttendanceMonth"].ToString().ToLower();
                        analysis.ContributorCount = (string.IsNullOrEmpty(dr["ContributorCount"].ToString())) ? 0 : Convert.ToInt64(dr["ContributorCount"]);
                        analysis.FSWUserId = (string.IsNullOrEmpty(dr["UserId"].ToString())) ? (Guid?)null : new Guid(dr["UserId"].ToString());
                        analysis.FSWUserName = (string.IsNullOrEmpty(dr["UserName"].ToString())) ? "N/A" : dr["UserName"].ToString();
                        analysis.PresentCount = (string.IsNullOrEmpty(dr["PresentCount"].ToString())) ? 0 : Convert.ToInt64(dr["PresentCount"]);
                        analysis.TotalFamilies= (string.IsNullOrEmpty(dr["TotalFamilies"].ToString())) ? 0 : Convert.ToInt64(dr["TotalFamilies"]);
                        cfrAnalysisList.Add(analysis);
                    }

                }
                useridList = cfrAnalysisList.Select(x => x.FSWUserId).Distinct().ToList();
                foreach(var item in useridList)
                {
                    ana.CfrList = cfrAnalysisList.Where(x => x.FSWUserId == item).ToList();
                    list.Add(ana.CfrList);
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
            return list;
        }
    }


   
}
