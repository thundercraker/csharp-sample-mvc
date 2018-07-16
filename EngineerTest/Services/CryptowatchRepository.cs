using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineerTest.Data;
using EngineerTest.Extensions;
using EngineerTest.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngineerTest.Services
{
    public class CryptowatchRepository
    {
        private readonly ApplicationDbContextFactory _dbContextFactory;
        private readonly ILogger _logger;

        public CryptowatchRepository(
            ApplicationDbContextFactory dbContextFactory,
            ILogger<CryptowatchRepository> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        /// <summary>
        /// Retreive crypto trades for the user dashboard
        /// </summary>
        public virtual async Task<IList<CryptoTrade>> GetUserDashboard(
            ApplicationUser applicationUser,
            TimeSpan timePeriod)
        {
            var userExchanges = applicationUser.ExchangeChoices?.Split(",");
            var userMarkets = applicationUser.CurrencyChoices?
                .Split(",")
                .Select(c =>
                {
                    if (string.IsNullOrEmpty(c) || !c.Contains("-"))
                    {
                        _logger.LogError("Found invalid cuurency {cur} in {user}", c, applicationUser.Id);
                        return null;
                    }
                    return c;
                })
                .Where(c => c != null)
                .ToArray();

            // if empty then use default dashboard of gdax btcusd
            if (userExchanges == null) userExchanges = new[] {"gdax"};
            if (userMarkets == null) userMarkets = new [] { "btc-usd" };

            return await GetCryptoTrades(userExchanges, userMarkets, timePeriod)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get all trades for a time period
        /// </summary>
        /// <param name="timePeriod">The timespan since UtcNow to retreive trades from</param>
        public virtual async Task<IList<CryptoTrade>> GetAllTrades(TimeSpan timePeriod)
        {
            List<CryptoTrade> trades;

            using (var db = _dbContextFactory.GetContext())
            {
                trades = await (
                        from trade in db.CryptoTrades
                        where trade.TimeStamp >= (DateTime.UtcNow - timePeriod).ToUnixTimeStamp()
                        select trade)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            
            return trades;
        }

        /// <summary>
        /// Retreive crypto trades, ordered by ascending timestamp
        /// </summary>
        /// <param name="exchanges">List of exchanges eg: "gdax", "bitfinex"</param>
        /// <param name="markets">List of markets from <see cref="ApplicationUser.CurrencyChoices"/>
        /// eg: "btc-usd", "ltc-usd"</param>
        /// <param name="timePeriod">The timespan since UtcNow to retreive trades from</param>
        private async Task<IList<CryptoTrade>> GetCryptoTrades(
            IEnumerable<string> exchanges,
            IEnumerable<string> markets,
            TimeSpan timePeriod,
            int limit = 10)
        {
            List<CryptoTrade> trades;
            
            using (var db = _dbContextFactory.GetContext())
            {
                trades = await (
                        from trade in db.CryptoTrades
                        where exchanges.Contains(trade.Exchange)
                              && markets.Contains(trade.BaseCurrency + "-" + trade.SubCurrency)
                        // && trade.TimeStamp >= (DateTime.UtcNow - timePeriod).ToUnixTimeStamp()
                        orderby trade.TimeStamp
                        select trade)
                    .DistinctBy(trade =>
                        trade.TimeStamp + "/" + trade.Exchange + "/" + trade.BaseCurrency + trade.SubCurrency)
                    .Take(limit)
                    .ToAsyncEnumerable()
                    .ToList()
                    .ConfigureAwait(false);
            }

            return trades;
        }
    }
}