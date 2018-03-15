using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FingerprintsModel;

namespace FingerprintsData
{
    public class ExternalLeadsData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        //SqlDataReader dataReader = null;
        //SqlTransaction tranSaction = null;
        SqlDataAdapter DataAdapter = null;
        DataTable _dataTable = null;
        DataSet _dataset = null;

        public ExternalLeadsFamily GetExternalLeadsData(int parentId)
        {

            Family family = new Family();
            Child child = new Child();
            List<Child> childList = new List<Child>();
            ExternalLeadsFamily externalFamily = new ExternalLeadsFamily();


            try
            {

                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetExternalLeadsInfo";
                //command.Parameters.AddWithValue("@AgencyId", AgencyId);
                //command.Parameters.AddWithValue("@CenterId", CenterId);
                command.Parameters.AddWithValue("@ParentId", parentId);
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr  in _dataset.Tables[0].Rows)
                        {

                            family.FirstName = dr["ParentFirstName"].ToString();
                            family.LastName = dr["ParentLastName"].ToString();
                            family.Address = dr["Address"].ToString();
                            family.ParentId = Convert.ToInt32(dr["ParentId"]);
                            family.PhoneNumber = dr["PhoneNumber"].ToString();
                            family.EmailAddress = dr["Email"].ToString();
                            family.State = dr["State"].ToString();
                            family.ZipCode = dr["ZipCode"].ToString();
                            family.City = dr["City"].ToString();
                            family.Extension = dr["Extension"].ToString();
                            family.ChildTransport = dr["ChildTransport"] != null ? Convert.ToBoolean(dr["ChildTransport"]) : false;
                            family.IsHomeBased = dr["IsHomeBased"] != null ? Convert.ToBoolean(dr["IsHomeBased"]) : false;
                            family.DOB = Convert.ToDateTime(dr["ParentDOB"]).ToString("MM/dd/yyyy");
                            family.IsPartyDay = dr["IsPartyDay"] != null ? Convert.ToBoolean(dr["IsPartyDay"]) : false;
                            family.IsFullDay = dr["IsFullDay"] != null ? Convert.ToBoolean(dr["IsFullDay"]) : false;
                            family.IsSchoolDay = dr["IsSchoolDay"] != null ? Convert.ToBoolean(dr["IsSchoolDay"]) : false;
                            string Location = "";
                            if(family.IsPartyDay)
                                Location = Location+ "Part Day";
                            if (family.IsFullDay)
                                Location = Location+",Full Day";
                            if (family.IsSchoolDay)
                                Location = Location+",School Day";
                            if (family.IsHomeBased)
                                Location = Location + ",Home Based";
                            family.LocationRequest = Location.TrimStart(',').TrimEnd(',');
                        }
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in _dataset.Tables[1].Rows)
                        {
                            child = new Child();
                            child.FirstName = dr1["ChildFirstName"].ToString();
                            child.LastName = dr1["ChildLastName"].ToString();
                            child.DOB = Convert.ToDateTime(dr1["ChildDob"]).ToString("MM/dd/yyyy");
                            child.Gender = dr1["Gender"].ToString();
                            child.Disability = dr1["Disability"].ToString();
                            childList.Add(child);

                        }
                    }

                    externalFamily.child = childList;
                    externalFamily.family = family;



                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return externalFamily;
        }

        public bool UpdateYakkrRoutingLead(Int64 ParentId)
        {
            bool isInserted = false;
            try
            {
                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_UpdateYakkrRoutingLEAD";
                command.Parameters.AddWithValue("@ParentId", ParentId);
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return isInserted;
        }


        public bool UpdateRejection(long parentId, string Reason, string UserId)
        {
            bool isInserted = false;
            try
            {
                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_AddExteralApplicationReject";
                command.Parameters.AddWithValue("@ParentId", parentId);
                command.Parameters.AddWithValue("@Reason", Reason);
                command.Parameters.AddWithValue("@CreatedBy", UserId);
                int id = command.ExecuteNonQuery();
                if (id > 0)
                    isInserted = true;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return isInserted;
        }

        public bool SaveFamilyIntake(ExternalLeadsFamily objExternalLeads, string UserId, int ParentId)
        {
            bool isInserted = false;
            try
            {
                Int64 HouseHoldId = 0;
                command.Parameters.Clear();
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USp_InsertExternalFamilyIntakeHousehold";
                command.Parameters.AddWithValue("@Street", objExternalLeads.family.Address);
                command.Parameters.AddWithValue("@ZipCode", objExternalLeads.family.ZipCode);
                command.Parameters.AddWithValue("@City", objExternalLeads.family.City);
                command.Parameters.AddWithValue("@State", objExternalLeads.family.State);
                command.Parameters.AddWithValue("@CreatedBy", UserId);
                command.Parameters.AddWithValue("@IsHomeBased", objExternalLeads.family.IsHomeBased);
                Object id = command.ExecuteScalar();
                if (id != null)
                {
                    HouseHoldId = Convert.ToInt64(id);
                    SaveClientDetails(objExternalLeads, HouseHoldId, UserId);
                    UpdateYakkrRoutingLead(ParentId);
                    isInserted = true;
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
            return isInserted;
        }

        public void SaveClientDetails(ExternalLeadsFamily objNotes, Int64 HouseHoldId, string UserId)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(objNotes.family.DOB);
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                //Parent Insert
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USp_InsertExternalFamilyIntakeChild";
                command.Parameters.AddWithValue("@Householdidgenerated", HouseHoldId);
                command.Parameters.AddWithValue("@Pfirstname", objNotes.family.FirstName);
                command.Parameters.AddWithValue("@Plastname", objNotes.family.LastName);
                command.Parameters.AddWithValue("@PDOB", dt);
                command.Parameters.AddWithValue("@Pemailid", objNotes.family.EmailAddress);
                command.Parameters.AddWithValue("@CreatedBy", UserId);
                command.Parameters.AddWithValue("@Phoneno", objNotes.family.PhoneNumber);
                command.Parameters.AddWithValue("@IsParent", true);
                int RowsAffected = command.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    foreach (Child objChild in objNotes.child)
                    {
                        DateTime dt1 = Convert.ToDateTime(objChild.DOB);
                        command.Parameters.Clear();
                        command.Connection = Connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "USp_InsertExternalFamilyIntakeChild";
                        command.Parameters.AddWithValue("@IsParent", false);
                        command.Parameters.AddWithValue("@Householdidgenerated", HouseHoldId);
                        command.Parameters.AddWithValue("@Pfirstname", objChild.FirstName);
                        command.Parameters.AddWithValue("@Plastname", objChild.LastName);
                        command.Parameters.AddWithValue("@PDOB", dt1);
                        if (objChild.Gender == "Male")
                            command.Parameters.AddWithValue("@PGender", 1);
                        else
                            command.Parameters.AddWithValue("@PGender", 2);
                        command.Parameters.AddWithValue("@CreatedBy", UserId);
                        bool Trasnport = objNotes.family.ChildTransport != null ? Convert.ToBoolean(objNotes.family.ChildTransport) : false;
                        command.Parameters.AddWithValue("@CTransportNeeded", Trasnport);
                        if (objChild.Disability == "IEP")
                            command.Parameters.AddWithValue("@IsIEP", true);
                        if (objChild.Disability == "IFSP")
                            command.Parameters.AddWithValue("@IsIFSP", true);
                        if (objChild.Disability == "Suspected")
                            command.Parameters.AddWithValue("@Parentdisable", 1);
                        if (objChild.Disability == "None")
                            command.Parameters.AddWithValue("@Parentdisable", 2);
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }



    }
}
