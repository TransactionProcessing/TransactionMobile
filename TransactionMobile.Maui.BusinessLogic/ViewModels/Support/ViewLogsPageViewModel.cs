namespace TransactionMobile.Maui.BusinessLogic.ViewModels.Support;

using MediatR;
using Models;
using MvvmHelpers;
using Requests;

public class ViewLogsPageViewModel : BaseViewModel
{
    #region Fields

    private readonly IMediator Mediator;

    #endregion

    #region Constructors

    public ViewLogsPageViewModel(IMediator mediator) {
        this.Mediator = mediator;
        this.Title = "View Logs";
    }

    #endregion

    #region Properties

    public List<LogMessageViewModel> LogMessages { get; private set; }

    #endregion

    #region Methods

    public async Task LoadLogMessages() {
        ViewLogsRequest viewLogsRequest = ViewLogsRequest.Create();

        List<LogMessage> logMessages = await this.Mediator.Send(viewLogsRequest, CancellationToken.None);

        List<LogMessageViewModel> logMessageViewModels = new List<LogMessageViewModel>();

        logMessages.ForEach(l => {
                                logMessageViewModels.Add(new LogMessageViewModel {
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