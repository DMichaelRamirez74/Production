using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fingerprints.Common;
using FingerprintsModel;
using FingerprintsData;
using Fingerprints.Filters;

namespace Fingerprints.Areas.HealthManager.Controllers
{

    [CustAuthFilter(FingerprintsModel.Enums.RoleEnum.HealthManager)]
    public class DashboardController : Controller
    {
        //
        // GET: /HealthManager/Dashboard/


        StaffDetails staff = FactoryInstance.Instance.CreateInstance<StaffDetails>();

        public ActionResult Dashboard()
        {

          var model=  FactoryInstance.Instance.CreateInstance<HealthManagerData>().GetHealthManagerDashboard(staff);

            return View(model);

        }

    }
}
