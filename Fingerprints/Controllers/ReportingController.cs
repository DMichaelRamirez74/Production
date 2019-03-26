using System;
using System.Web.Mvc;
using FingerprintsModel;
using FingerprintsData;
using Fingerprints.Filters;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using Fingerprints.Utilities;

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
        public ActionResult AddCLASReview() {

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

                var fattach = new Attachments() {
                     AttachmentFile = Request.Files[i],
                      AttachmentFileName = Request.Files[i].FileName,
                       AttachmentFileExtension = Path.GetExtension(Request.Files[i].FileName),
                        AttachmentFileByte = new BinaryReader(Request.Files[i].InputStream).ReadBytes(Request.Files[i].ContentLength),
                         AttachmentStatus=true
                         
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
        public ActionResult getCLASReviews(GridParams gparam,long centerid=0,long month=0)
        {
            //  var sm = Request.QueryString["centerid"].ToString();
            long TotalCount = 0;
            var Data = new Reporting().getCLASReviews(gparam, 1,centerid,month, ref TotalCount);

            // return Json( result, JsonRequestBehavior.AllowGet);
            return Json(new { Data, TotalCount }, JsonRequestBehavior.AllowGet);

        }

        [CustAuthFilter()]
        public ActionResult ViewCLASAttachment(long attachmentId) {

            var result = new Reporting().GetCLASAttachmentById(attachmentId);

           return Json(result,JsonRequestBehavior.AllowGet);
        }

        #endregion CLASReview

    }
}