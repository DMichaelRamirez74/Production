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
        public ReportingModel MonthlyMealReport(string AgencyID){
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

            List<ReportingModel> mealList = new List<ReportingModel>();
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                mealList.Add(new ReportingModel
                {
                    MonthName = Convert.ToString(dr["AttendanceDate"]),
                    CenterName = Convert.ToString(dr["CenterName"]),
                    MealType = Convert.ToString(dr["MealType"]),
                    MealCount = Convert.ToString(dr["mealcount"]),
                    AmealCount = Convert.ToString(dr["AMCount"])
                });
            }
           // _ReportingM.reporttype = 3;
            //_ReportingM.ColumnName = "Race";
            _ReportingM.Reportlst = mealList;
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
    }
} 