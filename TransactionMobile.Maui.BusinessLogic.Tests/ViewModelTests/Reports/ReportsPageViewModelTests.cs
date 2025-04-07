using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Reports
{
    using System.Threading;
    using BusinessLogic.Common;
    using LiveChartsCore.Measure;
    using Maui.UIServices;
    using MediatR;
    using Models;
    using Services;
    using Shouldly;
    using UIServices;
    using ViewModels.Reports;

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
                    this.NavigationService.Verify(v => v.GoToReportsSalesAnalysis(), Times.Once);
                    break;
                case 1:
                    this.NavigationService.Verify(v => v.GoToReportsBalanceAnalysis(), Times.Once);
                    break;
                default:
                    this.NavigationService.Verify(v => v.GoToReportsSalesAnalysis(), Times.Never);
                    this.NavigationService.Verify(v => v.GoToReportsBalanceAnalysis(), Times.Never);
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
        public async Task ReportsPageViewModel_BackButtonCommand_HomePageIsShown()
        {
            this.ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }

    public class ReportsSalesAnalysisPageViewModelTests{
        private readonly ReportsSalesAnalysisPageViewModel ViewModel;
        private readonly Mock<INavigationService> NavigationService;
        private readonly Mock<INavigationParameterService> NavigationParameterService;
        private readonly Mock<IApplicationCache> ApplicationCache;
        private readonly Mock<IDialogService> DialogService;
        private readonly Mock<IDeviceService> DeviceService;
        private readonly Mock<IMediator> Mediator;

        public ReportsSalesAnalysisPageViewModelTests(){
            this.NavigationService = new Mock<INavigationService>();
            this.NavigationParameterService = new Mock<INavigationParameterService>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.DialogService = new Mock<IDialogService>();
            this.DeviceService = new Mock<IDeviceService>();
            this.Mediator = new Mock<IMediator>();

            this.ViewModel = new ReportsSalesAnalysisPageViewModel(this.NavigationService.Object,
                                                                   this.ApplicationCache.Object,
                                                                   this.DialogService.Object,
                                                                   this.DeviceService.Object,
                                                                   this.Mediator.Object,
                                                                   this.NavigationParameterService.Object);
        }

        [Fact]
        public async Task ReportsSalesAnalysisPageViewModel_Initialise_Execute_IsExecuted(){
            await this.ViewModel.Initialise(CancellationToken.None);

            this.ViewModel.ComparisonDates.Count.ShouldBe(3);
        }

        [Fact]
        public async Task ReportsSalesAnalysisPageViewModel_ComparisonDatePickerSelectedIndexChangedCommand_HomePageIsShown(){
            ComparisonDate comparisonDate = new ComparisonDate(DateTime.Now.AddDays(-1), "Yesterday");
            this.ViewModel.SelectedItem = comparisonDate;
            this.ViewModel.ComparisonDatePickerSelectedIndexChangedCommand.Execute(comparisonDate);

            // Nothing to actually verify yet here
            this.ViewModel.SalesAnalysisList.Count.ShouldBe(2);
        }

        [Fact]
        public async Task ReportsSalesAnalysisPageViewModel_BackButtonCommand_HomePageIsShown()
        {
            this.ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.Verify(n => n.GoBack(), Times.Once);
        }
    }

    public class ReportsBalanceAnalysisPageViewModelTests
    {
        private ReportsBalanceAnalysisPageViewModel ViewModel;
        private Mock<INavigationService> NavigationService;
        private Mock<INavigationParameterService> NavigationParameterService;
        private Mock<IApplicationCache> ApplicationCache;
        private Mock<IDialogService> DialogService;
        private readonly Mock<IDeviceService> DeviceService;
        private Mock<IMediator> Mediator;

        public ReportsBalanceAnalysisPageViewModelTests()
        {
            this.NavigationService = new Mock<INavigationService>();
            this.NavigationParameterService = new Mock<INavigationParameterService>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.DialogService = new Mock<IDialogService>();
            this.DeviceService = new Mock<IDeviceService>();
            this.Mediator = new Mock<IMediator>();

            this.ViewModel = new ReportsBalanceAnalysisPageViewModel(this.NavigationService.Object,
                                                                     this.ApplicationCache.Object,
                                                                     this.DialogService.Object,
                                                                     this.DeviceService.Object,
                                                                     this.Mediator.Object,
                                                                     this.NavigationParameterService.Object);
        }

        [Fact]
        public async Task ReportsBalanceAnalysisPageViewModel_Initialise_Execute_IsExecuted()
        {
            await ViewModel.Initialise(CancellationToken.None);

            this.ViewModel.XAxes.ShouldNotBeNull();
            this.ViewModel.YAxes.ShouldNotBeNull();
            this.ViewModel.TooltipFindingStrategy.ShouldBe(TooltipFindingStrategy.CompareOnlyX);
            this.ViewModel.TooltipPosition.ShouldBe(TooltipPosition.Top);
            this.ViewModel.Series.ShouldNotBeNull();
        }

        //[Fact]
        //public async Task ReportsSalesAnalysisPageViewModel_ComparisonDatePickerSelectedIndexChangedCommand_HomePageIsShown()
        //{
        //    ComparisonDate comparisonDate = new ComparisonDate(DateTime.Now.AddDays(-1), "Yesterday");
        //    this.viewModel.SelectedItem = comparisonDate;
        //    viewModel.ComparisonDatePickerSelectedIndexChangedCommand.Execute(comparisonDate);

        //    // Nothing to actually verify yet here
        //    this.viewModel.SalesAnalysisList.Count.ShouldBe(2);
        //}

        [Fact]
        public async Task ReportsBalanceAnalysisPageViewModel_BackButtonCommand_HomePageIsShown()
        {
            ViewModel.BackButtonCommand.Execute(null);

            this.NavigationService.Verify(n => n.GoBack(), Times.Once);
        }
    }
}
