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
        DBManager dbManager;
        public HealthManagerDashboard GetHealthManagerDashboard(StaffDetails staff)
        {


            var healthManagerDashboard = FactoryInstance.Instance.CreateInstance<HealthManagerDashboard>();
          
            try
            {

              


                 dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);

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
                        ScreeningID = Convert.ToInt32(reader["ScreeningID"]),
                        ScreeningName = Convert.ToString(reader["ScreeningName"]),
                        UptoDate = Convert.ToInt64(reader["UptoDate"]),
                        Expired = Convert.ToInt64(reader["Expired"]),
                        Expiring = Convert.ToInt64(reader["Expiring"]),
                        Missing = Convert.ToInt64(reader["Missing"])
                    });
                }

                //healthManagerDashboard.ScreeningMatrix = Enumerable.Range(0, int.MaxValue).TakeWhile(i => reader.Read())

                //  .Select(x => new ScreeningMatrix
                //  {
                //      ScreeningID = Convert.ToInt32(reader["ScreeningID"]),
                //      ScreeningName = Convert.ToString(reader["ScreeningName"]),
                //      UptoDate = Convert.ToInt64(reader["UptoDate"]),
                //      Expired = Convert.ToInt64(reader["Expired"]),
                //      Expiring = Convert.ToInt64(reader["Expiring"]),
                //      Missing = Convert.ToInt64(reader["Missing"])
                //  });


                healthManagerDashboard.ScreeningMatrix = screeningList.AsEnumerable();



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
