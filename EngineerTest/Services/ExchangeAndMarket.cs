using System;

namespace EngineerTest.Services
{
    public class ExchangeAndMarket
    {
        public string Exchange { get; }
        public Tuple<string,string>[] Markets { get; }

        public ExchangeAndMarket(
            string exchamge,
            Tuple<string, string>[] markets)
        {
            Exchange = exchamge;
            Markets = markets;
        }
    }
}