using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Reports;

public class RecentActivityReceiptDetailPageViewModelTests
{
    private readonly Mock<INavigationService> NavigationService;
    private readonly Mock<IApplicationCache> ApplicationCache;
    private readonly Mock<IDialogService> DialogService;
    private readonly Mock<IDeviceService> DeviceService;
    private readonly Mock<INavigationParameterService> NavigationParameterService;
    private readonly Mock<IReportsService> ReportsService;
    private readonly RecentActivityReceiptDetailPageViewModel ViewModel;

    public RecentActivityReceiptDetailPageViewModelTests()
    {
        this.NavigationService = new Mock<INavigationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.DialogService = new Mock<IDialogService>();
        this.DeviceService = new Mock<IDeviceService>();
        this.NavigationParameterService = new Mock<INavigationParameterService>();
        this.ReportsService = new Mock<IReportsService>();
        this.ViewModel = new RecentActivityReceiptDetailPageViewModel(this.NavigationService.Object,
                                                                       this.ApplicationCache.Object,
                                                                       this.DialogService.Object,
                                                                       this.DeviceService.Object,
                                                                       this.NavigationParameterService.Object,
                                                                       this.ReportsService.Object);
    }

    [Fact]
    public async Task Initialise_LoadsSelectedItemFromNavigationParameters()
    {
        RecentActivityReceiptItemModel item = new("TXN-10001",
                                                  "Mobile Topup",
                                                  "Custom",
                                                  "Safaricom",
                                                  "Success",
                                                  100.00m,
                                                  new DateTime(2026, 7, 6, 9, 30, 0),
                                                  "RCPT-10001");

        this.NavigationParameterService.Setup(p => p.GetParameters()).Returns(new Dictionary<string, object>
        {
            { nameof(RecentActivityReceiptItemModel), item }
        });

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.Reference.ShouldBe("TXN-10001");
        this.ViewModel.ReceiptReference.ShouldBe("RCPT-10001");
        this.ViewModel.HasReceipt.ShouldBeTrue();
    }

    [Fact]
    public async Task ResendReceiptCommand_SendsReceiptAndShowsSuccessMessage()
    {
        RecentActivityReceiptItemModel item = new("TXN-10001",
                                                  "Mobile Topup",
                                                  "Custom",
                                                  "Safaricom",
                                                  "Success",
                                                  100.00m,
                                                  new DateTime(2026, 7, 6, 9, 30, 0),
                                                  "RCPT-10001");

        this.NavigationParameterService.Setup(p => p.GetParameters()).Returns(new Dictionary<string, object>
        {
            { nameof(RecentActivityReceiptItemModel), item }
        });

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.EmailAddress = "customer@example.com";
        this.ReportsService.Setup(r => r.ResendRecentActivityReceipt("TXN-10001",
                                                                     "customer@example.com",
                                                                     It.IsAny<CancellationToken>()))
                           .ReturnsAsync(Result.Success(new RecentActivityReceiptResendResultModel
                           {
                               Success = true,
                               Message = "Receipt resend requested.",
                               Reference = "TXN-10001"
                           }));

        await this.ViewModel.ResendReceiptCommand.ExecuteAsync(null);

        this.ReportsService.Verify(r => r.ResendRecentActivityReceipt("TXN-10001",
                                                                      "customer@example.com",
                                                                      It.IsAny<CancellationToken>()),
                                   Times.Once);
        this.DialogService.Verify(d => d.ShowSuccessToast("Receipt resend requested. Sent to customer@example.com.", null, "OK", null, It.IsAny<CancellationToken>()), Times.Once);
        this.ViewModel.EmailAddress.ShouldBe(string.Empty);
    }

    [Fact]
    public async Task ResendReceiptCommand_ShowsWarningForInvalidEmail()
    {
        RecentActivityReceiptItemModel item = new("TXN-10001",
                                                  "Mobile Topup",
                                                  "Custom",
                                                  "Safaricom",
                                                  "Success",
                                                  100.00m,
                                                  new DateTime(2026, 7, 6, 9, 30, 0),
                                                  "RCPT-10001");

        this.NavigationParameterService.Setup(p => p.GetParameters()).Returns(new Dictionary<string, object>
        {
            { nameof(RecentActivityReceiptItemModel), item }
        });

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.EmailAddress = "invalid-email";

        await this.ViewModel.ResendReceiptCommand.ExecuteAsync(null);

        this.DialogService.Verify(d => d.ShowWarningToast("Enter a valid email address before resending the receipt.", null, "OK", null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ResendReceiptCommand_ShowsWarningWhenApiFails()
    {
        RecentActivityReceiptItemModel item = new("TXN-10001",
                                                  "Mobile Topup",
                                                  "Custom",
                                                  "Safaricom",
                                                  "Success",
                                                  100.00m,
                                                  new DateTime(2026, 7, 6, 9, 30, 0),
                                                  "RCPT-10001");

        this.NavigationParameterService.Setup(p => p.GetParameters()).Returns(new Dictionary<string, object>
        {
            { nameof(RecentActivityReceiptItemModel), item }
        });

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.EmailAddress = "customer@example.com";
        this.ReportsService.Setup(r => r.ResendRecentActivityReceipt("TXN-10001",
                                                                     "customer@example.com",
                                                                     It.IsAny<CancellationToken>()))
                           .ReturnsAsync(Result.Failure("Receipt resend failed."));

        await this.ViewModel.ResendReceiptCommand.ExecuteAsync(null);

        this.DialogService.Verify(d => d.ShowWarningToast("Receipt resend failed.", null, "OK", null, It.IsAny<CancellationToken>()), Times.Once);
    }
}
