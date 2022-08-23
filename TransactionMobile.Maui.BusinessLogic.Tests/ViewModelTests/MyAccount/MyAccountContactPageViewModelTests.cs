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

public class MyAccountContactPageViewModelTests
{
    [Fact]
    public async Task MyAccountContactPageViewModel_Initialise_IsInitialised()
    {
        Logger.Initialise(NullLogger.Instance);
        Mock<INavigationService> navigationService = new Mock<INavigationService>();
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        applicationCache.Setup(a => a.GetMerchantDetails()).Returns(TestData.MerchantDetailsModel);
        Mock<IMediator> mediator = new Mock<IMediator>();

        MyAccountContactPageViewModel viewModel = new MyAccountContactPageViewModel(navigationService.Object,
                                                                                    applicationCache.Object,
                                                                                    mediator.Object);
        await viewModel.Initialise(CancellationToken.None);

        applicationCache.Verify(a => a.GetMerchantDetails(), Times.Once);
        viewModel.Contact.ShouldNotBeNull();
        viewModel.Contact.EmailAddress.ShouldBe(TestData.ContactEmailAddress);
        viewModel.Contact.Name.ShouldBe(TestData.ContactName);
        viewModel.Contact.MobileNumber.ShouldBe(TestData.ContactMobileNumber);
    }
}