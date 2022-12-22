namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Support
{
    using System.Text;
    using System.Windows.Input;
    using Database;
    using Logging;
    using Maui.UIServices;
    using MediatR;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using Requests;
    using Services;
    using UIServices;

    public class SupportPageViewModel : ExtendedBaseViewModel
    {
        #region Fields

        private readonly IApplicationInfoService ApplicationInfoService;

        private readonly IDatabaseContext DatabaseContext;

        private readonly IDeviceService DeviceService;

        private readonly IMediator Mediator;
        
        #endregion

        #region Constructors

        public SupportPageViewModel(IDeviceService deviceService,
                                    IApplicationInfoService applicationInfoService,
                                    IDatabaseContext databaseContext,
                                    IMediator mediator,
                                    INavigationService navigationService,
                                    IApplicationCache applicationCache,
                                    IDialogService dialogService, ILoggerService logger) : base(applicationCache, dialogService, navigationService, logger)
        {
            this.DeviceService = deviceService;
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
                platform.Append("Manufacturer: ").AppendLine(DeviceInfo.Manufacturer);
                platform.Append("Device: ").AppendLine(this.DeviceService.GetModel());

                return platform.ToString();
            }
        }

        //public Boolean ShowUploadLogsButton => this.ApplicationCache.GetUseTrainingMode() == false;

        //public Boolean ShowViewLogsButton => this.ApplicationCache.GetUseTrainingMode();

        public ICommand UploadLogsCommand { get; }

        public ICommand ViewLogsCommand { get; }

        #endregion

        #region Methods

        private async Task UploadLogsCommandExecute() {
            Logger.LogInformation("UploadLogsCommandExecute called");

            UploadLogsRequest uploadLogsRequest = UploadLogsRequest.Create(String.Empty);

            Boolean response = await this.Mediator.Send(uploadLogsRequest, CancellationToken.None);

            // TODO: Act on the response (display message or something)...
            await this.NavigationService.GoToHome();
        }

        private async Task ViewLogsCommandExecute() {
            Logger.LogInformation("ViewLogsCommandExecute called");

            // TODO: Act on the response (display message or something)...
            await this.NavigationService.GoToViewLogsPage();
        }

        #endregion
    }
}