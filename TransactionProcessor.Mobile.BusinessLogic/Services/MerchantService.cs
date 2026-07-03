using System.Diagnostics.CodeAnalysis;
using Shared.Results;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Serialisation;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessorACL.DataTransferObjects.Responses;
using ProductType = TransactionProcessor.Mobile.BusinessLogic.Models.ProductType;

namespace TransactionProcessor.Mobile.BusinessLogic.Services;

public interface IMerchantService
{
    #region Methods

    Task<Result<List<ContractProductModel>>> GetContractProducts(CancellationToken cancellationToken);

    Task<Result<Decimal>> GetMerchantBalance(CancellationToken cancellationToken);

    Task<Result<MerchantDetailsModel>> GetMerchantDetails(CancellationToken cancellationToken);

    #endregion
}
public class MerchantService : ClientProxyBase.ClientProxyBase, IMerchantService
{
    #region Fields

    private readonly IApplicationCache ApplicationCache;
    private readonly IApplicationInfoService ApplicationInfoService;

    private readonly Func<String, String> BaseAddressResolver;

    #endregion

    #region Constructors

    public MerchantService(Func<String, String> baseAddressResolver,
                           HttpClient httpClient,
                           IApplicationCache applicationCache,
                           IApplicationInfoService applicationInfoService, 
                           Func<Object, String> serialise, 
                           Func<String, Type, Object> deserialise) : base(httpClient, serialise, deserialise)
    {
        this.BaseAddressResolver = baseAddressResolver;
        this.ApplicationCache = applicationCache;
        this.ApplicationInfoService = applicationInfoService;
    }

    #endregion

    #region Methods

    private String BuildRequestUrl(String route)
    {
        String baseAddress = this.BaseAddressResolver("TransactionProcessorACL");

        String requestUri = $"{baseAddress}{route}";

        return requestUri;
    }

    public async Task<Result<List<ContractProductModel>>> GetContractProducts(CancellationToken cancellationToken) {
        List<ContractProductModel> models = new();

        TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

        String requestUri = this.BuildRequestUrl($"/api/merchants/contracts?applicationVersion={this.ApplicationInfoService.VersionString}");

        Logger.LogInformation("About to request merchant contracts");
        Logger.LogDebug($"Merchant Contract Request details: Access Token {accessToken.AccessToken}");

        Result<List<ContractResponse>>? responseDataResult = await this.Get<List<ContractResponse>>(requestUri, accessToken.AccessToken, cancellationToken);

        if (responseDataResult.IsFailed) {
            Logger.LogInformation($"GetMerchantContracts failed {responseDataResult.Status}");
            return ResultHelpers.CreateFailure(responseDataResult);
        }

        Logger.LogInformation($"{responseDataResult.Data.Count} for merchant requested successfully");
        Logger.LogDebug($"Merchant Contract Response: [{StringSerialiser.Serialise(responseDataResult.Data)}]");

        foreach (ContractResponse contractResponse in responseDataResult.Data) {
            foreach (ContractProduct contractResponseProduct in contractResponse.Products) {
                ProductType productType = GetProductType(contractResponse.OperatorName);

                models.Add(new ContractProductModel {
                    OperatorId = contractResponse.OperatorId,
                    ContractId = contractResponse.ContractId,
                    ProductId = contractResponseProduct.ProductId,
                    OperatorIdentfier = contractResponse.OperatorName,
                    OperatorName = GetOperatorName(contractResponse, productType),
                    Value = contractResponseProduct.Value ?? 0,
                    IsFixedValue = contractResponseProduct.Value.HasValue,
                    ProductDisplayText = contractResponseProduct.DisplayText,
                    ProductType = productType,
                    ProductSubType = GetProductSubType(contractResponse.OperatorName),
                });
            }
        }

        return Result.Success(models);
    }

    [ExcludeFromCodeCoverage(Justification = "Need to have a think of best way of this data being retrieved")]
    public async Task<Result<Decimal>> GetMerchantBalance(CancellationToken cancellationToken) {
        try {
            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
            
            Logger.LogInformation("About to request merchant balance");
            Logger.LogDebug($"Merchant Balance Request details: Access Token {accessToken.AccessToken}");

            Logger.LogInformation("Balance for merchant requested successfully");
            return Result.Success(0.0m);
        }
        catch(Exception ex) {
            Logger.LogError("Error getting merchant balance",ex);
            return ResultExtensions.FailureExtended("Error getting merchant balance", ex);
        }
    }

