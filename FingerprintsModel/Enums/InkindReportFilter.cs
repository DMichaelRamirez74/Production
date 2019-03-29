using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel.Enums
{
    public enum InkindReportFilter
    {


        [Description("Center")]
        Center = 1,

        [Description("Contributer")]
        Contributor = 2,

        [Description("Contribution Activity")]
        ContributionActivity = 3,

        [Description("Date Entered")]
        DateEntered = 4,

        [Description("Entered By")]
        EnteredBy = 5


    }
}
