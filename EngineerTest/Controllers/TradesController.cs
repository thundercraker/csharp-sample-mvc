using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineerTest.Extensions;
using EngineerTest.Models.Data;
using EngineerTest.Models.View;
using EngineerTest.Models.View.Trades;
using EngineerTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EngineerTest.Controllers
{
    [Authorize]
    public class TradesController : ControllerBase
    {
        private readonly CryptowatchRepository _cryptowatchRepository;
        
        public TradesController(
            CryptowatchRepository cryptowatchRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<HomeController> logger
        ): base(userManager, signInManager, logger)
        {
            _cryptowatchRepository = cryptowatchRepository;
        }
        
        public async Task<IActionResult> Index()
        {
            var limitPerMarket = 10;
            
            var result = new IndexViewModel();
            var data = new Dictionary<string, List<TradeDataPoint>>();
            var allTrades = await _cryptowatchRepository.GetAllTrades(TimeSpan.FromDays(365));
            foreach (var trade in allTrades)
            {
                var key = trade.Exchange + "/" + trade.BaseCurrency + "-" + trade.SubCurrency;
                if (!data.ContainsKey(key))
                {
                    data[key] = new List<TradeDataPoint>();
                }

                if (data[key].Count < limitPerMarket)
                {
                    data[key].Add(new TradeDataPoint()
                    {
                        x = trade.TimeStamp.FromUnixTimeStamp().ToLongTimeString(),
                        y = trade.Amount,
                    });
                }
            }

            result.TradeData = data.Select(kvp => new ExchangeMarketAndTrades()
            {
                Meta = kvp.Key,
                Trades = kvp.Value,
            }).ToList();
            
            return View(result);
        }
    }
}