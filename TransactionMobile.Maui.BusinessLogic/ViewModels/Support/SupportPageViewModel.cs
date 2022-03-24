using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Support
{
    using Database;
    using MediatR;
    using MvvmHelpers;
    using MvvmHelpers.Commands;
    using System.Windows.Input;
    using TransactionMobile.Maui.BusinessLogic.Requests;
    using TransactionMobile.Maui.UIServices;
    using UIServices;

    public class SupportPageViewModel : BaseViewModel
    {
        private readonly IDeviceService DeviceService;

        private readonly IApplicationInfoService ApplicationInfoService;

        private readonly IDatabaseContext DatabaseContext;

        private readonly IMediator Mediator;

        private readonly INavigationService NavigationService;

        public SupportPageViewModel(IDeviceService deviceService,IApplicationInfoService applicationInfoService,IDatabaseContext databaseContext,
            IMediator mediator,INavigationService navigationService)
        {
            this.DeviceService = deviceService;
            this.ApplicationInfoService = applicationInfoService;
            this.DatabaseContext = databaseContext;
            this.UploadLogsCommand = new AsyncCommand(this.UploadLogsCommandExecute);
            this.Mediator = mediator;
            this.NavigationService = navigationService;
            this.Title = "Support";
        }

        public ICommand UploadLogsCommand { get; }

        public String NumberTransactionsStored => $"Transactions Stored: {this.DatabaseContext.GetTransactions().Result.Count()}";

        public String ApplicationName => $"{this.ApplicationInfoService.ApplicationName} v{this.ApplicationInfoService.VersionString}";

        private async Task UploadLogsCommandExecute()
        {
            Shared.Logger.Logger.LogInformation("UploadLogsCommandExecute called");

            UploadLogsRequest uploadLogsRequest = UploadLogsRequest.Create(this.DeviceService.GetIdentifier());

            Boolean response = await this.Mediator.Send(uploadLogsRequest, CancellationToken.None);

            // TODO: Act on the response (display message or something)...
            await this.NavigationService.GoToHome();
        }

        public string Platform
        {
            get
            {
                StringBuilder platform = new();
                platform.Append("Platform: ").AppendLine(this.DeviceService.GetPlatform());
                platform.Append("Manufacturer: ").AppendLine(DeviceInfo.Manufacturer);
                platform.Append("Device: ").AppendLine(this.DeviceService.GetModel());

                return platform.ToString();
            }
        }

    }
}
