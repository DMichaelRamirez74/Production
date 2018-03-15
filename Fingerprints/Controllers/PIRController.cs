using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsModel;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Globalization;
using FingerprintsData;




namespace Fingerprints.Controllers
{
    public class PIRController : Controller
    {

        string userid = "ee850d45-cd00-41a1-b311-99e70d28d79e";
        //this roleid (2d9822cd-85a3-4269-9609-9aabb914d792) is for HRManager testing
        //this roleid (2d9822cd-85a3-4269-9609-9aabb914d725) is made up for staff testing since
        //it doesn't matter what it is. only matters that it's not HR Manager.
       // string roleid = "2d9822cd-85a3-4269-9609-9aabb914d725";
       // string agencyid = "C40BB313-BAC6-44E3-A746-C34B03979797";
      
        public ActionResult PIRSummary(string id)
        {

            return View(new PIRData().GetPIR(Session["UserID"].ToString(), Session["AgencyID"].ToString(),id));

        }
        [HttpGet]
        public ActionResult PIRDetails(string id)
        {

          
                //if (Session["RoleName"] != null && (Session["RoleName"].ToString().ToUpper().Contains("a65bb7c2-e320-42a2-aed4-409a321c08a5")))
            return View(new PIRData().GetPIRDetails(Session["UserID"].ToString(), Session["AgencyID"].ToString(), id));
                //Session["PIRQuestion"] = _PIR.pirQuestion;

           
        }
       
    }
}
