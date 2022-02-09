namespace TransactionMobile.Maui.ViewModels.Transactions;

using System.Windows.Input;
using BusinessLogic.Models;
using BusinessLogic.Requests;
using MediatR;
using MvvmHelpers;
using MvvmHelpers.Commands;

public class MobileTopupSelectOperatorPageViewModel : BaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public MobileTopupSelectOperatorPageViewModel(IMediator mediator)
    {
        this.Mediator = mediator;
        this.OperatorSelectedCommand = new AsyncCommand<SelectedItemChangedEventArgs>(this.OperatorSelectedCommandExecute);
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
        GetContractProductsRequest request = GetContractProductsRequest.Create("", App.EstateId, App.MerchantId);

        List<ContractProductModel> products = await this.Mediator.Send(request, cancellationToken);

        // TODO: Should this logic live in the Reqest handler ???
        List<ContractOperatorModel> operators = products.Where(c => c.ProductType == ProductType.MobileTopup).GroupBy(c => new
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

    private async Task OperatorSelectedCommandExecute(SelectedItemChangedEventArgs e)
    {
        ContractOperatorModel operatorModel = e.SelectedItem as ContractOperatorModel;
        await Shell.Current.GoToAsync($"{nameof(MobileTopupSelectProductPage)}?OperatorIdentifier={operatorModel.OperatorIdentfier}");
    }

    #endregion
}