using LiveChartsCore.Measure;
using MediatR;
using Moq;
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
        private readonly Mock<INavigationService> NavigationService;
        private readonly Mock<INavigationParameterService> NavigationParameterService;
        private readonly Mock<IApplicationCache> ApplicationCache;
        private readonly Mock<IDialogService> DialogService;
        private readonly Mock<IDeviceService> DeviceService;
        private readonly Mock<IMediator> Mediator;

        private readonly ReportsPageViewModel ViewModel;

        public ReportsPageViewModelTests(){
            this.NavigationService = new Mock<INavigationService>();
            this.NavigationParameterService = new Mock<INavigationParameterService>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.DialogService = new Mock<IDialogService>();
            this.DeviceService = new Mock<IDeviceService>();
            this.Mediator = new Mock<IMediator>();

            this.ViewModel = new ReportsPageViewModel(this.NavigationService.Object,
                                                      this.ApplicationCache.Object,
                                                      this.DialogService.Object,
                                                      this.DeviceService.Object,
                                                      this.Mediator.Object,
                                                      this.NavigationParameterService.Object);
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
                    this.NavigationService.Verify(v => v.GoToDailyPerformanceSummaryPage(), Times.Once);
                    break;
                default:
                    this.NavigationService.Verify(v => v.GoToDailyPerformanceSummaryPage(), Times.Never);
                    break;
            }
        }

        [Fact]
        public async Task ReportsPageViewModel_Initialise_IsInitialised()
        {
            await this.ViewModel.Initialise(CancellationToken.None);
            this.ViewModel.ReportsMenuOptions.Count.ShouldBe(2);
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

            this.NavigationService.Verify(v => v.GoToTransactionMixSummaryPage(), Times.Once);
        }

        [Fact]
        public async Task ReportsPageViewModel_BackButtonCommand_HomePageIsShown()
        {
            this.ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }
}
