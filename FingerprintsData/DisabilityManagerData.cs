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
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;
using System.Drawing;


namespace FingerprintsData
{
    public class DisabilityManagerData
    {

        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        //SqlDataReader dataReader = null;
        //SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        //DataTable familydataTable = null;
        DataSet _dataset = null;
        //DataTable _dataTable = null;
        public List<DissabilityManagerDashboard> GetDissabilityStaffDashboard(ref DisabilityCumulative DisabilityCumulative ,ref int yakkrcount, ref int appointment, string Agencyid, string userid)
        {
            List<DissabilityManagerDashboard> centerList = new List<DissabilityManagerDashboard>();
            List<HrCenterInfo> centerList1 = new List<HrCenterInfo>();

            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[SP_DissabilityStaffDashboard]";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            DissabilityManagerDashboard info = new DissabilityManagerDashboard();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["center"].ToString());
                            info.Name = dr["centername"].ToString();
                            info.TotalChildren = dr["totalchildren"].ToString();
                            info.DisabilityPercentage = dr["TotalDisablePercentage"].ToString();
                            info.Indicated = dr["Indicated"].ToString();
                            info.Pending = (Convert.ToString(dr["pending"].ToString()));// + Convert.ToInt32(dr["WithdrawnPending"].ToString())).ToString();
                            info.Indicated = (Convert.ToString(dr["Indicated"].ToString()));// + Convert.ToInt32(dr["WithdrawnIndicated"].ToString())).ToString();
                            info.Qualified = dr["Qualified"].ToString();
                            info.Released = dr["Released"].ToString();
                            info.Completed = dr["Completed"].ToString();
                            centerList.Add(info);
                        }
                    }

                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        DisabilityCumulative.TotalAcivePercent = Convert.ToDouble(_dataset.Tables[1].Rows[0]["ActivePercent"]);
                        DisabilityCumulative.TotalActiveClient = Convert.ToInt32(_dataset.Tables[1].Rows[0]["TotalActive"]);
                        DisabilityCumulative.TotalCumulativeClient = Convert.ToInt32(_dataset.Tables[1].Rows[0]["TotalCumulative"]);
                        DisabilityCumulative.TotalCumulativePercent = Convert.ToDouble(_dataset.Tables[1].Rows[0]["cumulativePercent"]);
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

