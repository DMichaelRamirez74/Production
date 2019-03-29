using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel.Enums
{

    /// <summary>
    /// Enumaration for determining expiration period
    /// </summary>
    public enum ScreeningExpiration
    {
        [Description(null)]
        Current = 0,
        [Description("Expiring")]
        Expiring = 1,
        [Description("Expired")]
        Expired = 2
    }

}
