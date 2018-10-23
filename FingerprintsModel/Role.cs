using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FingerprintsModel
{
    public class Role
    {

        public Role()
        {
            //this.UserList = new List<UserDetails>();
        }
        public List<Role> RoleList { get; set; }
        public List<FingerprintsModel.RosterNew.User> ClientList { get; set; }
        public string AssignedRoles { get; set; }
        public string RoleId { get; set; }
        public bool IsAllow { get; set; }
        public string RoleName { get; set; }
        public bool Defaultrole { get; set; }
        public string ColorCode { get; set; }
        public bool ToView { get; set; }
        public bool ToFollowUp { get; set; }
        public bool ToEnter { get; set; }
        public List<UserDetails> UserList { get; set; }

        public static readonly Dictionary<int, string> RolesDictionary = new Dictionary<int, string>()
        {

            { (int)RoleEnum.AgencyAdmin, agencyAdmin},
            {(int)RoleEnum.AreaManager, areaManager},
            {(int)RoleEnum.BillingManager, billingManager},
            {(int)RoleEnum.BusDriver,busDriver },
            { (int)RoleEnum.BusMonitor,busMonitor},
            { (int)RoleEnum.CenterManager,centerManager},
            { (int)RoleEnum.ChildCareProvider,childCareProvider},
            { (int)RoleEnum.DisabilitiesManager,disabilitiesManager},
            { (int)RoleEnum.DisabilityStaff,disabilityStaff},
            { (int)RoleEnum.EducationManager,educationManager},
            {(int)RoleEnum.ERSEAManager,erseaManager },
            { (int)RoleEnum.Executive,executive},
            { (int)RoleEnum.FacilitiesManager,facilitiesManager},
            {(int)RoleEnum.FacilityWorker,facilityWorker },
            { (int)RoleEnum.FamilyServiceWorker,familyServiceWorker},
            {(int)RoleEnum.GenesisEarthAdministrator,gEarthAdministrator },
            { (int)RoleEnum.HealthManager,healthManager},
            { (int)RoleEnum.HealthNurse,healthNurse},
            { (int)RoleEnum.HomeVisitor,homeVisitor},
            { (int)RoleEnum.HRManager,hrManager},
            { (int)RoleEnum.HRStaff,hrStaff},
            { (int)RoleEnum.MentalHealthSpecialist,mentalHealthSpecialist},
            { (int)RoleEnum.Nutritionist,nutritionist},
            { (int)RoleEnum.Parent,parent},
            { (int)RoleEnum.SocialServiceManager,socialServiceManager},
            { (int)RoleEnum.SuperAdmin,superAdmin},
            {(int)RoleEnum.Teacher,teacher },
            { (int)RoleEnum.TeacherAssistant,teacherAssistant},
            { (int)RoleEnum.TransportManager,transportManager}

         };

        #region  Roles  (Private fields)
        public const string agencyAdmin = "3B49B025-68EB-4059-8931-68A0577E5FA2";
        public const string areaManager = "2AF7205E-87B4-4CA7-8CA8-95827C08564C";
        public const string billingManager = "944D3851-75CC-41E9-B600-3FA904CF951F";
        public const string busDriver = "259D2818-3832-46B0-A75C-1731F9F6ABC7";
        public const string busMonitor = "AE148380-F94E-4F7A-A378-897C106F1A52";
        public const string centerManager = "B4D86D72-0B86-41B2-ADC4-5CCCE7E9775B";
        public const string childCareProvider = "DEE83529-01B9-40F6-A35B-9037A5128741";
        public const string disabilitiesManager = "047C02FE-B8F1-4A9B-B01F-539D6A238D80";
        public const string disabilityStaff = "9C34EC8E-2359-4704-BE89-D9F4B7706E82";
        public const string educationManager = "4B77AAB6-EED1-4AC3-B498-F3E80CF129C0";
        public const string erseaManager = "4B77AAB6-EED1-4AC3-B498-F3E80CF129C0";
        public const string executive = "7C2422BA-7BD4-4278-99AF-B694DCAB7367";
        public const string facilitiesManager = "825F6940-9973-42D2-B821-5B6C7C937BFE";
        public const string facilityWorker = "CB540CEA-154C-482E-82A6-C1E0A189F611";
        public const string familyServiceWorker = "CB540CEA-154C-482E-82A6-C1E0A189F611";
        public const string gEarthAdministrator = "A65BB7C2-E320-42A2-AED4-409A321C08A5";
        public const string healthManager = "9AD1750E-2522-4717-A71B-5916A38730ED";
        public const string healthNurse = "A31B1716-B042-46B7-ACC0-95794E378B26";
        public const string homeVisitor = "E4C80FC2-8B64-447A-99B4-95D1510B01E9";
        public const string hrManager = "2D9822CD-85A3-4269-9609-9AABB914D792";
        public const string hrStaff = " E8DB42F2-8D04-4398-BAB0-2996C48CE1B6";
        public const string mentalHealthSpecialist = "699168AC-AD2D-48AC-B9DE-9855D5DC9AF8";
        public const string nutritionist = "CE744500-7CA2-4122-B15F-686C44811A51";
        public const string parent = "5AC211B2-7D4A-4E54-BD61-5C39D67A1106";
        public const string socialServiceManager = "C352F959-CFD5-4902-A529-71DE1F4824CC";
        public const string superAdmin = "F87B4A71-F0A8-43C3-AEA7-267E5E37A59D";
        public const string teacher = "82B862E6-1A0F-46D2-AAD4-34F89F72369A";
        public const string teacherAssistant = "2ADFE9C6-0768-4A35-9088-E0E6EA91F709";
        public const string transportManager = "6ED25F82-57CB-4C04-AC8F-A97C44BDB5BA";
        #endregion

        //#region Roles (public fields with get properties
        //public  static string AgencyAdmin { get { return agencyAdmin; } }
        //public static string AreaMangaer { get { return areaManager; } }
        //public static string BillingManager { get { return billingManager; } }
        //public static string BusDriver { get { return busDriver; } }
        //public static string BusMonitor { get { return BusMonitor; } }
        //public static string CenterManager { get { return centerManager; } }
        //public static string ChildCareProvider { get { return childCareProvider; } }
        //public static string DisabilityManager { get { return disabilitiesManager; } }
        //public static string DisabilityStaff { get { return disabilityStaff; } }
        //public static string EducationManager { get { return educationManager; } }
        //public static string ERSEAManager { get { return erseaManager; } }
        //public static string Executive { get { return executive; } }
        //public static string FacilitiesManager { get { return facilitiesManager; } }
        //public static string FacilitesWorker { get { return facilityWorker; } }
        //public static string FamilyServiceWorker { get { return familyServiceWorker; } }
        //public static string GEAdministrator { get { return gEarthAdministrator; } }
        //public static string HealthManager { get { return healthManager; } }
        //public static string HealthNurse { get { return healthNurse; } }
        //public static string HomeVisitor { get { return homeVisitor; } }
        //public static string HRManager { get { return hrManager; } }
        //public static string HRStaff { get { return hrStaff; } }
        //public static string MentalHealthSpecialist { get { return mentalHealthSpecialist; } }
        //public static string Nutritionist { get { return nutritionist; } }
        //public static string Parent { get { return parent; } }
        //public static string SocialServiceManager { get { return socialServiceManager; } }
        //public static string SuperAdmin { get { return superAdmin; } }
        //public static string Teacher { get { return teacher; } }
        //public static string TeacherAssistant { get { return teacherAssistant; } }
        //public static string TransportManager { get { return transportManager; } }
        //#endregion
    }





    public enum RoleEnum
    {

        AgencyAdmin = 1,
        AreaManager = 2,
        BillingManager = 3,
        BusDriver = 4,
        BusMonitor = 5,
        CenterManager = 6,
        ChildCareProvider = 7,
        DisabilitiesManager = 8,
        DisabilityStaff = 9,
        EducationManager = 10,
        ERSEAManager = 11,
        Executive = 12,
        FacilitiesManager = 13,
        FacilityWorker = 14,
        FamilyServiceWorker = 15,
        GenesisEarthAdministrator = 16,
        HealthManager = 17,
        HealthNurse = 18,
        HomeVisitor = 19,
        HRManager = 20,
        HRStaff = 21,
        MentalHealthSpecialist = 22,
        Nutritionist = 23,
        Parent = 24,
        SocialServiceManager = 25,
        SuperAdmin = 26,
        Teacher = 27,
        TeacherAssistant = 28,
        TransportManager = 29
    }
    public class UserDetails
    {
        public bool ToView { get; set; }
        public bool ToFollowUp { get; set; }
        public bool ToEnter { get; set; }
        public string ColorCode { get; set; }
        public string UserId { get; set; }
        public string StaffName { get; set; }
        public bool IsAllow { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }

    }

    public class AcceptanceRole
    {
        public List<Role> RoleList = new List<Role>();
        public int Priority { get; set; }
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public bool isAllowIncome { get; set; }
        public string ActiveProgramYear { get; set; }
    }
}
