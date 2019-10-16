using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fingerprints.Hubs.ExecutiveHubs
{
    public class ExecutiveDashboardTicker : IExecutiveDashboadTicker
    {
        public ExecutiveDashboardTicker(IHubConnectionContext<dynamic> clients)
        {
            if (clients == null)
            {
                throw new ArgumentNullException("clients");
            }

            Clients = clients;
        }

        //  private readonly static HubConnectionMapping<string> _connections = new HubConnectionMapping<string>();
        private readonly ConcurrentDictionary<string, ExecutiveDashboardTickerModel> _dashboardModel = new ConcurrentDictionary<string, ExecutiveDashboardTickerModel>();
        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }



        public void LoadExecutiveDashboardTicker()
        {
            _dashboardModel.Clear();
        }

        public void StopExecutiveTicker()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExecutiveDashboardTickerModel> GetDashboardTickerData(ExecutiveDashboardTickerModel model)
        {
            string adaPercentage = string.Empty;
            string seats = string.Empty;

            //FingerprintsModel.StaffDetails staff = new FingerprintsModel.StaffDetails(false)
            //{
            //    AgencyId = new Guid(model.AppUserState.AgencyId),
            //    RoleId = new Guid(model.AppUserState.RoleId),
            //    UserId = new Guid(model.AppUserState.UserId)
            //};

         



            new FingerprintsData.ExecutiveData().GetADASeatsDaily(ref adaPercentage, ref seats, model.AppUserState);

            model.SeatsFilled = seats;
            model.ADAPercentage = adaPercentage;

            // _dashboardModel.TryAdd(model.AppUserState.Name, model);


            return new List<ExecutiveDashboardTickerModel> { model };
            // return _dashboardModel.TryGetValue(.Values;

        }

        public void StartExecutiveDashboardTicker(ExecutiveDashboardTickerModel model)
        {
            //throw new NotImplementedException();


            FingerprintsModel.StaffDetails staff = new FingerprintsModel.StaffDetails(false)
            {
                AgencyId = new Guid(model.AppUserState.AgencyId),
                RoleId = new Guid(model.AppUserState.RoleId),
                UserId = new Guid(model.AppUserState.UserId)
            };

            _dashboardModel.TryAdd(model.AppUserState.ConnectionId, model);
            BroadCastExecutiveDashboardTicker();
        }

        public void RemoveExecutiveTickerClient(string key)
        {
            ExecutiveDashboardTickerModel model = new ExecutiveDashboardTickerModel();

            _dashboardModel.TryRemove(key, out model);
        }

        public void BroadCastExecutiveDashboardTicker()
        {
            foreach (var item in _dashboardModel)
            {
                var modal = GetDashboardTickerData(item.Value);

                foreach (var item2 in modal)
                {
                    Clients.Client(item2.AppUserState.ConnectionId).executiveDashboardTicker(modal);
                }
            }
        }

        public void AddExecutiveTickerClient(string key, ExecutiveDashboardTickerModel model)
        {
            _dashboardModel.TryAdd(key, model);
        }
    }
}