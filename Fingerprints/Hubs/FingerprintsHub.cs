using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Linq;
using System.Data.SqlClient;
using System;
using FingerprintsModel;
using System.Data;
using System.Configuration;
using SignalRWebApp.SqlServerNotifier;
using System.Collections.Generic;
using Fingerprints.SqlServerNotifier;

namespace Fingerprints.Hubs
{
    [HubName("fingerprintsHub")]

    public class FingerprintsHub : Hub
    {

        private readonly static HubConnectionMapping<string> _connections = HubConnectionMapping<string>.Instance;
        internal NotifierEntity NotifierEntity { get; private set; }

        public void DispatchToClient()
        {
            SendYakkrNotifications();
        }

        [HubMethodName("initialize")]
        public void Initialize(string agencyId, string roleId, string userId)
        {
            //  NotifierEntity = NotifierEntity.FromJson(value);
            var contexts = _connections.GetConnections(Context.User.Identity.Name).Where(x => x.ConnectionId == Context.ConnectionId).ToList();

            contexts.ForEach(x =>
            {
                x.AgencyId = agencyId;
                x.RoleId = roleId;
                x.UserId = userId;

            });

            Parallel.ForEach(contexts, (current) =>
            {
                _connections.Add(Context.User.Identity.Name, current);
            });

            NotifierEntity = new NotifierEntity();

            NotifierEntity.SqlConnectionString = ConfigurationManager.ConnectionStrings[FingerprintsData.connection.ConnectionString].ConnectionString;
            NotifierEntity.SqlQuery = "SELECT [YakkrId],[ClientId],[StaffID],[StaffIDR] from [dbo].[YakkrRouting]";


            NotifierEntity.SqlParameters = new List<SqlParameter>();

            //if (NotifierEntity == null)
            //    return;

            Action<String> dispatcher = (t) => { DispatchToClient(); };
            PushSqlDependency.Instance(NotifierEntity, dispatcher);
        }


        [HubMethodName("getYakkrCount")]
        public int GetYakkrCount(string agencyId, string roleId, string userId)
        {
            return Utilities.Helper.GetYakkrCountByUserId(userId, agencyId);
        }

        public void SendYakkrNotifications()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<FingerprintsHub>();

            if (_connections.Count > 0)
            {
                var connections = _connections.GetAllConnections();
                Parallel.ForEach(connections, (curent) =>
                {
                    int yakkrCount = GetYakkrCount(curent.AgencyId, curent.RoleId, curent.UserId);
                    hubContext.Clients.Client(curent.ConnectionId).broadcastYakkrCount(yakkrCount);//Will update all the clients with message log.
                });
            }
        }

        public override Task OnConnected()
        {

            FingerprintsModel.AppUserState appUserState = new AppUserState
            {
                Name = Context.User.Identity.Name,
                ConnectionId = Context.ConnectionId,
            };
            _connections.Add(appUserState.Name, appUserState);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            _connections.Remove(name, Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;
            var connections = _connections.GetConnections(name);
            if (connections.Where(x => x.ConnectionId == Context.ConnectionId).Any())
            {
                _connections.Add(name, connections.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault());
            }
            return base.OnConnected();
        }
    }
}