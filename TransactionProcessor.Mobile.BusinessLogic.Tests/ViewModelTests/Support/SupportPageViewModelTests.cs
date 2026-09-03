using System.Text;
using MediatR;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Support
{
    [Collection("ViewModelTests")]
    public class SupportPageViewModelTests
    {
        private readonly INavigationServiceImposter NavigationService;

        private readonly INavigationParameterServiceImposter NavigationParameterService;

        private readonly IDatabaseContextImposter DatabaseContext;

        private readonly IMediatorImposter Mediator;

        private readonly IDeviceServiceImposter DeviceService;

        private readonly IApplicationInfoServiceImposter ApplicationInfoService;

        private readonly IApplicationCacheImposter ApplicationCache;

        private readonly IDialogServiceImposter DialogService;

        private readonly SupportPageViewModel ViewModel;
        public SupportPageViewModelTests() {
            this.NavigationService = new INavigationServiceImposter();
            this.NavigationParameterService = new INavigationParameterServiceImposter();
            this.DatabaseContext = new IDatabaseContextImposter();
            this.Mediator = new IMediatorImposter();
            this.DeviceService = new IDeviceServiceImposter();
            this.ApplicationInfoService = new IApplicationInfoServiceImposter();
            this.ApplicationCache = new IApplicationCacheImposter();
            this.DialogService = new IDialogServiceImposter();
            this.DialogService.ShowUploadLogsCompleteNotice().Returns(Task.CompletedTask);
            this.ViewModel = new SupportPageViewModel(this.DeviceService.Instance(),
                                                      this.ApplicationInfoService.Instance(),
                                                      this.DatabaseContext.Instance(),
                                                      this.Mediator.Instance(),
                                                      this.NavigationService.Instance(),
                                                      this.ApplicationCache.Instance(),
                                                      this.DialogService.Instance(),
                                                      this.NavigationParameterService.Instance());
        }

        [Fact]
        public async Task SupportPageViewModel_UploadLogsCommand_Execute_IsExecuted()
        {
            this.Mediator.Send(Arg<IRequest<Result>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());
            await this.ViewModel.UploadLogsCommand.ExecuteAsync(null);

            this.Mediator.Send(Arg<IRequest<Result>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
            this.DialogService.ShowUploadLogsCompleteNotice().Called(Count.Once());
            this.NavigationService.GoBack().Called(Count.Never());
        }

        [Fact]
        public void SupportPageViewModel_ViewLogsCommand_Execute_IsExecuted()
        {
            this.ViewModel.ViewLogsCommand.Execute(null);

            this.NavigationService.GoToViewLogsPage().Called(Count.Once());
        }

        [Fact]
        public void SupportPageViewModel_Platform_ValueIsReturned(){
            this.DeviceService.GetPlatform().Returns("Platform");
            this.DeviceService.GetManufacturer().Returns("Manufacturer");
            this.DeviceService.GetModel().Returns("Model");

            String platform = this.ViewModel.Platform;

            StringBuilder expectedPlatform = new();
            expectedPlatform.Append("Platform: ").AppendLine("Platform");
            expectedPlatform.Append("Manufacturer: ").AppendLine("Manufacturer");
            expectedPlatform.Append("Device: ").AppendLine("Model");

            platform.ShouldBe(expectedPlatform.ToString());
        }

        [Fact]
        public void SupportPageViewModel_NumberTransactionsStored_ValueIsReturned()
        {
            this.DatabaseContext.GetTransactions(Arg<Boolean>.Any()).ReturnsAsync(new List<TransactionRecord>{
                                                                                                                                new TransactionRecord()

                                                                                                                            });
            String numberTransactionsStored = this.ViewModel.NumberTransactionsStored;

            String expectedNumberTransactionsStored = $"Transactions Stored: 1";

            numberTransactionsStored.ShouldBe(expectedNumberTransactionsStored);
        }

        [Fact]
        public void SupportPageViewModel_ApplicationName_ValueIsReturned()
        {
            this.ApplicationInfoService.ApplicationName.Getter().Returns("ApplicationName");
            this.ApplicationInfoService.VersionString.Getter().Returns("VersionString");

            String applicationName = this.ViewModel.ApplicationName;

            String expectedApplicationName = $"ApplicationName vVersionString";

            applicationName.ShouldBe(expectedApplicationName);
        }

        [Fact]
        public void SupportPageViewModel_BackButtonCommand_Execute_IsExecuted()
        {
            this.ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.GoToHome().Called(Count.Once());
        }
    }
}
