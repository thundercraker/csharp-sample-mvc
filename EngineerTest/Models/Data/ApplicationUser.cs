using Microsoft.AspNetCore.Identity;

namespace EngineerTest.Models.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// comma seperated values of exchanges that will appear on the
        /// user's home/dashboard
        /// </summary>
        public string ExchangeChoices { get; set; }
        /// <summary>
        /// comma seperated values of currencies that will appear on the
        /// user's home/dashboard, the format is a base and sub currency pair,
        /// eg: "btc-usd", "ltc-jpy"
        /// </summary>
        public string CurrencyChoices { get; set; }
    }
}
