using CommunityToolkit.Mvvm.Input;
using MediatR;
using MvvmHelpers.Commands;
using SimpleResults;
using System.Windows.Input;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Transactions;

public partial class MobileTopupSelectOperatorPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public MobileTopupSelectOperatorPageViewModel(IMediator mediator, INavigationService navigationService, IDialogService dialogService,
                                                  IApplicationCache applicationCache,
                                                  IDeviceService deviceService,
                                                  INavigationParameterService navigationParameterService) : base(applicationCache,dialogService, navigationService, deviceService,navigationParameterService)
    {
        this.Mediator = mediator;
        this.Title = "Select an Operator";
    }

    #endregion

    #region Properties

    public List<ContractOperatorModel> Operators { get; private set; }
    
    #endregion

    #region Methods

    public async Task Initialise(CancellationToken cancellationToken)
    {
        MerchantQueries.GetProductOperatorsQuery request = new(ProductType.MobileTopup);
        Result<List<ContractOperatorModel>> operatorsResult = await this.Mediator.Send(request, cancellationToken);
        if (operatorsResult.IsFailed)
        {
            await this.DialogService.ShowWarningToast("Unable to load operators. Please try again later.", cancellationToken: cancellationToken);
            return;
        }
        this.Operators = operatorsResult.Data;
    }

    [RelayCommand]
    private async Task OperatorSelected(ItemSelected<ContractOperatorModel> e)
    {
        Logger.LogInformation("OperatorSelected called");
        ProductDetails productDetails = new ProductDetails() {
                                                                 OperatorId = e.SelectedItem.OperatorId
                                                             };

        await this.NavigationService.GoToMobileTopupSelectProductPage(productDetails);

    }

    #endregion
}