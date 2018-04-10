using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class Transition
    {
        public long ClientId { get; set; }
        public string EClientID { get; set; }
        public long ProgramTypeId { get; set; }
        public string DateOfTransition { get; set; }

        public string Name { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }

        public string HouseholdId { get; set; }
        public string Status { get; set; }
        public string StatusText { get; set; }
        public string Reason { get; set; }
        public string ddlreason { get; set; }
        public string ddlreasontext { get; set; }
        public string IsWaiting { get; set; }
        public int EnrollmentStatus { get; set; }
        public int InsuranceType { get; set; }

        public int BirthType { get; set; }

        public bool IsEHS { get; set; }

        public bool IsHS { get; set; }

        public string ActiveRecCode { get; set; }
        public int MedicaidCHIP_S_C3A1 { get; set; }
        public int MedicaidCHIP_E_C3A2 { get; set; }
        public int StateFunded_S_C3B1 { get; set; }
        public int StateFunded_E_C3B2 { get; set; }
        public int PrivateIns_S_C3C1 { get; set; }
        public int PrivateIns_E_C3C2 { get; set; }

        public int OtherIns_S_C3D1 { get; set; }
        public int OtherIns_E_C3D2 { get; set; }
        public string Description_S_C3D11 { get; set; }
        public string Description_E_C3D11 { get; set; }
        public int NoIns_S_C4 { get; set; }
        public int NoIns_E_C4 { get; set; }






        public int MedicaidCHIP_S_C1A1 { get; set; }
        public int MedicaidCHIP_E_C1A2 { get; set; }
        public int StateFunded_S_C1B1 { get; set; }
        public int StateFunded_E_C1B2 { get; set; }
        public int PrivateIns_S_C1C1 { get; set; }
        public int PrivateIns_E_C1C2 { get; set; }

        public int OtherIns_S_C1D1 { get; set; }
        public int OtherIns_E_C1D2 { get; set; }
        public string Description_S_C1D11 { get; set; }
        public string Description_E_C1D11 { get; set; }
        public int NoIns_S_C2 { get; set; }
        public int NoIns_E_C2 { get; set; }
        public  bool JobTrainingFinished { get; set; }

        public int ShoolAchievement { get; set; }
        public bool TANF { get; set; }
        public bool SSI { get; set; }

        public bool WIC { get; set; }
        public bool SNAP { get; set; }
        public bool HighSchool { get; set; }
        public bool HighSchoolGraduate { get; set; }
        public bool AssociateDegree { get; set; }
        public bool AdvancedDegree { get; set; }

        public int ImmunizationService { get; set; }

        public bool MedicalHome { get; set; }

        public bool DentalHome { get; set; }
        public bool DentalServices { get; set; }
        public bool MedicalServices { get; set; }
        public int MedicalServiceTypes { get; set; }
        public int DentalCare { get; set; }

        public string TrnsInsuranceType { get; set; }

        public int CenterId { get; set; }

        public int ClassRoomId { get; set; }

        public string OtherInsuranceTypeDesc { get; set; }
        public string ChildOtherInsuranceTypeDesc { get; set; }

        public string ChildInsuranceType { get; set; }

        public int TransitioningType { get; set; }
    }

    public class TransitionDetails {
        public Transition Transition { get; set; }
        public List<PregMomChilds> PregMomChilds { get; set; }
    }
    public class PregMomChilds {

        public Transition Transition { get; set; }
        public string DateOfTransition { get; set; }

        public string Name { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }


        
        public int InsuranceType { get; set; }
        public bool IsEHS { get; set; }
    }


 
    public class SeatAvailability
    {
        public int SloatAvailable { get; set; }
        public int SeatAvailable { get; set; }
        public int Result { get; set; }
        public string ChildName { get; set; }
    }
}
