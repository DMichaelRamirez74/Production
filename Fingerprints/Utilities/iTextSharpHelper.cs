using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fingerprints.Utilities
{
    public class iTextSharpHelper
    {
    }



    public class BinaryContentResult : ActionResult
    {
        private string ContentType;
        private byte[] ContentBytes;

        public BinaryContentResult(byte[] contentBytes, string contentType)
        {
            this.ContentBytes = contentBytes;
            this.ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = this.ContentType;

            var stream = new MemoryStream(this.ContentBytes);
            stream.WriteTo(response.OutputStream);
            stream.Dispose();
        }
    }


    /// <summary>
    /// Custom Class for render image with base64 data
    /// <ref>
    /// https://stackoverflow.com/questions/19389999/can-itextsharp-xmlworker-render-embedded-images
    /// Answer of VahidN
    /// </ref>
    /// </summary>
    public class CustomImageTagProcessor : iTextSharp.tool.xml.html.Image
    {
        public override IList<IElement> End(IWorkerContext ctx, Tag tag, IList<IElement> currentContent)
        {
            IDictionary<string, string> attributes = tag.Attributes;
            string src;
            if (!attributes.TryGetValue(iTextSharp.tool.xml.html.HTML.Attribute.SRC, out src))
                return new List<IElement>(1);

            if (string.IsNullOrEmpty(src))
                return new List<IElement>(1);

            if (src.StartsWith("data:image/", StringComparison.InvariantCultureIgnoreCase))
            {
                // data:[<MIME-type>][;charset=<encoding>][;base64],<data>
                var base64Data = src.Substring(src.IndexOf(",") + 1);
                var imagedata = Convert.FromBase64String(base64Data);
                var image = iTextSharp.text.Image.GetInstance(imagedata);

                var list = new List<IElement>();
                var htmlPipelineContext = GetHtmlPipelineContext(ctx);
                list.Add(GetCssAppliers().Apply(new Chunk((iTextSharp.text.Image)GetCssAppliers().Apply(image, tag, htmlPipelineContext), 0, 0, true), tag, htmlPipelineContext));
                return list;
            }
            else
            {
                return base.End(ctx, tag, currentContent);
            }
        }
    }


    class PDFBackgroundHelper : PdfPageEventHelper
    {

        private PdfContentByte cb;
        private List<PdfTemplate> templates;
        //constructor
        public PDFBackgroundHelper()
        {
            this.templates = new List<PdfTemplate>();
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            cb = writer.DirectContentUnder;
            PdfTemplate templateM = cb.CreateTemplate(50, 50);
            templates.Add(templateM);

            int pageN = writer.CurrentPageNumber;
            String pageText = "Page " + pageN.ToString() + " of ";
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            float len = bf.GetWidthPoint(pageText, 10);

            float offset = 0;

            offset = (document.PageSize.Width / 2) -(len/2); //whole page width/2=> half page then substract half of text

            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
           

            //  cb.SetTextMatrix(document.LeftMargin, document.PageSize.GetBottom(document.BottomMargin));
            cb.SetTextMatrix(offset, document.PageSize.GetBottom(document.BottomMargin));
            cb.ShowText(pageText);

            
            



            cb.EndText();
            cb.AddTemplate(templateM, offset+len, document.PageSize.GetBottom(document.BottomMargin));
            // cb.AddTemplate(templateM, document.LeftMargin + len, document.PageSize.GetBottom(document.BottomMargin));
            // cb.AddTemplate(templateM, offset, document.PageSize.GetBottom(document.BottomMargin));
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            foreach (PdfTemplate item in templates)
            {
                item.BeginText();
                item.SetFontAndSize(bf, 10);
                item.SetTextMatrix(0, 0);
                item.ShowText("" + (writer.PageNumber));
                item.EndText();
            }

        }
    }


    }