using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsModel;
using Fingerprints.Filters;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Data;


namespace Fingerprints.Controllers
{
    public class ScreeningController : Controller
    {
        /*role=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
         role=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
         role=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
         role=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
         role=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
         */



        public ActionResult AddScreening(DataTable CustomScreening)
        {
            Screening _screening = new Screening();
            try
            {

                _screening.CustomScreening = CustomScreening;
                return View(_screening);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View(_screening);
            }
        }
         
        public ActionResult AddLead()
        {
            return View();
        }
         
        

    }
}
