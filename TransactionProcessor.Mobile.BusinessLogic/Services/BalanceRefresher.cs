using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransactionProcessor.Mobile.BusinessLogic.Logging;

namespace TransactionProcessor.Mobile.BusinessLogic.Services
{
    public interface IBalanceRefresher
    {
        void StartRefreshing();
        void StopRefreshing();
    }

    public class BalanceRefresher : IBalanceRefresher
    {
        private readonly IApplicationCache ApplicationCache;
        private readonly IMerchantService MerchantService;
        private CancellationTokenSource? _cts;

        public BalanceRefresher(IApplicationCache applicationCache, IMerchantService merchantService) {
            this.ApplicationCache = applicationCache;
            this.MerchantService = merchantService;
        }

        public void StartRefreshing()
        {
            _cts = new CancellationTokenSource();
            // Run the refresh loop on a background thread so it does not run on the main UI thread.
            _ = Task.Run(() => RefreshLoopAsync(_cts.Token));
        }

        public void StopRefreshing()
        {
            _cts?.Cancel();
        }

        private async Task RefreshLoopAsync(CancellationToken token)
        {
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

            try
            {
                // Do the first refresh
                await RefreshBalanceAsync(token).ConfigureAwait(false);

                while (await timer.WaitForNextTickAsync(token).ConfigureAwait(false))
                {
                    await RefreshBalanceAsync(token).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task RefreshBalanceAsync(CancellationToken token)
        {
            decimal balance = await GetBalanceFromApi(token).ConfigureAwait(false);

            // Setting the cache is lightweight; do on background thread to avoid UI thread work.
            this.ApplicationCache.SetMerchantBalance(balance);
        }

        private async Task<decimal> GetBalanceFromApi(CancellationToken token)
        {
            var result = await this.MerchantService.GetMerchantBalance(token).ConfigureAwait(false);
            if (result.IsSuccess)
            {
                return result.Data;
            }

            // Handle the failure case as needed, e.g., log the error or return a default value
            Logger.LogWarning($"Failed to refresh merchant balance: {result.Message}");
            return 0.0m;
        }
    }
}
