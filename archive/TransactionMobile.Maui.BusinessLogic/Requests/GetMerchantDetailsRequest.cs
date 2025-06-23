namespace TransactionMobile.Maui.BusinessLogic.Requests;

using Common;
using MediatR;
using Models;
using RequestHandlers;
using SimpleResults;

public class GetMerchantDetailsRequest : IRequest<Result<MerchantDetailsModel>>
{
    #region Constructors

    #endregion

    #region Methods

    public static GetMerchantDetailsRequest Create() {
        return new GetMerchantDetailsRequest();
    }

    #endregion
}