using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FingerprintsModel;
using System.Data;
using FingerprintsDataAccessHandler;
using Fingerprints.Common;

namespace FingerprintsData
{
    public class HealthManagerData
    {
        IDbConnection _connection;
        IDataReader reader;
        DBManager dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);

        public HealthManagerDashboard GetHealthManagerDashboard(StaffDetails staff)
        {


            var healthManagerDashboard = FactoryInstance.Instance.CreateInstance<HealthManagerDashboard>();
          
            try
            {

              


              

                var parameters = new IDbDataParameter[]
                {

                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid)

                };

                 reader = dbManager.GetDataReader("USP_GetHealthManagerDashboard", CommandType.StoredProcedure, parameters, out _connection);





                //healthManagerDashboard.ScreeningMatrix = reader.DataRecord().Select(x => new ScreeningMatrix
                //{
                //    ScreeningID = Convert.ToInt32(reader["ScreeningID"]),
                //        ScreeningName = Convert.ToString(reader["ScreeningName"]),
                //        UptoDate = Convert.ToInt64(reader["UptoDate"]),
                //        Expired = Convert.ToInt64(reader["Expired"]),
                //        Expiring = Convert.ToInt64(reader["Expiring"]),
                //        Missing = Convert.ToInt64(reader["Missing"])

                //});


                var screeningList =new List<ScreeningMatrix>();

                while (reader.Read())
                {


                    screeningList.Add(


                                         new ScreeningMatrix
                    {
                        ScreeningID =reader["ScreeningID"]==DBNull.Value?0: Convert.ToInt32(reader["ScreeningID"]),
                        ScreeningName = reader["ScreeningName"]==DBNull.Value?string.Empty:Convert.ToString(reader["ScreeningName"]),
                        UptoDate = reader["UptoDate"]==DBNull.Value?0: Convert.ToInt64(reader["UptoDate"]),
                        Expired = reader["Expired"]==DBNull.Value?0:Convert.ToInt64(reader["Expired"]),
                        Expiring =reader["Expiring"]==DBNull.Value?0: Convert.ToInt64(reader["Expiring"]),
                        Missing =reader["Missing"]==DBNull.Value?0: Convert.ToInt64(reader["Missing"])
                    });
                }


                var screeningReviewList = new List<NDaysScreeningReview>();

                if(reader.NextResult())
                {
                    while(reader.Read())
                    {
                        screeningReviewList.Add(new NDaysScreeningReview
                        {

                            ScreeningID = reader["ScreeningID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ScreeningID"]),
                            ScreeningName = reader["ScreeningName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ScreeningName"]),
                            Completed = reader["Completed"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Completed"]),
                            CompletedButLate = reader["CompletedButLate"] == DBNull.Value ? 0 : Convert.ToInt64(reader["CompletedButLate"]),
                            NotExpired = reader["NotExpired"] == DBNull.Value ? 0 : Convert.ToInt64(reader["NotExpired"]),
                            NotCompletedandLate = reader["NotCompletedandLate"] == DBNull.Value ? 0 : Convert.ToInt64(reader["NotCompletedandLate"])

                        });


                    }

                }

                if(reader.NextResult())
                {
                    while(reader.Read())
                    {
                        healthManagerDashboard.AccessScreeningMatrix = reader["AccessScreeningMatrix"] == DBNull.Value ? false : Convert.ToBoolean(reader["AccessScreeningMatrix"]);
                        healthManagerDashboard.AccessScreeningReview = reader["AccessScreeningReview"] == DBNull.Value ? false : Convert.ToBoolean(reader["AccessScreeningReview"]);
                    }
                }

                


                healthManagerDashboard.ScreeningMatrix = screeningList.AsEnumerable();
                healthManagerDashboard.ScreeningReview = screeningReviewList.AsEnumerable();



            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                reader.Close();
                dbManager.CloseConnection(_connection);
            }

         


            return healthManagerDashboard;
           
        }

    }
}
