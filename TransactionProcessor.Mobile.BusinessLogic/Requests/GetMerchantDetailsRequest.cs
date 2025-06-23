using MediatR;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

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