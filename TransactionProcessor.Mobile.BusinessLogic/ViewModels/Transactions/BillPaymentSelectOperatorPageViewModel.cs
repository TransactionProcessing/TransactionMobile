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

public class BillPaymentSelectOperatorPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public BillPaymentSelectOperatorPageViewModel(IMediator mediator, INavigationService navigationService,
                                                  IApplicationCache applicationCache,
                                                  IDialogService dialogService,
                                                  IDeviceService deviceService,
                                                  INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService,navigationParameterService)

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
        GetContractProductsRequest request = GetContractProductsRequest.Create(ProductType.BillPayment);

        Result<List<ContractProductModel>> productsresult = await this.Mediator.Send(request, cancellationToken);
        // TODO: Handle the failure result
        List<ContractProductModel> products = productsresult.Data;

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
        Logger.LogInformation("OperatorSelectedCommandExecute called");

        ProductDetails productDetails = new() {
                                                  OperatorId = e.SelectedItem.OperatorId
                                              };

        await this.NavigationService.GoToBillPaymentSelectProductPage(productDetails);
    }

    #endregion
}