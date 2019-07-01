using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel.Enums
{
    public enum RoleEnum
    {
        [Description(Role.agencyAdmin)]
        
        AgencyAdmin = 1,

        [Description(Role.areaManager)]
        AreaManager = 2,

        [Description(Role.billingManager)]
        BillingManager = 3,

        [Description(Role.busDriver)]
        BusDriver = 4,

        [Description(Role.busMonitor)]
        BusMonitor = 5,

        [Description(Role.centerManager)]
        CenterManager = 6,

        [Description(Role.childCareProvider)]
        ChildCareProvider = 7,

        [Description(Role.disabilitiesManager)]
        DisabilitiesManager = 8,

        [Description(Role.disabilityStaff)]
        DisabilityStaff = 9,

        [Description(Role.educationManager)]
        EducationManager = 10,

        [Description(Role.erseaManager)]
        ERSEAManager = 11,

        [Description(Role.executive)]
        Executive = 12,

        [Description(Role.facilitiesManager)]
        FacilitiesManager = 13,

        [Description(Role.facilityWorker)]
        FacilityWorker = 14,

        [Description(Role.familyServiceWorker)]
        FamilyServiceWorker = 15,

        [Description(Role.gEarthAdministrator)]
        GenesisEarthAdministrator = 16,

        [Description(Role.healthManager)]
        HealthManager = 17,

        [Description(Role.healthNurse)]
        HealthNurse = 18,

        [Description(Role.homeVisitor)]
        HomeVisitor = 19,

        [Description(Role.hrManager)]
        HRManager = 20,

        [Description(Role.hrStaff)]
        HRStaff = 21,

        [Description(Role.mentalHealthSpecialist)]
        MentalHealthSpecialist = 22,

        [Description(Role.nutritionist)]
        Nutritionist = 23,

        [Description(Role.parent)]
        Parent = 24,

        [Description(Role.socialServiceManager)]
        SocialServiceManager = 25,

        [Description(Role.superAdmin)]
        SuperAdmin = 26,

        [Description(Role.teacher)]
        Teacher = 27,

        [Description(Role.teacherAssistant)]
        TeacherAssistant = 28,

        [Description(Role.transportManager)]
        TransportManager = 29
    }
}
