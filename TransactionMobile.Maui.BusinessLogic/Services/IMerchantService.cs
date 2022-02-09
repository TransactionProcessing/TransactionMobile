namespace TransactionMobile.Maui.BusinessLogic.Services;

using Models;

public interface IMerchantService
{
    #region Methods

    Task<List<ContractProductModel>> GetContractProducts(String accessToken,
                                                         Guid estateId,
                                                         Guid merchantId,
                                                         CancellationToken cancellationToken);

    Task<Decimal> GetMerchantBalance(String accessToken,
                                     Guid estateId,
                                     Guid merchantId,
                                     CancellationToken cancellationToken);

    #endregion
}