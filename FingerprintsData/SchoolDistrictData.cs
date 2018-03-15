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
   public class SchoolDistrictData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataReader dataReader = null;
        SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable schooldataTable = null;
        public string AddSchool(SchoolDistrict info, int mode, Guid userId, string AgencyId)
        {
            try
            {
                command.Connection = Connection;
                command.CommandText = "SP_addschooldist";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@SchoolDistrictID", info.SchoolDistrictID);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Acronym", (info.Acronym).Trim());
                command.Parameters.AddWithValue("@Description", (info.Description).Trim());
                command.Parameters.AddWithValue("@TransitionDate", info.TransitionDate);
                command.Parameters.AddWithValue("@FormalAgreement", info.FormalAgreement);
                command.Parameters.AddWithValue("@CreatedBy", userId);
                command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
                return command.Parameters["@result"].Value.ToString();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return command.Parameters["@result"].Value.ToString();
        }
        public List<SchoolDistrict> SchoolInfo(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            List<SchoolDistrict> _schoollist = new List<SchoolDistrict>();
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
                command.Parameters.Add(new SqlParameter("@agencyid", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_School_list";
                DataAdapter = new SqlDataAdapter(command);
                schooldataTable = new DataTable();
                DataAdapter.Fill(schooldataTable);
                if (schooldataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < schooldataTable.Rows.Count; i++)
                    {
                        SchoolDistrict addSchoolRow = new SchoolDistrict();
                        addSchoolRow.SchoolDistrictID = Convert.ToInt32(schooldataTable.Rows[i]["SchoolDistrictID"]);
                        addSchoolRow.Acronym = Convert.ToString(schooldataTable.Rows[i]["Acronym"]);
                        addSchoolRow.Description = Convert.ToString(schooldataTable.Rows[i]["Description"]);
                        addSchoolRow.TransitionDate = Convert.ToString(schooldataTable.Rows[i]["TransitionDate"]);
                        addSchoolRow.FormalAgreement = Convert.ToBoolean(schooldataTable.Rows[i]["FormalAgreement"]);
                        addSchoolRow.CreatedDate = Convert.ToDateTime(schooldataTable.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");

                        _schoollist.Add(addSchoolRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _schoollist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _schoollist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                schooldataTable.Dispose();
            }
        }

        public SchoolDistrict Getschoolinfo(string SchoolDistrictID, string AgencyId)
        {
            SchoolDistrict obj = new SchoolDistrict();
            try
            {
                command.Parameters.Add(new SqlParameter("@SchoolDistrictID", SchoolDistrictID));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_schoolinfo";
                DataAdapter = new SqlDataAdapter(command);
                schooldataTable = new DataTable();
                DataAdapter.Fill(schooldataTable);
                if (schooldataTable != null && schooldataTable.Rows.Count > 0)
                {
                    obj.SchoolDistrictID = Convert.ToInt32(schooldataTable.Rows[0]["SchoolDistrictID"]);
                    obj.Acronym = schooldataTable.Rows[0]["Acronym"].ToString();
                    obj.TransitionDate = schooldataTable.Rows[0]["TransitionDate"].ToString();
                    obj.Description = schooldataTable.Rows[0]["Description"].ToString();
                    obj.CreatedDate = Convert.ToDateTime(schooldataTable.Rows[0]["DateEntered"]).ToString("MM/dd/yyyy");
                    obj.FormalAgreement = Convert.ToBoolean(schooldataTable.Rows[0]["FormalAgreement"].ToString());
                    


                }
                DataAdapter.Dispose();
                command.Dispose();
                schooldataTable.Dispose();
                return obj;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return obj;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                schooldataTable.Dispose();
            }
        }


        public string Deleteschoolinfo(string SchoolDistrictID, string AgencyId)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteschoolinfo";
                command.Parameters.Add(new SqlParameter("@SchoolDistrictID", SchoolDistrictID));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
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
    }
}
