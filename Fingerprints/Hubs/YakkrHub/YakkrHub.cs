using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using FingerprintsModel;
using System.Threading.Tasks;
using Fingerprints.SqlServerNotifier;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNet.SignalR.Hubs;

namespace Fingerprints.Hubs.YakkrHub
{
    [HubName("yakkrDashboardTicker")]
    public class YakkrHub : Hub
    {
        private readonly IYakkrTicker _yakkrTicker;
        private readonly static HubConnectionMapping<string> _connections = HubConnectionMapping<string>.Instance;
        internal NotifierEntity NotifierEntity { get; private set; }
        public YakkrHub(IYakkrTicker yakkrTicker)
        {
            _yakkrTicker = yakkrTicker;
        }

       
        public void Initialize(AppUserState appUserState)
        {
            var contexts = _connections.GetConnections(Context.User.Identity.Name).Where(x => x.ConnectionId == Context.ConnectionId && x.UserRoleType=="YakkrHub").ToList();

            contexts.ForEach(x =>
            {
                x.AgencyId = appUserState.AgencyId;
                x.UserId = appUserState.UserId;
                x.RoleId = appUserState.RoleId;
            });

            Parallel.ForEach(contexts, (current) =>
            {
                _connections.Add(Context.User.Identity.Name, current);
            });
            SetNotificationEntry();

            YakkrTickerModel model = new YakkrTickerModel();
            model.AppUserState = appUserState;
            _yakkrTicker.StartYakkrTicker(model);
        }

        private void SetNotificationEntry()
        {
            NotifierEntity = new NotifierEntity();

            NotifierEntity.SqlConnectionString = ConfigurationManager.ConnectionStrings[FingerprintsData.connection.ConnectionString].ConnectionString;
            NotifierEntity.SqlQuery = "SELECT [YakkrId],[ClientId],[StaffID],[StaffIDR] from [dbo].[YakkrRouting]";


            NotifierEntity.SqlParameters = new List<SqlParameter>();

            Action<String> dispatcher = (t) => { _yakkrTicker.BroadcastYakkrTicker(); };
            PushSqlDependency.Instance(NotifierEntity, dispatcher);
        }

        public override Task OnConnected()
        {

            FingerprintsModel.AppUserState appUserState = new AppUserState
            {
                Name = Context.User.Identity.Name,
                ConnectionId = Context.ConnectionId,
                UserRoleType="YakkrHub"
            };
            _connections.Add(appUserState.Name, appUserState);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            _connections.Remove(name, Context.ConnectionId);
            _yakkrTicker.RemoveYakkrTickerClient(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;
            var connections = _connections.GetConnections(name);
            if (connections.Where(x => x.ConnectionId == Context.ConnectionId).Any())
            {
                _connections.Add(name, connections.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault());
                YakkrTickerModel model = new YakkrTickerModel();
                model.AppUserState = connections.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
                _yakkrTicker.AddYakkrTickerClient(Context.ConnectionId, model);
            }

            return base.OnConnected();
        }

    }
}