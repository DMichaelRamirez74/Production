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
using System.Reflection;
using System.Globalization;
using System.Dynamic;
//using System.Web.Script.Serialization;

namespace FingerprintsData
{

    public class TeacherData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;
        HttpContext context = HttpContext.Current;
       

        public List<Nurse.NurseScreening> Getchildscreeningcenter(string centerid, string userid, string agencyid)
        {
            List<Nurse.NurseScreening> List = new List<Nurse.NurseScreening>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getteacherchildscreeningcenter";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Nurse.NurseScreening info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Nurse.NurseScreening();
                            info.Screeningid = dr["screeningid"].ToString() != "" ? EncryptDecrypt.Encrypt64(dr["screeningid"].ToString()) : "";
                            info.Screeningname = dr["ScreeningName"].ToString();
                            info.Missingcount = dr["missingscreening"].ToString();
                            List.Add(info);
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
            return List;

        }
        public async Task<DataTable> TeacherDashboard()
        {
            DataTable Screeninglist = new DataTable();
            try
            {



                StaffDetails staffThread = await Task.Factory.StartNew(() => StaffDetails.GetThreadedInstance(context));

                command.Parameters.Add(new SqlParameter("@Agencyid", staffThread.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", staffThread.UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CS_Getteacherdashboard";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    Screeninglist = _dataset.Tables[0];
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
            return await Task.FromResult(Screeninglist);

        }
        public List<Roster> GetteacherDeclinedScreenings(string centerid, string userid, string agencyid, string RoleID)
        {
            List<Roster> RosterList = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleID", RoleID));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetteacherDeclinedScreenings";
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
                            info.ScreeningName = dr["screeningname"].ToString();
                            info.CenterName = dr["centername"].ToString();

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
                if (Connection != null)
                    Connection.Close();
            }
            return RosterList;

        }
        public ScreeningMatrix Getallchildmissingscreening(string centerid, string ClassRoom, string userid, string agencyid)
        {
            List<List<string>> List = new List<List<string>>();
            ScreeningMatrix ScreeningMatrix = new ScreeningMatrix();
            List<ClassRoom> Classlist = new List<ClassRoom>();
            List<Roster> Rosterlist = new List<Roster>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                if (!string.IsNullOrEmpty(ClassRoom))
                    command.Parameters.Add(new SqlParameter("@ClassRoom", ClassRoom));
                else
                    command.Parameters.Add(new SqlParameter("@ClassRoom", DBNull.Value));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getchildmissingscreeningcenterteacher";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        List<string> column = new List<string>();
                        foreach (DataColumn dc in _dataset.Tables[0].Columns)
                        {
                            column.Add(dc.ColumnName);
                        }
                        List.Add(column);
                        for (int i = 0; i < _dataset.Tables[0].Rows.Count; i++)
                        {
                            List<string> row = new List<string>();
                            for (int j = 0; j < _dataset.Tables[0].Columns.Count; j++)
                            {
                                row.Add(_dataset.Tables[0].Rows[i][j].ToString());
                            }
                            List.Add(row);
                        }
                        ScreeningMatrix.Screenings = List;
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        ClassRoom Class = null;
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {

                            Class = new ClassRoom();
                            Class.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                            Class.ClassName = dr["ClassroomName"].ToString();
                            Classlist.Add(Class);
                        }
                        ScreeningMatrix.Classroom = Classlist;
                    }
                    //if (_dataset.Tables[2].Rows.Count > 0)
                    //{
                    //    Roster Roster = null;
                    //    foreach (DataRow dr in _dataset.Tables[2].Rows)
                    //    {
                    //        Roster = new Roster();
                    //        Roster.Eclientid = dr["clientid"].ToString();
                    //        Roster.Name = dr["name"].ToString();
                    //        Roster.CenterName = dr["CenterName"].ToString();
                    //        Roster.ClassroomName = dr["ClassroomName"].ToString();
                    //        Roster.ScreeningName = dr["ScreeningName"].ToString();
                    //        Rosterlist.Add(Roster);
                    //    }
                    //    ScreeningMatrix.ClientsClassroom = Rosterlist;
                    //}
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
            return ScreeningMatrix;

        }

        public void GetChildDevelopmentTeamByChildId(ref DataTable dtCenters, string ClientId, string CenterId, string UserId, string AgencyId)
        {
            dtCenters = new DataTable();
            try
            {
                command.Parameters.Add(new SqlParameter("@clientid", Convert.ToInt64(ClientId)));
                command.Parameters.Add(new SqlParameter("@centerid", Convert.ToInt64(CenterId)));
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetChildDevelopmentTeam";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtCenters);
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
        public TeacherModel GetChildList(bool notChecked = false)
        {
            TeacherModel _TeacherM = new TeacherModel();
            _TeacherM.Tdate = String.Format("{0:MM/dd/yyyy}", DateTime.Now.ToString("MM/dd/yyyy")).Replace('-', '/');

            try
            {

            StaffDetails staff = StaffDetails.GetInstance();
            SqlConnection Connection = connection.returnConnection();
            SqlCommand command = new SqlCommand();
            SqlDataAdapter DataAdapter = null;
            DataSet _dataset = null;

          
            command.Connection = Connection;
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
            command.Parameters.Add(new SqlParameter("@ClientID", "1"));
            command.Parameters.Add(new SqlParameter("@isNotChecked", notChecked));
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_GetTeacherList";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
                Connection.Close();
            DataTable dt = _dataset.Tables[0];
            List<TeacherModel> chList = new List<TeacherModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new TeacherModel
                {
                    ClientID = Convert.ToString(dr["ClientID"]),
                    Enc_ClientId = EncryptDecrypt.Encrypt64(dr["ClientID"].ToString()),
                    Programid = Convert.ToString(dr["ProgramID"]),
                    CenterID = Convert.ToString(dr["CenterID"]),
                    Enc_CenterId = EncryptDecrypt.Encrypt64(dr["CenterID"].ToString()),
                        Enc_ClassRoomId=EncryptDecrypt.Encrypt64(dr["ClassroomID"].ToString()),
                    Enc_ProgramId = EncryptDecrypt.Encrypt64(dr["ProgramID"].ToString()),
                    Enc_HouseholdId = EncryptDecrypt.Encrypt64(dr["HouseholdId"].ToString()),
                    CName = Convert.ToString(dr["CName"]),
                    CDOB = Convert.ToString(dr["DOB"]),
                    CImage = Convert.ToString(dr["FileNameul"]),
                    // CIFileData = (byte[])dr["profilepic"],
                    EnrollmentDays = Convert.ToString(dr["EnrollmentDays"]),
                        PercentAbsent = Convert.ToDecimal(dr["AbsentPercent"]),
                    AttendanceType = Convert.ToString(dr["AttendanceType"]),
                    CNotes = Convert.ToString(dr["Notes"]),
                    Parent1ID = Convert.ToString(dr["A1ID"]),
                    Parent1Name = Convert.ToString(dr["A1Name"]),
                    //Parent2ID = Convert.ToString(dr["A2ID"]),
                    //Parent2Name = Convert.ToString(dr["A2Name"]),
                    TimeIn = Convert.ToString(dr["TimeIn"]),
                    TimeIn2 = Convert.ToString(dr["TimeIn2"]),
                    TimeOut = Convert.ToString(dr["TimeOut"]),
                    TimeOut2 = Convert.ToString(dr["TimeOut2"]),
                    ObservationChecked = Convert.ToBoolean(dr["Observation"]),
                    Disability = Convert.ToString(dr["Disability"]),
                    DisabilityDescription = Convert.ToString(dr["DisabilityDescription"]),
                    Dateofclassstartdate = Convert.ToString(dr["Dateofclassstartdate"]),
                    IsLateArrival = Convert.ToBoolean(dr["IsLateArrival"]),
                    NotCheckedCount = Convert.ToInt32(dr["AttendanceTypChecked"]),
                        IsCaseNoteEntered = Convert.ToInt32(dr["IsCaseNoteEntered"]),
                        CenterName=Convert.ToString(dr["CenterName"]),
                        ClassroomName=Convert.ToString(dr["ClassroomName"])
                });

            }
                chList.ForEach(x => x.PercentAbsent = (x.PercentAbsent == 0 || x.PercentAbsent >= 100) ? Math.Round(x.PercentAbsent) : x.PercentAbsent);

            _TeacherM.AbsenceReasonList = new List<SelectListItem>();

            if (_dataset.Tables[1] != null)
            {
                if (_dataset.Tables[1].Rows.Count > 0)
                {
                    _TeacherM.AbsenceReasonList = (from DataRow dr5 in _dataset.Tables[1].Rows
                                                   select new SelectListItem
                                                   {
                                                       Text = dr5["absenseReason"].ToString(),
                                                       Value = dr5["reasonid"].ToString()
                                                   }).ToList();
                    _TeacherM.AbsenceReasonList.Add(
                         new SelectListItem { Text = "Others", Value = "-1" }
                        );

                }
            }

            _TeacherM.ClosedDetails = new ClosedInfo();

            if (_dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
            {
                // _TeacherM.TodayClosed = Convert.ToInt32(_dataset.Tables[3].Rows[0]["TodayClosed"]);

                _TeacherM.ClosedDetails = new ClosedInfo
                {
                    ClosedToday = Convert.ToInt32(_dataset.Tables[2].Rows[0]["TodayClosed"]),
                    CenterName = _dataset.Tables[2].Rows[0]["CenterName"].ToString(),
                    ClassRoomName = _dataset.Tables[2].Rows[0]["ClassRoomName"].ToString(),
                    AgencyName = _dataset.Tables[2].Rows[0]["AgencyName"].ToString()
                };
            }

            _TeacherM.AttendanceTypeList = new List<SelectListItem>();

            if (_dataset.Tables[3] != null)
            {
                if (_dataset.Tables[3].Rows.Count > 0)
                {
                    _TeacherM.AttendanceTypeList = (from DataRow dr5 in _dataset.Tables[3].Rows
                                                    select new SelectListItem
                                                    {
                                                        Text = dr5["Description"].ToString(),
                                                        Value = dr5["AttendanceTypeId"].ToString()
                                                    }).ToList();


                }
            }

                if(_dataset.Tables.Count>4 && _dataset.Tables[4]!=null && _dataset.Tables[4].Rows.Count>0)
                {
                    _TeacherM.AllowCaseNoteTeacher = Convert.ToString(_dataset.Tables[4].Rows[0]["AllowCaseNoteTeacher"]);
                }

                _TeacherM.Itemlst = chList;
                _TeacherM.NotCheckedCount = _TeacherM.Itemlst.Count(x => x.NotCheckedCount == 0);

            _TeacherM.RosterCount = (notChecked) ? (_TeacherM.NotCheckedCount > 0 && _TeacherM.NotCheckedCount < 10) ? "0" + _TeacherM.NotCheckedCount.ToString() : _TeacherM.NotCheckedCount.ToString() : (_TeacherM.Itemlst.Count() > 0 && _TeacherM.Itemlst.Count() < 10) ? "0" + _TeacherM.Itemlst.Count().ToString() : _TeacherM.Itemlst.Count().ToString();
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
            
            return _TeacherM;

        }
        public TeacherModel GetMainChildDisplay(string clientID, int accesstype, string UserID, string agencyid)
        {
            SqlConnection Connection = connection.returnConnection();
            SqlConnection Connection2 = connection.returnConnection();
            SqlConnection Connection3 = connection.returnConnection();
            SqlCommand command = new SqlCommand();
            SqlCommand command2 = new SqlCommand();
            SqlCommand command3 = new SqlCommand();
            SqlDataAdapter DataAdapter = null;
            SqlDataAdapter DataAdapter2 = null;
            SqlDataAdapter DataAdapter3 = null;
            DataSet _dataset = null;
            DataSet _dataset2 = null;
            DataSet _dataset3 = null;


            Connection.Open();
            command.Connection = Connection;
            command.Parameters.Add(new SqlParameter("@UserID", UserID));
            command.Parameters.Add(new SqlParameter("@ClientID", clientID));
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_GetTeacherList1";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];


            TeacherModel _TeacherM = new TeacherModel();
            _TeacherM.Tdate = System.DateTime.Now.ToString("MM/dd/yyyy");

            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {

                _TeacherM.CName = Convert.ToString(dr["CName"]);
                _TeacherM.Parent1Name = Convert.ToString(dr["A1Name"]);
                _TeacherM.Parent2Name = Convert.ToString(dr["A2Name"]);
                _TeacherM.ParentSig = Convert.ToString(dr["PSignature"]);
                _TeacherM.ParentSigOut = Convert.ToString(dr["PSignatureOut"]);
                _TeacherM.Parent1ID = Convert.ToString(dr["A1ID"]);
                _TeacherM.Parent2ID = Convert.ToString(dr["A2ID"]);
                _TeacherM.ParentCheckedIn = Convert.ToString(dr["SignedInBy"]);
                _TeacherM.ParentCheckedOut = Convert.ToString(dr["SignedOutBy"]);
                _TeacherM.OtherName = Convert.ToString(dr["Notes"]);
                _TeacherM.OtherNameTeacher = Convert.ToString(dr["TeacherOtherNotes"]);
                _TeacherM.TeacherName = Convert.ToString(dr["TeacherName"]);
                _TeacherM.TeacherCheckedIn = Convert.ToString(dr["TeacherSignature"]);
                _TeacherM.TimeIn = Convert.ToString(dr["TimeIn"]);
                _TeacherM.TimeIn2 = Convert.ToString(dr["TimeIn2"]);
                _TeacherM.TimeOut = Convert.ToString(dr["TimeOut"]);
                _TeacherM.TimeOut2 = Convert.ToString(dr["TimeOut2"]);
                _TeacherM.ParentSig2 = Convert.ToString(dr["PSignature2"]);
                _TeacherM.ParentSigOut2 = Convert.ToString(dr["PSignatureOutBy2"]);
                _TeacherM.ParentCheckedIn2 = Convert.ToString(dr["SignedInBy2"]);
                _TeacherM.ParentCheckedOut2 = Convert.ToString(dr["SignedOutBy2"]);
                _TeacherM.OtherNameIn2 = Convert.ToString(dr["OtherNotesIn2"]);
                _TeacherM.OtherNameOut = Convert.ToString(dr["OtherNotesOut"]);
                _TeacherM.OtherNameOut2 = Convert.ToString(dr["OtherNotesOut2"]);
            }
            Connection2.Open();
            command2.Connection = Connection2;
            command2.Parameters.Add(new SqlParameter("@AgencyID", "0bcff6e0-e162-4d82-8fe2-a70a2623b4f9"));
            command2.CommandType = CommandType.StoredProcedure;
            command2.CommandText = "SP_GetObservationLookup";
            DataAdapter2 = new SqlDataAdapter(command2);
            _dataset2 = new DataSet();
            DataAdapter2.Fill(_dataset2);
            DataTable dt2 = _dataset2.Tables[0];
            List<TeacherModel> observationlst = new List<TeacherModel>();
            foreach (DataRow dr2 in _dataset2.Tables[0].Rows)
            {
                observationlst.Add(new TeacherModel
                {
                    ObservationID = Convert.ToString(dr2["ObservationKey"]),
                    ObservationDescription = Convert.ToString(dr2["Description"])
                });
            }
            _TeacherM.Observationlst = observationlst;

            Connection3.Open();
            command3.Connection = Connection3;
            command3.Parameters.Add(new SqlParameter("@AgencyID", agencyid));
            command3.Parameters.Add(new SqlParameter("@ClientID", clientID));
            command3.CommandType = CommandType.StoredProcedure;
            command3.CommandText = "SP_GetDailyObservation";
            DataAdapter3 = new SqlDataAdapter(command3);
            _dataset3 = new DataSet();
            DataAdapter3.Fill(_dataset3);
            DataTable dt3 = _dataset3.Tables[0];
            List<TeacherModel> observationlstChecked = new List<TeacherModel>();
            foreach (DataRow dr3 in _dataset3.Tables[0].Rows)
            {
                observationlstChecked.Add(new TeacherModel
                {
                    ObservationIDChecked = Convert.ToString(dr3["Observation"]),

                });
                _TeacherM.TeacherCheckInSig = Convert.ToString(dr3["TeacherCheckInSig"]);
                _TeacherM.OtherNameTeacher = Convert.ToString(dr3["TeacherOther"]);
            }



            _TeacherM.ObservationlstChecked = observationlstChecked;
            _TeacherM.Observationlst = observationlst;

            Connection.Close();
            Connection3.Close();
            Connection2.Close();
            command.Dispose();
            command2.Dispose();
            command3.Dispose();



            return _TeacherM;
        }
        public TeacherModel MarkAbsent(ref string result, string ChildID, string UserID, string absentType, string Cnotes, string agencyid, int AbsentReasonid, string NewReason)
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
                    command.Parameters.Add(new SqlParameter("@clientID", ChildID));
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
            return GetChildList();
        }


        public TeacherModel GetParentList(ref string result, string clientID, int accesstype, string UserID, string agencyid, string available)
        {

            TeacherModel _TeacherM = new TeacherModel();
            try
            {
                result = "";
                SqlConnection Connection = connection.returnConnection();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter DataAdapter = null;
                DataSet _dataset = null;
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@UserID", UserID));
                command.Parameters.Add(new SqlParameter("@ClientID", clientID));
                command.Parameters.Add(new SqlParameter("@AgencyID", agencyid));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetTeacherList1";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                DataTable dt = _dataset.Tables[0];
                Connection.Close();

                _TeacherM.EmergencyContactList = new List<FamilyHousehold>();
                _TeacherM.Tdate = String.Format("{0:MM/dd/yyyy}", DateTime.Now.ToString("MM/dd/yyyy")).Replace('-', '/');

                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        _TeacherM.CName = Convert.ToString(dr["CName"]);
                        _TeacherM.Parent1Name = Convert.ToString(dr["A1Name"]);
                        _TeacherM.Parent2Name = Convert.ToString(dr["A2Name"]);
                        _TeacherM.Parent1ID = Convert.ToString(dr["A1ID"]);
                        _TeacherM.Parent2ID = Convert.ToString(dr["A2ID"]);
                        _TeacherM.OtherNameTeacher = Convert.ToString(dr["TeacherOtherNotes"]);
                        _TeacherM.TeacherName = Convert.ToString(dr["TeacherName"]);
                        _TeacherM.CIFileData = (byte[])dr["profilepic"];
                        _TeacherM.CImage = dr["FileNameul"].ToString();

                    }


                    if (!string.IsNullOrEmpty(clientID) || clientID != "1")
                    {
                        if (_dataset.Tables.Count > 1)
                        {
                            _TeacherM.EmergencyContactList = (from DataRow dr1 in _dataset.Tables[1].Rows
                                                              select new FamilyHousehold
                                                              {
                                                                  EmegencyId = Convert.ToInt32(dr1["ID"]),
                                                                  Efirstname = Convert.ToString(dr1["Name"]),
                                                                  EDOB = dr1["DOB"].ToString() == "" ? "" : Convert.ToDateTime(dr1["DOB"]).ToString("MM/dd/yyyy"),
                                                                  ERelationwithchild = Convert.ToString(dr1["RelationName"]),
                                                                  EImagejson = dr1["DocumentFile"].ToString() == "" ? "" : Convert.ToBase64String((byte[])dr1["DocumentFile"])
                                                              }

                                                            ).ToList();
                        }
                    }

                    List<TeacherModel> observationlst = new List<TeacherModel>();

                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr2 in _dataset.Tables[2].Rows)
                        {
                            observationlst.Add(new TeacherModel
                            {
                                ObservationID = Convert.ToString(dr2["ObservationKey"]),
                                ObservationDescription = Convert.ToString(dr2["Description"])
                            });
                        }
                    }

                    _TeacherM.Observationlst = observationlst;


                    List<TeacherModel> observationlstChecked = new List<TeacherModel>();

                    if (_dataset.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr3 in _dataset.Tables[3].Rows)
                        {
                            observationlstChecked.Add(new TeacherModel
                            {
                                ObservationIDChecked = Convert.ToString(dr3["Observation"]),

                            });
                            _TeacherM.TeacherCheckInSig = Convert.ToString(dr3["TeacherCheckInSig"]);
                            _TeacherM.OtherNameTeacher = Convert.ToString(dr3["TeacherOther"]);
                        }
                    }
                    _TeacherM.ObservationlstChecked = observationlstChecked;

