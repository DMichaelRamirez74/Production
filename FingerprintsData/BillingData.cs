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
using System.Web;
using System.IO;

namespace FingerprintsData
{
    public class BillingData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;

        public void GetParentDetails(ref List<SelectListItem> lstItems, string AgencyId, string ProgramTypeId, string CenterId, string SearchText)
        {
            lstItems = new List<SelectListItem>();
            _dataset = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetParentDetails";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@ProgramType", ProgramTypeId);
                command.Parameters.AddWithValue("@CenterId", CenterId);
                command.Parameters.AddWithValue("@SearchText", SearchText);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            lstItems.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["ClientID"].ToString() });
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
                if (Connection != null)
                    Connection.Close();
            }
        }

        public void GetChildDetailsByProgramId(ref List<InvoiceDetails> listDetails, ref List<string> lstClientId, string ProgramTypeId,string AgencyId)
        {
            listDetails = new List<InvoiceDetails>();
            lstClientId = new List<string>();
            _dataset = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetChildDetailsByProgramId";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@ProgramType", ProgramTypeId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            lstClientId.Add(dr["ClientID"].ToString());
                        }
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            InvoiceDetails invoice = new InvoiceDetails();
                            invoice.ChildId = dr["ClientID"].ToString();
                            invoice.ChildName = dr["Name"].ToString();
                            invoice.Amount = "0";
                            listDetails.Add(invoice);
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
                if (Connection != null)
                    Connection.Close();
            }
        }

        public void GetOrganization(ref List<SelectListItem> lstItems,ref List<string> lstClientId, string AgencyId, string ProgramTypeId, string CenterId, string SearchText)
        {
            lstItems = new List<SelectListItem>();
            lstClientId = new List<string>();
            _dataset = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetOrganization";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@ProgramType", ProgramTypeId);
                command.Parameters.AddWithValue("@SearchText", SearchText);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            lstItems.Add(new SelectListItem { Text = dr["OrganizationName"].ToString(), Value = dr["Id"].ToString() });
                        }
                    }
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            lstClientId.Add(dr["ClientID"].ToString());
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
                if (Connection != null)
                    Connection.Close();
            }
        }

        public void GetAgencyDetails(ref DataTable dtDomainDetails)
        {
            dtDomainDetails = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetAgency";
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtDomainDetails);
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

        public void GetProgramTypeDetails(ref DataTable dtDomainDetails, string AgencyId)
        {
            dtDomainDetails = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetProgramTypeDetails";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtDomainDetails);
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

        public void GetCenterDetails(ref DataTable dtDomainDetails, string AgencyId)
        {
            dtDomainDetails = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetCenterDetails";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtDomainDetails);
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

        public void GetClassroomDetails(ref DataTable dtDomainDetails, string AgencyId)
        {
            dtDomainDetails = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetClassRoomDetails";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtDomainDetails);
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

        public void GetClientByFamilyId(ref List<SelectListItem> lstItems, string Clientid, string AgencyId, string ProgramTypeId, string CenterId)
        {
            lstItems = new List<SelectListItem>();
            _dataset = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetChildDetailsByFamilyId";
                command.Parameters.AddWithValue("@Client", Clientid);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@ProgramType", ProgramTypeId);
                command.Parameters.AddWithValue("@CenterId", CenterId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            lstItems.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["ClientID"].ToString() });
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
                if (Connection != null)
                    Connection.Close();
            }
        }

        public void GetFamilyOverrideByUserId(ref DataTable dtFamilyOverride, string UserId)
        {
            dtFamilyOverride = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetFamilyOverrideByUserId";
                command.Parameters.AddWithValue("@UserId", UserId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtFamilyOverride);
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

        public void GetInvoiceDetailsByUserId(ref DataTable dtInvoice, string UserId)
        {
            dtInvoice = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetInvoiceByUserId";
                command.Parameters.AddWithValue("@UserId", UserId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtInvoice);
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

        public void GetAccountReceivableListingsByUserId(ref DataTable dtInvoice, string UserId)
        {
            dtInvoice = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetAccountReceivableListing";
                command.Parameters.AddWithValue("@UserId", UserId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtInvoice);
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

        public void GetInvoiceDetailByUserId(ref DataTable dtInvoice, string UserId)
        {
            dtInvoice = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetInvoiceDetails";
                command.Parameters.AddWithValue("@UserId", UserId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtInvoice);
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

        public void GeOrganizationByProgramId(ref DataTable dtOrganization, string UserId, string AgencyId, string ProgramId)
        {
            dtOrganization = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetOrganizationByProgramId";
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@ProgramTypeId", ProgramId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtOrganization);
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
        public void GetRatesByProgramId(ref DataTable dtOrganization, string UserId, string AgencyId, string ProgramId)
        {
            dtOrganization = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetRatesByProgramId";
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@ProgramTypeId", ProgramId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtOrganization);
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
        public void GetBillingRatesByUserId(ref DataTable dtBilling, string UserId, string AgencyId)
        {
            dtBilling = new DataTable();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetBillingRatesByUserId";
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                DataAdapter = new SqlDataAdapter(command);
                DataAdapter.Fill(dtBilling);
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

    


        public bool DeleteFamilyOverride(string Id)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@Id", Id));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DeleteFamilyOverrideById";
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

        public bool DeleteInvoice(string ProgarmType,string Month)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@Programtypeid", ProgarmType));
                command.Parameters.Add(new SqlParameter("@Month", Month));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DeleteInvoiceById";
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

        public bool DeleteBillingRates(string Id)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@Id", Id));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_DeleteBillingRatesById";
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

      
        public List<Zipcodes> Checkaddress(int Zipcode)
        {
            List<Zipcodes> ZipcodesList = new List<Zipcodes>();

            try
            {

                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetAddressByZipcode";
                command.Parameters.Add(new SqlParameter("@Zipcode", Zipcode));
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            Zipcodes info = new Zipcodes();
                            info.Zipcode = dr["zipcode"].ToString();
                            info.City = dr["City"].ToString();
                            info.State = dr["state"].ToString();
                            info.County = dr["county"].ToString();
                            ZipcodesList.Add(info);
                        }
                    }
                }
                return ZipcodesList;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return ZipcodesList;
            }
            finally
            {
                command.Dispose();

            }
        }

        public bool SaveFamilyOverride(FamilyOverride family)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                if (!string.IsNullOrEmpty(family.Id))
                    command.Parameters.Add(new SqlParameter("@Id", family.Id));
                command.Parameters.Add(new SqlParameter("@ChildId", family.ChildId));
                command.Parameters.Add(new SqlParameter("@FixedAmount", family.FixedAmount));
                command.Parameters.Add(new SqlParameter("@NeverLessThan", family.NeverLessThan));
                command.Parameters.Add(new SqlParameter("@NeverMoreThan", family.NeverMoreThan));
                command.Parameters.Add(new SqlParameter("@OverrideEarlyRate", family.OverrideEarlyRate)); ;
                command.Parameters.Add(new SqlParameter("@OverrideNormalRate", family.OverrideNormalRate));
                command.Parameters.Add(new SqlParameter("@OverrideLateRate", family.OverrideLateRate));
                command.Parameters.Add(new SqlParameter("@Email", family.Email));
                command.Parameters.Add(new SqlParameter("@Print", family.Print));
                command.Parameters.Add(new SqlParameter("@ProgramTypeId", family.ProgramTypeId));
                command.Parameters.Add(new SqlParameter("@FamilyId", family.FamilyId));
                command.Parameters.Add(new SqlParameter("@UserId", family.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", family.AgencyId));
                command.Parameters.Add(new SqlParameter("@BilDirect", family.BillDirectToFamily));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SaveFamilyOverride";
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

        public bool SaveInvoiceDetails(InvoiceDetails invoice)
        {
            bool isInserted = false;
            try
            {
                DateTime dt = Convert.ToDateTime(invoice.Invoicedate);
                var firstDayOfMonth = new DateTime(dt.Year, dt.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                command = new SqlCommand();
                if (!string.IsNullOrEmpty(invoice.Id))
                    command.Parameters.Add(new SqlParameter("@Id", invoice.Id));
                command.Parameters.Add(new SqlParameter("@AgencyId", invoice.AgencyId));
                command.Parameters.Add(new SqlParameter("@ProgramTypeId", invoice.ProgramTypeId));
                command.Parameters.Add(new SqlParameter("@ChildId", invoice.ChildId));
                command.Parameters.Add(new SqlParameter("@InvoiceDate", dt));
                command.Parameters.Add(new SqlParameter("@Amount", invoice.Amount));
                command.Parameters.Add(new SqlParameter("@DueDate", lastDayOfMonth));
                command.Parameters.Add(new SqlParameter("@UserId", invoice.UserId));
                command.Parameters.Add(new SqlParameter("@CenterId", invoice.CenterId));
                command.Parameters.Add(new SqlParameter("@FamilyId", invoice.FamilyId));
                command.Parameters.Add(new SqlParameter("@Month", invoice.Month));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SaveInvoiceDetails";
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


        public bool SaveBillingRates(BillingRates billing)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                if (!string.IsNullOrEmpty(billing.Id))
                    command.Parameters.Add(new SqlParameter("@Id", billing.Id));
                command.Parameters.Add(new SqlParameter("@AgencyId", billing.AgencyId));
                command.Parameters.Add(new SqlParameter("@ProgramTypeId", billing.ProgramTypeId));
                command.Parameters.Add(new SqlParameter("@EarlyRate", billing.EarlyRate)); ;
                command.Parameters.Add(new SqlParameter("@NormalRate", billing.NormalRate));
                command.Parameters.Add(new SqlParameter("@LateRate", billing.LateRate));
                command.Parameters.Add(new SqlParameter("@BilDirect", billing.BillDirectToFamily));
                command.Parameters.Add(new SqlParameter("@AllowOverrideRate", billing.AllowOverrideRate));
                command.Parameters.Add(new SqlParameter("@UserId", billing.UserId));
                command.Parameters.Add(new SqlParameter("@FixedAmount", billing.FixedAmount));
                command.Parameters.Add(new SqlParameter("@EarlyorLateTimes", billing.EarlyorLateTimes));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SaveBillingRate";
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


     

        public bool SaveOrganization(OrganizatonDetail detail)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                if (!string.IsNullOrEmpty(detail.Id))
                    command.Parameters.Add(new SqlParameter("@Id", detail.Id));
                command.Parameters.Add(new SqlParameter("@OrganizationName", detail.OrganizationName)); ;
                command.Parameters.Add(new SqlParameter("@AddressLine1", detail.AddressLine1));
                command.Parameters.Add(new SqlParameter("@AddressLine2", detail.AddressLine2));
                command.Parameters.Add(new SqlParameter("@City", detail.City));
                command.Parameters.Add(new SqlParameter("@State", detail.State));
                command.Parameters.Add(new SqlParameter("@Zip", detail.Zip));
                command.Parameters.Add(new SqlParameter("@ContactName", detail.ContactName));
                command.Parameters.Add(new SqlParameter("@AccountNumber", detail.AccountNumber));
                command.Parameters.Add(new SqlParameter("@ReferenceNumber", detail.ReferenceNumber));
                command.Parameters.Add(new SqlParameter("@ContactNumber", detail.ContactNumber));
                command.Parameters.Add(new SqlParameter("@Extension", detail.Extension));
                command.Parameters.Add(new SqlParameter("@UserId", detail.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", detail.AgencyId));
                command.Parameters.Add(new SqlParameter("@ProgramTypeId", detail.ProgramTypeId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_SaveOrganizationBillingAddress";
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

        public void GetClientBillingReview(ref DataSet ds, string AgencyId, string UserId)
        {
            ds = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetClientBillingReview";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@UserId", UserId);
                DataAdapter = new SqlDataAdapter(command);
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

        public void GetClientBillingReviewByMonth(ref DataSet ds, string AgencyId, string UserId, string ClientId, string Month)
        {
            ds = new DataSet();
            try
            {
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_GetClientBillingReviewByMonth";
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@ClientId", ClientId);
                command.Parameters.AddWithValue("@Month", Month);
                DataAdapter = new SqlDataAdapter(command);
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

        public bool BillingAttachments(string AgencyId, string ChildId, string Path, string UserId)
        {
            bool isInserted = false;
            try
            {
                command = new SqlCommand();
                command.Parameters.Add(new SqlParameter("@AgencyId", AgencyId));
                command.Parameters.Add(new SqlParameter("@ChildId", ChildId));
                command.Parameters.Add(new SqlParameter("@Path", Path));
                command.Parameters.Add(new SqlParameter("@UserId", UserId));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_BillingAttachments";
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
