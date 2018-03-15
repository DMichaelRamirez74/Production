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
namespace Fingerprints.Controllers
{
    public class DisablityController : Controller
    {
        DisabilityManagerData _disable = new DisabilityManagerData();
        NurseData _nurse = new NurseData();
        public ActionResult YakkrDetails()
        {
            try
            {
                ViewBag.userlist = _disable.Getallclients(Session["AgencyID"].ToString(), Session["UserID"].ToString(), null);
                Nurse obj = new Nurse();
                return View(obj);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }

    }
}
