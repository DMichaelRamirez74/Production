using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fingerprints.Hubs.YakkrHub
{
    public class YakkrTickerModel
    {
        public string YakkrCount { get; set; }
        public AppUserState AppUserState {get;set;}
    }
}