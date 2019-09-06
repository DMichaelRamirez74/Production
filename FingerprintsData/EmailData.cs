using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FingerprintsData
{
    public class EmailData : FingerprintsModel.AbstractEmail
    {

        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        //DataTable _dataTable = null;
        DataSet _dataset = null;
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        public async Task<int> SendEmailParentsStaffs(StaffDetails staff, FingerprintsModel.Enums.EmailType emailType, bool isStaff, long centerId=0, long classRoomId=0, params object[] list)
        {
            Dictionary<String, String> dictEmail = new Dictionary<string, string>();


            try
            {
                dynamic optionalData = null;

                if (emailType == FingerprintsModel.Enums.EmailType.UnscheduledSchoolDay)
                    optionalData = list.Where(x => x.GetType() == typeof(long)).First();


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetParentsAndManagementEmail";
                command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", centerId));
                command.Parameters.Add(new SqlParameter("@ClassRoomId", classRoomId));
                command.Parameters.Add(new SqlParameter("@RecordType", (int)emailType));
                command.Parameters.Add(new SqlParameter("@OptionalParameter", optionalData));
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);



                Array.Resize(ref list, (list.Length + 3));
                list[list.Length - 3] = staff;
                list[list.Length - 2] = isStaff;
                list[list.Length - 1] = _dataset;

                int result=0;
                switch (emailType)
                {
                    case FingerprintsModel.Enums.EmailType.CenterClosure:
                        result= Task.FromResult( await Task.Factory.StartNew(() => this.SendEmailCenterClosure(list))).Result.Result;
                        break;

                    case FingerprintsModel.Enums.EmailType.UnscheduledSchoolDay:


                        result = Task.FromResult(await Task.Factory.StartNew(() => this.SendEmailUnscheduledSchoolDay(list))).Result.Result;
                        break;
                }

                return result;



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
                Connection.Close();
            }

            return 0;

        }


        protected override async Task<int> SendEmailCenterClosure(params object[] list)
        {


            if (await Task.Run(() => SendMail.SendEmailWithTask("", "", "", "")))
            {

            }
            return  0;
        }


        protected override async Task<int> SendEmailUnscheduledSchoolDay(params object[] list)
        {

            try
            {



                DataSet dataset = new DataSet();
                bool isStaff;
                StreamReader reader = null;
                UnscheduledSchoolDay unscheduledSchoolDay = new UnscheduledSchoolDay();
                string emailMessage = "";
                StaffDetails staff = null;
                // StaffDetails staffThread = await Task.Factory.StartNew(() => StaffDetails.GetThreadedInstance(context));

                FingerprintsModel.Enums.EmailStatus emailStatusEnum = FingerprintsModel.Enums.EmailStatus.All;
                string serer = "".Replace(" ", "").Trim();
                foreach (var item in list)
                {
                    if (item.GetType() == typeof(DataSet))
                    {
                        dataset = (DataSet)item;
                    }

                    else if (item.GetType() == typeof(bool))
                    {
                        isStaff = (bool)item;
                    }

                    else if (item.GetType() == typeof(UnscheduledSchoolDay))
                    {

                    }
                    else if (item.GetType() == typeof(StreamReader))
                    {
                        reader = (StreamReader)item;

                    }

                    else if (item.GetType() == typeof(StaffDetails))
                    {
                        staff = (StaffDetails)item;
                    }

                    else if (item.GetType() == typeof(FingerprintsModel.Enums.EmailStatus))
                    {
                        emailStatusEnum = (FingerprintsModel.Enums.EmailStatus)item;
                    }


                }








                if (dataset.Tables.Count > 0)
                {


                    switch (emailStatusEnum)
                    {
                        case FingerprintsModel.Enums.EmailStatus.BouncedEmails:





                            break;
                    }

                    string reason = string.Empty;
                    string classDaste = string.Empty;
                    long unscheduledSchoolDayID = 0;
                    string staffRole = string.Empty;
                    string agencyName = string.Empty;

                    if (dataset.Tables.Count > 2 && dataset.Tables[2] != null && dataset.Tables[2].Rows.Count > 0)
                    {
                        reason = (Convert.ToString(dataset.Tables[2].Rows[0]["Reason"]));
                        classDaste = (Convert.ToString(dataset.Tables[2].Rows[0]["ClassDate"]));
                        unscheduledSchoolDayID = Convert.ToInt64(dataset.Tables[2].Rows[0]["UnscheduledSchoolDayID"]);
                        staffRole = Convert.ToString(dataset.Tables[2].Rows[0]["StaffRole"]);
                        agencyName = Convert.ToString(dataset.Tables[2].Rows[0]["AgencyName"]);
                    }


              Email.TotalEmails=  dataset.Tables[0].AsEnumerable().Where(x => x.Field<int>("EmailStatus") == (int)emailStatusEnum || emailStatusEnum == FingerprintsModel.Enums.EmailStatus.All).Count();


                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {


                        string centerName = string.Empty;
                        string classroomName = string.Empty;
                        string parentName = string.Empty;
                        string childName = string.Empty;
                        string parentEmail = string.Empty;
                        string subject = string.Empty;
                        long clientID = 0;
                        long parentID = 0;
                        bool emailStatus = false;
                        int emailstatusType = 0;

                        emailMessage = reader.ReadToEnd();
                        parentName = Convert.ToString(dr["ParentName"]);
                        childName = Convert.ToString(dr["ChildName"]);
                        parentEmail = Convert.ToString(dr["Email"]);
                        centerName = Convert.ToString(dr["CenterName"]);
                        classroomName = Convert.ToString(dr["ClassroomName"]);
                        clientID = Convert.ToInt64(dr["ClientID"]);
                        parentID = Convert.ToInt64(dr["ParentID"]);
                        emailstatusType = Convert.ToInt32(dr["EmailStatus"]);


                        // either the email needs to be sent for all or it should be sent to only for unsent or bounced emails //
                        if (emailStatusEnum == FingerprintsModel.Enums.EmailStatus.All || (int)emailStatusEnum == emailstatusType)
                        {



                            string day = DateTime.Parse(classDaste, new CultureInfo("en-US", true)).ToString("dddd");
                            emailMessage = emailMessage.Replace("{Name}", parentName)
                                                       .Replace("{reason}", reason)
                                                       .Replace("{reasonBody}", "School has been scheduled on " + classDaste + " (" + day + "). Your child (" + childName + ") will be expected to attend school as usual.")
                                                       .Replace("{reportbody}", "<p style='color:#0e2965;'>Regards,<br>" + staff.FullName + "<br>" + staffRole + "<br>" + centerName + "</p>")
                                                       .Replace("{copyright}", "<p style='text-align:center'>Copyright © " + DateTime.Now.Year + " All Rights Reserved</p><p style='text-align:center;'>" + agencyName + "</p>");
                            subject = "School has been scheduled on " + classDaste + "";

                            if (await Task.Run(() => SendMail.SendEmailWithTask(staff.EmailID, parentEmail, emailMessage, subject)))
                            {

                                emailStatus = true;
                            }
                            else
                            {
                                emailStatus = false;
                            }


                            Email.ClientEmailReport emailReport = new Email.ClientEmailReport
                            {
                                ClientID = clientID,
                                ParentID = parentID,
                                EmailType = FingerprintsModel.Enums.EmailType.UnscheduledSchoolDay,
                                EmailStatus = (emailStatus)? FingerprintsModel.Enums.EmailStatus.SentEmails:FingerprintsModel.Enums.EmailStatus.BouncedEmails,
                                staffDetails = staff,
                                ReferenceID = unscheduledSchoolDayID
                            };


                            await Task.Run(() => this.AddClientEmailReport(emailReport)).ContinueWith(x =>
                            {
                                //if(x.IsCompleted)
                                //Email.conque.Enqueue((int)x.Result);

                            });

                            
                        }
                    }

                }

                return 1;


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                
            }
            return 0;
        }



        private async Task<int> AddClientEmailReport(Email.ClientEmailReport emailReport)
        {

            try
            {
             

                using (Connection = connection.returnConnection())
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyID", emailReport.staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleID", emailReport.staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserID", emailReport.staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@ClientID", emailReport.ClientID));
                    command.Parameters.Add(new SqlParameter("@ParentID", emailReport.ParentID));
                    command.Parameters.Add(new SqlParameter("@ReferenceID", emailReport.ReferenceID));
                    command.Parameters.Add(new SqlParameter("@EmailType", (int)emailReport.EmailType));
                    command.Parameters.Add(new SqlParameter("@EmailStatus", (int)emailReport.EmailStatus));
                    command.CommandText = "USP_AddClientEmailReport";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = Connection;
                    Connection.Open();
                   await Task.Run(() => command.ExecuteNonQuery());
                    return (int)emailReport.EmailStatus;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return 0;
            }
            finally
            {
                Connection.Dispose();
                command.Dispose();
            }

        }

        public  async Task<T> SendEmailSubstituteRoleModified<T>(StaffDetails staff, long SubstituteID,int emailType,string mailTemplatePath, params object[] list)
        {
            
            try
            {

            

                string toEmailAddress = string.Empty;
                string fromEmailAddress = string.Empty;

                string emailMessage = string.Empty;

                string assignSubRole = "Today, you were assigned to be a substitute teacher for classroom <strong>[classroom name]</strong> at center <strong>[center name]</strong>. The dates are <strong>[date from]</strong> to <strong>[date to]</strong>. When you log onto the system use your alternate role called substitute teacher to see the classroom screens.";
                string cancelSubRole = "Your upcoming assignment as a substitute teacher has been canceled for classroom <strong>[classroom name]</strong> at center <strong>[center name]</strong> on the following dates <strong>[date from]</strong> to <strong>[date to]</strong>.";
            
                byte[] logoImage=null;
                string subject = string.Empty;

                 XmlNodeList xmlnode;
                 XmlDocument xmlDoc = new XmlDocument();
                string footer = string.Empty;
                string body = string.Empty;

                xmlDoc.Load(mailTemplatePath + "\\SubstituteRole.xml");
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Footer");
                footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;


                emailMessage = body;

                //StreamReader streamReader = new StreamReader(string.Concat(mailTemplatePath + "/SubstituteRole.xml"));
                //emailMessage = streamReader.ReadToEnd();
             

                var dbManager = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<FingerprintsDataAccessHandler.DBManager>(connection.ConnectionString);

                var parameters = new IDbDataParameter[]
                {
                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@SubstituteID",SubstituteID,DbType.Int64),
                };

                DataSet dataSet = dbManager.GetDataSet("USP_SubstituteRole_EmailData", CommandType.StoredProcedure, parameters);

                if(dataSet != null && dataSet.Tables.Count>0)
                {
                   if(dataSet.Tables[0].Rows.Count>0)
                    {

                        toEmailAddress = Convert.ToString(dataSet.Tables[0].Rows[0]["EmailAddress"]);


                        emailMessage = emailMessage.Replace("$Name", string.Concat(Convert.ToString(dataSet.Tables[0].Rows[0]["FirstName"]), " ", Convert.ToString(dataSet.Tables[0].Rows[0]["LastName"])).Trim());

                        if (emailType==1)
                        {

                            assignSubRole= assignSubRole.Replace("[classroom name]", Convert.ToString(dataSet.Tables[0].Rows[0]["ClassroomName"]).Trim());
                            assignSubRole= assignSubRole.Replace("[center name]", Convert.ToString(dataSet.Tables[0].Rows[0]["CenterName"]).Trim());
                            assignSubRole= assignSubRole.Replace("[date from]", Convert.ToString(dataSet.Tables[0].Rows[0]["FromDate"]).Trim());
                            assignSubRole= assignSubRole.Replace("[date to]", Convert.ToString(dataSet.Tables[0].Rows[0]["ToDate"]).Trim());

                           
                        }
                        else
                        {

                            cancelSubRole= cancelSubRole.Replace("[classroom name]", Convert.ToString(dataSet.Tables[0].Rows[0]["ClassroomName"]).Trim());
                            cancelSubRole= cancelSubRole.Replace("[center name]", Convert.ToString(dataSet.Tables[0].Rows[0]["CenterName"]).Trim());
                            cancelSubRole= cancelSubRole.Replace("[date from]", Convert.ToString(dataSet.Tables[0].Rows[0]["FromDate"]).Trim());
                            cancelSubRole= cancelSubRole.Replace("[date to]", Convert.ToString(dataSet.Tables[0].Rows[0]["ToDate"]).Trim());
                        }

                        emailMessage = emailMessage.Replace("$CenterName$", Convert.ToString(dataSet.Tables[0].Rows[0]["CenterName"]).Trim());
                        emailMessage = emailMessage.Replace("$CenterAddress$", Convert.ToString(dataSet.Tables[0].Rows[0]["Address"]).Trim());
                        emailMessage = emailMessage.Replace("$City$", Convert.ToString(dataSet.Tables[0].Rows[0]["City"]).Trim());
                        emailMessage = emailMessage.Replace("$State$", Convert.ToString(dataSet.Tables[0].Rows[0]["State"]).Trim());
                        emailMessage = emailMessage.Replace("$Zip$", Convert.ToString(dataSet.Tables[0].Rows[0]["Zip"]).Trim());
                    }

                   if(dataSet.Tables.Count>1 && dataSet.Tables[1]!=null && dataSet.Tables[1].Rows.Count>0)
                    {

                        fromEmailAddress = Convert.ToString(dataSet.Tables[1].Rows[0]["EmailAddress"]).Trim();

                        emailMessage = emailMessage.Replace("$FromName$", string.Concat(Convert.ToString(dataSet.Tables[1].Rows[0]["FirstName"]), " ", Convert.ToString(dataSet.Tables[1].Rows[0]["LastName"]).Trim()));


                        emailMessage = emailMessage.Replace("$Designation$","Center Manager");

                        logoImage = dataSet.Tables[1].Rows[0]["Logo"] != DBNull.Value ? (byte[])dataSet.Tables[1].Rows[0]["Logo"] : new byte[0];

                        emailMessage = emailMessage.Replace("$alt$", Convert.ToString(dataSet.Tables[1].Rows[0]["AgencyName"]));
                    }

                }

                if(emailType==1)
                {
                    emailMessage= emailMessage.Replace("$MessageBody", assignSubRole);

                    subject = "Substitute teacher role has been assigned.";
                }
                else
                {
                    emailMessage= emailMessage.Replace("$MessageBody", cancelSubRole);

                    subject = "Assignment of substitute teacher role has been canceled.";
                }

                if (await Task.Run(() => SendMail.SendEmailWithTask(fromEmailAddress, toEmailAddress, emailMessage, subject, logoImage)))
                {

                    return (T)Convert.ChangeType(true, typeof(T));
                }
                else
                {
                    return (T)Convert.ChangeType(false, typeof(T));
                }


            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
                return (T)Convert.ChangeType(false, typeof(T));
            }
         
  
         
        }
    }
}
