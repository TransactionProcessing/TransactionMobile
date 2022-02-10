namespace TransactionMobile.Maui.ViewModels.Transactions;

using System.Windows.Input;
using BusinessLogic.Models;
using BusinessLogic.Requests;
using MediatR;
using MvvmHelpers;
using MvvmHelpers.Commands;
using UIServices;

[QueryProperty(nameof(MobileTopupSelectProductPageViewModel.OperatorIdentifier), nameof(MobileTopupSelectProductPageViewModel.OperatorIdentifier))]
public class MobileTopupSelectProductPageViewModel : BaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    private readonly INavigationService NavigationService;

    #endregion

    #region Constructors

    public MobileTopupSelectProductPageViewModel(IMediator mediator, INavigationService navigationService)
    {
        this.Mediator = mediator;
        this.NavigationService = navigationService;
        this.ProductSelectedCommand = new AsyncCommand<SelectedItemChangedEventArgs>(this.ProductSelectedCommandExecute);
        this.Title = "Select a Product";
    }

    #endregion

    #region Properties

    public String OperatorIdentifier { get; set; }

    public List<ContractProductModel> Products { get; private set; }

    public ICommand ProductSelectedCommand { get; }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        GetContractProductsRequest request = GetContractProductsRequest.Create("",App.EstateId,App.MerchantId);

        List<ContractProductModel> products = await this.Mediator.Send(request, cancellationToken);

        products = products.Where(p => p.OperatorIdentfier == this.OperatorIdentifier).ToList();

        this.Products = products;
    }

    private async Task ProductSelectedCommandExecute(SelectedItemChangedEventArgs e)
    {
        ContractProductModel productModel = e.SelectedItem as ContractProductModel;
        await this.NavigationService.GoToMobileTopupPerformTopupPage(productModel.OperatorIdentfier, productModel.ContractId, productModel.ProductId, productModel.Value);
    }

    #endregion
}