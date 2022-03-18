namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;

public class GetMerchantBalanceRequest : IRequest<Decimal>
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