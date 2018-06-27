using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using System.Threading;
using Fingerprints.ViewModel;
using System.Globalization;
using System.IO;
using System.Configuration;
using Fingerprints.CustomClasses;
using System.Text;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.end;

namespace Fingerprints.Controllers
{
    public class BillingController : Controller
    {
        public ActionResult FamilyOverride()
        {
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        public ActionResult BillingRates()
        {
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        public ActionResult GenerateInvoice()
        {
            return View();
        }
        public ActionResult AccountReceivableListing()
        {
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        public JsonResult Checkaddress(int Zipcode)
        {
            try
            {
                var Zipcodelist = new BillingData().Checkaddress(Zipcode);
                return Json(Zipcodelist);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occurred please try again.");
            }
        }
        [HttpPost]
        public JsonResult SaveFamilyOverride(FamilyOverride Family)
        {
            bool isResult = false;
            try
            {
                Family.AgencyId = Session["AgencyID"].ToString();
                Family.UserId = Session["UserID"].ToString();
                isResult = new BillingData().SaveFamilyOverride(Family);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }
        [HttpPost]
        public JsonResult SaveInvoiceDetails(InvoiceDetails invoice)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table><thead><tr><th>Client Name</th><th>Invoice Date</th><th>Amount</th><th>DueDate</th></tr></thead><tbody>");
            bool isResult = false;
            try
            {
                invoice.AgencyId = Session["AgencyID"].ToString();
                invoice.UserId = Session["UserID"].ToString();
                List<string> lstClientId = new List<string>();
                List<InvoiceDetails> listDetails = new List<InvoiceDetails>();
                new BillingData().GetChildDetailsByProgramId(ref listDetails, ref lstClientId, invoice.ProgramTypeId, Session["AgencyID"].ToString());
                isResult = new BillingData().DeleteInvoice(invoice.ProgramTypeId, invoice.Month);
                foreach (string id in lstClientId)
                {
                    string TotalEarly = "", TotalNormal = "", TotalLate = "", FTotalHours = "";
                    decimal FTotalBilling = 0;
                    DataSet ds = new DataSet();
                    new BillingData().GetClientBillingReviewByMonth(ref ds, Session["AgencyID"].ToString(), Session["UserID"].ToString(), id, invoice.Month);

                    var listHours = GetMonthSummary(ds, Convert.ToInt32(invoice.Month), ref TotalEarly, ref TotalNormal, ref TotalLate, ref FTotalHours, ref FTotalBilling);
                    // TotBil = TotBil + FTotalBilling;
                    invoice.Amount = FTotalBilling.ToString();
                    invoice.ChildId = id;
                    sb.Append("<tr><td>" + id + "</td><td>" + invoice.Invoicedate + "</td><td>" + FTotalBilling.ToString() + "</td><td>" + invoice.DueDate + "</td></tr>");
                    isResult = new BillingData().SaveInvoiceDetails(invoice);
                }
                sb.Append("</tbody></table>");
                SendMail.SendBillingEmail("Aravindh.Adhithan@gmail.com", sb.ToString(), Session["EmailID"].ToString(), "", "");
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }
        [HttpPost]
        public JsonResult SaveBillingRates(BillingRates Biling)
        {
            bool isResult = false;
            try
            {
                Biling.AgencyId = Session["AgencyID"].ToString();
                Biling.UserId = Session["UserID"].ToString();
                isResult = new BillingData().SaveBillingRates(Biling);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }

        
        [HttpPost]
        public JsonResult SaveOrganization(OrganizatonDetail organization)
        {
            bool isResult = false;
            try
            {
                organization.AgencyId = Session["AgencyID"].ToString();
                organization.UserId = Session["UserID"].ToString();
                isResult = new BillingData().SaveOrganization(organization);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }
        public string Capitalize(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        public ActionResult DownloadDocuments()
        {
            string[] arr = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            byte[] fileBytes = null;
            string FilePath = string.Empty;
            try
            {
                int Month = 0;
                string MonthName = ""; string ClientName = ""; string ClientId = "";
                if (!string.IsNullOrEmpty(Request.QueryString["Month"].ToString()))
                    Month = Convert.ToInt32(Request.QueryString["Month"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["Client"].ToString()))
                    ClientName = Request.QueryString["Client"].ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["Id"].ToString()))
                    ClientId = Request.QueryString["Id"].ToString();
                MonthName = arr[Month - 1];
                FilePath = ConvertPDF(Month.ToString(), MonthName, Capitalize(ClientName), ClientId);
                var _ext = Path.GetExtension(FilePath);
                _ext = _ext.Replace('.', ' ').Trim();
                if (FilePath.Contains("~"))
                    FilePath = FilePath.Replace('~', ' ').Trim();
                string absolute = Server.MapPath("~" + FilePath);
                //string[] filename = Path.Split('/');
                //fileBytes = System.IO.File.ReadAllBytes(absolute);
                //string[] fileName1 = filename[filename.Length - 1].Split('.'); ;
                //string[] name = fileName1[0].Split('_');
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name[0] + "." + fileName1[fileName1.Length - 1]);
                string ContentType = "";
                if (_ext.ToLower() == "pdf")
                    ContentType = "application/" + _ext;
                else
                    ContentType = "image/" + _ext;
                return File(absolute, ContentType);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
            }

        }
        public ActionResult GetTotalBillingByMonth(string Month, string ClientId)
        {
            List<ENLHours> listHours = new List<ENLHours>();
            string TotalEarly = "", TotalNormal = "", TotalLate = "", FTotalHours = "";
            decimal FTotalBilling = 0;
            try
            {
                DataSet ds = new DataSet();
                new BillingData().GetClientBillingReviewByMonth(ref ds, Session["AgencyID"].ToString(), Session["UserID"].ToString(), ClientId, Month);

                listHours = GetMonthSummary(ds, Convert.ToInt32(Month), ref TotalEarly, ref TotalNormal, ref TotalLate, ref FTotalHours, ref FTotalBilling);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(FTotalBilling);
        }
        public string ConvertPDF(string Month, string MonthName, string ClientName, string ClientId)
        {

            string Html = @"<div class='billing_title'>
                                                <h4 class='header-test'>Billing Details for <span>##MonthName##</span></h4>
                                            </div>
                                            <div class='billing_titl'>
                                                <h4 style='display:inline-block;width:auto;font-weight:normal;text-transform:capitalize!important;color: #163b69;'><strong>Client Name:</strong> ##ClientName##</h4>

                                            </div>
                                           <table class='col-md-12 table-striped table-condensed cf table-change bill-table  bill-table-ch' style='width:100%;'>
                                                <thead class='table-hd' style='width:1000px;border:1px solid red;'>
                                                    <tr style='width:100%;'>
                                                        <th style='background-color:#34495e;color:#fff;border:1px solid #fff;line-height:25px;width:100px;'>Month</th>
                                                        <th style='background-color:#2ecc71;color:#fff;border:1px solid #fff;line-height:25px;'>Early</th>
                                                        <th style='background-color:#2ecc71;color:#fff;border:1px solid #fff;line-height:25px;'>Normal</th>
                                                        <th style='background-color:#2ecc71;color:#fff;border:1px solid #fff;line-height:25px;'>Late</th>
                                                        <th style='background-color:#2ecc71;color:#fff;border:1px solid #fff;line-height:25px;'>Total Hours</th>
                                                        <th style='background-color:#34495e;color:#fff;border:1px solid #fff;line-height:25px;'>Total Billing</th>
                                                    </tr>
                                                </thead>
                                                <tbody style='width:100%;' class='bill-table-head'>";
            Html = Html.Replace("##MonthName##", MonthName.Trim());
            Html = Html.Replace("##ClientName##", ClientName.Trim());
            //StringBuilder sbTemp = new StringBuilder();

            StringBuilder sb = new StringBuilder(Html);
            List<ENLHours> listHours = new List<ENLHours>();

            DataSet ds = new DataSet();
            new BillingData().GetClientBillingReviewByMonth(ref ds, Session["AgencyID"].ToString(), Session["UserID"].ToString(), ClientId, Month);
            string TotalEarly = "", TotalNormal = "", TotalLate = "", FTotalHours = "";
            decimal FTotalBilling = 0;
            listHours = GetMonthSummary(ds, Convert.ToInt32(Month), ref TotalEarly, ref TotalNormal, ref TotalLate, ref FTotalHours, ref FTotalBilling);
            foreach (ENLHours objHours in listHours)
            {
                string template = @"<tr>
                                              <td data-title='Month'>
                                                  <div class=''>
                                                        <p>##MONTH##</p>
                                                   </div>
                                               </td>
                                               <td data-title='Early'>
                                                   <div class=''>
                                                        <p style='text-align: right;'>##EARLY##</p>
                                                    </div>
                                                </td>
                                              <td data-title='Normal'>
                                                   <div class=''>
                                                       <p style='text-align: right;'>##NORMAL##</p>
                                                    </div>
                                               </td>
                                               <td data-title='Late'>
                                                   <div class=''>
                                                        <p style='text-align: right;'>##LATE##</p>
                                                   </div>
                                                </td>
                                               <td data-title='Total Hours'>
                                                 <div class=''>
                                                       <p style='text-align: right;'>##TOTALHOURS##</p>
                                                  </div>
                                               </td>
                                              <td data-title='Total Billing'>
                                                    <div class=''>
                                                      <p style='text-align: right;'>##TOTALBILLING##</p>
                                                 </div>
                                              </td>
                                          </tr>";
                string EarlyHours = objHours.EarlyRate.Days > 0 ? objHours.EarlyRate.Days * objHours.EarlyRate.Hours + ":" + objHours.EarlyRate.Minutes.ToString("D2") : objHours.EarlyRate.Hours + ":" + objHours.EarlyRate.Minutes.ToString("D2");
                string NormalHours = objHours.NormalRate.Days > 0 ? objHours.NormalRate.Days * objHours.NormalRate.Hours + ":" + objHours.NormalRate.Minutes.ToString("D2") : objHours.NormalRate.Hours + ":" + objHours.NormalRate.Minutes.ToString("D2");
                string LateHours = objHours.LateRate.Days > 0 ? objHours.LateRate.Days * objHours.LateRate.Hours + ":" + objHours.LateRate.Minutes.ToString("D2") : objHours.LateRate.Hours + ":" + objHours.LateRate.Minutes.ToString("D2");
                string TotalHours = objHours.TotalHours.Days > 0 ? objHours.TotalHours.Days * objHours.TotalHours.Hours + ":" + objHours.TotalHours.Minutes.ToString("D2") : objHours.TotalHours.Hours + ":" + objHours.TotalHours.Minutes.ToString("D2");
                template = template.Replace("##MONTH##", objHours.Month);
                template = template.Replace("##EARLY##", EarlyHours);
                template = template.Replace("##NORMAL##", NormalHours);
                template = template.Replace("##LATE##", LateHours);
                template = template.Replace("##TOTALHOURS##", TotalHours);
                template = template.Replace("##TOTALBILLING##", "$" + objHours.TotalBiling.ToString("N2"));
                sb.Append(template);
            }
            //Html.Replace("##TEMPLATE##", sbTemp.ToString());
            string Footer = @"<tr style='background:#163b69!important;'><td data-title='Month' style='text-align: center!important;font-weight: bold!important;background:#163b69!important;color:#163b69!important;'>
                                                  <div class='' style='text-align: center!important;font-weight: bold!important;background:#163b69!important;color:#fff!important;'>
                                                        <p style='text-align: center!important;font-weight: bold!important;background:#163b69!important;color:#fff!important;'>Total</p>
                                                   </div>
                                               </td>
                                               <td data-title='Early'>
                                                   <div class=''>
                                                        <p style='text-align: right;color:#fff!important;'>##Early##</p>
                                                    </div>
                                                </td>
                                              <td data-title='Normal'>
                                                   <div class=''>
                                                       <p style='text-align: right;color:#fff!important;'>##Normal##</p>
                                                    </div>
                                               </td>
                                               <td data-title='Late'>
                                                   <div class=''>
                                                        <p style='text-align: right;color:#fff!important;'>##Late##</p>
                                                   </div>
                                                </td>
                                               <td data-title='Total Hours'>
                                                 <div class=''>
                                                       <p style='text-align: right;color:#fff!important;'>##TotalHours##</p>
                                                  </div>
                                               </td>
                                              <td data-title='Total Billing'>
                                                    <div class=''>
                                                      <p style='text-align: right;color:#fff!important;'>$##TotalBilling##</p>
                                                 </div>
                                              </td>
                                          </tr>
                                          </tbody></table>";
            Footer = Footer.Replace("##Early##", TotalEarly);
            Footer = Footer.Replace("##Late##", TotalLate);
            Footer = Footer.Replace("##Normal##", TotalNormal);
            Footer = Footer.Replace("##TotalBilling##", FTotalBilling.ToString());
            Footer = Footer.Replace("##TotalHours##", FTotalHours);
            sb.Append(Footer);
            string Test1 = sb.ToString();
            Guid id = Guid.NewGuid();
            string relativePath = "~/Content/PDF/Billing_" + id.ToString() + ".pdf";
            string Path = Server.MapPath(relativePath);
            System.IO.File.WriteAllBytes(Path, ConvertPDF(sb.ToString()));
            return relativePath;
        }
        public ActionResult GeneratePDF(string ClientId, string Month, string MonthName, string ClientName, string Early, string Normal, string Late, string TotalHours, string TotalBilling, string EmailId)
        {
            bool isSend = true;
            string Html = @"<div class='billing_title'>
                                                <h4 class='header-test'>Billing Details for <span>##MonthName##</span></h4>
                                            </div>
                                            <div class='billing_titl'>
                                                <h4 style='display:inline-block;width:auto;font-weight:normal;text-transform:capitalize!important;color: #163b69;'><strong>Client Name:</strong> ##ClientName##</h4>

                                            </div>
                                           <table class='col-md-12 table-striped table-condensed cf table-change bill-table  bill-table-ch' style='width:100%;'>
                                                <thead class='table-hd' style='width:1000px;border:1px solid red;'>
                                                    <tr style='width:100%;'>
                                                        <th style='background-color:#34495e;color:#fff;border:1px solid #fff;line-height:25px;width:100px;'>Month</th>
                                                        <th style='background-color:#2ecc71;color:#fff;border:1px solid #fff;line-height:25px;'>Early</th>
                                                        <th style='background-color:#2ecc71;color:#fff;border:1px solid #fff;line-height:25px;'>Normal</th>
                                                        <th style='background-color:#2ecc71;color:#fff;border:1px solid #fff;line-height:25px;'>Late</th>
                                                        <th style='background-color:#2ecc71;color:#fff;border:1px solid #fff;line-height:25px;'>Total Hours</th>
                                                        <th style='background-color:#34495e;color:#fff;border:1px solid #fff;line-height:25px;'>Total Billing</th>
                                                    </tr>
                                                </thead>
                                                <tbody style='width:100%;' class='bill-table-head'>";
            Html = Html.Replace("##MonthName##", MonthName.Trim());
            Html = Html.Replace("##ClientName##", Capitalize(ClientName.Trim()));
            //StringBuilder sbTemp = new StringBuilder();

            StringBuilder sb = new StringBuilder(Html);
            List<ENLHours> listHours = new List<ENLHours>();
            try
            {
                DataSet ds = new DataSet();
                new BillingData().GetClientBillingReviewByMonth(ref ds, Session["AgencyID"].ToString(), Session["UserID"].ToString(), ClientId, Month);
                string TotalEarly = "", TotalNormal = "", TotalLate = "", FTotalHours = "";
                decimal FTotalBilling = 0;
                listHours = GetMonthSummary(ds, Convert.ToInt32(Month), ref TotalEarly, ref TotalNormal, ref TotalLate, ref FTotalHours, ref FTotalBilling);
                foreach (ENLHours objHours in listHours)
                {
                    string template = @"<tr>
                                              <td data-title='Month'>
                                                  <div class=''>
                                                        <p>##MONTH##</p>
                                                   </div>
                                               </td>
                                               <td data-title='Early'>
                                                   <div class=''>
                                                        <p style='text-align: right;'>##EARLY##</p>
                                                    </div>
                                                </td>
                                              <td data-title='Normal'>
                                                   <div class=''>
                                                       <p style='text-align: right;'>##NORMAL##</p>
                                                    </div>
                                               </td>
                                               <td data-title='Late'>
                                                   <div class=''>
                                                        <p style='text-align: right;'>##LATE##</p>
                                                   </div>
                                                </td>
                                               <td data-title='Total Hours'>
                                                 <div class=''>
                                                       <p style='text-align: right;'>##TOTALHOURS##</p>
                                                  </div>
                                               </td>
                                              <td data-title='Total Billing'>
                                                    <div class=''>
                                                      <p style='text-align: right;'>##TOTALBILLING##</p>
                                                 </div>
                                              </td>
                                          </tr>";
                    string EarlyHours = objHours.EarlyRate.Days > 0 ? objHours.EarlyRate.Days * objHours.EarlyRate.Hours + ":" + objHours.EarlyRate.Minutes.ToString("D2") : objHours.EarlyRate.Hours + ":" + objHours.EarlyRate.Minutes.ToString("D2");
                    string NormalHours = objHours.NormalRate.Days > 0 ? objHours.NormalRate.Days * objHours.NormalRate.Hours + ":" + objHours.NormalRate.Minutes.ToString("D2") : objHours.NormalRate.Hours + ":" + objHours.NormalRate.Minutes.ToString("D2");
                    string LateHours = objHours.LateRate.Days > 0 ? objHours.LateRate.Days * objHours.LateRate.Hours + ":" + objHours.LateRate.Minutes.ToString("D2") : objHours.LateRate.Hours + ":" + objHours.LateRate.Minutes.ToString("D2");
                    string TotalHour = objHours.TotalHours.Days > 0 ? objHours.TotalHours.Days * objHours.TotalHours.Hours + ":" + objHours.TotalHours.Minutes.ToString("D2") : objHours.TotalHours.Hours + ":" + objHours.TotalHours.Minutes.ToString("D2");
                    template = template.Replace("##MONTH##", objHours.Month);
                    template = template.Replace("##EARLY##", EarlyHours);
                    template = template.Replace("##NORMAL##", NormalHours);
                    template = template.Replace("##LATE##", LateHours);
                    template = template.Replace("##TOTALHOURS##", TotalHour);
                    template = template.Replace("##TOTALBILLING##", "$" + objHours.TotalBiling.ToString("N2"));
                    sb.Append(template);
                }
                //Html.Replace("##TEMPLATE##", sbTemp.ToString());
                string Footer = @"<tr style='background:#163b69!important;'><td data-title='Month' style='text-align: center!important;font-weight: bold!important;background:#163b69!important;color:#163b69!important;'>
                                                  <div class='' style='text-align: center!important;font-weight: bold!important;background:#163b69!important;color:#fff!important;'>
                                                        <p style='text-align: center!important;font-weight: bold!important;background:#163b69!important;color:#fff!important;'>Total</p>
                                                   </div>
                                               </td>
                                               <td data-title='Early'>
                                                   <div class=''>
                                                        <p style='text-align: right;color:#fff!important;'>##Early##</p>
                                                    </div>
                                                </td>
                                              <td data-title='Normal'>
                                                   <div class=''>
                                                       <p style='text-align: right;color:#fff!important;'>##Normal##</p>
                                                    </div>
                                               </td>
                                               <td data-title='Late'>
                                                   <div class=''>
                                                        <p style='text-align: right;color:#fff!important;'>##Late##</p>
                                                   </div>
                                                </td>
                                               <td data-title='Total Hours'>
                                                 <div class=''>
                                                       <p style='text-align: right;color:#fff!important;'>##TotalHours##</p>
                                                  </div>
                                               </td>
                                              <td data-title='Total Billing'>
                                                    <div class=''>
                                                      <p style='text-align: right;color:#fff!important;'>$##TotalBilling##</p>
                                                 </div>
                                              </td>
                                          </tr>
                                          </tbody></table>";
                Footer = Footer.Replace("##Early##", TotalEarly);
                Footer = Footer.Replace("##Late##", TotalLate);
                Footer = Footer.Replace("##Normal##", TotalNormal);
                Footer = Footer.Replace("##TotalBilling##", FTotalBilling.ToString());
                Footer = Footer.Replace("##TotalHours##", FTotalHours);
                sb.Append(Footer);
                string Test1 = sb.ToString();
                Guid id = Guid.NewGuid();
                string relativePath = "~/Content/PDF/Billing_" + id.ToString() + ".pdf";
                string Path = Server.MapPath(relativePath);
                System.IO.File.WriteAllBytes(Path, ConvertPDF(sb.ToString()));
                isSend = SendMail.SendBillingEmail(EmailId, "Billing Review", Session["EmailID"].ToString(), Path, "");
                //string ChildId = "0";
                if (isSend)
                {
                    isSend = new BillingData().BillingAttachments(Session["AgencyID"].ToString(), ClientId, relativePath, Session["UserID"].ToString());
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isSend);

        }
        public byte[] ConvertPDF(string html)
        {



            byte[] bytesArray = null;
            // iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(bytes);
            // image.ScaleAbsolute(159f, 159f);
            // image.ScalePercent(24f);
            using (var ms = new MemoryStream())
            {
                using (var document = new Document())
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(document, ms))
                    {
                        document.Open();
                        using (var strReader = new StringReader(html))
                        {
                            //Set factories
                            HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                            htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                            //Set css
                            ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                            cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/css/bootstrap.min.css"), true);
                            cssResolver.AddCssFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/css/BillingPrint.css"), true);
                            //Export
                            IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(document, writer)));
                            var worker = new XMLWorker(pipeline, true);
                            var xmlParse = new XMLParser(true, worker);
                            xmlParse.Parse(strReader);
                            xmlParse.Flush();
                            //image.ScaleAbsoluteWidth(document.PageSize.Width - 20);
                            //image.SetAbsolutePosition(0, image.AbsoluteY);

                            //document.Add(image);
                        }
                        document.Close();
                    }
                }
                bytesArray = ms.ToArray();
            }

            return bytesArray;
            // return File(bytesArray, "application/pdf");

        }
        [HttpPost]
        public JsonResult DeleteFamilyOverride(string Id)
        {
            bool isResult = false;
            try
            {

                isResult = new BillingData().DeleteFamilyOverride(Id);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }
        [HttpPost]
        public JsonResult DeleteInvoiceDetails(string ddlProgramType, string Month)
        {
            bool isResult = false;
            try
            {

                isResult = new BillingData().DeleteInvoice(ddlProgramType, Month);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }
        [HttpPost]
        public JsonResult DeleteBillingRates(string Id)
        {
            bool isResult = false;
            try
            {

                isResult = new BillingData().DeleteBillingRates(Id);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }


      
        public ActionResult AgencyBilling()
        {
            return View();
        }
        public ActionResult GetParentByProgramId(string ProgramId, string CenterId, string SearchText, string FamilyId)
        {
            List<SelectListItem> lstItems = new List<SelectListItem>();
            try
            {
                // if (string.IsNullOrEmpty(FamilyId))
                new BillingData().GetParentDetails(ref lstItems, Session["AgencyID"].ToString(), ProgramId, CenterId, SearchText);
                //else
                //    new BillingData().GetClientByFamilyId(ref lstItems, FamilyId, Session["AgencyID"].ToString(), ProgramId, CenterId, SearchText);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(lstItems);
        }
        public ActionResult GetOrganization(string ProgramId, string CenterId, string SearchText, string FamilyId)
        {
            List<SelectListItem> lstItems = new List<SelectListItem>();
            List<string> lstClientId = new List<string>();
            try
            {
                TempData["lstClientId"] = null;
                // if (string.IsNullOrEmpty(FamilyId))
                new BillingData().GetOrganization(ref lstItems, ref lstClientId, Session["AgencyID"].ToString(), ProgramId, CenterId, SearchText);
                //else
                //    new BillingData().GetClientByFamilyId(ref lstItems, FamilyId, Session["AgencyID"].ToString(), ProgramId, CenterId, SearchText);
                TempData["lstClientId"] = lstClientId;
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(new { lstItems, lstClientId });
        }
        public ActionResult GetBillingAmount(string Programtype, string Month)
        {
            List<ENLHours> listHours = new List<ENLHours>();
            List<InvoiceDetails> listDetails = new List<InvoiceDetails>();
            decimal TotBil = 0;

            try
            {
                List<string> lstClientId = new List<string>();
                new BillingData().GetChildDetailsByProgramId(ref listDetails, ref lstClientId, Programtype, Session["AgencyID"].ToString());
                if (lstClientId != null && lstClientId.Count() > 0)
                {
                    foreach (string id in lstClientId)
                    {

                        string TotalEarly = "", TotalNormal = "", TotalLate = "", FTotalHours = "";
                        decimal FTotalBilling = 0;
                        DataSet ds = new DataSet();
                        new BillingData().GetClientBillingReviewByMonth(ref ds, Session["AgencyID"].ToString(), Session["UserID"].ToString(), id, Month);

                        listHours = GetMonthSummary(ds, Convert.ToInt32(Month), ref TotalEarly, ref TotalNormal, ref TotalLate, ref FTotalHours, ref FTotalBilling);
                        TotBil = TotBil + FTotalBilling;
                        foreach (InvoiceDetails invoice in listDetails)
                        {
                            if (invoice.ChildId == id)
                            {
                                invoice.Amount = FTotalBilling.ToString();
                            }
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(new { TotBil, listDetails });
        }
        public ActionResult GetChildByFamilyId(string ProgramId, string CenterId, string FamilyId)
        {
            List<SelectListItem> lstItems = new List<SelectListItem>();
            try
            {
                new BillingData().GetClientByFamilyId(ref lstItems, FamilyId, Session["AgencyID"].ToString(), ProgramId, CenterId);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(lstItems);
        }        
        public ActionResult GetFamilyOverrideByUserId()
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtDevelopmentTeamDetails = new DataTable();
                new BillingData().GetFamilyOverrideByUserId(ref dtDevelopmentTeamDetails, Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtDevelopmentTeamDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public ActionResult GetInvoiceDetalsByUserId()
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtInvoice = new DataTable();
                new BillingData().GetInvoiceDetailsByUserId(ref dtInvoice, Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtInvoice);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public ActionResult GetAccountReceivableByUserId()
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtInvoice = new DataTable();
                new BillingData().GetAccountReceivableListingsByUserId(ref dtInvoice, Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtInvoice);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public ActionResult GetInvoiceDetailByUserId()
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtInvoice = new DataTable();
                new BillingData().GetInvoiceDetailByUserId(ref dtInvoice, Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtInvoice);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public class ZipDetails
        {
            public string country { get; set; }
            public string state { get; set; }
            public string city { get; set; }
        }
        public ActionResult GetCityStateByZipcode(string ZipCode)
        {
            ZipDetails zipDetails = new ZipDetails();
            try
            {
                Task<ZipDetails> tset = GetAddress(ZipCode);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(zipDetails);
        }
        public async Task<ZipDetails> GetAddress(string ZipCode)
        {
            ZipDetails zipDetails = new ZipDetails();
            try
            {
                string baseaddress = "http://ziptasticapi.com/" + ZipCode;
                HttpClient httpClient = new HttpClient();
                var result = await httpClient.GetAsync(new Uri(baseaddress));
                string resultString = await result.Content.ReadAsStringAsync();
                zipDetails = JsonConvert.DeserializeObject<ZipDetails>(resultString);
                if (zipDetails.city != null)
                {
                    if (zipDetails.country == "US")
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            return zipDetails;
        }
        public ActionResult GetOrganizationByProgramId(string ProgramId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtDevelopmentTeamDetails = new DataTable();
                new BillingData().GeOrganizationByProgramId(ref dtDevelopmentTeamDetails, Session["UserID"].ToString(), Session["AgencyID"].ToString(), ProgramId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtDevelopmentTeamDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public ActionResult GetRatesByProgramId(string ProgramId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtDevelopmentTeamDetails = new DataTable();
                new BillingData().GetRatesByProgramId(ref dtDevelopmentTeamDetails, Session["UserID"].ToString(), Session["AgencyID"].ToString(), ProgramId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtDevelopmentTeamDetails);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public ActionResult GetBillingRatesByUserId()
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtBilling = new DataTable();
                new BillingData().GetBillingRatesByUserId(ref dtBilling, Session["UserID"].ToString(), Session["AgencyID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtBilling);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
       

        [HttpGet]
        public ActionResult BillingReview(string test = "0")
        {
            decimal a = 1;
            string Test = Math.Round(a, 2).ToString();
            List<BillingReview> list = new List<FingerprintsModel.BillingReview>();
            try
            {
                DataSet ds = new DataSet();
                new BillingData().GetClientBillingReview(ref ds, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                list = SummaryHours(ds);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult GetClientBillingReviewByMonth(string ClientId, string Month)
        {
            List<ENLHours> listHours = new List<ENLHours>();
            string TotalEarly = "", TotalNormal = "", TotalLate = "", FTotalHours = "";
            decimal FTotalBilling = 0;
            try
            {
                DataSet ds = new DataSet();
                new BillingData().GetClientBillingReviewByMonth(ref ds, Session["AgencyID"].ToString(), Session["UserID"].ToString(), ClientId, Month);

                listHours = GetMonthSummary(ds, Convert.ToInt32(Month), ref TotalEarly, ref TotalNormal, ref TotalLate, ref FTotalHours, ref FTotalBilling);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(new { listHours, TotalEarly, TotalNormal, TotalLate, FTotalHours, FTotalBilling });
        }
        public static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }
        public List<ENLHours> GetMonthSummary(DataSet ds, int Month, ref string TotalEarly, ref string TotalNormal, ref string TotalLate, ref string FTotalHours, ref decimal FTotalBilling)
        {
            List<ENLHours> listHours = new List<ENLHours>();
            TimeSpan FEarly = TimeSpan.Zero; TimeSpan FNormal = TimeSpan.Zero; TimeSpan FLate = TimeSpan.Zero;
            TimeSpan FTHours = TimeSpan.Zero;
            try
            {
                FamilyRate objFamilyRate = new FamilyRate();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            objFamilyRate.ClientId = dr["ChildId"].ToString();
                            objFamilyRate.OverrideEarlyRate = !string.IsNullOrEmpty(dr["OverrideEarlyRate"].ToString()) ? Convert.ToDecimal(dr["OverrideEarlyRate"].ToString()) : 0;
                            objFamilyRate.OverrideNormalRate = !string.IsNullOrEmpty(dr["OverrideNormalRate"].ToString()) ? Convert.ToDecimal(dr["OverrideNormalRate"].ToString()) : 0;
                            objFamilyRate.OverrideLateRate = !string.IsNullOrEmpty(dr["OverrideLateRate"].ToString()) ? Convert.ToDecimal(dr["OverrideLateRate"].ToString()) : 0;
                            objFamilyRate.FixedAmount = !string.IsNullOrEmpty(dr["FixedAmount"].ToString()) ? Convert.ToDecimal(dr["FixedAmount"].ToString()) : 0;
                            objFamilyRate.EarlyorLateTimes = !string.IsNullOrEmpty(dr["Times"].ToString()) ? Convert.ToDouble(dr["Times"].ToString()) : 0;
                            objFamilyRate.NeverLessThan = !string.IsNullOrEmpty(dr["NeverLessThan"].ToString()) ? Convert.ToDecimal(dr["NeverLessThan"].ToString()) : 0;
                            objFamilyRate.NeverMoreThan = !string.IsNullOrEmpty(dr["NeverMoreThan"].ToString()) ? Convert.ToDecimal(dr["NeverMoreThan"].ToString()) : 0;
                        }
                    }
                    List<DateTime> datesOfMonth = GetDates(DateTime.Now.Year, Month);
                    foreach (DateTime objdate in datesOfMonth)
                    {
                        ENLHours hours = new ENLHours();
                        hours.Month = objdate.ToString("MMM") + " - " + objdate.Day.ToString("D2");
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                DateTime dt = Convert.ToDateTime(dr["AttendanceDate"].ToString());
                                hours.ClientName = dr["Name"].ToString();
                                if (objdate == dt)
                                {
                                    hours.ClientId = dr["ClientID"].ToString();
                                    hours.CenterId = dr["CenterID"].ToString();
                                    hours.ProgramTypeId = dr["ProgramTypeId"].ToString();
                                    hours.AttendanceDate = dt;
                                    string[] ClassStartTime = dr["StartTime"].ToString().Split('.')[0].Split(':');
                                    TimeSpan tsClassStartTime = new TimeSpan(Convert.ToInt32(ClassStartTime[0]), Convert.ToInt32(ClassStartTime[1]), Convert.ToInt32(ClassStartTime[2]));
                                    string[] ClassEndTime = dr["EndTime"].ToString().Split('.')[0].Split(':');
                                    TimeSpan tsClassEndTime = new TimeSpan(Convert.ToInt32(ClassEndTime[0]), Convert.ToInt32(ClassEndTime[1]), Convert.ToInt32(ClassEndTime[2]));
                                    string[] TimeIn = dr["TimeIn"].ToString().Split('.')[0].Split(':');
                                    TimeSpan tsTimeIn = new TimeSpan(Convert.ToInt32(TimeIn[0]), Convert.ToInt32(TimeIn[1]), Convert.ToInt32(TimeIn[2]));
                                    string[] TimeOut = dr["TimeOut"].ToString().Split('.')[0].Split(':');
                                    TimeSpan tsTimeOut = new TimeSpan(Convert.ToInt32(TimeOut[0]), Convert.ToInt32(TimeOut[1]), Convert.ToInt32(TimeOut[2]));
                                    if (tsClassStartTime > tsTimeIn)
                                        hours.EarlyRate = tsClassStartTime - tsTimeIn;
                                    if (tsClassEndTime > tsClassStartTime)
                                        hours.NormalRate = tsClassEndTime - tsClassStartTime;
                                    if (tsTimeOut > tsClassEndTime)
                                        hours.LateRate = tsTimeOut - tsClassEndTime;
                                    hours.TotalHours = hours.TotalHours.Add(hours.EarlyRate).Add(hours.NormalRate).Add(hours.LateRate);
                                    string[] earlyHours = hours.EarlyRate.ToString().Split(':');
                                    decimal totEarlyHours = Convert.ToDecimal(earlyHours[0] + "." + earlyHours[1]);
                                    string[] normalHours = hours.NormalRate.ToString().Split(':');
                                    decimal totNormalHours = Convert.ToDecimal(normalHours[0] + "." + normalHours[1]);
                                    string[] lateHours = hours.LateRate.ToString().Split(':');
                                    decimal totLateHours = Convert.ToDecimal(lateHours[0] + "." + lateHours[1]);
                                    if (objFamilyRate.FixedAmount != 0)
                                        hours.TotalBiling = objFamilyRate.FixedAmount;
                                    // hours.TotalBiling = (totEarlyHours * objFamilyRate.FixedAmount) + (totNormalHours * objFamilyRate.FixedAmount) + (totLateHours * objFamilyRate.FixedAmount);
                                    else
                                    {
                                        decimal earlyHour = 0, lateHour = 0;
                                        double TotalEarlyMin = hours.EarlyRate.TotalMinutes;
                                        double TotalLateMin = hours.LateRate.TotalMinutes;
                                        if (objFamilyRate.EarlyorLateTimes < TotalEarlyMin)
                                            earlyHour = (totEarlyHours * objFamilyRate.OverrideEarlyRate);
                                        if (objFamilyRate.EarlyorLateTimes < TotalLateMin)
                                            lateHour = (totLateHours * objFamilyRate.OverrideLateRate);
                                        hours.TotalBiling = earlyHour + (totNormalHours * objFamilyRate.OverrideNormalRate) + lateHour;
                                    }
                                }
                            }
                        }

                        hours.TotalBiling = Math.Round(hours.TotalBiling, 2);

                        //Total Calculation
                        FEarly = FEarly.Add(hours.EarlyRate);
                        FNormal = FNormal.Add(hours.NormalRate);
                        FLate = FLate.Add(hours.LateRate);
                        FTHours = FTHours.Add(hours.TotalHours);
                        FTotalBilling = FTotalBilling + hours.TotalBiling;
                        listHours.Add(hours);
                    }
                    TotalEarly = FEarly.Days > 0 ? FEarly.Days * FEarly.Hours + ":" + FEarly.Minutes.ToString("D2") : FEarly.Hours + ":" + FEarly.Minutes.ToString("D2");
                    TotalNormal = FNormal.Days > 0 ? FNormal.Days * FNormal.Hours + ":" + FNormal.Minutes.ToString("D2") : FNormal.Hours + ":" + FNormal.Minutes.ToString("D2");
                    TotalLate = FLate.Days > 0 ? FLate.Days * FLate.Hours + ":" + FLate.Minutes.ToString("D2") : FLate.Hours + ":" + FLate.Minutes.ToString("D2");
                    FTotalHours = FTHours.Days > 0 ? FTHours.Days * FTHours.Hours + ":" + FTHours.Minutes.ToString("D2") : FTHours.Hours + ":" + FTHours.Minutes.ToString("D2");
                    if (objFamilyRate.NeverLessThan != 0)
                    {
                        if (FTotalBilling < objFamilyRate.NeverLessThan)
                        {
                            FTotalBilling = objFamilyRate.NeverLessThan;
                        }
                    }
                    if (objFamilyRate.NeverMoreThan != 0)
                    {
                        if (FTotalBilling > objFamilyRate.NeverMoreThan)
                        {
                            FTotalBilling = objFamilyRate.NeverMoreThan;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);

            }
            return listHours;

        }
        public List<BillingReview> SummaryHours(DataSet ds)
        {
            List<BillingReview> listReview = new List<FingerprintsModel.BillingReview>();
            try
            {
                List<ENLHours> listHours = new List<ENLHours>();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BillingReview billing = new BillingReview();
                            billing.ClientId = dr["ClientID"].ToString();
                            billing.ClientName = dr["Name"].ToString();
                            billing.EmailId = dr["EmailId"].ToString();
                            billing.CenterId = dr["CenterID"].ToString();
                            billing.ProgramTypeId = dr["ProgramTypeId"].ToString();
                            billing.Email = Convert.ToBoolean(dr["Email"]);
                            billing.Print = Convert.ToBoolean(dr["Print"]);
                            listReview.Add(billing);
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            ENLHours hours = new ENLHours();
                            hours.ClientId = dr["ClientID"].ToString();
                            hours.ClientName = dr["Name"].ToString();
                            hours.CenterId = dr["CenterID"].ToString();
                            hours.ProgramTypeId = dr["ProgramTypeId"].ToString();
                            DateTime dt = Convert.ToDateTime(dr["AttendanceDate"].ToString());
                            hours.AttendanceDate = dt;
                            string[] ClassStartTime = dr["StartTime"].ToString().Split('.')[0].Split(':');
                            TimeSpan tsClassStartTime = new TimeSpan(Convert.ToInt32(ClassStartTime[0]), Convert.ToInt32(ClassStartTime[1]), Convert.ToInt32(ClassStartTime[2]));
                            string[] ClassEndTime = dr["EndTime"].ToString().Split('.')[0].Split(':');
                            TimeSpan tsClassEndTime = new TimeSpan(Convert.ToInt32(ClassEndTime[0]), Convert.ToInt32(ClassEndTime[1]), Convert.ToInt32(ClassEndTime[2]));
                            string[] TimeIn = dr["TimeIn"].ToString().Split('.')[0].Split(':');
                            TimeSpan tsTimeIn = new TimeSpan(Convert.ToInt32(TimeIn[0]), Convert.ToInt32(TimeIn[1]), Convert.ToInt32(TimeIn[2]));
                            string[] TimeOut = dr["TimeOut"].ToString().Split('.')[0].Split(':');
                            TimeSpan tsTimeOut = new TimeSpan(Convert.ToInt32(TimeOut[0]), Convert.ToInt32(TimeOut[1]), Convert.ToInt32(TimeOut[2]));
                            if (tsClassStartTime > tsTimeIn)
                                hours.EarlyRate = tsClassStartTime - tsTimeIn;
                            if (tsClassEndTime > tsClassStartTime)
                                hours.NormalRate = tsClassEndTime - tsClassStartTime;
                            if (tsTimeOut > tsClassEndTime)
                                hours.LateRate = tsTimeOut - tsClassEndTime;
                            listHours.Add(hours);
                        }
                        if (listHours.Count() > 0)
                        {
                            foreach (BillingReview objReview in listReview)
                            {
                                TimeSpan janHours = TimeSpan.Zero;
                                TimeSpan febHours = TimeSpan.Zero;
                                TimeSpan marHours = TimeSpan.Zero;
                                TimeSpan aprHours = TimeSpan.Zero;
                                TimeSpan mayHours = TimeSpan.Zero;
                                TimeSpan junHours = TimeSpan.Zero;
                                TimeSpan julHours = TimeSpan.Zero;
                                TimeSpan augHours = TimeSpan.Zero;
                                TimeSpan sepHours = TimeSpan.Zero;
                                TimeSpan octHours = TimeSpan.Zero;
                                TimeSpan novHours = TimeSpan.Zero;
                                TimeSpan decHours = TimeSpan.Zero;
                                foreach (ENLHours objHours in listHours)
                                {

                                    if (objReview.ClientId == objHours.ClientId)
                                    {
                                        if (objHours.AttendanceDate.Month == 1)
                                            janHours = janHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 2)
                                            febHours = febHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 3)
                                            marHours = marHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 4)
                                            aprHours = aprHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 5)
                                            mayHours = mayHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 6)
                                            junHours = junHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 7)
                                            julHours = julHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 8)
                                            augHours = augHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 9)
                                            sepHours = sepHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 10)
                                            octHours = octHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 11)
                                            novHours = novHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                        if (objHours.AttendanceDate.Month == 12)
                                            decHours = decHours.Add(objHours.EarlyRate).Add(objHours.NormalRate).Add(objHours.LateRate);
                                    }
                                }

                                objReview.Jan = janHours.Days > 0 ? janHours.Days * janHours.Hours + ":" + janHours.Minutes.ToString("D2") : janHours.Hours + ":" + janHours.Minutes.ToString("D2");
                                objReview.Feb = febHours.Days > 0 ? febHours.Days * febHours.Hours + ":" + febHours.Minutes.ToString("D2") : febHours.Hours + ":" + febHours.Minutes.ToString("D2");
                                objReview.Mar = marHours.Days > 0 ? marHours.Days * marHours.Hours + ":" + marHours.Minutes.ToString("D2") : marHours.Hours + ":" + marHours.Minutes.ToString("D2");
                                objReview.Apr = aprHours.Days > 0 ? aprHours.Days * aprHours.Hours + ":" + aprHours.Minutes.ToString("D2") : aprHours.Hours + ":" + aprHours.Minutes.ToString("D2");
                                objReview.May = mayHours.Days > 0 ? mayHours.Days * mayHours.Hours + ":" + mayHours.Minutes.ToString("D2") : mayHours.Hours + ":" + mayHours.Minutes.ToString("D2");
                                objReview.Jun = junHours.Days > 0 ? junHours.Days * junHours.Hours + ":" + junHours.Minutes.ToString("D2") : junHours.Hours + ":" + junHours.Minutes.ToString("D2");
                                objReview.Jul = julHours.Days > 0 ? julHours.Days * julHours.Hours + ":" + julHours.Minutes.ToString("D2") : julHours.Hours + ":" + julHours.Minutes.ToString("D2");
                                objReview.Aug = augHours.Days > 0 ? augHours.Days * augHours.Hours + ":" + augHours.Minutes.ToString("D2") : augHours.Hours + ":" + augHours.Minutes.ToString("D2");
                                objReview.Sep = sepHours.Days > 0 ? sepHours.Days * sepHours.Hours + ":" + sepHours.Minutes.ToString("D2") : sepHours.Hours + ":" + sepHours.Minutes.ToString("D2");
                                objReview.Oct = octHours.Days > 0 ? octHours.Days * octHours.Hours + ":" + octHours.Minutes.ToString("D2") : octHours.Hours + ":" + octHours.Minutes.ToString("D2");
                                objReview.Nov = novHours.Days > 0 ? novHours.Days * novHours.Hours + ":" + novHours.Minutes.ToString("D2") : novHours.Hours + ":" + novHours.Minutes.ToString("D2");
                                objReview.Dec = decHours.Days > 0 ? decHours.Days * decHours.Hours + ":" + decHours.Minutes.ToString("D2") : decHours.Hours + ":" + decHours.Minutes.ToString("D2");

                                //objReview.Jan = janHours.ToString().Split(':')[0] + "." + janHours.ToString().Split(':')[1];
                                //objReview.Feb = febHours.ToString().Split(':')[0] + "." + febHours.ToString().Split(':')[1];
                                //objReview.Mar = marHours.ToString().Split(':')[0] + "." + marHours.ToString().Split(':')[1];
                                //objReview.Apr = aprHours.ToString().Split(':')[0] + "." + aprHours.ToString().Split(':')[1];
                                //objReview.May = mayHours.ToString().Split(':')[0] + "." + mayHours.ToString().Split(':')[1];
                                //objReview.Jun = junHours.ToString().Split(':')[0] + "." + junHours.ToString().Split(':')[1];
                                //objReview.Jul = julHours.ToString().Split(':')[0] + "." + julHours.ToString().Split(':')[1];
                                //objReview.Aug = augHours.ToString().Split(':')[0] + "." + augHours.ToString().Split(':')[1];
                                //objReview.Sep = sepHours.ToString().Split(':')[0] + "." + sepHours.ToString().Split(':')[1];
                                //objReview.Oct = octHours.ToString().Split(':')[0] + "." + octHours.ToString().Split(':')[1];
                                //objReview.Nov = novHours.ToString().Split(':')[0] + "." + novHours.ToString().Split(':')[1];
                                //objReview.Dec = decHours.ToString().Split(':')[0] + "." + decHours.ToString().Split(':')[1];

                                //objReview.Jan = janHours.ToString();
                                //objReview.Feb = febHours.ToString();
                                //objReview.Mar = marHours.ToString();
                                //objReview.Apr = aprHours.ToString();
                                //objReview.May = mayHours.ToString();
                                //objReview.Jun = junHours.ToString();
                                //objReview.Jul = julHours.ToString();
                                //objReview.Aug = augHours.ToString();
                                //objReview.Sep = sepHours.ToString();
                                //objReview.Oct = octHours.ToString();
                                //objReview.Nov = novHours.ToString();
                                //objReview.Dec = decHours.ToString();
                            }

                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);

            }
            return listReview;
        }

    }
}
