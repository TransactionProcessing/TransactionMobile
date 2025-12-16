using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using ClientProxyBase;
using Newtonsoft.Json;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
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
                           IApplicationInfoService applicationInfoService) : base(httpClient) {
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
        Guid estateId = this.ApplicationCache.GetEstateId();
        Guid merchantId = this.ApplicationCache.GetMerchantId();

        String requestUri = this.BuildRequestUrl($"/api/merchants/contracts?application_version={this.ApplicationInfoService.VersionString}");

        Logger.LogInformation("About to request merchant contracts");
        Logger.LogDebug($"Merchant Contract Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        var httpResponse = await this.HttpClient.SendAsync(request, cancellationToken);

        // Process the response
        Result<String> content = await this.HandleResponseX(httpResponse, cancellationToken);

        if (content.IsFailed) {
            Logger.LogInformation($"GetMerchantContracts failed {content.Status}");
            return Result.Failure(content.Message);
        }

        Logger.LogDebug($"Transaction Response details:  Status {httpResponse.StatusCode} Payload {content.Data}");

        List<ContractResponse>? responseData = JsonConvert.DeserializeObject<List<ContractResponse>>(content.Data);

        Logger.LogInformation($"{responseData.Count} for merchant requested successfully");
        Logger.LogDebug($"Merchant Contract Response: [{JsonConvert.SerializeObject(responseData)}]");

        foreach (ContractResponse contractResponse in responseData) {
            foreach (ContractProduct contractResponseProduct in contractResponse.Products) {
                var productType = GetProductType(contractResponse.OperatorName);

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
            Guid estateId = this.ApplicationCache.GetEstateId();
            Guid merchantId = this.ApplicationCache.GetMerchantId();


            Logger.LogInformation("About to request merchant balance");
            Logger.LogDebug($"Merchant Balance Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

            //MerchantBalanceResponse merchantBalance = new MerchantBalanceResponse {
            //                                                                          AvailableBalance = 0,
            //                                                                          Balance = 0
            //                                                                      };
            //await this.EstateClient.GetMerchantBalance(accessToken.AccessToken, estateId, merchantId, cancellationToken);

            Logger.LogInformation("Balance for merchant requested successfully");
            //Logger.LogDebug($"Merchant Balance Response: [{JsonConvert.SerializeObject(merchantBalance)}]");

            return Result.Success(0.0m);
        }
        catch(Exception ex) {
            Logger.LogError("Error getting merchant balance",ex);
            return ResultExtensions.FailureExtended("Error getting merchant balance", ex);
        }
    }

    public async Task<Result<MerchantDetailsModel>> GetMerchantDetails(CancellationToken cancellationToken) {
        TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
        Guid estateId = this.ApplicationCache.GetEstateId();
        Guid merchantId = this.ApplicationCache.GetMerchantId();


        String requestUri = this.BuildRequestUrl($"/api/merchants?application_version={this.ApplicationInfoService.VersionString}");

        Logger.LogInformation("About to request merchant details");
        Logger.LogDebug($"Merchant Details Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        var httpResponse = await this.HttpClient.SendAsync(request, cancellationToken);

        // Process the response
        Result<String> content = await this.HandleResponseX(httpResponse, cancellationToken);

        if (content.IsFailed)
        {
            Logger.LogInformation($"GetMerchantContracts failed {content.Status}");
            return Result.Failure(content.Message);
        }

        Logger.LogDebug($"Transaction Response details:  Status {httpResponse.StatusCode} Payload {content.Data}");

        //ResponseData<MerchantResponse> responseData = this.HandleResponseContent<MerchantResponse>(content.Data);
        MerchantResponse responseData = JsonConvert.DeserializeObject<MerchantResponse>(content.Data);
        Logger.LogInformation("Merchant details requested successfully");
        Logger.LogDebug($"Merchant Details Response: [{JsonConvert.SerializeObject(responseData)}]");

        MerchantDetailsModel model = new MerchantDetailsModel {
                                                                  MerchantName = responseData.MerchantName,
                                                                  NextStatementDate = responseData.NextStatementDate,
                                                                  LastStatementDate = new DateTime(),
                                                                  SettlementSchedule = responseData.SettlementSchedule.ToString(),
                                                                  //AvailableBalance = merchantResponse.AvailableBalance,
                                                                  //Balance = merchantResponse.Balance,
                                                                  Contact = new ContactModel {
                                                                                                 Name = responseData.Contacts.First().ContactName,
                                                                                                 EmailAddress = responseData.Contacts.First().ContactEmailAddress,
                                                                                                 MobileNumber = responseData.Contacts.First().ContactPhoneNumber
                                                                                             },
                                                                  Address = new AddressModel {
                                                                                                 AddressLine3 = responseData.Addresses.First().AddressLine3,
                                                                                                 Town = responseData.Addresses.First().Town,
                                                                                                 AddressLine4 = responseData.Addresses.First().AddressLine4,
                                                                                                 PostalCode = responseData.Addresses.First().PostalCode,
                                                                                                 Region = responseData.Addresses.First().Region,
                                                                                                 AddressLine1 = responseData.Addresses.First().AddressLine1,
                                                                                                 AddressLine2 = responseData.Addresses.First().AddressLine2
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