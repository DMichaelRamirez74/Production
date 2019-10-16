using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Hubs.YakkrHub
{
    public  interface IYakkrTicker
    {
        void StopYakkrTicker();

        IEnumerable<YakkrTickerModel> GetYakkrTickerData(YakkrTickerModel model);

        void StartYakkrTicker(YakkrTickerModel model);

        void RemoveYakkrTickerClient(string key);
        void AddYakkrTickerClient(string key, YakkrTickerModel model);
        void BroadcastYakkrTicker();
    }
}
