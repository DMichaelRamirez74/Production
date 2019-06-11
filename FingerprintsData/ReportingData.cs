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
using System.Drawing;
using FingerprintsData;
using ClosedXML;
using ClosedXML.Excel;
using FingerprintsDataAccessHandler;
using System.Collections;
using Fingerprints.Common;
using System.Globalization;
using System.Dynamic;

namespace FingerprintsData
{
    public class Reporting
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;
        public ReportingModel ReturnChildList(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", 1));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];

            List<ReportingModel> chList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new ReportingModel 
                {
                    Firstname = Convert.ToString(dr["Firstname"]),
                    Lastname = Convert.ToString(dr["Lastname"]),
                    DOB = Convert.ToString(dr["DOB"]),
                    Status = Convert.ToString(dr["Status"]),
                    ProgramType = Convert.ToString(dr["programType"]),
                    ReasonForAcceptance = Convert.ToString(dr["ReasonForAcceptance"])
                });
            }
            _ReportingM.reporttype = 1;
            _ReportingM.Reportlst = chList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;
        }
        public ReportingModel ReturnChildStatus(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", 1));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport1]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];

            List<ReportingModel> chList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new ReportingModel
                {
                    Firstname = Convert.ToString(dr["Firstname"]),
                    Lastname = Convert.ToString(dr["Lastname"]),
                    DOB = Convert.ToString(dr["DOB"]),
                    Status = Convert.ToString(dr["Status"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    ClassroomName = Convert.ToString(dr["ClassroomName"]),
                    ReasonForAcceptance = Convert.ToString(dr["ReasonForAcceptance"]),
                    Address = Convert.ToString(dr["Address"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Email = Convert.ToString(dr["EMailID"]),
                    Guardian = Convert.ToString(dr["Guardian"]),
                    ProgramType = Convert.ToString(dr["programType"]),
                    DaysEnrolled = Convert.ToString(dr["DaysEnrolled"]),
                });
            }
            _ReportingM.reporttype = 1;
            _ReportingM.ColumnName = "Enrollment Status";
            _ReportingM.Reportlst = chList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;
        }
        public ReportingModel ReturnChildInsurance(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID",AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", 2));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];

            List<ReportingModel> chList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new ReportingModel
                {
                    Firstname = Convert.ToString(dr["Firstname"]),
                    Lastname = Convert.ToString(dr["Lastname"]),
                    DOB = Convert.ToString(dr["DOB"]),
                    Status = Convert.ToString(dr["Insurance"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    ClassroomName = Convert.ToString(dr["ClassroomName"]),
                    ReasonForAcceptance = Convert.ToString(dr["ReasonForAcceptance"]),
                    Address = Convert.ToString(dr["Address"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Email = Convert.ToString(dr["EMailID"]),
                    Guardian = Convert.ToString(dr["Guardian"]),
                    ProgramType = Convert.ToString(dr["programType"]),
                    DaysEnrolled = Convert.ToString(dr["DaysEnrolled"]),
                });
            }
            _ReportingM.reporttype = 2;
            _ReportingM.ColumnName = "Primary Insurance";
            _ReportingM.Reportlst = chList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;
        }
        public ReportingModel MonthlyMealReport(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
           // command.Parameters.Add(new SqlParameter("@ReportType", 3));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[GetMonthlyMealCount]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];
          
            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "CenterID", "AttendanceMonth","AttendanceDate","CenterName","BreakfastTotal","LunchTotal","DinnerTotal");
            DataView viewMonth = new DataView(dt);
            DataTable distinctValuesMonth = viewMonth.ToTable(true, "AttendanceDate", "BreakfastTotalMonth", "LunchTotalMonth", "DinnerTotalMonth");
            List<ReportingModel> mealList = new List<ReportingModel>();
            List<ReportingModel> centerList = new List<ReportingModel>();
            List<ReportingModel> monthList = new List<ReportingModel>();
            foreach (DataRow row in distinctValuesMonth.Rows)
            {
                monthList.Add(new ReportingModel
                {

                    AttendanceDateMonth = row["AttendanceDate"].ToString(),
                    BreakfastTotalMonth = row["BreakfastTotalMonth"].ToString(),
                    LunchTotalMonth = row["LunchTotalMonth"].ToString(),
                    SnackTotalMonth = row["DinnerTotalMonth"].ToString()

                });


            }
            foreach (DataRow row in distinctValues.Rows)
            {
                centerList.Add(new ReportingModel
                {
                    CenterIDCenter = row["CenterID"].ToString(),
                    AttendanceMonthCenter = row["AttendanceMonth"].ToString(),
                    MonthNameCenter = row["AttendanceDate"].ToString(),
                    CenterNameCenter = row["CenterName"].ToString(),
                    BreakfastTotal = row["BreakfastTotal"].ToString(),
                    LunchTotal = row["LunchTotal"].ToString(),
                    SnackTotal = row["DinnerTotal"].ToString()
                });
               

            }
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                mealList.Add(new ReportingModel
                {
                    MonthName = Convert.ToString(dr["AttendanceDate"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    ClassroomName = Convert.ToString(dr["ClassroomName"]),
                    Breakfast = Convert.ToString(dr["Breakfast"]),
                    Lunch = Convert.ToString(dr["Lunch"]),
                    Snack = Convert.ToString(dr["Dinner"]),
                    CenterID = Convert.ToString(dr["CenterID"]),
                    AttendanceMonth = Convert.ToString(dr["AttendanceMonth"]),
                    ABreakfast = Convert.ToString(dr["ABreakfast"]),
                    ALunch = Convert.ToString(dr["ALunch"]),
                    ASnack = Convert.ToString(dr["ADinner"]),
                    BreakfastTotal = Convert.ToString(dr["BreakfastTotal"]),
                    LunchTotal = Convert.ToString(dr["LunchTotal"]),
                    SnackTotal = Convert.ToString(dr["DinnerTotal"])
                });
            }
           // _ReportingM.reporttype = 3;
            //_ReportingM.ColumnName = "Race";
            _ReportingM.Reportlst = centerList;
            _ReportingM.Meallst = mealList;
            _ReportingM.Monthlst = monthList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;

    }
        public ReportingModel ReturnChildRace(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", 3));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];

            List<ReportingModel> chList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new ReportingModel
                {
                    Firstname = Convert.ToString(dr["Firstname"]),
                    Lastname = Convert.ToString(dr["Lastname"]),
                    DOB = Convert.ToString(dr["DOB"]),
                    Status = Convert.ToString(dr["Race"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    ClassroomName = Convert.ToString(dr["ClassroomName"]),
                    ReasonForAcceptance = Convert.ToString(dr["ReasonForAcceptance"]),
                    Address = Convert.ToString(dr["Address"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Email = Convert.ToString(dr["EMailID"]),
                    Guardian = Convert.ToString(dr["Guardian"]),
                    ProgramType = Convert.ToString(dr["programType"]),
                    DaysEnrolled = Convert.ToString(dr["DaysEnrolled"]),
                });
            }
            _ReportingM.reporttype = 3;
            _ReportingM.ColumnName = "Race";
            _ReportingM.Reportlst = chList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;
        }
        public ReportingModel ReturnChildEthnicity(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", 4));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];

            List<ReportingModel> chList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new ReportingModel
                {
                    Firstname = Convert.ToString(dr["Firstname"]),
                    Lastname = Convert.ToString(dr["Lastname"]),
                    DOB = Convert.ToString(dr["DOB"]),
                    Status = Convert.ToString(dr["Ethnicity"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    ClassroomName = Convert.ToString(dr["ClassroomName"]),
                    ReasonForAcceptance = Convert.ToString(dr["ReasonForAcceptance"]),
                    Address = Convert.ToString(dr["Address"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Email = Convert.ToString(dr["EMailID"]),
                    Guardian = Convert.ToString(dr["Guardian"]),
                    ProgramType = Convert.ToString(dr["programType"]),
                    DaysEnrolled = Convert.ToString(dr["DaysEnrolled"]),
                });
            }
            _ReportingM.reporttype = 4;
            _ReportingM.ColumnName = "Ethnicity";
            _ReportingM.Reportlst = chList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;
        }
        public ReportingModel ReturnChildLanguage(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", 7));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];

            List<ReportingModel> chList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new ReportingModel
                {
                    Firstname = Convert.ToString(dr["Firstname"]),
                    Lastname = Convert.ToString(dr["Lastname"]),
                    DOB = Convert.ToString(dr["DOB"]),
                    Status = Convert.ToString(dr["PLanguage"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    ClassroomName = Convert.ToString(dr["ClassroomName"]),
                    ReasonForAcceptance = Convert.ToString(dr["ReasonForAcceptance"]),
                    Address = Convert.ToString(dr["Address"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Email = Convert.ToString(dr["EMailID"]),
                    Guardian = Convert.ToString(dr["Guardian"]),
                    ProgramType = Convert.ToString(dr["programType"]),
                    DaysEnrolled = Convert.ToString(dr["DaysEnrolled"]),
                });
            }
            _ReportingM.reporttype = 7;
            _ReportingM.ColumnName = "Primary Language";
            _ReportingM.Reportlst = chList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;
        }
        public ReportingModel ReturnChildAge(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", 6));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];

            List<ReportingModel> chList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new ReportingModel
                {
                    Firstname = Convert.ToString(dr["Firstname"]),
                    Lastname = Convert.ToString(dr["Lastname"]),
                    DOB = Convert.ToString(dr["DOB"]),
                    Status = Convert.ToString(dr["Age"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    ClassroomName = Convert.ToString(dr["ClassroomName"]),
                    ReasonForAcceptance = Convert.ToString(dr["ReasonForAcceptance"]),
                    Address = Convert.ToString(dr["Address"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Email = Convert.ToString(dr["EMailID"]),
                    Guardian = Convert.ToString(dr["Guardian"]),
                    ProgramType = Convert.ToString(dr["programType"]),
                    DaysEnrolled = Convert.ToString(dr["DaysEnrolled"]),
                });
            }
            _ReportingM.reporttype = 6;
            _ReportingM.ColumnName = "Age by Date";
            _ReportingM.Reportlst = chList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;
        }
        public ReportingModel ReturnChildGender(string AgencyID)
        {
            ReportingModel _ReportingM = new ReportingModel();
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", 5));

            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport]";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];

            List<ReportingModel> chList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                chList.Add(new ReportingModel
                {
                    Firstname = Convert.ToString(dr["Firstname"]),
                    Lastname = Convert.ToString(dr["Lastname"]),
                    DOB = Convert.ToString(dr["DOB"]),
                    Status = Convert.ToString(dr["Gender"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    ClassroomName = Convert.ToString(dr["ClassroomName"]),
                    ReasonForAcceptance = Convert.ToString(dr["ReasonForAcceptance"]),
                    Address = Convert.ToString(dr["Address"]),
                    Phone = Convert.ToString(dr["Phone"]),
                    Email = Convert.ToString(dr["EMailID"]),
                    Guardian = Convert.ToString(dr["Guardian"]),
                    ProgramType = Convert.ToString(dr["programType"]),
                    DaysEnrolled = Convert.ToString(dr["DaysEnrolled"]),
                });
            }
            _ReportingM.reporttype = 5;
            _ReportingM.ColumnName = "Gender";
            _ReportingM.Reportlst = chList;
            Connection.Close();
            command.Dispose();

            return _ReportingM;
        }
        public ReportingModel ExportData(int exporttype, string AgencyID)
        {
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@AgencyID", AgencyID));
            command.Parameters.Add(new SqlParameter("@ReportType", exporttype));
            command.Connection = Connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[ChildDataReport]";
            SqlDataAdapter DataAdapter1 = null;
            DataAdapter1 = new SqlDataAdapter(command);
            DataSet _dataset1 = null;
            _dataset1 = new DataSet();
            DataAdapter1.Fill(_dataset1);
            string FileName = "attachment; filename = EnrollmentStatusReport.xlsx";
            if (exporttype == 2) { FileName = "attachment; filename = EnrollmentInsuranceReport.xlsx"; }
            if (exporttype == 3) { FileName = "attachment; filename = EnrollmentRaceReport.xlsx"; }
            if (exporttype == 4) { FileName = "attachment; filename = EnrollmentEthnicityReport.xlsx"; }
            if (exporttype == 5) { FileName = "attachment; filename = EnrollmentGenderReport.xlsx"; }
            if (exporttype == 6) { FileName = "attachment; filename = EnrollmentAgeReport.xlsx"; }
            if (exporttype == 7) { FileName = "attachment; filename = EnrollmentLanguageReport.xlsx"; }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(_dataset1);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
               
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Buffer = true;
               // System.Web.HttpContext.Current.Response.Charset = "";
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", FileName);

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(System.Web.HttpContext.Current.Response.OutputStream);
                    System.Web.HttpContext.Current.Response.Flush();
                    System.Web.HttpContext.Current.Response.End();
                }
            }
            ReportingModel _ReportingM = new ReportingModel();
            return _ReportingM;

        }
        
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }


        #region CLASReview

        public bool AddCLASReview(CLASReview data, int mode)
        {

            var success = false;
            try
            {
                var stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_UpdateCLASReview";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@Mode", mode));

                command.Parameters.Add(new SqlParameter("@Center", data.Center));
                command.Parameters.Add(new SqlParameter("@ClassRoom", data.ClassRoom));
                command.Parameters.Add(new SqlParameter("@CommentNote", data.CommentNote));
                command.Parameters.Add(new SqlParameter("@DateofReview", data.DateofReview));
                command.Parameters.Add(new SqlParameter("@Score", data.Score));
                command.Parameters.Add(new SqlParameter("@TimeofReview", data.TimeofReview));


                DataTable attachmentdt = new DataTable();

                attachmentdt.Columns.AddRange(new DataColumn[5] {

new DataColumn("IndexID",typeof(long)),
new DataColumn("Attachment",typeof(byte[])),
new DataColumn("AttachmentName",typeof(string)),
new DataColumn("AttachmentExtension",typeof(string)),
new DataColumn("Status",typeof(bool))

                });


                if (data.CLASReviewAttachment != null && data.CLASReviewAttachment.Count > 0)
                {
                    int i = 0;
                    foreach (var item in data.CLASReviewAttachment)
                    {
                        attachmentdt.Rows.Add(0,
                                                  item.AttachmentFileByte,
                                                  item.AttachmentFileName,
                                                  item.AttachmentFileExtension,
                                                 item.AttachmentStatus
                                                  );
                        i++;
                    }

                }

                command.Parameters.Add(new SqlParameter("@Attachments", attachmentdt));

                var re = command.ExecuteNonQuery();

                if (re > 0)
                {
                    success = true;
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {

            }


            return success;
        }


        public List<CLASReview> getCLASReviews(GridParams gridParams, int mode, long centerid, long seasonid, ref long TotalCount)
        {

            var listResult = new List<CLASReview>();
          

            try
            {
                var stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CLASReviewDetails";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@Mode", mode));

                command.Parameters.Add(new SqlParameter("@PageNo", gridParams.RequestedPage));
                command.Parameters.Add(new SqlParameter("@PageSize", gridParams.PageSize));
                command.Parameters.Add(new SqlParameter("@Search", gridParams.Search == null ? "" : gridParams.Search));
                command.Parameters.Add(new SqlParameter("@Sortclmn", gridParams.SortColumn));
                command.Parameters.Add(new SqlParameter("@Sortdir", gridParams.SortOrder));

                command.Parameters.Add(new SqlParameter("@CenterId", centerid));
                command.Parameters.Add(new SqlParameter("@Season", seasonid));


                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset != null)
                {
                    if (_dataset.Tables[0] != null)
                    {
                        if (_dataset.Tables[0].Rows.Count > 0)
                        {

                            listResult = Fingerprints.Common.DbHelper.DataTableToList<CLASReview>(_dataset.Tables[0], new List<string>());

                        }
                    }

                    if (_dataset.Tables.Count > 1 && _dataset.Tables[1] != null && _dataset.Tables[1].Rows.Count > 0)
                    {


                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            var _attach = new Attachments()
                            {
                                MainTableId = Convert.ToInt64(dr["ReviewId"]),
                                AttachmentFileName = dr["AttachmentName"].ToString(),
                                AttachmentID = Convert.ToInt64(dr["AttachmentID"]),
                                 AttachmentFileExtension = dr["AttachmentExtension"].ToString()

                            };

                            var j = 0;
                            foreach (var item in listResult)
                            {
                                if(item.ReviewId == _attach.MainTableId)
                                {
                                    if (listResult[j].CLASReviewAttachment == null)
                                    {
                                        listResult[j].CLASReviewAttachment = new List<Attachments>();
                                    }

                                    listResult[j].CLASReviewAttachment.Add(_attach);
                                }
                                j++;
                            }

                        }

                        

                    }

                    if (_dataset.Tables.Count > 2 && _dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
                    {
                        TotalCount = DBNull.Value == _dataset.Tables[2].Rows[0]["TotalCount"] ? 0 : Convert.ToInt64(_dataset.Tables[2].Rows[0]["TotalCount"]);
                    }

                    } 

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return listResult;
        }


        public SelectListItem GetCLASAttachmentById(long AttachmentId)
        {
            var imageData = new SelectListItem();

            GridParams gridParams = new GridParams();
            int mode = 2; long centerid = 0; long seasonid = 0;
            try
            {
                var stf = StaffDetails.GetInstance();

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_CLASReviewDetails";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@Mode", mode));

                command.Parameters.Add(new SqlParameter("@PageNo", gridParams.RequestedPage));
                command.Parameters.Add(new SqlParameter("@PageSize", gridParams.PageSize));
                command.Parameters.Add(new SqlParameter("@Search", gridParams.Search == null ? "" : gridParams.Search));
                command.Parameters.Add(new SqlParameter("@Sortclmn", gridParams.SortColumn));
                command.Parameters.Add(new SqlParameter("@Sortdir", gridParams.SortOrder));

                command.Parameters.Add(new SqlParameter("@CenterId", centerid));
                command.Parameters.Add(new SqlParameter("@Season", seasonid));
                command.Parameters.Add(new SqlParameter("@AttachmentId", AttachmentId));


                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);

                if (_dataset != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            imageData.Text = string.IsNullOrEmpty(dr["Attachment"].ToString()) ? "" : Convert.ToBase64String((byte[])dr["Attachment"]);
                            imageData.Value = dr["AttachmentName"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }



                return imageData;

        }

        #endregion CLASReview



        #region MDTReport


        public JsonResult GetUsersDetailsForMDT(long clientid)
        {

            var Parents = new List<SelectListItem>();
            var Facilitator = new List<SelectListItem>();
            var FamilyAdvocate = new List<SelectListItem>();

            try
            {
                var staff = StaffDetails.GetInstance();
                var dbManager = new DBManager(connection.ConnectionString);
                //var result = new { Parents= new List<SelectListItem>(), Facilitator = new List<SelectListItem>(),
               
                //};
               // IDbConnection _connection;
                var parameters = new IDbDataParameter[]
                {


                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@mode",1,DbType.Int32),
                    dbManager.CreateParameter("@ClientId",clientid,DbType.Int64)

                };

                //IDataReader reader = dbManager.GetDataReader("USP_MDTReportDetails", CommandType.StoredProcedure, parameters, out _connection);
                DataSet _dataset = dbManager.GetDataSet("USP_MDTReportDetails", CommandType.StoredProcedure, parameters);

                if (_dataset != null)
                {


                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            Parents.Add(new SelectListItem() { Text = dr["Name"].ToString(), Value = dr["ClientID"].ToString() });
                        }
                    }

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            Facilitator.Add(new SelectListItem() { Text = dr["Name"].ToString(), Value = dr["UserId"].ToString() });
                        }
                    }

                    if (_dataset.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[2].Rows)
                        {
                            FamilyAdvocate.Add(new SelectListItem() { Text = dr["Name"].ToString(), Value = dr["UserId"].ToString() });
                        }
                    }


                }
                //  dbManager.GetDataSet
                //while (reader.Read())
                //{

                //    //reader["CenterID"]
                //}

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return new JsonResult { Data = new {Parents, Facilitator, FamilyAdvocate } };
        }


        public List<MDTReport> GetMDTList()
        {

            var result = new List<MDTReport>();

            try
            {
                var staff = StaffDetails.GetInstance();
                var dbManager = new DBManager(connection.ConnectionString);


                var parameters = new IDbDataParameter[]
                {


                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@mode",3,DbType.Int32),


                };

                //IDataReader reader = dbManager.GetDataReader("USP_MDTReportDetails", CommandType.StoredProcedure, parameters, out _connection);
                DataSet _dataset = dbManager.GetDataSet("USP_MDTReportDetails", CommandType.StoredProcedure, parameters);

                if (_dataset != null)
                {


                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        result = DbHelper.DataTableToList<MDTReport>(_dataset.Tables[0],new List<string>() { });

                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


                        return result;
        }



        //   public MDTReport SubmitMDTForm(MDTReport MDT) {
        public bool SubmitMDTForm(MDTReport MDT)
        {

            //var MDTR = new MDTReport();
            var success = false;

            try
            {

                var staff = StaffDetails.GetInstance();
                //   var dbManager = new DBManager(connection.ConnectionString);
                //var result = new { Parents= new List<SelectListItem>(), Facilitator = new List<SelectListItem>(),

                //};

                MDT.MDTActions = MDT.MDTActions ?? new List<MDTAction>();

                var _ActionTable = new List<string>()
                { "ActionId","MDTId","ActionFor","ActionNotes","Status"};
                var _decrypted = new List<string>() {  };
                DataTable ActionDT = Fingerprints.Common.DbHelper.ToUserDefinedDataTable(MDT.MDTActions, _ActionTable, _decrypted);


                //var stf = StaffDetails.GetInstance();

                //if (Connection.State == ConnectionState.Open)
                //    Connection.Close();

                //Connection.Open();
                //command.Parameters.Clear();
                //command.Connection = Connection;
                //command.CommandType = CommandType.StoredProcedure;
                //command.CommandText = "USP_UpdateMDTReport";
                //command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                //command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                //command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                //command.Parameters.Add(new SqlParameter("@Mode", 1));

                //command.Parameters.Add(new SqlParameter("@ClientId", MDT.ClientId));
                //command.Parameters.Add(new SqlParameter("@MDTId", MDT.MDTId));
                //command.Parameters.Add(new SqlParameter("@Goal", MDT.Goal));
                //command.Parameters.Add(new SqlParameter("@Summary", MDT.Summary));
                //command.Parameters.Add(new SqlParameter("@IsDisability", MDT.IsDisability));
                //command.Parameters.Add(new SqlParameter("@IsMentalIssue", MDT.IsMentalIssue));
                //command.Parameters.Add(new SqlParameter("@IsCompleted", MDT.IsCompleted)); 

                ////parent details
                //command.Parameters.Add(new SqlParameter("@ParentId", MDT.ParentId));
                //command.Parameters.Add(new SqlParameter("@ParentSign", MDT.ParentSign));
                //command.Parameters.Add(new SqlParameter("@ParentSignType", MDT.ParentSignType));

                ////Facilitator  details
                //command.Parameters.Add(new SqlParameter("@FacilitatorId", MDT.FacilitatorId));
                //command.Parameters.Add(new SqlParameter("@FacilitatorSign", MDT.FacilitatorSign));
                //command.Parameters.Add(new SqlParameter("@FacilitatorSignType", MDT.FacilitatorSignType));
                ////Family Advocate details
                //command.Parameters.Add(new SqlParameter("@FamilyAdvocateId", MDT.FamilyAdvocateId));
                //command.Parameters.Add(new SqlParameter("@FamilyAdvocateSign", MDT.FamilyAdvocateSign));
                //command.Parameters.Add(new SqlParameter("@FASignType", MDT.FASignType));

                //command.Parameters.Add(new SqlParameter("@ActionDT", ActionDT));

                //int rowaffected = command.ExecuteNonQuery();

                //if (rowaffected > 0) success = true;



                var dbManager = new DBManager(connection.ConnectionString);
                var parameters = new IDbDataParameter[]
         {


                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@mode",1,DbType.Int32),

                    dbManager.CreateParameter("@ClientId",MDT.ClientId,DbType.Int64),
                    dbManager.CreateParameter("@MDTId",MDT.MDTId,DbType.Int64),
                    dbManager.CreateParameter("@Goal",MDT.Goal,DbType.String),
                    dbManager.CreateParameter("@Summary",MDT.Summary,DbType.String),
                    dbManager.CreateParameter("@IsDisability",MDT.IsDisability,DbType.Boolean),
                    dbManager.CreateParameter("@IsMentalIssue",MDT.IsMentalIssue,DbType.Boolean),

                    //parent details
                    dbManager.CreateParameter("@ParentId", MDT.ParentId,DbType.Int64),
                    dbManager.CreateParameter("@ParentSign",MDT.ParentSign,DbType.String),
                    dbManager.CreateParameter("@ParentSignType",MDT.ParentSignType,DbType.Int32),

                  //  Facilitator  details
                    dbManager.CreateParameter("@FacilitatorId",Guid.Parse(MDT.FacilitatorId),DbType.Guid),
                    dbManager.CreateParameter("@FacilitatorSign",MDT.FacilitatorSign,DbType.String),
                    dbManager.CreateParameter("@FacilitatorSignType",MDT.FacilitatorSignType,DbType.Int32),
                 //   Family Advocate details
                     dbManager.CreateParameter("@FamilyAdvocateId",Guid.Parse(MDT.FamilyAdvocateId),DbType.Guid),
                    dbManager.CreateParameter("@FamilyAdvocateSign",MDT.FamilyAdvocateSign,DbType.String),
                    dbManager.CreateParameter("@FASignType",MDT.FASignType,DbType.Int32),

                    dbManager.CreateParameter("@ActionDT",ActionDT,DbType.Object)

         };
                // DataSet _dataset = dbManager.GetDataSet("USP_UpdateMDTReport", CommandType.StoredProcedure, parameters);

                success = dbManager.ExecuteWithNonQuery<bool>("USP_UpdateMDTReport", CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


            return success;
        }

        public bool SubmitMDTAttachment(long id, HttpPostedFileBase attachment)
        {
            bool success = false;
            try
            {
                var stf = StaffDetails.GetInstance();

                var fbyte = new BinaryReader(attachment.InputStream).ReadBytes(attachment.ContentLength);

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();

                Connection.Open();
                command.Parameters.Clear();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "USP_UpdateMDTReport";
                command.Parameters.Add(new SqlParameter("@AgencyId", stf.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", stf.UserId));
                command.Parameters.Add(new SqlParameter("@RoleId", stf.RoleId));
                command.Parameters.Add(new SqlParameter("@Mode", 2));
                command.Parameters.Add(new SqlParameter("@MDTId", id));

                command.Parameters.Add(new SqlParameter("@MDTAttachment", fbyte));
                command.Parameters.Add(new SqlParameter("@AttachmentName", attachment.FileName));
                command.Parameters.Add(new SqlParameter("@AttachmentExtension", Path.GetExtension(attachment.FileName)));


                int rowaffected = command.ExecuteNonQuery();

                if (rowaffected > 0) success = true;


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return success;
        }

        public MDTReport GetMDTReportById(long id, string _for = "edit")
        {

            var MDTReport = new MDTReport();
            MDTReport.AgnecyInfo = new Agency();
            MDTReport.MDTActions = new List<MDTAction>();
          var _MDTActions = new List<MDTAction>();
            try
            {
                var staff = StaffDetails.GetInstance();
                var dbManager = new DBManager(connection.ConnectionString);


                var parameters = new IDbDataParameter[]
                {


                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@mode",2,DbType.Int32),
                    dbManager.CreateParameter("@ClientId",0,DbType.Int64),
                    dbManager.CreateParameter("@MDTId",id,DbType.Int64)

                };

                //IDataReader reader = dbManager.GetDataReader("USP_MDTReportDetails", CommandType.StoredProcedure, parameters, out _connection);
                DataSet _dataset = dbManager.GetDataSet("USP_MDTReportDetails", CommandType.StoredProcedure, parameters);

                if (_dataset != null)
                {


                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        //  MDTReport = DbHelper.DataTableToList<MDTReport>(_dataset.Tables[0], new List<string>() { })[0];

                        var _tmp = _dataset.Tables[0].Rows[0];

                        MDTReport.MDTId = Convert.ToInt64(_tmp["MDTId"].ToString());
                        MDTReport.ClientId = Convert.ToInt64(_tmp["ClientId"].ToString());

                        MDTReport.AgencyName = _tmp["AgencyName"].ToString();
                        MDTReport.CreatedDate = _tmp["CreatedDate"].ToString();
                        MDTReport.CenterName= _tmp["CenterName"].ToString();
                        MDTReport.DOB = _tmp["DOB"].ToString();

                        // MDTReport.IsCompleted = bool.Parse( _tmp["IsCompleted"].ToString() ?? "False");
                        //MDTReport.HaveAttachment = bool.Parse(_tmp["HaveAttachment"].ToString() ?? "False");
                        MDTReport.IsCompleted = bool.Parse( _tmp["IsCompleted"].ToString());
                        MDTReport.HaveAttachment = bool.Parse(_tmp["HaveAttachment"].ToString());

                        MDTReport.Goal = _tmp["Goal"].ToString();
                        MDTReport.Name = _tmp["Name"].ToString();
                        MDTReport.Summary = _tmp["Summary"].ToString();
                        MDTReport.IsDisability = Convert.ToBoolean(_tmp["IsDisability"].ToString());
                        MDTReport.IsMentalIssue = Convert.ToBoolean(_tmp["IsMentalIssue"].ToString());

                        MDTReport.ParentId = Convert.ToInt64(_tmp["ParentId"].ToString());
                        
                        MDTReport.ParentSignType = Convert.ToInt32(_tmp["ParentSignType"].ToString());

                        MDTReport.FacilitatorId = _tmp["FacilitatorId"].ToString();
                        
                        MDTReport.FacilitatorSignType = Convert.ToInt32(_tmp["FacilitatorSignType"].ToString());

                        MDTReport.FamilyAdvocateId = _tmp["FamilyAdvocateId"].ToString();
                       
                        MDTReport.FASignType =  Convert.ToInt32(_tmp["FASignType"].ToString());


                        MDTReport.ParentSign = _tmp["ParentSign"].ToString();
                        MDTReport.FacilitatorSign = _tmp["FacilitatorSign"].ToString();
                        MDTReport.FamilyAdvocateSign = _tmp["FamilyAdvocateSign"].ToString();

/*
                        if (_for == "pdf")
                        {
                             
                            MDTReport.ParentSign= Fingerprints.Common.Helpers.ImageHelper.GetBase64Png(DBNull.Value == _tmp["ParentSign"] ? "{\"lines\":[]}" : _tmp["ParentSign"].ToString(), 400, 200);
                            MDTReport.FacilitatorSign = Fingerprints.Common.Helpers.ImageHelper.GetBase64Png(DBNull.Value == _tmp["FacilitatorSign"] ? "{\"lines\":[]}" : _tmp["FacilitatorSign"].ToString(), 400, 200);
                            MDTReport.FamilyAdvocateSign = Fingerprints.Common.Helpers.ImageHelper.GetBase64Png(DBNull.Value == _tmp["FamilyAdvocateSign"] ? "{\"lines\":[]}" : _tmp["FamilyAdvocateSign"].ToString(), 400, 200);
                        }
                        else {
                            MDTReport.ParentSign = _tmp["ParentSign"].ToString();
                            MDTReport.FacilitatorSign = _tmp["FacilitatorSign"].ToString();
                            MDTReport.FamilyAdvocateSign = _tmp["FamilyAdvocateSign"].ToString();

                        }


                        */


                        MDTReport.AgnecyInfo = new Agency()
                        {
                            address1 = _tmp["Address1"].ToString(),
                            address2 = _tmp["Address2"].ToString(),
                            State = _tmp["State"].ToString(),
                            zipCode = _tmp["ZipCode"].ToString(),
                            phone1 = _tmp["Phone1"].ToString()
                        };
                        //foreach (DataRow dr in _dataset.Tables[0].Rows)
                        //{
                        //    Parents.Add(new SelectListItem() { Text = dr["Name"].ToString(), Value = dr["ClientID"].ToString() });
                        //}
                    }

                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        _MDTActions = DbHelper.DataTableToList<MDTAction>(_dataset.Tables[1], new List<string>() { });
                        MDTReport.MDTActions = _MDTActions;
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

                    return MDTReport;

        }


        public Attachments GetMDTAttachmentById(long id)
        {
            var _attchment = new Attachments();

            try
            {
                var staff = StaffDetails.GetInstance();
                var dbManager = new DBManager(connection.ConnectionString);


                var parameters = new IDbDataParameter[]
                {


                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@mode",4,DbType.Int32),
                    dbManager.CreateParameter("@MDTId",id,DbType.Int64)

                };


                DataSet _dataset = dbManager.GetDataSet("USP_MDTReportDetails", CommandType.StoredProcedure, parameters);

                if (_dataset != null)
                {
                    if (_dataset.Tables.Count > 0 && _dataset.Tables[0].Rows.Count > 0)
                    {
                        var _tR = _dataset.Tables[0].Rows[0];
                        _attchment.AttachmentFileByte = (byte[])_tR["Attachment"];
                        _attchment.AttachmentFileName = _tR["Attachment"].ToString();
                        _attchment.AttachmentFileExtension = _tR["AttachmentExtension"].ToString();

                    }
                }

            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }
            return _attchment;
        }

        #endregion MDTReport

        #region Family Activity Report

        public FamilyActivityReport GetFamilyActivityReport(StaffDetails staff, FamilyActivityReport modal)
        {
            IDbConnection dbConnection;

            try
            {


                modal.FamilyActivityList = new List<FamilyActivityModel>();

                var centerIds = string.Join(",", modal.CenterIDs.Select(x => EncryptDecrypt.Decrypt64(x)));
              //  var classroomIds = string.Join(",", modal.ClassroomIDs.Select(x => EncryptDecrypt.Decrypt64(x)));
                var months = string.Join(",", modal.Months.Select(x => x));
                var dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);
                var parameters = new IDbDataParameter[]
                {

                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),

                  //  dbManager.CreateParameter("@Months",months,DbType.String),
                    dbManager.CreateParameter("@CenterIDs",centerIds,DbType.String),
                   // dbManager.CreateParameter("@ClassroomIDs",classroomIds,DbType.String),
                   // dbManager.CreateParameter("@Take",modal.PageSize,DbType.Int32),
                   // dbManager.CreateParameter("@Skip",modal.SkipRows,DbType.Int32),
                    dbManager.CreateParameter("@SortOrder",modal.SortOrder,DbType.String),
                    dbManager.CreateParameter("@SortColumn",modal.SortColumn,DbType.String),
                    dbManager.CreateParameter("@SearchTerm",modal.SearchTerm,DbType.String),

                    dbManager.CreateParameter("@TotalRecord",int.MaxValue,0,DbType.Int32,ParameterDirection.Output)
                };

                IDataReader reader = dbManager.GetDataReader("USP_GetFamilyActivityReport", CommandType.StoredProcedure, parameters, out dbConnection);


                while (reader.Read())
                {
                    modal.FamilyActivityList.Add(new FamilyActivityModel
                    {
                        CenterID = reader["CenterID"] == DBNull.Value ? "0" : Convert.ToString(reader["CenterID"]),
                        CenterName = reader["CenterName"] == DBNull.Value ? "0" : Convert.ToString(reader["CenterName"]),
                       // ClassroomID = reader["ClassroomID"] == DBNull.Value ? "0" : Convert.ToString(reader["ClassroomID"]),
                      //  ClassroomName = reader["ClassroomName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ClassroomName"]),
                        Month=reader["Month"]==DBNull.Value?string.Empty:Convert.ToString(reader["Month"]),
                        MonthLastDate=reader["MonthEnd"] ==DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["MonthEnd"].ToString(),new CultureInfo("en-US",true)),
                        FPA = reader["FPA"] == DBNull.Value ? 0 : Convert.ToInt64(reader["FPA"]),
                        Referral = reader["Referral"] == DBNull.Value ? 0 : Convert.ToInt64(reader["Referral"]),
                        InternalReferral = reader["InternalReferral"] == DBNull.Value ? 0 : Convert.ToInt64(reader["InternalReferral"]),
                        QualityOfReferral = reader["QualityOfReferral"] == DBNull.Value ? 0 : Convert.ToInt64(reader["QualityOfReferral"]),
                        StepUpToQualityStars = reader["StepUpToQualityStars"] == DBNull.Value ? "0" : Convert.ToString(reader["StepUpToQualityStars"])

                    });
                }

                reader.Close();
                dbManager.CloseConnection(dbConnection);

                modal.TotalRecord = (int)parameters.Where(x => x.ParameterName == "@TotalRecord" && x.Direction == ParameterDirection.Output).Select(x => x.Value).First();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {

            }



            return modal;
        }


        #endregion

        #region Center Monthly Report

        public CenterMonthlyReport GetCenterMonthlyReport(StaffDetails staff, CenterMonthlyReport report)
        {
            try
            {
                var dbManager = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);

                report.CenterMonthlyReportList = new List<CenterMonthlyReportModel>();

                var centerIds = string.Join(",", report.CenterIDs.Select(x => EncryptDecrypt.Decrypt64(x)));
                var months = string.Join(",", report.Months.Select(x => x));

                var parameters = new IDbDataParameter[]
                {

                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@CenterIDs",centerIds,DbType.String),
                    dbManager.CreateParameter("@Months",months,DbType.String)

                };

                _dataset = dbManager.GetDataSet("USP_GetCenterMonthlyReport", CommandType.StoredProcedure, parameters);


                if (_dataset != null && _dataset.Tables.Count > 0)
                {

                    int i = 0;

                    while (i < report.Months.Length)
                    {


                        List<CenterMonthlyReportModel> modelList = (from DataRow dr in _dataset.Tables[0].Rows
                                                                    where Convert.ToInt32(dr["MonthNumber"]) == report.Months[i]
                                                                    select new CenterMonthlyReportModel
                                                                    {
                                                                        CenterID = dr["CenterID"] != DBNull.Value ? EncryptDecrypt.Encrypt64(dr["CenterID"].ToString()) : string.Empty,
                                                                        CenterName = dr["CenterName"] != DBNull.Value ? Convert.ToString(dr["CenterName"]) : string.Empty,
                                                                        StepUpToQualityStars = dr["StepUpToQualityStars"] != DBNull.Value ? Convert.ToString(dr["StepUpToQualityStars"]) : string.Empty,
                                                                        Month = dr["Month"] != DBNull.Value ? Convert.ToString(dr["Month"]) : string.Empty,
                                                                        MonthLastDate = dr["MonthEnd"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(dr["MonthEnd"].ToString(), new CultureInfo("en-US", true)),
                                                                        ADA = dr["ADA"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["ADA"]) == 0 ? 0 : Convert.ToDecimal(dr["ADA"]),
                                                                        ExplanationADAUnderPercentage = dr["ExplanationADAUnderPercentage"] != DBNull.Value ? Convert.ToString(dr["ExplanationADAUnderPercentage"]) : string.Empty,

                                                                        FamilyServiceWorkers = (_dataset.Tables.Count > 1 && _dataset.Tables[1].Rows.Count > 0) ? (from DataRow dr1 in _dataset.Tables[1].Rows
                                                                                                                                                                   where Convert.ToInt64(dr1["CenterID"]) == Convert.ToInt64(dr["CenterID"]) && Convert.ToString(dr1["RoleID"]).ToLowerInvariant() == FingerprintsModel.EnumHelper.GetEnumDescription(FingerprintsModel.Enums.RoleEnum.FamilyServiceWorker).ToLowerInvariant()
                                                                                                                                                                   select new SelectListItem
                                                                                                                                                                   {
                                                                                                                                                                       Text = string.Concat(Convert.ToString(dr1["FirstName"]), " ", Convert.ToString(dr1["LastName"])),
                                                                                                                                                                       Value = Convert.ToString(dr1["RoleID"])


                                                                                                                                                                   }).ToList() : new List<SelectListItem>(),

                                                                        CenterCordinators = (_dataset.Tables.Count > 1 && _dataset.Tables[1].Rows.Count > 0) ? (from DataRow dr1 in _dataset.Tables[1].Rows
                                                                                                                                                                where Convert.ToInt64(dr1["CenterID"]) == Convert.ToInt64(dr["CenterID"]) && Convert.ToString(dr1["RoleID"]).ToLowerInvariant() == FingerprintsModel.EnumHelper.GetEnumDescription(FingerprintsModel.Enums.RoleEnum.CenterManager).ToLowerInvariant()
                                                                                                                                                                select new SelectListItem
                                                                                                                                                                {
                                                                                                                                                                    Text = string.Concat(Convert.ToString(dr1["FirstName"]), " ", Convert.ToString(dr1["LastName"])),
                                                                                                                                                                    Value = Convert.ToString(dr1["RoleID"])


                                                                                                                                                                }).ToList() : new List<SelectListItem>(),
                                                                        ChildFamilyReview = (_dataset.Tables.Count > 2 && _dataset.Tables[2].Rows.Count > 0) ? (from DataRow dr2 in _dataset.Tables[2].Rows
                                                                                                                                                                where Convert.ToInt32(dr2["MonthNumber"]) == report.Months[i] && Convert.ToInt64(dr2["CenterID"]) == Convert.ToInt64(dr["CenterID"])
                                                                                                                                                                select new SelectListItem
                                                                                                                                                                {
                                                                                                                                                                    Text = dr2["StaffName"] == DBNull.Value ? string.Empty : Convert.ToString(dr2["StaffName"]),
                                                                                                                                                                    Value = dr2["CFRDate"] == DBNull.Value ? string.Empty : Convert.ToString(dr2["CFRDate"])
                                                                                                                                                                }


                                                                                                                                                        ).FirstOrDefault() : new SelectListItem(),



                                                                        FPA = (_dataset.Tables.Count > 3 && _dataset.Tables[3].Rows.Count > 0) ? (from DataRow dr3 in _dataset.Tables[3].Rows
                                                                                                                                                  where Convert.ToInt32(dr3["MonthNumber"]) == report.Months[i] && Convert.ToInt64(dr3["CenterID"]) == Convert.ToInt64(dr["CenterID"])
                                                                                                                                                  select new SelectListItem
                                                                                                                                                  {
                                                                                                                                                      Text = dr3["StaffName"] == DBNull.Value ? string.Empty : Convert.ToString(dr3["StaffName"]).Trim(),
                                                                                                                                                      Value = dr3["FPA"] == DBNull.Value ? "0" : Convert.ToString(dr3["FPA"])
                                                                                                                                                  }).OrderBy(x => x.Text).ToList() : new List<SelectListItem>(),

                                                                        Referral = (_dataset.Tables.Count > 4 && _dataset.Tables[4].Rows.Count > 0) ? (from DataRow dr4 in _dataset.Tables[4].Rows
                                                                                                                                                       where Convert.ToInt32(dr4["MonthNumber"]) == report.Months[i] && Convert.ToInt64(dr4["CenterID"]) == Convert.ToInt64(dr["CenterID"])
                                                                                                                                                       select new SelectListItem
                                                                                                                                                       {
                                                                                                                                                           Text = dr4["StaffName"] == DBNull.Value ? string.Empty : Convert.ToString(dr4["StaffName"]).Trim(),
                                                                                                                                                           Value = dr4["Referral"] == DBNull.Value ? string.Empty : Convert.ToString(dr4["Referral"])
                                                                                                                                                       }).ToList() : new List<SelectListItem>(),


                                                                        FSWHomeVisit = (_dataset.Tables.Count > 5 && _dataset.Tables[5].Rows.Count > 0) ? (from DataRow dr5 in _dataset.Tables[5].Rows
                                                                                                                                                           where Convert.ToInt32(dr5["MonthNumber"]) == report.Months[i] && Convert.ToInt64(dr5["CenterID"]) == Convert.ToInt64(dr["CenterID"])

                                                                                                                                                           select new SelectListItem
                                                                                                                                                           {
                                                                                                                                                               Text = dr5["StaffName"] == DBNull.Value ? string.Empty : Convert.ToString(dr5["StaffName"]).Trim(),
                                                                                                                                                               Value = dr5["FSWHomeVisit"] == DBNull.Value ? "0" : Convert.ToString(dr5["FSWHomeVisit"])
                                                                                                                                                           }).OrderBy(x => x.Text).ToList() : new List<SelectListItem>(),

                                                                        TeacherHomeVisit = (_dataset.Tables.Count > 6 && _dataset.Tables[6].Rows.Count > 0) ? (from DataRow dr6 in _dataset.Tables[6].Rows
                                                                                                                                                               where Convert.ToInt32(dr6["MonthNumber"]) == report.Months[i] && Convert.ToInt64(dr6["CenterID"]) == Convert.ToInt64(dr["CenterID"])
                                                                                                                                                               select new SelectListItem
                                                                                                                                                               {
                                                                                                                                                                   Text = dr6["StaffName"] == DBNull.Value ? string.Empty : Convert.ToString(dr6["StaffName"]).Trim(),
                                                                                                                                                                   Value = dr6["TeacherHomeVisit"] == DBNull.Value ? "0" : Convert.ToString(dr6["TeacherHomeVisit"])
                                                                                                                                                               }).ToList() : new List<SelectListItem>(),

                                                                        ParentTeacherConference = (_dataset.Tables.Count > 7 && _dataset.Tables[7].Rows.Count > 0) ? (from DataRow dr7 in _dataset.Tables[7].Rows

                                                                                                                                                                      where Convert.ToInt32(dr7["MonthNumber"]) == report.Months[i] && Convert.ToInt64(dr7["CenterID"]) == Convert.ToInt64(dr["CenterID"])
                                                                                                                                                                      select new SelectListItem
                                                                                                                                                                      {
                                                                                                                                                                          Text = dr7["StaffName"] == DBNull.Value ? string.Empty : Convert.ToString(dr7["StaffName"]).Trim(),
                                                                                                                                                                          Value = dr7["ParentTeacherConference"] == DBNull.Value ? "0" : Convert.ToString(dr7["ParentTeacherConference"])
                                                                                                                                                                      }

                                                                                                                                                               ).ToList() : new List<SelectListItem>(),





                                                                        ParentMeeting = (_dataset.Tables.Count > 8 && _dataset.Tables[8].Rows.Count > 0) ? (from DataRow dr8 in _dataset.Tables[8].Rows
                                                                                                                                                            where Convert.ToInt32(dr8["MonthNumber"]) == report.Months[i] && Convert.ToInt64(dr8["CenterID"]) == Convert.ToInt64(dr["CenterID"])
                                                                                                                                                            select new ParentMeeting
                                                                                                                                                            {
                                                                                                                                                                WorkshopName = dr8["WorkshopTitle"] != DBNull.Value ? Convert.ToString(dr8["WorkshopTitle"]) : string.Empty,
                                                                                                                                                                Description = dr8["EventDescription"] != DBNull.Value ? Convert.ToString(dr8["EventDescription"]) : string.Empty,
                                                                                                                                                                AttendanceCount = dr8["Attended"] != DBNull.Value ? Convert.ToInt32(dr8["Attended"]) : 0,
                                                                                                                                                                EducationComponentDescription = dr8["EducationComponentDescription"] != DBNull.Value ? Convert.ToString(dr8["EducationComponentDescription"]) : string.Empty

                                                                                                                                                            }


                                                                                                                                                   ).FirstOrDefault() : new ParentMeeting(),

                                                                        RecruitmentActivitiesList = (_dataset.Tables.Count > 9 && _dataset.Tables[9].Rows.Count > 0) ? (from DataRow dr9 in _dataset.Tables[9].Rows
                                                                                                                                                                        where Convert.ToInt32(dr9["MonthNumber"]) == report.Months[i] && Convert.ToInt64(dr9["CenterID"]) == Convert.ToInt64(dr["CenterID"])
                                                                                                                                                                        select new RecruitmentActivities
                                                                                                                                                                        {
                                                                                                                                                                            EnteredBy = string.Concat(Convert.ToString(dr9["FirstName"]).Trim(), " " + Convert.ToString(dr9["LastName"]).Trim(), " ", "(", Convert.ToString(dr9["RoleName"]).Trim(), ")").Trim(),
                                                                                                                                                                            Description = Convert.ToString(dr9["Description"])

                                                                                                                                                                        }).ToList() : new List<RecruitmentActivities>()
                                                                    }

                                                       ).ToList();


                        modelList.ForEach(x => x.ChildFamilyReview = x.ChildFamilyReview ?? new SelectListItem());


                        report.CenterMonthlyReportList.AddRange(modelList);

                        i++;
                    }


                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


            return report;
        }

        #endregion


        #region UFCReport

        public UFCReport GetUFCReport(UFCReport ufcReport)
        {

            ufcReport.UFCReportList = new List<UFCReport>();
            ufcReport.CenterList = new List<SelectListItem>();

            //  IDbConnection dbConnection;
            try
            {

                string centers = string.Join(",", ufcReport.CenterID.Split(',').Select(x => EncryptDecrypt.Decrypt64(x)).ToArray());
                StaffDetails staff = StaffDetails.GetInstance();
                var dbManager = FactoryInstance.Instance.CreateInstance<DBManager>(connection.ConnectionString);
                var parameters = new IDbDataParameter[]
                {

                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@Take",ufcReport.PageSize,DbType.Int32),
                    dbManager.CreateParameter("@Skip",ufcReport.SkipRows,DbType.Int32),
                    dbManager.CreateParameter("@CenterID",!string.IsNullOrEmpty(ufcReport.CenterID) && ufcReport.CenterID!="0"?string.Join(",",ufcReport.CenterID.Split(',').Select(x=>EncryptDecrypt.Decrypt64(x)).ToArray()) :"0",DbType.String),
                    dbManager.CreateParameter("@Months",!string.IsNullOrEmpty(ufcReport.MonthType) && ufcReport.MonthType!="0"?ufcReport.MonthType:"",DbType.String),
                    dbManager.CreateParameter("@SortOrder",ufcReport.SortOrder,DbType.String),
                    dbManager.CreateParameter("@SortColumn",ufcReport.SortColumn,DbType.String),
                    dbManager.CreateParameter("@SearchTerm",ufcReport.SearchTerm,DbType.String),
                    dbManager.CreateParameter("@Mode",(int)ufcReport.ReportMode,DbType.Int32)
                };

               
                _dataset = dbManager.GetDataSet("USP_UFCReportDetails", CommandType.StoredProcedure, parameters);




                var householdIdList = new List<Int64>();
                var monthList = new List<string>();
                var centerList = new List<long>();

                if (_dataset != null && _dataset.Tables.Count > 0)
                {
                    if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                    {
                        householdIdList = (from DataRow dr in _dataset.Tables[0].Rows
                                           select Convert.ToInt64(dr["HouseholdId"])
                                         ).Distinct().ToList();

                        monthList = (from DataRow dr in _dataset.Tables[0].Rows
                                     select Convert.ToString(dr["MonthEndDate"]).Trim()
                                         ).Distinct().ToList();


                        centerList = (from DataRow dr in _dataset.Tables[0].Rows
                                     select Convert.ToInt64(dr["CenterID"])
                                         ).Distinct().ToList();

                        foreach(var center in centerList)
                        {
                            foreach (var months in monthList)
                            {
                                foreach (var item in householdIdList)
                                {


                                    if ((from DataRow dr in _dataset.Tables[0].Rows
                                         where Convert.ToInt32(dr["HouseholdID"]) == item
                                            && Convert.ToString(dr["MonthEndDate"]).Trim() == months.Trim()
                                            && Convert.ToInt64(dr["CenterID"])==center
                                         select 1
                                           ).Any())
                                    {


                                        ufcReport.UFCReportList.Add((from DataRow dr in _dataset.Tables[0].Rows
                                                                     where Convert.ToInt32(dr["HouseholdID"]) == item
                                                                        && Convert.ToString(dr["MonthEndDate"]).Trim() == months.Trim()
                                                                        && Convert.ToInt64(dr["CenterID"])==center
                                                                     select new UFCReport
                                                                     {
                                                                         CenterID = dr["CenterID"] != DBNull.Value ? EncryptDecrypt.Encrypt64(Convert.ToString(dr["CenterID"])) : "0",
                                                                         CenterName = Convert.ToString(dr["CenterName"] ?? string.Empty),
                                                                         StepUpToQualityStars = Convert.ToString(dr["StepUpToQualityStars"]),
                                                                         LastCaseNoteDate = Convert.ToString(dr["LastCaseNoteDate"] ?? ""),
                                                                         HouseholdId = Convert.ToInt64(dr["HouseholdId"] ?? 0),
                                                                         Month = Convert.ToInt64(dr["MonthNumber"] ?? 0),
                                                                         MonthType = Convert.ToString(dr["MonthType"] ?? ""),
                                                                         Children = (from DataRow dr1 in _dataset.Tables[0].Rows
                                                                                     where Convert.ToInt64(dr1["HouseholdID"]) == Convert.ToInt64(dr["HouseholdID"])
                                                                                     && Convert.ToString(dr1["MonthEndDate"]).Trim() == Convert.ToString(dr["MonthEndDate"]).Trim()
                                                                                     && Convert.ToInt64(dr1["CenterID"])== Convert.ToInt64(dr["CenterID"])
                                                                                     select new UFCClientDetails
                                                                                     {
                                                                                         ClientName = Convert.ToString(dr1["ChildName"]),
                                                                                         EnrollmentStatus = Convert.ToInt32(dr1["EnrollmentStatus"])
                                                                                     }).Distinct().ToList(),
                                                                         Parents = Convert.ToString(dr["Parent1Name"]) + ", " + Convert.ToString(dr["Parent2Name"]),
                                                                         MonthLastDate = dr["MonthEndDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(dr["MonthEndDate"].ToString(), new CultureInfo("en-US", true)),

                                                                     }).Distinct().FirstOrDefault());
                                    }
                                }
                            }


                        }

                        



                    }

                }


                if (_dataset.Tables.Count>1 && _dataset.Tables[1]!=null && _dataset.Tables[1].Rows.Count>0)
                {
                   foreach(DataRow dr2 in _dataset.Tables[1].Rows)
                    {
                        ufcReport.UFCReportList.ForEach(x =>
                        {
                            x.TotalRecord = x.CenterID == EncryptDecrypt.Encrypt64(Convert.ToString(dr2["CenterID"])) ? Convert.ToInt32(dr2["TotalRecord"]) : x.TotalRecord;
                            x.SortOrder = ufcReport.SortOrder;
                            x.SortColumn = ufcReport.SortColumn;
                        });
                    }

                    }





                if (_dataset.Tables.Count > 2 && _dataset.Tables[2] != null && _dataset.Tables[2].Rows.Count > 0)
                {
                    foreach(DataRow dr3 in _dataset.Tables[2].Rows)
                    {
                        ufcReport.CenterList.Add(new SelectListItem
                        {
                            Text = Convert.ToString(dr3["CenterName"]),
                            Value = EncryptDecrypt.Encrypt64(Convert.ToString(dr3["CenterID"]))
                        });
                    }
                }





                //while (reader.Read())
                //{
                //    //modal.FamilyActivityList.Add(new FamilyActivityModel
                //    //{
                //    //    CenterID = reader["CenterID"] == DBNull.Value ? "0" : Convert.ToString(reader["CenterID"]),
                //    //}
                //    string parents = Convert.ToString(reader["Parent1Name"] ?? "");
                //    if (DBNull.Value != reader["Parent2Name"]) {
                //        parents += ", " + Convert.ToString(reader["Parent2Name"] ?? "");
                //    }
                //    result.Add(new UFCReport()
                //    {
                //        CenterId = Convert.ToInt64(reader["CenterID"] ?? 0),
                //        CenterName = Convert.ToString(reader["CenterName"] ?? ""),
                //        LastCaseNoteDate = Convert.ToString(reader["LastCaseNoteDate"] ?? ""),
                //        HouseholdId = Convert.ToInt64(reader["HouseholdId"] ?? 0),
                //        Month = Convert.ToInt64(reader["MonthNumber"] ?? 0),
                //        //  Parents = Convert.ToString(reader["Parent1Name"] ?? 0) +","+ Convert.ToString(reader["Parent2Name"] ?? 0),
                //      //  Parents = parents,
                //     //   Children=Convert.ToString(reader["Children"]),
                //        MonthType = Convert.ToString(reader["MonthType"] ?? ""),

                //    });

                //}

                //reader.Close();
                //dbManager.CloseConnection(dbConnection);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {

            }
            return ufcReport;
        }


        public MemoryStream GetUFCReportExcel(List<UFCReport> dataList, string imagePath)
        {

            MemoryStream memoryStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<MemoryStream>();
            try
            {
                XLWorkbook wb = new XLWorkbook();


                if (dataList != null && dataList.Count > 0)
                {
                    var familyActivityModel = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<FamilyActivityModel>();
                    var displayNameHelper = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<Fingerprints.Common.Helpers.DisplayNameHelper>();

                    var centerList = dataList.Select(x => x.CenterID).Distinct().ToList();
                    for (int i = 0; i < centerList.Count; i++)
                    {
                        #region Adding Worksheet

                        var reportWithCenterList = dataList.OrderBy(x => x.Month).Where(x => x.CenterID == centerList[i]).ToList();
                        var centerName = reportWithCenterList.Select(x => x.CenterName).First();


                        var vs = wb.Worksheets.Add(centerName.Length > 31 ? centerName.Substring(0, 15) : centerName);


                        #region Headers with Quality Stars

                        // string starImageUrl = imagePath + "\\220px-Star_rating_" + reportWithCenterList[0].StepUpToQualityStars + "_of_5.png";
                        // System.Drawing.Bitmap fullImage = new System.Drawing.Bitmap(starImageUrl);

                        //vs.AddPicture(fullImage).MoveTo(vs.Cell("F2"), new System.Drawing.Point(100, 1)).Scale(0.3);// optional: resize picture
                        //vs.Range("F2:F2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        vs.Range("B2:D2").Merge().Value = centerName;
                        vs.Range("B2:D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        vs.Range("B2:D2").Style.Font.SetBold(true);
                        vs.Range("B2:D2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        vs.Range("B3:D3").Merge().Value = "UFC Report";
                        vs.Range("B3:D3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        vs.Range("B3:D3").Style.Font.SetBold(true);
                        vs.Range("B3:D3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        #endregion Headers with Quality Stars


                        #region Table Headers

                        vs.Cell(4, 2).Value = "Month";
                        vs.Cell(4, 2).Style.Font.SetBold(true);
                        vs.Cell(4, 2).WorksheetColumn().Width = 30;

                        vs.Cell(4, 3).Value = "Parents";
                        vs.Cell(4, 3).Style.Font.SetBold(true);
                        vs.Cell(4, 3).WorksheetColumn().Width = 30;

                        vs.Cell(4, 4).Value = "Last Case Note Date";
                        vs.Cell(4, 4).Style.Font.SetBold(true);
                        vs.Cell(4, 4).WorksheetColumn().Width = 30;
                        vs.Range("B4:D4").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        vs.Range("B4:D4").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                        vs.Range("B4:D4").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                        #endregion Table Headers


                        #region Table Rows

                        int inititalRow = 5;

                        int ReportRow = inititalRow;
                        int Reportcolumn = 2;

                        for (int j = 0; j < reportWithCenterList.Count; j++)
                        {
                            bool isFeaturedMonth = false;

                            // bool isFeaturedMonth = Array.IndexOf(dataList., reportWithCenterList[j].MonthLastDate.GetValueOrDefault().Month) > -1;


                            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(reportWithCenterList[j].Month)).Substring(0, 3);


                            vs.Cell(ReportRow, Reportcolumn).DataType = XLDataType.Text;
                            // vs.Cell(ReportRow, Reportcolumn).Value = reportWithCenterList[j].Month.Replace("-","");
                           vs.Cell(ReportRow, Reportcolumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn).Style.Font.SetBold(isFeaturedMonth);
                            vs.Cell(ReportRow, Reportcolumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            // vs.Cell(ReportRow, Reportcolumn).SetValue<string>(Convert.ToString(reportWithCenterList[j].Month));
                            vs.Cell(ReportRow, Reportcolumn).SetValue<string>(Convert.ToString(monthName+"-"+ reportWithCenterList[j].MonthType));


                            vs.Cell(ReportRow, Reportcolumn + 1).Value = reportWithCenterList[j].Parents;
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Font.SetBold(isFeaturedMonth);
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            vs.Cell(ReportRow, Reportcolumn + 2).Value = reportWithCenterList[j].LastCaseNoteDate;
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(isFeaturedMonth);
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            //vs.Cell(ReportRow, Reportcolumn + 3).Value = reportWithCenterList[j].LastCaseNoteDate;
                            //vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            //vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(isFeaturedMonth);
                            //vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                     
                            ReportRow++;


                        }


                        #endregion Table Rows




                        #endregion Adding Worksheet





                    }
                    wb.SaveAs(memoryStream);
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return memoryStream;

                }

        #endregion UFCReport
        #region Get Substitute Role Report

        public SubstituteRole GetSubstituteRoleReport(StaffDetails staff, SubstituteRole substituteRole)
        {
            IDbConnection dbConnection = null;
            IDataReader reader = null;
            var dbManager = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<FingerprintsDataAccessHandler.DBManager>(connection.ConnectionString);

            try
            {
                substituteRole.SubsituteRoleList = new List<SubstituteRole>();
                substituteRole.CenterList = new List<SelectListItem>();

                var parameters = new IDbDataParameter[]
                {
                    dbManager.CreateParameter("@AgencyID",staff.AgencyId,DbType.Guid),
                    dbManager.CreateParameter("@RoleID",staff.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@UserID",staff.UserId,DbType.Guid),
                    dbManager.CreateParameter("@StaffRoleID",substituteRole.StaffDetails.RoleId,DbType.Guid),
                    dbManager.CreateParameter("@Take",substituteRole.PageSize,DbType.Int32),
                    dbManager.CreateParameter("@Skip",substituteRole.SkipRows,DbType.Int32),
                    dbManager.CreateParameter("@CenterID",!string.IsNullOrEmpty(substituteRole.CenterID) && substituteRole.CenterID!="0"?string.Join(",",substituteRole.CenterID.Split(',').Select(x=>EncryptDecrypt.Decrypt64(x)).ToArray()) :"0",DbType.String),
                    dbManager.CreateParameter("@Months",!string.IsNullOrEmpty(substituteRole.Month) && substituteRole.Month!="0"?substituteRole.Month:"",DbType.String),
                    dbManager.CreateParameter("@SortOrder",substituteRole.SortOrder,DbType.String),
                    dbManager.CreateParameter("@SortColumn",substituteRole.SortColumn,DbType.String),
                    dbManager.CreateParameter("@SearchTerm",substituteRole.SearchTerm,DbType.String),
                    dbManager.CreateParameter("@Mode",substituteRole.SubstituteRoleMode,DbType.Int32)

                };

                reader = dbManager.GetDataReader("USP_GetSubstituteRoleReport", CommandType.StoredProcedure, parameters, out dbConnection);


                while (reader.Read())
                {
                    substituteRole.SubsituteRoleList.Add(new SubstituteRole
                    {
                        CenterID = EncryptDecrypt.Encrypt64(Convert.ToString(reader["CenterID"])),
                        ClassroomID = EncryptDecrypt.Encrypt64(Convert.ToString(reader["ClassroomID"])),
                        CenterName = Convert.ToString(reader["CenterName"]),
                        ClassroomName = Convert.ToString(reader["ClassroomName"]),
                        StaffDetails = new StaffDetails(false)
                        {
                            UserId = new Guid(Convert.ToString(reader["UserID"])),
                            FullName = string.Concat(Convert.ToString(reader["FirstName"]), " ", Convert.ToString(reader["LastName"])).Trim(),
                            RoleId = new Guid(Convert.ToString(reader["RoleID"]))
                        },
                        SubstituteID = Convert.ToInt64(reader["SubstituteID"]),
                        FromDate = Convert.ToString(reader["FromDate"]),
                        ToDate = Convert.ToString(reader["ToDate"]),
                        Month = Convert.ToString(reader["Month"]),
                        StepUpToQualityStars=Convert.ToString(reader["StepUptoQualityStars"]),
                        MonthLastDate = reader["MonthEndDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["MonthEndDate"].ToString(), new CultureInfo("en-US", true)),
                    });
                }

                //if (substituteRole.SubstituteRoleMode == 2)
                //{
                //    var subRoleList = substituteRole.SubsituteRoleList;

                //    var subList = new List<SubstituteRole>();
                //    foreach (var item in subRoleList)
                //    {
                //        var fromDate = DateTime.Parse(item.FromDate, new CultureInfo("en-US", true));
                //        var toDate = DateTime.Parse(item.ToDate, new CultureInfo("en-US", true));
                //        var subRole = new SubstituteRole();

                //        if (toDate.Month > fromDate.Month)
                //        {
                //            var firstDayOfMonth = new DateTime(fromDate.Year, fromDate.Month, 1);
                //            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);


                //            subRole = item;

                //            subRole.ToDate = lastDayOfMonth.ToShortDateString();

                //            subList.Add(subRole);

                //            var firstDayOfMonth2 = new DateTime(toDate.Year, toDate.Month, 1);

                //            item.FromDate = firstDayOfMonth2.ToShortDateString();
                //            subList.Add(item);

                //        }
                //        else
                //        {
                //            subRole = item;
                //            subList.Add(subRole);
                //        }





                //    }

                //    substituteRole.SubsituteRoleList = subList;
                //}


                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        substituteRole.SubsituteRoleList.ForEach(x =>
                        {
                            x.TotalRecord = x.CenterID == EncryptDecrypt.Encrypt64(Convert.ToString(reader["CenterID"])) ? Convert.ToInt32(reader["TotalRecord"]) : x.TotalRecord;
                            x.SortOrder = substituteRole.SortOrder;
                            x.SortColumn = substituteRole.SortColumn;
                            x.SubstituteRoleMode = substituteRole.SubstituteRoleMode;
                        });

                        //substituteRole.SubsituteRoleList.ForEach(x => {
                        //    x.TotalRecord = 48;
                        //});
                    }


                }


                if(reader.NextResult())
                {
                    while(reader.Read())
                    {
                        substituteRole.CenterList.Add(new SelectListItem
                        {
                            Text = Convert.ToString(reader["CenterName"]),
                            Value = EncryptDecrypt.Encrypt64(Convert.ToString(reader["CenterID"]))
                        });
                    }
                }

                reader.Close();
                dbManager.CloseConnection(dbConnection);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {

            }

            return substituteRole;
        }


        #endregion
    }
} 