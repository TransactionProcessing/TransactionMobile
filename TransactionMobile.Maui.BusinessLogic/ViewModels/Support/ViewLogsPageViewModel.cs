namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Support;

using Logging;
using Maui.UIServices;
using MediatR;
using Models;
using MvvmHelpers;
using Requests;
using Services;
using UIServices;

public class ViewLogsPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public ViewLogsPageViewModel(IMediator mediator, INavigationService navigationService,
    IApplicationCache applicationCache,
                      IDialogService dialogService,
    IDeviceService deviceService) : base(applicationCache, dialogService, navigationService, deviceService) {
        this.Mediator = mediator;
        this.Title = "View Logs";
    }

    #endregion

    #region Properties

    public List<LogMessageModel> LogMessages { get; private set; }

    #endregion

    #region Methods

    public async Task LoadLogMessages() {
        ViewLogsRequest viewLogsRequest = ViewLogsRequest.Create();

        List<LogMessage> logMessages = await this.Mediator.Send(viewLogsRequest, CancellationToken.None);

        List<LogMessageModel> logMessageViewModels = new List<LogMessageModel>();

        logMessages.ForEach(l => {
                                logMessageViewModels.Add(new LogMessageModel
                                                         {
                                                                                     LogLevel = l.LogLevel,
                                                                                     LogLevelString = l.LogLevelString,
                                                                                     Message = l.Message,
                                                                                     EntryDateTime = l.EntryDateTime,
                                                                                     Id = l.Id
                                                                                 });
                            });

        this.LogMessages = logMessageViewModels;
    }

    #endregion
}