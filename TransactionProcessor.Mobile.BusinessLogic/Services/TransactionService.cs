using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Serialisation;
using TransactionProcessorACL.DataTransferObjects;
using TransactionProcessorACL.DataTransferObjects.Responses;

namespace TransactionProcessor.Mobile.BusinessLogic.Services
{
    public interface ITransactionService
    {
        #region Methods

        Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformMobileTopupResponseModel>> PerformMobileTopup(PerformMobileTopupRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformReconciliationResponseModel>> PerformReconciliation(PerformReconciliationRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformVoucherIssueResponseModel>> PerformVoucherIssue(PerformVoucherIssueRequestModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentGetAccountResponseModel>> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentMakePaymentResponseModel>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model, CancellationToken cancellationToken);

        Task<Result<PerformBillPaymentGetMeterResponseModel>> PerformBillPaymentGetMeter(PerformBillPaymentGetMeterModel model, CancellationToken cancellationToken);

        #endregion
    }

    public class TransactionService : ClientProxyBase.ClientProxyBase, ITransactionService
    {
        #region Fields

        private readonly IApplicationCache ApplicationCache;

        private readonly Func<String, String> BaseAddressResolver;

        #endregion

        #region Constructors

        public TransactionService(Func<String, String> baseAddressResolver,
                                  HttpClient httpClient,
                                  IApplicationCache applicationCache, Func<Object, String> serialise, 
                                  Func<String, Type, Object> deserialise) : base(httpClient, serialise, deserialise) {
            this.BaseAddressResolver = baseAddressResolver;
            this.ApplicationCache = applicationCache;
        }

        #endregion

        #region Methods

        public async Task<Result<PerformBillPaymentGetAccountResponseModel>> PerformBillPaymentGetAccount(PerformBillPaymentGetAccountModel model,
                                                                                                          CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform bill payment get account transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), "api/saletransactions", cancellationToken);

            if (result.IsSuccess == false) {
                Logger.LogWarning("Error performing bill payment - get account transaction");
                return Result.Failure("Error performing bill payment - get account transaction");
            }

            PerformBillPaymentGetAccountResponseModel responseModel = Factory.ToPerformBillPaymentGetAccountResponseModel(result.Data);

            Logger.LogInformation("Bill payment - get account transaction performed successfully");

            return Result.Success(responseModel);
        }

        public async Task<Result<PerformBillPaymentMakePaymentResponseModel>> PerformBillPaymentMakePayment(PerformBillPaymentMakePaymentModel model,
                                                                                                            CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform bill payment make payment transaction");
            
            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(),"api/saletransactions", cancellationToken);

            if (result.IsSuccess == false) {
                Logger.LogWarning("Error performing bill payment - make payment transaction");
                return Result.Failure("Error performing bill payment - make payment transaction");
            }

            PerformBillPaymentMakePaymentResponseModel responseModel = Factory.ToPerformBillPaymentMakePaymentResponseModel(result.Data);

            Logger.LogInformation("Bill payment - make payment transaction performed successfully");

            return Result.Success(responseModel);
        }

        public async Task<Result<PerformBillPaymentGetMeterResponseModel>> PerformBillPaymentGetMeter(PerformBillPaymentGetMeterModel model, CancellationToken cancellationToken){
            Logger.LogInformation("About to perform bill payment get meter transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), "api/saletransactions", cancellationToken);

            if (result.IsSuccess == false) {
                Logger.LogWarning("Error performing bill payment - get meter transaction");
                return Result.Failure("Error performing bill payment - get meter transaction");
            }

            PerformBillPaymentGetMeterResponseModel responseModel = Factory.ToPerformBillPaymentGetMeterResponseModel(result.Data, model.MeterNumber);

            Logger.LogInformation("Bill payment - get meter transaction performed successfully");

            return Result.Success(responseModel);
        }

        public async Task<Result<PerformLogonResponseModel>> PerformLogon(PerformLogonRequestModel model,
                                                                          CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform logon transaction");

            Result<LogonTransactionResponseMessage> result =  await this.SendTransactionRequest<LogonTransactionRequestMessage, LogonTransactionResponseMessage>(model.ToLogonTransactionRequest(), "api/logontransactions", cancellationToken);

            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing Logon transaction");
                return Result.Failure("Error performing Logon transaction");
            }

            PerformLogonResponseModel responseModel = Factory.ToPerformLogonResponseModel(result.Data);

            Logger.LogInformation("Logon transaction performed successfully");

            return Result.Success(responseModel);
        }

