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


        private static readonly object padlock = new object();

        private static StaffDetails instance = null;



        private static StaffDetails Instance
        {

            get
            {
                if(instance==null)
                {
                    lock(padlock)
                    {
                        if(instance==null)
                        {
                            instance = new StaffDetails();
                        }

                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// Static method to create an instance of the StaffDetails class.
        /// </summary>
        /// <returns>new StaffDetails()</returns>
        public static StaffDetails GetInstance()
        {
            return new StaffDetails();
        }

        public  static StaffDetails GetThreadedInstance(HttpContext currentContext)
        {
            return new StaffDetails(currentContext);
           
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

        public StaffDetails(HttpContext _currentContext)
        {
            this.AgencyId = ((_currentContext.Session["AgencyID"] == null) ? (Guid?)null : new Guid(_currentContext.Session["AgencyID"].ToString()));
            this.UserId = ((_currentContext.Session["UserID"] == null) ? (Guid?)null : new Guid(_currentContext.Session["UserID"].ToString()));
            this.RoleId = ((_currentContext.Session["RoleID"] == null) ? (Guid?)null : new Guid(_currentContext.Session["RoleID"].ToString()));
            this.FullName = ((_currentContext.Session["FullName"] == null) ? string.Empty : _currentContext.Session["FullName"].ToString());
            this.EmailID = ((_currentContext.Session["EmailID"] == null) ? string.Empty : _currentContext.Session["EmailID"].ToString());


        }

        public StaffDetails(bool createInstance)
        {
            if(createInstance)
             Fingerprints.Common.FactoryInstance.Instance.CreateInstance<StaffDetails>();
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
