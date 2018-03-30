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
using System.Collections;

namespace FingerprintsData
{
    public class RosterData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        //SqlDataReader dataReader = null;
        SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;
        public List<Roster> Getrosterclient(string AgencyId, string UserId)
        {
            Roster _roster = new Roster();
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetrosterList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            info = new Roster();
                            info.Householid = dr["Householdid"].ToString();
                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.Gender = dr["gender"].ToString();
                            info.Picture = dr["DobAttachment"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["DobAttachment"]);
                            info.CenterName = dr["CenterName"].ToString();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString());
                            info.ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString());
                            info.ClassroomName = dr["ClassroomName"].ToString();
                            RosterList.Add(info);
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
            return RosterList;
        }
        public List<CaseNote> GetCaseNote(ref string Name, ref FingerprintsModel.RosterNew.Users Userlist, int Householdid, int centerid, string id, string AgencyId, string UserId)
        {
            List<CaseNote> CaseNoteList = new List<CaseNote>();


            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@clientid", id));
                command.Parameters.Add(new SqlParameter("@centerid", centerid));
                command.Parameters.Add(new SqlParameter("@Householdid", Householdid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getcasenotes";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CaseNote info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new CaseNote();
                            info.Householid = dr["householdid"].ToString();
                            info.CaseNoteid = dr["casenoteid"].ToString();
                            info.BY = dr["By"].ToString();
                            info.Title = dr["Title"].ToString();
                            info.Attachment = dr["Attachment"].ToString();
                            info.References = dr["References"].ToString();
                            info.Date = Convert.ToDateTime(dr["casenotedate"]).ToString("MM/dd/yyyy");
                            info.SecurityLevel = Convert.ToBoolean(dr["SecurityLevel"]);
                            CaseNoteList.Add(info);
                        }
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {

                            Name = dr["Name"].ToString();

                        }
                    }
                    if (_dataset != null && _dataset.Tables[2].Rows.Count > 0)
                    {

                        List<FingerprintsModel.RosterNew.User> Clientlist = new List<FingerprintsModel.RosterNew.User>();
                        FingerprintsModel.RosterNew.User obj = null;
                        foreach (DataRow dr in _dataset.Tables[2].Rows)
                        {
                            obj = new FingerprintsModel.RosterNew.User();
                            obj.Id = dr["clientid"].ToString();
                            obj.Name = dr["Name"].ToString();
                            Clientlist.Add(obj);
                        }
                        Userlist.Clientlist = Clientlist;
                    }
                    if (_dataset.Tables[3] != null && _dataset.Tables[3].Rows.Count > 0)
                    {
                        List<FingerprintsModel.RosterNew.User> _userlist = new List<FingerprintsModel.RosterNew.User>();
                        FingerprintsModel.RosterNew.User obj = null;
                        foreach (DataRow dr in _dataset.Tables[3].Rows)
                        {
                            obj = new FingerprintsModel.RosterNew.User();
                            obj.Id = (dr["UserId"]).ToString();
                            obj.Name = dr["Name"].ToString();
                            _userlist.Add(obj);
                        }
                        Userlist.UserList = _userlist;
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
            return CaseNoteList;
        }
        public void GetCaseNoteByTags(ref DataTable dtMaterial, int Householdid, int centerid, string id, string AgencyId, string UserId, string Tagnames)
        {
            dtMaterial = new DataTable();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@clientid", id));
                command.Parameters.Add(new SqlParameter("@centerid", centerid));
                command.Parameters.Add(new SqlParameter("@Householdid", Householdid));
                command.Parameters.Add(new SqlParameter("@Tagnames", Tagnames));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetcasenotesByTags";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtMaterial);
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

        }

        public void GetCaseNoteByNoteId(ref DataTable dtNotes, string NoteId)
        {
            dtNotes = new DataTable();
            try
            {
                command.Parameters.Add(new SqlParameter("@NoteId", NoteId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetNoteByCaseNoteId";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtNotes);
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

        }
        public void GetSubNotes(ref DataTable dtSubNotes, string CaseNoteId)
        {
            dtSubNotes = new DataTable();
            try
            {

                command.Parameters.Add(new SqlParameter("@CaseNoteId", CaseNoteId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetSubCaseNoteById";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtSubNotes);
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

        }
        public bool SaveSubNotes(string CaseNoteId, string CaseNoteDate, int Householdid, int centerid, string ClassRoomId, string AgencyId, string UserId, string Notes, string RoleId)
        {
            bool isInserted = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                DateTime dt = Convert.ToDateTime(CaseNoteDate);
                command.Parameters.Add(new SqlParameter("@CaseNoteID", CaseNoteId));
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@Householdid", Householdid));
                command.Parameters.Add(new SqlParameter("@ProgramYR", "16-17"));
                command.Parameters.Add(new SqlParameter("@SubCaseNoteDate", dt));
                command.Parameters.Add(new SqlParameter("@WrittenBy", UserId));
                command.Parameters.Add(new SqlParameter("@RoleOfOwner", RoleId));
                command.Parameters.Add(new SqlParameter("@Note", Notes));
                command.Parameters.Add(new SqlParameter("@CenterId", ClassRoomId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AddSubNotes";
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
                command.Dispose();
            }
            return isInserted;
        }
        public List<CaseNote> GetcaseNoteDetail(string Casenoteid, string ClientId, string AgencyId, string UserId)
        {
            List<CaseNote> CaseNoteList = new List<CaseNote>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@casenotid", Casenoteid));
                command.Parameters.Add(new SqlParameter("@Clientid", ClientId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getcasenote";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CaseNote info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new CaseNote();
                            info.Date = Convert.ToDateTime(dr["casenotedate"]).ToString("MM/dd/yyyy");
                            info.Title = dr["Title"].ToString();
                            info.clientid = dr["ClientID"].ToString();
                            info.Staffid = dr["Staffid"].ToString();
                            info.Note = dr["NoteField"].ToString();
                            info.Name = dr["Name"].ToString();
                            info.BY = dr["By"].ToString();
                            info.Tagname = dr["tagname"].ToString();
                            info.Attachment = dr["AttachmentId"].ToString();
                            info.SecurityLevel = Convert.ToBoolean(dr["SecurityLevel"]);
                            info.GroupCaseNote = dr["GroupCaseNote"].ToString();
                            CaseNoteList.Add(info);
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
            return CaseNoteList;
        }
        public FingerprintsModel.RosterNew.Users GetClient(string ClientId, string Agencyid)
        {
            FingerprintsModel.RosterNew.Users Userlist = new FingerprintsModel.RosterNew.Users();

            try
            {
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_getCnClients";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    List<FingerprintsModel.RosterNew.User> Clientlist = new List<FingerprintsModel.RosterNew.User>();
                    FingerprintsModel.RosterNew.User obj = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        obj = new FingerprintsModel.RosterNew.User();
                        obj.Id = dr["clientid"].ToString();
                        obj.Name = dr["Name"].ToString();
                        Clientlist.Add(obj);
                    }
                    Userlist.Clientlist = Clientlist;
                }
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    List<FingerprintsModel.RosterNew.User> _userlist = new List<FingerprintsModel.RosterNew.User>();
                    FingerprintsModel.RosterNew.User obj = null;
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        obj = new FingerprintsModel.RosterNew.User();
                        obj.Id = (dr["UserId"]).ToString();
                        obj.Name = dr["Name"].ToString();
                        _userlist.Add(obj);
                    }
                    Userlist.UserList = _userlist;
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


            return Userlist;
        }

        public bool SaveAttachmentsOnSubNote(string AgencyId, string CaseNoteId, byte[] file, string AttachmentName, string AttachmentExtension, string UserId)
        {
            bool isInserted = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@CaseNoteID", CaseNoteId));
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@Attachment", file));
                command.Parameters.Add(new SqlParameter("@AttachmentName", AttachmentName));
                command.Parameters.Add(new SqlParameter("@AttachmentExtension", AttachmentExtension));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AddCaseNoteAttachments";
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
                command.Dispose();
            }
            return isInserted;
        }

        public string SaveCaseNotes(ref string Name, ref List<CaseNote> CaseNoteList, ref FingerprintsModel.RosterNew.Users Userlist, RosterNew.CaseNote CaseNote, List<RosterNew.Attachment> Attachments, string Agencyid, string UserID)
        {
            string result = string.Empty;

            try
            {
                string HouseholdId = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@ClientId", CaseNote.ClientId));
                command.Parameters.Add(new SqlParameter("@CenterId", CaseNote.CenterId));
                command.Parameters.Add(new SqlParameter("@CNoteid", CaseNote.CaseNoteid));
                command.Parameters.Add(new SqlParameter("@ProgramId", CaseNote.ProgramId));
                command.Parameters.Add(new SqlParameter("@HouseHoldIdnew", CaseNote.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@Note", CaseNote.Note));
                command.Parameters.Add(new SqlParameter("@CaseNoteDate", CaseNote.CaseNoteDate));
                command.Parameters.Add(new SqlParameter("@CaseNoteSecurity", CaseNote.CaseNoteSecurity));
                command.Parameters.Add(new SqlParameter("@CaseNotetags", CaseNote.CaseNotetags));
                command.Parameters.Add(new SqlParameter("@CaseNotetitle", CaseNote.CaseNotetitle));
                command.Parameters.Add(new SqlParameter("@ClientIds", CaseNote.ClientIds));
                command.Parameters.Add(new SqlParameter("@StaffIds", CaseNote.StaffIds));
                command.Parameters.Add(new SqlParameter("@userid", UserID));
                command.Parameters.Add(new SqlParameter("@agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters.Add(new SqlParameter("@IsLateArrival", CaseNote.IsLateArrival));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("Attachment", typeof(byte[])),
                      new DataColumn("AttachmentName",typeof(string)),
                        new DataColumn("Attachmentextension",typeof(string))
                    });
                foreach (RosterNew.Attachment Attachment in Attachments)
                {
                    if (Attachment != null && Attachment.file != null)
                    {
                        dt.Rows.Add(new BinaryReader(Attachment.file.InputStream).ReadBytes(Attachment.file.ContentLength), Attachment.file.FileName, Path.GetExtension(Attachment.file.FileName));

                    }
                }
                command.Parameters.Add(new SqlParameter("@Attachments", dt));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveCaseNote";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CaseNote info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new CaseNote();
                            info.Householid = dr["householdid"].ToString();
                            info.CaseNoteid = dr["casenoteid"].ToString();
                            info.BY = dr["By"].ToString();
                            info.Title = dr["Title"].ToString();
                            info.Attachment = dr["Attachment"].ToString();
                            info.References = dr["References"].ToString();
                            info.Date = Convert.ToDateTime(dr["casenotedate"]).ToString("MM/dd/yyyy");
                            CaseNoteList.Add(info);
                        }
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {

                            Name = dr["Name"].ToString();

                        }
                    }

                    if (_dataset != null && _dataset.Tables[2].Rows.Count > 0)
                    {

                        List<FingerprintsModel.RosterNew.User> Clientlist = new List<FingerprintsModel.RosterNew.User>();
                        FingerprintsModel.RosterNew.User obj = null;
                        foreach (DataRow dr in _dataset.Tables[2].Rows)
                        {
                            obj = new FingerprintsModel.RosterNew.User();
                            obj.Id = dr["clientid"].ToString();
                            obj.Name = dr["Name"].ToString();
                            Clientlist.Add(obj);
                        }
                        Userlist.Clientlist = Clientlist;
                    }
                    if (_dataset != null && _dataset.Tables[3].Rows.Count > 0)
                    {
                        List<FingerprintsModel.RosterNew.User> _userlist = new List<FingerprintsModel.RosterNew.User>();
                        FingerprintsModel.RosterNew.User obj = null;
                        foreach (DataRow dr in _dataset.Tables[3].Rows)
                        {
                            obj = new FingerprintsModel.RosterNew.User();
                            obj.Id = (dr["UserId"]).ToString();
                            obj.Name = dr["Name"].ToString();
                            _userlist.Add(obj);
                        }
                        Userlist.UserList = _userlist;
                    }

                }



                result = command.Parameters["@result"].Value.ToString();

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
            return result;





        }
        //Changes on 30Dec2016
        public Roster GetrosterList(out string totalrecord, string sortOrder, string sortDirection, string Center, string Classroom, int skip, int pageSize, string userid, string agencyid, string roleId, int filterOption,string searchText="")
        {
            Roster _roster = new Roster();
            List<Roster> RosterList = new List<Roster>();
            List<HrCenterInfo> centerList = new List<HrCenterInfo>();
            _roster.Rosters = new List<Roster>();
            int IssuePercentage = 0;
            try
            {
                totalrecord = string.Empty;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Center", Center));
                command.Parameters.Add(new SqlParameter("@Classroom", Classroom));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                command.Parameters.Add(new SqlParameter("@SearchText", searchText));
                command.Parameters.Add(new SqlParameter("@FilterOption", filterOption));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@issuePercentage", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_rosterList";
              // command.CommandText = "SP_rosterList_Test_02_05_2018";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                IssuePercentage = Convert.ToInt32(command.Parameters["@issuePercentage"].Value.ToString());
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        //Roster info = null;
                        //foreach (DataRow dr in _dataset.Tables[0].Rows)
                        //{

                        //    info = new Roster();
                        //    info.Householid = dr["Householdid"].ToString();

                        //    info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                        //    info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                        //    info.Name = dr["name"].ToString();
                        //    info.Gender = dr["gender"].ToString();
                        //    info.CenterName = dr["CenterName"].ToString();
                        //    info.CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString());
                        //    info.ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString());
                        //    info.RosterYakkr = dr["Yakkr"].ToString();
                        //    info.Yakkr600 = DBNull.Value.Equals(dr["yakkr600"]) ? 0 : Convert.ToInt32(dr["yakkr600"]);
                        //    info.Yakkr601 = DBNull.Value.Equals(dr["Yakkr601"]) ? 0 : Convert.ToInt32(dr["Yakkr601"]);
                        //    info.ClassroomName = dr["ClassroomName"].ToString();
                        //    info.MarkAbsenseReason = DBNull.Value.Equals(dr["MarkedAbsentReason"]) ? "" : Convert.ToString(dr["MarkedAbsentReason"]);
                        //    info.IsPresent = DBNull.Value.Equals(dr["IsPresent"]) ? 0 : Convert.ToInt32(dr["IsPresent"]);//.ToString() //Added on 30Dec2016
                        //    info.Acronym = dr["AcronymName"].ToString();
                        //    info.LastCaseNoteDate = string.IsNullOrEmpty(dr["LastCaseNoteDate"].ToString()) ? "" : dr["LastCaseNoteDate"].ToString();
                        //    info.LastFPADate = string.IsNullOrEmpty(dr["FPALastDate"].ToString()) ? "" : dr["FPALastDate"].ToString();
                        //    info.LastReferralDate = string.IsNullOrEmpty(dr["LastReferralDate"].ToString()) ? "" : dr["LastReferralDate"].ToString();
                        //    info.LateArivalNotes= DBNull.Value.Equals(dr["LateArivalNotes"]) ? "" : Convert.ToString(dr["LateArivalNotes"]);
                        //    info.LateArrivalDuration= DBNull.Value.Equals(dr["LateArrivalDuration"]) ? "" : Convert.ToString(dr["LateArrivalDuration"]);
                        //    info.IsLateArrival= DBNull.Value.Equals(dr["IsLateArrival"]) ? false : Convert.ToBoolean(dr["IsLateArrival"]);
                        //    info.IsCaseNoteEntered= DBNull.Value.Equals(dr["IsCaseNoteEntered"]) ? false : Convert.ToBoolean(dr["IsCaseNoteEntered"]);
                        //    info.IsHomeBased = Convert.ToInt32(dr["IsHomeBased"]);
                        //    info.IsAppointMentYakkr600601 = DBNull.Value.Equals(dr["IsAppointMentYakkr600601"]) ? 0 : Convert.ToInt32(dr["IsAppointMentYakkr600601"]);
                        //    info.Age = dr["Age"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dr["Age"].ToString());
                        //    info.IsPreg = string.IsNullOrEmpty(dr["IsPreg"].ToString()) ? 0 : Convert.ToInt32(dr["IsPreg"].ToString());
                        //    info.ProgramType = "";

                        //    if (dr["ProgramType"].ToString() == "EHS" && info.Age > 3)
                        //    {
                        //        info.ProgramType = "TRN";
                        //    }
                        //    else if (info.IsPreg == 1)
                        //    {
                        //        info.ProgramType = "TRN";
                        //    }
                        //    RosterList.Add(info);
                        //}


                        _roster.Rosters = (from DataRow dr in _dataset.Tables[0].Rows
                                           select new Roster
                                           {
                                               Householid = dr["Householdid"].ToString(),
                                               Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString()),
                                               EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString()),
                                               Name = dr["name"].ToString(),
                                               Gender = dr["gender"].ToString(),
                                               CenterName = dr["CenterName"].ToString(),
                                               CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                                               ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString()),
                                               RosterYakkr = dr["Yakkr"].ToString(),
                                               Yakkr600 = DBNull.Value.Equals(dr["yakkr600"]) ? 0 : Convert.ToInt32(dr["yakkr600"]),
                                               Yakkr601 = DBNull.Value.Equals(dr["Yakkr601"]) ? 0 : Convert.ToInt32(dr["Yakkr601"]),
                                               ClassroomName = dr["ClassroomName"].ToString(),
                                               MarkAbsenseReason = DBNull.Value.Equals(dr["MarkedAbsentReason"]) ? "" : Convert.ToString(dr["MarkedAbsentReason"]),
                                               IsPresent = DBNull.Value.Equals(dr["IsPresent"]) ? 0 : Convert.ToInt32(dr["IsPresent"]),//.ToString() //Added on 30Dec2016
                                               Acronym = dr["AcronymName"].ToString(),
                                               LastCaseNoteDate = string.IsNullOrEmpty(dr["LastCaseNoteDate"].ToString()) ? "" : dr["LastCaseNoteDate"].ToString(),
                                               LastFPADate = string.IsNullOrEmpty(dr["FPALastDate"].ToString()) ? "" : dr["FPALastDate"].ToString(),
                                               LastReferralDate = string.IsNullOrEmpty(dr["LastReferralDate"].ToString()) ? "" : dr["LastReferralDate"].ToString(),
                                               LateArivalNotes = DBNull.Value.Equals(dr["LateArivalNotes"]) ? "" : Convert.ToString(dr["LateArivalNotes"]),
                                               LateArrivalDuration = DBNull.Value.Equals(dr["LateArrivalDuration"]) ? "" : Convert.ToString(dr["LateArrivalDuration"]),
                                               IsLateArrival = DBNull.Value.Equals(dr["IsLateArrival"]) ? false : Convert.ToBoolean(dr["IsLateArrival"]),
                                               IsCaseNoteEntered = DBNull.Value.Equals(dr["IsCaseNoteEntered"]) ? false : Convert.ToBoolean(dr["IsCaseNoteEntered"]),
                                               IsHomeBased = Convert.ToInt32(dr["IsHomeBased"]),
                                               IsClassStarted = DBNull.Value.Equals(dr["IsClassStarted"]) ? 0 : Convert.ToInt32(dr["IsClassStarted"]),

                                               IsAppointMentYakkr600601 = DBNull.Value.Equals(dr["IsAppointMentYakkr600601"]) ? 0 : Convert.ToInt32(dr["IsAppointMentYakkr600601"]),
                                               Age = dr["Age"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dr["Age"].ToString()),
                                               IsPreg = string.IsNullOrEmpty(dr["IsPreg"].ToString()) ? 0 : Convert.ToInt32(dr["IsPreg"].ToString()),
                                               ProgramType = (dr["ProgramType"].ToString() == "EHS" && (dr["Age"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dr["Age"].ToString())) >= 3) ? "TRN" : (dr["IsPreg"].ToString() == "1") ? "TRN" : ""
                                           }

                                         ).Distinct().ToList();

                      //  _roster.Rosters = RosterList;
                    }
                }

                //if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                //{
                //    HrCenterInfo info = null;
                //    foreach (DataRow dr in _dataset.Tables[1].Rows)
                //    {
                //        info = new HrCenterInfo();
                //        info.CenterId = dr["center"].ToString();
                //        info.Name = dr["centername"].ToString();
                //        centerList.Add(info);
                //    }
                //    _roster.Centers = centerList;
                //}

                _roster.AbsenceTypeList = new List<SelectListItem>();

                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        _roster.AbsenceTypeList = (from DataRow dr5 in _dataset.Tables[1].Rows
                                                     select new SelectListItem
                                                     {
                                                         Text = dr5["Description"].ToString(),
                                                         Value = dr5["AttendanceTypeId"].ToString()
                                                     }).ToList();

                    }
                }
                _roster.AbsenceReasonList = new List<SelectListItem>();
                if (_dataset.Tables[2] != null)
                {
                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        _roster.AbsenceReasonList = (from DataRow dr5 in _dataset.Tables[2].Rows
                                                       select new SelectListItem
                                                       {
                                                           Text = dr5["absenseReason"].ToString(),
                                                           Value = dr5["reasonid"].ToString()
                                                       }).ToList();

                    }
                }

                if (_dataset.Tables[3] != null)
                {
                    // _TeacherM.TodayClosed = Convert.ToInt32(_dataset.Tables[3].Rows[0]["TodayClosed"]);

                    List<ClosedInfo> closedList = new List<ClosedInfo>();
                    closedList = (from DataRow dr5 in _dataset.Tables[3].Rows
                                  select new ClosedInfo
                                  {
                                      ClosedToday = Convert.ToInt32(dr5["TodayClosed"]),
                                      CenterName = dr5["CenterName"].ToString(),
                                      ClassRoomName = dr5["ClassRoomName"].ToString(),
                                      AgencyName = dr5["AgencyName"].ToString()
                                  }
                              ).ToList();
                    closedList = closedList.Where(x => x.ClosedToday > 0).ToList();
                    if(closedList.Count()>0)
                    {
                        _roster.ClosedDetails = new ClosedInfo
                        {
                            ClosedToday = closedList.Select(x => x.ClosedToday).FirstOrDefault(),
                            CenterName = string.Join(",", closedList.Select(x => x.CenterName).Distinct().ToArray()),
                            ClassRoomName = string.Join(",", closedList.Select(x => x.ClassRoomName).Distinct().ToArray()),
                            AgencyName = closedList.Select(x => x.AgencyName).FirstOrDefault()
                        };
                    }
                    else
                    {
                        _roster.ClosedDetails = new ClosedInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);

            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();

            }
            return _roster;

        }





        public void MarkAbsent(ref string result, string ChildID, string UserID, string absentType, string Cnotes, string agencyid, int AbsentReasonid,string NewReason)
        {

        
            try
            {
                result = "";
                if (absentType == "1")
                {
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();
                    Connection.Open();
                    command.Connection = Connection;
                    command.Parameters.Add(new SqlParameter("@UserID", UserID));
                    command.Parameters.Add(new SqlParameter("@clientID", ChildID));
                    command.Parameters.Add(new SqlParameter("@PSignature", " "));
                    command.Parameters.Add(new SqlParameter("@PareID", " "));
                    command.Parameters.Add(new SqlParameter("@Notes", " "));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters["@result"].Direction = ParameterDirection.Output;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_MarkAttendancePresent";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    command.Dispose();
                    result = command.Parameters["@result"].Value.ToString();
                }
                else
                {
                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();
                    Connection.Open();
                    command.Connection = Connection;
                    command.Parameters.Add(new SqlParameter("@UserID", UserID));
                    command.Parameters.Add(new SqlParameter("@ClientID", ChildID));
                    command.Parameters.Add(new SqlParameter("@AttendanceType", absentType));
                    command.Parameters.Add(new SqlParameter("@AbsenceReasonId", AbsentReasonid));
                    command.Parameters.Add(new SqlParameter("@NewReason", NewReason));
                    command.Parameters.Add(new SqlParameter("@Notes", ""));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters["@result"].Direction = ParameterDirection.Output;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_MarkAttendanceAbsent";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    result = command.Parameters["@result"].Value.ToString();
                    Connection.Close();
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

        }


        public List<REF> AutoCompleteSerType(string Services, string agencyId)
        {
            List<REF> RefList = new List<REF>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "AutoComplete_ReferralType";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Services", Services);
                        command.Parameters.AddWithValue("@AgencyId", agencyId);

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
                            REF obj = new REF();
                            obj.Services = dr["OrganizationName"].ToString();
                            obj.ServiceID = Convert.ToInt32(dr["CommunityId"]);
                            obj.Address = dr["Address"].ToString();
                            obj.Phone = dr["PhoneNo"].ToString();
                            obj.Email = dr["Email"].ToString();
                            RefList.Add(obj);
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
            return RefList;
        }
        public List<ClassRoom> Getclassrooms(string Centerid, string Agencyid)
        {
            List<ClassRoom> _ClassRoomlist = new List<ClassRoom>();

            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
                command.Parameters.Add(new SqlParameter("@Centerid", Centerid));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getclassrooms";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    ClassRoom obj = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        obj = new ClassRoom();
                        obj.ClassroomID = Convert.ToInt32(dr["ClassroomID"].ToString());
                        obj.ClassName = dr["ClassroomName"].ToString();
                        obj.Enc_ClassRoomId = EncryptDecrypt.Encrypt64(dr["ClassroomID"].ToString());
                        _ClassRoomlist.Add(obj);
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
            return _ClassRoomlist;
        }

        public string AddFPA(FPA info, int mode, Guid userId, string AgencyId)
        {
            DataTable dt = new DataTable();
            try
            {

                //DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[6] {
                    new DataColumn("Desc ", typeof(string)),
                    new DataColumn("Status",typeof(string)),
                    new DataColumn("CompletionDate",typeof(string)),
                    new DataColumn("Email",typeof(bool)),
                    new DataColumn("StepID",typeof(Int32)),
                    new DataColumn("DaysForReminder",typeof(Int32)),
                   // new DataColumn("Category",typeof(Int32))
                    });


                command.Connection = Connection;
                command.CommandText = "SP_addFPA";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@FPAID", info.FPAID);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Goal", info.Goal);
                command.Parameters.AddWithValue("@GoalDate", info.GoalDate);
                command.Parameters.AddWithValue("@ClientId", info.ClientId);
                command.Parameters.AddWithValue("@CompletionDate", info.CompletionDate);
                command.Parameters.AddWithValue("@Element", info.Element);
                command.Parameters.AddWithValue("@Domain", info.Domain);
                command.Parameters.AddWithValue("@Category", info.Category);
                command.Parameters.AddWithValue("@Status", info.GoalStatus);
                command.Parameters.AddWithValue("@StaffId", userId);
                command.Parameters.AddWithValue("@CreatedBy", userId);
                command.Parameters.AddWithValue("@GoalFor", info.GoalFor);
                if (info.GoalSteps.Count > 0)
                {
                    command.Parameters.AddWithValue("@SignatureData", info.SignatureData);
                }
                else
                {
                    command.Parameters.AddWithValue("@SignatureData", null);
                }

                if (info.GoalSteps== null && info.GoalSteps.Count == 0)
                {
                    

                    foreach (FPASteps steps in info.GoalSteps)
                    {
                        if (steps.Description != null)
                        {
                            dt.Rows.Add(steps.Description, steps.Status, steps.StepsCompletionDate, steps.Email, steps.StepID, steps.Reminderdays);//
                        }
                    }
                }
                command.Parameters.Add(new SqlParameter("@tblSteps", dt));

                SqlParameter result = new SqlParameter("@result", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };

                //Changes

                //command.Parameters.AddWithValue("@uncheckedall", info.UncheckedAll);
                //command.Parameters.AddWithValue("@Centers", info.Centers);
                command.Parameters.Add(result);

                command.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                command.ExecuteScalar();
                Connection.Close();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return ex.Message;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                command.Dispose();
            }

        }

        public void CheckByClient(string GetParameter, int Mode)
        {
            //   DataTable dt = new DataTable();   
            //string Parameter = "INSERT";  
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Command", GetParameter);
                command.Parameters.AddWithValue("@Mode", Mode);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CheckByClient";
                int RowsAffected = command.ExecuteNonQuery();
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

        }



        //--------------------------------------------------------------------------------------------------------
        public List<MatchProviderModel> MatchProviders(string AgencyId, string CommunityId, long? ReferralClientId)
        {
            List<MatchProviderModel> matchProviderModel = new List<MatchProviderModel>();
            try
            {
                DataSet ds = null;
                ds = new DataSet();
                command.Connection = Connection;
                Connection.Open();
                command.CommandText = "SP_ReferralCategory_Services";

                if (ReferralClientId > 0)
                {
                    command.Parameters.AddWithValue("@ReferralClientId", ReferralClientId);
                    command.Parameters.AddWithValue("@Command", "EDIT");
                }
                else
                {
                    command.Parameters.AddWithValue("@Command", "SELECT");
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@CommunityId", CommunityId);
                }

                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                Connection.Close();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MatchProviderModel matchProvider = new MatchProviderModel();
                        matchProvider.ServiceId = Convert.ToInt32(dr["ServiceID"]);
                        matchProvider.Services = dr["Services"].ToString();
                        matchProvider.Notes = dr["Notes"].ToString();
                       // matchProvider.OrganizationName = dr["OrganizationName"].ToString();
                       // matchProvider.Address = dr["Address"].ToString();
                        matchProvider.AgencyId = dr["AgencyId"].ToString();
                        matchProvider.IsFunction = Convert.ToBoolean(dr["IsFunction"]);
                        matchProvider.CommunityId = Convert.ToInt32(dr["CommunityId"]);

                        if (ReferralClientId > 0)
                        {
                            matchProvider.City = dr["City"].ToString();
                            matchProvider.State = dr["State"].ToString();
                            matchProvider.ZipCode = dr["ZipCode"].ToString();
                            matchProvider.Email = dr["Email"].ToString();
                            matchProvider.Phone = dr["PhoneNo"].ToString();
                            matchProvider.ReferralDate = (dr["ReferralDate"].ToString() == "") ? DateTime.Now.ToString("MM/dd/yyyy") : dr["ReferralDate"].ToString();
                            matchProvider.ReferralClientServiceId = Convert.ToInt64(dr["ReferralClientServiceId"]);
                            matchProvider.ClientId = Convert.ToInt64(dr["ClientId"]);
                        }
                        matchProviderModel.Add(matchProvider);
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return matchProviderModel;
        }


        public bool DeleteReferralService(long ReferralClientServiceId)
        {
            try
            {
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.CommandText = "sp_ReferralOperations";
                command.Parameters.AddWithValue("@ReferralClientServiceId", ReferralClientServiceId);
                command.Parameters.AddWithValue("@Command", "DeleteReferralService");
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return false;
            }
        }

        public bool SaveHouseHold(long ClientId, long CommonClientId, int Step, bool Status, long HouseHoldId, long referralClientId, string queryCommand, string clientIdarray = "")
        {
            try
            {
                bool result = false;
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.CommandText = "sp_FamilyHouseHoldOperations";
                command.Parameters.AddWithValue("@ClientId", ClientId);
                command.Parameters.AddWithValue("@CommonClientId", CommonClientId);
                command.Parameters.AddWithValue("@Step", Step);
                command.Parameters.AddWithValue("@HouseHoldStatus", Status);
                command.Parameters.AddWithValue("@HouseHoldId", HouseHoldId);
                command.Parameters.AddWithValue("@ReferralClientServiceId", referralClientId);
                command.Parameters.AddWithValue("@ClinetIdArray", clientIdarray);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                Connection.Close();

                return result;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return false;
            }
        }

        public long SaveReferralClient(long ServiceId, long CommonClientId, Guid AgencyId, Int32 Step, bool Status, int CreatedBy, long referralclientId)
        {
            try
            {
                DataSet ds = null;
                ds = new DataSet();
                string queryCommand = "INSERT";
                if (referralclientId > 0)
                {
                    Connection.Open();
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.CommandText = "sp_ReferralOperations";
                    command.Parameters.AddWithValue("@ServiceId", ServiceId);
                    command.Parameters.AddWithValue("@CommonClientId", CommonClientId);
                    command.Parameters.AddWithValue("@Command", "SELECT");
                    command.Parameters.AddWithValue("@Step", Step);
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Close();
                    SqlDataAdapter DA = new SqlDataAdapter(command);
                    DA.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        queryCommand = "ReferralServiceUPDATE";
                    }
                }


                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.CommandText = "sp_ReferralOperations";
                command.Parameters.AddWithValue("@CommonClientId", CommonClientId);
                command.Parameters.AddWithValue("@ServiceId", ServiceId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Step", Step);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                var obj = command.ExecuteScalar();
                Connection.Close();
                long retriveID = Convert.ToInt64(obj);
                return retriveID;

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return 0;
            }
        }

        public List<MatchProviderModel> GetAllReferralClient(long UniqueClientId)
        {
            List<MatchProviderModel> matchproviderList = new List<MatchProviderModel>();
            DataSet ds = null;
            ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "sp_ReferralOperations";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@ClientId", UniqueClientId);
            command.Parameters.AddWithValue("@Command", "GETALLSELECT");
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sqlDA = new SqlDataAdapter(command);
            sqlDA.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    MatchProviderModel matchProvider = new MatchProviderModel();
                    matchProvider.ServiceId = Convert.ToInt32(dr["ServiceId"]);
                    matchProvider.ClientId = Convert.ToInt32(dr["ClientId"]);
                    matchproviderList.Add(matchProvider);
                }
            }
            return matchproviderList;
        }

        public bool UpdateReferralClient(long ClientId, long ServiceId)
        {
            try
            {
                command.Connection = Connection;
                Connection.Open();
                command.CommandText = "sp_ReferralOperations";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@ClientId", ClientId);
                command.Parameters.AddWithValue("@ServiceId", ServiceId);
                command.Parameters.AddWithValue("@Command", "UPDATE");
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool SaveMatchProviders(string PickupDate, string NotesId, int ServiceResourceId, string AgencyId, string UserId, int ClientId, long CommunityId, long ReferralClientServiceId)
        {
            try
            {
                int Step = 3;
                bool Status = true;
                int CreatedBy = ClientId;



                DateTime date = Convert.ToDateTime(PickupDate);

                Connection.Open();
                command.Connection = Connection;
                command.CommandText = "sp_MatchProviderOperations";
                if (ReferralClientServiceId > 0)
                {
                    command.Parameters.AddWithValue("@Command", "");
                }
                else
                {
                    command.Parameters.AddWithValue("@Command", "INSERT");
                }
                command.Parameters.AddWithValue("@ReferralDate", date);
                command.Parameters.AddWithValue("@ClientId", ClientId);
                command.Parameters.AddWithValue("@NotesId", NotesId);
                command.Parameters.AddWithValue("@ServiceId", ServiceResourceId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Step", Step);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                command.Parameters.AddWithValue("@CommunityId", CommunityId);
                command.Parameters.AddWithValue("@ReferralClientServiceId", ReferralClientServiceId);

                command.CommandType = CommandType.StoredProcedure;
                var query = command.ExecuteNonQuery();
                Connection.Close();
                bool Success = true;
                return Success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool YakkarInsert(string agencyId, string userId, int clientID, int routeCode = 450)
        {
            try
            {

                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.CommandText = "sp_InsertYakkrReferral";
                command.Parameters.AddWithValue("@AgencyId", agencyId);
                command.Parameters.AddWithValue("@ClientId", clientID);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@HouseHoldId", null);
                command.Parameters.AddWithValue("@Routecode", routeCode);
                command.Parameters.AddWithValue("@ProgramId", null);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return false;
            }
        }
        public List<ReferralServiceModel> ReferralService(long ClientId)
        {
            List<ReferralServiceModel> referralServiceModelList = new List<ReferralServiceModel>();

            DataSet ds = null;
            ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "sp_ReferralServiceOperations";
            command.Parameters.AddWithValue("@ClientId", ClientId);
            command.Parameters.AddWithValue("@Command", "SELECT");
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ReferralServiceModel referralService = new ReferralServiceModel();
                    referralService.ServiceId = Convert.ToInt32(dr["ServiceID"]);
                    referralService.ServiceName = dr["Services"].ToString();
                    referralService.ReferralDate = Convert.ToDateTime(dr["ReferralDate"]);
                    referralService.ConvReferralDate = Convert.ToDateTime(dr["ReferralDate"]).ToString("MM/dd/yyyy");
                    referralService.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    referralService.ConvCreatedDate = Convert.ToDateTime(dr["CreatedDate"]).ToString("MM/dd/yyyy");
                    referralService.Step = Convert.ToInt32(dr["Step"]);
                    referralService.ClientId = Convert.ToInt32(dr["ClientId"]);
                    referralService.ReferralClientServiceId = Convert.ToInt32(dr["ReferralClientServiceId"]);
                    referralService.ParentName = dr["ParentName"].ToString();
                    referralServiceModelList.Add(referralService);
                }
            }

            return referralServiceModelList;
        }


        public MatchProviderModel FamilyResourcesList(Int32 ServiceId, Guid AgencyId)
        {
            DataSet ds = null;
            MatchProviderModel matchProviderModel = new MatchProviderModel();
            ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "sp_FamilyNeeds";
            command.Parameters.AddWithValue("@CommunityId", ServiceId);
            command.Parameters.AddWithValue("@AgencyID", AgencyId);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    matchProviderModel.CommunityId = Convert.ToInt32(dr["CommunityId"].ToString());
                    matchProviderModel.OrganizationName = dr["OrganizationName"].ToString();
                }
            }

            return matchProviderModel;
        }

        public MatchProviderModel GetOrganization(Int32 CommunityId)
        {
            DataSet ds = null;
            MatchProviderModel matchProviderModel = new MatchProviderModel();
            ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "sp_GetOrganization";
            command.Parameters.AddWithValue("@CommunityId", CommunityId);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    matchProviderModel.Address = dr["Address"].ToString();
                    matchProviderModel.CommunityId = Convert.ToInt32(dr["CommunityId"].ToString());
                    matchProviderModel.OrganizationName = dr["OrganizationName"].ToString();
                    matchProviderModel.City = dr["City"].ToString();
                    matchProviderModel.State = dr["State"].ToString();
                    matchProviderModel.ZipCode = dr["ZipCode"].ToString();
                    matchProviderModel.Email = dr["Email"].ToString();
                    matchProviderModel.Phone = dr["PhoneNo"].ToString();
                }
            }

            return matchProviderModel;
        }



        public List<SelectListItem> FamilyServiceList(Int64 ServiceId, string AgencyId)
        {
            try
            {
                DataSet ds = null;
                Connection.Open();
                command.Parameters.Clear();
                List<MatchProviderModel> matchProviderModels = new List<MatchProviderModel>();
                List<SelectListItem> organizationList = new List<SelectListItem>();
                ds = new DataSet();
                command.Connection = Connection;

                command.CommandText = "sp_FamilyNeeds";
                command.Parameters.AddWithValue("@CommunityId", ServiceId);
                command.Parameters.AddWithValue("@AgencyID", AgencyId);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                Connection.Close();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MatchProviderModel matchProviderModel = new MatchProviderModel();
                        matchProviderModel.CommunityId = Convert.ToInt32(dr["CommunityId"].ToString());
                        matchProviderModel.OrganizationName = dr["OrganizationName"].ToString();
                        matchProviderModels.Add(matchProviderModel);

                    }
                    organizationList = matchProviderModels
                             .Select(x => new SelectListItem
                             {
                                 Text = x.OrganizationName,
                                 Value = x.CommunityId.ToString()
                             }).ToList();
                }

                return organizationList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<REF> ReferralCategory(int clientid, long? ReferralClientId, int? Step)
        {
            List<REF> objreferralList = new List<REF>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "SP_GetReferralInfo";
                        if (ReferralClientId > 0)
                        {
                            command.Parameters.AddWithValue("@ReferralClientId", ReferralClientId);
                            command.Parameters.AddWithValue("@ClientId", clientid);
                            command.Parameters.AddWithValue("@Command", "EDIT");
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ClientId", clientid);
                            command.Parameters.AddWithValue("@Command", "SELECT");
                        }
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        REF objReferral = new REF();
                        var FullName = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                        objReferral.ParentName = FullName;
                        objReferral.AgencyID = dr["AgencyID"].ToString();
                        objReferral.ClientID = Convert.ToInt64(dr["ClientID"]);
                        objReferral.IsChild = Convert.ToBoolean(dr["IsChild"]);
                        objReferral.IsFamily = Convert.ToBoolean(dr["IsFamily"]);
                        objReferral.HouseHoldId = Convert.ToInt32(dr["HouseholdID"]);
                        objReferral.ParentRole = Convert.ToInt32(dr["ParentRole"].ToString());
                        objReferral.Step = Step;
                        objReferral.IsFamilyCheckStatus = true;
                        objReferral.IsClient = Convert.ToBoolean(dr["IsClient"]);
                        objreferralList.Add(objReferral);
                    }
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        REF objReferral = new REF();
                        objReferral.ServiceID = Convert.ToInt32(dr["ServiceID"].ToString());
                        objReferral.Description = dr["Description"].ToString();
                        objReferral.Services = dr["Services"].ToString();
                        objReferral.Status = Convert.ToBoolean(dr["Status"]);
                        objReferral.CategoryID = Convert.ToInt64(dr["CategoryID"].ToString());
                        objReferral.IsFamilyCheckStatus = false;
                        objreferralList.Add(objReferral);
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return objreferralList;
        }


        public List<SelectListItem> GetSelectedReferrals(long referralClientId)
        {

            DataSet ds = null;
            List<SelectListItem> selectedReferrals = new List<SelectListItem>();

            Connection.Open();
            command.Parameters.Clear();
            command.Connection = Connection;

            ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "sp_FamilyHouseHoldOperations";
            command.Parameters.AddWithValue("@ClientId", 0);
            command.Parameters.AddWithValue("@CommonClientId", 0);
            command.Parameters.AddWithValue("@Step", 0);
            command.Parameters.AddWithValue("@HouseHoldStatus", 0);
            command.Parameters.AddWithValue("@HouseHoldId", 0);
            command.Parameters.AddWithValue("@ReferralClientServiceId", referralClientId);
            command.Parameters.AddWithValue("@ClinetIdArray", "");
            command.Parameters.AddWithValue("@Command", "SELECT");
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            Connection.Close();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    selectedReferrals.Add(new SelectListItem
                    {
                        Text = "",
                        Value = dr["ClientId"].ToString()

                    });
                }

            }
            return selectedReferrals;
        }



        //-------------------------------------------------------------------------------------------------------------------
        public List<REF> ReferralCategoryCompany(int clientid)
        {
            List<REF> objreferralList = new List<REF>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "SP_GetReferInfo";
                        command.Parameters.AddWithValue("@ClientId", clientid);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        REF objReferral = new REF();
                        var FullName = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                        objReferral.ParentName = FullName;
                        objReferral.AgencyID = dr["AgencyID"].ToString();
                        objReferral.ClientID = Convert.ToInt64(dr["ClientID"]);
                        objReferral.IsChild = Convert.ToBoolean(dr["IsChild"]);
                        objReferral.HouseHoldId = Convert.ToInt32(dr["HouseholdID"]);
                        objReferral.IsFamily = Convert.ToBoolean(dr["IsFamily"]);
                        objReferral.ParentRole = Convert.ToInt32(dr["ParentRole"].ToString());
                        objReferral.Status = true;
                        objReferral.IsClient = Convert.ToBoolean(dr["IsClient"]);
                        objreferralList.Add(objReferral);
                    }

                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        REF objReferral = new REF();
                        objReferral.ServiceID = Convert.ToInt32(dr["ServiceID"].ToString());
                        objReferral.Description = dr["Description"].ToString();
                        objReferral.Services = dr["Services"].ToString();
                        objReferral.CategoryID = Convert.ToInt64(dr["CategoryID"].ToString());
                        objReferral.Status = false;
                        objreferralList.Add(objReferral);
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return objreferralList;
        }
        public FPA editFPA(string id)
        {
            FPA FPAinfo = new FPA();
            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getFPAinfo";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["GoalTopic"]).ToString()))
                        {
                            FPAinfo.Goal = (_dataset.Tables[0].Rows[0]["GoalTopic"]).ToString();
                        }
                        else
                        {
                            FPAinfo.Goal = string.Empty;
                        }
                        FPAinfo.FPAID = Convert.ToInt64(_dataset.Tables[0].Rows[0]["FPAID"]);
                        FPAinfo.GoalDate = Convert.ToDateTime(_dataset.Tables[0].Rows[0]["Date"]).ToString("MM/dd/yyyy");
                        FPAinfo.CompletionDate = Convert.ToDateTime(_dataset.Tables[0].Rows[0]["CompletionDate"]).ToString("MM/dd/yyyy");
                        //Changes
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["Element"]).ToString()))
                        {
                            FPAinfo.Element = (_dataset.Tables[0].Rows[0]["Element"]).ToString();
                        }
                        else
                        {
                            FPAinfo.Element = string.Empty;
                        }
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["Description"]).ToString()))
                        {
                            FPAinfo.ElemDesc = (_dataset.Tables[0].Rows[0]["Description"]).ToString();
                        }
                        else
                        {
                            FPAinfo.ElemDesc = string.Empty;
                        }
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["Domain"]).ToString()))
                        {
                            FPAinfo.Domain = (_dataset.Tables[0].Rows[0]["Domain"]).ToString();
                        }
                        else
                        {
                            FPAinfo.Domain = string.Empty;
                        }
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["Category"]).ToString()))
                        {
                            FPAinfo.Category = (_dataset.Tables[0].Rows[0]["Category"]).ToString();
                        }
                        else
                        {
                            FPAinfo.Category = string.Empty;
                        }



                        FPAinfo.GoalStatus = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Status"]);

                    }

                }
                DataAdapter.Dispose();
                command.Dispose();
                return FPAinfo;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return FPAinfo;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
        }
        public FPA GetData_AllDropdown()
        {
            FPA _objFPA = new FPA();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_FPA_Dropdowndata";//Sp_Sel_RefProg_Dropdowndata  Sp_Sel_Prog_Dropdowndata
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0] != null)
                {
                    try
                    {
                        List<FPA.DomainInfo> _domlist = new List<FPA.DomainInfo>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FPA.DomainInfo obj = new FPA.DomainInfo();
                            obj.Id = dr["ID"].ToString();//ReferenceId  ProgramTypeID
                            obj.Name = dr["Description"].ToString();//ProgramType //Name
                            _domlist.Add(obj);
                        }

                        _objFPA.domList = _domlist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    try
                    {
                        List<FPA.CategoryInfo> _catelist = new List<FPA.CategoryInfo>();
                        //_staff.myList
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            FPA.CategoryInfo obj = new FPA.CategoryInfo();
                            obj.Id = dr["ID"].ToString();//ReferenceId  ProgramTypeID
                            obj.Name = dr["CategoryName"].ToString();//ProgramType //Name
                            _catelist.Add(obj);
                        }

                        _objFPA.cateList = _catelist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                return _objFPA;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _objFPA;
            }
            finally
            {

                command.Dispose();
            }

        }
        public List<ElementInfo> GetElementInfo(string DoaminId)
        {
            List<ElementInfo> _Elementlist = new List<ElementInfo>();

            try
            {
                command.Parameters.Add(new SqlParameter("@DoaminId", DoaminId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetElements";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    ElementInfo obj = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        obj = new ElementInfo();
                        obj.Id = (dr["ID"].ToString());
                        obj.Name = dr["Description"].ToString();
                        _Elementlist.Add(obj);
                    }
                }
                return _Elementlist;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _Elementlist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            // return _Elementlist;
        }
        //Added by Akansha on 18Nov2016
        public string AddFPASteps(FPASteps stepsDetails, string userId, string agencyId, List<FPASteps.StepsInfo> StepsData)//, List<Agency.FundSource.ProgramType> Prog
        {
            SqlDataReader dataReader = null;
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
                command.CommandText = "Sp_addstepsinfo";//Sp_addeditagency_withfunds   Sp_addeditagency
                command.Parameters.Add(new SqlParameter("@FPAID", stepsDetails.FPAID));
                command.Parameters.Add(new SqlParameter("@ClientId", stepsDetails.ClientId));
                command.Parameters.Add(new SqlParameter("@agencyId", agencyId));
                //command.Parameters.Add(new SqlParameter("@IsActive", 1));

                //  command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@createdBy", userId));


                //Category and Service 
                if (StepsData != null && StepsData.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("Desc ", typeof(string)),
                    new DataColumn("Status",typeof(string)),
                    new DataColumn("CompletionDate",typeof(string)),
                    new DataColumn("Email",typeof(bool)),
                    new DataColumn("StepID",typeof(Int32)),
                   // new DataColumn("Category",typeof(Int32))
                    });

                    //foreach (FPASteps.StepsInfo steps in StepsData)
                    //{
                    //    if (steps.Description != null)
                    //    {
                    //        dt.Rows.Add(steps.Description, steps.Assignment, steps.StepsCompletionDate, steps.Email, steps.StepsID);//
                    //    }


                    //}
                    command.Parameters.Add(new SqlParameter("@tblSteps", dt));
                    //command.Parameters.Add(new SqlParameter("@tblprog", dt1));
                }
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
        public FPASteps editFPASteps(Int64 id)
        {
            FPASteps _FPASteps = new FPASteps();
            try
            {
                command.Parameters.Add(new SqlParameter("@FPAID", id));

                //  command.Parameters.Add(new SqlParameter("@agencyid",agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getFPAStepsinfo";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                //if (_dataset.Tables.Count > 1)
                //{
                if (_dataset.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < _dataset.Tables[0].Rows.Count; i++)
                    {
                        _FPASteps.FPAID = Convert.ToInt64(_dataset.Tables[0].Rows[i]["FPAID"].ToString());
                        _FPASteps.ClientId = Convert.ToInt32(_dataset.Tables[0].Rows[i]["ClientId"].ToString());
                        _FPASteps.Name = _dataset.Tables[0].Rows[i]["GoalTopic"].ToString();
                        //_FPASteps.ChildName = _dataset.Tables[0].Rows[i]["ChildName"].ToString();
                        //_FPASteps.ParentName = _dataset.Tables[0].Rows[i]["ParentName"].ToString();
                        //_FPASteps.FSWName = _dataset.Tables[0].Rows[i]["name"].ToString();
                        //  _FPASteps.ParentEmailId = _dataset.Tables[0].Rows[i]["ParentEmail"].ToString();
                        //if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[i]["ParentEmail"].ToString()))
                        //{
                        //    _FPASteps.ParentEmailId = _dataset.Tables[0].Rows[i]["ParentEmail"].ToString();
                        //}
                        //else
                        //{
                        //    _FPASteps.ParentEmailId = string.Empty;
                        //}

                        List<FingerprintsModel.FPASteps.StepsInfo> _categorylist = new List<FingerprintsModel.FPASteps.StepsInfo>();
                        FingerprintsModel.FPASteps.StepsInfo obj = new FingerprintsModel.FPASteps.StepsInfo();
                        //obj.StepsID = Convert.ToInt32(_dataset.Tables[0].Rows[i]["StepID"].ToString());
                        //obj.Description = _dataset.Tables[0].Rows[i]["Desc"].ToString();

                        //obj.ClassSession = _dataset.Tables[1].Rows[i]["ClassSession"].ToString();
                        //if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[i]["Status"].ToString()))
                        //{
                        //    obj.Assignment = _dataset.Tables[0].Rows[i]["Status"].ToString();
                        //}
                        //else
                        //{
                        //    obj.Assignment = string.Empty;
                        //}

                        //if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[i]["CompletionDate"].ToString()))
                        //{
                        //    // FPAinfo.GoalDate = Convert.ToDateTime(_dataset.Tables[0].Rows[0]["Date"]).ToString("MM/dd/yyyy");
                        //    obj.StepsCompletionDate = Convert.ToDateTime(_dataset.Tables[0].Rows[i]["CompletionDate"]).ToString("MM/dd/yyyy");
                        //}
                        //else
                        //{
                        //    obj.StepsCompletionDate = string.Empty;
                        //}





                        _FPASteps.StepsData.Add(obj);

                    }

                }

                // if (_dataset.Tables[2].Rows.Count > 0)
                // }




                return _FPASteps;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _FPASteps;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
        }
        public List<FPA> FPAlist(string ClientId, out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string agencyid)
        {
            List<FPA> _FPAlist = new List<FPA>();
            DataTable _dataTable = null;
            try
            {
                totalrecord = string.Empty;
                string searchcenter = string.Empty;
                string AgencyId = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchcenter = string.Empty;
                else
                    searchcenter = search;
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.Parameters.Add(new SqlParameter("@Search", searchcenter));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                if (!string.IsNullOrEmpty(agencyid))
                    command.Parameters.Add(new SqlParameter("@agencyID", agencyid));
                // command.Parameters.Add(new SqlParameter("@agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_getFPAlist";
                DataAdapter = new SqlDataAdapter(command);
                _dataTable = new DataTable();
                DataAdapter.Fill(_dataTable);
                if (_dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < _dataTable.Rows.Count; i++)
                    {
                        FPA addFPA = new FPA();
                        addFPA.FPAID = Convert.ToInt64(_dataTable.Rows[i]["FPAID"]);
                        addFPA.Goal = _dataTable.Rows[i]["GoalTopic"].ToString();
                        addFPA.Category = _dataTable.Rows[i]["Category"].ToString();
                        addFPA.Domain = _dataTable.Rows[i]["Domain"].ToString();
                        addFPA.DomainDesc = _dataTable.Rows[i]["DomainDesc"].ToString();
                        addFPA.CategoryDesc = _dataTable.Rows[i]["CategoryDesc"].ToString();
                        addFPA.Element = _dataTable.Rows[i]["Element"].ToString();
                        addFPA.ElemDesc = _dataTable.Rows[i]["Description"].ToString();
                        addFPA.CompletionDate = Convert.ToDateTime(_dataTable.Rows[i]["CompletionDate"]).ToString("MM/dd/yyyy");
                        addFPA.GoalStatus = Convert.ToInt32(_dataTable.Rows[i]["Status"].ToString());
                        addFPA.ClientId = Convert.ToInt32(_dataTable.Rows[i]["ClientID"].ToString());

                        _FPAlist.Add(addFPA);
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
                _dataTable.Dispose();
            }
        }
        public FPA GetFPADetails(string FPAId)
        {
            FPA obj = new FPA();
            try
            {
                command.Parameters.Add(new SqlParameter("@FPAId", FPAId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetFPADetails";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    obj.FPAID = Convert.ToInt64(_dataset.Tables[0].Rows[0]["FPAID"]);
                    obj.Category = _dataset.Tables[0].Rows[0]["Category"].ToString();
                    obj.Goal = _dataset.Tables[0].Rows[0]["GoalTopic"].ToString();
                    obj.GoalDate = Convert.ToDateTime(_dataset.Tables[0].Rows[0]["Date"]).ToString("MM/dd/yyyy");
                    obj.Domain = _dataset.Tables[0].Rows[0]["Domain"].ToString();
                    obj.CategoryDesc = _dataset.Tables[0].Rows[0]["CategoryDesc"].ToString();
                    obj.CompletionDate = Convert.ToDateTime(_dataset.Tables[0].Rows[0]["CompletionDate"]).ToString("MM/dd/yyyy");
                    obj.Element = _dataset.Tables[0].Rows[0]["Element"].ToString();
                    obj.ElemDesc = _dataset.Tables[0].Rows[0]["Description"].ToString();
                    obj.GoalStatus = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Status"].ToString());
                    obj.Category = _dataset.Tables[0].Rows[0]["Category"].ToString();
                    obj.ClientId = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ClientId"]);
                    //obj.StaffId = (_dataset.Tables[0].Rows[0]["StaffID"]);

                }


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
                _dataset.Dispose();
            }
        }
        public string DeleteStepView(string StepId)
        {
            string result = string.Empty;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@StepID", StepId));
                //  command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UDP_DeleteStepById";
                command.Connection.Open();
                int i = command.ExecuteNonQuery();
                command.Connection.Close();
                result = "1";
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
            finally
            {
                command.Connection.Close();
                command.Dispose();
            }
        }
        public object DeleteFPA(string FPAID)
        {
            string strresult = string.Empty;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@FPAId", Convert.ToInt32(FPAID)));
                SqlParameter result = new SqlParameter("@result", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                command.Parameters.Add(result);
                //  command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UDP_DeleteGoal";
                command.Connection.Open();
                command.ExecuteScalar();

                command.Connection.Close();
                strresult = command.Parameters["@result"].Value.ToString();
                if (strresult == "0")
                {
                    string DeleteParameter = "DELETE";
                    int Mode = 2;
                  //  CheckByClient(DeleteParameter, Mode);
                }
                return strresult;
            }
            catch (Exception ex)
            {
                strresult = ex.Message;
                return strresult;
            }
            finally
            {
                command.Connection.Close();
                command.Dispose();
            }
        }
        public List<FPA> GetFPAGoalListForHousehold(out string totalrecord, string search, string ClientId, string sortOrder, string sortDirection, int skip, int pageSize)
        {
            List<FPA> li = new List<FPA>();
            command = new SqlCommand();
            DataAdapter = new SqlDataAdapter(command);

            try
            {
                command.Parameters.Add(new SqlParameter("@id", ClientId));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@_search", search));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getFPAinfo";
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                DataAdapter.Dispose();
                command.Dispose();
                foreach (DataRow item in _dataset.Tables[0].Rows)
                {
                    FPA FPAinfo = new FPA();
                    if (!string.IsNullOrEmpty((item["GoalTopic"]).ToString()))
                    {
                        FPAinfo.Goal = (item["GoalTopic"]).ToString();
                    }
                    else
                    {
                        FPAinfo.Goal = string.Empty;
                    }
                    // FPAinfo.AgencyId = new Guid(item["AgencyId"].ToString());
                    //  FPAinfo.ClientId = Convert.ToInt64(item["ClientId"].ToString());
                    FPAinfo.ChildName = item["ChildName"].ToString().TrimEnd().TrimStart();
                    FPAinfo.EncyrptedClientId = FingerprintsModel.EncryptDecrypt.Encrypt64(item["ClientId"].ToString());
                    //  FPAinfo.FPAID = Convert.ToInt64(item["FPAID"]);
                    FPAinfo.GoalStatus = Convert.ToInt32(item["Status"]);
                    FPAinfo.EncriptedFPAID = FingerprintsModel.EncryptDecrypt.Encrypt64(Convert.ToInt32(item["FPAID"]).ToString());
                    FPAinfo.GoalDate = Convert.ToDateTime(item["GoalDate"]).ToString("MM/dd/yyyy");
                    FPAinfo.CompletionDate = Convert.ToDateTime(item["CompletionDate"]).ToString("MM/dd/yyyy");
                    //Changes
                    FPAinfo.GoalFor = Convert.ToInt32(item["GoalForParent"].ToString());
                    FPAinfo.ParentName1 = item["Parentname1"].ToString();
                    FPAinfo.ParentEmailId1 = item["parentEmail1"].ToString();
                    FPAinfo.ParentName2 = item["Parentname2"].ToString();
                    FPAinfo.ParentEmailId2 = item["parentEmail2"].ToString();
                    FPAinfo.IsEmail1 = item["noEmail1"].ToString().TrimEnd().TrimStart() == "1" ? false : true;
                    FPAinfo.IsEmail2 = item["noEmail2"].ToString().TrimEnd().TrimStart() == "1" ? false : true;
                    FPAinfo.IsSingleParent = item["IsSingleParent"].ToString().TrimEnd().TrimStart() == "1" ? true : false;
                    if (!string.IsNullOrEmpty((item["Element"]).ToString()))
                    {
                        FPAinfo.Element = (item["Element"]).ToString();
                    }
                    else
                    {
                        FPAinfo.Element = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((item["ElementDescription"]).ToString()))
                    {
                        FPAinfo.ElemDesc = (item["ElementDescription"]).ToString();
                    }
                    else
                    {
                        FPAinfo.ElemDesc = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((item["DomainDescription"]).ToString()))
                    {
                        FPAinfo.DomainDesc = (item["DomainDescription"]).ToString();
                    }
                    else
                    {
                        FPAinfo.DomainDesc = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((item["Domain"]).ToString()))
                    {
                        FPAinfo.Domain = (item["Domain"]).ToString();
                    }
                    else
                    {
                        FPAinfo.Domain = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((item["Category"]).ToString()))
                    {
                        FPAinfo.Category = (item["Category"]).ToString();
                    }
                    else
                    {
                        FPAinfo.Category = string.Empty;
                    }
                    if (!string.IsNullOrEmpty((item["CategoryDescription"]).ToString()))
                    {
                        FPAinfo.CategoryDesc = (item["CategoryDescription"]).ToString();
                    }
                    else
                    {
                        FPAinfo.CategoryDesc = string.Empty;
                    }
                    FPAinfo.GoalStatus = Convert.ToInt32(item["Status"]);




                    li.Add(FPAinfo);
                }

                return li;
                //foreach (DataRow item in _dataset.Tables[1].Rows)
                //{
                //    //cl.HouseHoldId,cl.Isfamily,cl.[IsChild],cl.[IsOther],
                //    //fd.FPAID,fd.StepId,fd.AgencyId,fd.ClientId,fd.[status] as StepStatus,
                //    //fd.CompletionDate,fd.Email,fd.DaysForReminder,cl.Firstname,cl.Lastname,cl.[Status] as ClientStatus

                //    FPASteps obj = new FPASteps();
                //    obj.AgencyId = new Guid(item["AgencyId"].ToString());
                //    if (item["Isfamily"].ToString() == "1")
                //    {
                //        obj.ParentName = item["Firstname"].ToString() + "  " + item["Lastname"].ToString();
                //    }
                //    else if (item["IsChild"].ToString() == "1")
                //    {
                //        obj.ChildName = item["Firstname"].ToString() + "  " + item["Lastname"].ToString();
                //    }
                //    obj.ClientId = Convert.ToInt64(item["ClientId"].ToString());
                //    obj.Name = item["GoalName"].ToString();
                //    if (!string.IsNullOrEmpty(item["EmailId"].ToString()))
                //    {
                //        obj.ParentEmailId = item["EmailId"].ToString();
                //    }
                //    obj.FPAID = Convert.ToInt32(item["FPAID"].ToString());
                //    obj.Description = item["desc"].ToString();
                //    obj.Status =Convert.ToInt32( item["StepStatus"].ToString());
                //    if (!string.IsNullOrEmpty(item["DaysForReminder"].ToString()))
                //    {
                //        obj.Reminderdays = Convert.ToInt32(item["DaysForReminder"].ToString());
                //    }
                //    obj.StepsCompletionDate = Convert.ToDateTime(item["stepComplitionDate"]).ToString("MM/dd/yyyy");

                //    li.Where(x => x.FPAID.ToString() == item["FPAID"].ToString()).FirstOrDefault().GoalSteps.Add(obj);
                //}



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                totalrecord = string.Empty;
                return li;

            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
        }


        public FPA GetFpa(Int64 fpaid)
        {
            FPA obj = new FPA();
            try
            {
                command = new SqlCommand();
                command.Connection = Connection;
                SqlParameter param = new SqlParameter("@FPAId", fpaid);
                command.CommandText = "UDP_GetFPAGOALby_Id";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(param);
                DataSet ds = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(ds);
                command.Connection.Close();
                DataAdapter.Dispose();
                command.Dispose();
                if (ds != null && ds.Tables.Count > 0)
                {

                    obj.FPAID = Convert.ToInt64(ds.Tables[0].Rows[0]["FPAID"]);
                    obj.Category = ds.Tables[0].Rows[0]["Category"].ToString();
                    obj.GoalFor = Convert.ToInt32(ds.Tables[0].Rows[0]["GoalForParent"].ToString());
                    obj.Goal = ds.Tables[0].Rows[0]["GoalTopic"].ToString();
                    obj.GoalDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["Date"]).ToString("MM/dd/yyyy");
                    obj.CategoryDesc = ds.Tables[0].Rows[0]["CategoryName"].ToString();
                    obj.CompletionDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CompletionDate"]).ToString("MM/dd/yyyy");
                    obj.GoalStatus = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString());
                    obj.ClientId = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientId"]);
                    obj.ParentName1 = ds.Tables[0].Rows[0]["Parentname1"].ToString();
                    obj.ParentEmailId1 = ds.Tables[0].Rows[0]["parentEmail1"].ToString();
                    obj.ParentName2 = ds.Tables[0].Rows[0]["Parentname2"].ToString();
                    obj.ParentEmailId2 = ds.Tables[0].Rows[0]["parentEmail2"].ToString();
                    obj.SignatureData = ds.Tables[0].Rows[0]["SignatureData"].ToString();
                    obj.ChildName = ds.Tables[0].Rows[0]["childName"].ToString();
                    obj.IsEmail1 = ds.Tables[0].Rows[0]["noEmail1"].ToString().TrimEnd().TrimStart() == "True" ? false : true;
                    obj.IsEmail2 = ds.Tables[0].Rows[0]["noEmail2"].ToString().TrimEnd().TrimStart() == "True" ? false : true;
                    obj.IsSingleParent = ds.Tables[0].Rows[0]["IsSingleParent"].ToString().TrimEnd().TrimStart() == "1" ? true : false;
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Logo"].ToString()))
                    {
                        obj.AgencyLogo = Convert.ToBase64String((byte[])ds.Tables[0].Rows[0]["Logo"]);
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ActualGoalCompletionDate"].ToString()))
                    {
                        obj.ActualGoalCompletionDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ActualGoalCompletionDate"]).ToString("MM/dd/yyyy");
                    }

                }
                //[StepID],[Desc],[Category],[Status],[CompletionDate],[Email] as IsEmail,[DaysForReminder]
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    FPASteps objstep = new FPASteps();
                    objstep.Description = item["Desc"].ToString();
                    objstep.Status = Convert.ToInt32(item["Status"].ToString());
                    objstep.Reminderdays = Convert.ToInt32((item["DaysForReminder"] != DBNull.Value ? item["DaysForReminder"].ToString() : null));
                    if (!string.IsNullOrEmpty(item["CompletionDate"].ToString()))
                    {
                        objstep.StepsCompletionDate = Convert.ToDateTime(item["CompletionDate"]).ToString("MM/dd/yyyy");
                    }
                    objstep.StepID = Convert.ToInt32(item["StepID"].ToString());
                    objstep.Comments = item["Comments"].ToString();
                    if (!string.IsNullOrEmpty(item["ActualCompletionDate"].ToString()))
                    {
                        objstep.ActualCompletionDate = Convert.ToDateTime(item["ActualCompletionDate"].ToString()).ToString("MM/dd/yyyy");
                    }
                    objstep.FPAID = obj.FPAID;
                    obj.GoalSteps.Add(objstep);
                }




            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Connection.Close();
                DataAdapter.Dispose();
                command.Dispose();
            }
            return obj;
        }


        public DataSet getParentNames(long clientId)
        {
            DataSet ds = new DataSet();
            try
            {
                command = new SqlCommand();
                command.Connection = Connection;
                command.CommandText = "USP_getParentnamebyClientId";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClientId", clientId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(ds);
                command.Connection.Close();
                DataAdapter.Dispose();
                command.Dispose();
                return ds;

            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            finally
            {
                command.Connection.Close();
                DataAdapter.Dispose();
                command.Dispose();
            }
            return ds;
        }
        //AddFPAParent on 27Dec2016
        public string UpdateFPAParent(FPA info)//, Guid userId, string AgencyId)
        {
            DataTable dt = new DataTable();
            try
            {
                command.Connection = Connection;
                command.CommandText = "SP_UpdateFPA";

                command.Parameters.AddWithValue("@FPAID", info.FPAID);
                command.Parameters.AddWithValue("@ActualGoalCompletionDate", info.ActualGoalCompletionDate);
                command.Parameters.AddWithValue("@Status", info.GoalStatus);
                if (info.GoalSteps != null && info.GoalSteps.Count > 0)
                {
                    //DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[8] {
                    new DataColumn("Desc ", typeof(string)),
                    new DataColumn("Status",typeof(string)),
                    new DataColumn("CompletionDate",typeof(string)),
                    new DataColumn("Email",typeof(bool)),
                    new DataColumn("StepID",typeof(Int32)),
                    new DataColumn("DaysForReminder",typeof(Int32)),
                    new DataColumn("Comments",typeof(string)),
                     new DataColumn("ActualCompletionDate",typeof(string))
                   // new DataColumn("Category",typeof(Int32))
                    });

                    foreach (FPASteps steps in info.GoalSteps)
                    {
                        if (steps.Description != null)
                        {
                            dt.Rows.Add(steps.Description, steps.Status, steps.StepsCompletionDate, steps.Email, steps.StepID, steps.Reminderdays, steps.Comments, steps.ActualCompletionDate);//
                        }
                    }
                }
                command.Parameters.Add(new SqlParameter("@tblSteps", dt));

                SqlParameter result = new SqlParameter("@result", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };

                //Changes

                //command.Parameters.AddWithValue("@uncheckedall", info.UncheckedAll);
                //command.Parameters.AddWithValue("@Centers", info.Centers);
                command.Parameters.Add(result);

                command.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                command.ExecuteScalar();
                Connection.Close();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return ex.Message;
            }
            finally
            {
                // DataAdapter.Dispose();
                command.Dispose();
            }

        }

        //End
        //End
        //rohit 01032017
        public List<CaseNote> GetgroupcaseNoteDetail(string Casenoteid, string AgencyId, string UserId)
        {
            List<CaseNote> CaseNoteList = new List<CaseNote>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@casenotid", Casenoteid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetGroupcasenote";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CaseNote info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new CaseNote();
                            info.Date = Convert.ToDateTime(dr["casenotedate"]).ToString("MM/dd/yyyy");
                            info.Title = dr["Title"].ToString();
                            info.clientid = dr["ClientID"].ToString();
                            info.Staffid = dr["Staffid"].ToString();
                            info.Note = dr["NoteField"].ToString();
                            info.Name = dr["Name"].ToString();
                            info.BY = dr["By"].ToString();
                            info.Tagname = dr["tagname"].ToString();
                            info.Attachment = dr["AttachmentId"].ToString();
                            info.SecurityLevel = Convert.ToBoolean(dr["SecurityLevel"]);
                            CaseNoteList.Add(info);
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
            return CaseNoteList;
        }

        public FPA GetFpaforParents(Int64 fpaid)
        {
            FPA obj = new FPA();
            try
            {
                command = new SqlCommand();
                command.Connection = Connection;
                SqlParameter param = new SqlParameter("@FPAId", fpaid);
                command.CommandText = "UDP_GetFPAGOALby_Id";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(param);
                DataSet ds = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(ds);
                command.Connection.Close();
                DataAdapter.Dispose();
                command.Dispose();
                if (ds != null && ds.Tables.Count > 0)
                {

                    obj.FPAID = Convert.ToInt64(ds.Tables[0].Rows[0]["FPAID"]);
                    obj.Category = ds.Tables[0].Rows[0]["Category"].ToString();
                    obj.GoalFor = Convert.ToInt32(ds.Tables[0].Rows[0]["GoalForParent"].ToString());
                    obj.Goal = ds.Tables[0].Rows[0]["GoalTopic"].ToString();
                    obj.GoalDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["Date"]).ToString("MM/dd/yyyy");
                    obj.CategoryDesc = ds.Tables[0].Rows[0]["CategoryName"].ToString();
                    obj.CompletionDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CompletionDate"]).ToString("MM/dd/yyyy");
                    obj.GoalStatus = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString());
                    obj.ClientId = Convert.ToInt32(ds.Tables[0].Rows[0]["ClientId"]);
                    obj.ParentName1 = ds.Tables[0].Rows[0]["Parentname1"].ToString();
                    obj.ParentEmailId1 = ds.Tables[0].Rows[0]["parentEmail1"].ToString();
                    obj.ParentName2 = ds.Tables[0].Rows[0]["Parentname2"].ToString();
                    obj.ParentEmailId2 = ds.Tables[0].Rows[0]["parentEmail2"].ToString();
                    obj.SignatureData = ds.Tables[0].Rows[0]["SignatureData"].ToString();
                    obj.ChildName = ds.Tables[0].Rows[0]["childName"].ToString();
                    obj.IsEmail1 = ds.Tables[0].Rows[0]["noEmail1"].ToString().TrimEnd().TrimStart() == "True" ? false : true;
                    obj.IsEmail2 = ds.Tables[0].Rows[0]["noEmail2"].ToString().TrimEnd().TrimStart() == "True" ? false : true;
                    obj.IsSingleParent = ds.Tables[0].Rows[0]["IsSingleParent"].ToString().TrimEnd().TrimStart() == "1" ? true : false;
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ActualGoalCompletionDate"].ToString()))
                    {
                        obj.ActualGoalCompletionDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ActualGoalCompletionDate"]).ToString("MM/dd/yyyy");
                    }


                }
                //[StepID],[Desc],[Category],[Status],[CompletionDate],[Email] as IsEmail,[DaysForReminder]
                foreach (DataRow item in ds.Tables[1].Rows)
                {
                    FPASteps objstep = new FPASteps();
                    objstep.Description = item["Desc"].ToString();
                    objstep.Status = Convert.ToInt32(item["Status"].ToString());
                    objstep.Reminderdays = Convert.ToInt32((item["DaysForReminder"] != DBNull.Value ? item["DaysForReminder"].ToString() : null));
                    objstep.StepsCompletionDate = Convert.ToDateTime(item["CompletionDate"]).ToString("MM/dd/yyyy"); ;
                    objstep.StepID = Convert.ToInt32(item["StepID"].ToString());
                    objstep.Comments = item["Comments"].ToString();
                    if (!string.IsNullOrEmpty(item["ActualCompletionDate"].ToString()))
                    {
                        objstep.ActualCompletionDate = Convert.ToDateTime(item["ActualCompletionDate"].ToString()).ToString("MM/dd/yyyy");
                    }
                    objstep.FPAID = obj.FPAID;
                    obj.GoalSteps.Add(objstep);
                }




            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Connection.Close();
                DataAdapter.Dispose();
                command.Dispose();
            }
            return obj;
        }
        public List<PDFGeneration> CompleteServicePdf(string ClientID)
        {
            List<PDFGeneration> RefList = new List<PDFGeneration>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "PdfGeneration";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ClientId", ClientID);
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
                            PDFGeneration obj = new PDFGeneration();
                            //   obj.DOB = Convert.ToDateTime(dr["DOB"]).ToString("MM/dd/yyyy");
                            obj.DOB = dr["DOB"].ToString();

                            obj.FirstName = dr["FirstName"].ToString();
                            // obj.LastName = dr["LastName"].ToString();
                            RefList.Add(obj);
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
            return RefList;
        }


        public List<CompanyDetails> CompanyDetailsList(string ServiceId)
        {
            List<CompanyDetails> RefList1 = new List<CompanyDetails>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "PDFCompanyDetails";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@serviceID", ServiceId);

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        try
                        {

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                CompanyDetails obj = new CompanyDetails();
                                obj.Services = dr["Services"].ToString();
                                RefList1.Add(obj);
                            }
                        }
                        catch (Exception ex)
                        {
                            clsError.WriteException(ex);
                        }
                    }
                }

                else
                {

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return RefList1;
        }



        public List<CommunityDetails> CommunityDetailsList(string CommunityID)
        {
            List<CommunityDetails> RefList2 = new List<CommunityDetails>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "PDFCommunityDetails";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CommunityID", CommunityID);
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
                            CommunityDetails obj = new CommunityDetails();
                            obj.CompanyName = dr["CompanyName"].ToString();
                            obj.Address = dr["Address"].ToString();
                            obj.Phoneno = dr["Phoneno"].ToString();
                            obj.Email = dr["Email"].ToString();
                            RefList2.Add(obj);
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
            return RefList2;
        }

        public List<SurveyOptions> LoadSurveyOptions(long ReferralClientId, string userId)
        {
            List<SurveyOptions> surveyOptionsList = new List<SurveyOptions>();
            try
            {
                string queryCommand = "Select";
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "SP_SurveyOptions";
                        command.Parameters.Clear();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Command", queryCommand);
                        command.Parameters.AddWithValue("@ReferralClientId", ReferralClientId);
                        command.Parameters.AddWithValue("@UserId", userId);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);

                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SurveyOptions surveyOptions = new SurveyOptions();
                        surveyOptions.QuestionsId = Convert.ToInt32(dr["QuestionId"]);
                        surveyOptions.Questions = dr["Questions"].ToString();
                        surveyOptions.Answer = dr["Answer"].ToString();
                        surveyOptions.Explanation = dr["Explanation"].ToString();
                        var answerid = dr["SurveyAnswerId"].ToString();
                        surveyOptions.AnswerId = (string.IsNullOrEmpty(answerid)) ? 0 : Convert.ToInt32(answerid);
                        surveyOptions.CreatedDate = dr["CreatedDate"].ToString();
                        surveyOptionsList.Add(surveyOptions);

                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return surveyOptionsList;
        }


        public void InsertSurveyOptions(List<SurveyOptions> options, long ReferralClientId, string userId, bool isUpdate)
        {

            string queryCommand = (isUpdate) ? "Update" : "insert";
            if (options.Count > 0)
            {
                foreach (var item in options)
                {
                    Connection.Open();
                    command.Connection = Connection;

                    command.Connection = Connection;
                    command.CommandText = "SP_SurveyOptions";
                    command.Parameters.Clear();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Command", queryCommand);
                    command.Parameters.AddWithValue("@QuestionId", item.QuestionsId);
                    command.Parameters.AddWithValue("@Answer", item.Answer);
                    command.Parameters.AddWithValue("@SurveyAnswerId", item.AnswerId);
                    command.Parameters.AddWithValue("@Explanation", item.Explanation);
                    command.Parameters.AddWithValue("@ReferralClientId", ReferralClientId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    var data = command.ExecuteNonQuery();
                    Connection.Close();


                }
            }
        }

        public List<BusinessHours> BusinessHoursList(string ServiceId, string AgencyID, string CommunityID)
        {
            List<BusinessHours> RefList3 = new List<BusinessHours>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "PdfGenerationBusinessHours";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@serviceID", ServiceId);
                        command.Parameters.AddWithValue("@AgencyID", AgencyID);
                        command.Parameters.AddWithValue("@CommunityID", CommunityID);
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
                            BusinessHours obj1 = new BusinessHours();
                            obj1.MonFrom = dr["MonFrom"].ToString();
                            obj1.MonTo = dr["MonTo"].ToString();
                            obj1.TueFrom = dr["TueFrom"].ToString();
                            obj1.TueTo = dr["TueTo"].ToString();
                            obj1.WedFrom = dr["WedFrom"].ToString();
                            obj1.WedTo = dr["WedTo"].ToString();
                            obj1.ThursFrom = dr["ThursFrom"].ToString();
                            obj1.ThursTo = dr["ThursTo"].ToString();
                            obj1.FriFrom = dr["FriFrom"].ToString();
                            obj1.FriTo = dr["FriTo"].ToString();
                            obj1.SatFrom = dr["SatFrom"].ToString();
                            obj1.SatTo = dr["SatTo"].ToString();
                            obj1.SunFrom = dr["SunFrom"].ToString();
                            obj1.SunTo = dr["SunTo"].ToString();
                            obj1.Services = dr["Services"].ToString();
                            RefList3.Add(obj1);
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
            return RefList3;
        }

        public long SaveReferral(string PickupDate, string NotesId, int ServiceResourceId, string AgencyId, string UserId, int ClientId, long CommunityId, long ReferralClientServiceId)
        {
            try
            {
                int Step = 3;
                bool Status = true;
                int CreatedBy = ClientId;

                bool IsAgency = true;

                DateTime date = Convert.ToDateTime(PickupDate);

                Connection.Open();
                command.Connection = Connection;
                command.CommandText = "sp_ReferalOperations";
                if (ReferralClientServiceId > 0)
                {
                    command.Parameters.AddWithValue("@Command", "");
                }
                else
                {
                    command.Parameters.AddWithValue("@Command", "INSERT");
                }
                command.Parameters.AddWithValue("@ReferralDate", date);
                command.Parameters.AddWithValue("@ClientId", ClientId);
                command.Parameters.AddWithValue("@NotesId", NotesId);
                command.Parameters.AddWithValue("@ServiceId", ServiceResourceId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Step", Step);
                command.Parameters.AddWithValue("@IsAgency", IsAgency);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                command.Parameters.AddWithValue("@CommunityId", CommunityId);
                command.Parameters.AddWithValue("@ReferralClientServiceId", ReferralClientServiceId);

                command.CommandType = CommandType.StoredProcedure;
                //  command.ExecuteNonQuery();
                var obj = command.ExecuteScalar();
                var refId = Convert.ToInt64(obj);
                Connection.Close();

                return refId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> FamilyServiceList(Int32 ServiceId, string AgencyId)
        {
            try
            {
                DataSet ds = null;
                Connection.Open();
                command.Parameters.Clear();
                List<MatchProviderModel> matchProviderModels = new List<MatchProviderModel>();
                List<SelectListItem> organizationList = new List<SelectListItem>();
                ds = new DataSet();
                command.Connection = Connection;

                command.CommandText = "sp_FamilyNeeds";
                command.Parameters.AddWithValue("@CommunityId", ServiceId);
                command.Parameters.AddWithValue("@AgencyID", AgencyId);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                Connection.Close();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        MatchProviderModel matchProviderModel = new MatchProviderModel();
                        matchProviderModel.CommunityId = Convert.ToInt32(dr["CommunityId"].ToString());
                        matchProviderModel.OrganizationName = dr["OrganizationName"].ToString();
                        matchProviderModels.Add(matchProviderModel);

                    }
                    organizationList = matchProviderModels
                             .Select(x => new SelectListItem
                             {
                                 Text = x.OrganizationName,
                                 Value = x.CommunityId.ToString()
                             }).ToList();
                }

                return organizationList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public REF GetOrganizationCompany(Int32 CommunityId)
        {
            DataSet ds = null;
            REF refOrg = new REF();
            ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "sp_GetOrganizationCompany";
            command.Parameters.AddWithValue("@CommunityId", CommunityId);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    refOrg.Address = dr["Address"].ToString();
                    refOrg.CommunityId = Convert.ToInt32(dr["CommunityId"].ToString());
                    refOrg.OrganizationName = dr["OrganizationName"].ToString();
                    refOrg.City = dr["City"].ToString();
                    refOrg.State = dr["State"].ToString();
                    refOrg.ZipCode = dr["ZipCode"].ToString();
                    refOrg.Email = dr["Email"].ToString();
                    refOrg.Phone = dr["PhoneNo"].ToString();
                }
            }

            return refOrg;
        }

        public List<SelectListItem> GetServiceReference(string agencyId)
        {
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            DataSet ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "SP_GetReferralServices";
            command.Parameters.AddWithValue("@AgencyID", agencyId);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    selectedlist.Add(new SelectListItem
                    {
                        Text = dr["Services"].ToString(),
                        Value = dr["ServiceId"].ToString()
                    });
                }
            }
            return selectedlist;
        }

        public List<SelectListItem> FamilyServiceListCompany(Int32 ServiceId, string AgencyId)
        {
            try
            {
                DataSet ds = null;
                Connection.Open();
                command.Parameters.Clear();
                List<REF> refModels = new List<REF>();
                List<SelectListItem> organizationList = new List<SelectListItem>();
                ds = new DataSet();
                command.Connection = Connection;

                command.CommandText = "sp_FamilyNeedsCompanyDetails";
                command.Parameters.AddWithValue("@CommunityId", ServiceId);
                command.Parameters.AddWithValue("@AgencyID", AgencyId);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                Connection.Close();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        REF refModel = new REF();
                        refModel.CommunityId = Convert.ToInt32(dr["CommunityId"].ToString());
                        refModel.OrganizationName = dr["OrganizationName"].ToString();
                        refModels.Add(refModel);

                    }
                    organizationList = refModels
                             .Select(x => new SelectListItem
                             {
                                 Text = x.OrganizationName,
                                 Value = x.CommunityId.ToString()
                             }).ToList();
                }

                return organizationList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> GetReferralType(int communityId, string agencyId)
        {
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            DataSet ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "SP_GetReferralByCommunity";
            command.Parameters.AddWithValue("@AgencyId", agencyId);
            command.Parameters.AddWithValue("@CommunityId", communityId);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    selectedlist.Add(new SelectListItem
                    {
                        Text = dr["Services"].ToString(),
                        Value = dr["ServiceId"].ToString()
                    });
                }
            }
            return selectedlist;
        }


        public MatrixScore GetMatrixScoreList(long Householdid, Guid agencyId, long clientId, long programId)
        {
            MatrixScore MatrixScore = new MatrixScore();
            MatrixScore matrixScore = null;
            DataSet ds = new DataSet();
            List<MatrixScore> listMatrix = new List<MatrixScore>();
            List<ParentNames> ParentList = new List<ParentNames>();
            ParentNames Names = new ParentNames();
            try { 
            List<long> catelst = new List<long>();
            string querycommand = "SELECT";
            command.Connection = Connection;
            command.CommandText = "SP_MatrixScore";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@AgencyId", agencyId);
            command.Parameters.AddWithValue("@HouseHoldId", Householdid);
            command.Parameters.AddWithValue("@ClientId", clientId);
            command.Parameters.AddWithValue("@ProgramId", programId);
            command.Parameters.AddWithValue("@Command", querycommand);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    catelst.Add(Convert.ToInt64(dr["AssessmentCategoryId"]));
                }
                MatrixScore.CategoryIdList = catelst;
            }
            if (ds.Tables[1].Rows.Count > 0)
            {


                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    matrixScore = new MatrixScore();
                    matrixScore.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                    matrixScore.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                    matrixScore.AssessmentGroupType = dr["AssessmentGroupType"].ToString();
                    matrixScore.AnnualAssessmentType = Convert.ToInt64(dr["AnnualAssessmentType"]);
                    matrixScore.AssessmentCategory = dr["Category"].ToString();
                    matrixScore.ClassRoomId = Convert.ToInt64(dr["ClassroomID"]);
                    matrixScore.ProgramType = dr["ProgramType"].ToString();
                    matrixScore.ActiveYear = dr["ActiveProgramYear"].ToString();
                    listMatrix.Add(matrixScore);

                }
                MatrixScore.MatrixScoreList = listMatrix;

            }
            //if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in ds.Tables[2].Rows)
            //    {
            //        Names = new ParentNames();
            //        Names.ParentID = Convert.ToInt32(dr["parentid"]);
            //        Names.ParentName = dr["ParentName"].ToString();
            //        Names.ParentInvolved = DBNull.Value==dr["ParentInvolved"]?0:Convert.ToInt32(dr["ParentInvolved"]);
            //        ParentList.Add(Names);
            //    }
            //    MatrixScore.ParentList = ParentList;
            //}
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

            return MatrixScore;



        }

        public MatrixScore GetClientDetails(out List<ShowRecommendations> RecList, long houseHoldId, Guid agencyId)
        {
            MatrixScore score = new MatrixScore();
            DataSet ds = null;
            List<SelectListItem> activeYearList = new List<SelectListItem>();
            RecList = new List<ShowRecommendations>();
            List<ShowRecommendations> recommList = new List<ShowRecommendations>();
            ShowRecommendations rec = null;
            try
            {


                ds = new DataSet();
                string queryCommand = "CLIENTSTATUS";
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AgencyId", agencyId);
                command.Parameters.AddWithValue("@HouseHoldId", houseHoldId);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        score.ActiveYear = dr["ActiveProgramYear"].ToString();

                        score.ProfilePic = dr["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePic"]);
                        //score.HouseHoldId = Convert.ToInt64(dr["HouseHoldId"]);
                        score.ProgramType = dr["ProgramType"].ToString();
                        score.ParentName = dr["ParentName"].ToString().ToUpper();
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["ActiveProgramYear"].ToString()))
                        {
                            activeYearList.Add(new SelectListItem
                            {
                                Text = dr["ActiveProgramYear"].ToString()
                            });
                        }
                    }
                    score.ActiveYearList = activeYearList;
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        rec = new ShowRecommendations();
                        rec.AgencyId = new Guid(dr["AgencyId"].ToString());
                        rec.AssessmentNumber = Convert.ToInt64(dr["AssessmentNumber"]);
                        rec.ShowPopup = Convert.ToBoolean(dr["ShowPopUp"]);
                        RecList.Add(rec);
                    }

                    score.ActiveYearList = activeYearList;
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        rec = new ShowRecommendations();
                        rec.AgencyId = new Guid(dr["AgencyId"].ToString());
                        rec.AssessmentNumber = Convert.ToInt64(dr["assessmentNumber"]);
                        rec.ShowPopup = Convert.ToBoolean(dr["ShowRecPopUP"]);
                        recommList.Add(rec);
                    }
                }
                if (recommList.Count > 0)
                {
                    foreach (var item in RecList)
                    {
                        bool showPopup = recommList.Where(x => x.AssessmentNumber == item.AssessmentNumber).Select(x => x.ShowPopup).SingleOrDefault();
                        if (item.ShowPopup)
                        {

                            item.ShowPopup = showPopup;


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
            return score; ;
        }


        public ArrayList GetRecommendations(long householdId, long AssessmentNo, Guid AgencyId, string activeProgamYear)
        {
            MatrixRecommendations recommendation = null;
            DataSet ds = null;
            List<MatrixRecommendations> recommendationList = new List<MatrixRecommendations>();
            ArrayList arraylist = new ArrayList();
            try
            {


                command.Connection = Connection;
                command.CommandText = "USP_GetMatrixRecommendations";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@HouseHoldId", householdId);
                command.Parameters.AddWithValue("@AnnualAssessmentType", AssessmentNo);
                command.Parameters.AddWithValue("@ProgramTypeYear", activeProgamYear);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        recommendation = new MatrixRecommendations();
                        recommendation.FPASuggested = Convert.ToBoolean(dr["FPASuggested"]);
                        recommendation.ReferralSuggested = Convert.ToBoolean(dr["ReferralSuggested"]);
                        recommendation.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                        recommendation.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                        recommendation.AssessmentGroupType = dr["AssessmentGroupType"].ToString();
                        recommendation.AssessmentNumber = Convert.ToInt64(dr["AssessmentNumber"]);
                        recommendation.Category = dr["Category"].ToString();
                        recommendation.CategoryPosition = Convert.ToInt64(dr["CategoryPosition"]);
                        recommendation.TestValue = Convert.ToInt64(dr["TestValue"]);
                        recommendation.Description = dr["Description"].ToString();
                        recommendationList.Add(recommendation);
                    }
                }

                var list2 = recommendationList.OrderBy(x => x.CategoryPosition).GroupBy(x => x.CategoryPosition).ToList();
                arraylist.AddRange(list2);
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
            return arraylist;
        }

        public List<AssessmentResults> GetDescription(int groupId, long clientId, Guid agencyid)
        {
            List<AssessmentResults> resultList = new List<AssessmentResults>();
            AssessmentResults results = null;
            try
            {
                string queryCommand = "GETDESCRIPTION";
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AssessmentGroupId", groupId);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Parameters.AddWithValue("@AgencyId", agencyid);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        results = new AssessmentResults();
                        results.AssessmentResultId = Convert.ToInt64(dr["AssessmentResultId"]);
                        results.Description = dr["Description"].ToString();
                        results.MatrixValue = Convert.ToInt64(dr["MatrixValue"]);
                        // results.MatrixId = (dr["AssessmentNumber"].ToString() == "") ? 0 : Convert.ToInt32(dr["AssessmentNumber"]);
                        resultList.Add(results);
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
            return resultList;
        }


        public QuestionsModel GetQuestions(long groupId, long clientId)
        {
            QuestionsModel assessmentQuestions = null;
            try
            {
                string queryCommand = "GETQUESTIONS";
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AssessmentGroupId", groupId);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        assessmentQuestions = new QuestionsModel();
                        assessmentQuestions.AssessmentQuestionId = Convert.ToInt64(dr["AssessmentQuestionId"]);
                        assessmentQuestions.AssessmentQuestion = dr["AssessmentQuestion"].ToString();
                        assessmentQuestions.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
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
            return assessmentQuestions;
        }


        public int InsertMatrixScore(MatrixScore matrixScore, out bool isShow, out ArrayList arraylist)
        {
            int rowaffected = 0;
            isShow = false;
            //string queryCommand = matrixScore.MatrixScoreId == 0 ? "INSERT" : "UPDATE";
            string queryCommand = "INSERT";
            DataSet ds = new DataSet();
            MatrixRecommendations recommendation = null;
            //List<IGrouping<long, MatrixRecommendations>> List2 = new List<IGrouping<long, MatrixRecommendations>>();
            arraylist = new ArrayList();
            List<MatrixRecommendations> recommendationList = new List<MatrixRecommendations>();
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", matrixScore.AgencyId));
                command.Parameters.Add(new SqlParameter("@ProgramTypeYear", matrixScore.ActiveYear));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", matrixScore.Dec_HouseHoldId));
                command.Parameters.Add(new SqlParameter("@AnnualAssessmentType", matrixScore.AnnualAssessmentType));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", matrixScore.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@TestValue", matrixScore.Testvalue));
                command.Parameters.Add(new SqlParameter("@CenterId", matrixScore.Dec_CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", matrixScore.ClassRoomId));
                command.Parameters.Add(new SqlParameter("@ProgramType", matrixScore.ProgramType));
                command.Parameters.Add(new SqlParameter("@ClientId", matrixScore.Dec_ClientId));
                command.Parameters.Add(new SqlParameter("@UserId", matrixScore.UserId));
                command.Parameters.Add(new SqlParameter("@ProgramId", matrixScore.Dec_ProgramId));
                command.Parameters.Add(new SqlParameter("@MatrixScoreId", matrixScore.MatrixScoreId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_MatrixScore";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rowaffected = Convert.ToInt32(dr["LastInsertedRecord"]);
                        isShow = Convert.ToBoolean(dr["ShowPopup"]);
                        bool isCleared = Convert.ToBoolean(dr["ShowRec"]);
                        if (isShow)
                        {
                            isShow = isCleared;
                        }
                    }
                }

                if (isShow)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            recommendation = new MatrixRecommendations();
                            recommendation.FPASuggested = Convert.ToBoolean(dr["FPASuggested"]);
                            recommendation.ReferralSuggested = Convert.ToBoolean(dr["ReferralSuggested"]);
                            recommendation.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                            recommendation.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                            recommendation.AssessmentGroupType = dr["AssessmentGroupType"].ToString();
                            recommendation.AssessmentNumber = Convert.ToInt64(dr["AssessmentNumber"]);
                            recommendation.Category = dr["Category"].ToString();
                            recommendation.CategoryPosition = Convert.ToInt64(dr["CategoryPosition"]);
                            recommendation.TestValue = Convert.ToInt64(dr["TestValue"]);
                            recommendation.Description = dr["Description"].ToString();
                            recommendationList.Add(recommendation);
                        }
                    }

                    var list2 = recommendationList.OrderBy(x => x.CategoryPosition).GroupBy(x => x.CategoryPosition).ToList();
                    arraylist.AddRange(list2);

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
            return rowaffected;
        }

        public List<MatrixScore> GetChartDetails(out AnnualAssessment assessment, out List<ChartDetails> chartlist, Guid? AgencyId, Guid? UserID, long houseHoldId, string date, long clientId)
        {
            MatrixScore score = null;
            List<MatrixScore> listMatrixScore = new List<MatrixScore>();
            AnnualAssessment assess = new AnnualAssessment();
            ChartDetails chartdetails = null;
            List<ChartDetails> chartdetailslist = new List<ChartDetails>();
            chartlist = null;
            assessment = null;
            try
            {
                string queryCommand = (string.IsNullOrEmpty(date)) ? "GETCHARTDETAILS" : "SETCHARTDROPDOWN";
                //  string queryCommand = "GETCHARTDETAILS";
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@UserId", UserID);
                command.Parameters.AddWithValue("@HouseHoldId", houseHoldId);
                command.Parameters.AddWithValue("@ProgramTypeYear", date);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        score = new MatrixScore();
                        score.MatrixScoreId = Convert.ToInt64(dr["MatrixScoreId"]);
                        score.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                        score.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                        score.Testvalue = Convert.ToInt64(dr["TestValue"]);
                        score.AnnualAssessmentType = Convert.ToInt64(dr["AssessmentNumber"]);
                        listMatrixScore.Add(score);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        assess.AnnualAssessmentId = Convert.ToInt64(dr["AnnualAssessmentId"]);
                        assess.AnnualAssessmentType = Convert.ToInt64(dr["AnnualAssessmentType"]);
                        assess.Assessment1From = dr["Assessment1FromDate"].ToString();
                        assess.Assessment1To = dr["Assessment1ToDate"].ToString();
                        assess.Assessment2From = dr["Assessment2FromDate"].ToString();
                        assess.Assessment2To = dr["Assessment2ToDate"].ToString();
                        assess.Assessment3From = dr["Assessment3FromDate"].ToString();
                        assess.Assessment3To = dr["Assessment3ToDate"].ToString();
                        assessment = assess;
                    }
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        chartdetails = new ChartDetails();
                        chartdetails.TestValueSum = Convert.ToInt64(dr["TestTotalValue"]);
                        chartdetails.AssessmentNumber = Convert.ToInt64(dr["AssessmentNumber"]);
                        chartdetails.AssessementCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                        chartdetails.ResultPercentage = Convert.ToDouble(dr["Percentage"]);
                        chartdetails.GroupIdCount = Convert.ToInt64(dr["GroupCount"]);
                        chartdetails.ChartHeight = (dr["ChartHeight"].ToString() == "") ? 0 : Convert.ToDouble(dr["ChartHeight"]);
                        chartdetailslist.Add(chartdetails);
                    }
                    chartlist = chartdetailslist;
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
            return listMatrixScore;
        }
        public List<MatrixScore> SetChart(Guid? AgencyId)
        {
            MatrixScore score = null;
            List<MatrixScore> listMatrixScore = new List<MatrixScore>();
            try
            {
                string queryCommand = "SETCHART";
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        score = new MatrixScore();
                        score.Testvalue = Convert.ToInt64(dr["TestValue"]);
                        score.AssessmentNumber = Convert.ToInt64(dr["AssessmentNumber"]);
                        score.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                        score.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                        listMatrixScore.Add(score);
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
            return listMatrixScore;

        }

        public List<MatrixScore> GetName(MatrixScore matrixScore)
        {
            List<MatrixScore> MatrixscoreList = new List<MatrixScore>();
            try
            {
                DataSet ds = null;

                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        string queryCommand = (string.IsNullOrEmpty(matrixScore.ActiveYear)) ? "GETNAME" : "GETNAMEBYSELECT";
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "SP_MatrixScore";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AgencyId", matrixScore.AgencyId);
                        command.Parameters.AddWithValue("@ProgramTypeYear", matrixScore.ActiveYear);
                        command.Parameters.AddWithValue("@HouseHoldId", matrixScore.Dec_HouseHoldId);
                        command.Parameters.AddWithValue("@UserId", matrixScore.UserId);
                        command.Parameters.AddWithValue("@Command", queryCommand);
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
                            MatrixScore obj = new MatrixScore();
                            obj.StaffName = dr["StaffName"].ToString();
                            obj.AssessmentNumber = Convert.ToInt64(dr["AssessmentNumber"]);
                            obj.Date = Convert.ToDateTime(dr["Date"]).ToString("MM/dd/yyyy");
                            MatrixscoreList.Add(obj);
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
            return MatrixscoreList;
        }



        public bool InsertMatrixRecommendationData(MatrixRecommendations matrixRecomm)
        {


            int rowsAffected = 0;
            DataSet ds = new DataSet();
            bool isResult = false;

            List<MatrixRecommendations> recommendationList = new List<MatrixRecommendations>();
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", matrixRecomm.AgencyId));
                command.Parameters.Add(new SqlParameter("@ProgramTypeYear", matrixRecomm.ActiveProgramYear));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", matrixRecomm.Dec_HouseHoldId));
                command.Parameters.Add(new SqlParameter("@AnnualAssessmentType", matrixRecomm.AssessmentNumber));
                command.Parameters.Add(new SqlParameter("@ClientId", matrixRecomm.Dec_ClientId));
                command.Parameters.Add(new SqlParameter("@UserId", matrixRecomm.UserId));
                //command.Parameters.Add(new SqlParameter("@MatrixRecommendationId", matrixRecomm.MatrixRecommendationId));
                command.Parameters.Add(new SqlParameter("@Status", matrixRecomm.Status));
                command.CommandText = "USP_InsertMatrixRecommendations";
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
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return isResult;

        }
        public void GetDomainDetails(ref DataTable dtDomainDetails)
        {
            dtDomainDetails = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetDomianDetails";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtDomainDetails);
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

        public bool SaveObservatioNotes(ObservationNotes objNotes)
        {
            bool isInserted = false;
            try
            {
                Guid? NoteID = null;
                DateTime dt = Convert.ToDateTime(objNotes.Date);
                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertObservationNoteDetail";
                command.Parameters.AddWithValue("@Date", dt);
                if (!string.IsNullOrEmpty(objNotes.NoteId))
                    command.Parameters.AddWithValue("@NoteId", objNotes.NoteId);
                command.Parameters.AddWithValue("@Title", objNotes.Title);
                command.Parameters.AddWithValue("@Notes", objNotes.Note);
                command.Parameters.AddWithValue("@DomainId", objNotes.DomainId);
                command.Parameters.AddWithValue("@ElementId", objNotes.ElementId);
                command.Parameters.AddWithValue("@UserId", objNotes.UserId);
                Object Note = command.ExecuteScalar();
                if (Note != null)
                {
                    NoteID = new Guid(Note.ToString());
                    SaveChildNoteIntersection(objNotes, NoteID);
                    SaveChildNoteAttachment(objNotes, NoteID);
                    isInserted = true;
                }
                SaveDomainObservationResults(objNotes);
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

        public void SaveChildNoteIntersection(ObservationNotes objNotes, Guid? NoteId)
        {
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                foreach (var ClientId in objNotes.lstClientid)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_InsertChildNoteIntersection";
                    command.Parameters.AddWithValue("@ClientId", Convert.ToInt64(ClientId));
                    command.Parameters.AddWithValue("@NoteId", NoteId);
                    command.Parameters.AddWithValue("@UserId", objNotes.UserId);
                    int RowsAffected = command.ExecuteNonQuery();
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


        public void SaveChildNoteAttachment(ObservationNotes objNotes, Guid? NoteId)
        {
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                foreach (var path in objNotes.AttachmentPath)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_InsertObservationNoteAttachment";
                    command.Parameters.AddWithValue("@NoteId", NoteId);
                    command.Parameters.AddWithValue("@UserId", objNotes.UserId);
                    command.Parameters.AddWithValue("@Path", path.ToString());
                    int RowsAffected = command.ExecuteNonQuery();
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

        public void SaveDomainObservationResults(ObservationNotes objNotes)
        {
            try
            {
                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertDomainObservationResults";
                command.Parameters.AddWithValue("@UserId", objNotes.UserId);
                int RowsAffected = command.ExecuteNonQuery();
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

        public void GetNotesDetialByNoteId(ref ObservationNotes objNotes, string NoteId)
        {
            objNotes = new ObservationNotes();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@NoteId", NoteId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetNotesDetailByNoteId";
                _dataset = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                objNotes.dictClientDetails = new Dictionary<long, string>();
                if (_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        objNotes.NoteId = !string.IsNullOrEmpty(dr["NoteId"].ToString()) ? dr["NoteId"].ToString() : "";
                        objNotes.Date = !string.IsNullOrEmpty(dr["Date"].ToString()) ? dr["Date"].ToString() : "";
                        objNotes.Title = !string.IsNullOrEmpty(dr["Title"].ToString()) ? dr["Title"].ToString() : "";
                        objNotes.Note = !string.IsNullOrEmpty(dr["Note"].ToString()) ? dr["Note"].ToString() : "";
                        objNotes.DomainId = !string.IsNullOrEmpty(dr["DomainId"].ToString()) ? Convert.ToInt64(dr["DomainId"].ToString()) : 0;
                        objNotes.ElementId = !string.IsNullOrEmpty(dr["ElementId"].ToString()) ? Convert.ToInt64(dr["ElementId"].ToString()) : 0;
                    }
                }
                if (_dataset.Tables[1].Rows.Count > 0)
                {
                    objNotes.AttachmentPath = new string[_dataset.Tables[1].Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        objNotes.AttachmentPath[i] = !string.IsNullOrEmpty(dr["Attachment"].ToString()) ? dr["Attachment"].ToString() : "";
                        i++;
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

        public void GetChildlistByUserId(ref ObservationNotes objNotes, string UserId, string AgencyId)
        {
            objNotes = new ObservationNotes();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetChildlistByUserId";
                _dataset = new DataSet();
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                objNotes.dictClientDetails = new Dictionary<long, string>();
                if (_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        Int64 ClientId = !string.IsNullOrEmpty(dr["ClientID"].ToString()) ? Convert.ToInt64(dr["ClientID"].ToString()) : 0;
                        string ClientName = !string.IsNullOrEmpty(dr["Name"].ToString()) ? dr["Name"].ToString() : "";
                        objNotes.dictClientDetails.Add(ClientId, ClientName);
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
        public void GetElementDetailsByDomainId(ref DataTable dtElements, string DomainId)
        {
            dtElements = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@DomainId", Convert.ToInt64(DomainId)));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetElementDetailsByDomainId";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtElements);
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


        public SelectListItem GetChildrenImageData(long ClientId)
        {
            SelectListItem child = new SelectListItem();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetChildrenImage";
                command.Parameters.AddWithValue("@ClientId", ClientId);
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            child.Text = string.IsNullOrEmpty(dr["profilepic"].ToString()) ? "" : Convert.ToBase64String((byte[])dr["profilepic"]);
                            child.Value = dr["gender"].ToString();
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
            return child;
        }

        public AttendenceDetailsByDate GetAttendenceDetailsByDate(string selectedDate, long clientId, string agencyId)
        {
            AttendenceDetailsByDate attendence = null;
            try
            {
                //string queryCommand = "GetAttendenceDetailsByDate";
                DataSet ds = new DataSet();
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                DateTime date = DateTime.ParseExact(selectedDate, "MM/dd/yyyy", culture);
                attendence = new AttendenceDetailsByDate();
                command.Connection = Connection;
                command.CommandText = "SP_GetAttendenceDetailsByDate";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AttendanceDate", date);
                command.Parameters.AddWithValue("@AgencyId", agencyId);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        
                        attendence.AttendenceDate = Convert.ToDateTime(dr["AttendanceDate"]);
                        attendence.AttendenceStatus = dr["AttendenceType"].ToString();
                        attendence.Center = Convert.ToString(dr["CenterName"]);
                        attendence.Class = Convert.ToString(dr["FirstName"]);

                        attendence.Snacks = (dr["Snacks"] != DBNull.Value) ? Convert.ToInt32(dr["Snacks"]) : 0;
                        attendence.Breakfast = (dr["breakfast"] != DBNull.Value) ? Convert.ToInt32(dr["breakfast"]) : 0;
                        attendence.Lunch = (dr["Lunch"] != DBNull.Value) ? Convert.ToInt32(dr["Lunch"]) : 0;

                        attendence.ClientName = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                        attendence.ParentName = dr["ParentName"].ToString();
                        attendence.SignedInBy = Convert.ToString(dr["signedInBy"]);
                        attendence.SignedOutBy = Convert.ToString(dr["signedOutBy"]);
                        attendence.StaffName = Convert.ToString(dr["TeacherSignature"]);
                        attendence.SignedInTime = Convert.ToString(dr["TimeIn"]);
                        attendence.SignedOutTime = Convert.ToString(dr["Timeout"]);

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
            return attendence;
        }
		
		 public bool SaveProgramInformationReport(int ProgramTypeId, string ProgramType, bool FamilyAssessment, bool FamilyGoalSetting, bool ChildHS, bool PolicyCouncil, bool WorkShops, string ActiveProgramYear, string AgencyId,int EventRegCount=0)
        {
            bool isInserted = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@ProgramTypeID", ProgramTypeId));
                command.Parameters.Add(new SqlParameter("@ProgramType", ProgramType));
                command.Parameters.Add(new SqlParameter("@FamilyAssessment", FamilyAssessment));

                command.Parameters.Add(new SqlParameter("@FamilyGoalSetting", FamilyGoalSetting));
                command.Parameters.Add(new SqlParameter("@ChildHS", ChildHS));
                command.Parameters.Add(new SqlParameter("@PolicyCouncil", PolicyCouncil));

                command.Parameters.Add(new SqlParameter("@WorkShops", WorkShops));
                command.Parameters.Add(new SqlParameter("@ActiveProgramYear", "16-17"));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));

                command.Parameters.Add(new SqlParameter("@EventRegCount", EventRegCount));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertFatherHood";
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
                command.Dispose();
            }
            return isInserted;
        }


        public Roster GetPregnantMomList(out string totalrecord, string sortOrder, string sortDirection, string Center, string Classroom, int skip, int pageSize, string userid, string agencyid)
        {
            Roster _roster = new Roster();
            List<Roster> RosterList = new List<Roster>();
            List<HrCenterInfo> centerList = new List<HrCenterInfo>();
            try
            {
                totalrecord = string.Empty;
                command.Parameters.Add(new SqlParameter("@Center", Center));
                command.Parameters.Add(new SqlParameter("@Classroom", Classroom));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetPregnantMomList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Roster info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            info = new Roster();
                            info.Householid = dr["Householdid"].ToString();

                            info.Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString());
                            info.EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString());
                            info.Name = dr["name"].ToString();
                            info.Gender = dr["gender"].ToString();
                            // info.Picture = dr["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePic"]);
                            info.CenterName = dr["CenterName"].ToString();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString());
                            info.ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString());
                            info.RosterYakkr = dr["Yakkr"].ToString();
                            info.ClassroomName = dr["ClassroomName"].ToString();
                            info.IsPresent = DBNull.Value.Equals(dr["IsPresent"]) ? 0 : Convert.ToInt32(dr["IsPresent"]);//.ToString() //Added on 30Dec2016
                            info.Acronym = dr["AcronymName"].ToString();
                            RosterList.Add(info);
                        }
                        _roster.Rosters = RosterList;
                    }
                }
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    HrCenterInfo info = null;
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        info = new HrCenterInfo();
                        info.CenterId = dr["center"].ToString();
                        info.Name = dr["centername"].ToString();
                        centerList.Add(info);
                    }
                    _roster.Centers = centerList;
                }
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);

            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();

            }
            return _roster;

        }
        public List<SeatAvailability> SaveChildHeadStartTranstion(TransitionDetails Transition, string AgencyId, string UserId, string RoleId)
        {
            List<SeatAvailability> AvailabilityList = new List<SeatAvailability>();
            try
            {

           

                switch (Transition.Transition.TrnsInsuranceType)
                {
                    case "C3A1":

                        Transition.Transition.MedicaidCHIP_S_C3A1 = 1;
                        Transition.Transition.MedicaidCHIP_E_C3A2 = 1;
                        Transition.Transition.InsuranceType = 1;
                        break;
                    case "C3B1":
                        Transition.Transition.StateFunded_S_C3B1 = 1;
                        Transition.Transition.StateFunded_E_C3B2 = 1;
                        Transition.Transition.InsuranceType = 5;
                        break;
                    case "C3C1":
                        Transition.Transition.PrivateIns_S_C3C1 = 1;
                        Transition.Transition.PrivateIns_E_C3C2 = 1;
                        Transition.Transition.InsuranceType = 4;
                        break;
                    case "C3D1":
                        Transition.Transition.OtherIns_S_C3D1 = 1;
                        Transition.Transition.OtherIns_E_C3D2 = 1;
                        Transition.Transition.InsuranceType = 3;
                        Transition.Transition.Description_S_C3D11 = Transition.Transition.OtherInsuranceTypeDesc;
                        Transition.Transition.Description_E_C3D11 = Transition.Transition.OtherInsuranceTypeDesc;
                        break;
                    case "C4":
                        Transition.Transition.NoIns_S_C4 = 1;
                        Transition.Transition.NoIns_E_C4 = 1;
                        Transition.Transition.InsuranceType = 2;
                        break;

                }

                switch (Transition.Transition.ChildInsuranceType)
                {
                    case "C3A1":

                        Transition.Transition.MedicaidCHIP_S_C1A1 = 1;
                        Transition.Transition.MedicaidCHIP_E_C1A2 = 1;
                        break;
                    case "C3B1":
                        Transition.Transition.StateFunded_S_C1B1 = 1;
                        Transition.Transition.StateFunded_E_C1B2 = 1;
                        break;
                    case "C3C1":
                        Transition.Transition.PrivateIns_S_C1C1 = 1;
                        Transition.Transition.PrivateIns_E_C1C2 = 1;
                        break;
                    case "C3D1":
                        Transition.Transition.OtherIns_S_C1D1 = 1;
                        Transition.Transition.OtherIns_E_C1D2 = 1;
                        Transition.Transition.Description_S_C1D11 = Transition.Transition.ChildOtherInsuranceTypeDesc;
                        Transition.Transition.Description_E_C1D11 = Transition.Transition.ChildOtherInsuranceTypeDesc;

                        break;
                    case "C4":
                        Transition.Transition.NoIns_S_C2 = 1;
                        Transition.Transition.NoIns_E_C2 = 1;
                        break;
                }


                int i = 1;
                foreach (PregMomChilds objChild in Transition.PregMomChilds)
                {
                    string dateOfBirth = "";
                    SeatAvailability seats = new SeatAvailability();
                    Connection.Open();
                    command.Connection = Connection;

                    command.CommandText = "USP_AddPregnantMomEHS";
                    command.Parameters.Clear();

                    command.CommandType = CommandType.StoredProcedure;

                    if(objChild.DOB!=null)
                     dateOfBirth = Convert.ToDateTime(objChild.DOB).ToString("yyyy-MM-dd");

                    command.Parameters.Add(new SqlParameter("@ClientId", Transition.Transition.ClientId));
                    command.Parameters.Add(new SqlParameter("@ProgramTypeId", Transition.Transition.ProgramTypeId));
                    command.Parameters.Add(new SqlParameter("@DateOfTransition", Transition.Transition.DateOfTransition));
                    command.Parameters.Add(new SqlParameter("@ParentInsuranceType", Transition.Transition.InsuranceType));
                    command.Parameters.Add(new SqlParameter("@BirthType", Transition.Transition.BirthType));
                    command.Parameters.Add(new SqlParameter("@InsuranceType", Transition.Transition.InsuranceType));

                    command.Parameters.Add(new SqlParameter("@Name", objChild.Name));
                    command.Parameters.Add(new SqlParameter("@DOB", dateOfBirth));
                    command.Parameters.Add(new SqlParameter("@Gender", objChild.Gender));
                    //command.Parameters.Add(new SqlParameter("@EnrollmentStatus", objChild.EnrollmentStatus));
                    command.Parameters.Add(new SqlParameter("@IsEHS", objChild.IsEHS));
                    command.Parameters.Add(new SqlParameter("@IsFirstChild", i == 1 ? true : false));
                    command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
                    //parameters for insurance type
                    command.Parameters.Add(new SqlParameter("@ActiveRecCode", Transition.Transition.ChildInsuranceType));

                    command.Parameters.Add(new SqlParameter("@MedicaidCHIP_S_C1A1", Transition.Transition.MedicaidCHIP_S_C1A1));
                    command.Parameters.Add(new SqlParameter("@MedicaidCHIP_E_C1A2", Transition.Transition.MedicaidCHIP_E_C1A2));
                    command.Parameters.Add(new SqlParameter("@StateFunded_S_C1B1", Transition.Transition.StateFunded_S_C1B1));
                    command.Parameters.Add(new SqlParameter("@StateFunded_E_C1B2", Transition.Transition.StateFunded_E_C1B2));
                    command.Parameters.Add(new SqlParameter("@PrivateIns_S_C1C1", Transition.Transition.PrivateIns_S_C1C1));
                    command.Parameters.Add(new SqlParameter("@PrivateIns_E_C1C2", Transition.Transition.PrivateIns_E_C1C2));
                    command.Parameters.Add(new SqlParameter("@OtherIns_S_C1D1", Transition.Transition.OtherIns_S_C1D1));
                    command.Parameters.Add(new SqlParameter("@OtherIns_E_C1D2", Transition.Transition.OtherIns_E_C1D2));

                    command.Parameters.Add(new SqlParameter("@Description_S_C1D1", Transition.Transition.Description_S_C1D11));
                    command.Parameters.Add(new SqlParameter("@Description_E_C1D1", Transition.Transition.Description_S_C1D11));
                    command.Parameters.Add(new SqlParameter("@NoIns_S_C2", Transition.Transition.NoIns_S_C2));
                    command.Parameters.Add(new SqlParameter("@NoIns_E_C2", Transition.Transition.NoIns_E_C2));

                   
                    if(i==1)
                    {
                        command.Parameters.Add(new SqlParameter("@PregActiveRecCode", Transition.Transition.TrnsInsuranceType));
                        command.Parameters.Add(new SqlParameter("@MedicaidCHIP_S_C3A1", Transition.Transition.MedicaidCHIP_S_C3A1));
                        command.Parameters.Add(new SqlParameter("@MedicaidCHIP_E_C3A2", Transition.Transition.MedicaidCHIP_E_C3A2));
                        command.Parameters.Add(new SqlParameter("@StateFunded_S_C3B1", Transition.Transition.StateFunded_S_C3B1));
                        command.Parameters.Add(new SqlParameter("@StateFunded_E_C3B2", Transition.Transition.StateFunded_E_C3B2));
                        command.Parameters.Add(new SqlParameter("@PrivateIns_S_C3C1", Transition.Transition.PrivateIns_S_C3C1));
                        command.Parameters.Add(new SqlParameter("@PrivateIns_E_C3C2", Transition.Transition.PrivateIns_E_C3C2));
                        command.Parameters.Add(new SqlParameter("@OtherIns_S_C3D1", Transition.Transition.OtherIns_S_C3D1));
                        command.Parameters.Add(new SqlParameter("@OtherIns_E_C3D2", Transition.Transition.OtherIns_E_C3D2));

                        command.Parameters.Add(new SqlParameter("@Description_S_C3D1", Transition.Transition.Description_S_C3D11));
                        command.Parameters.Add(new SqlParameter("@Description_E_C3D1", Transition.Transition.Description_S_C3D11));
                        command.Parameters.Add(new SqlParameter("@NoIns_S_C4", Transition.Transition.NoIns_S_C4));
                        command.Parameters.Add(new SqlParameter("@NoIns_E_C4", Transition.Transition.NoIns_E_C4));

                    }

                    command.Parameters.Add(new SqlParameter("@Result", string.Empty));
                    command.Parameters.Add(new SqlParameter("@SlotCount", string.Empty));
                    command.Parameters.Add(new SqlParameter("@SeatCount", string.Empty));
                    command.Parameters["@Result"].Direction = ParameterDirection.Output;
                    command.Parameters["@SlotCount"].Direction = ParameterDirection.Output;
                    command.Parameters["@SeatCount"].Direction = ParameterDirection.Output;

                    int RowsAffected = command.ExecuteNonQuery();
                    seats.Result = Convert.ToInt32((DBNull.Value == command.Parameters["@Result"].Value) ? 0 : command.Parameters["@Result"].Value);
                    seats.SeatAvailable = Convert.ToInt32((DBNull.Value == command.Parameters["@SeatCount"].Value) ? 0: command.Parameters["@SeatCount"].Value);
                    seats.SloatAvailable = Convert.ToInt32((DBNull.Value == command.Parameters["@SlotCount"].Value) ? 0 : command.Parameters["@SlotCount"].Value);
                    seats.ChildName = objChild.Name;
                    AvailabilityList.Add(seats);                                       
                    i++;

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
                command.Dispose();
            }
            return AvailabilityList;
        }

        public Roster GetCenterList(string userid, string agencyid)
        {
            Roster _roster = new Roster();
            List<HrCenterInfo> centerList = new List<HrCenterInfo>();
            try
            {
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetAllCenterByAgencyID";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    HrCenterInfo info = null;
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        info = new HrCenterInfo();
                        info.CenterId = dr["center"].ToString();
                        info.Name = dr["centername"].ToString();
                        centerList.Add(info);
                    }
                    _roster.Centers = centerList;
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
            return _roster;

        }

        public bool SaveHeadStartTranstion(Transition Transition, string AgencyId,string UserId)
        {
            bool isInserted = false;
            int resultValue = 0;
            try
            {
                switch (Transition.TrnsInsuranceType)
                {
                    case "C3A1":

                        Transition.MedicaidCHIP_S_C1A1 = 1;
                        Transition.MedicaidCHIP_E_C1A2 = 1;
                        break;
                    case "C3B1":
                        Transition.StateFunded_S_C1B1 = 1;
                        Transition.StateFunded_E_C1B2 = 1;
                        break;
                    case "C3C1":
                        Transition.PrivateIns_S_C1C1 = 1;
                        Transition.PrivateIns_E_C1C2 = 1;
                        break;
                    case "C3D1":
                        Transition.OtherIns_S_C1D1 = 1;
                        Transition.OtherIns_E_C1D2 = 1;
                        Transition.Description_S_C1D11 = Transition.OtherInsuranceTypeDesc;
                        Transition.Description_E_C1D11 = Transition.OtherInsuranceTypeDesc;

                        break;
                    case "C4":
                        Transition.NoIns_S_C2 = 1;
                        Transition.NoIns_E_C2 = 1;
                        break;
                }

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();

                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@ClientId", Transition.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));

                command.Parameters.Add(new SqlParameter("@DateOfTransition", Transition.DateOfTransition));

                command.Parameters.Add(new SqlParameter("@TransitioningType1", Transition.TransitioningType == 1 ? 1 : 0));
                command.Parameters.Add(new SqlParameter("@TransitioningType2", Transition.TransitioningType == 2 ? 1 : 0));
                command.Parameters.Add(new SqlParameter("@TransitioningType3", Transition.TransitioningType == 3 ? 1 : 0));
                command.Parameters.Add(new SqlParameter("@TransitioningType4", Transition.TransitioningType == 4 ? 1 : 0));

                command.Parameters.Add(new SqlParameter("@CenterId", Transition.CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", Transition.ClassRoomId));


                command.Parameters.Add(new SqlParameter("@ActiveRecCode", Transition.TrnsInsuranceType));

                command.Parameters.Add(new SqlParameter("@MedicaidCHIP_S_C1A1", Transition.MedicaidCHIP_S_C1A1));
                command.Parameters.Add(new SqlParameter("@MedicaidCHIP_E_C1A2", Transition.MedicaidCHIP_E_C1A2));
                command.Parameters.Add(new SqlParameter("@StateFunded_S_C1B1", Transition.StateFunded_S_C1B1));
                command.Parameters.Add(new SqlParameter("@StateFunded_E_C1B2", Transition.StateFunded_E_C1B2));
                command.Parameters.Add(new SqlParameter("@PrivateIns_S_C1C1", Transition.PrivateIns_S_C1C1));
                command.Parameters.Add(new SqlParameter("@PrivateIns_E_C1C2", Transition.PrivateIns_E_C1C2));
                command.Parameters.Add(new SqlParameter("@OtherIns_S_C1D1", Transition.OtherIns_S_C1D1));
                command.Parameters.Add(new SqlParameter("@OtherIns_E_C1D2", Transition.OtherIns_E_C1D2));

                command.Parameters.Add(new SqlParameter("@Description_S_C1D1", Transition.Description_S_C1D11));
                command.Parameters.Add(new SqlParameter("@Description_E_C1D1", Transition.Description_S_C1D11));
                command.Parameters.Add(new SqlParameter("@NoIns_S_C2", Transition.NoIns_S_C2));
                command.Parameters.Add(new SqlParameter("@NoIns_E_C2", Transition.NoIns_E_C2));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertHeadStartTransition";
                
                Object Result = command.ExecuteScalar();
                if (Result != null)
                    resultValue = Convert.ToInt32(Result);

                if (resultValue > 0)
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
                command.Dispose();
            }
            return isInserted;
        }


        public string GetAvailablitySetsByClass(string CenterId,string ClassRoomId, string Agencyid,string ClientId)
        {
            string availableSeats = "0";
            try
            {
                int id= string.IsNullOrEmpty(ClientId) ? 0 : Convert.ToInt32(ClientId);
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", ClassRoomId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@ClientId",id ));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_TotalAvailabilitySeatsByAgencyId";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                   
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        availableSeats = dr["AvailableSets"].ToString() == null ? "0" : dr["AvailableSets"].ToString();
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
            return availableSeats;
        }


        public Roster GetCenterAndClassRoomsByCenter(string centerid, string Classroom, string householdid ,string ChildId,string userid, string agencyid)
        {
           Roster RosterList = new Roster();
           List<CenterAndClassRoom> CenterAndClassRoom = new List<CenterAndClassRoom>();
           List<CaseNoteDetails> CaseNoteDetails = new List<CaseNoteDetails>();
          
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", householdid));
                command.Parameters.Add(new SqlParameter("@CenterId", centerid));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", Classroom));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetCenterAndClassRoomsByCenter";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);


                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CenterAndClassRoom centerAndClassRoom = new CenterAndClassRoom();
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            centerAndClassRoom = new CenterAndClassRoom();
                            centerAndClassRoom.Name = dr["ClientName"].ToString();
                            centerAndClassRoom.Eclientid = dr["ClientID"].ToString();
                            
                            if (dr["IsChild"].ToString() == "True")
                            {
                                centerAndClassRoom.clientenrolled = "Child";
                            }
                            else if (dr["Isfamily"].ToString() == "True")
                            {
                                centerAndClassRoom.clientenrolled = "Parent/Guardian";
                            }
                            CenterAndClassRoom.Add(centerAndClassRoom);


                        }
                        RosterList.CenterAndClassRoom = CenterAndClassRoom.Where(x => x.Eclientid == ChildId || x.clientenrolled == "Parent/Guardian").ToList();
                    }

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        CaseNoteDetails caseNoteDetail = new CaseNoteDetails();
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            caseNoteDetail = new CaseNoteDetails();
                            caseNoteDetail.UserId = dr["UserId"].ToString();
                            caseNoteDetail.Name = dr["Name"].ToString();
                            CaseNoteDetails.Add(caseNoteDetail);


                        }
                        RosterList.CaseNoteDetails = CaseNoteDetails.Where(x => x.Name.Contains("(Center Manager)") || x.Name.Contains("(Teacher)")).ToList();
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
            return RosterList;

        }


        public bool SaveCenterAndClassRoom(string ClientId, string DateOfTransition,string CenterId,string ClassRoomId, string AgencyId, string UserId,int ReasonID,string NewReason)
        {
            bool isInserted = false;
            int resultValue = 0;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();

                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@ReasonID", ReasonID));
                command.Parameters.Add(new SqlParameter("@NewReason", NewReason));
                command.Parameters.Add(new SqlParameter("@DateOfTransition", DateOfTransition));


                command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", ClassRoomId));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertCenterAndClassRoom";

                Object Result = command.ExecuteScalar();
                if (Result != null)
                    resultValue = Convert.ToInt32(Result);

                if (resultValue > 0)
                    isInserted = true;

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();

                command.Parameters.Clear();
           

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
            return isInserted;
        }

        public string SaveCaseNotes1(ref string Name, ref List<CaseNote> CaseNoteList, ref FingerprintsModel.RosterNew.Users Userlist, CaseNoteNew CaseNote, List<RosterNew.Attachment> Attachments, string Agencyid, string UserID)
        {
            string result = string.Empty;

            try
            {
                string HouseholdId = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@ClientId", CaseNote.ClientId));
                command.Parameters.Add(new SqlParameter("@CenterId", CaseNote.CenterId));
                command.Parameters.Add(new SqlParameter("@CNoteid", CaseNote.CaseNoteid));
                command.Parameters.Add(new SqlParameter("@ProgramId", CaseNote.ProgramId));
                command.Parameters.Add(new SqlParameter("@HouseHoldIdnew", CaseNote.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@Note", CaseNote.Note));
                command.Parameters.Add(new SqlParameter("@CaseNoteDate", CaseNote.CaseNoteDate));
                command.Parameters.Add(new SqlParameter("@CaseNoteSecurity", CaseNote.CaseNoteSecurity));
                command.Parameters.Add(new SqlParameter("@CaseNotetags", CaseNote.CaseNotetags));
                command.Parameters.Add(new SqlParameter("@CaseNotetitle", CaseNote.CaseNotetitle));
                command.Parameters.Add(new SqlParameter("@ClientIds", CaseNote.ClientIds));
                command.Parameters.Add(new SqlParameter("@StaffIds", CaseNote.StaffIds));
                command.Parameters.Add(new SqlParameter("@userid", UserID));
                command.Parameters.Add(new SqlParameter("@agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("Attachment", typeof(byte[])),
                      new DataColumn("AttachmentName",typeof(string)),
                        new DataColumn("Attachmentextension",typeof(string))
                    });
                foreach (RosterNew.Attachment Attachment in Attachments)
                {
                    if (Attachment != null && Attachment.file != null)
                    {
                        dt.Rows.Add(new BinaryReader(Attachment.file.InputStream).ReadBytes(Attachment.file.ContentLength), Attachment.file.FileName, Path.GetExtension(Attachment.file.FileName));

                    }
                }
                command.Parameters.Add(new SqlParameter("@Attachments", dt));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveCaseNote";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CaseNote info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new CaseNote();
                            info.Householid = dr["householdid"].ToString();
                            info.CaseNoteid = dr["casenoteid"].ToString();
                            info.BY = dr["By"].ToString();
                            info.Title = dr["Title"].ToString();
                            info.Attachment = dr["Attachment"].ToString();
                            info.References = dr["References"].ToString();
                            info.Date = Convert.ToDateTime(dr["casenotedate"]).ToString("MM/dd/yyyy");
                            CaseNoteList.Add(info);
                        }
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {

                            Name = dr["Name"].ToString();

                        }
                    }

                    if (_dataset != null && _dataset.Tables[2].Rows.Count > 0)
                    {

                        List<FingerprintsModel.RosterNew.User> Clientlist = new List<FingerprintsModel.RosterNew.User>();
                        FingerprintsModel.RosterNew.User obj = null;
                        foreach (DataRow dr in _dataset.Tables[2].Rows)
                        {
                            obj = new FingerprintsModel.RosterNew.User();
                            obj.Id = dr["clientid"].ToString();
                            obj.Name = dr["Name"].ToString();
                            Clientlist.Add(obj);
                        }
                        Userlist.Clientlist = Clientlist;
                    }
                    if (_dataset != null && _dataset.Tables[3].Rows.Count > 0)
                    {
                        List<FingerprintsModel.RosterNew.User> _userlist = new List<FingerprintsModel.RosterNew.User>();
                        FingerprintsModel.RosterNew.User obj = null;
                        foreach (DataRow dr in _dataset.Tables[3].Rows)
                        {
                            obj = new FingerprintsModel.RosterNew.User();
                            obj.Id = (dr["UserId"]).ToString();
                            obj.Name = dr["Name"].ToString();
                            _userlist.Add(obj);
                        }
                        Userlist.UserList = _userlist;
                    }

                }



                result = command.Parameters["@result"].Value.ToString();

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
            return result;

        }

        public int InsertParentDetailsMatrixScore(MatrixScore matrixScore,string agencyid,string userid)
        {
            int isaffected = 0;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyid));
                command.Parameters.Add(new SqlParameter("@ProgramTypeYear", matrixScore.ActiveYear));
                command.Parameters.Add(new SqlParameter("@HouseHoldId", matrixScore.Dec_HouseHoldId));
                command.Parameters.Add(new SqlParameter("@AnnualAssessmentType", matrixScore.AnnualAssessmentType));
                command.Parameters.Add(new SqlParameter("@ProgramType", matrixScore.ProgramType));
                command.Parameters.Add(new SqlParameter("@TableID", matrixScore.Id));
                command.Parameters.Add(new SqlParameter("@ParentID", matrixScore.ParentId));
                command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64( matrixScore.@ClientId)));
                command.Parameters.Add(new SqlParameter("@UserId", userid));
                command.Parameters.Add(new SqlParameter("@ProgramId", matrixScore.Dec_ProgramId));
                command.Parameters.Add(new SqlParameter("@IsChecked", matrixScore.IsChecked));
                command.CommandText = "SP_InsertParentDetails_Assessmnt";
                // SqlDataAdapter da = new SqlDataAdapter(command);
                isaffected=command.ExecuteNonQuery();
                

               

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
            return isaffected;
        }


        public List<SelectListItem> GetCaseNoteTagsonInput(string searchText)
        {

            List<SelectListItem> tagsList = new List<SelectListItem>();
            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection)
                {

                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@SearchText", searchText));
                    command.CommandText = "USP_GetCaseNoteTagsBytext";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }

                if(_dataset!=null && _dataset.Tables[0].Rows.Count>0)
                {
                    tagsList = (from DataRow dr1 in _dataset.Tables[0].Rows
                                select new SelectListItem
                                {
                                    Text = dr1["TagName"].ToString(),
                                    Value = dr1["TagKey"].ToString()
                                }
                              ).ToList();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return tagsList;
        }


    }
}
