using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
using System.Data;
using System.Web.Mvc;

namespace FingerprintsModel
{
    public class Export
    {
        public void Exportpdf(Agencyreport Agencyreport, Stream strPdf, string imagepath)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 30f, 30f, 5f, 10f);
                PdfWriter.GetInstance(pdfDoc, strPdf);
                pdfDoc.Open();
                Image logo = Image.GetInstance(imagepath);
                pdfDoc.Add(logo);
                Chunk chunk = new Chunk("Section B of the PIR as on " + DateTime.Now, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));
                Paragraph reportHeadline = new Paragraph(chunk);
                reportHeadline.Alignment = Element.ALIGN_CENTER;
                reportHeadline.SpacingBefore = 12.0f;
                pdfDoc.Add(reportHeadline);
                var fontWhiteBold = FontFactory.GetFont("Tahoma", 12, Font.NORMAL, CMYKColor.BLACK);
                var fontSimple = FontFactory.GetFont("Tahoma", 10, Font.NORMAL);
                var fontBold = FontFactory.GetFont("Tahoma", 12, Font.NORMAL);
                PdfPTable _table = new PdfPTable(3);
                _table.SpacingBefore = 20;
                _table.TotalWidth = 1000f;
                PdfPCell cell1 = new PdfPCell();
                cell1.Border = 0;
                _table.AddCell(cell1);
                PdfPCell cell2 = new PdfPCell(new Phrase("(1) # of Head Start or Early Head Start Staff"));
                cell2.BackgroundColor = CMYKColor.LIGHT_GRAY;
                cell2.FixedHeight = 33f;
                _table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase("(2) # of Contracted Staff"));
                cell3.BackgroundColor = CMYKColor.LIGHT_GRAY;
                _table.AddCell(cell3);
                cell3.FixedHeight = 33f;
                _table.AddCell("Total no of staff member, regardless of the funding source for their salary or no of hours worked.");
                _table.AddCell(Agencyreport.totalhdstarterlyhdstart);
                _table.AddCell(Agencyreport.totalcontracterhdstarterlyhdstart);
                int countheadstart = 0;
                int countcontractor = 0;
                foreach (var item in Agencyreport.Agencystaffreport)
                {
                    countheadstart = countheadstart + Convert.ToInt32(item.totalAssociatedProgram);
                    countcontractor = countcontractor + Convert.ToInt32(item.Contractor);
                }
                _table.AddCell("a. Of these, the number who are current or former Head Start or Early Head Start parents");
                _table.AddCell(Convert.ToString(countheadstart));
                _table.AddCell(Convert.ToString(countcontractor));
                _table.AddCell("b. Of these, the number who left since last year's PIR was reported.");
                _table.AddCell(Agencyreport.terminationdate);
                _table.AddCell(Agencyreport.Contractortotalterminated);
                _table.AddCell("1. Of these, the number who were replaced.");
                _table.AddCell(Agencyreport.totalreplaced);
                _table.AddCell(Agencyreport.totalreplacedcontrator);
                PdfPCell cell4 = new PdfPCell(new Phrase("Program completing the PIR survey for the first time should report the number of staff who left since the program began."));
                cell4.Colspan = 3;
                cell4.BackgroundColor = CMYKColor.LIGHT_GRAY;
                _table.AddCell(cell4);
                pdfDoc.Add(_table);
                pdfDoc.Close();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
           
        }

        public MemoryStream Exportexcel(Agencyreport Agencyreport)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                XLWorkbook wb = new XLWorkbook();
                var vs = wb.Worksheets.Add("Section B of the PIR");
                vs.Range("A1:C1").Merge().Value = "Section B of the PIR on " + DateTime.Now;
                vs.Cell(2, 1).Value = "";
                vs.Cell(2, 2).Value = "(1) # of Head Start or Early Head Start Staff";
                vs.Cell(2, 3).Value = "(2) # or Contracted Staff";
                vs.Cell(3, 1).Value = "Total no of staff member, regardless of the funding source for their salary or no of hours worked.";
                vs.Cell(3, 2).Value = Agencyreport.totalhdstarterlyhdstart;
                vs.Cell(3, 3).Value = Agencyreport.totalcontracterhdstarterlyhdstart;
                int row = 4;
                int countheadstart = 0;
                int countcontractor = 0;
                foreach (var item in Agencyreport.Agencystaffreport)
                {
                    countheadstart = countheadstart + Convert.ToInt32(item.totalAssociatedProgram);
                    countcontractor = countcontractor + Convert.ToInt32(item.Contractor);

                }
                vs.Cell(row, 1).Value = "a. Of these,the number who are current or former Head Start or Early Head Start parents";
                vs.Cell(row, 2).Value = Convert.ToString(countheadstart);
                vs.Cell(row, 3).Value = Convert.ToString(countcontractor);
                row++;
                vs.Cell(row, 1).Value = "b. Of these, the number who left since last year's PIR was reported.";
                vs.Cell(row, 2).Value = Agencyreport.terminationdate;
                vs.Cell(row, 3).Value = Agencyreport.Contractortotalterminated;
                row++;
                vs.Cell(row, 1).Value = "1. Of these, the number who were replaced.";
                vs.Cell(row, 2).Value = Agencyreport.totalreplaced;
                vs.Cell(row, 3).Value = Agencyreport.totalreplacedcontrator;
                vs.Range("A7:C7").Merge().Value = "Program completing the PIR survey for the first time should report the number of staff who left since the program began.";
               
                wb.SaveAs(memoryStream);
                //memoryStream.WriteTo(strExcel);
                
            }
            catch (Exception ex)
            {
                clsError.WriteException( ex);
            }
            return memoryStream;
        }

        public void Exportpdf(FPA obj, Stream strPdf, string imagepath)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4, 30f, 30f, 5f, 10f);
                PdfWriter.GetInstance(pdfDoc, strPdf);
                pdfDoc.Open();
                Image logo = Image.GetInstance(imagepath);
                pdfDoc.Add(logo);
                Chunk chunk = new Chunk("Family Partnership Agreement ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, BaseColor.BLUE));
                Paragraph reportHeadline = new Paragraph(chunk);
                reportHeadline.Alignment = Element.ALIGN_CENTER;
                reportHeadline.SpacingBefore = 20.0f; reportHeadline.SpacingAfter = 8.0f;
                pdfDoc.Add(reportHeadline);
                var fontWhiteBold = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, CMYKColor.BLACK);
                var fontSimple = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, Font.NORMAL);
                var fontBold = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL);
                iTextSharp.text.Font f = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 10);

                iTextSharp.text.Font f2 = new iTextSharp.text.Font(FontFactory.GetFont(FontFactory.TIMES_BOLD, 10));
                iTextSharp.text.Font f3 = new iTextSharp.text.Font(FontFactory.GetFont(FontFactory.TIMES_BOLD, 12));

                PdfPTable _table = new PdfPTable(3);
                _table.SpacingAfter = 5.0f;
                _table.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell p1 = new PdfPCell(new Phrase(2, ("Goal "), f));
                p1.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p1);
                PdfPCell p2 = new PdfPCell(new Phrase(2, (":"), f)); p2.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p2);
                PdfPCell p3 = new PdfPCell(new Phrase(2, (obj.Goal), f)); p3.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p3);
                PdfPCell p11 = new PdfPCell(new Phrase(2, ("Start Date "), f)); p11.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p11);
                PdfPCell p12 = new PdfPCell(new Phrase(2, (":"), f)); p12.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p12);
                PdfPCell p13 = new PdfPCell(new Phrase(2, (obj.GoalDate), f)); p13.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p13);
                PdfPCell p21 = new PdfPCell(new Phrase(2, ("FEO "), f)); p21.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p21);
                PdfPCell p22 = new PdfPCell(new Phrase(2, (":"), f)); p22.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p22);
                PdfPCell p23 = new PdfPCell(new Phrase(2, (obj.CategoryDesc), f)); p23.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p23);
                PdfPCell p31 = new PdfPCell(new Phrase(2, ("Status "), f)); p31.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p31);
                PdfPCell p32 = new PdfPCell(new Phrase(2, (":"), f)); p32.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p32);
                PdfPCell p33 = new PdfPCell(new Phrase(2, obj.GoalStatus == 0 ? "Open" : obj.GoalStatus == 1 ? "Complete" : obj.GoalStatus == 2 ? "Abandoned" : "Refused to do a FPA", f)); p33.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p33);
                PdfPCell p41 = new PdfPCell(new Phrase(2, ("Completion Date "), f)); p41.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p41);
                PdfPCell p42 = new PdfPCell(new Phrase(2, (":"), f)); p42.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p42);
                PdfPCell p43 = new PdfPCell(new Phrase(2, (obj.CompletionDate), f)); p43.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p43);
                PdfPCell p51 = new PdfPCell(new Phrase(2, ("Assigned To "), f)); p51.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p51);
                PdfPCell p52 = new PdfPCell(new Phrase(2, (":"), f)); p52.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p52);
                string Assignedto = obj.GoalFor == 1 && obj.IsSingleParent ? obj.ParentName1 :
                    obj.GoalFor == 3 ? obj.ParentName1 + ", " + obj.ParentName2 :
                    obj.GoalFor == 1 ? obj.ParentName1 :
                    obj.ParentName2;
                PdfPCell p53 = new PdfPCell(new Phrase(2, Assignedto, f)); p53.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p53);
                Chunk chunk2 = new Chunk("Goal Details ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));
                Paragraph goal = new Paragraph(chunk2);
                goal.Alignment = Element.ALIGN_CENTER;
                goal.SpacingBefore = 5.0f; goal.SpacingAfter = 5.0f;
                pdfDoc.Add(goal);
                pdfDoc.Add(_table);
                Chunk chunk3 = new Chunk(" Goal Steps ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));
                Paragraph steps = new Paragraph(chunk3);
                steps.Alignment = Element.ALIGN_CENTER;
                steps.SpacingBefore = 5.0f; steps.SpacingAfter = 5.0f;
                pdfDoc.Add(steps);
                PdfPTable _table2 = new PdfPTable(4);
                _table2.SpacingBefore = 5.0f;
                PdfPCell sp1 = new PdfPCell(new Phrase(2, "Description ", f2));
                _table2.AddCell(sp1);
                PdfPCell sp2 = new PdfPCell(new Phrase(2, "Date of Completion", f2));
                _table2.AddCell(sp2);
                PdfPCell sp3 = new PdfPCell(new Phrase(2, "Status", f2));
                _table2.AddCell(sp3);
                PdfPCell sp4 = new PdfPCell(new Phrase(2, "Reminder for", f2));
                _table2.AddCell(sp4);

                foreach (var item in obj.GoalSteps)
                {
                    PdfPCell sp11 = new PdfPCell(new Phrase(2, item.Description, f));
                    _table2.AddCell(sp11);
                    PdfPCell sp21 = new PdfPCell(new Phrase(2, item.StepsCompletionDate, f));
                    _table2.AddCell(sp21);
                    PdfPCell sp31 = new PdfPCell(new Phrase(2, item.Status == 0 ? "Open" : item.Status == 1 ? "Complete" : "Abandoned", f));
                    _table2.AddCell(sp31);
                    PdfPCell sp41 = new PdfPCell(new Phrase(2, item.Reminderdays.ToString(), f));
                    _table2.AddCell(sp41);

                }

                pdfDoc.Add(_table2);
                if (!string.IsNullOrEmpty(obj.SignatureData))
                {
                    PdfPTable _tableim = new PdfPTable(1);
                    _tableim.SpacingBefore = 20f;
                    _table.DefaultCell.Border = Rectangle.NO_BORDER;
                    string theSource = obj.SignatureData.Replace("data:image/png;base64,", "");
                    var src = theSource;
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Convert.FromBase64String(src));
                    image.ScaleAbsolute(100f, 20f);
                    iTextSharp.text.pdf.PdfPCell imgCell1 = new iTextSharp.text.pdf.PdfPCell();
                    imgCell1.Border = Rectangle.NO_BORDER;
                    imgCell1.AddElement(new Chunk(image, 0, 0));
                    _tableim.AddCell(imgCell1);
                    PdfPCell cign = new PdfPCell(new Phrase("Signature:"));
                    cign.Border = Rectangle.NO_BORDER;
                    _tableim.AddCell(cign);
                    pdfDoc.Add(_tableim);

                }
                pdfDoc.Close();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                
            }
            
        }

        public void ExportPdf2(FPA obj, Stream PDFData, string imagepath)
        {

            // Document document = new Document(PageSize.LETTER, 50, 50, 80, 50);
            Document pdfDoc = new Document(PageSize.A4, 50, 50, 160, 50);
            PdfWriter PDFWriter = PdfWriter.GetInstance(pdfDoc, PDFData);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            // Our custom Header and Footer is done using Event Handler
            TwoColumnHeaderFooter PageEventHandler = new TwoColumnHeaderFooter();
            PDFWriter.PageEvent = PageEventHandler;
            // Define the page header
            PageEventHandler.Title = "Family Partnership Agreement";

            PageEventHandler.HeaderFont = FontFactory.GetFont(BaseFont.COURIER_BOLD, 14, Font.BOLD);
            Image logo;
            if (!string.IsNullOrEmpty(obj.AgencyLogo))
            {
                string theSource = obj.AgencyLogo.Replace("data:image/png;base64,", "");
                var src = theSource;
                logo = iTextSharp.text.Image.GetInstance(Convert.FromBase64String(src));
            }
            else
            {
                logo = Image.GetInstance(imagepath);
            }
            logo.ScaleAbsolute(120f, 100f);
            PageEventHandler.HeaderLeft = logo;

            //PageEventHandler.HeaderRight = "1";
            pdfDoc.Open();
            try
            {

                AddOutline(PDFWriter, "Group ", pdfDoc.PageSize.Height);
                var fontWhiteBold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, Font.NORMAL, CMYKColor.BLACK);
                var fontSimple = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10, Font.NORMAL);
                var fontBold = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12, Font.NORMAL);
                iTextSharp.text.Font f3 = new iTextSharp.text.Font(FontFactory.GetFont(FontFactory.TIMES_BOLD, 11));
                iTextSharp.text.Font f4 = new iTextSharp.text.Font(Font.FontFamily.TIMES_ROMAN, 11);
                iTextSharp.text.Font f2 = new iTextSharp.text.Font(FontFactory.GetFont(FontFactory.TIMES_BOLD, 10));
                iTextSharp.text.Font f = new iTextSharp.text.Font(FontFactory.GetFont(FontFactory.TIMES_BOLD, 12));

                PdfPTable _table = new PdfPTable(3);
                _table.SpacingAfter = 5.0f;
                _table.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell p1 = new PdfPCell(new Phrase(2, ("Goal "), f4));
                p1.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p1);
                PdfPCell p2 = new PdfPCell(new Phrase(2, (":"), f)); p2.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p2);
                PdfPCell p3 = new PdfPCell(new Phrase(2, (obj.Goal), f)); p3.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p3);
                PdfPCell p11 = new PdfPCell(new Phrase(2, ("Start Date "), f4)); p11.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p11);
                PdfPCell p12 = new PdfPCell(new Phrase(2, (":"), f)); p12.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p12);
                PdfPCell p13 = new PdfPCell(new Phrase(2, (obj.GoalDate), f4)); p13.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p13);
                //PdfPCell p21 = new PdfPCell(new Phrase(2, ("FEO "), f)); p21.Border = PdfPCell.NO_BORDER;
                //_table.AddCell(p21);
                //PdfPCell p22 = new PdfPCell(new Phrase(2, (":"), f)); p22.Border = PdfPCell.NO_BORDER;
                //_table.AddCell(p22);
                //PdfPCell p23 = new PdfPCell(new Phrase(2, (obj.CategoryDesc), f)); p23.Border = PdfPCell.NO_BORDER;
                //_table.AddCell(p23);
                PdfPCell p31 = new PdfPCell(new Phrase(2, ("Status "), f4)); p31.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p31);
                PdfPCell p32 = new PdfPCell(new Phrase(2, (":"), f)); p32.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p32);
                PdfPCell p33 = new PdfPCell(new Phrase(2, obj.GoalStatus == 0 ? "Open" : obj.GoalStatus == 1 ? "Complete" : obj.GoalStatus == 2 ? "Abandoned" : "Refused to do a FPA", f4)); p33.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p33);
                PdfPCell p41 = new PdfPCell(new Phrase(2, ("Completion Date "), f4)); p41.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p41);
                PdfPCell p42 = new PdfPCell(new Phrase(2, (":"), f)); p42.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p42);
                PdfPCell p43 = new PdfPCell(new Phrase(2, (obj.CompletionDate), f3)); p43.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p43);
                PdfPCell p51 = new PdfPCell(new Phrase(2, ("Assigned Parent/Guardian "), f4)); p51.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p51);
                PdfPCell p52 = new PdfPCell(new Phrase(2, (":"), f)); p52.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p52);
                string Assignedto = obj.IsSingleParent ? obj.ParentName1 :
                    obj.GoalFor == 3 ? obj.ParentName1 + ", " + obj.ParentName2 :
                    obj.GoalFor == 1 ? obj.ParentName1 :
                    obj.ParentName2;
                PdfPCell p53 = new PdfPCell(new Phrase(2, Assignedto, f4)); p53.Border = PdfPCell.NO_BORDER;
                _table.AddCell(p53);
                Chunk chunk2 = new Chunk("Goal Details ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));
                Paragraph goal = new Paragraph(chunk2);
                goal.Alignment = Element.ALIGN_CENTER;
                goal.SpacingBefore = 5.0f; goal.SpacingAfter = 5.0f;
                pdfDoc.Add(goal);
                pdfDoc.Add(_table);
                Chunk chunk3 = new Chunk(" Goal Steps ", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE));
                Paragraph steps = new Paragraph(chunk3);
                steps.Alignment = Element.ALIGN_CENTER;
                steps.SpacingBefore = 5.0f; steps.SpacingAfter = 5.0f;
                pdfDoc.Add(steps);
                PdfPTable _table2 = new PdfPTable(4);
                _table2.SpacingBefore = 5.0f;
                PdfPCell sp1 = new PdfPCell(new Phrase(2, "Description ", f3));
                _table2.AddCell(sp1);
                PdfPCell sp2 = new PdfPCell(new Phrase(2, "Date of Completion", f3));
                _table2.AddCell(sp2);
                PdfPCell sp3 = new PdfPCell(new Phrase(2, "Status", f3));
                _table2.AddCell(sp3);
                PdfPCell sp4 = new PdfPCell(new Phrase(2, "Actual Complition Date", f3));
                _table2.AddCell(sp4);

                foreach (var item in obj.GoalSteps)
                {
                    PdfPCell sp11 = new PdfPCell(new Phrase(2, item.Description, f4));
                    _table2.AddCell(sp11);
                    PdfPCell sp21 = new PdfPCell(new Phrase(2, item.StepsCompletionDate, f4));
                    _table2.AddCell(sp21);
                    PdfPCell sp31 = new PdfPCell(new Phrase(2, item.Status == 0 ? "Open" : item.Status == 1 ? "Complete" : "Abandoned", f4));
                    _table2.AddCell(sp31);
                    PdfPCell sp41;
                    if (!string.IsNullOrEmpty(item.ActualCompletionDate))
                    {
                        sp41 = new PdfPCell(new Phrase(2, item.ActualCompletionDate.ToString(), f4));
                    }
                    else
                    {
                        sp41 = new PdfPCell(new Phrase(2, "", f));
                    }
                    _table2.AddCell(sp41);

                }

                pdfDoc.Add(_table2);
                if (!string.IsNullOrEmpty(obj.SignatureData))
                {
                    PdfPTable _tableim = new PdfPTable(1);
                    _tableim.SpacingBefore = 20f;
                    _table.DefaultCell.Border = Rectangle.NO_BORDER;
                    string theSource = obj.SignatureData.Replace("data:image/png;base64,", "");
                    var src = theSource;
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Convert.FromBase64String(src));
                    image.ScaleAbsolute(100f, 20f);
                    iTextSharp.text.pdf.PdfPCell imgCell1 = new iTextSharp.text.pdf.PdfPCell();
                    imgCell1.Border = Rectangle.NO_BORDER;
                    imgCell1.AddElement(new Chunk(image, 0, 0));
                    _tableim.AddCell(imgCell1);
                    PdfPCell cign = new PdfPCell(new Phrase("Signature:"));
                    cign.Border = Rectangle.NO_BORDER;
                    _tableim.AddCell(cign);
                    pdfDoc.Add(_tableim);

                }
                pdfDoc.Close();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
        }
        public void AddOutline(PdfWriter writer, string Title, float Position)
        {
            PdfDestination destination = new PdfDestination(PdfDestination.FITH, Position);
            PdfOutline outline = new PdfOutline(writer.DirectContent.RootOutline, destination, Title);
            writer.DirectContent.AddOutline(outline, "Name = " + Title);
        }

        public MemoryStream ExportExcelScreeningMatrix( ScreeningMatrix ScreeningMatrix   )
        {
             MemoryStream memoryStream = new MemoryStream();
            try
            {
                List<List<string>> screening = ScreeningMatrix.Screenings;
                XLWorkbook wb = new XLWorkbook();
                var vs = wb.Worksheets.Add("Missing Screening report");
                if(screening.Count>3)
                {
                    vs.Range("B1:H1").Merge().Value = "Missing Screening Collection report for " + ScreeningMatrix.Screenings[1][3].ToString() + " on " + DateTime.Now.ToString("MM/dd/yyyy");
                }
                else
                {
                    vs.Range("B1:H1").Merge().Value = "Missing Screening Collection report  as on " + DateTime.Now.ToString("MM/dd/yyyy");
                }
                vs.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                vs.Range("B1:H1").Style.Font.SetBold(true);

                int Reportcolumn = 2;
                int ReportRow = 3;
                for (var i = 0; i < screening[0].Count; i++)
                {
                    vs.Cell(3, Reportcolumn).Value = screening[0][i];
                    vs.Cell(3, Reportcolumn).Style.Font.SetBold(true);
                    Reportcolumn++;
                }
                ReportRow = 5;
                for (var i = 1; i < screening.Count; i++)
                {
                    Reportcolumn = 2;
                    for (var j = 0; j < screening[i].Count; j++)
                    {
                        if (j == 1)
                        {
                            vs.Cell(ReportRow, Reportcolumn).Value = screening[i][j];
                            IXLColumn _column = vs.Cell(ReportRow, Reportcolumn).WorksheetColumn();
                            _column.Width = 20;
                        }
                        else if (screening[i][j] == "M")
                        {
                            vs.Cell(ReportRow, Reportcolumn).Value = screening[i][j];
                            vs.Cell(ReportRow, Reportcolumn).AsRange().Style.Font.FontColor = XLColor.FromHtml("#295b8f");
                            IXLColumn _column = vs.Cell(ReportRow, Reportcolumn).WorksheetColumn();
                            _column.Width = 10;
                        }
                        else if (screening[i][j].Contains("ScreeningDate"))
                        {
                            vs.Cell(ReportRow, Reportcolumn).Value = screening[i][j].Replace("ScreeningDate", "");
                            vs.Cell(ReportRow, Reportcolumn).Style.NumberFormat.Format = "mm/dd/yyyy";
                            vs.Cell(ReportRow, Reportcolumn).AsRange().Style.Font.FontColor = XLColor.FromArgb(0, 132, 209);
                            IXLColumn _column = vs.Cell(ReportRow, Reportcolumn).WorksheetColumn();
                            _column.Width = 10;
                        }
                        else if (screening[i][j].Contains("ExpiringDate"))
                        {
                            vs.Cell(ReportRow, Reportcolumn).Value = screening[i][j].Replace("ExpiringDate", "");
                            vs.Cell(ReportRow, Reportcolumn).Style.NumberFormat.Format = "mm/dd/yyyy";
                            vs.Cell(ReportRow, Reportcolumn).AsRange().Style.Font.FontColor = XLColor.Orange;
                            IXLColumn _column = vs.Cell(ReportRow, Reportcolumn).WorksheetColumn();
                            _column.Width = 10;
                        }
                        else if (screening[i][j].Contains("ExpiredDate"))
                        {
                            vs.Cell(ReportRow, Reportcolumn).Value = "X";
                            vs.Cell(ReportRow, Reportcolumn).AsRange().Style.Font.FontColor = XLColor.Red;
                            vs.Cell(ReportRow, Reportcolumn).Comment.AddText(screening[i][j].Replace("ExpiredDate", ""));
                            IXLColumn _column = vs.Cell(ReportRow, Reportcolumn).WorksheetColumn();
                            _column.Width = 10;
                        }
                        else if (screening[i][j] != "")
                        {
                            vs.Cell(ReportRow, Reportcolumn).Value = screening[i][j];
                            IXLColumn _column = vs.Cell(ReportRow, Reportcolumn).WorksheetColumn();
                            _column.Width = 10;
                        }
                        else
                        {
                            vs.Cell(ReportRow, Reportcolumn).Value = "REFUSED";
                            IXLColumn _column = vs.Cell(ReportRow, Reportcolumn).WorksheetColumn();
                            _column.Width = 10;
                        }
                        Reportcolumn++;
                    }
                    ReportRow++;
                }
                ReportRow++;
                int Status = 0;
                int missingorexpired = 0;
                int completed = 0;
                for (var j = 0; j < 4; j++)
                {
                    ReportRow++;
                    Reportcolumn = 3;
                    Status = 0;
                    for (var i = 0; i < screening[0].Count - 1; i++)
                    {
                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                vs.Cell(ReportRow, 2).Value = "Complete";
                            }
                            if (j == 1)
                            {
                                vs.Cell(ReportRow, 2).Value = "Missing";
                                vs.Cell(ReportRow, 2).AsRange().Style.Font.FontColor = XLColor.FromHtml("#295b8f");
                            }
                            if (j == 2)
                            {
                                vs.Cell(ReportRow, 2).Value = "Expired";
                                vs.Cell(ReportRow, 2).AsRange().Style.Font.FontColor = XLColor.Red;
                            }
                            if (j == 3)
                            {
                                vs.Cell(ReportRow, 2).Value = "Expiring";
                                vs.Cell(ReportRow, 2).AsRange().Style.Font.FontColor = XLColor.Orange;
                            }
                        }
                        else
                        {
                            var count = 0;
                            if (j == 0)
                            {
                                for (var k = 1; k < screening.Count; k++)
                                {
                                    if (screening[k][i + 1].Contains("ScreeningDate"))
                                        count = count + 1;
                                }
                                vs.Cell(ReportRow, Reportcolumn).Value = count;
                                Status = Status + count;
                                completed = completed + count;
                            }
                            count = 0;
                            if (j == 1)
                            {
                                for (var k = 1; k < screening.Count; k++)
                                {
                                    if (screening[k][i + 1] == "M")
                                        count = count + 1;
                                }
                                vs.Cell(ReportRow, Reportcolumn).Value = count;
                                Status = Status + count;
                                missingorexpired = missingorexpired + count;
               //            }
               //            count = 0;
               //            if (j == 2)
               //            {
               //                for (var k = 1; k < screening.Count; k++)
               //                {
               //                    if (screening[k][i + 1].Contains("ExpiredDate"))
               //                        count = count + 1;
               //                }
               //                vs.Cell(ReportRow, Reportcolumn).Value = count;
               //                Status = Status + count;
               //                missingorexpired = missingorexpired + count;
               //            }
               //            count = 0;
               //            if (j == 3)
               //            {
               //                for (var k = 1; k < screening.Count; k++)
               //                {
               //                    if (screening[k][i + 1].Contains("ExpiringDate"))
               //                        count = count + 1;
               //                }
               //                vs.Cell(ReportRow, Reportcolumn).Value = count;
               //                Status = Status + count;
               //            }
               //            count = 0;
               //        }
               //        Reportcolumn++;
               //    }
               //    vs.Cell(ReportRow, Reportcolumn + 1).Value = Status;
               //}
               //vs.Cell(ReportRow + 2, 2).Value = "Total missing or expired records";
               //vs.Cell(ReportRow + 2, 2).Style.Font.SetBold(true);
               //vs.Cell(ReportRow + 2, 3).Value = missingorexpired;
               //vs.Cell(ReportRow + 2, 4).Value = "Total completed screening";
               //vs.Cell(ReportRow + 2, 4).Style.Font.SetBold(true);
               //vs.Cell(ReportRow + 2, 5).Value = completed;
               //ReportRow = ReportRow + 4;
                            }
                            count = 0;
                            if (j == 2)
               {
                                for (var k = 1; k < screening.Count; k++)
                   {
                                    if (screening[k][i + 1].Contains("ExpiredDate"))
                                        count = count + 1;
                                }
                                vs.Cell(ReportRow, Reportcolumn).Value = count;
                                Status = Status + count;
                       //vs.Cell(ReportRow, 5).Value ="Status";
                       //vs.Cell(ReportRow, 6).Value = "Notes";
                                missingorexpired = missingorexpired + count;
                   }
                            count = 0;
                            if (j == 3)
                   {
                                for (var k = 1; k < screening.Count; k++)
                                {
                                    if (screening[k][i + 1].Contains("ExpiringDate"))
                                        count = count + 1;
                                }
                                vs.Cell(ReportRow, Reportcolumn).Value = count;
                                Status = Status + count;
                   }
                            count = 0;
                        }
                        Reportcolumn++;
               }
                    vs.Cell(ReportRow, Reportcolumn + 1).Value = Status;
                }
                vs.Cell(ReportRow + 2, 2).Value = "Total missing or expired records";
                vs.Cell(ReportRow + 2, 2).Style.Font.SetBold(true);
                vs.Cell(ReportRow + 2, 3).Value = missingorexpired;
                vs.Cell(ReportRow + 2, 4).Value = "Total completed screening";
                vs.Cell(ReportRow + 2, 4).Style.Font.SetBold(true);
                vs.Cell(ReportRow + 2, 5).Value = completed;
                ReportRow = ReportRow + 4;
                wb.SaveAs(memoryStream);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return memoryStream;
        }


        public MemoryStream ExportERSEACenterAnalysisReport(string reportFor, ChildrenInfoClass infoClass, List<SelectListItem> parentList, bool isAllCenter)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {

                XLWorkbook wb = new XLWorkbook();
                IXLWorksheet workSheet = null;
                int ReportRow = 3;
                switch (reportFor)
                {
                    case "#FosterDisplaymodal":
                        workSheet = wb.Worksheets.Add("Enrolled Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value = (isAllCenter) ? "Enrolled Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy") : "Enrolled Client Report for " + infoClass.ChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if (isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 5).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 6).Value = "Attendance";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 7).Value = "Over Income";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 8).Value = "Foster";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 9).Value = "Attendance Status";
                            workSheet.Cell(ReportRow, 9).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 9).WorksheetColumn().Width = 20;
                        }
                        else
                        {


                            workSheet.Cell(ReportRow, 5).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 6).Value = "Attendance";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 7).Value = "Over Income";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 8).Value = "Foster";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 9).Value = "Attendance Status";
                            workSheet.Cell(ReportRow, 9).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 9).WorksheetColumn().Width = 20;
                        }
                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.ChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ClientName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;
                            if (isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.ClassStartDate;
                                workSheet.Cell(ReportRow, 7).Value = item.AttendancePercentage + "%";
                                workSheet.Cell(ReportRow, 8).Value = item.OverIncome;
                                workSheet.Cell(ReportRow, 9).Value = (item.Foster == "1") ? "Y" : "N";
                                workSheet.Cell(ReportRow, 10).Value = (item.ChildAttendance == "0") ? "Not Checked-in" : (item.ChildAttendance == "1") ? "Present" : (item.ChildAttendance == "2") ? "Absent Excused" : (item.ChildAttendance == "3") ? "Absent No Show" : "Present Other";
                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.ClassStartDate;
                                workSheet.Cell(ReportRow, 6).Value = item.AttendancePercentage + "%";
                                workSheet.Cell(ReportRow, 7).Value = item.OverIncome;
                                workSheet.Cell(ReportRow, 8).Value = (item.Foster == "1") ? "Y" : "N";
                                workSheet.Cell(ReportRow, 9).Value = (item.ChildAttendance == "0") ? "Not Checked-in" : (item.ChildAttendance == "1") ? "Present" : (item.ChildAttendance == "2") ? "Absent Excused" : (item.ChildAttendance == "3") ? "Absent No Show" : "Present Other";
                            }
                            ReportRow++;
                        }
                        break;
                    case "#EnrolledModal":
                        workSheet = wb.Worksheets.Add("Enrolled Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value = (isAllCenter) ? "Enrolled Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy") : "Enrolled Client Report for " + infoClass.ChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);

                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if (isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Nae";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);

                            workSheet.Cell(ReportRow, 6).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 7).Value = "Attendance";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 8).Value = "Over Income";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 9).Value = "Foster";
                            workSheet.Cell(ReportRow, 9).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 9).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 10).Value = "Attendance Status";
                            workSheet.Cell(ReportRow, 10).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 10).WorksheetColumn().Width = 20;
                        }


                        else
                        {


                            workSheet.Cell(ReportRow, 5).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 6).Value = "Attendance";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 7).Value = "Over Income";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 8).Value = "Foster";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 9).Value = "Attendance Status";
                            workSheet.Cell(ReportRow, 9).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 9).WorksheetColumn().Width = 20;
                        }
                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.ChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ClientName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;

                            if (isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.ClassStartDate;
                                workSheet.Cell(ReportRow, 7).Value = item.AttendancePercentage + "%";
                                workSheet.Cell(ReportRow, 8).Value = item.OverIncome;
                                workSheet.Cell(ReportRow, 9).Value = (item.Foster == "1") ? "Y" : "N";
                                workSheet.Cell(ReportRow, 10).Value = (item.ChildAttendance == "0") ? "Not Checked-in" : (item.ChildAttendance == "1") ? "Present" : (item.ChildAttendance == "2") ? "Absent Excused" : (item.ChildAttendance == "3") ? "Absent No Show" : "Present Other";

                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.ClassStartDate;
                                workSheet.Cell(ReportRow, 6).Value = item.AttendancePercentage + "%";
                                workSheet.Cell(ReportRow, 7).Value = item.OverIncome;
                                workSheet.Cell(ReportRow, 8).Value = (item.Foster == "1") ? "Y" : "N";
                                workSheet.Cell(ReportRow, 9).Value = (item.ChildAttendance == "0") ? "Not Checked-in" : (item.ChildAttendance == "1") ? "Present" : (item.ChildAttendance == "2") ? "Absent Excused" : (item.ChildAttendance == "3") ? "Absent No Show" : "Present Other";


                            }

                            ReportRow++;
                        }
                        break;


                    case "#WithdrawnModal":
                        workSheet = wb.Worksheets.Add("Withdrawn Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value = (isAllCenter) ? "Withdrawn Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy") : "Withdrawn Client Report for " + infoClass.WithdrawnChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if (isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 6).Value = "Date on List";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 7).Value = "Program Type";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 8).Value = "Selection Points";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 20;
                        }
                        else
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Date on List";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 6).Value = "Program Type";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 7).Value = "Selection Points";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                        }

                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.WithdrawnChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ChildrenName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;

                            if (isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.DateOnList;
                                workSheet.Cell(ReportRow, 7).Value = item.ProgramType;
                                workSheet.Cell(ReportRow, 8).Value = item.SelectionPoints;
                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.DateOnList;
                                workSheet.Cell(ReportRow, 6).Value = item.ProgramType;
                                workSheet.Cell(ReportRow, 7).Value = item.SelectionPoints;
                            }

                            ReportRow++;
                        }
                        break;


                    case "#DroppedModal":
                        workSheet = wb.Worksheets.Add("Dropped Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value = (isAllCenter) ? "Dropped Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy") : "Dropped Client Report for " + infoClass.DroppedChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if (isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 6).Value = "Date on List";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 7).Value = "Program Type";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;

                            workSheet.Cell(ReportRow, 8).Value = "Selection Points";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 20;
                        }
                        else
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Date on List";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 6).Value = "Program Type";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 7).Value = "Selection Points";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                        }


                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.DroppedChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ChildrenName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;

                            if (isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.DateOnList;
                                workSheet.Cell(ReportRow, 7).Value = item.ProgramType;
                                workSheet.Cell(ReportRow, 8).Value = item.SelectionPoints;
                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.DateOnList;
                                workSheet.Cell(ReportRow, 6).Value = item.ProgramType;
                                workSheet.Cell(ReportRow, 7).Value = item.SelectionPoints;
                            }

                            ReportRow++;
                        }
                        break;

                    case "#WaitingModal":
                        workSheet = wb.Worksheets.Add("Waiting Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value = (isAllCenter) ? "Waiting Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy") : "Waiting Client Report for " + infoClass.WaitingChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if (isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 6).Value = "Date on List";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 7).Value = "Center Choice";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;

                            workSheet.Cell(ReportRow, 8).Value = "Program Type";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 20;

                            workSheet.Cell(ReportRow, 9).Value = "Selection Points";
                            workSheet.Cell(ReportRow, 9).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 9).WorksheetColumn().Width = 15;
                        }
                        else
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Date on List";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 6).Value = "Center Choice";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 7).Value = "Program Type";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 8).Value = "Selection Points";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 15;
                        }


                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.WaitingChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ChildrenName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;

                            if (isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.DateOnList;
                                workSheet.Cell(ReportRow, 7).Value = item.CenterChoice;
                                workSheet.Cell(ReportRow, 8).Value = item.ProgramType;
                                workSheet.Cell(ReportRow, 9).Value = item.SelectionPoints;

                            }
                            else
                            {

                                workSheet.Cell(ReportRow, 5).Value = item.DateOnList;
                                workSheet.Cell(ReportRow, 6).Value = item.CenterChoice;
                                workSheet.Cell(ReportRow, 7).Value = item.ProgramType;
                                workSheet.Cell(ReportRow, 8).Value = item.SelectionPoints;
                            }

                            ReportRow++;
                        }
                        break;

                    case "#ReturningModal":
                        workSheet = wb.Worksheets.Add("Returning Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value =(isAllCenter) ? "Returning Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy"): "Returning Client Report for " + infoClass.ReturningList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if(isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 6).Value = "Start Date";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 7).Value = "Program Type";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                        }
                        else
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 6).Value = "Program Type";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                        }

                       
                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.ReturningList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ClientName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;

                            if(isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.ClassStartDate;
                                workSheet.Cell(ReportRow, 7).Value = item.ProgramType;
                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.ClassStartDate;
                                workSheet.Cell(ReportRow, 6).Value = item.ProgramType;
                            }

                           
                            ReportRow++;
                        }
                        break;

                    case "#GraduatingModal":
                        workSheet = wb.Worksheets.Add("Graduating Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value =(isAllCenter) ? "Graduating Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy"): "Graduating Client Report for " + infoClass.GraduatingList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;
                        if(isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 6).Value = "Start Date";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 7).Value = "Program Type";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                        }
                        else
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Start Date";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 6).Value = "Program Type";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                        }

                       
                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.GraduatingList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ClientName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;

                            if(isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.ClassStartDate;
                                workSheet.Cell(ReportRow, 7).Value = item.ProgramType;

                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.ClassStartDate;
                                workSheet.Cell(ReportRow, 6).Value = item.ProgramType;
                            }
                            
                            ReportRow++;
                        }
                        break;

                    case "#OverIncomeModal":
                        workSheet = wb.Worksheets.Add("Over Income Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value =(isAllCenter) ? "Over Income Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy"): "Over Income Client Report for " + infoClass.OverIncomeChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if(isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 6).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 7).Value = "Parent/Guardian";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;

                            workSheet.Cell(ReportRow, 8).Value = "Income Percentage";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 20;
                        }
                        else
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                            workSheet.Cell(ReportRow, 6).Value = "Parent/Guardian";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 20;
                            workSheet.Cell(ReportRow, 7).Value = "Income Percentage";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 20;
                        }
                 
                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.OverIncomeChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ClientName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;

                            if(isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.ClassStartDate;

                                item.ParentName = "";
                                if (parentList.Count() > 0)
                                {
                                    string[] parentArr = parentList.Where(x => x.Value == EncryptDecrypt.Decrypt64(item.Enc_ClientId)).Select(x => x.Text).ToArray();
                                    if (parentArr.Count() > 0)
                                    {
                                        item.ParentName = String.Join(",", parentArr);
                                    }
                                }
                                workSheet.Cell(ReportRow, 7).Value = item.ParentName;
                                workSheet.Cell(ReportRow, 8).Value = item.ChildIncome;

                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.ClassStartDate;
                                item.ParentName = "";
                                if (parentList.Count() > 0)
                                {
                                    string[] parentArr = parentList.Where(x => x.Value == EncryptDecrypt.Decrypt64(item.Enc_ClientId)).Select(x => x.Text).ToArray();
                                    if (parentArr.Count() > 0)
                                    {
                                        item.ParentName = String.Join(",", parentArr);
                                    }
                                }
                                workSheet.Cell(ReportRow, 6).Value = item.ParentName;
                                workSheet.Cell(ReportRow, 7).Value = item.ChildIncome;
                            }

                        
                            ReportRow++;
                        }
                        break;

                    case "#FosterModal":
                        workSheet = wb.Worksheets.Add("Foster Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value =(isAllCenter) ? "Foster Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy"): "Foster Client Report for " + infoClass.FosterChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if(isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            //workSheet.Cell(ReportRow, 6).Value = "Attachment";
                            //workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            //workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;
                        }
                        //else
                        //{
                        //    workSheet.Cell(ReportRow, 5).Value = "Attachment";
                        //    workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                        //    workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                        //}

                   
                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.FosterChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ClientName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;
                            if(isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                               // workSheet.Cell(ReportRow, 6).Value = item.FileAttached;


                            }
                            //else
                            //{
                            //    workSheet.Cell(ReportRow, 5).Value = item.FileAttached;

                            //}
                            ReportRow++;
                        }
                        break;

                    case "#HomeLessModal":
                        workSheet = wb.Worksheets.Add("Homeless Client Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value =(isAllCenter) ? "Homeless Client Report as on " + DateTime.Now.ToString("MM/dd/yyyy"): "Homeless Client Report for " + infoClass.HomeLessChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;

                        if(isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Center Name";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 6).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;
                        }
                        else
                        {
                            workSheet.Cell(ReportRow, 5).Value = "Class Start Date";
                            workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                        }
              
                        ReportRow = ReportRow + 1;

                        foreach (var item in infoClass.HomeLessChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ChildrenName;
                            workSheet.Cell(ReportRow, 3).Value = (item.Gender == "1") ? "Male" : (item.Gender == "2") ? "Female" : "Others";
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;
                            if (isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 6).Value = item.ClassStartDate;


                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 5).Value = item.ClassStartDate;
                            }
                           
                            ReportRow++;
                        }
                        break;

                    case "#ExternalLeadsModal":
                        workSheet = wb.Worksheets.Add("External Leads Report");
                        workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range("B1:H1").Merge().Value = "External Leads Report for " + infoClass.LeadsChildrenList[0].CenterName + " as on " + DateTime.Now.ToString("MM/dd/yyyy");
                        workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        workSheet.Range("B1:H1").Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 1).Value = "Client ID";
                        workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).Value = "Client Name";
                        workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                        workSheet.Cell(ReportRow, 3).Value = "Gender";
                        workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).Value = "Date of Birth";
                        workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;
                        workSheet.Cell(ReportRow, 5).Value = "Parent/Guardian";
                        workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                        workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;

                        if(isAllCenter)
                        {
                            workSheet.Cell(ReportRow, 6).Value = "Center Name";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 7).Value = "FSW Assigned";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 8).Value = "Contacted by FSW";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 9).Value = "Disability";
                            workSheet.Cell(ReportRow, 9).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 9).WorksheetColumn().Width = 15;
                        }
                        else
                        {
                            workSheet.Cell(ReportRow, 6).Value = "FSW Assigned";
                            workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 7).Value = "Contacted by FSW";
                            workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 15;

                            workSheet.Cell(ReportRow, 8).Value = "Disability";
                            workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                            workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 15;
                        }
                    
                        ReportRow = ReportRow + 1;
                        //                        myExcelWorksheet.Columns[

                        //"X:Z"].HorizontalAlignment = HorizontalAlignment.Center;


                        foreach (var item in infoClass.LeadsChildrenList)
                        {
                            workSheet.Cell(ReportRow, 1).Value = EncryptDecrypt.Decrypt64(item.Enc_ClientId);
                            workSheet.Cell(ReportRow, 2).Value = item.ChildrenName;
                            workSheet.Cell(ReportRow, 3).Value = item.Gender;
                            workSheet.Cell(ReportRow, 4).Value = item.Dob;
                            workSheet.Cell(ReportRow, 5).Value = item.ParentName;

                            if(isAllCenter)
                            {
                                workSheet.Cell(ReportRow, 6).Value = item.CenterName;
                                workSheet.Cell(ReportRow, 7).Value = item.FSWName;
                                workSheet.Cell(ReportRow, 8).Value = item.ContactStatus;
                                workSheet.Cell(ReportRow, 9).Value = item.Disability;
                            }
                            else
                            {
                                workSheet.Cell(ReportRow, 6).Value = item.FSWName;
                                workSheet.Cell(ReportRow, 7).Value = item.ContactStatus;
                                workSheet.Cell(ReportRow, 8).Value = item.Disability;

                            }

                            ReportRow++;
                        }
                        break;
                    default:
                        workSheet = wb.Worksheets.Add("Default Client Report");
                        break;
                }

                wb.SaveAs(memoryStream);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return memoryStream;
        }


        public MemoryStream ExportInkindReport(InkindReportModel inkindReportModel)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {

                XLWorkbook wb = new XLWorkbook();
                IXLWorksheet workSheet = null;
             

                workSheet = wb.Worksheets.Add("In-Kind Report");
                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                if(inkindReportModel.FilterTypeEnum!=InkindReportFilterEnum.DateEntered)
                {
                    workSheet.Range("B1:H1").Merge().Value = "In-Kind Report based on " + EnumHelper.GetEnumDescription(inkindReportModel.FilterTypeEnum) + " " + "dates from" + " " + inkindReportModel.FromDate + " " + "to " + inkindReportModel.ToDate;

                }
                else
                {
                    workSheet.Range("B1:H1").Merge().Value = "In-Kind Report based on " + EnumHelper.GetEnumDescription(inkindReportModel.FilterTypeEnum) + " " + inkindReportModel.DateEntered;

                }
                workSheet.Range("B1:H1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                workSheet.Range("B1:H1").Style.Font.SetBold(true);





                //In-Kind Report Total Count Legends//


              

                workSheet.Cell(3, 6).Value = "Total Hours";
                workSheet.Cell(3, 6).Style.Font.SetBold(true);
                workSheet.Cell(3, 6).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AshGrey;
                workSheet.Cell(3, 6).Style.Font.FontColor = ClosedXML.Excel.XLColor.Black;
             

                workSheet.Cell(3, 7).Value = inkindReportModel.TotalHours;
                workSheet.Cell(3, 7).Style.Font.SetBold(true);
                workSheet.Cell(3, 7).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AshGrey;
                workSheet.Cell(3, 7).Style.Font.FontColor = ClosedXML.Excel.XLColor.Black;


                workSheet.Cell(4, 6).Value = "Total Miles Driven";
                workSheet.Cell(4, 6).Style.Font.SetBold(true);
                workSheet.Cell(4, 6).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AshGrey;
                workSheet.Cell(4, 6).Style.Font.FontColor = ClosedXML.Excel.XLColor.Black;

                workSheet.Cell(4, 7).Value = inkindReportModel.TotalMiles;
                workSheet.Cell(4, 7).Style.Font.SetBold(true);
                workSheet.Cell(4, 7).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AshGrey;
                workSheet.Cell(4, 7).Style.Font.FontColor = ClosedXML.Excel.XLColor.Black;


                workSheet.Cell(5, 6).Value = "Total In-Kind Amount";
                workSheet.Cell(5, 6).Style.Font.SetBold(true);
                workSheet.Cell(5, 6).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AshGrey;
                workSheet.Cell(5, 6).Style.Font.FontColor = ClosedXML.Excel.XLColor.Black;
                               
                workSheet.Cell(5, 7).Value = inkindReportModel.TotalAmount;
                workSheet.Cell(5, 7).Style.Font.SetBold(true);
                workSheet.Cell(5, 7).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AshGrey;
                workSheet.Cell(5, 7).Style.Font.FontColor = ClosedXML.Excel.XLColor.Black;


                workSheet.Cell(6, 6).Value = "Total Record(s)";
                workSheet.Cell(6, 6).Style.Font.SetBold(true);
                workSheet.Cell(6, 6).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AshGrey;
                workSheet.Cell(6, 6).Style.Font.FontColor = ClosedXML.Excel.XLColor.Black;
                               
                workSheet.Cell(6, 7).Value = inkindReportModel.TotalRecord;
                workSheet.Cell(6, 7).Style.Font.SetBold(true);
                workSheet.Cell(6, 7).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.AshGrey;
                workSheet.Cell(6, 7).Style.Font.FontColor = ClosedXML.Excel.XLColor.Black;

                int ReportRow = 8;

                //In-Kind Report Total Count Legends//

                workSheet.Cell(ReportRow, 1).Value = "Contributor";
                workSheet.Cell(ReportRow, 1).Style.Font.SetBold(true);
                workSheet.Cell(ReportRow, 1).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                workSheet.Cell(ReportRow, 1).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                workSheet.Cell(ReportRow, 2).Value = "Center";
                workSheet.Cell(ReportRow, 2).Style.Font.SetBold(true);
                workSheet.Cell(ReportRow, 2).WorksheetColumn().Width = 30;
                workSheet.Cell(ReportRow, 2).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                workSheet.Cell(ReportRow, 2).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                workSheet.Cell(ReportRow, 3).Value = "Contributor Activity";
                workSheet.Cell(ReportRow, 3).WorksheetColumn().Width = 15;
                workSheet.Cell(ReportRow, 3).Style.Font.SetBold(true);
                workSheet.Cell(ReportRow, 3).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                workSheet.Cell(ReportRow, 3).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                workSheet.Cell(ReportRow, 4).Value = "Activity Date";
                workSheet.Cell(ReportRow, 4).Style.Font.SetBold(true);
                workSheet.Cell(ReportRow, 4).WorksheetColumn().Width = 15;
                workSheet.Cell(ReportRow, 4).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                workSheet.Cell(ReportRow, 4).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;


                workSheet.Cell(ReportRow, 5).Value = "Time Spent";
                workSheet.Cell(ReportRow, 5).Style.Font.SetBold(true);
                workSheet.Cell(ReportRow, 5).WorksheetColumn().Width = 15;
                workSheet.Cell(ReportRow, 5).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                workSheet.Cell(ReportRow, 5).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                workSheet.Cell(ReportRow, 6).Value = "Miles Driven";
                workSheet.Cell(ReportRow, 6).Style.Font.SetBold(true);
                workSheet.Cell(ReportRow, 6).WorksheetColumn().Width = 15;
                workSheet.Cell(ReportRow, 6).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                workSheet.Cell(ReportRow, 6).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;


                workSheet.Cell(ReportRow, 7).Value = "In-Kind Amount";
                workSheet.Cell(ReportRow, 7).Style.Font.SetBold(true);
                workSheet.Cell(ReportRow, 7).WorksheetColumn().Width = 15;
                workSheet.Cell(ReportRow, 7).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                workSheet.Cell(ReportRow, 7).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;


                workSheet.Cell(ReportRow, 8).Value = "Entered By";
                workSheet.Cell(ReportRow, 8).Style.Font.SetBold(true);
                workSheet.Cell(ReportRow, 8).WorksheetColumn().Width = 15;
                workSheet.Cell(ReportRow, 8).Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.BlueGray;
                workSheet.Cell(ReportRow, 8).Style.Font.FontColor = ClosedXML.Excel.XLColor.White;



                ReportRow = ReportRow + 1;

                foreach (var item in inkindReportModel.InkindReportList)
                {
                    workSheet.Cell(ReportRow, 1).Value = item.Name;
                    workSheet.Cell(ReportRow, 2).Value = item.CenterName;
                    workSheet.Cell(ReportRow, 3).Value = item.ActivityDescription;
                    workSheet.Cell(ReportRow, 4).Value = item.ActivityDate;
                    workSheet.Cell(ReportRow, 5).Value = string.Concat(item.Hours, " ", "Hours", " ", item.Minutes, " ", "Minutes");
                    workSheet.Cell(ReportRow, 6).Value = item.MilesDriven;
                    workSheet.Cell(ReportRow, 7).Value = string.Concat("$"," ", item.InKindAmount);
                    workSheet.Cell(ReportRow, 8).Value = item.StaffEntered;
                    ReportRow++;
                }

                wb.SaveAs(memoryStream);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return memoryStream;
        }

        public MemoryStream ExportCaseNoteByTagId(List<CaseNote> cnlist, long tagid, long total, string tname)
        {

            MemoryStream memoryStream = new MemoryStream();
            try
            {
               // List<List<string>> screening = ScreeningMatrix.Screenings;

                XLWorkbook wb = new XLWorkbook();

                if (!string.IsNullOrEmpty(tname) && tname.Length > 23)
                {

                    tname = tname.Substring(0, 20);
                    tname = tname + "...";

                }
                //var Ws = wb.Worksheets.Add("Case Note report-"+tname+""+total+")");
                var Ws = wb.Worksheets.Add(tname+"("+total+")");



                //Header
                Ws.Cell(1,1).Value = "Created By";
                Ws.Cell(1,2).Value = "Title";
                Ws.Cell(1,3).Value = "Created Date";
                Ws.Range("A1:H1").Style.Font.SetBold(true);

               

                int CurrentRow = 2;
                foreach (var item in cnlist)
                {

                    Ws.Cell(CurrentRow, 1).Value = item.WrittenBy;
                    Ws.Cell(CurrentRow, 2).Value = item.Name;
                    Ws.Cell(CurrentRow, 3).Value = item.Date;
                    CurrentRow++;
                }

                Ws.Column(1).AdjustToContents();
                Ws.Column(2).AdjustToContents();
                Ws.Column(3).AdjustToContents();

                wb.SaveAs(memoryStream);
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
            }

            return memoryStream;

            }


    }

    public class TwoColumnHeaderFooter : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // we will put the final number of pages in a template
        PdfTemplate template;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        #region Properties
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private Image _HeaderLeft;
        public Image HeaderLeft
        {
            get { return _HeaderLeft; }
            set { _HeaderLeft = value; }
        }
        private string _HeaderRight;
        public string HeaderRight
        {
            get { return _HeaderRight; }
            set { _HeaderRight = value; }
        }
        private Font _HeaderFont;
        public Font HeaderFont
        {
            get { return _HeaderFont; }
            set { _HeaderFont = value; }
        }
        private Font _FooterFont;
        public Font FooterFont
        {
            get { return _FooterFont; }
            set { _FooterFont = value; }
        }
        #endregion
        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
                clsError.WriteException(de);
            }
            catch (System.IO.IOException ioe)
            { 
             clsError.WriteException(ioe);
            }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            Rectangle pageSize = document.PageSize;
            if (Title != string.Empty)
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 15);
                cb.SetRGBColorFill(50, 50, 200);
                cb.SetTextMatrix(pageSize.GetLeft(210), pageSize.GetTop(40));
                cb.ShowText(Title);
                cb.EndText();
            }
            if ( HeaderRight != string.Empty)
            {
                PdfPTable HeaderTable = new PdfPTable(2);
                HeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                HeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                HeaderTable.TotalWidth = pageSize.Width - 80;
                HeaderTable.SetWidthPercentage(new float[] { 45, 45 }, pageSize);

                PdfPCell HeaderLeftCell = new PdfPCell(HeaderLeft);
                HeaderLeftCell.Padding = 5;
                HeaderLeftCell.Border = Rectangle.NO_BORDER;
                HeaderLeftCell.PaddingBottom = 8;
                HeaderLeftCell.BorderWidthRight = 0;
                HeaderTable.AddCell(HeaderLeftCell);
                PdfPCell HeaderRightCell = new PdfPCell(new Phrase(8, HeaderRight, HeaderFont));
                HeaderRightCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                HeaderRightCell.Padding = 5;
                HeaderRightCell.PaddingBottom = 8;
                HeaderRightCell.Border = Rectangle.NO_BORDER;
                HeaderRightCell.BorderWidthLeft = 0;
                HeaderTable.AddCell(HeaderRightCell);
                cb.SetRGBColorFill(0, 0, 0);
                HeaderTable.WriteSelectedRows(0, -1, pageSize.GetLeft(40), pageSize.GetTop(50), cb);
            }
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            Rectangle pageSize = document.PageSize;
            //show page no in right side of the footer
            //String text = "";
            //float len = bf.GetWidthPoint(text, 8);
            
            //cb.SetRGBColorFill(100, 100, 100);
            //cb.BeginText();
            //cb.SetFontAndSize(bf, 8);
            //cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(30));
            ////cb.ShowText(text);
            //cb.EndText();
            //cb.AddTemplate(template, pageSize.GetLeft(40) + len, pageSize.GetBottom(30));

            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
                "powered by: GEFingerPrints™  Copyright 2016, 2017 " ,
                pageSize.GetLeft(315),
                pageSize.GetBottom(30), 0);
            cb.EndText();
        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            template.ShowText("");
            template.EndText();
        }
    }
}
