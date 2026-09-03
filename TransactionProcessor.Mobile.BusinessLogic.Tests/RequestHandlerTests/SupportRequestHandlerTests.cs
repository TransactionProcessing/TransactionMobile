using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Database;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;
using LogLevel = TransactionProcessor.Mobile.BusinessLogic.Database.LogLevel;
using LogMessage = TransactionProcessor.Mobile.BusinessLogic.Database.LogMessage;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.RequestHandlerTests;

public class SupportRequestHandlerTests
{

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_NoLogs_Handle_IsHandled()
    {
        IConfigurationServiceImposter configurationService = new IConfigurationServiceImposter();
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Instance();
        });

        IDatabaseContextImposter databaseContext = new IDatabaseContextImposter();
        databaseContext.GetLogMessages(Arg<Int32>.Any(), Arg<Boolean>.Any()).ReturnsAsync(new List<Database.LogMessage>());
        IApplicationCacheImposter applicationCache = new IApplicationCacheImposter();
        applicationCache.GetConfiguration().Returns(new Configuration());
        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Instance(), applicationCache.Instance());

        SupportCommands.UploadLogsCommand request = new(TestData.DeviceIdentifier);

        Result response = await handler.Handle(request, CancellationToken.None);

        response.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_NoConfiguration_ReturnsFailure()
    {
        IConfigurationServiceImposter configurationService = new IConfigurationServiceImposter();
        Func<Boolean, IConfigurationService> configurationServiceResolver = _ => configurationService.Instance();

        IDatabaseContextImposter databaseContext = new IDatabaseContextImposter();
        IApplicationCacheImposter applicationCache = new IApplicationCacheImposter();
        applicationCache.GetConfiguration().Returns((Configuration)null);

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Instance(), applicationCache.Instance());

        SupportCommands.UploadLogsCommand request = new(TestData.DeviceIdentifier);

        Result response = await handler.Handle(request, CancellationToken.None);

        response.IsFailed.ShouldBeTrue();
        configurationService.PostDiagnosticLogs(Arg<String>.Any(), Arg<List<Models.LogMessage>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        databaseContext.GetLogMessages(Arg<Int32>.Any(), Arg<Boolean>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_LogsToUpload_Only10Messages_Handle_IsHandled()
    {
        IConfigurationServiceImposter configurationService = new IConfigurationServiceImposter();
        configurationService.PostDiagnosticLogs(Arg<String>.Any(), Arg<List<Models.LogMessage>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Instance();
        });
        IDatabaseContextImposter databaseContext = new IDatabaseContextImposter();
        databaseContext.GetLogMessages(Arg<Int32>.Any(), Arg<Boolean>.Any()).ReturnsAsync(new List<Database.LogMessage>()
        {
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()}
        }).Then().ReturnsAsync(new List<Database.LogMessage>());

        IApplicationCacheImposter applicationCache = new IApplicationCacheImposter();
        applicationCache.GetConfiguration().Returns(new Configuration());

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Instance(), applicationCache.Instance());

        SupportCommands.UploadLogsCommand request = new(TestData.DeviceIdentifier);

        Result response = await handler.Handle(request, CancellationToken.None);

        response.IsSuccess.ShouldBeTrue();
        databaseContext.RemoveUploadedMessages(Arg<List<Database.LogMessage>>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_LogsToUpload_15Messages_Handle_IsHandled()
    {
        IConfigurationServiceImposter configurationService = new IConfigurationServiceImposter();
        configurationService.PostDiagnosticLogs(Arg<String>.Any(), Arg<List<Models.LogMessage>>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success());
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Instance();
        });
        IDatabaseContextImposter databaseContext = new IDatabaseContextImposter();
        databaseContext.GetLogMessages(Arg<Int32>.Any(), Arg<Boolean>.Any()).ReturnsAsync(new List<Database.LogMessage>()
        {
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
        }).Then().ReturnsAsync(new List<Database.LogMessage>()
        {
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
        }).Then().ReturnsAsync(new List<Database.LogMessage>());

        IApplicationCacheImposter applicationCache = new IApplicationCacheImposter();
        applicationCache.GetConfiguration().Returns(new Configuration());

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Instance(), applicationCache.Instance());

        SupportCommands.UploadLogsCommand request = new(TestData.DeviceIdentifier);

        Result response = await handler.Handle(request, CancellationToken.None);

        response.IsSuccess.ShouldBeTrue();
        databaseContext.RemoveUploadedMessages(Arg<List<Database.LogMessage>>.Any()).Called(Count.Exactly(2));
    }

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_InvalidLogLevel_ReturnsFailure()
    {
        IConfigurationServiceImposter configurationService = new IConfigurationServiceImposter();
        Func<Boolean, IConfigurationService> configurationServiceResolver = _ => configurationService.Instance();

        IDatabaseContextImposter databaseContext = new IDatabaseContextImposter();
        databaseContext.GetLogMessages(Arg<Int32>.Any(), Arg<Boolean>.Any()).ReturnsAsync(new List<Database.LogMessage>
        {
            new Database.LogMessage { LogLevel = "NotALogLevel" }
        });

        IApplicationCacheImposter applicationCache = new IApplicationCacheImposter();
        applicationCache.GetConfiguration().Returns(new Configuration());

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Instance(), applicationCache.Instance());

        SupportCommands.UploadLogsCommand request = new(TestData.DeviceIdentifier);

        Result response = await handler.Handle(request, CancellationToken.None);

        response.IsFailed.ShouldBeTrue();
        configurationService.PostDiagnosticLogs(Arg<String>.Any(), Arg<List<Models.LogMessage>>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        databaseContext.RemoveUploadedMessages(Arg<List<Database.LogMessage>>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_CancellationStopsFurtherProcessing()
    {
        IConfigurationServiceImposter configurationService = new IConfigurationServiceImposter();
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        configurationService.PostDiagnosticLogs(Arg<String>.Any(), Arg<List<Models.LogMessage>>.Any(), Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success())
            .Callback((String _, List<Models.LogMessage> _, CancellationToken _) =>
            {
                cancellationTokenSource.Cancel();
                return Task.CompletedTask;
            });

        Func<Boolean, IConfigurationService> configurationServiceResolver = _ => configurationService.Instance();

        IDatabaseContextImposter databaseContext = new IDatabaseContextImposter();
        databaseContext.GetLogMessages(Arg<Int32>.Any(), Arg<Boolean>.Any())
            .ReturnsAsync(new List<Database.LogMessage>
            {
                new Database.LogMessage { LogLevel = LogLevel.Debug.ToString() }
            })
            .Then().ReturnsAsync(new List<Database.LogMessage>
            {
                new Database.LogMessage { LogLevel = LogLevel.Debug.ToString() }
            });

        IApplicationCacheImposter applicationCache = new IApplicationCacheImposter();
        applicationCache.GetConfiguration().Returns(new Configuration());

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Instance(), applicationCache.Instance());

        SupportCommands.UploadLogsCommand request = new(TestData.DeviceIdentifier);

        await Should.ThrowAsync<OperationCanceledException>(async () => await handler.Handle(request, cancellationTokenSource.Token));

        configurationService.PostDiagnosticLogs(Arg<String>.Any(), Arg<List<Models.LogMessage>>.Any(), cancellationTokenSource.Token).Called(Count.Once());
        databaseContext.GetLogMessages(Arg<Int32>.Any(), Arg<Boolean>.Any()).Called(Count.Once());
        databaseContext.RemoveUploadedMessages(Arg<List<Database.LogMessage>>.Any()).Called(Count.Once());
    }

    [Theory]
    [InlineData(true, 4)]
    [InlineData(false, 3)]
    public async Task SupportRequestHandlerTests_ViewLogsRequest_Handle_IsHandled(Boolean isTrainingMode, Int32 expectedNumberMessages)
    {
        IConfigurationServiceImposter configurationService = new IConfigurationServiceImposter();
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Instance();
        });
        Func<Database.LogLevel> logLevelFunc = new Func<Database.LogLevel>(() =>
                                                                           {
                                                                               return Database.LogLevel.Debug;
                                                                           });
        IDatabaseContext databaseContext = new DatabaseContext(":memory:", logLevelFunc);
        await databaseContext.InitialiseDatabase();

        List<LogMessage> logMessages = new List<LogMessage>();
        logMessages.Add(new LogMessage { LogLevel = LogLevel.Debug.ToString() });
        logMessages.Add(new LogMessage { LogLevel = LogLevel.Debug.ToString() });
        logMessages.Add(new LogMessage { LogLevel = LogLevel.Debug.ToString() });
        logMessages.Add(new LogMessage { LogLevel = LogLevel.Debug.ToString(), IsTrainingMode = true });
        logMessages.Add(new LogMessage { LogLevel = LogLevel.Debug.ToString(), IsTrainingMode = true });
        logMessages.Add(new LogMessage { LogLevel = LogLevel.Debug.ToString(), IsTrainingMode = true });
        logMessages.Add(new LogMessage { LogLevel = LogLevel.Debug.ToString(), IsTrainingMode = true });
        await databaseContext.InsertLogMessages(logMessages);

        IApplicationCacheImposter applicationCache = new IApplicationCacheImposter();
        applicationCache.GetUseTrainingMode().Returns(isTrainingMode);
        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext, applicationCache.Instance());

        SupportQueries.ViewLogsQuery request = new();
        Result<List<Models.LogMessage>> result = await handler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.Count.ShouldBe(expectedNumberMessages);
    }
}
