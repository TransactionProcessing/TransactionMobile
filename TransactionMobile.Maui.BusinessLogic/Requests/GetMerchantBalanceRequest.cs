﻿namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using RequestHandlers;
using SimpleResults;

public class GetMerchantBalanceRequest : IRequest<Result<Decimal>>
{
    #region Constructors

    private GetMerchantBalanceRequest()
    {

    }

    #endregion

    #region Properties
    #endregion

    #region Methods

    public static GetMerchantBalanceRequest Create()
    {
        return new GetMerchantBalanceRequest();
    }

    #endregion
}