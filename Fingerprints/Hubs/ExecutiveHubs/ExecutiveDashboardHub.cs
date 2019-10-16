using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using FingerprintsModel;
using System.Threading.Tasks;
using SignalRWebApp.SqlServerNotifier;
using System.Configuration;
using System.Data.SqlClient;
using Fingerprints.SqlServerNotifier;

namespace Fingerprints.Hubs.ExecutiveHubs
{
    [HubName("executiveDashboardTicker")]
    public class ExecutiveDashboardHub : Hub
    {
        private readonly IExecutiveDashboadTicker _executiveDashboardTicker;
        private readonly static HubConnectionMapping<string> _connections = HubConnectionMapping<string>.Instance;
        internal NotifierEntity NotifierEntity { get; private set; }
        public ExecutiveDashboardHub(IExecutiveDashboadTicker executiveDashboardTicker)
        {
            _executiveDashboardTicker = executiveDashboardTicker;
        }

        //public Task JoinGroup(string groupName)
        //{
        //    return Groups.Add(Context.ConnectionId, groupName);
        //}
        //public Task LeaveGroup(string groupName)
        //{
        //    return Groups.Remove(Context.ConnectionId, groupName);
        //}

        public IEnumerable<ExecutiveDashboardTickerModel> getExecutiveDasboardData(AppUserState appUserState)
        {
            ExecutiveDashboardTickerModel model = new ExecutiveDashboardTickerModel { AppUserState = appUserState };

            return _executiveDashboardTicker.GetDashboardTickerData(model);
        }


        public void Initialize(AppUserState appUserState)
        {
            var contexts = _connections.GetConnections(Context.User.Identity.Name).Where(x => x.ConnectionId == Context.ConnectionId && x.UserRoleType=="ExecutiveHub" ).ToList();

            contexts.ForEach(x => {
                x.AgencyId = appUserState.AgencyId;
                x.RoleId = appUserState.RoleId;
                x.UserId = appUserState.UserId;
            });

            Parallel.ForEach(contexts, (current) =>
            {
                _connections.Add(Context.User.Identity.Name, current);
            });
           
            SetNotificationEntry();
            ExecutiveDashboardTickerModel model = new ExecutiveDashboardTickerModel { AppUserState = appUserState };

            _executiveDashboardTicker.StartExecutiveDashboardTicker(model);
        }


        public void SetNotificationEntry()
        {
            NotifierEntity = new NotifierEntity();

            NotifierEntity.SqlConnectionString = ConfigurationManager.ConnectionStrings[FingerprintsData.connection.ConnectionString].ConnectionString;
            NotifierEntity.SqlQuery = "SELECT [ClientId],[AttendanceDate],[CenterID],[ClassroomId] from [dbo].[ClientAttendance]";


            NotifierEntity.SqlParameters = new List<SqlParameter>();

            Action<String> dispatcher = (t) => { _executiveDashboardTicker.BroadCastExecutiveDashboardTicker(); };
            PushSqlDependency.Instance(NotifierEntity, dispatcher);
        }

        public override Task OnConnected()
        {

            FingerprintsModel.AppUserState appUserState = new AppUserState
            {
                Name = Context.User.Identity.Name,
                ConnectionId = Context.ConnectionId,
                UserRoleType ="ExecutiveHub"
            };
            _connections.Add(appUserState.Name, appUserState);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            _connections.Remove(name, Context.ConnectionId);
            _executiveDashboardTicker.RemoveExecutiveTickerClient(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;
            var connections = _connections.GetConnections(name);
            if (connections.Where(x => x.ConnectionId == Context.ConnectionId).Any())
            {
                _connections.Add(name, connections.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault());
                ExecutiveDashboardTickerModel model = new ExecutiveDashboardTickerModel();
                model.AppUserState = connections.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
                _executiveDashboardTicker.AddExecutiveTickerClient(Context.ConnectionId, model);
            }

            return base.OnConnected();
        }




    }
}