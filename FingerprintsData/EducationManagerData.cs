using DocumentFormat.OpenXml.Drawing.Charts;
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsData
{
    public  class EducationManagerData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();  
        DataSet _dataset = null;

        public EducationManager GetEducationDashboard()
        {
            EducationManager per = new EducationManager();

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                StaffDetails staff = new StaffDetails();
                 _dataset = new DataSet();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@Agencyid", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@userid", staff.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_GetTeachersEducationLevel";
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        switch (dr["HighestEducation"].ToString())
                        {
                            case "0":
                                per.EduLevel0 = Convert.ToInt32(dr["EducationCount"]);
                                break;
                            case "2":
                                per.EduLevel2 = Convert.ToInt32(dr["EducationCount"]);
                                break;

                            case "3":
                                per.EduLevel3 = Convert.ToInt32(dr["EducationCount"]);
                                break;
                            case "4":
                                per.EduLevel4 = Convert.ToInt32(dr["EducationCount"]);
                                break;

                        }
                    }
                }

                if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                {
                   
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        if (dr["GettingDegree"]==DBNull.Value)
                        {
                            per.GettingDegree = 0;
                            per.NotGettingDegree = 0;
                        }
                        else
                        {
                            switch (dr["GettingDegree"].ToString())
                            {
                                case "0":
                                    per.GettingDegree = Convert.ToInt32(dr["DegreeCount"]);
                                    break;
                                case "1":
                                    per.NotGettingDegree = Convert.ToInt32(dr["DegreeCount"]);
                                    break;
                            }
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
                Connection.Close();
                command.Dispose();
            }
            return per;
        }


    }
}
