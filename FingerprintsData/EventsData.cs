using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FingerprintsModel;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace FingerprintsData
{
    public class EventsData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
       // DataTable _dataTable = null;
        DataSet _dataset = null;

        /// <summary>
        /// method to create/update event from center manager
        /// </summary>
        /// <param name="objEvents"></param>
        /// <returns></returns>
        public bool SaveEvents(Events objEvents)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", objEvents.AgencyId));
                command.Parameters.Add(new SqlParameter("@EventName", objEvents.Workshopname));
                command.Parameters.Add(new SqlParameter("@EventDate", Convert.ToDateTime(objEvents.EventDate)));
                command.Parameters.Add(new SqlParameter("@OpenToCenter", Convert.ToDateTime(objEvents.CenterDate)));
                command.Parameters.Add(new SqlParameter("@OpenToPublic", Convert.ToDateTime(objEvents.OtherCenterDate)));
                command.Parameters.Add(new SqlParameter("@EventTime", objEvents.EventTime));
                command.Parameters.Add(new SqlParameter("@Description", objEvents.Workshopdescription));
                command.Parameters.Add(new SqlParameter("@CenterId", objEvents.CenterId));
                command.Parameters.Add(new SqlParameter("@Duration", !string.IsNullOrEmpty(objEvents.Duration) ? Convert.ToInt32(objEvents.Duration) : 0));
                command.Parameters.Add(new SqlParameter("@Speaker", objEvents.Speaker));
                command.Parameters.Add(new SqlParameter("@Comments", objEvents.Comments));
                command.Parameters.Add(new SqlParameter("@MaxAttendance", objEvents.NoOfSeats));
                if (!string.IsNullOrEmpty(objEvents.RSVPDate))
                    command.Parameters.Add(new SqlParameter("@RSVPCutOffDate", Convert.ToDateTime(objEvents.RSVPDate)));
                command.Parameters.Add(new SqlParameter("@RSVPPoints", objEvents.RSVPPoints));
                command.Parameters.Add(new SqlParameter("@AttendPoints", objEvents.AttendancePoints));
                command.Parameters.Add(new SqlParameter("@Budget", !string.IsNullOrEmpty(objEvents.Budget) ? Convert.ToInt64(objEvents.Budget) : 0));
                command.Parameters.Add(new SqlParameter("@Actual", !string.IsNullOrEmpty(objEvents.ActualCosts) ? Convert.ToInt64(objEvents.ActualCosts) : 0));
                command.Parameters.Add(new SqlParameter("@HourlyRate", !string.IsNullOrEmpty(objEvents.HourlyRate) ? Convert.ToDecimal(objEvents.HourlyRate) : 0));
                command.Parameters.Add(new SqlParameter("@MileageRate", !string.IsNullOrEmpty(objEvents.MileageRate) ? Convert.ToDecimal(objEvents.MileageRate) : 0));
                command.Parameters.Add(new SqlParameter("@ImagePath", objEvents.ImagePath));
                command.Parameters.Add(new SqlParameter("@Workshopid", objEvents.Workshopid));
                command.Parameters.Add(new SqlParameter("@EventStatus", objEvents.EventStatus));
                command.Parameters.Add(new SqlParameter("@Color", objEvents.Color));
                command.Parameters.Add(new SqlParameter("@CreatedBy", objEvents.UserId));

                command.Parameters.Add(new SqlParameter("@EventStatusChangeId", objEvents.EventStatusChange));
                command.Parameters.Add(new SqlParameter("@EventDateChangeId", objEvents.EventDateChange));
                command.Parameters.Add(new SqlParameter("@EventTimeChangeId", objEvents.EventTimeChange));

                command.Parameters.Add(new SqlParameter("@EventStatusChangeDesc", objEvents.EventStatusDescription));
                command.Parameters.Add(new SqlParameter("@EventDateChangeDesc", objEvents.EventDateDescription));
                command.Parameters.Add(new SqlParameter("@EventTimeChangeDesc", objEvents.EventTimeDescription));
                command.Parameters.Add(new SqlParameter("@EventId", objEvents.Eventid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SaveWorkshopDetail";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
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
            }
            return isInserted;
        }

        /// <summary>
        /// method to get regeistered parent emails if the event/workshop is changed by Center Manager
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public Events GetRegisteredParentEmailData(Events events)
        {

            RegisteredMembers household = new RegisteredMembers();
            List<SelectListItem> householdList = new List<SelectListItem>();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@WorkShopId", events.Workshopid));
                command.Parameters.Add(new SqlParameter("@UserId", events.UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", Convert.ToInt64(events.CenterId)));
                command.Parameters.Add(new SqlParameter("@EventId", Convert.ToInt64(events.Eventid)));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetRegisteredParentEmaiForEvent";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {


                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            events = new Events
                            {
                                Workshopname = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["WorkShopName"].ToString()) ? _dataset.Tables[0].Rows[0]["WorkShopName"].ToString() : "",
                                CenterName = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["CenterName"].ToString()) ? _dataset.Tables[0].Rows[0]["CenterName"].ToString() : "",
                                EventDate = !string.IsNullOrEmpty(Convert.ToDateTime(_dataset.Tables[0].Rows[0]["EventDate"]).ToString("MM/dd/yyyy")) ? Convert.ToDateTime(_dataset.Tables[0].Rows[0]["EventDate"]).ToString("MM/dd/yyyy") : "",
                                EventTime = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["EventTime"].ToString()) ? GetFormattedTime(_dataset.Tables[0].Rows[0]["EventTime"].ToString()) : "",
                                EventDateDescription = _dataset.Tables[0].Rows[0]["EventDateDescription"].ToString(),
                                EventTimeDescription = _dataset.Tables[0].Rows[0]["EventTimeDescription"].ToString(),
                                EventStatusDescription = _dataset.Tables[0].Rows[0]["EventStatusDescription"].ToString()

                            };
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {

                                householdList.Add(new SelectListItem
                                {
                                    Text = dr["FullName"].ToString(),
                                    Value = dr["EmailId"].ToString()
                                });


                            }
                            events.RegisteredMembers = householdList;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return events;
        }

        /// <summary>
        /// method to get the cancelled workshops for parent portal.
        /// </summary>
        /// <param name="eventsList"></param>
        /// <param name="events"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool GetCancelledWorkShopForParent(ref List<EventsList> eventsList, EventsList events, int mode)
        {
            bool isResult = false;


            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@EmailId", events.EmailId));
                command.Parameters.Add(new SqlParameter("@UserId", events.UserId));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CancelledEventsForParent";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);

                if (mode == 1)
                {
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr1 in _dataset.Tables[0].Rows)
                            {
                                isResult = Convert.ToBoolean(dr1["IsExists"]);
                                break;
                            }
                        }
                    }
                }

                else if (mode == 2)
                {

                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr2 in _dataset.Tables[0].Rows)
                            {
                                events = new EventsList
                                {
                                    EventColor = dr2["Color"].ToString(),
                                    WorkShopName = dr2["WorkshopName"].ToString(),
                                    EventDate = dr2["EventDate"].ToString(),
                                    EventTime = dr2["EventTime"].ToString(),
                                    Enc_CenterId = EncryptDecrypt.Encrypt64(dr2["CenterId"].ToString()),
                                    CenterId = Convert.ToInt64(dr2["CenterId"]),
                                    CenterName = dr2["CenterName"].ToString(),
                                    Enc_EventId = EncryptDecrypt.Encrypt64(dr2["EventId"].ToString()),
                                    EventId = Convert.ToInt64(dr2["EventId"])
                                };
                                eventsList.Add(events);
                            }
                        }
                    }


                }


                return isResult;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return isResult;
            }
        }

        public ParentRegisterEvent GetCancelledWorkShopInfo(EventsList events, int mode)
        {
            ParentHousehold household = new ParentHousehold();
            List<ParentHousehold> householdList = new List<ParentHousehold>();
            ParentRegisterEvent parentRegisterEvent = new ParentRegisterEvent();
            string emailId = events.EmailId;
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@EmailId", events.EmailId));
                command.Parameters.Add(new SqlParameter("@UserId", events.UserId));
                command.Parameters.Add(new SqlParameter("@EventId", events.EventId));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CancelledEventsForParent";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (mode == 3)
                {
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                events = new EventsList
                                {
                                    EventId = Convert.ToInt64(dr["EventId"]),
                                    Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                    // EventName = dr["EventName"].ToString(),
                                    EventDate = Convert.ToDateTime(dr["EventDate"]).ToString("MM/dd/yyyy"),
                                    EventTime = GetFormattedTime(dr["EventTime"].ToString()),
                                    SpeakerName = dr["Speaker"].ToString(),
                                    OpenToPublicDate = dr["OpenToPublic"].ToString(),
                                    WorkShopName = dr["WorkshopName"].ToString(),
                                    WokShopId = Convert.ToInt64(dr["WorkShopId"].ToString()),
                                    EventDescription = dr["Description"].ToString(),
                                    ProgramYear = dr["ProgramYear"].ToString(),
                                    CenterId = Convert.ToInt64(dr["CenterId"]),
                                    Enc_CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                                    TotalDuration = dr["Duration"].ToString(),
                                    RSVPCutOffDate = (dr["RSVPCutOffDate"].ToString() == "") ? "" : Convert.ToDateTime(dr["RSVPCutOffDate"]).ToString("MM/dd/yyyy"),
                                    CenterAddress = dr["CenterAddress"].ToString(),
                                    ImagePath = dr["ImagePath"].ToString(),
                                    MaxAttend = dr["MaxAttendance"].ToString(),
                                    EventChangeReason=dr["ReasonForDateChange"].ToString()+","+dr["ReasonForStatusChange"].ToString()+","+dr["ReasonForTimeChange"].ToString(),

                                    CenterName = dr["CenterName"].ToString()
                                };
                            }

                            var list2 = events.EventChangeReason.Split(',').ToArray();
                            List<string> str = new List<string>();
                            if(list2.Count()>0)
                            {
                                foreach(var item in list2)
                                {
                                    if(item!="")
                                    {
                                        str.Add(item);
                                    }

                                }
                                events.EventChangeReason = string.Join(",", str.ToArray());
                            }
                          
                        }
                    }

                    if (_dataset.Tables[1] != null)
                    {
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {


                            foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                            {
                                household = new ParentHousehold
                                {
                                    FullName = dr1["FullName"].ToString(),
                                    ClientId = Convert.ToInt64(dr1["ClientId"]),
                                    Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString()),
                                    Gender = dr1["GenderType"].ToString(),
                                    EmailId = dr1["EmailId"].ToString(),
                                    IsChild = dr1["IsChild"].ToString(),
                                    IsFamily = dr1["IsFamily"].ToString(),
                                    IsSelected = (dr1["EmailId"].ToString() == emailId),
                                    // IsOther=Convert.ToBoolean(dr1["IsOther"]),
                                    Enc_RSVPId = "0"
                                };




                                householdList.Add(household);
                            }
                        }
                    }

                    if (_dataset.Tables[2] != null)
                    {

                        if (_dataset.Tables[2].Rows.Count > 0)
                        {
                            events.IsUpdate = true;

                            foreach (DataRow dr2 in _dataset.Tables[2].Rows)
                            {

                                long clientid = Convert.ToInt64(dr2["ClientId"].ToString());
                                if (clientid != 0)
                                {
                                    householdList.Where(w => w.ClientId == clientid).ToList().ForEach(w =>
                                    {
                                        w.IsRegistered = true;
                                        w.EventRSVPId = Convert.ToInt64(dr2["EventRSVPId"]);
                                        w.Enc_RSVPId = EncryptDecrypt.Encrypt64(dr2["EventRSVPId"].ToString());
                                        w.ProfilePic = (string.IsNullOrEmpty(dr2["ProfilePic"].ToString())) ? "" : Convert.ToBase64String((byte[])dr2["ProfilePic"]);
                                        w.IsOther = Convert.ToBoolean(dr2["IsOther"]);
                                        w.Signature = dr2["Signature"].ToString();
                                    });

                                }
                                else
                                {
                                    household = new ParentHousehold
                                    {
                                        FullName = dr2["FullName"].ToString(),
                                        ClientId = Convert.ToInt64(dr2["ClientId"]),
                                        Enc_ClientId = EncryptDecrypt.Encrypt64(dr2["ClientId"].ToString()),
                                        Gender = dr2["GenderType"].ToString(),
                                        IsRegistered = true,
                                        IsOther = Convert.ToBoolean(dr2["IsOther"]),
                                        EventRSVPId = Convert.ToInt64(dr2["EventRSVPId"]),
                                        Enc_RSVPId = EncryptDecrypt.Encrypt64(dr2["EventRSVPId"].ToString()),
                                        Signature = dr2["Signature"].ToString(),
                                        ProfilePic = (string.IsNullOrEmpty(dr2["ProfilePic"].ToString())) ? "" : Convert.ToBase64String((byte[])dr2["ProfilePic"])

                                    };
                                    householdList.Add(household);
                                }

                                events.Signature = dr2["Signature"].ToString();
                                events.IsAttended = Convert.ToBoolean(dr2["Attended"]);
                                events.MilesDriven = Convert.ToDouble(dr2["MilesDriven"]);
                                events.TimeTaken = dr2["TimeTaken"].ToString();
                            }
                        }
                    }
                }
                parentRegisterEvent = new ParentRegisterEvent
                {
                    Events = events,
                    HouseholdList = householdList
                };
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
            return parentRegisterEvent;
        }

        public string GetCenterAddress(string Id)
        {
            string Address = "";
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@CenterId", Id));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetCenterAddressByCenterId";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
                Object objAddress = command.ExecuteScalar();
                if (objAddress != null)
                    Address = objAddress.ToString();
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
            return Address;
        }

        public void GetWorkshopDetails(ref Events objEvents, string Id)
        {
            // string Address = "";
            try
            {
                command = new SqlCommand();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Workshopid", Id));
                command.Parameters.Add(new SqlParameter("@CenterId", objEvents.CenterId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetWorkShopDetails";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0].Rows.Count > 0)
                {
                    objEvents.Workshopdescription = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Description"].ToString()) ? _dataset.Tables[0].Rows[0]["Description"].ToString() : "";
                    objEvents.Speaker = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Speaker"].ToString()) ? _dataset.Tables[0].Rows[0]["Speaker"].ToString() : "";
                    objEvents.Comments = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Comments"].ToString()) ? _dataset.Tables[0].Rows[0]["Comments"].ToString() : "";
                    objEvents.EventStatus = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["EventStatus"].ToString()) ? _dataset.Tables[0].Rows[0]["EventStatus"].ToString() : "";
                    objEvents.EventDate = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["EventDate"].ToString()) ? _dataset.Tables[0].Rows[0]["EventDate"].ToString() : "";
                    objEvents.CenterDate = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["OpenToCenter"].ToString()) ? _dataset.Tables[0].Rows[0]["OpenToCenter"].ToString() : "";
                    objEvents.OtherCenterDate = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["OpenToPublic"].ToString()) ? _dataset.Tables[0].Rows[0]["OpenToPublic"].ToString() : "";
                    objEvents.RSVPDate = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["RSVPCutOffDate"].ToString()) ? _dataset.Tables[0].Rows[0]["RSVPCutOffDate"].ToString() : "";
                    objEvents.EventTime = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Time"].ToString()) ? GetFormattedTime(_dataset.Tables[0].Rows[0]["Time"].ToString()) : "";
                    objEvents.Duration = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Duration"].ToString()) ? _dataset.Tables[0].Rows[0]["Duration"].ToString() : "0";
                    objEvents.NoOfSeats = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["MaxAttendance"].ToString()) ? _dataset.Tables[0].Rows[0]["MaxAttendance"].ToString() : "";
                    objEvents.RSVPPoints = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["RSVPPoints"].ToString()) ? _dataset.Tables[0].Rows[0]["RSVPPoints"].ToString() : "";
                    objEvents.Budget = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Budget"].ToString()) ? _dataset.Tables[0].Rows[0]["Budget"].ToString() : "";
                    objEvents.ActualCosts = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Actual"].ToString()) ? _dataset.Tables[0].Rows[0]["Actual"].ToString() : "";
                    objEvents.AttendancePoints = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["AttendPoints"].ToString()) ? _dataset.Tables[0].Rows[0]["AttendPoints"].ToString() : "";
                    objEvents.HourlyRate = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["HourlyRate"].ToString()) ? _dataset.Tables[0].Rows[0]["HourlyRate"].ToString() : "";
                    objEvents.MileageRate = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["MileageRate"].ToString()) ? _dataset.Tables[0].Rows[0]["MileageRate"].ToString() : "";
                    objEvents.CenterId = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["CenterId"].ToString()) ? _dataset.Tables[0].Rows[0]["CenterId"].ToString() : "0";
                    objEvents.Color = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Color"].ToString()) ? _dataset.Tables[0].Rows[0]["Color"].ToString() : "";
                    objEvents.Eventid = Convert.ToInt32(_dataset.Tables[0].Rows[0]["EventId"].ToString());
                    objEvents.MinutesDiff = Convert.ToInt64(_dataset.Tables[0].Rows[0]["MinuteDiff"]);
                    objEvents.Eventid = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["EventId"].ToString()) ? Convert.ToInt32(_dataset.Tables[0].Rows[0]["EventId"]) : 0;
                    string Imagepath = !string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["ImagePath"].ToString()) ? _dataset.Tables[0].Rows[0]["ImagePath"].ToString() : "";
                    string[] arr = Imagepath.Split('/');
                    if (arr.Length > 0)
                        objEvents.ImagePath = arr[arr.Length - 1];
                    else
                        objEvents.ImagePath = Imagepath;
                    objEvents.IsUpdate = !string.IsNullOrEmpty(objEvents.Speaker);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                //  if (Connection != null)
                //   Connection.Close();
            }
            // return Address;
        }

        public EventsModal GetEventsListData(EventsList events)
        {

            List<EventsList> eventsList = new List<EventsList>();
            List<EventsCenter> centerList = new List<EventsCenter>();
            EventsCenter center = new EventsCenter();
            EventsModal eventsModule = new EventsModal();

            try
            {

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@userId", events.UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", events.CenterId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEventsList";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            events = new EventsList
                            {
                                EventId = Convert.ToInt64(dr["EventId"]),
                                EventName = dr["EventName"].ToString(),
                                EventDate = Convert.ToDateTime(dr["EventDate"]).ToString("MM/dd/yyyy"),
                                EventTime = GetFormattedTime(dr["EventTime"].ToString()),
                                WorkShopName = dr["WorkShopName"].ToString(),
                                EventStatus = dr["EventStatus"].ToString(),
                                Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                EventColor = dr["EventColor"].ToString()
                            };
                            eventsList.Add(events);

                        }
                    }

                }


                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {

                            center = new EventsCenter
                            {
                                CenterId = Convert.ToInt64(dr["CenterId"]),
                                Enc_CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                                CenterName = dr["CenterName"].ToString()
                            };
                            centerList.Add(center);

                        }
                    }

                }

                eventsModule._EventsList = eventsList;
                eventsModule._CenterList = centerList;
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
            return eventsModule;
        }

        public List<EventsList> GetRegisteredEvent(EventsList events)
        {
            List<EventsList> eventsList = new List<EventsList>();

            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@userId", events.UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_ParentRegisteredEvent";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            events = new EventsList
                            {
                                EventId = Convert.ToInt64(dr["EventId"]),
                                Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                WorkShopName = dr["WorkShopName"].ToString(),
                                EventDate = Convert.ToDateTime(dr["EventDate"]).ToString("MM/dd/yyyy"),
                                EventTime = GetFormattedTime(dr["EventTime"].ToString()),
                                EventDateFormat = Convert.ToDateTime(dr["EventDate"]).ToString("yyyy-MM-dd"),
                                CenterName = dr["CenterName"].ToString(),
                                EventStatus = dr["EventStatus"].ToString(),
                                MinDiff = Convert.ToInt64(dr["MinuteDiff"]),
                                EventColor = dr["EventColor"].ToString(),
                                IsOther = Convert.ToBoolean(dr["IsOther"]),
                                MaxAttend = dr["MaxAttendance"].ToString(),
                                TotalRegistered = Convert.ToInt64(dr["TotalRegistered"]),
                                AvailableStatus = dr["AvailStatus"].ToString(),
                                Signature = dr["Signature"].ToString(),
                                IsAttended = Convert.ToBoolean(dr["Attended"]),

                            };

                            eventsList.Add(events);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return eventsList;
        }

        public List<EventsCalender> GetRegisteredEventCalendar(EventsList events)
        {
            List<EventsCalender> calenderEveList = new List<EventsCalender>();
            EventsCalender calender = new EventsCalender();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@userId", events.UserId));
                command.Parameters.Add(new SqlParameter("@EmailId", events.EmailId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_ParentRegisteredEvent";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            DateTime startDateTime = Convert.ToDateTime(dr["EventDate"]).Add(TimeSpan.Parse(dr["EventTime"].ToString()));
                            DateTime endDateTime = startDateTime.AddMinutes(Convert.ToDouble(dr["Duration"]));

                            calender = new EventsCalender
                            {
                                title = dr["WorkShopName"].ToString(),
                                id = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                description = (dr["AvailStatus"].ToString() == "Open") ? "<span class='openstatus'>Open</span>" : (dr["AvailStatus"].ToString() == "Full") ? "<span class='fullstatus'>Full</span>" : "<span class='openstatus'>Available</span>",
                                start = startDateTime.ToString(),
                                end = endDateTime.ToString(),
                                allDay = false,
                                IsOther = Convert.ToBoolean(dr["IsOther"]),
                                color = (Convert.ToBoolean(dr["IsOther"])) ? "#4472c4" : "#ff0000",
                                IsClick = true,

                            };

                            calenderEveList.Add(calender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return calenderEveList;
        }
        public List<EventsCalender> GetRegisteredEventCount(EventsList events)
        {
            List<EventsCalender> calenderEveList = new List<EventsCalender>();
            EventsCalender calender = new EventsCalender();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@userId", events.UserId));
                command.Parameters.Add(new SqlParameter("@EmailId", events.EmailId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetRegisterEventCountCalender";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            calender = new EventsCalender
                            {
                                start = Convert.ToDateTime(dr["EventDate"]).ToString("yyyy-MM-dd"),
                                end = Convert.ToDateTime(dr["EventDate"]).ToString("yyyy-MM-dd"),
                                allDay = false,
                                icon = "fa-calendar-check-o",
                                IsClick = false,
                                title = dr["EventsOnDate"].ToString(),
                                color = "#4472c4",
                                className = "calender-notify",

                            };

                            calenderEveList.Add(calender);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return calenderEveList;
        }
        public ParentSelectionEvent GetParentEventSelectionData(EventsList events, string emailId)
        {

            List<ParentEvent> parentEventList = new List<ParentEvent>();
            ParentEvent _parentEvents = new ParentEvent();
            ParentSelectionEvent parentSelectionEvents = new ParentSelectionEvent();
            EventsCenter _eventCenter = new EventsCenter();
            EventsList _events = new EventsList();
            List<EventsCenter> centerList = new List<EventsCenter>();
            List<EventsList> eventsList = new List<EventsList>();
            long selfCenterId = 0;
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@userId", events.UserId));
                command.Parameters.Add(new SqlParameter("@EmailId", emailId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEventsForParent";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);

                if (_dataset.Tables[3] != null)
                {
                    if (_dataset.Tables[3].Rows.Count > 0)
                    {
                        selfCenterId = Convert.ToInt64(_dataset.Tables[3].Rows[0]["SelfCenter"]);
                    }
                }

                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            _events = new EventsList
                            {
                                EventId = Convert.ToInt64(dr["EventId"]),
                                Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                EventName = dr["EventName"].ToString(),
                                EventDate = Convert.ToDateTime(dr["EventDate"]).ToString("MM/dd/yyyy"),
                                EventTime = GetFormattedTime(dr["EventTime"].ToString()),
                                SpeakerName = dr["Speaker"].ToString(),
                                OpenToPublicDate = dr["OpenToPublic"].ToString(),
                                WorkShopName = dr["WorkshopName"].ToString(),
                                WokShopId = Convert.ToInt64(dr["WorkShopId"].ToString()),
                                EventDescription = dr["Description"].ToString(),
                                ProgramYear = dr["ProgramYear"].ToString(),
                                CenterId = Convert.ToInt64(dr["CenterId"]),
                                IsRegistered = Convert.ToBoolean(dr["Registered"]),
                                EventColor = dr["Color"].ToString()
                            };
                            eventsList.Add(_events);
                        }



                    }
                }
                if (_dataset.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow _row in _dataset.Tables[2].Rows)
                    {
                        if (Convert.ToInt64(_row["CenterId"]) == selfCenterId)
                        {
                            _eventCenter = new EventsCenter
                            {
                                CenterId = Convert.ToInt64(_row["CenterId"].ToString()),
                                Enc_CenterId = EncryptDecrypt.Encrypt64(_row["CenterId"].ToString()),
                                CenterName = _row["CenterName"].ToString(),
                                CenterAddress = _row["CenterAddress"].ToString()
                            };
                        }
                    }
                    _eventCenter._EventsList = eventsList;
                }
                centerList.Add(_eventCenter);
                parentSelectionEvents.SelfCenterList = centerList;

                if (_dataset.Tables[2].Rows.Count > 0)
                {
                    centerList = new List<EventsCenter>();

                    foreach (DataRow _row1 in _dataset.Tables[2].Rows)
                    {
                        if (Convert.ToInt64(_row1["CenterId"]) != selfCenterId)
                        {
                            _eventCenter = new EventsCenter
                            {
                                CenterId = Convert.ToInt64(_row1["CenterId"].ToString()),
                                Enc_CenterId = EncryptDecrypt.Encrypt64(_row1["CenterId"].ToString()),
                                CenterName = _row1["CenterName"].ToString(),
                                CenterAddress = _row1["CenterAddress"].ToString()
                            };
                            eventsList = new List<EventsList>();
                            if (_dataset.Tables[1] != null)
                            {
                                if (_dataset.Tables[1].Rows.Count > 0)
                                {

                                    foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                                    {
                                        if (_eventCenter.CenterId == Convert.ToInt64(dr1["CenterId"]))
                                        {
                                            _events = new EventsList
                                            {
                                                EventId = Convert.ToInt64(dr1["EventId"]),
                                                Enc_EventId = EncryptDecrypt.Encrypt64(dr1["EventId"].ToString()),
                                                EventName = dr1["EventName"].ToString(),
                                                EventDate = Convert.ToDateTime(dr1["EventDate"]).ToString("MM/dd/yyyy"),
                                                EventTime = GetFormattedTime(dr1["EventTime"].ToString()),
                                                SpeakerName = dr1["Speaker"].ToString(),
                                                OpenToPublicDate = dr1["OpenToPublic"].ToString(),
                                                WorkShopName = dr1["WorkshopName"].ToString(),
                                                WokShopId = Convert.ToInt64(dr1["WorkShopId"].ToString()),
                                                EventDescription = dr1["Description"].ToString(),
                                                ProgramYear = dr1["ProgramYear"].ToString(),
                                                IsRegistered = Convert.ToBoolean(dr1["Registered"]),
                                                EventColor = dr1["Color"].ToString()
                                            };
                                            eventsList.Add(_events);
                                        }

                                    }


                                }
                            }
                            _eventCenter._EventsList = eventsList;
                            centerList.Add(_eventCenter);
                        }
                    }

                }
                parentSelectionEvents.OtherCentersList = centerList;



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
            return parentSelectionEvents;
        }

        public ParentRegisterEvent GetEventsForRegisterData(EventsList events, string emailId)
        {

            ParentRegisterEvent parentRegister = new ParentRegisterEvent();
            ParentHousehold household = new ParentHousehold();
            List<ParentHousehold> householdList = new List<ParentHousehold>();

            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", events.UserId));
                command.Parameters.Add(new SqlParameter("@EventId", events.EventId));
                command.Parameters.Add(new SqlParameter("@EmailId", emailId));
                command.Parameters.Add(new SqlParameter("@ClientId", events.ClientId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEventsForRegistraion";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            events = new EventsList
                            {
                                EventId = Convert.ToInt64(dr["EventId"]),
                                Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                EventDate = Convert.ToDateTime(dr["EventDate"]).ToString("MM/dd/yyyy"),
                                EventTime = GetFormattedTime(dr["EventTime"].ToString()),
                                SpeakerName = dr["Speaker"].ToString(),
                                OpenToPublicDate = dr["OpenToPublic"].ToString(),
                                WorkShopName = dr["WorkshopName"].ToString(),
                                WokShopId = Convert.ToInt64(dr["WorkShopId"].ToString()),
                                EventDescription = dr["Description"].ToString(),
                                ProgramYear = dr["ProgramYear"].ToString(),
                                CenterId = Convert.ToInt64(dr["CenterId"]),
                                TotalDuration = dr["Duration"].ToString(),
                                RSVPCutOffDate = (dr["RSVPCutOffDate"].ToString() == "") ? "" : Convert.ToDateTime(dr["RSVPCutOffDate"]).ToString("MM/dd/yyyy"),
                                CenterAddress = dr["CenterAddress"].ToString(),
                                ImagePath = dr["ImagePath"].ToString(),
                                MaxAttend = dr["MaxAttendance"].ToString(),
                                TotalRegistered = Convert.ToInt64(dr["TotalRegistered"]),
                                AvailableSlots = Convert.ToInt64(dr["AvailableSeats"]),
                                CenterName = dr["CenterName"].ToString()
                            };
                        }
                    }
                }

                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {


                        foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                        {
                            household = new ParentHousehold
                            {
                                FullName = dr1["FullName"].ToString(),
                                ClientId = Convert.ToInt64(dr1["ClientId"]),
                                Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString()),
                                Gender = dr1["GenderType"].ToString(),
                                EmailId = dr1["EmailId"].ToString(),
                                IsChild = dr1["IsChild"].ToString(),
                                IsFamily = dr1["IsFamily"].ToString(),
                                IsSelected = (dr1["EmailId"].ToString() == emailId),
                                Enc_RSVPId = "0"
                            };




                            householdList.Add(household);
                        }
                    }
                }

                if (_dataset.Tables[2] != null)
                {

                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        events.IsUpdate = true;

                        foreach (DataRow dr2 in _dataset.Tables[2].Rows)
                        {

                            long clientid = Convert.ToInt64(dr2["ClientId"].ToString());
                            if (clientid != 0)
                            {
                                householdList.Where(w => w.ClientId == clientid).ToList().ForEach(w =>
                                {
                                    w.IsRegistered = true;
                                    w.EventRSVPId = Convert.ToInt64(dr2["EventRSVPId"]);
                                    w.Enc_RSVPId = EncryptDecrypt.Encrypt64(dr2["EventRSVPId"].ToString());
                                    w.ProfilePic = (string.IsNullOrEmpty(dr2["ProfilePic"].ToString())) ? "" : Convert.ToBase64String((byte[])dr2["ProfilePic"]);
                                    w.IsOther = Convert.ToBoolean(dr2["IsOther"]);
                                    w.Signature = dr2["Signature"].ToString();
                                });

                            }
                            else
                            {
                                household = new ParentHousehold
                                {
                                    FullName = dr2["FullName"].ToString(),
                                    ClientId = Convert.ToInt64(dr2["ClientId"]),
                                    Enc_ClientId = EncryptDecrypt.Encrypt64(dr2["ClientId"].ToString()),
                                    Gender = dr2["GenderType"].ToString(),
                                    IsRegistered = true,
                                    IsOther = Convert.ToBoolean(dr2["IsOther"]),
                                    EventRSVPId = Convert.ToInt64(dr2["EventRSVPId"]),
                                    Enc_RSVPId = EncryptDecrypt.Encrypt64(dr2["EventRSVPId"].ToString()),
                                    Signature = dr2["Signature"].ToString(),
                                    ProfilePic = (string.IsNullOrEmpty(dr2["ProfilePic"].ToString())) ? "" : Convert.ToBase64String((byte[])dr2["ProfilePic"])

                                };
                                householdList.Add(household);
                            }

                            events.Signature = dr2["Signature"].ToString();
                            events.IsAttended = Convert.ToBoolean(dr2["Attended"]);
                            events.MilesDriven = Convert.ToDouble(dr2["MilesDriven"]);
                            events.TimeTaken = dr2["TimeTaken"].ToString();
                        }
                    }
                }
                if (_dataset.Tables[3] != null)
                {
                    if (_dataset.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr3 in _dataset.Tables[3].Rows)
                        {
                            events.TotalCount = Convert.ToInt64(dr3["TotalCount"]);
                            events.CurrentCount = Convert.ToInt64(dr3["CurrentCount"]);
                        }
                    }
                }

                parentRegister = new ParentRegisterEvent
                {
                    Events = events,
                    HouseholdList = householdList
                };

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
            return parentRegister;

        }


        public RegisteredFeeback AcceptSignatureParent(ParentRegisterEvent parentEvent)
        {

            DataTable eventDt = new DataTable();
            RegisteredFeeback regFeedBack = new RegisteredFeeback();
            try
            {

                parentEvent.Events.ClientId = (parentEvent.Events.Enc_ClientId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(parentEvent.Events.Enc_ClientId));
                parentEvent.Events.EventId = (parentEvent.Events.Enc_EventId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(parentEvent.Events.Enc_EventId));
                eventDt = GetDataTableFromList(parentEvent);

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", parentEvent.Events.AgencyId));
                command.Parameters.Add(new SqlParameter("@EventId", parentEvent.Events.EventId));
                command.Parameters.Add(new SqlParameter("@ClientId", parentEvent.Events.ClientId));
                command.Parameters.Add(new SqlParameter("@tblEventRsvp", eventDt));
                command.CommandText = "USP_InsertParentSigForEvents";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            regFeedBack = new RegisteredFeeback
                            {
                                AvailableSlots = Convert.ToInt64(string.Format("0{0:00}", dr["AvailSlots"])),
                                MaxAttendance = Convert.ToInt64(dr["MaxAttendance"]),
                                NewHousholds = Convert.ToInt64(dr["NewlyAdded"]),
                                TotalRegisterd = Convert.ToInt64(dr["NewlyAdded"]),
                                RegStatus = Convert.ToBoolean(dr["Status"])
                            };
                        }

                    }
                }

                return regFeedBack;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return regFeedBack;
            }

        }

        public DataTable GetDataTableFromList(ParentRegisterEvent parentRegEvent)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.AddRange(new DataColumn[25] {
                    new DataColumn("EventRSVPId ", typeof(long)),
                    new DataColumn("Registered",typeof(bool)),
                    new DataColumn("EventId",typeof(long)),
                    new DataColumn("ClientId",typeof(long)),
                    new DataColumn("UserId",typeof(Guid)),
                    new DataColumn("RegDate",typeof(DateTime)),

                    new DataColumn("EventPoints ", typeof(string)),
                    new DataColumn("RSVPPoints",typeof(string)),
                    new DataColumn("MilesDriven",typeof(double)),
                    new DataColumn("TimeTaken",typeof(string)),
                    new DataColumn("TimeAmount",typeof(double)),
                    new DataColumn("Attended",typeof(bool)),

                    new DataColumn("Gender",typeof(string)),
                    new DataColumn("ProgrmYear",typeof(string)),
                    new DataColumn("Signature ", typeof(string)),
                    new DataColumn("AgencyId",typeof(Guid)),
                    new DataColumn("CreatedBy",typeof(Guid)),
                    new DataColumn("ModifiedBy",typeof(Guid)),

                    new DataColumn("CreatedDate",typeof(DateTime)),
                    new DataColumn("ModifiedDate",typeof(DateTime)),
                    new DataColumn("Status",typeof(bool)),
                    new DataColumn("ClientId2 ", typeof(long)),
                    new DataColumn("FullName",typeof(string)),
                    new DataColumn("HousholdId",typeof(long)),

                    new DataColumn("IsOther",typeof(bool))

                });


                foreach (ParentHousehold household in parentRegEvent.HouseholdList)
                {
                    string gender = (household.Gender == "Male") ? "1" : (household.Gender == "Female") ? "2" : "3";
                    household.ClientId = (string.IsNullOrEmpty(household.Enc_ClientId)) ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(household.Enc_ClientId));
                    household.IsOther = (household.ClientId == 0);
                    household.EventRSVPId = (household.Enc_RSVPId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(household.Enc_RSVPId).ToString());

                    dt.Rows.Add(
                        household.EventRSVPId,
                        household.IsRegistered,
                        parentRegEvent.Events.EventId,
                        parentRegEvent.Events.ClientId,
                        parentRegEvent.Events.UserId,
                        DateTime.Now,
                        0,
                        0,
                        parentRegEvent.Events.MilesDriven,
                        parentRegEvent.Events.TimeTaken,
                        0,
                        household.IsAttended,
                        gender,
                        string.Empty,
                        household.Signature,
                        parentRegEvent.Events.AgencyId,
                        parentRegEvent.Events.UserId,
                        parentRegEvent.Events.UserId,
                        DateTime.Now,
                        DateTime.Now,
                        household.Status,
                        household.ClientId,
                        household.FullName,
                        0,
                        household.IsOther
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

        public ShowEventDetails GetEventDetails(string id, Guid agencyId)
        {
            ShowEventDetails eventDetails = new ShowEventDetails();
            RegisteredMembers members = new RegisteredMembers();
            List<RegisteredMembers> householdList = new List<RegisteredMembers>();
            EventsList events = new EventsList();
            try
            {
                long eventId = Convert.ToInt64(EncryptDecrypt.Decrypt64(id));
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@EventId", eventId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEventDetails";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            events = new EventsList
                            {
                                EventId = Convert.ToInt64(dr["EventId"]),
                                Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                EventName = dr["EventName"].ToString(),
                                EventDate = Convert.ToDateTime(dr["EventDate"]).ToString("MM/dd/yyyy"),
                                SpeakerName = dr["Speaker"].ToString(),
                                OpenToPublicDate = dr["OpenToPublic"].ToString(),
                                WorkShopName = dr["WorkshopName"].ToString(),
                                WokShopId = Convert.ToInt64(dr["WorkShopId"].ToString()),
                                EventDescription = dr["Description"].ToString(),
                                ProgramYear = dr["ProgramYear"].ToString(),
                                CenterId = Convert.ToInt64(dr["CenterId"]),
                                TotalDuration = dr["Duration"].ToString(),
                                RSVPCutOffDate = Convert.ToDateTime(dr["RSVPCutOffDate"]).ToString("MM/dd/yyyy"),
                                CenterAddress = dr["CenterAddress"].ToString(),
                                ImagePath = dr["ImagePath"].ToString(),
                                MaxAttend = dr["MaxAttendance"].ToString(),
                                AvailableSlots = Convert.ToInt64(dr["AvailableSeats"]),
                                CenterName = dr["CenterName"].ToString(),
                                EventStatus = dr["EventStatus"].ToString()
                            };
                        }
                    }
                }

                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                        {
                            members = new RegisteredMembers
                            {
                                FullName = dr1["FullName"].ToString(),
                                CenterName = dr1["CenterName"].ToString(),
                                CenterAddress = dr1["CenterAddress"].ToString(),
                                Category = dr1["Category"].ToString(),
                                IsOther = Convert.ToBoolean(dr1["IsOther"]),
                                IsOtherCenter = Convert.ToBoolean(dr1["OtherCenter"])
                            };
                            householdList.Add(members);
                        }
                    }
                }

                eventDetails = new ShowEventDetails
                {
                    Events = events,
                    RegisteredUsers = householdList
                };

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
            return eventDetails;
        }

        public RegisteredFeeback RegisterForEventData(ParentRegisterEvent parentEvent, int mode)
        {
            DataTable eventDt = new DataTable();
            RegisteredFeeback regFeedBack = new RegisteredFeeback();
            try
            {

                parentEvent.Events.ClientId = (parentEvent.Events.Enc_ClientId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(parentEvent.Events.Enc_ClientId));
                parentEvent.Events.EventId = (parentEvent.Events.Enc_EventId == "0") ? 0 : Convert.ToInt64(EncryptDecrypt.Decrypt64(parentEvent.Events.Enc_EventId));
                parentEvent.HouseholdList.ForEach(x => { x.IsAttended = false; x.Signature = string.Empty; });

                if (mode == 2)
                {
                    parentEvent.HouseholdList = parentEvent.HouseholdList.Where(x => x.IsRegistered == true).ToList();
                    parentEvent.HouseholdList.ForEach(x => { x.IsRegistered = false; });
                }
                else
                {
                    parentEvent.HouseholdList.ForEach(x => { x.IsRegistered = true; });
                }
                eventDt = GetDataTableFromList(parentEvent);

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", parentEvent.Events.AgencyId));
                command.Parameters.Add(new SqlParameter("@EventId", parentEvent.Events.EventId));
                command.Parameters.Add(new SqlParameter("@ClientId", parentEvent.Events.ClientId));
                command.Parameters.Add(new SqlParameter("@tblEventRsvp", eventDt));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@EmailId", parentEvent.Events.EmailId));
                command.CommandText = "USP_InsertParentRegistration";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            regFeedBack = new RegisteredFeeback
                            {
                                AvailableSlots = Convert.ToInt64(dr["AvailSlots"]),
                                MaxAttendance = Convert.ToInt64(dr["MaxAttendance"]),
                                NewHousholds = Convert.ToInt64(dr["NewlyAdded"]),
                                TotalRegisterd = Convert.ToInt64(dr["NewlyAdded"]),
                                RegStatus = Convert.ToBoolean(dr["Status"])
                            };
                        }

                    }
                }

                return regFeedBack;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return regFeedBack;
            }

        }

        public void GetEventDetails(ref DataTable dtEventDetails, string Agencyid, string workshopid, string centerid)
        {
            dtEventDetails = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEventForHousehold";
                command.Parameters.AddWithValue("@AgencyId", Agencyid);
                command.Parameters.AddWithValue("@Workshopid", workshopid);
                command.Parameters.AddWithValue("@CenterId", centerid);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtEventDetails);
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

     
        public ParentRegisterEvent GetEventsForNewRegisterData(EventsList events, string emailId)
        {

            ParentRegisterEvent parentRegister = new ParentRegisterEvent();
            ParentHousehold household = new ParentHousehold();
            List<ParentHousehold> householdList = new List<ParentHousehold>();

            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", events.UserId));
                command.Parameters.Add(new SqlParameter("@Center_id", events.CenterId));
                command.Parameters.Add(new SqlParameter("@Workshopid", events.WokShopId));
                command.Parameters.Add(new SqlParameter("@EmailId", emailId));
                command.Parameters.Add(new SqlParameter("@ClientId", events.ClientId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetEventsForNewRegistraion";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            events = new EventsList
                            {
                                EventId = Convert.ToInt64(dr["EventId"]),
                                Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString()),
                                EventDate = Convert.ToDateTime(dr["EventDate"]).ToString("MM/dd/yyyy"),
                                EventTime = GetFormattedTime(dr["EventTime"].ToString()),
                                SpeakerName = dr["Speaker"].ToString(),
                                OpenToPublicDate = dr["OpenToPublic"].ToString(),
                                WorkShopName = dr["WorkshopName"].ToString(),
                                WokShopId = Convert.ToInt64(dr["WorkShopId"].ToString()),
                                EventDescription = dr["Description"].ToString(),
                                ProgramYear = dr["ProgramYear"].ToString(),
                                CenterId = Convert.ToInt64(dr["CenterId"]),
                                TotalDuration = dr["Duration"].ToString(),
                                RSVPCutOffDate = (dr["RSVPCutOffDate"].ToString() == "") ? "" : Convert.ToDateTime(dr["RSVPCutOffDate"]).ToString("MM/dd/yyyy"),
                                CenterAddress = dr["CenterAddress"].ToString(),
                                ImagePath = dr["ImagePath"].ToString(),
                                MaxAttend = dr["MaxAttendance"].ToString(),
                                TotalRegistered = Convert.ToInt64(dr["TotalRegistered"]),
                                AvailableSlots = Convert.ToInt64(dr["AvailableSeats"]),
                                CenterName = dr["CenterName"].ToString()
                            };
                        }
                    }
                }

                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {


                        foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                        {
                            household = new ParentHousehold
                            {
                                FullName = dr1["FullName"].ToString(),
                                ClientId = Convert.ToInt64(dr1["ClientId"]),
                                Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString()),
                                Gender = dr1["GenderType"].ToString(),
                                EmailId = dr1["EmailId"].ToString(),
                                IsChild = dr1["IsChild"].ToString(),
                                IsFamily = dr1["IsFamily"].ToString(),
                                IsSelected = (dr1["EmailId"].ToString() == emailId),
                                Enc_RSVPId = "0"
                            };


                            if (Convert.ToBoolean(dr1["ParentId"]) == false)
                            {
                                events.Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString());
                                events.ClientId = Convert.ToInt64(dr1["ClientId"].ToString());
                            }

                            householdList.Add(household);
                        }
                    }
                }

                if (_dataset.Tables[2] != null)
                {

                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        events.IsUpdate = true;

                        foreach (DataRow dr2 in _dataset.Tables[2].Rows)
                        {

                            long clientid = Convert.ToInt64(dr2["ClientId"].ToString());
                            if (clientid != 0)
                            {
                                householdList.Where(w => w.ClientId == clientid).ToList().ForEach(w =>
                                {
                                    w.IsRegistered = true;
                                    w.EventRSVPId = Convert.ToInt64(dr2["EventRSVPId"]);
                                    w.Enc_RSVPId = EncryptDecrypt.Encrypt64(dr2["EventRSVPId"].ToString());
                                    w.ProfilePic = (string.IsNullOrEmpty(dr2["ProfilePic"].ToString())) ? "" : Convert.ToBase64String((byte[])dr2["ProfilePic"]);
                                    w.IsOther = Convert.ToBoolean(dr2["IsOther"]);
                                    w.Signature = dr2["Signature"].ToString();
                                });

                            }
                            else
                            {
                                household = new ParentHousehold
                                {
                                    FullName = dr2["FullName"].ToString(),
                                    ClientId = Convert.ToInt64(dr2["ClientId"]),
                                    Enc_ClientId = EncryptDecrypt.Encrypt64(dr2["ClientId"].ToString()),
                                    Gender = dr2["GenderType"].ToString(),
                                    IsRegistered = true,
                                    IsOther = Convert.ToBoolean(dr2["IsOther"]),
                                    EventRSVPId = Convert.ToInt64(dr2["EventRSVPId"]),
                                    Enc_RSVPId = EncryptDecrypt.Encrypt64(dr2["EventRSVPId"].ToString()),
                                    Signature = dr2["Signature"].ToString(),
                                    ProfilePic = (string.IsNullOrEmpty(dr2["ProfilePic"].ToString())) ? "" : Convert.ToBase64String((byte[])dr2["ProfilePic"])

                                };
                                householdList.Add(household);
                            }

                            events.Signature = dr2["Signature"].ToString();
                            events.IsAttended = Convert.ToBoolean(dr2["Attended"]);
                            events.MilesDriven = Convert.ToDouble(dr2["MilesDriven"]);
                            events.TimeTaken = dr2["TimeTaken"].ToString();
                        }
                    }
                }

                parentRegister = new ParentRegisterEvent
                {
                    Events = events,
                    HouseholdList = householdList
                };

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
            return parentRegister;

        }

        public List<NewRegistration> GetClientForNewRegistration(ref List<EventsCenter> centerList, EventsList events, string searchName = "", long centerId2 = 0, int mode = 0)
        {

            List<NewRegistration> list = new List<NewRegistration>();
            List<NewRegistration> newList2 = new List<NewRegistration>();
            EventsCenter center = new EventsCenter();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", events.UserId));
                command.Parameters.Add(new SqlParameter("@WorkshopId", events.WokShopId));
                command.Parameters.Add(new SqlParameter("@centerId", events.CenterId));
                command.Parameters.Add(new SqlParameter("@SearchText", searchName));
                command.Parameters.Add(new SqlParameter("@CenterId2", centerId2));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetClientForNewRegistration";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);

                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in _dataset.Tables[0].Rows)
                        {
                            center = new EventsCenter
                            {
                                CenterId = Convert.ToInt64(dr1["CenterId"]),
                                Enc_CenterId = EncryptDecrypt.Encrypt64(dr1["CenterId"].ToString()),
                                CenterName = dr1["CenterName"].ToString()
                            };
                            centerList.Add(center);
                        }
                    }
                }


                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            NewRegistration register = new NewRegistration
                            {
                                CenterId = EncryptDecrypt.Encrypt64(dr["CenterId"].ToString()),
                                ParentId = dr["ParentId"].ToString(),
                                ParentName = dr["ParentName"].ToString(),
                                ClientId = EncryptDecrypt.Encrypt64(dr["ClientID"].ToString()),
                                ChildName = dr["ChildrenName"].ToString(),
                                HouseholdId = Convert.ToInt64(dr["HouseholdId"])
                            };
                            list.Add(register);
                        }

                        var list2 = list.Select(x => x.HouseholdId).Distinct().ToList();

                        if (list2.Count() > 0)
                        {


                            foreach (var item in list2)
                            {
                                var child = string.Join(",", list.Where(p => p.HouseholdId == item)
                                 .Select(p => p.ChildName).Distinct());
                                var parent = string.Join(",", list.Where(p => p.HouseholdId == item)
                                .Select(p => p.ParentName).Distinct());

                                NewRegistration register = new NewRegistration
                                {
                                    CenterId = list.Where(x => x.HouseholdId == item).Select(x => x.CenterId).FirstOrDefault(),
                                    ParentId = list.Where(x => x.HouseholdId == item).Select(x => x.ParentId).FirstOrDefault(),
                                    ParentName = parent,
                                    ClientId = list.Where(x => x.HouseholdId == item).Select(x => x.ClientId).FirstOrDefault(),
                                    ChildName = child,
                                    HouseholdId = item

                                };
                                newList2.Add(register);
                            }

                        }

                    }
                }

            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return newList2;
        }
        public ParentRegisterEvent GetRegisteredParentsForEvents(EventsList events, int mode, string searchName = "")
        {
            ParentRegisterEvent parentRegister = new ParentRegisterEvent();
            ParentHousehold household = new ParentHousehold();
            List<ParentHousehold> householdList = new List<ParentHousehold>();
            List<EventsCenter> centerList = new List<EventsCenter>();
            EventsCenter center = new EventsCenter();

            try
            {

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", events.UserId));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@WorkShopId", events.EventId));
                command.Parameters.Add(new SqlParameter("@CenterId", events.CenterId));
                command.Parameters.Add(new SqlParameter("@SearchText", searchName));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetRegisteredParents";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);

                if (mode == 1)
                {
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                center = new EventsCenter
                                {
                                    WorkShopName = dr["WorkShopName"].ToString(),
                                    WorkShopId = Convert.ToInt64(dr["WorkShopId"]),
                                    Enc_WorkShopId = EncryptDecrypt.Encrypt64(dr["WorkShopId"].ToString())
                                };
                                centerList.Add(center);
                            }
                        }
                    }
                }

                else if (mode == 2)
                {
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _dataset.Tables[0].Rows)
                            {
                                center = new EventsCenter
                                {
                                    CenterId = Convert.ToInt64(dr["Center"].ToString()),
                                    Enc_CenterId = EncryptDecrypt.Encrypt64(dr["Center"].ToString()),
                                    CenterName = dr["CenterName"].ToString(),
                                    WorkShopName = dr["WorkShopName"].ToString(),
                                    WorkShopId = Convert.ToInt64(dr["WorkShopId"]),
                                    Enc_WorkShopId = EncryptDecrypt.Encrypt64(dr["WorkShopId"].ToString())
                                };
                                centerList.Add(center);
                            }
                        }
                    }
                }

                else if (mode == 3)
                {

                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr1 in _dataset.Tables[0].Rows)
                            {
                                household = new ParentHousehold
                                {
                                    ClientId = Convert.ToInt64(dr1["ClientId"].ToString()),
                                    Enc_ClientId = EncryptDecrypt.Encrypt64(dr1["ClientId"].ToString()),
                                    FullName = dr1["FullName"].ToString(),
                                    EventDate = Convert.ToDateTime(dr1["EventDate"]).ToString("MM/dd/yyyy"),
                                    EventTime = GetFormattedTime(dr1["EventTime"].ToString()),
                                    IsAttended = Convert.ToBoolean(dr1["Attended"].ToString()),
                                    WorkShopName = dr1["WorkShopName"].ToString(),
                                    Enc_EventId = EncryptDecrypt.Encrypt64(dr1["EventId"].ToString())
                                };
                                householdList.Add(household);
                            }
                        }
                    }
                    bool IsFull = false;
                    if (_dataset.Tables[1] != null)
                    {
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr2 in _dataset.Tables[1].Rows)
                            {
                                IsFull = Convert.ToBoolean(dr2["IsFull"]);
                            }
                        }
                        parentRegister.IsFull = IsFull;
                    }
                }
                parentRegister.HouseholdList = householdList;
                parentRegister.AllCenters = centerList;
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
            return parentRegister;
        }

        public bool CancelEventRegistrationData(EventsList events)
        {
            bool isRowAffected = false;
            DataTable dt = new DataTable();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@EventId", events.EventId));
                command.Parameters.Add(new SqlParameter("@ClientId", events.ClientId));
                command.Parameters.Add(new SqlParameter("@mode", 2));
                command.Parameters.Add(new SqlParameter("@EmailId", events.EmailId));
                command.Parameters.Add(new SqlParameter("@UserId", events.UserId));
                command.Parameters.Add(new SqlParameter("@tblEventRsvp", dt));
                command.CommandText = "USP_InsertParentRegistration";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isRowAffected;
        }

        public bool UpdateEventsImage(EventsList events)
        {
            bool isInserted = false;
            try
            {

                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_ChangeEventsImage";
                command.Parameters.AddWithValue("@AgencyId", events.AgencyId);
                command.Parameters.AddWithValue("@EventId", events.EventId);
                command.Parameters.AddWithValue("@UserId", events.UserId);
                command.Parameters.AddWithValue("@ImagePath", events.ImagePath);
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
            }
            return isInserted;
        }
        public string GetFormattedTime(string _time)
        {
            var str = _time.Split(':')[0] + ":" + _time.Split(':')[1];
            var timeFromInput = DateTime.ParseExact(str, "H:m", null, DateTimeStyles.None);

            string timeIn12HourFormatForDisplay = timeFromInput.ToString(
                "hh:mm tt",
                CultureInfo.InvariantCulture);
            return timeIn12HourFormatForDisplay;
        }

        public void GetWorkshop(ref DataTable dtWorkShop)
        {
            dtWorkShop = new DataTable();
            try
            {
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetWorkshop";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtWorkShop);
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

        public void WorkshopReport(ref DataTable dtWorkshop, string Workshopid, string CenterId)
        {
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetWorkshopReport";
                command.Parameters.AddWithValue("@Workshopid", Workshopid);
                command.Parameters.AddWithValue("@CenterId", CenterId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtWorkshop);
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
        public void GetCancelledWorkshop(ref DataTable dtWorkshop, string CenterId)
        {
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetCancelledWorkshop";
                command.Parameters.AddWithValue("@CenterId", CenterId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtWorkshop);
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

        public long GetRemainingSeats(EventsList events)
        {
            long AvailSeats = 0;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", events.AgencyId));
                command.Parameters.Add(new SqlParameter("@EventId", events.EventId));
                command.Parameters.Add(new SqlParameter("@EmailId", events.EmailId));
                command.CommandText = "USP_GetRemainingSlotsForEvent";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            AvailSeats = Convert.ToInt64(string.Format("0{0:00}", (int)dr["AvailableSeats"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataAdapter.Dispose();
                command.Dispose();
                clsError.WriteException(ex);
            }
            return AvailSeats;
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

        public EventsList CheckDate(EventsList events)
        {
            EventsList list = new EventsList();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyId", events.AgencyId);
                command.Parameters.AddWithValue("@EventId", events.EventId);
                command.CommandText = "USP_GetEventsDate";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            events = new EventsList
                            {
                                TotalCount = Convert.ToInt64(dr["ResultCount"]),

                            };
                        }
                    }
                }

                list = events;


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
            return list;
        }
        public List<SelectListItem> LoadReasons(Events events)
        {
            SelectListItem objReasons = new SelectListItem();
            List<SelectListItem> lst = new List<SelectListItem>();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Event_RegisterReasons";
                command.Parameters.AddWithValue("@AgencyId", events.AgencyId);
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            objReasons = new SelectListItem();
                            objReasons.Text = dr["ReasonDescription"].ToString();
                            objReasons.Value = dr["EventReasonId"].ToString();
                            lst.Add(objReasons);
                        }
                        lst.Add(new SelectListItem
                        {
                            Text = "Others",
                            Value = "Others"

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
            return lst;
        }

        public bool GetUpdatedReasons(EventsList events, int mode)
        {
            bool isResult = false;

            try
            {



                command.Parameters.Clear();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyId", events.AgencyId);
                command.Parameters.AddWithValue("@UserId", events.UserId);
                command.Parameters.AddWithValue("@EmailId", events.EmailId);
                command.Parameters.AddWithValue("@mode", mode);
                command.CommandText = "USP_GetStatusChangedWorshop";
                command.CommandType = CommandType.StoredProcedure;
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        isResult = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["Result"]);

                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return isResult;
        }


        public List<EventsList> GetChangedEventsForParent(EventsList events, int mode)
        {

            List<EventsList> eventsList = new List<EventsList>();
            try
            {

                command.Parameters.Clear();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyId", events.AgencyId);
                command.Parameters.AddWithValue("@UserId", events.UserId);
                command.Parameters.AddWithValue("@EmailId", events.EmailId);
                command.Parameters.AddWithValue("@mode", mode);
                command.CommandText = "USP_GetStatusChangedWorshop";
                command.CommandType = CommandType.StoredProcedure;
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            events = new EventsList
                            {
                                EventColor = dr["Color"].ToString(),
                                WorkShopName = dr["WorkShopName"].ToString(),
                                EventDate = Convert.ToDateTime(dr["EventDate"]).ToString("MM/dd/yyyy"),
                                EventStatus = dr["EventStatus"].ToString(),
                                EventTime = GetFormattedTime(dr["EventTime"].ToString()),
                                Enc_EventId = EncryptDecrypt.Encrypt64(dr["EventId"].ToString())
                            };
                            eventsList.Add(events);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return eventsList;
        }

        public bool InsertResponseEventChangeData(EventsList events, bool isAttend)
        {
            bool isRowAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@EventId", events.EventId));
                command.Parameters.Add(new SqlParameter("@EmailId", events.EmailId));
                command.Parameters.Add(new SqlParameter("@IsAttend", isAttend));
                command.CommandText = "USP_EventChangeParentResponse";

                command.CommandType = CommandType.StoredProcedure;
                DataAdapter = new SqlDataAdapter(command);
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isRowAffected;
        }


     


    }
}
