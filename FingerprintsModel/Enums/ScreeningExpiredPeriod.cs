using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel.Enums
{
    public enum ScreeningExpiredPeriod
    {
        [Description("15 Days")]
        Days90 = 1,
        [Description("30 Days")]
        Months60 = 2,
        [Description("60 Days")]
        Days60 = 3

    }
}
