using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Fingerprints.Hubs
{
    public class FingerprintsHub:Hub
    {


        //[HubMethodName("NotifyClients")]
        //public static void NotifyCurrentEmployeeInformationToAllClients(string message = "done")
        //{
        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<FingerprintsHub>();
        //    // the update client method will update the connected client about any recent changes in the server data
        //    context.Clients.All.updatedClients();
        //}


        [HubMethodName("progressData")]
        public static void ProgressData(string connectionId, string progressMessage, int progressCount, int totalItems)
        {
            //IN ORDER TO INVOKE SIGNALR FUNCTIONALITY DIRECTLY FROM SERVER SIDE WE MUST USE THIS
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<FingerprintsHub>();

            //CALCULATING PERCENTAGE BASED ON THE PARAMETERS SENT
            var percentage = (progressCount * 100) / totalItems;

            //PUSHING DATA TO ALL CLIENTS
            //hubContext.Clients.Client(connectionId).AddProgress(progressMessage, percentage + "%");

            hubContext.Clients.All.AddProgress(progressMessage, percentage + "%");

        }
    }
}