namespace TransactionMobile.Maui.BusinessLogic.Requests;

using MediatR;
using Models;

public class GetMerchantDetailsRequest : IRequest<MerchantDetailsModel>
{
    #region Constructors

    #endregion

    #region Methods

    public static GetMerchantDetailsRequest Create() {
        return new GetMerchantDetailsRequest();
    }

    #endregion
}