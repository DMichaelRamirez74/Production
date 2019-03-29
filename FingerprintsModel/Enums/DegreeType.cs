using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel.Enums
{
    public enum DegreeType
    {
        [Description("N/A")]
        NotApplicable = 1,
        [Description("ECE")]
        ECE = 2,
        [Description("ECE Equiv.")]
        ECEEquiv = 3
    }
}
