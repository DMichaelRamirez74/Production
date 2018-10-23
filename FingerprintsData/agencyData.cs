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
using System.Globalization;
using System.Web;
using System.IO;
namespace FingerprintsData
{
    public class agencyData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataReader dataReader = null;
        SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable agencydataTable = null;
        DataSet _dataset = null;
        public string agencycode()
        {
            string agencycode = string.Empty;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "select max(agencycode) as  agencycode from agencyinfo";
                dataReader = command.ExecuteReader();
                if (dataReader.Read() && dataReader.HasRows)
                {
                    if (!dataReader.IsDBNull(0))
                        agencycode = (Convert.ToInt32(dataReader.GetValue(0)) + 1).ToString();
                    else
                        agencycode = "1001";

                }
                else
                {
                    agencycode = "1001";

                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return agencycode;
        }
        public string addeditAgency(Agency agencyDetails, int mode, string randomPassword, Guid userId, out string AgncyCode, List<Agency.FundSource> FundSource)//, List<Agency.FundSource.ProgramType> Prog
        {
            AgncyCode = "";
            try
            {
                SqlCommand commandAK = new SqlCommand();
                string agencyCode = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();

                commandAK.Connection = Connection;
                if (mode == 0)
                {
                    commandAK.CommandType = CommandType.Text;
                    commandAK.CommandText = "select max(agencycode) as  agencycode  from agencyinfo";
                    dataReader = commandAK.ExecuteReader();
                    if (dataReader.Read() && dataReader.HasRows)
                    {
                        if (!dataReader.IsDBNull(0))
                            agencyCode = (Convert.ToInt32(dataReader.GetValue(0)) + 1).ToString();
                        else
                            agencyCode = "1001";
                    }
                    else
                    {
                        agencyCode = "1001";
                    }
                    dataReader.Close();
                }
                else
                    agencyCode = agencyDetails.agencyCode;
                AgncyCode = agencyCode;

                tranSaction = Connection.BeginTransaction();
                command.Connection = Connection;
                command.Transaction = tranSaction;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_addeditagency_withfunds";//Sp_addeditagency_withfunds   Sp_addeditagency



                command.Parameters.Add(new SqlParameter("@agencyId", agencyDetails.agencyId));
                command.Parameters.Add(new SqlParameter("@Transport", agencyDetails.Transportation));//Changes
                command.Parameters.Add(new SqlParameter("@agencyCode", agencyCode));
                command.Parameters.Add(new SqlParameter("@agencyName", agencyDetails.agencyName));
                command.Parameters.Add(new SqlParameter("@primaryEmail", agencyDetails.primaryEmail.Trim()));
                command.Parameters.Add(new SqlParameter("@userName", DBNull.Value));
                // command.Parameters.Add(new SqlParameter("@programName", agencyDetails.programName));
                if (!string.IsNullOrEmpty(agencyDetails.programName))
                    command.Parameters.Add(new SqlParameter("@programName", agencyDetails.programName));
                else
                    command.Parameters.Add(new SqlParameter("@programName", string.Empty));
                command.Parameters.Add(new SqlParameter("@nameGranteeDelegate", agencyDetails.nameGranteeDelegate));
                if (!string.IsNullOrEmpty(agencyDetails.grantNo))
                    command.Parameters.Add(new SqlParameter("@grantNo", agencyDetails.grantNo));
                else
                    command.Parameters.Add(new SqlParameter("@grantNo", string.Empty));
                command.Parameters.Add(new SqlParameter("@firstname", agencyDetails.firstName));
                if (!string.IsNullOrEmpty(agencyDetails.LastName))
                    command.Parameters.Add(new SqlParameter("@lastname", agencyDetails.LastName));
                else
                    command.Parameters.Add(new SqlParameter("@lastname", string.Empty));
                command.Parameters.Add(new SqlParameter("@address1", agencyDetails.address1));
                if (!string.IsNullOrEmpty(agencyDetails.address2))
                    command.Parameters.Add(new SqlParameter("@address2", agencyDetails.address2));
                else
                    command.Parameters.Add(new SqlParameter("@address2", string.Empty));
                command.Parameters.Add(new SqlParameter("@city", agencyDetails.city));
                command.Parameters.Add(new SqlParameter("@State", agencyDetails.State));
                command.Parameters.Add(new SqlParameter("@County", agencyDetails.County));
                command.Parameters.Add(new SqlParameter("@zipCode", agencyDetails.zipCode));
                if (!string.IsNullOrEmpty(agencyDetails.phone1))
                    command.Parameters.Add(new SqlParameter("@phone1", agencyDetails.phone1));
                else
                    command.Parameters.Add(new SqlParameter("@phone1", string.Empty));
                if (!string.IsNullOrEmpty(agencyDetails.phone2))
                    command.Parameters.Add(new SqlParameter("@phone2", agencyDetails.phone2));
                else
                    command.Parameters.Add(new SqlParameter("@phone2", string.Empty));
                if (!string.IsNullOrEmpty(agencyDetails.fax))
                    command.Parameters.Add(new SqlParameter("@fax", agencyDetails.fax));
                else
                    command.Parameters.Add(new SqlParameter("@fax", string.Empty));
                if (agencyDetails.nationality == "0")
                    command.Parameters.Add(new SqlParameter("@nationality", DBNull.Value));
                else
                    command.Parameters.Add(new SqlParameter("@nationality", agencyDetails.nationality));
                command.Parameters.Add(new SqlParameter("@programstartDate", agencyDetails.programstartDate));
                command.Parameters.Add(new SqlParameter("@programendDate", agencyDetails.programendDate));
                command.Parameters.Add(new SqlParameter("@maximumcapacityforalldayClassrooms", agencyDetails.maximumcapacityforalldayClassrooms));
                command.Parameters.Add(new SqlParameter("@maximumcapacityforhalfdayClassrooms", agencyDetails.maximumcapacityforhalfdayClassrooms));
                command.Parameters.Add(new SqlParameter("@Access_Days", agencyDetails.AccessDays));
                command.Parameters.Add(new SqlParameter("@Start_Time", agencyDetails.AccessStart));
                command.Parameters.Add(new SqlParameter("@EndTime", agencyDetails.AccessStop));
                command.Parameters.Add(new SqlParameter("@AccStartDate", agencyDetails.AccessStartDate));
                command.Parameters.Add(new SqlParameter("@timezoneid", agencyDetails.TimeZoneID));
                command.Parameters.Add(new SqlParameter("@password", EncryptDecrypt.Encrypt(randomPassword)));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@createdBy", userId));
                command.Parameters.Add(new SqlParameter("@status", agencyDetails.status));
                command.Parameters.Add(new SqlParameter("@SpeedZip", agencyDetails.SpeedZip));//SpeedZip
                command.Parameters.Add(new SqlParameter("@ProgramStartTime", agencyDetails.ProgramStartTime));
                command.Parameters.Add(new SqlParameter("@ProgramEndTime", agencyDetails.ProgramEndTime));
                command.Parameters.Add(new SqlParameter("@FSWYearlyVisit", agencyDetails.FSWYearlyVisit));
                command.Parameters.Add(new SqlParameter("@Areabreakdown", agencyDetails.Areabreakdown));
                command.Parameters.Add(new SqlParameter("@DivisionBreakDown", agencyDetails.DivisionBreakDown));
                command.Parameters.Add(new SqlParameter("@Yakkr600Days", agencyDetails.Yakkr600));
                command.Parameters.Add(new SqlParameter("@AcceptanceProcess", agencyDetails.AcceptanceProcess));
                command.Parameters.Add(new SqlParameter("@AttendanceIssuePercentage", agencyDetails.Yakkr601));
                command.Parameters.Add(new SqlParameter("@PurchasedSlots", agencyDetails.PurchasedSlots));
                command.Parameters.Add(new SqlParameter("@AttendanceIssueStartDay", agencyDetails.AttendanceIssueStartDay));

                HttpPostedFileBase _file = agencyDetails.logo;
                string filename = null;
                string fileextension = null;
                byte[] filedata = null;
                if (_file != null && _file.FileName != "")
                {
                    filename = _file.FileName;
                    fileextension = Path.GetExtension(_file.FileName);
                    filedata = new BinaryReader(_file.InputStream).ReadBytes(_file.ContentLength);
                }
                command.Parameters.Add(new SqlParameter("@AgencyLogo", filedata));
                command.Parameters.Add(new SqlParameter("@Logoname", filename));
                command.Parameters.Add(new SqlParameter("@logofileExt", fileextension));

                if (!string.IsNullOrEmpty(agencyDetails.ActiveProgYear))
                    command.Parameters.Add(new SqlParameter("@ActiveProgYear", agencyDetails.ActiveProgYear));
                else
                    command.Parameters.Add(new SqlParameter("@ActiveProgYear", string.Empty));
                if (!string.IsNullOrEmpty(agencyDetails.DocsStorage))
                    command.Parameters.Add(new SqlParameter("@DocsStorage", agencyDetails.DocsStorage));
                else
                    command.Parameters.Add(new SqlParameter("@DocsStorage", string.Empty));

                //Fund and Program Types
                if (FundSource != null && FundSource.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[29] {
                    new DataColumn("Acronym ", typeof(string)),
                    new DataColumn("Description",typeof(string)),
                    new DataColumn("Amount",typeof(string)),
                    new DataColumn("Date",typeof(string)),
                    new DataColumn("Duration",typeof(string)),
                       new DataColumn("ServiceQty ",typeof(string)),
                          new DataColumn("FundingType",typeof(string)),
                             new DataColumn("ProgramYear",typeof(string)),
                             new DataColumn("GranteeNo",typeof(string)),
                             new DataColumn("Grantee",typeof(string)),
                                new DataColumn("Status",typeof(string)),
                                 new DataColumn("FundID",typeof(Int32)),
                             new DataColumn("OldFund",typeof(string)),
                             new DataColumn("FundQ1",typeof(string)),
                             new DataColumn("FundQ2",typeof(string)),
                             new DataColumn("FundQ3",typeof(string)),
                             new DataColumn("FundQ4",typeof(string)),
                             new DataColumn("FundQ5",typeof(string)),
                             new DataColumn("FundQ6",typeof(string)),
                             new DataColumn("FundQ7",typeof(string)),
                             new DataColumn("FundQ8",typeof(string)),
                             new DataColumn("FundQ9",typeof(string)),
                             new DataColumn("FundQ10",typeof(string)),
                             new DataColumn("FundQ11",typeof(string)),
                             new DataColumn("FundQ12",typeof(string)),
                             new DataColumn("FundQ13",typeof(string)),
                             new DataColumn("FundQ14",typeof(string)),
                             new DataColumn("FundQ15",typeof(string)),
                             new DataColumn("FundQ16",typeof(string))
                    });

                    DataTable dt1 = new DataTable();
                    dt1.Columns.AddRange(new DataColumn[19] {
                        new DataColumn("ProgramType", typeof(string)),
                        new DataColumn("Description",typeof(string)),
                        new DataColumn("PIRReport",typeof(bool)),
                        new DataColumn("Slots",typeof(string)),
                        new DataColumn("ReferenceProg",typeof(string)),
                         new DataColumn("DivisionID",typeof(string)),
                         new DataColumn("MinAge",typeof(string)),
                        new DataColumn("MaxAge",typeof(string)),
                        new DataColumn("programstartDate",typeof(string)),
                        new DataColumn("programendDate",typeof(string)),
                        new DataColumn("ProgramID",typeof(Int32)),
                         new DataColumn("FundID",typeof(string)),
                          new DataColumn("OldFund",typeof(string)),
                          new DataColumn("HealthReview",typeof(bool)),
                          new DataColumn("LastDateCurrentApplication",typeof(string)),
                          new DataColumn("DateFutureApplication",typeof(string)),
                          new DataColumn("TransitionDate",typeof(string)),
                           new DataColumn("ProgramTypeAssociation",typeof(string)),
                           new DataColumn("Status",typeof(int))

                        });

                    foreach (Agency.FundSource fund in agencyDetails.FundSourcedata)
                    {
                        if (fund.Acronym != null && fund.Description != null)
                        {
                            dt.Rows.Add(fund.Acronym, fund.Description, fund.Amount, fund.Date, fund.Duration, fund.ServiceQty, fund.fundingtype,
                                //(string.IsNullOrEmpty(fund.ProgramYear)) ? "" : (fund.ProgramYear).Replace("-", ""), 
                                fund.ProgramYear,
                                fund.grantNo,

                                fund.nameGranteeDelegate, fund.FundStatus, fund.FundID, fund.OldFund,
                                fund.FundQ1, fund.FundQ2, fund.FundQ3, fund.FundQ4, fund.FundQ5, fund.FundQ6, fund.FundQ7,
                                fund.FundQ8, fund.FundQ9, fund.FundQ10, fund.FundQ11, fund.FundQ12, fund.FundQ13, fund.FundQ14
                                , fund.FundQ15, fund.FundQ16
                                );
                        }
                    }

                    foreach (Agency.ProgramType prog in agencyDetails.ProgramTypeList)
                    {
                        if (prog.ProgramTypes != null && prog.Description != null)
                        {
                            dt1.Rows.Add(prog.ProgramTypes, prog.Description, prog.PIRReport,
                                prog.Slots, prog.ReferenceProg, prog.DivisionID, prog.MinAge, prog.MaxAge,
                                prog.programstartDate, prog.programendDate, prog.ProgramID,
                                prog.FundID, prog.OldFund, prog.HealthReview, prog.LastDateCurrentApplication, prog.DateFutureApplication, prog.TransitionDate, prog.ProgramTypeAssociation, prog.ProgStatus);//changes

                        }
                    }


                    command.Parameters.Add(new SqlParameter("@tblfund", dt));
                    command.Parameters.Add(new SqlParameter("@tblprog", dt1));

                    command.Parameters.Add(new SqlParameter("@FundQ1", agencyDetails._FundedEnrollment.FundQ1));
                    command.Parameters.Add(new SqlParameter("@FundQ2", agencyDetails._FundedEnrollment.FundQ2));
                    command.Parameters.Add(new SqlParameter("@FundQ3", agencyDetails._FundedEnrollment.FundQ3));
                    command.Parameters.Add(new SqlParameter("@FundQ4", agencyDetails._FundedEnrollment.FundQ4));
                    command.Parameters.Add(new SqlParameter("@FundQ5", agencyDetails._FundedEnrollment.FundQ5));
                    command.Parameters.Add(new SqlParameter("@FundQ6", agencyDetails._FundedEnrollment.FundQ6));
                    command.Parameters.Add(new SqlParameter("@FundQ7", agencyDetails._FundedEnrollment.FundQ7));
                    command.Parameters.Add(new SqlParameter("@FundQ8", agencyDetails._FundedEnrollment.FundQ8));
                    command.Parameters.Add(new SqlParameter("@FundQ9", agencyDetails._FundedEnrollment.FundQ9));
                    command.Parameters.Add(new SqlParameter("@FundQ10", agencyDetails._FundedEnrollment.FundQ10));
                    command.Parameters.Add(new SqlParameter("@FundQ11", agencyDetails._FundedEnrollment.FundQ11));
                    command.Parameters.Add(new SqlParameter("@FundQ12", agencyDetails._FundedEnrollment.FundQ12));
                    command.Parameters.Add(new SqlParameter("@FundQ13", agencyDetails._FundedEnrollment.FundQ13));
                    command.Parameters.Add(new SqlParameter("@FundQ14", agencyDetails._FundedEnrollment.FundQ14));
                    command.Parameters.Add(new SqlParameter("@FundQ15", agencyDetails._FundedEnrollment.FundQ15));
                    command.Parameters.Add(new SqlParameter("@FundQ16", agencyDetails._FundedEnrollment.FundQ16));


                }
                command.ExecuteNonQuery();
                tranSaction.Commit();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                if (tranSaction != null)
                    tranSaction.Rollback();
                clsError.WriteException(ex);
                return ex.Message;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
                Connection.Close();
                command.Dispose();
            }
        }


