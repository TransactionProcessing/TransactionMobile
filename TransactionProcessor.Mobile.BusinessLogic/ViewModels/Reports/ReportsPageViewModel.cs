using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Common;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Reports
{
    public partial class ReportsPageViewModel : ExtendedBaseViewModel
    {
        #region Fields

        private readonly IMediator Mediator;

        #endregion

        #region Constructors

        public ReportsPageViewModel(INavigationService navigationService,
                                    IApplicationCache applicationCache,
                                    IDialogService dialogService,
                                    IDeviceService deviceService,
                                    IMediator mediator,
                                    INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService,navigationParameterService)
        {
            this.Mediator = mediator;
            this.Title = "Reports";
        }

        #endregion

        #region Properties

        public List<ListViewItem> ReportsMenuOptions { get; set; }

        #endregion

        #region Methods

        public override async Task Initialise(CancellationToken cancellationToken)
        {
            this.ReportsMenuOptions = new List<ListViewItem> {
                                                                 new ListViewItem {
                                                                                      Title = "Daily Performance Summary"
                                                                                  },
                                                                 new ListViewItem {
                                                                                      Title = "Transaction Mix"
                                                                                  },
                                                                 new ListViewItem {
                                                                                      Title = "Recent Activity and Receipt Report"
                                                                                  },
                                                               };
            await base.Initialise(cancellationToken);
        }

        [RelayCommand]
        private async Task OptionSelected(ItemSelected<ListViewItem> arg)
        {
            CorrelationIdProvider.NewId();
            ReportsOptions selectedOption = (ReportsOptions)arg.SelectedItemIndex;

            Task navigationTask = selectedOption switch
            {
                ReportsOptions.DailyPerformanceSummary => this.NavigationService.GoToDailyPerformanceSummaryPage(),
                ReportsOptions.TransactionMix => this.NavigationService.GoToTransactionMixSummaryPage(),
                ReportsOptions.RecentActivityAndReceiptReport => this.NavigationService.GoToRecentActivityReportPage(),
                _ => Task.Factory.StartNew(() => Logger.LogWarning($"Unsupported option selected {selectedOption}"))
            };

            await navigationTask;
        }

        #endregion

        #region Others

        public enum ReportsOptions
        {
            DailyPerformanceSummary = 0,

            TransactionMix = 1,

            RecentActivityAndReceiptReport = 2,
        }

        #endregion
    }
}
