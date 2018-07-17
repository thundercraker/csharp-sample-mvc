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
}