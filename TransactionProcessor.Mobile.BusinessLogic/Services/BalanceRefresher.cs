using System;
using System.Collections.Generic;
using System.Text;
using TransactionProcessor.Mobile.BusinessLogic.Logging;

namespace TransactionProcessor.Mobile.BusinessLogic.Services
{
    public interface IBalanceRefresher
    {
        event Action<Decimal> BalanceChanged;
        void StartRefreshing();
        void StopRefreshing();
    }

    public class BalanceRefresher : IBalanceRefresher
    {
        private readonly IApplicationCache ApplicationCache;
        private readonly Func<Boolean, IMerchantService> MerchantServiceResolver;
        private CancellationTokenSource? _cts;

        public event Action<Decimal> BalanceChanged;

        public BalanceRefresher(IApplicationCache applicationCache, Func<Boolean, IMerchantService> merchantServiceResolver) {
            this.ApplicationCache = applicationCache;
            this.MerchantServiceResolver = merchantServiceResolver;
        }

        public void StartRefreshing()
        {
            _cts = new CancellationTokenSource();
            _ = RefreshLoopAsync(_cts.Token);
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
                await RefreshBalanceAsync();

                while (await timer.WaitForNextTickAsync(token))
                {
                    await RefreshBalanceAsync();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task RefreshBalanceAsync()
        {
            decimal balance = await GetBalanceFromApi();

            this.ApplicationCache.SetMerchantBalance(balance);
            this.BalanceChanged?.Invoke(balance);
        }

        private async Task<decimal> GetBalanceFromApi()
        {
            Boolean useTrainingMode = this.ApplicationCache.GetUseTrainingMode();
            IMerchantService merchantService = this.MerchantServiceResolver(useTrainingMode);

            var result = await merchantService.GetMerchantBalance(CancellationToken.None);
            if (result.IsSuccess) {
                Logger.LogInformation($"Refreshed merchant balance: {result.Data}");
                return result.Data;
            }

            // Handle the failure case as needed, e.g., log the error or return a default value
            Logger.LogWarning($"Failed to refresh merchant balance: {result.Message}");
            return 0.0m;
        }
    }
}
