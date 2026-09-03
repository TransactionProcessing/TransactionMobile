using MediatR;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using TransactionProcessor.Mobile.BusinessLogic.UIServices;
using TransactionProcessor.Mobile.BusinessLogic.ViewModels.Support;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ViewModelTests.Support;

[Collection("ViewModelTests")]
public class ViewLogsPageViewModelTests
{
    private readonly INavigationServiceImposter NavigationService;

    private readonly INavigationParameterServiceImposter NavigationParameterService;

    private readonly IMediatorImposter Mediator;

    private readonly IApplicationCacheImposter ApplicationCache;

    private readonly IDialogServiceImposter DialogService;

    private readonly ViewLogsPageViewModel ViewModel;

    private readonly IDeviceServiceImposter DeviceService;

    public ViewLogsPageViewModelTests() {
        this.NavigationService = new INavigationServiceImposter();
        this.NavigationParameterService = new INavigationParameterServiceImposter();
        this.Mediator = new IMediatorImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.DialogService = new IDialogServiceImposter();
        this.DeviceService = new IDeviceServiceImposter();
        this.ViewModel = new ViewLogsPageViewModel(this.Mediator.Instance(),
                                                   this.NavigationService.Instance(),
                                                   this.ApplicationCache.Instance(),
                                                   this.DialogService.Instance(),
                                                   this.DeviceService.Instance(), this.NavigationParameterService.Instance());
    }

    [Fact]
    public async Task SupportPageViewModel_UploadLogsCommand_Execute_IsExecuted(){

        this.Mediator.Send(Arg<IRequest<Result<List<LogMessage>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new List<LogMessage>(){
                                                                                                                                                              new LogMessage()
                                                                                                                                                          }));

        await this.ViewModel.LoadLogMessages();
        this.ViewModel.LogMessages.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ViewLogsPageViewModel_LoadLogMessages_RaisesPropertyChanged(){
        this.Mediator.Send(Arg<IRequest<Result<List<LogMessage>>>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new List<LogMessage>(){
                                                                                                                                                              new LogMessage()
                                                                                                                                                          }));
        String? changedPropertyName = null;
        this.ViewModel.PropertyChanged += (_, args) => changedPropertyName = args.PropertyName;

        await this.ViewModel.LoadLogMessages();

        changedPropertyName.ShouldBe(nameof(ViewLogsPageViewModel.LogMessages));
    }

    [Fact]
    public void ViewLogsPageViewModel_BackButtonCommand_Execute_IsExecuted()
    {
        this.ViewModel.BackButtonCommand.Execute(null);

        this.NavigationService.GoBack().Called(Count.Once());
    }
}
