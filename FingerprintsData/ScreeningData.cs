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

      

        public ScreeningMatrixReport GetScreeningMatrixReport(ScreeningMatrixReport matrixReport, StaffDetails staff)
        {
            try
            {

                matrixReport.ScreeningMatrix = FactoryInstance.Instance.CreateInstance<List<ScreeningMatrix>>();
                matrixReport.CenterID = string.IsNullOrEmpty(matrixReport.CenterID) ? "0" : matrixReport.CenterID;
                matrixReport.ClassroomID = string.IsNullOrEmpty(matrixReport.ClassroomID) ? "0" : matrixReport.ClassroomID;
                matrixReport.ScreeningID = string.IsNullOrEmpty(matrixReport.ScreeningID) ? "0" : matrixReport.ScreeningID;

                var dbManager = new DBManager(connection.ConnectionString);

                IDbConnection _connection;
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

                IDataReader reader = dbManager.GetDataReader("USP_GetScreeningMatrixReport", CommandType.StoredProcedure, parameters, out _connection);

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
                    dbManager.CloseConnection(_connection);

                }


                matrixReport.TotalRecord = Convert.ToInt32(parameters.Where(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output).Select(x => x.Value).First());


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return matrixReport;
        }


        public ScreeningMatrixReport GetScreeningMatrixDashboard(StaffDetails staffDetails)
        {
            ScreeningMatrixReport screeningMatrixReport = FactoryInstance.Instance.CreateInstance<ScreeningMatrixReport>();
            try
            {

            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return screeningMatrixReport;
        }


        public List<ScreeningNew> GetScreeningsByUserAccess(StaffDetails staffDetails)
        {

            List<ScreeningNew> screeningList = new List<ScreeningNew>();
            try
            {
               const string parameterString = "select distinct ScreeningID,ScreeningName from fn_GetAccessScreeningsByRole(@AgencyID,@RoleID,@UserID)";

               var dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);

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

    }
}
