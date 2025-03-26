﻿using TransactionProcessor.Client;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;

namespace TransactionMobile.Maui.BusinessLogic.Services;

using System.Diagnostics.CodeAnalysis;
using Common;
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
    
    private readonly ITransactionProcessorClient TransactionProcessorClient;

    #endregion

    #region Constructors

    public MerchantService(ITransactionProcessorClient transactionProcessorClient,
                           IApplicationCache applicationCache) {
        this.TransactionProcessorClient = transactionProcessorClient;
        this.ApplicationCache = applicationCache;
    }

    #endregion

    #region Methods

    public async Task<Result<List<ContractProductModel>>> GetContractProducts(CancellationToken cancellationToken) {
        try {
            List<ContractProductModel> models = new List<ContractProductModel>();

            TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();
            Guid estateId = this.ApplicationCache.GetEstateId();
            Guid merchantId = this.ApplicationCache.GetMerchantId();

            Logger.LogInformation("About to request merchant contracts");
            Logger.LogDebug($"Merchant Contract Request details:  Estate Id {estateId} Merchant Id {merchantId} Access Token {accessToken.AccessToken}");

            Result<List<ContractResponse>> result = await this.TransactionProcessorClient.GetMerchantContracts(accessToken.AccessToken, estateId, merchantId, cancellationToken);

            if (result.IsFailed) {
                Logger.LogInformation($"GetMerchantContracts failed {result.Status}");
                return Result.Failure(result.Message);
            }

            Logger.LogInformation($"{result.Data.Count} for merchant requested successfully");
            Logger.LogDebug($"Merchant Contract Response: [{JsonConvert.SerializeObject(result.Data)}]");

            foreach (ContractResponse contractResponse in result.Data) {
                foreach (ContractProduct contractResponseProduct in contractResponse.Products){
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
        catch(Exception ex) {
            Logger.LogError("Error getting contract products",ex);

            return ResultExtensions.FailureExtended("Error getting contract products", ex);
        }
    }

    [ExcludeFromCodeCoverage(Justification = "Need to have a think of best way of this data being retieved")]
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

            MerchantResponse merchantResponse = await this.TransactionProcessorClient.GetMerchant(accessToken.AccessToken, estateId, merchantId, cancellationToken);

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

    public static ProductSubType GetProductSubType(String operatorName)
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