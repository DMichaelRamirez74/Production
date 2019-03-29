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


        public List<CLASReview> getCLASReviews(GridParams gridParams, int mode, long centerid, long month,ref long TotalCount)
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
                command.Parameters.Add(new SqlParameter("@MonthId", month));


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
                                    if (listResult[j].CLASReviewAttachment == null) {
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


        public SelectListItem GetCLASAttachmentById(long AttachmentId) {
            var imageData = new SelectListItem();

            GridParams gridParams = new GridParams();
            int mode = 2; long centerid = 0; long month = 0;
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
                command.Parameters.Add(new SqlParameter("@MonthId", month));
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
            catch (Exception ex) {

                clsError.WriteException(ex);
            }



                return imageData;

        }

        #endregion CLASReview

    }
} 