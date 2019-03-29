using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FingerprintsModel
{


   


    public sealed class TransitionWithdrawal
    {
       
        private static TransitionWithdrawal instance = null;
        private static readonly object padlock = new object();
        private TransitionWithdrawal()
        {
        }

        public static TransitionWithdrawal Instance
        {
            get
            {
                lock (padlock)
                {
                    
                        instance = new TransitionWithdrawal();
                  
                    return instance;
                }
            }
        }


        private string _wQ1 = string.Empty;
        private string WQ2 = string.Empty;
        private string WQ3 = string.Empty;

           // public List<TranWithClients> ClientTransitionDetails { get; set; }

     //   public List<SelectListItem> FSWHVList { get; set; }

        public List<WithdrawalQuestions> TransitionWithdrawnClients { get; set; }

        public bool IsWithdrawal { get; set; }
        public int TotalRecord { get; set; }

        public Enums.TransitionMode ProcessMode;

        public List<SelectListItem> ProgramYears { get; set; }

    }

    public class TranWithClients
    {

        public Transition TransitionClients{get; set;}

        public string FSWHV { get; set; }
        public string HealthWorker { get; set; }
        public bool isReturning { get; set; }
        public bool isWaiting { get; set; }
        public string CenterName { get; set; }
        public string ClassroomName{get;set;}


    }


    public class WithdrawalQuestions
    {

        public string Client { get; set; }
        public string Center { get; set; }

        public long ClientID { get; set; }
        public string Enc_ClientID { get; set; }

        public string Classroom { get; set; }

        public bool ISWaiting { get; set; }
        public string WithdrawnDate { get; set; }
        public int WithdrawnDays { get; set; }
        //Transition//
        public string LDAAge { get; set; }
      
        public bool IsReturning { get; set; }
        //Transition//
        public string FSWHV { get; set; }
        public string ParentID { get; set; }
        public string ParentID1 { get; set; }

        public string ParentName { get; set; }
        public string ParentName1 { get; set; }
        public string HealthManager { get; set; }
        public string InsuranceTypeQ1Start { get; set; }

        public string DescInsurancTypeQ1Start { get; set; }

        public string InsuranceTypeQ1End { get; set; }

        public string DescInsurancTypeQ1End { get; set; }

        public bool IsAnsweredQ1 { get; set; }
        public string MedicalHomeQ2Start { get; set; }
        public string MedicalHomeQ2End { get; set; }
        public bool IsAnsweredQ2 { get; set; }


        public string MedicalServiceQ3Start { get; set; }
        public string MedicalServiceQ3End { get; set; }
        public bool IsAnsweredQ3 { get; set; }


        public string DentalHomeQ4Start { get; set; }
        public string DentalHomeQ4End { get; set; }
        public bool IsAnsweredQ4 { get; set; }



        public string DentalServiceQ5Start { get; set; }
        public string DentalServiceQ5End { get; set; }
        public bool IsAnsweredQ5 { get; set; }



        public string ImmunizationQ6Start { get; set; }
        
        public string ImmunizationQ6End { get; set; }
        public bool IsAnsweredQ6 { get; set; }



        public string FamilyServiceTANFQ7Start { get; set; }

        public string FamilyServiceSSIQ7Start { get; set; }


        public string FamilyServiceSNAPQ7Start { get; set; }


        public string FamilyServiceWICQ7Start { get; set; }

        public string FamilyServiceNoneQ7Start { get; set; }
        public string FamilyServiceTANFQ7End { get; set; }
        public string FamilyServiceSSIQ7End { get; set; }

        public string FamilyServiceSNAPQ7End{ get; set; }

        public string FamilyServiceWICQ7End { get; set; }

        public string FamilyServiceNoneQ7End { get; set; }
        public bool IsAnsweredQ7 { get; set; }



        public string EducationQ8Start { get; set; }
        public string EducationQ8End { get; set; }

        public string EducationQ8Start1 { get; set; }
        public string EducationQ8End1 { get; set; }

        public bool IsAnsweredQ8 { get; set; }


        public bool IsJobTrainingSchool { get; set; }

        public string JobTrainingCompletedQ9Start { get; set; }
        public string JobTrainingCompletedQ9End { get; set; }

        public string JobTrainingCompletedQ9Start1 { get; set; }
        public string JobTrainingCompletedQ9End1 { get; set; }
        public bool IsAnsweredQ9 { get; set; }
        public bool IsPregMom { get; set; }

        public bool IsShowDentalServiceQuestion { get; set; }

        public bool IsShowQ9 { get; set; }

        public string ProgramTypeID { get; set; }

        public string ProgramType { get; set; }

       //for Ajax call success//
        public bool ResponseStatus { get; set; }
      
    }


    //public enum Mode
    //{
    //    Withdrawal = 1,
    //    Transition = 2,
    //    Others=3
    //}


}
