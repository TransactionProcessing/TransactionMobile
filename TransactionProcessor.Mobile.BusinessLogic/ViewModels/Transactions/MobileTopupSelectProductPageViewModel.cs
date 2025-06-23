using System.Windows.Input;
using MediatR;
using MvvmHelpers.Commands;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public class MobileTopupSelectProductPageViewModel : ExtendedBaseViewModel//, IQueryAttributableX
{
    #region Fields

    private readonly IMediator Mediator;
    
    public ProductDetails ProductDetails { get; private set; }

    #endregion

    #region Constructors


    public MobileTopupSelectProductPageViewModel(IMediator mediator, INavigationService navigationService,
                                                 IApplicationCache applicationCache, IDialogService dialogService,
                                                 IDeviceService deviceService,
                                                 INavigationParameterService navigationParameterService) :base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService)
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

        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.MobileTopup);

        Result<List<ContractProductModel>> productsResult = await this.Mediator.Send(request, cancellationToken);
        List<ContractProductModel> products = productsResult.Data;

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

        await this.NavigationService.GoToMobileTopupPerformTopupPage(productDetails, e.SelectedItem.Value);
    }

    #endregion
}