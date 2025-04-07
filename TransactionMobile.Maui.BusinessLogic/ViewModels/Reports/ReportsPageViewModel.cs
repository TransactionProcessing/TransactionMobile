using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Reports
{
    using System.Windows.Input;
    using Common;
    using Logging;
    using Maui.UIServices;
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Primitives;
    using Models;
    using MvvmHelpers.Commands;
    using MyAccount;
    using RequestHandlers;
    using Requests;
    using Services;
    using UIServices;

    public class ReportsPageViewModel : ExtendedBaseViewModel
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
            this.OptionSelectedCommand = new AsyncCommand<ItemSelected<ListViewItem>>(this.OptionSelectedCommandExecute);
            this.Title = "Reports";
        }

        #endregion

        #region Properties

        public List<ListViewItem> ReportsMenuOptions { get; set; }

        public ICommand OptionSelectedCommand { get; set; }

        #endregion

        #region Methods

        public override async Task Initialise(CancellationToken cancellationToken)
        {
            this.ReportsMenuOptions = new List<ListViewItem> {
                                                                 new ListViewItem {
                                                                                      Title = "Sales Analysis"
                                                                                  },
                                                                 new ListViewItem {
                                                                                      Title = "Balance Analysis"
                                                                                  }
                                                             };
            await base.Initialise(cancellationToken);
        }

        private async Task OptionSelectedCommandExecute(ItemSelected<ListViewItem> arg)
        {
            ReportsOptions selectedOption = (ReportsOptions)arg.SelectedItemIndex;

            Task navigationTask = selectedOption switch
            {
                ReportsOptions.SalesAnalysis => this.NavigationService.GoToReportsSalesAnalysis(),
                ReportsOptions.BalanceAnalysis => this.NavigationService.GoToReportsBalanceAnalysis(),
                _ => Task.Factory.StartNew(() => Logger.LogWarning($"Unsupported option selected {selectedOption}"))
            };

            await navigationTask;
        }

        #endregion

        #region Others

        public enum ReportsOptions
        {
            SalesAnalysis = 0,

            BalanceAnalysis = 1,
        }

        #endregion
    }
}
