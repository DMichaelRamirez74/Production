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
    public class EducationMaterialData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
       // DataSet _dataset = null;
        public bool SaveEducationMaterial(EductionMaterial objEdu)
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
                command.CommandText = "USP_InsertEducationMaterial";
                if (!string.IsNullOrEmpty(objEdu.Id))
                    command.Parameters.AddWithValue("@Id", objEdu.Id);
                command.Parameters.AddWithValue("@Group", objEdu.Group);
                command.Parameters.AddWithValue("@Title", objEdu.Title);
                command.Parameters.AddWithValue("@Description", objEdu.Description);
                command.Parameters.AddWithValue("@URL", objEdu.URL);
                command.Parameters.AddWithValue("@URLNote", objEdu.URLNote);
                command.Parameters.AddWithValue("@UserId", objEdu.UserId);
                command.Parameters.AddWithValue("@AgencyId", objEdu.AgencyId);
                Object Note = command.ExecuteScalar();
                if (Note != null && objEdu.AttachmentPath!=null  && objEdu.AttachmentPath.Length>0)
                {
                    Int64 NoteID = Convert.ToInt64(Note);
                    SaveMaterialAttachment(objEdu, NoteID);
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
        public bool SavePostedDocumnetMaterial(string AgencyId, string UserId, string[] ClientIds, string MaterialId)
        {
            bool isInserted = false;
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_InsertEducationMaterialPostedDocument";
                foreach (var id in ClientIds)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ClientId", id);
                    command.Parameters.AddWithValue("@MaterialId", MaterialId);
                    command.Parameters.AddWithValue("@UserId", UserId);
                    command.Parameters.AddWithValue("@AgencyId", AgencyId);
                    int RowsAffected = command.ExecuteNonQuery();
                    if (RowsAffected > 0)
                    {
                        isInserted = true;
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
            return isInserted;
        }

        public void GetParentEmailbyClientId(ref DataSet ds,string ClientId, string MaterialId)
        {
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetParentEmailByClientId";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@ClientId", ClientId);
                command.Parameters.AddWithValue("@MaterialId", MaterialId);
                DataAdapter = new SqlDataAdapter(command);
                ds = new DataSet();
                DataAdapter.Fill(ds);
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
        public void SaveMaterialAttachment(EductionMaterial objEdu, Int64 MaterialId)
        {
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                foreach (var path in objEdu.AttachmentPath)
                {
                    command.Parameters.Clear();
                    command.Connection = Connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_InsertEducationMaterialAttachment";
                    command.Parameters.AddWithValue("@MaterialId", MaterialId);
                    command.Parameters.AddWithValue("@UserId", objEdu.UserId);
                    command.Parameters.AddWithValue("@Path", path.ToString());
                    int RowsAffected = command.ExecuteNonQuery();
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

        public void GetMaterialDetails(ref DataTable dtElements)
        {
            dtElements = new DataTable();
            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetMaterial";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtElements);
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

        public void GetMaterialDetailsShare(ref DataTable dtElements)
        {
            dtElements = new DataTable();
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetMaterialShare";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtElements);
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

        public void GetMaterialDetailsBySearchText(ref DataTable dtElements,string SerachText)
        {
            dtElements = new DataTable();
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@RoleId", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@SearchText", SerachText));
                command.Parameters.Add(new SqlParameter("@UserId", staff.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetMaterialBySerachText";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtElements);
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

        public void GetPostedDocumentsDetails(ref DataTable dtElements)
        {
            dtElements = new DataTable();
            try
            {

                StaffDetails staff = StaffDetails.GetInstance();

                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@RoleId",staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetMaterialPostedHistoryByUserId";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtElements);
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

        public void GetPostedDocumentsDetailsForParent(ref DataSet dtDocument, string ClientId)
        {
            dtDocument = new DataSet();
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@ClientId", ClientId));
                command.Parameters.Add(new SqlParameter("@RoleId", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetMaterialDocumentForParent";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtDocument);
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

        public void GetAttachmentByMaterialId(ref DataTable dtElements, string MaterialId)
        {
            dtElements = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@MaterialId", MaterialId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetAttachment";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtElements);
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

        public bool DeleteMaterial(string Id)
        {
            bool isInserted = false;
            try
            {
                StaffDetails staff = StaffDetails.GetInstance();
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@Id", Id));
                command.Parameters.Add(new SqlParameter("@AgencyID", staff.AgencyId));
                command.Parameters.Add(new SqlParameter("@RoleID", staff.RoleId));
                command.Parameters.Add(new SqlParameter("@UserID", staff.UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DeleteMaterialById";
                if (Connection.State == ConnectionState.Open) Connection.Close();
                Connection.Open();
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
    }
}
