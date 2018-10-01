using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class EducationManager
    {

        public int EduLevel0 { get; set; }
        public int EduLevel2 { get; set; }
        public int EduLevel3 { get; set; }
        public int EduLevel4 { get; set; }
        public int GettingDegree { get; set; }
        public int NotGettingDegree { get; set; }
    }



    public class StaffEventCreation
    {
        public int EventType { get; set; }
        public bool IsEventCancelled { get; set; }
        public int EventChangesOn { get; set; }

        public string InitialEventDate { get; set; }
        public string InitialEventTime { get; set; }
        public int RSVPStatus { get; set; }
        public bool IsEditMode { get; set; }
        public string Heading { get; set; }
        public int Eventid { get; set; }
        public string EventName { get; set; }
        public string EventDate { get; set; }
        public string Trainer { get; set; }
        public string StartTime { get; set; }
        public string TotalHours { get; set; }
        public string ContinuingEdu  { get; set; }
        public  int CenterId { get; set; }
        public string EventDescription { get; set; }
        public string CreatedBy { get; set; }
        //  public List<SelectListItem> CenterList { get; set; }
        public List<CenterListItem> CenterList { get; set; }

        public class CenterListItem : SelectListItem
        {
        public bool? HomeBased { get; set; }
        }
       
        public List<string> CenterIds { get; set; }
        public List<SelectListItem> ActiveRoles { get; set; }
        public List<StaffEventCreation> events { get; set; }
        public List<string> RolesList { get; set; }
        public List<string> UsersList { get; set; }
        public string ModifiedDate { get; set; }
        public string EvenitAddress { get; set; }
        public string CancelReason { get; set; }
        public int IsTodayEvent { get; set; }

    }



    public class EventReportDetails
    {
        public int Eventid { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventDate { get; set; }
        public string Trainer { get; set; }
        public string EventAddress { get; set; }

        public int ParticipantCount { get; set; }
        public List<ParticipantDetail> ParticipantDetails { get; set; }

    }

  public class ParticipantDetail {
          public string ParticipantName { get; set; }
          public string ParticipantId { get; set; }
          public string ParticipantRoleName { get; set; }
          public string RSVPStatusModifiedDate { get; set; }
          public int? RSVPStatusId { get; set; }
          public string RSVPStatusName { get; set; }
            public string Avatar { get; set; }
        public int Gender { get; set; }
        public string Signature { get; set; }
        public int? IsAttended { get; set; } 
        
    }

    public class UserBasedEventReport {

        public string UserName { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<EventDetail> EventDetails { get; set; }
        public int SumOfEventsHourPerUser { get; set; }
        public int SumOfEventsCEHourPerUser { get; set; }
        public int TotalEvent { get; set; }
    }

    public class EventDetail {
        public int Eventid { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventDate { get; set; }
        public string Trainer { get; set; }
        public string EventAddress { get; set; }
        public int TotalHours { get; set; }
        public int ContinuingEducation { get; set; }
       

    }
   
    public enum StaffEventListType
    {

        Initial=1,
        ByEventId=2,
        UpcomingEvents=3,
        CancelledEvents=4,
        CompletedEvents=5,
        OpenEvents = 6 //today event not inculded

    }
}
