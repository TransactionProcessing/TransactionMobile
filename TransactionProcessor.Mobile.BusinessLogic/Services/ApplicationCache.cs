using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Services
{
    public interface IApplicationCache
    {
        String GetConfigHostUrl();

        void SetConfigHostUrl(String value, MemoryCacheEntryOptions options = default);

        Boolean GetUseTrainingMode();

        void SetUseTrainingMode(Boolean value, MemoryCacheEntryOptions options = default);

        Boolean GetIsLoggedIn();

        void SetIsLoggedIn(Boolean value, MemoryCacheEntryOptions options = default);

        Configuration GetConfiguration();

        void SetConfiguration(Configuration value, MemoryCacheEntryOptions options = default);

        List<ContractProductModel> GetContractProducts();

        void SetContractProducts(List<ContractProductModel> value, MemoryCacheEntryOptions options = default);

        TokenResponseModel GetAccessToken();

        void SetAccessToken(TokenResponseModel value, MemoryCacheEntryOptions options = default);

        Guid GetEstateId();

        void SetEstateId(Guid value, MemoryCacheEntryOptions options = default);

        Guid GetMerchantId();

        void SetMerchantId(Guid value, MemoryCacheEntryOptions options = default);

        MerchantDetailsModel GetMerchantDetails();

        void SetMerchantDetails(MerchantDetailsModel value, MemoryCacheEntryOptions options = default);
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
            return this.TryGetValue<String>("ConfigHostUrl");
        }

        public void SetConfigHostUrl(String value,
                                     MemoryCacheEntryOptions options = default)
        {
            this.Set("ConfigHostUrl", value, options);
        }

        public Boolean GetUseTrainingMode()
        {
            return this.TryGetValue<Boolean>("UseTrainingMode");
        }

        public void SetAccessToken(TokenResponseModel value,
                                   MemoryCacheEntryOptions options = default)
        {
            this.Set("AccessToken", value, options);
        }

        public void SetConfiguration(Configuration value,
                                     MemoryCacheEntryOptions options = default)
        {
            this.Set("Configuration", value, options);
        }

        public void SetContractProducts(List<ContractProductModel> value,
                                        MemoryCacheEntryOptions options = default)
        {
            this.Set("ContractProducts", value, options);
        }

        public void SetEstateId(Guid value,
                                MemoryCacheEntryOptions options = default)
        {
            this.Set("EstateId", value, options);
        }

        public void SetIsLoggedIn(Boolean value,
                                  MemoryCacheEntryOptions options = default)
        {
            this.Set("isLoggedIn", value, options);
        }

        public void SetMerchantId(Guid value,
                                  MemoryCacheEntryOptions options = default)
        {
            this.Set("MerchantId", value, options);
        }

        public MerchantDetailsModel GetMerchantDetails()
        {
            return this.TryGetValue<MerchantDetailsModel>("MerchantDetails");
        }

        public void SetMerchantDetails(MerchantDetailsModel value,
                                       MemoryCacheEntryOptions options = default)
        {
            this.Set("MerchantDetails", value, options);
        }

        public void SetUseTrainingMode(Boolean value,
                                       MemoryCacheEntryOptions options = default)
        {
            this.Set("UseTrainingMode", value, options);
        }

        private void Set<T>(String key,
                            T cache,
                            MemoryCacheEntryOptions options)
        {
            this.MemoryCache.Set(key, cache, options);
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
