namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Web;
using System.Windows.Input;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Requests;
using Services;
using UIServices;

public class VoucherSelectProductPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields

    private readonly IMediator Mediator;

    public ProductDetails ProductDetails { get; private set; }

    #endregion

    #region Constructors

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
    }

    public VoucherSelectProductPageViewModel(IMediator mediator,
                                             INavigationService navigationService,
                                             IApplicationCache applicationCache,
                                             IDialogService dialogService,
                                             IDeviceService deviceService) : base(applicationCache, dialogService, navigationService, deviceService)
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
        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.Voucher);

        var result = await this.Mediator.Send(request, cancellationToken);
        var products = result.Data;
        products = products.Where(p => p.OperatorIdentfier == this.ProductDetails.OperatorIdentifier).ToList();

        this.Products = products;
    }

    private async Task ProductSelectedCommandExecute(ItemSelected<ContractProductModel> e)
    {
        Logger.LogInformation("ProductSelectedCommandExecute called");
        ProductDetails productDetails = new ProductDetails()
                                        {
                                            OperatorIdentifier = e.SelectedItem.OperatorIdentfier,
                                            ContractId = e.SelectedItem.ContractId,
                                            ProductId = e.SelectedItem.ProductId
                                        };
        await this.NavigationService.GoToVoucherIssueVoucherPage(productDetails, e.SelectedItem.Value);
    }

    #endregion
}