        public string Add_Edit_AgencyStaffInfo(AgencyStaff obj, string mode, string agencyId, string RoleId, out string ID, out string AgencyCode, bool isEndOfYear = false)
        {
            string res = "";
            AgencyCode = ID = "";

            try
            {
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        Connection.Open();
                        command.Connection = Connection;
                        tranSaction = Connection.BeginTransaction();
                        command.Transaction = tranSaction;
                        command.CommandText = "Sp_Insert_Update_AgencyStaff";
                        command.Parameters.AddWithValue("@AgencyId", agencyId);
                        command.Parameters.AddWithValue("@Usrname", obj.Username);
                        if (!String.IsNullOrEmpty(obj.Password))
                            command.Parameters.AddWithValue("@password", EncryptDecrypt.Encrypt(obj.Password));
                        else command.Parameters.AddWithValue("@password", " ");
                        if (RoleId == "0")
                            command.Parameters.AddWithValue("@RoleId", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@RoleId", RoleId);
                        command.Parameters.AddWithValue("@Fname", obj.FirstName);
                        command.Parameters.AddWithValue("@Lname", obj.LastName);
                        command.Parameters.AddWithValue("@Race", obj.Race);
                        command.Parameters.AddWithValue("@Nation", obj.Natinality);
                        command.Parameters.AddWithValue("@Hire_Date", obj.HireDate);
                        command.Parameters.AddWithValue("@Term_Date", obj.TerDate);
                        command.Parameters.AddWithValue("@Email", obj.EmailAddress);
                        command.Parameters.AddWithValue("@Cellno", obj.CellNumber);
                        command.Parameters.AddWithValue("@Hr_Center", obj.HRCenter);
                        command.Parameters.AddWithValue("@HgestEdu", obj.HighestEducation);
                        command.Parameters.AddWithValue("@Child_Hood", obj.EarlyChildHood);
                        command.Parameters.AddWithValue("@Getting_Degree", obj.GettingDegree);
                        command.Parameters.AddWithValue("@Hrly_Rate", obj.HourlyRate);
                        command.Parameters.AddWithValue("@Slry", obj.Salary);
                        command.Parameters.AddWithValue("@Cntractor", obj.Contractor);
                        command.Parameters.AddWithValue("@Associated_Program", obj.AssociatedProgram);
                        command.Parameters.AddWithValue("@Rplacement", obj.Replacement);
                        command.Parameters.AddWithValue("@DateOfBirth", obj.DOB);
                        command.Parameters.AddWithValue("@Access_Days", obj.AccessDays);
                        command.Parameters.AddWithValue("@Start_Time", obj.AccessStart);
                        command.Parameters.AddWithValue("@EndTime", obj.AccessStop);
                        command.Parameters.AddWithValue("@timezoneid", obj.TimeZoneID);
                        command.Parameters.AddWithValue("@Avtr", obj.AvatarUrl);
                        command.Parameters.AddWithValue("@AvtrH", obj.AvatarhUrl);
                        command.Parameters.AddWithValue("@Avtrs", obj.AvatarsUrl);
                        command.Parameters.AddWithValue("@AvtrT", obj.AvatartUrl);
                        command.Parameters.AddWithValue("@CreatBy", obj.CreatedBy);
                        command.Parameters.AddWithValue("@UpdatBy", obj.UpdatedBy);
                        command.Parameters.AddWithValue("@mode", mode);
                        command.Parameters.AddWithValue("@AgencyStaffId", obj.AgencyStaffId);
                        command.Parameters.AddWithValue("@EnRollId", obj.EnrollmentId);
                        command.Parameters.AddWithValue("@LginAllowed", obj.LoginAllowed);
                        command.Parameters.AddWithValue("@AccStartDate", obj.AccessStartDate);
                        command.Parameters.AddWithValue("@Gnder", obj.Gender);
                        command.Parameters.AddWithValue("@pirroleid", obj.PirRoleid);
                        command.Parameters.AddWithValue("@enroleid", obj.enrollid);
                        command.Parameters.AddWithValue("@centerlist", obj.centerlist);
                        command.Parameters.AddWithValue("@Classrooms", obj.Classrooms);
                        command.Parameters.AddWithValue("@Rolelist", obj.Rolelist);
                        command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                        command.Parameters.Add(new SqlParameter("@UsrID", string.Empty)).Direction = ParameterDirection.Output;
                        command.Parameters["@UsrID"].Size = 50;
                        command.Parameters.Add(new SqlParameter("@AgncyCode", string.Empty)).Direction = ParameterDirection.Output;
                        command.Parameters["@AgncyCode"].Size = 50;
                        // to add primary language

                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[3] {
                                 new DataColumn("LanguageId", typeof(string)),
                                 new DataColumn("IsSpoken",typeof(bool)),
                                  new DataColumn("OtherLanguage",typeof(string))
                    });
                        foreach (PrimaryLanguages lang in obj.LangList)
                        {
                            if (lang.LanguageId != 0 && lang.IsSpoken)
                            {
                                dt.Rows.Add(lang.LanguageId, lang.IsSpoken, lang.OtherLanguage);

                            }
                        }
                        command.Parameters.Add(new SqlParameter("@PrimaryLanguage", dt));
                        command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                        command.Parameters.Add(new SqlParameter("@ProgramYear", obj.ProgramYear));
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                        res = command.Parameters["@result"].Value.ToString();
                        ID = command.Parameters["@UsrID"].Value.ToString();
                        AgencyCode = command.Parameters["@AgncyCode"].Value.ToString();
                        tranSaction.Commit();
                        Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                if (tranSaction != null)
                    tranSaction.Rollback();
                clsError.WriteException(ex);
            }
            return res;
        }
        public Agency editAgency(string id)
        {
            Agency agency = new Agency();
            agency._FundedEnrollment = new Agency.FundedEnrollment();
            agency.FundSourcedata = new List<Agency.FundSource>();
            agency.ProgramTypeList = new List<Agency.ProgramType>();
            agency.DivisionsList = new List<SelectListItem>();
            agency.ProgramYearList = new List<SelectListItem>();
            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getagencyinfo";

                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["address1"]).ToString()))
                        {
                            agency.address1 = (_dataset.Tables[0].Rows[0]["address1"]).ToString();
                        }
                        else
                        {
                            agency.address1 = string.Empty;
                        }
                        //Changes
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["ChildTransport"]).ToString()))
                        {
                            agency.Transportation = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["ChildTransport"]);
                        }
                        else
                        {
                            agency.Transportation = false;

                        }
                        agency.address2 = Convert.ToString(_dataset.Tables[0].Rows[0]["address2"]);
                        agency.agencyCode = Convert.ToString(_dataset.Tables[0].Rows[0]["agencyCode"]);
                        agency.agencyId = Convert.ToString(_dataset.Tables[0].Rows[0]["agencyId"]);
                        agency.agencyName = Convert.ToString(_dataset.Tables[0].Rows[0]["agencyName"]);
                        agency.AcceptanceProcess = string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["WaterFallOrLinear"].ToString()) ? false : Convert.ToBoolean(_dataset.Tables[0].Rows[0]["WaterFallOrLinear"]);
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["city"]).ToString()))
                            agency.city = (_dataset.Tables[0].Rows[0]["city"]).ToString();
                        else
                            agency.city = string.Empty;

                        agency.State = string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["State"].ToString()) ? "" : _dataset.Tables[0].Rows[0]["State"].ToString();
                        agency.County = string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["County"].ToString()) ? "" : _dataset.Tables[0].Rows[0]["County"].ToString();

                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["fax"]).ToString()))
                            agency.fax = (_dataset.Tables[0].Rows[0]["fax"]).ToString();
                        else
                            agency.fax = string.Empty;
                        agency.phone1 = Convert.ToString(_dataset.Tables[0].Rows[0]["phone1"]);
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["phone2"]).ToString()))
                            agency.phone2 = (_dataset.Tables[0].Rows[0]["phone2"]).ToString();
                        else
                            agency.phone2 = string.Empty;
                        agency.primaryEmail = Convert.ToString(_dataset.Tables[0].Rows[0]["primaryEmail"]);
                        agency.zipCode = Convert.ToString(_dataset.Tables[0].Rows[0]["zipCode"]);
                        agency.status = Convert.ToChar(_dataset.Tables[0].Rows[0]["status"]);
                        agency.nationality = Convert.ToString(_dataset.Tables[0].Rows[0]["Nationality"]);



                        agency.firstName = Convert.ToString(_dataset.Tables[0].Rows[0]["firstname"]);
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["lastname"]).ToString()))
                        {
                            agency.LastName = Convert.ToString(_dataset.Tables[0].Rows[0]["lastname"]);
                        }
                        else
                        {
                            agency.LastName = string.Empty;
                        }

                        agency.AccessDays = _dataset.Tables[0].Rows[0]["Accesstype"].ToString();
                        agency.AccessStartDate = Convert.ToString(_dataset.Tables[0].Rows[0]["AccessStartDate"]);
                        agency.AccessStop = _dataset.Tables[0].Rows[0]["AccessStop"].ToString();
                        agency.AccessStart = _dataset.Tables[0].Rows[0]["AccessStart"].ToString();
                        agency.TimeZoneID = _dataset.Tables[0].Rows[0]["TimeZone_ID"].ToString();
                        if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["programstartDate"])))
                            agency.programstartDate = Convert.ToString(_dataset.Tables[0].Rows[0]["programstartDate"]);
                        else
                            agency.programstartDate = string.Empty;
                        if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["programendDate"])))
                            agency.programendDate = Convert.ToString(_dataset.Tables[0].Rows[0]["programendDate"]);
                        else
                            agency.programendDate = string.Empty;

                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["SpeedZip"]).ToString()))
                            agency.SpeedZip = _dataset.Tables[0].Rows[0]["SpeedZip"].ToString();
                        else
                            agency.SpeedZip = string.Empty;
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["ActiveProgramYear"]).ToString()))
                            agency.ActiveProgYear = _dataset.Tables[0].Rows[0]["ActiveProgramYear"].ToString();//Active Prog year
                        else
                            agency.ActiveProgYear = string.Empty;
                        if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[0]["DocsStorage"]).ToString()))
                            agency.DocsStorage = _dataset.Tables[0].Rows[0]["DocsStorage"].ToString();
                        else
                            agency.DocsStorage = string.Empty;
                        agency.FSWYearlyVisit = _dataset.Tables[0].Rows[0]["FSWYearlyVisit"].ToString();
                        agency.Areabreakdown = _dataset.Tables[0].Rows[0]["Areabreakdown"].ToString();
                        agency.DivisionBreakDown = _dataset.Tables[0].Rows[0]["DivisionBreakDown"].ToString();
                        agency.AreaReference = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["AreaReference"]);
                        agency.DivisionReference = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["DivisionReference"]);

                        if (!string.IsNullOrEmpty(Convert.ToString(_dataset.Tables[0].Rows[0]["Yakkr600Days"])))
                            agency.Yakkr600 = Convert.ToString(_dataset.Tables[0].Rows[0]["Yakkr600Days"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(_dataset.Tables[0].Rows[0]["AttendanceIssuePercentage"])))
                            agency.Yakkr601 = Convert.ToString(_dataset.Tables[0].Rows[0]["AttendanceIssuePercentage"]);
                        agency.AttendanceIssueStartDay = (!string.IsNullOrEmpty(Convert.ToString(_dataset.Tables[0].Rows[0]["AttendanceIssueCheckDays"]))) ? Convert.ToString(_dataset.Tables[0].Rows[0]["AttendanceIssueCheckDays"]) : "";
                        agency.PurchasedSlots = Convert.ToInt32(_dataset.Tables[0].Rows[0]["PurchasedSlots"]);

                    }

                    //if (_dataset.Tables[1].Rows.Count > 0)
                    //{
                    //    List<FingerprintsModel.Agency.FundSource> listprog = new List<FingerprintsModel.Agency.FundSource>();
                    //  DataTable dv = _dataset.Tables[1].DefaultView.ToTable(true, "FundID");
                    //    FingerprintsModel.Agency.FundSource obj = null;
                    //    List<FingerprintsModel.Agency.ProgramType> listfundprog = null;
                    //    for (int i = 0; i < dv.Rows.Count; i++)
                    //    {
                    //        DataRow[] drs = _dataset.Tables[1].Select("FundID=" + dv.Rows[i]["FundID"].ToString());
                    //        obj = new FingerprintsModel.Agency.FundSource();
                    //        obj.FundID = Convert.ToInt32(drs[0]["FundID"].ToString());
                    //        obj.OldFund = "O";
                    //        obj.Acronym = drs[0]["Acronym"].ToString();
                    //        obj.Amount = Convert.ToInt32(drs[0]["Amount"].ToString());
                    //        DateTime dt = new DateTime();
                    //        DateTime.TryParse(drs[0]["Date"].ToString(), out dt);
                    //        obj.Date = dt.ToString("MM/dd/yyyy");
                    //        obj.Duration = drs[0]["Duration"].ToString();
                    //        obj.fundingtype = drs[0]["FundingType"].ToString();
                    //        obj.grantNo = drs[0]["GranteeNo"].ToString();
                    //        obj.nameGranteeDelegate = drs[0]["Grantee"].ToString();
                    //        obj.ProgramYear = drs[0]["ProgramYear"].ToString();
                    //        obj.ServiceQty = drs[0]["ServiceQty"].ToString();
                    //        obj.Description = drs[0]["FundDescription"].ToString();
                    //        obj.FundStatus = Convert.ToInt32(drs[0]["FundStatus"].ToString());
                    //        //Add fund Question
                    //        obj.FundQ1 = drs[0]["FundQ1"].ToString();
                    //        obj.FundQ2 = drs[0]["FundQ2"].ToString();
                    //        obj.FundQ3 = drs[0]["FundQ3"].ToString();
                    //        obj.FundQ4 = drs[0]["FundQ4"].ToString();
                    //        obj.FundQ5 = drs[0]["FundQ5"].ToString();
                    //        obj.FundQ6 = drs[0]["FundQ6"].ToString();
                    //        obj.FundQ7 = drs[0]["FundQ7"].ToString();
                    //        obj.FundQ8 = drs[0]["FundQ8"].ToString();
                    //        obj.FundQ9 = drs[0]["FundQ9"].ToString();
                    //        obj.FundQ10 = drs[0]["FundQ10"].ToString();
                    //        obj.FundQ11 = drs[0]["FundQ11"].ToString();
                    //        obj.FundQ12 = drs[0]["FundQ12"].ToString();
                    //        obj.FundQ13 = drs[0]["FundQ13"].ToString();
                    //        obj.FundQ14 = drs[0]["FundQ14"].ToString();
                    //        obj.FundQ15 = drs[0]["FundQ15"].ToString();
                    //        obj.FundQ16 = drs[0]["FundQ16"].ToString();
                    //        ///
                    //        listfundprog = new List<FingerprintsModel.Agency.ProgramType>();
                    //        FingerprintsModel.Agency.ProgramType objprog;
                    //        foreach (DataRow dr in drs)
                    //        {
                    //            objprog = new FingerprintsModel.Agency.ProgramType();
                    //            objprog.ProgramID = Convert.ToInt32(dr["ProgramTypeID"].ToString());
                    //            objprog.FundID = Convert.ToInt32(dr["FundID"].ToString());
                    //            objprog.ProgramTypes = dr["ProgramType"].ToString();
                    //            objprog.Description = dr["ProgDesc"].ToString();
                    //            if (!DBNull.Value.Equals((dr["PIRReport"]))) //Changes
                    //            {
                    //                objprog.PIRReport = Convert.ToBoolean(dr["PIRReport"].ToString());
                    //            }
                    //            else
                    //            {
                    //                objprog.PIRReport = false;// Convert.ToInt32(string.Empty);
                    //            }
                    //            objprog.Slots = dr["Slots"].ToString();
                    //            objprog.ReferenceProg = dr["ReferenceProg"].ToString();
                    //            objprog.Area = dr["AreaID"].ToString();
                    //            objprog.MinAge = Convert.ToInt32(dr["MinAge"].ToString());
                    //            objprog.MaxAge = Convert.ToInt32(dr["MaxAge"].ToString());
                    //            objprog.HealthReview = Convert.ToBoolean(dr["HealthReview"].ToString());
                    //            if (!DBNull.Value.Equals((dr["ProgStatus"]))) //Changes
                    //            {
                    //                objprog.ProgStatus = Convert.ToInt32(dr["ProgStatus"].ToString());
                    //            }
                    //            else
                    //            {
                    //                objprog.ProgStatus = 1;// Convert.ToInt32(string.Empty);
                    //            }
                    //            DateTime dt1 = new DateTime();
                    //            DateTime.TryParse(dr["programstartDate"].ToString(), out dt1);
                    //            objprog.programstartDate = dt1.ToString("MM/dd/yyyy");
                    //            DateTime dt2 = new DateTime();
                    //            DateTime.TryParse(dr["programendDate"].ToString(), out dt2);
                    //            objprog.programendDate = dt2.ToString("MM/dd/yyyy");
                    //            listfundprog.Add(objprog);
                    //        }
                    //        obj.progtypelist = listfundprog;
                    //        agency.FundSourcedata.Add(obj);
                    //    }
                    //}

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        agency.FundSourcedata = _dataset.Tables[1].AsEnumerable().Select(x => new Agency.FundSource
                        {

                            FundID = (int)x.Field<long>("FundID"),
                            OldFund = "0",
                            Acronym = x.Field<string>("Acronym"),
                            Description = x.Field<string>("FundDescription"),
                            Amount = x.Field<int>("Amount"),
                            Date = x.Field<string>("Date"),
                            FundStatus = x.Field<int>("FundStatus"),
                            Duration = x.Field<int>("Duration").ToString(),
                            ServiceQty = x.Field<int>("ServiceQty").ToString(),
                            fundingtype = x.Field<int>("FundingType").ToString(),
                            // ProgramYear = string.IsNullOrEmpty(x.Field<string>("ProgramYear").ToString()) ? "0" : x.Field<string>("ProgramYear").Substring(0, 2) + "-" + x.Field<string>("ProgramYear").Substring(2, 2),

                            ProgramYear = string.IsNullOrEmpty(x.Field<string>("ProgramYear").ToString()) ? "0" : x.Field<string>("ProgramYear"),

                            nameGranteeDelegate = x.Field<string>("Grantee"),
                            grantNo = x.Field<string>("GranteeNo"),
                            IsReferredByProgram = Convert.ToBoolean(x.Field<int>("IsReferredByProgram"))
                        }).ToList();
                    }


                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        agency._FundedEnrollment = _dataset.Tables[2].AsEnumerable().Select(x => new Agency.FundedEnrollment
                        {
                            FundIndexID = x.Field<long>("FundIndexID"),
                            FundQ1 = (x.Field<int>("FundQ1") == 0) ? "" : x.Field<int>("FundQ1").ToString(),
                            FundQ2 = (x.Field<int>("FundQ2") == 0) ? "" : x.Field<int>("FundQ2").ToString(),
                            FundQ3 = (x.Field<int>("FundQ3") == 0) ? "" : x.Field<int>("FundQ3").ToString(),
                            FundQ4 = (x.Field<int>("FundQ4") == 0) ? "" : x.Field<int>("FundQ4").ToString(),
                            FundQ5 = (x.Field<int>("FundQ5") == 0) ? "" : x.Field<int>("FundQ5").ToString(),
                            FundQ6 = (x.Field<int>("FundQ6") == 0) ? "" : x.Field<int>("FundQ6").ToString(),
                            FundQ7 = (x.Field<int>("FundQ7") == 0) ? "" : x.Field<int>("FundQ7").ToString(),
                            FundQ8 = (x.Field<int>("FundQ8") == 0) ? "" : x.Field<int>("FundQ8").ToString(),
                            FundQ9 = (x.Field<int>("FundQ9") == 0) ? "" : x.Field<int>("FundQ9").ToString(),
                            FundQ10 = (x.Field<int>("FundQ10") == 0) ? "" : x.Field<int>("FundQ10").ToString(),
                            FundQ11 = (x.Field<int>("FundQ11") == 0) ? "" : x.Field<int>("FundQ11").ToString(),
                            FundQ12 = (x.Field<int>("FundQ12") == 0) ? "" : x.Field<int>("FundQ12").ToString(),
                            FundQ13 = (x.Field<int>("FundQ13") == 0) ? "" : x.Field<int>("FundQ13").ToString(),
                            FundQ14 = (x.Field<int>("FundQ14") == 0) ? "" : x.Field<int>("FundQ14").ToString(),
                            FundQ15 = (x.Field<int>("FundQ15") == 0) ? "" : x.Field<int>("FundQ15").ToString(),
                            FundQ16 = (x.Field<int>("FundQ16") == 0) ? "" : x.Field<int>("FundQ16").ToString(),
                            AgencyID = x.Field<Guid>("AgencyID")

                        }).ToList()[0];
                    }

                    if (_dataset.Tables[3].Rows.Count > 0)
                    {
                        agency.ProgramTypeList = _dataset.Tables[3].AsEnumerable().Select(x => new Agency.ProgramType
                        {

                            ProgramID = Convert.ToInt32(x.Field<long>("ProgramTypeID")),
                            FundID = x.Field<string>("FundID"),
                            ProgramTypes = x.Field<string>("ProgramType"),
                            Description = x.Field<string>("ProgDesc"),
                            PIRReport = x.Field<bool>("PIRReport"),
                            Slots = string.IsNullOrEmpty(x.Field<int?>("Slots").ToString()) ? "" : x.Field<int>("Slots").ToString(),
                            ReferenceProg = string.IsNullOrEmpty(x.Field<int?>("ReferenceProg").ToString()) ? "" : x.Field<int?>("ReferenceProg").ToString(),
                            DivisionID = string.IsNullOrEmpty(x.Field<long?>("DivisionID").ToString()) ? "1" : x.Field<long>("DivisionID").ToString(),
                            //Area = x.Field<string>("AreaID"),
                            MinAge = string.IsNullOrEmpty(x.Field<int?>("MinAge").ToString()) ? 0 : x.Field<int>("MinAge"),
                            MaxAge = string.IsNullOrEmpty(x.Field<int?>("MaxAge").ToString()) ? 0 : x.Field<int>("MaxAge"),
                            HealthReview = x.Field<bool>("HealthReview"),
                            ProgStatus = string.IsNullOrEmpty(x.Field<int?>("ProgStatus").ToString()) ? 1 : x.Field<int>("ProgStatus"),
                            programstartDate = string.IsNullOrEmpty(x.Field<string>("ProgramStartDate")) ? "" : x.Field<string>("ProgramStartDate"),
                            programendDate = string.IsNullOrEmpty(x.Field<string>("ProgramEndDate")) ? "" : x.Field<string>("ProgramEndDate"),
                            LastDateCurrentApplication = string.IsNullOrEmpty(x.Field<string>("LastDateCurrentApplication")) ? "" : x.Field<string>("LastDateCurrentApplication"),
                            DateFutureApplication = string.IsNullOrEmpty(x.Field<string>("DateFutureApplication")) ? "" : x.Field<string>("DateFutureApplication"),
                            TransitionDate = string.IsNullOrEmpty(x.Field<string>("TransitionDate")) ? "" : x.Field<string>("TransitionDate"),
                            ProgramTypeAssociation = string.IsNullOrEmpty(x.Field<long?>("ProgramTypeAssociation").ToString()) ? "" : x.Field<long>("ProgramTypeAssociation").ToString()

                        }).ToList();


                    }
                    if (_dataset.Tables[4] != null && _dataset.Tables[4].Rows.Count > 0)
                    {
                        agency.DivisionsList = _dataset.Tables[4].AsEnumerable().Select(x => new SelectListItem
                        {
                            Text = x.Field<long>("DivisionID").ToString(),
                            Value = x.Field<long>("DivisionID").ToString()

                        }).ToList();
                    }

                    if(_dataset.Tables.Count>5 && _dataset.Tables[5]!=null && _dataset.Tables[5].Rows.Count>0)
                    {
                        agency.ProgramYearList = (from DataRow dr5 in _dataset.Tables[5].Rows
                                                  select new SelectListItem
                                                  {
                                                      Text = Convert.ToString(dr5["ActiveProgramYear"]),
                                                      Value = Convert.ToString(dr5["ActiveProgramYear"])
                                                  }

                                                ).ToList();
                    }

                }

                DataAdapter.Dispose();
                command.Dispose();
                return agency;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return agency;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
        }
        public List<Agency> getagencyList(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string status, string userid)
        {
            List<Agency> _agencylist = new List<Agency>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                if (status.Contains("0") || status.Contains("1"))
                    command.Parameters.Add(new SqlParameter("@status", DBNull.Value));
                if (status.Contains("2"))
                    command.Parameters.Add(new SqlParameter("@status", "1"));
                if (status.Contains("3"))
                    command.Parameters.Add(new SqlParameter("@status", "0"));

                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_agency_list";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        Agency addagencyRow = new Agency();
                        addagencyRow.agencyCode = Convert.ToString(agencydataTable.Rows[i]["agencyCode"]);
                        addagencyRow.agencyId = Convert.ToString(agencydataTable.Rows[i]["agencyId"]);
                        addagencyRow.agencyName = Convert.ToString(agencydataTable.Rows[i]["agencyName"]);
                        addagencyRow.nameGranteeDelegate = Convert.ToString(agencydataTable.Rows[i]["nameGranteeDelegate"]);
                        addagencyRow.primaryEmail = Convert.ToString(agencydataTable.Rows[i]["primaryEmail"]);
                        addagencyRow.programName = Convert.ToString(agencydataTable.Rows[i]["programName"]);
                        addagencyRow.status = Convert.ToChar(agencydataTable.Rows[i]["status"]);
                        addagencyRow.createdDate = Convert.ToDateTime(agencydataTable.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");
                        addagencyRow.LastLogin = agencydataTable.Rows[i]["lastlogin"].ToString();
                        addagencyRow.userName = agencydataTable.Rows[i]["Name"].ToString();
                        _agencylist.Add(addagencyRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _agencylist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _agencylist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public List<Agency> AgencyList()
        {
            List<Agency> _agencylist = new List<Agency>();
            try
            {
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_AgencyList";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Agency obj = new Agency();
                        obj.agencyId = Convert.ToString(dr["agencyId"].ToString());
                        obj.agencyName = dr["agencyName"].ToString();
                        _agencylist.Add(obj);
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return _agencylist;
        }
        public AgencyStaff GetData_AllDropdown(string agencyid = "", int i = 0, Guid id = new Guid(), AgencyStaff staff = null, bool isEndOfYear = false)
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            AgencyStaff _staff = new AgencyStaff();
            PrimaryLanguages language = new PrimaryLanguages();
            _staff.ProgramYearList = new List<SelectListItem>();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_AgencyUser_Dropdowndata";
                        command.Parameters.Add(new SqlParameter("@AgencyStaffId", id));
                        command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                        if (!string.IsNullOrEmpty(agencyid))
                            command.Parameters.Add(new SqlParameter("@agencyID", agencyid));
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }
                if (staff != null)
                {
                    _staff = staff;
                }
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                try
                {
                    List<Agency> listAgency = new List<Agency>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Agency obj = new Agency();
                        obj.agencyId = Convert.ToString(dr["agencyId"].ToString());
                        obj.agencyName = dr["agencyName"].ToString();
                        obj.ActiveProgYear = string.IsNullOrEmpty(dr["ActiveProgramYear"].ToString()) ? "" : dr["ActiveProgramYear"].ToString();
                        listAgency.Add(obj);
                    }
                    //listAgency.Insert(0, new Agency() { agencyId = "0", agencyName = "Select" });
                    _staff.agncylist = listAgency;
                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }
                //  }
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    try
                    {
                        List<Role> _rolelist = new List<Role>();
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            Role obj = new Role();
                            obj.RoleId = dr["roleid"].ToString();
                            obj.RoleName = dr["roleName"].ToString();
                            _rolelist.Add(obj);
                        }
                        _staff.rolelist = _rolelist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    try
                    {
                        List<RaceInfo> _racelist = new List<RaceInfo>();
                        //_staff.myList
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            RaceInfo obj = new RaceInfo();
                            obj.RaceId = dr["Id"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _racelist.Add(obj);
                        }
                        //_racelist.Insert(0, new RaceInfo() { RaceId = "0", Name = "Select" });
                        _staff.raceList = _racelist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    try
                    {
                        List<NationalityInfo> _nationlist = new List<NationalityInfo>();
                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            NationalityInfo obj = new NationalityInfo();
                            obj.NationId = dr["Id"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _nationlist.Add(obj);
                        }
                        // _nationlist.Insert(0, new NationalityInfo() { NationId = "0", Name = "Select" });
                        _staff.nationList = _nationlist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {
                    try
                    {
                        List<EducationLevelIno> _EducationLevel = new List<EducationLevelIno>();
                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            EducationLevelIno obj = new EducationLevelIno();
                            obj.EducationLevel = dr["EducationLevel"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _EducationLevel.Add(obj);
                        }
                        //_EducationLevel.Insert(0, new EducationLevelIno() { EducationLevel = "-1", Name = "Select" });
                        _staff.EducationLevelList = _EducationLevel;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }

                if (ds.Tables[5] != null && ds.Tables[5].Rows.Count > 0)
                {

                    List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                    foreach (DataRow dr in ds.Tables[5].Rows)
                    {
                        HrCenterInfo obj = new HrCenterInfo();
                        obj.CenterId = dr["CenterId"].ToString();
                        obj.Name = dr["CenterName"].ToString();
                        obj.Homebased = Convert.ToBoolean(dr["HomeBased"].ToString());

                        centerList.Add(obj);
                    }
                    //centerList.Insert(0, new HrCenterInfo() { CenterId = "0", Name = "Select" });
                    _staff.HrcenterList = centerList;
                }
                if (ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                {

                    List<PirInfo> PirList = new List<PirInfo>();
                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        PirInfo obj = new PirInfo();
                        obj.PirId = dr["Id"].ToString();
                        obj.Name = dr["PirRole"].ToString();
                        PirList.Add(obj);
                    }
                    //PirList.Insert(0, new PirInfo() { PirId = "0", Name = "Select" });
                    _staff.Pirlist = PirList;
                }
                if (ds.Tables[7] != null && ds.Tables[7].Rows.Count > 0)
                {

                    List<TimeZoneinfo> TimeZonelist = new List<TimeZoneinfo>();
                    foreach (DataRow dr in ds.Tables[7].Rows)
                    {
                        TimeZoneinfo obj = new TimeZoneinfo();
                        obj.TimZoneId = dr["TimeZone_ID"].ToString();
                        obj.TimZoneName = dr["TIMEZONENAME"].ToString();
                        TimeZonelist.Add(obj);
                    }
                    //TimeZonelist.Insert(0, new TimeZoneinfo() { TimZoneId = "0", TimZoneName = "Select" });
                    _staff.TimeZonelist = TimeZonelist;
                }
                if (ds.Tables[8] != null && ds.Tables[8].Rows.Count > 0)
                {

                    List<RefInfo> prolist = new List<RefInfo>();
                    foreach (DataRow dr in ds.Tables[8].Rows)
                    {
                        RefInfo obj = new RefInfo();
                        obj.Id = dr["ProgramTypeID"].ToString();
                        obj.Name = dr["ProgramType"].ToString();
                        prolist.Add(obj);
                    }
                    //TimeZonelist.Insert(0, new TimeZoneinfo() { TimZoneId = "0", TimZoneName = "Select" });
                    _staff.progList = prolist;
                }
                if (ds.Tables[9] != null && ds.Tables[9].Rows.Count > 0)
                {

                    List<ProgInfo> proglist = new List<ProgInfo>();
                    foreach (DataRow dr in ds.Tables[9].Rows)
                    {
                        ProgInfo obj = new ProgInfo();
                        obj.Id = dr["ReferenceId"].ToString();
                        obj.Name = dr["Name"].ToString();
                        proglist.Add(obj);
                    }
                    //TimeZonelist.Insert(0, new TimeZoneinfo() { TimZoneId = "0", TimZoneName = "Select" });
                    _staff.refList = proglist;
                }
                //Changes
                if (ds.Tables[10] != null && ds.Tables[10].Rows.Count > 0)
                {

                    List<ClassRoom> classlist = new List<ClassRoom>();
                    foreach (DataRow dr in ds.Tables[10].Rows)
                    {
                        ClassRoom obj = new ClassRoom();
                        obj.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                        obj.ClassName = dr["ClassroomName"].ToString();
                        classlist.Add(obj);
                    }
                    _staff.Classroom = classlist;
                }
                if (ds.Tables[12].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[12].Rows)
                    {
                        language = new PrimaryLanguages();
                        language.LanguageId = Convert.ToInt32(dr["LanguageID"]);
                        language.LanguageName = Convert.ToString(dr["PrimaryLanguage"]);
                        language.OtherLanguage = Convert.ToString(dr["OtherLanguage"]);
                        language.IsSpoken = ((dr["IsSpoken"]) == DBNull.Value) ? false : true;
                        _staff.LangList.Add(language);
                    }

                }

                if (ds.Tables[13] != null && ds.Tables[13].Rows.Count > 0)
                {
                    _staff.ProgramYearList = (from DataRow dr13 in ds.Tables[13].Rows
                                              select new SelectListItem
                                              {
                                                  Text = Convert.ToString(dr13["ActiveProgramYear"]),
                                                  Value = Convert.ToString(dr13["ActiveProgramYear"]),
                                                  Selected = (isEndOfYear) ? true : false


                                              }


                                            ).ToList();

                    if (!isEndOfYear)
                    {
                        _staff.ProgramYearList.Insert(0, new SelectListItem());
                    }
                }



                //if (ds.Tables[11] != null && ds.Tables[11].Rows.Count > 0)
                //{

                //    List<UserInfo> userlist = new List<UserInfo>();
                //    foreach (DataRow dr in ds.Tables[11].Rows)
                //    {
                //        UserInfo obj = new UserInfo();
                //        obj.userId =(dr["ID"]).ToString();
                //        obj.Name = dr["Name"].ToString();
                //        userlist.Add(obj);
                //    }
                //    _staff.UserList = userlist;
                //}

                //End
                if (i == 1)
                {
                    GetAgencyStaffDetail(id, _staff, isEndOfYear);
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            //  _agencyStafflist.Add(_staff);
            return _staff;
        }
        public AgencyStaff GetAgencyStaffDetail(Guid id, AgencyStaff obj, bool isEndOfYear = false)
        {
            PrimaryLanguages language = new PrimaryLanguages();
            //AgencyStaff obj = new AgencyStaff();
            try
            {
                DataSet ds = null;
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "Sp_SelectStaffDetail";
                        command.Parameters.AddWithValue("@StaffId", id);
                        command.Parameters.AddWithValue("@IsEndOfYear", isEndOfYear);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
                #region StaffDetailSection
                if (dt.Rows.Count > 0)
                {
                    obj.FirstName = Convert.ToString(dt.Rows[0]["FirstName"].ToString());
                    obj.LastName = Convert.ToString(dt.Rows[0]["LastName"].ToString());
                    //obj.Username = Convert.ToString(dt.Rows[0]["UserName"].ToString());
                    obj.AgencyStaffId = Guid.Parse(dt.Rows[0]["Id"].ToString());
                    obj.CellNumber = dt.Rows[0]["CellNumber"].ToString();
                    obj.EmailAddress = Convert.ToString(dt.Rows[0]["EmailAddress"].ToString());
                    obj.LoginAllowed = Convert.ToBoolean(dt.Rows[0]["LoginAllowed"].ToString());
                    obj.Race = Convert.ToString(dt.Rows[0]["Race"].ToString());
                    obj.Natinality = Convert.ToString(dt.Rows[0]["Nationality"].ToString());
                    obj.TerDate = Convert.ToString(dt.Rows[0]["TermDate"].ToString());
                    obj.HireDate = Convert.ToString(dt.Rows[0]["HireDate"].ToString());
                    obj.EmailAddress = Convert.ToString(dt.Rows[0]["EmailAddress"].ToString());
                    obj.HourlyRate = Convert.ToString(dt.Rows[0]["HourlyRate"].ToString());
                    obj.Salary = Convert.ToString(dt.Rows[0]["Salary"].ToString());
                    obj.DOB = Convert.ToString(dt.Rows[0]["DOB"].ToString());
                    obj.AccessStartDate = Convert.ToString(dt.Rows[0]["AccessStartDate"].ToString());
                    obj.AccessStart = Convert.ToString(dt.Rows[0]["AccessStart"].ToString());
                    obj.AccessStop = Convert.ToString(dt.Rows[0]["AccessStop"].ToString());
                    obj.SelectedAgencyId = Guid.Parse(dt.Rows[0]["AgencyId"].ToString());
                    obj.SelectedRoleId = Convert.ToString(dt.Rows[0]["RoleId"].ToString());
                    obj.AvatarUrl = Convert.ToString(dt.Rows[0]["Avatar"].ToString());
                    obj.EarlyChildHood = Convert.ToString(dt.Rows[0]["EarlyChildHood"].ToString());
                    obj.GettingDegree = Convert.ToString(dt.Rows[0]["GettingDegree"].ToString());
                    obj.AvatarhUrl = Convert.ToString(dt.Rows[0]["AvatarH"].ToString());
                    obj.AvatarsUrl = Convert.ToString(dt.Rows[0]["AvatarS"].ToString());
                    obj.AvatartUrl = Convert.ToString(dt.Rows[0]["AvatarT"].ToString());
                    obj.HRCenter = Convert.ToString(dt.Rows[0]["HrCenter"].ToString());
                    obj.AccessDays = Convert.ToString(dt.Rows[0]["AccessDays"].ToString());
                    obj.Gender = Convert.ToString(dt.Rows[0]["Gender"].ToString());
                    obj.Contractor = Convert.ToString(dt.Rows[0]["Contractor"].ToString());
                    obj.Replacement = Convert.ToString(dt.Rows[0]["Replacement"].ToString());
                    obj.AssociatedProgram = Convert.ToString(dt.Rows[0]["AssociatedProgram"].ToString());
                    obj.HighestEducation = Convert.ToString(dt.Rows[0]["HighestEducation"].ToString());
                    obj.AgencyName = Convert.ToString(dt.Rows[0]["AgencyName"].ToString());
                    obj.roleName = Convert.ToString(dt.Rows[0]["RoleName"].ToString());
                    obj.PirRoleid = Convert.ToString(dt.Rows[0]["PirRoleid"].ToString());
                    obj.TimeZoneID = Convert.ToString(dt.Rows[0]["TimeZone_ID"].ToString());
                    obj.ProgramYear = Convert.ToString(dt.Rows[0]["ActiveProgramYear"]);
                    if (!string.IsNullOrEmpty(dt.Rows[0]["ClassroomID"].ToString()))
                    {
                        obj.Classrooms = dt.Rows[0]["ClassroomID"].ToString();
                    }
                    else
                    {
                        obj.Classrooms = string.Empty;
                    }

                }
                #endregion

                DataTable dtHr = new DataTable();
                dtHr = ds.Tables[1];
                List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                if (dtHr.Rows.Count > 0)
                {

                    for (int i = 0; i < dtHr.Rows.Count; i++)
                    {
                        HrCenterInfo info = new HrCenterInfo();
                        info.CenterId = Convert.ToString(dtHr.Rows[i]["CenterId"].ToString());
                        info.Name = Convert.ToString(dtHr.Rows[i]["CenterName"].ToString());
                        info.Homebased = Convert.ToBoolean(dtHr.Rows[i]["HomeBased"]);

                        centerList.Add(info);
                    }
                    //centerList.Insert(0, new HrCenterInfo() { CenterId = "0", Name = "Select" });


                }
                //if (ds.Tables[4].Rows.Count > 0)
                //{

                //    foreach (DataRow dr in ds.Tables[4].Rows)
                //    {
                //        language = new PrimaryLanguages();
                //        language.LanguageId = Convert.ToInt32(dr["LanguageID"]);
                //        language.LanguageName = Convert.ToString(dr["PrimaryLanguage"]);
                //        language.IsSpoken = ((dr["IsSpoken"]) == DBNull.Value) ? false : true;
                //        obj.LangList.Add(language);
                //    }

                //}
                obj.HrcenterList = centerList;
                #region
                DataTable dtcenter = new DataTable();
                dtcenter = ds.Tables[2];
                List<HrCenterInfo> _centerList = new List<HrCenterInfo>();
                if (dtcenter.Rows.Count > 0)
                {

                    for (int i = 0; i < dtcenter.Rows.Count; i++)
                    {
                        HrCenterInfo info = new HrCenterInfo();
                        info.CenterId = Convert.ToString(dtcenter.Rows[i]["center"].ToString());
                        _centerList.Add(info);
                    }

                }
                obj.centers = _centerList;


                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    List<Role> _RoleList = new List<Role>();
                    Role info = null;
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        info = new Role();
                        info.RoleId = ds.Tables[3].Rows[i]["RoleId"].ToString();
                        _RoleList.Add(info);
                    }


                    obj._rolelist = _RoleList;

                }





                #endregion



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return obj;

        }
        public List<SelectListItem> CenterListByAgency(string id, bool isEndOfYear = false)
        {
            List<SelectListItem> centerlist = new List<SelectListItem>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_CenterByAgency";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@AgencyId", Guid.Parse(id));
                        command.Parameters.AddWithValue("@IsEndOfYear", isEndOfYear);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Value = EncryptDecrypt.Encrypt64(dr["CenterID"].ToString());
                            obj.Text = dr["CenterName"].ToString();
                            centerlist.Add(obj);
                        }
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
            return centerlist;
        }
        public int updateAgency(Guid id, int mode, Guid userId)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@agencyID", id));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@userid", userId));
                command.CommandText = "Sp_Update_Agency";
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return 0;
            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
        }
        public List<AgencyStaff> getagencyuserList(out string totalrecord, bool agencyuserAll, Guid? agencyId, string sortOrder, string sortDirection, string search, int skip, int pageSize, string status)
        {
            List<AgencyStaff> staffList = new List<AgencyStaff>();
            try
            {
                totalrecord = string.Empty;
                if (agencyId == null && agencyuserAll == false && status.Contains("0"))
                    return staffList;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                if (agencyuserAll)
                    command.Parameters.Add(new SqlParameter("@agencyId", DBNull.Value));
                else
                    if (agencyId != null)
                    command.Parameters.Add(new SqlParameter("@agencyId", agencyId));
                else
                    command.Parameters.Add(new SqlParameter("@agencyId", DBNull.Value));


                if (status.Contains("0") || status.Contains("1"))
                    command.Parameters.Add(new SqlParameter("@status", DBNull.Value));
                if (status.Contains("2"))
                    command.Parameters.Add(new SqlParameter("@status", "1"));
                if (status.Contains("3"))
                    command.Parameters.Add(new SqlParameter("@status", "0"));
                if (status.Contains("4"))
                    command.Parameters.Add(new SqlParameter("@status", "2"));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_agencyuser_list";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        AgencyStaff staff = new AgencyStaff();
                        staff.FirstName = Convert.ToString(agencydataTable.Rows[i]["firstname"]);
                        staff.LastName = Convert.ToString(agencydataTable.Rows[i]["lastname"]);
                        staff.roleName = Convert.ToString(agencydataTable.Rows[i]["roleName"]);
                        staff.EmailAddress = Convert.ToString(agencydataTable.Rows[i]["EmailAddress"]);
                        staff.ISActive = Convert.ToChar(agencydataTable.Rows[i]["status"]);
                        staff.AgencyStatus = Convert.ToChar(agencydataTable.Rows[i]["AgencyStatus"]);
                        staff.AgencyStaffId = Guid.Parse(Convert.ToString(agencydataTable.Rows[i]["Id"]));
                        staff.createdDate = Convert.ToDateTime(agencydataTable.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");

                        staffList.Add(staff);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return staffList;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return staffList;
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
        public int updateagencyUser(Guid id, int mode, Guid userId)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Update_Agency_user";
                command.Parameters.Add(new SqlParameter("@agencyuserId", id));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@userid", userId));
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return 0;
            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
        }
        public string enrollmentcodeGeneration(char activationtime, Guid userId, Guid agencyId, string Description)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@userid", userId));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyId));
                command.Parameters.Add(new SqlParameter("@Description", Description));
                command.Parameters.Add(new SqlParameter("@enrollmentcodegenerated", string.Empty)).Direction = ParameterDirection.Output;
                command.Parameters["@enrollmentcodegenerated"].Size = 10;
                command.Parameters.Add(new SqlParameter("@validupto", activationtime));
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Enrollment_code_generation";
                command.ExecuteNonQuery();
                return Convert.ToString(command.Parameters["@enrollmentcodegenerated"].Value);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return ex.Message;
            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
        }
        public string addenrollmentemail(string userid, string emailIds, string enrollmentCode)
        {
            string result = string.Empty;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@email", emailIds));
                command.Parameters.Add(new SqlParameter("@enrollmentCode", enrollmentCode));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.Parameters["@result"].Size = 1000;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Add_Enrollmentemail";
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;

        }
        public List<Agency> AutoCompleteAgencyList(string term, string active = "0")
        {
            List<Agency> AgencyList = new List<Agency>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "AutoComplete_AgencyList";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@AgnyName", term);
                        command.Parameters.AddWithValue("@Active", active);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Agency obj = new Agency();
                            obj.agencyId = Convert.ToString(dr["AgencyId"].ToString());
                            obj.agencyName = dr["AgencyName"].ToString();
                            obj.AccessDays = dr["Accesstype"].ToString();
                            obj.AccessStartDate = Convert.ToDateTime(dr["AccessStartDate"]).ToString("MM/dd/yyyy");
                            obj.AccessStart = dr["AccessStart"].ToString();
                            obj.AccessStop = dr["AccessStop"].ToString();
                            obj.TimeZoneID = dr["TimeZone_ID"].ToString();
                            obj.ActiveProgYear = dr["ActiveProgramYear"].ToString();
                            obj.Slots = Convert.ToInt32(dr["Slots"]);
                            obj.SlotId = Convert.ToInt32(dr["SlotId"]);


                            AgencyList.Add(obj);
                        }
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
            return AgencyList;
        }
        public List<PendingApproval> getpendingApproval(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string agencyId)
        {
            List<PendingApproval> _Pendinglist = new List<PendingApproval>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyId));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_Pending_Approval";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        PendingApproval PendingApproval = new PendingApproval();
                        PendingApproval.Id = Convert.ToString(agencydataTable.Rows[i]["Id"]);
                        PendingApproval.Email = Convert.ToString(agencydataTable.Rows[i]["Email"]);
                        PendingApproval.EnrollmentCode = Convert.ToString(agencydataTable.Rows[i]["EnrollmentCode"]);
                        PendingApproval.MobileNo = Convert.ToString(agencydataTable.Rows[i]["MobileNo"]);
                        PendingApproval.name = Convert.ToString(agencydataTable.Rows[i]["name"]);
                        //PendingApproval.IsResend = Convert.ToString(agencydataTable.Rows[i]["IsResend"]);
                        //PendingApproval.IsApprove = Convert.ToString(agencydataTable.Rows[i]["Isapproved"]);
                        PendingApproval.RoleId = Convert.ToString(agencydataTable.Rows[i]["RoleId"]);
                        _Pendinglist.Add(PendingApproval);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _Pendinglist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _Pendinglist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public string approverejectRequest(Guid id, string action, string roleid, string userId)
        {
            string result = string.Empty;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@role", roleid));
                command.Parameters.Add(new SqlParameter("@action", action));
                command.Parameters.Add(new SqlParameter("@createdby", userId));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.CommandText = "Sp_approve_reject_request";
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;
        }
        public string Staffregistration(staffRegistration obj, out string StaffId)
        {
            string res = "";
            StaffId = "";
            try
            {
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = Connection;
                        command.CommandText = "SP_Staff_Registration_process";
                        if (!String.IsNullOrEmpty(obj.password))
                            command.Parameters.AddWithValue("@password", EncryptDecrypt.Encrypt(obj.password));
                        else command.Parameters.AddWithValue("@password", " ");
                        command.Parameters.AddWithValue("@firstName", obj.firstName);
                        if (!String.IsNullOrEmpty(obj.lastName))
                            command.Parameters.AddWithValue("@lastName", obj.lastName);
                        else command.Parameters.AddWithValue("@lastName", " ");
                        command.Parameters.AddWithValue("@Email", obj.emailid);
                        //command.Parameters.AddWithValue("@mobileNo", obj.mobile);
                        command.Parameters.AddWithValue("@enrollmentcode", obj.agencyCode);
                        command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                        command.Parameters.AddWithValue("@StaffId", string.Empty).Direction = ParameterDirection.Output;
                        command.Parameters["@StaffId"].Size = 50;
                        command.CommandType = CommandType.StoredProcedure;
                        Connection.Open();
                        command.ExecuteNonQuery();
                        Connection.Close();
                        res = command.Parameters["@result"].Value.ToString();
                        StaffId = command.Parameters["@StaffId"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return res;
        }
        public List<PendingApproval> getpendingVerification(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string agencyid)
        {
            List<PendingApproval> _Pendinglist = new List<PendingApproval>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_Pending_Verification";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        PendingApproval PendingApproval = new PendingApproval();
                        PendingApproval.Id = Convert.ToString(agencydataTable.Rows[i]["Id"]);
                        PendingApproval.Email = Convert.ToString(agencydataTable.Rows[i]["Email"]);
                        PendingApproval.EnrollmentCode = Convert.ToString(agencydataTable.Rows[i]["EnrollmentCode"]);
                        PendingApproval.MobileNo = Convert.ToString(agencydataTable.Rows[i]["MobileNo"]);
                        PendingApproval.name = Convert.ToString(agencydataTable.Rows[i]["name"]);
                        PendingApproval.rolename = Convert.ToString(agencydataTable.Rows[i]["Rolename"]);
                        PendingApproval.IsEndOfYear = Convert.ToBoolean(agencydataTable.Rows[i]["IsEndOfYear"]);
                        PendingApproval.ActiveProgramYear = Convert.ToString(agencydataTable.Rows[i]["ActiveProgramYear"]);
                        _Pendinglist.Add(PendingApproval);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _Pendinglist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _Pendinglist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public string approverejectRequest(string id, string action, string roleid, string userId)
        {
            string result = string.Empty;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@id", id));
                if (string.IsNullOrEmpty(roleid))
                    command.Parameters.Add(new SqlParameter("@role", DBNull.Value));
                else
                    command.Parameters.Add(new SqlParameter("@role", roleid));
                command.Parameters.Add(new SqlParameter("@action", action));
                command.Parameters.Add(new SqlParameter("@createdby", userId));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.CommandText = "Sp_approve_reject_request";
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;
        }
        public string approverejectrequestagencyHR(string id, string userId)
        {
            string result = string.Empty;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@createdby", userId));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.CommandText = "Sp_approve_reject_request_AgencyHR";
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;
        }
        public string emailVerification(AgencyStaff obj, string id)
        {
            string result = string.Empty;
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.AddWithValue("@Fname", obj.FirstName);
                command.Parameters.AddWithValue("@Lname", obj.LastName);
                command.Parameters.AddWithValue("@Race", obj.Race);
                command.Parameters.AddWithValue("@Nation", obj.Natinality);
                command.Parameters.AddWithValue("@Cellno", obj.CellNumber);
                command.Parameters.AddWithValue("@HgestEdu", obj.HighestEducation);
                command.Parameters.AddWithValue("@Child_Hood", obj.EarlyChildHood);
                command.Parameters.AddWithValue("@Getting_Degree", obj.GettingDegree);
                command.Parameters.AddWithValue("@DateOfBirth", obj.DOB);
                command.Parameters.AddWithValue("@Gnder", obj.Gender);
                command.Parameters.AddWithValue("@Avtr", obj.AvatarUrl);
                command.Parameters.AddWithValue("@AvtrH", obj.AvatarhUrl);
                command.Parameters.AddWithValue("@Avtrs", obj.AvatarsUrl);
                command.Parameters.AddWithValue("@AvtrT", obj.AvatartUrl);
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] {
                                 new DataColumn("LanguageId", typeof(string)),
                                 new DataColumn("IsSpoken",typeof(bool)),
                                     new DataColumn("OtherLanguage",typeof(string))
                    });
                foreach (PrimaryLanguages lang in obj.LangList)
                {
                    if (lang.LanguageId != 0 && lang.IsSpoken)
                    {
                        dt.Rows.Add(lang.LanguageId, lang.IsSpoken, lang.OtherLanguage);

                    }
                }
                command.Parameters.Add(new SqlParameter("@PrimaryLanguage", dt));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@ProgramYear", obj.ProgramYear));
                command.CommandText = "SP_Staff_Personalinfo";
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Close();
                command.Dispose();
                command.Parameters.Clear();
            }
            return result;
        }
        public List<string> getEmail(string id)
        {
            List<string> staffdetail = new List<string>();
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@id", id));
                command.CommandText = "SP_get_email";
                dataReader = command.ExecuteReader();
                if (dataReader.Read() && dataReader.HasRows)
                {
                    staffdetail.Add(dataReader.GetValue(0).ToString());
                    staffdetail.Add(dataReader.GetValue(1).ToString());
                    staffdetail.Add(dataReader.GetValue(2).ToString());
                    staffdetail.Add(dataReader.GetValue(3).ToString());
                    staffdetail.Add(dataReader.GetValue(4).ToString());
                    staffdetail.Add(dataReader.GetValue(5).ToString());
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                dataReader.Close();
            }
            finally
            {
                Connection.Close();
                command.Dispose();
                dataReader.Close();
            }
            return staffdetail;
        }
        public List<AgencyStaff> getagencystaffList(out string totalrecord, string agencyId, string sortOrder, string sortDirection, string search, int skip, int pageSize, string status, bool isEndOfYear = false)
        {
            List<AgencyStaff> staffList = new List<AgencyStaff>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@agencyId", agencyId));
                if (status.Contains("0") || status.Contains("1"))
                    command.Parameters.Add(new SqlParameter("@status", DBNull.Value));
                if (status.Contains("2"))
                    command.Parameters.Add(new SqlParameter("@status", "1"));
                if (status.Contains("3"))
                    command.Parameters.Add(new SqlParameter("@status", "0"));
                if (status.Contains("4"))
                    command.Parameters.Add(new SqlParameter("@status", "2"));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_staff_list";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        AgencyStaff staff = new AgencyStaff();
                        staff.CreatedBy = Convert.ToString(agencydataTable.Rows[i]["UserId"]);
                        staff.FirstName = Convert.ToString(agencydataTable.Rows[i]["firstname"]);
                        staff.LastName = Convert.ToString(agencydataTable.Rows[i]["lastname"]);
                        staff.roleName = Convert.ToString(agencydataTable.Rows[i]["roleName"]);
                        staff.EmailAddress = Convert.ToString(agencydataTable.Rows[i]["EmailAddress"]);
                        staff.ISActive = Convert.ToChar(agencydataTable.Rows[i]["status"]);
                        staff.AgencyStaffId = Guid.Parse(Convert.ToString(agencydataTable.Rows[i]["Id"]));
                        staff.createdDate = Convert.ToString(agencydataTable.Rows[i]["DateEntered"]);
                        staff.ProgramYear = Convert.ToString(agencydataTable.Rows[i]["ActiveProgramYear"]);
                        staffList.Add(staff);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return staffList;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return staffList;
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
        public string updateagencystaff(Guid id, int mode, Guid userId, bool isEndOfYear)
        {
            string result = string.Empty;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Update_Agency_staff";
                command.Parameters.Add(new SqlParameter("@staffId", id));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@userid", userId));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.ExecuteNonQuery();
                result = command.Parameters["@result"].Value.ToString();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;
        }
        public List<AgencyStaff> AutoCompleteAgencystaffList(string term, string agencyid, string active = "0")
        {
            List<AgencyStaff> AgencyList = new List<AgencyStaff>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "AutoComplete_AgencystaffList";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", term);
                        command.Parameters.AddWithValue("@Active", active);
                        command.Parameters.AddWithValue("@agencyid", agencyid);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            AgencyStaff obj = new AgencyStaff();
                            obj.AgencyStaffId = Guid.Parse(dr["Id"].ToString());
                            obj.FirstName = dr["name"].ToString();
                            AgencyList.Add(obj);
                        }
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
            return AgencyList;
        }
        public Dictionary<string, string> GetagencyAdmindashboard(string agencyid)
        {
            Dictionary<string, string> superadmindashboard = null;
            try
            {
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "SP_agencyadmindashboard";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@agencyid", agencyid);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    superadmindashboard = new Dictionary<string, string>();
                    superadmindashboard.Add("totalstaff", dt.Rows[0]["totalstaff"].ToString());
                    superadmindashboard.Add("totalactivestaff", dt.Rows[0]["totalactivestaff"].ToString());
                    superadmindashboard.Add("totaldeactivestaff", dt.Rows[0]["totaldeactivestaff"].ToString());
                    superadmindashboard.Add("totalsuspendedstaff", dt.Rows[0]["totalsuspendedstaff"].ToString());
                    superadmindashboard.Add("pendingapproval", dt.Rows[0]["pendingapproval"].ToString());
                    superadmindashboard.Add("pendingverification", dt.Rows[0]["pendingverification"].ToString());
                    superadmindashboard.Add("Rejectedrequest", dt.Rows[0]["Rejectedrequest"].ToString());
                    superadmindashboard.Add("ShowEndOfYearNotification", dt.Rows[0]["ShowEndOfYearNotification"].ToString());

                    //superadmindashboard.Add("totalactiveagencyuser", dt.Rows[0]["totalactiveagencyuser"].ToString());
                    //superadmindashboard.Add("totaldeactiveagencyuser", dt.Rows[0]["totaldeactiveagencyuser"].ToString());
                    //superadmindashboard.Add("totalsuspendedagencyuser", dt.Rows[0]["totalsuspendedagencyuser"].ToString());

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return superadmindashboard;
        }
        public Agencyreport agencystaffreoprt(string agencyid)
        {
            Agencyreport agencyReport = new Agencyreport();
            try
            {
                DataSet dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        dt = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "SP_Staff_Report";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@agencyid", agencyid);
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                    }
                }
                if (dt.Tables[0].Rows.Count > 0)
                {
                    agencyReport.totalhdstarterlyhdstart = Convert.ToString(dt.Tables[0].Rows[0]["AssociatedProgramtotal"]);
                    agencyReport.totalcontracterhdstarterlyhdstart = Convert.ToString(dt.Tables[0].Rows[0]["Contractortotal"]);
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    List<Agencystaffreport> reportlist = new List<Agencystaffreport>();
                    for (int i = 0; i < dt.Tables[1].Rows.Count; i++)
                    {
                        Agencystaffreport list = new Agencystaffreport();
                        list.AssociatedProgram = Convert.ToString(dt.Tables[1].Rows[i]["AssociatedProgram"]);
                        list.Contractor = Convert.ToString(dt.Tables[1].Rows[i]["Contractor"]) == string.Empty ? "0" : Convert.ToString(dt.Tables[1].Rows[i]["Contractor"]);
                        list.totalAssociatedProgram = Convert.ToString(dt.Tables[1].Rows[i]["totalAssociatedProgram"]);
                        reportlist.Add(list);
                    }
                    if (reportlist.Count < 2)
                    {
                        string a1 = "0";
                        string a2 = "0";
                        if (reportlist[0].AssociatedProgram == "1")
                        {
                            a1 = "2";
                            a2 = "3";
                        }
                        if (reportlist[0].AssociatedProgram == "2")
                        {
                            a1 = "1";
                            a2 = "3";
                        }
                        if (reportlist[0].AssociatedProgram == "3")
                        {
                            a1 = "1";
                            a2 = "2";
                        }
                        Agencystaffreport list1 = new Agencystaffreport();
                        list1.AssociatedProgram = a1;
                        list1.Contractor = "0";
                        list1.totalAssociatedProgram = "0";
                        reportlist.Add(list1);
                        Agencystaffreport list2 = new Agencystaffreport();
                        list2.AssociatedProgram = a2;
                        list2.Contractor = "0";
                        list2.totalAssociatedProgram = "0";
                        reportlist.Add(list2);
                    }
                    if (reportlist.Count < 3)
                    {
                        string a1 = "0";
                        if (reportlist[0].AssociatedProgram == "1" && reportlist[1].AssociatedProgram == "2"
                             || reportlist[0].AssociatedProgram == "2" && reportlist[1].AssociatedProgram == "1")
                            a1 = "3";
                        if (reportlist[0].AssociatedProgram == "1" && reportlist[1].AssociatedProgram == "3"
                          || reportlist[0].AssociatedProgram == "3" && reportlist[1].AssociatedProgram == "1")
                            a1 = "2";
                        if (reportlist[0].AssociatedProgram == "2" && reportlist[1].AssociatedProgram == "3"
                          || reportlist[0].AssociatedProgram == "3" && reportlist[1].AssociatedProgram == "2")
                            a1 = "1";
                        Agencystaffreport list1 = new Agencystaffreport();
                        list1.AssociatedProgram = a1;
                        list1.Contractor = "0";
                        list1.totalAssociatedProgram = "0";
                        reportlist.Add(list1);
                    }
                    agencyReport.Agencystaffreport = reportlist.OrderBy(P => P.AssociatedProgram).ToList();
                }
                else
                {
                    List<Agencystaffreport> reportlist = new List<Agencystaffreport>();
                    for (int i = 0; i < 3; i++)
                    {
                        Agencystaffreport list = new Agencystaffreport();
                        list.AssociatedProgram = (i + 1).ToString();
                        list.Contractor = "0";
                        list.totalAssociatedProgram = "0";
                        reportlist.Add(list);
                    }
                    agencyReport.Agencystaffreport = reportlist;

                }
                if (dt.Tables[2].Rows.Count > 0)
                {
                    agencyReport.hiredate = Convert.ToString(dt.Tables[2].Rows[0]["hiredate"]);
                    agencyReport.Contractortotalhired = Convert.ToString(dt.Tables[2].Rows[0]["Contractortotalhired"]);
                }
                if (dt.Tables[3].Rows.Count > 0)
                {
                    agencyReport.Contractortotalterminated = Convert.ToString(dt.Tables[3].Rows[0]["Contractortotalterminated"]);
                    agencyReport.terminationdate = Convert.ToString(dt.Tables[3].Rows[0]["TermDate"]);
                }
                if (dt.Tables[4].Rows.Count > 0)
                {
                    agencyReport.totalreplaced = Convert.ToString(dt.Tables[4].Rows[0]["replacement"]);
                    agencyReport.totalreplacedcontrator = Convert.ToString(dt.Tables[4].Rows[0]["Contractortotal"]);
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return agencyReport;
        }
        public List<Enrolementcode> enrollmentcode(out string totalrecord, ref string programYear, string sortOrder, string sortDirection, string search, int skip, int pageSize, string agencyId, bool isEndYear)
        {
            List<Enrolementcode> _Enrolementcode = new List<Enrolementcode>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyId));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@ProgramYear", SqlDbType.NVarChar, 7)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndYear));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_enrolementcodelist";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        Enrolementcode Enrolementcode = new Enrolementcode();
                        Enrolementcode.AuthorisationCode = Convert.ToString(agencydataTable.Rows[i]["AuthorisationCode"]);
                        Enrolementcode.Description = Convert.ToString(agencydataTable.Rows[i]["Description"]);
                        Enrolementcode.DateEntered = Convert.ToString(agencydataTable.Rows[i]["DateEntered"]);
                        Enrolementcode.ValidUpto = Convert.ToString(agencydataTable.Rows[i]["ValidUpto"]);
                        Enrolementcode.status = Convert.ToString(agencydataTable.Rows[i]["status"]);
                        _Enrolementcode.Add(Enrolementcode);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();

                }

                programYear = command.Parameters["@ProgramYear"].Value.ToString();

                return _Enrolementcode;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _Enrolementcode;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public List<PendingApproval> getrejectedList(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string agencyid)
        {
            List<PendingApproval> _Pendinglist = new List<PendingApproval>();
            try
            {
                totalrecord = string.Empty;
                string searchAgency = string.Empty;
                if (string.IsNullOrEmpty(search.Trim()))
                    searchAgency = string.Empty;
                else
                    searchAgency = search;
                command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                command.Parameters.Add(new SqlParameter("@take", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_rejected_list";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < agencydataTable.Rows.Count; i++)
                    {
                        PendingApproval PendingApproval = new PendingApproval();
                        PendingApproval.Id = Convert.ToString(agencydataTable.Rows[i]["Id"]);
                        PendingApproval.Email = Convert.ToString(agencydataTable.Rows[i]["Email"]);
                        PendingApproval.EnrollmentCode = Convert.ToString(agencydataTable.Rows[i]["EnrollmentCode"]);
                        PendingApproval.MobileNo = Convert.ToString(agencydataTable.Rows[i]["MobileNo"]);
                        PendingApproval.name = Convert.ToString(agencydataTable.Rows[i]["name"]);
                        PendingApproval.DateEntered = Convert.ToDateTime(agencydataTable.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");
                        if (string.IsNullOrEmpty(Convert.ToString(agencydataTable.Rows[i]["datemodified"])))
                            PendingApproval.datemodified = string.Empty;
                        else
                            PendingApproval.datemodified = Convert.ToDateTime(agencydataTable.Rows[i]["datemodified"]).ToString("MM/dd/yyyy");
                        _Pendinglist.Add(PendingApproval);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _Pendinglist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _Pendinglist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public DateTime getexpirytime(string AuthorisationCode)
        {
            DateTime expirytime = new DateTime();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@AuthorisationCode", AuthorisationCode));
                command.CommandText = "select ValidUpto from EnrollmentGenerationInfo where  AuthorisationCode=@AuthorisationCode";
                dataReader = command.ExecuteReader();
                if (dataReader.Read() && dataReader.HasRows)
                {
                    if (!dataReader.IsDBNull(0))
                        expirytime = Convert.ToDateTime(dataReader.GetValue(0));
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return expirytime;
        }
        public Dictionary<string, string> getuserroleagencyname(string staffid)
        {
            Dictionary<string, string> userinfo = new Dictionary<string, string>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@staffid", staffid));
                command.CommandText = "SP_getagencyroleusername";
                dataReader = command.ExecuteReader();
                if (dataReader.Read() && dataReader.HasRows)
                {
                    if (!dataReader.IsDBNull(0))
                        userinfo.Add("agencyname", Convert.ToString(dataReader.GetValue(0)));
                    if (!dataReader.IsDBNull(1))
                        userinfo.Add("Rolename", Convert.ToString(dataReader.GetValue(1)));
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                dataReader.Close();
                Connection.Close();
            }
            return userinfo;
        }
        public AgencyStaff GetData_AllDropdownforstaff(string id)
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            AgencyStaff _staff = new AgencyStaff();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_AgencyUser_Dropdowndata";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }

                try
                {
                    List<Agency> listAgency = new List<Agency>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Agency obj = new Agency();
                        obj.agencyId = Convert.ToString(dr["agencyId"].ToString());
                        obj.agencyName = dr["agencyName"].ToString();
                        listAgency.Add(obj);
                    }
                    listAgency.Insert(0, new Agency() { agencyId = "0", agencyName = "Select" });
                    _staff.agncylist = listAgency;
                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }
                //  }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    try
                    {
                        List<Role> _rolelist = new List<Role>();
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            Role obj = new Role();
                            obj.RoleId = dr["roleid"].ToString();
                            obj.RoleName = dr["roleName"].ToString();
                            _rolelist.Add(obj);
                        }
                        _rolelist.Insert(0, new Role() { RoleId = "0", RoleName = "Select" });
                        _staff.rolelist = _rolelist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    try
                    {
                        List<RaceInfo> _racelist = new List<RaceInfo>();
                        //_staff.myList
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            RaceInfo obj = new RaceInfo();
                            obj.RaceId = dr["Id"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _racelist.Add(obj);
                        }
                        _racelist.Insert(0, new RaceInfo() { RaceId = "0", Name = "Select" });
                        _staff.raceList = _racelist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    try
                    {
                        List<NationalityInfo> _nationlist = new List<NationalityInfo>();
                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            NationalityInfo obj = new NationalityInfo();
                            obj.NationId = dr["Id"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _nationlist.Add(obj);
                        }
                        // _nationlist.Insert(0, new NationalityInfo() { NationId = "0", Name = "Select" });
                        _staff.nationList = _nationlist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    try
                    {
                        List<EducationLevelIno> _EducationLevel = new List<EducationLevelIno>();
                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            EducationLevelIno obj = new EducationLevelIno();
                            obj.EducationLevel = dr["EducationLevel"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _EducationLevel.Add(obj);
                        }
                        _EducationLevel.Insert(0, new EducationLevelIno() { EducationLevel = "-1", Name = "Select" });
                        _staff.EducationLevelList = _EducationLevel;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }

                if (ds.Tables[5].Rows.Count > 0)
                {

                    List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                    foreach (DataRow dr in ds.Tables[5].Rows)
                    {
                        HrCenterInfo obj = new HrCenterInfo();
                        obj.CenterId = dr["Id"].ToString();
                        obj.Name = dr["CenterName"].ToString();
                        centerList.Add(obj);
                    }
                    centerList.Insert(0, new HrCenterInfo() { CenterId = "0", Name = "Select" });
                    _staff.HrcenterList = centerList;
                }
                if (ds.Tables[6].Rows.Count > 0)
                {

                    List<PirInfo> PirList = new List<PirInfo>();
                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        PirInfo obj = new PirInfo();
                        obj.PirId = dr["Id"].ToString();
                        obj.Name = dr["PirRole"].ToString();
                        PirList.Add(obj);
                    }
                    PirList.Insert(0, new PirInfo() { PirId = "0", Name = "Select" });
                    _staff.Pirlist = PirList;
                }
                GetAgencyStaffDetailnew(id, _staff);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            //  _agencyStafflist.Add(_staff);
            return _staff;
        }
        public AgencyStaff GetAgencyStaffDetailnew(string id, AgencyStaff obj)
        {
            //AgencyStaff obj = new AgencyStaff();
            try
            {
                DataSet ds = null;
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Staffpersonalinfo";
                        command.Parameters.AddWithValue("@Id", id);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
                #region StaffDetailSection
                if (dt.Rows.Count > 0)
                {
                    obj.AgencyStaffId = Guid.Parse(dt.Rows[0]["Id"].ToString());
                    obj.FirstName = Convert.ToString(dt.Rows[0]["FirstName"].ToString());
                    obj.LastName = Convert.ToString(dt.Rows[0]["LastName"].ToString());
                    obj.CellNumber = dt.Rows[0]["mobileno"].ToString();
                    obj.EmailAddress = Convert.ToString(dt.Rows[0]["Email"].ToString());
                    obj.Race = Convert.ToString(dt.Rows[0]["Race"].ToString());
                    obj.Natinality = Convert.ToString(dt.Rows[0]["Nationality"].ToString());
                    obj.EarlyChildHood = Convert.ToString(dt.Rows[0]["EarlyChildHood"].ToString());
                    obj.HighestEducation = Convert.ToString(dt.Rows[0]["HighestEducation"].ToString());
                    obj.GettingDegree = Convert.ToString(dt.Rows[0]["GettingDegree"].ToString());
                    obj.DOB = dt.Rows[0]["DOB"].ToString() == string.Empty ? "" : Convert.ToDateTime(dt.Rows[0]["DOB"]).ToString("MM/dd/yyyy");
                    obj.Gender = Convert.ToString(dt.Rows[0]["Gender"].ToString());
                    obj.AvatarUrl = Convert.ToString(dt.Rows[0]["Avatar"].ToString());
                    obj.AvatarhUrl = Convert.ToString(dt.Rows[0]["AvatarH"].ToString());
                    obj.AvatarsUrl = Convert.ToString(dt.Rows[0]["AvatarS"].ToString());
                    obj.AvatartUrl = Convert.ToString(dt.Rows[0]["AvatarT"].ToString());
                }
                #endregion

                DataTable dtHr = new DataTable();
                dtHr = ds.Tables[1];
                List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                for (int i = 0; i < dtHr.Rows.Count; i++)
                {
                    HrCenterInfo info = new HrCenterInfo();
                    info.CenterId = Convert.ToString(dtHr.Rows[i]["Id"].ToString());
                    info.Name = Convert.ToString(dtHr.Rows[i]["CenterName"].ToString());
                    centerList.Add(info);
                }
                centerList.Insert(0, new HrCenterInfo() { CenterId = "0", Name = "Select" });
                obj.HrcenterList = centerList;

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return obj;

        }
        public AgencyStaff GetUserRequestDropdown(string agencyid, int i = 0, Guid id = new Guid(), AgencyStaff staff = null)
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            AgencyStaff _staff = new AgencyStaff();
            PrimaryLanguages language = new PrimaryLanguages();
            _staff.ProgramYearList = new List<SelectListItem>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.Parameters.Clear();
                        command.CommandText = "Sp_Sel_AgencyUser_Dropdowndata";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@agencyID", agencyid));
                        command.Parameters.Add(new SqlParameter("@AgencyStaffId", id));
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }
                if (staff != null)
                {
                    _staff = staff;
                }
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                try
                {
                    List<Agency> listAgency = new List<Agency>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Agency obj = new Agency();
                        obj.agencyId = Convert.ToString(dr["agencyId"].ToString());
                        obj.agencyName = dr["agencyName"].ToString();
                        listAgency.Add(obj);
                    }
                    //listAgency.Insert(0, new Agency() { agencyId = "0", agencyName = "Select" });
                    _staff.agncylist = listAgency;
                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }
                //  }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    try
                    {
                        List<Role> _rolelist = new List<Role>();
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            Role obj = new Role();
                            obj.RoleId = dr["roleid"].ToString();
                            obj.RoleName = dr["roleName"].ToString();
                            _rolelist.Add(obj);
                        }
                        //_rolelist.Insert(0, new Role() { RoleId = "0", RoleName = "Select" });
                        _staff.rolelist = _rolelist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    try
                    {
                        List<RaceInfo> _racelist = new List<RaceInfo>();
                        //_staff.myList
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            RaceInfo obj = new RaceInfo();
                            obj.RaceId = dr["Id"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _racelist.Add(obj);
                        }
                        //_racelist.Insert(0, new RaceInfo() { RaceId = "0", Name = "Select" });
                        _staff.raceList = _racelist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    try
                    {
                        List<NationalityInfo> _nationlist = new List<NationalityInfo>();
                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            NationalityInfo obj = new NationalityInfo();
                            obj.NationId = dr["Id"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _nationlist.Add(obj);
                        }
                        // _nationlist.Insert(0, new NationalityInfo() { NationId = "0", Name = "Select" });
                        _staff.nationList = _nationlist;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    try
                    {
                        List<EducationLevelIno> _EducationLevel = new List<EducationLevelIno>();
                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            EducationLevelIno obj = new EducationLevelIno();
                            obj.EducationLevel = dr["EducationLevel"].ToString();
                            obj.Name = dr["Name"].ToString();
                            _EducationLevel.Add(obj);
                        }
                        //_EducationLevel.Insert(0, new EducationLevelIno() { EducationLevel = "-1", Name = "Select" });
                        _staff.EducationLevelList = _EducationLevel;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }

                if (ds.Tables[5].Rows.Count > 0)
                {

                    List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                    foreach (DataRow dr in ds.Tables[5].Rows)
                    {
                        HrCenterInfo obj = new HrCenterInfo();
                        obj.CenterId = dr["CenterId"].ToString();
                        obj.Name = dr["CenterName"].ToString();
                        obj.Homebased = Convert.ToBoolean(dr["HomeBased"]);
                        centerList.Add(obj);
                    }
                    //centerList.Insert(0, new HrCenterInfo() { CenterId = "0", Name = "Select" });
                    _staff.HrcenterList = centerList;
                }
                if (ds.Tables[6].Rows.Count > 0)
                {

                    List<PirInfo> PirList = new List<PirInfo>();
                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        PirInfo obj = new PirInfo();
                        obj.PirId = dr["Id"].ToString();
                        obj.Name = dr["PirRole"].ToString();
                        PirList.Add(obj);
                    }
                    //PirList.Insert(0, new PirInfo() { PirId = "0", Name = "Select" });
                    _staff.Pirlist = PirList;
                }
                if (ds.Tables[7].Rows.Count > 0)
                {

                    List<TimeZoneinfo> TimeZonelist = new List<TimeZoneinfo>();
                    foreach (DataRow dr in ds.Tables[7].Rows)
                    {
                        TimeZoneinfo obj = new TimeZoneinfo();
                        obj.TimZoneId = dr["TimeZone_ID"].ToString();
                        obj.TimZoneName = dr["TIMEZONENAME"].ToString();
                        TimeZonelist.Add(obj);
                    }
                    //TimeZonelist.Insert(0, new TimeZoneinfo() { TimZoneId = "0", TimZoneName = "Select" });
                    _staff.TimeZonelist = TimeZonelist;
                }
                if (ds.Tables[12].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[12].Rows)
                    {
                        language = new PrimaryLanguages();
                        language.LanguageId = Convert.ToInt32(dr["LanguageID"]);
                        language.LanguageName = Convert.ToString(dr["PrimaryLanguage"]);
                        language.OtherLanguage = Convert.ToString(dr["OtherLanguage"]);
                        language.IsSpoken = ((dr["IsSpoken"]) == DBNull.Value) ? false : true;
                        _staff.LangList.Add(language);
                    }

                }

                if (ds.Tables[13] != null && ds.Tables[13].Rows.Count > 0)
                {
                    _staff.ProgramYearList = (from DataRow dr13 in ds.Tables[13].Rows
                                              select new SelectListItem
                                              {

                                                  Text = Convert.ToString(dr13["ActiveProgramYear"]),
                                                  Value = Convert.ToString(dr13["ActiveProgramYear"])
                                              }
                                             ).ToList();
                }

                if (i == 1)
                {
                    GetAgencyuserpendingapprovaldetail(id, _staff);
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            //  _agencyStafflist.Add(_staff);
            return _staff;
        }
        public AgencyStaff GetAgencyuserpendingapprovaldetail(Guid id, AgencyStaff obj)
        {
            try
            {
                DataSet ds = null;
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "SP_get_agencysuerdetails";
                        command.Parameters.AddWithValue("@id", id);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                        dt = ds.Tables[0];
                    }
                }
                #region StaffDetailSection
                if (dt.Rows.Count > 0)
                {
                    obj.FirstName = Convert.ToString(dt.Rows[0]["FirstName"].ToString());
                    obj.LastName = Convert.ToString(dt.Rows[0]["LastName"].ToString());
                    obj.CellNumber = dt.Rows[0]["MobileNo"].ToString();
                    obj.EmailAddress = Convert.ToString(dt.Rows[0]["Email"].ToString());
                    obj.Race = Convert.ToString(dt.Rows[0]["Race"].ToString());
                    obj.Natinality = Convert.ToString(dt.Rows[0]["Nationality"].ToString());
                    obj.DOB = dt.Rows[0]["DOB"].ToString() == string.Empty ? string.Empty : Convert.ToDateTime(dt.Rows[0]["DOB"]).ToString("dd/MM/yyyy");
                    obj.AvatarUrl = Convert.ToString(dt.Rows[0]["Avatar"].ToString());
                    obj.EarlyChildHood = Convert.ToString(dt.Rows[0]["EarlyChildHood"].ToString());
                    obj.GettingDegree = Convert.ToString(dt.Rows[0]["GettingDegree"].ToString());
                    obj.AvatarhUrl = Convert.ToString(dt.Rows[0]["AvatarH"].ToString());
                    obj.AvatarsUrl = Convert.ToString(dt.Rows[0]["AvatarS"].ToString());
                    obj.AvatartUrl = Convert.ToString(dt.Rows[0]["AvatarT"].ToString());
                    obj.Gender = Convert.ToString(dt.Rows[0]["Gender"].ToString());
                    obj.HighestEducation = Convert.ToString(dt.Rows[0]["HighestEducation"].ToString());
                }
                #endregion

                //DataTable dtHr = new DataTable();
                //dtHr = ds.Tables[1];
                //List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                //for (int i = 0; i < dtHr.Rows.Count; i++)
                //{
                //    HrCenterInfo info = new HrCenterInfo();
                //    info.CenterId = Convert.ToString(dtHr.Rows[i]["Id"].ToString());
                //    info.Name = Convert.ToString(dtHr.Rows[i]["CenterName"].ToString());
                //    centerList.Add(info);
                //}
                //centerList.Insert(0, new HrCenterInfo() { CenterId = "0", Name = "Select" });
                //obj.HrcenterList = centerList;

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return obj;

        }
        public List<Agency.ProgramType> ProgDetails(string FundId, string Agencyid)
        {
            List<Agency.ProgramType> _progType = new List<Agency.ProgramType>();
            try
            {
                command.Parameters.Add(new SqlParameter("@FundId", FundId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_proglist";
                DataAdapter = new SqlDataAdapter(command);
                agencydataTable = new DataTable();
                DataAdapter.Fill(agencydataTable);
                if (agencydataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in agencydataTable.Rows)
                    {
                        Agency.ProgramType _progadd = new Agency.ProgramType();
                        _progadd.ProgramID = Convert.ToInt32(row["ProgramTypeID"]);
                        _progadd.ProgramTypes = row["ProgramType"].ToString();
                        _progadd.Description = row["Description"].ToString();
                        _progadd.Slots = row["Slots"].ToString();
                        _progadd.PIRReport = (row["PIRReport"]).ToString() == "" ? false : Convert.ToBoolean(row["PIRReport"]);
                        _progadd.ReferenceProg = row["ReferenceProg"].ToString();
                        _progadd.MinAge = Convert.ToInt32(row["MinAge"]);
                        _progadd.MaxAge = Convert.ToInt32(row["MaxAge"]);
                        _progadd.programstartDate = row["programstartDate"].ToString();
                        _progadd.programendDate = row["programendDate"].ToString();
                        _progadd.FundID = Convert.ToString(row["FundId"]);
                        _progadd.Area = row["AreaID"].ToString();
                        _progType.Add(_progadd);
                    }
                }
                return _progType;
            }
            catch (Exception ex)
            {
                //  totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _progType;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                agencydataTable.Dispose();
            }
        }
        public List<ClassRoom> getclassroominfo(string centerId, string agencyid, string programYear)
        {
            List<ClassRoom> classList = new List<ClassRoom>();
            AgencyStaff _staff = new AgencyStaff();
            try
            {
                if (!string.IsNullOrEmpty(agencyid))
                    command.Parameters.Add(new SqlParameter("@Agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@centerId", centerId));
                command.Parameters.Add(new SqlParameter("@ProgramYear", programYear));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                // command.CommandText = "Sp_Sel_AgencyUser_Dropdowndata";
                command.CommandText = "SP_Getclassrooms";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                //if (_dataset.Tables[10] != null && _dataset.Tables[10].Rows.Count > 0)
                //{
                //    //  List<ClassRoom> classlist = new List<ClassRoom>();
                //    foreach (DataRow dr in _dataset.Tables[10].Rows)
                //    {
                //        ClassRoom obj = new ClassRoom();
                //        obj.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                //        obj.ClassName = dr["ClassroomName"].ToString();
                //        classList.Add(obj);
                //    }
                //    _staff.Classroom = classList;

                //}


                if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0)
                {
                    classList = (from DataRow dr0 in _dataset.Tables[0].Rows
                                 select new ClassRoom
                                 {
                                     ClassroomID = Convert.ToInt32(dr0["ClassRoomID"]),
                                     ClassName = Convert.ToString(dr0["ClassRoomName"])

                                 }
                               ).ToList();
                }

                DataAdapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return classList;
        }
        public List<UserInfo> getmanagerinfo(string RoleId, string agencyid)
        {
            List<UserInfo> userList = new List<UserInfo>();
            AgencyStaff _staff = new AgencyStaff();
            try
            {
                if (!String.IsNullOrWhiteSpace(agencyid))
                {
                    command.Parameters.AddWithValue("@Agencyid", agencyid);
                }
                //command.Parameters.Add(new SqlParameter("@Agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@RoleId", RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_AgencyUser_Dropdowndata";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[11] != null && _dataset.Tables[11].Rows.Count > 0)
                {
                    //  List<ClassRoom> classlist = new List<ClassRoom>();
                    foreach (DataRow dr in _dataset.Tables[11].Rows)
                    {
                        UserInfo obj = new UserInfo();
                        obj.userId = (dr["Id"]).ToString();
                        obj.Name = dr["Name"].ToString();
                        userList.Add(obj);
                    }
                    _staff.UserList = userList;

                }

                DataAdapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return userList;
        }
        public List<UserInfo> getusersinfo(string agencyid)
        {
            List<UserInfo> userList = new List<UserInfo>();
            AgencyStaff _staff = new AgencyStaff();
            try
            {
                if (!String.IsNullOrWhiteSpace(agencyid))
                {
                    command.Parameters.AddWithValue("@Agencyid", agencyid);
                }
                //command.Parameters.Add(new SqlParameter("@Agencyid", agencyid));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_AgencyUser_Dropdowndata";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[11] != null && _dataset.Tables[11].Rows.Count > 0)
                {
                    //  List<ClassRoom> classlist = new List<ClassRoom>();
                    foreach (DataRow dr in _dataset.Tables[11].Rows)
                    {
                        UserInfo obj = new UserInfo();
                        obj.userId = (dr["UserId"]).ToString();
                        obj.Name = dr["Name"].ToString();
                        userList.Add(obj);
                    }
                    _staff.UserList = userList;

                }

                DataAdapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return userList;
        }
        public List<HrCenterInfo> getagencyid(string agencyid, string Type)
        {
            List<HrCenterInfo> centerList = new List<HrCenterInfo>();
            try
            {

                command.Parameters.Add(new SqlParameter("@Agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@Type", Type));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getcenterclass";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        HrCenterInfo obj = new HrCenterInfo();
                        obj.CenterId = dr["CenterId"].ToString();
                        obj.Name = dr["CenterName"].ToString();
                        centerList.Add(obj);
                    }
                }

                DataAdapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return centerList;
        }
        public AgencyStaff getclassroomdetails(string centerId)
        {
            List<ClassRoom> classList = new List<ClassRoom>();
            List<UserInfo> userList = new List<UserInfo>();
            AgencyStaff _staff = new AgencyStaff();
            try
            {

                //command.Parameters.Add(new SqlParameter("@Agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@centerId", centerId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_Centerclassroom";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    //  List<ClassRoom> classlist = new List<ClassRoom>();
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        ClassRoom obj = new ClassRoom();
                        obj.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                        obj.ClassName = dr["ClassroomName"].ToString();
                        classList.Add(obj);
                    }
                    _staff.Classroom = classList;

                }
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    //  List<ClassRoom> classlist = new List<ClassRoom>();
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        UserInfo obj = new UserInfo();
                        obj.userId = (dr["UserId"]).ToString();
                        obj.Name = dr["Name"].ToString();
                        userList.Add(obj);
                    }
                    _staff.UserList = userList;

                }

                DataAdapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return _staff;
        }
        public string AssignClass(AgencyStaff info, int mode)
        {
            string result = string.Empty;
            try
            {
                SqlCommand commandAK = new SqlCommand();
                string agencyCode = string.Empty;
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                commandAK.Connection = Connection;
                tranSaction = Connection.BeginTransaction();
                command.Connection = Connection;
                command.Transaction = tranSaction;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_assignClass";
                command.Parameters.Add(new SqlParameter("@Classrooms", info.Classrooms));
                command.Parameters.Add(new SqlParameter("@ClassAssignID", info.ClassAssignID));//Changes
                command.Parameters.Add(new SqlParameter("@AgencyId", info.SelectedAgencyId));
                command.Parameters.Add(new SqlParameter("@centerlist", info.centerlist));
                command.Parameters.Add(new SqlParameter("@primaryEmail", info.Users));
                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                // command.Parameters.Add(new SqlParameter("@createdBy", userId));
                command.ExecuteNonQuery();
                tranSaction.Commit();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                if (tranSaction != null)
                    tranSaction.Rollback();
                clsError.WriteException(ex);
                return ex.Message;
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Close();
                Connection.Close();
                command.Dispose();
            }
        }
        public AgencyStaff getclassroomdetailsassign(string centerId, string Type, string Agencyid)
        {
            List<ClassRoom> classList = new List<ClassRoom>();
            List<UserInfo> userList = new List<UserInfo>();
            List<classAssign> userAssignList = new List<classAssign>();
            AgencyStaff _staff = new AgencyStaff();
            try
            {
                command.Parameters.Add(new SqlParameter("@centerId", centerId));
                command.Parameters.Add(new SqlParameter("@Type", Type));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_Sel_Centerclassroomassign";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        ClassRoom obj = new ClassRoom();
                        obj.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                        obj.ClassName = dr["ClassroomName"].ToString();
                        classList.Add(obj);
                    }
                    _staff.Classroom = classList;
                }
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        UserInfo obj = new UserInfo();
                        obj.userId = (dr["UserId"]).ToString();
                        obj.Name = dr["Name"].ToString();
                        userList.Add(obj);
                    }
                    _staff.UserList = userList;
                }
                if (_dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[2].Rows)
                    {
                        classAssign obj = new classAssign();
                        obj.ClassAssignID = Convert.ToInt32(dr["ID"]);
                        obj.ClassroomID = Convert.ToInt32(dr["ClassroomID"]);
                        obj.userId = dr["FSWUserId"].ToString();
                        userAssignList.Add(obj);
                    }
                    _staff.UserAssignList = userAssignList;
                }

                DataAdapter.Dispose();
                command.Dispose();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return _staff;
        }
        public List<CoreTeam> GetCoreTeam(string AgencyId, string UserId)
        {
            List<CoreTeam> _CoreTeamList = new List<CoreTeam>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetCoreTeam";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CoreTeam obj = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            obj = new CoreTeam();
                            obj.RoleId = dr["Roleid"].ToString();
                            obj.RoleName = dr["Rolename"].ToString();
                            obj.IsCoreTean = Convert.ToBoolean(dr["iscoreteam"]);
                            // obj.UserColor = string.IsNullOrEmpty(dr["UserColor"].ToString())?"#ffffff": dr["iscoreteam"].ToString();
                            obj.UserColor = dr["UserColor"].ToString();
                            _CoreTeamList.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _CoreTeamList;
        }

        public List<Demographic> GetDemographicSection(string AgencyId, string UserId)
        {
            List<Demographic> _DemographicList = new List<Demographic>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetDemographic";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Demographic obj = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            obj = new Demographic();
                            obj.RoleId = dr["Roleid"].ToString();
                            obj.RoleName = dr["Rolename"].ToString();
                            obj.IsDemographic = Convert.ToBoolean(dr["IsDemographic"]);
                            // obj.UserColor = string.IsNullOrEmpty(dr["UserColor"].ToString())?"#ffffff": dr["iscoreteam"].ToString();
                            // obj.UserColor = dr["UserColor"].ToString();
                            _DemographicList.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _DemographicList;
        }

        public List<AcceptanceRole> GetAcceptanceProcess(string AgencyId, bool isEndOfYear = false)
        {
            List<AcceptanceRole> _AcceptanceProcessList = new List<AcceptanceRole>();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetAcceptanceProcess";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        AcceptanceRole obj = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            obj = new AcceptanceRole();
                            obj.RoleID = new Guid(dr["Roleid"].ToString());
                            obj.isAllowIncome = Convert.ToBoolean(dr["isAllowIncome"]);
                            obj.Priority = Convert.ToInt32(dr["PriorityLevel"]);
                            obj.ActiveProgramYear = Convert.ToString(dr["ActiveProgramYear"]);
                            _AcceptanceProcessList.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _AcceptanceProcessList;
        }
        public List<CoreTeam> SaveCoreTeam(ref string message, List<CoreTeam> CoreTeams, string AgencyId, string UserId)
        {
            List<CoreTeam> _CoreTeamList = new List<CoreTeam>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                //command.Parameters.Add(new SqlParameter("@UserColor", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("RoleId", typeof(string)),
                      new DataColumn("IsCoreTeam",typeof(bool)),
                        new DataColumn("UserColor",typeof(string))

                    });
                foreach (CoreTeam Team in CoreTeams)
                {
                    if (Team.RoleId != null && Team.IsCoreTean)
                    {
                        dt.Rows.Add(Team.RoleId, Team.IsCoreTean, Team.UserColor);

                    }
                }
                command.Parameters.Add(new SqlParameter("@CoreTeams", dt));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveCoreTeam";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                message = command.Parameters["@result"].Value.ToString();
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        CoreTeam obj = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            obj = new CoreTeam();
                            obj.RoleId = dr["Roleid"].ToString();
                            obj.RoleName = dr["Rolename"].ToString();
                            obj.IsCoreTean = Convert.ToBoolean(dr["iscoreteam"]);
                            obj.UserColor = dr["UserColor"].ToString();
                            _CoreTeamList.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _CoreTeamList;
        }

        public List<Demographic> SaveDemographic(ref string message, List<Demographic> Demographics, string AgencyId, string UserId)
        {
            List<Demographic> _DemographicList = new List<Demographic>();
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                //command.Parameters.Add(new SqlParameter("@UserColor", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("RoleId", typeof(string)),
                      new DataColumn("IsDemographic",typeof(bool))


                    });
                foreach (Demographic Team in Demographics)
                {
                    if (Team.RoleId != null && Team.IsDemographic)
                    {
                        dt.Rows.Add(Team.RoleId, Team.IsDemographic);

                    }
                }
                command.Parameters.Add(new SqlParameter("@Demographics", dt));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveDemographic";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                message = command.Parameters["@result"].Value.ToString();
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Demographic obj = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            obj = new Demographic();
                            obj.RoleId = dr["Roleid"].ToString();
                            obj.RoleName = dr["Rolename"].ToString();
                            obj.IsDemographic = Convert.ToBoolean(dr["IsDemographic"]);
                            obj.UserColor = dr["UserColor"].ToString();
                            _DemographicList.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _DemographicList;
        }
        public List<AcceptanceProcess> SaveAcceptanceProcess(ref string message, List<AcceptanceProcess> AcceptanceProcess, string AgencyId, string UserId)
        {
            List<AcceptanceProcess> _AcceptanceProcessList = new List<AcceptanceProcess>();
            string result = "";
            try
            {
                command.Parameters.Add(new SqlParameter("@Agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                //command.Parameters.Add(new SqlParameter("@UserColor", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2] {
                    new DataColumn("RoleId", typeof(string)),
                      new DataColumn("IsAcceptance",typeof(bool)),


                    });
                foreach (AcceptanceProcess Team in AcceptanceProcess)
                {
                    if (Team.RoleId != null && Team.IsAcceptance)
                    {
                        dt.Rows.Add(Team.RoleId, Team.IsAcceptance);

                    }
                }
                command.Parameters.Add(new SqlParameter("@AcceptanceProcesss", dt));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveAcceptanceRole";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                message = command.Parameters["@result"].Value.ToString();
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        AcceptanceProcess obj = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            obj = new AcceptanceProcess();
                            obj.RoleId = dr["Roleid"].ToString();
                            obj.RoleName = dr["Rolename"].ToString();
                            obj.IsAcceptance = Convert.ToBoolean(dr["IsAcceptance"]);
                            //  obj.UserColor = dr["UserColor"].ToString();
                            _AcceptanceProcessList.Add(obj);
                        }
                    }
                }
                result = command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return _AcceptanceProcessList;
        }

        public AgencySlots GetRefProgram(string Agencyid, bool isEndOfYear)
        {
            AgencySlots agencySlot = new AgencySlots();
            List<SelectListItem> prog = new List<SelectListItem>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_GetRefProgramandcenters";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Clear();
                        command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                        command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            prog.Add(new SelectListItem() { Text = dr["ProgramType"].ToString(), Value = dr["ProgramTypeID"].ToString() });

                        }
                        agencySlot.RefProgram = prog;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    agencySlot._CenterTable = ds.Tables[1];
                }
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        agencySlot.SlotPurchased = dr["SlotNumber"].ToString();
                        agencySlot.SlotAllocated = dr["SlotAllocatted"].ToString();
                        agencySlot.MenuEnabled = Convert.ToBoolean(dr["MenuEnabled"]);
                    }
                }

                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                {
                    agencySlot._Centerprogram = ds.Tables[3];
                }

                if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
                {
                    agencySlot.ProgramYear = Convert.ToString(ds.Tables[4].Rows[0]["ActiveProgramYear"]);
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return agencySlot;
        }
        public List<ClassRoom> GetSlot(ref string Slots, string ProgramId, string Agencyid)
        {
            List<ClassRoom> classlist = new List<ClassRoom>();
            try
            {
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetSlot";
                command.Parameters.Add(new SqlParameter("@ProgramId", ProgramId));
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Slots = dr["SlotNumber"].ToString();
                    }


                }


                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    try
                    {
                        ClassRoom _class = null;
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            _class = new ClassRoom();
                            _class.ClassroomID = Convert.ToInt32(dr["Classroom"]);
                            _class.ActualSeats = Convert.ToInt32(dr["seats"]);
                            classlist.Add(_class);
                        }

                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }

                return classlist;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return classlist;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }
        public string AddSlots(ref AgencySlots Slot, List<ClassRoom> ClassSlot, string UserId, string agencyid, bool isEndOfYear = false)
        {
            string result = string.Empty;
            List<SelectListItem> prog = new List<SelectListItem>();
            try
            {
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                if (ClassSlot != null && ClassSlot.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("ProgramId", typeof(int)),
                    new DataColumn("ClassroomID", typeof(int)),
                    new DataColumn("MaxCapacity",typeof(int))
                    });
                    foreach (ClassRoom slot in ClassSlot)
                    {
                        if (slot.ClassroomID != 0)
                        {
                            dt.Rows.Add(slot.ProgramId, slot.ClassroomID, slot.ActualSeats);
                        }
                    }
                    command.Parameters.Add(new SqlParameter("@tblSlots", dt));

                }
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Add_Slots";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            prog.Add(new SelectListItem() { Text = dr["ProgramType"].ToString(), Value = dr["ProgramTypeID"].ToString() });

                        }
                        Slot.RefProgram = prog;
                    }
                    catch (Exception ex)
                    {
                        clsError.WriteException(ex);
                    }
                }
                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                    Slot._CenterTable = _dataset.Tables[1];
                }
                if (_dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[2].Rows)
                    {
                        Slot.SlotPurchased = dr["SlotNumber"].ToString();
                        Slot.SlotAllocated = dr["SlotAllocatted"].ToString();
                        Slot.MenuEnabled = Convert.ToBoolean(dr["MenuEnabled"]);
                    }
                }
                if (_dataset.Tables[3] != null && _dataset.Tables[3].Rows.Count > 0)
                {
                    Slot._Centerprogram = _dataset.Tables[3];
                }

                if (_dataset.Tables[4] != null && _dataset.Tables[4].Rows.Count > 0)
                {
                    Slot.ProgramYear = Convert.ToString(_dataset.Tables[4].Rows[0]["ActiveProgramYear"]);
                }
                result = command.Parameters["@result"].Value.ToString();



            }
            catch (Exception ex)
            {

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
        public string CheckProgram(string Agencyid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_CheckProgramtype";
                command.Parameters.Add(new SqlParameter("@Agencyid", Agencyid));
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@result", string.Empty));
                command.Parameters["@result"].Direction = ParameterDirection.Output;
                command.Parameters["@result"].Size = 10;
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
                if (Connection != null)
                    Connection.Close();
                command.Dispose();

            }
        }
        public string CheckForcenters(string Agencyid, string userid)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_CheckCenters";
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
                if (Connection != null)
                    Connection.Close();
                command.Dispose();

            }
        }
        public List<HomeBased> HomeBasedsocialization(string userid, string agencyid)
        {
            HomeBased _roster = new HomeBased();
            List<HomeBased> HomeBasedList = new List<HomeBased>();
            try
            {

                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Parameters.Add(new SqlParameter("@agencyid", agencyid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Homebasedsocialization";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);

                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        HomeBased info = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            info = new HomeBased();
                            info.ClientID = dr["Clientid"].ToString();
                            info.ClientName = dr["name"].ToString();
                            info.Absent = false;
                            info.Present = false;
                            HomeBasedList.Add(info);
                        }

                    }
                }


            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return HomeBasedList;

        }


        public List<SelectListItem> GetUsersByRoleId(string roleId, string agencyId)
        {
            List<SelectListItem> usersList = new List<SelectListItem>();
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetUsersByRoleId";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", agencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", roleId));
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        usersList = (from DataRow dr in _dataset.Tables[0].Rows
                                     select new SelectListItem
                                     {
                                         Text = dr["StaffName"].ToString(),
                                         Value = dr["UserID"].ToString()
                                     }
                                   ).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                command.Dispose();
            }
            return usersList;
        }

        public AgencyAdditionalSlots GetAdditionalSlotDetails(string AgencyId, string UserId, string yakkrid)
        {
            AgencyAdditionalSlots slots = new AgencyAdditionalSlots();
            try
            {

                command.Parameters.Add(new SqlParameter("@userid", UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@yakkrid", yakkrid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetAddtionalSeatsDetails";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);

                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        slots.SlotsCount = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Slots"]);

                    }
                }
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return slots;

        }



        public bool SaveAdditionalSeats(string AgencyId, string UserId, List<AgencyAdditionalSlots> Seats, int YakkrID)
        {
            AgencyAdditionalSlots slots = new AgencyAdditionalSlots();
            bool result = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                DataTable dt = new DataTable();
                if (Seats != null && Seats.Count > 0)
                {

                    dt.Columns.AddRange(new DataColumn[4] {
                    new DataColumn("CenterId", typeof(int)),
                    new DataColumn("ClassId",typeof(int)),
                    new DataColumn("SeatCount",typeof(int)),
                    new DataColumn("ProgramType",typeof(string)),

                    });
                    foreach (AgencyAdditionalSlots seat in Seats)
                    {
                        if (seat != null)
                        {
                            dt.Rows.Add(seat.CenterId, seat.ClassroomId, seat.Seats, seat.ProgramType);
                        }
                    }
                    //command.Parameters.Add(new SqlParameter("@tblphone", dt));

                }
                command.Parameters.Add(new SqlParameter("@CreatedBy", UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@Yakkrid", YakkrID));
                command.Parameters.Add(new SqlParameter("@AdditionalSeats", dt));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_InsertAdditionalSeats";
                int isrowaffected = command.ExecuteNonQuery();
                if (isrowaffected > 0)
                    result = true;


            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return result;

        }

        public bool SaveAcceptancePrirorityRoles(string AgencyId, string UserId, List<AcceptanceRole> Roles, bool isEndOfYear = false)
        {
            AgencyAdditionalSlots slots = new AgencyAdditionalSlots();
            bool result = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.Parameters.Clear();
                DataTable dt = new DataTable();
                if (Roles != null && Roles.Count > 0)
                {

                    dt.Columns.AddRange(new DataColumn[4] {
                    new DataColumn("RoleId", typeof(Guid)),
                    new DataColumn("PriorityLevel",typeof(int)),
                      new DataColumn("IsAllowIncome",typeof(bool)),
                        new DataColumn("IsAccessAllCenter", typeof(bool))
                    });


                    foreach (AcceptanceRole role in Roles)
                    {
                        if (role != null)
                        {
                            if (((role.RoleID).ToString() == "a65bb7c2-e320-42a2-aed4-409a321c08a5") || ((role.RoleID).ToString() == "7c2422ba-7bd4-4278-99af-b694dcab7367") || ((role.RoleID).ToString() == "b65759ba-4813-4906-9a69-e180156e42fc") || ((role.RoleID).ToString() == "c352f959-cfd5-4902-a529-71de1f4824cc") ||
                            ((role.RoleID).ToString() == "2af7205e-87b4-4ca7-8ca8-95827c08564c") || ((role.RoleID).ToString() == "4b77aab6-eed1-4ac3-b498-f3e80cf129c0"))
                                dt.Rows.Add(role.RoleID, role.Priority, role.isAllowIncome, true);
                            else
                                dt.Rows.Add(role.RoleID, role.Priority, role.isAllowIncome, false);

                        }
                    }


                }
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Parameters.Add(new SqlParameter("@agencyid", AgencyId));
                command.Parameters.Add(new SqlParameter("@result", ""));
                command.Parameters.Add(new SqlParameter("@AcceptanceProcesss", dt));
                command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_SaveAcceptanceRole";
                int isrowaffected = command.ExecuteNonQuery();
                if (isrowaffected > 0)
                    result = true;


            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);

            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return result;

        }



        //public List<SelectListItem> GetFamiliesUnderUserId(string userId, string agencyId,string roleId)
        //{
        //    List<SelectListItem> usersList = new List<SelectListItem>();
        //    try
        //    {
        //        if (Connection.State == ConnectionState.Open)
        //            Connection.Close();
        //        Connection.Open();
        //        command.Connection = Connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "USP_GetFamiliesUnderUserId";
        //        command.Parameters.Clear();
        //        command.Parameters.Add(new SqlParameter("@Agencyid", agencyId));
        //        command.Parameters.Add(new SqlParameter("@UserId", userId));
        //        command.Parameters.Add(new SqlParameter("@RoleId", roleId));

        //        DataAdapter = new SqlDataAdapter(command);
        //        _dataset = new DataSet();
        //        DataAdapter.Fill(_dataset);
        //        if (_dataset != null)
        //        {
        //            if (_dataset.Tables[0].Rows.Count > 0)
        //            {
        //                usersList = (from DataRow dr in _dataset.Tables[0].Rows
        //                             select new SelectListItem
        //                             {
        //                                 Text = dr["clientFamily"].ToString(),
        //                                 Value = EncryptDecrypt.Encrypt64(dr["ClientId"].ToString())
        //                             }
        //                           ).ToList();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsError.WriteException(ex);

        //    }
        //    finally
        //    {
        //        if (Connection != null)
        //            Connection.Close();
        //        command.Dispose();
        //    }
        //    return usersList;
        //}

        /// <summary>
        /// method to get the List of  Roles to access the PIR Section.
        /// </summary>
        /// <returns></returns>
        public List<PIRAccessRoles> GetPIRAccessRoles()
        {
            List<PIRAccessRoles> pirAccessrolesList = new List<PIRAccessRoles>();
            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetPIRAccessRoles";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        pirAccessrolesList = _dataset.Tables[0].AsEnumerable().OrderBy(x => x.Field<string>("RoleName")).Select(x => new PIRAccessRoles
                        {
                            RoleId = x.Field<Guid>("RoleId"),
                            RoleName = x.Field<String>("RoleName"),
                            IsAccessPIR = x.Field<bool>("IsAccessPIR")
                        }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return pirAccessrolesList;
        }
        /// <summary>
        /// method to insert the allowed PIR Roles in the database.
        /// </summary>
        /// <param name="isRowsAffected"></param>
        /// <param name="accessRolesList"></param>
        /// <returns></returns>
        /// 
        public List<PIRAccessRoles> InsertPIRAccessRoles(out int isRowsAffected, List<PIRAccessRoles> accessRolesList)
        {
            List<PIRAccessRoles> pirAccessrolesList = new List<PIRAccessRoles>();

            isRowsAffected = 0;
            DataTable pirAccessdt = new DataTable();

            pirAccessdt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("PIRTeamId", typeof(int)),
                    new DataColumn("AgencyId",typeof(Guid)),
                    new DataColumn("RoleId",typeof(Guid)),
                    new DataColumn("IsShowPIR",typeof(bool)),
                    new DataColumn("Status",typeof(bool))

                    });


            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
                foreach (var item in accessRolesList)
                {
                    pirAccessdt.Rows.Add(0, staffDetails.AgencyId, item.RoleId, item.IsAccessPIR, (item.IsAccessPIR) ? true : false);
                }



                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                command.Parameters.Add(new SqlParameter("@PIRAccessRolesType", pirAccessdt));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertPIRAccessRoles";
                Connection.Open();
                isRowsAffected = command.ExecuteNonQuery();
                if (isRowsAffected > 0)
                {
                    pirAccessrolesList = this.GetPIRAccessRoles();
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return pirAccessrolesList;
        }


        /// <summary>
        /// method to get the List of  Roles to decide, who can review the family Income.
        /// </summary>
        /// <returns></returns>
        public List<IncomeReviewRoles> GetIncomeReviewRoles()
        {
            List<IncomeReviewRoles> incomeReviewRolesList = new List<IncomeReviewRoles>();
            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetIncomeReviewRoles";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        incomeReviewRolesList = _dataset.Tables[0].AsEnumerable().OrderBy(x => x.Field<string>("RoleName")).Select(x => new IncomeReviewRoles
                        {
                            RoleId = x.Field<Guid>("RoleId"),
                            RoleName = x.Field<String>("RoleName"),
                            IsReviewIncome = x.Field<bool>("IsReviewIncome")
                        }).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return incomeReviewRolesList;
        }

        /// <summary>
        /// method to insert the Roles, who can access the Family Income and returns the List<IncomeReviewRoles>
        /// </summary>
        /// <param name="rowsAffected"></param>
        /// <param name="incomeReviewRolesList"></param>
        /// <returns></returns>
        public List<IncomeReviewRoles> InsertIncomeReviewRoles(out int isRowsAffected, List<IncomeReviewRoles> incomeReviewRolesList)
        {
            List<IncomeReviewRoles> incomeReviewList = new List<IncomeReviewRoles>();

            isRowsAffected = 0;
            DataTable incomeReivewdt = new DataTable();

            incomeReivewdt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("IncomeReviewID", typeof(long)),
                    new DataColumn("AgencyID",typeof(Guid)),
                    new DataColumn("RoleID",typeof(Guid)),
                    new DataColumn("IsReviewIncome",typeof(bool)),
                    new DataColumn("Status",typeof(bool))

                    });

            try
            {
                StaffDetails staffDetails = StaffDetails.GetInstance();
                foreach (var item in incomeReviewRolesList)
                {
                    incomeReivewdt.Rows.Add(0, staffDetails.AgencyId, item.RoleId, item.IsReviewIncome, (item.IsReviewIncome) ? true : false);
                }

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                command.Connection = Connection;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                command.Parameters.Add(new SqlParameter("@IncomeReviewRolesType", incomeReivewdt));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertIncomeReviewRoles";
                Connection.Open();
                isRowsAffected = command.ExecuteNonQuery();
                if (isRowsAffected > 0)
                {
                    incomeReviewList = this.GetIncomeReviewRoles();
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return incomeReviewList;
        }

        /// <summary>
        /// method to insert the Areas
        /// </summary>
        /// <param name="areaBreakDownList"></param>
        /// <returns></returns>
        public bool AddAreas(List<Areas> areaBreakDownList)
        {

            bool isRowAffected = false;
            try
            {
                DataTable areaDt = new DataTable();
                areaDt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("AreaIndexID", typeof(long)),
                    new DataColumn("AreaID", typeof(long)),
                    new DataColumn("Description",typeof(string)),
                     new DataColumn("AgencyId",typeof(Guid)),
                    new DataColumn("Status",typeof(bool))
                    });


                foreach (var item in areaBreakDownList)
                {
                    areaDt.Rows.Add(item.AreaIndexID, item.AreaID, item.Description, item.AgencyID, item.Status);
                }

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@AgencyID", areaBreakDownList[0].StaffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", areaBreakDownList[0].StaffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", areaBreakDownList[0].StaffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@AreasType", areaDt));
                    command.CommandText = "USP_Add_Areas";
                    Connection.Open();
                    isRowAffected = (command.ExecuteNonQuery() > 0);
                    Connection.Close();

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
            }
            return isRowAffected;
        }

        /// <summary>
        /// method to get the Area Break down list
        /// </summary>
        /// <param name="areaBreakDownList"></param>
        /// <returns></returns>
        public List<Areas> GetAreas(List<Areas> areaBreakDownList)
        {
            try
            {
                StaffDetails details = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@AgencyID", areaBreakDownList[0].AgencyID));
                    command.Parameters.Add(new SqlParameter("@RoleID", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", details.UserId));
                    command.CommandText = "USP_Get_Areas";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    Connection.Close();
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);

                    if (_dataset != null && _dataset.Tables[0].Rows.Count > 0)
                    {
                        areaBreakDownList = _dataset.Tables[0].AsEnumerable().Select(x => new Areas
                        {
                            AreaIndexID = x.Field<long>("AreaIndexID"),
                            AreaID = x.Field<long>("AreaID"),
                            Description = x.Field<string>("Description"),
                            IsAreaReferred = Convert.ToBoolean(x.Field<int>("IsAreaReferred"))
                        }).ToList();
                    }
                    else
                    {
                        areaBreakDownList.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                areaBreakDownList.Clear();
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
                _dataset.Dispose();

            }
            return areaBreakDownList;
        }

        /// <summary>
        /// method to get the Divison break down list
        /// </summary>
        /// <param name="dvisionBreakDownList"></param>
        /// <returns></returns>
        public List<Divisions> GetDivisons(List<Divisions> dvisionBreakDownList)
        {
            try
            {
                StaffDetails details = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@AgencyID", dvisionBreakDownList[0].AgencyID));
                    command.Parameters.Add(new SqlParameter("@RoleID", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", details.UserId));
                    command.CommandText = "USP_Get_Divisions";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    Connection.Close();
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);

                    if (_dataset != null && _dataset.Tables[0].Rows.Count > 0)
                    {
                        dvisionBreakDownList = _dataset.Tables[0].AsEnumerable().Select(x => new Divisions
                        {
                            DivisionIndexID = x.Field<long>("DivisionIndexID"),
                            DivisionID = x.Field<long>("DivisionID"),
                            Description = x.Field<string>("Description"),
                            IsDivisionReferred = Convert.ToBoolean(x.Field<int>("IsDivisionReferred"))
                        }).ToList();
                    }
                    else
                    {
                        dvisionBreakDownList.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                dvisionBreakDownList.Clear();
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
                _dataset.Dispose();
            }
            return dvisionBreakDownList;
        }


        /// <summary>
        /// method to insert the Divisions
        /// </summary>
        /// <param name="divisionsBreakDownList"></param>
        /// <returns></returns>
        public List<Divisions> AddDivisions(out bool isRowAffected, List<Divisions> divisionsBreakDownList)
        {

            isRowAffected = false;
            try
            {
                DataTable divdt = new DataTable();
                divdt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("DivisionIndexID", typeof(long)),
                    new DataColumn("DivisionID", typeof(long)),
                    new DataColumn("Description",typeof(string)),
                     new DataColumn("AgencyId",typeof(Guid)),
                    new DataColumn("Status",typeof(bool))
                    });


                foreach (var item in divisionsBreakDownList)
                {
                    divdt.Rows.Add(item.DivisionIndexID, item.DivisionID, item.Description, item.AgencyID, item.Status);
                }

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@AgencyID", divisionsBreakDownList[0].StaffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", divisionsBreakDownList[0].StaffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", divisionsBreakDownList[0].StaffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@DivisionsType", divdt));
                    command.CommandText = "USP_Add_Divisions";
                    Connection.Open();
                    isRowAffected = (command.ExecuteNonQuery() > 0);
                    Connection.Close();

                    divisionsBreakDownList = this.GetDivisons(divisionsBreakDownList);


                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
            }
            return divisionsBreakDownList;
        }

        /// <summary>
        /// method to get, Whether Logged in user is allowed to access PIR and Section B
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool GetUserAccessPIR(string mode)
        {
            bool isAccess = false;
            try
            {

                StaffDetails details = StaffDetails.GetInstance();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Connection = Connection;
                command.Parameters.Add(new SqlParameter("@AgencyID", details.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleID", details.RoleId));
                command.Parameters.Add(new SqlParameter("@UserID", details.UserId));
                command.Parameters.Add(new SqlParameter("@mode", Convert.ToInt32(mode)));
                command.CommandText = "USP_GetIsAccessPIR";
                command.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                isAccess = Convert.ToBoolean(command.ExecuteScalar());
                Connection.Close();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();

            }
            return isAccess;
        }

        /// <summary>
        /// method to insert the Fund types based on the AgencyId
        /// </summary>
        /// <param name="fund"></param>
        /// <returns></returns>
        public bool AddFunds(Agency.FundSource fund, int mode = 0)
        {
            bool isRowsAffected = false;
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[29] {
                    new DataColumn("Acronym ", typeof(string)),
                    new DataColumn("Description",typeof(string)),
                    new DataColumn("Amount",typeof(string)),
                    new DataColumn("Date",typeof(string)),
                    new DataColumn("Duration",typeof(string)),
                       new DataColumn("ServiceQty ",typeof(string)),
                          new DataColumn("FundingType",typeof(string)),
                             new DataColumn("ProgramYear",typeof(string)),
                             new DataColumn("GranteeNo",typeof(string)),
                             new DataColumn("Grantee",typeof(string)),
                                new DataColumn("Status",typeof(string)),
                                 new DataColumn("FundID",typeof(Int32)),
                             new DataColumn("OldFund",typeof(string)),
                             new DataColumn("FundQ1",typeof(string)),
                             new DataColumn("FundQ2",typeof(string)),
                             new DataColumn("FundQ3",typeof(string)),
                             new DataColumn("FundQ4",typeof(string)),
                             new DataColumn("FundQ5",typeof(string)),
                             new DataColumn("FundQ6",typeof(string)),
                             new DataColumn("FundQ7",typeof(string)),
                             new DataColumn("FundQ8",typeof(string)),
                             new DataColumn("FundQ9",typeof(string)),
                             new DataColumn("FundQ10",typeof(string)),
                             new DataColumn("FundQ11",typeof(string)),
                             new DataColumn("FundQ12",typeof(string)),
                             new DataColumn("FundQ13",typeof(string)),
                             new DataColumn("FundQ14",typeof(string)),
                             new DataColumn("FundQ15",typeof(string)),
                             new DataColumn("FundQ16",typeof(string))
                    });



                if (fund.Acronym != null && fund.Description != null)
                {
                    dt.Rows.Add(fund.Acronym, fund.Description, fund.Amount, fund.Date, fund.Duration, fund.ServiceQty, fund.fundingtype, (string.IsNullOrEmpty(fund.ProgramYear)) ? "" : fund.ProgramYear, fund.grantNo,
                        fund.nameGranteeDelegate, fund.FundStatus, fund.FundID, fund.OldFund,
                        fund.FundQ1, fund.FundQ2, fund.FundQ3, fund.FundQ4, fund.FundQ5, fund.FundQ6, fund.FundQ7,
                        fund.FundQ8, fund.FundQ9, fund.FundQ10, fund.FundQ11, fund.FundQ12, fund.FundQ13, fund.FundQ14
                        , fund.FundQ15, fund.FundQ16
                        );
                }

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection)
                {


                    command.Parameters.Clear();
                    command.Connection = Connection;


                    command.Parameters.Add(new SqlParameter("@AgencyId", (mode == 0) ? fund.AgencyID : staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@LoginAgencyId", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@tblfund", dt));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = (mode == 0) ? "USP_AddFund" : "USP_AddFund_EndOfProgramYear";
                    Connection.Open();
                    isRowsAffected = (command.ExecuteNonQuery() > 0);
                    Connection.Close();
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
                Connection.Dispose();
            }
            return isRowsAffected;
        }

        /// <summary>
        /// Gets Seats with classroom and center
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public MoveSeats GetCenterandClassRoomSeats(MoveSeats moveSeats)
        {

            try
            {
                StaffDetails staff = StaffDetails.GetInstance();
                moveSeats.AgencyID = new Guid(staff.AgencyId.ToString());
                moveSeats.CenterClassPairList = new List<MoveSeats.CenteClassPair>();
                moveSeats.AgencyCenterList = new List<SelectListItem>();
                moveSeats.CenterList = new List<Center>();

                List<ProgramType> programList = new List<ProgramType>();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@SearchTerm", moveSeats.SearchTerm));
                    command.Parameters.Add(new SqlParameter("@RequestedPage", moveSeats.RequestedPage));
                    command.Parameters.Add(new SqlParameter("@Take", moveSeats.Take));
                    command.Parameters.Add(new SqlParameter("@Skip", moveSeats.Skip));
                    command.Parameters.Add(new SqlParameter("@IsEndOfYear", moveSeats.IsEndOfYear));
                    command.CommandText = "USP_GetCenterWithSeats";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }

                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        moveSeats.TotalPurchaedSeats = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalSeats"]);
                    }


                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {

                        moveSeats.TotalRecord = Convert.ToInt32(_dataset.Tables[1].Rows[0]["TotalRecord"]);

                        moveSeats.CenterList = _dataset.Tables[1].AsEnumerable()
                   .GroupBy(dr => new
                   {
                       centerID = dr.Field<long>("CenterID"),

                       centerName = dr.Field<string>("CenterName")
                   }
                   )



                   .Select((group, dr) => new Center
                   {
                       CenterId = Convert.ToInt32(group.Key.centerID),
                       Enc_CenterId = EncryptDecrypt.Encrypt64(group.Key.centerID.ToString()),
                       CenterName = group.Key.centerName,
                       TotalSeats = group.Sum(ds => ds.Field<int>("TotalSeats")),
                       OccupiedSeats = group.Sum(ds => ds.Field<int>("OccupiedSeats")),
                       AvailableSeats = group.Sum(ds => ds.Field<int>("AvailableSeats")),
                       Classroom = _dataset.Tables[1].AsEnumerable().Where(dr1 => dr1.Field<long>("CenterID") == group.Key.centerID).GroupBy(dr1 => new
                       {
                           classRoomID = dr1.Field<long>("ClassRoomID"),

                           classRoomName = dr1.Field<string>("ClassRoomName")
                       }).
                       Select((group1, dr1) => new Center.ClassRoom
                       {
                           ClassroomID = Convert.ToInt32(group1.Key.classRoomID),
                           Enc_ClassRoomId = EncryptDecrypt.Encrypt64(group1.Key.classRoomID.ToString()),
                           ClassName = group1.Key.classRoomName,
                           TotalSeats = group1.Sum(ds => ds.Field<int>("TotalSeats")),
                           OccupiedSeats = group1.Sum(ds => ds.Field<int>("OccupiedSeats")),
                           AvailableSeats = group1.Sum(ds => ds.Field<int>("AvailableSeats"))


                           // ProgramTypeList = _dataset.Tables[1].AsEnumerable().Where(dr2 => dr2.Field<long>("ClassRoomID") == group1.Key.classRoomID).
                           //GroupBy( dr2 =>new { programTypeID = dr2.Field<long>("ProgramTypeID"), programType = dr2.Field<string>("ProgramType") }).
                           //Select((group2, dr2) => new ProgramType {

                           //    ProgramID=Convert.ToInt32(group2.Key.programTypeID),
                           //    ProgramTypes = group2.Key.programType,
                           //    TotalSeats = group2.Sum(ds => ds.Field<int>("TotalSeats")),
                           //    OccupiedSeats = group2.Sum(ds => ds.Field<int>("OccupiedSeats")),
                           //    AvailableSeats = group2.Sum(ds => ds.Field<int>("AvailableSeats")),

                           //}).ToList()


                       }).ToList(),

                   }).ToList();

                    }


                    if (_dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
                    {
                        moveSeats.ProgramYear = Convert.ToString(_dataset.Tables[2].Rows[0]["ActiveProgramYear"]);
                    }
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return moveSeats;
        }

        /// <summary>
        /// method to Get the Seats Count by Center and Classroom
        /// </summary>
        /// <param name="centerId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="agencyID"></param>
        /// <returns></returns>
        public int GetSeatsBy(long centerId, long classRoomId, Guid agencyID, int reqOpen = 0, bool isEndOfYear = false)
        {

            int seatCount = 0;
            try
            {

                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {

                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", agencyID));
                    command.Parameters.Add(new SqlParameter("@CenterID", centerId));
                    command.Parameters.Add(new SqlParameter("@ClassRoomID", classRoomId));
                    command.Parameters.Add(new SqlParameter("@avail", reqOpen));
                    command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetSeats_Count";
                    Connection.Open();
                    //var result = command.ExecuteScalar();
                    int.TryParse(command.ExecuteScalar().ToString(), out seatCount);
                    Connection.Close();
                }


            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();

            }
            return seatCount;
        }


        public string AddMovedSeats(MoveSeats centerpair)
        {

            string result = "";
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();


                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[4] {
                    new DataColumn("CenterId",typeof(int)),
                    new DataColumn("ClassId",typeof(int)),
                    new DataColumn("SeatCount",typeof(int)),
                    new DataColumn("ProgramType",typeof(string))
                });

                int movedSeats = 0;

                if (centerpair.CenterClassPairList.Count > 0)
                {
                    foreach (var item in centerpair.CenterClassPairList)
                    {
                        dt.Rows.Add(

                            Convert.ToInt32(item.ToCenter),
                            Convert.ToInt32(item.ToClassRoom),
                            Convert.ToInt32(item.SeatsMoved),
                            ""
                            );

                        movedSeats += Convert.ToInt32(item.SeatsMoved);
                    }
                }

                int availCount = this.GetSeatsBy(Convert.ToInt64(centerpair.CenterClassPairList[0].FromCenter), Convert.ToInt64(centerpair.CenterClassPairList[0].FromClassRoom), centerpair.AgencyID, 1, centerpair.IsEndOfYear);


                if (movedSeats > availCount)
                {
                    return "2";
                }

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", centerpair.AgencyID));
                    command.Parameters.Add(new SqlParameter("@SeatsMoved", movedSeats));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@FromCenter", Convert.ToInt64(centerpair.CenterClassPairList[0].FromCenter)));
                    command.Parameters.Add(new SqlParameter("@FromClassroom", Convert.ToInt64(centerpair.CenterClassPairList[0].FromClassRoom)));
                    command.Parameters.Add(new SqlParameter("@SeatsTable", dt));
                    command.Parameters.Add(new SqlParameter("@IsEndOfYear", centerpair.IsEndOfYear));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_Add_MovedSeats";
                    Connection.Open();
                    //DataAdapter = new SqlDataAdapter(command);
                    //_dataset = new DataSet();
                    //DataAdapter.Fill(_dataset);
                    result = command.ExecuteScalar().ToString();
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return result;
        }



        public AccessStaffs GetStaffsByRole(string roleId)
        {
            AccessStaffs staffs = new AccessStaffs();
            try
            {

                StaffDetails details = StaffDetails.GetInstance();

                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();

                using (Connection)
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", details.UserId));
                    command.Parameters.Add(new SqlParameter("@TargetRole", roleId));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetStaffByRole";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }

                if (_dataset != null)
                {

                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return staffs;
        }



        public TransitionWithdrawal GetTransitionWithDrawalClients(int mode, int centerid = 0, int classroomid = 0, string fswid = "", string searchText = "", string progYear = "", int reqPage = 0, int pgSize = 10)

        {

            TransitionWithdrawal transWithdrawal = TransitionWithdrawal.Instance;
            transWithdrawal.TransitionWithdrawnClients = new List<WithdrawalQuestions>();
            int skip = pgSize * (reqPage - 1);
            skip = (skip < 0) ? 0 : skip;
            try
            {
                StaffDetails staff = new StaffDetails();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@CenterID", centerid));
                    command.Parameters.Add(new SqlParameter("@ClassroomID", classroomid));
                    command.Parameters.Add(new SqlParameter("@fswid", string.IsNullOrEmpty(fswid) || fswid == "0" ? (Guid?)null : new Guid(fswid)));
                    // command.Parameters.Add(new SqlParameter("@fswid",null));

                    command.Parameters.Add(new SqlParameter("@SearchText1", (searchText != "") ? searchText.Split(' ')[0] : searchText));
                    command.Parameters.Add(new SqlParameter("@SearchText2", (searchText != "") ? searchText.Split(' ')[1] : searchText));
                    command.Parameters.Add(new SqlParameter("@take", pgSize));
                    command.Parameters.Add(new SqlParameter("@skip", skip));
                    command.Parameters.Add(new SqlParameter("@sortcolumn", ""));
                    command.Parameters.Add(new SqlParameter("@sortorder", ""));
                    command.Parameters.Add(new SqlParameter("@ClientID", 0));
                    command.Parameters.Add(new SqlParameter("@ProgramYear", progYear));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = (mode == 1) ? "USP_GetWithdrawalClientList" : "USP_GetTransitionClientList";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }

                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        transWithdrawal.TotalRecord = Convert.ToInt32(_dataset.Tables[0].Rows[0]["TotalRecord"]);

                        transWithdrawal.TransitionWithdrawnClients = (from DataRow dr in _dataset.Tables[0].Rows
                                                                      select new WithdrawalQuestions
                                                                      {
                                                                          Client = dr["ClientName"].ToString(),
                                                                          ClientID = Convert.ToInt64(dr["ClientID"].ToString()),
                                                                          Enc_ClientID = EncryptDecrypt.Encrypt64(dr["ClientId"].ToString()),
                                                                          ISWaiting = Convert.ToBoolean(dr["IsWaiting"]),
                                                                          Center = dr["CenterName"].ToString(),
                                                                          Classroom = dr["ClassroomName"].ToString(),
                                                                          WithdrawnDays = string.IsNullOrEmpty(dr["WithdrawnDays"].ToString()) ? 0 : Convert.ToInt32(dr["WithdrawnDays"].ToString()),
                                                                          HealthManager = dr["HealthNurse"].ToString(),
                                                                          FSWHV = dr["FSW"].ToString(),
                                                                          WithdrawnDate = dr["DateWithdrawn"].ToString(),
                                                                          IsPregMom = Convert.ToBoolean((dr["IsPregMom"])),
                                                                          IsAnsweredQ1 = Convert.ToBoolean(dr["IsAnsweredQ1"]),
                                                                          IsAnsweredQ2 = Convert.ToBoolean(dr["IsAnsweredQ2"]),
                                                                          IsAnsweredQ3 = Convert.ToBoolean(dr["IsAnsweredQ3"]),
                                                                          IsAnsweredQ4 = Convert.ToBoolean(dr["IsAnsweredQ4"]),
                                                                          IsAnsweredQ5 = Convert.ToBoolean(dr["IsAnsweredQ5"]),
                                                                          IsAnsweredQ6 = Convert.ToBoolean(dr["IsAnsweredQ6"]),
                                                                          IsAnsweredQ7 = Convert.ToBoolean(dr["IsAnsweredQ7"]),
                                                                          IsAnsweredQ8 = Convert.ToBoolean(dr["IsAnsweredQ8"]),
                                                                          IsAnsweredQ9 = Convert.ToBoolean(dr["IsAnsweredQ9"]),
                                                                          IsShowQ9 = Convert.ToBoolean(dr["IsShowQ9"]),
                                                                          IsShowDentalServiceQuestion = Convert.ToBoolean(dr["IsShowDentalServiceQuestion"]),
                                                                          ProgramTypeID = EncryptDecrypt.Encrypt64(dr["ProgramID"].ToString()),
                                                                          IsReturning = Convert.ToBoolean(dr["Returning"]),
                                                                          LDAAge = Convert.ToString(dr["LDAAge"]),
                                                                          ProgramType = Convert.ToString(dr["ProgramType"])

                                                                      }).ToList();
                    }

                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        transWithdrawal.ProgramYears = (from DataRow dr1 in _dataset.Tables[1].Rows
                                                        select new SelectListItem
                                                        {
                                                            Text = Convert.ToString(dr1["ActiveProgramYear"]),
                                                            Value = Convert.ToString(dr1["ActiveProgramYear"]),
                                                            Selected = Convert.ToBoolean(dr1["Selected"])
                                                        }
                                                      ).ToList();
                    }
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
                DataAdapter.Dispose();
                _dataset.Dispose();
            }
            return transWithdrawal;
        }
        public AccessRoles SP_AccessRole(int type, string Agencyid)
        {
            AccessRoles AccessRoles = new AccessRoles();
            try
            {
                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", Agencyid));
                    command.Parameters.Add(new SqlParameter("@type", type));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_GetAllRoles";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    GetAllAccessRoles(AccessRoles, _dataset, type);
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return AccessRoles;
        }

        public void GetAllAccessRoles(AccessRoles AccessRoles, DataSet _dataset, int type)
        {

            if (_dataset.Tables[0].Rows.Count > 0)
            {
                AccessRoles.TitleList = (from DataRow dr5 in _dataset.Tables[0].Rows

                                         select new AccessRoles
                                         {
                                             TitleId = Convert.ToInt32(dr5["MasterId"].ToString()),
                                             TitleDescription = dr5["Description"].ToString(),
                                             ColorCode = dr5["ColorCode"].ToString().Trim()
                                         }).ToList();
            }
            Role role = null;
            if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
            {
                AccessRoles.RoleList = new List<Role>();
                foreach (DataRow dr in _dataset.Tables[1].Rows)
                {
                    role = new Role();
                    role.RoleId = dr["RoleId"].ToString();
                    role.RoleName = dr["RoleName"].ToString();
                    role.UserList = new List<UserDetails>();
                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        role.UserList = (from DataRow dr5 in _dataset.Tables[2].Rows
                                         where role.RoleId == dr5["RoleId"].ToString() && dr5["UserId"].ToString() != ""
                                         select new UserDetails
                                         {
                                             UserId = dr5["UserId"].ToString(),
                                             StaffName = dr5["StaffName"].ToString(),
                                             IsAllow = Convert.ToBoolean(dr5["IsChecked"]),
                                             ToView = Convert.ToBoolean(dr5["ToView"]),
                                             ToEnter = Convert.ToBoolean(dr5["ToEnter"]),
                                             ToFollowUp = Convert.ToBoolean(dr5["ToFollow"])
                                         }).ToList();
                        if (role.UserList.Count > 0)
                        {
                            role.IsAllow = (from DataRow dr1 in _dataset.Tables[2].Rows
                                            where role.RoleId == dr1["RoleId"].ToString() && Convert.ToInt32(dr1["IsChecked"]) > 0
                                            select (true)).Any();

                            if (type == 1) // for developmental team color code
                            {
                                role.ColorCode = (from DataRow dr1 in _dataset.Tables[2].AsEnumerable()
                                                  where role.RoleId == dr1["RoleId"].ToString() && Convert.ToInt32(dr1["IsChecked"]) > 0
                                                  select (dr1["ColorCode"].ToString())).FirstOrDefault();

                            }
                            if (type == 6) // for screening
                            {
                                role.ToView = (from DataRow dr1 in _dataset.Tables[2].Rows
                                               where role.RoleId == dr1["RoleId"].ToString() && Convert.ToBoolean(dr1["ToView"]) == true
                                               select (true)).Any();
                                role.ToEnter = (from DataRow dr1 in _dataset.Tables[2].Rows
                                                where role.RoleId == dr1["RoleId"].ToString() && Convert.ToBoolean(dr1["ToEnter"]) == true
                                                select (true)).Any();

                                role.ToFollowUp = (from DataRow dr1 in _dataset.Tables[2].Rows
                                                   where role.RoleId == dr1["RoleId"].ToString() && Convert.ToBoolean(dr1["ToFollow"]) == true
                                                   select (true)).Any();
                            }
                            AccessRoles.RoleList.Add(role);
                        }

                    }

                }
            }
        }
        public AccessRoles SaveAccessRoles(List<Role> RoleList, string AgencyId, string UserId, int type)
        {
            AccessRoles AccessRoles = new AccessRoles();
            try
            {

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@Userid", UserId);
                    command.Parameters.AddWithValue("@type", type);

                    if (RoleList != null && RoleList.Count > 0)
                    {

                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[7]
                        {
                            new DataColumn("RoleId",typeof(string)),
                            new DataColumn("UserId",typeof(string)),
                            new DataColumn("ColorCode",typeof(string)),
                              new DataColumn("MasterId",typeof(int)),
                             new DataColumn("ToView",typeof(bool)),
                             new DataColumn("ToEnter",typeof(bool)),
                              new DataColumn("ToFollowup",typeof(bool)),


                        });

                        foreach (Role role in RoleList)
                        {
                            if (role.IsAllow || role.ToView || role.ToFollowUp || role.ToEnter)

                            {
                                if (role.UserList.Count == 0)
                                {
                                    dt.Rows.Add(role.RoleId, null, role.ColorCode, type, role.ToView, role.ToEnter, role.ToFollowUp);

                                }
                                else
                                {
                                    foreach (UserDetails user in role.UserList)
                                    {
                                        if (user.IsAllow || user.ToView || user.ToFollowUp || user.ToEnter)
                                            dt.Rows.Add(role.RoleId, user.UserId, role.ColorCode, type, user.ToView, user.ToEnter, user.ToFollowUp);
                                    }

                                }
                            }

                        }
                        command.Parameters.AddWithValue("@AccessRoles", dt);
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_SaveAccessRoles";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    GetAllAccessRoles(AccessRoles, _dataset, type);

                    Connection.Close();

                }
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return AccessRoles;

        }

        public AccessRoles SaveAccessRolesForScreening(List<Role> RoleList, string AgencyId, string UserId, int type)
        {
            AccessRoles AccessRoles = new AccessRoles();
            try
            {

                using (Connection = connection.returnConnection())
                {
                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    command.Parameters.AddWithValue("@Userid", UserId);
                    command.Parameters.AddWithValue("@type", type);

                    if (RoleList != null && RoleList.Count > 0)
                    {

                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[6]
                        {
                            new DataColumn("UserID",typeof(string)),
                            new DataColumn("RoleID",typeof(string)),
                            new DataColumn("ToView",typeof(bool)),
                             new DataColumn("ToEnter",typeof(bool)),
                              new DataColumn("ToFollowup",typeof(bool)),
                             new DataColumn("MasterId",typeof(int)),
                        });

                        foreach (Role role in RoleList)
                        {
                            if (role.IsAllow)
                            {
                                if (role.UserList.Count == 0)
                                {
                                    dt.Rows.Add(role.RoleId, null, role.ColorCode, type);

                                }
                                else
                                {
                                    foreach (UserDetails user in role.UserList)
                                    {
                                        if (user.IsAllow)
                                            dt.Rows.Add(role.RoleId, user.UserId, user.ToView, user.ToEnter, user.ToFollowUp, type);
                                    }
                                }
                            }

                        }
                        command.Parameters.AddWithValue("@AccessRolesForScreening", dt);
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_SaveAccessRoles";
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);
                    GetAllAccessRoles(AccessRoles, _dataset, type);
                    Connection.Close();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return AccessRoles;

        }


        public NewProgramYearTransition EndOfProgramYear()
        {

            NewProgramYearTransition newProgramYear = new NewProgramYearTransition();
            try
            {



                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {

                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetEndOfYearTransitionDashboard";
                    Connection.Open();
                    _dataset = new DataSet();
                    DataAdapter = new SqlDataAdapter(command);
                    DataAdapter.Fill(_dataset);
                    Connection.Close();
                }


                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        newProgramYear.EndOfProgramYearDashboard.Funds = new NewProgramYearTransitionCounts
                        {
                            Active = Convert.ToInt64(_dataset.Tables[0].Rows[0]["activefund"])
                        };

                        newProgramYear.EndOfProgramYearDashboard.ProgramTypes = new NewProgramYearTransitionCounts
                        {
                            //Total = Convert.ToInt64(_dataset.Tables[0].Rows[0]["totalprograms"]),
                            Active = Convert.ToInt64(_dataset.Tables[0].Rows[0]["activeprogramtypes"])
                        };


                        newProgramYear.EndOfProgramYearDashboard.Centers = new NewProgramYearTransitionCounts
                        {
                            // Total = Convert.ToInt64(_dataset.Tables[0].Rows[0]["totalCenters"]),
                            Active = Convert.ToInt64(_dataset.Tables[0].Rows[0]["activeCenters"])
                        };

                        newProgramYear.EndOfProgramYearDashboard.Classrooms = new NewProgramYearTransitionCounts
                        {

                            // Total = Convert.ToInt64(_dataset.Tables[0].Rows[0]["totalClassrooms"]),
                            Active = Convert.ToInt64(_dataset.Tables[0].Rows[0]["activeclassrooms"])
                        };

                        //newProgramYear.EndOfProgramYearDashboard.Seats = new EndOfYearSlotsSeats
                        //{

                        //    Total = Convert.ToInt64(_dataset.Tables[0].Rows[0]["TotalSeats"]),
                        //    Occupied = Convert.ToInt64(_dataset.Tables[0].Rows[0]["OccupiedSeats"]),
                        //    Opened = Convert.ToInt64(_dataset.Tables[0].Rows[0]["OpenSeats"])

                        //};

                        newProgramYear.EndOfProgramYearDashboard.Slots = new EndOfYearSlotsSeats
                        {

                            Total = Convert.ToInt64(_dataset.Tables[0].Rows[0]["TotalSlots"]),
                            //Occupied = Convert.ToInt64(_dataset.Tables[0].Rows[0]["OccupiedSlots"]),
                            //Opened = Convert.ToInt64(_dataset.Tables[0].Rows[0]["OpenSlots"]),
                            //Expiring = Convert.ToInt64(_dataset.Tables[0].Rows[0]["ExpiringSlots"])

                        };


                        newProgramYear.EndOfProgramYearDashboard.Staffs = new NewProgramYearTransitionCounts
                        {

                            Active = Convert.ToInt64(_dataset.Tables[0].Rows[0]["ActiveStaffs"])

                        };

                        newProgramYear.EndOfProgramYearDashboard.ProgramYearStartDate = Convert.ToString(_dataset.Tables[0].Rows[0]["ProgramYearStartDate"]);

                    }
                }



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return newProgramYear;
        }


        public Agency GetEndOfYearFunds_Programs()
        {
            Agency _agency = new Agency();
            _agency.FundSourcedata = new List<Agency.FundSource>();
            _agency.ProgramTypeList = new List<Agency.ProgramType>();
            _agency.ProgramYearList = new List<SelectListItem>();
            _agency.ReferenceProgramList = new List<SelectListItem>();
            _agency._FundedEnrollment = new Agency.FundedEnrollment();
            _agency.DivisionsFullList = new List<Divisions>();
            _agency.AreasFullList = new List<Areas>();
            _agency.DivisionsList = new List<SelectListItem>();

            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();


                using (Connection = connection.returnConnection())
                {

                    command.Connection = Connection;
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.CommandText = "USP_GetEndOfYearFunds_ProgramTypes";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);

                    if (_dataset != null && _dataset.Tables.Count > 0)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0) //funding source
                        {

                            _agency.FundSourcedata = _dataset.Tables[0].AsEnumerable().Select(x => new Agency.FundSource
                            {

                                FundID = (int)x.Field<long>("FundID"),
                                OldFund = "0",
                                Acronym = x.Field<string>("Acronym"),
                                Description = x.Field<string>("FundDescription"),
                                Amount = x.Field<int>("Amount"),
                                Date = x.Field<string>("Date"),
                                FundStatus = x.Field<int>("FundStatus"),
                                Duration = x.Field<int>("Duration").ToString(),
                                ServiceQty = x.Field<int>("ServiceQty").ToString(),
                                fundingtype = x.Field<int>("FundingType").ToString(),
                                ProgramYear = string.IsNullOrEmpty(x.Field<string>("ProgramYear").ToString()) ? "0" : x.Field<string>("ProgramYear"),
                                nameGranteeDelegate = x.Field<string>("Grantee"),
                                grantNo = x.Field<string>("GranteeNo"),
                                IsReferredByProgram = Convert.ToBoolean(x.Field<int>("IsReferredByProgram"))
                            }).ToList();
                        }



                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            _agency.ProgramTypeList = _dataset.Tables[1].AsEnumerable().Select(x => new Agency.ProgramType
                            {

                                ProgramID = Convert.ToInt32(x.Field<long>("ProgramTypeID")),
                                FundID = x.Field<string>("FundID"),
                                ProgramTypes = x.Field<string>("ProgramType"),
                                Description = x.Field<string>("ProgDesc"),
                                PIRReport = x.Field<bool>("PIRReport"),
                                Slots = string.IsNullOrEmpty(x.Field<int?>("Slots").ToString()) ? "" : x.Field<int>("Slots").ToString(),
                                ReferenceProg = string.IsNullOrEmpty(x.Field<int?>("ReferenceProg").ToString()) ? "" : x.Field<int?>("ReferenceProg").ToString(),
                                DivisionID = string.IsNullOrEmpty(x.Field<long?>("DivisionID").ToString()) ? "1" : x.Field<long>("DivisionID").ToString(),
                                //Area = x.Field<string>("AreaID"),
                                MinAge = string.IsNullOrEmpty(x.Field<int?>("MinAge").ToString()) ? 0 : x.Field<int>("MinAge"),
                                MaxAge = string.IsNullOrEmpty(x.Field<int?>("MaxAge").ToString()) ? 0 : x.Field<int>("MaxAge"),
                                //  HealthReview = x.Field<bool>("HealthReview"),
                                ProgStatus = string.IsNullOrEmpty(x.Field<int?>("ProgStatus").ToString()) ? 1 : x.Field<int>("ProgStatus"),
                                programstartDate = string.IsNullOrEmpty(x.Field<string>("ProgramStartDate")) ? "" : x.Field<string>("ProgramStartDate"),
                                programendDate = string.IsNullOrEmpty(x.Field<string>("ProgramEndDate")) ? "" : x.Field<string>("ProgramEndDate"),
                                LastDateCurrentApplication = string.IsNullOrEmpty(x.Field<string>("LastDateCurrentApplication")) ? "" : x.Field<string>("LastDateCurrentApplication"),
                                DateFutureApplication = string.IsNullOrEmpty(x.Field<string>("DateFutureApplication")) ? "" : x.Field<string>("DateFutureApplication"),
                                TransitionDate = string.IsNullOrEmpty(x.Field<string>("TransitionDate")) ? "" : x.Field<string>("TransitionDate"),
                                ProgramTypeAssociation = string.IsNullOrEmpty(x.Field<long?>("ProgramTypeAssociation").ToString()) ? "" : x.Field<long>("ProgramTypeAssociation").ToString(),
                                refList = (_dataset.Tables[3] != null && _dataset.Tables[3].Rows.Count > 0) ? _dataset.Tables[3].AsEnumerable().Select(y => new Agency.ProgramType.ReferenceProgInfo
                                {
                                    Id = y.Field<long>("ReferenceId").ToString(),
                                    Name = y.Field<string>("Name")
                                }).ToList() : new List<Agency.ProgramType.ReferenceProgInfo>()

                            }).ToList();


                        }
                        if (_dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
                        {
                            _agency.DivisionsList = _dataset.Tables[2].AsEnumerable().Select(x => new SelectListItem
                            {
                                Text = x.Field<long>("DivisionID").ToString(),
                                Value = x.Field<long>("DivisionID").ToString()

                            }).ToList();
                        }
                        if (_dataset.Tables[3] != null && _dataset.Tables[3].Rows.Count > 0)
                        {
                            _agency.ReferenceProgramList = _dataset.Tables[3].AsEnumerable().Select(x => new
                              SelectListItem
                            {

                                Text = x.Field<string>("Name").ToString(),
                                Value = x.Field<long>("ReferenceId").ToString()
                            }).ToList();
                        }

                        if (_dataset.Tables[4] != null && _dataset.Tables[4].Rows.Count > 0)
                        {
                            _agency.ProgramYearList = _dataset.Tables[4].AsEnumerable().Select(x => new SelectListItem
                            {
                                Text = x.Field<string>("FutureProgramYear")
                            }
                            ).ToList();
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return _agency;

        }


        public string InsertEndOfYearFundsPrograms(Agency agencyDetails, List<Agency.FundSource> FundSource)
        {
            string result = "";

            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;


                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[29] {
                    new DataColumn("Acronym ", typeof(string)),
                    new DataColumn("Description",typeof(string)),
                    new DataColumn("Amount",typeof(string)),
                    new DataColumn("Date",typeof(string)),
                    new DataColumn("Duration",typeof(string)),
                       new DataColumn("ServiceQty ",typeof(string)),
                          new DataColumn("FundingType",typeof(string)),
                             new DataColumn("ProgramYear",typeof(string)),
                             new DataColumn("GranteeNo",typeof(string)),
                             new DataColumn("Grantee",typeof(string)),
                                new DataColumn("Status",typeof(string)),
                                 new DataColumn("FundID",typeof(Int32)),
                             new DataColumn("OldFund",typeof(string)),
                             new DataColumn("FundQ1",typeof(string)),
                             new DataColumn("FundQ2",typeof(string)),
                             new DataColumn("FundQ3",typeof(string)),
                             new DataColumn("FundQ4",typeof(string)),
                             new DataColumn("FundQ5",typeof(string)),
                             new DataColumn("FundQ6",typeof(string)),
                             new DataColumn("FundQ7",typeof(string)),
                             new DataColumn("FundQ8",typeof(string)),
                             new DataColumn("FundQ9",typeof(string)),
                             new DataColumn("FundQ10",typeof(string)),
                             new DataColumn("FundQ11",typeof(string)),
                             new DataColumn("FundQ12",typeof(string)),
                             new DataColumn("FundQ13",typeof(string)),
                             new DataColumn("FundQ14",typeof(string)),
                             new DataColumn("FundQ15",typeof(string)),
                             new DataColumn("FundQ16",typeof(string))
                    });

                    DataTable dt1 = new DataTable();
                    dt1.Columns.AddRange(new DataColumn[19] {
                        new DataColumn("ProgramType", typeof(string)),
                        new DataColumn("Description",typeof(string)),
                        new DataColumn("PIRReport",typeof(bool)),
                        new DataColumn("Slots",typeof(string)),
                        new DataColumn("ReferenceProg",typeof(string)),
                         new DataColumn("DivisionID",typeof(string)),
                         new DataColumn("MinAge",typeof(string)),
                        new DataColumn("MaxAge",typeof(string)),
                        new DataColumn("programstartDate",typeof(string)),
                        new DataColumn("programendDate",typeof(string)),
                        new DataColumn("ProgramID",typeof(Int32)),
                         new DataColumn("FundID",typeof(string)),
                          new DataColumn("OldFund",typeof(string)),
                          new DataColumn("HealthReview",typeof(bool)),
                          new DataColumn("LastDateCurrentApplication",typeof(string)),
                          new DataColumn("DateFutureApplication",typeof(string)),
                          new DataColumn("TransitionDate",typeof(string)),
                           new DataColumn("ProgramTypeAssociation",typeof(string)),
                           new DataColumn("Status",typeof(int))
                        });



                    //Fund and Program Types
                    if (FundSource != null && FundSource.Count > 0)
                    {

                        foreach (Agency.FundSource fund in agencyDetails.FundSourcedata)
                        {
                            if (fund.Acronym != null && fund.Description != null)
                            {
                                dt.Rows.Add(fund.Acronym, fund.Description, fund.Amount, fund.Date, fund.Duration, fund.ServiceQty, fund.fundingtype,
                                    //(string.IsNullOrEmpty(fund.ProgramYear)) ? "" : (fund.ProgramYear).Replace("-", ""), 
                                    fund.ProgramYear,
                                    fund.grantNo,

                                    fund.nameGranteeDelegate, fund.FundStatus.ToString(), fund.FundID, fund.OldFund,
                                    fund.FundQ1, fund.FundQ2, fund.FundQ3, fund.FundQ4, fund.FundQ5, fund.FundQ6, fund.FundQ7,
                                    fund.FundQ8, fund.FundQ9, fund.FundQ10, fund.FundQ11, fund.FundQ12, fund.FundQ13, fund.FundQ14
                                    , fund.FundQ15, fund.FundQ16
                                    );
                            }
                        }

                        foreach (Agency.ProgramType prog in agencyDetails.ProgramTypeList)
                        {
                            if (prog.ProgramTypes != null && prog.Description != null)
                            {
                                dt1.Rows.Add(prog.ProgramTypes, prog.Description, prog.PIRReport,
                                    prog.Slots, prog.ReferenceProg, prog.DivisionID, prog.MinAge, prog.MaxAge,
                                    prog.programstartDate, prog.programendDate, prog.ProgramID,
                                    prog.FundID, prog.OldFund, prog.HealthReview, prog.LastDateCurrentApplication, prog.DateFutureApplication, prog.TransitionDate, prog.ProgramTypeAssociation, prog.ProgStatus);//changes

                            }
                        }
                    }

                    command.Parameters.Add(new SqlParameter("@AgencyId", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@tblfund", dt));
                    command.Parameters.Add(new SqlParameter("@tblprog", dt1));
                    command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                    command.CommandText = "USP_EndOfYear_Add_Programs_Funds";
                    command.CommandType = CommandType.StoredProcedure;

                    Connection.Open();
                    command.ExecuteNonQuery();
                    Connection.Close();
                    result = command.Parameters["@result"].Value.ToString();


                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();

            }
            return result;



        }


        public bool SetNextProgramYearDate(string progDate)
        {
            bool isRowsAffected = false;

            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();

                }

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@NextProgramStartDate", progDate));
                    command.Parameters.Add(new SqlParameter("@Result", 0)).Direction = ParameterDirection.Output;
                    command.Connection = Connection;
                    command.CommandText = "USP_InsertNextProgramYearDate";
                    command.CommandType = CommandType.StoredProcedure;
                    Connection.Open();
                    command.ExecuteNonQuery();
                    isRowsAffected = (command.Parameters["@Result"].Value.ToString() == "1");
                    Connection.Close();


                }

            }
            catch (Exception ex)
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


        public void GetCentersByProgramYear(List<HrCenterInfo> hrCenterList, string programYear)
        {
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();

                }


                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@ProgramYear", programYear));
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetCentersByProgramYear";
                    DataAdapter = new SqlDataAdapter(command);
                    _dataset = new DataSet();
                    DataAdapter.Fill(_dataset);

                    if (_dataset != null && _dataset.Tables.Count > 0)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {
                            hrCenterList = (from DataRow dr0 in _dataset.Tables[0].Rows
                                            select new HrCenterInfo
                                            {
                                                CenterId = Convert.ToString(dr0["CenterID"]),
                                                Enc_CenterID = EncryptDecrypt.Encrypt64(dr0["CenterID"].ToString()),
                                                Name = Convert.ToString(dr0["CenterName"])

                                            }


                                          ).ToList();
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
            }

        }

        public void ChangeAgencySlots(ref string result, ref DataSet des, string slotNumber, string slotChangeType, bool isEndOfYear = false)
        {

            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                    command.Parameters.Add(new SqlParameter("@SlotNumber", slotNumber));
                    command.Parameters.Add(new SqlParameter("@SlotChangeType", slotChangeType));
                    command.Parameters.Add(new SqlParameter("@IsEndOfYear", isEndOfYear));
                    command.Parameters.Add(new SqlParameter("@Result", result)).Direction = ParameterDirection.Output;
                    command.CommandText = "USP_ChangeAgencySlotsCount";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = Connection;
                    Connection.Open();
                    // command.ExecuteNonQuery();
                    DataAdapter = new SqlDataAdapter(command);
                    des = new DataSet();
                    DataAdapter.Fill(des);
                    result = command.Parameters["@Result"].Value.ToString();
                    Connection.Close();
                }


                //if (result == "1" && slotChangeType!="0")
                //{
                //    if(_dataset!=null && _dataset.Tables.Count>0 && _dataset.Tables[0].Rows.Count>0)
                //    {
                //        SendMail.SendEmailForChangeInAgencySlots(_dataset.Tables[0].Rows[0]["AgencyName"].ToString(),
                //            _dataset.Tables[0].Rows[0]["SlotNumber"].ToString(),_dataset.Tables[0].Rows[0]["SlotNumberold"].ToString(),
                //            _dataset.Tables[0].Rows[0]["SlotChangeType"].ToString(),filepath,imagePath,
                //            _dataset.Tables[0].Rows[0]["AgencyAdminName"].ToString())
                //            ;

                //    }

                //}

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();

            }


        }

    
    public Boolean GetSingleAccessStatus(int type) {

        bool _access = false;

        try
        {

            StaffDetails stf = StaffDetails.GetInstance();

            if (Connection.State == ConnectionState.Open)
                Connection.Close();

            command.Parameters.Clear();
            command.Connection = Connection;
            command.CommandType = CommandType.Text;
            command.CommandText = "select top 1 id from  AccessRoleList where MasterId =" + type + "  and UserId = '" + stf.UserId + "' and RoleId ='" + stf.RoleId + "'";
            Connection.Open();
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            Connection.Close();
            if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0) {
                _access = true;
            }

        }
        catch (Exception ex) {
            clsError.WriteException(ex);

        }
        return _access;
    }

        public StaffRoleMapping GetStaffRoleMappingDetails(string agencyId, string Command="",List<string> staffRoles=null,string mRoleId=null,string StaffRoleId=null)
        {
            StaffRoleMapping SRMDetails = new StaffRoleMapping();
            SRMDetails.RolesList = new List<StaffRoleMapping.RoleList>();
            SRMDetails.ManagerRoleTableList = new List<StaffRoleMapping.ManagerRoleTable>();
            try
            {

                StaffDetails stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", String.IsNullOrEmpty(agencyId)?null:agencyId));
                command.Parameters.Add(new SqlParameter("@Userid", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@Command", Command));
                command.Parameters.Add(new SqlParameter("@ManagerRoleId", mRoleId));
                //staffRoles
                DataTable StaffRoleIds = new DataTable();
                StaffRoleIds.Columns.AddRange(new DataColumn[1] {
                     new DataColumn("RoleId", typeof(string))
                });
                if (staffRoles != null && staffRoles.Count > 0) {
                    foreach (var item in staffRoles) {
                        StaffRoleIds.Rows.Add(item);
                    }
                }
                command.Parameters.Add(new SqlParameter("@StaffRoleIds", StaffRoleIds));
                command.Parameters.Add(new SqlParameter("@StaffRId", StaffRoleId));  //for yakkar mapping page


                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetStaffRoleMappingDetails";
                Connection.Open();
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                Connection.Close();

                //for yakkarmapping page
                if (_dataset != null && _dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0 && Command == "mgrole")
                {
                    var _dr = _dataset.Tables[0].Rows[0];

                    var RoleLi = new StaffRoleMapping.RoleList();
                    RoleLi.RoleId = _dr["Id"].ToString();
                    RoleLi.RoleName = _dr["RoleName"].ToString();
                    SRMDetails.RolesList.Add(RoleLi);

                }


                if (_dataset != null && _dataset.Tables.Count > 0 && Command == "Update")
                {

             SRMDetails.Updated = DBNull.Value == _dataset.Tables[0].Rows[0]["Updated"] ? false : Convert.ToBoolean(_dataset.Tables[0].Rows[0]["Updated"]);





                    if (_dataset.Tables.Count > 2 )
                    {

                        //for manager list

                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            var RoleLi = new StaffRoleMapping.RoleList();
                            RoleLi.RoleId = dr["Id"].ToString();
                            RoleLi.RoleName = dr["RoleName"].ToString();
                            SRMDetails.RolesList.Add(RoleLi);

                        }


                        //for mapped staff role list
                        var MGTable = new List<StaffRoleMapping.ManagerRoleTable>();

                        foreach (var item in SRMDetails.RolesList)
                        {
                            var MgRole = new StaffRoleMapping.ManagerRoleTable();
                            MgRole.StaffRoles = new List<StaffRoleMapping.StaffRole>();
                            MgRole.RoleId = item.RoleId;
                            MgRole.RoleName = item.RoleName;
                            foreach (DataRow dr in _dataset.Tables[2].Rows)
                            {
                                var stafRole = new StaffRoleMapping.StaffRole();

                                if (MgRole.RoleId == dr["MRoleId"].ToString())
                                {

                                    stafRole.RoleId = dr["MRoleId"].ToString();
                                    stafRole.RoleName = dr["RoleName"].ToString();
                                    MgRole.StaffRoles.Add(stafRole);
                                }

                            }
                            MGTable.Add(MgRole);
                        }

                        SRMDetails.ManagerRoleTableList = MGTable;
                    }

                }


                if (_dataset != null && Command == "StaffRoleListBymId" && _dataset.Tables.Count > 0) {

                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        var RoleLi = new StaffRoleMapping.RoleList();
                        RoleLi.RoleId = dr["Id"].ToString();
                        RoleLi.RoleName = dr["RoleName"].ToString();
                        RoleLi.Checked = DBNull.Value == dr["Checked"] ? false : Convert.ToBoolean(dr["Checked"]);
                        SRMDetails.RolesList.Add(RoleLi);

                    }

                    //for super admin

                    if (_dataset.Tables.Count > 2)
                    {
                        if (_dataset.Tables.Count > 1 && _dataset.Tables[1].Rows.Count > 0)
                        {

                            var MGTable = new List<StaffRoleMapping.ManagerRoleTable>();
                            foreach (DataRow dr in _dataset.Tables[1].Rows)
                            {
                                var MgRole = new StaffRoleMapping.ManagerRoleTable();
                                var stafRole = new List<StaffRoleMapping.StaffRole>();

                                MgRole.RoleId = dr["Id"].ToString();
                                MgRole.RoleName = dr["RoleName"].ToString();


                                MGTable.Add(MgRole);
                            }
                            SRMDetails.ManagerRoleTableList = MGTable;

                        }


                        if (_dataset.Tables.Count > 2 && _dataset.Tables[2].Rows.Count > 0)
                        {
                           
                            for (int i=0; i < SRMDetails.ManagerRoleTableList.Count;i++) {

                                var stafRole = new List<StaffRoleMapping.StaffRole>();
                                foreach (DataRow dr in _dataset.Tables[2].Rows)
                                {
                                    if (SRMDetails.ManagerRoleTableList[i].RoleId == dr["MRoleId"].ToString())
                                    {
                                        stafRole.Add(new StaffRoleMapping.StaffRole()
                                        {
                                            RoleId = dr["StaffRoleId"].ToString(),
                                            RoleName = dr["RoleName"].ToString()
                                        });
                                    }

                                }

                                SRMDetails.ManagerRoleTableList[i].StaffRoles = stafRole;
                            }

                        }

                    }


                    }

                if (_dataset != null && (Command == "SuperAdmin" || Command == "AgencyAdmin")  && _dataset.Tables.Count > 0)
                {

                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        var RoleLi = new StaffRoleMapping.RoleList();
                        RoleLi.RoleId = dr["Id"].ToString();
                        RoleLi.RoleName = dr["RoleName"].ToString();
                        SRMDetails.RolesList.Add(RoleLi);

                    }

                    //for Agency Admin
                    if (_dataset.Tables.Count > 1)
                    {

                        var MGTable = new List<StaffRoleMapping.ManagerRoleTable>();

                        foreach (var item in SRMDetails.RolesList)
                        {
                            var MgRole = new StaffRoleMapping.ManagerRoleTable();
                            MgRole.StaffRoles = new List<StaffRoleMapping.StaffRole>();
                            MgRole.RoleId = item.RoleId;
                            MgRole.RoleName = item.RoleName;
                            foreach (DataRow dr in _dataset.Tables[1].Rows)
                            {
                                var stafRole = new StaffRoleMapping.StaffRole();

                                if (MgRole.RoleId == dr["MRoleId"].ToString())
                                {

                                    stafRole.RoleId = dr["MRoleId"].ToString();
                                    stafRole.RoleName = dr["RoleName"].ToString();
                                    MgRole.StaffRoles.Add(stafRole);
                                }

                            }
                            MGTable.Add(MgRole);
                        }

                        SRMDetails.ManagerRoleTableList = MGTable;
                    }


                }


                /*
                if (_dataset != null && Command == "AgencyAdmin" && _dataset.Tables.Count > 0)
                {

                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        var RoleLi = new StaffRoleMapping.RoleList();
                        RoleLi.RoleId = dr["Id"].ToString();
                        RoleLi.RoleName = dr["RoleName"].ToString();
                        SRMDetails.RolesList.Add(RoleLi);

                    }


                }


                if (_dataset != null && _dataset.Tables.Count > 0 && Command != "Update")
                {
                    //List of roles
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            var RoleLi = new StaffRoleMapping.RoleList();
                            RoleLi.RoleId = dr["Id"].ToString();
                            RoleLi.RoleName = dr["RoleName"].ToString();
                            if (Command == "StaffRoleListBymId")
                            {
                                RoleLi.Checked = DBNull.Value == dr["Checked"] ? false : Convert.ToBoolean(dr["Checked"]);
                            }


                            SRMDetails.RolesList.Add(RoleLi);

                        }
                    }


                    if (_dataset.Tables.Count > 1)
                    {

                        var MGTable = new List<StaffRoleMapping.ManagerRoleTable>();

                        foreach (var item in SRMDetails.RolesList)
                        {
                            var MgRole = new StaffRoleMapping.ManagerRoleTable();
                            MgRole.StaffRoles = new List<StaffRoleMapping.StaffRole>();
                            MgRole.RoleId = item.RoleId;
                            MgRole.RoleName = item.RoleName;
                            foreach (DataRow dr in _dataset.Tables[1].Rows)
                            {
                                var stafRole = new StaffRoleMapping.StaffRole();

                                if (MgRole.RoleId == dr["MRoleId"].ToString())
                                {

                                    stafRole.RoleId = dr["MRoleId"].ToString();
                                    stafRole.RoleName = dr["RoleName"].ToString();
                                    MgRole.StaffRoles.Add(stafRole);
                                }

                            }
                            MGTable.Add(MgRole);
                        }

                        SRMDetails.ManagerRoleTableList = MGTable;
                    }

                }
                else if (_dataset != null && _dataset.Tables.Count > 0 && Command == "Update") {
SRMDetails.Updated = DBNull.Value == _dataset.Tables[0].Rows[0]["Updated"]  ? false : Convert.ToBoolean(_dataset.Tables[0].Rows[0]["Updated"]);
                }
                */

                    }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return SRMDetails;
        }




            }
}