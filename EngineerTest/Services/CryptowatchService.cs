using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EngineerTest.Data;
using EngineerTest.Models.Api.Cryptowatch;
using EngineerTest.Models.Data;
using EngineerTest.Wrappers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EngineerTest.Services
{
    public class CryptowatchService
    {
        /// <summary>
        /// The default market list for all exchanges
        /// </summary>
        public static Tuple<string, string>[] DefaultMarkets = {
            Tuple.Create("btc","usd"), 
            Tuple.Create("eth","usd"),
            Tuple.Create("ltc","usd")
        };
        
        /// <summary>
        /// All the exchanges that will be monitored and the markets in
        /// those exchanges that will be monitored
        /// </summary>
        public static ExchangeAndMarket[] AllExchangesAndMarkets { get; } =
        {
            new ExchangeAndMarket("gdax", DefaultMarkets),
            new ExchangeAndMarket("bitfinex", DefaultMarkets),
            new ExchangeAndMarket("bitstamp", DefaultMarkets),
            new ExchangeAndMarket("kraken", DefaultMarkets),
            new ExchangeAndMarket("cexio", DefaultMarkets),
        };
        
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private readonly ILogger _logger;
        private readonly IHttpClient _httpClient;

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="dbContextFactory">The application datacontext factory</param>
        /// <param name="logger">The logger</param>
        /// <param name="httpClient">If null, will use a new <see cref="HttpClient"/> object</param>
        /// <param name="baseAddress">The base address</param>
        /// <param name="timeoutInMillis">The default timeout in milliseconds</param>
        public CryptowatchService(
            ApplicationDbContextFactory dbContextFactory,
            ILogger<CryptowatchService> logger,
            IHttpClient httpClient = null,
            string baseAddress = "https://api.cryptowat.ch",
            int timeoutInMillis = 5000)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
            _httpClient = httpClient ?? new HttpClientWrapper(new HttpClient());
            _httpClient.BaseAddress = new Uri(baseAddress);
            _httpClient.Timeout = TimeSpan.FromSeconds(timeoutInMillis);
        }

        /// <summary>
        /// Will retreive the latest trades from the exchanges and markets
        /// defined in CryptowatchService.AllExchangesAndMarkets and will save
        /// them to the database table <see cref="CryptoTrade"/>
        /// </summary>
        public virtual void GetMarketTradeItemsAndSaveSummarySync()
        {
            GetMarketTradeItemsAndSaveSummary()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// Will retreive the latest trades from the exchanges and markets
        /// defined in CryptowatchService.AllExchangesAndMarkets and will save
        /// them to the database table <see cref="CryptoTrade"/>
        /// </summary>
        public virtual async Task GetMarketTradeItemsAndSaveSummary()
        {
            var newBatchId = Guid.NewGuid();
            int tradeNum = 1;
            var getDataTasks = (
                from exchange in AllExchangesAndMarkets 
                from market in exchange.Markets 
                select GetMarketTradeItems(exchange.Exchange, market))
                .ToList();
            var allData = await Task.WhenAll(getDataTasks).ConfigureAwait(false);

            // add to database
            using (var dbContext = _dbContextFactory.GetContext())
            {
                var latestTs = dbContext.CryptoTrades
                                   .OrderByDescending(t => t.TimeStamp)
                                   .Take(1)
                                   .FirstOrDefault()?.TimeStamp ?? int.MinValue;
                
                var total = allData.Sum(mt => mt.Trades?.Count ?? 0);
                
                var results =
                    (from marketTrades in allData
                        where marketTrades != null
                        from trade in marketTrades.Trades
                        where trade.TimeStamp > latestTs
                        select new CryptoTrade()
                        {
                            RunId = newBatchId,
                            TradeNum = tradeNum++,
                            Amount = trade.Price,
                            BaseCurrency = marketTrades.Market.Item1,
                            SubCurrency = marketTrades.Market.Item2,
                            Exchange = marketTrades.Exchange,
                            Volume = trade.Volume,
                            TimeStamp = trade.TimeStamp,
                        }).ToList();
            
                dbContext.CryptoTrades.AddRange(results);
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                _logger.LogInformation("Added {number} entries to CryptoTrades from {total}", results.Count, total);
            }
        }
        
        private async Task<MarketTrades> GetMarketTradeItems(string exchange, Tuple<string, string> market)
        {
            var eventId = new EventId();
            var getUrl = $"markets/{exchange}/{market.Item1}{market.Item2}/trades";
            
            _logger.LogInformation(eventId, "Beginning get {url}", getUrl);
            var watch = Stopwatch.StartNew();
            
            MarketTrades result = null;
            var resp = await _httpClient.GetStringAsync(getUrl).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(resp))
            {
                var res = JsonConvert.DeserializeObject<MarketTradeResponse>(resp);
                result = new MarketTrades()
                {
                    Exchange = exchange,
                    Market = market,
                    Trades = res.ToTradeItems().ToList()
                };
            }
            
            _logger.LogInformation(eventId, "Returned in {time}", watch.ElapsedMilliseconds);
            return result;
        }

    }
}