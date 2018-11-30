using System;
using System.Web.Mvc;
using FingerprintsModel;
using FingerprintsData;

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
    }
}