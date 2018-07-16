using System.Collections.Generic;
using Newtonsoft.Json;

namespace EngineerTest.Models.View.Home
{
    public class IndexViewModel
    {
        public bool HasDashboard { get; set; }
        public bool NoSetup { get; set; }
        public List<ExchangeMarketAndTrades> Data { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ExchangeMarketAndTrades
    {
        public string Meta { get; set; }
        public List<TradeDataPoint> Trades { get; set; }
    }
}