using FingerprintsDataAccessHandler;
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.Common;
using System.Web.Mvc;

namespace FingerprintsData
{
    public class ScreeningData
    {
        IDbConnection dbConnection;
        DBManager dbManager;
        IDataReader reader;

        public ScreeningData()
        {
            dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);
        }


     

        public ScreeningMatrixReport GetScreeningMatrixReport(ScreeningMatrixReport matrixReport, StaffDetails staff)
        {
            try
            {

                matrixReport.ScreeningMatrix = FactoryInstance.Instance.CreateInstance<List<ScreeningMatrix>>();
                matrixReport.CenterID = string.IsNullOrEmpty(matrixReport.CenterID) ? "0" : matrixReport.CenterID;
                matrixReport.ClassroomID = string.IsNullOrEmpty(matrixReport.ClassroomID) ? "0" : matrixReport.ClassroomID;
                matrixReport.ScreeningID = string.IsNullOrEmpty(matrixReport.ScreeningID) ? "0" : matrixReport.ScreeningID;

             //   var dbManager = new DBManager(connection.ConnectionString);

                
                var parameters = new IDbDataParameter[]
                {


                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@CenterIDs",matrixReport.CenterID,DbType.AnsiString),
                    dbManager.CreateParameter("@ClassroomIDs",matrixReport.ClassroomID,DbType.AnsiString),
                    dbManager.CreateParameter("@ScreeningIDs", matrixReport.ScreeningID, DbType.AnsiString),
                    dbManager.CreateParameter("@Take",matrixReport.PageSize,DbType.Int32),
                    dbManager.CreateParameter("@Skip",matrixReport.SkipRows,DbType.Int32),
                    dbManager.CreateParameter("@SortOrder",matrixReport.SortOrder,DbType.String),
                    dbManager.CreateParameter("@SortColumn",matrixReport.SortColumn,DbType.String),
                    dbManager.CreateParameter("@SearchTerm",matrixReport.SearchTerm,DbType.String),
                    dbManager.CreateParameter("@TotalRecord",int.MaxValue,0,DbType.Int32,ParameterDirection.Output)

                };

                //DataSet datset = dbManager.GetDataSet("USP_GetScreeningMatrixReport", CommandType.StoredProcedure, parameters);

                 reader = dbManager.GetDataReader("USP_GetScreeningMatrixReport", CommandType.StoredProcedure, parameters, out dbConnection);

                try
                {
                    while (reader.Read())
                    {

                        matrixReport.ScreeningMatrix.Add(new ScreeningMatrix
                        {

                            CenterID =reader["CenterID"]==DBNull.Value?"0": EncryptDecrypt.Encrypt64(Convert.ToString(reader["CenterID"])),
                            CenterName =reader["CenterName"]==DBNull.Value?string.Empty: Convert.ToString(reader["CenterName"]),
                            ClassroomID=reader["ClassroomID"]==DBNull.Value?"0": EncryptDecrypt.Encrypt64(Convert.ToString(reader["ClassroomID"])),
                            ClassroomName=reader["ClassroomName"]==DBNull.Value?string.Empty: Convert.ToString(reader["ClassroomName"]),
                            ScreeningID =reader["ScreeningID"]==DBNull.Value?0: Convert.ToInt32(reader["ScreeningID"]),
                            ScreeningName=reader["ScreeningName"]==DBNull.Value?string.Empty: Convert.ToString(reader["ScreeningName"]),
                            UptoDate = reader["UptoDate"]==DBNull.Value?0: Convert.ToInt64(reader["UptoDate"]),
                            Missing =reader["Missing"]==DBNull.Value?0: Convert.ToInt64(reader["Missing"]),
                            Expired = reader["Expired"]==DBNull.Value?0:Convert.ToInt64(reader["Expired"]),
                            Expiring =reader["Expiring"]==DBNull.Value?0: Convert.ToInt64(reader["Expiring"]),
                            StepUpToQualityStars=reader["StepUpToQualityStars"]==DBNull.Value?string.Empty:Convert.ToString(reader["StepUpToQualityStars"])
                            
                        });
                    }


                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }
                finally
                {
                    reader.Close();
                    dbManager.CloseConnection(dbConnection);

                }


                matrixReport.TotalRecord = Convert.ToInt32(parameters.Where(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output).Select(x => x.Value).First());


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return matrixReport;
        }


        //public ScreeningMatrixReport GetScreeningMatrixDashboard(StaffDetails staffDetails)
        //{
        //    ScreeningMatrixReport screeningMatrixReport = FactoryInstance.Instance.CreateInstance<ScreeningMatrixReport>();
        //    try
        //    {

        //    }
        //    catch(Exception ex)
        //    {
        //        clsError.WriteException(ex);
        //    }
        //    return screeningMatrixReport;
        //}


        public List<ScreeningNew> GetScreeningsByUserAccess(StaffDetails staffDetails)
        {

            List<ScreeningNew> screeningList = new List<ScreeningNew>();
            try
            {
               const string parameterString = "select distinct ScreeningID,ScreeningName from fn_GetAccessScreeningsByRole(@AgencyID,@RoleID,@UserID)";

               //var dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);

                var parameters = new IDbDataParameter[]
                {
                    dbManager.CreateParameter("@AgencyID",staffDetails.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staffDetails.UserId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staffDetails.RoleId,DbType.Guid)
                };

               

               var _dataTable = dbManager.GetDataTable(parameterString, CommandType.Text, parameters);


                screeningList =(from DataRow dr in _dataTable.Rows

                                 select new ScreeningNew
                                 {
                                     ScreeningID = Convert.ToInt32(dr["ScreeningID"]),
                                     ScreeningName = Convert.ToString(dr["ScreeningName"])
                                 }
                               ).OrderBy(x=>x.ScreeningName).ToList();

            }

            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }

            return screeningList;


        }


