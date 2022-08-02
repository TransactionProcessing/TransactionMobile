namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Transactions;

using System.Windows.Input;
using Common;
using Maui.UIServices;
using MediatR;
using Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Requests;
using UIServices;

public class MobileTopupSelectOperatorPageViewModel : BaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    private readonly INavigationService NavigationService;

    #endregion

    #region Constructors

    public MobileTopupSelectOperatorPageViewModel(IMediator mediator, INavigationService navigationService)
    {
        this.Mediator = mediator;
        this.NavigationService = navigationService;
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

        List<ContractProductModel> products = await this.Mediator.Send(request, cancellationToken);

        // TODO: Should this logic live in the Reqest handler ???
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
        Shared.Logger.Logger.LogInformation("OperatorSelectedCommandExecute called");
        await this.NavigationService.GoToMobileTopupSelectProductPage(e.SelectedItem.OperatorIdentfier);

    }

    #endregion
}