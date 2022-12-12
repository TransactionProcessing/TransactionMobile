namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;
using RequestHandlers;

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