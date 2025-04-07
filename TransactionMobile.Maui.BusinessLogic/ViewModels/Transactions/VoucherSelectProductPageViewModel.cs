namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Models;
using MvvmHelpers.Commands;
using Requests;
using Services;
using UIServices;

public class VoucherSelectProductPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    public ProductDetails ProductDetails { get; private set; }

    #endregion

    #region Constructors
    
    public VoucherSelectProductPageViewModel(IMediator mediator,
                                             INavigationService navigationService,
                                             IApplicationCache applicationCache,
                                             IDialogService dialogService,
                                             IDeviceService deviceService,
                                             INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
    {
        this.Mediator = mediator;
        this.ProductSelectedCommand = new AsyncCommand<ItemSelected<ContractProductModel>>(this.ProductSelectedCommandExecute);
        this.Title = "Select a Product";
    }

    #endregion

    #region Properties

    public List<ContractProductModel> Products { get; private set; }

    public ICommand ProductSelectedCommand { get; }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        IDictionary<String, Object> query = this.NavigationParameterService.GetParameters();
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;

        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.Voucher);

        var result = await this.Mediator.Send(request, cancellationToken);
        var products = result.Data;
        products = products.Where(p => p.OperatorId == this.ProductDetails.OperatorId).ToList();

        this.Products = products;
    }

    private async Task ProductSelectedCommandExecute(ItemSelected<ContractProductModel> e)
    {
        Logger.LogInformation("ProductSelectedCommandExecute called");
        ProductDetails productDetails = new ProductDetails()
                                        {
                                            OperatorId = e.SelectedItem.OperatorId,
                                            ContractId = e.SelectedItem.ContractId,
                                            ProductId = e.SelectedItem.ProductId
                                        };
        await this.NavigationService.GoToVoucherIssueVoucherPage(productDetails, e.SelectedItem.Value);
    }

    #endregion
}