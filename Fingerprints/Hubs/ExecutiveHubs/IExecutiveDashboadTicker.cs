using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Hubs.ExecutiveHubs
{
   public interface IExecutiveDashboadTicker
    {
        void StopExecutiveTicker();

        IEnumerable<ExecutiveDashboardTickerModel> GetDashboardTickerData(ExecutiveDashboardTickerModel model);

        void StartExecutiveDashboardTicker(ExecutiveDashboardTickerModel model);

        void RemoveExecutiveTickerClient(string key);
        void AddExecutiveTickerClient(string key, ExecutiveDashboardTickerModel model);
        void BroadCastExecutiveDashboardTicker();



    }
}
