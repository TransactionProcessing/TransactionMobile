namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Models;
using Moq;
using RequestHandlers;
using Requests;
using Services;
using Shouldly;
using Xunit;

public class LoginRequestHandlerTests
{
    [Fact]
    public async Task LoginRequestHandler_Handle_LoginRequest_IsHandled()
    {
        Mock<IAuthenticationService> authenticationService = new Mock<IAuthenticationService>();
        authenticationService.Setup(a => a.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.AccessToken);
        Mock<IConfigurationService> configurationService = new Mock<IConfigurationService>();
        configurationService.Setup(c => c.GetConfiguration(It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Configuration());
        LoginRequestHandler handler = new LoginRequestHandler(authenticationService.Object,configurationService.Object);
            
        LoginRequest request = LoginRequest.Create(TestData.UserName,TestData.Password);

        String accessToken = await handler.Handle(request, CancellationToken.None);

        accessToken.ShouldBe(TestData.AccessToken);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_GetConfigurationRequest_IsHandled()
    {
        Mock<IAuthenticationService> authenticationService = new Mock<IAuthenticationService>();
        authenticationService.Setup(a => a.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.AccessToken);
        Mock<IConfigurationService> configurationService = new Mock<IConfigurationService>();
        configurationService.Setup(c => c.GetConfiguration(It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Configuration());
        LoginRequestHandler handler = new LoginRequestHandler(authenticationService.Object, configurationService.Object);

        GetConfigurationRequest request = GetConfigurationRequest.Create(TestData.DeviceIdentifier);

        Configuration configuration = await handler.Handle(request, CancellationToken.None);

        configuration.ShouldNotBeNull();
    }
}