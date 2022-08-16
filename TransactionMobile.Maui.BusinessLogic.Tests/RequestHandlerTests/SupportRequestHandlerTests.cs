namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Database;
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
        databaseContext.Setup(d => d.GetLogMessages(It.IsAny<Int32>())).ReturnsAsync(new List<Database.LogMessage>());
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
        databaseContext.SetupSequence(d => d.GetLogMessages(It.IsAny<Int32>())).ReturnsAsync(new List<Database.LogMessage>()
        {
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage()
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
        databaseContext.SetupSequence(d => d.GetLogMessages(It.IsAny<Int32>())).ReturnsAsync(new List<Database.LogMessage>()
        {
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
        }).ReturnsAsync(new List<Database.LogMessage>()
        {
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
            new Database.LogMessage(),
        }).ReturnsAsync(new List<Database.LogMessage>());

        Mock<IApplicationCache> applicationCache = new Mock<IApplicationCache>();

        SupportRequestHandler handler = new SupportRequestHandler(configurationServiceResolver, databaseContext.Object, applicationCache.Object);

        UploadLogsRequest request = UploadLogsRequest.Create(TestData.DeviceIdentifier);

        Boolean response = await handler.Handle(request, CancellationToken.None);

        response.ShouldBeTrue();
        databaseContext.Verify(d => d.RemoveUploadedMessages(It.IsAny<List<Database.LogMessage>>()), Times.Exactly(2));
    }
}