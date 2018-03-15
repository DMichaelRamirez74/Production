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
    public class CategoryData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataReader dataReader = null;
        SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable categorydataTable = null;
        DataSet _dataset = null;
        public List<Categoryinfo> AutoCompleteCategoryInfo(string term, string active = "0")
        {
            List<Categoryinfo> CategoryList = new List<Categoryinfo>();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "AutoComplete_CategoryList";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CategoryName", term);
                        command.Parameters.AddWithValue("@IsDeleted", active);

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
                            Categoryinfo obj = new Categoryinfo();
                            obj.CategoryID = Convert.ToInt32(dr["CategoryId"].ToString());
                            obj.Description = Convert.ToString(dr["Description"].ToString());
                            // obj.CreatedDate = Convert.ToDateTime(dr["DateEntered"]).ToString("MM/dd/yyyy");

                            // obj.AgencyId = (dr["AgencyId"].ToString());
                            //  obj.TimeZoneID = dr["TimeZone_ID"].ToString();

                            CategoryList.Add(obj);
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
            return CategoryList;
        }



        public Categoryinfo GetCategoryList()
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            Categoryinfo Info = new Categoryinfo();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_CategoryInfo";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                    try
                    {
                        List<FingerprintsModel.Categoryinfo.CategoryDetail> _categorylist = new List<FingerprintsModel.Categoryinfo.CategoryDetail>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FingerprintsModel.Categoryinfo.CategoryDetail obj = new FingerprintsModel.Categoryinfo.CategoryDetail();
                            obj.Id = dr["CategoryId"].ToString();
                            obj.Name = dr["Description"].ToString();
                            _categorylist.Add(obj);
                        }
                        //  _rolelist.Insert(0, new Role() { RoleId = "0", RoleName = "Select" });
                        Info.categoryList = _categorylist;
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
            return Info;
        }

        public string AddCategoryInfoAjax(Categoryinfo Info, string CategoryDesc,string CategoryId, string userId, string mode)//string AgencyId
        {
            try
            {

                command.Connection = Connection;
                command.CommandText = "SP_addcategory";
                command.Parameters.AddWithValue("@CategoryID", CategoryId);
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@CategoryDesc", CategoryDesc);
                command.Parameters.AddWithValue("@CreatedBy", userId);
                command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                _dataset = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                try
                {
                    List<FingerprintsModel.Categoryinfo.CategoryDetail> _categorylist = new List<FingerprintsModel.Categoryinfo.CategoryDetail>();

                    if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            FingerprintsModel.Categoryinfo.CategoryDetail obj = new FingerprintsModel.Categoryinfo.CategoryDetail();
                            obj.Id = dr["CategoryID"].ToString();
                            obj.Name = dr["Description"].ToString();
                            _categorylist.Add(obj);
                        }
                    Info.categoryList = _categorylist;
                }
                catch (Exception ex)
                {
                    clsError.WriteException(ex);
                }




                return command.Parameters["@result"].Value.ToString();
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
            return command.Parameters["@result"].Value.ToString();
        }

        public string Deletecategoryinfo(string CategoryID)//, string userId
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deletecategoryinfo";
                command.Parameters.Add(new SqlParameter("@CategoryID", CategoryID));
                // command.Parameters.Add(new SqlParameter("@ModifiedBy", userId));
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

        public string addeditCategory(Categoryinfo categoryDetails, int mode, Guid userId, List<Categoryinfo.ServiceInfo> ServiceData)//, List<Agency.FundSource.ProgramType> Prog
        {

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
                command.CommandText = "Sp_addeditservices_withcategory";//Sp_addeditagency_withfunds   Sp_addeditagency
                command.Parameters.Add(new SqlParameter("@CategoryID", categoryDetails.CategoryID));
                command.Parameters.Add(new SqlParameter("@Description", categoryDetails.Description));


                command.Parameters.Add(new SqlParameter("@mode", mode));
                command.Parameters.Add(new SqlParameter("@result", string.Empty)).Direction = ParameterDirection.Output;
                command.Parameters.Add(new SqlParameter("@createdBy", userId));


                //Category and Service 
                if (ServiceData != null && ServiceData.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[7] { 
                    new DataColumn("Services ", typeof(string)),
                    new DataColumn("CommonTag",typeof(string)), 
                    new DataColumn("PIRMatchID",typeof(string)), 
                    new DataColumn("Status",typeof(string)), 
                     new DataColumn("Year",typeof(int)), 
                    new DataColumn("ServiceID",typeof(Int32)),
                    new DataColumn("Category",typeof(Int32))
                    });


                    foreach (Categoryinfo.ServiceInfo service in ServiceData)
                    {
                        if (service.Description != null)
                        {
                            dt.Rows.Add(service.Description, service.CommonTag, service.PIRMatch, service.Status, (service.Year).Replace("-", ""), service.ServiceID, categoryDetails.CategoryID);//
                        }


                    }
                    command.Parameters.Add(new SqlParameter("@tblservices", dt));
                    //command.Parameters.Add(new SqlParameter("@tblprog", dt1));
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

        public List<Categoryinfo> CategoryInfo(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            List<Categoryinfo> _categorylist = new List<Categoryinfo>();
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
                // command.Parameters.Add(new SqlParameter("@agencyid", userid));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Category_list";
                DataAdapter = new SqlDataAdapter(command);
                categorydataTable = new DataTable();
                DataAdapter.Fill(categorydataTable);
                if (categorydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < categorydataTable.Rows.Count; i++)
                    {
                        Categoryinfo addCategoryRow = new Categoryinfo();
                        addCategoryRow.CategoryID = Convert.ToInt32(categorydataTable.Rows[i]["Category"]);
                        addCategoryRow.Description = Convert.ToString(categorydataTable.Rows[i]["Description"]);

                        addCategoryRow.ServiceID = Convert.ToInt32(categorydataTable.Rows[i]["ServiceID"]);
                        addCategoryRow.ServiceDescription = Convert.ToString(categorydataTable.Rows[i]["Services"]);
                        // addCategoryRow.Year = Convert.ToString(categorydataTable.Rows[i]["Year"]);
                        // addCategoryRow.CommonTag = Convert.ToString(categorydataTable.Rows[i]["CommonTag"]);
                        //  addCategoryRow.PIRMatch = Convert.ToString(categorydataTable.Rows[i]["PIRMatchID"]);
                        // addCategoryRow.Status = Convert.ToBoolean(categorydataTable.Rows[i]["Status"]);
                        addCategoryRow.CreatedDate = Convert.ToDateTime(categorydataTable.Rows[i]["CreatedDate"]).ToString("MM/dd/yyyy");

                        _categorylist.Add(addCategoryRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _categorylist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _categorylist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                categorydataTable.Dispose();
            }
        }


        public Categoryinfo EditServices(string id)
        {
            Categoryinfo category = new Categoryinfo();
            try
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getservicesinfo";
                DataAdapter = new SqlDataAdapter(command);
                //  agencydataTable = new DataTable();
                //  DataAdapter.Fill(agencydataTable);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                      for (int i = 0; i < _dataset.Tables[0].Rows.Count; i++)
                    {
                    //if (_dataset.Tables[0].Rows.Count > 0)
                    //{
                        category.Description = Convert.ToString(_dataset.Tables[0].Rows[i]["Description"].ToString());
                        category.CategoryID = Convert.ToInt32(_dataset.Tables[0].Rows[i]["Category"].ToString());
                        category.mode ="1";
                         List<FingerprintsModel.Categoryinfo.ServiceInfo> listprog = new List<FingerprintsModel.Categoryinfo.ServiceInfo>();
                         FingerprintsModel.Categoryinfo.ServiceInfo obj = new FingerprintsModel.Categoryinfo.ServiceInfo();
                         obj.ServiceID = Convert.ToInt32(_dataset.Tables[0].Rows[i]["ServiceID"].ToString());
                         obj.Category = Convert.ToInt32(_dataset.Tables[0].Rows[i]["Category"].ToString());
                         obj.Description=Convert.ToString(_dataset.Tables[0].Rows[i]["Services"]);
                         obj.Status = Convert.ToBoolean(_dataset.Tables[0].Rows[i]["Status"]);
                         
                         if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[i]["Year"]).ToString()))
                        {
                              obj.Year=(_dataset.Tables[0].Rows[i]["Year"]).ToString();
                        }
                        else
                        {
                             obj.Year=string.Empty;
                        }
                          if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[i]["CommonTag"]).ToString()))
                        {
                              obj.CommonTag=(_dataset.Tables[0].Rows[i]["CommonTag"]).ToString();
                        }
                        else
                        {
                             obj.CommonTag=string.Empty;
                        }
                          if (!string.IsNullOrEmpty((_dataset.Tables[0].Rows[i]["PIRMatchID"]).ToString()))
                        {
                            obj.PIRMatch = (_dataset.Tables[0].Rows[i]["PIRMatchID"]).ToString();
                        }
                        else
                        {
                             obj.PIRMatch=string.Empty;
                        }
                      category.ServiceData.Add(obj);

                    }
                  
                }


                DataAdapter.Dispose();
                command.Dispose();
                //agencydataTable.Dispose();
                return category;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return category;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                // agencydataTable.Dispose();
            }
        }

        public string Deleteserviceinfo(string ServiceID)//, string userId
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deleteserviceinfo";
                command.Parameters.Add(new SqlParameter("@ServiceID", ServiceID));
                // command.Parameters.Add(new SqlParameter("@ModifiedBy", userId));
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

