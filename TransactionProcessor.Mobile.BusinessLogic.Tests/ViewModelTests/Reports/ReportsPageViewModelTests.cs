using LiveChartsCore.Measure;
using MediatR;
using Imposter.Abstractions;
using Shouldly;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Reports
{
    public class ReportsPageViewModelTests
    {
        private readonly INavigationServiceImposter NavigationService;
        private readonly INavigationParameterServiceImposter NavigationParameterService;
        private readonly IApplicationCacheImposter ApplicationCache;
        private readonly IDialogServiceImposter DialogService;
        private readonly IDeviceServiceImposter DeviceService;
        private readonly IMediatorImposter Mediator;

        private readonly ReportsPageViewModel ViewModel;

        public ReportsPageViewModelTests(){
            this.NavigationService = new INavigationServiceImposter();
            this.NavigationParameterService = new INavigationParameterServiceImposter();
            this.ApplicationCache = new IApplicationCacheImposter();
            this.DialogService = new IDialogServiceImposter();
            this.DeviceService = new IDeviceServiceImposter();
            this.Mediator = new IMediatorImposter();

            this.ViewModel = new ReportsPageViewModel(this.NavigationService.Instance(),
                                                      this.ApplicationCache.Instance(),
                                                      this.DialogService.Instance(),
                                                      this.DeviceService.Instance(),
                                                      this.Mediator.Instance(),
                                                      this.NavigationParameterService.Instance());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(99)]

        public async Task ReportsPageViewModel_OptionSelectedCommand_Execute_IsExecuted(Int32 selectedIndex){
            ListViewItem li = new ListViewItem{
                                                  Title = "Test"
                                              };
            ItemSelected<ListViewItem> itemSelected = new ItemSelected<ListViewItem>{
                                                                                        SelectedItem = li,
                                                                                        SelectedItemIndex = selectedIndex,
                                                                                    };
            this.ViewModel.OptionSelectedCommand.Execute(itemSelected);

            switch(selectedIndex){
                case 0:
                    this.NavigationService.GoToDailyPerformanceSummaryPage().Called(Count.Once());
                    break;
                default:
                    this.NavigationService.GoToDailyPerformanceSummaryPage().Called(Count.Never());
                    break;
            }
        }

        [Fact]
        public async Task ReportsPageViewModel_Initialise_IsInitialised()
        {
            await this.ViewModel.Initialise(CancellationToken.None);
            this.ViewModel.ReportsMenuOptions.Count.ShouldBe(3);
        }

        [Fact]
        public async Task ReportsPageViewModel_TransactionMixCommand_Execute_IsExecuted()
        {
            ItemSelected<ListViewItem> itemSelected = new ItemSelected<ListViewItem>
            {
                SelectedItem = new ListViewItem { Title = "Transaction Mix" },
                SelectedItemIndex = 1,
            };

            this.ViewModel.OptionSelectedCommand.Execute(itemSelected);

            this.NavigationService.GoToTransactionMixSummaryPage().Called(Count.Once());
        }

        [Fact]
        public async Task ReportsPageViewModel_RecentActivityCommand_Execute_IsExecuted()
        {
            ItemSelected<ListViewItem> itemSelected = new ItemSelected<ListViewItem>
            {
                SelectedItem = new ListViewItem { Title = "Recent Activity and Receipt Report" },
                SelectedItemIndex = 2,
            };

            this.ViewModel.OptionSelectedCommand.Execute(itemSelected);

            this.NavigationService.GoToRecentActivityReportPage().Called(Count.Once());
        }

        [Fact]
        public async Task ReportsPageViewModel_BackButtonCommand_HomePageIsShown()
        {
            this.ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.GoToHome().Called(Count.Once());
        }
    }
}
