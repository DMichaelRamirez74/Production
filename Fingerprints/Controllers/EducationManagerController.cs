using FingerprintsData;
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fingerprints.Controllers
{
    public class EducationManagerController : Controller
    {
        //
        // GET: /EducationManager/

        EducationManagerData EduData = new EducationManagerData();
        public ActionResult EducationManagerDashboard()
        {
           
            return View();
        }

        public JsonResult GetEducationManagerDashboard()
        {
            EduData = new EducationManagerData();
            EducationManager manager = new EducationManager();
            try
            {
                manager = EduData.GetEducationDashboard();
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(manager, JsonRequestBehavior.AllowGet);
         }
        public ActionResult StaffEventCreation()
        {
            return View();
        }

        public ActionResult EventsList()
        {
            return View();
        }

    }
}
