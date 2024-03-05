namespace TransactionMobile.Maui.BusinessLogic.Services;

using Common;
using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses;
using Logging;
using Models;
using Newtonsoft.Json;
using RequestHandlers;
using SimpleResults;
using ViewModels;

public class MerchantService : IMerchantService
{
    #region Fields

    private readonly IApplicationCache ApplicationCache;
    
    private readonly IEstateClient EstateClient;

    #endregion

    #region Constructors

    public MerchantService(IEstateClient estateClient,
                           IApplicationCache applicationCache) {
        this.EstateClient = estateClient;
        this.ApplicationCache = applicationCache;
    }

    #endregion

    #region Methods

    public async Task<Result<List<ContractProductModel>>> GetContractProducts(CancellationToken cancellationToken) {
        try {
            List<ContractProductModel> result = new List<ContractProductModel>();

            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
            Guid estateId = this.ApplicationCache.GetEstateId();
            Guid merchantId = this.ApplicationCache.GetMerchantId();

            Logger.LogInformation("About to request merchant contracts");
            Logger.LogDebug($"Merchant Contract Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

            List<ContractResponse> merchantContracts = await this.EstateClient.GetMerchantContracts(accessToken.AccessToken, estateId, merchantId, cancellationToken);

            Logger.LogInformation($"{merchantContracts.Count} for merchant requested successfully");
            Logger.LogDebug($"Merchant Contract Response: [{JsonConvert.SerializeObject(merchantContracts)}]");

            foreach (ContractResponse contractResponse in merchantContracts) {
                foreach (ContractProduct contractResponseProduct in contractResponse.Products) {
                    result.Add(new ContractProductModel {
                                                            OperatorId = contractResponse.OperatorId,
                                                            ContractId = contractResponse.ContractId,
                                                            ProductId = contractResponseProduct.ProductId,
                                                            OperatorIdentfier = contractResponse.OperatorName,
                                                            OperatorName = this.GetOperatorName(contractResponse),
                                                            Value = contractResponseProduct.Value ?? 0,
                                                            IsFixedValue = contractResponseProduct.Value.HasValue,
                                                            ProductDisplayText = contractResponseProduct.DisplayText,
                                                            ProductType = this.GetProductType(contractResponse.OperatorName),
                                                            ProductSubType = this.GetProductSubType(contractResponse.OperatorName),
                                                        });
                }
            }

            return Result.Success(result);
        }
        catch(Exception ex) {
            Logger.LogError("Error getting contract products",ex);

            return ResultExtensions.FailureExtended("Error getting contract products", ex);
        }
    }

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
        try {
            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
            Guid estateId = this.ApplicationCache.GetEstateId();
            Guid merchantId = this.ApplicationCache.GetMerchantId();

            Logger.LogInformation("About to request merchant details");
            Logger.LogDebug($"Merchant Details Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

            MerchantResponse merchantResponse = await this.EstateClient.GetMerchant(accessToken.AccessToken, estateId, merchantId, cancellationToken);

            Logger.LogInformation("Merchant details requested successfully");
            Logger.LogDebug($"Merchant Details Response: [{JsonConvert.SerializeObject(merchantResponse)}]");

            MerchantDetailsModel model = new MerchantDetailsModel {
                                                                      MerchantName = merchantResponse.MerchantName,
                                                                      NextStatementDate = merchantResponse.NextStatementDate,
                                                                      LastStatementDate = new DateTime(),
                                                                      SettlementSchedule = merchantResponse.SettlementSchedule.ToString(),
                                                                      //AvailableBalance = merchantResponse.AvailableBalance,
                                                                      //Balance = merchantResponse.Balance,
                                                                      Contact = new ContactModel {
                                                                                                     Name = merchantResponse.Contacts.First().ContactName,
                                                                                                     EmailAddress = merchantResponse.Contacts.First().ContactEmailAddress,
                                                                                                     MobileNumber = merchantResponse.Contacts.First().ContactPhoneNumber
                                                                                                 },
                                                                      Address = new AddressModel {
                                                                                                     AddressLine3 = merchantResponse.Addresses.First().AddressLine3,
                                                                                                     Town = merchantResponse.Addresses.First().Town,
                                                                                                     AddressLine4 = merchantResponse.Addresses.First().AddressLine4,
                                                                                                     PostalCode = merchantResponse.Addresses.First().PostalCode,
                                                                                                     Region = merchantResponse.Addresses.First().Region,
                                                                                                     AddressLine1 = merchantResponse.Addresses.First().AddressLine1,
                                                                                                     AddressLine2 = merchantResponse.Addresses.First().AddressLine2
                                                                                                 }
                                                                  };

            return Result.Success(model);
        }
        catch(Exception ex) {
            Logger.LogError("Error getting merchant details",ex);
            return ResultExtensions.FailureExtended("Error getting merchant details", ex);
        }
    }

    private String GetOperatorName(ContractResponse contractResponse) {
        String operatorName = null;
        ProductType productType = this.GetProductType(contractResponse.OperatorName);
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

    private ProductType GetProductType(String operatorName) {
        ProductType productType = ProductType.NotSet;
        switch(operatorName) {
            case "Safaricom":
                productType = ProductType.MobileTopup;
                break;
            case "Voucher":
                productType = ProductType.Voucher;
                break;
            case "PataPawa PostPay":
            case "PataPawa PrePay":
                productType = ProductType.BillPayment;
                break;
        }

        return productType;
    }

    private ProductSubType GetProductSubType(String operatorName)
    {
        ProductSubType productType = ProductSubType.NotSet;
        switch (operatorName)
        {
            case "Safaricom":
                productType = ProductSubType.MobileTopup;
                break;
            case "Voucher":
                productType = ProductSubType.Voucher;
                break;
            case "PataPawa PostPay":
                productType = ProductSubType.BillPaymentPostPay;
                break;
            case "PataPawa PrePay":
                productType = ProductSubType.BillPaymentPrePay;
                break;
        }

        return productType;
    }

    #endregion
}