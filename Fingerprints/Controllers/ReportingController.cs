using System;
using System.Web.Mvc;
using FingerprintsModel;
using FingerprintsData;
using Fingerprints.Filters;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using Fingerprints.Utilities;
using Fingerprints.Common;
using System.Text;
using System.Web;
using iTextSharp.text;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.parser;
using iTextSharp.text.pdf;
using FingerprintsModel.Enums;

namespace Fingerprints.Controllers
{
    public class ReportingController : Controller
    {
        
   
   

        [HttpGet]
        public ActionResult Reporting()
        {
            return View(new Reporting().ReturnChildList(Session["AgencyID"].ToString()));
        }



        public ActionResult MonthlyMealCount()
        {

            return View(new Reporting().MonthlyMealReport(Session["AgencyID"].ToString()));

        }

        [HttpGet]
        [Filters.CustAuthFilter()]
        public ActionResult ReportingStatus(int reporttype)
        {
            try
            {
                string userid = Session["UserID"].ToString();// "16AEBB86-AA5B-484A-A117-275024D8A172";
              
                if (reporttype == 1)
                {
                    return View(new Reporting().ReturnChildStatus(Session["AgencyID"].ToString()));
                }
                if (reporttype == 2)
                {
                    return View(new Reporting().ReturnChildInsurance(Session["AgencyID"].ToString()));
                }
                if (reporttype == 3)
                {
                    return View(new Reporting().ReturnChildRace(Session["AgencyID"].ToString()));
                }
                if (reporttype == 4)
                {
                    return View(new Reporting().ReturnChildEthnicity(Session["AgencyID"].ToString()));
                }
                if (reporttype == 5)
                {
                    return View(new Reporting().ReturnChildGender(Session["AgencyID"].ToString()));
                }
                if (reporttype == 6)
                {
                    return View(new Reporting().ReturnChildAge(Session["AgencyID"].ToString()));
                }
                if (reporttype == 7)
                {
                    return View(new Reporting().ReturnChildLanguage(Session["AgencyID"].ToString()));
                }
                else
                {
                    return View(new Reporting().ReturnChildStatus(Session["AgencyID"].ToString()));
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }
            
        }

        [HttpGet]
        public ActionResult ExportData(int exporttype)
        {
            try
            {
                return View(new Reporting().ExportData(exporttype,Session["AgencyID"].ToString()));
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return null;
            }
            
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

        [CustAuthFilter()]
        public ActionResult CLASReview()
        {

            return View();
        }

        [CustAuthFilter()]
        public ActionResult AddCLASReview()
        {

            return View();
        }

        [HttpPost]
        [CustAuthFilter()]
        //  public ActionResult AddCLASReview(CLASReview data)
        public ActionResult AddCLASReview(string modelString = "", string cameraUploads = null )
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            CLASReview data = new CLASReview();
            data = serializer.Deserialize<CLASReview>(modelString);

            var files = Request.Files;
            var fileKeys = Request.Files.AllKeys;

            data.CLASReviewAttachment = new List<Attachments>();
            //model.InkindTransactionsList[0].InkindAttachmentsList = new List<InkindAttachments>();

            for (int i = 0; i < fileKeys.Length; i++)
            {

                var fattach = new Attachments()
                {
                    AttachmentFile = Request.Files[i],
                    AttachmentFileName = Request.Files[i].FileName,
                    AttachmentFileExtension = Path.GetExtension(Request.Files[i].FileName),
                    AttachmentFileByte = new BinaryReader(Request.Files[i].InputStream).ReadBytes(Request.Files[i].ContentLength),
                    AttachmentStatus = true

                };

                data.CLASReviewAttachment.Add(fattach);

            };

            if (!string.IsNullOrEmpty(cameraUploads))
            {
                List<SelectListItem> cameraUplodList = serializer.Deserialize<List<SelectListItem>>(cameraUploads);

                foreach (var item in cameraUplodList)
                {

                    data.CLASReviewAttachment.Add(new Attachments()
                    {
                        AttachmentFileName = item.Text,
                        AttachmentFileExtension = ".png",
                        AttachmentFileByte = Convert.FromBase64String(item.Value)
                    });
                
                }
            }




            var result = new Reporting().AddCLASReview(data,1);

     
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public ActionResult getCLASReviews(GridParams gparam,long centerid=0,long seasonid = 0)
        {
            //  var sm = Request.QueryString["centerid"].ToString();
            long TotalCount = 0;
            var Data = new Reporting().getCLASReviews(gparam, 1,centerid, seasonid, ref TotalCount);

            // return Json( result, JsonRequestBehavior.AllowGet);
            return Json(new { Data, TotalCount }, JsonRequestBehavior.AllowGet);

        }

        [CustAuthFilter()]
        public ActionResult ViewCLASAttachment(long attachmentId)
        {

            var result = new Reporting().GetCLASAttachmentById(attachmentId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


       

        #endregion CLASReview


        #region MDTReport

        //[CustAuthFilter()]
        //public ActionResult MDTReport() {

        //    ViewBag.MDTId = 0;
        //   // ViewBag.MDTModal = new MDTReport();
        //    return View();
        //}

        [CustAuthFilter()]
        public ActionResult MDTList()
        {

            return View();

        }

        public ActionResult getMDTList()
        {

            var result = FactoryInstance.Instance.CreateInstance<Reporting>().GetMDTList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public ActionResult MDTReport(long id = 0)
        {

            ViewBag.MDTId = id;

            return View();
        }

        [CustAuthFilter()]
        public ActionResult GetMDTReportById(long id)
        {
            var result = FactoryInstance.Instance.CreateInstance<Reporting>().GetMDTReportById(id, "edit");

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public ActionResult getUsersDetailsForMDT(long ClientId)
        {

            var result = new Reporting().GetUsersDetailsForMDT(ClientId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public ActionResult SubmitMDT(MDTReport data)
        {
            var result = new Reporting().SubmitMDTForm(data);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustAuthFilter()]
        public ActionResult MDTUploadAttachment(long id)
        {
            bool result = false;
            try
            {
                var _files = Request.Files;

                if (_files != null && _files.Count > 0)
                {
                    result = new Reporting().SubmitMDTAttachment(id, _files[0]);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [CustAuthFilter()]
        public ActionResult MDTFormPdfView(long id = 0)
        {
            var result = FactoryInstance.Instance.CreateInstance<Reporting>().GetMDTReportById(id, "pdf");
            return View(result);
        }


        [CustAuthFilter()]
        //public FileResult GetEligibilityForm(string id = "0")
        public ActionResult GetMDTFormPDF(long id = 0,bool isdoc=false)
        {
            var fileBytes =new byte[0];

            if (isdoc) {

                var attachDetails = FactoryInstance.Instance.CreateInstance<Reporting>().GetMDTAttachmentById(id);
                fileBytes = attachDetails.AttachmentFileByte;
            }
            else
            {
                var result = FactoryInstance.Instance.CreateInstance<Reporting>().GetMDTReportById(id, "pdf");
                // Render the view xml to a string, then parse that string into an XML dom.
                string html = this.RenderActionResultToString(this.View("MDTFormPdfView", result));
                var output = new MemoryStream();
                using (var doc = new Document(PageSize.A4))
                {
                    var writer = PdfWriter.GetInstance(doc, output);

                    PDFBackgroundHelper pageEventHelper = new PDFBackgroundHelper();
                    writer.PageEvent = pageEventHelper;

                    writer.CloseStream = false;
                    doc.Open();


                    var tagProcessors = (DefaultTagProcessorFactory)Tags.GetHtmlTagProcessorFactory();
                    tagProcessors.RemoveProcessor(HTML.Tag.IMG); // remove the default processor
                    tagProcessors.AddProcessor(HTML.Tag.IMG, new CustomImageTagProcessor()); // use our new processor

                    CssFilesImpl cssFiles = new CssFilesImpl();
                    cssFiles.Add(XMLWorkerHelper.GetInstance().GetDefaultCSS());
                    var cssResolver = new StyleAttrCSSResolver(cssFiles);
                    cssResolver.AddCss(@"code { padding: 2px 4px; }", "utf-8", true);
                    var charset = Encoding.UTF8;
                    var hpc = new HtmlPipelineContext(new CssAppliersImpl(new XMLWorkerFontProvider()));
                    hpc.SetAcceptUnknown(true).AutoBookmark(true).SetTagFactory(tagProcessors); // inject the tagProcessors
                    var htmlPipeline = new HtmlPipeline(hpc, new PdfWriterPipeline(doc, writer));
                    var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                    var worker = new XMLWorker(pipeline, true);
                    var xmlParser = new XMLParser(true, worker, charset);
                    xmlParser.Parse(new StringReader(html));
                }

                output.Position = 0;
                 fileBytes = output.ToArray();
            }
            // Send the binary data to the browser.
            return new BinaryContentResult(fileBytes, "application/pdf");
            // return File(fileBytes, "application/pdf", "MDT form of" + result.Name + ".pdf");

        }

        protected string RenderActionResultToString(ActionResult result)
        {
            // Create memory writer.
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            // Create fake http context to render the view.
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request,
                fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                this.ControllerContext.RouteData,
                this.ControllerContext.Controller);
            var oldContext = System.Web.HttpContext.Current;
            System.Web.HttpContext.Current = fakeContext;

            // Render the view.
            result.ExecuteResult(fakeControllerContext);

            // Restore old context.
            System.Web.HttpContext.Current = oldContext;

            // Flush memory and return output.
            memWriter.Flush();
            return sb.ToString();
        }


        #endregion MDTReport



        #region Family Activity Report

        [CustAuthFilter()]
        [HttpGet]
        public ActionResult FamilyActivityReport()
        {

            var model = FactoryInstance.Instance.CreateInstance<FamilyActivityReport>();

            model.SearchTerm = string.Empty;
            model.SkipRows = 0;
           

            return View(model);
        }


        public PartialViewResult GetFamilyActivityReport(FamilyActivityReport familyActivityReport)
        {
            familyActivityReport = this.GetFamilyActivity(familyActivityReport);

            return PartialView("~/Views/Reporting/_FamilyActivity.cshtml", familyActivityReport);
        }

        public FamilyActivityReport GetFamilyActivity(FamilyActivityReport familyActivityReport)
        {
            familyActivityReport.SkipRows = familyActivityReport.GetSkipRows();
            familyActivityReport =FactoryInstance.Instance.CreateInstance<Reporting>().GetFamilyActivityReport(StaffDetails.GetInstance(), familyActivityReport);

            return familyActivityReport;
        }


        #region Export Family Activity Report
        public void ExportFamilyActivityReport(FamilyActivityReport familyActivityReport, int reportFormatType)
        {


            try
            {
         
                familyActivityReport.SortColumn = "Classroom";
                familyActivityReport.SortOrder = "ASC";

                familyActivityReport = this.GetFamilyActivity(familyActivityReport);


                #region Itextsharp PDF generation Region

                string imagePath = Server.MapPath("~/Images/");


                var reportTypeEnum = FingerprintsModel.EnumHelper.GetEnumByStringValue<FingerprintsModel.Enums.ReportFormatType>(reportFormatType.ToString());

                MemoryStream workStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<Export>().ExportFamilyActivityReport(familyActivityReport, reportTypeEnum, imagePath);
                string reportName = "Family_Activity_Report";

                DownloadReport(workStream, reportTypeEnum, reportName);



                #endregion

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }






        }
        #endregion

        #endregion


        #region Download Report Generic Method (PDF,EXCEL)

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
        #endregion


    }
}