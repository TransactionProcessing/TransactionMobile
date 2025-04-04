namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Web;
using System.Windows.Input;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Maui.Controls;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using RequestHandlers;
using Requests;
using Services;
using SimpleResults;
using UIServices;

public class BillPaymentSelectProductPageViewModel : ExtendedBaseViewModel, IQueryAttributable
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public void ApplyQueryAttributes(IDictionary<string, Object> query)
    {
        this.ProductDetails = query[nameof(this.ProductDetails)] as ProductDetails;
    }

    public BillPaymentSelectProductPageViewModel(IMediator mediator, INavigationService navigationService,
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

    public ProductDetails ProductDetails { get; private set; }

    public List<ContractProductModel> Products { get; private set; }

    public ICommand ProductSelectedCommand { get; }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.BillPayment);

        Result<List<ContractProductModel>> productsresult = await this.Mediator.Send(request, cancellationToken);
        // TODO: Handle the failure result
        List<ContractProductModel> products = productsresult.Data;
        products = products.Where(p => p.OperatorIdentfier == this.ProductDetails.OperatorIdentifier).ToList();

        this.Products = products;
    }

    private async Task ProductSelectedCommandExecute(ItemSelected<ContractProductModel> e)
    {
        Logger.LogInformation("ProductSelectedCommandExecute called");
        ProductDetails productDetails = new()
                                        {
                                            OperatorIdentifier = e.SelectedItem.OperatorIdentfier,
                                            ContractId = e.SelectedItem.ContractId,
                                            ProductId = e.SelectedItem.ProductId
                                        };
        Task task = e.SelectedItem.ProductSubType switch{
            ProductSubType.BillPaymentPostPay => this.NavigationService.GoToBillPaymentGetAccountPage(productDetails),
            _ => this.NavigationService.GoToBillPaymentGetMeterPage(productDetails)
        };

        await task;
    }

    #endregion
}