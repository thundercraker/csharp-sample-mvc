using System.Collections.Generic;
using Newtonsoft.Json;

namespace EngineerTest.Models.View.Trades
{
    public class IndexViewModel
    {
        public List<ExchangeMarketAndTrades> TradeData { get; set; }
        
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}