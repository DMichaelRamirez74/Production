using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using FingerprintsModel;
using System.Web.Mvc;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Xml;
using Newtonsoft.Json;

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
        public List<CaseNote> GetCaseNote(ref string Name, ref FingerprintsModel.RosterNew.Users Userlist, int Householdid, int centerid, string id, string AgencyId, string roleID, string UserId)
        {
            List<CaseNote> CaseNoteList = new List<CaseNote>();


            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleID", roleID));
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
                SetCaseNoteDetails(CaseNoteList, ref Name, ref Userlist, _dataset);


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

        public void SetCaseNoteDetails(List<CaseNote> CaseNoteList, ref string Name, ref FingerprintsModel.RosterNew.Users Userlist, DataSet _dataset)
        {

            if (_dataset!=null && _dataset.Tables.Count>0 && _dataset.Tables[0] != null)
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
                        info.Date = dr["casenotedate"].ToString();
                        info.SecurityLevel = Convert.ToBoolean(dr["SecurityLevel"]);
                        info.IsAllowSecurityCN = Convert.ToBoolean(dr["isAllowSecurityCN"]);
                        info.WrittenBy = Convert.ToString(dr["WrittenBy"]);
                        info.IsEditable = Convert.ToBoolean(dr["Editable"]);

                        CaseNoteList.Add(info);
                    }
                }
                if (_dataset != null && _dataset.Tables.Count > 1 && _dataset.Tables[1]!=null && _dataset.Tables[1].Rows.Count > 0)

                {

                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {

                        Name = dr["Name"].ToString();

                    }

                }

                if (_dataset != null && _dataset.Tables.Count > 2 && _dataset.Tables[2]!=null && _dataset.Tables[2].Rows.Count > 0)

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

                    //selectList.Clientlist = (from DataRow dr1 in _dataset.Tables[2].Rows
                    //                     where (Convert.ToInt32(dr1["IsFamily"]) == 1 || Convert.ToInt32(dr1["IsChild"]) == 1)
                    //                     select new RosterNew.User
                    //                     {
                    //                         Id = Convert.ToString(dr1["clientid"]),
                    //                         Name = Convert.ToString(dr1["Name"]).Split('(')[0]
                    //                     }
                    //                   ).ToList();



                }
                if (_dataset.Tables.Count > 3 && _dataset.Tables[3] != null && _dataset.Tables[3].Rows.Count > 0)
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
        public List<CaseNote> GetCaseNoteByTags(ref string Name, ref FingerprintsModel.RosterNew.Users Userlist, int Householdid, int centerid, string id, string AgencyId, string UserId, string Tagnames, int IsFromFamilySummary)
        {
            List<CaseNote> CaseNoteList = new List<CaseNote>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@clientid", id));
                command.Parameters.Add(new SqlParameter("@centerid", centerid));
                command.Parameters.Add(new SqlParameter("@Householdid", Householdid));
                command.Parameters.Add(new SqlParameter("@Tagnames", Tagnames));
                command.Parameters.Add(new SqlParameter("@IsFromFamilySummary", IsFromFamilySummary));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetcasenotesByTags";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                SetCaseNoteDetails(CaseNoteList, ref Name, ref Userlist, _dataset);
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

        public void GetCaseNotesByClient(ref DataTable dtCaseNote, string id, string householdID)
        {
            dtCaseNote = new DataTable();
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@clientid", id));
                command.Parameters.Add(new SqlParameter("@centerid", 0));
                command.Parameters.Add(new SqlParameter("@Householdid", householdID));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getcasenotes";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtCaseNote);
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


        public List<CaseNote> GetCaseNoteByClientId(ref string Name, ref FingerprintsModel.RosterNew.Users Userlist, int Householdid, int clientid, string AgencyId, string UserId)
        {

            List<CaseNote> CaseNoteList = new List<CaseNote>();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@clientid", clientid));
                command.Parameters.Add(new SqlParameter("@Householdid", Householdid));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetcasenotesByClientId";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                SetCaseNoteDetails(CaseNoteList, ref Name, ref Userlist, _dataset);







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
        public List<SubCaseNote> GetSubNotes(string CaseNoteId)
        {
           
            List<SubCaseNote> subCNList = new List<SubCaseNote>();
            string casenoteids = "";
            try
            {
                int subnotesCount = 0;
                command.Parameters.Add(new SqlParameter("@CaseNoteId", CaseNoteId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "GetSubCaseNoteById";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        subnotesCount = Convert.ToInt32(_dataset.Tables[0].Rows[0]["subnotesCount"]);
                    }

                }

                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            SubCaseNote subCasenote = new SubCaseNote();
                            subCasenote.SubCasenoteid = dr["SubCaseNoteID"].ToString();
                            if (subnotesCount > 0 && !casenoteids.Contains(subCasenote.SubCasenoteid))
                            {
                                casenoteids += subCasenote.SubCasenoteid;
                                subnotesCount--;
                                subCasenote.Notes = dr["NoteField"].ToString();
                                subCasenote.WrittenDate = dr["Date"].ToString();
                                subCasenote.Name = dr["Name"].ToString();

                                var items = (from DataRow dr5 in _dataset.Tables[1].Rows
                                             where dr5["SubCaseNoteId"].ToString() == subCasenote.SubCasenoteid && dr5["TagName"].ToString() != ""
                                             select dr5["TagName"].ToString()
                                            ).Distinct().ToList();

                                subCasenote.Tags = items;

                                var attachments = (from DataRow dr5 in _dataset.Tables[1].Rows
                                                   where dr5["SubCaseNoteId"].ToString() == subCasenote.SubCasenoteid && dr5["AttachmentId"].ToString()!=""
                                                   select dr5["AttachmentId"].ToString()
                                            ).Distinct().ToList();

                                subCasenote.Attachment = attachments;
                                subCNList.Add(subCasenote);
                            }
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
            return subCNList;
        }
        //public string SaveSubNotes(string CaseNoteId, string CaseNoteDate, int Householdid, int centerid, string ClassRoomId, string AgencyId, string UserId, string Notes, string RoleId, string Tags)
       public string SaveSubNotes(StaffDetails staff,RosterNew.CaseNote _caseNote)
       
        
        {
            string Subcasenoteid = "";
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@CaseNoteID", _caseNote.CaseNoteid));
                command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@Householdid", _caseNote.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@CaseNotetags", !string.IsNullOrEmpty(_caseNote.CaseNotetags)? _caseNote.CaseNotetags.Trim(',').TrimEnd(','):string.Empty));
                command.Parameters.Add(new SqlParameter("@SubCaseNoteDate", _caseNote.CaseNoteDate));
                command.Parameters.Add(new SqlParameter("@WrittenBy", staff.UserId));
                command.Parameters.Add(new SqlParameter("@RoleOfOwner", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@Note", _caseNote.Note));
                command.Parameters.Add(new SqlParameter("@CenterId", _caseNote.CenterId));
                command.Parameters.Add(new SqlParameter("@ClassroomId", _caseNote.Classroomid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AddSubNotes";
                Subcasenoteid = command.ExecuteScalar().ToString();

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
            return Subcasenoteid;
        }
        public List<CaseNote> GetcaseNoteDetail(string Casenoteid, string ClientId, string AgencyId, string UserId)
        {
            List<CaseNote> CaseNoteList = new List<CaseNote>();
            try
            {
                command.Parameters.Clear();
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
                            info.Date = dr["casenotedate"].ToString();
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
                            info.CaseNoteid = dr["CaseNoteID"].ToString();
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

        public bool SaveAttachmentsOnSubNote(string AgencyId, string CaseNoteId, byte[] file, string AttachmentName, string AttachmentExtension, string UserId, string Subcasenoteid)
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
                command.Parameters.Add(new SqlParameter("@Subcasenoteid", Subcasenoteid));
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

        public string SaveCaseNotes(ref string Name, ref List<CaseNote> CaseNoteList, ref FingerprintsModel.RosterNew.Users Userlist, RosterNew.CaseNote CaseNote, List<RosterNew.Attachment> Attachments, string Agencyid,string roleID, string UserID, int mode = 1)
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
                
                command.Parameters.Add(new SqlParameter("@RoleID", roleID));
                command.Parameters.Add(new SqlParameter("@userid", UserID));
                command.Parameters.Add(new SqlParameter("@agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters.Add(new SqlParameter("@IsLateArrival", CaseNote.IsLateArrival));
                command.Parameters.Add(new SqlParameter("@mode", mode));
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

                    else if(Attachment.AttachmentFileByte!=null && Attachment.AttachmentFileByte.Length>0)
                    {
                        dt.Rows.Add(Attachment.AttachmentFileByte, Attachment.AttachmentFileName,Attachment.AttachmentFileExtension);

                    }
                }
                command.Parameters.Add(new SqlParameter("@Attachments", dt));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveCaseNote";


                if (mode == 1)
                {

                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                    SetCaseNoteDetails(CaseNoteList, ref Name, ref Userlist, _dataset);



                }
                else if(mode==2)
                {
                    Name=Convert.ToString( command.ExecuteScalar());
                    Connection.Close();
                }
                else
                {
                    command.ExecuteNonQuery();
                    Connection.Close();
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
        public Roster GetrosterList(out string totalrecord, string sortOrder, string sortDirection, string Center, string Classroom, int skip, int pageSize, string userid, string agencyid, string roleId, int filterOption, string searchText = "")
        {
            Roster _roster = new Roster();
            List<Roster> RosterList = new List<Roster>();
            List<HrCenterInfo> centerList = new List<HrCenterInfo>();
            _roster.Rosters = new List<Roster>();
            _roster.AbsenceTypeList = new List<SelectListItem>();
            _roster.AbsenceReasonList = new List<SelectListItem>();
            List<ClosedInfo> closedList = new List<ClosedInfo>();
            //int IssuePercentage = 0;
            try
            {


                using (Connection = connection.returnConnection())
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
                    //command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                    //command.Parameters.Add(new SqlParameter("@issuePercentage", 0)).Direction = ParameterDirection.Output;
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_rosterList";
                    Connection.Open();
                    using (SqlDataReader dr = command.ExecuteReader())
                    {


                        
                        int resultSet = 0;
                        do
                        {

                            while (dr.Read() && dr.HasRows)
                            {
                                switch (resultSet)
                                {
                                    case 0:
                                        totalrecord = Convert.ToString(dr["TotalRecord"]);
                                        _roster.Rosters.Add(
                                                            new Roster
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
                                                                //Yakkr600 = DBNull.Value.Equals(dr["yakkr600"]) ? 0 : Convert.ToInt32(dr["yakkr600"]),
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
                                                                // IsAppointMentYakkr600601 = DBNull.Value.Equals(dr["IsAppointMentYakkr600601"]) ? 0 : Convert.ToInt32(dr["IsAppointMentYakkr600601"]),
                                                                Age = dr["Age"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dr["Age"].ToString()),
                                                                IsPreg = string.IsNullOrEmpty(dr["IsPreg"].ToString()) ? 0 : Convert.ToInt32(dr["IsPreg"].ToString()),
                                                                IsClassStarted = string.IsNullOrEmpty(dr["IsClassStarted"].ToString()) ? 0 : Convert.ToInt32(dr["IsClassStarted"].ToString()),
                                                                PrimaryInsurance = string.IsNullOrEmpty(dr["PrimaryInsurance"].ToString()) ? 0 : Convert.ToInt32(dr["PrimaryInsurance"].ToString()),
                                                                ProgramType = (dr["ProgramType"].ToString() == "EHS" && (dr["Age"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dr["Age"].ToString())) >= 3) ? "TRN" : (dr["IsPreg"].ToString() == "1") ? "TRN" : "",
                                                                FamilyHomeless = string.IsNullOrEmpty(dr["FamilyHomeless"].ToString()) ? 0 : Convert.ToInt32(dr["FamilyHomeless"]),
                                                                IsScreeningFollowUpReq = Convert.ToBoolean(dr["ScreeningFollowUpReq"]),
                                                                IsScreeningFollowUpComplete = Convert.ToBoolean(dr["ScreeningFollowUpComplete"]),
                                                                ReferenceProg = Convert.ToInt32(dr["ReferenceProg"]),
                                                                IsFutureWithdrawal = Convert.ToBoolean(dr["StatusUpdated"]),
                                                                IsShowTransition = Convert.ToBoolean(dr["IsShowTransition"]),
                                                                TransitionColor = Convert.ToString(dr["TransitionColor"]),
                                                                TransitionType = Convert.ToInt32(dr["TransitionType"]),
                                                                Returning = Convert.ToString(dr["Returning"]),
                                                                IsShowScreeningFollowUp = Convert.ToBoolean(dr["ShowScreeningFollowUp"]),
                                                                FamilyAdvocate = Convert.ToString(dr["FamilyAdvocate"]),
                                                                IsAllowAttendanceIssueReview = Convert.ToString(dr["FamilyAdvocate"]).ToLowerInvariant() == userid.ToLowerInvariant()

                                                            }
                                      );

                                        _roster.Rosters = _roster.Rosters.Distinct().ToList();


                                        break;

                                    case 1:
                                        _roster.AbsenceTypeList.Add(
                                                                    new SelectListItem
                                                                    {
                                                                        Text = Convert.ToString(dr["Description"]),
                                                                        Value =Convert.ToString(dr["AttendanceTypeId"])
                                                                    });
                                        break;
                                    case 2:
                                        _roster.AbsenceReasonList.Add(new SelectListItem
                                        {
                                            Text = Convert.ToString(dr["absenseReason"]),
                                            Value =Convert.ToString(dr["reasonid"])
                                        });
                                        break;

                                    case 3:


                                        if (Convert.ToInt32(dr["TodayClosed"]) > 0)
                                        {
                                            closedList.Add(new ClosedInfo
                                            {
                                                ClosedToday = Convert.ToInt32(dr["TodayClosed"]),
                                                CenterName = Convert.ToString(dr["CenterName"]),
                                                ClassRoomName =Convert.ToString(dr["ClassRoomName"]),
                                                AgencyName =Convert.ToString(dr["AgencyName"])
                                            }
                                                 );
                                        }



                                        break;
                                }

                            }

                            resultSet++;


                        } while (dr.NextResult());

                    }


                }




                if (closedList.Count() > 0)
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


                //DataAdapter = new SqlDataAdapter(command);
                //_dataset = new DataSet();
                //DataAdapter.Fill(_dataset);
                //totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                //IssuePercentage = Convert.ToInt32(command.Parameters["@issuePercentage"].Value.ToString());
                //if (_dataset.Tables[0] != null)
                //{
                //    if (_dataset.Tables[0].Rows.Count > 0)
                //    {
                //        totalrecord = Convert.ToString(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                //        _roster.Rosters = (from DataRow dr in _dataset.Tables[0].Rows
                //                           select new Roster
                //                           {
                //                               Householid = dr["Householdid"].ToString(),
                //                               Eclientid = EncryptDecrypt.Encrypt64(dr["Clientid"].ToString()),
                //                               EHouseholid = EncryptDecrypt.Encrypt64(dr["Householdid"].ToString()),
                //                               Name = dr["name"].ToString(),
                //                               Gender = dr["gender"].ToString(),
                //                               CenterName = dr["CenterName"].ToString(),
                //                               CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                //                               ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString()),
                //                               RosterYakkr = dr["Yakkr"].ToString(),
                //                               //Yakkr600 = DBNull.Value.Equals(dr["yakkr600"]) ? 0 : Convert.ToInt32(dr["yakkr600"]),
                //                               Yakkr601 = DBNull.Value.Equals(dr["Yakkr601"]) ? 0 : Convert.ToInt32(dr["Yakkr601"]),
                //                               ClassroomName = dr["ClassroomName"].ToString(),
                //                               MarkAbsenseReason = DBNull.Value.Equals(dr["MarkedAbsentReason"]) ? "" : Convert.ToString(dr["MarkedAbsentReason"]),
                //                               IsPresent = DBNull.Value.Equals(dr["IsPresent"]) ? 0 : Convert.ToInt32(dr["IsPresent"]),//.ToString() //Added on 30Dec2016
                //                               Acronym = dr["AcronymName"].ToString(),
                //                               LastCaseNoteDate = string.IsNullOrEmpty(dr["LastCaseNoteDate"].ToString()) ? "" : dr["LastCaseNoteDate"].ToString(),
                //                               LastFPADate = string.IsNullOrEmpty(dr["FPALastDate"].ToString()) ? "" : dr["FPALastDate"].ToString(),
                //                               LastReferralDate = string.IsNullOrEmpty(dr["LastReferralDate"].ToString()) ? "" : dr["LastReferralDate"].ToString(),
                //                               LateArivalNotes = DBNull.Value.Equals(dr["LateArivalNotes"]) ? "" : Convert.ToString(dr["LateArivalNotes"]),
                //                               LateArrivalDuration = DBNull.Value.Equals(dr["LateArrivalDuration"]) ? "" : Convert.ToString(dr["LateArrivalDuration"]),
                //                               IsLateArrival = DBNull.Value.Equals(dr["IsLateArrival"]) ? false : Convert.ToBoolean(dr["IsLateArrival"]),
                //                               IsCaseNoteEntered = DBNull.Value.Equals(dr["IsCaseNoteEntered"]) ? false : Convert.ToBoolean(dr["IsCaseNoteEntered"]),
                //                               IsHomeBased = Convert.ToInt32(dr["IsHomeBased"]),
                //                               // IsAppointMentYakkr600601 = DBNull.Value.Equals(dr["IsAppointMentYakkr600601"]) ? 0 : Convert.ToInt32(dr["IsAppointMentYakkr600601"]),
                //                               Age = dr["Age"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dr["Age"].ToString()),
                //                               IsPreg = string.IsNullOrEmpty(dr["IsPreg"].ToString()) ? 0 : Convert.ToInt32(dr["IsPreg"].ToString()),
                //                               IsClassStarted = string.IsNullOrEmpty(dr["IsClassStarted"].ToString()) ? 0 : Convert.ToInt32(dr["IsClassStarted"].ToString()),
                //                               PrimaryInsurance = string.IsNullOrEmpty(dr["PrimaryInsurance"].ToString()) ? 0 : Convert.ToInt32(dr["PrimaryInsurance"].ToString()),
                //                               ProgramType = (dr["ProgramType"].ToString() == "EHS" && (dr["Age"].ToString() == "" ? Convert.ToDecimal(0) : Convert.ToDecimal(dr["Age"].ToString())) >= 3) ? "TRN" : (dr["IsPreg"].ToString() == "1") ? "TRN" : "",
                //                               FamilyHomeless = string.IsNullOrEmpty(dr["FamilyHomeless"].ToString()) ? 0 : Convert.ToInt32(dr["FamilyHomeless"]),
                //                               IsScreeningFollowUpReq = Convert.ToBoolean(dr["ScreeningFollowUpReq"]),
                //                               IsScreeningFollowUpComplete = Convert.ToBoolean(dr["ScreeningFollowUpComplete"]),
                //                               ReferenceProg = Convert.ToInt32(dr["ReferenceProg"]),
                //                               IsFutureWithdrawal = Convert.ToBoolean(dr["StatusUpdated"]),
                //                               IsShowTransition = Convert.ToBoolean(dr["IsShowTransition"]),
                //                               TransitionColor = Convert.ToString(dr["TransitionColor"]),
                //                               TransitionType = Convert.ToInt32(dr["TransitionType"]),
                //                               Returning = Convert.ToString(dr["Returning"]),
                //                               IsShowScreeningFollowUp = Convert.ToBoolean(dr["ShowScreeningFollowUp"]),
                //                               FamilyAdvocate = Convert.ToString(dr["FamilyAdvocate"]),
                //                               IsAllowAttendanceIssueReview = Convert.ToString(dr["FamilyAdvocate"]).ToLowerInvariant() == userid.ToLowerInvariant()

                //                           }
                //                         ).Distinct().ToList();


                //    }
                //}



                //_roster.AbsenceTypeList = new List<SelectListItem>();

                //if (_dataset.Tables[1] != null)
                //{
                //    if (_dataset.Tables[1].Rows.Count > 0)
                //    {
                //        _roster.AbsenceTypeList = (from DataRow dr5 in _dataset.Tables[1].Rows
                //                                   select new SelectListItem
                //                                   {
                //                                       Text = dr5["Description"].ToString(),
                //                                       Value = dr5["AttendanceTypeId"].ToString()
                //                                   }).ToList();

                //    }
                //}
                //_roster.AbsenceReasonList = new List<SelectListItem>();
                //if (_dataset.Tables[2] != null)
                //{
                //    if (_dataset.Tables[2].Rows.Count > 0)
                //    {
                //        _roster.AbsenceReasonList = (from DataRow dr5 in _dataset.Tables[2].Rows
                //                                     select new SelectListItem
                //                                     {
                //                                         Text = dr5["absenseReason"].ToString(),
                //                                         Value = dr5["reasonid"].ToString()
                //                                     }).ToList();

                //    }
                //}

                //if (_dataset.Tables[3] != null)
                //{
                  

                //    closedList = (from DataRow dr5 in _dataset.Tables[3].Rows
                //                  select new ClosedInfo
                //                  {
                //                      ClosedToday = Convert.ToInt32(dr5["TodayClosed"]),
                //                      CenterName = dr5["CenterName"].ToString(),
                //                      ClassRoomName = dr5["ClassRoomName"].ToString(),
                //                      AgencyName = dr5["AgencyName"].ToString()
                //                  }
                //              ).ToList();
                //    closedList = closedList.Where(x => x.ClosedToday > 0).ToList();
                //    if (closedList.Count() > 0)
                //    {
                //        _roster.ClosedDetails = new ClosedInfo
                //        {
                //            ClosedToday = closedList.Select(x => x.ClosedToday).FirstOrDefault(),
                //            CenterName = string.Join(",", closedList.Select(x => x.CenterName).Distinct().ToArray()),
                //            ClassRoomName = string.Join(",", closedList.Select(x => x.ClassRoomName).Distinct().ToArray()),
                //            AgencyName = closedList.Select(x => x.AgencyName).FirstOrDefault()
                //        };
                //    }
                //    else
                //    {
                //        _roster.ClosedDetails = new ClosedInfo();
                //    }
                //}
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);

            }
            finally
            {
               
                command.Dispose();
                Connection.Dispose();

            }
            return _roster;

        }





        public void MarkAbsent(ref string result, string ChildID,StaffDetails staff,  string absentType, string Cnotes,  int AbsentReasonid, string NewReason)
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
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@clientID", ChildID));
                    command.Parameters.Add(new SqlParameter("@PSignature", " "));
                    command.Parameters.Add(new SqlParameter("@PareID", " "));
                    command.Parameters.Add(new SqlParameter("@Notes", " "));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters.Add(new SqlParameter("@SubstituteID", staff.SubstituteID));
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
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@ClientID", ChildID));
                    command.Parameters.Add(new SqlParameter("@AttendanceType", absentType));
                    command.Parameters.Add(new SqlParameter("@AbsenceReasonId", AbsentReasonid));
                    command.Parameters.Add(new SqlParameter("@NewReason", NewReason));
                    command.Parameters.Add(new SqlParameter("@Notes", ""));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters.Add(new SqlParameter("@SubstituteID", staff.SubstituteID));
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
        public List<ClassRoom> Getclassrooms(string Centerid,StaffDetails staffDetails, bool isEndOfYear = false, bool isInkind = false)
        {
            List<ClassRoom> _ClassRoomlist = new List<ClassRoom>();

            try
            {
                int cenID = 0;
                Centerid = int.TryParse(Centerid, out cenID) ? Centerid : EncryptDecrypt.Decrypt64(Centerid);

             
                command.Parameters.Add(new SqlParameter("@Centerid", Centerid));
                command.Parameters.Add(new SqlParameter("@Agencyid", staffDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                command.Parameters.Add(new SqlParameter("@SubstituteID", staffDetails.SubstituteID));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                command.Parameters.Add(new SqlParameter("@IsInkind", isInkind));

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

                command.Parameters.Clear();
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

                if (info.GoalSteps!= null && info.GoalSteps.Count > 0)
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
                command.Parameters.Clear();
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
                        matchProvider.AgencyId = dr["AgencyId"].ToString();
                        matchProvider.IsFunction = Convert.ToBoolean(dr["IsFunction"]);
                        matchProvider.CommunityId = Convert.ToInt32(dr["CommunityId"]);
                        matchProvider.ReferralDate = Convert.ToString(dr["ReferralDate"]);

                        if (ReferralClientId > 0)
                        {
                            matchProvider.City = dr["City"].ToString();
                            matchProvider.State = dr["State"].ToString();
                            matchProvider.ZipCode = dr["ZipCode"].ToString();
                            matchProvider.Email = dr["Email"].ToString();
                            matchProvider.Phone = dr["PhoneNo"].ToString();
                            matchProvider.ReferralDate = Convert.ToString(dr["ReferralDate"]);
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


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {
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
                    Connection.Open();
                    command.ExecuteNonQuery();
                    Connection.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return false;
            }
        }

        public long SaveReferralClient(long ServiceId, long CommonClientId, Guid AgencyId, Int32 Step, bool Status, int CreatedBy, long referralclientId, long screeningReferralYakkr)
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
                    command.Parameters.AddWithValue("@ScreeningReferralYakkr", screeningReferralYakkr);
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
                command.Parameters.AddWithValue("@ScreeningReferralYakkr", screeningReferralYakkr);
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


        public bool SaveMatchProviders(ListRoster matchProvider)
        {
            try
            {
                int Step = 3;
                bool Status = true;
                bool Success = false;

                StaffDetails staff = StaffDetails.GetInstance();

                Connection.Open();
                command.Connection = Connection;
                command.CommandText = "sp_MatchProviderOperations";
                if (matchProvider.ReferralClientServiceId > 0)
                {
                    command.Parameters.AddWithValue("@Command", "");
                }
                else
                {
                    command.Parameters.AddWithValue("@Command", "INSERT");
                }
                command.Parameters.AddWithValue("@ReferralDate", matchProvider.ReferralDate);
                command.Parameters.AddWithValue("@ClientId", matchProvider.ClientId);
                command.Parameters.AddWithValue("@NotesId", matchProvider.Description);
                command.Parameters.AddWithValue("@ServiceId", matchProvider.ServiceResourceId);
                command.Parameters.AddWithValue("@AgencyId", matchProvider.AgencyId);
                command.Parameters.AddWithValue("@Step", Step);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@UserId", staff.UserId);
                command.Parameters.AddWithValue("@CreatedBy", matchProvider.ClientId);
                command.Parameters.AddWithValue("@CommunityId", matchProvider.CommunityId);
                command.Parameters.AddWithValue("@ReferralClientServiceId", matchProvider.ReferralClientServiceId);
                command.Parameters.AddWithValue("@ScreeningReferralYakkr", EncryptDecrypt.Decrypt64(matchProvider.ScreeningReferralYakkr));

                command.CommandType = CommandType.StoredProcedure;
                var query = command.ExecuteNonQuery();
                Connection.Close();
                if(query>0)
                {
                     Success = true;
                }


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
        public List<ReferralServiceModel> ReferralService(ref int totalRecords, long ClientId, int takeRows, int skipRows, int yakkrCode)
        {
            List<ReferralServiceModel> referralServiceModelList = new List<ReferralServiceModel>();


            _dataset = new DataSet();
            command.Connection = Connection;
            command.CommandText = "sp_ReferralServiceOperations";
            command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
            command.Parameters.Add(new SqlParameter("@Take", takeRows));
            command.Parameters.Add(new SqlParameter("@Skip", skipRows));
            command.Parameters.Add(new SqlParameter("@YakkrCode", yakkrCode));
            command.Parameters.AddWithValue("@Command", "SELECT");
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(_dataset);

            if (_dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in _dataset.Tables[0].Rows)
                {
                    ReferralServiceModel referralService = new ReferralServiceModel();
                    referralService.ServiceId = Convert.ToInt32(dr["ServiceID"]);
                    referralService.ServiceName = dr["Services"].ToString();
                    //code0643 temp change
                    //referralService.ReferralDate =string.IsNullOrEmpty(Convert.ToString(dr["ReferralDate"]))?(DateTime?)null: Convert.ToDateTime(dr["ReferralDate"]);
                    referralService.ReferralDate = string.IsNullOrEmpty(Convert.ToString(dr["ReferralDate"])) ? (DateTime?)null : DateTime.Parse(dr["ReferralDate"].ToString(), new CultureInfo("en-US", true));
                    referralService.ConvReferralDate = Convert.ToString(dr["ReferralDate"]);
                    //  referralService.CreatedDate = string.IsNullOrEmpty(Convert.ToString(dr["CreatedDate"]))? (DateTime?)null : Convert.ToDateTime(dr["CreatedDate"]);
                    //code0643 temp change
                    referralService.CreatedDate = string.IsNullOrEmpty(Convert.ToString(dr["CreatedDate"])) ? (DateTime?)null : DateTime.Parse(dr["CreatedDate"].ToString(), new CultureInfo("en-US", true));
                    referralService.ConvCreatedDate = Convert.ToString(dr["CreatedDate"]);
                    referralService.Step = Convert.ToInt32(dr["Step"]);
                    referralService.ClientId = Convert.ToInt32(dr["ClientId"]);
                    referralService.ReferralClientServiceId = Convert.ToInt32(dr["ReferralClientServiceId"]);
                    referralService.ParentName = dr["ParentName"].ToString();
                    referralService.ScreeningReferralYakkr = dr["ScreeningReferralYakkr"] != DBNull.Value ? EncryptDecrypt.Encrypt64(dr["ScreeningReferralYakkr"].ToString()) : EncryptDecrypt.Encrypt64("0");
                    referralServiceModelList.Add(referralService);
                }

                totalRecords = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

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
                   matchProviderModel.CRColorCode = dr["CRColorCode"].ToString();
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
                    //string DeleteParameter = "DELETE";
                    //int Mode = 2;
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
                    FPAinfo.GoalDate = Convert.ToString(item["GoalDate"]);
                    FPAinfo.CompletionDate = Convert.ToString(item["CompletionDate"]);
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
                    obj.GoalDate = Convert.ToString(ds.Tables[0].Rows[0]["Date"]);
                    obj.CategoryDesc = ds.Tables[0].Rows[0]["CategoryName"].ToString();
                    obj.CompletionDate = Convert.ToString(ds.Tables[0].Rows[0]["CompletionDate"]);
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
                        obj.ActualGoalCompletionDate = Convert.ToString(ds.Tables[0].Rows[0]["ActualGoalCompletionDate"]);
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
                        objstep.StepsCompletionDate = Convert.ToString(item["CompletionDate"]);
                    }
                    objstep.StepID = Convert.ToInt32(item["StepID"].ToString());
                    objstep.Comments = item["Comments"].ToString();
                    if (!string.IsNullOrEmpty(item["ActualCompletionDate"].ToString()))
                    {
                        objstep.ActualCompletionDate = Convert.ToString(item["ActualCompletionDate"]);
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

        public long SaveReferral(ListRoster companyReferral)
        {
            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                int Step = 3;
                bool Status = true;
                bool IsAgency = true;


                DateTime date = DateTime.Parse(companyReferral.ReferralDate, new CultureInfo("en-US", true));

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {


                    command.Connection = Connection;
                    command.CommandText = "sp_ReferalOperations";
                    if (Convert.ToInt32(companyReferral.ReferralClientServiceId) > 0)
                    {
                        command.Parameters.AddWithValue("@Command", "");
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Command", "INSERT");
                    }
                    command.Parameters.AddWithValue("@ReferralDate", date);
                    command.Parameters.AddWithValue("@ClientId", Convert.ToInt32(companyReferral.CommonClientId));
                    command.Parameters.AddWithValue("@NotesId", companyReferral.Description == null ? "" : companyReferral.Description);
                    command.Parameters.AddWithValue("@ServiceId", Convert.ToInt32(companyReferral.ServiceResourceId));
                    command.Parameters.AddWithValue("@AgencyId", companyReferral.AgencyId);
                    command.Parameters.AddWithValue("@Step", Step);
                    command.Parameters.AddWithValue("@IsAgency", IsAgency);
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.AddWithValue("@UserId", staff.UserId);
                    command.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(companyReferral.CommonClientId));
                    command.Parameters.AddWithValue("@CommunityId", Convert.ToInt32(companyReferral.CommunityId));
                    command.Parameters.AddWithValue("@ReferralClientServiceId", Convert.ToInt32(companyReferral.ReferralClientServiceId));
                    command.Parameters.AddWithValue("@ScreeningReferralYakkr", EncryptDecrypt.Decrypt64(companyReferral.ScreeningReferralYakkr));
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    var obj = command.ExecuteScalar();
                    var refId = Convert.ToInt64(obj);
                    Connection.Close();
                    return refId;
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return 0;
            }
        }

        public List<SelectListItem> FamilyServiceList(int ServiceId, string AgencyId)
        {
            try
            {
                DataSet ds = null;

                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Open();
                command.Parameters.Clear();
                List<MatchProviderModel> matchProviderModels = new List<MatchProviderModel>();
                List<SelectListItem> organizationList = new List<SelectListItem>();
                ds = new DataSet();
                using (Connection = connection.returnConnection())
                {

                    command.Connection = Connection;

                    command.CommandText = "sp_FamilyNeeds";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@CommunityId", ServiceId);
                    command.Parameters.AddWithValue("@AgencyID", AgencyId);
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    Connection.Close();
                    da.Fill(ds);
                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
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
                clsError.WriteException(ex);
                throw ex;
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();

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
            try
            {
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
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        Names = new ParentNames();
                        Names.ParentID = Convert.ToInt32(dr["parentid"]);
                        Names.ParentName = dr["ParentName"].ToString();
                        Names.ParentInvolved = DBNull.Value == dr["ParentInvolved"] ? 0 : Convert.ToInt32(dr["ParentInvolved"]);
                        ParentList.Add(Names);
                    }
                    MatrixScore.ParentList = ParentList;
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

            return MatrixScore;



        }

        public MatrixScore GetClientDetails(out List<ShowRecommendations> RecList, long houseHoldId)
        {
            MatrixScore score = new MatrixScore();
            DataSet ds = null;
            List<SelectListItem> activeYearList = new List<SelectListItem>();
            RecList = new List<ShowRecommendations>();
            List<ShowRecommendations> recommList = new List<ShowRecommendations>();
            ShowRecommendations rec = null;
            try
            {

                StaffDetails staff = StaffDetails.GetInstance();


                ds = new DataSet();
                string queryCommand = "CLIENTSTATUS";
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AgencyId", staff.AgencyId);
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
                                Text = dr["ActiveProgramYear"].ToString(),
                                Selected=Convert.ToBoolean(dr["Selected"])
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


        public ArrayList GetRecommendations(long householdId, long AssessmentNo, string activeProgamYear)
        {
            MatrixRecommendations recommendation = null;
            DataSet ds = null;
            List<MatrixRecommendations> recommendationList = new List<MatrixRecommendations>();
            ArrayList arraylist = new ArrayList();
            try
            {

                StaffDetails staff = StaffDetails.GetInstance();
                command.Connection = Connection;
                command.CommandText = "USP_GetMatrixRecommendations";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AgencyId", staff.AgencyId);
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

        public List<AssessmentResults> GetDescription(int groupId, long clientId)
        {
            List<AssessmentResults> resultList = new List<AssessmentResults>();
            AssessmentResults results = null;
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                string queryCommand = "GETDESCRIPTION";

                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AssessmentGroupId", groupId);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Parameters.AddWithValue("@AgencyId", staff.AgencyId);
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

        public List<MatrixScore> GetChartDetails(out AnnualAssessment assessment, out List<ChartDetails> chartlist, long houseHoldId, string date, long clientId)
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
                StaffDetails staff = StaffDetails.GetInstance();

                string queryCommand = (string.IsNullOrEmpty(date)) ? "GETCHARTDETAILS" : "SETCHARTDROPDOWN";
                //  string queryCommand = "GETCHARTDETAILS";
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AgencyId", staff.AgencyId);
                command.Parameters.AddWithValue("@UserId", staff.UserId);
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
                        assess.Assessment1From = string.IsNullOrEmpty(dr["Assessment1FromDays"].ToString()) ? "0" : dr["Assessment1FromDays"].ToString();
                        assess.Assessment1To = string.IsNullOrEmpty(dr["Assessment1ToDays"].ToString()) ? "0" : dr["Assessment1ToDays"].ToString();
                        assess.Assessment2From = string.IsNullOrEmpty(dr["Assessment2FromDays"].ToString()) ? "0" : dr["Assessment2FromDays"].ToString();
                        assess.Assessment2To = string.IsNullOrEmpty(dr["Assessment2ToDays"].ToString()) ? "0" : dr["Assessment2ToDays"].ToString();
                        assess.Assessment3From = string.IsNullOrEmpty(dr["Assessment3FromDays"].ToString())?"0": dr["Assessment3FromDays"].ToString();
                        assess.Assessment3To =string.IsNullOrEmpty(dr["Assessment3ToDays"].ToString())?"0" : dr["Assessment3ToDays"].ToString();
                        assess.EnrollmentDays = string.IsNullOrEmpty(Convert.ToString(dr["EnrollmentDays"]))?"0": Convert.ToString(dr["EnrollmentDays"]);
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
                        chartdetails.MaximumMatrixValue = Convert.ToInt64(dr["MaxMatrix"]);
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
        public List<MatrixScore> SetChart()
        {
            MatrixScore score = null;
            List<MatrixScore> listMatrixScore = new List<MatrixScore>();
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                string queryCommand = "SETCHART";
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "SP_MatrixScore";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AgencyId", staff.AgencyId);
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
                            obj.Date = Convert.ToString(dr["Date"]);
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


        public SelectListItem GetChildrenImageData(long ClientId,int mode=1)
        {
            SelectListItem child = new SelectListItem();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetChildrenImage";
                command.Parameters.AddWithValue("@ClientId", ClientId);
                command.Parameters.AddWithValue("@Mode", mode);
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

        public List<AttendenceDetailsByDate> GetAttendenceDetailsByDate(out int totalRecord,CenterAuditReport report,StaffDetails staff)
        {
            List<AttendenceDetailsByDate> attendanceDetails = new List<AttendenceDetailsByDate>();

            AttendenceDetailsByDate attendence = null;
            totalRecord = 0;
            try
            {
             
                DataSet ds = new DataSet();

              
                command.Connection = Connection;
                command.CommandText = "SP_GetAttendenceDetailsByDate";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AttendanceFromDate", report.FromDate);
                command.Parameters.AddWithValue("@AttendanceToDate", report.ToDate??report.FromDate);
                command.Parameters.AddWithValue("@AgencyId", staff.AgencyId);
                command.Parameters.AddWithValue("@ClientId", EncryptDecrypt.Decrypt64(report.Enc_ClientID));
                command.Parameters.AddWithValue("@Take", report.PageSize);
                command.Parameters.AddWithValue("@Skip", report.SkipRows);
                command.Parameters.AddWithValue("@TotalRecord", report.TotalRecord).Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 200;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);

                totalRecord = Convert.ToInt32(command.Parameters["@TotalRecord"].Value);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        attendence = new AttendenceDetailsByDate();
                        attendence.Enc_ClientId = EncryptDecrypt.Encrypt64(Convert.ToString(dr["ClientID"]));
                        attendence.ClientName = Convert.ToString(dr["ChildName"]);
                        attendence.AttendanceDate = Convert.ToString(dr["AttendanceDate"]);
                        attendence.AttendenceStatus = Convert.ToString(dr["AttendanceTypeName"]);
                        attendence.Breakfast =dr["Breakfast"]!=DBNull.Value? Convert.ToBoolean(dr["Breakfast"]):false;
                        attendence.BreakfastServedOn = dr["BreakfastServedOn"] != DBNull.Value ? Convert.ToDateTime(dr["BreakfastServedOn"]) : (DateTime?)null;
                        attendence.Lunch = dr["Lunch"] != DBNull.Value ? Convert.ToBoolean(dr["Lunch"]) : false;
                        attendence.LunchServedOn = dr["LunchServedOn"] != DBNull.Value ? Convert.ToDateTime(dr["LunchServedOn"]) : (DateTime?)null;
                        attendence.Snack = dr["Snacks"] != DBNull.Value ? Convert.ToBoolean(dr["Snacks"]) : false;
                        attendence.SnackServedOn = dr["SnackServedOn"] != DBNull.Value ? Convert.ToDateTime(dr["SnackServedOn"]) : (DateTime?)null;
                        attendence.Dinner = dr["Dinner"] != DBNull.Value ? Convert.ToBoolean(dr["Dinner"]) : false;
                        attendence.DinnerServedOn = dr["DinnerServedOn"] != DBNull.Value ? Convert.ToDateTime(dr["DinnerServedOn"]) : (DateTime?)null;
                        attendence.TimeIn = Convert.ToString(dr["TimeIn"]);
                        attendence.ParentSig = dr["PSignature"] != DBNull.Value ? Convert.ToString(dr["PSignature"]) : string.Empty;
                        attendence.SignedInName = Convert.ToString(dr["SignedInName"]);
                       // attendence.ParentCheckedIn = Convert.ToString(dr["SignedInBy"]);
                        attendence.TimeOut = Convert.ToString(dr["TimeOut"]);
                        attendence.ParentSigOut = dr["PSignatureOut"] != DBNull.Value ? Convert.ToString(dr["PSignatureOut"]) : string.Empty;
                        attendence.SignedOutName = Convert.ToString(dr["SignedOutName"]);
                      //  attendence.ParentCheckedOut = Convert.ToString(dr["SignedOutBy"]);
                        attendence.TimeIn2 = Convert.ToString(dr["TimeIn2"]);
                        attendence.ParentSig2 = dr["PSignature2"] != DBNull.Value ? Convert.ToString(dr["PSignature2"]) : string.Empty;
                        attendence.SignedIn2Name = Convert.ToString(dr["SignedIn2Name"]);
                      //  attendence.ParentCheckedIn2 = Convert.ToString(dr["SignedInBy2"]);
                        attendence.TimeOut2 = Convert.ToString(dr["TimeOut2"]);
                        attendence.ParentSigOut2 = dr["PSignatureOutBy2"] != DBNull.Value ? Convert.ToString(dr["PSignatureOutBy2"]) : string.Empty;
                        attendence.SignedOut2Name = Convert.ToString(dr["SignedOut2Name"]);
                      //  attendence.ParentCheckedOut2 = Convert.ToString(dr["SignedOutBy2"]);
                        attendence.TeacherCheckInSig = Convert.ToString(dr["TSignatureIn"]);
                        attendence.TeacherCheckInTime = Convert.ToString(dr["TeacherCheckInTime"]);
                        attendence.TeacherCheckInTime2 = Convert.ToString(dr["TeacherCheckInTime2"]);
                        attendence.TeacherCheckInSig2 = Convert.ToString(dr["TSignatureIn2"]);
                      //  attendence.TeacherCheckedIn = Convert.ToString(dr["TeacherUserID"]);
                        attendence.TeacherName = Convert.ToString(dr["TeacherName"]);
                        attendence.TeacherName2 = Convert.ToString(dr["TeacherName2"]);
                        attendence.ObservationDescription = Convert.ToString(dr["ObservationDescription"]);
                        attendence.AbsenceReason = Convert.ToString(dr["AbsenceReason"]);
                        attendence.ProtectiveBadge = dr["IdNo"] != DBNull.Value ? Convert.ToString(dr["IdNo"]) : string.Empty;
                        attendence.ProtectiveBadge2 = dr["IdNo2"] != DBNull.Value ? Convert.ToString(dr["IdNo2"]) : string.Empty;

                        attendence.CenterName = Convert.ToString(dr["CenterName"]);
                        attendence.ClassroomName = Convert.ToString(dr["ClassroomName"]);

                        attendanceDetails.Add(attendence);

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
            return attendanceDetails;
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
        public List<SeatAvailability> SaveChildHeadStartTranstion(TransitionDetails Transition, string AgencyId, string UserId, string RoleId,bool isStatusChange=false)
        {
            List<SeatAvailability> AvailabilityList = new List<SeatAvailability>();
            try
            {

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("Attachment", typeof(byte[])),
                      new DataColumn("AttachmentName",typeof(string)),
                        new DataColumn("Attachmentextension",typeof(string))
                    });
                foreach (RosterNew.Attachment Attachment in Transition.Transition.CaseNoteDetails.CaseNoteAttachmentList)
                {
                    if (Attachment != null && Attachment.file != null)
                    {
                        dt.Rows.Add(new BinaryReader(Attachment.file.InputStream).ReadBytes(Attachment.file.ContentLength), Attachment.file.FileName, Path.GetExtension(Attachment.file.FileName));

                    }
                }


                int i = 1;

                if (((Transition.Transition.TypeOfTransition==10 ||Transition.Transition.TypeOfTransition==9) && Transition.Transition.NewProgramYearTransition == false) || (isStatusChange))
                {

                    foreach (PregMomChilds objChild in Transition.PregMomChilds)
                    {
                        
                        SeatAvailability seats = new SeatAvailability();
                        Connection.Open();
                        command.Connection = Connection;

                        command.CommandText = "USP_AddPregnantMomEHS";
                        command.Parameters.Clear();

                        command.CommandType = CommandType.StoredProcedure;

                        //if (!string.IsNullOrEmpty(objChild.DOB))
                        //    dateOfBirth = Convert.ToDateTime(objChild.DOB).ToString("yyyy-MM-dd");


                        command.Parameters.Add(new SqlParameter("@ClientId", Transition.Transition.ClientId));
                        command.Parameters.Add(new SqlParameter("@ProgramTypeId", Transition.Transition.ProgramTypeId));
                        command.Parameters.Add(new SqlParameter("@DateOfTransition", Transition.Transition.DateOfTransition));
                      
                        command.Parameters.Add(new SqlParameter("@BirthType", Transition.Transition.BirthType));
                        command.Parameters.Add(new SqlParameter("@UpdatePregMom", (i == 1)));
                        command.Parameters.Add(new SqlParameter("@Name", objChild.Name));
                        command.Parameters.Add(new SqlParameter("@ChildFirstName", objChild.FirstName));
                        command.Parameters.Add(new SqlParameter("@ChildLastName", objChild.LastName));
                        command.Parameters.Add(new SqlParameter("@DOB", objChild.DOB));
                        command.Parameters.Add(new SqlParameter("@Gender", objChild.Gender));
                        command.Parameters.Add(new SqlParameter("@IsEHS", objChild.IsEHS));
                        command.Parameters.Add(new SqlParameter("@IsFirstChild", i == 1 ? true : false));
                        command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                        command.Parameters.Add(new SqlParameter("@UserId", UserId));
                        command.Parameters.Add(new SqlParameter("@RoleId", RoleId));

                       
                        //parameters for insurance type
                        command.Parameters.Add(new SqlParameter("@ChildInsuranceType", Transition.Transition.ChildInsuranceType));
                        command.Parameters.Add(new SqlParameter("@ChildOtherInsuranceDescription", Transition.Transition.ChildOtherInsuranceTypeDesc));

                        if (i == 1)
                        {
                            command.Parameters.Add(new SqlParameter("@PregMomInsuranceType", Transition.Transition.TrnsInsuranceType));
                            command.Parameters.Add(new SqlParameter("@ParentOtherInsuranceDescription", Transition.Transition.OtherInsuranceTypeDesc));
                        }

                        command.Parameters.Add(new SqlParameter("@Result", string.Empty));
                        command.Parameters.Add(new SqlParameter("@SlotCount", 0));
                        command.Parameters.Add(new SqlParameter("@SeatCount",0));
                        command.Parameters["@Result"].Direction = ParameterDirection.Output;
                        command.Parameters["@SlotCount"].Direction = ParameterDirection.Output;
                        command.Parameters["@SeatCount"].Direction = ParameterDirection.Output;

                        command.Parameters.Add(new SqlParameter("@Attachments", dt));
                        command.Parameters.Add(new SqlParameter("@CaseNoteDate", Transition.Transition.CaseNoteDetails.CaseNoteDate));
                        command.Parameters.Add(new SqlParameter("@CaseNotetitle", Transition.Transition.CaseNoteDetails.CaseNotetitle));
                        command.Parameters.Add(new SqlParameter("@Note", Transition.Transition.CaseNoteDetails.Note));
                        command.Parameters.Add(new SqlParameter("@CaseNotetags", Transition.Transition.CaseNoteDetails.CaseNotetags));
                        command.Parameters.Add(new SqlParameter("@CaseNoteSecurity", Transition.Transition.CaseNoteDetails.CaseNoteSecurity));
                        command.Parameters.Add(new SqlParameter("@Reason", Transition.Transition.Reason));
                        command.Parameters.Add(new SqlParameter("@StatusText", Transition.Transition.StatusText));
                        command.Parameters.Add(new SqlParameter("@status", Transition.Transition.Status));
                        command.Parameters.Add(new SqlParameter("@ddlreason", Transition.Transition.ddlreason));
                        command.Parameters.Add(new SqlParameter("@ddlreasontext", Transition.Transition.ddlreasontext));
                        command.Parameters.Add(new SqlParameter("@ClientIds", string.Join(",", Transition.Transition.Users.Clientlist.Select(x => x.Id).ToArray())));
                        command.Parameters.Add(new SqlParameter("@StaffIds", string.Join(",", Transition.Transition.Users.UserList.Select(x => x.Id).ToArray())));
                        command.Parameters.Add(new SqlParameter("@MedicalServices", Transition.Transition.MedicalServices));
                        command.Parameters.Add(new SqlParameter("@DentalService", Transition.Transition.DentalServices));

                        command.Parameters.Add(new SqlParameter("@IsWaiting", string.IsNullOrEmpty(Transition.Transition.IsWaiting) ? false : Transition.Transition.IsWaiting == "1" ? true : false));
                        command.Parameters.Add(new SqlParameter("@NewProgramYearTransition", Transition.Transition.NewProgramYearTransition));
                        command.Parameters.Add(new SqlParameter("@TypeOfTransition", Transition.Transition.TypeOfTransition));
                        command.Parameters.Add(new SqlParameter("@TransitionType", Transition.Transition.TransitioningType));
                        command.Parameters.Add(new SqlParameter("@AllowFutureApplication", Transition.Transition.IsFutureApplication));

                        int RowsAffected = command.ExecuteNonQuery();
                        seats.Result = Convert.ToInt32((DBNull.Value == command.Parameters["@Result"].Value) ? 0 : command.Parameters["@Result"].Value);
                        seats.SeatAvailable = Convert.ToInt32((DBNull.Value == command.Parameters["@SeatCount"].Value) ? 0 : command.Parameters["@SeatCount"].Value);
                        seats.SloatAvailable = Convert.ToInt32((DBNull.Value == command.Parameters["@SlotCount"].Value) ? 0 : command.Parameters["@SlotCount"].Value);
                        seats.ChildName = objChild.Name;
                        AvailabilityList.Add(seats);
                        i++;

                        Connection.Close();
                    }
                }

                else
                {
                    SeatAvailability seats = new SeatAvailability();
                    Connection.Open();
                    command.Connection = Connection;
                    command.CommandText = "USP_AddPregnantMomEHS";
                    command.Parameters.Clear();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ClientId", Transition.Transition.ClientId));
                    command.Parameters.Add(new SqlParameter("@ProgramTypeId", Transition.Transition.ProgramTypeId));
                    command.Parameters.Add(new SqlParameter("@DateOfTransition", Transition.Transition.DateOfTransition));
                    command.Parameters.Add(new SqlParameter("@BirthType", Transition.Transition.BirthType));
                    command.Parameters.Add(new SqlParameter("@PregMomInsuranceType", Transition.Transition.TrnsInsuranceType));
                    command.Parameters.Add(new SqlParameter("@ParentOtherInsuranceDescription", Transition.Transition.OtherInsuranceTypeDesc));
                    command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
                    //parameters for insurance type
                    command.Parameters.Add(new SqlParameter("@UpdatePregMom", (i==1)));
                    command.Parameters.Add(new SqlParameter("@Result", string.Empty));
                    command.Parameters.Add(new SqlParameter("@SlotCount",0));
                    command.Parameters.Add(new SqlParameter("@SeatCount", 0));
                    command.Parameters["@Result"].Direction = ParameterDirection.Output;
                    command.Parameters["@SlotCount"].Direction = ParameterDirection.Output;
                    command.Parameters["@SeatCount"].Direction = ParameterDirection.Output;
                    command.Parameters.Add(new SqlParameter("@Attachments", dt));
                    command.Parameters.Add(new SqlParameter("@CaseNoteDate", Transition.Transition.CaseNoteDetails.CaseNoteDate));
                    command.Parameters.Add(new SqlParameter("@CaseNotetitle", Transition.Transition.CaseNoteDetails.CaseNotetitle));
                    command.Parameters.Add(new SqlParameter("@Note", Transition.Transition.CaseNoteDetails.Note));
                    command.Parameters.Add(new SqlParameter("@CaseNotetags", Transition.Transition.CaseNoteDetails.CaseNotetags.Trim(',')));
                    command.Parameters.Add(new SqlParameter("@CaseNoteSecurity", Transition.Transition.CaseNoteDetails.CaseNoteSecurity));
                    command.Parameters.Add(new SqlParameter("@Reason", Transition.Transition.Reason));
                    command.Parameters.Add(new SqlParameter("@StatusText", Transition.Transition.StatusText));
                    command.Parameters.Add(new SqlParameter("@status", Transition.Transition.Status));
                    command.Parameters.Add(new SqlParameter("@ddlreason", Transition.Transition.ddlreason));
                    command.Parameters.Add(new SqlParameter("@ddlreasontext", Transition.Transition.ddlreasontext));
                    command.Parameters.Add(new SqlParameter("@ClientIds", string.Join(",", Transition.Transition.Users.Clientlist.Select(x => x.Id).ToArray())));
                    command.Parameters.Add(new SqlParameter("@StaffIds", string.Join(",", Transition.Transition.Users.UserList.Select(x => x.Id).ToArray())));
                    command.Parameters.Add(new SqlParameter("@MedicalServices", Transition.Transition.MedicalServices));
                    command.Parameters.Add(new SqlParameter("@IsWaiting", string.IsNullOrEmpty(Transition.Transition.IsWaiting) ? false : Transition.Transition.IsWaiting == "1" ? true : false));
                    command.Parameters.Add(new SqlParameter("@NewProgramYearTransition", Transition.Transition.NewProgramYearTransition));
                    command.Parameters.Add(new SqlParameter("@DentalService", Transition.Transition.DentalServices));
                    command.Parameters.Add(new SqlParameter("@TypeOfTransition", Transition.Transition.TypeOfTransition));
                    command.Parameters.Add(new SqlParameter("@TransitionType", Transition.Transition.TransitioningType));
                    command.Parameters.Add(new SqlParameter("@AllowFutureApplication", Transition.Transition.IsFutureApplication));

                    int RowsAffected = command.ExecuteNonQuery();
                    seats.Result = Convert.ToInt32((DBNull.Value == command.Parameters["@Result"].Value) ? 0 : command.Parameters["@Result"].Value);
                    seats.SeatAvailable = Convert.ToInt32((DBNull.Value == command.Parameters["@SeatCount"].Value) ? 0 : command.Parameters["@SeatCount"].Value);
                    seats.SloatAvailable = Convert.ToInt32((DBNull.Value == command.Parameters["@SlotCount"].Value) ? 0 : command.Parameters["@SlotCount"].Value);
                    AvailabilityList.Add(seats);
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

        public Roster GetCenterList(string programYear = "")
        {
            Roster _roster = new Roster();
            List<HrCenterInfo> centerList = new List<HrCenterInfo>();
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@ProgramYear", programYear));
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
                        info.Enc_CenterID = EncryptDecrypt.Encrypt64(Convert.ToString(dr["center"]));
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

        public int SaveHeadStartTranstion(Transition Transition, string AgencyId, string UserId)
        {
            
            int resultValue = 0;
            try
            {
                
                


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();

                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@ClientId", Transition.ClientId));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@DateOfTransition", Transition.DateOfTransition));
                command.Parameters.Add(new SqlParameter("@TransitionType", Transition.TransitioningType));
                command.Parameters.Add(new SqlParameter("@TypeOfTransition", Transition.TypeOfTransition));
                command.Parameters.Add(new SqlParameter("@EHS_HS", Transition.EHSHSEnrolled));
                command.Parameters.Add(new SqlParameter("@CenterId", Transition.CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", Transition.ClassRoomId));
                command.Parameters.Add(new SqlParameter("@MedicalHome", Transition.MedicalHome));
                command.Parameters.Add(new SqlParameter("@DentalHome", Transition.DentalHome));
                command.Parameters.Add(new SqlParameter("@MedicalService", Transition.MedicalServices));
                command.Parameters.Add(new SqlParameter("@DentalService", Transition.DentalServices));
                command.Parameters.Add(new SqlParameter("@TANF", Transition.TANF));
                command.Parameters.Add(new SqlParameter("@SSI", Transition.SSI));
                command.Parameters.Add(new SqlParameter("@WIC", Transition.WIC));
                command.Parameters.Add(new SqlParameter("@SNAP", Transition.SNAP));
                command.Parameters.Add(new SqlParameter("@NONE", Transition.NONE));
                command.Parameters.Add(new SqlParameter("@ImmunizationService", Transition.ImmunizationService));

                //Father//
                command.Parameters.Add(new SqlParameter("@ParentID", Transition.ParentID));
                command.Parameters.Add(new SqlParameter("@ShoolAchievement", Transition.ShoolAchievement));
                command.Parameters.Add(new SqlParameter("@JobTrainingFinished", Transition.JobTrainingFinished));
                command.Parameters.Add(new SqlParameter("@AcceptJobTrainingFinished", Transition.AcceptJobTrainingFinished));
                
                
                //Mother//
                command.Parameters.Add(new SqlParameter("@ParentID2", Transition.ParentID2));
                command.Parameters.Add(new SqlParameter("@ShoolAchievement2", Transition.ShoolAchievement2));
                command.Parameters.Add(new SqlParameter("@JobTrainingFinished2", Transition.JobTrainingFinished2));
                command.Parameters.Add(new SqlParameter("@AcceptJobTrainingFinished2", Transition.AcceptJobTrainingFinished2));


                command.Parameters.Add(new SqlParameter("@ChildInsuranceType", Transition.ChildInsuranceType));
                command.Parameters.Add(new SqlParameter("@ChildOtherInsuranceDescription", Transition.ChildOtherInsuranceTypeDesc));

                command.Parameters.Add(new SqlParameter("@TransitionProgramType", Transition.TransProgramTypeID));
                command.Parameters.Add(new SqlParameter("@CurrentProgramTypeID", Transition.ProgramTypeId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertHeadStartTransition";

                Object Result = command.ExecuteScalar();
                if (Result != null)
                    resultValue = Convert.ToInt32(Result);

                //if (resultValue > 0 && resultValue!=2)
                //    isInserted = true;

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
            return resultValue;
        }


        public int GetAvailablitySeatsByClass(string CenterId, string ClassRoomId, string Agencyid, string ClientId)
        {

            int available = 0;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {


                    int id = string.IsNullOrEmpty(ClientId) ? 0 : Convert.ToInt32(ClientId);
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                    command.Parameters.Add(new SqlParameter("@ClassRoomId", ClassRoomId));
                    command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                    command.Parameters.Add(new SqlParameter("@ClientId", id));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_TotalAvailabilitySeatsByAgencyId";
                    Connection.Open();
                    int.TryParse(command.ExecuteScalar().ToString(), out available);
                    Connection.Close();

                }
                //DataAdapter = new SqlDataAdapter(command);
                //_dataset = new DataSet();
                //DataAdapter.Fill(_dataset);

                //if (_dataset != null && _dataset.Tables.Count > 0)
                //{

                //    foreach (DataRow dr in _dataset.Tables[0].Rows)
                //    {
                //        availableSeats = dr["AvailableSeats"].ToString() == null ? "0" : dr["AvailableSeats"].ToString();
                //    }
                //}
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
            return available;
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

                if (_dataset != null && _dataset.Tables[0].Rows.Count > 0)
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

        public FamilyHouseless GetHouselessClient(string householdId, string clientId)
        {

            FamilyHouseless Family = new FamilyHouseless();
            Family.FamilyHousehold = new FamilyHousehold();
            Family.UsersList = new RosterNew.Users();
            Family.UsersList.Clientlist = new List<RosterNew.User>();
            Family.UsersList.UserList = new List<RosterNew.User>();


            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@HouseholdId", Convert.ToInt64(EncryptDecrypt.Decrypt64(householdId))));
                    command.Parameters.Add(new SqlParameter("@ClientId", Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId))));
                    command.CommandText = "USP_GetHomeLessHousingClient";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }

                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Family.FamilyHousehold.ClientFname = _dataset.Tables[0].Rows[0]["Name"].ToString();
                        Family.FamilyHousehold.clientIdnew = EncryptDecrypt.Encrypt64(_dataset.Tables[0].Rows[0]["ClientId"].ToString());
                        Family.FamilyHousehold.CProgramType = EncryptDecrypt.Encrypt64(_dataset.Tables[0].Rows[0]["ProgramID"].ToString());
                        Family.FamilyHousehold.CenterId = Convert.ToInt64(_dataset.Tables[0].Rows[0]["CenterID"]);

                    }

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        Family.UsersList.Clientlist = _dataset.Tables[1].AsEnumerable().Select(x => new RosterNew.User
                        {
                            Id = EncryptDecrypt.Encrypt64(x.Field<long>("ClientId").ToString()),
                            Name = x.Field<string>("ParentName")
                        }).ToList();
                        Family.FamilyHousehold.EAddress1 = _dataset.Tables[1].Rows[0]["HouseholdAddress"].ToString();
                        Family.FamilyHousehold.Encrypthouseholid = EncryptDecrypt.Encrypt64(_dataset.Tables[1].Rows[0]["ID"].ToString());
                        Family.FamilyHousehold.HouseholdId = Convert.ToInt32(_dataset.Tables[1].Rows[0]["ID"]);

                    }

                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        Family.UsersList.UserList = _dataset.Tables[2].AsEnumerable().Select(x => new RosterNew.User
                        {

                            Id = x.Field<Guid>("UserId").ToString(),
                            Name = x.Field<string>("Name").ToString()

                        }).ToList();
                    }


                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Family;

        }


        public List<SelectListItem> GetFollowUpScreenings(string clientId, int followup = 0)
        {
            List<SelectListItem> followupScreen = new List<SelectListItem>();


            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@ClientID", Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId))));
                    command.Parameters.Add(new SqlParameter("@followup", followup));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@AgencyId", staff.AgencyId));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetFollowupScreeningByClient";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                }

                if (_dataset != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    followupScreen = _dataset.Tables[0].AsEnumerable().Select(x => new SelectListItem
                    {

                        Text = x.Field<string>("ScreeningName"),
                        Value = x.Field<int>("ScreeningID").ToString()

                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return followupScreen;
        }


        public List<ClassRoom> GetClassroomsWithFSWHVByCenter(ref List<SelectListItem> staffList, string Centerid)
        {
            List<ClassRoom> _ClassRoomlist = new List<ClassRoom>();
            staffList = new List<SelectListItem>();
            try
            {
                int cenID = 0;
                Centerid = int.TryParse(Centerid, out cenID) ? Centerid : EncryptDecrypt.Decrypt64(Centerid);

                StaffDetails staffDetails = StaffDetails.GetInstance();


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Centerid", Centerid));
                    command.Parameters.Add(new SqlParameter("@Agencyid", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetClassroomWithFSWHVByCenter";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        _ClassRoomlist = _dataset.Tables[0].AsEnumerable().Select(x => new ClassRoom
                        {

                            ClassroomID = Convert.ToInt32(x.Field<long>("ClassroomID")),
                            Enc_ClassRoomId = EncryptDecrypt.Encrypt64(x.Field<long>("ClassroomID").ToString()),
                            ClassName = x.Field<string>("ClassroomName")

                        }).ToList();
                    }



                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        staffList = _dataset.Tables[1].AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = x.Field<Guid>("UserID").ToString(),
                            Text = x.Field<string>("StaffName")
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
                Connection.Dispose();
                DataAdapter.Dispose();
                command.Dispose();
            }
            return _ClassRoomlist;
        }


        //public bool NextProgramYearTransition(string clientId)
        //{
        //    bool isResult = false;
        //    try
        //    {

        //        StaffDetails staff = StaffDetails.GetInstance();

        //        if (Connection.State == ConnectionState.Open)
        //            Connection.Close();


        //        using (Connection = connection.returnConnection())
        //        {
        //            command.Connection = Connection;
        //            command.Parameters.Clear();
        //            command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
        //            command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
        //            command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
        //            command.Parameters.Add(new SqlParameter("ClientID", Convert.ToInt64(EncryptDecrypt.Decrypt64(clientId))));
        //            command.Parameters.Add(new SqlParameter("@mode", 0));
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "USP_InsertNextProgramYearTransition";
        //            Connection.Open();
        //            isResult = (Convert.ToInt32(command.ExecuteNonQuery()) > 0);

        //            Connection.Close();
        //        }




        //    }

        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    finally
        //    {
        //        Connection.Dispose();
        //        command.Dispose();
        //    }
        //    return isResult;
        //}


        public bool UpdateReturningTransitionClient(int returnValue,string clientID)
        {

            bool isRowsAffected = false;
            try
            {
                clientID = EncryptDecrypt.Decrypt64(clientID);
                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@Returning", returnValue));
                    command.Parameters.Add(new SqlParameter("@ClientID", Convert.ToInt64(clientID)));
                    command.CommandText = "USP_UpdateReturning_TransitionClient";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    isRowsAffected = (Convert.ToInt32(command.ExecuteNonQuery()) > 0);
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isRowsAffected;
        }

        public List<HouseholdTags> GetHouseholdCasenoteTags(string household)
        {


            List<HouseholdTags> HouseholdTagsList = new List<HouseholdTags>();

            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Agencyid", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@householdid", household));
                    command.Parameters.Add(new SqlParameter("@Roleid", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_GetHouseholdCasenoteTags";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            HouseholdTags HouseholdTags = null;
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                HouseholdTags = new HouseholdTags();
                                HouseholdTags.TagCount = Convert.ToInt32(dr["TagCount"]);
                                HouseholdTags.TagName = Convert.ToString(dr["TagName"]);
                                var items = (from DataRow dr5 in _dataset.Tables[1].Rows
                                             where HouseholdTags.TagName == dr5["TagName"].ToString()
                                             select new
                                             {
                                                 CasenoteId = dr5["CasenoteId"].ToString()
                                             }).ToList();
                                string casenoteids = string.Join(",", items);

                                List<string> list = (from DataRow dr5 in _dataset.Tables[1].Rows
                                                                where casenoteids.Contains(dr5["CasenoteId"].ToString()) &&
                                                                HouseholdTags.TagName != dr5["TagName"].ToString()

                                                     select dr5["TagName"].ToString()).Distinct().ToList();
                                string str = "";
                                HouseholdTags.AssociatedTags = new List<AssociatedTags>();
                                foreach (var item in list)
                                {
                                   
                                    if(!str.Contains(item))
                                    {
                                        AssociatedTags t= new AssociatedTags();
                                        t.TagName = item;
                                        t.TagId = item;
                                        HouseholdTags.AssociatedTags.Add(t);
                                    }
                                    str += item;
                                }
                               

                                HouseholdTagsList.Add(HouseholdTags);
                            }


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
                Connection.Dispose();
                DataAdapter.Dispose();
                command.Dispose();
            }
            return HouseholdTagsList;
        }


        #region ReferalReviewList

        public List<ReferalDetails> GetOrganizationListWithCount(int ServiceId, string AgencyId,int mode)
        {

            List<ReferalDetails> OrgList = new List<ReferalDetails>();

            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
               

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                    command.Parameters.Add(new SqlParameter("@Roleid", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@mode", mode));
                    command.Parameters.Add(new SqlParameter("@ServiceId", ServiceId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_RefOrgnaizationReviewList";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }

                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                           
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                var Org = new ReferalDetails()
                                {

                              CompanyName = dr["OrganizationName"].ToString(),
                              CommunityResourceID = Convert.ToInt32( dr["CommunityId"].ToString()),
                              CRColorCode = DBNull.Value == dr["ColorCode"] ? 0 : Convert.ToInt32(dr["ColorCode"].ToString()),
                              ReviewCount = DBNull.Value == dr["ReviewCount"] ? 0 : Convert.ToInt32(dr["ReviewCount"].ToString()),
                                    
                                };
                                OrgList.Add(Org);

                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }

            return OrgList;


        }


        public List<ReferalDetails> GetReviewList(int communityid,int mode)
        {

            List<ReferalDetails> OrgList = new List<ReferalDetails>();

            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@Agencyid", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@Roleid", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@mode", mode));
                    command.Parameters.Add(new SqlParameter("@CommunityId", communityid));
                    //command.Parameters.Add(new SqlParameter("@ServiceId", ServiceId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_RefOrgnaizationReviewList";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }

                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                var Org = new ReferalDetails()
                                {

                                   // CompanyName = dr["OrganizationName"].ToString(),
               
                                     QuestionaireID = Convert.ToInt32(dr["QuestionaireId"].ToString()),
                                      MgNotes = dr["MgNotes"].ToString(),
                                    CRColorCode = DBNull.Value == dr["ColorCode"] ? 0 : Convert.ToInt32(dr["ColorCode"].ToString()),
                                   // ReviewCount = DBNull.Value == dr["ReviewCount"] ? 0 : Convert.ToInt32(dr["ReviewCount"].ToString()),
                                     ModifiedBy = dr["ModifiedBy"].ToString(),
                                       ModifiedDate = dr["ModifiedDate"].ToString()

                                };
                                OrgList.Add(Org);

                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }

            return OrgList;


        }



        #endregion  ReferalReviewList



        public bool DeleteCaseNote(int casenoteid,int[] appendcid,bool deletemain,int mode)
        {
            var success = false;
            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@CaseNoteId", casenoteid));
                    command.Parameters.Add(new SqlParameter("@IsDeleteMainCN", deletemain ? 1 : 0));
                    command.Parameters.Add(new SqlParameter("@Mode", mode));
                    DataTable T1 = new DataTable();
                    T1.Columns.AddRange(new DataColumn[2] {
                        new DataColumn("CaseNoteId", typeof(int)),
                        new DataColumn("SubCaseNoteID", typeof(int))
                    });
                    if (appendcid != null && appendcid.Length > 0)
                    {

                        for (int i = 0; i < appendcid.Length; i++)
                        {
                            T1.Rows.Add(casenoteid, appendcid[i]);
                        }
                    }
                    command.Parameters.Add(new SqlParameter("@AppendCaseNoteIds", T1));
                    command.CommandText = "USP_DeleteCaseNote";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    success = (Convert.ToInt32(command.ExecuteNonQuery()) > 0);
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


            return success;

        }

        #region CaseNote_Tag_Report

        public CaseNoteTagReport GetCaseNoteTagReport(long pno, long psize,string search, string sortclmn, string sortdir, int mode = 1)
        {

            CaseNoteTagReport TagReport = new CaseNoteTagReport();
            TagReport.TagReport = new List<CaseNoteTag>();
            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@TagId", 1));
                    command.Parameters.Add(new SqlParameter("@Mode", mode));
                    command.Parameters.Add(new SqlParameter("@PageSize", psize));
                    command.Parameters.Add(new SqlParameter("@PageNo", pno));
                    command.Parameters.Add(new SqlParameter("@Search", search));
                    command.Parameters.Add(new SqlParameter("@Sortclmn", sortclmn));
                    command.Parameters.Add(new SqlParameter("@Sortdir", sortdir));

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_CNTagReportDetails";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                    if (_dataset != null && _dataset.Tables.Count > 1)
                    {

                            if (_dataset.Tables[0].Rows.Count > 0)
                            {

                                foreach (DataRow dr in _dataset.Tables[0].Rows)
                                {

                                var tagrow = new CaseNoteTag()
                                {

                                    TagId = Convert.ToInt64(dr["TagKey"].ToString()),
                                    TagName = dr["TagName"].ToString(),
                                    Count = Convert.ToInt64(dr["Counter"].ToString())
                                };

                                TagReport.TagReport.Add(tagrow);

                                }
                            }

                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            TagReport.TotalRecord = Convert.ToInt64(_dataset.Tables[1].Rows[0]["TotalRecord"].ToString());
                        }

                        }



                                }

                            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return TagReport;
            }

        public List<CaseNote> GetCaseNotesByTagId(long tagid, int mode)
        {
            var NoteList = new List<CaseNote>();

            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@TagId", tagid));
                    command.Parameters.Add(new SqlParameter("@Mode", mode)); //2
                    command.Parameters.Add(new SqlParameter("@PageSize", 1));
                    command.Parameters.Add(new SqlParameter("@PageNo", 1));
                    command.Parameters.Add(new SqlParameter("@Search", ""));
                    command.Parameters.Add(new SqlParameter("@Sortclmn", ""));
                    command.Parameters.Add(new SqlParameter("@Sortdir", ""));


                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_CNTagReportDetails";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                    if (_dataset != null && _dataset.Tables.Count > 0)
                    {

                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                var _cn = new CaseNote()
                                {
                                    CaseNoteid = dr["CaseNoteID"].ToString(),
                                    Name = dr["Title"].ToString(),
                                    Date = dr["CaseNoteDate"].ToString(),
                                    WrittenBy= dr["Name"].ToString(),
                                    //Tagname=
                                };

                                NoteList.Add(_cn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return NoteList;

        }


        public bool EditDeleteCNTag(long tagid, string tagname, int mode,long ExistsTagId, ref long availableTag)
        {
            var success = false;

            try
            {
                StaffDetails staff = StaffDetails.GetInstance();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@TagId", tagid));
                    command.Parameters.Add(new SqlParameter("@Mode", mode));
                    command.Parameters.Add(new SqlParameter("@TagName", tagname));
                    command.Parameters.Add(new SqlParameter("@ExistsTagId", ExistsTagId)); 
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_ModifyCNTag";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                    if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0)
                    {

                        success = Convert.ToBoolean( _dataset.Tables[0].Rows[0]["Result"].ToString());
                    }

                    if (!success && _dataset.Tables.Count > 1 && _dataset.Tables[1].Rows.Count > 0)
                    { //if tagname available

                        availableTag = Convert.ToInt64(_dataset.Tables[1].Rows[0]["TagKey"].ToString());
                    }


                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return success;

        }


        #endregion CaseNote_Tag_Report


        #region TimeLine


        public Clientprofile GetClientDetails(long clientid)
        {
            var result = new Clientprofile();
            try
            {


                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {

                    command.Connection = Connection;
                    command.Parameters.Clear();
                    // command.CommandText = "select Name=(Firstname+' '+Lastname),DOB=(convert(varchar(10),DOB,101)),ProfilePic from Client where ClientID =" + clientid + "";
                    //command.CommandText = "select Name=(Firstname+' '+Lastname),DOB=(convert(varchar(10),DOB,101)),ProfilePic," +
                    //     "ED.ProgramID,PD.ProgramType from Client C inner join EnrollmentDetail ED" +
                    //     " on C.ClientID = ED.ClientID and C.Status = 1 and ED.Status in (0,1,4,5) and ED.IsActive = 0" +
                    //     " inner join ProgramDetails PD on PD.ProgramTypeID = ED.ProgramID where ED.ClientID = " + clientid + " ";

                    string query=  @"select Name = (Firstname + ' ' + Lastname), DOB = (convert(varchar(10), DOB, 101)), ProfilePic,
                         ED.ProgramID,PD.ProgramType from Client C inner join EnrollmentDetail ED
                          on C.ClientID = ED.ClientID and C.Status = 1 and ED.Status in (0,1,4,5) and ED.IsActive = 0
                          inner join ProgramDetails PD on PD.ProgramTypeID = ED.ProgramID 
where ED.ClientID = {0} ; select PD.ProgramType,Ed.ProgramID from Client C
inner join EnrollmentDetail ED on C.ClientID = ED.ClientID and C.Status = 1
 inner join
ProgramDetails PD on PD.ProgramTypeID = ED.ProgramID
inner join ReferenceProgram RP on RP.Name = PD.ProgramType
 where ED.ClientID = {1}
group by PD.ProgramType, ED.ProgramID
";
                    command.CommandText = string.Format(query, clientid, clientid);

                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                    if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0)
                    {
                        var dr = _dataset.Tables[0].Rows[0];
                        result.ChildName = dr["Name"].ToString();
                        result.DOB = dr["DOB"].ToString();
                        result.Profilepic = dr["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePic"]);
                        result.ProgramType = dr["ProgramType"].ToString();

                    }

                    if (_dataset != null && _dataset.Tables.Count > 1 && _dataset.Tables[1].Rows.Count > 0)
                    {
                        result.ProgramHistroy = new List<string>();

                        foreach (DataRow dr1 in _dataset.Tables[1].Rows) { 

                            result.ProgramHistroy.Add( dr1["ProgramType"].ToString());
                        }
                    }


                    }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return result;
                }

        public List<ClientTimeLineModel> GetClientTimeLine(long clientid,int Mode,string stepIds)
        {

            var result = new List<ClientTimeLineModel>();

            try
            {


                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@Mode", Mode));
                    command.Parameters.Add(new SqlParameter("@Clientid", clientid));
                    command.Parameters.Add(new SqlParameter("@StepIds", stepIds)); 
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_TimeLineDetails";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                    if (_dataset != null && _dataset.Tables.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            if (Mode == 1)
                            {

                                var row = new ClientTimeLineModel()
                                {
                                    TimeLineId = Convert.ToInt64(dr["Id"].ToString()),
                                    StepType = Convert.ToInt64(dr["StepType"].ToString()),
                                    StepName = dr["StepName"].ToString(),
                                    EventBodyJson = dr["EventBodyJson"].ToString(),
                                    ActiveProgramYear = dr["ActiveProgramYear"].ToString(),
                                    ClientId = dr["ClientId"].ToString(),
                                    CreatedDate = dr["CreatedDate"].ToString(),
                                    EventCreatedDate = dr["EventCreatedDate"].ToString(),
                                    EventDate = dr["EventDate"].ToString(),
                                    EventId = dr["EventId"].ToString(),
                                    EventRole = dr["EventRole"].ToString(),
                                    EventTime = dr["EventTime"].ToString(),
                                    ModifiedDate = dr["ModifiedDate"].ToString(),
                                    Status = dr["Status"].ToString(),

                                };

                               // var devId = new long[] {2,5,6,8,9 };
                                //if (devId.Contains(row.StepType)) {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(row.EventBodyJson);

                                    row.EventBodyJson =  JsonConvert.SerializeXmlNode(doc);
                               // }

                                result.Add(row);
                            }
                            else if (Mode == 2)
                            {

                                var row = new ClientTimeLineModel()
                                {
                                    StepType = Convert.ToInt64(dr["StepId"].ToString()),
                                    StepName = dr["StepName"].ToString(),
                                    Status = dr["Status"].ToString(),

                                };
                                result.Add(row);
                            }
                           
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }


            return result;
        }


        public string GetChildofTheDay()
        {
            string _child = "";

            try
            {


                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {

                    command.Connection = Connection;
                    command.Parameters.Clear();
                   // command.Parameters.Add(new SqlParameter("@Clientid", 1));
                    command.Parameters.Add(new SqlParameter("@AgencyId", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@Role", staff.RoleId));
                    //command.Parameters.Add(new SqlParameter("@Mode", 3));
                   
                   // command.Parameters.Add(new SqlParameter("@StepIds", ""));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_ChildofTheDayDetails";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();

                    if (_dataset != null && _dataset.Tables.Count > 0)
                    {

                        if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0)
                        {
                            _child = EncryptDecrypt.Encrypt64(_dataset.Tables[0].Rows[0]["ClientId"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


                        return _child;
        }
        #endregion TimeLine


        #region Case notes for attendance issue


        public CaseNoteByClientID GetDevelopmentalMembersByClientID(long clientId, StaffDetails staffDetails, int yakkrcode = 0)
        {

            CaseNoteByClientID clientCaseNote = new CaseNoteByClientID();
            try
            {

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@ClientID", clientId));
                    command.Parameters.Add(new SqlParameter("@YakkrCode", yakkrcode));
                    command.CommandText = "USP_GetDevelopmentalTeam_HouseholdByClient";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = Connection;
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                }

                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        clientCaseNote.CaseNoteList = (from DataRow dr0 in _dataset.Tables[0].Rows

                                                       select new CaseNote
                                                       {
                                                           clientid = EncryptDecrypt.Encrypt64(Convert.ToString(dr0["ClientID"])),
                                                           Name = Convert.ToString(dr0["ClientName"]),
                                                           Householid = EncryptDecrypt.Encrypt64( Convert.ToString(dr0["HouseholdID"])),
                                                           Date = Convert.ToString(dr0["CaseNoteDate"]),
                                                           ProgramID = EncryptDecrypt.Encrypt64(Convert.ToString(dr0["ProgramID"])),
                                                           ProgramType=Convert.ToString(dr0["ProgramType"]),
                                                           CenterId= EncryptDecrypt.Encrypt64(Convert.ToString(dr0["CenterID"])),
                                                           Classroomid= EncryptDecrypt.Encrypt64(Convert.ToString(dr0["ClassroomID"]))
                                                          
                                                           
                                                       }

                                         ).ToList();
                    }

                    if (_dataset.Tables.Count > 1 && _dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        clientCaseNote.UserList = (from DataRow dr1 in _dataset.Tables[1].Rows
                                                   select new RosterNew.User
                                                   {
                                                       Id = Convert.ToString(dr1["UserID"]),
                                                       Name = String.Concat(Convert.ToString(dr1["StaffName"]), " ", "(", Convert.ToString(dr1["RoleName"]), ")").ToString()
                                                   }

                                                     ).ToList();
                    }

                    if (_dataset.Tables.Count > 2 && _dataset.Tables[2].Rows.Count > 0)
                    {
                        clientCaseNote.Clientlist = (from DataRow dr2 in _dataset.Tables[2].Rows
                                                     //where dr2["ClientID"].ToString() == Convert.ToString(clientId)
                                                     select new RosterNew.User
                                                     {
                                                         Id = Convert.ToString(dr2["clientID"]),
                                                         Name = Convert.ToString(dr2["Name"])
                                                     }

                                                     ).ToList();
                    }


                    if (_dataset.Tables.Count > 3 && _dataset.Tables[3].Rows.Count > 0)
                    {




                        clientCaseNote.ParentContactInfoList = (from DataRow dr3 in _dataset.Tables[3].Rows

                                                                orderby dr3["ParentName"].ToString()

                                                                select new ParentInfo
                                                                {
                                                                    ParentName = DBNull.Value == dr3["ParentName"] ? string.Empty : Convert.ToString(dr3["ParentName"]),
                                                                    PhoneNo = Convert.ToString(dr3["PhoneNumber"]),
                                                                    EmailId = DBNull.Value == dr3["EmailId"] ? string.Empty : Convert.ToString(dr3["EmailId"])

                                                                }


                                                       ).Distinct().ToList();






                        //clientCaseNote.ParentContactInfoList = (from DataRow dr3 in _dataset.Tables[3].Rows 
                        //                                        join parent in parentIdList on dr3["ParentID"].ToString() equals parentIdList.ToString()
                        //                                        select new ParentInfo
                        //                                        {
                        //                                            ParentName = DBNull.Value == dr3["ParentName"] ? string.Empty : Convert.ToString(dr3["ParentName"]),
                        //                                            PhoneNo = string.Join(",",(from DataRow _dr4 in _dataset.Tables[3].Rows
                        //                                                       where DBNull.Value != _dr4["PhoneNo"] && Convert.ToString(_dr4["PhoneNo"]) != string.Empty
                        //                                                       && Convert.ToString(_dr4["ParentID"]) ==Convert.ToString(dr3["ParentID"])

                        //                                                       select (Convert.ToString(_dr4["PhoneNo"])
                        //                                                       ).ToArray())),
                        //                                            EmailId=DBNull.Value==dr3["EmailId"]?string.Empty :Convert.ToString(dr3["EmailId"])




                        //                                        }
                        //                                      ).ToList();


                    }


                    //Reference YAkkr Code//
                    if(yakkrcode>0 && _dataset.Tables.Count>4 && _dataset.Tables[4]!=null && _dataset.Tables[4].Rows.Count>0)
                    {
                        clientCaseNote.ReferenceYakkrID = EncryptDecrypt.Encrypt64(_dataset.Tables[4].Rows[0]["YakkrID"].ToString());
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
                DataAdapter.Dispose();
                _dataset.Dispose();
            }

            return clientCaseNote;
        }



        #endregion


        #region caseNotes List for Attendance issue

        public void GetAttendanceIssueCaseNoteList(ref CaseNoteByClientID caseNoteByClient, StaffDetails staffDetails, string clientID)
        {


            try
            {

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@ClientID", EncryptDecrypt.Decrypt64(clientID)));
                    command.Parameters.Add(new SqlParameter("@YakkrCode", 601));
                    command.Parameters.Add(new SqlParameter("@take", caseNoteByClient.PageSize));
                    command.Parameters.Add(new SqlParameter("@Skip", caseNoteByClient.SkipRows));
                    command.Connection = Connection;
                    command.CommandText = "USP_GetCaseNotesByYakkr";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);

                }

                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                    {


                        caseNoteByClient.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        caseNoteByClient.CaseNoteList = (from DataRow dr0 in _dataset.Tables[0].Rows
                                                         select new CaseNote
                                                         {
                                                             Householid = Convert.ToString(dr0["HouseholdID"]),
                                                             CaseNoteid = Convert.ToString(dr0["CaseNoteID"]),
                                                             Title = Convert.ToString(dr0["Title"]),
                                                             Date = Convert.ToString(dr0["CaseNoteDate"]),
                                                             BY = Convert.ToString(dr0["By"]),
                                                             Attachment = Convert.ToString(dr0["Attachment"]),
                                                             References = Convert.ToString(dr0["References"]),
                                                             SecurityLevel = Convert.ToBoolean(dr0["SecurityLevel"]),
                                                             WrittenBy = Convert.ToString(dr0["WrittenBy"]),
                                                             IsAllowSecurityCN = Convert.ToBoolean(dr0["IsAllowSecurityCN"]),
                                                             IsEditable = Convert.ToBoolean(dr0["Editable"]),
                                                             clientid= clientID
                                                         }
                                                        ).ToList();
                    }

                    if (_dataset.Tables.Count > 1 && _dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {

                        caseNoteByClient.Clientlist = (from DataRow dr1 in _dataset.Tables[1].Rows
                                                       select new RosterNew.User
                                                       {
                                                           Id = Convert.ToString(dr1["ClientID"]),
                                                           Name = Convert.ToString(dr1["Name"])
                                                       }
                                                     ).ToList();
                    }

                    if(_dataset.Tables.Count>2 && _dataset.Tables[2]!=null && _dataset.Tables[2].Rows.Count>0)
                    {
                        caseNoteByClient.UserList = (from DataRow dr2 in _dataset.Tables[2].Rows
                                                     select new RosterNew.User
                                                     {
                                                         Id = Convert.ToString(dr2["UserID"]),
                                                         Name = String.Concat(dr2["Name"].ToString(), " ", "(", dr2["RoleName"].ToString(), ")")
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
                Connection.Dispose();
                command.Dispose();
                _dataset.Dispose();
                DataAdapter.Dispose();
            }

        }

        #endregion


        #region Reset Attendance issues


        public bool ResetExcessiveAbsence(StaffDetails staff,int clientId, int yakkrId)
        {

            bool isRowsAffected = false;
            try
            {
                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@ClientID", clientId));
                    command.Parameters.Add(new SqlParameter("@YakkrID", yakkrId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_ResetAttendanceIssues";
                    Connection.Open();
                    isRowsAffected= Convert.ToBoolean(command.ExecuteNonQuery());
                }
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
                isRowsAffected = false;
            }
            return isRowsAffected;
        }

        #endregion

    }
}
