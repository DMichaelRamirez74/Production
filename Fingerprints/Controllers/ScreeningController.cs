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
using static Fingerprints.Controllers.RosterController;
using iTextSharp.tool.xml.html.table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                report.ScreeningList = FactoryInstance.Instance.CreateInstance<ScreeningData>().GetScreeningsByUserAccess(staff);

            }

            );


            return View(report);
        }


        public ScreeningMatrixReport GetScreeningMatrix(ScreeningMatrixReport report)
        {
            try
            {

                report = FactoryInstance.Instance.CreateInstance<ScreeningData>().GetScreeningMatrixReport(report, staff);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return report;
        }


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
        public void GenerateScreeningMatrixReport(ScreeningMatrixReport screeningMatrixReport)
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





                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");

                Document doc = new Document();
           
                doc.SetMargins(50f, 50f, 50f, 50f);
              
                //Create PDF Table with 6 columns  
                Int32 colCount = 6;
           
                PdfPTable tableLayout = new PdfPTable(colCount);
               
                //Create PDF Table  

                //file will created in this path  
                // string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

                var writer = PdfWriter.GetInstance(doc, workStream);
                writer.CloseStream = false;
                doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                doc.Open();

                //Add Content to PDF   




                tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
                tableLayout.HeaderRows = 1;
                //var accountID = int.Parse(Session["AccountID"].ToString());
                //var QuickReportSetting = partBS.GetQuickReportSetting(accountID);
                if (colCount == 6)
                {
                    float[] headers = { 30, 30, 30, 20, 30, 30};
                    tableLayout.SetWidths(headers); //Set the pdf headers
                }
                
         
        

                if (screeningMatrixReport != null && screeningMatrixReport.ScreeningMatrix.Count > 0)
                {

                   


                    //RefdataBody = RefdataBody.Replace("[[Center]]", screeningMatrixReport.ScreeningMatrix[0].CenterName);

                    var centerList = screeningMatrixReport.ScreeningMatrix.Select(x => x.CenterID).Distinct().ToList();

                    for (int i = 0; i < centerList.Count; i++)
                    {
                        var screeningWithCenterList = screeningMatrixReport.ScreeningMatrix.Where(x => x.CenterID == centerList[i]).ToList();

                        var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();



                        //Add Title to the PDF file at the top 
                        PdfPCell cell = new PdfPCell(new Phrase(screeningWithCenterList[0].CenterName, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0))));


                        cell.Colspan = colCount;
                        cell.Padding = 5;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        tableLayout.AddCell(cell);

                        tableLayout.AddCell(new PdfPCell(new Phrase("Screening Matrix Report", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Colspan = colCount,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 5
                        });



                        ////Add header 

                        tableLayout.AddCell(new PdfPCell(new Phrase("Classroom", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                        });


                        tableLayout.AddCell(new PdfPCell(new Phrase("Screening Type", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                        });



                        tableLayout.AddCell(new PdfPCell(new Phrase("Up-to-Date", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase("Missing", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase("Expired", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase("Expiring", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                        });









                        for (int j = 0; j < classroomList.Count; j++)
                        {
                            var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();



                            for (int k = 0; k < screeningList.Count; k++)

                            {
                                //stringBuilder.Append("<tr>");

                                var rowSpan = screeningList.Count();

                                //if (j == 0 && k == 0)
                                //{


                                //    stringBuilder.Append("<td  style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' rowspan =" + rowSpan + ">" + screeningList[k].CenterName + "</td>");

                                //}

                                if (k == 0)


                                {
                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        Rowspan = rowSpan,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                }
                                //else
                                //{
                                //    //    stringBuilder.Append("<td style='display:none;border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'  rowspan = " + rowSpan + "></td>");

                                //}



                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ScreeningName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].UptoDate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].Missing.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].Expired.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].Expiring.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                //stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].ScreeningName + "</td>");

                                //stringBuilder.Append("<td  style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].UptoDate + "</td>");

                                //stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Missing + "</td>");
                                //stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Expired + "</td>");

                                //stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Expiring + "</td>");


                                //stringBuilder.Append("</tr>");






                            }



                        }



                    }



                    ///duplicate code//
                    ///

                    for (int i = 0; i < centerList.Count; i++)
                    {
                        var screeningWithCenterList = screeningMatrixReport.ScreeningMatrix.Where(x => x.CenterID == centerList[i]).ToList();

                        var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();




                        for (int j = 0; j < classroomList.Count; j++)
                        {
                            var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();



                            for (int k = 0; k < screeningList.Count; k++)

                            {
                                //stringBuilder.Append("<tr>");

                                var rowSpan = screeningList.Count();

                                //if (j == 0 && k == 0)
                                //{


                                //    stringBuilder.Append("<td  style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' rowspan =" + rowSpan + ">" + screeningList[k].CenterName + "</td>");

                                //}

                                if (k == 0)


                                {
                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        Rowspan = rowSpan,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                }
                                //else
                                //{
                                //    //    stringBuilder.Append("<td style='display:none;border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'  rowspan = " + rowSpan + "></td>");

                                //}



                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ScreeningName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].UptoDate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].Missing.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].Expired.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].Expiring.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                //stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].ScreeningName + "</td>");

                                //stringBuilder.Append("<td  style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].UptoDate + "</td>");

                                //stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Missing + "</td>");
                                //stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Expired + "</td>");

                                //stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Expiring + "</td>");


                                //stringBuilder.Append("</tr>");






                            }



                        }



                    }


                }


                ////Add body

                //foreach (var rep in deviceData)
                //{
                //    if (!string.IsNullOrEmpty(rep.PartNumber))
                //    {
                //        AddCellToBody(tableLayout, rep.MachineName.ToString());
                //        AddCellToBody(tableLayout, rep.PartNumber.ToString());
                //        AddCellToBody(tableLayout, rep.Description.ToString());
                //        AddCellToBodyRight(tableLayout, rep.ProductionHourRt);
                //        AddCellToBodyRight(tableLayout, rep.StoppedHourRt);
                //        AddCellToBodyRight(tableLayout, rep.PlannedShutdownHourRt);
                //        AddCellToBodyRight(tableLayout, rep.EquipEfficencyRt.ToString() + "%");
                //        AddCellToBodyRight(tableLayout, rep.NoOfIncidentRt.ToString());
                //        AddCellToBody(tableLayout, rep.StartDateTime.ToString());
                //        AddCellToBody(tableLayout, rep.EndDateTime.ToString());
                //    }
                //}
                //}


                //else
                //{
                //    var grossQty = (bool)(Session["GrossQtyFlag"]);
                //    ////Add header  
                //    AddCellToHeader(tableLayout, "Machine Name:");
                //    AddCellToHeader(tableLayout, "Part No:");
                //    if (QuickReportSetting.Description == true)
                //    {
                //        AddCellToHeader(tableLayout, "Description");
                //    }
                //    if (QuickReportSetting.ProductionHours == true)
                //    {
                //        AddCellToHeaderRight(tableLayout, "Production Hrs");
                //    }
                //    if (QuickReportSetting.PartsProduced == true)
                //    {
                //        AddCellToHeaderRight(tableLayout, "Parts Produced");
                //    }
                //    if (QuickReportSetting.AvgCycle == true)
                //    {
                //        AddCellToHeaderRight(tableLayout, "AVG Cycle");
                //    }
                //    if (QuickReportSetting.Target == true)
                //    {
                //        AddCellToHeaderRight(tableLayout, "Target");
                //    }
                //    if (QuickReportSetting.NoOfIncidents == true)
                //    {
                //        AddCellToHeaderRight(tableLayout, "Incidents");
                //    }
                //    if (QuickReportSetting.Scrap == true)
                //    {
                //        AddCellToHeaderRight(tableLayout, "Scrap");
                //    }
                //    if (grossQty == true)
                //        AddCellToHeaderRight(tableLayout, "Gross Qty");

                //    if (QuickReportSetting.TotalCost == true)
                //    {
                //        AddCellToHeaderRight(tableLayout, "Total Cost");
                //    }
                //    if (QuickReportSetting.TotalValue == true)
                //    {
                //        AddCellToHeaderRight(tableLayout, "Total Value");
                //    }
                //    AddCellToHeader(tableLayout, "Start Date:");
                //    AddCellToHeader(tableLayout, "End Date:");
                //    ////Add body

                //    foreach (var rep in deviceData)
                //    {
                //        if (!string.IsNullOrEmpty(rep.PartNumber))
                //        {
                //            AddCellToBody(tableLayout, rep.MachineName.ToString());
                //            AddCellToBody(tableLayout, rep.PartNumber.ToString());

                //            if (QuickReportSetting.Description == true)
                //            {
                //                AddCellToBody(tableLayout, rep.Description.ToString());
                //            }
                //            if (QuickReportSetting.ProductionHours == true)
                //            {
                //                AddCellToBodyRight(tableLayout, rep.ProductionHourRt + " / " + rep.ExpectedHourRt);
                //            }
                //            if (QuickReportSetting.PartsProduced == true)
                //            {
                //                AddCellToBodyRight(tableLayout, rep.PartsProducedRt + " / " + rep.ExpectedProducedRt);
                //            }
                //            if (QuickReportSetting.AvgCycle == true)
                //            {
                //                AddCellToBodyRight(tableLayout, rep.AvgCycleRt + " / " + rep.AssignedCycleRt);
                //            }
                //            if (QuickReportSetting.Target == true)
                //            {
                //                AddCellToBodyRight(tableLayout, rep.EquipEfficencyRt.ToString() + "%");
                //            }
                //            if (QuickReportSetting.NoOfIncidents == true)
                //            {
                //                AddCellToBodyRight(tableLayout, rep.NoOfIncidentRt.ToString());
                //            }
                //            if (QuickReportSetting.Scrap == true)
                //            {
                //                AddCellToBodyRight(tableLayout, rep.ScrapRt.ToString());
                //            }
                //            if (grossQty == true)
                //                AddCellToBodyRight(tableLayout, rep.GrossQtyRt.ToString());

                //            if (QuickReportSetting.TotalCost == true)
                //            {
                //                AddCellToBody(tableLayout, rep.TotalCostRt.ToString());
                //            }
                //            if (QuickReportSetting.TotalValue == true)
                //            {
                //                AddCellToBody(tableLayout, rep.TotalValueRt.ToString());
                //            }

                //            AddCellToBody(tableLayout, rep.StartDateTime.ToString());
                //            AddCellToBody(tableLayout, rep.EndDateTime.ToString());
                //        }
                //    }
                //}








                doc.Add(tableLayout);
                writer.PageEvent = new Footer();
                // Closing the document  
                doc.Close();



                byte[] bytes = workStream.ToArray();
                workStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=Screening_Matrix_Report.pdf");
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();


                //byte[] byteInfo = workStream.ToArray();
                //workStream.Write(byteInfo, 0, byteInfo.Length);
                //workStream.Position = 0;

                //Session[strAttachment] = workStream;
                // return Json(new { success = true, strAttachment }, JsonRequestBehavior.AllowGet);




















                //string RefdataBody = string.Empty;

                //StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/ServicePdf/ScreeningMatrixReport.html"));

                //RefdataBody = reader.ReadToEnd();






                //StringBuilder stringBuilder = new StringBuilder();







                //if (screeningMatrixReport != null && screeningMatrixReport.ScreeningMatrix.Count > 0)
                //{


                //    RefdataBody = RefdataBody.Replace("[[Center]]", screeningMatrixReport.ScreeningMatrix[0].CenterName);

                //    var centerList = screeningMatrixReport.ScreeningMatrix.Select(x => x.CenterID).Distinct().ToList();

                //    for (int i = 0; i < centerList.Count; i++)
                //    {
                //        var screeningWithCenterList = screeningMatrixReport.ScreeningMatrix.Where(x => x.CenterID == centerList[i]).ToList();

                //        var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();




                //        for (int j = 0; j < classroomList.Count; j++)
                //        {
                //            var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();



                //            for (int k = 0; k < screeningList.Count; k++)

                //            {
                //                stringBuilder.Append("<tr>");

                //                var rowSpan = screeningList.Count();

                //                //if (j == 0 && k == 0)
                //                //{


                //                //    stringBuilder.Append("<td  style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' rowspan =" + rowSpan + ">" + screeningList[k].CenterName + "</td>");

                //                //}

                //                if (k == 0)


                //                {
                //                    stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'  rowspan = '" + rowSpan + "'>" + screeningList[k].ClassroomName + "</td>");

                //                }
                //                else
                //                {
                //                    //    stringBuilder.Append("<td style='display:none;border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;'  rowspan = " + rowSpan + "></td>");

                //                }

                //                stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].ScreeningName + "</td>");

                //                stringBuilder.Append("<td  style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].UptoDate + "</td>");

                //                stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Missing + "</td>");
                //                stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Expired + "</td>");

                //                stringBuilder.Append("<td style='border: 1px solid #ddd;border-top:0;color: #787878;text-transform: none;font-size: 14px;text-decoration: none;font-family: Arial, Helvetica, sans-serif;font-weight: 400;text-align: center;padding: 5px;' >" + screeningList[k].Expiring + "</td>");


                //                stringBuilder.Append("</tr>");






                //            }



                //        }



                //    }
                //}




                //RefdataBody = RefdataBody.Replace("[[MatrixReportRow]]", stringBuilder.ToString());

                //var bytes = System.Text.Encoding.UTF8.GetBytes(RefdataBody);

                //var input = new MemoryStream(bytes);
                //var output = new MemoryStream(); // this MemoryStream is closed by FileStreamResult
                //var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER, 50, 50, 50, 50);
                //var writer = PdfWriter.GetInstance(document, output);
                //writer.CloseStream = false;
                //document.Open();
                //MemoryStream stream = new MemoryStream();
                //XMLWorkerHelper xmlWorker = XMLWorkerHelper.GetInstance();
                //xmlWorker.ParseXHtml(writer, document, input, stream);

                //writer.PageEvent = new Footer();
                //Paragraph welcomeParagraph = new Paragraph("");
                //document.Add(welcomeParagraph);
                ////writer.PageEvent = new Footer();

                ////Paragraph welcomeParagraph = new Paragraph("");

                ////document.Add(welcomeParagraph);
                //document.Close();
                //output.Position = 0;
                //var fileBytes = output.ToArray();
                //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                //Response.AddHeader("content-disposition", "attachment;filename= ScreeningMatrixReport.pdf");
                //Response.ContentType = "application/octectstream";
                //Response.BinaryWrite(fileBytes);
                //Response.End();



                //Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
                //PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //pdfDoc.Open();
                ///*End*/



                //PdfPTable table = new PdfPTable(6);

                //PdfPCell cell = new PdfPCell(new Phrase("Glenwood Center"));

                //cell.Colspan = 6;

                //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                //table.AddCell(cell);

                //table.AddCell("Col 1 Row 1");

                //table.AddCell("Col 2 Row 1");

                //table.AddCell("Col 3 Row 1");

                //table.AddCell("Col 1 Row 2");

                //table.AddCell("Col 2 Row 2");

                //table.AddCell("Col 3 Row 2");

                //pdfDoc.Add(table);

                ///*Creating the PDF file*/
                //pdfWriter.CloseStream = false;
                //pdfDoc.Close();
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=Credit-Card-Report.pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.Write(pdfDoc);
                //Response.End()



                #endregion

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }


        }




    }
}
