using FingerprintsData;
using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fingerprints.Areas.ParentPortal.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /ParentPortal/Home/

        public ActionResult FamilyInfo()
        {
            if (Session["ChildDetails"] == null)
            {
                List<BillingReview> list = new List<BillingReview>();
                string ProfilePic = string.Empty;
                string ParentName = string.Empty;
                DataTable dtParentAndChildDetails = new DataTable();
                if (Session["EmailID"] != null)
                    new ParentData().GetParentDetails(ref dtParentAndChildDetails, Session["EmailID"].ToString(), ref ProfilePic);
                if (dtParentAndChildDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtParentAndChildDetails.Rows)
                    {
                        if (!Convert.ToBoolean(dr["Isfamily"].ToString()))
                        {
                            list.Add(new BillingReview() { ClientId = dr["ClientID"].ToString(), ClientName = dr["Name"].ToString() });
                        }
                        else
                        {
                            if (!Convert.ToBoolean(dr["ParentId"].ToString()))
                            {
                                ParentName = dr["Name"].ToString();
                            }
                        }

                    }

                }

                Session["ChildDetails"] = list;
                Session["ParentName"] = ParentName;
                ProfilePic = ProfilePic == "" ? ("/Images/prof-image.png") : ("data:image/jpg;base64," + ProfilePic);
                Session["ProfilePic"] = ProfilePic;
            }

            //ViewBag.ClientId = ClientId;
            ViewBag.ClientId = 0;
            return View();
        }

    }
}
