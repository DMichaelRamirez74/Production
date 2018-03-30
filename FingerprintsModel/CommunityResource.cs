using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class CommunityResource
    {
       public Int32 CommunityID { get; set; }
       public string Title { get; set; }
       public Guid AgencyId { get; set; }
       public string SelectedRoleId { get; set; }
       public string Centers { get; set; }
       public string Fname { get; set; }
       public string Lname { get; set; }
       public string Community { get; set; }
       public string Notes { get; set; }
       public string Username { get; set;}
       public string UsernameDentist { get; set; }
       public string CreatedBy { get; set;}
       public string CreatedDate { get; set;}
       public int mode { get; set; }
       public string Companyname { get; set; }
       public string Address { get; set; }
       public string Phoneno { get; set; }
       public string OperationHours { get; set; }
       public string StartTime { get; set; }
       public string StopTime { get; set; }
       public int ZipCode { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string County { get; set; }
       public bool OpenonSat { get; set; }
       public string Region { get; set; }
       public string Email { get; set; }
       public string Comments { get; set; }
       //Added on 31Jan2017
       public bool DocCheck { get; set; }
       public bool DenCheck { get; set; }
       //End
       public bool AssistanceToFamilies { get; set; }
       public bool ChildAbuse { get; set; }
       public bool CommunityInvolvement { get; set; }
       public bool CrisisIntervention { get; set; }
       public bool DomesticViolence { get; set; }
       public bool FinancialAssistance { get; set; }
       public bool FoodPantry { get; set; }
       public bool Furniture { get; set; }
       public bool ChildHealth { get; set; }
       public bool FamilyHealth { get; set; }
       public bool ReproductiveHealth { get; set; }
       public bool ChildCare { get; set; }
       public bool ChildSupport { get; set; }
       public bool Clothing { get; set; }
       public bool Housing { get; set; }
       public bool LifeSkill { get; set; }
       public bool Nutritions { get; set; }
       public bool Transition { get; set; }
       public bool Transportation { get; set; }

       //Added by Akansha on 14Dec2016
       public string MedicalCenter { get; set; }
       public string MedicalNotes { get; set; }
       public string DentalCenter { get; set; }
       public string DentalNotes { get; set; }
        public string OtherCenter { get; set; }
        public string OtherNotes { get; set; }

       //Added by akansha on 16Dec2016
       public string CompanynameD { get; set; }
        public string CompanyNameOt { get; set; }
       //End


       //changes
       public string MonFrom { get; set; }
       public string MonTo { get; set; }
       public string TueFrom { get; set; }
       public string TueTo { get; set; }
       public string WedFrom { get; set; }
       public string WedTo { get; set; }
       public string ThursFrom { get; set; }
       public string ThursTo { get; set; }
       public string FriFrom { get; set; }
       public string FriTo { get; set; }
       public string SatFrom { get; set; }
       public string SatTo { get; set; }
       public string SunFrom { get; set; }
       public string SunTo { get; set; }

       public string Services { get; set; }
       public int gender { get; set; }
       public bool office24hours { get; set; }
       public bool Mon { get; set; }
       public bool Tue { get; set; }
       public bool Wed { get; set; }
       public bool Thurs { get; set; }
       public bool Fri { get; set; }
       public bool Sat { get; set; }
       public bool Sun { get; set; }
       public bool CheckedAll { get; set; }
       public bool UncheckedAll { get; set; }
      // public bool Furniture { get; set; }
       public string ServiceType { get; set; }
       public List<CategoryServiceInfo> AvailableService { get; set; }
       public List<CategoryServiceInfo> SelectedService { get; set; }
       public PostedService PostedPostedService { get; set; }
       public List<HrCenterInfo> HrcenterList { get; set; }
       public List<DoctorInfo> doctorList = new List<DoctorInfo>();
       public List<DentistInfo> dentistList = new List<DentistInfo>();
       public class DoctorInfo
       {
           public string Id { get; set; }
           public string Name { get; set; }
           public string Community { get; set; }
       }
       public class DentistInfo
       {
           public string Id { get; set; }
           public string Name { get; set; }
       }

       public class CategoryServiceInfo
       {
           //public int Id { get; set; }
           //public string Name { get; set; }
           public string CategoryId { get; set; }
           public string CategoryName { get; set; }
           public bool IsSelected { get; set; }
          
           public List<ServiceInfo> ServiceInfoDetail { get; set; }
        
          // public List<>
       }
       public class ServiceInfo
       {
           public int Id { get; set; }
           public string Name { get; set; }
           public bool IsSelected { get; set; }
       }
       public class PostedService
       {
           public string[] ServiceID { get; set; }
       }
    }
}
