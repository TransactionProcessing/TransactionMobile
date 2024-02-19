using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TransactionMobile.Maui.BusinessLogic.Tests.ViewModelTests.Reports
{
    using System.Threading;
    using Common;
    using LiveChartsCore.Measure;
    using Maui.UIServices;
    using MediatR;
    using Microsoft.Maui.Handlers;
    using Models;
    using Services;
    using Shouldly;
    using UIServices;
    using ViewModels.Reports;

    public class ReportsPageViewModelTests
    {
        private Mock<INavigationService> navigationService;
        private Mock<IApplicationCache> applicationCache;
        private Mock<IDialogService> dialogService;
        private readonly Mock<IDeviceService> DeviceService;
        private Mock<IMediator> mediator;
        private ReportsPageViewModel viewModel;
        public ReportsPageViewModelTests(){
            navigationService = new Mock<INavigationService>();
            applicationCache = new Mock<IApplicationCache>();
            dialogService = new Mock<IDialogService>();
            this.DeviceService = new Mock<IDeviceService>();
            mediator = new Mock<IMediator>();

            this.viewModel = new ReportsPageViewModel(this.navigationService.Object,
                                                      this.applicationCache.Object,
                                                      this.dialogService.Object,
                                                      this.DeviceService.Object,
                                                      this.mediator.Object);
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
            this.viewModel.OptionSelectedCommand.Execute(itemSelected);

            switch(selectedIndex){
                case 0:
                    this.navigationService.Verify(v => v.GoToReportsSalesAnalysis(), Times.Once);
                    break;
                case 1:
                    this.navigationService.Verify(v => v.GoToReportsBalanceAnalysis(), Times.Once);
                    break;
                default:
                    this.navigationService.Verify(v => v.GoToReportsSalesAnalysis(), Times.Never);
                    this.navigationService.Verify(v => v.GoToReportsBalanceAnalysis(), Times.Never);
                    break;
            }
        }

        [Fact]
        public async Task ReportsPageViewModel_Initialise_IsInitialised()
        {
            await viewModel.Initialise(CancellationToken.None);
            this.viewModel.ReportsMenuOptions.Count.ShouldBe(2);
        }

        [Fact]
        public async Task ReportsPageViewModel_BackButtonCommand_HomePageIsShown()
        {
            viewModel.BackButtonCommand.Execute(null);

            navigationService.Verify(n => n.GoToHome(), Times.Once);
        }
    }

    public class ReportsSalesAnalysisPageViewModelTests{
        private ReportsSalesAnalysisPageViewModel viewModel;
        private Mock<INavigationService> navigationService;
        private Mock<IApplicationCache> applicationCache;
        private Mock<IDialogService> dialogService;
        private readonly Mock<IDeviceService> DeviceService;
        private Mock<IMediator> mediator;

        public ReportsSalesAnalysisPageViewModelTests(){
            navigationService = new Mock<INavigationService>();
            applicationCache = new Mock<IApplicationCache>();
            dialogService = new Mock<IDialogService>();
            this.DeviceService = new Mock<IDeviceService>();
            mediator = new Mock<IMediator>();

            this.viewModel = new ReportsSalesAnalysisPageViewModel(this.navigationService.Object,
                                                                   this.applicationCache.Object,
                                                                   this.dialogService.Object,
                                                                   this.DeviceService.Object,
                                                                   this.mediator.Object);
        }

        [Fact]
        public async Task ReportsSalesAnalysisPageViewModel_Initialise_Execute_IsExecuted(){
            await viewModel.Initialise(CancellationToken.None);

            this.viewModel.ComparisonDates.Count.ShouldBe(3);
        }

        [Fact]
        public async Task ReportsSalesAnalysisPageViewModel_ComparisonDatePickerSelectedIndexChangedCommand_HomePageIsShown(){
            ComparisonDate comparisonDate = new ComparisonDate(DateTime.Now.AddDays(-1), "Yesterday");
            this.viewModel.SelectedItem = comparisonDate;
            viewModel.ComparisonDatePickerSelectedIndexChangedCommand.Execute(comparisonDate);

            // Nothing to actually verify yet here
            this.viewModel.SalesAnalysisList.Count.ShouldBe(2);
        }

        [Fact]
        public async Task ReportsSalesAnalysisPageViewModel_BackButtonCommand_HomePageIsShown()
        {
            viewModel.BackButtonCommand.Execute(null);

            navigationService.Verify(n => n.GoBack(), Times.Once);
        }
    }

    public class ReportsBalanceAnalysisPageViewModelTests
    {
        private ReportsBalanceAnalysisPageViewModel viewModel;
        private Mock<INavigationService> navigationService;
        private Mock<IApplicationCache> applicationCache;
        private Mock<IDialogService> dialogService;
        private readonly Mock<IDeviceService> DeviceService;
        private Mock<IMediator> mediator;

        public ReportsBalanceAnalysisPageViewModelTests()
        {
            navigationService = new Mock<INavigationService>();
            applicationCache = new Mock<IApplicationCache>();
            dialogService = new Mock<IDialogService>();
            this.DeviceService = new Mock<IDeviceService>();
            mediator = new Mock<IMediator>();

            this.viewModel = new ReportsBalanceAnalysisPageViewModel(this.navigationService.Object,
                                                                     this.applicationCache.Object,
                                                                     this.dialogService.Object,
                                                                     this.DeviceService.Object,
                                                                     this.mediator.Object);
        }

        [Fact]
        public async Task ReportsBalanceAnalysisPageViewModel_Initialise_Execute_IsExecuted()
        {
            await viewModel.Initialise(CancellationToken.None);

            this.viewModel.XAxes.ShouldNotBeNull();
            this.viewModel.YAxes.ShouldNotBeNull();
            this.viewModel.TooltipFindingStrategy.ShouldBe(TooltipFindingStrategy.CompareOnlyX);
            this.viewModel.TooltipPosition.ShouldBe(TooltipPosition.Top);
            this.viewModel.Series.ShouldNotBeNull();
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
            viewModel.BackButtonCommand.Execute(null);

            navigationService.Verify(n => n.GoBack(), Times.Once);
        }
    }
}
