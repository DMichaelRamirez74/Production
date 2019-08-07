using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FingerprintsModel;
using System.Web.Mvc;
using System.IO;
using System.Web;
using ClosedXML;
using ClosedXML.Excel;

namespace FingerprintsData
{
    public class PIRData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;

        public void GetPIRSummary(DataSet _dataset, PIRModel _PIR)
        {
           if (_dataset != null)
            {
                if (_dataset.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {


                         _PIR.Gen_Agency = dr["AgencyName"].ToString();
                          _PIR.Gen_Address = dr["Address1"].ToString();
                          _PIR.Gen_Delegate = dr["Delegate"].ToString();
                          _PIR.Gen_Grantee = dr["GranteeNo"].ToString();
                          _PIR.Gen_FName = dr["Firstname"].ToString();
                          _PIR.Gen_LName = dr["Lastname"].ToString();
                          _PIR.Gen_Phone1 = dr["Phone1"].ToString();
                          _PIR.Gen_Fax = dr["Fax"].ToString();
                           _PIR.Gen_Zip = dr["ZipCode"].ToString();
                           _PIR.Gen_Email = dr["EmailAddress"].ToString();
                           _PIR.A_ProgramStartDate = dr["ProgramStartDate"].ToString();
                           _PIR.A_ProgramEndDate = dr["ProgramEndDate"].ToString();
                        _PIR.A_Slots = dr["ServiceQty"].ToString();
                        _PIR.A_SlotsOther = dr["ServiceQtyOther"].ToString();
                         _PIR.A_FundQ1 = dr["FundQ1"].ToString();
                           _PIR.A_FundQ2 = dr["FundQ2"].ToString();
                           _PIR.A_FundQ3 = dr["FundQ3"].ToString();
                           _PIR.A_FundQ4 = dr["FundQ4"].ToString();
                           _PIR.A_FundQ5 = dr["FundQ5"].ToString();
                           _PIR.A_FundQ6 = dr["FundQ6"].ToString();
                           _PIR.A_FundQ7 = dr["FundQ7"].ToString();
                           _PIR.A_FundQ8 = dr["FundQ8"].ToString();
                           _PIR.A_FundQ9 = dr["FundQ9"].ToString();
                           _PIR.A_FundQ10 = dr["FundQ10"].ToString();
                           _PIR.A_FundQ11 = dr["FundQ11"].ToString();
                           _PIR.A_FundQ12 = dr["FundQ12"].ToString();
                           _PIR.A_FundQ13 = dr["FundQ13"].ToString();
                           _PIR.A_FundQ14 = dr["FundQ14"].ToString();
                           _PIR.A_FundQ15 = dr["FundQ15"].ToString();
                           _PIR.A_FundQ16 = dr["FundQ16"].ToString();
                            _PIR.A_11 = dr["A11"].ToString();
                         _PIR.A_12 = dr["A12"].ToString();
                          _PIR.A_12a = dr["A12a"].ToString();
                         _PIR.A_13_0 = dr["A13_0"].ToString();
                          _PIR.A_13_1 = dr["A13_1"].ToString();
                          _PIR.A_13_2 = dr["A13_2"].ToString();
                          _PIR.A_13_3 = dr["A13_3"].ToString();
                          _PIR.A_13_4 = dr["A13_4"].ToString();
                          _PIR.A_13_5 = dr["A13_5"].ToString();
                        
                          _PIR.A_14_Preg = dr["A14_Preg"].ToString();
                         _PIR.A_15 = dr["A15Total"].ToString();
                         _PIR.A_16A = dr["A16A"].ToString();
                         _PIR.A_16B = dr["A16B"].ToString();
                         _PIR.A_16C = dr["A16C"].ToString();
                         _PIR.A_16D = dr["A16D"].ToString();
                         _PIR.A_16E = dr["A16E"].ToString();
                         _PIR.A_16F = dr["A16F"].ToString();
                          _PIR.A_18A = dr["A18_SECOND"].ToString();
                          _PIR.A_18B = dr["A18_THREE"].ToString();
                          _PIR.A_19 = dr["A19_LEFT"].ToString();
                         _PIR.A_19A = dr["A19A_45DAYS"].ToString();
                         _PIR.A_19B = dr["A19_KINDERGARTEN"].ToString();
                         _PIR.A_20 = dr["A20"].ToString();
                         _PIR.A_20A = dr["A20A_45DAYS"].ToString();
                         _PIR.A_20B = dr["A20B"].ToString();
                         _PIR.A_20B_1 = dr["A20B_1"].ToString();
                         _PIR.A_20B_2 = dr["A20B_2"].ToString();
                         _PIR.A_20B_3 = dr["A20B_3"].ToString();
                         _PIR.A_21 = dr["A21"].ToString();
                         _PIR.A_22 = dr["A22"].ToString();
                         _PIR.A_22_A = dr["A22A"].ToString();
                         _PIR.A_22_B = dr["A22B"].ToString();
                         _PIR.A_24 = dr["A24"].ToString();

                         _PIR.A_25a1 = dr["A25a1"].ToString();
                         _PIR.A_25a2 = dr["A25a2"].ToString();
                         _PIR.A_25b1 = dr["A25b1"].ToString();
                         _PIR.A_25b2 = dr["A25b2"].ToString();
                         _PIR.A_25c1 = dr["A25c1"].ToString();
                         _PIR.A_25c2 = dr["A25c2"].ToString();
                         _PIR.A_25d1 = dr["A25d1"].ToString();
                         _PIR.A_25d2 = dr["A25d2"].ToString();
                         _PIR.A_25e1 = dr["A25e1"].ToString();
                         _PIR.A_25e2 = dr["A25e2"].ToString();
                         _PIR.A_25f1 = dr["A25f1"].ToString();
                         _PIR.A_25f2 = dr["A25f2"].ToString();
                         _PIR.A_25g1 = dr["A25g1"].ToString();
                         _PIR.A_25g2 = dr["A25g2"].ToString();
                         _PIR.A_25h1 = dr["A25h1"].ToString();
                         _PIR.A_25h2 = dr["A25h2"].ToString();
                            _PIR.A_26a = dr["A26a"].ToString();
                            _PIR.A_26b = dr["A26b"].ToString();
                            _PIR.A_26c = dr["A26c"].ToString();
                            _PIR.A_26d = dr["A26d"].ToString();
                            _PIR.A_26e = dr["A26e"].ToString();
                            _PIR.A_26f = dr["A26f"].ToString();
                            _PIR.A_26g = dr["A26g"].ToString();
                            _PIR.A_26h = dr["A26h"].ToString();
                            _PIR.A_26i = dr["A26i"].ToString();
                            _PIR.A_26j = dr["A26j"].ToString();
                            _PIR.A_26k = dr["A26k"].ToString();
                         _PIR.A_27 = dr["ChildTransport"].ToString();
                         _PIR.A_27A = dr["A27A"].ToString();


                           _PIR.C_1_1 = dr["C1_AtEnroll"].ToString();
                          _PIR.C_1_2 = dr["C1_EndEnroll"].ToString();
                          _PIR.C_A_1 = dr["C1A_AtEnroll"].ToString();
                          _PIR.C_A_2 = dr["C1A_EndEnroll"].ToString();
                          _PIR.C_B_1 = dr["C1B_AtEnroll"].ToString();
                          _PIR.C_B_2 = dr["C1B_EndEnroll"].ToString();
                          _PIR.C_C_1 = dr["C1C_AtEnroll"].ToString();
                          _PIR.C_C_2 = dr["C1C_EndEnroll"].ToString();
                          _PIR.C_D_1 = dr["C1D_AtEnroll"].ToString();
                          _PIR.C_D_2 = dr["C1D_EndEnroll"].ToString();
                          _PIR.C_2_1 = dr["C2_AtEnroll"].ToString();
                          _PIR.C_2_2 = dr["C2_EndEnroll"].ToString();
                           _PIR.C_3_1 = dr["C3_AtEnroll"].ToString();
                           _PIR.C_3_2 = dr["C3_EndEnroll"].ToString();
                           _PIR.C_3A_1 = dr["C3A_AtEnroll"].ToString();
                           _PIR.C_3A_2 = dr["C3A_EndEnroll"].ToString();
                           _PIR.C_3B_1 = dr["C3B_AtEnroll"].ToString();
                           _PIR.C_3B_2 = dr["C3B_EndEnroll"].ToString();
                           _PIR.C_3C_1 = dr["C3C_AtEnroll"].ToString();
                           _PIR.C_3C_2 = dr["C3C_EndEnroll"].ToString();
                           _PIR.C_3D_1 = dr["C3D_AtEnroll"].ToString();
                           _PIR.C_3D_2 = dr["C3D_EndEnroll"].ToString();
                           _PIR.C_4_1 = dr["C4_1"].ToString();
                           _PIR.C_4_2 = dr["C4_2"].ToString();
                           _PIR.C_5_1 = dr["C5_1"].ToString();
                           _PIR.C_5_2 = dr["C5_2"].ToString();
                           _PIR.C_8_1 = dr["C8_1"].ToString();
                           _PIR.C_8_2 = dr["C8_2"].ToString();
                           _PIR.C_8_A = dr["C8_A"].ToString();
                           _PIR.C_8_A_1 = dr["C8_A_1"].ToString();
                           _PIR.C_9_A = dr["C9_A"].ToString();
                           _PIR.C_9_B = dr["C9_B"].ToString();
                           _PIR.C_9_C = dr["C9_C"].ToString();
                           _PIR.C_9_D = dr["C9_D"].ToString();
                           _PIR.C_9_E = dr["C9_E"].ToString();
                           _PIR.C_9_F = dr["C9_F"].ToString();
                           _PIR.C_10_A = dr["C10_A"].ToString();
                           _PIR.C_10_B = dr["C10_B"].ToString();
                           _PIR.C_10_C = dr["C10_C"].ToString();
                           _PIR.C_10_D = dr["C10_D"].ToString();
                           _PIR.C_11_1 = dr["C11_1"].ToString();
                           _PIR.C_11_2 = dr["C11_2"].ToString();
                           _PIR.C_12_1 = dr["C12_1"].ToString();
                           _PIR.C_12_2 = dr["C12_2"].ToString();
                           _PIR.C_13_1 = dr["C13_1"].ToString();
                           _PIR.C_13_2 = dr["C13_2"].ToString();
                           _PIR.C_14_A = dr["C14A"].ToString();
                           _PIR.C_14_B = dr["C14B"].ToString();
                           _PIR.C_14_C = dr["C14C"].ToString();
                           _PIR.C_14_D = dr["C14D"].ToString();
                           _PIR.C_14_E = dr["C14E"].ToString();
                           _PIR.C_14_F = dr["C14F"].ToString();
                           _PIR.C_14_G = dr["C14G"].ToString();
                           _PIR.C_15_A = dr["C15A"].ToString();
                           _PIR.C_15_B = dr["C15B"].ToString();
                           _PIR.C_15_C = dr["C15C"].ToString();
                           _PIR.C_16 = dr["C16"].ToString();
                           _PIR.C_17_1 = dr["C17_1"].ToString();
                           _PIR.C_17_2 = dr["C17_2"].ToString();
                           _PIR.C_18 = dr["C18"].ToString();
                           _PIR.C_19 = dr["C19"].ToString();
                           _PIR.C_19_A = dr["C19_A"].ToString();
                           _PIR.C_19_A_1 = dr["C19_A_1"].ToString();
                           _PIR.C_20 = dr["C20"].ToString();
                           _PIR.C_21 = dr["C21"].ToString();
                           _PIR.C_25 = dr["C25"].ToString();
                           _PIR.C_26 = dr["C26"].ToString();
                           _PIR.C_27_1 = dr["C27_1"].ToString();
                           _PIR.C_27_2 = dr["C27_2"].ToString();
                           _PIR.C_27_A_1 = dr["C27A_AtEnroll_1"].ToString();
                           _PIR.C_27_A_2 = dr["C27A_AtEnroll_2"].ToString();
                           _PIR.C_27_B_1 = dr["C27B_AtEnroll_1"].ToString();
                           _PIR.C_27_B_2 = dr["C27B_AtEnroll_2"].ToString();
                           _PIR.C_27_C_1 = dr["C27C_AtEnroll_1"].ToString();
                           _PIR.C_27_C_2 = dr["C27C_AtEnroll_2"].ToString();
                           _PIR.C_27_D_1 = dr["C27D_AtEnroll_1"].ToString();
                           _PIR.C_27_D_2 = dr["C27D_AtEnroll_2"].ToString();
                           _PIR.C_27_E_1 = dr["C27E_AtEnroll_1"].ToString();
                           _PIR.C_27_E_2 = dr["C27E_AtEnroll_2"].ToString();
                           _PIR.C_27_F_1 = dr["C27F_AtEnroll_1"].ToString();
                           _PIR.C_27_F_2 = dr["C27F_AtEnroll_2"].ToString();
                           _PIR.C_27_G_1 = dr["C27G_AtEnroll_1"].ToString();
                           _PIR.C_27_G_2 = dr["C27G_AtEnroll_2"].ToString();
                           _PIR.C_27_H_1 = dr["C27H_AtEnroll_1"].ToString();
                           _PIR.C_27_H_2 = dr["C27H_AtEnroll_2"].ToString();
                           _PIR.C_27_I_1 = dr["C27I_AtEnroll_1"].ToString();
                           _PIR.C_27_I_2 = dr["C27I_AtEnroll_2"].ToString();
                           _PIR.C_27_J_1 = dr["C27J_AtEnroll_1"].ToString();
                           _PIR.C_27_J_2 = dr["C27J_AtEnroll_2"].ToString();
                           _PIR.C_27_K_1 = dr["C27K_AtEnroll_1"].ToString();
                           _PIR.C_27_K_2 = dr["C27K_AtEnroll_2"].ToString();
                           _PIR.C_27_L_1 = dr["C27L_AtEnroll_1"].ToString();
                           _PIR.C_27_L_2 = dr["C27L_AtEnroll_2"].ToString();
                           _PIR.C_27_M_1 = dr["C27M_AtEnroll_1"].ToString();
                           _PIR.C_27_M_2 = dr["C27M_AtEnroll_2"].ToString();
                           _PIR.C_35 = dr["C35"].ToString();
                           _PIR.C_35_A = dr["C35A"].ToString();
                           _PIR.C_35_B = dr["C35B"].ToString();
                           _PIR.C_36_A = dr["C36A"].ToString();
                           _PIR.C_36_B = dr["C36B"].ToString();
                           _PIR.C_36_C = dr["C36C"].ToString();
                           _PIR.C_36_D = dr["C36D"].ToString();
                           _PIR.C_36_E = dr["C36E"].ToString();
                           _PIR.C_37_A = dr["C37A"].ToString();
                           _PIR.C_37_B = dr["C37B"].ToString();
                           _PIR.C_37_C = dr["C37C"].ToString();
                           _PIR.C_37_D = dr["C37D"].ToString();
                           _PIR.C_37_E = dr["C37E"].ToString();
                           _PIR.C_37_F = dr["C37F"].ToString();
                           _PIR.C_38_A = dr["C38A"].ToString();
                           _PIR.C_38_B = dr["C38B"].ToString();
                           _PIR.C_38_C = dr["C38C"].ToString();
                           _PIR.C_39_A = dr["C39A"].ToString();
                           _PIR.C_39_B = dr["C39B"].ToString();
                           _PIR.C_40_A = dr["C40A"].ToString();
                           _PIR.C_40_B = dr["C40B"].ToString();
                           _PIR.C_41_1 = dr["C41_1"].ToString();
                           _PIR.C_41_2 = dr["C41_2"].ToString();
                           _PIR.C_42_1 = dr["C42_1"].ToString();
                           _PIR.C_42_2 = dr["C42_2"].ToString();
                           _PIR.C_43_1 = dr["C43_1"].ToString();
                           _PIR.C_43_2 = dr["C43_2"].ToString();
                           _PIR.C_44_1 = dr["C44_1"].ToString();
                           _PIR.C_44_2 = dr["C44_2"].ToString();
                           _PIR.C_45_A = dr["C45A"].ToString();
                           _PIR.C_45_B = dr["C45B"].ToString();
                           _PIR.C_45_C = dr["C45C"].ToString();
                           _PIR.C_46_A = dr["C46A"].ToString();
                           _PIR.C_46_B = dr["C46B"].ToString();
                           _PIR.C_47_A = dr["C47A"].ToString();
                           _PIR.C_47_B = dr["C47B"].ToString();
                           _PIR.C_47_C = dr["C47C"].ToString();
                           _PIR.C_47_D = dr["C47D"].ToString();
                           _PIR.C_48 = dr["C48"].ToString();
                           _PIR.C_49_A = dr["C49A"].ToString();
                           _PIR.C_49_B = dr["C49B"].ToString();
                           _PIR.C_49_C = dr["C49C"].ToString();
                           _PIR.C_49_D = dr["C49D"].ToString();
                           _PIR.C_50_A_1 = dr["C50A_1"].ToString();
                           _PIR.C_50_A_2 = dr["C50A_2"].ToString();

                           _PIR.C_50_B_1 = dr["C50B_1"].ToString();
                           _PIR.C_50_B_2 = dr["C50B_2"].ToString();
                           _PIR.C_50_C_1 = dr["C50C_1"].ToString();
                           _PIR.C_50_C_2 = dr["C50C_2"].ToString();
                           _PIR.C_50_D_1 = dr["C50D_1"].ToString();
                           _PIR.C_50_D_2 = dr["C50D_2"].ToString();
                           _PIR.C_50_E_1 = dr["C50E_1"].ToString();
                           _PIR.C_50_E_2 = dr["C50E_2"].ToString();
                           _PIR.C_50_F_1 = dr["C50F_1"].ToString();
                           _PIR.C_50_F_2 = dr["C50F_2"].ToString();
                           _PIR.C_50_G_1 = dr["C50G_1"].ToString();
                           _PIR.C_50_G_2 = dr["C50G_2"].ToString();
                           _PIR.C_50_H_1 = dr["C50H_1"].ToString();
                           _PIR.C_50_H_2 = dr["C50H_2"].ToString();
                           _PIR.C_50_I_1 = dr["C50I_1"].ToString();
                           _PIR.C_50_I_2 = dr["C50I_2"].ToString();
                           _PIR.C_50_J_1 = dr["C50J_1"].ToString();
                           _PIR.C_50_J_2 = dr["C50J_2"].ToString();
                           _PIR.C_50_K_1 = dr["C50K_1"].ToString();
                           _PIR.C_50_K_2 = dr["C50K_2"].ToString();
                           _PIR.C_50_L_1 = dr["C50L_1"].ToString();
                           _PIR.C_50_L_2 = dr["C50L_2"].ToString();
                           _PIR.C_50_M_1 = dr["C50M_1"].ToString();
                           _PIR.C_50_M_2 = dr["C50M_2"].ToString();
                           _PIR.C_50_N_1 = dr["C50N_1"].ToString();
                           _PIR.C_50_N_2 = dr["C50N_2"].ToString();
                           _PIR.C_50_O_1 = dr["C50O_1"].ToString();
                           _PIR.C_50_O_2 = dr["C50O_2"].ToString();
                           _PIR.C_50_P_1 = dr["C50P_1"].ToString();
                           _PIR.C_50_P_2 = dr["C50P_2"].ToString();
                           _PIR.C_51_1 = dr["C51_1"].ToString();
                           _PIR.C_51_2 = dr["C51_2"].ToString();
                           _PIR.C_52_A = dr["C52A"].ToString();
                           _PIR.C_52_B = dr["C52B"].ToString();
                           _PIR.C_52_C = dr["C52C"].ToString();
                           _PIR.C_52_D = dr["C52D"].ToString();
                           _PIR.C_52_E = dr["C52E"].ToString();
                           _PIR.C_53 = dr["C53"].ToString();
                           _PIR.C_54 = dr["C54"].ToString();
                           _PIR.C_55 = dr["C55"].ToString();
                           _PIR.C_56 = dr["C56"].ToString();
                           _PIR.C_57 = dr["C57"].ToString();
                           _PIR.B1_1 = dr["B1_1"].ToString();
                           _PIR.B1_2 = dr["B1_2"].ToString();
                           _PIR.B1A_1 = dr["B1A_1"].ToString();
                           _PIR.B1A_2 = dr["B1A_2"].ToString();
                           _PIR.B1B_1 = dr["B1B_1"].ToString();
                           _PIR.B1B_2 = dr["B1B_2"].ToString();
                           _PIR.B1B_1_1 = dr["B1B_1_1"].ToString();
                           _PIR.B1B_1_2 = dr["B1B_1_2"].ToString();

                           _PIR.B3A_1 = dr["B3A_1"].ToString();
                           _PIR.B3A_2 = dr["B3A_2"].ToString();
                           _PIR.B3B_1 = dr["B3B_1"].ToString();
                           _PIR.B3B_2 = dr["B3B_2"].ToString();
                           _PIR.B3C_1 = dr["B3C_1"].ToString();
                           _PIR.B3C_2 = dr["B3C_2"].ToString();
                           _PIR.B3D_1 = dr["B3D_1"].ToString();
                           _PIR.B3D_2 = dr["B3D_2"].ToString();
                           _PIR.B3E_1 = dr["B3E_1"].ToString();
                           _PIR.B3E_2 = dr["B3E_2"].ToString();
                           _PIR.B3F_1 = dr["B3F_1"].ToString();
                           _PIR.B3F_2 = dr["B3F_2"].ToString();
                           _PIR.B3G_1 = dr["B3G_1"].ToString();
                           _PIR.B3G_2 = dr["B3G_2"].ToString();

                           _PIR.B5_1 = dr["B5_1"].ToString();
                             _PIR.B5_2 = dr["B5_2"].ToString();
                            _PIR.B5A_1_1 = dr["B5A_1_1"].ToString();
                            _PIR.B5A_1_2 = dr["B5A_1_2"].ToString();
                            _PIR.B5A_2_1 = dr["B5A_2_1"].ToString();
                            _PIR.B5A_2_2 = dr["B5A_2_2"].ToString();
                            _PIR.B5B_1_1 = dr["B5B_1_1"].ToString();
                            _PIR.B5B_1_2 = dr["B5B_1_2"].ToString();
                            _PIR.B5B_2_1 = dr["B5B_2_1"].ToString();
                            _PIR.B5B_2_2 = dr["B5B_2_2"].ToString();
                            _PIR.B5B_3_1 = dr["B5B_3_1"].ToString();
                            _PIR.B5B_3_2 = dr["B5B_3_2"].ToString();
                            _PIR.B5B_4_1 = dr["B5B_4_1"].ToString();
                            _PIR.B5B_4_2 = dr["B5B_4_2"].ToString();
                            _PIR.B5C_1_1 = dr["B5C_1_1"].ToString();
                            _PIR.B5C_1_2 = dr["B5C_1_2"].ToString();
                            _PIR.B5C_2_1 = dr["B5C_2_1"].ToString();
                            _PIR.B5C_2_2 = dr["B5C_2_2"].ToString();
                            _PIR.B5C_3_1 = dr["B5C_3_1"].ToString();
                            _PIR.B5C_3_2 = dr["B5C_3_2"].ToString();
                            _PIR.B5D_1 = dr["B5D_1"].ToString();
                            _PIR.B5D_2 = dr["B5D_2"].ToString();
                            _PIR.B5D_1_1 = dr["B5D_1_1"].ToString();
                            _PIR.B5D_1_2 = dr["B5D_1_2"].ToString();
                            _PIR.B5D_2_1 = dr["B5D_2_1"].ToString();
                            _PIR.B5D_2_2 = dr["B5D_2_2"].ToString();
                            _PIR.B5D_3_1 = dr["B5D_3_1"].ToString();
                            _PIR.B5D_3_2 = dr["B5D_3_2"].ToString();
                            _PIR.B5E_1_1 = dr["B5E_1_1"].ToString();
                            _PIR.B5E_1_2 = dr["B5E_1_2"].ToString();
                            _PIR.B5E_2_1 = dr["B5E_2_1"].ToString();
                            _PIR.B5E_2_2 = dr["B5E_2_2"].ToString();
                            _PIR.B5E_3_1 = dr["B5E_3_1"].ToString();
                            _PIR.B5E_3_2 = dr["B5E_3_2"].ToString();
                            _PIR.B6 = dr["B6"].ToString();
                            _PIR.B7 = dr["B7"].ToString();

                            _PIR.B8_1 = dr["B8_1"].ToString();
                            _PIR.B8_2 = dr["B8_2"].ToString();
                            _PIR.B8A_1_1 = dr["B8A_1_1"].ToString();
                            _PIR.B8A_1_2 = dr["B8A_1_2"].ToString();
                            _PIR.B8A_2_1 = dr["B8A_2_1"].ToString();
                            _PIR.B8A_2_2 = dr["B8A_2_2"].ToString();
                            _PIR.B8B_1_1 = dr["B8B_1_1"].ToString();
                            _PIR.B8B_1_2 = dr["B8B_1_2"].ToString();
                            _PIR.B8B_2_1 = dr["B8B_2_1"].ToString();
                            _PIR.B8B_2_2 = dr["B8B_2_2"].ToString();
                            _PIR.B8B_3_1 = dr["B8B_3_1"].ToString();
                            _PIR.B8B_3_2 = dr["B8B_3_2"].ToString();
                            _PIR.B8B_4_1 = dr["B8B_4_1"].ToString();
                            _PIR.B8B_4_2 = dr["B8B_4_2"].ToString();
                            _PIR.B8C_1_1 = dr["B8C_1_1"].ToString();
                            _PIR.B8C_1_2 = dr["B8C_1_2"].ToString();
                            _PIR.B8C_2_1 = dr["B8C_2_1"].ToString();
                            _PIR.B8C_2_2 = dr["B8C_2_2"].ToString();
                            _PIR.B8C_3_1 = dr["B8C_3_1"].ToString();
                            _PIR.B8C_3_2 = dr["B8C_3_2"].ToString();
                            _PIR.B8D_1 = dr["B8D_1"].ToString();
                            _PIR.B8D_2 = dr["B8D_2"].ToString();
                            _PIR.B8D_1_1 = dr["B8D_1_1"].ToString();
                            _PIR.B8D_1_2 = dr["B8D_1_2"].ToString();
                            _PIR.B8D_2_1 = dr["B8D_2_1"].ToString();
                            _PIR.B8D_2_2 = dr["B8D_2_2"].ToString();
                            _PIR.B8D_3_1 = dr["B8D_3_1"].ToString();
                            _PIR.B8D_3_2 = dr["B8D_3_2"].ToString();
                            _PIR.B8E_1_1 = dr["B8E_1_1"].ToString();
                            _PIR.B8E_1_2 = dr["B8E_1_2"].ToString();
                            _PIR.B8E_2_1 = dr["B8E_2_1"].ToString();
                            _PIR.B8E_2_2 = dr["B8E_2_2"].ToString();
                            _PIR.B8E_3_1 = dr["B8E_3_1"].ToString();
                            _PIR.B8E_3_2 = dr["B8E_3_2"].ToString();

                            _PIR.B9_1 = dr["B9_1"].ToString();
                            _PIR.B9_2 = dr["B9_2"].ToString();
                            _PIR.B9_3 = dr["B9_3"].ToString();
                            _PIR.B9_4 = dr["B9_4"].ToString();
                            _PIR.B9A_1_1 = dr["B9A_1_1"].ToString();
                            _PIR.B9A_1_2 = dr["B9A_1_2"].ToString();
                            _PIR.B9A_1_3 = dr["B9A_1_3"].ToString();
                            _PIR.B9A_1_4 = dr["B9A_1_4"].ToString();
                            _PIR.B9A_2_1 = dr["B9A_2_1"].ToString();
                            _PIR.B9A_2_2 = dr["B9A_2_2"].ToString();
                            _PIR.B9A_2_3 = dr["B9A_2_3"].ToString();
                            _PIR.B9A_2_4 = dr["B9A_2_4"].ToString();
                            _PIR.B9A_3_1 = dr["B9A_3_1"].ToString();
                            _PIR.B9A_3_2 = dr["B9A_3_2"].ToString();
                            _PIR.B9A_3_3 = dr["B9A_3_3"].ToString();
                            _PIR.B9A_3_4 = dr["B9A_3_4"].ToString();
                            _PIR.B9A_4_1 = dr["B9A_4_1"].ToString();
                            _PIR.B9A_4_2 = dr["B9A_4_2"].ToString();
                            _PIR.B9A_4_3 = dr["B9A_4_3"].ToString();
                            _PIR.B9A_4_4 = dr["B9A_4_4"].ToString();
                            _PIR.B9A_5_1 = dr["B9A_5_1"].ToString();
                            _PIR.B9A_5_2 = dr["B9A_5_2"].ToString();
                            _PIR.B9A_5_3 = dr["B9A_5_3"].ToString();
                            _PIR.B9A_5_4 = dr["B9A_5_4"].ToString();
                            _PIR.B9A_6_1 = dr["B9A_6_1"].ToString();
                            _PIR.B9A_6_2 = dr["B9A_6_2"].ToString();
                            _PIR.B9A_6_3 = dr["B9A_6_3"].ToString();
                            _PIR.B9A_6_4 = dr["B9A_6_4"].ToString();
                            _PIR.B9A_7_1 = dr["B9A_7_1"].ToString();
                            _PIR.B9A_7_2 = dr["B9A_7_2"].ToString();
                            _PIR.B9A_7_3 = dr["B9A_7_3"].ToString();
                            _PIR.B9A_7_4 = dr["B9A_7_4"].ToString();
                            _PIR.B9A_8_1 = dr["B9A_8_1"].ToString();
                            _PIR.B9A_8_2 = dr["B9A_8_2"].ToString();
                            _PIR.B9A_8_3 = dr["B9A_8_3"].ToString();
                            _PIR.B9A_8_4 = dr["B9A_8_4"].ToString();
                            _PIR.B9B_1_1 = dr["B9B_1_1"].ToString();
                            _PIR.B9B_1_2 = dr["B9B_1_2"].ToString();
                            _PIR.B9B_1_3 = dr["B9B_1_3"].ToString();
                            _PIR.B9B_1_4 = dr["B9B_1_4"].ToString();
                            _PIR.B9B_2_1 = dr["B9B_2_1"].ToString();
                            _PIR.B9B_2_2 = dr["B9B_2_2"].ToString();
                            _PIR.B9B_2_3 = dr["B9B_2_3"].ToString();
                            _PIR.B9B_2_4 = dr["B9B_2_4"].ToString();
                            _PIR.B9B_3_1 = dr["B9B_3_1"].ToString();
                            _PIR.B9B_3_2 = dr["B9B_3_2"].ToString();
                            _PIR.B9B_3_3 = dr["B9B_3_3"].ToString();
                            _PIR.B9B_3_4 = dr["B9B_3_4"].ToString();
                            _PIR.B9B_4_1 = dr["B9B_4_1"].ToString();
                            _PIR.B9B_4_2 = dr["B9B_4_2"].ToString();
                            _PIR.B9B_4_3 = dr["B9B_4_3"].ToString();
                            _PIR.B9B_4_4 = dr["B9B_4_4"].ToString();
                            _PIR.B9B_5_1 = dr["B9B_5_1"].ToString();
                            _PIR.B9B_5_2 = dr["B9B_5_2"].ToString();
                            _PIR.B9B_5_3 = dr["B9B_5_3"].ToString();
                            _PIR.B9B_5_4 = dr["B9B_5_4"].ToString();
                            _PIR.B9B_6_1 = dr["B9B_6_1"].ToString();
                            _PIR.B9B_6_2 = dr["B9B_6_2"].ToString();
                            _PIR.B9B_6_3 = dr["B9B_6_3"].ToString();
                            _PIR.B9B_6_4 = dr["B9B_6_4"].ToString();
                            _PIR.B9B_7_1 = dr["B9B_7_1"].ToString();
                            _PIR.B9B_7_2 = dr["B9B_7_2"].ToString();
                            _PIR.B9B_7_3 = dr["B9B_7_3"].ToString();
                            _PIR.B9B_7_4 = dr["B9B_7_4"].ToString();
                            _PIR.B9C_1_1 = dr["B9C_1_1"].ToString();
                            _PIR.B9C_1_2 = dr["B9C_1_2"].ToString();
                            _PIR.B9C_1_3 = dr["B9C_1_3"].ToString();
                            _PIR.B9C_1_4 = dr["B9C_1_4"].ToString();
                            _PIR.B9C_2_1 = dr["B9C_2_1"].ToString();
                            _PIR.B9C_2_2 = dr["B9C_2_2"].ToString();
                            _PIR.B9C_2_3 = dr["B9C_2_3"].ToString();
                            _PIR.B9C_2_4 = dr["B9C_2_4"].ToString();
                            _PIR.B9C_3_1 = dr["B9C_3_1"].ToString();
                            _PIR.B9C_3_2 = dr["B9C_3_2"].ToString();
                            _PIR.B9C_3_3 = dr["B9C_3_3"].ToString();
                            _PIR.B9C_3_4 = dr["B9C_3_4"].ToString();
                            _PIR.B9C_4_1 = dr["B9C_4_1"].ToString();
                            _PIR.B9C_4_2 = dr["B9C_4_2"].ToString();
                            _PIR.B9C_4_3 = dr["B9C_4_3"].ToString();
                            _PIR.B9C_4_4 = dr["B9C_4_4"].ToString();
                            _PIR.B9C_5_1 = dr["B9C_5_1"].ToString();
                            _PIR.B9C_5_2 = dr["B9C_5_2"].ToString();
                            _PIR.B9C_5_3 = dr["B9C_5_3"].ToString();
                            _PIR.B9C_5_4 = dr["B9C_5_4"].ToString();
                            _PIR.B9C_6_1 = dr["B9C_6_1"].ToString();
                            _PIR.B9C_6_2 = dr["B9C_6_2"].ToString();
                            _PIR.B9C_6_3 = dr["B9C_6_3"].ToString();
                            _PIR.B9C_6_4 = dr["B9C_6_4"].ToString();
                            _PIR.B9C_7_1 = dr["B9C_7_1"].ToString();
                            _PIR.B9C_7_2 = dr["B9C_7_2"].ToString();
                            _PIR.B9C_7_3 = dr["B9C_7_3"].ToString();
                            _PIR.B9C_7_4 = dr["B9C_7_4"].ToString();
                            _PIR.B9D_1_1 = dr["B9D_1_1"].ToString();
                            _PIR.B9D_1_2 = dr["B9D_1_2"].ToString();
                            _PIR.B9D_1_3 = dr["B9D_1_3"].ToString();
                            _PIR.B9D_1_4 = dr["B9D_1_4"].ToString();
                            _PIR.B9D_2_1 = dr["B9D_2_1"].ToString();
                            _PIR.B9D_2_2 = dr["B9D_2_2"].ToString();
                            _PIR.B9D_2_3 = dr["B9D_2_3"].ToString();
                            _PIR.B9D_2_4 = dr["B9D_2_4"].ToString();
                            _PIR.B9D_3_1 = dr["B9D_3_1"].ToString();
                            _PIR.B9D_3_2 = dr["B9D_3_2"].ToString();
                            _PIR.B9D_3_3 = dr["B9D_3_3"].ToString();
                            _PIR.B9D_3_4 = dr["B9D_3_4"].ToString();
                            _PIR.B9D_4_1 = dr["B9D_4_1"].ToString();
                            _PIR.B9D_4_2 = dr["B9D_4_2"].ToString();
                            _PIR.B9D_4_3 = dr["B9D_4_3"].ToString();
                            _PIR.B9D_4_4 = dr["B9D_4_4"].ToString();
                            _PIR.B9D_5_1 = dr["B9D_5_1"].ToString();
                            _PIR.B9D_5_2 = dr["B9D_5_2"].ToString();
                            _PIR.B9D_5_3 = dr["B9D_5_3"].ToString();
                            _PIR.B9D_5_4 = dr["B9D_5_4"].ToString();
                            _PIR.B9E_1 = dr["B9E_1"].ToString();
                            _PIR.B9E_2 = dr["B9E_2"].ToString();
                            _PIR.B9E_3 = dr["B9E_3"].ToString();
                            _PIR.B9E_4 = dr["B9E_4"].ToString();
                            _PIR.B9E_1_1 = dr["B9E_1_1"].ToString();
                            _PIR.B9E_1_2 = dr["B9E_1_2"].ToString();
                            _PIR.B9E_1_3 = dr["B9E_1_3"].ToString();
	                        _PIR.B9E_1_4 = dr["B8E_3_2"].ToString();
                            _PIR.B9E_2_1 = dr["B9E_1_4"].ToString();
                            _PIR.B9E_2_2 = dr["B9E_2_2"].ToString();
                            _PIR.B9E_2_3 = dr["B9E_2_3"].ToString();
                            _PIR.B9E_2_4 = dr["B9E_2_4"].ToString();
                            _PIR.B9E_3_1 = dr["B9E_3_1"].ToString();
                            _PIR.B9E_3_2 = dr["B9E_3_2"].ToString();
                            _PIR.B9E_3_3 = dr["B9E_3_3"].ToString();
                            _PIR.B9E_3_4 = dr["B9E_3_4"].ToString();
                            _PIR.B9E_4_1 = dr["B9E_4_1"].ToString();
                            _PIR.B9E_4_2 = dr["B9E_4_2"].ToString();
                            _PIR.B9E_4_3 = dr["B9E_4_3"].ToString();
                            _PIR.B9E_4_4 = dr["B9E_4_4"].ToString();


                    }

                }
            }


        }
        public PIRModel GetPIR(string UserID,string AgencyID,string Program, string Refresh)
        {
            PIRModel _PIR = new PIRModel();
            
            if (Program == null) Program = "HS";
            _PIR.program = Program;
            if (Refresh == "1")
            {
                
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
                command.Parameters.Add(new SqlParameter("@UserID", UserID));
                command.Parameters.Add(new SqlParameter("@ProgramType", Program));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 500;
                command.CommandText = "SP_GENERATE_PIR";

                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
            }
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@UserID",UserID));
           command.Parameters.Add(new SqlParameter("@ProgramType", Program));

            command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getPIR";

            DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                GetPIRSummary(_dataset, _PIR);
                return _PIR;
          
        }
        public void RefreshPIR(string UserID, string AgencyID, string Program)
        {
           // PIRModel _PIR = new PIRModel();

            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@UserID", UserID));
            command.Parameters.Add(new SqlParameter("@ProgramType", Program));
            command.CommandTimeout = 500;
            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_GENERATE_PIR";

            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
           // GetPIR(UserID, AgencyID, Program);
           // return _PIR;

        }
        public void GetPIRSummaryDetails(DataSet _dataset, PIRModel _PIR, string PIRQuestion)
        {
            if (_dataset != null)
            {
                if (_dataset.Tables[0].Rows.Count > 0)
                {
                    List<PIRModel> PIRList = new List<PIRModel>();
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {


                       PIRList.Add(new PIRModel
                        {
                            clientName = Convert.ToString(dr["ChildName"]),
                            DOB = Convert.ToString(dr["DOB"]),
                            center = Convert.ToString(dr["CenterID"]),
                            classroom = Convert.ToString(dr["ClassroomID"]),
                            piratenrollment = Convert.ToString(dr["pirquest"]),
                            pirafterenrollment = Convert.ToString(dr["pirquest2"]),
                            clientStatus = Convert.ToString(dr["Status"])
                           
                        });
                       _PIR.piratenrollmentDesc = Convert.ToString(dr["Description"]);
                       _PIR.pirafterenrollmentDesc = Convert.ToString(dr["Description2"]);
                    }
                    _PIR.PIRlst = PIRList;
                    _PIR.pirQuestion = PIRQuestion;
                   
                    }

                }

            }



