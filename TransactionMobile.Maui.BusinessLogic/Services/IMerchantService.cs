namespace TransactionMobile.Maui.BusinessLogic.Services;

using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses;
using Microsoft.Extensions.Caching.Memory;
using Models;

public interface IMerchantService
{
    #region Methods

    Task<List<ContractProductModel>> GetContractProducts(CancellationToken cancellationToken);

    Task<Decimal> GetMerchantBalance(CancellationToken cancellationToken);

    Task<MerchantDetailsModel> GetMerchantDetails(CancellationToken cancellationToken);

    #endregion
}
