using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fingerprints.Hubs.YakkrHub
{
    public class YakkrTicker : IYakkrTicker
    {

        public YakkrTicker(IHubConnectionContext<dynamic> clients)
        {
            if (clients == null)
            {
                throw new ArgumentNullException("clients");
            }

            Clients = clients;
        }

        private readonly ConcurrentDictionary<string, YakkrTickerModel> _yakkrDictionary = new ConcurrentDictionary<string, YakkrTickerModel>();


        private IHubConnectionContext<dynamic> Clients { get; set; }
        public void AddYakkrTickerClient(string key, YakkrTickerModel model)
        {
            _yakkrDictionary.TryAdd(key, model);
        }

        public void BroadcastYakkrTicker()
        {
            foreach (var item in _yakkrDictionary)
            {
                var modal = GetYakkrTickerData(item.Value);

                foreach (var item2 in modal)
                {
                    Clients.Client(item2.AppUserState.ConnectionId).sendYakkrTicker(modal);
                }
            }
        }

        public IEnumerable<YakkrTickerModel> GetYakkrTickerData(YakkrTickerModel model)
        {
            model.YakkrCount = Convert.ToString(Utilities.Helper.GetYakkrCountByUserId(model.AppUserState.UserId, model.AppUserState.AgencyId));

            return new List<YakkrTickerModel> { model };
        }

        public void RemoveYakkrTickerClient(string key)
        {
            YakkrTickerModel model = new YakkrTickerModel();
            _yakkrDictionary.TryRemove(key, out model);
        }

        public void StartYakkrTicker(YakkrTickerModel model)
        {
            _yakkrDictionary.TryAdd(model.AppUserState.ConnectionId, model);


            BroadcastYakkrTicker();
        }

        public void StopYakkrTicker()
        {
            throw new NotImplementedException();
        }
    }
}