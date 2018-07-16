using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EngineerTest.Extensions;
using Microsoft.AspNetCore.Mvc;
using EngineerTest.Models.Data;
using EngineerTest.Models.View;
using EngineerTest.Models.View.Home;
using EngineerTest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EngineerTest.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly CryptowatchRepository _cryptowatchRepository;
        
        public HomeController(
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
            // if logged in, retreive the dashboard
            var result = new IndexViewModel();
            if (_signInManager.IsSignedIn(User))
            {
                result.HasDashboard = true;
                var data = new Dictionary<string, List<TradeDataPoint>>();
                var user = await _userManager.GetUserAsync(User);
                if (string.IsNullOrEmpty(user.CurrencyChoices)
                    && string.IsNullOrEmpty(user.ExchangeChoices))
                {
                    result.NoSetup = true;
                }
                
                foreach (var trade in 
                    await _cryptowatchRepository.GetUserDashboard(user, TimeSpan.FromDays(365)))
                {
                    var key = trade.Exchange + "/" + trade.BaseCurrency + "-" + trade.SubCurrency;
                    if (!data.ContainsKey(key))
                    {
                        data[key] = new List<TradeDataPoint>();
                    }
                    data[key].Add(new TradeDataPoint()
                    {
                        x = trade.TimeStamp.FromUnixTimeStamp().ToLongTimeString(),
                        y = trade.Amount,
                    });
                }

                result.Data = data.Select(kvp => new ExchangeMarketAndTrades()
                {
                    Meta = kvp.Key,
                    Trades = kvp.Value,
                }).ToList();
            }
            return View(result);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
