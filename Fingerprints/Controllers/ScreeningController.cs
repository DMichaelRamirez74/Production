using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsModel;
using Fingerprints.Filters;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Data;
using Fingerprints.Common;
using FingerprintsData;
using System.Threading.Tasks;
using iTextSharp.text;
using System.Text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
//using static Fingerprints.Controllers.RosterController;
using iTextSharp.tool.xml.html.table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FingerprintsModel.Enums;

namespace Fingerprints.Controllers
{
    public class ScreeningController : Controller
    {
        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         */


        StaffDetails staff = FactoryInstance.Instance.CreateInstance<StaffDetails>();

        ScreeningData screeningData = FactoryInstance.Instance.CreateInstance<ScreeningData>();
        public ActionResult AddScreening(DataTable CustomScreening)
        {
            Screening _screening = new Screening();
            try
            {

                _screening.CustomScreening = CustomScreening;
                return View(_screening);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View(_screening);
            }
        }

        public ActionResult AddLead()
        {
            return View();
        }


        [CustAuthFilter()]
        [HttpGet]
        public ActionResult ScreeningMatrix()
        {









            var report = FactoryInstance.Instance.CreateInstance<ScreeningMatrixReport>();


            report.RequestedPage = 1;
            report.PageSize = 10;
            report.SkipRows = report.GetSkipRows();
            //report.SortOrder = "ASC";
            //report.SortColumn = "Center";

            Parallel.Invoke(

            //    () =>


            //{
            //    report = GetScreeningMatrix(report);


            //},

            () =>
            {
                report.ScreeningList = screeningData.GetScreeningsByUserAccess(staff);

            }

            );


            return View(report);
        }


        public ScreeningMatrixReport GetScreeningMatrix(ScreeningMatrixReport report)
        {
            try
            {

                report = screeningData.GetScreeningMatrixReport(report, staff);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return report;
        }

        [HttpPost]
        [CustAuthFilter()]
        public PartialViewResult GetScreeningMatrixReport(ScreeningMatrixReport screeningMatrixReport)
        {


            try
            {
                long _centerId = 0;
                screeningMatrixReport.SkipRows = screeningMatrixReport.GetSkipRows();


                screeningMatrixReport.CenterID = string.Join(",", screeningMatrixReport.CenterID.Split(',').Select(x => long.TryParse(x, out _centerId) ? x : EncryptDecrypt.Decrypt64(x)).ToArray());

                screeningMatrixReport = GetScreeningMatrix(screeningMatrixReport);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


            return PartialView("~/Views/Screening/_ScreeningMatrix.cshtml", screeningMatrixReport);
        }

        //[CustAuthFilter()]
        //[HttpPost]
        //[ValidateInput(false)]
        public void ExportScreeningMatrixReport(ScreeningMatrixReport screeningMatrixReport,int reportFormatType)
        {
            try
            {


                screeningMatrixReport.RequestedPage = 0;
                screeningMatrixReport.PageSize = 0;
                screeningMatrixReport.SkipRows = screeningMatrixReport.GetSkipRows();
                screeningMatrixReport.SortColumn = "Classroom";
                screeningMatrixReport.SortOrder = "ASC";

                long _centerId = 0;
                screeningMatrixReport.SkipRows = screeningMatrixReport.GetSkipRows();
                screeningMatrixReport.CenterID = string.Join(",", screeningMatrixReport.CenterID.Split(',').Select(x => long.TryParse(x, out _centerId) ? x : EncryptDecrypt.Decrypt64(x)).ToArray());

                screeningMatrixReport = GetScreeningMatrix(screeningMatrixReport);



                #region Itextsharp PDF generation Region

string imagePath=Server.MapPath("~/Images/");



                if(EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.ReportFormatType>(reportFormatType.ToString())==FingerprintsModel.Enums.ReportFormatType.Pdf)
                {

                    MemoryStream workStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<Export>().ExportScreeningMatrixReportPdf(screeningMatrixReport, imagePath);

                    byte[] bytes = workStream.ToArray();
                    workStream.Close();
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=Screening_Matrix_Report " + DateTime.Now.ToString("MM/dd/yyyy")+".pdf");
                    Response.Buffer = true;
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bytes);
                    Response.End();
                    Response.Close();
                }

                else if(EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.ReportFormatType>(reportFormatType.ToString()) == FingerprintsModel.Enums.ReportFormatType.Xls)
                {
                  


                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Screening_Matrix_Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                    MemoryStream ms = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<Export>().ExportScreeningMatrixReportExcel(screeningMatrixReport, imagePath);
                    ms.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();

                } 





                #endregion

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

          


        }


        [HttpGet]
        [CustAuthFilter()]
        public ActionResult ScreeningReview()
        {

            NDayScreeningReviewReport modal = new NDayScreeningReviewReport();
          
            modal.ScreeningReportPeriodsList =  FactoryInstance.Instance.CreateInstance<ScreeningData>().GetScreeningReportPeriods(staff);
            return View(modal);
        }


        [HttpPost]
        [CustAuthFilter()]

        public JsonResult GetScreeningByReportPeriods(int screeningReportPeriodID)
        {
            int userscreenings = 2;
            List<SelectListItem> screeningList = screeningData.GetScreeningByReportDays(staff, screeningReportPeriodID, userscreenings);


            return Json(screeningList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustAuthFilter()]
        public PartialViewResult GetScreeningReviewReport(NDayScreeningReviewReport nDayScreeningReviewReport)
        {

            nDayScreeningReviewReport.SkipRows = nDayScreeningReviewReport.GetSkipRows();


            nDayScreeningReviewReport = screeningData.GetScreeningReviewReport(staff, nDayScreeningReviewReport);

            return PartialView("~/Views/Screening/_ScreeningReview.cshtml", nDayScreeningReviewReport);
        }



        public void ExportScreeningReviewReport(NDayScreeningReviewReport nDayScreeningReviewReport, int reportFormatType)
        {
         

            try
            {


                nDayScreeningReviewReport.RequestedPage = 0;
                nDayScreeningReviewReport.PageSize = 0;
                nDayScreeningReviewReport.SkipRows = nDayScreeningReviewReport.GetSkipRows();
                nDayScreeningReviewReport.SortColumn = "Classroom";
                nDayScreeningReviewReport.SortOrder = "ASC";
              
                nDayScreeningReviewReport = screeningData.GetScreeningReviewReport(staff,nDayScreeningReviewReport);


                #region Itextsharp PDF generation Region

                string imagePath = Server.MapPath("~/Images/");


                var reportTypeEnum = FingerprintsModel.EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.ReportFormatType>(reportFormatType.ToString());

                    MemoryStream workStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<Export>().ExportScreeningReviewReport(nDayScreeningReviewReport, reportTypeEnum,imagePath);
                 string reportName = "45-Day_Screening_Review_Report";

                 DownloadReport(workStream, reportTypeEnum, reportName);



                #endregion

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


          



        }


        public void DownloadReport(MemoryStream memoryStream, ReportFormatType reportFormat, string reportName, params object[] args)
        {
            Response.Clear();
            Response.Buffer = true;

            switch (reportFormat)
            {
                case FingerprintsModel.Enums.ReportFormatType.Pdf:

                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + reportName + "" + DateTime.Now.ToString("MM/dd/yyyy") + ".pdf");

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bytes);

                    break;

                case FingerprintsModel.Enums.ReportFormatType.Xls:

                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + reportName + "" + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");

                    memoryStream.WriteTo(Response.OutputStream);

                    break;

            }


            Response.End();
            Response.Close();
        }


    }
}
