using MediatR;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;

namespace TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;

public class ViewLogsPageViewModel : ExtendedBaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public ViewLogsPageViewModel(IMediator mediator,
                                 INavigationService navigationService,
                                 IApplicationCache applicationCache,
                                 IDialogService dialogService,
                                 IDeviceService deviceService,
                                 INavigationParameterService navigationParameterService) : base(applicationCache, dialogService, navigationService, deviceService, navigationParameterService) {
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