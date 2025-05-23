namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Transactions.Voucher;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Common;
using Common;
using Logging;
using Maui.UIServices;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using SimpleResults;
using UIServices;
using ViewModels;
using ViewModels.Transactions;
using Xunit;

[Collection("ViewModelTests")]
public class VoucherSelectOperatorPageViewModelTests
{
    private readonly Mock<IMediator> Mediator;

    private readonly Mock<INavigationService> NavigationService;
    private Mock<INavigationParameterService> NavigationParameterService;
    private readonly Mock<IApplicationCache> ApplicationCache;

    private readonly Mock<IDialogService> DialogSevice;

    private readonly VoucherSelectOperatorPageViewModel ViewModel;

    private readonly Mock<IDeviceService> DeviceService;

    public VoucherSelectOperatorPageViewModelTests() {
        this.Mediator = new Mock<IMediator>();
        this.NavigationService = new Mock<INavigationService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogSevice = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.ViewModel = new VoucherSelectOperatorPageViewModel(this.Mediator.Object, this.NavigationService.Object, this.ApplicationCache.Object,
                                                                this.DialogSevice.Object, this.DeviceService.Object,
                                                                this.NavigationParameterService.Object);

    }
    [Fact]
    public async Task VoucherSelectOperatorPageViewModel_Initialise_IsInitialised()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));
        

        await this.ViewModel.Initialise(CancellationToken.None);
        this.Mediator.Verify(x => x.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>()), Times.Once);

        this.ViewModel.Operators.Count.ShouldBe(3);
    }

    [Fact]
    public async Task VoucherSelectOperatorPageViewModel_OperatorSelectedCommand_Execute_IsExecuted()
    {
        this.Mediator.Setup(m => m.Send(It.IsAny<GetContractProductsRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.ContractProductList));
        
        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.Operators.Count.ShouldBe(3);

        ItemSelected<ContractOperatorModel> selectedContractOperator = new ItemSelected<ContractOperatorModel>
                                                                       {
                                                                           SelectedItemIndex = 1,
                                                                           SelectedItem = TestData.ContractOperatorModel
                                                                       };

        this.ViewModel.OperatorSelectedCommand.Execute(selectedContractOperator);

        this.NavigationService.Verify(n => n.GoToVoucherSelectProductPage(It.IsAny<ProductDetails>()), Times.Once);
    }

    [Fact]
    public void VoucherSelectOperatorPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.Verify(v => v.GoBack(), Times.Once);
    }
}