namespace TransactionMobile.Maui.BusinessLogic.Services;

using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses;
using Logging;
using Models;
using Newtonsoft.Json;
using RequestHandlers;
using ViewModels;

public class MerchantService : IMerchantService
{
    #region Fields

    private readonly IApplicationCache ApplicationCache;

    private readonly ILoggerService Logger;

    private readonly IEstateClient EstateClient;

    #endregion

    #region Constructors

    public MerchantService(ILoggerService logger,
        IEstateClient estateClient,
                           IApplicationCache applicationCache) {
        this.Logger = logger;
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

            await Logger.LogInformation("About to request merchant contracts");
            await Logger.LogDebug($"Merchant Contract Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

            List<ContractResponse> merchantContracts = await this.EstateClient.GetMerchantContracts(accessToken.AccessToken, estateId, merchantId, cancellationToken);

            await Logger.LogInformation($"{merchantContracts.Count} for merchant requested successfully");
            await Logger.LogDebug($"Merchant Contract Response: [{JsonConvert.SerializeObject(merchantContracts)}]");

            foreach (ContractResponse contractResponse in merchantContracts) {
                foreach (ContractProduct contractResponseProduct in contractResponse.Products) {
                    result.Add(new ContractProductModel {
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

            return new SuccessResult<List<ContractProductModel>>(result);
        }
        catch(Exception ex) {
            await Logger.LogError("Error getting contract products",ex);

            return new ErrorResult<List<ContractProductModel>>("Error getting contract products");
        }
    }

    public async Task<Result<Decimal>> GetMerchantBalance(CancellationToken cancellationToken) {
        try {
            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
            Guid estateId = this.ApplicationCache.GetEstateId();
            Guid merchantId = this.ApplicationCache.GetMerchantId();


            await Logger.LogInformation("About to request merchant balance");
            await Logger.LogDebug($"Merchant Balance Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

            //MerchantBalanceResponse merchantBalance = new MerchantBalanceResponse {
            //                                                                          AvailableBalance = 0,
            //                                                                          Balance = 0
            //                                                                      };
            //await this.EstateClient.GetMerchantBalance(accessToken.AccessToken, estateId, merchantId, cancellationToken);

            await Logger.LogInformation("Balance for merchant requested successfully");
            //Logger.LogDebug($"Merchant Balance Response: [{JsonConvert.SerializeObject(merchantBalance)}]");

            return new SuccessResult<Decimal>(0);
        }
        catch(Exception ex) {
            await Logger.LogError("Error getting merchant balance",ex);
            return new ErrorResult<Decimal>("Error getting merchant balance");
        }
    }

    public async Task<Result<MerchantDetailsModel>> GetMerchantDetails(CancellationToken cancellationToken) {
        try {
            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
            Guid estateId = this.ApplicationCache.GetEstateId();
            Guid merchantId = this.ApplicationCache.GetMerchantId();

            await Logger.LogInformation("About to request merchant details");
            await Logger.LogDebug($"Merchant Details Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

            MerchantResponse merchantResponse = await this.EstateClient.GetMerchant(accessToken.AccessToken, estateId, merchantId, cancellationToken);

            await Logger.LogInformation("Merchant details requested successfully");
            await Logger.LogDebug($"Merchant Details Response: [{JsonConvert.SerializeObject(merchantResponse)}]");

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

            return new SuccessResult<MerchantDetailsModel>(model);
        }
        catch(Exception ex) {
            await Logger.LogError("Error getting merchant details",ex);
            return new ErrorResult<MerchantDetailsModel>("Error getting merchant details");
        }
    }

    private String GetOperatorName(ContractResponse contractResponse,
                                   ContractProduct contractProduct) {
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
                productType = ProductType.BillPayment;
                break;
        }

        return productType;
    }

    #endregion
}