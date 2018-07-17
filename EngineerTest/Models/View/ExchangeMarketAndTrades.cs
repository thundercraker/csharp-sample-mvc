using System.Collections.Generic;

namespace EngineerTest.Models.View
{
    public class ExchangeMarketAndTrades
    {
        public string Meta { get; set; }
        public List<TradeDataPoint> Trades { get; set; }
    }
}