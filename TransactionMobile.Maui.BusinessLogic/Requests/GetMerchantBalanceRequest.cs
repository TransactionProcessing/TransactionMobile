namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;
using TransactionMobile.Maui.BusinessLogic.Models;

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

public class GetMerchantDetailsRequest : IRequest<MerchantDetailsModel>
{
    public GetMerchantDetailsRequest() {
        
    }

    public static GetMerchantDetailsRequest Create() {
        return new GetMerchantDetailsRequest();
    }
}