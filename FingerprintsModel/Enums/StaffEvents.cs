using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel.Enums
{

    public enum StaffEventListType
    {

        Initial = 1,
        ByEventId = 2,
        UpcomingEvents = 3,
        CancelledEvents = 4,
        CompletedEvents = 5,
        OpenEvents = 6 //today event not inculded

    }
}
