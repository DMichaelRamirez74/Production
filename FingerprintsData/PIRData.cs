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

                        
                                            /*   
                                             
                                              **/

                    }

                }
            }


        }
        public PIRModel GetPIR(string UserID,string AgencyID,string Program)
        {
            PIRModel _PIR = new PIRModel();
            
            if (Program == null) Program = "HS";
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
                            pirafterenrollment = Convert.ToString(dr["pirquest2"])
                           
                        });
                       _PIR.piratenrollmentDesc = Convert.ToString(dr["Description"]);
                       _PIR.pirafterenrollmentDesc = Convert.ToString(dr["Description2"]);
                    }
                    _PIR.PIRlst = PIRList;
                    _PIR.pirQuestion = PIRQuestion;
                   
                    }

                }

            }



        public PIRModel GetPIRDetails(string UserID, string AgencyID, string pirquestion)
        {

            PIRModel _PIR = new PIRModel();
           
          
          
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ProgramType", "HS"));
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
        
       
    }
}
