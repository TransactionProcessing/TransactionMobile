namespace TransactionMobile.Maui.BusinessLogic.Services;

using Microsoft.Extensions.Caching.Memory;
using Models;

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