    public async Task<Result<MerchantDetailsModel>> GetMerchantDetails(CancellationToken cancellationToken) {
        TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

        String requestUri = this.BuildRequestUrl($"/api/merchants?applicationVersion={this.ApplicationInfoService.VersionString}");

        Logger.LogInformation("About to request merchant details");
        Logger.LogDebug($"Merchant Details Request details: Access Token {accessToken.AccessToken}");

        Result<MerchantResponse>? responseDataResult = await this.Get<MerchantResponse>(requestUri, accessToken.AccessToken, cancellationToken);

        if (responseDataResult.IsFailed)
        {
            Logger.LogInformation($"GetMerchantContracts failed {responseDataResult.Status}");
            return ResultHelpers.CreateFailure(responseDataResult);
        }

        Logger.LogInformation("Merchant details requested successfully");
        Logger.LogDebug($"Merchant Details Response: [{StringSerialiser.Serialise(responseDataResult.Data)}]");
        
        MerchantDetailsModel model = new MerchantDetailsModel {
                                                                  EstateId = responseDataResult.Data.EstateId,
                                                                  MerchantId = responseDataResult.Data.MerchantId,
                                                                  MerchantReportingId = responseDataResult.Data.MerchantReportingId,
                                                                  MerchantName = responseDataResult.Data.MerchantName,
                                                                  NextStatementDate = responseDataResult.Data.NextStatementDate,
                                                                  LastStatementDate = new DateTime(),
                                                                  SettlementSchedule = responseDataResult.Data.SettlementSchedule.ToString(),
                                                                  Contact = new ContactModel {
                                                                                                 Name = responseDataResult.Data.Contacts.First().ContactName,
                                                                                                 EmailAddress = responseDataResult.Data.Contacts.First().ContactEmailAddress,
                                                                                                 MobileNumber = responseDataResult.Data.Contacts.First().ContactPhoneNumber
                                                                                             },
                                                                  Address = new AddressModel {
                                                                                                 AddressLine3 = responseDataResult.Data.Addresses.First().AddressLine3,
                                                                                                 Town = responseDataResult.Data.Addresses.First().Town,
                                                                                                 AddressLine4 = responseDataResult.Data.Addresses.First().AddressLine4,
                                                                                                 PostalCode = responseDataResult.Data.Addresses.First().PostalCode,
                                                                                                 Region = responseDataResult.Data.Addresses.First().Region,
                                                                                                 AddressLine1 = responseDataResult.Data.Addresses.First().AddressLine1,
                                                                                                 AddressLine2 = responseDataResult.Data.Addresses.First().AddressLine2
                                                                                             }
                                                              };

        return Result.Success(model);
    }

    public static String GetOperatorName(ContractResponse contractResponse, ProductType productType) {
        String operatorName = null;
        switch(productType) {
            case ProductType.Voucher:
                operatorName = contractResponse.Description;
                break;
            default:
                operatorName = contractResponse.OperatorName;
                break;
        }

        return operatorName;
    }

    public static ProductType GetProductType(String operatorName) {
        return operatorName switch
        {
            "Safaricom" => ProductType.MobileTopup,
            "Voucher" => ProductType.Voucher,
            "PataPawa PostPay" => ProductType.BillPayment,
            "PataPawa PrePay" => ProductType.BillPayment,
            _ => ProductType.NotSet,
        };
    }

    public static ProductSubType GetProductSubType(String operatorName)
    {
        return operatorName switch
        {
            "Safaricom" => ProductSubType.MobileTopup,
            "Voucher" => ProductSubType.Voucher,
            "PataPawa PostPay" => ProductSubType.BillPaymentPostPay,
            "PataPawa PrePay" => ProductSubType.BillPaymentPrePay,
            _ => ProductSubType.NotSet,
        };
    }

    #endregion
}
