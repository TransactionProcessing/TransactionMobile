namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Web;
using System.Windows.Input;
using Common;
using Maui.UIServices;
using MediatR;
using Microsoft.Maui.Controls;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Requests;
using Services;
using UIServices;

public class MobileTopupSelectProductPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.OperatorIdentifier = HttpUtility.UrlDecode(query[nameof(OperatorIdentifier)].ToString());
    }

    public MobileTopupSelectProductPageViewModel(IMediator mediator, INavigationService navigationService,
                                                 IApplicationCache applicationCache, IDialogService dialogService) :base(applicationCache, dialogService, navigationService)
    {
        this.Mediator = mediator;
        this.ProductSelectedCommand = new AsyncCommand<ItemSelected<ContractProductModel>>(this.ProductSelectedCommandExecute);
        this.Title = "Select a Product";
    }

    #endregion

    #region Properties

    public String OperatorIdentifier { get; private set; }

    public List<ContractProductModel> Products { get; private set; }

    public ICommand ProductSelectedCommand { get; }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.MobileTopup);

        List<ContractProductModel> products = await this.Mediator.Send(request, cancellationToken);

        products = products.Where(p => p.OperatorIdentfier == this.OperatorIdentifier).ToList();

        this.Products = products;
    }

    private async Task ProductSelectedCommandExecute(ItemSelected<ContractProductModel> e)
    {
        Shared.Logger.Logger.LogInformation("ProductSelectedCommandExecute called");
        await this.NavigationService.GoToMobileTopupPerformTopupPage(e.SelectedItem.OperatorIdentfier, e.SelectedItem.ContractId, e.SelectedItem.ProductId, e.SelectedItem.Value);
    }

    #endregion
}