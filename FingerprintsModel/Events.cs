using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{

    public class Events
    {
        public int CheckMode { get; set; }
        public bool IsUpdate { get; set; }
        public int Workshopid { get; set; }
        public string Workshopname { get; set; }
        public string Workshopdescription { get; set; }
        public int Eventid { get; set; }
        public string Speaker { get; set; }
        public string Comments { get; set; }
        public string CenterDate { get; set; }
        public string EventDate { get; set; }
        public string DefaultDate { get; set; }
        public string ChangeDate { get; set; }
        public string OtherCenterDate { get; set; }
        public string EventTime { get; set; }
        public string DefaultTime { get; set; }
        public string ChangeTime { get; set; }
        public string NoOfSeats { get; set; }
        public string Duration { get; set; }
        public string EventStatus { get; set; }
        public string DefaultStatus { get; set; }
        public string ChangeStatus { get; set; }
        public string CenterId { get; set; }
        public string CenterName { get; set; }
        public string ImagePath { get; set; }
        public string RSVPDate { get; set; }
        public string RSVPPoints { get; set; }
        public string AttendancePoints { get; set; }
        public string Budget { get; set; }
        public string ActualCosts { get; set; }
        public string HourlyRate { get; set; }
        public string MileageRate { get; set; }
        public string AgencyId { get; set; }
        public string UserId { get; set; }
        public string Color { get; set; }

        public string EventDateChange { get; set; }
        public string EventTimeChange { get; set; }
        public string EventStatusChange { get; set; }
        public string EventStatusDescription { get; set; }
        public string EventDateDescription { get; set; }
        public string EventTimeDescription
        {
            get;set;
        }

        public List<SelectListItem> ListReason { get; set; }

        public List<SelectListItem> RegisteredMembers { get; set; }

        public long MinutesDiff { get; set; }


    }

    public class ReasonList
    {
        public long EventReasonId { get; set; }
        public string ReasonDescription { get; set; }
        public string ReasonType { get; set; }
        public string AgencyId { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }

    }

    public class EventsList
    {
        public long EventId { get; set; }
        public long TotalCount { get; set; }
        public bool ActiveRecord { get; set; }
        public Guid AgencyId { get; set; }
        public Guid UserId { get; set; }
        public string ProgramYear { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string EventDateFormat { get; set; }
        public string EventTime { get; set; }
        public string OpenToPublicDate { get; set; }
        public string TotalDuration { get; set; }
        public bool DurationType { get; set; }
        public string EmailId { get; set; }
        public string EventDescription { get; set; }
        public long CenterId { get; set; }
        public string CenterName { get; set; }
        public string Enc_CenterId { get; set; }
        public string SpeakerName { get; set; }
        public string Comments { get; set; }
        public string MaxAttend { get; set; }
        public string RSVPCutOffDate { get; set; }
        public long RSVPPoints { get; set; }
        public long AttendPoints { get; set; }
        public long Budget { get; set; }
        public long ActualCosts { get; set; }
        public double HourlyRate { get; set; }
        public string Signature { get; set; }
        public string EventStatus { get; set; }
        public string Enc_EventId { get; set; }
        public string WorkShopName { get; set; }
        public long WokShopId { get; set; }
        //public long AvailableSeats { get; set; }
        public string ImagePath { get; set; }
        public long TotalRegistered { get; set; }
        public long AvailableSlots { get; set; }
        public long ClientId { get; set; }
        public string CenterAddress { get; set; }
        public string Enc_ClientId { get; set; }
        public string TimeTaken { get; set; }
        public double MilesDriven { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsOther { get; set; }
        public string EventColor { get; set; }
        public bool IsUpdate { get; set; }
        public long MinDiff { get; set; }
        public string ParentEmailId { get; set; }
        public string AvailableStatus { get; set; }
        public bool IsAttended { get; set; }

        public long CurrentCount { get; set; }
        public string EventChangeReason { get; set; }
        

    }


    public class EventsCalender
    {
        public string id { get; set; }
        public string title { get; set; }
        public string Enc_EventId { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string StatusString { get; set; }
        public string StatusColor { get; set; }
        public string className { get; set; }
        public bool IsOther { get; set; }
        public bool allDay { get; set; }
        public string color { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public bool IsClick { get; set; }

    }
    public class EventsCenter
    {
        public long CenterId { get; set; }
        public string Enc_CenterId { get; set; }
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public string WorkShopName { get; set; }
        public string Enc_WorkShopId { get; set; }
        public long WorkShopId { get; set; }
        public List<EventsList> _EventsList { get; set; }
    }

    public class EventsModal
    {
        public List<EventsList> _EventsList { get; set; }
        public List<EventsCenter> _CenterList { get; set; }
    }

    public class ParentEvent
    {
        // public List<EventsList> _EvenList { get; set; }
        public EventsCenter _EventCenter { get; set; }
    }
    public class ParentSelectionEvent
    {
        public List<EventsCenter> SelfCenterList { get; set; }
        public List<EventsCenter> OtherCentersList { get; set; }
    }

    public class ParentHousehold
    {
        public string FullName { get; set; }
        public long ClientId { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public bool IsSelected { get; set; }
        public string IsFamily { get; set; }
        public string ParentRole { get; set; }
        public string IsChild { get; set; }
        public string Enc_ClientId { get; set; }
        public bool IsOther { get; set; }
        public bool IsRegistered { get; set; }
        public long EventRSVPId { get; set; }
        public string Enc_RSVPId { get; set; }
        public bool Status { get; set; }
        public bool IsAttended { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string WorkShopName { get; set; }
        public string Enc_EventId { get; set; }
        public string Signature { get; set; }
        public string ProfilePic { get; set; }

    }

    public class ParentRegisterEvent
    {
        public EventsList Events { get; set; }

        public List<ParentHousehold> HouseholdList { get; set; }
        public List<EventsCenter> AllCenters { get; set; }
        public bool IsFull { get; set; }
    }

    public class NewRegistration
    {
        public String ParentId { get; set; }
        public String ParentName { get; set; }
        public String ClientId { get; set; }
        public String ChildName { get; set; }
        public String CenterId { get; set; }
        public long HouseholdId { get; set; }

    }

    public class WalkinRegistraton
    {
        public List<NewRegistration> NewRegistrationList
        {
            get; set;
        }
        public List<EventsCenter> CenterList { get; set; }
    }

    public class ShowEventDetails
    {
        public EventsList Events { get; set; }

        public List<RegisteredMembers> RegisteredUsers { get; set; }

    }

    public class RegisteredMembers
    {
        public string FullName { get; set; }
        public long ClientId { get; set; }
        public string CenterAddress { get; set; }
        public string Category { get; set; }
        public string CenterName { get; set; }
        public bool IsOther { get; set; }
        public bool IsOtherCenter { get; set; }
        public string EmailId { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string WorkShopName { get; set; }
        
    }

    public class RegisteredFeeback
    {
        public long MaxAttendance { get; set; }
        public long AvailableSlots { get; set; }
        public long NewHousholds { get; set; }
        public long TotalRegisterd { get; set; }
        public bool RegStatus { get; set; }
    }

}
