using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Reports;

public class RecentActivityReceiptDetailPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;
    private readonly IApplicationCacheImposter ApplicationCache;
    private readonly IDialogServiceImposter DialogService;
    private readonly IDeviceServiceImposter DeviceService;
    private readonly INavigationParameterServiceImposter NavigationParameterService;
    private readonly IReportsServiceImposter ReportsService;
    private readonly RecentActivityReceiptDetailPageViewModel ViewModel;

    public RecentActivityReceiptDetailPageViewModelTests()
    {
        this.NavigationService = new INavigationServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.ReportsService = new IReportsServiceImposter();
        this.ViewModel = new RecentActivityReceiptDetailPageViewModel(this.NavigationService.Instance(),
                                                                       this.ApplicationCache.Instance(),
                                                                       this.DialogService.Instance(),
                                                                       this.DeviceService.Instance(),
                                                                       this.NavigationParameterService.Instance(),
                                                                       this.ReportsService.Instance());
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

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<string, object>
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

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<string, object>
        {
            { nameof(RecentActivityReceiptItemModel), item }
        });

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.EmailAddress = "customer@example.com";
        this.ReportsService.ResendRecentActivityReceipt("TXN-10001",
                                                                     "customer@example.com",
                                                                     Arg<CancellationToken>.Any())
                           .ReturnsAsync(Result.Success(new RecentActivityReceiptResendResultModel
                           {
                               Success = true,
                               Message = "Receipt resend requested.",
                               Reference = "TXN-10001"
                           }));

        await this.ViewModel.ResendReceiptCommand.ExecuteAsync(null);

        this.ReportsService.ResendRecentActivityReceipt("TXN-10001",
                                                                      "customer@example.com",
                                                                      Arg<CancellationToken>.Any()).Called(Count.Once());
        this.DialogService.ShowSuccessToast("Receipt resend requested. Sent to customer@example.com.", Arg<Action?>.Any(), "OK", Arg<TimeSpan?>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
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

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<string, object>
        {
            { nameof(RecentActivityReceiptItemModel), item }
        });

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.EmailAddress = "invalid-email";

        await this.ViewModel.ResendReceiptCommand.ExecuteAsync(null);

        this.DialogService.ShowWarningToast("Enter a valid email address before resending the receipt.", Arg<Action?>.Any(), "OK", Arg<TimeSpan?>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
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

        this.NavigationParameterService.GetParameters().Returns(new Dictionary<string, object>
        {
            { nameof(RecentActivityReceiptItemModel), item }
        });

        await this.ViewModel.Initialise(CancellationToken.None);

        this.ViewModel.EmailAddress = "customer@example.com";
        this.ReportsService.ResendRecentActivityReceipt("TXN-10001",
                                                                     "customer@example.com",
                                                                     Arg<CancellationToken>.Any())
                           .ReturnsAsync(Result.Failure("Receipt resend failed."));

        await this.ViewModel.ResendReceiptCommand.ExecuteAsync(null);

        this.DialogService.ShowWarningToast("Receipt resend failed.", Arg<Action?>.Any(), "OK", Arg<TimeSpan?>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}
