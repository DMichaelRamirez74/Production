using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class PIRModel
    {
        public string Gen_Agency { get; set; }
        public string Gen_Address { get; set; }
        public string Gen_Phone1 { get; set; }
        public string Gen_Fax { get; set; }
        public string Gen_FName { get; set; }
        public string Gen_LName { get; set; }
        public string Gen_Email { get; set; }
        public string Gen_Grantee { get; set; }
        public string Gen_Delegate { get; set; }
        public string Gen_Zip { get; set; }
        public string Gen_ProgramType { get; set; }
        public string A_ProgramEndDate { get; set; }
        public string A_ProgramStartDate { get; set; }
        public string A_Slots { get; set; }
        public string A_SlotsOther { get; set; }
        public string A_FundQ1 { get; set; }
        public string A_FundQ2 { get; set; }
        public string A_FundQ3 { get; set; }
        public string A_FundQ4 { get; set; }
        public string A_FundQ5 { get; set; }
        public string A_FundQ6 { get; set; }
        public string A_FundQ7 { get; set; }
        public string A_FundQ8 { get; set; }
        public string A_FundQ9 { get; set; }
        public string A_FundQ10 { get; set; }
        public string A_FundQ11 { get; set; }
        public string A_FundQ12 { get; set; }
        public string A_FundQ13 { get; set; }
        public string A_FundQ14 { get; set; }
        public string A_FundQ15 { get; set; }
        public string A_FundQ16 { get; set; }
        public string A_11 { get; set; }
        public string A_12 { get; set; }
        public string A_12a { get; set; }
        public string A_13_0 { get; set; }
        public string A_13_1 { get; set; }
        public string A_13_2 { get; set; }
        public string A_13_3 { get; set; }
        public string A_13_4 { get; set; }
        public string A_13_5 { get; set; }
        public string A_13_6 { get; set; }
        public string A_14_Preg { get; set; }
        public string A_15 { get; set; }
        public string A_16A { get; set; }
        public string A_16B { get; set; }
        public string A_16C { get; set; }
        public string A_16D { get; set; }
        public string A_16E { get; set; }
        public string A_16F { get; set; }
        public string A_18A { get; set; }
        public string A_18B { get; set; }
        public string A_19 { get; set; }
        public string A_19A { get; set; }
        public string A_19B { get; set; }
        public string A_20 { get; set; }
        public string A_20A { get; set; }
        public string A_20B { get; set; }
        public string A_20B_1 { get; set; }
        public string A_20B_2 { get; set; }
        public string A_20B_3 { get; set; }
        public string A_21 { get; set; }
        public string A_22 { get; set; }
        public string A_22_A { get; set; }
        public string A_22_B { get; set; }
        public string A_25a1 { get; set; }
        public string A_25a2 { get; set; }
        public string A_25b1 { get; set; }
        public string A_25b2 { get; set; }
        public string A_25c1 { get; set; }
        public string A_25c2 { get; set; }
        public string A_25d1 { get; set; }
        public string A_25d2 { get; set; }
        public string A_25e1 { get; set; }
        public string A_25e2 { get; set; }
        public string A_25f1 { get; set; }
        public string A_25f2 { get; set; }
        public string A_25g1 { get; set; }
        public string A_25g2 { get; set; }
        public string A_25h1 { get; set; }
        public string A_25h2 { get; set; }
        public string A_26a { get; set; }
        public string A_26b { get; set; }
        public string A_26c { get; set; }
        public string A_26d { get; set; }
        public string A_26e { get; set; }
        public string A_26f { get; set; }
        public string A_26g { get; set; }
        public string A_26h { get; set; }
        public string A_26i { get; set; }
        public string A_26j { get; set; }
        public string A_26k { get; set; }
        public string A_27 { get; set; }
        public string A_27A { get; set; }


        public string C_1_1 { get; set; }
        public string C_1_2 { get; set; }
        public string C_A_1 { get; set; }
        public string C_A_2 { get; set; }
        public string C_B_1 { get; set; }
        public string C_B_2 { get; set; }
        public string C_C_1 { get; set; }
        public string C_C_2 { get; set; }
        public string C_D_1 { get; set; }
        public string C_D_2 { get; set; }
        public string C_2_1 { get; set; }
        public string C_2_2 { get; set; }
        public string C_3_1 { get; set; }
        public string C_3_2 { get; set; }
        public string C_3A_1 { get; set; }
        public string C_3A_2 { get; set; }
        public string C_3B_1 { get; set; }
        public string C_3B_2 { get; set; }
        public string C_3C_1 { get; set; }
        public string C_3C_2 { get; set; }
        public string C_3D_1 { get; set; }
        public string C_3D_2 { get; set; }
        public string C_4_1 { get; set; }
        public string C_4_2 { get; set; }
        public string C_5_1 { get; set; }
        public string C_5_2 { get; set; }
        public string C_6_1 { get; set; }
        public string C_6_2 { get; set; }
        public string C_8_1 { get; set; }
        public string C_8_2 { get; set; }

        public string C_8_A { get; set; }
        public string C_8_A_1 { get; set; }
        public string C_9_A { get; set; }
        public string C_9_B { get; set; }
        public string C_9_C { get; set; }
        public string C_9_D { get; set; }
        public string C_9_E { get; set; }
        public string C_9_F { get; set; }
        public string C_10_A { get; set; }
        public string C_10_B { get; set; }
        public string C_10_C { get; set; }
        public string C_10_D { get; set; }
        public string C_11_1 { get; set; }
        public string C_11_2 { get; set; }
        public string C_12_1 { get; set; }
        public string C_12_2 { get; set; }
        public string C_13_1 { get; set; }
        public string C_13_2 { get; set; }


        public string C_14_A { get; set; }
        public string C_14_B { get; set; }
        public string C_14_C { get; set; }
        public string C_14_D { get; set; }
        public string C_14_E { get; set; }
        public string C_14_F { get; set; }
        public string C_14_G { get; set; }
        public string C_15_A { get; set; }
        public string C_15_B { get; set; }
        public string C_15_C { get; set; }
        public string C_16 { get; set; }

        public string C_17_1 { get; set; }
        public string C_17_2 { get; set; }
        public string C_18 { get; set; }
        public string C_19 { get; set; }
        public string C_19_A { get; set; }
        public string C_19_A_1 { get; set; }
        public string C_20 { get; set; }
        public string C_21 { get; set; }

       
        public string pirQuestion { get; set; }
        public string clientName { get; set; }
       
            public string DOB { get; set; }
            public string center { get; set; }
            public string classroom { get; set; }
            public string piratenrollment { get; set; }
            public string piratenrollmentDesc { get; set; }
            public string pirafterenrollment { get; set; }
            public string pirafterenrollmentDesc { get; set; }
            public List<PIRModel> PIRlst { get; set; }

       
    }

   
}
