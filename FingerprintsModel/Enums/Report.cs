using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel.Enums
{


    public enum MDTActionType {

        [Description("Family")]
        Family =1,
        [Description("Family Advocate")]
        FamilyAdvocate =2,
        [Description("Disability Department")]
        DisabilityDepartment=3
    }
}
