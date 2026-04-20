using Microsoft.Extensions.Caching.Memory;
using Moq;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.RequestHandlerTests;

public class LoginRequestHandlerTests
{
    private Mock<IAuthenticationService> AuthenticationService = null;
    private Mock<IConfigurationService> ConfigurationService = null;
    private LoginRequestHandler LoginRequestHandler = null;
    private readonly Mock<IApplicationCache> ApplicationCache;
    private Func<Boolean, IAuthenticationService> AuthenticationServiceResolver;
    private Func<Boolean, IConfigurationService> ConfigurationServiceResolver;

    public LoginRequestHandlerTests() {
        this.AuthenticationService = new Mock<IAuthenticationService>();
        this.ConfigurationService= new Mock<IConfigurationService>();
        this.ApplicationCache = new Mock<IApplicationCache>();
        this.AuthenticationServiceResolver = _ => this.AuthenticationService.Object;
        this.ConfigurationServiceResolver = _ => this.ConfigurationService.Object;

        this.LoginRequestHandler = new LoginRequestHandler(this.AuthenticationServiceResolver, this.ConfigurationServiceResolver, this.ApplicationCache.Object);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_LoginRequest_IsHandled()
    {
        this.ApplicationCache.Setup(a => a.GetConfiguration()).Returns(new Configuration { ClientId = TestData.ClientId, ClientSecret = TestData.ClientSecret });
        this.AuthenticationService.Setup(a => a.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.AccessToken));

        LogonCommands.GetTokenCommand request = new(TestData.UserName, TestData.Password);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.AccessToken.ShouldBe(TestData.Token);
        result.Data.ExpiryInMinutes.ShouldBe(TestData.TokenExpiryInMinutes);
        result.Data.RefreshToken.ShouldBe(TestData.RefreshToken);
        this.AuthenticationService.Verify(a => a.GetToken(TestData.UserName, TestData.Password, TestData.ClientId, TestData.ClientSecret, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_LoginRequest_ConfigurationNotCached_FailureReturned()
    {
        this.ApplicationCache.Setup(a => a.GetConfiguration()).Returns((Configuration)null);

        LogonCommands.GetTokenCommand request = new(TestData.UserName, TestData.Password);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        this.AuthenticationService.Verify(a => a.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_RefreshTokenRequest_IsHandled()
    {
        this.ApplicationCache.Setup(a => a.GetConfiguration()).Returns(new Configuration { ClientId = TestData.ClientId, ClientSecret = TestData.ClientSecret });
        this.AuthenticationService.Setup(a => a.RefreshAccessToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(TestData.AccessToken));

        LogonCommands.RefreshTokenCommand request = new(TestData.RefreshToken);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.AccessToken.ShouldBe(TestData.Token);
        result.Data.ExpiryInMinutes.ShouldBe(TestData.TokenExpiryInMinutes);
        result.Data.RefreshToken.ShouldBe(TestData.RefreshToken);
        this.AuthenticationService.Verify(a => a.RefreshAccessToken(TestData.RefreshToken, TestData.ClientId, TestData.ClientSecret, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_RefreshTokenRequest_ConfigurationNotCached_FailureReturned()
    {
        this.ApplicationCache.Setup(a => a.GetConfiguration()).Returns((Configuration)null);

        LogonCommands.RefreshTokenCommand request = new(TestData.RefreshToken);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        this.AuthenticationService.Verify(a => a.RefreshAccessToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_GetConfigurationRequest_CacheMiss_ServiceIsCalled_ConfigIsCached()
    {
        this.ApplicationCache.Setup(a => a.GetConfiguration()).Returns((Configuration)null);
        this.ConfigurationService.Setup(c => c.GetConfiguration(It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Success(new Configuration()));

        LogonQueries.GetConfigurationQuery request = new LogonQueries.GetConfigurationQuery(TestData.DeviceIdentifier);

        Result<Configuration> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        this.ConfigurationService.Verify(c => c.GetConfiguration(It.IsAny<String>(), It.IsAny<CancellationToken>()), Times.Once);
        this.ApplicationCache.Verify(a => a.SetConfiguration(It.IsAny<Configuration>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_GetConfigurationRequest_CacheHit_ServiceIsNotCalled()
    {
        this.ApplicationCache.Setup(a => a.GetConfiguration()).Returns(new Configuration());

        LogonQueries.GetConfigurationQuery request = new LogonQueries.GetConfigurationQuery(TestData.DeviceIdentifier);

        Result<Configuration> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        this.ConfigurationService.Verify(c => c.GetConfiguration(It.IsAny<String>(), It.IsAny<CancellationToken>()), Times.Never);
        this.ApplicationCache.Verify(a => a.SetConfiguration(It.IsAny<Configuration>(), It.IsAny<MemoryCacheEntryOptions>()), Times.Never);
    }
}
