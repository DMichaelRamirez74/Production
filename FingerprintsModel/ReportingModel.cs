﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FingerprintsModel
{
    public class ReportingModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DOB { get; set; }
        public string Status { get; set; }
        public string Insurance { get; set; }
        public string ColumnName { get; set; }
        public string CenterName { get; set; }
        public string ClassroomName { get; set; }
        public int reporttype { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Guardian { get; set; }
        public string ProgramType { get; set; }
        public string DaysEnrolled { get; set; }
        public string ReasonForAcceptance { get; set; }
        public List<ReportingModel> Reportlst { get; set; }
        public List<ReportingModel> Meallst { get; set; }
        public List<ReportingModel> Monthlst { get; set; }
      //  public string CenterName { get; set; }
        public string MonthName { get; set; }
        public string MealCount { get; set; }
        public string AmealCount { get; set; }
        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Snack { get; set; }
        public string BreakfastTotal { get; set; }
        public string LunchTotal { get; set; }
        public string SnackTotal { get; set; }
        public string BreakfastTotalMonth { get; set; }
        public string LunchTotalMonth { get; set; }
        public string SnackTotalMonth { get; set; }
        public string ABreakfast { get; set; }
        public string ALunch { get; set; }
        public string ASnack { get; set; }
        public string MealType { get; set; }
        public string CenterID { get; set; }
        public string AttendanceMonth { get; set; }
        public string AttendanceDate { get; set; }

         public string AttendanceDateMonth { get; set; }
         public string CenterIDCenter { get; set; }
         public string AttendanceMonthCenter { get; set; }
         public string MonthNameCenter { get; set; }
         public string CenterNameCenter { get; set; }
        
            
    }
}