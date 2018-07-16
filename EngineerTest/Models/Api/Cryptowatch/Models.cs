using System;
using System.Collections.Generic;

namespace EngineerTest.Models.Api.Cryptowatch
{

    public class MarketTradeResponse
    {
        public decimal[][] result { get; set; }

        public IEnumerable<MarketTradeItem> ToTradeItems()
        {
            if (result == null) yield break;
            
            foreach (var item in result)
            {
                yield return new MarketTradeItem()
                {
                    ID = (long) item[0],
                    TimeStamp = (long) item[1],
                    Price = item[2],
                    Volume = item[3]
                };
            }
        }
    }

    public class MarketTradeItem
    {
        public long ID { get; set; }
        public long TimeStamp { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
    }

    public class MarketTrades
    {
        public string Exchange { get; set; }
        public Tuple<string, string> Market { get; set; }
        public List<MarketTradeItem> Trades { get; set; }
    }
    
}