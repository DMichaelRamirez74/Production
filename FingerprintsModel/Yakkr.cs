﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class Yakkr
    {
      public Int32  YakkrRoleID {get;set;}
      public String AgencyID {get;set;}
      public String YakkrID {get;set;}
      public String StaffRoleID {get;set;}
      public String SecondaryRoleID { get; set; }
      public String StaffRoleName { get; set; }
      public String OptionalRoleName { get; set; }
      public String Status { get; set; }
      public string DateEntered {get;set;}
      public String Value {get;set;}
        public bool IsEnableEmail { get; set; }
      public String Description { get; set; }

      public List<YakkrCode> YakkrList = new List<YakkrCode>();
      public class YakkrCode
      {
            public string YakkrDescription { get; set; }
          public string _YakkrID { get; set; }
          public string _YakkrCode { get; set; }
      }

      public List<YakkrRoles> _YakkrRolesList = new List<YakkrRoles>();
      public class YakkrRoles
      {
          public string _RoleID { get; set; }
          public string _RoleName { get; set; }
      }

      public List<YakkrAgencyCodes> _YakkrAgencyCodes = new List<YakkrAgencyCodes>();
      public class YakkrAgencyCodes
      {
          public Int32 YakkrRoleID { get; set; }
          public String AgencyID { get; set; }
          public Int32 YakkrID { get; set; }
          public String YakkrCode { get; set; }
          public String StaffRoleID { get; set; }
          public String StaffRoleName { get; set; }
          public String Status { get; set; }
          public DateTime DateEntered { get; set; }
          public String Value { get; set; }
          public String Description { get; set; }
      }
     

    }

    public class YakkrEmail
    {
        public List<YakkrEmail> EmailList = new List<YakkrEmail>();
        public int YakkrId { get; set; }

        public int OtherID { get; set; }
        public int EmailId { get; set; }
        public string Result { get; set; }
        public bool EmailType { get; set; }
        public bool IsOther { get; set; }
        public bool IsHaveService { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string RawBody { get; set; }
        public string StringEmailBody { get; set; }
       public List<SelectListItem> YakkrList = new List<SelectListItem>();
        public List<SelectListItem> OtherList = new List<SelectListItem>();

        public string YakkrDescription { get; set; }
        public string ListMergeFields { get; set; }
    }
}
