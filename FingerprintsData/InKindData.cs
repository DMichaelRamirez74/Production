﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using FingerprintsModel;
using System.Globalization;

namespace FingerprintsData
{
    /// <summary>
    /// 
    /// </summary>
    public class InKindData
    {
        private SqlConnection _connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataSet dataset;

        /// <summary>
        /// Public Constructor to initialize the Class members and closes the SQL Connection,In case of in open state.
        /// </summary>
        public InKindData()
        {
            this._connection = connection.returnConnection();
            this.command = new SqlCommand();
            this.adapter = new SqlDataAdapter(this.command);
            this.dataset = new DataSet();
            if (this._connection.State == ConnectionState.Open)
            {
                this._connection.Close();
            }
        }


        /// <summary>
        /// Get the existing In-kind Parent or Company Donors based on agency from the database.
        /// </summary>
        /// <param name="staffDetails">StafDetails</param>
        /// <param name="searchText">string</param>
        /// <returns>Inkind</returns>
        public Inkind GetInkindParentCompanyDonors(StaffDetails staffDetails, string searchText)
        {

            Inkind inkind = new Inkind();
            inkind.InkindDonorsList = new List<InkindDonors>();
            try
            {
                using (_connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", staffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", staffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", staffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@SearchText", searchText));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetInkindDonors";
                    _connection.Open();
                    dataset = new DataSet();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataset);
                    _connection.Close();

                    if (dataset != null && dataset.Tables.Count > 0)
                    {
                        inkind.InkindDonorsList = (from DataRow dr in dataset.Tables[0].Rows
                                                   select new InkindDonors
                                                   {
                                                       Name = dr["Name"].ToString(),
                                                       EmailAddress = dr["EmailId"].ToString(),
                                                       Address = dr["Address"].ToString(),
                                                       State = dr["State"].ToString(),
                                                       ZipCode = dr["ZipCode"].ToString(),
                                                       InkindDonorId = dr["ClientId"].ToString(),
                                                       CompanyName = dr["CompanyName"].ToString(),
                                                       PhoneNoList = (dataset.Tables[1].Rows.Count > 0) ?
                                                       (from DataRow dr2 in dataset.Tables[1].Rows
                                                        where (Convert.ToInt64(dr2["ClientId"]) == Convert.ToInt64(dr["ClientId"]))
                                                        select new FamilyHousehold.phone
                                                        {
                                                            PhoneNo = dr2["Phoneno"].ToString(),
                                                            PhoneType = dr2["PhoneType"].ToString(),
                                                            ParentId = Convert.ToInt64(dr2["ClientId"])
                                                        }

                                                                                                     ).ToList() : new List<FamilyHousehold.phone>(),
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
                command.Dispose();
                _connection.Dispose();
                dataset.Dispose();
            }
            return inkind;
        }

        /// <summary>
        /// Gets the In-Kind Activities List based on the AgencyId.
        /// </summary>
        /// <param name="details"></param>
        /// <returns>Inkind class</returns>

        public Inkind GetInkindActivities(StaffDetails details,int activityCode=0,int amountType=0,bool getVolunteerBased =false,int activityType=0)
        {

            Inkind inkindDetails = new Inkind();
            int rowscount = 0;

            inkindDetails.InkindActivityList = new List<InkindActivity>();
            try
            {
                using (_connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", details.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@ActivityCode", activityCode));
                    command.Parameters.Add(new SqlParameter("@AmountType", amountType));
                    command.Parameters.Add(new SqlParameter("@GetVolunteerBased", getVolunteerBased));
                    command.Parameters.Add(new SqlParameter("@ActivityType", activityType));
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = _connection;
                    command.CommandText = "USP_GetInkindActivities";
                    _connection.Open();
                    adapter = new SqlDataAdapter(command);
                    dataset = new DataSet();
                    adapter.Fill(dataset);
                    _connection.Close();
                }
                if (dataset != null)
                {
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        inkindDetails.InkindActivityList = (from DataRow dr in dataset.Tables[0].Rows
                                                            select new InkindActivity
                                                            {
                                                                ActivityAmountRate = dr["Rate"].ToString(),
                                                                ActivityDescription = dr["ActivityDescription"].ToString(),
                                                                ActivityAmountType = dr["AmountyType"].ToString(),
                                                                ActivityCode = dr["ActivityCode"].ToString(),
                                                                Volunteer = Convert.ToBoolean(dr["Volunteer"]),
                                                                ActivityType = dr["ActivityType"].ToString(),
                                                                IsSignatureRequired = Convert.ToBoolean(dr["IsSignatureRequired"]),
                                                                SubActivityList = (int.TryParse(dataset.Tables[1].Rows.Count.ToString(), out rowscount)) ? (from DataRow dr1 in dataset.Tables[1].Rows
                                                                                                                                                            where Convert.ToInt32(dr1["ActivityCode"]) == Convert.ToInt32(dr["ActivityCode"])
                                                                                                                                                            select new SubActivities
                                                                                                                                                            {
                                                                                                                                                                ActivityCode = Convert.ToInt32(dr1["ActivityCode"]),
                                                                                                                                                                SubActivityId = Convert.ToInt64(dr1["SubActivityId"]),
                                                                                                                                                                ActivityDescription = dr1["ActivityDescription"].ToString()

                                                                                                                                                            }).ToList() : new List<SubActivities>()


                                                            }
                                                          ).ToList();
                    }

                    if (dataset.Tables.Count > 2)
                    {
                        inkindDetails.HomeActivityCount = Convert.ToInt32(dataset.Tables[2].Rows[0]["HomeActivityCount"]);
                    }
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                dataset.Dispose();
                _connection.Dispose();
            }
            return inkindDetails;

        }

