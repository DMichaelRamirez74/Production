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
    public class ExternalLeadsController : Controller
    {
        //
        // GET: /ExternalLeads/
        /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
      roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
      roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
      roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
      roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
      roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
      roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacvher)
      roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
      roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
      */

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult ExternalApplicantInformation(int parentId=0)
        {
            ViewBag.ParentId = parentId;
            ExternalLeadsFamily externalFamily = new ExternalLeadsFamily();
            externalFamily=  new ExternalLeadsData().GetExternalLeadsData(parentId);
            return View(externalFamily);
        }


        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult AcceptExternalApplicant(int parentId = 0)
        {
            ExternalLeadsFamily externalFamily = new ExternalLeadsFamily();
            externalFamily = new ExternalLeadsData().GetExternalLeadsData(parentId);
            
           bool IsUpdated= new ExternalLeadsData().SaveFamilyIntake(externalFamily,Session["UserID"].ToString(), parentId);
            return RedirectToAction("YakkrDetails", "Yakkr");
        }

        [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,e4c80fc2-8b64-447a-99b4-95d1510b01e9")]
        public ActionResult ApplicationRejection(string ParentId,string Reason)
        {
            bool iSRejected = false;
            try
            {
                iSRejected = new ExternalLeadsData().UpdateRejection(Convert.ToInt64(ParentId), Reason, Session["UserID"].ToString());
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
           
            return Json(iSRejected);
        }

    }
}
