namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Common;
using Maui.UIServices;
using MediatR;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using RequestHandlers;
using Requests;
using Services;
using UIServices;

public class MobileTopupSelectOperatorPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public MobileTopupSelectOperatorPageViewModel(IMediator mediator, INavigationService navigationService, IDialogService dialogService,
                                                  IApplicationCache applicationCache): base(applicationCache,dialogService, navigationService)
    {
        this.Mediator = mediator;
        this.OperatorSelectedCommand = new AsyncCommand<ItemSelected<ContractOperatorModel>>(this.OperatorSelectedCommandExecute);
        this.Title = "Select an Operator";
    }

    #endregion

    #region Properties

    public List<ContractOperatorModel> Operators { get; private set; }

    public ICommand OperatorSelectedCommand { get; }

    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.MobileTopup);

        Result<List<ContractProductModel>> productsresult = await this.Mediator.Send(request, cancellationToken);
        List<ContractProductModel> products = productsresult.Data;

        // TODO: Should this logic live in the Request handler ???
        List<ContractOperatorModel> operators = products.GroupBy(c => new
            {
                c.OperatorName,
                c.OperatorId,
                c.OperatorIdentfier
            }).Select(g => new ContractOperatorModel
                           {
                               OperatorId = g.Key.OperatorId,
                               OperatorName = g.Key.OperatorName,
                               OperatorIdentfier = g.Key.OperatorIdentfier
                           }).ToList();

        this.Operators = operators;
    }

    private async Task OperatorSelectedCommandExecute(ItemSelected<ContractOperatorModel> e)
    {
        Logger.LogInformation("OperatorSelectedCommandExecute called");
        ProductDetails productDetails = new ProductDetails() {
                                                                 OperatorIdentifier = e.SelectedItem.OperatorIdentfier
                                                             };

        await this.NavigationService.GoToMobileTopupSelectProductPage(productDetails);

    }

    #endregion
}