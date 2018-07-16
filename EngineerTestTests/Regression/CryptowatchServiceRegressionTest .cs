using System;
using System.Linq;
using System.Threading.Tasks;
using EngineerTest.Data;
using Xunit;
using EngineerTest.Services;
using EngineerTestTests.Common;
using Moq;

namespace EngineerTestTests.Regression
{
    public class CryptowatchServiceRegressionTest
    {
        [Fact]
        public async Task CryptowatchService_CanLoadAll()
        {
            var mockLogger = new MockLogger<CryptowatchService>();

            var dbName = Guid.NewGuid().ToString();
            var db = DbContext.GetTestContext(dbName);
            var factory = new Mock<ApplicationDbContextFactory>(null, null);
            factory.Setup(f => f.GetContext())
                .Returns(DbContext.GetTestContext(dbName));
            
            var service = new CryptowatchService(
                factory.Object, mockLogger);

            await service.GetMarketTradeItemsAndSaveSummary();
            
            // Check data context for values
            var allTrades = db.CryptoTrades.ToList();
            foreach (var exchangesAndMarket in CryptowatchService.AllExchangesAndMarkets)
            {
                // All exchanges should have some trades
                Assert.Contains(exchangesAndMarket.Exchange, 
                    allTrades.Select(_ => _.Exchange));
                // All markets for all exchanges should have some trades
                foreach (var market in exchangesAndMarket.Markets)
                {
                    Assert.Contains(market, 
                        allTrades.Select(_ => Tuple.Create(_.BaseCurrency, _.SubCurrency)));
                }
            }

            // Check that amount, volume and timestamp make sense
            foreach (var trade in allTrades)
            {
                Assert.True(trade.RunId != new Guid());
                Assert.True(trade.TradeNum > 0);
                Assert.True(trade.Amount > 0);
                Assert.True(trade.Volume > 0);
                Assert.True(trade.TimeStamp > 0);
            }
        }
    }
}