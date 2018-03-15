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

namespace FingerprintsData
{
    public class SelectPointsData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        //SqlDataReader dataReader = null;
        SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable agencydataTable = null;
        DataSet _dataset = null;
        public SelectPoints GetData_AllDropdown(string agencyId)
        {
            SelectPoints _prog = new SelectPoints();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Parameters.Add(new SqlParameter("@id", agencyId));
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_Prog_Dropdowndata";//Sp_Sel_RefProg_Dropdowndata  Sp_Sel_Prog_Dropdowndata
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        List<SelectPoints.ReferenceProgInfo> _proglist = new List<SelectPoints.ReferenceProgInfo>();
                        //_staff.myList
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SelectPoints.ReferenceProgInfo obj = new SelectPoints.ReferenceProgInfo();
                            obj.Id = dr["ProgramTypeID"].ToString();//ReferenceId  ProgramTypeID
                            obj.Name = dr["ProgramType"].ToString();//ProgramType //Name
                            _proglist.Add(obj);
                        }

                        _prog.refList = _proglist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return _prog;
        }

        public string AddEditSelectPoint(SelectPoints obj, int mode, Guid ID, List<SelectPoints.CustomQuestion> CustomQues)
        {
            string result = string.Empty;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                tranSaction = Connection.BeginTransaction();
                command.Transaction = tranSaction;
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@IsLocked", obj.IsLocked));
                command.Parameters.Add(new SqlParameter("@SPID", obj.SPID));
                command.Parameters.Add(new SqlParameter("@ReferenceProg", obj.ReferenceProg));
                command.Parameters.Add(new SqlParameter("@SingleParent", obj.SingleParent));
                command.Parameters.Add(new SqlParameter("@TwoParent", obj.TwoParent));
                command.Parameters.Add(new SqlParameter("@English", obj.English));
                command.Parameters.Add(new SqlParameter("@Other", obj.Other));
                command.Parameters.Add(new SqlParameter("@HomelessYes", obj.HomelessYes));
                command.Parameters.Add(new SqlParameter("@HomelessNo", obj.HomelessNo));
                command.Parameters.Add(new SqlParameter("@TANF", obj.TANF));
                command.Parameters.Add(new SqlParameter("@SSI", obj.SSI));
                command.Parameters.Add(new SqlParameter("@SNAP", obj.SNAP));
                command.Parameters.Add(new SqlParameter("@WIC", obj.WIC));
                command.Parameters.Add(new SqlParameter("@None", obj.None));
                command.Parameters.Add(new SqlParameter("@CurrentlyWorkYes", obj.CurrentlyWorkYes));
                command.Parameters.Add(new SqlParameter("@CurrentlyWorkNo", obj.CurrentlyWorkNo));
                command.Parameters.Add(new SqlParameter("@JobTrainingno", obj.JobTrainingno));
                command.Parameters.Add(new SqlParameter("@JobTrainingyes", obj.JobTrainingyes));
                command.Parameters.Add(new SqlParameter("@Teenager", obj.Teenager));
                command.Parameters.Add(new SqlParameter("@Age20", obj.Age20));
                command.Parameters.Add(new SqlParameter("@Age30over", obj.Age30over));
                command.Parameters.Add(new SqlParameter("@MilitaryStatusActive", obj.MilitaryStatusActive));
                command.Parameters.Add(new SqlParameter("@MilitaryStatusNone", obj.MilitaryStatusNone));
                command.Parameters.Add(new SqlParameter("@MilitaryStatusVeteran", obj.MilitaryStatusVeteran));
                command.Parameters.Add(new SqlParameter("@G2Age20", obj.G2Age20));
                command.Parameters.Add(new SqlParameter("@G2Teenager", obj.G2Teenager));
                command.Parameters.Add(new SqlParameter("@G2Age30over", obj.G2Age30over));
                command.Parameters.Add(new SqlParameter("@G2MilitaryStatusActive", obj.G2MilitaryStatusActive));
                command.Parameters.Add(new SqlParameter("@G2MilitaryStatusNone", obj.G2MilitaryStatusNone));
                command.Parameters.Add(new SqlParameter("@G2MilitaryStatusVeteran", obj.G2MilitaryStatusVeteran));
                command.Parameters.Add(new SqlParameter("@G2CurrentlyWorkNo", obj.G2CurrentlyWorkNo));
                command.Parameters.Add(new SqlParameter("@G2CurrentlyWorkYes", obj.G2CurrentlyWorkYes));
                command.Parameters.Add(new SqlParameter("@G2JobTrainingno", obj.G2JobTrainingno));
                command.Parameters.Add(new SqlParameter("@G2JobTrainingyes", obj.G2JobTrainingyes));


                command.Parameters.Add(new SqlParameter("@MedicalHomeNo", obj.MedicalHomeNo));
                command.Parameters.Add(new SqlParameter("@MedicalHomeYes", obj.MedicalHomeYes));
                command.Parameters.Add(new SqlParameter("@DentalHomeNo", obj.DentalHomeNo));
                command.Parameters.Add(new SqlParameter("@DentalHomeYes", obj.DentalHomeYes));
                command.Parameters.Add(new SqlParameter("@InsuranceYes", obj.InsuranceYes));
                command.Parameters.Add(new SqlParameter("@InsuranceNo", obj.InsuranceNo));
                //Commented and breakit into 2 properties
                // command.Parameters.Add(new SqlParameter("@SuspecteddocNo", obj.SuspecteddocumentofdisabiltyNo));   SuspecteddisabiltyYes
                // command.Parameters.Add(new SqlParameter("@SuspecteddocYes", obj.SuspecteddocumentofdisabiltyYes)); SuspecteddisabiltyNo'
                command.Parameters.Add(new SqlParameter("@SuspecteddocNo", obj.SuspecteddisabiltyNo));
                command.Parameters.Add(new SqlParameter("@SuspecteddocYes", obj.SuspecteddisabiltyYes));
                command.Parameters.Add(new SqlParameter("@DisabilitydocNo", obj.DocumentofdisabiltyNo));
                command.Parameters.Add(new SqlParameter("@DisabilitydocYes", obj.DocumentofdisabiltyYes));



                command.Parameters.Add(new SqlParameter("@ChildWlfareNo", obj.ChildWlfareNo));
                command.Parameters.Add(new SqlParameter("@ChildWlfareYes", obj.ChildWlfareYes));
                command.Parameters.Add(new SqlParameter("@FosterChildNo", obj.FosterChildNo));
                command.Parameters.Add(new SqlParameter("@FosterChildYes", obj.FosterChildYes));
                command.Parameters.Add(new SqlParameter("@DualCustNo", obj.DualCustNo));
                command.Parameters.Add(new SqlParameter("@DualCustYes", obj.DualCustYes));
                command.Parameters.Add(new SqlParameter("@Age1yr", obj.Age1yr));
                command.Parameters.Add(new SqlParameter("@Age2yr", obj.Age2yr));
                command.Parameters.Add(new SqlParameter("@Age3Months", obj.Age3Months));
                command.Parameters.Add(new SqlParameter("@Age3yr", obj.Age3yr));
                command.Parameters.Add(new SqlParameter("@Age4yr", obj.Age4yr));
                command.Parameters.Add(new SqlParameter("@Age5yr", obj.Age5yr));
                command.Parameters.Add(new SqlParameter("@Age6Months", obj.Age6Months));
                command.Parameters.Add(new SqlParameter("@Age6yr", obj.Age6yr));
                command.Parameters.Add(new SqlParameter("@Agegreater10weeks", obj.Agegreater10weeks));
                command.Parameters.Add(new SqlParameter("@Ageless10weeks", obj.Ageless10weeks));
                //Poverty Points
                command.Parameters.Add(new SqlParameter("@poverty0to25", obj.poverty0to25));
                command.Parameters.Add(new SqlParameter("@poverty26to50", obj.poverty26to50));
                command.Parameters.Add(new SqlParameter("@poverty51to75", obj.poverty51to75));
                command.Parameters.Add(new SqlParameter("@poverty76to100", obj.poverty76to100));
                command.Parameters.Add(new SqlParameter("@poverty100to130", obj.poverty100to130));
                command.Parameters.Add(new SqlParameter("@povertygreater130", obj.povertygreater130));
                //End
                command.Parameters.Add(new SqlParameter("@PMMedicalHomeNo", obj.PMMedicalHomeNo));
                command.Parameters.Add(new SqlParameter("@PMMedicalHomeYes", obj.PMMedicalHomeYes));
                command.Parameters.Add(new SqlParameter("@PMInsuranceYes", obj.PMInsuranceYes));
                command.Parameters.Add(new SqlParameter("@PMInsuranceNo", obj.PMInsuranceNo));
                command.Parameters.Add(new SqlParameter("@Trimester1", obj.Trimester1));
                command.Parameters.Add(new SqlParameter("@Trimester2", obj.Trimester2));
                command.Parameters.Add(new SqlParameter("@Trimester3", obj.Trimester3));
                command.Parameters.Add(new SqlParameter("@agencyid", obj.AgencyID));
                command.Parameters.Add(new SqlParameter("@CreatedBy", ID));

                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;

                if (CustomQues != null && CustomQues.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[4] { 
                    new DataColumn("Question", typeof(string)),
                    new DataColumn("QuesYes",typeof(string)), 
                    new DataColumn("QuesNo",typeof(string)), 
                    new DataColumn("ID",typeof(string)), 
                    });
                    foreach (SelectPoints.CustomQuestion ques in CustomQues)
                    {
                        if (ques.Question != null)
                        {
                            dt.Rows.Add(ques.Question, ques.QuesYes, ques.QuesNo, ques.CQID);
                        }
                    }

                    command.Parameters.Add(new SqlParameter("@tblques", dt));
                }
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Add_SelectPointDetails";
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();

                tranSaction.Commit();

            }
            catch (Exception ex)
            {
                if (tranSaction != null)
                    tranSaction.Rollback();
                clsError.WriteException(ex);
                result = "";

            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;

        }


        public string DeleteQues(string Questionid, string Agencyid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deletequestion";
                command.Parameters.Add(new SqlParameter("@quesid", Questionid));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return "";
            }
            finally
            {
                command.Dispose();

            }
        }


        public SelectPoints EditSelectPointInfo(string ProgType, string AgencyId)
        {
            command.Connection = Connection;
            command.Parameters.Add(new SqlParameter("@ProgType", ProgType));
            command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "Sp_selectpoint";
            DataAdapter = new SqlDataAdapter(command);
            agencydataTable = new DataTable();
            DataAdapter.Fill(agencydataTable);
            SelectPoints obj = new SelectPoints();
            try
            {
                if (agencydataTable.Rows.Count > 0)
                {
                    //for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    //{
                    //SelectPoints obj = new SelectPoints();

                    obj.SingleParent = Convert.ToInt32(agencydataTable.Rows[0]["SingleParent"]);
                    obj.SPID = Convert.ToInt32(agencydataTable.Rows[0]["SPID"]);
                    obj.TwoParent = Convert.ToInt32(agencydataTable.Rows[0]["Twoparent"]);
                    obj.English = Convert.ToInt32(agencydataTable.Rows[0]["PrimaryLangEnglish"]);
                    obj.Other = Convert.ToInt32(agencydataTable.Rows[0]["PrimaryLangOther"]);
                    obj.HomelessYes = Convert.ToInt32(agencydataTable.Rows[0]["HomelessYes"]);
                    obj.HomelessNo = Convert.ToInt32(agencydataTable.Rows[0]["HomelessNo"]);
                    obj.TANF = Convert.ToInt32(agencydataTable.Rows[0]["TANF"]);
                    obj.SNAP = Convert.ToInt32(agencydataTable.Rows[0]["SNAP"]);
                    obj.SSI = Convert.ToInt32(agencydataTable.Rows[0]["SSI"]);
                    obj.WIC = Convert.ToInt32(agencydataTable.Rows[0]["WIC"]);
                    obj.None = Convert.ToInt32(agencydataTable.Rows[0]["NONE"]);
                    obj.CurrentlyWorkNo = Convert.ToInt32(agencydataTable.Rows[0]["G1CurrentlyWorkNo"]);
                    obj.CurrentlyWorkYes = Convert.ToInt32(agencydataTable.Rows[0]["G1CurrentlyWorkYes"]);
                    obj.JobTrainingno = Convert.ToInt32(agencydataTable.Rows[0]["G1JobTrainingno"]);
                    obj.JobTrainingyes = Convert.ToInt32(agencydataTable.Rows[0]["G1JobTrainingyes"]);
                    obj.Teenager = Convert.ToInt32(agencydataTable.Rows[0]["G1Teenager"]);
                    obj.Age20 = Convert.ToInt32(agencydataTable.Rows[0]["G1Age20"]);
                    obj.Age30over = Convert.ToInt32(agencydataTable.Rows[0]["G1Age30over"]);
                    obj.MilitaryStatusActive = Convert.ToInt32(agencydataTable.Rows[0]["G1MilitaryStatusActive"]);
                    obj.MilitaryStatusNone = Convert.ToInt32(agencydataTable.Rows[0]["G1MilitaryStatusNone"]);
                    obj.MilitaryStatusVeteran = Convert.ToInt32(agencydataTable.Rows[0]["G1MilitaryStatusVeteran"]);
                    obj.G2Age20 = Convert.ToInt32(agencydataTable.Rows[0]["G2Age20"]);
                    obj.G2Age30over = Convert.ToInt32(agencydataTable.Rows[0]["G2Age30over"]);
                    obj.G2Teenager = Convert.ToInt32(agencydataTable.Rows[0]["G2Teenager"]);
                    obj.G2MilitaryStatusActive = Convert.ToInt32(agencydataTable.Rows[0]["G2MilitaryStatusActive"]);
                    obj.G2MilitaryStatusNone = Convert.ToInt32(agencydataTable.Rows[0]["G2MilitaryStatusNone"]);
                    obj.G2MilitaryStatusVeteran = Convert.ToInt32(agencydataTable.Rows[0]["G2MilitaryStatusVeteran"]);
                    obj.G2JobTrainingyes = Convert.ToInt32(agencydataTable.Rows[0]["G2JobTrainingno"]);
                    obj.G2JobTrainingno = Convert.ToInt32(agencydataTable.Rows[0]["G2JobTrainingyes"]);
                    obj.G2CurrentlyWorkNo = Convert.ToInt32(agencydataTable.Rows[0]["G2CurrentlyWorkNo"]);
                    obj.G2CurrentlyWorkYes = Convert.ToInt32(agencydataTable.Rows[0]["G2CurrentlyWorkYes"]);
                    obj.MedicalHomeNo = Convert.ToInt32(agencydataTable.Rows[0]["MedicalHomeNo"]);
                    obj.MedicalHomeYes = Convert.ToInt32(agencydataTable.Rows[0]["MedicalHomeYes"]);
                    obj.DentalHomeNo = Convert.ToInt32(agencydataTable.Rows[0]["DentalHomeNo"]);
                    obj.DentalHomeYes = Convert.ToInt32(agencydataTable.Rows[0]["DentalHomeYes"]);
                    obj.InsuranceNo = Convert.ToInt32(agencydataTable.Rows[0]["InsuranceNo"]);
                    obj.InsuranceYes = Convert.ToInt32(agencydataTable.Rows[0]["InsuranceYes"]);
                    //Commented to break it in 2 new property
                    //obj.SuspecteddocumentofdisabiltyNo=Convert.ToInt32(agencydataTable.Rows[0]["SuspecteddocsNo"]);
                    // obj.SuspecteddocumentofdisabiltyYes=Convert.ToInt32(agencydataTable.Rows[0]["SuspecteddocsYes"]);
                    obj.SuspecteddisabiltyNo = agencydataTable.Rows[0]["SuspecteddocsNo"] != DBNull.Value ? Convert.ToInt32(agencydataTable.Rows[0]["SuspecteddocsNo"]) : 0;
                    obj.SuspecteddisabiltyYes = agencydataTable.Rows[0]["SuspecteddocsYes"] != DBNull.Value ? Convert.ToInt32(agencydataTable.Rows[0]["SuspecteddocsYes"]) : 0;
                    obj.DocumentofdisabiltyNo = agencydataTable.Rows[0]["DisabilitydocNo"] != DBNull.Value ? Convert.ToInt32(agencydataTable.Rows[0]["DisabilitydocNo"]) : 0;
                    obj.DocumentofdisabiltyYes = agencydataTable.Rows[0]["DisabilitydocYes"] != DBNull.Value ? Convert.ToInt32(agencydataTable.Rows[0]["DisabilitydocYes"]) : 0;
                    //End Comment
                    obj.ChildWlfareYes = Convert.ToInt32(agencydataTable.Rows[0]["ChildWlfareYes"]);
                    obj.ChildWlfareNo = Convert.ToInt32(agencydataTable.Rows[0]["ChildWlfareNo"]);
                    obj.FosterChildNo = Convert.ToInt32(agencydataTable.Rows[0]["FosterChildYes"]);
                    obj.FosterChildYes = Convert.ToInt32(agencydataTable.Rows[0]["FosterChildNo"]);
                    obj.DualCustNo = Convert.ToInt32(agencydataTable.Rows[0]["DualCustYes"]);
                    obj.DualCustNo = Convert.ToInt32(agencydataTable.Rows[0]["DualCustNo"]);
                    obj.Age3Months = Convert.ToInt32(agencydataTable.Rows[0]["ChildAge3Months"]);//change
                    obj.Age1yr = Convert.ToInt32(agencydataTable.Rows[0]["ChildAge1yr"]);
                    obj.Age2yr = Convert.ToInt32(agencydataTable.Rows[0]["ChildAge2yr"]);
                    obj.Age3yr = Convert.ToInt32(agencydataTable.Rows[0]["ChildAge3yr"]);
                    obj.Age4yr = Convert.ToInt32(agencydataTable.Rows[0]["ChildAge4yr"]);
                    obj.Age5yr = Convert.ToInt32(agencydataTable.Rows[0]["ChildAge5yr"]);
                    obj.Age6Months = Convert.ToInt32(agencydataTable.Rows[0]["ChildAge6months"]);
                    obj.Age6yr = Convert.ToInt32(agencydataTable.Rows[0]["ChildAge6yr"]);
                    obj.Agegreater10weeks = Convert.ToInt32(agencydataTable.Rows[0]["ChildAgeGreater10weeks"]);
                    obj.Ageless10weeks = Convert.ToInt32(agencydataTable.Rows[0]["ChildAgeLess10weeks"]);
                    obj.PMMedicalHomeNo = Convert.ToInt32(agencydataTable.Rows[0]["PMMedicalHomeNo"]);
                    obj.PMMedicalHomeYes = Convert.ToInt32(agencydataTable.Rows[0]["PMMedicalHomeYes"]);
                    obj.PMInsuranceNo = Convert.ToInt32(agencydataTable.Rows[0]["PMInsuranceNo"]);
                    obj.PMInsuranceYes = Convert.ToInt32(agencydataTable.Rows[0]["PMInsuranceYes"]);
                    obj.Trimester1 = Convert.ToInt32(agencydataTable.Rows[0]["Trimester1"]);
                    obj.Trimester2 = Convert.ToInt32(agencydataTable.Rows[0]["Trimester2"]);
                    obj.Trimester3 = Convert.ToInt32(agencydataTable.Rows[0]["Trimester3"]);
                    if (!DBNull.Value.Equals(agencydataTable.Rows[0]["IsLocked"]))
                    {
                        obj.IsLocked = Convert.ToBoolean(agencydataTable.Rows[0]["IsLocked"]);//change
                    }
                    else
                    {
                        obj.IsLocked = false;
                    }
                    if (!DBNull.Value.Equals(agencydataTable.Rows[0]["Poverty0to25"]))
                    {
                        obj.poverty0to25 = Convert.ToInt32(agencydataTable.Rows[0]["Poverty0to25"]);//change
                    }
                    else
                    {
                        obj.poverty0to25 = 0;
                    }
                    if (!DBNull.Value.Equals(agencydataTable.Rows[0]["Poverty26to50"]))
                    {
                        obj.poverty26to50 = Convert.ToInt32(agencydataTable.Rows[0]["Poverty26to50"]);//change
                    }
                    else
                    {
                        obj.poverty26to50 = 0;
                    }
                    if (!DBNull.Value.Equals(agencydataTable.Rows[0]["Poverty51to75"]))
                    {
                        obj.poverty51to75 = Convert.ToInt32(agencydataTable.Rows[0]["Poverty51to75"]);//change
                    }
                    else
                    {
                        obj.poverty51to75 = 0;
                    }
                    if (!DBNull.Value.Equals(agencydataTable.Rows[0]["Poverty76to100"]))
                    {
                        obj.poverty76to100 = Convert.ToInt32(agencydataTable.Rows[0]["Poverty76to100"]);//change
                    }
                    else
                    {
                        obj.poverty76to100 = 0;
                    }
                    if (!DBNull.Value.Equals(agencydataTable.Rows[0]["Poverty100to130"]))
                    {
                        obj.poverty100to130 = Convert.ToInt32(agencydataTable.Rows[0]["Poverty100to130"]);//change
                    }
                    else
                    {
                        obj.poverty100to130 = 0;
                    }
                    if (!DBNull.Value.Equals(agencydataTable.Rows[0]["Povertygreater130"]))
                    {
                        obj.povertygreater130 = Convert.ToInt32(agencydataTable.Rows[0]["Povertygreater130"]);//change
                    }
                    else
                    {
                        obj.povertygreater130 = 0;
                    }
                    obj.RefProgName = Convert.ToString(agencydataTable.Rows[0]["Name"]);


                    //   obj.ReferenceProg=






                    //return obj;
                    //staffList.Add(staff);

                    //return obj;
                }
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
                return obj;
            }
            catch (Exception ex)
            {
                // totalrecord = string.Empty;
                clsError.WriteException(ex);
                return obj;
            }
            finally
            {
                if (DataAdapter != null)
                    DataAdapter.Dispose();
                if (command != null)
                    command.Dispose();
                if (agencydataTable != null)
                    agencydataTable.Dispose();
            }
        }



        public List<SelectPoints.CustomQuestion> QuesDetails(string ProgramId, string Agencyid)//string CQID,
        {
            List<SelectPoints.CustomQuestion> _customques = new List<SelectPoints.CustomQuestion>();
            try
            {
                //command.Parameters.Add(new SqlParameter("@CQID", CQID));
                command.Parameters.Add(new SqlParameter("@ProgramId", ProgramId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_queslist";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in agencydataTable.Rows)
                    {
                        SelectPoints.CustomQuestion _quesadd = new SelectPoints.CustomQuestion();
                        _quesadd.CQID = Convert.ToInt32(row["Id"]);
                        _quesadd.Question = row["Question"].ToString();
                        _quesadd.QuesYes = row["QuesYes"].ToString();
                        _quesadd.QuesNo = row["QuesNo"].ToString();

                        _customques.Add(_quesadd);
                    }
                }
                return _customques;
            }
            catch (Exception ex)
            {
                //  totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _customques;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }


        public SelectPoints GetRefProglistInfo(string ProgType, string AgencyId)
        {
            command.Connection = Connection;
            command.Parameters.Add(new SqlParameter("@ProgType", ProgType));
            command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "Sp_selectrefprog";
            DataAdapter = new SqlDataAdapter(command);
            agencydataTable = new DataTable();
            DataAdapter.Fill(agencydataTable);
            SelectPoints obj = new SelectPoints();
            try
            {
                if (agencydataTable.Rows.Count > 0)
                {



                    obj.RefProgName = Convert.ToString(agencydataTable.Rows[0]["Name"]);


                    //   obj.ReferenceProg=






                    //return obj;
                    //staffList.Add(staff);

                    //return obj;
                }
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
                return obj;
            }
            catch (Exception ex)
            {
                // totalrecord = string.Empty;
                clsError.WriteException(ex);
                return obj;
            }
            finally
            {
                if (DataAdapter != null)
                    DataAdapter.Dispose();
                if (command != null)
                    command.Dispose();
                if (agencydataTable != null)
                    agencydataTable.Dispose();
            }
        }
    }
}