        public async Task<Result<PerformMobileTopupResponseModel>> PerformMobileTopup(PerformMobileTopupRequestModel model,
                                                                                      CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform mobile top-up transaction");

            Result<SaleTransactionResponseMessage> result =
                await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), "api/saletransactions", cancellationToken);

            if (result.IsSuccess == false) {
                Logger.LogWarning("Error performing Mobile top-up transaction");
                return Result.Failure("Error performing Mobile top-up transaction");
            }

            PerformMobileTopupResponseModel responseModel = Factory.ToPerformMobileTopupResponseModel(result.Data);

            Logger.LogInformation("Mobile top-up transaction performed successfully");

            return Result.Success(responseModel);

        }

        public async Task<Result<PerformReconciliationResponseModel>> PerformReconciliation(PerformReconciliationRequestModel model,
                                                                                            CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform reconciliation transaction");

            Result<ReconciliationResponseMessage> result = await this.SendTransactionRequest<ReconciliationRequestMessage, ReconciliationResponseMessage>(model.ToReconciliationRequest(), "api/reconciliationtransactions", cancellationToken);
            
            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing Reconciliation transaction");
                return Result.Failure("Error performing Reconciliation transaction");
            }

            PerformReconciliationResponseModel responseModel = Factory.ToPerformReconciliationResponseModel(result.Data);

            Logger.LogInformation("Reconciliation transaction performed successfully");

            return Result.Success(responseModel);

        }

        public async Task<Result<PerformVoucherIssueResponseModel>> PerformVoucherIssue(PerformVoucherIssueRequestModel model,
                                                                                      CancellationToken cancellationToken) {
            Logger.LogInformation("About to perform voucher transaction");

            Result<SaleTransactionResponseMessage> result = await this.SendTransactionRequest<SaleTransactionRequestMessage, SaleTransactionResponseMessage>(model.ToSaleTransactionRequest(), "api/saletransactions", cancellationToken);
            
            if (result.IsSuccess == false)
            {
                Logger.LogWarning("Error performing Voucher transaction");
                return Result.Failure("Error performing Voucher transaction");
            }

            PerformVoucherIssueResponseModel responseModel = Factory.ToPerformVoucherIssueResponseModel(result.Data);

            Logger.LogInformation("Voucher transaction performed successfully");

            return Result.Success(responseModel);
        }

        private String BuildRequestUrl(String route) {
            String baseAddress = this.BaseAddressResolver("TransactionProcessorACL");

            String requestUri = $"{baseAddress}/{route}";

            return requestUri;
        }

        private async Task<Result<TResponse>> SendTransactionRequest<TRequest, TResponse>(TRequest request,
                                                                                          String route, 
                                                                                          CancellationToken cancellationToken) {
            String requestUri = this.BuildRequestUrl(route);
            try {
                String requestSerialised = StringSerialiser.Serialise(request);
                
                // Add the access token to the client headers
                TokenResponseModel accessToken = this.ApplicationCache.GetAccessToken();

                Logger.LogDebug($"Transaction Request details: Uri {requestUri} Payload {requestSerialised} Access Token {accessToken.AccessToken}");

                Result<TResponse>? result = await this.Post<TRequest, TResponse>(requestUri, request, accessToken.AccessToken, cancellationToken);

                if (result.IsSuccess == false) {
                    Logger.LogWarning("Error performing transaction");
                    return Result.Failure("Error performing transaction");
                }

                Logger.LogDebug($"Transaction Response details: Payload {StringSerialiser.Serialise(result.Data)}");

                return Result.Success(result.Data);
            }
            catch (Exception ex) {
                // An exception has occurred, add some additional information to the message
                Logger.LogError("Error posting transaction.", ex);

                return ResultExtensions.FailureExtended("Error posting transaction", ex);
            }
        }

        #endregion
    }
}
