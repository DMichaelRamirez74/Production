using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fingerprints.Filters;
using FingerprintsData;
using FingerprintsModel;
using System.Data;
using Fingerprints.CustomClasses;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.end;
using System.Text;
using System.Dynamic;
//using Ghostscript.NET.Rasterizer;
using System.Drawing.Imaging;
//using Ghostscript.NET;

namespace Fingerprints.Controllers
{
    public class EducationMaterialController : Controller
    {

        public ActionResult EducationMaterialMaintenance()
        {
            return View();
        }

        public JsonResult UploadFile(string monitor)
        {
            string _imgname = string.Empty;
            string[] _imgpath = new string[System.Web.HttpContext.Current.Request.Files.Count];
            try
            {
                int i = 0;
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    foreach (var key in System.Web.HttpContext.Current.Request.Files)
                    {

                        var pic = System.Web.HttpContext.Current.Request.Files[key.ToString()];
                        if (pic.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(pic.FileName);
                            var _ext = Path.GetExtension(pic.FileName);
                            string name = Path.GetFileNameWithoutExtension(fileName);
                            _imgname = Guid.NewGuid().ToString();
                            _imgname = name + "_" + _imgname + _ext;
                            var _comPath = Server.MapPath("/Content/MaterialAttachment/") + _imgname;
                            _imgpath[i] = "/Content/MaterialAttachment/" + _imgname;
                            i++;
                            var path = _comPath;
                            pic.SaveAs(path);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(_imgpath, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveEducationMaterial(EductionMaterial objEductaion)
        {
            bool Result = false;
            try
            {
                if (objEductaion != null)
                {
                    objEductaion.UserId = Session["UserID"].ToString();
                    objEductaion.AgencyId = Session["AgencyID"].ToString();
                    Result = new EducationMaterialData().SaveEducationMaterial(objEductaion);
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(Result);
        }

        public ActionResult GetMaterialDetails()
        {
            string JSONString = string.Empty;
            string UserId = Session["UserID"].ToString();
            try
            {
                DataTable dtMaterial = new DataTable();
                new EducationMaterialData().GetMaterialDetails(ref dtMaterial, Session["Roleid"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterial);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(new { JSONString, UserId });
        }

        public ActionResult GetMaterialDetailsShare()
        {
            string JSONString = string.Empty;
            string UserId = Session["UserID"].ToString();
            try
            {
                DataTable dtMaterial = new DataTable();
                new EducationMaterialData().GetMaterialDetailsShare(ref dtMaterial, Session["Roleid"].ToString(), Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterial);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(new { JSONString, UserId });
        }

        public ActionResult GetMaterialDetailsBySerachText(string SearchText)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtMaterial = new DataTable();
                new EducationMaterialData().GetMaterialDetailsBySearchText(ref dtMaterial, Session["Roleid"].ToString(), SearchText, Session["UserID"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterial);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public ActionResult GetPostedDocumentsDetails()
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtMaterial = new DataTable();
                new EducationMaterialData().GetPostedDocumentsDetails(ref dtMaterial, Session["Roleid"].ToString());
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterial);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        public ActionResult GetPostedDocumentsDetailsForParent(string ClientId)
        {
            string JSONString = string.Empty;
            try
            {
                DataSet dtDocument = new DataSet();
                new EducationMaterialData().GetPostedDocumentsDetailsForParent(ref dtDocument, ClientId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtDocument);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }
        public ActionResult GetAttachmentByMaterialId(string MaterialId)
        {
            string JSONString = string.Empty;
            try
            {
                DataTable dtMaterial = new DataTable();
                new EducationMaterialData().GetAttachmentByMaterialId(ref dtMaterial, MaterialId);
                JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterial);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(JSONString);
        }

        [HttpPost]
        public JsonResult DeleteMaterial(string Id)
        {
            bool isResult = false;
            try
            {

                isResult = new EducationMaterialData().DeleteMaterial(Id);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
            }
            return Json(isResult);
        }

        public ActionResult EducationMaterialGroupShare()
        {
            try
            {
                // FamilyData obj = new FamilyData();
                List<SelectListItem> Centerlist = new List<SelectListItem>();
                List<FingerprintsModel.RosterNew.User> _userlist = new List<FingerprintsModel.RosterNew.User>();
                DataSet _dataset = new FamilyData().GetCenterCaseNote(Session["AgencyID"].ToString(), Session["UserID"].ToString());
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    SelectListItem info = null;
                    Centerlist.Add(new SelectListItem { Value = "0", Text = "Choose" });
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        info = new SelectListItem();
                        info.Value = dr["center"].ToString();
                        info.Text = dr["centername"].ToString();
                        Centerlist.Add(info);
                    }
                    TempData["GroupCaseNotes"] = Centerlist;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View();
        }
        public JsonResult Getclassrooms(string Centerid = "0")
        {
            try
            {
                return Json(new RosterData().Getclassrooms(Centerid, Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        [JsonMaxLengthAttribute]
        //[CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult LoadGroupCaseNoteClient(string Centerid = "0", string Classroom = "0")
        {
            try
            {
                return Json(new NurseData().LoadGroupCaseNoteClient(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        public ActionResult ShareMaterial(string ClientId)
        {
            TempData["Single"] = "";
            if (Request.QueryString["Single"] != null)
            {
                TempData["Single"] = "Single";
            }

            string[] tempId = ClientId.TrimStart(',').TrimEnd(',').Split(',');
            TempData["ClientId"] = tempId;
            return View();
        }
        [HttpPost]
        public JsonResult SendPost(string Id)
        {
            bool isPosted = false;
            try
            {
                string[] ClientId = null;

                if (TempData["ClientId"] != null)
                {
                    string[] tempId = TempData["ClientId"] as string[];
                    ClientId = new string[tempId.Length];
                    int i = 0;
                    foreach (string client in tempId)
                    {
                        string Ids = EncryptDecrypt.Decrypt64(client);
                        ClientId[i] = Ids;
                        i++;
                    }
                    isPosted = new EducationMaterialData().SavePostedDocumnetMaterial(Session["AgencyID"].ToString(), Session["UserID"].ToString(), ClientId, Id);
                    TempData.Keep();
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isPosted);
        }

        [HttpPost]
        public JsonResult SendEmail(string Id)
        {
            bool isPosted = false;
            string EMailTemplate = string.Empty;
            try
            {
                string[] ClientId = null;

                if (TempData["ClientId"] != null)
                {
                    string[] tempId = TempData["ClientId"] as string[];
                    ClientId = new string[tempId.Length];
                    int i = 0;
                    foreach (string client in tempId)
                    {
                        string Ids = EncryptDecrypt.Decrypt64(client);
                        ClientId[i] = Ids;
                        i++;
                    }
                    foreach (string client in ClientId)
                    {
                        DataSet ds = new DataSet();
                        new EducationMaterialData().GetParentEmailbyClientId(ref ds, client, Id);
                        string Attachments = "";
                        string siteURI = Request.Url.OriginalString;
                        Uri uriResource = new Uri(siteURI);
                        StreamReader reader = new StreamReader(Server.MapPath("~/Content/EducationMaterial/EmailTemplate/EducationMaterialEmailTemplate.html"));
                        EMailTemplate = reader.ReadToEnd();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string imagePath = "http://" + uriResource.Authority + "/Content/img/ge_logo_banner_left2.png";
                            EMailTemplate = EMailTemplate.Replace("{image}", imagePath);
                            string Email = "", cc = "";
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Email = !string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()) ? ds.Tables[0].Rows[0][0].ToString() : "";
                                if (ds.Tables[0].Rows.Count == 2)
                                {
                                    if (Email == "")
                                        Email = !string.IsNullOrEmpty(ds.Tables[0].Rows[1][0].ToString()) ? ds.Tables[0].Rows[1][0].ToString() : "";
                                    else
                                        cc = !string.IsNullOrEmpty(ds.Tables[0].Rows[1][0].ToString()) ? ds.Tables[0].Rows[1][0].ToString() : "";
                                }
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                                EMailTemplate = EMailTemplate.Replace("{ChildName}", !string.IsNullOrEmpty(ds.Tables[1].Rows[0][0].ToString()) ? ds.Tables[1].Rows[0][0].ToString() : "");
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                EMailTemplate = EMailTemplate.Replace("{Group}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["Group"].ToString()) ? ds.Tables[2].Rows[0]["Group"].ToString() : "");
                                EMailTemplate = EMailTemplate.Replace("{Title}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["Title"].ToString()) ? ds.Tables[2].Rows[0]["Title"].ToString() : "");
                                EMailTemplate = EMailTemplate.Replace("{Description}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["Description"].ToString()) ? ds.Tables[2].Rows[0]["Description"].ToString() : "");
                                EMailTemplate = EMailTemplate.Replace("{URL}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["URL"].ToString()) ? ds.Tables[2].Rows[0]["URL"].ToString() : "");
                                EMailTemplate = EMailTemplate.Replace("{URLNote}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["URLNote"].ToString()) ? ds.Tables[2].Rows[0]["URLNote"].ToString() : "");
                                EMailTemplate = EMailTemplate.Replace("{AssignedBy}", Session["EmailID"].ToString());
                            }
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[3].Rows)
                                {
                                    string filepath = Server.MapPath(dr["Attachmentpath"].ToString());
                                    Attachments = Attachments + "," + filepath;
                                }

                            }
                            if (Email.Trim() != "")
                            {
                                bool isSent = SendMail.SendBillingEmail(Email, EMailTemplate, Session["EmailID"].ToString(), Attachments, cc);
                                if (!isPosted)
                                    isPosted = isSent;
                            }


                        }

                    }
                    TempData.Keep();
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(isPosted);
        }

        // var fileName = Path.GetFileNameWithoutExtension(pdfFile);

        [HttpPost]
        public JsonResult PrintMaterial(string Id)
        {
            // bool isPosted = false;
            string Template = string.Empty;
            try
            {
                string[] ClientId = null;

                if (TempData["ClientId"] != null)
                {
                    string[] tempId = TempData["ClientId"] as string[];
                    ClientId = new string[tempId.Length];
                    int i = 0;
                    foreach (string client in tempId)
                    {
                        string Ids = EncryptDecrypt.Decrypt64(client);
                        ClientId[i] = Ids;
                        i++;
                    }
                    foreach (string client in ClientId)
                    {
                        DataSet ds = new DataSet();
                        new EducationMaterialData().GetParentEmailbyClientId(ref ds, client, Id);
                        string Attachments = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Template = @"<table style='width:100%;margin-top:20px;'>
                                       <tbody>
                                       <tr style='cellpadding='10';'><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'><strong>Child Name:</strong></p></td><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'>{ChildName}</P></td></tr>
                                       <tr style='cellpadding='10';'><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'><strong>Group:</strong></P></td><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'>{Group}</P></td></tr>
                                       <tr style='cellpadding='10';'><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'><strong>Title:</strong></P></td><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'>{Title}</P></td></tr>
                                       <tr style='cellpadding='10';'><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'><strong>Description:</strong></P></td><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'>{Description}</P></td></tr>
                                       <tr style='cellpadding='10';'><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'><strong>URL:</strong></P></td><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'>{URL}</P></td></tr>
                                       <tr style='cellpadding='10';'><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'><strong>URL Note:</strong></P></td><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'>{URLNote}</P></td></tr>
                                       <tr style='cellpadding='10';'><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'><strong>Assigned By:</strong></P></td><td style='border:1px solid #ddd;padding='10px';'><p style='padding:5px;font-size:20px;'>{AssignedBy}</P></td></tr>
                                       </tbody>
                                       </table>";
                            string Email = "", cc = "";
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Email = !string.IsNullOrEmpty(ds.Tables[0].Rows[0][0].ToString()) ? ds.Tables[0].Rows[0][0].ToString() : "";
                                if (ds.Tables[0].Rows.Count == 2)
                                {
                                    if (Email == "")
                                        Email = !string.IsNullOrEmpty(ds.Tables[0].Rows[1][0].ToString()) ? ds.Tables[0].Rows[1][0].ToString() : "";
                                    else
                                        cc = !string.IsNullOrEmpty(ds.Tables[0].Rows[1][0].ToString()) ? ds.Tables[0].Rows[1][0].ToString() : "";
                                }
                            }
                            Email = "Aravindh.Adhithan@gmail.com";
                            cc = "Aravindhanmca06@gmail.com";
                            if (ds.Tables[1].Rows.Count > 0)
                                Template = Template.Replace("{ChildName}", !string.IsNullOrEmpty(ds.Tables[1].Rows[0][0].ToString()) ? ds.Tables[1].Rows[0][0].ToString() : "");
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                Template = Template.Replace("{Group}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["Group"].ToString()) ? ds.Tables[2].Rows[0]["Group"].ToString() : "");
                                Template = Template.Replace("{Title}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["Title"].ToString()) ? ds.Tables[2].Rows[0]["Title"].ToString() : "");
                                Template = Template.Replace("{Description}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["Description"].ToString()) ? ds.Tables[2].Rows[0]["Description"].ToString() : "");
                                Template = Template.Replace("{URL}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["URL"].ToString()) ? ds.Tables[2].Rows[0]["URL"].ToString() : "");
                                Template = Template.Replace("{URLNote}", !string.IsNullOrEmpty(ds.Tables[2].Rows[0]["URLNote"].ToString()) ? ds.Tables[2].Rows[0]["URLNote"].ToString() : "");
                                Template = Template.Replace("{AssignedBy}", Session["EmailID"].ToString());
                            }
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[3].Rows)
                                {
                                    // Template = Template + "<div><img src='" + dr["Attachmentpath"].ToString() + "'/></div>";
                                    // string filepath = Server.MapPath(dr["Attachmentpath"].ToString());

                                    // if (filepath.Contains("pdf"))
                                    //  {
                                    //   var fileName = Path.GetFileNameWithoutExtension(filepath);
                                    //  LoadImage(filepath,1);
                                    //Attachments = Attachments + "," + Path;
                                    //PdfReader reader = new PdfReader(filepath);
                                    //var raf = new RandomAccessFileOrArray(filepath);

                                    //PdfDocument dco = new PdfDocument();
                                    //int Count = reader.NumberOfPages;
                                    //for (int j= 1; j <= Count; j++)
                                    //{
                                    //    PdfDictionary pg = reader.GetPageN(j);

                                    //    // System.Drawing.Image image =
                                    //    // image.Save(string.Format("ImagePage{0}.png", i), System.Drawing.Imaging.ImageFormat.Png);
                                    //}
                                    //}

                                }

                            }
                            // SendMail.SendBillingEmail(Email, Template, Session["EmailID"].ToString(), Attachments, cc);
                        }

                    }
                    TempData.Keep();
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(Template);
        }
   /**     public void LoadImage(string InputPDFFile, int PageNumber)
        {

            string outImageName = Path.GetFileNameWithoutExtension(InputPDFFile);
            outImageName = outImageName + "_" + PageNumber.ToString() + "_.png";


            GhostscriptPngDevice dev = new GhostscriptPngDevice(GhostscriptPngDeviceType.Png256);
            dev.GraphicsAlphaBits = GhostscriptImageDeviceAlphaBits.V_4;
            dev.TextAlphaBits = GhostscriptImageDeviceAlphaBits.V_4;
            dev.ResolutionXY = new GhostscriptImageDeviceResolution(290, 290);
            dev.InputFiles.Add(InputPDFFile);
            dev.Pdf.FirstPage = PageNumber;
            dev.Pdf.LastPage = PageNumber;
            dev.CustomSwitches.Add("-dDOINTERPOLATE");
            dev.OutputPath = Server.MapPath(@"~/Content/PDFToImages/" + outImageName);
            dev.Process();

        }
**/
        public ActionResult DownloadDocuments(string MaterialId)
        {
            string FilePath = "";
            byte[] fileBytes = null;
            try
            {
                DataTable dtMaterial = new DataTable();
                new EducationMaterialData().GetAttachmentByMaterialId(ref dtMaterial, MaterialId);
                if (dtMaterial.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtMaterial.Rows)
                    {
                        FilePath = FilePath + "," + dr["AttachmentPath"].ToString();
                    }

                }
                FilePath = FilePath.TrimStart(',').TrimEnd(',');
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
            return Json(FilePath);
        }
    }
}