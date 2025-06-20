using MediatR;
using SimpleResults;

namespace TransactionProcessor.Mobile.BusinessLogic.Requests;

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