                    if (_dataset.Tables[4].Rows.Count > 0)
                    {
                        foreach (DataRow dr4 in _dataset.Tables[4].Rows)
                        {

                            _TeacherM.ClassID = Convert.ToString(dr4["ClassroomID"]);
                            _TeacherM.CenterID = Convert.ToString(dr4["CenterID"]);
                        }
                    }



                    if(_dataset.Tables.Count>5 && _dataset.Tables[5]!=null && _dataset.Tables[5].Rows.Count>0)
                    {
                        _TeacherM.InkindPeriodList = (from DataRow dr5 in _dataset.Tables[5].Rows

                                                      select new InkindPeriods
                                                      {
                                                          StartDate = Convert.ToString(dr5["StartDate"]),
                                                          EndDate = Convert.ToString(dr5["EndDate"]),
                                                          InkindPeriodID = Convert.ToInt64(dr5["InkindPeriodID"])
                                                      }

                                                    ).ToList();
                    }
                  


                }

                _TeacherM.Activitylst = new List<InkindActivity>();
                _TeacherM.Activitylst = new InKindData().GetInkindActivities(new StaffDetails(), 0, 0, true, 2).InkindActivityList;

                Connection.Close();
                command.Dispose();

                _TeacherM.Available = available.ToString();
                List<TeacherModel> hours = new List<TeacherModel>();
                hours.Add(new TeacherModel { hourID = "0", hourDes = "0" });
                hours.Add(new TeacherModel { hourID = "1", hourDes = "1" });
                hours.Add(new TeacherModel { hourID = "2", hourDes = "2" });
                hours.Add(new TeacherModel { hourID = "3", hourDes = "3" });
                hours.Add(new TeacherModel { hourID = "4", hourDes = "4" });
                _TeacherM.Hours = hours;
                List<TeacherModel> minutes = new List<TeacherModel>();
                minutes.Add(new TeacherModel { minID = "0", minDes = "0" });
                minutes.Add(new TeacherModel { minID = "15", minDes = "15" });
                minutes.Add(new TeacherModel { minID = "30", minDes = "30" });
                minutes.Add(new TeacherModel { minID = "45", minDes = "45" });
                _TeacherM.Minutes = minutes;
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
            return _TeacherM;
        }


        public TeacherModel GetParentList(ref string result, string clientID, string UserID, FormCollection collection, int savetype, string agencyid)
        {
            result = "";
            string result1 = "";

            string TAvailable = collection.Get("Available");
            if (savetype == 1) //Check In  --> first time attendance entry
            {
                string sigType = collection.Get("sigtype");
                if (sigType == "1")   //Mark ClientAttendance Table for Client as present//
                {
                    string imgSig = collection.Get("imageSig");
                    string ParentID = collection.Get("Parent1");
                    string OtherNotes = collection.Get("OtherNotes");



                    if (Connection.State == ConnectionState.Open)
                        Connection.Close();
                    Connection.Open();
                    command.Connection = Connection;
                    command.Parameters.Add(new SqlParameter("@UserID", UserID));
                    command.Parameters.Add(new SqlParameter("@clientID", clientID));
                    command.Parameters.Add(new SqlParameter("@PSignature", imgSig));
                    command.Parameters.Add(new SqlParameter("@PareID", ParentID));
                    command.Parameters.Add(new SqlParameter("@Notes", OtherNotes));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty));
                    command.Parameters["@result"].Direction = ParameterDirection.Output;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_MarkAttendancePresent";
                    //DataAdapter = new SqlDataAdapter(command);
                    //_dataset = new DataSet();
                    //DataAdapter.Fill(_dataset);
                    command.ExecuteNonQuery();
                    result = command.Parameters["@result"].Value.ToString();
                    command.Parameters.Clear();
                    Connection.Close();
                    command.Dispose();

                    string hours = collection.Get("Hours");
                    string minutes = collection.Get("Minutes");
                    string activitynotes = collection.Get("ActivityNotes");
                    string activity = collection.Get("ActivityCode");
                    string activityDate = collection.Get("ActivityDate");
                    string centerid = collection.Get("CenterID");
                    string classroomid = collection.Get("ClassroomID");

                    if (string.IsNullOrWhiteSpace(activity) == false)
                    {
                        List<string> activitylst = activity.Split(',').ToList();
                        List<InKindTransactions> transactionList = new List<InKindTransactions>();

                        if (activitylst.Count() > 0)
                        {
                            activitylst = activitylst.Select(x => x).Distinct().ToList();

                            transactionList = (from act in activitylst
                                               where act != "false"
                                               select new InKindTransactions
                                               {
                                                   AgencyId = new Guid(agencyid),
                                                   ParentID = EncryptDecrypt.Encrypt64(ParentID),
                                                   ActivityDate = activityDate,
                                                   CenterID = Convert.ToInt32(centerid),
                                                   ClassroomID = Convert.ToInt32(classroomid),
                                                   ActivityID = Convert.ToInt32(act),
                                                   Hours = string.IsNullOrEmpty(hours) ? 0 : Convert.ToInt32(hours),
                                                   Minutes = string.IsNullOrEmpty(minutes) ? 0 : Convert.ToInt32(minutes),
                                                   ActivityNotes = activitynotes,
                                                   ClientID = clientID,
                                                   IsActive = true,
                                                   DonorSignature = string.IsNullOrEmpty(imgSig) ? "" : imgSig
                                               }

                                             ).Distinct().ToList();




                        }
                        //foreach (var act in activitylst)
                        //{
                        //    if (act != "false")
                        //    {
                        //        Connection.Close();
                        //        command.Dispose();
                        //        Connection.Open();
                        //        command.Connection = Connection;
                        //        command.Parameters.Add(new SqlParameter("@AgencyID", agencyid));
                        //        command.Parameters.Add(new SqlParameter("@ParentID", ParentID));
                        //        command.Parameters.Add(new SqlParameter("@ActivityDate", activityDate));
                        //        command.Parameters.Add(new SqlParameter("@CenterID", centerid));
                        //        command.Parameters.Add(new SqlParameter("@ClassroomID", classroomid));
                        //        command.Parameters.Add(new SqlParameter("@ActivityID", act));
                        //        command.Parameters.Add(new SqlParameter("@Hours", hours));
                        //        command.Parameters.Add(new SqlParameter("@Minutes", minutes));
                        //        command.Parameters.Add(new SqlParameter("@ActivityNotes", activitynotes));
                        //        command.Parameters.Add(new SqlParameter("@ChildId", clientID));
                        //        command.CommandType = CommandType.StoredProcedure;
                        //        command.CommandText = "SP_MarkInkindActivity";
                        //        DataAdapter = new SqlDataAdapter(command);
                        //        _dataset = new DataSet();
                        //        DataAdapter.Fill(_dataset);
                        //        Connection.Close();
                        //        command.Parameters.Clear();
                        //        command.Dispose();
                        //    }
                        //}

                        if (transactionList.Count() > 0)
                        {
                            new InKindData().InsertParentParticipation(transactionList);
                        }
                    }
                }
                else //Health Check CheckIn
                {
                    string imgSig = collection.Get("imageSigTeacher");
                    string TeacherID = collection.Get("Teacher");
                    string OtherNotes = collection.Get("OtherNotesTeacher");
                    string observation = collection.Get("Observation");
                    List<string> observationlist = null;
                    if (observation != null)
                    {
                        observationlist = observation.Split(',').ToList();
                        foreach (var obs in observationlist)
                        {
                            if (obs != "false")
                            {
                                Connection.Close();
                                command.Dispose();
                                Connection.Open();
                                command.Connection = Connection;
                                command.Parameters.Add(new SqlParameter("@AgencyID", agencyid));
                                command.Parameters.Add(new SqlParameter("@UserID", UserID));
                                command.Parameters.Add(new SqlParameter("@ClientID", clientID));
                                command.Parameters.Add(new SqlParameter("@TSignature", imgSig));
                                command.Parameters.Add(new SqlParameter("@TeacherOther", OtherNotes));
                                command.Parameters.Add(new SqlParameter("@Observation", obs));
                                command.Parameters.Add(new SqlParameter("@ObservationType", 1));
                                command.CommandType = CommandType.StoredProcedure;
                                command.CommandText = "SP_MarkDailyObservation";
                                DataAdapter = new SqlDataAdapter(command);
                                _dataset = new DataSet();
                                DataAdapter.Fill(_dataset);
                                Connection.Close();
                                command.Parameters.Clear();
                                command.Dispose();
                            }
                        }
                    }



                }

                return GetParentList(ref result1, clientID, 2, UserID, agencyid, TAvailable);


            }
            else //Check Out --> record already exists in client attendance table (attendance type may be present or other)
            {


                string imgSig = collection.Get("imageSig");
                string ParentID = string.IsNullOrEmpty(collection.Get("Parent1")) ? "0" : collection.Get("Parent1");
                string OtherNotes = collection.Get("OtherNotes");
                string emergency = string.IsNullOrEmpty(collection.Get("emergency")) ? "0" : collection.Get("emergency");
                string protectiveName = string.IsNullOrEmpty(collection.Get("protectiveName")) ? "" : collection.Get("protectiveName");
                string ProtectiveBadge = string.IsNullOrEmpty(collection.Get("protectiveBadgeNo")) ? "" : collection.Get("protectiveBadgeNo");

                OtherNotes = (protectiveName != "") ? protectiveName : OtherNotes;
                ParentID = (ParentID == "00000") ? (Convert.ToInt32(emergency) > 0) ? emergency : ParentID : ParentID;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
             
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@UserID", UserID));
                command.Parameters.Add(new SqlParameter("@clientID", clientID));
                command.Parameters.Add(new SqlParameter("@PSignature", imgSig));
                command.Parameters.Add(new SqlParameter("@PareID", ParentID));
                command.Parameters.Add(new SqlParameter("@OtherNotes", OtherNotes));
                command.Parameters.Add(new SqlParameter("@BadgeNo", ProtectiveBadge));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_UpdateAttendanceDetails";
                //DataAdapter = new SqlDataAdapter(command);
                //_dataset = new DataSet();
                //DataAdapter.Fill(_dataset);
                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
                command.Dispose();
                return GetParentList(ref result1, clientID, 2, UserID, agencyid, TAvailable);

            }
        }
        public TeacherModel GetMeals(string UserID, string agencyid)
        {
            TeacherModel _TeacherM = new TeacherModel();
            try
            {
                _TeacherM.Tdate = String.Format("{0:MM/dd/yyyy}", DateTime.Now.ToString("MM/dd/yyyy")).Replace('-', '/');
                Guid agency = new Guid(agencyid);
                SqlConnection Connection = connection.returnConnection();
                SqlCommand command = new SqlCommand();
                SqlCommand commandTeacher = new SqlCommand();
                SqlCommand commandMeal = new SqlCommand();
                SqlDataAdapter DataAdapter = null;
                SqlDataAdapter DataAdapterMeal = null;
                DataSet _dataset = null;
                DataSet _datasetMeal = null;
                Connection.Open();
                commandTeacher.Connection = Connection;
                commandTeacher.Parameters.Add(new SqlParameter("@UserID", UserID));
                commandTeacher.CommandType = CommandType.StoredProcedure;
                commandTeacher.CommandText = "SP_GetTeacherInfo";
                DataAdapter = new SqlDataAdapter(commandTeacher);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                foreach (DataRow dr in _dataset.Tables[0].Rows)
                {

                    _TeacherM.ClassID = Convert.ToString(dr["ClassroomID"]);
                    _TeacherM.CenterID = Convert.ToString(dr["CenterID"]);
                }
                commandTeacher.Dispose();

                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@CenterID", _TeacherM.CenterID));
                command.Parameters.Add(new SqlParameter("@ClassroomID", _TeacherM.ClassID));
                command.Parameters.Add(new SqlParameter("@AgencyID", agency));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetMealList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                DataTable dt = _dataset.Tables[0];
                List<TeacherModel> chList = new List<TeacherModel>();

                foreach (DataRow dr in _dataset.Tables[0].Rows)
                {
                    chList.Add(new TeacherModel
                    {
                        ClientID = Convert.ToString(dr["ClientID"]),
                        Programid = Convert.ToString(dr["ProgramID"]),
                        CenterID = Convert.ToString(dr["CenterID"]),
                        CName = Convert.ToString(dr["CName"]),
                        TimeIn = Convert.ToString(dr["TimeIn"]),
                        TimeIn2 = Convert.ToString(dr["TimeIn2"]),
                        TimeOut = Convert.ToString(dr["TimeOut"]),
                        TimeOut2 = Convert.ToString(dr["TimeOut2"]),
                        AttendanceType = Convert.ToString(dr["AttendanceType"]),
                        Breakfast = Convert.ToBoolean(dr["Breakfast"]),
                        Lunch = Convert.ToBoolean(dr["Lunch"]),
                        Snack = Convert.ToBoolean(dr["Snack"]),
                        Dinner = Convert.ToBoolean(dr["Dinner"]),
                        Snack2 = Convert.ToBoolean(dr["Snack2"]),
                        ABreakfast = Convert.ToString(dr["ABreakfast"]),
                        ALunch = Convert.ToString(dr["ALunch"]),
                        ASnack = Convert.ToString(dr["ASnack"]),
                        ADinner = Convert.ToString(dr["ADinner"]),
                        ASnack2 = Convert.ToString(dr["ASnack2"])
                    });


                }

                commandMeal.Connection = Connection;
                commandMeal.Parameters.Add(new SqlParameter("@ClassroomID", _TeacherM.ClassID));
                commandMeal.Parameters.Add(new SqlParameter("@CenterID", _TeacherM.CenterID));
                commandMeal.Parameters.Add(new SqlParameter("@AgencyID", agency));
                commandMeal.CommandType = CommandType.StoredProcedure;
                commandMeal.CommandText = "Sp_Get_classinfo";
                DataAdapterMeal = new SqlDataAdapter(commandMeal);
                _datasetMeal = new DataSet();
                DataAdapterMeal.Fill(_datasetMeal);
                DataTable dtMeal = _datasetMeal.Tables[0];
                List<TeacherModel> meals = new List<TeacherModel>();
                foreach (DataRow dr in _datasetMeal.Tables[0].Rows)
                {
                    if (Convert.ToBoolean(dr["Breakfast"]))
                    {
                        meals.Add(new TeacherModel
                        {
                            MealID = "1",
                            MealType = "Breakfast"

                        });
                    }
                    if (Convert.ToBoolean(dr["Lunch"]))
                    {
                        meals.Add(new TeacherModel
                        {
                            MealID = "2",
                            MealType = "Lunch"

                        });
                    }
                    if (Convert.ToBoolean(dr["Snack"]))
                    {
                        meals.Add(new TeacherModel
                        {
                            MealID = "3",
                            MealType = "Snack"

                        });
                    }
                    if (Convert.ToBoolean(dr["Dinner"]))
                    {
                        meals.Add(new TeacherModel
                        {
                            MealID = "4",
                            MealType = "Dinner"

                        });
                    }
                    if (Convert.ToBoolean(dr["Snack2"]))
                    {
                        meals.Add(new TeacherModel
                        {
                            MealID = "5",
                            MealType = "Snack2"

                        });
                    }
                }

                _TeacherM.Itemlst = chList;
                _TeacherM.Meallst = meals;
                Connection.Close();
                command.Dispose();
                commandMeal.Dispose();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return _TeacherM;
        }
        public TeacherModel GetMeals(ref string result, string UserID, string agencyid, FormCollection collection)
        {
            try
            {

                result = "";
                string ClientMeals = "";
                string mealserved = collection.Get("AdultMeals");
                string CenterID = collection.Get("CenterID");
                string ClassroomID = collection.Get("ClassroomID");
                string MealType = collection.Get("MealTypeSelected");
                if (MealType == "1")
                {
                    ClientMeals = string.IsNullOrEmpty(collection.Get("ClientIDB")) ? "false" : collection.Get("ClientIDB");
                }
                else if (MealType == "2")
                {
                    ClientMeals = string.IsNullOrEmpty(collection.Get("ClientIDL")) ? "false" : collection.Get("ClientIDL");
                }
                else if (MealType == "3")
                {
                    ClientMeals = string.IsNullOrEmpty(collection.Get("ClientIDS")) ? "false" : collection.Get("ClientIDS");
                }
                else if (MealType == "4")
                {
                    ClientMeals = string.IsNullOrEmpty(collection.Get("ClientIDD")) ? "false" : collection.Get("ClientIDD");
                }
                else if (MealType == "5")
                {
                    ClientMeals = string.IsNullOrEmpty(collection.Get("ClientIDS2")) ? "false" : collection.Get("ClientIDS2");
                }
                List<string> ClientIDlist = ClientMeals.Split(',').ToList();
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                command.Connection = Connection;

                command.Parameters.Add(new SqlParameter("@AgencyID", agencyid));
                command.Parameters.Add(new SqlParameter("@UserID", UserID));
                command.Parameters.Add(new SqlParameter("@clientID", 1));
                command.Parameters.Add(new SqlParameter("@MealsServed", mealserved));
                command.Parameters.Add(new SqlParameter("@CenterID", CenterID));
                command.Parameters.Add(new SqlParameter("@ClassroomID", ClassroomID));
                command.Parameters.Add(new SqlParameter("@MealType", MealType));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_MarkMeals";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                result = command.Parameters["@result"].Value.ToString();
                Connection.Close();
                command.Parameters.Clear();
                command.Dispose();
                foreach (var obs in ClientIDlist)
                {
                    if (obs != "false")
                    {
                        if (Connection.State == ConnectionState.Open) Connection.Close();
                        Connection.Open();
                        command.Connection = Connection;

                        command.Parameters.Add(new SqlParameter("@AgencyID", agencyid));
                        command.Parameters.Add(new SqlParameter("@UserID", UserID));
                        command.Parameters.Add(new SqlParameter("@clientID", obs));
                        command.Parameters.Add(new SqlParameter("@MealsServed", mealserved));
                        command.Parameters.Add(new SqlParameter("@CenterID", CenterID));
                        command.Parameters.Add(new SqlParameter("@ClassroomID", ClassroomID));
                        command.Parameters.Add(new SqlParameter("@MealType", MealType));
                        command.Parameters.Add(new SqlParameter("@result", string.Empty));
                        command.Parameters["@result"].Direction = ParameterDirection.Output;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_MarkMeals";
                        DataAdapter = new SqlDataAdapter(command);
                        _dataset = new DataSet();
                        DataAdapter.Fill(_dataset);
                        result = command.Parameters["@result"].Value.ToString();
                        Connection.Close();
                        command.Parameters.Clear();
                        command.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return GetMeals(UserID, agencyid);
        }
        public void ExecutiveDashboard(ref DataTable Screeninglist, string Agencyid, string userid)
        {
            Screeninglist = new DataTable();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CS_Getteacherdashboard";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    Screeninglist = _dataset.Tables[0];
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




        public List<Nurse.NurseScreening> Getchildscreeningcenterexecutive(string centerid, string userid, string agencyid)
        {
            List<Nurse.NurseScreening> List = new List<Nurse.NurseScreening>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Center", centerid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Getexecutivechildscreeningcenter";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Nurse.NurseScreening info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            info = new Nurse.NurseScreening();
                            info.Screeningid = dr["screeningid"].ToString() != "" ? EncryptDecrypt.Encrypt64(dr["screeningid"].ToString()) : "";
                            info.Screeningname = dr["ScreeningName"].ToString();
                            info.Missingcount = dr["missingscreening"].ToString();
                            List.Add(info);
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
            return List;

        }

        public List<DailySaftyCheckImages> GetDailySaftyCheckImages(Guid? UserId, string RoleID, Int64? CenterId)
        {
            List<DailySaftyCheckImages> listImage = new List<DailySaftyCheckImages>();
            try
            {

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetDailySafetyCheckImages";
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", RoleID));
                if (CenterId != null)
                    command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        DailySaftyCheckImages images = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            images = new DailySaftyCheckImages();
                            images.Id = new Guid(dr["Id"].ToString());
                            images.ImageDescription = dr["ImageDescription"].ToString();
                            images.ImagePath = dr["ImagePath"].ToString();
                            if (!string.IsNullOrEmpty(dr["PassFailCode"].ToString()))
                            {
                                bool PassFailCode = Convert.ToBoolean(dr["PassFailCode"].ToString());
                                images.PassFailCode = PassFailCode;
                            }
                            if (!string.IsNullOrEmpty(dr["ToStaffId"].ToString()))
                                images.ToStaffId = new Guid(dr["ToStaffId"].ToString());
                            images.RouteCode = dr["RouteCode"].ToString();
                            images.ImageOfDamage = dr["ImageOfDamage"].ToString();
                            images.WorkOrderDescription = dr["WorkOrderDescription"].ToString();
                            listImage.Add(images);
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
            return listImage;

        }

        public Guid? InsertMonitoringDetail(Monitoring objMonitoring)
        {
            Guid? MonitorId = null;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@AgencyID", objMonitoring.AgencyID));
                command.Parameters.Add(new SqlParameter("@ImageId", objMonitoring.ImageId));
                command.Parameters.Add(new SqlParameter("@PassFailCode", objMonitoring.PassFailCode));
                command.Parameters.Add(new SqlParameter("@CenterId", objMonitoring.CenterId));
                command.Parameters.Add(new SqlParameter("@UserId", objMonitoring.UserID));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Monitoring";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                MonitorId = new Guid(command.ExecuteScalar().ToString());
                if (objMonitoring.PassFailCode)
                {
                    InsertYakkrRouting(new YakkrRouting
                    {
                        AgencyID = objMonitoring.AgencyID,
                        CenterId = objMonitoring.CenterId,
                        ClassRoomId = objMonitoring.ClassRoomId,
                        UserID = objMonitoring.UserID,
                        ToSataffId = objMonitoring.ToSataffId,
                        RouteCode = objMonitoring.RouteCode,
                        Imageid = objMonitoring.ImageId,

                    }, "Delete");
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
            return MonitorId;
        }

        public bool ADDChildReferralNotes(YakkrRouting objYakkrRouting)
        {
            bool isInserted = false;
            try
            {
                Int64 yakkrID = InsertYakkrRouting(objYakkrRouting);
                if (yakkrID != 0)
                {
                    command = new SqlCommand();
                    command.Parameters.Add(new SqlParameter("@AgencyID", objYakkrRouting.AgencyID));
                    command.Parameters.Add(new SqlParameter("@Notes", objYakkrRouting.Message));
                    command.Parameters.Add(new SqlParameter("@CenterId", objYakkrRouting.CenterId));
                    command.Parameters.Add(new SqlParameter("@ClientId", objYakkrRouting.ClientId));
                    command.Parameters.Add(new SqlParameter("@ToStaffId", objYakkrRouting.ToSataffId));
                    command.Parameters.Add(new SqlParameter("@YakkrId", yakkrID));
                    command.Parameters.Add(new SqlParameter("@UserId", objYakkrRouting.UserID));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_InsertTeacherReferralsNotes";
                    if (Connection.State == ConnectionState.Open) Connection.Close();
                    Connection.Open();
                    int RowsAffected = command.ExecuteNonQuery();
                    if (RowsAffected > 0)
                        isInserted = false;
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
            return isInserted;
        }


        public Int64 InsertYakkrRouting(YakkrRouting objMonitoring)
        {
            Int64 YakkkrId = 0;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@AgencyID", objMonitoring.AgencyID));
                command.Parameters.Add(new SqlParameter("@CenterId", objMonitoring.CenterId));
                command.Parameters.Add(new SqlParameter("@UserId", objMonitoring.UserID));
                command.Parameters.Add(new SqlParameter("@ToStaffId", objMonitoring.ToSataffId));
                command.Parameters.Add(new SqlParameter("@RouteCode", objMonitoring.RouteCode));
                command.Parameters.Add(new SqlParameter("@Householdid", objMonitoring.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@ClientId", objMonitoring.ClientId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_InsertYakkrRoutingForChildReferral";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                Object yakkr = command.ExecuteScalar();
                if (yakkr != null)
                    YakkkrId = Convert.ToInt64(yakkr);
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
            return YakkkrId;
        }


        public List<Int64> GetDailyOpenCloseRequest(Guid? UserId)
        {
            List<Int64> listCenters = new List<Int64>();
            try
            {

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetDailySafetyCheckOpenCloseRequest";
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            listCenters.Add(!string.IsNullOrEmpty(dr["CenterId"].ToString()) ? Convert.ToInt64(dr["CenterId"].ToString()) : 0);
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
            return listCenters;
        }

        public bool AddDailySafetyCheckOpenCloseRequest(string Message, bool isClosed, bool isCenter, bool isClassRoom, Guid? UserId, Monitoring objMonitoring, string CenterId)
        {
            bool isUpdated = false;
            try
            {
                if (isClosed)
                {
                    YakkrRouting yakkrRouting = new YakkrRouting();
                    yakkrRouting.AgencyID = objMonitoring.AgencyID;
                    yakkrRouting.UserID = objMonitoring.UserID;
                    yakkrRouting.RouteCode = "73";
                    if (!string.IsNullOrEmpty(CenterId))
                        yakkrRouting.CenterId = Convert.ToInt64(CenterId);

                    long YakkrId = InsertYakkrRoutingForDSCClosedRequest(yakkrRouting);

                    if (YakkrId > 0)
                    {
                        isUpdated = UpdateOpenCloseRequest(Message, isClosed, isCenter, isClassRoom, UserId, objMonitoring, CenterId, YakkrId);
                    }
                }
                else
                {
                    isUpdated = UpdateOpenCloseRequest(Message, isClosed, isCenter, isClassRoom, UserId, objMonitoring, CenterId, null);
                    if (isUpdated)
                    {
                        isUpdated = DeleteYakkrRouting(new YakkrRouting
                        {
                            CenterId = objMonitoring.CenterId,
                            AgencyID = objMonitoring.AgencyID,
                            UserID = objMonitoring.UserID,
                            RouteCode = "73"
                        });
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
            return isUpdated;
        }
        public bool AcceptRejectRequest(string YakkrID, Guid? userId, Guid? agencyId)
        {
            bool isUpdatd = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@YakkrID", YakkrID));
                command.Parameters.Add(new SqlParameter("@UserId", userId));
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AcceptRejectClassroomRequest";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isUpdatd = true;
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
            return isUpdatd;
        }

        public bool ChangeRequest(string RequestId, string[] classRoomIdarray, string userId, string agencyId, string centerId)
        {
            bool isUpdatd = false;
            try
            {
                foreach (string classroom in classRoomIdarray)
                {
                    command = new SqlCommand();
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@RequestId", (RequestId == "null") ? null : RequestId));
                    command.Parameters.Add(new SqlParameter("@ClassRoomId", Convert.ToInt64(classroom)));
                    command.Parameters.Add(new SqlParameter("@CenterId", Convert.ToInt64(centerId)));
                    command.Parameters.Add(new SqlParameter("@UserId", userId));
                    command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_ChangeRequest";
                    if (Connection.State == ConnectionState.Open) Connection.Close();
                    Connection.Open();
                    int RowsAffected = command.ExecuteNonQuery();
                    if (RowsAffected > 0)
                        isUpdatd = true;
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
            return isUpdatd;
        }
        public bool UpdateOpenCloseRequest(string Message, bool isClosed, bool isCenter, bool isClassRoom, Guid? UserId, Monitoring objMonitoring, string CenterId, Int64? YakkrId)
        {
            bool isUpdatd = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@Notes", Message));
                command.Parameters.Add(new SqlParameter("@isClosed", isClosed));
                command.Parameters.Add(new SqlParameter("@isCenter", isCenter));
                command.Parameters.Add(new SqlParameter("@isClassRoom", isClassRoom));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                if (YakkrId != null)
                    command.Parameters.Add(new SqlParameter("@YakkrId", YakkrId));
                if (!string.IsNullOrEmpty(CenterId))
                    command.Parameters.Add(new SqlParameter("@CenterId", Convert.ToInt64(CenterId)));
                //if (isCenter)
                command.Parameters.Add(new SqlParameter("@Status", '0'));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_AddDailySafetyCheckOpenCloseRequest";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isUpdatd = true;
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

            return isUpdatd;
        }

        public bool DeleteExistingDailySafetyCheckOpenCloseRequest(Guid UserId, string CenterId, string AgencyID, string RouteCode)
        {
            bool isDeleted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", CenterId));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyID));
                command.Parameters.Add(new SqlParameter("@RouteCode", RouteCode));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_DeleteDailySafetyCheckCloseRequest";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isDeleted = true;

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
            return isDeleted;
        }
        public bool DeleteDailySafetyCheckOpenCloseRequest(Guid UserId, bool isCenter, Monitoring objMonitoring)
        {
            bool isDeleted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                if (isCenter)
                    command.Parameters.Add(new SqlParameter("@Status", '0'));
                //if (objMonitoring.CenterId != null)
                    command.Parameters.Add(new SqlParameter("@CenterId", objMonitoring.CenterId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_DeleteDailySafetyCheckOpenCloseRequest";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isDeleted = true;
                if (isDeleted)
                {
                    isDeleted = DeleteYakkrRouting(new YakkrRouting
                    {
                        CenterId = objMonitoring.CenterId,
                        AgencyID = objMonitoring.AgencyID,
                        UserID = objMonitoring.UserID,
                        RouteCode = "73"
                    });
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
            return isDeleted;
        }

        public bool InsertWorkOrderDetail(Monitoring objMonitoring)
        {
            bool isInserted = false;
           
            try
            {
                command = new SqlCommand();
                command.Parameters.AddWithValue("@AgencyID", objMonitoring.AgencyID);
                command.Parameters.AddWithValue("@ImageId", objMonitoring.ImageId);
                if (objMonitoring.CenterId != 0)
                    command.Parameters.AddWithValue("@CenterId", objMonitoring.CenterId);
                command.Parameters.AddWithValue("@UserId", objMonitoring.UserID);
                command.Parameters.AddWithValue("@Description", objMonitoring.Description);
                command.Parameters.AddWithValue("@ImageOfDamage", objMonitoring.ImageOfDamage);
                command.Parameters.AddWithValue("@MonitorId", objMonitoring.Id);
                command.Parameters.AddWithValue("@ToStaffId", objMonitoring.ToSataffId);
                command.Parameters.AddWithValue("@RouteCode", objMonitoring.RouteCode);
                command.Parameters.AddWithValue("@MonitorUniqueId", string.Empty);


                command.Parameters["@MonitorUniqueId"].Direction = ParameterDirection.Output;

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_WorkOrder";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                string RowsAffected = Convert.ToString(command.ExecuteScalar());

                if (RowsAffected != "")
                {
                    isInserted = InsertYakkrRouting(new YakkrRouting
                    {
                        AgencyID = objMonitoring.AgencyID,
                        CenterId = objMonitoring.CenterId,
                        ClassRoomId = objMonitoring.ClassRoomId,
                        UserID = objMonitoring.UserID,
                        ToSataffId = objMonitoring.ToSataffId,
                        RouteCode = objMonitoring.RouteCode,
                        MonitorId = RowsAffected

                    }, "TeacherRole");
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
            return isInserted;
        }

        public bool InsertYakkrRouting(YakkrRouting objMonitoring, string Command)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@AgencyID", objMonitoring.AgencyID));
                command.Parameters.Add(new SqlParameter("@CenterId", objMonitoring.CenterId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", objMonitoring.ClassRoomId));
                command.Parameters.Add(new SqlParameter("@UserId", objMonitoring.UserID));
                command.Parameters.Add(new SqlParameter("@ToStaffId", objMonitoring.ToSataffId));
                command.Parameters.Add(new SqlParameter("@RouteCode", objMonitoring.RouteCode));
                command.Parameters.Add(new SqlParameter("@Householdid", objMonitoring.HouseHoldId));
                command.Parameters.Add(new SqlParameter("@Imageid", objMonitoring.Imageid));
                command.Parameters.Add(new SqlParameter("@Email", objMonitoring.Email));
                if (objMonitoring.ClientId != null)
                    command.Parameters.Add(new SqlParameter("@ClientId", objMonitoring.ClientId));
                command.Parameters.Add(new SqlParameter("@Command", Command));
                command.Parameters.Add(new SqlParameter("@Message", objMonitoring.Message));
                command.Parameters.Add(new SqlParameter("@MonitorId", objMonitoring.MonitorId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_InsertYakkrRouting";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = (int)command.ExecuteNonQuery();
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

        public Int64 InsertYakkrRoutingForDSCClosedRequest(YakkrRouting objMonitoring)
        {
            Int64 YakkrId = 0;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@AgencyID", objMonitoring.AgencyID));
                command.Parameters.Add(new SqlParameter("@UserId", objMonitoring.UserID));
                command.Parameters.Add(new SqlParameter("@RouteCode", objMonitoring.RouteCode));
                if (objMonitoring.CenterId != 0)
                    command.Parameters.Add(new SqlParameter("@CenterId", objMonitoring.CenterId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertYakkrForDSCClosedRequest";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                Object Yakkr = command.ExecuteScalar();
                if (Yakkr != null)
                    YakkrId = Convert.ToInt64(Yakkr);
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
            return YakkrId;
        }

        public bool DeleteYakkrRouting(YakkrRouting objMonitoring)
        {
            bool isDeleted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@AgencyID", objMonitoring.AgencyID));
                command.Parameters.Add(new SqlParameter("@UserId", objMonitoring.UserID));
                command.Parameters.Add(new SqlParameter("@RouteCode", objMonitoring.RouteCode));
                //if (objMonitoring.CenterId != null)
                    command.Parameters.Add(new SqlParameter("@CenterId", objMonitoring.CenterId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DeleteYakkrRouting";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = (int)command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isDeleted = true;
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
            return isDeleted;
        }
        public string GetFireExpirationDate(Guid UserId, string Command)
        {
            string ExpirationDate = "";
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@Command", Command));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Get_FireExtinguisherExpirationDate";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                object expireDate = command.ExecuteScalar();
                if (expireDate != DBNull.Value)
                    ExpirationDate = Convert.ToDateTime(expireDate).ToString("MM/dd/yyyy");
                else
                    ExpirationDate = expireDate.ToString();
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
            return ExpirationDate;
        }

        public bool UpdateFireExpirationDate(Guid UserId, string Date, string Command)
        {
            bool isUpdated = false;
            try
            {
                DateTime expirationDate = Convert.ToDateTime(Date);
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@Date", expirationDate));
                command.Parameters.Add(new SqlParameter("@Command", Command));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_UpdateExpirationDate";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected >= 1)
                    isUpdated = true;
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
            return isUpdated;
        }

        public DomainObservationResults GetDomainObservationResults(Guid? UserId, Int64? ClientId)
        {
            DomainObservationResults results = new DomainObservationResults();
            try
            {
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetDomainObservationResults";
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                if (ClientId != null)
                    command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            results.lstDomain.Add(new Domain { Id = dr["DomainId"].ToString(), Name = dr["Domain"].ToString(), Count = dr["Count"].ToString() });

                        }
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            results.lstNotes.Add(new Notes
                            {
                                NoteId = dr["NoteId"].ToString(),
                                Date = dr["Date"].ToString(),
                                Note = dr["Note"].ToString(),
                                Name = dr["Name"].ToString(),
                                Title = dr["Title"].ToString(),
                                Element = dr["ElementName"].ToString(),
                                Attchment = dr["Attchment"].ToString()
                            });

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
            return results;
        }

        public void GetAttachmentByNoteId(ref DataTable dtAttachments, string NoteId, string UserId)
        {
            dtAttachments = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@NoteId", NoteId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetAttachmentByNoteId";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtAttachments);
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

        public TeacherModel GetChildListByUserIdByCenter(string UserID, string AgencyID, long centerId, long classroomId, bool ishistorical, string attendanceDate)
        {
            TeacherModel _TeacherM = new TeacherModel();

            try
            {
                _TeacherM.Tdate = String.Format("{0:MM/dd/yyyy}", DateTime.Now.ToString("MM/dd/yyyy")).Replace('-', '/');
                SqlConnection Connection = connection.returnConnection();
                SqlCommand command = new SqlCommand();
                //SqlDataAdapter DataAdapter = null;
                //DataSet _dataset = null;
                List<TeacherModel> chList = new List<TeacherModel>();
                List<TeacherModel> chListByDate = new List<TeacherModel>();
                List<OfflineAttendance> teacherList = new List<OfflineAttendance>();
                Center center = new Center();
                List<Center> _centerList = new List<Center>();
                _TeacherM.AbsenceReasonList = new List<SelectListItem>();

                List<Tuple<int, DateTime, long, DateTime>> classStartDateList = new List<Tuple<int, DateTime, long, DateTime>>();
                List<Tuple<int, DateTime, long, DateTime>> droppedList = new List<Tuple<int, DateTime, long, DateTime>>();
                List<Tuple<int, DateTime, long, DateTime>> withdrawnList = new List<Tuple<int, DateTime, long, DateTime>>();
                var inputDatesString = attendanceDate.Split(',').OrderBy(x => DateTime.Parse(x
                                                  , new CultureInfo("en-US", true))).OrderBy(x => x).ToList();

                var inputDates = inputDatesString.Select(x => DateTime.Parse(x
                                                    , new CultureInfo("en-US", true))).OrderBy(x => x).ToList();

                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                using (Connection=connection.returnConnection())
                {
                    Connection.Open();
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@UserID", UserID));
                    command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
                    command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                    command.Parameters.Add(new SqlParameter("@ClassRoomId", classroomId));
                    command.Parameters.Add(new SqlParameter("@IsHistorical", ishistorical));
                    command.Parameters.Add(new SqlParameter("@AttendanceDate", attendanceDate));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 500; //timeout 3mins
                    command.CommandText = "USP_GetWeeklyAttendanceList";
                    //DataAdapter = new SqlDataAdapter(command);
                    //_dataset = new DataSet();
                    //DataAdapter.Fill(_dataset);
                    //Connection.Close();


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

                                        chList.Add(new TeacherModel
                                        {

                                            ClientID = Convert.ToString(dr["ClientID"]),
                                            Enc_ClientId = EncryptDecrypt.Encrypt64(dr["ClientID"].ToString()),
                                            CName = Convert.ToString(dr["Firstname"]) + " " + Convert.ToString(dr["Lastname"]),
                                            CDOB = Convert.ToString(dr["DOB"]),
                                            CenterID = (ishistorical) ? centerId.ToString() : dr["CenterId"].ToString(),
                                            Enc_CenterId = EncryptDecrypt.Encrypt64((ishistorical) ? centerId.ToString() : dr["CenterID"].ToString()),
                                            ClassID = (ishistorical) ? classroomId.ToString() : dr["ClassRoomId"].ToString(),
                                            Enc_ClassRoomId = EncryptDecrypt.Encrypt64((ishistorical) ? classroomId.ToString() : dr["ClassRoomId"].ToString()),
                                            Parent1ID = dr["Parent1ID"].ToString(),
                                            Parent2ID = dr["Parent2ID"].ToString(),
                                            Parent1Name = dr["Parent1Name"].ToString(),
                                            Parent2Name = dr["Parent2Name"].ToString(),
                                            //AccessDateString = dr1["AccessDateString"].ToString(),
                                            //Dateofclassstartdate = Convert.ToString(dr1["DateOfClassStartDate"])
                                            AccessDateString = string.Empty,
                                            Dateofclassstartdate = string.Empty


                                        });
                                        break;
                                    case 1:
                                        teacherList.Add(new OfflineAttendance
                                        {
                                            ClientID = EncryptDecrypt.Encrypt64(dr["ClientID"].ToString()),
                                            CenterID = EncryptDecrypt.Encrypt64((ishistorical) ? centerId.ToString() : dr["CenterID"].ToString()),
                                            ClassroomID = EncryptDecrypt.Encrypt64((ishistorical) ? classroomId.ToString() : dr["ClassRoomID"].ToString()),
                                            AttendanceType = dr["AttendanceType"].ToString(),
                                            AttendanceDate = dr["AttendanceDate"].ToString(),
                                            TimeIn = GetFormattedTime(dr["TimeIn"].ToString()),
                                            TimeOut = GetFormattedTime(dr["TimeOut"].ToString()),
                                            BreakFast = (dr["BreakFast"].ToString() != "0" && dr["BreakFast"].ToString() != "") ? "1" : "0",
                                            Lunch = (dr["Lunch"].ToString() != "0" && dr["Lunch"].ToString() != "") ? "1" : "0",
                                            Snacks = (dr["Snacks"].ToString() != "0" && dr["Snacks"].ToString() != "") ? "1" : "0",
                                            AdultBreakFast = dr["AdultBreakFast"].ToString(),
                                            AdultLunch = dr["AdultLunch"].ToString(),
                                            AdultSnacks = dr["AdultSnacks"].ToString(),
                                            PSignatureIn = (ishistorical) ? "" : string.IsNullOrEmpty(dr["PSignatureIn"].ToString()) ? "" : dr["PSignatureIn"].ToString(),
                                            PSignatureOut = (ishistorical) ? "" : string.IsNullOrEmpty(dr["PSignatureOut"].ToString()) ? "" : dr["PSignatureOut"].ToString(),
                                            SignedInBy = (ishistorical) ? "" : string.IsNullOrEmpty(dr["SignedInBy"].ToString()) ? "" : dr["SignedInBy"].ToString(),
                                            SignedOutBy = (ishistorical) ? "" : string.IsNullOrEmpty(dr["SignedOutBy"].ToString()) ? "" : dr["SignedOutBy"].ToString(),
                                            TSignatureIn = (ishistorical) ? "" : string.IsNullOrEmpty(dr["TSignatureIn"].ToString()) ? "" : dr["TSignatureIn"].ToString(),
                                            TSignatureOut = "",
                                            AbsenceReasonId = Convert.ToString(dr["AbsenceReasonId"])
                                        });

                                        break;
                                    case 2:
                                        _TeacherM.AbsenceReasonList.Add(new SelectListItem
                                        {
                                            Text = dr["Reason"].ToString(),
                                            Value = dr["ReasonId"].ToString()
                                        });
                                        break;
                                    case 3:
                                        _TeacherM.ClosedDetails = new ClosedInfo
                                        {

                                            ClosedToday = Convert.ToInt32(dr["TodayClosed"]),
                                            CenterName = Convert.ToString(dr["ClosedCenterName"]),
                                            ClassRoomName = Convert.ToString(dr["ClosedClassRoomName"]),
                                            AgencyName = Convert.ToString(dr["ClosedAgencyName"])
                                        };
                                        break;
                                    case 4:
                                        classStartDateList.Add(
                                                 Tuple.Create(1, DateTime.Parse(Convert.ToString(dr["ClassStartDate"])
                                                 , new CultureInfo("en-US", true))
                                                 , Convert.ToInt64(dr["ClientID"])
                                                 , DateTime.Parse(Convert.ToString(dr["DateEntered"])
                                                 , new CultureInfo("en-US", true))));

                                        classStartDateList = classStartDateList.OrderBy(x => x.Item2).ToList();
                                        break;
                                    case 5:
                                        droppedList.Add(Tuple.Create(2, DateTime.Parse(Convert.ToString(dr["DateDropped"])
                                                      , new CultureInfo("en-US", true))
                                                      , Convert.ToInt64(dr["ClientID"])
                                                        , DateTime.Parse(Convert.ToString(dr["DateEntered"])
                                                             , new CultureInfo("en-US", true))));

                                        droppedList = droppedList.OrderBy(x => x.Item2).ToList();
                                        break;
                                    case 6:
                                        withdrawnList.Add(Tuple.Create(3, DateTime.Parse(Convert.ToString(dr["DateWithDrawn"])
                                                        , new CultureInfo("en-US", true))
                                                        , Convert.ToInt64(dr["ClientID"])
                                                          , DateTime.Parse(Convert.ToString(dr["DateEntered"])
                                                             , new CultureInfo("en-US", true))));

                                        withdrawnList = withdrawnList.OrderBy(x => x.Item2).ToList();
                                        break;



                                }

                            }

                            resultSet++;

                        } while (dr.NextResult());
                    }

                }
                if (chList.Count > 0)
                {
                    Parallel.ForEach(chList,(child,loopState,index)=>
                     {
                         var dateList = new List<DateTime>();
                         var _clientClassStartDateList = classStartDateList.Where(x => x.Item3.ToString() == child.ClientID).ToList();
                         var finalEnrolledList = _clientClassStartDateList
                           .Union(droppedList.Where(x => x.Item3.ToString() == child.ClientID).ToList())
                           .Union(withdrawnList.Where(x => x.Item3.ToString() == child.ClientID).ToList())
                           .OrderBy(x => x.Item2).ThenBy(x => x.Item4).ToList();
                         if (finalEnrolledList.Select(x => x.Item1).Last() == 1)
                         {
                             //var alldayslist = Enumerable.Range(0, 1 + inputDates.Select(y => y).Last().Subtract(finalEnrolledList.Select(x => x.Item2).Last()).Days)
                             //.Select(offset => finalEnrolledList.Select(x => x.Item2).Last().AddDays(offset)).ToList();

                             //var alldayslist = Enumerable.Range(0, (1 + Math.Abs(inputDates.Select(y => y).Last().Subtract(finalEnrolledList.Select(x => x.Item2).Last()).Days)))
                             //.Select(offset => finalEnrolledList.Select(x => x.Item2).Last().AddDays(offset)).ToList();


                             var allDays = Enumerable.Range(0, (1 + Math.Abs(inputDates.Select(y => y).Last().Subtract(finalEnrolledList.Select(x => x.Item2).Last()).Days)))
               .Select(offset => finalEnrolledList.Select(x => x.Item2).Last().AddDays(offset))
               .Intersect(inputDates).ToList();
                             dateList.AddRange(
                                 allDays
                                 );

                             // dateList.AddRange(dateList,dateL);
                         }


                         int i = 0;

                         while (i < finalEnrolledList.Count - 1)
                         {
                             var status1 = finalEnrolledList[i].Item1;
                             var date1 = finalEnrolledList[i].Item2;


                             var status2 = finalEnrolledList[i + 1].Item1;
                             var date2 = finalEnrolledList[i + 1].Item2;

                             if (status1 == 1)
                             {
                                 var allDays2 = Enumerable.Range(0, 1 + date2.Subtract(date1).Days)
              .Select(offset => date1.AddDays(offset)).Intersect(inputDates)
              .ToList();
                                 dateList.AddRange(allDays2);


                             }
                             else
                             {
                                 date1 = date1.AddDays(1);
                                 if (status2 == 1)
                                 {
                                     if (inputDates.Where(x => x == date2).Any())
                                     {
                                         dateList.Add(date2);
                                     }
                                     date2 = date2.AddDays(-1);
                                 }
                                 if (status2 == 3)
                                 {
                                     if (inputDates.Where(x => x == date2).Any())
                                     {
                                         dateList.Add(date2);
                                     }
                                 }
                             }
                             i += 1;
                         }
                         chList[Convert.ToInt32(index)].Dateofclassstartdate = string.Join(",", _clientClassStartDateList.Select(x => String.Format("{0:MM/dd/yyyy}", x.Item2).Replace('-', '/')));
                         chList[Convert.ToInt32(index)].AccessDateString = string.Join(",", dateList.Select(x => String.Format("{0:MM/dd/yyyy}", x).Replace('-', '/')));
                     });
                }
                _TeacherM.CenterList = _centerList;
                _TeacherM.Itemlst = chList;
                _TeacherM.WeeklyAttendance = teacherList;
                var lsits = chList.Where(x => x.AccessDateString != "").ToList();
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

            _TeacherM.UserId = UserID;
            _TeacherM.AgencyId = AgencyID;
            return _TeacherM;
        }


        public TeacherModel GetChildAttendanceDetailsByDate(string agencyId, string attendanceDate, bool isHistorical, long centerId, long classRoomId)
        {

            TeacherModel model = new TeacherModel();
            List<OfflineAttendance> teacherList = new List<OfflineAttendance>();
            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
                SqlConnection Connection = connection.returnConnection();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter DataAdapter = null;
                DataSet _dataset = null;
                model.Itemlst = new List<TeacherModel>();

                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyID", agencyId));
                command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", classRoomId));
                command.Parameters.Add(new SqlParameter("@IsHistorical", isHistorical));
                command.Parameters.Add(new SqlParameter("@AttendanceDate", attendanceDate));
                command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetClientAttendanceByAttendanceDate";
                command.CommandTimeout = 180; // timeout for 2 minutes
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                Connection.Open();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                command.Dispose();
                if (_dataset.Tables[0].Rows.Count > 0)
                {
                    teacherList = (from DataRow dr in _dataset.Tables[0].Rows
                                   select new OfflineAttendance
                                   {
                                       ClientID = EncryptDecrypt.Encrypt64(dr["ClientID"].ToString()),
                                       CenterID = EncryptDecrypt.Encrypt64((isHistorical) ? centerId.ToString() : dr["CenterID"].ToString()),
                                       ClassroomID = EncryptDecrypt.Encrypt64((isHistorical) ? classRoomId.ToString() : dr["ClassRoomID"].ToString()),
                                       AttendanceType = dr["AttendanceType"].ToString(),
                                       AttendanceDate = dr["AttendanceDate"].ToString(),
                                       TimeIn = GetFormattedTime(dr["TimeIn"].ToString()),
                                       TimeOut = GetFormattedTime(dr["TimeOut"].ToString()),
                                       BreakFast = (dr["BreakFast"].ToString() != "0" && dr["BreakFast"].ToString() != "") ? "1" : "0",
                                       Lunch = (dr["Lunch"].ToString() != "0" && dr["Lunch"].ToString() != "") ? "1" : "0",
                                       Snacks = (dr["Snacks"].ToString() != "0" && dr["Snacks"].ToString() != "") ? "1" : "0",
                                       AdultBreakFast = dr["AdultBreakFast"].ToString(),
                                       AdultLunch = dr["AdultLunch"].ToString(),
                                       AdultSnacks = dr["AdultSnacks"].ToString(),
                                       PSignatureIn = (isHistorical) ? "" : string.IsNullOrEmpty(dr["PSignatureIn"].ToString()) ? "" : dr["PSignatureIn"].ToString(),
                                       PSignatureOut = (isHistorical) ? "" : string.IsNullOrEmpty(dr["PSignatureOut"].ToString()) ? "" : dr["PSignatureOut"].ToString(),
                                       SignedInBy = (isHistorical) ? "" : string.IsNullOrEmpty(dr["SignedInBy"].ToString()) ? "" : dr["SignedInBy"].ToString(),
                                       SignedOutBy = (isHistorical) ? "" : string.IsNullOrEmpty(dr["SignedOutBy"].ToString()) ? "" : dr["SignedOutBy"].ToString(),
                                       TSignatureIn = (isHistorical) ? "" : string.IsNullOrEmpty(dr["TSignatureIn"].ToString()) ? "" : dr["TSignatureIn"].ToString(),
                                       TSignatureOut = "",
                                       AbsenceReasonId = Convert.ToString(dr["AbsenceReasonId"])
                                   }
                             ).ToList();

                }

                if (_dataset.Tables[1].Rows.Count > 0)
                {
                    model.Itemlst = (from DataRow dr1 in _dataset.Tables[1].Rows
                                     select new TeacherModel
                                     {
                                         ClientID = Convert.ToString(dr1["ClientID"]),
                                         Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientID"].ToString()),
                                         CName = Convert.ToString(dr1["Firstname"]) + " " + Convert.ToString(dr1["Lastname"]),
                                         CDOB = Convert.ToString(dr1["DOB"]),
                                         CenterID = (isHistorical) ? centerId.ToString(): dr1["CenterId"].ToString(),
                                         Enc_CenterId = EncryptDecrypt.Encrypt64((isHistorical)?centerId.ToString(): dr1["CenterID"].ToString()),
                                         ClassID = (isHistorical) ? classRoomId.ToString(): dr1["ClassRoomId"].ToString(),
                                         Enc_ClassRoomId = EncryptDecrypt.Encrypt64((isHistorical)?classRoomId.ToString(): dr1["ClassRoomId"].ToString()),
                                         Parent1ID = dr1["FatherId"].ToString(),
                                         Parent2ID = dr1["MotherId"].ToString(),
                                         Parent1Name = dr1["FatherName"].ToString(),
                                         Parent2Name = dr1["MotherName"].ToString(),
                                         AccessDateString = dr1["AccessDateString"].ToString(),
                                         //RestrictedDateString = dr1["RestrictedDateString"].ToString()
                                       Dateofclassstartdate=Convert.ToString(dr1["DateOfClassStartDate"])

                                       //Dateofclassstartdate=string.Join(",",_dataset.Tables[1].AsEnumerable().
                                       //                      Where(x=>x.Field<long>("ClientID")==Convert.ToInt64(dr1["ClientID"])).
                                       //                      Select(x=>x.Field<string>("DateOfClassStartDate")).ToArray())
                                     }

                            ).Distinct().ToList();
                }

                model.WeeklyAttendance = teacherList;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return model;
        }

        public TeacherModel GetCenterBasedCenters(string userId, string agencyId, string roleId)
        {
            TeacherModel model = new TeacherModel();
            model.CenterList = new List<Center>();
            try
            {
                SqlConnection Connection = connection.returnConnection();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter DataAdapter = null;
                DataSet _dataset = null;
                Center center = new Center();


                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserID", userId));
                command.Parameters.Add(new SqlParameter("@AgencyID", agencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetCenterBasedCenters";
                command.CommandTimeout = 180; // timeout for 2 minutes
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        model.CenterList = (from DataRow dr in _dataset.Tables[0].Rows
                                            select new Center
                                            {
                                                CenterId = Convert.ToInt32(dr["CenterId"]),
                                                Enc_CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                                                CenterName = dr["CenterName"].ToString(),
                                                TimeZoneID = dr["TimeZone"].ToString(),
                                                TimeZoneMinuteDiff = Convert.ToInt32(dr["UTCMINUTEDIFFERENC"])
                                            }).ToList();
                        model.Dateofclassstartdate = _dataset.Tables[0].Rows[0]["AccessStartDate"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            //finally
            //{
            //    DataAdapter.Dispose();
            //    _dataset.Dispose();

            //}
            return model;
        }


        ///public List<OfflineAttendance> InsertOfflineAttendanceData(List<OfflineAttendance> _offlineAttendance, List<TeacherModel> _mealsList, List<TeacherModel> _adultMealsList, string UserID, string AgencyID, string dateString)

        public bool InsertOfflineAttendanceData(List<OfflineAttendance> _offlineAttendance, List<TeacherModel> _mealsList, List<TeacherModel> _adultMealsList, string UserID, string AgencyID, string dateString, bool isHistorical)

        {

            //  List<OfflineAttendance> teacherList = new List<OfflineAttendance>();
            bool isRowAffected = false;
            try
            {

                DataTable attendancetable = GetOfflineAttendanceTable(_offlineAttendance);
                DataTable clientMealsTable = GetClientMealsTable(_mealsList);
                DataTable adultMealsTable = GetAdultMealsTable(_adultMealsList);


                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@UserID", UserID));
                    command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
                    command.Parameters.Add(new SqlParameter("@OfflineAttendanceTable", attendancetable));
                    command.Parameters.Add(new SqlParameter("@ClientMealsTable", clientMealsTable));
                    command.Parameters.Add(new SqlParameter("@AdultMealsTable", adultMealsTable));
                    command.Parameters.Add(new SqlParameter("@AttendanceDateString", dateString));
                    command.Parameters.Add(new SqlParameter("@IsHistorical", isHistorical));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_InsertOfflineAttendance";
                    command.CommandTimeout = 180; // timeout for 2 minutes
                    if (Connection.State == ConnectionState.Open) Connection.Close();
                    Connection.Open();
                    //DataAdapter = new SqlDataAdapter(command);
                    //_dataset = new DataSet();
                    //DataAdapter.Fill(_dataset);
                    isRowAffected = Convert.ToInt32(command.ExecuteNonQuery()) > 0;
                    Connection.Close();
                }
                //if (_dataset != null)
                //{
                //    if (_dataset.Tables.Count>0 && _dataset.Tables[0].Rows.Count > 0)
                //    {


                //        teacherList = (from DataRow dr in _dataset.Tables[0].Rows
                //                       select new OfflineAttendance
                //                       {
                //                           ClientID = EncryptDecrypt.Encrypt64(dr["ClientID"].ToString()),
                //                           CenterID = EncryptDecrypt.Encrypt64(dr["CenterID"].ToString()),
                //                           ClassroomID = EncryptDecrypt.Encrypt64(dr["ClassRoomID"].ToString()),
                //                           AttendanceType = dr["AttendanceType"].ToString(),
                //                           AttendanceDate = Convert.ToString(dr["AttendanceDate"]),
                //                           TimeIn = GetFormattedTime(dr["TimeIn"].ToString()),
                //                           TimeOut = GetFormattedTime(dr["TimeOut"].ToString()),
                //                           BreakFast = (dr["BreakFast"].ToString() != "0" && dr["BreakFast"].ToString() != "") ? "1" : "0",
                //                           Lunch = (dr["Lunch"].ToString() != "0" && dr["BreakFast"].ToString() != "") ? "1" : "0",
                //                           Snacks = (dr["Lunch"].ToString() != "0" && dr["BreakFast"].ToString() != "") ? "1" : "0",
                //                           AdultBreakFast = dr["AdultBreakFast"].ToString(),
                //                           AdultLunch = dr["AdultLunch"].ToString(),
                //                           AdultSnacks = dr["AdultSnacks"].ToString(),
                //                           PSignatureIn = string.IsNullOrEmpty(dr["PSignatureIn"].ToString()) ? "" : dr["PSignatureIn"].ToString(),
                //                           PSignatureOut = string.IsNullOrEmpty(dr["PSignatureOut"].ToString()) ? "" : dr["PSignatureOut"].ToString(),
                //                           SignedInBy = string.IsNullOrEmpty(dr["SignedInBy"].ToString()) ? "" : dr["SignedInBy"].ToString(),
                //                           SignedOutBy = string.IsNullOrEmpty(dr["SignedOutBy"].ToString()) ? "" : dr["SignedOutBy"].ToString(),
                //                           TSignatureIn = string.IsNullOrEmpty(dr["TSignatureIn"].ToString()) ? "" : dr["TSignatureIn"].ToString(),
                //                           TSignatureOut = "",
                //                           AbsenceReasonId = Convert.ToString(dr["AbsenceReasonId"])

                //                       }
                //                     ).ToList();


                //    }
                //}

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
            /// return teacherList;
            return isRowAffected;

        }


        public DataTable GetOfflineAttendanceTable(List<OfflineAttendance> offlineAttendance)
        {
            DataTable dt = new DataTable();
            try
            {

                string inTime = null;
                string outTime = null;
                dt.Columns.AddRange(new DataColumn[17] {
                    new DataColumn("ClientID", typeof(long)),
                    new DataColumn("AgencyID",typeof(Guid)),
                    new DataColumn("CenterID",typeof(long)),
                    new DataColumn("ClassroomID",typeof(long)),
                    new DataColumn("AttendanceType",typeof(string)),
                    new DataColumn("StaffCreated",typeof(Guid)),
                    new DataColumn("AttendanceDate ", typeof(string)),
                    new DataColumn("IsActive",typeof(bool)),
                    new DataColumn("SignedInBy",typeof(string)),
                    new DataColumn("PSignatureIn",typeof(string)),
                    new DataColumn("PSignatureOut",typeof(string)),
                    new DataColumn("TSignatureIn",typeof(string)),
                    new DataColumn("TSignatureOut",typeof(string)),
                    new DataColumn("TimeIn",typeof(string)),
                    new DataColumn("TimeOut",typeof(string)),
                    new DataColumn("SignedOutBy",typeof(string)),
                    new DataColumn("AbsenceReasonId",typeof(int))

                });


                foreach (var item in offlineAttendance)
                {
                    inTime = (item.TimeIn == "") ? "" : DateTime.ParseExact(item.TimeIn,
                                    "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay.ToString();
                    outTime = (item.TimeOut == "") ? "" : DateTime.ParseExact(item.TimeOut,
                                    "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay.ToString();

                    dt.Rows.Add(
                       Convert.ToInt64(item.ClientID),
                       new Guid(item.AgencyId),
                      Convert.ToInt64(item.CenterID),
                       Convert.ToInt64(item.ClassroomID),
                       item.AttendanceType,
                       new Guid(item.UserID),
                       item.AttendanceDate,
                       true,
                       item.SignedInBy,
                       item.PSignatureIn,
                       item.PSignatureOut,
                       item.TSignatureIn,
                       item.TSignatureOut,
                       inTime,
                       outTime,
                       item.SignedOutBy,
                       string.IsNullOrEmpty(item.AbsenceReasonId) ? 0 : Convert.ToInt32(item.AbsenceReasonId)
                        );

                }

                return dt;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return dt;
            }

        }


        public DataTable GetClientMealsTable(List<TeacherModel> mealsList)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.AddRange(new DataColumn[8] {
                      new DataColumn("AgencyID",typeof(Guid)),
                    new DataColumn("CenterID",typeof(long)),
                    new DataColumn("ClassroomID",typeof(long)),
                      new DataColumn("ClientID ", typeof(long)),
                    new DataColumn("AttendanceDate ", typeof(string)),
                    new DataColumn("MealType",typeof(string)),
                    new DataColumn("StaffID",typeof(Guid)),
                    new DataColumn("IsActive",typeof(bool))

                });


                foreach (var item in mealsList)
                {

                    dt.Rows.Add(
                        new Guid(item.AgencyId),
                      Convert.ToInt64(item.CenterID),
                       Convert.ToInt64(item.ClassID),
                       Convert.ToInt64(item.ClientID),
                       item.AttendanceDate,
                       item.MealType,
                       new Guid(item.UserId),
                       1
                        );

                }

                return dt;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return dt;
            }

        }


        public DataTable GetAdultMealsTable(List<TeacherModel> adultMealsList)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.AddRange(new DataColumn[8] {
                    new DataColumn("AgencyID",typeof(Guid)),
                    new DataColumn("StaffID",typeof(Guid)),
                    new DataColumn("CenterID",typeof(long)),
                    new DataColumn("ClassroomID",typeof(long)),
                    new DataColumn("AttendanceDate ", typeof(string)),
                    new DataColumn("MealType",typeof(string)),
                    new DataColumn("MealsServed",typeof(int)),
                    new DataColumn("IsActive",typeof(bool))

                });


                foreach (var item in adultMealsList)
                {

                    dt.Rows.Add(
                        new Guid(item.AgencyId),
                           new Guid(item.UserId),
                      Convert.ToInt64(item.CenterID),
                       Convert.ToInt64(item.ClassID),
                       item.AttendanceDate,
                       item.MealType,
                       Convert.ToInt32(item.MealSelected),
                       1
                        );

                }

                return dt;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return dt;
            }

        }




        public string GetFormattedTime(string _time)
        {
            var str = _time.Split(':')[0] + ":" + _time.Split(':')[1];
            var timeFromInput = DateTime.ParseExact(str, "H:m", null, System.Globalization.DateTimeStyles.None);

            string timeIn12HourFormatForDisplay = timeFromInput.ToString(
                "hh:mm tt",
                System.Globalization.CultureInfo.InvariantCulture);
            return timeIn12HourFormatForDisplay;
        }


        public List<AbsenceReason> GetAbsenceReason(Guid? AgencyId)
        {
            List<AbsenceReason> reasonList = new List<AbsenceReason>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Parameters.Add(new SqlParameter("@mode", 1));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AbsenceReason";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        reasonList = (from DataRow dr in _dataset.Tables[0].Rows
                                      select new AbsenceReason
                                      {
                                          ReasonId = Convert.ToInt32(dr["ReasonId"]),
                                          Reason = dr["Reason"].ToString(),
                                          AgencyId = string.IsNullOrEmpty(dr["AgencyId"].ToString()) ? (Guid?)null : new Guid(dr["AgencyId"].ToString())
                                      }
                                      ).ToList();

                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return reasonList;
        }


        public List<AbsenceReason> SaveAbsenceReason(out bool isRowAffected, AbsenceReason reason)
        {
            isRowAffected = false;
            List<AbsenceReason> reasonList = new List<AbsenceReason>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", reason.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", reason.CreatedBy));
                command.Parameters.Add(new SqlParameter("@ReasonId", reason.ReasonId));
                command.Parameters.Add(new SqlParameter("@Reason", reason.Reason));
                command.Parameters.Add(new SqlParameter("@Status", reason.Status));
                command.Parameters.Add(new SqlParameter("@mode", 2));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AbsenceReason";
                int RowsAffected = (int)command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
                reasonList = GetAbsenceReason(reason.AgencyId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return reasonList;
        }


        public List<AttendanceType> GetAttendanceType(Guid? AgencyId)
        {
            List<AttendanceType> attendanceTypeList = new List<AttendanceType>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Parameters.Add(new SqlParameter("@mode", 1));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AttendanceTypes";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        attendanceTypeList = (from DataRow dr in _dataset.Tables[0].Rows
                                              select new AttendanceType
                                              {
                                                  AttendanceTypeId = Convert.ToInt64(dr["AttendanceTypeId"]),
                                                  Description = dr["Description"].ToString(),
                                                  AgencyId = string.IsNullOrEmpty(dr["AgencyId"].ToString()) ? (Guid?)null : new Guid(dr["AgencyId"].ToString()),
                                                  Acronym = dr["Acronym"].ToString(),
                                                  HomeBased = Convert.ToBoolean(dr["HomeBased"].ToString()),
                                                  IndexId = Convert.ToInt64(dr["IndexId"])
                                              }
                                      ).ToList();

                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return attendanceTypeList;
        }


        public AttendanceTypeModel InsertAttendanceType(out bool isRowAffected, AttendanceType model)
        {

            AttendanceTypeModel typeModel = new AttendanceTypeModel();
            List<AttendanceType> typeList = new List<AttendanceType>();
            isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", model.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", model.UserId));
                command.Parameters.Add(new SqlParameter("@AttendanceTypeId", model.AttendanceTypeId));
                command.Parameters.Add(new SqlParameter("@Description", model.Description));
                command.Parameters.Add(new SqlParameter("@Acronym", model.Acronym));
                command.Parameters.Add(new SqlParameter("@HomeBased", model.HomeBased));
                command.Parameters.Add(new SqlParameter("@Status", model.Status));
                command.Parameters.Add(new SqlParameter("@IndexId", model.IndexId));
                command.Parameters.Add(new SqlParameter("@mode", 2));
                command.Parameters.Add(new SqlParameter("@SuperAdminCenterBased", 4));
                command.Parameters.Add(new SqlParameter("@SuperAdminHomeBased", 9));
                command.Parameters.Add(new SqlParameter("@AgencyCenterBased", 6));
                command.Parameters.Add(new SqlParameter("@AgencyHomeBased", 6));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AttendanceTypes";
                int RowsAffected = (int)command.ExecuteNonQuery();
                if (RowsAffected > 0)
                {
                    isRowAffected = true;
                }
                typeList = GetAttendanceType(model.AgencyId);
                typeModel.attendanceTypeList = typeList;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return typeModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="model"></param>
        /// <returns>result=> 0=not available in database,1= Description and Acronym available in database,2=Description available in database,3=Acronym available in database </returns>
        public int GetAvailableAttendanceType(out int result, AttendanceType model)
        {
            int availCount = 0;
            result = 0;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", model.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", model.UserId));
                command.Parameters.Add(new SqlParameter("@HomeBased", model.HomeBased));
                command.Parameters.Add(new SqlParameter("@AttendanceTypeId", model.AttendanceTypeId));
                command.Parameters.Add(new SqlParameter("@Description", model.Description));
                command.Parameters.Add(new SqlParameter("@Acronym", model.Acronym));
                command.Parameters.Add(new SqlParameter("@SuperAdminCenterBased", 4));
                command.Parameters.Add(new SqlParameter("@SuperAdminHomeBased", 9));
                command.Parameters.Add(new SqlParameter("@AgencyCenterBased", 6));
                command.Parameters.Add(new SqlParameter("@AgencyHomeBased", 6));
                command.Parameters.Add(new SqlParameter("@mode", 3));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AttendanceTypes";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        availCount = Convert.ToInt32(_dataset.Tables[0].Rows[0]["AvailableCount"]);
                        result = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Result"]);
                    }
                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return availCount;
        }

        public List<SelectListItem> GetAttendanceType(string agencyId, string userId, bool homeBased)
        {
            List<SelectListItem> attendanceList = new List<SelectListItem>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", new Guid(agencyId)));
                command.Parameters.Add(new SqlParameter("@HomeBased", homeBased));
                command.Parameters.Add(new SqlParameter("@UserId", new Guid(userId)));
                command.Parameters.Add(new SqlParameter("@mode", 4));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AttendanceTypes";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        attendanceList = (from DataRow dr in _dataset.Tables[0].Rows
                                          select new SelectListItem
                                          {
                                              Text = dr["Description"].ToString(),
                                              Value = dr["AttendanceTypeId"].ToString()
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
                if (Connection != null)
                    Connection.Close();
            }
            return attendanceList;
        }



        public TeacherModel GetClassRoomsByCenterHistorical(TeacherModel model)
        {
            Center center = new Center();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", model.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", model.UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", model.CenterID));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetClassRoomsByCenterId_Historical";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();

                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        center.CenterId = Convert.ToInt32(_dataset.Tables[0].Rows[0]["CenterId"]);
                        center.CenterName = _dataset.Tables[0].Rows[0]["CenterName"].ToString();
                        center.Enc_CenterId = EncryptDecrypt.Encrypt64(_dataset.Tables[0].Rows[0]["CenterId"].ToString());

                        center.Classroom = (from DataRow dr1 in _dataset.Tables[0].Rows
                                            select new Center.ClassRoom
                                            {
                                                ClassroomID = Convert.ToInt32(dr1["ClassRoomId"]),
                                                Enc_ClassRoomId = EncryptDecrypt.Encrypt64(dr1["ClassRoomId"].ToString()),
                                                ClassName = dr1["ClassRoomName"].ToString(),
                                                Monday = Convert.ToBoolean(dr1["Monday"]),
                                                Tuesday = Convert.ToBoolean(dr1["Tuesday"]),
                                                Wednesday = Convert.ToBoolean(dr1["Wednesday"]),
                                                Thursday = Convert.ToBoolean(dr1["Thursday"]),
                                                Friday = Convert.ToBoolean(dr1["Friday"]),
                                                Saturday = Convert.ToBoolean(dr1["Saturday"]),
                                                Sunday = Convert.ToBoolean(dr1["Sunday"]),
                                                StartTime = string.IsNullOrEmpty(dr1["StartTime"].ToString()) ? "" : GetFormattedTime(dr1["StartTime"].ToString()),
                                                StopTime = string.IsNullOrEmpty(dr1["EndTime"].ToString()) ? "" : GetFormattedTime(dr1["EndTime"].ToString()),
                                                ClosedToday = Convert.ToInt32(dr1["ClosedToday"])
                                            }
                                       ).ToList();
                    }


                }

                model = new TeacherModel();
                model.CenterList = new List<Center>();
                model.CenterList.Add(center);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return model;
        }

        public TeacherModel GetFSWandTeacherByClassRoom(TeacherModel model)
        {

            Center center = new Center();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", model.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", model.UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", model.CenterID));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", model.ClassID));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetFSWandTeacherByClassRoom";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                model = new TeacherModel();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr1 in _dataset.Tables[0].Rows)
                        {


                            model.TeacherName = string.IsNullOrEmpty(dr1["TeacherName"].ToString()) ? "" : dr1["TeacherName"].ToString();
                            model.FSWName = string.IsNullOrEmpty(dr1["FswName"].ToString()) ? "" : dr1["FswName"].ToString();
                            model.TeacherTimeZoneDiff = Convert.ToInt32(dr1["TeacherTimeDiff"]);
                            model.FSWTimeZoneDiff = Convert.ToInt32(dr1["FSWTimeDiff"]);
                            model.UserId = dr1["TeacherUserId"].ToString();
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

                if (Connection == null)
                    Connection.Close();

                DataAdapter.Dispose();
                _dataset.Dispose();
                Connection.Dispose();
            }

            return model;
        }

        public bool CheckUserHasHomeBased(StaffDetails details)
        {
            bool result = false;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", details.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CheckUserHasHomeBasedCenter";
                Connection.Open();
                result = Convert.ToBoolean(command.ExecuteScalar());
                Connection.Close();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Close();
                }
                command.Dispose();
            }
            return result;
        }

        #region GrowthAnalysis

        public List<ClientGrowth> GetChildrenInfoForWH(int mode,string AssDate,long classroomid,long ClientId)
        {

            var _clientGrowth = new List<ClientGrowth>();
            try
            {

                var stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GrowthAnalysisDetails";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));

                command.Parameters.Add(new SqlParameter("@mode", mode)); //1
                command.Parameters.Add(new SqlParameter("@AssessmentDate", AssDate)); 
                command.Parameters.Add(new SqlParameter("@ClassroomId", classroomid));
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId)); 

                DataAdapter = new SqlDataAdapter(command);
                DataSet _ds = new DataSet();
                DataAdapter.Fill(_ds);

                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    var encryField = new List<string>();
                   encryField.Add("ClientID");

                    _clientGrowth= _ds.Tables[0].DataTableToList<ClientGrowth>(encryField);
                   // _clientGrowth = _ds.Tables[0].DataTableToList<ClientGrowth>();
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return _clientGrowth;
        }


        public bool AddChildWH(List<ClientGrowth> data, int mode) {
            bool IsSuccess = false;


            try
            {

                var stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_UpdateGrowthAnalysis";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@mode", mode)); //1

                var _GrowthTable = new List<string>()
                { "ClientID","AssessmentDate","Height","Weight","BMI","HeadCirc","InputType" };
                var _decrypted = new List<string>() { "ClientID" };
                DataTable dt = Helpers.ToUserDefinedDataTable(data, _GrowthTable,_decrypted);
                command.Parameters.AddWithValue("@data", dt);

                int res = command.ExecuteNonQuery();
                if (res > 0)
                    IsSuccess = true;


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


            return IsSuccess;
        }



        public List<ClientGrowth> GetHistoricalRecordByChildId(long clientid)
        {
            var _clientGrowth = new List<ClientGrowth>();
            try
            {

                var stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_HistoricalGrowthDetails";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@ClientId", clientid));
                command.Parameters.Add(new SqlParameter("@mode", 1)); //1

                DataAdapter = new SqlDataAdapter(command);
                DataSet _ds = new DataSet();
                DataAdapter.Fill(_ds);

                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    var encryField = new List<string>();
                    encryField.Add("ClientID");

                    _clientGrowth = _ds.Tables[0].DataTableToList<ClientGrowth>(encryField);
                    // _clientGrowth = _ds.Tables[0].DataTableToList<ClientGrowth>();
                }
            }
            catch (Exception ex) {
                clsError.WriteException(ex);
            }
            return _clientGrowth;
            }

        public bool DeleteHistoricalRecordById(long indexid,long clientid)
        {
            var success = false;
            try
            {

                var stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_HistoricalGrowthDetails";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@ClientId", clientid));
                command.Parameters.Add(new SqlParameter("@mode", 2)); //1
                command.Parameters.Add(new SqlParameter("@IndexId", indexid)); //1

                var re = command.ExecuteNonQuery();

                if(re > 0) {
                    success = true;
                }


            }
            catch (Exception ex) {
                clsError.WriteException(ex);
            }

            return success;

        }



        // public GrowthChart GetGrowthChart(int mode, long clientid,int type)
        public ExpandoObject GetGrowthChart(int mode, long clientid, int type)
        {
            // var result = new GrowthChart();
            dynamic result = new ExpandoObject();
            result.ChildGrowth = new List<ClientGrowth>();
            result.DTHeadCircuGrowth = new List<STDTable>();
            result.DTLengthGrowth = new List<STDTable>();
            result.DTWeightGrowth = new List<STDTable>();
            result.DTWeightLengthGrowth = new List<STDTable>();
            result.DTBMIGrowth = new List<STDTable>();



            try
            {

                var stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetGrowthAnalysisChart";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@Mode", mode)); //1
                command.Parameters.Add(new SqlParameter("@ClientId", clientid));
                command.Parameters.Add(new SqlParameter("@type", type));
                

                DataAdapter = new SqlDataAdapter(command);
                DataSet _ds = new DataSet();
                DataAdapter.Fill(_ds);
                if (_ds != null)
                {
                    

                    if (_ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                    {
                        // result.ChildGrowth = _ds.Tables[0].DataTableToList<ClientGrowth>(new List<string>());
                        result.ChildGrowth = _ds.Tables[0];

                    }
                    
                    //Standard Datatables
                    if (_ds.Tables.Count > 1 && _ds.Tables[1].Rows.Count > 0)
                    {
                        //  result.DTHeadCircuGrowth = _ds.Tables[1].DataTableToList<STDTable>(new List<string>());

                        result.DTHeadCircuGrowth = _ds.Tables[1];
                    }
                    
                    if ( _ds.Tables.Count > 2 && _ds.Tables[2].Rows.Count > 0)
                    {
                        //  result.DTLengthGrowth = _ds.Tables[2].DataTableToList<STDTable>(new List<string>());
                        result.DTLengthGrowth = _ds.Tables[2];
                    }
                    if (_ds.Tables.Count > 3 && _ds.Tables[3].Rows.Count > 0)
                    {
                        //result.DTWeightGrowth = _ds.Tables[3].DataTableToList<STDTable>(new List<string>());
                        result.DTWeightGrowth = _ds.Tables[3];
                    }
                    if (_ds.Tables.Count > 4 && _ds.Tables[4].Rows.Count > 0)
                    {
                        if (type == 1)
                        {
                            // result.DTWeightLengthGrowth = _ds.Tables[4].DataTableToList<STDTable>(new List<string>());
                            result.DTWeightLengthGrowth = _ds.Tables[4];
                        }
                        else if (type == 2) {
                            //result.DTBMIGrowth = _ds.Tables[4].DataTableToList<STDTable>(new List<string>());
                            result.DTBMIGrowth = _ds.Tables[4];
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            //  return result;
            return result;
        }


            #endregion GrowthAnalysis

    }
}
