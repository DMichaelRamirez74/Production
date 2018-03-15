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
    public class CommunityResourceData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        //SqlDataReader dataReader = null;
        //SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable communitydataTable = null;
        DataSet _dataset = null;
        public string AddCommunity(CommunityResource info, int mode, Guid userId, string AgencyId)
        {
            try
            {
                command.Connection = Connection;
                command.CommandText = "SP_addcommunity";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@CommunityID", info.CommunityID);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Title", info.Title);
                if (info.Community == "1")
                {
                    command.Parameters.AddWithValue("@Companyname", info.Companyname);
                }
                else if (info.Community == "2")
                {
                    command.Parameters.AddWithValue("@Companyname", info.CompanynameD);
                }
                command.Parameters.AddWithValue("@Region", info.Region);
                command.Parameters.AddWithValue("@Fname", info.Fname);
                command.Parameters.AddWithValue("@Lname", info.Lname);
                command.Parameters.AddWithValue("@Community", info.Community);
                command.Parameters.AddWithValue("@Address", info.Address);
                command.Parameters.AddWithValue("@ZipCode", info.ZipCode);
                command.Parameters.AddWithValue("@City", info.City);
                command.Parameters.AddWithValue("@State", info.State);
                command.Parameters.AddWithValue("@County", info.County);
                command.Parameters.AddWithValue("@StartTime", info.StartTime);
                command.Parameters.AddWithValue("@StopTime", info.StopTime);
                command.Parameters.AddWithValue("@OpenonSat", info.OpenonSat);
                command.Parameters.AddWithValue("@Phoneno", info.Phoneno);
                //Added by Akansha on 14Dec2016
                command.Parameters.AddWithValue("@DentalCenter", info.DentalCenter);
                command.Parameters.AddWithValue("@DentalNotes", info.DentalNotes);
                command.Parameters.AddWithValue("@MedicalNotes", info.MedicalNotes);
                command.Parameters.AddWithValue("@MedicalCenter", info.MedicalCenter);

                //End



                command.Parameters.AddWithValue("@CreatedBy", userId);
                //Changes
                command.Parameters.AddWithValue("@Email", info.Email);
                command.Parameters.AddWithValue("@Comments", info.Comments);
                command.Parameters.AddWithValue("@gender", info.gender);
                command.Parameters.AddWithValue("@office24hours", info.office24hours);
                command.Parameters.AddWithValue("@Mon", info.Mon);
                command.Parameters.AddWithValue("@MonFrom", info.MonFrom);
                command.Parameters.AddWithValue("@MonTo", info.MonTo);
                command.Parameters.AddWithValue("@Tue", info.Tue);
                command.Parameters.AddWithValue("@TueFrom", info.TueFrom);
                command.Parameters.AddWithValue("@TueTo", info.TueTo);
                command.Parameters.AddWithValue("@Wed", info.Wed);
                command.Parameters.AddWithValue("@WedFrom", info.WedFrom);
                command.Parameters.AddWithValue("@WedTo", info.WedTo);
                command.Parameters.AddWithValue("@Thurs", info.Thurs);
                command.Parameters.AddWithValue("@ThursFrom", info.ThursFrom);
                command.Parameters.AddWithValue("@ThursTo", info.ThursTo);
                command.Parameters.AddWithValue("@Fri", info.Fri);
                command.Parameters.AddWithValue("@FriFrom", info.FriFrom);
                command.Parameters.AddWithValue("@FriTo", info.FriTo);
                command.Parameters.AddWithValue("@Sat", info.Sat);
                command.Parameters.AddWithValue("@SatFrom", info.SatFrom);
                command.Parameters.AddWithValue("@SatTo", info.SatTo);
                command.Parameters.AddWithValue("@SunFrom", info.SunFrom);
                command.Parameters.AddWithValue("@SunTo", info.SunTo);
                command.Parameters.AddWithValue("@Sun", info.Sun);
                command.Parameters.AddWithValue("@Services", info.Services);
                command.Parameters.AddWithValue("@checkedall", info.CheckedAll);
                command.Parameters.AddWithValue("@uncheckedall", info.UncheckedAll);
                command.Parameters.AddWithValue("@Centers", info.Centers);
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
        public List<CommunityResource> Communitydetails(out string totalrecord, string sortOrder, string sortDirection, string search, int skip, int pageSize, string userid)
        {
            List<CommunityResource> _communitylist = new List<CommunityResource>();
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
                //command.Parameters.Add(new SqlParameter("@CommunityID", CommunityID));
                command.Parameters.Add(new SqlParameter("@totalRecord", 0)).Direction = ParameterDirection.Output;
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Community_list";
                DataAdapter = new SqlDataAdapter(command);
                communitydataTable = new DataTable();
                DataAdapter.Fill(communitydataTable);
                if (communitydataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < communitydataTable.Rows.Count; i++)
                    {
                        CommunityResource addCommunityRow = new CommunityResource();
                        addCommunityRow.CommunityID = Convert.ToInt32(communitydataTable.Rows[i]["ID"]);
                        //addCommunityRow.Title = Convert.ToString(communitydataTable.Rows[i]["Title"]);
                        addCommunityRow.Companyname = Convert.ToString(communitydataTable.Rows[i]["CompanyName"]);
                        addCommunityRow.Fname = Convert.ToString(communitydataTable.Rows[i]["Fname"]);
                        addCommunityRow.Lname = Convert.ToString(communitydataTable.Rows[i]["Lname"]);
                        //addCommunityRow.Notes = Convert.ToString(communitydataTable.Rows[i]["Notes"]);

                        //    addCommunityRow.Community = Convert.ToString(communitydataTable.Rows[i]["Community"]);
                        addCommunityRow.CreatedDate = Convert.ToDateTime(communitydataTable.Rows[i]["DateEntered"]).ToString("MM/dd/yyyy");
                        //if ((Convert.ToString(communitydataTable.Rows[i]["Community"]) == "1"))
                        //{
                        //    addCommunityRow.Community = "Doctor";
                        //}
                        //else if ((Convert.ToString(communitydataTable.Rows[i]["Community"]) == "2"))
                        //{
                        //    addCommunityRow.Community = "Dentist";
                        //}
                        //else
                        //{
                        //    addCommunityRow.Community = "Other";
                        //}
                        _communitylist.Add(addCommunityRow);
                    }
                    totalrecord = command.Parameters["@totalRecord"].Value.ToString();
                }
                return _communitylist;
            }
            catch (Exception ex)
            {
                totalrecord = string.Empty;
                clsError.WriteException(ex);
                return _communitylist;
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
                communitydataTable.Dispose();
            }
        }
        public CommunityResource GetcommunityDetails(string CommunityId, string AgencyId)
        {
            CommunityResource obj = new CommunityResource();
            try
            {
                command.Parameters.Add(new SqlParameter("@CommunityId", CommunityId));
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_communityinfo";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    obj.CommunityID = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ID"]);
                    obj.Fname = _dataset.Tables[0].Rows[0]["Fname"].ToString();
                    obj.Lname = _dataset.Tables[0].Rows[0]["Lname"].ToString();
                    obj.CreatedDate = Convert.ToDateTime(_dataset.Tables[0].Rows[0]["DateEntered"]).ToString("MM/dd/yyyy");
                    obj.Companyname = _dataset.Tables[0].Rows[0]["CompanyName"].ToString();
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Community"].ToString()))
                    {
                        obj.Community = _dataset.Tables[0].Rows[0]["Community"].ToString();
                    }
                    else
                    {
                        obj.Community = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Region"].ToString()))
                    {
                        obj.Region = _dataset.Tables[0].Rows[0]["Region"].ToString();
                    }
                    else
                    {
                        obj.Region = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Title"].ToString()))
                    {
                        obj.Title = _dataset.Tables[0].Rows[0]["Title"].ToString();
                    }
                    else
                    {
                        obj.Title = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Address"].ToString()))
                    {
                        obj.Address = Convert.ToString(_dataset.Tables[0].Rows[0]["Address"]);
                    }
                    else
                    {
                        obj.Address = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["City"].ToString()))
                    {
                        obj.City = Convert.ToString(_dataset.Tables[0].Rows[0]["City"]);
                    }
                    else
                    {
                        obj.City = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["State"].ToString()))
                    {
                        obj.State = Convert.ToString(_dataset.Tables[0].Rows[0]["State"]);
                    }
                    else
                    {
                        obj.State = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["County"].ToString()))
                    {
                        obj.County = Convert.ToString(_dataset.Tables[0].Rows[0]["County"]);
                    }
                    else
                    {
                        obj.County = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Email"].ToString()))
                    {
                        obj.Email = Convert.ToString(_dataset.Tables[0].Rows[0]["Email"]);
                    }
                    else
                    {
                        obj.Email = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["Comments"].ToString()))
                    {
                        obj.Comments = Convert.ToString(_dataset.Tables[0].Rows[0]["Comments"]);
                    }
                    else
                    {
                        obj.Comments = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["PhoneNo"].ToString()))
                    {
                        obj.Phoneno = Convert.ToString(_dataset.Tables[0].Rows[0]["PhoneNo"]);
                    }
                    else
                    {
                        obj.Phoneno = string.Empty;
                    }

                    obj.ZipCode = Convert.ToInt32(_dataset.Tables[0].Rows[0]["ZipCode"]);
                    if (!DBNull.Value.Equals(_dataset.Tables[0].Rows[0]["Office24Hours"]))
                    {
                        obj.office24hours = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["Office24Hours"]);
                    }
                    else
                    {
                        obj.office24hours = false;
                    }
                    if (!DBNull.Value.Equals(_dataset.Tables[0].Rows[0]["MonOffice"]))
                    {
                        obj.Mon = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["MonOffice"]);
                    }
                    else
                    {
                        obj.Mon = false;
                    }
                    if (!DBNull.Value.Equals(_dataset.Tables[0].Rows[0]["TueOffice"]))
                    {
                        obj.Tue = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["TueOffice"]);
                    }
                    else
                    {
                        obj.Tue = false;
                    }
                    if (!DBNull.Value.Equals(_dataset.Tables[0].Rows[0]["WedOffice"]))
                    {
                        obj.Wed = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["WedOffice"]);
                    }
                    else
                    {
                        obj.Wed = false;
                    }
                    if (!DBNull.Value.Equals(_dataset.Tables[0].Rows[0]["ThursOffice"]))
                    {
                        obj.Thurs = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["ThursOffice"]);
                    }
                    else
                    {
                        obj.Thurs = false;
                    }
                    if (!DBNull.Value.Equals(_dataset.Tables[0].Rows[0]["FriOffice"]))
                    {
                        obj.Fri = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["FriOffice"]);
                    }
                    else
                    {
                        obj.Fri = false;
                    }
                    if (!DBNull.Value.Equals(_dataset.Tables[0].Rows[0]["SatOffice"]))
                    {
                        obj.Sat = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["SatOffice"]);
                    }
                    else
                    {
                        obj.Sat = false;
                    }
                    if (!DBNull.Value.Equals(_dataset.Tables[0].Rows[0]["SunOffice"]))
                    {
                        obj.Sun = Convert.ToBoolean(_dataset.Tables[0].Rows[0]["SunOffice"]);
                    }
                    else
                    {
                        obj.Sun = false;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["MonTo"])))
                    {
                        obj.MonTo = _dataset.Tables[0].Rows[0]["MonTo"].ToString();
                    }
                    else
                    {
                        obj.MonTo = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["MonFrom"])))
                    {
                        obj.MonFrom = _dataset.Tables[0].Rows[0]["MonFrom"].ToString();
                    }
                    else
                    {
                        obj.MonFrom = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["TueTo"])))
                    {
                        obj.TueTo = _dataset.Tables[0].Rows[0]["TueTo"].ToString();
                    }
                    else
                    {
                        obj.TueTo = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["TueFrom"])))
                    {
                        obj.TueFrom = _dataset.Tables[0].Rows[0]["TueFrom"].ToString();
                    }
                    else
                    {
                        obj.TueFrom = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["WedTo"])))
                    {
                        obj.WedTo = _dataset.Tables[0].Rows[0]["WedTo"].ToString();
                    }
                    else
                    {
                        obj.WedTo = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["WedFrom"])))
                    {
                        obj.WedFrom = _dataset.Tables[0].Rows[0]["WedFrom"].ToString();
                    }
                    else
                    {
                        obj.WedFrom = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["ThursTo"])))
                    {
                        obj.ThursTo = _dataset.Tables[0].Rows[0]["ThursTo"].ToString();
                    }
                    else
                    {
                        obj.ThursTo = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["ThursFrom"])))
                    {
                        obj.ThursFrom = _dataset.Tables[0].Rows[0]["ThursFrom"].ToString();
                    }
                    else
                    {
                        obj.ThursFrom = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["FriTo"])))
                    {
                        obj.FriTo = _dataset.Tables[0].Rows[0]["FriTo"].ToString();
                    }
                    else
                    {
                        obj.FriTo = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["FriFrom"])))
                    {
                        obj.FriFrom = _dataset.Tables[0].Rows[0]["FriFrom"].ToString();
                    }
                    else
                    {
                        obj.FriFrom = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["SatTo"])))
                    {
                        obj.SatTo = _dataset.Tables[0].Rows[0]["SatTo"].ToString();
                    }
                    else
                    {
                        obj.SatTo = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["SatFrom"])))
                    {
                        obj.SatFrom = _dataset.Tables[0].Rows[0]["SatFrom"].ToString();
                    }
                    else
                    {
                        obj.SatFrom = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["SunTo"])))
                    {
                        obj.SunTo = _dataset.Tables[0].Rows[0]["SunTo"].ToString();
                    }
                    else
                    {
                        obj.SunTo = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["SunFrom"])))
                    {
                        obj.SunFrom = _dataset.Tables[0].Rows[0]["SunFrom"].ToString();
                    }
                    else
                    {
                        obj.SunFrom = string.Empty;
                    }
                    if (!DBNull.Value.Equals((_dataset.Tables[0].Rows[0]["Gender"])))
                    {
                        obj.gender = Convert.ToInt32(_dataset.Tables[0].Rows[0]["Gender"].ToString());
                    }
                    else
                    {
                        obj.gender = 0;
                    }
                    //Added by Akansha on 14Dec2016
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["MedicalCenter"].ToString()))
                    {
                        obj.MedicalCenter = _dataset.Tables[0].Rows[0]["MedicalCenter"].ToString();
                    }
                    else
                    {
                        obj.MedicalCenter = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["DentalCenter"].ToString()))
                    {
                        obj.DentalCenter = _dataset.Tables[0].Rows[0]["DentalCenter"].ToString();
                    }
                    else
                    {
                        obj.DentalCenter = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["MedicalNotes"].ToString()))
                    {
                        obj.MedicalNotes = _dataset.Tables[0].Rows[0]["MedicalNotes"].ToString();
                    }
                    else
                    {
                        obj.MedicalNotes = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_dataset.Tables[0].Rows[0]["DentalNotes"].ToString()))
                    {
                        obj.DentalNotes = _dataset.Tables[0].Rows[0]["DentalNotes"].ToString();
                    }
                    else
                    {
                        obj.DentalNotes = string.Empty;
                    }
                    //End


                    if (_dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {
                        List<CommunityResource.CategoryServiceInfo> Services = new List<CommunityResource.CategoryServiceInfo>();
                        List<CommunityResource.ServiceInfo> ServicedetailRecords = new List<CommunityResource.ServiceInfo>();
                        CommunityResource.CategoryServiceInfo objService = new CommunityResource.CategoryServiceInfo();
                        CommunityResource.ServiceInfo obj1;
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            obj1 = new CommunityResource.ServiceInfo();
                            obj1.Id = Convert.ToInt32(dr["Services"]);
                            ServicedetailRecords.Add(obj1);
                        }
                        objService.ServiceInfoDetail = ServicedetailRecords;
                        Services.Add(objService);
                        obj.AvailableService = Services;
                    }
                    if (_dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
                    {

                        List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                        foreach (DataRow dr in _dataset.Tables[2].Rows)
                        {
                            HrCenterInfo center = new HrCenterInfo();
                            center.CenterId = dr["CenterId"].ToString();
                            centerList.Add(center);
                        }
                        obj.HrcenterList = centerList;
                    }
                }


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
                _dataset.Dispose();
            }
        }
        public string Deletecommunity(string CommunityId, string AgencyId)
        {

            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_deletecommunityinfo";
                command.Parameters.Add(new SqlParameter("@CommunityId", CommunityId));
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

        public CommunityResource GetData_AllDropdown(string agenycyid, int i = 0, CommunityResource familyInfo = null)
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            CommunityResource Info = new CommunityResource();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_DoctorInfo_Dropdowndata";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@agenycyid", agenycyid));
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                    try
                    {
                        List<FingerprintsModel.CommunityResource.DoctorInfo> _doctorlist = new List<FingerprintsModel.CommunityResource.DoctorInfo>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FingerprintsModel.CommunityResource.DoctorInfo obj = new FingerprintsModel.CommunityResource.DoctorInfo();
                            obj.Id = dr["Id"].ToString();
                            obj.Name = dr["DoctorName"].ToString();
                            _doctorlist.Add(obj);
                        }
                        //  _rolelist.Insert(0, new Role() { RoleId = "0", RoleName = "Select" });
                        Info.doctorList = _doctorlist;
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
        public CommunityResource GetServiceData(string Agencyid, CommunityResource communityInfo = null)
        {
            CommunityResource Info = new CommunityResource();
            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_Service_Dropdowndata";
                        command.Parameters.AddWithValue("@AgencyId", Agencyid);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                    try
                    {
                        List<CommunityResource.CategoryServiceInfo> Services = new List<CommunityResource.CategoryServiceInfo>();
                        List<FingerprintsModel.CommunityResource.ServiceInfo> listservice = null;
                        DataTable dv = ds.Tables[0].DefaultView.ToTable(true, "Category");
                        for (int i = 0; i < dv.Rows.Count; i++)
                        {
                            DataRow[] drs = ds.Tables[0].Select("Category=" + dv.Rows[i]["Category"].ToString());
                            CommunityResource.CategoryServiceInfo obj = new CommunityResource.CategoryServiceInfo();
                            FingerprintsModel.CommunityResource.ServiceInfo objservice;
                            obj.CategoryName = drs[0]["Description"].ToString();
                            obj.CategoryId = drs[0]["Category"].ToString();
                            listservice = new List<FingerprintsModel.CommunityResource.ServiceInfo>();
                            foreach (DataRow dr in drs)
                            {
                                objservice = new FingerprintsModel.CommunityResource.ServiceInfo();
                                objservice.Id = Convert.ToInt32(dr["ServiceID"]);
                                objservice.Name = dr["Services"].ToString();
                                listservice.Add(objservice);
                            }
                            obj.ServiceInfoDetail = listservice;
                            Services.Add(obj);
                        }
                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {

                            List<HrCenterInfo> centerList = new List<HrCenterInfo>();
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                HrCenterInfo obj = new HrCenterInfo();
                                obj.CenterId = dr["CenterId"].ToString();
                                obj.Name = dr["CenterName"].ToString();
                                obj.Homebased = Convert.ToBoolean(dr["HomeBased"].ToString());

                                centerList.Add(obj);
                            }
                            Info.HrcenterList = centerList;
                        }


                        Info.AvailableService = Services;
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
        //Changes on 1Feb2017
        public string AddCommunityAjax(CommunityResource Info, string Fname, string Lname, string CompanyName, string Address, string Zip, string Phoneno,
               string City, string State, string County, string userId, string AgencyId, string mode, string Community, string DentalCenter, string DentalNotes, string MedicalNotes, string MedicalCenter, string CompanyNameden, string DocCheck, string DenCheck)
        {
            try
            {

                command.Connection = Connection;
                command.CommandText = "SP_addcommunity";
                command.Parameters.AddWithValue("@mode", mode);
                command.Parameters.AddWithValue("@CommunityID", Info.CommunityID);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@Community", Community);
                //command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Fname", Fname);
                command.Parameters.AddWithValue("@Lname", Lname);
                if (Community == "1")
                {
                    command.Parameters.AddWithValue("@CompanyName", CompanyName);
                }
                else if (Community == "2")
                {
                    command.Parameters.AddWithValue("@CompanyName", CompanyNameden);
                }
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@ZipCode", Zip);
                command.Parameters.AddWithValue("@Phoneno", Phoneno);
                command.Parameters.AddWithValue("@City", City);
                command.Parameters.AddWithValue("@State", State);
                command.Parameters.AddWithValue("@County", County);
                //command.Parameters.AddWithValue("@Notes", Notes);
                command.Parameters.AddWithValue("@CreatedBy", userId);
                //Added by Akansha on 14Dec2016
                command.Parameters.AddWithValue("@DentalCenter", DentalCenter);
                command.Parameters.AddWithValue("@DentalNotes", DentalNotes);
                command.Parameters.AddWithValue("@MedicalNotes", MedicalNotes);
                command.Parameters.AddWithValue("@MedicalCenter", MedicalCenter);
                command.Parameters.AddWithValue("@DocCheck", DocCheck);
                command.Parameters.AddWithValue("@DenCheck", DenCheck);

                //End
                command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                command.CommandType = CommandType.StoredProcedure;
                _dataset = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(_dataset);
                try
                {
                    List<FingerprintsModel.CommunityResource.DoctorInfo> _doctorlist = new List<FingerprintsModel.CommunityResource.DoctorInfo>();
                    //if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                    //    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    //    {
                    //        FingerprintsModel.CommunityResource.DoctorInfo obj = new FingerprintsModel.CommunityResource.DoctorInfo();
                    //        obj.Id = dr["Id"].ToString();
                    //        //obj.Name = dr["DoctorName"].ToString();
                    //        //  if (obj.Community == "1")
                    //        obj.Community = dr["Community"].ToString();
                    //        if (obj.Community == "1")
                    //        {
                    //            obj.Name = dr["MedicalCenter"].ToString();
                    //        }
                    //        if (obj.Community == "2")
                    //        {
                    //            obj.Name = dr["DentalCenter"].ToString();
                    //        }
                    //        _doctorlist.Add(obj);
                    //    }
                    if (string.IsNullOrEmpty(Community))
                    {
                        if (_dataset.Tables[0] != null)
                        {
                            if (_dataset.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in _dataset.Tables[0].Rows)
                                {
                                    FingerprintsModel.CommunityResource.DoctorInfo obj = new FingerprintsModel.CommunityResource.DoctorInfo();
                                    obj.Id = dr["Id"].ToString();
                                    //obj.Name = dr["DoctorName"].ToString();
                                    //  if (obj.Community == "1")
                                    obj.Community = dr["Community"].ToString();
                                    if (obj.Community == "1")
                                    {
                                        obj.Name = dr["DoctorName"].ToString();
                                    }

                                    _doctorlist.Add(obj);
                                }
                            }
                        }
                    }

                    else if (_dataset.Tables[1] != null)
                    {
                        if (_dataset.Tables[1].Rows.Count > 0)
                        {
                            if (Community == "1")
                            {
                                foreach (DataRow dr in _dataset.Tables[1].Rows)
                                {
                                    FingerprintsModel.CommunityResource.DoctorInfo obj = new FingerprintsModel.CommunityResource.DoctorInfo();
                                    obj.Id = dr["Id"].ToString();
                                    //obj.Name = dr["DoctorName"].ToString();
                                    //  if (obj.Community == "1")
                                    obj.Community = dr["Community"].ToString();
                                    if (obj.Community == "1")
                                    {
                                        obj.Name = dr["DoctorName"].ToString();
                                    }

                                    _doctorlist.Add(obj);
                                }
                            }
                            if (Community == "2")
                            {
                                foreach (DataRow dr in _dataset.Tables[1].Rows)
                                {
                                    FingerprintsModel.CommunityResource.DoctorInfo obj = new FingerprintsModel.CommunityResource.DoctorInfo();
                                    obj.Id = dr["Id"].ToString();
                                    //obj.Name = dr["DoctorName"].ToString();
                                    //  if (obj.Community == "1")
                                    obj.Community = dr["Community"].ToString();

                                    if (obj.Community == "2")
                                    {
                                        obj.Name = dr["DoctorName"].ToString();
                                    }
                                    _doctorlist.Add(obj);
                                }
                            }

                        }

                    }
                    //if (_dataset.Tables[2] != null)
                    //{
                    //    if (_dataset.Tables[2].Rows.Count > 0)
                    //    {
                    //        if (Community == "2")
                    //        {
                    //            foreach (DataRow dr in _dataset.Tables[2].Rows)
                    //            {
                    //                FingerprintsModel.CommunityResource.DoctorInfo obj = new FingerprintsModel.CommunityResource.DoctorInfo();
                    //                obj.Id = dr["Id"].ToString();
                    //                //obj.Name = dr["DoctorName"].ToString();
                    //                //  if (obj.Community == "1")
                    //                obj.Community = dr["Community"].ToString();

                    //                if (obj.Community == "2")
                    //                {
                    //                    obj.Name = dr["DoctorName"].ToString();
                    //                }
                    //                _doctorlist.Add(obj);
                    //            }
                    //        }
                    //    }
                    //}
                    Info.doctorList = _doctorlist;
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




        //added by Akansha on 15Dec2016
        public CommunityResource GetCommunityInfo(string community, string agenycyid)
        {
            //  List<AgencyStaff> _agencyStafflist = new List<AgencyStaff>();
            CommunityResource Info = new CommunityResource();

            try
            {
                DataSet ds = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        ds = new DataSet();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Get_CommunityInfo_Dropdowndata";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@agenycyid", agenycyid));
                        command.Parameters.Add(new SqlParameter("@community", community));//Added by Akansha
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(ds);
                    }
                    try
                    {
                        List<FingerprintsModel.CommunityResource.DoctorInfo> _doctorlist = new List<FingerprintsModel.CommunityResource.DoctorInfo>();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FingerprintsModel.CommunityResource.DoctorInfo obj = new FingerprintsModel.CommunityResource.DoctorInfo();
                            obj.Id = dr["Id"].ToString();
                            obj.Name = dr["Center"].ToString();
                            _doctorlist.Add(obj);
                        }
                        //  _rolelist.Insert(0, new Role() { RoleId = "0", RoleName = "Select" });
                        Info.doctorList = _doctorlist;
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

        //End

    }
}
