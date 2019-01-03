using Fingerprints.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;

namespace Fingerprints.Controllers
{
    [CustAuthFilter("ae148380-f94e-4f7a-a378-897c106f1a52")]
    public class BusMonitorController : Controller
    {
        // GET: /BusMonitor/        
        public ActionResult Dashboard()
        {
            BusMonitorAnalysis objBusMonitor = new BusMonitorAnalysis();
            BusMonitorAnalysisList objList = new BusMonitorAnalysisList();
            try
            {
                objBusMonitor.AgencyId = new Guid(Session["AgencyId"].ToString());
                objBusMonitor.UserId = new Guid(Session["UserID"].ToString());
                objList = new BusMonitorData().GetBusMonitorData(objBusMonitor);
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }

            return View(objList);
        }
        public JsonResult GetBusRiderChildren(string centerid,string routetype)
        {
            List<BusRiderChildrens> riderChildrenData = new List<BusRiderChildrens>();
            try
            {
                long dec_centerId = Convert.ToInt64((centerid));
                Guid agencyId = new Guid(Session["AgencyId"].ToString());
                riderChildrenData = new BusMonitorData().GetBusRiderChildrensData(agencyId, dec_centerId,routetype);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(riderChildrenData, JsonRequestBehavior.AllowGet);
        }
      

    }
}
