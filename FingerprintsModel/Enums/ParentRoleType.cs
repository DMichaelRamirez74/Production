using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel.Enums
{
    public enum ParentRoleType
    {

        [Description("Father Figure")]
        FatherFigure = 1,

        [Description("Mother Figure")]
        MontherFiguer = 2,

        [Description("Grandparents")]
        Grandparents = 3,

        [Description("Relatives other than grandparents")]
        Relatives = 4,

        [Description("Foster Parent - Not including relative")]
        FosterParent = 5,

        [Description("Other")]
        Other = 6

    }
}
