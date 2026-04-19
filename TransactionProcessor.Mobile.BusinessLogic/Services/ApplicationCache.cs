using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Services
{
    public interface IApplicationCache
    {
        String GetConfigHostUrl();

        void SetConfigHostUrl(String value, Int32 timeout=60);

        Boolean GetUseTrainingMode();

        void SetUseTrainingMode(Boolean value, Int32 timeout = 60);

        Boolean GetIsLoggedIn();

        void SetIsLoggedIn(Boolean value, Int32 timeout = 60);

        Configuration GetConfiguration();

        void SetConfiguration(Configuration value, Int32 timeout = 60);

        List<ContractProductModel> GetContractProducts();

        void SetContractProducts(List<ContractProductModel> value, Int32 timeout = 60);

        TokenResponseModel GetAccessToken();

        void SetAccessToken(TokenResponseModel value, MemoryCacheEntryOptions cacheEntryOptions = null);
        Guid GetEstateId();

        void SetEstateId(Guid value, Int32 timeout = 60);

        Guid GetMerchantId();

        void SetMerchantId(Guid value, Int32 timeout = 60);
        MerchantDetailsModel GetMerchantDetails();

        void SetMerchantDetails(MerchantDetailsModel value, Int32 timeout = 60);

        Decimal GetMerchantBalance();

        void SetMerchantBalance(Decimal value);
    }

    [ExcludeFromCodeCoverage]
    public class ApplicationCache : IApplicationCache
    {
        #region Fields

        private readonly IMemoryCache MemoryCache;

        #endregion

        #region Constructors

        public ApplicationCache(IMemoryCache memoryCache)
        {
            this.MemoryCache = memoryCache;
        }

        #endregion

        #region Methods

        public TokenResponseModel GetAccessToken()
        {
            return this.TryGetValue<TokenResponseModel>("AccessToken");
        }

        public Configuration GetConfiguration()
        {
            return this.TryGetValue<Configuration>("Configuration");
        }

        public List<ContractProductModel> GetContractProducts()
        {
            return this.TryGetValue<List<ContractProductModel>>("ContractProducts");
        }

        public Guid GetEstateId()
        {
            return this.TryGetValue<Guid>("EstateId");
        }

        public Boolean GetIsLoggedIn()
        {
            return this.TryGetValue<Boolean>("isLoggedIn");
        }

        public Guid GetMerchantId()
        {
            return this.TryGetValue<Guid>("MerchantId");
        }

        public String GetConfigHostUrl()
        {
            var url = this.TryGetValue<String>("ConfigHostUrl");
            if (String.IsNullOrWhiteSpace(url)) {
                return "http://192.168.1.163:9200";
            }
            return url;
        }

        public void SetConfigHostUrl(String value,
                                     Int32 timeout = 60)
        {
            this.Set("ConfigHostUrl", value, timeout);
        }

        public Boolean GetUseTrainingMode()
        {
            return this.TryGetValue<Boolean>("UseTrainingMode");
        }

        public void SetAccessToken(TokenResponseModel value,
                                   MemoryCacheEntryOptions cacheEntryOptions = null)
        {
            this.Set("AccessToken", value, cacheEntryOptions);
        }

        public void SetConfiguration(Configuration value,
                                     Int32 timeout = 60)
        {
            this.Set("Configuration", value, timeout);
        }

        public void SetContractProducts(List<ContractProductModel> value,
                                        Int32 timeout = 60)
        {
            this.Set("ContractProducts", value, timeout);
        }

        public void SetEstateId(Guid value,
                                Int32 timeout = 60)
        {
            this.Set("EstateId", value, timeout);
        }

        public void SetIsLoggedIn(Boolean value,
                                  Int32 timeout = 60)
        {
            this.Set("isLoggedIn", value, timeout);
        }

        public void SetMerchantId(Guid value,
                                  Int32 timeout = 60)
        {
            this.Set("MerchantId", value, timeout);
        }

        public MerchantDetailsModel GetMerchantDetails()
        {
            return this.TryGetValue<MerchantDetailsModel>("MerchantDetails");
        }

        public void SetMerchantDetails(MerchantDetailsModel value,
                                       Int32 timeout = 60)
        {
            this.Set("MerchantDetails", value, timeout);
        }

        public Decimal GetMerchantBalance() {
            return this.TryGetValue<Decimal>("MerchantBalance");
        }

        public void SetMerchantBalance(Decimal value) {
            this.Set("MerchantBalance", value, 60);
        }

        public void SetUseTrainingMode(Boolean value,
                                       Int32 timeout = 60)
        {
            this.Set("UseTrainingMode", value, timeout);
        }

        private void Set<T>(String key,
                            T cache,
                            Int32 timeout)
        {
            DateTime expirationTime = DateTime.Now.AddSeconds(timeout);
            CancellationChangeToken expirationToken = new(new CancellationTokenSource(TimeSpan.FromSeconds(timeout)).Token);
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                // Pin to cache.
                .SetPriority(CacheItemPriority.NeverRemove)
                // Set the actual expiration time
                .SetAbsoluteExpiration(expirationTime)
                // Force eviction to run
                .AddExpirationToken(expirationToken);

            this.MemoryCache.Set(key, cache, cacheEntryOptions);
        }

        private void Set<T>(String key,
                            T cache,
                            MemoryCacheEntryOptions cacheEntryOptions)
        {
            this.MemoryCache.Set(key, cache, cacheEntryOptions);
        }

        private T TryGetValue<T>(String Key)
        {
            if (this.MemoryCache.TryGetValue(Key, out T cachedItem))
            {
                return cachedItem;
            }

            return default;
        }

        #endregion
    }
}