        public List<DissabilityManagerDashboard> GetDissabilityManagerDashboard(ref DisabilityCumulative DisabilityCumulative, ref int yakkrcount, ref int appointment, string Agencyid, string userid)
        {
            List<DissabilityManagerDashboard> centerList = new List<DissabilityManagerDashboard>();
            List<HrCenterInfo> centerList1 = new List<HrCenterInfo>();
         
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[SP_DissabilityManagerDashboard]";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            DissabilityManagerDashboard info = new DissabilityManagerDashboard();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["center"].ToString());
                            info.Name = dr["centername"].ToString();
                            info.TotalChildren = dr["totalchildren"].ToString();
                            info.DisabilityPercentage = dr["TotalDisablePercentage"].ToString();
                            info.Indicated = dr["Indicated"].ToString();
                            info.Pending = (Convert.ToString(dr["pending"].ToString()));// + Convert.ToInt32(dr["WithdrawnPending"].ToString())).ToString();
                            info.Indicated = (Convert.ToString(dr["Indicated"].ToString()));// + Convert.ToInt32(dr["WithdrawnIndicated"].ToString())).ToString();
                            info.Qualified = dr["Qualified"].ToString();
                            info.Released = dr["Released"].ToString();
                            info.Completed = dr["Completed"].ToString();
                            centerList.Add(info);
                        }
                    }

                    if (_dataset.Tables[1]!=null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        DisabilityCumulative.TotalAcivePercent = Convert.ToDouble(_dataset.Tables[1].Rows[0]["ActivePercent"]);
                        DisabilityCumulative.TotalActiveClient = Convert.ToInt32(_dataset.Tables[1].Rows[0]["TotalActive"]);
                        DisabilityCumulative.TotalCumulativeClient = Convert.ToInt32(_dataset.Tables[1].Rows[0]["TotalCumulative"]);
                        DisabilityCumulative.TotalCumulativePercent =Convert.ToDouble( _dataset.Tables[1].Rows[0]["cumulativePercent"]);
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


        public Roster GetAllRoster(string centerid, string Classroom, string userid, string agencyid, string RoleId)
        {
            List<Roster> RosterList = new List<Roster>();
            Roster _roster = new Roster();
            List<ClassRoom> classList = new List<ClassRoom>();
            try
            {

                command.Parameters.Add(new SqlParameter("@Classroom", Classroom));
                command.Parameters.Add(new SqlParameter("@Center", EncryptDecrypt.Decrypt64(centerid)));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleId", RoleId));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[SP_RosterListAll]";
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
                            info.DOB = Convert.ToDateTime(dr["dob"]).ToString("MM/dd/yyyy");
                            info.Gender = dr["gender"].ToString();
                            info.CenterName = dr["CenterName"].ToString();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString());
                            info.ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString());
                            info.ClassroomName = dr["ClassroomName"].ToString();
                            info.FSW = dr["fswname"].ToString();
                            info.Teacher = dr["teacher"].ToString();//DBNull.Value.Equals(dr["ChildTransport"])
                            info.IsPresent = DBNull.Value.Equals(dr["IsPresent"]) ? 0 : Convert.ToInt32(dr["IsPresent"]);//.ToString() //Added on 30Dec2016
                            info.RosterYakkr= DBNull.Value==(dr["yakkrcode"]) ? "0" : Convert.ToString(dr["yakkrcode"]);//.ToString() //Added on 30Dec2016
                            info.classroomid = DBNull.Value == (dr["classroomid"]) ? "0" : Convert.ToString(dr["classroomid"]);//.ToString() //Added on 30Dec2016
                            info.IsWithdrawn = dr["status"].ToString() == "3" ? 1 : 0;

                            //  info.Dayscount = dr["dayscount"].ToString();
                            //  info.Picture = dr["ProfilePic"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr["ProfilePic"]);
                            info.District = Convert.ToString(dr["District"]);
                            RosterList.Add(info);


                        }
                        _roster.Rosters = RosterList;//Changes on 29Dec2016
                    }
                }
                //Changes on 29Dec2016
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    ClassRoom info = null;
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        info = new ClassRoom();
                        info.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                        info.ClassName = dr["ClassroomName"].ToString();
                        classList.Add(info);
                    }
                    _roster.ClassroomsNurse = classList;
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
            return _roster;

        }

        public Roster GetPendingDisableRoster(string centerid, string Classroom, string userid, string agencyid, string RoleId, string Mode, string sortOrder, string sortDirection,string clientId)
        {
            List<Roster> RosterList = new List<Roster>();
            Roster _roster = new Roster();
            List<ClassRoom> classList = new List<ClassRoom>();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Classroom", Classroom));
                command.Parameters.Add(new SqlParameter("@Center", EncryptDecrypt.Decrypt64(centerid)));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
                command.Parameters.Add(new SqlParameter("@Mode", Mode));
                command.Parameters.Add(new SqlParameter("@sortDirection", sortDirection));
                command.Parameters.Add(new SqlParameter("@ClientId", clientId)); 
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_DisableRosterList";
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
                            info.DOB = dr["dob"].ToString();
                            info.Gender = dr["gender"].ToString();
                            info.CenterName = dr["CenterName"].ToString();
                            info.CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString());
                            info.ProgramId = EncryptDecrypt.Encrypt64(dr["programid"].ToString());
                            info.ClassroomName = dr["ClassroomName"].ToString();
                            info.FSW = dr["fswname"].ToString();
                            info.Teacher = dr["teacher"].ToString();//DBNull.Value.Equals(dr["ChildTransport"])
                            info.IsPresent = DBNull.Value.Equals(dr["IsPresent"]) ? 0 : Convert.ToInt32(dr["IsPresent"]);//.ToString() //Added on 30Dec2016
                            //  info.Dayscount = dr["dayscount"].ToString();
                            info.IsWithdrawn = dr["status"].ToString() == "3" ? 1 : 0;
                            info.District = Convert.ToString(dr["District"]);
                            info.classroomid = dr["classroomid"].ToString();
                            if (dr["recordCreated"] != DBNull.Value)
                            {
                                info.recordCreated = dr["recordCreated"].ToString();
                                DateTime dt = Convert.ToDateTime(info.recordCreated);
                                TimeSpan tt = DateTime.Now.Subtract(dt);
                                info.totalday = tt.Days.ToString();
                            }
                            else
                            {
                                info.recordCreated = "";
                            }
                            //info.recordCreated = dr["recordCreated"].ToString();

                            if (Mode == "Qualified")
                            {
                                info.SpecialService = string.IsNullOrEmpty(dr["SpecialServices"].ToString()) ? "" : dr["SpecialServices"].ToString();
                            }
                            RosterList.Add(info);

                        }
                        _roster.Rosters = RosterList;//Changes on 29Dec2016
                    }
                }
                //Changes on 29Dec2016
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    ClassRoom info = null;
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        info = new ClassRoom();
                        info.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                        info.ClassName = dr["ClassroomName"].ToString();
                        classList.Add(info);
                    }
                    _roster.ClassroomsNurse = classList;
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
            return _roster;

        }

        public Roster GetDisableNotesList(string agencyid, string ClientId)
        {
            List<Roster> RosterList = new List<Roster>();
            Roster _roster = new Roster();
            List<ClassRoom> classList = new List<ClassRoom>();
            List<DisableNotes> disablenotes = new List<DisableNotes>();
            _roster.disablenotes = new List<DisableNotes>();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64(ClientId)));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "DisableNotesList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                 
                        _roster.disablenotes = new List<DisableNotes>();
                        if (_dataset.Tables[0] != null)
                        {
                            if (_dataset.Tables[0].Rows.Count > 0)
                            {
                                _roster.disablenotes = (from DataRow dr5 in _dataset.Tables[0].Rows
                                                        select new DisableNotes
                                                        {
                                                            Name = Convert.ToString(dr5["Firstname"]),
                                                             Notes = Convert.ToString(dr5["Notes"]),
                                                             DisableDocumentName = Convert.ToString(dr5["DisableDocumentName"]),
                                                             DocumentDescription = Convert.ToString(dr5["DocumentDescription"]),
                                                             noteid = Convert.ToString(dr5["DisableNotesId"]),
                                                             Createdon = Convert.ToString(dr5["DateEntered"]),
                                                             DisabilityTypeID = Convert.ToString(dr5["DisablitiesTypeId"]),
                                                             YakkrId = Convert.ToInt32(dr5["YakkrId"]),
                                                             SpecialServiceDisability = Convert.ToString(dr5["ReceivedServicesId"]),
                                                             PrimaryDisability = int.Parse(Convert.ToString(dr5["PrimaryDisability"])),
                                                           }).ToList();

                            }
                        }

                        DisableNotes notes = new DisableNotes();
                        int primaryDisability = 0;
                        string disabilityType = _roster.disablenotes.Select(x => x.DisabilityTypeID).Where(x => !string.IsNullOrEmpty(x)).ToList().FirstOrDefault();

                         primaryDisability = _roster.disablenotes.Select(x => x.PrimaryDisability).Where(x =>x>0).ToList().FirstOrDefault();

                        //notes.NotesList = _roster.disablenotes.Where(x => x.Notes != string.Empty).Select(x => new DisableNotes
                        //{
                        //    Notes = x.Notes,
                        //    noteid = x.noteid,
                        //    Createdon = x.Createdon,
                        //    YakkrId = x.YakkrId,
                        //    SpecialServiceDisability = x.SpecialServiceDisability,
                        //    DisabilityTypeID = disabilityType,
                        //    Name = x.Name,
                        //    PrimaryDisability= primaryDisability
                        //}).Distinct().ToList();


                        notes.NotesList = _roster.disablenotes.Where(x => x.Notes != string.Empty || !string.IsNullOrEmpty(x.DisabilityTypeID)).Select(x => new DisableNotes
                        {
                            Notes = x.Notes,
                            noteid = x.noteid,
                            Createdon = x.Createdon,
                            YakkrId = x.YakkrId,
                            SpecialServiceDisability = x.SpecialServiceDisability,
                            DisabilityTypeID = disabilityType,
                            Name = x.Name,
                            PrimaryDisability = primaryDisability
                        }).Distinct().ToList();



                        notes.DocumentList = _roster.disablenotes.Select(x => new DocumentInformation
                        {
                            DisableDocumentName = x.DisableDocumentName,
                            DocumentDescription = x.DocumentDescription,
                            NotesId = x.noteid,
                            CreatedOn = x.Createdon


                        }).Where(x => !string.IsNullOrEmpty(x.DisableDocumentName)).ToList();

                        disablenotes = new List<DisableNotes>();
                        disablenotes.Add(notes);
                        _roster.disablenotes = disablenotes;

                        //List<int> YakkrIdList = _roster.disablenotes.Select(x => x.YakkrId).Distinct().ToList();

                        //foreach (int yakkrid in YakkrIdList)
                        //{
                        //    var list3 = _roster.disablenotes.Where(x => x.YakkrId == yakkrid).ToList();
                        //    var list2 = list3.Select(x => new DisableNotes
                        //    {
                        //        Name = x.Name,
                        //        Notes = x.Notes,
                        //        Createdon = x.Createdon,
                        //        DisabilityTypeID = x.DisabilityTypeID,
                        //        YakkrId = x.YakkrId,
                        //        SpecialServiceDisability = x.SpecialServiceDisability,
                        //        DocumentList = list3.Select(y => new DocumentInformation
                        //        {
                        //            NotesId = y.noteid,
                        //            DocumentDescription = y.DocumentDescription,
                        //            DisableDocumentName = y.DisableDocumentName
                        //        }).ToList()
                        //    }).Distinct().ToList();

                        //    disablenotes.Add(list2[0]);

                        //}

                        // _roster.disablenotes = disablenotes;


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
            return _roster;

        }

        //public Roster GetDisableDocumentsList(string agencyid, string ClientId)
        //{
        //    List<Roster> RosterList = new List<Roster>();
        //    Roster _roster = new Roster();
        //    List<ClassRoom> classList = new List<ClassRoom>();
        //    List<DisableNotes> disablenotes = new List<DisableNotes>();
        //    try
        //    {
        //        command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
        //        command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64(ClientId)));
        //        command.Connection = Connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "DisableDocumentsList";
        //        DataAdapter = new SqlDataAdapter(command);
        //        _dataset = new DataSet();
        //        DataAdapter.Fill(_dataset);
        //        if (_dataset.Tables[0] != null)
        //        {
        //            if (_dataset.Tables[0].Rows.Count > 0)
        //            {
        //                DisableNotes info = null;
        //                foreach (DataRow dr in _dataset.Tables[0].Rows)
        //                {
        //                    info = new DisableNotes();
        //                    info.noteid = dr["DisableNotesId"].ToString(); ;
        //                    info.Name = dr["Firstname"].ToString();
        //                    info.DisableDocumentName = dr["DisableDocumentName"].ToString();
        //                    info.DocumentDescription = dr["DocumentDescription"].ToString();
        //                    disablenotes.Add(info);
        //                }
        //                _roster.disablenotes = disablenotes;//Changes on 29Dec2016
        //            }
        //        }

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
        //    return _roster;

        //}
        public DissabilityManagerDashboard BindDisableTypeByClient(string clientid)
        {
            DissabilityManagerDashboard _dissabilityManagerDashboard = new DissabilityManagerDashboard();
            List<DisablilityType> _DisablilityType = new List<DisablilityType>();
            try
            {
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ClientId", EncryptDecrypt.Decrypt64(clientid));
                command.CommandText = "[SP_DisablitiesTypeByClientId]";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        DisablilityType info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new DisablilityType();
                            info.Id = Convert.ToInt32(dr["ID"]);
                            info.DisabilityType = dr["DisablitiesType"].ToString();
                            info.IsChecked = Convert.ToBoolean(Convert.ToInt32(dr["ischecked"]));
                            info.PrimaryTypeId = Convert.ToInt32(dr["PrimaryDisability"]);
                            
                            _DisablilityType.Add(info);

                        }
                        _dissabilityManagerDashboard.disabilitytype = _DisablilityType;//Changes on 29Dec2016
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
            return _dissabilityManagerDashboard;

        }

        public DissabilityManagerDashboard BindDisableType()
        {
            DissabilityManagerDashboard _dissabilityManagerDashboard = new DissabilityManagerDashboard();
            List<DisablilityType> _DisablilityType = new List<DisablilityType>();
            try
            {
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[SP_DisablitiesTypeDropdownList]";                
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        DisablilityType info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new DisablilityType();
                            info.Id = Convert.ToInt32(dr["ID"]);
                            info.DisabilityType = dr["DisablitiesType"].ToString();
                            //info.Notes = dr["Notes"].ToString();

                            _DisablilityType.Add(info);

                        }
                        _dissabilityManagerDashboard.disabilitytype = _DisablilityType;//Changes on 29Dec2016
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
            return _dissabilityManagerDashboard;

        }

        public bool SaveDisabilityTypes(string clientid, string distypes,string primarydisablity)
        {
            bool result = false;
            try
            {
              
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveDisablityTypes";
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Clientid", EncryptDecrypt.Decrypt64(clientid)));
                command.Parameters.Add(new SqlParameter("@DisablityTypes", distypes));
                command.Parameters.Add(new SqlParameter("@PrimaryDisabilityType", primarydisablity));              
             
                int res= command.ExecuteNonQuery();
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


        public string SavePendingDisableUseInfo(string Clientid, string ClassroomID, string centerid
         , string StartDate, string agencyid, string userid, string Programid, string Notes, string Mode, DataTable documentTable, string disabilitytype, string ddlqualifiedreleased, string DocumentDate, string txtdocdesc, string recService = "",string primaryDistype="",bool IsCompleted=false
            ,string IsWithdrawn="")
        {
            try
            {
                primaryDistype = (string.IsNullOrEmpty(primaryDistype)) ? "0" : primaryDistype;

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                //if(IsWithdrawn=="1")
                //{
                //    command.CommandText = "SP_SaveWithdrawnDisableClient";
                //}
                //else
                //{
                    command.CommandText = "SaveYakarPendingDisable";
                //}
               
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@Clientid", EncryptDecrypt.Decrypt64(Clientid)));
                command.Parameters.Add(new SqlParameter("@ClassroomID", ClassroomID));
                command.Parameters.Add(new SqlParameter("@centerid", EncryptDecrypt.Decrypt64(centerid)));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@CreatedBy", userid));
                command.Parameters.Add(new SqlParameter("@Notes", Notes));
                command.Parameters.Add(new SqlParameter("@DocumentDate", DocumentDate));
                command.Parameters.Add(new SqlParameter("@Programid", EncryptDecrypt.Decrypt64(Programid)));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters.Add(new SqlParameter("@DocumentDescription", txtdocdesc));
                command.Parameters.Add(new SqlParameter("@ReceivedService", recService));
                command.Parameters.Add(new SqlParameter("@IsCompleted", IsCompleted));
                command.Parameters.Add(new SqlParameter("@PrimaryDisabilityType", primaryDistype));

                if (Mode == "QualifyReleased")
                {
                    Mode = ddlqualifiedreleased;
                }
                if (Mode == "")
                {
                    Mode = "Qualified";
                }
                command.Parameters.Add(new SqlParameter("@Mode", Mode));
                //if (FileUpload1 != null)
                //{
                //    //command.Parameters.Add(new SqlParameter("@DisableDocumentName", FileUpload1.FileName));
                //    //command.Parameters.Add(new SqlParameter("@DisableDocument", new BinaryReader(FileUpload1.InputStream).ReadBytes(FileUpload1.ContentLength)));
                //}

                //if (documentTable != null)
                //{
                //    if (documentTable.Rows.Count > 0)
                //    {
                command.Parameters.Add(new SqlParameter("@DisableDocumentTable", documentTable));
                //}

                command.Parameters.Add(new SqlParameter("@DisablitiesTypeID", disabilitytype));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
                //}
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
            return "0";
        }

        public List<Fswuserapproval> Getallclients(string Agencyid, string userid, string centerid = null)
        {
            List<Fswuserapproval> FswuserapprovalList = new List<Fswuserapproval>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                // command.Parameters.Add(new SqlParameter("@centerid", centerid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[SP_DisableStaffYakkrDetails]";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        Fswuserapproval info = new Fswuserapproval();
                        info.ClientId = dr["ClientId"].ToString();
                        info.ClientName = dr["Name"].ToString();
                        info.Date = Convert.ToDateTime(dr["DateEntered"]).ToString("MM/dd/yyyy");
                        info.CenterName = dr["centername"].ToString();
                        info.StaffName = dr["staffname"].ToString();
                        info.routecode = dr["RouteCode"].ToString();
                        info.Status = dr["Status"].ToString();
                        info.YakkrID = dr["yakkrid"].ToString();
                        info.CenterId = centerid;
                        FswuserapprovalList.Add(info);
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
            return FswuserapprovalList;
        }
        public List<HrCenterInfo> Getallcenter(string mode, string RoleId, string Agencyid, string userid)
        {
            List<HrCenterInfo> centerList = new List<HrCenterInfo>();
            try
            {
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getallcnterforfsw";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        HrCenterInfo info = new HrCenterInfo();
                        info.CenterId = dr["center"].ToString();
                        info.Name = dr["centername"].ToString();
                        info.Address = dr["address"].ToString();
                        info.Zip = dr["Zip"].ToString();
                        info.SeatsAvailable = dr["AvailSeats"].ToString();
                        centerList.Add(info);
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

        public DissabilityManagerDashboard getDisableDocument(string Clientid)
        {
            DissabilityManagerDashboard obj = new DissabilityManagerDashboard();
            //obj.income1 = new FamilyHousehold.calculateincome();

            try
            {
                command.Parameters.Add(new SqlParameter("@DisableNotesId", Clientid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[getDisableDocument]";
                DataAdapter = new SqlDataAdapter(command);
                //Due to Phone Type
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    obj.EImageByte = (byte[])_dataset.Tables[0].Rows[0]["DisableDocument"];
                    obj.EFileExtension = _dataset.Tables[0].Rows[0]["DisableDocumentName"].ToString();
                    obj.EFileName = _dataset.Tables[0].Rows[0]["DisableDocumentName"].ToString();
                }
                DataAdapter.Dispose();
                command.Dispose();
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
            }
        }

        public Role GetInternalRefDetails(string ClientId)
        {
            Role role = new Role();
            role.RoleList = new List<Role>();
            role.ClientList = new List<RosterNew.User>();
            StaffDetails staff = StaffDetails.GetInstance();
            try
            {
                command.Parameters.Add(new SqlParameter("@agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@ClientId", EncryptDecrypt.Decrypt64(ClientId)));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[SP_GetInternalRefDetails]";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0]!=null)
                {

                    role.RoleList = (from DataRow dr in _dataset.Tables[0].Rows
                                     select new Role
                                     {
                                         RoleId = dr["RoleId"].ToString(),
                                         RoleName = dr["RoleName"].ToString()
                                     }).ToList();


                }
                if ( _dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count>0)
                {

                    role.AssignedRoles=String.Join("," ,(from DataRow dr in _dataset.Tables[1].Rows
                                select  dr["RoleName"].ToString() ).ToList());


                }
                if (_dataset != null && _dataset.Tables[2].Rows.Count > 0)
                {
                   FingerprintsModel.RosterNew.User obj = null;
                    foreach (DataRow dr in _dataset.Tables[2].Rows)
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


        public bool SaveInternalReferral(InternalReferral internRef,string casenoteid)
        {
            bool result = false;
            try
            {
                StaffDetails staff = new StaffDetails();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveInternalReferral";
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Clientid", EncryptDecrypt.Decrypt64( internRef.CaseClientId)));
                command.Parameters.Add(new SqlParameter("@CenterId", EncryptDecrypt.Decrypt64(internRef.CaseCenterId)));
               command.Parameters.Add(new SqlParameter("@ClassroomId", (internRef.CaseClassroomId)));
                command.Parameters.Add(new SqlParameter("@ProgramId", EncryptDecrypt.Decrypt64((internRef.CaseProgramId))));
                command.Parameters.Add(new SqlParameter("@SelectedRole", (internRef.RoleId)));
                command.Parameters.Add(new SqlParameter("@CaseNoteId", casenoteid));
                command.Parameters.Add(new SqlParameter("@YakkrCode", internRef.YakkrCode));
                command.Parameters.Add(new SqlParameter("@AgencyId", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                int res = command.ExecuteNonQuery();
                if (res >= 1)
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


    }
}
