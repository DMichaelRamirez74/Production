using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{
     public class Center
    {
         public int CenterId { get; set; }
         public string AgencyId { get; set; }
         public string Enc_CenterId { get; set; }
         //Changes
         public string AgencyName { get; set; }
         public string SelectedAgencyId { get; set;}
         public string CenterName { get; set; }
         public string StepUpToQualityStars { get; set; }
         public string NAEYCCertified { get; set; }
         public string Address { get; set; }
         public string City { get; set; }
         public string State { get; set; }
         public string Zip { get; set; }
         public int mode { get; set; }
         public string AdminSite { get; set; }
         public string TimeZoneID { get; set; }
         public int TimeZoneMinuteDiff { get; set; }
         public string CreatedBy { get; set; }
         public string DateEntered { get; set; }
         public string status { get; set; }
         public bool HomeBased { get; set; }
         // public string RoomName { get; set; }
         //public bool DoubleSession { get; set; }
         //public string ClassSession { get; set; }
         //public string StartTime { get; set; }
         //public string StopTime { get; set; }
         //public bool Dinner { get; set; }
         //public bool Lunch { get; set; }
         //public bool Breakfast { get; set; }
         //public bool Monday { get; set; }
         //public bool Tuesday { get; set; }
         //public bool Wednesday { get; set; }
         //public bool Thursday { get; set; }
         //public bool Friday { get; set; }
         //public bool Saturday { get; set; }
         //public bool Sunday { get; set; }
         //public bool Snack { get; set; }
         public ClassRoom ClassRoomDetails { get; set; }

         public List<ClassRoom> Classroom = new List<ClassRoom>();
         public List<TimeZoneinfo> TimeZonelist = new List<TimeZoneinfo>();
         public class ClassRoom
         {
             public string RoomName { get; set; }
             public string MaxCapacity { get; set;}
             public bool DoubleSession { get; set; }
             public string ClassSession { get; set; }
             public string StartTime { get; set; }
             public string StopTime { get; set; }
             public bool Dinner { get; set; }
             public bool Lunch { get; set; }
             public bool Breakfast { get; set; }
             public bool Monday { get; set; }
             public bool Tuesday { get; set; }
             public bool Wednesday { get; set; }
             public bool Thursday { get; set; }
             public bool Friday { get; set; }
             public bool Saturday { get; set; }
             public bool Sunday { get; set; }
             public bool Snack { get; set; }
             public bool Snack2 { get; set; }
             public string ClassName { get; set; }
             public int Capacity { get; set; }
             public int ClassroomID { get; set; }
             public string Enc_ClassRoomId { get; set; }
             public string Noofseats { get; set; }
             public string ActualSeats { get; set; }
             public int TimeBetweenMeals { get; set; }//1=10min;2=15min;3=20min;4=25min;5=30 min
             public string BreakfastFromTime {get;set;}
			 
			 public string	LunchFromTime {get;set;}
			 
			 public string SnackFromTime {get;set;}
             public string Snack2FromTime { get; set; }
             public string DinnerFromTime { get; set; }
             public string BreakfastToTime { get; set; }
             public string LunchToTime { get; set; }
             public string SnackToTime { get; set; }
             public string DinnerToTime { get; set; }
             public string Snack2ToTime { get; set; }

            public int ClosedToday { get; set; }

            public List<SelectListItem> TimeBetweenMealsList { get; set; }

            public List<SelectListItem> ClassSessionList { get; set; }

            public ProgramType ProgramDetails { get; set; }

        }
    }
}
