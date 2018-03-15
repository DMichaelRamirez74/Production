using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


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
      public String Description { get; set; }

      public List<YakkrCode> YakkrList = new List<YakkrCode>();
      public class YakkrCode
      {
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
}
