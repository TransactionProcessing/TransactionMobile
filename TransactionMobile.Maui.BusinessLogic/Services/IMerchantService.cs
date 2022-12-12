﻿namespace TransactionMobile.Maui.BusinessLogic.Services;

using EstateManagement.Client;
using EstateManagement.DataTransferObjects.Responses;
using Microsoft.Extensions.Caching.Memory;
using Models;
using RequestHandlers;

public interface IMerchantService
{
    #region Methods

    Task<Result<List<ContractProductModel>>> GetContractProducts(CancellationToken cancellationToken);

    Task<Result<Decimal>> GetMerchantBalance(CancellationToken cancellationToken);

    Task<Result<MerchantDetailsModel>> GetMerchantDetails(CancellationToken cancellationToken);

    #endregion
}