        public bool InsertInkindActivity(InkindActivity activity)
        {
            bool isRowsAffected = false;
            try
            {
                int rowsAffected = 0;
                DataTable actb = new DataTable();
                actb = this.GetSubActivitiesTable(activity);
                using (_connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", activity.StaffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", activity.StaffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", activity.StaffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@ActivityType", activity.ActivityType));
                    command.Parameters.Add(new SqlParameter("@AmountType", activity.ActivityAmountType));
                    command.Parameters.Add(new SqlParameter("@Rate", activity.ActivityAmountRate));
                    command.Parameters.Add(new SqlParameter("@IsSignatureRequired", activity.IsSignatureRequired));
                    command.Parameters.Add(new SqlParameter("@ActivityDescription", activity.ActivityDescription));
                    command.Parameters.Add(new SqlParameter("@ActivityCode", string.IsNullOrEmpty(activity.ActivityCode) ? "0" : activity.ActivityCode));
                    command.Parameters.Add(new SqlParameter("@Volunteer", activity.Volunteer));
                    command.Parameters.Add(new SqlParameter("@SubActivities", actb));
                    command.CommandText = "USP_InsertInkindActivity";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = _connection;
                    _connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                    _connection.Close();
                }

                isRowsAffected = (rowsAffected > 0);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
                _connection.Dispose();
            }
            return isRowsAffected;
        }



        public DataTable GetSubActivitiesTable(InkindActivity activity)
        {
            DataTable activityTable = new DataTable();
            try
            {
                activityTable.Clear();
                activityTable.Columns.AddRange(new DataColumn[9] {
                    new DataColumn("SubActivityId",typeof(long)),
                    new DataColumn("AgencyId",typeof(Guid)),
                    new DataColumn("ActivityCode",typeof(int)),
                    new DataColumn("ActivityDescription ", typeof(string)),
                    new DataColumn("IsActive", typeof(bool)),
                    new DataColumn("CreatedBy",typeof(Guid)),
                    new DataColumn("CreatedDate",typeof(DateTime)){ AllowDBNull=true},
                    new DataColumn("ModifiedBy",typeof(Guid)){ AllowDBNull=true},
                    new DataColumn("ModifiedDate",typeof(DateTime)){ AllowDBNull=true}
                });

                if (activity.SubActivityList.Count() > 0)
                {


                    foreach (var item in activity.SubActivityList)
                    {

                        item.ActivityCode = string.IsNullOrEmpty(activity.ActivityCode) ? 0 : Convert.ToInt32(activity.ActivityCode);
                        activityTable.Rows.Add(
                          item.SubActivityId,
                          activity.StaffDetails.AgencyId,
                          item.ActivityCode,
                          item.ActivityDescription,
                          item.IsActive,
                          activity.StaffDetails.UserId,
                          (DateTime?)null,
                          (Guid?)null,
                          (DateTime?)null
                          );
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return activityTable;
        }
        public bool DeleteInkindActivity(StaffDetails details, string activityCode)
        {
            bool isRowsAffected = false;
            try
            {
                int rowsAffected = 0;
                using (_connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", details.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));

                    command.Parameters.Add(new SqlParameter("@ActivityCode", string.IsNullOrEmpty(activityCode) ? "0" : activityCode));
                    command.CommandText = "USP_DeleteInkindActivity";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = _connection;
                    _connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                    _connection.Close();
                    if (rowsAffected > 0)
                    {
                        isRowsAffected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
                _connection.Dispose();
            }
            return isRowsAffected;
        }

        /// <summary>
        /// method to check whether In-Kind Activity is already exists in the database based on agencyId.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public string CheckActivityExists(InkindActivity activity)
        {
            string executeRecord = "";
            try
            {


                using (_connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", activity.StaffDetails.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", activity.StaffDetails.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", activity.StaffDetails.RoleId));
                    command.Parameters.Add(new SqlParameter("@ActivityType", activity.ActivityType));
                    command.Parameters.Add(new SqlParameter("@AmountType", activity.ActivityAmountType));
                    command.Parameters.Add(new SqlParameter("@Rate", activity.ActivityAmountRate));
                    command.Parameters.Add(new SqlParameter("@IsSignatureRequired", activity.IsSignatureRequired));
                    command.Parameters.Add(new SqlParameter("@ActivityCode", string.IsNullOrEmpty(activity.ActivityCode) ? "0" : activity.ActivityCode));
                    command.Parameters.Add(new SqlParameter("@ActivityDescription", activity.ActivityDescription));
                    command.CommandText = "USP_CheckInkindActivityExists";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = _connection;
                    _connection.Open();
                    executeRecord = Convert.ToString(command.ExecuteScalar());
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
                _connection.Dispose();
            }


            return executeRecord;

        }

        /// <summary>
        /// method to insert the Inkind/Asset Entry by parent or Corporate to the database.
        /// </summary>
        /// <param name="inkindTransactions"></param>
        /// <returns></returns>
        public int InsertInkindTransactions(InKindTransactions inkindTransactions)
        {
            int rowsAffected = 0;
            try
            {

                StaffDetails details = StaffDetails.GetInstance();
                using (_connection)
                {
                    command.Parameters.Clear();
                    command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", details.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@ChildId", inkindTransactions.ClientID));
                    command.Parameters.Add(new SqlParameter("@ParentId", inkindTransactions.ParentID));
                    command.Parameters.Add(new SqlParameter("@ActivityDate", inkindTransactions.ActivityDate));
                    command.Parameters.Add(new SqlParameter("@CenterID", inkindTransactions.CenterID));
                    command.Parameters.Add(new SqlParameter("@ClassroomID", inkindTransactions.ClassroomID));
                    command.Parameters.Add(new SqlParameter("@ActivityID", inkindTransactions.ActivityID));
                    command.Parameters.Add(new SqlParameter("@hours", inkindTransactions.Hours));
                    command.Parameters.Add(new SqlParameter("@minutes", inkindTransactions.Minutes));
                    command.Parameters.Add(new SqlParameter("@ActivityNotes", inkindTransactions.ActivityNotes));
                    command.Parameters.Add(new SqlParameter("@IsCompany", inkindTransactions.IsCompany));
                    command.Parameters.Add(new SqlParameter("@DonorSignature", inkindTransactions.DonorSignature));
                    command.Parameters.Add(new SqlParameter("@StaffSignature", inkindTransactions.StaffSignature));
                    command.Parameters.Add(new SqlParameter("@InKindAmount", inkindTransactions.InKindAmount));
                    command.Parameters.Add(new SqlParameter("@MilesDriven", inkindTransactions.MilesDriven));
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_MarkInkindActivity";
                    command.Connection = _connection;
                    _connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                    _connection.Close();
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
                _connection.Dispose();
                dataset.Dispose();
            }
            return rowsAffected;
        }


        /// <summary>
        /// method to insert or add the In-kind Donars to the database.
        /// </summary>
        /// <param name="corporate"></param>
        /// <returns></returns>
        public long InsertInKindDonors(InKindDonarsContact corporate)
        {
            long identityNo = 0;
            try
            {
                StaffDetails details = new StaffDetails();
                using (_connection)
                {
                    command.Parameters.Clear();
                    //  command.Prepare();
                    command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", details.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@CorporateId", corporate.InKindDonarId));
                    command.Parameters.Add(new SqlParameter("@ContactName", corporate.ContactName));
                    command.Parameters.Add(new SqlParameter("@CorporateName", corporate.CorporateName));
                    command.Parameters.Add(new SqlParameter("@AddressLine1", corporate.AddressLine1));
                    command.Parameters.Add(new SqlParameter("@AddressLine2", corporate.AddressLine2));
                    command.Parameters.Add(new SqlParameter("@ZipCode", corporate.ZipCode));
                    command.Parameters.Add(new SqlParameter("@City", corporate.City));
                    command.Parameters.Add(new SqlParameter("@State", corporate.State));
                    command.Parameters.Add(new SqlParameter("@EmailId", corporate.EmailId));
                    command.Parameters.Add(new SqlParameter("@PhoneNo", corporate.PhoneNo));
                    command.Parameters.Add(new SqlParameter("@ApartmentNo ", corporate.ApartmentNo));
                    command.Parameters.Add(new SqlParameter("@PhoneType", corporate.PhoneType));
                    command.Parameters.Add(new SqlParameter("@IsCompany", corporate.IsCompany));
                    command.Parameters.Add(new SqlParameter("@IsInsert", corporate.IsInsert));
                    command.Parameters.Add(new SqlParameter("@Gender", corporate.Gender));
                    command.Parameters.Add(new SqlParameter("@County", corporate.County));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_InsertInKindDonors";
                    _connection.Open();
                    identityNo = Convert.ToInt64(command.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                command.Dispose();
                _connection.Dispose();
            }
            return identityNo;
        }


        public ParentParticipation GetParentParticipationInkind(StaffDetails details)
        {

            ParentParticipation parentPart = new ParentParticipation();
            try
            {
                using (_connection)
                {
                    command.Parameters.Clear();
                    //  command.Prepare();
                    command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@UserId", details.UserId));
                    command.Parameters.Add(new SqlParameter("@EmailId", details.EmailID));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_GetParentParticipationInKind";
                    dataset = new DataSet();
                    _connection.Open();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataset);
                    _connection.Close();

                }


                if (dataset != null)
                {
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        parentPart.ChildDetailsList = (from DataRow dr1 in dataset.Tables[0].Rows
                                                       select new EnrolledChildren
                                                       {
                                                           ChildrenName = dr1["ChildName"].ToString(),
                                                           ClassRoomId = Convert.ToInt64(dr1["ClassRoomId"].ToString()),
                                                           CenterName = dr1["CenterName"].ToString(),
                                                           CenterId = Convert.ToInt64(dr1["CenterId"].ToString()),
                                                           ClientId = Convert.ToInt64(dr1["ClientId"].ToString()),
                                                           ClassRoomName = dr1["ClassroomName"].ToString()
                                                       }

                                                     ).ToList();

                        parentPart.ParentId = dataset.Tables[0].Rows[0]["ParentId"].ToString();
                        parentPart.ParentId = EncryptDecrypt.Encrypt64(parentPart.ParentId);

                    }

                    if (dataset.Tables[1].Rows.Count > 0)
                    {
                        parentPart.InKindActivityList = (from DataRow dr2 in dataset.Tables[1].Rows
                                                         where dr2["ActivityDescription"].ToString().ToLower() != "parent engagement"
                                                         select new InkindActivity
                                                         {
                                                             ActivityAmountRate = dr2["Rate"].ToString(),
                                                             ActivityDescription = dr2["ActivityDescription"].ToString(),
                                                             ActivityAmountType = dr2["AmountyType"].ToString(),
                                                             ActivityCode = dr2["ActivityCode"].ToString(),
                                                             Volunteer = Convert.ToBoolean(dr2["Volunteer"]),
                                                             ActivityType = dr2["ActivityType"].ToString(),
                                                             IsSignatureRequired = Convert.ToBoolean(dr2["IsSignatureRequired"]),
                                                             SubActivityList = (dataset.Tables.Count > 2 && dataset.Tables[2].Rows.Count > 0) ? (from DataRow dr3 in dataset.Tables[2].Rows
                                                                                                                                                 where Convert.ToInt32(dr3["ActivityCode"]) == Convert.ToInt32(dr2["ActivityCode"])
                                                                                                                                                 select new SubActivities
                                                                                                                                                 {
                                                                                                                                                     ActivityCode = Convert.ToInt32(dr3["ActivityCode"]),
                                                                                                                                                     SubActivityId = Convert.ToInt64(dr3["SubActivityId"]),
                                                                                                                                                     ActivityDescription = dr3["ActivityDescription"].ToString()

                                                                                                                                                 }).ToList() : new List<SubActivities>()


                                                         }
                                                          ).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return parentPart;
        }

        /// <summary>
        /// Method to insert the In-Kind Activities from the Parent Portal
        /// </summary>
        /// <param name="inkindTransList"></param>
        /// <returns></returns>
        public bool InsertParentParticipation(List<InKindTransactions> inkindTransList)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                //List<InKindTransactions> tranList = new List<InKindTransactions>();
                inkindTransList = SetActivityAmtForParent(inkindTransList);
                if (inkindTransList != null && inkindTransList.Count() > 0)
                {
                    foreach (var item in inkindTransList)
                    {
                        returnResult = new InKindData().InsertInkindTransactions(item);
                    }
                }

                isResult = (returnResult == 1);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return isResult;

        }
        /// <summary>
        /// Method to set the Activity Amount for Parent 
        /// </summary>
        /// <param name="transactionsList"></param>
        /// <returns>List<InKindTransactions></returns>
        public List<InKindTransactions> SetActivityAmtForParent(List<InKindTransactions> transactionsList)
        {
            //List<InKindTransactions> transList = new List<InKindTransactions>();
            //transList = transactionsList;

            try
            {
                Inkind inkind = new Inkind();
                StaffDetails details = StaffDetails.GetInstance();
                inkind = new InKindData().GetInkindActivities(details);

                if (inkind.InkindActivityList.Count() > 0 && transactionsList != null && transactionsList.Count() > 0)
                {
                    foreach (var item in transactionsList)
                    {
                        item.ParentID = EncryptDecrypt.Decrypt64(item.ParentID);
                        string actvitytype = "";
                        string ActivityAmountRate = "";

                        if (item.ActivityID > 0)
                        {
                            actvitytype = inkind.InkindActivityList.Where(x => x.ActivityCode == item.ActivityID.ToString()).Select(x => x.ActivityType).FirstOrDefault();
                        }

                        if (actvitytype == "2" || item.ActivityID == 0)
                        {
                            actvitytype = (actvitytype == "") ? (item.Hours > 0 || item.Minutes > 0) ? "2" : (item.MilesDriven > 0) ? "1" : actvitytype : actvitytype;

                            item.ActivityID = inkind.InkindActivityList.Where(x => x.ActivityType == actvitytype).Select(x => Convert.ToInt32(x.ActivityCode)).FirstOrDefault();
                            ActivityAmountRate = inkind.InkindActivityList.Where(x => x.ActivityCode == item.ActivityID.ToString()).Select(x => x.ActivityAmountRate).FirstOrDefault();
                        }

                        else
                        {
                            ActivityAmountRate = inkind.InkindActivityList.Where(x => x.ActivityCode == item.ActivityID.ToString()).Select(x => x.ActivityAmountRate).FirstOrDefault();
                        }


                        if (item.Hours > 0 || item.Minutes > 0)
                        {
                            // var InKindAmount1 = Convert.ToDouble(ActivityAmountRate) * (4 + (45 / 60));//46.56
                            item.InKindAmount = Convert.ToDecimal((Convert.ToDouble(ActivityAmountRate) * (item.Hours + (item.Minutes / 60))).ToString("F", CultureInfo.InvariantCulture));
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return transactionsList;
        }
        /// <summary>
        /// method to delete the Sub-Inkind Activity 
        /// </summary>
        /// <param name="subActivityId"></param>
        /// <returns></returns>
        public bool DeleteInKindSubActivity(int subActivityId)
        {
            bool isRowsAffected = false;
            try
            {
                StaffDetails details = new StaffDetails();
                using (_connection)
                {
                    // command.Prepare();
                    command.Parameters.Add(new SqlParameter("@AgencyId", details.AgencyId));
                    command.Parameters.Add(new SqlParameter("@UserId", details.UserId));
                    command.Parameters.Add(new SqlParameter("@RoleId", details.RoleId));
                    command.Parameters.Add(new SqlParameter("@SubActivityId", subActivityId));
                    command.Connection = _connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_DeleteSubActivity";
                    _connection.Open();
                    isRowsAffected = (command.ExecuteNonQuery() > 0);
                    _connection.Close();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                _connection.Dispose();
                command.Dispose();

            }
            return isRowsAffected;
        }
    }
}
