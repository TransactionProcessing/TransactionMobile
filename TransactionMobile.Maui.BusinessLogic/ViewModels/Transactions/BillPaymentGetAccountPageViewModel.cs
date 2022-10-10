namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Maui.UIServices;
using MediatR;
using Models;
using MvvmHelpers.Commands;
using Requests;
using Services;
using Shared.Logger;
using UIServices;

public class BillPaymentGetAccountPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields

    private String customerAccountNumber;

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public BillPaymentGetAccountPageViewModel(INavigationService navigationService,
                                              IApplicationCache applicationCache,
                                              IDialogService dialogService,
                                              IMediator mediator) : base(applicationCache, dialogService, navigationService) {
        this.Mediator = mediator;
        this.GetAccountCommand = new AsyncCommand(this.GetAccountCommandExecute);
        this.Title = "Get Customer Account";
    }

    #endregion

    #region Properties

    public String CustomerAccountNumber {
        get => this.customerAccountNumber;
        set => this.SetProperty(ref this.customerAccountNumber, value);
    }

    public ICommand GetAccountCommand { get; }

    public ProductDetails ProductDetails { get; set; }

    #endregion

    #region Methods

    public void ApplyQueryAttributes(IDictionary<String, Object> query) {
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
    }

    private async Task GetAccountCommandExecute() {
        Logger.LogInformation("GetAccountCommandExecute called");

        PerformBillPaymentGetAccountRequest request = PerformBillPaymentGetAccountRequest.Create(DateTime.Now,
                                                                                                 this.ProductDetails.ContractId,
                                                                                                 this.ProductDetails.ProductId,
                                                                                                 this.ProductDetails.OperatorIdentifier,
                                                                                                 this.CustomerAccountNumber);

        PerformBillPaymentGetAccountResponseModel response = await this.Mediator.Send(request);

        if (response.IsSuccessful) {
            ProductDetails productDetails = new ProductDetails {
                                                                   ContractId = this.ProductDetails.ContractId,
                                                                   ProductId = this.ProductDetails.ProductId,
                                                                   OperatorIdentifier = this.ProductDetails.OperatorIdentifier
                                                               };

            await this.NavigationService.GoToBillPaymentPayBillPage(productDetails, response.BillDetails);
        }
        else {
            await this.NavigationService.GoToBillPaymentFailedPage();
        }
    }

    #endregion
}

public class BillDetails
{
    #region Properties

    public String AccountName { get; set; }

    public String AccountNumber { get; set; }

    public String Balance { get; set; }

    public String DueDate { get; set; }

    #endregion
}

public class ProductDetails
{
    #region Properties

    public Guid ContractId { get; set; }

    public String OperatorIdentifier { get; set; }

    public Guid ProductId { get; set; }

    #endregion
}