        public List<ScreeningReportPeriods> GetScreeningReportPeriods(StaffDetails staff)
        {

          
            List<ScreeningReportPeriods> screeningReportPeriodsList = new List<ScreeningReportPeriods>();

            try
            {
               // var dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);

                var parameters = new IDbDataParameter[]
                {


                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@ScreeningPeriodID",0,DbType.Int32)
                };

                DataTable _dataTable = dbManager.GetDataTable("USP_GetScreeningReportPeriods", CommandType.StoredProcedure, parameters);


                if(_dataTable != null && _dataTable.Rows.Count>0)
                {

                    screeningReportPeriodsList = Fingerprints.Common.DbHelper.DataTableToList<ScreeningReportPeriods>(_dataTable, new List<string>());
                }

              
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return screeningReportPeriodsList;
        }

        public List<SelectListItem> GetScreeningByReportDays(StaffDetails staff, int screeningReportPeriodID,int mode)
        {


            List<SelectListItem> screeningList = new List<SelectListItem>();

            try
            {
                var dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);

                var parameters = new IDbDataParameter[]
                {


                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@ScreeningReportPeriodID",screeningReportPeriodID,DbType.Int32),
                    dbManager.CreateParameter("@mode",mode,DbType.Int32)
                };

                DataTable _dataTable = dbManager.GetDataTable("USP_GetScreeningByReportDays", CommandType.StoredProcedure, parameters);


                if (_dataTable != null && _dataTable.Rows.Count > 0)
                {

                    screeningList = (from DataRow dr in _dataTable.Rows
                                     select new SelectListItem
                                     {
                                         Text = Convert.ToString(dr["ScreeningName"]),
                                         Value = EncryptDecrypt.Encrypt64(Convert.ToString(dr["ScreeningID"]))
                                       
                                     }).ToList();
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return screeningList;
        }


        public NDayScreeningReviewReport GetScreeningReviewReport(StaffDetails staff, NDayScreeningReviewReport modal)
        {

         
            try
            {

                modal.NDayScreeningReviewList = new List<NDaysScreeningReview>();
                modal.CenterID = string.Join(",", modal.CenterID.Split(',').Select(x => EncryptDecrypt.Decrypt64(x)));
                modal.ScreeningID = string.Join(",", modal.ScreeningID.Split(',').Select(x => EncryptDecrypt.Decrypt64(x)));
                modal.ClassroomID = modal.ClassroomID != "" && modal.ClassroomID != "0" ? string.Join(",", modal.ClassroomID.Split(',').Select(x => EncryptDecrypt.Decrypt64(x))):modal.ClassroomID;

                var parameters = new IDbDataParameter[]
                {

                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),

                    dbManager.CreateParameter("@ScreeningReportPeriodID",modal.ScreeningReportPeriodID,DbType.Int32),
                    dbManager.CreateParameter("@CenterIDs",modal.CenterID,DbType.AnsiString),
                    dbManager.CreateParameter("@ClassroomIDs",modal.ClassroomID,DbType.AnsiString),
                    dbManager.CreateParameter("@ScreeningIDs", modal.ScreeningID, DbType.AnsiString),
                    dbManager.CreateParameter("@Take",modal.PageSize,DbType.Int32),
                    dbManager.CreateParameter("@Skip",modal.SkipRows,DbType.Int32),
                    dbManager.CreateParameter("@SortOrder",modal.SortOrder,DbType.String),
                    dbManager.CreateParameter("@SortColumn",modal.SortColumn,DbType.String),
                    dbManager.CreateParameter("@SearchTerm",modal.SearchTerm,DbType.String),

                    dbManager.CreateParameter("@TotalRecord",int.MaxValue,0,DbType.Int32,ParameterDirection.Output)
                };

                reader = dbManager.GetDataReader("USP_GetScreeningReviewReport", CommandType.StoredProcedure, parameters, out dbConnection);


                while(reader.Read())
                {
                    modal.NDayScreeningReviewList.Add(new NDaysScreeningReview
                    {
                        CenterID=reader["CenterID"]==DBNull.Value?"0":Convert.ToString(reader["CenterID"]),
                        CenterName=reader["CenterName"]==DBNull.Value?"0":Convert.ToString(reader["CenterName"]),
                        ClassroomID=reader["ClassroomID"]==DBNull.Value?"0":Convert.ToString(reader["ClassroomID"]),
                        ClassroomName=reader["ClassroomName"]==DBNull.Value?string.Empty:Convert.ToString(reader["ClassroomName"]),
                        ScreeningID=reader["ScreeningID"]==DBNull.Value?0:Convert.ToInt32(reader["ScreeningID"]),
                        ScreeningName =reader["ScreeningName"]==DBNull.Value?string.Empty:Convert.ToString(reader["ScreeningName"]),
                        Completed=reader["Completed"]==DBNull.Value?0:Convert.ToInt64(reader["Completed"]),
                        CompletedButLate=reader["CompletedButLate"]==DBNull.Value?0:Convert.ToInt64(reader["CompletedButLate"]),
                        NotExpired=reader["NotExpired"]==DBNull.Value?0:Convert.ToInt64(reader["NotExpired"]),
                        NotCompletedandLate=reader["NotCompletedandLate"]==DBNull.Value?0:Convert.ToInt64(reader["NotCompletedandLate"]),
                        StepUpToQualityStars=reader["StepUpToQualityStars"]==DBNull.Value?"0":Convert.ToString(reader["StepUpToQualityStars"])

                    });
                }

                reader.Close();
                dbManager.CloseConnection(dbConnection);

                modal.TotalRecord = (int)parameters.Where(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output).Select(x => x.Value).First();

            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
               
            }

            

            return modal;
        }



        #region Screening Follow-up Report

        public ScreeningFollowupReport GetScreeningFollowupReport(StaffDetails staff, ScreeningFollowupReport modal)
        {


            try
            {

                Questions qn ;
                modal.ScreeningFollowupList = new List<ScreeningFollowup>();
                var centerIDs = string.Join(",", modal.CenterIDs.Select(x => EncryptDecrypt.Decrypt64(x)));
                var screeningIDs = string.Join(",", modal.ScreeningIDs.Select(x => EncryptDecrypt.Decrypt64(x)));
                var classroomIDs = modal.ClassroomIDs!=null && modal.ClassroomIDs.Length > 0 ? string.Join(",", modal.ClassroomIDs.Select(x => x!="0"? EncryptDecrypt.Decrypt64(x):x)) : "0";

                var parameters = new IDbDataParameter[]
                {

                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),

                    dbManager.CreateParameter("@CenterIDs",centerIDs,DbType.AnsiString),
                    dbManager.CreateParameter("@ClassroomIDs",classroomIDs,DbType.AnsiString),
                    dbManager.CreateParameter("@ScreeningIDs", screeningIDs, DbType.AnsiString),
                    dbManager.CreateParameter("@Take",modal.PageSize,DbType.Int32),
                    dbManager.CreateParameter("@Skip",modal.SkipRows,DbType.Int32),
                    dbManager.CreateParameter("@SortOrder",modal.SortOrder,DbType.String),
                    dbManager.CreateParameter("@SortColumn",modal.SortColumn,DbType.String),
                    dbManager.CreateParameter("@SearchTerm",modal.SearchTerm,DbType.String),

                    dbManager.CreateParameter("@TotalRecord",int.MaxValue,0,DbType.Int32,ParameterDirection.Output)
                };

                reader = dbManager.GetDataReader("USP_GetScreeningFollowupReport", CommandType.StoredProcedure, parameters, out dbConnection);


                while (reader.Read())
                {
                    modal.ScreeningFollowupList.Add(new ScreeningFollowup
                    {
                        CenterID = reader["CenterID"] == DBNull.Value ? "0" : Convert.ToString(reader["CenterID"]),
                        CenterName = reader["CenterName"] == DBNull.Value ? "0" : Convert.ToString(reader["CenterName"]),
                        ClassroomID = reader["ClassroomID"] == DBNull.Value ? "0" : Convert.ToString(reader["ClassroomID"]),
                        ClassroomName = reader["ClassroomName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ClassroomName"]),
                        ClientId = reader["ClientID"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ClientID"]),
                        ClientName = string.Concat(reader["FirstName"].ToString(), " ", reader["MiddleName"], " ", reader["LastName"]),
                        Dob = Convert.ToString(reader["DOB"]),
                        AgeInWords = Convert.ToString(reader["AgeInWords"]).Replace(" Years","Y").Replace(" Months","M").Replace(" Days","D"),
                        ProgramType = Convert.ToString(reader["ProgramType"]),
                        DateOfFirstService = Convert.ToString(reader["DateOfFirstService"]),
                        ScreeningQuestion = new ScreeningQ
                        {

                            ScreeningId = reader["ScreeningID"] == DBNull.Value ? "0" : Convert.ToString(reader["ScreeningID"]),
                            ScreeningName = reader["ScreeningName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ScreeningName"]),
                            Questionlist = new List<Questions>().ToList(),
                        },
                        ScreeningPeriods=new ScreeningPeriods
                        {
                            Description= reader["Description"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Description"]),
                            ScreeningPeriodIndex= reader["ScreeningPeriodID"] == DBNull.Value ? 0 : Convert.ToInt64(reader["ScreeningPeriodID"]),
                        },
                        StepUpToQualityStars = reader["StepUpToQualityStars"] == DBNull.Value ? "0" : Convert.ToString(reader["StepUpToQualityStars"]),
                        ScreeningDate=reader["ScreeningDate"]==DBNull.Value?string.Empty:Convert.ToString(reader["ScreeningDate"])
                        
                    });

                    modal.ScreeningFollowupList.Last().ScreeningQuestion.Questionlist.Add(new Questions
                    {
                        Question = reader["Question"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Question"]),
                    });
                  
                }

                reader.Close();
                dbManager.CloseConnection(dbConnection);

                modal.TotalRecord = (int)parameters.Where(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output).Select(x => x.Value).First();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

                if(reader!=null && !reader.IsClosed)
                {
                    reader.Close();
                    dbManager.CloseConnection(dbConnection);
                }
            
            }
            finally
            {

            }



            return modal;
        }



        #endregion
    }
}
