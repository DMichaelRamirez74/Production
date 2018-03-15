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
  public class RaceSubcategoryData
    {
      public RaceSubcategory GetData_AllDropdown(int i = 0, Guid id = new Guid(), RaceSubcategory staff = null)
      {
          RaceSubcategory _staff = new RaceSubcategory();

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

              if (ds.Tables[2].Rows.Count > 0)
              {
                  try
                  {
                      List<RaceSubcategory.RaceInfo> _racelist = new List<RaceSubcategory.RaceInfo>();
                      //_staff.myList
                      foreach (DataRow dr in ds.Tables[2].Rows)
                      {
                          RaceSubcategory.RaceInfo obj = new RaceSubcategory.RaceInfo();
                          obj.RaceId = dr["Id"].ToString();
                          obj.Name = dr["Name"].ToString();
                          _racelist.Add(obj);
                      }
                     
                      _staff.raceList = _racelist;
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
          return _staff;
      }

      public string addeditRaceInfo(RaceSubcategory raceinfo)
      {
          try
          {
              using (SqlConnection Connection = connection.returnConnection())
              {
                  using (SqlCommand command = new SqlCommand())
                  {
                      command.Connection = Connection;
                      command.CommandText = "SP_RaceSubcategorydetails";

                      command.Parameters.AddWithValue("@Operation", raceinfo.RaceSubCategoryID != 0 ? 1 : Convert.ToInt32(0));
                      command.Parameters.AddWithValue("@RaceSubCategoryID", raceinfo.RaceSubCategoryID);
                      command.Parameters.AddWithValue("@RaceID", raceinfo.RaceID);
                      command.Parameters.AddWithValue("@SubcategoryName", raceinfo.SubCategoryName);
                      command.Parameters.AddWithValue("@RaceSubCatDesc", raceinfo.RaceDescription);
                      command.Parameters.AddWithValue("@CreatedBy", raceinfo.CreatedBy);
                      command.Parameters.AddWithValue("@IsActive", raceinfo.IsActive);
                      command.Parameters.AddWithValue("@result","").Direction = ParameterDirection.Output;
                      command.CommandType = CommandType.StoredProcedure;
                      Connection.Open();
                      command.ExecuteNonQuery();
                      Connection.Close();
                      return command.Parameters["@result"].Value.ToString();
                  }
              }
          }
          catch (Exception ex)
          {
              clsError.WriteException(ex);
              return "";
          }
      }

      public List<RaceSubcategory> RaceSubcategorydetails(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize,string agencyId)//, 
      {
          List<RaceSubcategory> _RaceSubcategory = new List<RaceSubcategory>();
          SqlDataAdapter DataAdapter=null;
          DataTable dtRaceSubcategory=null;
          try
          {
              totalrecord = string.Empty;
              string searchAgency = string.Empty;
              if (string.IsNullOrEmpty(search.Trim()))
                  searchAgency = string.Empty;
              else
                  searchAgency = search;
              using (SqlConnection Connection = connection.returnConnection())
              {
                  using (SqlCommand command = new SqlCommand())
                  {
                      command.Connection = Connection;
                      command.CommandText = "Sp_Sel_racesubcategorydesc";
                      command.CommandType = CommandType.StoredProcedure;
                      command.Parameters.Add(new SqlParameter("@Search", searchAgency));
                      command.Parameters.Add(new SqlParameter("@take", pageSize));
                      command.Parameters.Add(new SqlParameter("@skip", skip));
                      command.Parameters.Add(new SqlParameter("@sortcolumn", sortOrder));
                      command.Parameters.Add(new SqlParameter("@sortorder", sortDirection));
                      command.Parameters.Add(new SqlParameter("@agencyid", agencyId));
                      command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                      DataAdapter = new SqlDataAdapter(command);
                      dtRaceSubcategory = new DataTable();
                      DataAdapter.Fill(dtRaceSubcategory);
                      if (dtRaceSubcategory.Rows.Count > 0)
                      {
                          for (int i = 0; i < dtRaceSubcategory.Rows.Count; i++)
                          {
                              RaceSubcategory __RaceSubcategory = new RaceSubcategory();
                              __RaceSubcategory.SubCategoryName = Convert.ToString(dtRaceSubcategory.Rows[i]["SubcategoryName"]);
                              __RaceSubcategory.RaceCategoryName = Convert.ToString(dtRaceSubcategory.Rows[i]["Name"]);
                              __RaceSubcategory.RaceID = Convert.ToString(dtRaceSubcategory.Rows[i]["RaceID"]);
                              __RaceSubcategory.RaceDescription = Convert.ToString(dtRaceSubcategory.Rows[i]["Description"]);
                              __RaceSubcategory.RaceSubCategoryID = Convert.ToInt32(dtRaceSubcategory.Rows[i]["RaceSubCategoryID"]);
                              __RaceSubcategory.IsActive = Convert.ToBoolean(dtRaceSubcategory.Rows[i]["IsActive"]);
                              __RaceSubcategory.CreatedDate =  Convert.ToDateTime(dtRaceSubcategory.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");
                              _RaceSubcategory.Add(__RaceSubcategory);
                          }
                          totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                      }
                  }
              }
              return _RaceSubcategory;
          }
          catch (Exception ex)
          {
              totalrecord = string.Empty;
              clsError.WriteException(ex);
              return _RaceSubcategory;
          }
          finally
          {
              if (DataAdapter != null)
              {
                  DataAdapter.Dispose();
              }
              if (dtRaceSubcategory != null)
              {
                  dtRaceSubcategory.Dispose();
              }
          }
      }
    }
}
