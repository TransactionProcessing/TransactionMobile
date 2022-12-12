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
using static SQLite.SQLite3;

public class LoginRequestHandlerTests
{
    private Mock<IAuthenticationService> AuthenticationService = null;
    private Mock<IConfigurationService> ConfigurationService = null;
    private LoginRequestHandler LoginRequestHandler = null;

    public LoginRequestHandlerTests() {
        this.AuthenticationService = new Mock<IAuthenticationService>();
        this.ConfigurationService= new Mock<IConfigurationService>();
        this.LoginRequestHandler = new LoginRequestHandler(this.AuthenticationService.Object, this.ConfigurationService.Object);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_LoginRequest_IsHandled()
    {
        this.AuthenticationService.Setup(a => a.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<TokenResponseModel>(TestData.AccessToken));
        this.ConfigurationService.Setup(c => c.GetConfiguration(It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Configuration>(new Configuration()));
           
        LoginRequest request = LoginRequest.Create(TestData.UserName,TestData.Password);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.AccessToken.ShouldBe(TestData.Token);
        result.Data.ExpiryInMinutes.ShouldBe(TestData.TokenExpiryInMinutes);
        result.Data.RefreshToken.ShouldBe(TestData.RefreshToken);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_RefreshTokenRequest_IsHandled()
    {
        Mock<IAuthenticationService> authenticationService = new Mock<IAuthenticationService>();
        Func<Boolean, IAuthenticationService> authenticationServiceResolver = new Func<bool, IAuthenticationService>((param) =>
        {
            return authenticationService.Object;
        });
        Mock<IConfigurationService> configurationService = new Mock<IConfigurationService>();
        Func<Boolean, IConfigurationService> configurationServiceResolver = new Func<bool, IConfigurationService>((param) =>
        {
            return configurationService.Object;
        });
        this.AuthenticationService.Setup(a => a.RefreshAccessToken(It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<TokenResponseModel>(TestData.AccessToken));
        this.ConfigurationService.Setup(c => c.GetConfiguration(It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Configuration>(new Configuration()));

        RefreshTokenRequest request = RefreshTokenRequest.Create(TestData.RefreshToken);

        Result<TokenResponseModel> result = await this.LoginRequestHandler .Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.AccessToken.ShouldBe(TestData.Token);
        result.Data.ExpiryInMinutes.ShouldBe(TestData.TokenExpiryInMinutes);
        result.Data.RefreshToken.ShouldBe(TestData.RefreshToken);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_GetConfigurationRequest_IsHandled()
    {
        this.AuthenticationService.Setup(a => a.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<TokenResponseModel>(TestData.AccessToken));
        this.ConfigurationService.Setup(c => c.GetConfiguration(It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(new SuccessResult<Configuration>(new Configuration()));

        GetConfigurationRequest request = GetConfigurationRequest.Create(TestData.DeviceIdentifier);

        Result<Configuration> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
    }
}