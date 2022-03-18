﻿namespace TransactionMobile.Maui.BusinessLogic.Services;

using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses;
using Microsoft.Extensions.Caching.Memory;
using Models;

public interface IMerchantService
{
    #region Methods

    Task<List<ContractProductModel>> GetContractProducts(CancellationToken cancellationToken);

    Task<Decimal> GetMerchantBalance(CancellationToken cancellationToken);

    #endregion
}

public class MerchantService : IMerchantService
{
    private readonly IEstateClient EstateClient;

    private readonly IMemoryCacheService MemoryCacheService;

    public MerchantService(IEstateClient estateClient, IMemoryCacheService memoryCacheService)
    {
        this.EstateClient = estateClient;
        this.MemoryCacheService = memoryCacheService;
    }
    public async Task<List<ContractProductModel>> GetContractProducts(CancellationToken cancellationToken)
    {
        List<ContractProductModel> result = new List<ContractProductModel>();

        this.MemoryCacheService.TryGetValue<TokenResponseModel>("AccessToken", out TokenResponseModel accessToken);
        this.MemoryCacheService.TryGetValue<Guid>("EstateId", out Guid estateId);
        this.MemoryCacheService.TryGetValue<Guid>("MerchantId", out Guid merchantId);

        List<ContractResponse> merchantContracts = await this.EstateClient.GetMerchantContracts(accessToken.AccessToken, estateId, merchantId, cancellationToken);

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
        this.MemoryCacheService.TryGetValue<TokenResponseModel>("AccessToken", out TokenResponseModel accessToken);
        this.MemoryCacheService.TryGetValue<Guid>("EstateId", out Guid estateId);
        this.MemoryCacheService.TryGetValue<Guid>("MerchantId", out Guid merchantId);

        MerchantBalanceResponse merchantBalance = await this.EstateClient.GetMerchantBalance(accessToken.AccessToken, estateId, merchantId, cancellationToken);

        return merchantBalance.AvailableBalance;
    }
}