        public PIRModel GetPIRDetails(string UserID, string AgencyID, string pirquestion, string Program)
        {

            PIRModel _PIR = new PIRModel();

            _PIR.program = Program;
          
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ProgramType", Program));
            command.Parameters.Add(new SqlParameter("@Quest_ID", pirquestion));
            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PIR_Reporting";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            GetPIRSummaryDetails(_dataset, _PIR, pirquestion);
            return _PIR;
            
        }
        
		  /// <summary>
        /// method to get the Staff under the PIR Access Roles based on Agency.
        /// </summary>
        /// <returns></returns>
        public PIRAccessStaffs GetPIRUsers(PIRAccessStaffs pirStaffs)
        {
            PIRAccessStaffs pIRAccessStaffs = new PIRAccessStaffs();
            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@Take", pirStaffs.Take));
                    command.Parameters.Add(new SqlParameter("@Skip", pirStaffs.Skip));
                    command.Parameters.Add(new SqlParameter("@RequestedPage", pirStaffs.RequestedPage));
                    command.Parameters.Add(new SqlParameter("@SearchText", pirStaffs.SearchText));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetPIRAccessStaffs";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        pIRAccessStaffs.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        pIRAccessStaffs.PIRStaffsList = _dataset.Tables[0].AsEnumerable().OrderBy(x => x.Field<string>("StaffName"))
                                  .Select(x => new PIRStaffs
                                  {

                                      StaffName = x.Field<string>("StaffName"),
                                      RoleId = x.Field<Guid>("RoleId"),
                                      UserId = x.Field<Guid>("UserId"),
                                      RoleName = x.Field<string>("RoleName"),
                                      IsShowSectionB = x.Field<bool>("IsShowSectionB")

                                  }).ToList();
                    }
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return pIRAccessStaffs;
        }

        /// <summary>
        /// method to insert the Staff's who are access to Section B in PIR
        /// </summary>
        /// <param name="pirStaffs"></param>
        /// <returns></returns>
        public bool InsertPIRSectionBAccessStaffs(PIRAccessStaffs pirStaffs)
        {
            bool isRowsAffected = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
               
                DataTable accessStaffdt = new DataTable();

                accessStaffdt.Columns.AddRange(new DataColumn[6] {
                    new DataColumn("PIRStaffIndexId",typeof(long)),
                    new DataColumn("AgencyId",typeof(Guid)),
                    new DataColumn("UserId",typeof(Guid)),
                    new DataColumn("RoleId",typeof(Guid)),
                    new DataColumn("IsShowSectionB",typeof(bool)),
                    new DataColumn("Status",typeof(bool))
                });

                if (pirStaffs.PIRStaffsList.Count() > 0)
                {

                    foreach (var item in pirStaffs.PIRStaffsList)
                        accessStaffdt.Rows.Add(
                            0,
                          pirStaffs.StaffDetails.AgencyId,
                          item.UserId,
                          item.RoleId,
                          item.IsShowSectionB,
                          (item.IsShowSectionB) ? true : false
                        );

                }

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", pirStaffs.StaffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", pirStaffs.StaffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", pirStaffs.StaffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@PIRAccessStaffsType", accessStaffdt));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_InsertSectionBPIRStaffs";
                    Connection.Open();
                    isRowsAffected = (command.ExecuteNonQuery() > 0);
                    Connection.Close();
                }
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
            }
            return isRowsAffected;
        }

        public PIRModel ExportData(string question, string AgencyID, string programtype)
        {
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ProgramType", programtype));
            command.Parameters.Add(new SqlParameter("@Quest_ID", question));
            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[PIR_Reporting]";
            SqlDataAdapter DataAdapter1 = null;
            DataAdapter1 = new SqlDataAdapter(command);
            DataSet _dataset1 = null;
            _dataset1 = new DataSet();
            DataAdapter1.Fill(_dataset1);
            string FileName = "attachment; filename = PIRExportReport.xlsx";
            

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(_dataset1);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Buffer = true;
                // System.Web.HttpContext.Current.Response.Charset = "";
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", FileName);

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.End();
                }
            }
            PIRModel _PIRM = new PIRModel();
            return _PIRM;

        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
       
    }
}
