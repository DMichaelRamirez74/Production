using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fingerprints.Hubs.ExecutiveHubs
{
    public class ExecutiveDashboardTickerModel
    {
        public string ADAPercentage { get; set; }
        public string SeatsFilled { get; set; }
        public AppUserState AppUserState { get; set; }

    }
}