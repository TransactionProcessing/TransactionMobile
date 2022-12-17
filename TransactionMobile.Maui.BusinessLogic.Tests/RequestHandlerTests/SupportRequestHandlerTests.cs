namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Microsoft.Data.Sqlite;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using Xunit;

public class SupportRequestHandlerTests
{

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_NoLogs_Handle_IsHandled()
    {
        Mock<IConfigurationService> configurationService = new Mock<IConfigurationService>();
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Object;
        });

        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        databaseContext.Setup(d => d.GetLogMessages(It.IsAny<Int32>(), It.IsAny<Boolean>())).ReturnsAsync(new List<Database.LogMessage>());
        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Object, applicationCache.Object);

        UploadLogsRequest request = UploadLogsRequest.Create(TestData.DeviceIdentifier);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
    }

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_LogsToUpload_Only10Messages_Handle_IsHandled()
    {
        Mock<IConfigurationService> configurationService = new Mock<IConfigurationService>();
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        databaseContext.SetupSequence(d => d.GetLogMessages(It.IsAny<Int32>(), It.IsAny<Boolean>())).ReturnsAsync(new List<Database.LogMessage>()
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
        }).ReturnsAsync(new List<Database.LogMessage>());

        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Object, applicationCache.Object);

        UploadLogsRequest request = UploadLogsRequest.Create(TestData.DeviceIdentifier);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
        databaseContext.Verify(d => d.RemoveUploadedMessages(It.IsAny<List<Database.LogMessage>>()), Times.Once);
    }

    [Fact]
    public async Task SupportRequestHandlerTests_UploadLogsRequest_LogsToUpload_15Messages_Handle_IsHandled()
    {
        Mock<IConfigurationService> configurationService = new Mock<IConfigurationService>();
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Object;
        });
        Mock<IDatabaseContext> databaseContext = new Mock<IDatabaseContext>();
        databaseContext.SetupSequence(d => d.GetLogMessages(It.IsAny<Int32>(), It.IsAny<Boolean>())).ReturnsAsync(new List<Database.LogMessage>()
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
        }).ReturnsAsync(new List<Database.LogMessage>()
        {
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
            new Database.LogMessage{LogLevel = LogLevel.Debug.ToString()},
        }).ReturnsAsync(new List<Database.LogMessage>());

        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Object, applicationCache.Object);

        UploadLogsRequest request = UploadLogsRequest.Create(TestData.DeviceIdentifier);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
        databaseContext.Verify(d => d.RemoveUploadedMessages(It.IsAny<List<Database.LogMessage>>()), Times.Exactly(2));
    }

    [Theory]
    [InlineData(true, 4)]
    [InlineData(false, 3)]
    public async Task SupportRequestHandlerTests_ViewLogsRequest_Handle_IsHandled(Boolean isTrainingMode, Int32 expectedNumberMessages)
    {
        Mock<IConfigurationService> configurationService = new Mock<IConfigurationService>();
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Object;
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

        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();
        applicationCache.Setup(s => s.GetUseTrainingMode()).Returns(isTrainingMode);
        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext, applicationCache.Object);

        ViewLogsRequest request = ViewLogsRequest.Create();
        List<Models.LogMessage>? result = await handler.Handle(request, CancellationToken.None);

        result.Count.ShouldBe(expectedNumberMessages);
    }
}