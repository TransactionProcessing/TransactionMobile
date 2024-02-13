﻿namespace TransactionMobile.Maui.BusinessLogic.Services.TrainingModeServices;

using Models;
using RequestHandlers;
using ViewModels.Transactions;

public class TrainingTransactionService : ITransactionService
{
    public async Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model,
                                                                      CancellationToken cancellationToken) {
        return new SuccessResult<PerformLogonResponseModel>(new PerformLogonResponseModel {
                                                                                              EstateId = Guid.Parse("D7E52254-E0BE-436A-9A34-CC291DA0D66A"),
                                                                                              MerchantId = Guid.Parse("DD034A3B-D8EE-45A4-A29F-8774751CEE76"),
                                                                                              ResponseCode = "0000",
                                                                                              ResponseMessage = "SUCCESS"
                                                                                          });
    }

    public async Task<Result<PerformMobileTopupResponseModel>> PerformMobileTopup(PerformMobileTopupRequestModel model,
                                                                                  CancellationToken cancellationToken) {
        PerformMobileTopupResponseModel responseModel = new() {
                                                                  ResponseCode = "0000",
                                                                  ResponseMessage = "SUCCESS"
                                                              };


        return new SuccessResult<PerformMobileTopupResponseModel>(responseModel);
    }

    public async Task<Result<PerformReconciliationResponseModel>> PerformReconciliation(PerformReconciliationRequestModel model,
                                                                                        CancellationToken cancellationToken) {
        return new SuccessResult<PerformReconciliationResponseModel>(new PerformReconciliationResponseModel {
                                                                                                                ResponseMessage = "SUCCESS",
                                                                                                                ResponseCode = "0000"
                                                                                                            });
    }

    public async Task<Result<PerformVoucherIssueResponseModel>> PerformVoucherIssue(PerformVoucherIssueRequestModel model,
                                                                                    CancellationToken cancellationToken) {
        PerformVoucherIssueResponseModel responseMessage = new PerformVoucherIssueResponseModel
                                                           {
                                                               ResponseCode = "0000",
                                                               ResponseMessage = "SUCCESS"
                                                           };
            
        return new SuccessResult<PerformVoucherIssueResponseModel>(responseMessage);
    }

    public async Task<Result<PerformBillPaymentGetAccountResponseModel>> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model,
                                                                                                      CancellationToken cancellationToken) {
        PerformBillPaymentGetAccountResponseModel responseMessage = new PerformBillPaymentGetAccountResponseModel
                                                                    {
                                                                        BillDetails = new BillDetails
                                                                                      {
                                                                                          AccountName = "Mr Test Customer",
                                                                                          AccountNumber = model.CustomerAccountNumber,
                                                                                          Balance = "100.00",
                                                                                          DueDate = DateTime.Now.AddDays(3).ToString("dd-MM-yyyy"),

                                                                                      },
                                                                        IsSuccessful = true
                                                                    };


        return new SuccessResult<PerformBillPaymentGetAccountResponseModel>(responseMessage);
    }

    public async Task<Result<PerformBillPaymentMakePaymentResponseModel>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model,
                                                                                                        CancellationToken cancellationToken) {
        PerformBillPaymentMakePaymentResponseModel responseMessage = new PerformBillPaymentMakePaymentResponseModel
                                                                     {
                                                                         ResponseCode = "0000",
                                                                         ResponseMessage = "SUCCESS"
                                                                     };


        return new SuccessResult<PerformBillPaymentMakePaymentResponseModel>(responseMessage);
    }

    public async Task<Result<PerformBillPaymentGetMeterResponseModel>> PerformBillPaymentGetMeter(PerformBillPaymentGetMeterModel model, CancellationToken cancellationToken){
        PerformBillPaymentGetMeterResponseModel responseMessage = new PerformBillPaymentGetMeterResponseModel
        {
                                                                      MeterDetails = new MeterDetails()
                                                                                    {
                                                                                        CustomerName = "Mr Test Customer",
                                                                                        MeterNumber = model.MeterNumber
                                                                                    },
                                                                      IsSuccessful = true
                                                                  };


        return new SuccessResult<PerformBillPaymentGetMeterResponseModel>(responseMessage);
    }
}