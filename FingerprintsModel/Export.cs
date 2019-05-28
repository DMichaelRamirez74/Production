using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
using System.Data;
using System.Web.Mvc;
using iTextSharp.text.pdf.languages;


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


        public MemoryStream ExportInkindReport(InkindReportModel inkindReportModel, string imagePath, FingerprintsModel.Enums.ReportFormatType reportFormat)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {


                #region PDF Report


                if(reportFormat==Enums.ReportFormatType.Pdf)
                {
                     // Sets the maximum rows per page //
                    Int32 colCount = 7; // column count //


                    Document doc = new Document();

                    doc.SetMargins(50f, 50f, 50f, 50f); // Defines margins of the page //

                    //Create PDF Table  


                    var writer = PdfWriter.GetInstance(doc, memoryStream);
                    writer.CloseStream = false;
                    doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());



                    doc.OpenDocument();

                    if (inkindReportModel != null && inkindReportModel.InkindReportList.Count > 0)
                    {

                        var centerList = inkindReportModel.InkindReportList.Select(x => x.CenterID).Distinct().ToList();

                        for (int i = 0; i < centerList.Count; i++)
                        {



                            PdfPTable tableLayout = new PdfPTable(colCount);
                            tableLayout.HeaderRows = 3;

                            //Add Content to PDF   
                            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  


                            if (colCount == 7)
                            {
                                float[] headers = { 30, 30, 30, 20, 30, 30,30 };
                                tableLayout.SetWidths(headers); //Set the pdf headers
                            }

                            if (i > 0)
                            {
                                doc.NewPage();

                            }



                            var inkindWithCenterList = inkindReportModel.InkindReportList.Where(x => x.CenterID == centerList[i]).ToList();



                            #region Adding Headers

                            #region Adding Star Rating image with Center Name

                            Paragraph p = new Paragraph(inkindWithCenterList[0].CenterName, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)));



                            string starImageUrl = "";

                            // Star Rating Image URL //
                            starImageUrl = imagePath + "\\220px-Star_rating_" + inkindWithCenterList[0].StepUpToQualityStars + "_of_5.png";


                            // starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";


                            iTextSharp.text.Image starJpeg = iTextSharp.text.Image.GetInstance(starImageUrl);

                            //Resize image depend upon your need

                            starJpeg.ScaleToFit(40f, 40f);

                            //Give space before image

                            starJpeg.SpacingBefore = 10f;

                            //Give some space after the image

                            starJpeg.SpacingAfter = 10f;

                            starJpeg.Alignment = Element.ALIGN_LEFT;

                            p.Add(new Chunk(starJpeg, 20 * 2, 0, true));

                            PdfPCell cell = new PdfPCell(p);
                            cell.Colspan = colCount;
                            cell.Padding = 5;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cell.VerticalAlignment = 1;
                            tableLayout.AddCell(cell);


                            #endregion

                            string headingName = string.Empty;
                            if (inkindReportModel.FilterTypeEnum != FingerprintsModel.Enums.InkindReportFilter.DateEntered)
                            {

                                headingName = "In-Kind Report based on " + EnumHelper.GetEnumDescription(inkindReportModel.FilterTypeEnum) + " " + inkindReportModel.DateEntered;
                            }

                            else if (inkindReportModel.FilterTypeEnum != FingerprintsModel.Enums.InkindReportFilter.EnteredBy)
                            {
                                headingName = "In-Kind Report based on " + EnumHelper.GetEnumDescription(inkindReportModel.FilterTypeEnum);
                            }

                            else
                            {
                                headingName = "In-Kind Report based on " + EnumHelper.GetEnumDescription(inkindReportModel.FilterTypeEnum) + " " + "dates from" + " " + inkindReportModel.FromDate + " " + "to " + inkindReportModel.ToDate;
                            }



                            tableLayout.AddCell(new PdfPCell(new Phrase(headingName, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Colspan = colCount,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            });

                            #endregion


                            ////Add header 

                            #region Table Headers

                            tableLayout.AddCell(new PdfPCell(new Phrase("Contributor", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase("Contributor Activity", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });



                            tableLayout.AddCell(new PdfPCell(new Phrase("Activity Date", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Time Spent", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Miles Driven", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("In-Kind Amount", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Entered By", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                           

                            #endregion



                            #region Table Rows
                            for (int j = 0; j < inkindWithCenterList.Count; j++)
                            {


                                tableLayout.AddCell(new PdfPCell(new Phrase(inkindWithCenterList[j].Name, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(inkindWithCenterList[j].ActivityDescription, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(inkindWithCenterList[j].ActivityDate, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(string.Concat(inkindWithCenterList[j].Hours, " ", "Hours", " ", inkindWithCenterList[j].Minutes, " ", "Minutes"), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(inkindWithCenterList[j].MilesDriven.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                tableLayout.AddCell(new PdfPCell(new Phrase(string.Concat("$", " ", inkindWithCenterList[j].InKindAmount), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(inkindWithCenterList[j].StaffEntered, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                });





                            }


                            #endregion

                            doc.Add(tableLayout);

                        }
                    }

                    doc.CloseDocument();


                }
                #endregion

                #region Excel Report
                else if (reportFormat == Enums.ReportFormatType.Xls)
                {
                    XLWorkbook wb = new XLWorkbook();
                    

                  


                    if (inkindReportModel.InkindReportList != null && inkindReportModel.InkindReportList.Count() > 0)
                    {
                        var centerList = inkindReportModel.InkindReportList.Select(x => x.CenterID).Distinct().ToList();


                        #region Splitting records to worksheets based on Centers

                        for (int i = 0; i < centerList.Count(); i++)
                        {


                            int inititalRow = 5;

                            int ReportRow = inititalRow;
                            int Reportcolumn = 2;

                            #region Adding Worksheet

                            var inkindWithCenterList = inkindReportModel.InkindReportList.Where(x => x.CenterID == centerList[i]).ToList();

                            //var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();

                            var centerName = inkindWithCenterList.Select(x => x.CenterName).First();
                            var qualityStars = inkindWithCenterList.Select(x => x.StepUpToQualityStars).First();

                            var vs = wb.Worksheets.Add(centerName.Length > 31 ? centerName.Substring(0, 15) : centerName);

                            #region Headers with Quality Stars

                            string starImageUrl = imagePath + "\\220px-Star_rating_" + inkindWithCenterList[0].StepUpToQualityStars + "_of_5.png";

                            System.Drawing.Bitmap fullImage = new System.Drawing.Bitmap(starImageUrl);

                            vs.AddPicture(fullImage).MoveTo(vs.Cell("F2"), new System.Drawing.Point(100, 1)).Scale(0.3);// optional: resize picture

                            vs.Range("F2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("F2").Style.Font.SetBold(true);
                            vs.Range("F2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            vs.Range("B2:C2").Merge();
                            vs.Range("B2:C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B2:C2").Style.Font.SetBold(true);
                            vs.Range("B2:C2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            vs.Range("G2:H2").Merge();
                            vs.Range("G2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("G2:H2").Style.Font.SetBold(true);
                            vs.Range("G2:H2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            vs.Range("D2:E2").Merge().Value = centerName;
                            vs.Range("D2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("D2:E2").Style.Font.SetBold(true);
                            vs.Range("D2:E2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            if (inkindReportModel.FilterTypeEnum != FingerprintsModel.Enums.InkindReportFilter.DateEntered)
                            {

                                vs.Range("B3:H3").Merge().Value = "In-Kind Report based on " + EnumHelper.GetEnumDescription(inkindReportModel.FilterTypeEnum) + " " + inkindReportModel.DateEntered;
                            }

                            else if (inkindReportModel.FilterTypeEnum != FingerprintsModel.Enums.InkindReportFilter.EnteredBy)
                            {
                                vs.Range("B3:H3").Merge().Value = "In-Kind Report based on " + EnumHelper.GetEnumDescription(inkindReportModel.FilterTypeEnum);
                            }

                            else
                            {
                                vs.Range("B3:H3").Merge().Value = "In-Kind Report based on " + EnumHelper.GetEnumDescription(inkindReportModel.FilterTypeEnum) + " " + "dates from" + " " + inkindReportModel.FromDate + " " + "to " + inkindReportModel.ToDate;
                            }

                            vs.Range("B3:H3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B3:H3").Style.Font.SetBold(true);
                            vs.Range("B3:H3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            #endregion

                            #region Table Headers


                            vs.Cell(4, 2).Value = "Contributor";
                            vs.Cell(4, 2).Style.Font.SetBold(true);
                            vs.Cell(4, 2).WorksheetColumn().Width = 30;

                            vs.Cell(4, 3).Value = "Contributor Activity";
                            vs.Cell(4, 3).Style.Font.SetBold(true);
                            vs.Cell(4, 3).WorksheetColumn().Width = 30;

                            vs.Cell(4, 4).Value = "Activity Date";
                            vs.Cell(4, 4).Style.Font.SetBold(true);
                            vs.Cell(4, 4).WorksheetColumn().Width = 30;

                            vs.Cell(4, 5).Value = "Time Spent";
                            vs.Cell(4, 5).Style.Font.SetBold(true);
                            vs.Cell(4, 5).WorksheetColumn().Width = 30;

                            vs.Cell(4, 6).Value = "Miles Driven";
                            vs.Cell(4, 6).Style.Font.SetBold(true);
                            vs.Cell(4, 6).WorksheetColumn().Width = 30;

                            vs.Cell(4, 7).Value = "In-Kind Amount";
                            vs.Cell(4, 7).Style.Font.SetBold(true);
                            vs.Cell(4, 7).WorksheetColumn().Width = 30;

                            vs.Cell(4, 8).Value = "Entered By";
                            vs.Cell(4, 8).Style.Font.SetBold(true);
                            vs.Cell(4, 8).WorksheetColumn().Width = 30;

                            vs.Range("B4:H4").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            vs.Range("B4:H4").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                            vs.Range("B4:H4").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                            #endregion

                            #region Table Rows

                            for (int j = 0; j < inkindWithCenterList.Count; j++)
                            {


                                vs.Cell(ReportRow, Reportcolumn).Value = inkindWithCenterList[j].Name;
                                vs.Cell(ReportRow, Reportcolumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                vs.Cell(ReportRow, Reportcolumn+1).Value = inkindWithCenterList[j].ActivityDescription;
                                vs.Cell(ReportRow, Reportcolumn+1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn+1).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn+1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                vs.Cell(ReportRow, Reportcolumn+2).Value = inkindWithCenterList[j].ActivityDate;
                                vs.Cell(ReportRow, Reportcolumn+2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn+2).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn+2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                vs.Cell(ReportRow, Reportcolumn+3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn+3).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn+3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Cell(ReportRow, Reportcolumn+3).Value = string.Concat(inkindWithCenterList[j].Hours, " ", "Hours", " ", inkindWithCenterList[j].Minutes, " ", "Minutes");

                                vs.Cell(ReportRow, Reportcolumn+4).Value = inkindWithCenterList[j].MilesDriven;
                                vs.Cell(ReportRow, Reportcolumn+4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn+4).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn+4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                vs.Cell(ReportRow, Reportcolumn+5).Value = string.Concat("$", " ", inkindWithCenterList[j].InKindAmount);
                                vs.Cell(ReportRow, Reportcolumn+5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn+5).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn+5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                vs.Cell(ReportRow, Reportcolumn+6).Value = inkindWithCenterList[j].StaffEntered;
                                vs.Cell(ReportRow, Reportcolumn+6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn+6).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn+6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                
                                ReportRow++;
                            }
                            #endregion
                            vs.Range("B4:H4").SetAutoFilter();

                           
                        }

                        #endregion
                        #endregion

                    }

                    wb.SaveAs(memoryStream);
                }
                #endregion

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



        public MemoryStream ExportScreeningMatrixReportPdf(ScreeningMatrixReport screeningMatrixReport, string imagePath)
        {

            MemoryStream workStream = new MemoryStream();
            try
            {

                int maxLinesPerPage = 33; // Sets the maximum rows per page //
                Int32 colCount = 6; // column count //


                Document doc = new Document();

                doc.SetMargins(50f, 50f, 50f, 50f); // Defines margins of the page //

                //Create PDF Table  


                var writer = PdfWriter.GetInstance(doc, workStream);
                writer.CloseStream = false;
                doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());



                doc.OpenDocument();

                if (screeningMatrixReport != null && screeningMatrixReport.ScreeningMatrix.Count > 0)
                {

                    var centerList = screeningMatrixReport.ScreeningMatrix.Select(x => x.CenterID).Distinct().ToList();

                    for (int i = 0; i < centerList.Count; i++)
                    {



                        PdfPTable tableLayout = new PdfPTable(colCount);
                        tableLayout.HeaderRows = 3;

                        //Add Content to PDF   
                        tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  


                        if (colCount == 6)
                        {
                            float[] headers = { 30, 30, 30, 20, 30, 30 };
                            tableLayout.SetWidths(headers); //Set the pdf headers
                        }

                        if (i > 0)
                        {
                            doc.NewPage();

                        }



                        var screeningWithCenterList = screeningMatrixReport.ScreeningMatrix.Where(x => x.CenterID == centerList[i]).ToList();

                        var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();


                        #region Adding Headers

                        #region Adding Star Rating image with Center Name

                        Paragraph p = new Paragraph(screeningWithCenterList[0].CenterName, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)));



                        string starImageUrl = "";

                        // Starr Rating Image URL //
                           starImageUrl = imagePath + "\\220px-Star_rating_" + screeningWithCenterList[0].StepUpToQualityStars + "_of_5.png";


                        // starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";


                        iTextSharp.text.Image starJpeg = iTextSharp.text.Image.GetInstance(starImageUrl);

                        //Resize image depend upon your need

                        starJpeg.ScaleToFit(40f, 40f);

                        //Give space before image

                        starJpeg.SpacingBefore = 10f;

                        //Give some space after the image

                        starJpeg.SpacingAfter = 10f;

                        starJpeg.Alignment = Element.ALIGN_LEFT;

                        p.Add(new Chunk(starJpeg, 20 * 2, 0, true));

                        PdfPCell cell = new PdfPCell(p);
                        cell.Colspan = colCount;
                        cell.Padding = 5;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell.VerticalAlignment = 1;
                        tableLayout.AddCell(cell);


                        #endregion


                        tableLayout.AddCell(new PdfPCell(new Phrase("Screening Matrix Report", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                        {
                            Colspan = colCount,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 5
                        });

                        #endregion


                        ////Add header 

                        #region Table Headers

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

                        #endregion



                        #region Table Rows
                        for (int j = 0; j < classroomList.Count; j++)
                        {
                            var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();



                            for (int k = 0; k < screeningList.Count; k++)

                            {
                                var rowSpan = screeningList.Count();


                                if (tableLayout.Rows.Count == maxLinesPerPage && k > 0)
                                {
                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        Rowspan = (rowSpan - k),
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                }


                                if (k == 0)
                                {
                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        Rowspan = ((tableLayout.Rows.Count + rowSpan <= maxLinesPerPage)) ? rowSpan : (tableLayout.Rows.Count + rowSpan > maxLinesPerPage) ? rowSpan - ((tableLayout.Rows.Count + rowSpan) - maxLinesPerPage) : rowSpan,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                }




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
                            }




                        }


                        #endregion

                        var uptoDate = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.UptoDate);
                        var missing = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.Missing);
                        var expired = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.Expired);
                        var expiring = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.Expiring);

                        #region Total Calculation
                        tableLayout.AddCell(new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT,
                            Padding = 3,
                            Colspan = 2,

                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)

                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase(uptoDate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase(missing.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase(expired.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                        });

                        tableLayout.AddCell(new PdfPCell(new Phrase(expiring.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            Padding = 3,
                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                        });

                        #endregion

                        doc.Add(tableLayout);

                    }
                }

                doc.CloseDocument();

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return workStream;
        }



        public MemoryStream ExportScreeningMatrixReportExcel(ScreeningMatrixReport screeningMatrixReport, string imagePath)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {


                XLWorkbook wb = new XLWorkbook();


                if (screeningMatrixReport.ScreeningMatrix != null && screeningMatrixReport.ScreeningMatrix.Count > 0)
                {


                    var centerList = screeningMatrixReport.ScreeningMatrix.Select(x => x.CenterID).Distinct().ToList();
                    for (int i = 0; i < centerList.Count; i++)
                    {


                        #region Adding Worksheet

                        var screeningWithCenterList = screeningMatrixReport.ScreeningMatrix.Where(x => x.CenterID == centerList[i]).ToList();

                        var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();

                        var centerName = screeningWithCenterList.Select(x => x.CenterName).First();
                        var qualityStars = screeningWithCenterList.Select(x => x.StepUpToQualityStars).First();

                        var vs = wb.Worksheets.Add(centerName.Length > 31 ? centerName.Substring(0, 15) : centerName);

                        #region Headers with Quality Stars

                        string starImageUrl = imagePath + "\\220px-Star_rating_" + screeningWithCenterList[0].StepUpToQualityStars + "_of_5.png";

                       // string starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";
                       


                        System.Drawing.Bitmap fullImage = new System.Drawing.Bitmap(starImageUrl);


                        vs.AddPicture(fullImage).MoveTo(vs.Cell("F2"), new System.Drawing.Point(100, 1)).Scale(0.3);// optional: resize picture



                        vs.Range("B2:C2").Merge();
                        vs.Range("B2:C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        vs.Range("B2:C2").Style.Font.SetBold(true);
                        vs.Range("B2:C2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                        vs.Range("F2:G2").Merge();
                        vs.Range("F2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        vs.Range("F2:G2").Style.Font.SetBold(true);
                        vs.Range("F2:G2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                        vs.Range("D2:E2").Merge().Value = centerName;
                        vs.Range("D2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        vs.Range("D2:E2").Style.Font.SetBold(true);
                        vs.Range("D2:E2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                        vs.Range("B3:G3").Merge().Value = "Screening Matrix Report";
                        vs.Range("B3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        vs.Range("B3:G3").Style.Font.SetBold(true);
                        vs.Range("B3:G3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        #endregion



                        #region Table Headers

                        vs.Cell(4, 2).Value = "Classroom";
                        vs.Cell(4, 2).Style.Font.SetBold(true);
                        vs.Cell(4, 2).WorksheetColumn().Width = 30;

                        vs.Cell(4, 3).Value = "Screening";
                        vs.Cell(4, 3).Style.Font.SetBold(true);
                        vs.Cell(4, 3).WorksheetColumn().Width = 30;

                        vs.Cell(4, 4).Value = "Up-to-Date";
                        vs.Cell(4, 4).Style.Font.SetBold(true);
                        vs.Cell(4, 4).WorksheetColumn().Width = 30;

                        vs.Cell(4, 5).Value = "Missing";
                        vs.Cell(4, 5).Style.Font.SetBold(true);
                        vs.Cell(4, 5).WorksheetColumn().Width = 30;

                        vs.Cell(4, 6).Value = "Expired";
                        vs.Cell(4, 6).Style.Font.SetBold(true);
                        vs.Cell(4, 6).WorksheetColumn().Width = 30;

                        vs.Cell(4, 7).Value = "Expiring";
                        vs.Cell(4, 7).Style.Font.SetBold(true);
                        vs.Cell(4, 7).WorksheetColumn().Width = 30;

                        vs.Range("B4:G4").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        vs.Range("B4:G4").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                        vs.Range("B4:G4").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                        #endregion

                        int ReportRow = 5;
                        int Reportcolumn = 2;



                        #region Table Rows

                        for (int j = 0; j < classroomList.Count; j++)
                        {
                            var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();


                            for (int k = 0; k < screeningList.Count; k++)
                            {
                                var rowSpan = screeningList.Count;

                                if (k == 0)
                                {
                                    vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Merge().Value = screeningList[k].ClassroomName;
                                    vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                    vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                    vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                    vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Font.SetBold(true);
                                    vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                }

                                vs.Cell(ReportRow, Reportcolumn + 1).Value = screeningList[k].ScreeningName;
                                vs.Cell(ReportRow, Reportcolumn + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 1).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                vs.Cell(ReportRow, Reportcolumn + 2).Value = screeningList[k].UptoDate;
                                vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                vs.Cell(ReportRow, Reportcolumn + 3).Value = screeningList[k].Missing;
                                vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                                vs.Cell(ReportRow, Reportcolumn + 4).Value = screeningList[k].Expired;
                                vs.Cell(ReportRow, Reportcolumn + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 4).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                                vs.Cell(ReportRow, Reportcolumn + 5).Value = screeningList[k].Expiring;
                                vs.Cell(ReportRow, Reportcolumn + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 5).Style.Font.SetBold(false);
                                vs.Cell(ReportRow, Reportcolumn + 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                ReportRow++;

                            }
                        }


                        #endregion



                        #region Total Calculation 

                        vs.Range("B" + ReportRow + ":C" + ReportRow + "").Merge().Value = "Total";
                        vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                        vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Font.SetBold(true);
                        vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        var uptoDate = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.UptoDate);
                        var missing = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.Missing);
                        var expired = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.Expired);
                        var expiring = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.Expiring);



                        vs.Cell(ReportRow, Reportcolumn + 2).Value = uptoDate;
                        vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(true);
                        vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        vs.Cell(ReportRow, Reportcolumn + 3).Value = missing;
                        vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(true);
                        vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        vs.Cell(ReportRow, Reportcolumn + 4).Value = expired;
                        vs.Cell(ReportRow, Reportcolumn + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        vs.Cell(ReportRow, Reportcolumn + 4).Style.Font.SetBold(true);
                        vs.Cell(ReportRow, Reportcolumn + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                        vs.Cell(ReportRow, Reportcolumn + 5).Value = expiring;
                        vs.Cell(ReportRow, Reportcolumn + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                        vs.Cell(ReportRow, Reportcolumn + 5).Style.Font.SetBold(true);
                        vs.Cell(ReportRow, Reportcolumn + 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        #endregion

                        #endregion
                    }
                }

                wb.SaveAs(memoryStream);
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return memoryStream;

        }





        public MemoryStream ExportScreeningReviewReport(NDayScreeningReviewReport screeningReviewReport, FingerprintsModel.Enums.ReportFormatType reportFormat, string imagePath)
        {

            MemoryStream memoryStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<MemoryStream>();
            try
            {


                var screeningReview = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<NDaysScreeningReview>();
                var displayNameHelper = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<Fingerprints.Common.Helpers.DisplayNameHelper>();


                screeningReview.ClassroomName = string.Empty;

                #region Export  PDF


                if (reportFormat == Enums.ReportFormatType.Pdf)
                {

                    int maxLinesPerPage = 33; // Sets the maximum rows per page //
                    Int32 colCount = 6; // column count //


                    Document doc = new Document();

                    doc.SetMargins(50f, 50f, 50f, 50f); // Defines margins of the page //

                    //Create PDF Table  


                    var writer = PdfWriter.GetInstance(doc, memoryStream);
                    writer.CloseStream = false;
                    doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());



                    doc.OpenDocument();

                    if (screeningReviewReport != null && screeningReviewReport.NDayScreeningReviewList.Count > 0)
                    {

                        var centerList = screeningReviewReport.NDayScreeningReviewList.Select(x => x.CenterID).Distinct().ToList();

                        for (int i = 0; i < centerList.Count; i++)
                        {



                            PdfPTable tableLayout = new PdfPTable(colCount);
                            tableLayout.HeaderRows = 3;

                            //Add Content to PDF   
                            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  


                            if (colCount == 6)
                            {
                                float[] headers = { 30, 30, 30, 20, 30, 30 };
                                tableLayout.SetWidths(headers); //Set the pdf headers
                            }

                            if (i > 0)
                            {
                                doc.NewPage();

                            }



                            var screeningWithCenterList = screeningReviewReport.NDayScreeningReviewList.Where(x => x.CenterID == centerList[i]).ToList();

                            var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();


                            #region Adding Headers

                            #region Adding Star Rating image with Center Name

                            Paragraph p = new Paragraph(screeningWithCenterList[0].CenterName, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)));



                            string starImageUrl = "";

                            // Star Rating Image URL //
                            starImageUrl = imagePath + "\\220px-Star_rating_" + screeningWithCenterList[0].StepUpToQualityStars + "_of_5.png";


                            // starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";


                            iTextSharp.text.Image starJpeg = iTextSharp.text.Image.GetInstance(starImageUrl);

                            //Resize image depend upon your need

                            starJpeg.ScaleToFit(40f, 40f);

                            //Give space before image

                            starJpeg.SpacingBefore = 10f;

                            //Give some space after the image

                            starJpeg.SpacingAfter = 10f;

                            starJpeg.Alignment = Element.ALIGN_LEFT;

                            p.Add(new Chunk(starJpeg, 20 * 2, 0, true));

                            PdfPCell cell = new PdfPCell(p);
                            cell.Colspan = colCount;
                            cell.Padding = 5;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cell.VerticalAlignment = 1;
                            tableLayout.AddCell(cell);


                            #endregion


                            tableLayout.AddCell(new PdfPCell(new Phrase("45-Day Screening Review Report", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Colspan = colCount,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            });

                            #endregion


                            ////Add header 

                            #region Table Headers

                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(screeningReview, "ClassroomName"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(screeningReview, "ScreeningName"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });



                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(screeningReview, "Completed"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(screeningReview, "CompletedButLate"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(screeningReview, "NotExpired"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(screeningReview, "NotCompletedandLate"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            #endregion



                            #region Table Rows
                            for (int j = 0; j < classroomList.Count; j++)
                            {
                                var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();



                                for (int k = 0; k < screeningList.Count; k++)

                                {
                                    var rowSpan = screeningList.Count();


                                    if (tableLayout.Rows.Count == maxLinesPerPage && k > 0)
                                    {
                                        tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                        {
                                            HorizontalAlignment = Element.ALIGN_LEFT,
                                            Padding = 3,
                                            Rowspan = (rowSpan - k),
                                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                        });

                                    }


                                    if (k == 0)
                                    {
                                        tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                        {
                                            HorizontalAlignment = Element.ALIGN_LEFT,
                                            Padding = 3,
                                            Rowspan = ((tableLayout.Rows.Count + rowSpan <= maxLinesPerPage)) ? rowSpan : (tableLayout.Rows.Count + rowSpan > maxLinesPerPage) ? rowSpan - ((tableLayout.Rows.Count + rowSpan) - maxLinesPerPage) : rowSpan,
                                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                        });

                                    }




                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ScreeningName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].Completed.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].CompletedButLate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].NotExpired.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].NotCompletedandLate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });
                                }




                            }


                            #endregion

                            var completed = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.Completed);
                            var completedButLate = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.CompletedButLate);
                            var notExpired = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.NotExpired);
                            var notCompletedandLate = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.NotCompletedandLate);

                            #region Total Calculation
                            tableLayout.AddCell(new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                            {
                                HorizontalAlignment = Element.ALIGN_RIGHT,
                                Padding = 3,
                                Colspan = 2,

                                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)

                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(completed.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(completedButLate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(notExpired.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(notCompletedandLate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                            });

                            #endregion

                            doc.Add(tableLayout);

                        }
                    }

                    doc.CloseDocument();



                }



                #endregion


                #region Export Excel
                else if (reportFormat == Enums.ReportFormatType.Xls)
                {
                    XLWorkbook wb = new XLWorkbook();


                    if (screeningReviewReport.NDayScreeningReviewList != null && screeningReviewReport.NDayScreeningReviewList.Count > 0)
                    {


                        var centerList = screeningReviewReport.NDayScreeningReviewList.Select(x => x.CenterID).Distinct().ToList();
                        for (int i = 0; i < centerList.Count; i++)
                        {


                            #region Adding Worksheet

                            var screeningWithCenterList = screeningReviewReport.NDayScreeningReviewList.Where(x => x.CenterID == centerList[i]).ToList();

                            var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();

                            var centerName = screeningWithCenterList.Select(x => x.CenterName).First();
                            var qualityStars = screeningWithCenterList.Select(x => x.StepUpToQualityStars).First();

                            var vs = wb.Worksheets.Add(centerName.Length > 31 ? centerName.Substring(0, 15) : centerName);

                            #region Headers with Quality Stars

                            string starImageUrl = imagePath + "\\220px-Star_rating_" + screeningWithCenterList[0].StepUpToQualityStars + "_of_5.png";

                            // string starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";



                            System.Drawing.Bitmap fullImage = new System.Drawing.Bitmap(starImageUrl);


                            vs.AddPicture(fullImage).MoveTo(vs.Cell("F2"), new System.Drawing.Point(100, 1)).Scale(0.3);// optional: resize picture



                            vs.Range("B2:C2").Merge();
                            vs.Range("B2:C2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B2:C2").Style.Font.SetBold(true);
                            vs.Range("B2:C2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            vs.Range("F2:G2").Merge();
                            vs.Range("F2:G2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("F2:G2").Style.Font.SetBold(true);
                            vs.Range("F2:G2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            vs.Range("D2:E2").Merge().Value = centerName;
                            vs.Range("D2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("D2:E2").Style.Font.SetBold(true);
                            vs.Range("D2:E2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            vs.Range("B3:G3").Merge().Value = "45-Day Screening Review Report";
                            vs.Range("B3:G3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B3:G3").Style.Font.SetBold(true);
                            vs.Range("B3:G3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            #endregion



                            #region Table Headers

                            vs.Cell(4, 2).Value = displayNameHelper.GetDisplayName(screeningReview, "ClassroomName");
                            vs.Cell(4, 2).Style.Font.SetBold(true);
                            vs.Cell(4, 2).WorksheetColumn().Width = 30;





                            vs.Cell(4, 3).Value = displayNameHelper.GetDisplayName(screeningReview, "ScreeningName");
                            vs.Cell(4, 3).Style.Font.SetBold(true);
                            vs.Cell(4, 3).WorksheetColumn().Width = 30;
                            //  vs.Range("C4:C4").SetAutoFilter();


                            vs.Cell(4, 4).Value = displayNameHelper.GetDisplayName(screeningReview, "Completed");
                            vs.Cell(4, 4).Style.Font.SetBold(true);
                            vs.Cell(4, 4).WorksheetColumn().Width = 30;

                            vs.Cell(4, 5).Value = displayNameHelper.GetDisplayName(screeningReview, "CompletedButLate");
                            vs.Cell(4, 5).Style.Font.SetBold(true);
                            vs.Cell(4, 5).WorksheetColumn().Width = 30;

                            vs.Cell(4, 6).Value = displayNameHelper.GetDisplayName(screeningReview, "NotExpired");
                            vs.Cell(4, 6).Style.Font.SetBold(true);
                            vs.Cell(4, 6).WorksheetColumn().Width = 30;

                            vs.Cell(4, 7).Value = displayNameHelper.GetDisplayName(screeningReview, "NotCompletedandLate");
                            vs.Cell(4, 7).Style.Font.SetBold(true);
                            vs.Cell(4, 7).WorksheetColumn().Width = 30;

                            vs.Range("B4:G4").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            vs.Range("B4:G4").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                            vs.Range("B4:G4").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                            #endregion

                            int inititalRow = 5;

                            int ReportRow = inititalRow;
                            int Reportcolumn = 2;



                            #region Table Rows

                            for (int j = 0; j < classroomList.Count; j++)
                            {
                                var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();


                                for (int k = 0; k < screeningList.Count; k++)
                                {
                                    var rowSpan = screeningList.Count;

                                    if (k == 0)
                                    {
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Merge().Value = screeningList[k].ClassroomName;
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Font.SetBold(true);
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    }

                                    vs.Cell(ReportRow, Reportcolumn + 1).Value = screeningList[k].ScreeningName;
                                    vs.Cell(ReportRow, Reportcolumn + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 1).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 2).Value = screeningList[k].Completed;
                                    vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 3).Value = screeningList[k].CompletedButLate;
                                    vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                                    vs.Cell(ReportRow, Reportcolumn + 4).Value = screeningList[k].NotExpired;
                                    vs.Cell(ReportRow, Reportcolumn + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 4).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                                    vs.Cell(ReportRow, Reportcolumn + 5).Value = screeningList[k].NotCompletedandLate;
                                    vs.Cell(ReportRow, Reportcolumn + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 5).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                    ReportRow++;

                                }
                            }


                            #endregion


                            vs.Range("B4:G4").SetAutoFilter();
                           


                            #region Total Calculation 

                            vs.Range("B" + ReportRow + ":C" + ReportRow + "").Merge().Value = "Total";
                            vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Font.SetBold(true);
                            vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            //var completed = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.Completed);
                            //var completedButLate = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.CompletedButLate);
                            //var notExpired = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.NotExpired);
                            //var notCompletedandLate = screeningWithCenterList.Where(x => x.CenterID == centerList[i]).Sum(x => x.NotCompletedandLate);


                            int summationRow = (inititalRow + (ReportRow - 1));




                            vs.Cell(ReportRow, Reportcolumn + 2).Value = vs.Evaluate("SUM(D" + inititalRow + ":D" + summationRow + ")");

                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            vs.Cell(ReportRow, Reportcolumn + 3).Value = vs.Evaluate("SUM(E" + inititalRow + ":E" + summationRow + ")");
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            vs.Cell(ReportRow, Reportcolumn + 4).Value = vs.Evaluate("SUM(F" + inititalRow + ":F" + summationRow + ")");
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            vs.Cell(ReportRow, Reportcolumn + 5).Value = vs.Evaluate("SUM(G" + inititalRow + ":G" + summationRow + ")");
                            vs.Cell(ReportRow, Reportcolumn + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 5).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            #endregion

                            #endregion
                        }
                    }

                    wb.SaveAs(memoryStream);
                }

                #endregion

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return memoryStream;
        }



        public MemoryStream ExportScreeningFollowupReport(ScreeningFollowupReport screeningFollowupReport, FingerprintsModel.Enums.ReportFormatType reportFormat, string imagePath)
        {

            MemoryStream memoryStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<MemoryStream>();
            try
            {
                #region Export  PDF


                if (reportFormat == Enums.ReportFormatType.Pdf)
                {

                    int maxLinesPerPage = 33; // Sets the maximum rows per page //
                    Int32 colCount = 11; // column count //


                    Document doc = new Document();

                    doc.SetMargins(50f, 50f, 50f, 50f); // Defines margins of the page //

                    //Create PDF Table  


                    var writer = PdfWriter.GetInstance(doc, memoryStream);
                    writer.CloseStream = false;
                    doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());



                    doc.OpenDocument();

                    if (screeningFollowupReport != null && screeningFollowupReport.ScreeningFollowupList.Count > 0)
                    {

                        var centerList = screeningFollowupReport.ScreeningFollowupList.Select(x => x.CenterID).Distinct().ToList();

                        for (int i = 0; i < centerList.Count; i++)
                        {



                            PdfPTable tableLayout = new PdfPTable(colCount);
                            tableLayout.HeaderRows = 3;

                            //Add Content to PDF   
                            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  


                            if (colCount == 11)
                            {
                                float[] headers = { 30, 30, 30, 20, 30, 30,30,30,30,30,30};
                                tableLayout.SetWidths(headers); //Set the pdf headers
                            }

                            if (i > 0)
                            {
                                doc.NewPage();

                            }



                            var screeningWithCenterList = screeningFollowupReport.ScreeningFollowupList.Where(x => x.CenterID == centerList[i]).ToList();

                            var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();


                            #region Adding Headers

                            #region Adding Star Rating image with Center Name

                            Paragraph p = new Paragraph(screeningWithCenterList[0].CenterName, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)));



                            string starImageUrl = "";

                            // Starr Rating Image URL //
                            starImageUrl = imagePath + "\\220px-Star_rating_" + screeningWithCenterList[0].StepUpToQualityStars + "_of_5.png";


                            // starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";


                            iTextSharp.text.Image starJpeg = iTextSharp.text.Image.GetInstance(starImageUrl);

                            //Resize image depend upon your need

                            starJpeg.ScaleToFit(40f, 40f);

                            //Give space before image

                            starJpeg.SpacingBefore = 10f;

                            //Give some space after the image

                            starJpeg.SpacingAfter = 10f;

                            starJpeg.Alignment = Element.ALIGN_LEFT;

                            p.Add(new Chunk(starJpeg, 20 * 2, 0, true));

                            PdfPCell cell = new PdfPCell(p);
                            cell.Colspan = colCount;
                            cell.Padding = 5;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cell.VerticalAlignment = 1;
                            tableLayout.AddCell(cell);


                            #endregion


                            tableLayout.AddCell(new PdfPCell(new Phrase("Screening Follow-up Report", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Colspan = colCount,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            });

                            #endregion


                            ////Add header 

                            #region Table Headers

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



                            tableLayout.AddCell(new PdfPCell(new Phrase("Client", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Date of Birth", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Age", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Program Type", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Date of First Service", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Follow-up Question", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase("Screening Period", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Custom Screening", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase("Date of Last Screening", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            #endregion



                            #region Table Rows
                            for (int j = 0; j < classroomList.Count; j++)
                            {
                                var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();


                              
                                for (int k = 0; k < screeningList.Count; k++)

                                {
                                    var rowSpan = screeningList.Count();


                                    if (tableLayout.Rows.Count == maxLinesPerPage && k > 0)
                                    {
                                        tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                        {
                                            HorizontalAlignment = Element.ALIGN_LEFT,
                                            Padding = 3,
                                            Rowspan = (rowSpan - k),
                                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                        });

                                    }


                                    if (k == 0)
                                    {
                                        tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                        {
                                            HorizontalAlignment = Element.ALIGN_LEFT,
                                            Padding = 3,
                                            Rowspan = ((tableLayout.Rows.Count + rowSpan <= maxLinesPerPage)) ? rowSpan : (tableLayout.Rows.Count + rowSpan > maxLinesPerPage) ? rowSpan - ((tableLayout.Rows.Count + rowSpan) - maxLinesPerPage) : rowSpan,
                                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                        });

                                    }




                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ScreeningQuestion.ScreeningName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ClientName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].Dob, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].AgeInWords,new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ProgramType, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].DateOfFirstService, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ScreeningQuestion.Questionlist.Select(x=>x.Question).First(), new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });


                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ScreeningPeriods.Description, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ScreeningPeriods.CustomScreeningPeriod ==1?"Yes":"No", new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(screeningList[k].ScreeningDate, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });
                                }




                            }


                            #endregion


                            doc.Add(tableLayout);

                        }
                    }

                    doc.CloseDocument();



                }



                #endregion


                #region Export Excel
                else if (reportFormat == Enums.ReportFormatType.Xls)
                {
                    XLWorkbook wb = new XLWorkbook();


                    if (screeningFollowupReport.ScreeningFollowupList != null && screeningFollowupReport.ScreeningFollowupList.Count > 0)
                    {


                        var centerList = screeningFollowupReport.ScreeningFollowupList.Select(x => x.CenterID).Distinct().ToList();
                        for (int i = 0; i < centerList.Count; i++)
                        {


                            #region Adding Worksheet

                            var screeningWithCenterList = screeningFollowupReport.ScreeningFollowupList.Where(x => x.CenterID == centerList[i]).ToList();

                            var classroomList = screeningWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();

                            var centerName = screeningWithCenterList.Select(x => x.CenterName).First();
                            var qualityStars = screeningWithCenterList.Select(x => x.StepUpToQualityStars).First();

                            var vs = wb.Worksheets.Add(centerName.Length > 31 ? centerName.Substring(0, 15) : centerName);

                            #region Headers with Quality Stars

                            string starImageUrl = imagePath + "\\220px-Star_rating_" + screeningWithCenterList[0].StepUpToQualityStars + "_of_5.png";

                            // string starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";



                            System.Drawing.Bitmap fullImage = new System.Drawing.Bitmap(starImageUrl);


                            vs.AddPicture(fullImage).MoveTo(vs.Cell("I2"), new System.Drawing.Point(100, 1)).Scale(0.3);// optional: resize picture



                            vs.Range("B2:D2").Merge();
                            vs.Range("B2:D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B2:D2").Style.Font.SetBold(true);
                            vs.Range("B2:D2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            vs.Range("G2:L2").Merge();
                            vs.Range("G2:L2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("G2:L2").Style.Font.SetBold(true);
                            vs.Range("G2:L2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            vs.Range("E2:H2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("E2:H2").Style.Font.SetBold(true);
                            vs.Range("E2:H2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            vs.Range("E2:H2").Merge().Value = centerName;



                            vs.Range("B3:L3").Merge().Value = "Screening Follow-up Report";
                            vs.Range("B3:L3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B3:L3").Style.Font.SetBold(true);
                            vs.Range("B3:L3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            #endregion



                            #region Table Headers

                            vs.Cell(4, 2).Value = "Classroom";
                            vs.Cell(4, 2).Style.Font.SetBold(true);
                            vs.Cell(4, 2).WorksheetColumn().Width = 30;

                            vs.Cell(4, 3).Value = "Screening Type";
                            vs.Cell(4, 3).Style.Font.SetBold(true);
                            vs.Cell(4, 3).WorksheetColumn().Width = 30;

                            vs.Cell(4, 4).Value = "Client";
                            vs.Cell(4, 4).Style.Font.SetBold(true);
                            vs.Cell(4, 4).WorksheetColumn().Width = 30;

                            vs.Cell(4, 5).Value = "Date of Birth";
                            vs.Cell(4, 5).Style.Font.SetBold(true);
                            vs.Cell(4, 5).WorksheetColumn().Width = 30;

                            vs.Cell(4, 6).Value = "Age";
                            vs.Cell(4, 6).Style.Font.SetBold(true);
                            vs.Cell(4, 6).WorksheetColumn().Width = 30;

                            vs.Cell(4, 7).Value = "Program Type";
                            vs.Cell(4, 7).Style.Font.SetBold(true);
                            vs.Cell(4, 7).WorksheetColumn().Width = 30;


                            vs.Cell(4, 8).Value = "Date of First Service";
                            vs.Cell(4, 8).Style.Font.SetBold(true);
                            vs.Cell(4, 8).WorksheetColumn().Width = 30;

                            vs.Cell(4, 9).Value = "Follow-up Question";
                            vs.Cell(4, 9).Style.Font.SetBold(true);
                            vs.Cell(4, 9).WorksheetColumn().Width = 30;

                            vs.Cell(4, 10).Value = "Screening Period";
                            vs.Cell(4, 10).Style.Font.SetBold(true);
                            vs.Cell(4, 10).WorksheetColumn().Width = 30;

                            vs.Cell(4, 11).Value = "Custom Screening";
                            vs.Cell(4, 11).Style.Font.SetBold(true);
                            vs.Cell(4, 11).WorksheetColumn().Width = 30;

                            vs.Cell(4, 12).Value = "Date of Last Screening";
                            vs.Cell(4, 12).Style.Font.SetBold(true);
                            vs.Cell(4, 12).WorksheetColumn().Width = 30;


                            vs.Range("B4:L4").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            vs.Range("B4:L4").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                            vs.Range("B4:L4").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                            #endregion

                            int ReportRow = 5;
                            int Reportcolumn = 2;



                            #region Table Rows

                            for (int j = 0; j < classroomList.Count; j++)
                            {
                                var screeningList = screeningWithCenterList.Where(x => x.ClassroomID == classroomList[j]).ToList();

                                // screeningList Classroom List //

                                for (int k = 0; k < screeningList.Count; k++)
                                {
                                    var rowSpan = screeningList.Count;

                                    if (k == 0)
                                    {
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Merge().Value = screeningList[k].ClassroomName;
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Font.SetBold(true);
                                        vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    }

                                    vs.Cell(ReportRow, Reportcolumn + 1).Value = screeningList[k].ScreeningQuestion.ScreeningName;
                                    vs.Cell(ReportRow, Reportcolumn + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 1).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 2).Value = screeningList[k].ClientName;
                                    vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 3).Value = screeningList[k].Dob;
                                    vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                                    vs.Cell(ReportRow, Reportcolumn + 4).Value = screeningList[k].AgeInWords;
                                    vs.Cell(ReportRow, Reportcolumn + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 4).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                                    vs.Cell(ReportRow, Reportcolumn + 5).Value = screeningList[k].ProgramType;
                                    vs.Cell(ReportRow, Reportcolumn + 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 5).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 6).Value = screeningList[k].DateOfFirstService;
                                    vs.Cell(ReportRow, Reportcolumn + 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 6).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 7).Value = screeningList[k].ScreeningQuestion.Questionlist.Select(x=>x.Question).First();
                                    vs.Cell(ReportRow, Reportcolumn + 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 7).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 8).Value = screeningList[k].ScreeningPeriods.Description;
                                    vs.Cell(ReportRow, Reportcolumn + 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 8).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 9).Value = (screeningList[k].ScreeningPeriods.ScreeningPeriod == 1 ? "Yes" : "No");
                                    vs.Cell(ReportRow, Reportcolumn + 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 9).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                    vs.Cell(ReportRow, Reportcolumn + 10).Value = screeningList[k].ScreeningDate;
                                    vs.Cell(ReportRow, Reportcolumn + 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    vs.Cell(ReportRow, Reportcolumn + 10).Style.Font.SetBold(false);
                                    vs.Cell(ReportRow, Reportcolumn + 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                    ReportRow++;

                                }
                            }


                            #endregion

                            #endregion
                        }
                    }

                    wb.SaveAs(memoryStream);
                }

                #endregion

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return memoryStream;
        }


        #region Export Family Activity Report

        public MemoryStream ExportFamilyActivityReport(FamilyActivityReport familyActivityReport, FingerprintsModel.Enums.ReportFormatType reportFormat, string imagePath)
        {

            MemoryStream memoryStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<MemoryStream>();
            try
            {


                var familyActivityModel = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<FamilyActivityModel>();
                var displayNameHelper = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<Fingerprints.Common.Helpers.DisplayNameHelper>();


                familyActivityModel.ClassroomName = string.Empty;

                #region Export  PDF


                if (reportFormat == Enums.ReportFormatType.Pdf)
                {

                   
                    Int32 colCount = 5; // column count //


                    Document doc = new Document();

                    doc.SetMargins(50f, 50f, 50f, 50f); // Defines margins of the page //

                    //Create PDF Table  


                    var writer = PdfWriter.GetInstance(doc, memoryStream);
                    writer.CloseStream = false;
                    doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());



                    doc.OpenDocument();

                    if (familyActivityReport != null && familyActivityReport.FamilyActivityList.Count > 0)
                    {

                        var centerList = familyActivityReport.FamilyActivityList.Select(x => x.CenterID).Distinct().ToList();

                        for (int i = 0; i < centerList.Count; i++)
                        {



                            PdfPTable tableLayout = new PdfPTable(colCount);
                            tableLayout.HeaderRows = 3;

                            //Add Content to PDF   
                            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  


                            if (colCount == 5)
                            {
                                float[] headers = { 20, 30, 30, 30, 30};
                                tableLayout.SetWidths(headers); //Set the pdf headers
                            }

                            if (i > 0)
                            {
                                doc.NewPage();

                            }



                            var reportWithCenterList = familyActivityReport.FamilyActivityList.OrderBy(x=>x.MonthLastDate).Where(x => x.CenterID == centerList[i]).ToList();


                            #region Adding Headers

                            #region Adding Star Rating image with Center Name

                            Paragraph p = new Paragraph(reportWithCenterList[0].CenterName, new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0)));



                            string starImageUrl = "";

                            // Star Rating Image URL //
                            starImageUrl = imagePath + "\\220px-Star_rating_" + reportWithCenterList[0].StepUpToQualityStars + "_of_5.png";


                            // starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";


                            iTextSharp.text.Image starJpeg = iTextSharp.text.Image.GetInstance(starImageUrl);

                            //Resize image depend upon your need

                            starJpeg.ScaleToFit(40f, 40f);

                            //Give space before image

                            starJpeg.SpacingBefore = 10f;

                            //Give some space after the image

                            starJpeg.SpacingAfter = 10f;

                            starJpeg.Alignment = Element.ALIGN_LEFT;

                            p.Add(new Chunk(starJpeg, 20 * 2, 0, true));

                            PdfPCell cell = new PdfPCell(p);
                            cell.Colspan = colCount;
                            cell.Padding = 5;
                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                            cell.VerticalAlignment = 1;
                            tableLayout.AddCell(cell);


                            #endregion


                            tableLayout.AddCell(new PdfPCell(new Phrase("Family Activity Report", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                            {
                                Colspan = colCount,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Padding = 5
                            });

                            #endregion


                            ////Add header 

                            #region Table Headers

                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(familyActivityModel, "Month"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(familyActivityModel, "FPA"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });



                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(familyActivityModel, "Referral"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(familyActivityModel, "InternalReferral"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                            tableLayout.AddCell(new PdfPCell(new Phrase(displayNameHelper.GetDisplayName(familyActivityModel, "QualityOfReferral"), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                            });

                           

                            #endregion



                            #region Table Rows
                            for (int j = 0; j < reportWithCenterList.Count; j++)
                            {
                              

                                bool isFeaturedMonth= Array.IndexOf(familyActivityReport.Months, reportWithCenterList[j].MonthLastDate.GetValueOrDefault().Month) > -1;

                                    tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].Month, new Font(Font.FontFamily.HELVETICA, 8, isFeaturedMonth ? Font.BOLD:0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].FPA.ToString(), new Font(Font.FontFamily.HELVETICA, 8, isFeaturedMonth ? Font.BOLD:0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].Referral.ToString(), new Font(Font.FontFamily.HELVETICA, 8, isFeaturedMonth?Font.BOLD: 0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].InternalReferral.ToString(), new Font(Font.FontFamily.HELVETICA, 8, isFeaturedMonth?Font.BOLD:0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });

                                    tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].QualityOfReferral.ToString(), new Font(Font.FontFamily.HELVETICA, 8, isFeaturedMonth?Font.BOLD:0, iTextSharp.text.BaseColor.BLACK)))
                                    {
                                        HorizontalAlignment = Element.ALIGN_LEFT,
                                        Padding = 3,
                                        BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                    });
                              
                            }


                            #endregion


                            #region Total All Months Calculation
                            tableLayout.AddCell(new PdfPCell(new Phrase("Total (All Months)", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList.Select(x=>x.FPA).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList.Select(x=>x.Referral).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList.Select(x => x.InternalReferral).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(reportWithCenterList.Select(x => x.QualityOfReferral).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });

                            #endregion


                            #region total Featured Months Calculation


                            var featureMonthList = reportWithCenterList.Join(familyActivityReport.Months, fa => fa.MonthLastDate.GetValueOrDefault().Month, m => m, (fa, m) => new
                                       FingerprintsModel.FamilyActivityModel
                            {
                                FPA = fa.FPA,
                                Referral = fa.Referral,
                                InternalReferral = fa.InternalReferral,
                                QualityOfReferral = fa.QualityOfReferral

                            }).ToList();


                            tableLayout.AddCell(new PdfPCell(new Phrase("Total (Featured Months)", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(featureMonthList.Select(x => x.FPA).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(featureMonthList.Select(x => x.Referral).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(featureMonthList.Select(x => x.InternalReferral).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            tableLayout.AddCell(new PdfPCell(new Phrase(featureMonthList.Select(x => x.QualityOfReferral).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                            {
                                HorizontalAlignment = Element.ALIGN_LEFT,
                                Padding = 3,
                                BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)

                            });


                            #endregion

                            doc.Add(tableLayout);

                        }
                    }

                    doc.CloseDocument();



                }



                #endregion


                #region Export Excel
                else if (reportFormat == Enums.ReportFormatType.Xls)
                {
                    XLWorkbook wb = new XLWorkbook();


                    if (familyActivityReport.FamilyActivityList != null && familyActivityReport.FamilyActivityList.Count > 0)
                    {


                        var centerList = familyActivityReport.FamilyActivityList.Select(x => x.CenterID).Distinct().ToList();
                        for (int i = 0; i < centerList.Count; i++)
                        {


                            #region Adding Worksheet

                            var reportWithCenterList = familyActivityReport.FamilyActivityList.OrderBy(x=>x.MonthLastDate).Where(x => x.CenterID == centerList[i]).ToList();



                            var centerName = reportWithCenterList.Select(x => x.CenterName).First();


                            var vs = wb.Worksheets.Add(centerName.Length > 31 ? centerName.Substring(0, 15) : centerName);

                            #region Headers with Quality Stars

                            string starImageUrl = imagePath + "\\220px-Star_rating_" + reportWithCenterList[0].StepUpToQualityStars + "_of_5.png";

                            // string starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";



                            System.Drawing.Bitmap fullImage = new System.Drawing.Bitmap(starImageUrl);


                            vs.AddPicture(fullImage).MoveTo(vs.Cell("F2"), new System.Drawing.Point(100, 1)).Scale(0.3);// optional: resize picture



                            vs.Range("F2:F2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;





                            vs.Range("B2:E2").Merge().Value = centerName;
                            vs.Range("B2:E2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B2:E2").Style.Font.SetBold(true);
                            vs.Range("B2:E2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            vs.Range("B3:F3").Merge().Value = "Family Activity Report";
                            vs.Range("B3:F3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B3:F3").Style.Font.SetBold(true);
                            vs.Range("B3:F3").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            #endregion



                            #region Table Headers

                            vs.Cell(4, 2).Value = displayNameHelper.GetDisplayName(familyActivityModel, "Month");
                            vs.Cell(4, 2).Style.Font.SetBold(true);
                            vs.Cell(4, 2).WorksheetColumn().Width = 30;





                            vs.Cell(4, 3).Value = displayNameHelper.GetDisplayName(familyActivityModel, "FPA");
                            vs.Cell(4, 3).Style.Font.SetBold(true);
                            vs.Cell(4, 3).WorksheetColumn().Width = 30;

                            vs.Cell(4, 4).Value = displayNameHelper.GetDisplayName(familyActivityModel, "Referral");
                            vs.Cell(4, 4).Style.Font.SetBold(true);
                            vs.Cell(4, 4).WorksheetColumn().Width = 30;

                            vs.Cell(4, 5).Value = displayNameHelper.GetDisplayName(familyActivityModel, "InternalReferral");
                            vs.Cell(4, 5).Style.Font.SetBold(true);
                            vs.Cell(4, 5).WorksheetColumn().Width = 30;

                            vs.Cell(4, 6).Value = displayNameHelper.GetDisplayName(familyActivityModel, "QualityOfReferal");
                            vs.Cell(4, 6).Style.Font.SetBold(true);
                            vs.Cell(4, 6).WorksheetColumn().Width = 30;


                            vs.Range("B4:F4").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            vs.Range("B4:F4").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                            vs.Range("B4:F4").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                            #endregion

                            int inititalRow = 5;

                            int ReportRow = inititalRow;
                            int Reportcolumn = 2;



                            #region Table Rows

                            for (int j = 0; j < reportWithCenterList.Count; j++)
                            {


                                bool isFeaturedMonth = Array.IndexOf(familyActivityReport.Months, reportWithCenterList[j].MonthLastDate.GetValueOrDefault().Month) > -1;


                                vs.Cell(ReportRow, Reportcolumn).DataType = XLDataType.Text;
                               // vs.Cell(ReportRow, Reportcolumn).Value = reportWithCenterList[j].Month.Replace("-","");
                                vs.Cell(ReportRow, Reportcolumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn).Style.Font.SetBold(isFeaturedMonth);
                                vs.Cell(ReportRow, Reportcolumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Cell(ReportRow, Reportcolumn).SetValue<string>(Convert.ToString(reportWithCenterList[j].Month));


                                vs.Cell(ReportRow, Reportcolumn + 1).Value = reportWithCenterList[j].FPA;
                                vs.Cell(ReportRow, Reportcolumn + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 1).Style.Font.SetBold(isFeaturedMonth);
                                vs.Cell(ReportRow, Reportcolumn + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                vs.Cell(ReportRow, Reportcolumn + 2).Value = reportWithCenterList[j].Referral;
                                vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(isFeaturedMonth);
                                vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                                vs.Cell(ReportRow, Reportcolumn + 3).Value = reportWithCenterList[j].InternalReferral;
                                vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(isFeaturedMonth);
                                vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                                vs.Cell(ReportRow, Reportcolumn + 4).Value = reportWithCenterList[j].QualityOfReferral;
                                vs.Cell(ReportRow, Reportcolumn + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                vs.Cell(ReportRow, Reportcolumn + 4).Style.Font.SetBold(isFeaturedMonth);
                                vs.Cell(ReportRow, Reportcolumn + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                ReportRow++;


                            }


                            #endregion


                            vs.Range("B4:B4").SetAutoFilter();


                   

                            #region Total Calculation (All Months)

                            vs.Cell(ReportRow, Reportcolumn ).Value = "Total (All Months)";
                            vs.Cell(ReportRow, Reportcolumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            vs.Cell(ReportRow, Reportcolumn + 1).Value = reportWithCenterList.Select(x => x.FPA).Sum().ToString();
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            vs.Cell(ReportRow, Reportcolumn + 2).Value = reportWithCenterList.Select(x => x.Referral).Sum().ToString();
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



                            vs.Cell(ReportRow, Reportcolumn + 3).Value = reportWithCenterList.Select(x => x.InternalReferral).Sum().ToString();
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            vs.Cell(ReportRow, Reportcolumn + 4).Value = reportWithCenterList.Select(x => x.QualityOfReferral).Sum().ToString();
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            vs.Range("B" + ReportRow + ":F" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            vs.Range("B" + ReportRow + ":F" + ReportRow + "").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                            vs.Range("B" + ReportRow + ":F" + ReportRow + "").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;



                            #endregion


                            ReportRow++;

                            #region Total Calculation (Featured Months)

                            var featureMonthList = reportWithCenterList.Join(familyActivityReport.Months, fa => fa.MonthLastDate.GetValueOrDefault().Month, m => m, (fa, m) => new
                                       FingerprintsModel.FamilyActivityModel
                            {
                                FPA = fa.FPA,
                                Referral = fa.Referral,
                                InternalReferral = fa.InternalReferral,
                                QualityOfReferral = fa.QualityOfReferral

                            }).ToList();



                            vs.Cell(ReportRow, Reportcolumn).Value = "Total (Featured Months)";
                            vs.Cell(ReportRow, Reportcolumn).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            vs.Cell(ReportRow, Reportcolumn + 1).Value = featureMonthList.Select(x => x.FPA).Sum().ToString();
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            vs.Cell(ReportRow, Reportcolumn + 2).Value = featureMonthList.Select(x => x.Referral).Sum().ToString();
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            vs.Cell(ReportRow, Reportcolumn + 3).Value = featureMonthList.Select(x => x.InternalReferral).Sum().ToString();
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            vs.Cell(ReportRow, Reportcolumn + 4).Value = featureMonthList.Select(x => x.QualityOfReferral).Sum().ToString();
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Font.SetBold(true);
                            vs.Cell(ReportRow, Reportcolumn + 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                            vs.Range("B" + ReportRow + ":F" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            vs.Range("B" + ReportRow + ":F" + ReportRow + "").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                            vs.Range("B" + ReportRow + ":F" + ReportRow + "").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;



                            #endregion

                            #endregion

                           
                        }
                    }

                
                    wb.SaveAs(memoryStream);

                   
                }

                #endregion

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return memoryStream;
        }


        #endregion


        #region Export Center Monthly Report

        public MemoryStream ExportCenterMonthlyReport(CenterMonthlyReport centerMonthlyReport, FingerprintsModel.Enums.ReportFormatType reportFormat, string imagePath)
        {
            MemoryStream memoryStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<MemoryStream>();
            try
            {
                #region Export  PDF


                if (reportFormat == Enums.ReportFormatType.Pdf)
                {





                    Document doc = new Document();

                    doc.SetMargins(50f, 50f, 50f, 50f); // Defines margins of the page //

                    //Create PDF Table  


                    var writer = PdfWriter.GetInstance(doc, memoryStream);
                    writer.CloseStream = false;
                    doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());



                    doc.OpenDocument();

                    if (centerMonthlyReport != null && centerMonthlyReport.CenterMonthlyReportList.Count > 0)
                    {

                        var centerList = centerMonthlyReport.CenterMonthlyReportList.Select(x => x.CenterID).Distinct().ToList();

                        for (int i = 0; i < centerList.Count; i++)
                        {




                            var reportWithCenterList = centerMonthlyReport.CenterMonthlyReportList.OrderBy(x => x.MonthLastDate).Where(x => x.CenterID == centerList[i]).ToList();


                            for (int j = 0; j < reportWithCenterList.Count; j++)
                            {

                                if (j > 0 || i > 0)
                                    doc.NewPage();


                                #region Heading of the Report

                                Paragraph headerPara = new Paragraph("Center Monthly Report", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
                                PdfPTable headerTable = new PdfPTable(1);
                                headerTable.WidthPercentage = 100;
                                float[] tableheaders = { 100 };
                                headerTable.SetWidths(tableheaders);
                                PdfPCell headerCell = new PdfPCell(headerPara);
                                headerCell.Border = 0;
                                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                headerTable.AddCell(headerCell);
                                doc.Add(headerTable);
                                #endregion

                                doc.Add(new Paragraph("\n"));

                                #region Center Details

                                PdfPTable centerTable = new PdfPTable(3);
                                centerTable.WidthPercentage = 100;
                                float[] centerHeaders = { 40, 20, 40 };
                                centerTable.SetWidths(centerHeaders);

                                #region Center Details Heading
                                centerTable.AddCell(new PdfPCell(new Phrase("Center", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });
                                centerTable.AddCell(new PdfPCell(new Phrase("Star Rating", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                centerTable.AddCell(new PdfPCell(new Phrase("Month", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                #endregion

                                #region Center Details Data

                                centerTable.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].CenterName, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                #region Adding Star Rating image 

                                Paragraph p = new Paragraph();
                                p.Alignment = Element.ALIGN_CENTER;


                                string starImageUrl = "";

                                // Starr Rating Image URL //
                                starImageUrl = imagePath + "\\220px-Star_rating_" + reportWithCenterList[0].StepUpToQualityStars + "_of_5.png";


                                // starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";


                                iTextSharp.text.Image starJpeg = iTextSharp.text.Image.GetInstance(starImageUrl);

                                //Resize image depend upon your need

                                starJpeg.ScaleToFit(40f, 40f);

                                //Give space before image

                              //  starJpeg.SpacingBefore = 10f;

                                //Give some space after the image

                               // starJpeg.SpacingAfter = 10f;

                                starJpeg.Alignment = Element.ALIGN_CENTER;

                                p.Add(new Chunk(starJpeg, 0, 0, true));

                                


                                #endregion


                                centerTable.AddCell(new PdfPCell(p)
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                centerTable.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].Month, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                #endregion


                                doc.Add(centerTable);
                                #endregion

                                #region adding space


                                doc.Add(new Paragraph("\n"));

                                #endregion


                                #region Center coordinator and Family service workers


                                PdfPTable fswTable = new PdfPTable(2);
                                fswTable.WidthPercentage = 100;
                                float[] fswHeadrs = { 50, 50 };
                                centerTable.SetWidths(centerHeaders);

                                #region Headers
                                fswTable.AddCell(new PdfPCell(new Phrase("Center Coordinator", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                fswTable.AddCell(new PdfPCell(new Phrase("Family Service Worker(s)", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });





                                #endregion


                                #region Data

                                Paragraph centerCordinatorPara = new Paragraph();

                                Paragraph fswPara = new Paragraph();

                                if(reportWithCenterList[j].CenterCordinators.Count>0)
                                {
                                    for (int k = 0; k < reportWithCenterList[j].CenterCordinators.Count; k++)
                                    {
                                        centerCordinatorPara.Add(new Phrase(reportWithCenterList[j].CenterCordinators[k].Text, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                        centerCordinatorPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                    }
                                }
                                else
                                {
                                    centerCordinatorPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                }

                                if(reportWithCenterList[j].FamilyServiceWorkers.Count>0)
                                {
                                    for (int l = 0; l < reportWithCenterList[j].FamilyServiceWorkers.Count; l++)
                                    {
                                        fswPara.Add(new Phrase(reportWithCenterList[j].FamilyServiceWorkers[l].Text, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                        fswPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                    }
                                }
                                else
                                {
                                    fswPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                }



                                fswTable.AddCell(new PdfPCell(centerCordinatorPara)
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                fswTable.AddCell(new PdfPCell(fswPara)
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });



                                #endregion

                                doc.Add(fswTable);
                                #endregion


                                #region adding space

                                doc.Add(new Paragraph("\n"));


                                #endregion

                                #region FPA,Referrals,FSW Home Visits

                                PdfPTable familyActivityTable = new PdfPTable(3);
                                familyActivityTable.WidthPercentage = 100;
                                float[] activityTableHeaders = { 33, 34, 33 };
                                familyActivityTable.SetWidths(activityTableHeaders);


                                #region Family Activity Headers
                                familyActivityTable.AddCell(new PdfPCell(new Phrase("Cumulative Family Goals", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                familyActivityTable.AddCell(new PdfPCell(new Phrase("Monthly Referrals", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                familyActivityTable.AddCell(new PdfPCell(new Phrase("FSW Home Visit (Month)", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                #endregion

                                #region Family Activity Data

                                #region FPA

                                Paragraph fpaPara = new Paragraph();
                                if (reportWithCenterList[j].FPA.Count>0)
                                {

                                    for (int a = 0; a < reportWithCenterList[j].FPA.Count; a++)
                                    {
                                        fpaPara.Add(new Phrase(reportWithCenterList[j].FPA[a].Text + " (" + reportWithCenterList[j].FPA[a].Value + ")", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                        fpaPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                    }

                                }
                                else
                                {
                                    fpaPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                }



                                familyActivityTable.AddCell(new PdfPCell(fpaPara)
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                #endregion

                                #region Referral

                                Paragraph referralPara = new Paragraph();

                                if(reportWithCenterList[j].Referral.Count>0)
                                {
                                    for (int a = 0; a < reportWithCenterList[j].Referral.Count; a++)
                                    {
                                        referralPara.Add(new Phrase(reportWithCenterList[j].Referral[a].Text + " (" + reportWithCenterList[j].Referral[a].Value + ")", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                        referralPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                    }
                                }
                                else
                                {
                                    referralPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                }



                                familyActivityTable.AddCell(new PdfPCell(referralPara)
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                #endregion

                                #region FSW Home Visit


                                Paragraph fswHomeVisitPara = new Paragraph();

                                if(reportWithCenterList[j].FSWHomeVisit.Count>0)
                                {
                                    for (int a = 0; a < reportWithCenterList[j].FSWHomeVisit.Count; a++)
                                    {
                                        fswHomeVisitPara.Add(new Phrase(reportWithCenterList[j].FSWHomeVisit[a].Text + " (" + reportWithCenterList[j].FSWHomeVisit[a].Value + ")", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                        fswHomeVisitPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                    }
                                }
                                else
                                {
                                    fswHomeVisitPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                }


                                familyActivityTable.AddCell(new PdfPCell(fswHomeVisitPara)
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                #endregion


                                #endregion


                                doc.Add(familyActivityTable);
                                #endregion


                                #region adding space

                                doc.Add(new Paragraph("\n"));

                                #endregion

                                #region Monthly Recruitment Activities

                                PdfPTable recruitmentTable = new PdfPTable(3);
                                recruitmentTable.WidthPercentage = 100;

                                
                                float[] recruitmentHeaders = { 20, 30,50 };
                                recruitmentTable.SetWidths(recruitmentHeaders);


                                #region Recruitment Activities


                                #region Getting Data

                                var activitiesStaffList = reportWithCenterList[j].RecruitmentActivitiesList.OrderBy(x=>x.EnteredBy).Select(x => x.EnteredBy).Distinct().ToList();

                                int rowSpanRecruitmentActivities = 0;


                                foreach(var item in activitiesStaffList)
                                {
                                    rowSpanRecruitmentActivities += reportWithCenterList[j].RecruitmentActivitiesList.Where(x => x.EnteredBy == item).Count();
                                }
                                #endregion

                                recruitmentTable.AddCell(new PdfPCell(new Phrase("Monthly Recruitment Activities", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    VerticalAlignment=Element.ALIGN_CENTER,
                                    Padding = 5,
                                    Rowspan= rowSpanRecruitmentActivities,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                #endregion

                                #region Data

                                if(activitiesStaffList.Count>0)
                                {
                                    for (int m = 0; m < activitiesStaffList.Count; m++)
                                    {
                                        var activitiesList = reportWithCenterList[j].RecruitmentActivitiesList.Where(x => x.EnteredBy == activitiesStaffList[m]).ToList();

                                        for (int n = 0; n < activitiesList.Count; n++)
                                        {
                                            if (n == 0)
                                            {
                                                recruitmentTable.AddCell(new PdfPCell(new Phrase(activitiesList[n].EnteredBy, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                                {

                                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                                    VerticalAlignment = Element.ALIGN_CENTER,
                                                    Padding = 5,
                                                    Rowspan = activitiesList.Count

                                                });
                                            }

                                            recruitmentTable.AddCell(new PdfPCell(new Phrase(activitiesList[n].Description, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                            {

                                                HorizontalAlignment = Element.ALIGN_LEFT,
                                                Padding = 5,
                                            });

                                        }


                                    }
                                }
                                else
                                {
                                    recruitmentTable.AddCell(new PdfPCell(new Phrase( string.Empty, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                    {
                                        Padding = 5,
                                        Colspan=2
                                    });
                                }
                               


                                
                                #endregion;


                                doc.Add(recruitmentTable);

                                #endregion


                                #region adding space

                                doc.Add(new Paragraph("\n"));


                                #endregion


                                #region Parent Meeting
                                PdfPTable parentMeetingTable = new PdfPTable(4);
                                parentMeetingTable.WidthPercentage = 100;
                                float[] meetingHeaders = { 20, 40, 30, 10 };
                                parentMeetingTable.SetWidths(meetingHeaders);

                                #region Headers & Data


                                parentMeetingTable.AddCell(new PdfPCell(new Phrase("Parent Meeting Title", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });



                                parentMeetingTable.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].ParentMeeting != null?reportWithCenterList[j].ParentMeeting.WorkshopName:string.Empty, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 5

                                });

                                parentMeetingTable.AddCell(new PdfPCell(new Phrase("Number in Attendance", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                parentMeetingTable.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].ParentMeeting != null ? reportWithCenterList[j].ParentMeeting.AttendanceCount.ToString():string.Empty, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                parentMeetingTable.AddCell(new PdfPCell(new Phrase("Description of Event", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });



                                parentMeetingTable.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].ParentMeeting != null?reportWithCenterList[j].ParentMeeting.Description:string.Empty, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 5,
                                    Colspan = 3
                                });

                                parentMeetingTable.AddCell(new PdfPCell(new Phrase("Description of the Education Component", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                parentMeetingTable.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].ParentMeeting != null?reportWithCenterList[j].ParentMeeting.EducationComponentDescription:string.Empty, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 5,
                                    Colspan = 3
                                });



                                #endregion

                                doc.Add(parentMeetingTable);
                                #endregion


                                #region adding space

                                doc.Add(new Paragraph("\n"));



                                #endregion

                                #region ADA Section

                                PdfPTable adaTable = new PdfPTable(4);
                                adaTable.WidthPercentage = 100;
                                float[] adaHeaders = { 10, 10, 20, 60 };
                                adaTable.SetWidths(adaHeaders);

                                #region Headers

                                adaTable.AddCell(new PdfPCell(new Phrase("Monthly ADA", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                adaTable.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].ADA.ToString() + " " + "%", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5

                                });

                                adaTable.AddCell(new PdfPCell(new Phrase("If ADA is below 85%, give explanation:", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                adaTable.AddCell(new PdfPCell(new Phrase(reportWithCenterList[j].ExplanationADAUnderPercentage, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                #endregion

                                doc.Add(adaTable);

                                #endregion


                                #region adding space

                                doc.Add(new Paragraph("\n"));




                                #endregion

                                #region Child & Family Review
                                PdfPTable cfrTable = new PdfPTable(3);
                                cfrTable.WidthPercentage = 100;
                                float[] cfrHeaders = { 30, 30, 30 };
                                cfrTable.SetWidths(cfrHeaders);


                                #region Child and Family Review Headers

                                cfrTable.AddCell(new PdfPCell(new Phrase("CFR Date", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                cfrTable.AddCell(new PdfPCell(new Phrase("Teacher Home Visits for the Month", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });


                                cfrTable.AddCell(new PdfPCell(new Phrase("P.T.C. for the Month", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });


                                #endregion

                                #region Child & Family Review

                                Paragraph familyReviewPara = new Paragraph();


                                familyReviewPara.Add(new Phrase(reportWithCenterList[j].ChildFamilyReview!=null ? reportWithCenterList[j].ChildFamilyReview.Value:string.Empty, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                familyReviewPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                familyReviewPara.Add(new Phrase(reportWithCenterList[j].ChildFamilyReview != null ? reportWithCenterList[j].ChildFamilyReview.Text:string.Empty, new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));


                                cfrTable.AddCell(new PdfPCell(familyReviewPara)
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });


                                #region Teacher Home Visit
                                Paragraph teacherHomeVisitPara = new Paragraph();

                                if(reportWithCenterList[j].TeacherHomeVisit.Count>0)
                                {
                                    for (int s = 0; s < reportWithCenterList[j].TeacherHomeVisit.Count; s++)
                                    {
                                        teacherHomeVisitPara.Add(new Phrase(reportWithCenterList[j].TeacherHomeVisit[s].Text + " (" + reportWithCenterList[j].TeacherHomeVisit[s].Value+")", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                        teacherHomeVisitPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                    }
                                }
                                else
                                {
                                    teacherHomeVisitPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                }



                                cfrTable.AddCell(new PdfPCell(teacherHomeVisitPara)
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                #endregion

                                #region Parent Teacher Conferences
                                Paragraph ptcPara = new Paragraph();

                                if(reportWithCenterList[j].ParentTeacherConference.Count>0)
                                {
                                    for (int s = 0; s < reportWithCenterList[j].ParentTeacherConference.Count; s++)
                                    {
                                        ptcPara.Add(new Phrase(reportWithCenterList[j].ParentTeacherConference[s].Text + " (" + reportWithCenterList[j].ParentTeacherConference[s].Value+")", new Font(Font.FontFamily.HELVETICA, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))));
                                        ptcPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                    }
                                }
                                else
                                {
                                    ptcPara.Add(new Phrase("\n\n", new Font(Font.FontFamily.HELVETICA, 10, 0, new iTextSharp.text.BaseColor(0, 0, 0))));

                                }




                                cfrTable.AddCell(new PdfPCell(ptcPara)
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });
                                #endregion

                                #endregion

                                doc.Add(cfrTable);
                                #endregion

                            }

                        }
                    }

                    doc.CloseDocument();



                }



                #endregion
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return memoryStream;
        }

        #endregion


        #region Export Substitute Role Report


         public MemoryStream ExportSubstituteRoleReport(SubstituteRole substituteRole, FingerprintsModel.Enums.ReportFormatType reportFormat, string imagePath)
        {

            MemoryStream memoryStream = Fingerprints.Common.FactoryInstance.Instance.CreateInstance<MemoryStream>();
            try
            {
                #region Export  PDF


                if (reportFormat == Enums.ReportFormatType.Pdf)
                {

                    int maxLinesPerPage = 33; // Sets the maximum rows per page //



                    Document doc = new Document();

                    doc.SetMargins(50f, 50f, 50f, 50f); // Defines margins of the page //

                    //Create PDF Table  


                    var writer = PdfWriter.GetInstance(doc, memoryStream);
                    writer.CloseStream = false;
                    doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());



                    doc.OpenDocument();

                    if (substituteRole != null && substituteRole.SubsituteRoleList.Count > 0)
                    {

                        var centerList = substituteRole.SubsituteRoleList.Select(x => x.CenterID).Distinct().ToList();

                        for (int i = 0; i < centerList.Count; i++)
                        {


                            var subRoleWithCenterList=substituteRole.SubsituteRoleList.OrderBy(x => x.MonthLastDate).Where(x => x.CenterID == centerList[i]).ToList();

                            // var subRoleWithCenterList = substituteRole.SubsituteRoleList.Where(x => x.CenterID == centerList[i]).ToList();
                            var monthList = subRoleWithCenterList.Select(x => x.MonthLastDate).Distinct().ToList();



                            for(int j=0; j<monthList.Count;j++)
                            {

                                if (j > 0 || i > 0)
                                {
                                    doc.NewPage();

                                }





                                PdfPTable tableLayout = new PdfPTable(4);
                                tableLayout.HeaderRows = 3;

                                //Add Content to PDF   
                                tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  



                                float[] headers = { 30, 30, 30, 20, };
                                tableLayout.SetWidths(headers); //Set the pdf headers

                                var classroomWithCenterList = subRoleWithCenterList.Where(x=>x.MonthLastDate==monthList[j]).Distinct().ToList();
                                var classroomList = classroomWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();

                                #region Heading of the Report

                                Paragraph headerPara = new Paragraph("Substitute Teacher Monthly Report", new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD, new iTextSharp.text.BaseColor(0, 0, 0)));
                                PdfPTable headerTable = new PdfPTable(1);
                                headerTable.WidthPercentage = 100;
                                float[] tableheaders = { 100 };
                                headerTable.SetWidths(tableheaders);
                                PdfPCell headerCell = new PdfPCell(headerPara);
                                headerCell.Border = 0;
                                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                headerTable.AddCell(headerCell);
                                doc.Add(headerTable);
                                #endregion


                                doc.Add(new Paragraph("\n"));

                                #region Adding Headers




                                #region Center Details Heading
                                tableLayout.AddCell(new PdfPCell(new Phrase("Center", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228),
                                    Colspan = 2
                                });
                                tableLayout.AddCell(new PdfPCell(new Phrase("Star Rating", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)

                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase("Month", new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    BackgroundColor = new BaseColor(240, 238, 228)
                                });

                                #endregion


                                #region Adding Star Rating image with Center Name


                                tableLayout.AddCell(new PdfPCell(new Phrase(classroomWithCenterList[0].CenterName, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5,
                                    Colspan=2

                                });

                                #region Adding Star Rating image 

                                Paragraph p = new Paragraph();
                                p.Alignment = Element.ALIGN_CENTER;


                                string starImageUrl = "";

                                // Starr Rating Image URL //
                                starImageUrl = imagePath + "\\220px-Star_rating_" + classroomWithCenterList[0].StepUpToQualityStars + "_of_5.png";


                                // starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";


                                iTextSharp.text.Image starJpeg = iTextSharp.text.Image.GetInstance(starImageUrl);

                                //Resize image depend upon your need

                                starJpeg.ScaleToFit(40f, 40f);

                                //Give space before image

                                //  starJpeg.SpacingBefore = 10f;

                                //Give some space after the image

                                // starJpeg.SpacingAfter = 10f;

                                starJpeg.Alignment = Element.ALIGN_CENTER;

                                p.Add(new Chunk(starJpeg, 0, 0, true));




                                #endregion


                                tableLayout.AddCell(new PdfPCell(p)
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase(classroomWithCenterList[0].Month, new Font(Font.FontFamily.HELVETICA, 10, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
                                {

                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    Padding = 5
                                });

                                #endregion




                                #endregion


                                ////Add header 

                                #region Table Headers

                                tableLayout.AddCell(new PdfPCell(new Phrase("Classroom", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                                });


                                tableLayout.AddCell(new PdfPCell(new Phrase("Staff Name", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                                });



                                tableLayout.AddCell(new PdfPCell(new Phrase("From Date", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                                });

                                tableLayout.AddCell(new PdfPCell(new Phrase("To Date", new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, iTextSharp.text.BaseColor.WHITE)))
                                {
                                    HorizontalAlignment = Element.ALIGN_LEFT,
                                    Padding = 3,
                                    BackgroundColor = new iTextSharp.text.BaseColor(105, 105, 105)
                                });



                                #endregion

                             

                                #region Table Rows
                                for (int k = 0; k < classroomList.Count; k++)
                                {
                                    var subList = classroomWithCenterList.Where(x => x.ClassroomID == classroomList[k]).ToList();



                                    for (int l = 0; l < subList.Count; l++)

                                    {
                                        var rowSpan = subList.Count();


                                        if (tableLayout.Rows.Count == maxLinesPerPage && l > 0)
                                        {
                                            tableLayout.AddCell(new PdfPCell(new Phrase(subList[l].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                            {
                                                HorizontalAlignment = Element.ALIGN_LEFT,
                                                VerticalAlignment=Element.ALIGN_CENTER,
                                                Padding = 3,
                                                Rowspan = (rowSpan - l),
                                                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                            });

                                        }


                                        if (l == 0)
                                        {
                                            tableLayout.AddCell(new PdfPCell(new Phrase(subList[l].ClassroomName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                            {
                                                HorizontalAlignment = Element.ALIGN_LEFT,
                                                VerticalAlignment=Element.ALIGN_CENTER,
                                                Padding = 3,
                                                Rowspan = ((tableLayout.Rows.Count + rowSpan <= maxLinesPerPage)) ? rowSpan : (tableLayout.Rows.Count + rowSpan > maxLinesPerPage) ? rowSpan - ((tableLayout.Rows.Count + rowSpan) - maxLinesPerPage) : rowSpan,
                                                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                            });

                                        }




                                        tableLayout.AddCell(new PdfPCell(new Phrase(subList[l].StaffDetails.FullName, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                        {
                                            HorizontalAlignment = Element.ALIGN_LEFT,
                                            Padding = 3,
                                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                        });

                                        tableLayout.AddCell(new PdfPCell(new Phrase(subList[l].FromDate, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                        {
                                            HorizontalAlignment = Element.ALIGN_LEFT,
                                            Padding = 3,
                                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                        });

                                        tableLayout.AddCell(new PdfPCell(new Phrase(subList[l].ToDate, new Font(Font.FontFamily.HELVETICA, 8, 0, iTextSharp.text.BaseColor.BLACK)))
                                        {
                                            HorizontalAlignment = Element.ALIGN_LEFT,
                                            Padding = 3,
                                            BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                                        });


                                    }




                                }


                                #endregion

                                doc.Add(tableLayout);


                            }











                          

                        }
                    }

                    doc.CloseDocument();



                }



                #endregion


                #region Export Excel
                else if (reportFormat == Enums.ReportFormatType.Xls)
                {
                    XLWorkbook wb = new XLWorkbook();
                  

                    if (substituteRole.SubsituteRoleList != null && substituteRole.SubsituteRoleList.Count > 0)
                    {


                        var centerList = substituteRole.SubsituteRoleList.Select(x => x.CenterID).Distinct().ToList();

                        for (int i = 0; i < centerList.Count; i++)
                        {
                            var centerName = substituteRole.SubsituteRoleList.Where(x => x.CenterID == centerList[i]).Select(x => x.CenterName).FirstOrDefault();

                            var vs = wb.Worksheets.Add(centerName.Length > 31 ? centerName.Substring(0, 15) : centerName);

                            var subRoleWithCenterList = substituteRole.SubsituteRoleList.OrderBy(x => x.MonthLastDate).Where(x => x.CenterID == centerList[i]).ToList();

                            var monthList = subRoleWithCenterList.Select(x => x.MonthLastDate).Distinct().ToList();

                            int ReportRow = 2;
                            int Reportcolumn = 2;


                            vs.Range("B" + ReportRow + ":E" + ReportRow + "").Merge();
                            vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Font.SetBold(true);
                            vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            vs.Range("B" + ReportRow + ":E" + ReportRow + "").Merge().Value = "Substitute Teacher Monthly Report";

                            ReportRow = ReportRow + 2;

                            for (int j = 0; j < monthList.Count; j++)
                            {

                                if (j > 0 || i > 0)
                                {
                                    // doc.NewPage();

                                    ReportRow = ReportRow + 3;

                                }

                                #region Adding Worksheet

                                var subWithCenterList = subRoleWithCenterList.Where(x => x.MonthLastDate == monthList[j]).ToList();

                                var classroomList = subWithCenterList.Select(x => x.ClassroomID).Distinct().ToList();


                                var qualityStars = subWithCenterList.Select(x => x.StepUpToQualityStars).First();



                                #region Headers with Quality Stars



                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Merge();
                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Font.SetBold(true);
                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Merge().Value = "Center";
                               


                                vs.Cell("D" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                vs.Cell("D" + ReportRow + "").Style.Font.SetBold(true);
                                vs.Cell("D" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Cell("D" + ReportRow + "").Value = "Star Rating";

                                vs.Cell("E" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                vs.Cell("E" + ReportRow + "").Style.Font.SetBold(true);
                                vs.Cell("E" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Cell("E" + ReportRow + "").Value = "Month";

                                vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                                vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;


                                ReportRow++;

                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Font.SetBold(true);
                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Range("B" + ReportRow + ":C" + ReportRow + "").Merge().Value = centerName;

                                string starImageUrl = imagePath + "\\220px-Star_rating_" + subWithCenterList[0].StepUpToQualityStars + "_of_5.png";

                                // string starImageUrl = imagePath + "\\Star_" + screeningWithCenterList[0].StepUpToQualityStars + "_Rating.png";

                                System.Drawing.Bitmap fullImage = new System.Drawing.Bitmap(starImageUrl);

                                vs.Cell("D" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                             

                                vs.AddPicture(fullImage).MoveTo(vs.Cell("D" + ReportRow + ""), new System.Drawing.Point(70, 1)).Scale(0.3);// optional: resize picture


                                vs.Cell("E" + ReportRow + "").DataType = XLDataType.Text;
                                vs.Cell("E" + ReportRow + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                vs.Cell("E" + ReportRow + "").Style.Font.SetBold(true);
                                vs.Cell("E" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Cell("E" + ReportRow + "").Value = subWithCenterList[0].Month;
                                vs.Cell("E" + ReportRow + "").SetValue<string>(Convert.ToString(subWithCenterList[0].Month));
                            
                                ReportRow++;


                                #endregion

                                #region Table Headers

                                vs.Cell(ReportRow, Reportcolumn).Value = "Classroom";
                                vs.Cell(ReportRow, Reportcolumn).Style.Font.SetBold(true);
                                vs.Cell(ReportRow, Reportcolumn).WorksheetColumn().Width = 30;

                                vs.Cell(ReportRow, (Reportcolumn + 1)).Value = "Staff Name";
                                vs.Cell(ReportRow, (Reportcolumn + 1)).Style.Font.SetBold(true);
                                vs.Cell(ReportRow, (Reportcolumn + 1)).WorksheetColumn().Width = 30;

                                vs.Cell(ReportRow, (Reportcolumn + 2)).Value = "From Date";
                                vs.Cell(ReportRow, (Reportcolumn + 2)).Style.Font.SetBold(true);
                                vs.Cell(ReportRow, (Reportcolumn + 2)).WorksheetColumn().Width = 30;

                                vs.Cell(ReportRow, (Reportcolumn + 3)).Value = "To Date";
                                vs.Cell(ReportRow, (Reportcolumn + 3)).Style.Font.SetBold(true);
                                vs.Cell(ReportRow, (Reportcolumn + 3)).WorksheetColumn().Width = 30;

                                vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.Gray;
                                vs.Range("B" + ReportRow + ":E" + ReportRow + "").Style.Font.FontColor = ClosedXML.Excel.XLColor.White;

                                #endregion

                                ReportRow++;

                                #region Table Rows

                                for (int k = 0; k < classroomList.Count; k++)
                                {
                                    var subList = subWithCenterList.Where(x => x.ClassroomID == classroomList[k]).ToList();

                                    for (int l = 0; l < subList.Count; l++)
                                    {
                                        var rowSpan = subList.Count;

                                        if (l == 0)
                                        {
                                            vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Merge().Value = subList[l].ClassroomName;
                                            vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                            vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                            vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                                            vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Font.SetBold(true);
                                            vs.Range("B" + ReportRow + ":B" + (ReportRow + (rowSpan - 1)) + "").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                        }

                                        vs.Cell(ReportRow, Reportcolumn + 1).Value = subList[l].StaffDetails.FullName;
                                        vs.Cell(ReportRow, Reportcolumn + 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                        vs.Cell(ReportRow, Reportcolumn + 1).Style.Font.SetBold(false);
                                        vs.Cell(ReportRow, Reportcolumn + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                                        vs.Cell(ReportRow, Reportcolumn + 2).DataType = XLDataType.Text;
                                        vs.Cell(ReportRow, Reportcolumn + 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                        vs.Cell(ReportRow, Reportcolumn + 2).Style.Font.SetBold(false);
                                        vs.Cell(ReportRow, Reportcolumn + 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                        vs.Cell(ReportRow, Reportcolumn + 2).SetValue<string>(subList[l].FromDate);

                                        vs.Cell(ReportRow, Reportcolumn + 3).DataType = XLDataType.Text;
                                        vs.Cell(ReportRow, Reportcolumn + 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                        vs.Cell(ReportRow, Reportcolumn + 3).Style.Font.SetBold(false);
                                        vs.Cell(ReportRow, Reportcolumn + 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                        vs.Cell(ReportRow, Reportcolumn + 3).SetValue<string>(subList[l].ToDate);


                                        ReportRow++;

                                    }
                                }


                                #endregion

                                #endregion
                            }



                        }
                    }

                    wb.SaveAs(memoryStream);
                }

                #endregion

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return memoryStream;
        }

        #endregion

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
                    "powered by: GEFingerPrints™  Copyright 2016, 2017 ",
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
}
