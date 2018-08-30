
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsData
{
    public class EducationManagerData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        DataSet _dataset = null;

        public EducationManager GetEducationDashboard()
        {
            EducationManager per = new EducationManager();

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                StaffDetails staff = new StaffDetails();
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetTeachersEducationLevel";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        switch (dr["HighestEducation"].ToString())
                        {
                            case "0":
                                per.EduLevel0 = Convert.ToInt32(dr["EducationCount"]);
                                break;
                            case "2":
                                per.EduLevel2 = Convert.ToInt32(dr["EducationCount"]);
                                break;

                            case "3":
                                per.EduLevel3 = Convert.ToInt32(dr["EducationCount"]);
                                break;
                            case "4":
                                per.EduLevel4 = Convert.ToInt32(dr["EducationCount"]);
                                break;

                        }
                    }
                }

                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {

                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        if (dr["GettingDegree"] == DBNull.Value)
                        {
                            per.GettingDegree = 0;
                            per.NotGettingDegree = 0;
                        }
                        else
                        {
                            switch (dr["GettingDegree"].ToString())
                            {
                                case "0":
                                    per.GettingDegree = Convert.ToInt32(dr["DegreeCount"]);
                                    break;
                                case "1":
                                    per.NotGettingDegree = Convert.ToInt32(dr["DegreeCount"]);
                                    break;
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
                Connection.Close();
                command.Dispose();
            }
            return per;
        }


        public List<StaffEventCreation> GetStaffEventList(int Mode)
        {
            List<StaffEventCreation> events = new List<StaffEventCreation>();

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                StaffDetails staff = new StaffDetails();
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@Mode", Mode));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_EventCreation";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                if (_dataset != null && _dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {

                    events = (from DataRow dr5 in _dataset.Tables[0].Rows
                              select new StaffEventCreation
                              {
                                  EventDescription = dr5["Description"].ToString(),
                                  EvenitAddress = dr5["EventAddress"].ToString(),
                                  EventDate = dr5["TrainingDate"].ToString(),
                                  Eventid = Convert.ToInt32(dr5["EventId"]),
                                  EventName = dr5["TrainingName"].ToString(),
                                  ContinuingEdu = dr5["ContinuingEducation"].ToString(),
                                  TotalHours = dr5["TotalHours"].ToString(),
                                  Trainer = dr5["Trainer"].ToString(),
                                  CancelReason = dr5["CancellReason"].ToString(),
                                  ModifiedDate = dr5["ModifiedDate"].ToString(),
                                  IsTodayEvent = Convert.ToInt32(dr5["IsTodayEvent"])
                              }).ToList();



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
            return events;
        }

        public StaffEventCreation GetStaffEventCreation(int Mode, string Eventid = "")
        {
            StaffEventCreation evt = new StaffEventCreation();

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                StaffDetails staff = new StaffDetails();
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@Mode", Mode));
                command.Parameters.Add(new SqlParameter("@Eventid", Eventid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_EventCreation";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                if (_dataset != null && _dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    //evt.CenterList = new List<SelectListItem>();
                    //evt.CenterList = (from DataRow dr5 in _dataset.Tables[1].Rows
                    //                  select new SelectListItem
                    //                  {
                    //                      Text = dr5["Centername"].ToString(),
                    //                      Value = dr5["centerid"].ToString()
                    //                  }).ToList();
                    evt.CenterList = new List<StaffEventCreation.CenterListItem>();
                    evt.CenterList = (from DataRow dr5 in _dataset.Tables[1].Rows
                                      select new StaffEventCreation.CenterListItem
                                      {
                                          Text = dr5["Centername"].ToString(),
                                          Value = dr5["centerid"].ToString(),
                                          HomeBased =Convert.ToBoolean(dr5["homebased"])
                                      }).ToList();

                    evt.ActiveRoles = new List<SelectListItem>();
                    evt.ActiveRoles = (from DataRow dr5 in _dataset.Tables[2].Rows
                                       select new SelectListItem
                                       {
                                           Text = dr5["RoleName"].ToString(),
                                           Value = dr5["RoleId"].ToString()
                                       }).ToList();

                    if (Eventid != "")
                    {
                        evt.EventDescription = _dataset.Tables[0].Rows[0]["Description"].ToString();
                        evt.EvenitAddress = _dataset.Tables[0].Rows[0]["EventAddress"].ToString();
                        evt.EventDate = _dataset.Tables[0].Rows[0]["TrainingDate"].ToString();
                        evt.Eventid = Convert.ToInt32(_dataset.Tables[0].Rows[0]["EventId"]);
                        evt.EventName = _dataset.Tables[0].Rows[0]["TrainingName"].ToString();
                        evt.InitialEventDate = _dataset.Tables[0].Rows[0]["TrainingDate"].ToString();
                        evt.InitialEventTime = _dataset.Tables[0].Rows[0]["EventTime"].ToString();
                        evt.ContinuingEdu = _dataset.Tables[0].Rows[0]["ContinuingEducation"].ToString();
                        evt.TotalHours = _dataset.Tables[0].Rows[0]["TotalHours"].ToString();
                        evt.StartTime = _dataset.Tables[0].Rows[0]["EventTime"].ToString();
                        evt.Trainer = _dataset.Tables[0].Rows[0]["Trainer"].ToString();
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
            return evt;
        }


        public StaffEventCreation StaffEventInfo(string yakkrid)
        {
            StaffEventCreation evt = new StaffEventCreation();

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                StaffDetails staff = new StaffDetails();
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@yakkrid", yakkrid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_StaffEventInfo";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                if (_dataset != null && _dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    evt.EventDescription = _dataset.Tables[0].Rows[0]["Description"].ToString();
                    evt.EvenitAddress = _dataset.Tables[0].Rows[0]["EventAddress"].ToString();
                    evt.EventDate = _dataset.Tables[0].Rows[0]["TrainingDate"].ToString();
                    evt.Eventid = Convert.ToInt32(_dataset.Tables[0].Rows[0]["EventId"]);
                    evt.EventName = _dataset.Tables[0].Rows[0]["TrainingName"].ToString();
                    evt.Trainer = _dataset.Tables[0].Rows[0]["Trainer"].ToString();
                    evt.ContinuingEdu = _dataset.Tables[0].Rows[0]["ContinuingEducation"].ToString();
                    evt.TotalHours = _dataset.Tables[0].Rows[0]["TotalHours"].ToString();
                    evt.StartTime = _dataset.Tables[0].Rows[0]["EventTime"].ToString();
                    evt.RSVPStatus = Convert.ToInt32(_dataset.Tables[0].Rows[0]["RSVPStatus"]);
                    evt.EventChangesOn = Convert.ToInt32(_dataset.Tables[0].Rows[0]["EventChangesOn"]);
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
            return evt;
        }
        public bool SaveRSVP(string RSVP, string Eventid, string yakkrid)
        {
            bool isInserted = false;

            try
            {
                StaffDetails session = new StaffDetails();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveStaffRSVP";
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Agencyid", session.AgencyId);
                command.Parameters.AddWithValue("@UserId", session.UserId);
                command.Parameters.AddWithValue("@RSVP", RSVP);
                command.Parameters.AddWithValue("@Eventid", Eventid);
                command.Parameters.AddWithValue("@yakkrid", yakkrid);
                int res = command.ExecuteNonQuery();
                if (res > 1)
                    isInserted = true;
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
            return isInserted;
        }
        public bool SaveStaffEventDetails(StaffEventCreation evt)
        {
            bool isInserted = false;

            try
            {



                StaffDetails session = new StaffDetails();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                if(evt.EventType==1)
                command.CommandText = "SP_SaveStaffEventDetails";
                else if(evt.EventType == 2)
                    command.CommandText = "SP_SaveHistoricalStaffEventDetails"; 
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Agencyid", session.AgencyId);
                command.Parameters.AddWithValue("@UserId", session.UserId);
                command.Parameters.AddWithValue("@EventName", evt.EventName);
                command.Parameters.AddWithValue("@EventDescription", evt.EventDescription);
                command.Parameters.AddWithValue("@EventDate", evt.EventDate);
                command.Parameters.AddWithValue("@EventAddress", evt.EvenitAddress);
                command.Parameters.AddWithValue("@Centerid", evt.CenterIds != null ? String.Join(",", evt.CenterIds) : "");
                command.Parameters.AddWithValue("@ContinuingEdu", evt.ContinuingEdu);
                command.Parameters.AddWithValue("@TotalHours", evt.TotalHours);
                command.Parameters.AddWithValue("@Trainer", evt.Trainer);
                command.Parameters.AddWithValue("@EventTime", evt.StartTime);
                command.Parameters.AddWithValue("@ChangesOn", evt.EventChangesOn);
                command.Parameters.AddWithValue("@EventId", evt.Eventid);
                command.Parameters.AddWithValue("@isCancelled", evt.IsEventCancelled);
                command.Parameters.AddWithValue("@CancellReason", evt.CancelReason);
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] {
                    new DataColumn("RoleId", typeof(string)),
                    });

                if (evt.RolesList != null && evt.RolesList.Count > 0)
                {
                    foreach (string roles in evt.RolesList)
                    {
                        if (roles != "")
                        {
                            dt.Rows.Add(roles);

                        }
                    }
                    command.Parameters.AddWithValue("@RolesList", dt);

                }
                if (evt.EventType == 2)
                {
                    DataTable dt1 = new DataTable();
                    dt1.Columns.AddRange(new DataColumn[1] {
                    new DataColumn("UserId", typeof(string)),
                    });

                    if (evt.UsersList != null && evt.UsersList.Count > 0)
                    {
                        foreach (string userid in evt.UsersList)
                        {
                            if (userid != "")
                            {
                                dt1.Rows.Add(userid);

                            }
                        }

                    }
                    command.Parameters.AddWithValue("@UserList", dt1);

                }

                int res = command.ExecuteNonQuery();
                if (res > 1)
                    isInserted = true;
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
            return isInserted;
        }

        //public StaffEventCreation GetEventByYakkrId(string YakkrId)
        //{
        //    StaffEventCreation evt = new StaffEventCreation();
        //    StaffDetails staffDetails = StaffDetails.GetInstance();

        //    try
        //    {
        //        if (Connection.State == ConnectionState.Open)
        //            Connection.Close();

        //        command.Connection = Connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        _dataset = new DataSet();
        //        command.Parameters.Clear();
        //        command.Parameters.Add(new SqlParameter("@Agencyid", staffDetails.AgencyId));
        //        command.Parameters.Add(new SqlParameter("@userid", staffDetails.UserId));
        //        command.Parameters.Add(new SqlParameter("@yakkrid", YakkrId));
        //        command.CommandText = "SP_GetEventByEventId";
        //        SqlDataAdapter da = new SqlDataAdapter(command);
        //        da.Fill(_dataset);
        //        Connection.Close();
        //        if (_dataset != null)
        //        {
        //            if (_dataset.Tables[0].Rows.Count > 0)
        //            {
        //                evt.CreatedBy = _dataset.Tables[0].Rows[0]["StaffName"].ToString();
        //                evt.EventDate = _dataset.Tables[0].Rows[0]["EventDateTime"].ToString();
        //                evt.EventName = _dataset.Tables[0].Rows[0]["TrainingName"].ToString();
        //                evt.EventDescription = _dataset.Tables[0].Rows[0]["description"].ToString();
        //                evt.CancelReason = _dataset.Tables[0].Rows[0]["cancellreason"].ToString();
                      


        //            }
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    return evt;


        //}


        public EventReportDetails GetEventReportByID(int eventType,int eventId,int mode,string search="", string CheckInUserId=null,string AgencyId=null, string ManagerId =null)
        {

           
           EventReportDetails evtDetails = new EventReportDetails();
            evtDetails.ParticipantDetails = new List<ParticipantDetail>();
            StaffDetails staffDetails = StaffDetails.GetInstance();


            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@EventId", eventId));
                command.Parameters.Add(new SqlParameter("@EventType", eventType));
                command.Parameters.Add(new SqlParameter("@Mode", mode));
                command.Parameters.Add(new SqlParameter("@Userid", staffDetails.UserId));
                command.Parameters.Add(new SqlParameter("@Search", search));
                command.Parameters.Add(new SqlParameter("@CheckInUserId", CheckInUserId));
                command.CommandText = "GetEventReportByID";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        evtDetails.Eventid = Convert.ToInt32(_dataset.Tables[0].Rows[0]["EventID"]);
                        evtDetails.Trainer = _dataset.Tables[0].Rows[0]["Trainer"].ToString();
                        evtDetails.EventName = _dataset.Tables[0].Rows[0]["TrainingName"].ToString();
                        evtDetails.EventDescription = _dataset.Tables[0].Rows[0]["Description"].ToString();
                        evtDetails.EventDate = _dataset.Tables[0].Rows[0]["TrainingDate"].ToString();
                        evtDetails.EventAddress = _dataset.Tables[0].Rows[0]["EventAddress"].ToString();
                       // evtDetails.IsTodayEvent = Convert.ToInt32(_dataset.Tables[0].Rows[0]["EventAddress"]);
                        if (_dataset.Tables.Count > 1 && _dataset.Tables[1].Rows.Count > 0)
                        {
                            
                            foreach (DataRow item in _dataset.Tables[1].Rows)
                            {

                                var _pDetail = new ParticipantDetail() {
                                    ParticipantId = item["UserId"].ToString(),
                                    ParticipantName = item["Name"].ToString(),
                                    ParticipantRoleName = item["RoleName"].ToString(),
                                    RSVPStatusModifiedDate = item["RSVPStatusModifiedDate"].ToString() == "" ? "--" : item["RSVPStatusModifiedDate"].ToString(),
                                    RSVPStatusName = item["RSVPStatus"].ToString(),
                                    RSVPStatusId = DBNull.Value == item["RSVPStatusId"] ? -1 : Convert.ToInt32(item["RSVPStatusId"]),
                                     
                                };
                                if (mode == 2) {
                                    _pDetail.Signature = DBNull.Value == item["Signature"] ? "" : item["Signature"].ToString();
                                    _pDetail.IsAttended = DBNull.Value == item["IsAttended"] ? -1 : Convert.ToInt32(item["IsAttended"]);
                                    _pDetail.Avatar =  item["Avatar"].ToString();
                                }
                                if (mode == 3) {
                                    _pDetail.Avatar = item["Avatar"].ToString();
                                    _pDetail.Gender = DBNull.Value == item["Gender"] ? -1 : Convert.ToInt32(item["Gender"]);
                                }

                                evtDetails.ParticipantDetails.Add(_pDetail);

                               //evtDetails.ParticipantDetails.Add(new ParticipantDetail()
                               // {


                                //     ParticipantId = item["UserId"].ToString(),
                                //     ParticipantName = item["Name"].ToString(),
                                //     ParticipantRoleName = item["RoleName"].ToString(),
                                //     RSVPStatusModifiedDate= item["RSVPStatusModifiedDate"].ToString()==""?"--": item["RSVPStatusModifiedDate"].ToString(),
                                //     RSVPStatusName = item["RSVPStatus"].ToString(),
                                //     RSVPStatusId = DBNull.Value == item["RSVPStatusId"] ? -1 : Convert.ToInt32(item["RSVPStatusId"])


                                // });

                            }

                        }

                        if (mode == 1 && eventType == 1)
                        {
                            evtDetails.ParticipantCount = evtDetails.ParticipantDetails.Count(x => x.RSVPStatusId == 1);
                        }
                        else if (mode == 1 && eventType == 2)
                        {
                            evtDetails.ParticipantCount = evtDetails.ParticipantDetails.Count(x => x.RSVPStatusName == "Attended" || x.RSVPStatusName == "Walk-in");
                        }
                        else if (mode == 2) {
                      evtDetails.ParticipantCount = evtDetails.ParticipantDetails.Count(x => x.IsAttended == 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return evtDetails;
        }


        public bool InsertEventCheckIn(int eventId, List<string> UserId, List<string> Signature) {

          bool  IsSuccess = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                StaffDetails staffDetails = StaffDetails.GetInstance();

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.CommandText = "SP_SaveStaffEventCheckIn";
                command.Parameters.Add(new SqlParameter("@Agencyid", staffDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId)); // Mang ID
                command.Parameters.Add(new SqlParameter("@EventId", eventId));
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("UserId", typeof(string)),
                     new DataColumn("Signature", typeof(string))

                    });

                if (UserId != null && UserId.Count > 0)
                {
                    for (int i = 0; i < UserId.Count; i++)
                    {
                        if (UserId[i] != "" && Signature[i] != "")
                        {
                            dt.Rows.Add(UserId[i], Signature[i]);

                        }
                    }

                }
                command.Parameters.AddWithValue("@SignData", dt);
                int res = command.ExecuteNonQuery();
                if (res > 1)
                    IsSuccess = true;


            }
            catch (Exception ex) {

                clsError.WriteException(ex);
            }


            return IsSuccess;
        }



        public List<UserBasedEventReport> GetUserBasedEventReport(string Command, int? Center,Guid? Role,List<string> UserIds,Guid? UserIdForEventSummary)
        {
            List<UserBasedEventReport> report = new List<UserBasedEventReport>();
            //  report.EventDetails = new List<EventDetail>();

            StaffDetails staffDetails = StaffDetails.GetInstance();

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@EventCreatorId", staffDetails.UserId));
                command.Parameters.Add(new SqlParameter("@Center", Center == 0 ? null : Center ));
                command.Parameters.Add(new SqlParameter("@Role", Role));
                command.Parameters.Add(new SqlParameter("@Command", Command));
                command.Parameters.Add(new SqlParameter("@UserIdForEventSummary", UserIdForEventSummary)); 
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] {
                    new DataColumn("UserId", typeof(string))

                    });

                if (UserIds != null && UserIds.Count > 0)
                {
                    for (int i = 0; i < UserIds.Count; i++)
                    {
                        if (UserIds[i] != "")
                        {
                            dt.Rows.Add(UserIds[i]);

                        }
                    }

                }
                command.Parameters.AddWithValue("@UserIds", dt);




                command.CommandText = "GetUserBasedEventReport";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        if (Command == "StaffList")
                        {

                            for (int i = 0; i < _dataset.Tables[0].Rows.Count; i++)
                            {
                                var _tempRow = _dataset.Tables[0].Rows[i];

                                report.Add(new UserBasedEventReport()
                                {

                                    UserId = _tempRow["UserId"].ToString(),
                                    RoleId = _tempRow["RoleId"].ToString(),
                                    RoleName = _tempRow["RoleName"].ToString(),
                                    UserName = _tempRow["Name"].ToString(),

                                });

                            }

                        }
                        else if (Command == "StaffReport")
                        {

                            for (int i = 0; i < _dataset.Tables[0].Rows.Count; i++)
                            {
                                var _tempRow = _dataset.Tables[0].Rows[i];

                                report.Add(new UserBasedEventReport()
                                {

                                    UserId = _tempRow["UserId"].ToString(),
                                    RoleId = _tempRow["RoleId"].ToString(),
                                    RoleName = _tempRow["RoleName"].ToString(),
                                    UserName = _tempRow["Name"].ToString(),
                                    SumOfEventsHourPerUser = DBNull.Value == _tempRow["TotalEventHours"] ? 0 : Convert.ToInt32(_tempRow["TotalEventHours"]),
                                    SumOfEventsCEHourPerUser = DBNull.Value == _tempRow["TotalCEHours"] ? 0 : Convert.ToInt32(_tempRow["TotalCEHours"]),
                                      TotalEvent = DBNull.Value == _tempRow["TotalEvent"] ? 0 : Convert.ToInt32(_tempRow["TotalEvent"])

                                });

                            }

                        }
                        else if (Command == "EventSummary") {

                            var EVDetails = new List<EventDetail> ();
                          
                            for (int i = 0; i < _dataset.Tables[0].Rows.Count; i++)
                            {
                                var _tempRow = _dataset.Tables[0].Rows[i];
                                EVDetails.Add(new EventDetail()
                                {
                                   EventName = _tempRow["TrainingName"].ToString(),
                                    Eventid = DBNull.Value == _tempRow["EventId"] ? 0 : Convert.ToInt32(_tempRow["EventId"]),
                                   EventDate = _tempRow["TrainingDate"].ToString(),
                                   Trainer= _tempRow["Trainer"].ToString(),
                                    TotalHours = DBNull.Value == _tempRow["TotalHours"] ? 0 : Convert.ToInt32(_tempRow["TotalHours"]),
                                     ContinuingEducation = DBNull.Value == _tempRow["ContinuingEducation"] ? 0 : Convert.ToInt32(_tempRow["ContinuingEducation"])
                                });


                            }

                            report.Add(new UserBasedEventReport()
                            {

                                UserId = UserIdForEventSummary.ToString(),
                                EventDetails = EVDetails

                            });



                        }
                        else
                        {
                            //string _currentUser = "";
                            //var _tempReport = new UserBasedEventReport();
                            //for (int i = 0; i < _dataset.Tables[0].Rows.Count; i++)
                            //{
                            //    if (_currentUser != _dataset.Tables[0].Rows[i]["UserId"].ToString())
                            //    {  //new User
                            //        if (_currentUser != "")
                            //        {
                            //            report.Add(_tempReport);
                            //        }
                            //        _tempReport = new UserBasedEventReport();
                            //        _tempReport.UserId = _dataset.Tables[0].Rows[i]["UserId"].ToString();
                            //        _tempReport.UserName = _dataset.Tables[0].Rows[i]["Name"].ToString();
                            //        _tempReport.RoleId = _dataset.Tables[0].Rows[i]["RoleId"].ToString();
                            //        _tempReport.RoleName = _dataset.Tables[0].Rows[i]["RoleName"].ToString();
                            //        _currentUser = _dataset.Tables[0].Rows[i]["UserId"].ToString();
                            //    }

                            //    if (_tempReport.EventDetails == null)
                            //    {
                            //        _tempReport.EventDetails = new List<EventDetail>();
                            //    }

                            //    _tempReport.EventDetails.Add(new EventDetail()
                            //    {
                            //        Eventid = Convert.ToInt32(_dataset.Tables[0].Rows[i]["EventId"]),
                            //        EventName = _dataset.Tables[0].Rows[i]["TrainingName"].ToString(),
                            //        EventDate = _dataset.Tables[0].Rows[i]["TrainingDate"].ToString(),
                            //        TotalHours = Convert.ToInt32(_dataset.Tables[0].Rows[i]["TotalHours"]),
                            //        Trainer = _dataset.Tables[0].Rows[i]["Trainer"].ToString(),
                            //        ContinuingEducation = Convert.ToInt32(_dataset.Tables[0].Rows[i]["ContinuingEducation"])

                            //    });

                            //    _tempReport.SumOfEventsHourPerUser = _tempReport.EventDetails.Sum(x => x.TotalHours);
                            //    _tempReport.SumOfEventsCEHourPerUser = _tempReport.EventDetails.Sum(x => x.ContinuingEducation);

                            //    if (_dataset.Tables[0].Rows.Count == i + 1)
                            //    {
                            //        report.Add(_tempReport);
                            //    }

                            //}

                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return report;
        }

        public List<UserDetails> GetUsersforMultipleRoles(List<string> RolesList )
        {

            List<UserDetails> UserList = new List<UserDetails>();
           StaffDetails staffDetails = StaffDetails.GetInstance();

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", staffDetails.AgencyId));
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] {
                    new DataColumn("RoleId", typeof(string)),
                    });
                if (RolesList != null && RolesList.Count > 0)
                {
                    foreach (string roles in RolesList)
                    {
                        dt.Rows.Add(roles);
                    }
                }
                command.Parameters.AddWithValue("@RolesList", dt);
                command.CommandText = "SP_GetUsersforMultipleRoles";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        UserList = (from DataRow dr5 in _dataset.Tables[0].Rows
                                           select new UserDetails
                                           {
                                               StaffName = dr5["StaffName"].ToString(),
                                               UserId = dr5["UserId"].ToString(),
                                               RoleName = dr5["RoleName"].ToString()
                                           }).ToList();



                    }
                }



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return UserList;


        }


    }
}
