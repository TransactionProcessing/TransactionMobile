namespace TransactionMobile.Maui.BusinessLogic.Services;

using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses;
using Models;
using Newtonsoft.Json;

public class MerchantService : IMerchantService
{
    private readonly IEstateClient EstateClient;

    private readonly IApplicationCache ApplicationCache;
    
    public MerchantService(IEstateClient estateClient, IApplicationCache applicationCache)
    {
        this.EstateClient = estateClient;
        this.ApplicationCache = applicationCache;
    }
    public async Task<List<ContractProductModel>> GetContractProducts(CancellationToken cancellationToken)
    {
        List<ContractProductModel> result = new List<ContractProductModel>();

        TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
        Guid estateId = this.ApplicationCache.GetEstateId();
        Guid merchantId = this.ApplicationCache.GetMerchantId();

        Shared.Logger.Logger.LogInformation($"About to request merchant contracts");
        Shared.Logger.Logger.LogDebug($"Merchant Contract Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

        List<ContractResponse> merchantContracts = await this.EstateClient.GetMerchantContracts(accessToken.AccessToken, estateId, merchantId, cancellationToken);

        Shared.Logger.Logger.LogInformation($"{merchantContracts.Count} for merchant requested successfully");
        Shared.Logger.Logger.LogDebug($"Merchant Contract Response: [{JsonConvert.SerializeObject(merchantContracts)}]");

        foreach (ContractResponse contractResponse in merchantContracts)
        {
            foreach (ContractProduct contractResponseProduct in contractResponse.Products)
            {
                result.Add(new ContractProductModel
                           {
                               OperatorId = contractResponse.OperatorId,
                               ContractId = contractResponse.ContractId,
                               ProductId = contractResponseProduct.ProductId,
                               OperatorIdentfier = contractResponse.OperatorName,
                               OperatorName = this.GetOperatorName(contractResponse, contractResponseProduct),
                               Value = contractResponseProduct.Value ?? 0,
                               IsFixedValue = contractResponseProduct.Value.HasValue,
                               ProductDisplayText = contractResponseProduct.DisplayText,
                               ProductType = this.GetProductType(contractResponse.OperatorName)
                           });
            }
        }

        return result;
    }

    private String GetOperatorName(ContractResponse contractResponse, ContractProduct contractProduct)
    {
        String operatorName = null;
        ProductType productType = this.GetProductType(contractResponse.OperatorName);
        switch (productType)
        {
            case ProductType.Voucher:
                operatorName = contractResponse.Description;
                break;
            default:
                operatorName = contractResponse.OperatorName;
                break;

        }

        return operatorName;
    }

    private ProductType GetProductType(String operatorName)
    {
        ProductType productType = ProductType.NotSet;
        switch (operatorName)
        {
            case "Safaricom":
                productType = ProductType.MobileTopup;
                break;
            case "Voucher":
                productType = ProductType.Voucher;
                break;
        }

        return productType;
    }

    public async Task<Decimal> GetMerchantBalance(CancellationToken cancellationToken)
    {
        TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
        Guid estateId = this.ApplicationCache.GetEstateId();
        Guid merchantId = this.ApplicationCache.GetMerchantId();

        Shared.Logger.Logger.LogInformation($"About to request merchant merchant balance");
        Shared.Logger.Logger.LogDebug($"Merchant Balance Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

        MerchantBalanceResponse merchantBalance = await this.EstateClient.GetMerchantBalance(accessToken.AccessToken, estateId, merchantId, cancellationToken);

        Shared.Logger.Logger.LogInformation($"Balance for merchant requested successfully");
        Shared.Logger.Logger.LogDebug($"Merchant Balance Response: [{JsonConvert.SerializeObject(merchantBalance)}]");

        return merchantBalance.AvailableBalance;
    }

    public Task<MerchantDetailsModel> GetMerchantDetails(CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}