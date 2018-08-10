using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FingerprintsModel
{
    /// <summary>
    /// Represents a class that is used to information about the Staffs.
    /// </summary>
    public class StaffDetails
    {
        /// <summary>
        /// Static method to create an instance of the StaffDetails class.
        /// </summary>
        /// <returns>new StaffDetails()</returns>
        public static StaffDetails GetInstance()
        {
            return new StaffDetails();
        }

        /// <summary>
        /// Default Constructor initializes and assigns Session Values to its data members.
        /// </summary>
        public StaffDetails()
        {
            this.AgencyId = ((HttpContext.Current.Session["AgencyID"]==null) ? (Guid?)null : new Guid(HttpContext.Current.Session["AgencyID"].ToString()));
            this.UserId = ((HttpContext.Current.Session["UserID"]==null)? (Guid?)null :   new Guid(HttpContext.Current.Session["UserID"].ToString()));
            this.RoleId = ((HttpContext.Current.Session["RoleID"]==null)? (Guid?)null : new Guid(HttpContext.Current.Session["RoleID"].ToString()));
            this.FullName =((HttpContext.Current.Session["FullName"]==null)?string.Empty: HttpContext.Current.Session["FullName"].ToString());
            this.EmailID = ((HttpContext.Current.Session["EmailID"]==null)?string.Empty:HttpContext.Current.Session["EmailID"].ToString());
                

        }


        //public StaffDetails(Guid userId, Guid roleId, Guid? agencyId, string name = "")
        //{
        //    this.FullName = name;
        //    this.UserId = userId;
        //    this.RoleId = roleId;
        //    this.AgencyId = agencyId;
        //}
        public string FullName { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
        public Guid? AgencyId { get; set; }

        public string EmailID { get; set; }

    }


}
