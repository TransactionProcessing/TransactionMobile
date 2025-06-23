using System.Text;
using System.Windows.Input;
using MediatR;
using MvvmHelpers.Commands;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support
{
    public class SupportPageViewModel : ExtendedBaseViewModel
    {
        #region Fields

        private readonly IApplicationInfoService ApplicationInfoService;

        private readonly IDatabaseContext DatabaseContext;

        private readonly IMediator Mediator;
        
        #endregion

        #region Constructors

        public SupportPageViewModel(IDeviceService deviceService,
                                    IApplicationInfoService applicationInfoService,
                                    IDatabaseContext databaseContext,
                                    IMediator mediator,
                                    INavigationService navigationService,
                                    IApplicationCache applicationCache,
                                    IDialogService dialogService,
                                    INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService,navigationParameterService)
        {
            this.ApplicationInfoService = applicationInfoService;
            this.DatabaseContext = databaseContext;
            this.UploadLogsCommand = new AsyncCommand(this.UploadLogsCommandExecute);
            this.ViewLogsCommand = new AsyncCommand(this.ViewLogsCommandExecute);
            this.Mediator = mediator;
            this.Title = "Support";
        }

        #endregion

        #region Properties

        public String ApplicationName => $"{this.ApplicationInfoService.ApplicationName} v{this.ApplicationInfoService.VersionString}";

        public String NumberTransactionsStored => $"Transactions Stored: {this.DatabaseContext.GetTransactions(this.ApplicationCache.GetUseTrainingMode()).Result.Count()}";

        public String Platform {
            get {
                StringBuilder platform = new();
                platform.Append("Platform: ").AppendLine(this.DeviceService.GetPlatform());
                platform.Append("Manufacturer: ").AppendLine(this.DeviceService.GetManufacturer());
                platform.Append("Device: ").AppendLine(this.DeviceService.GetModel());

                return platform.ToString();
            }
        }

        public ICommand UploadLogsCommand { get; }

        public ICommand ViewLogsCommand { get; }

        #endregion

        #region Methods

        private async Task UploadLogsCommandExecute() {
            Logger.LogInformation("UploadLogsCommandExecute called");

            UploadLogsRequest uploadLogsRequest = UploadLogsRequest.Create(String.Empty);

            Boolean response = await this.Mediator.Send(uploadLogsRequest, CancellationToken.None);

            // TODO: Act on the response (display message or something)...
            //await this.NavigationService.GoBack();
        }

        private async Task ViewLogsCommandExecute() {
            Logger.LogInformation("ViewLogsCommandExecute called");

            // TODO: Act on the response (display message or something)...
            await this.NavigationService.GoToViewLogsPage();
        }

        #endregion
    }
}