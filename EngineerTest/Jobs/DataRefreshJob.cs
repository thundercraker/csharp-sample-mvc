using EngineerTest.Services;

namespace EngineerTest.Jobs
{
    public static class DataRefreshJob
    {
        public static void CryptowatchDataRefresh(
            CryptowatchService cryptowatchService)
        {
            cryptowatchService.GetMarketTradeItemsAndSaveSummary()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }
    }
}