namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.MyAccount;

using System.Threading;
using System.Threading.Tasks;
using Maui.UIServices;
using MediatR;
using Moq;
using Services;
using Shared.Logger;
using Shouldly;
using ViewModels.MyAccount;
using Xunit;

public class MyAccountAddressPageViewModelTests
{
    [Fact]
    public async Task MyAccountAddressPageViewModel_Initialise_IsInitialised()
    {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        applicationCache.Setup(a => a.GetMerchantDetails()).Returns(TestData.MerchantDetailsModel);
        Mock<IMediator> mediator = new Mock<IMediator>();

        MyAccountAddressPageViewModel viewModel = new MyAccountAddressPageViewModel(navigationService.Object,
                                                                                    applicationCache.Object,
                                                                                    mediator.Object);
        await viewModel.Initialise(CancellationToken.None);

        applicationCache.Verify(a => a.GetMerchantDetails(), Times.Once);
        viewModel.Address.ShouldNotBeNull();
        viewModel.Address.AddressLine1.ShouldBe(TestData.AddressLine1);
        viewModel.Address.AddressLine2.ShouldBe(TestData.AddressLine2);
        viewModel.Address.AddressLine3.ShouldBe(TestData.AddressLine3);
        viewModel.Address.AddressLine4.ShouldBe(TestData.AddressLine4);
        viewModel.Address.Region.ShouldBe(TestData.Region);
        viewModel.Address.Town.ShouldBe(TestData.Town);
        viewModel.Address.PostalCode.ShouldBe(TestData.PostalCode);
    }
}