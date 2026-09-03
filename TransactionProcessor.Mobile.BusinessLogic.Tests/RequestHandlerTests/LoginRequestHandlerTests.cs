using Microsoft.Extensions.Caching.Memory;
using Imposter.Abstractions;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.RequestHandlers;
using TransactionProcessor.Mobile.BusinessLogic.Requests;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.RequestHandlerTests;

public class LoginRequestHandlerTests
{
    private IAuthenticationServiceImposter AuthenticationService = null;
    private IConfigurationServiceImposter ConfigurationService = null;
    private LoginRequestHandler LoginRequestHandler = null;
    private readonly IApplicationCacheImposter ApplicationCache;
    private Func<Boolean, IAuthenticationService> AuthenticationServiceResolver;
    private Func<Boolean, IConfigurationService> ConfigurationServiceResolver;

    public LoginRequestHandlerTests() {
        this.AuthenticationService = new IAuthenticationServiceImposter();
        this.ConfigurationService= new IConfigurationServiceImposter();
        this.ApplicationCache = new IApplicationCacheImposter();
        this.AuthenticationServiceResolver = _ => this.AuthenticationService.Instance();
        this.ConfigurationServiceResolver = _ => this.ConfigurationService.Instance();

        this.LoginRequestHandler = new LoginRequestHandler(this.AuthenticationServiceResolver, this.ConfigurationServiceResolver, this.ApplicationCache.Instance());
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_LoginRequest_IsHandled()
    {
        this.ApplicationCache.GetConfiguration().Returns(new Configuration { ClientId = TestData.ClientId, ClientSecret = TestData.ClientSecret });
        this.AuthenticationService.GetToken(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.AccessToken));

        LogonCommands.GetTokenCommand request = new(TestData.UserName, TestData.Password);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.AccessToken.ShouldBe(TestData.Token);
        result.Data.ExpiryInMinutes.ShouldBe(TestData.TokenExpiryInMinutes);
        result.Data.RefreshToken.ShouldBe(TestData.RefreshToken);
        this.AuthenticationService.GetToken(TestData.UserName, TestData.Password, TestData.ClientId, TestData.ClientSecret, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_LoginRequest_ConfigurationNotCached_FailureReturned()
    {
        this.ApplicationCache.GetConfiguration().Returns((Configuration)null);

        LogonCommands.GetTokenCommand request = new(TestData.UserName, TestData.Password);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        this.AuthenticationService.GetToken(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_RefreshTokenRequest_IsHandled()
    {
        this.ApplicationCache.GetConfiguration().Returns(new Configuration { ClientId = TestData.ClientId, ClientSecret = TestData.ClientSecret });
        this.AuthenticationService.RefreshAccessToken(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(TestData.AccessToken));

        LogonCommands.RefreshTokenCommand request = new(TestData.RefreshToken);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.AccessToken.ShouldBe(TestData.Token);
        result.Data.ExpiryInMinutes.ShouldBe(TestData.TokenExpiryInMinutes);
        result.Data.RefreshToken.ShouldBe(TestData.RefreshToken);
        this.AuthenticationService.RefreshAccessToken(TestData.RefreshToken, TestData.ClientId, TestData.ClientSecret, Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_RefreshTokenRequest_ConfigurationNotCached_FailureReturned()
    {
        this.ApplicationCache.GetConfiguration().Returns((Configuration)null);

        LogonCommands.RefreshTokenCommand request = new(TestData.RefreshToken);

        Result<TokenResponseModel> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
        this.AuthenticationService.RefreshAccessToken(Arg<String>.Any(), Arg<String>.Any(), Arg<String>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_GetConfigurationRequest_CacheMiss_ServiceIsCalled_ConfigIsCached()
    {
        this.ApplicationCache.GetConfiguration().Returns((Configuration)null);
        this.ConfigurationService.GetConfiguration(Arg<String>.Any(), Arg<CancellationToken>.Any()).ReturnsAsync(Result.Success(new Configuration()));

        LogonQueries.GetConfigurationQuery request = new LogonQueries.GetConfigurationQuery(TestData.DeviceIdentifier);

        Result<Configuration> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        this.ConfigurationService.GetConfiguration(Arg<String>.Any(), Arg<CancellationToken>.Any()).Called(Count.Once());
        this.ApplicationCache.SetConfiguration(Arg<Configuration>.Any(), Arg<MemoryCacheEntryOptions>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task LoginRequestHandler_Handle_GetConfigurationRequest_CacheHit_ServiceIsNotCalled()
    {
        this.ApplicationCache.GetConfiguration().Returns(new Configuration());

        LogonQueries.GetConfigurationQuery request = new LogonQueries.GetConfigurationQuery(TestData.DeviceIdentifier);

        Result<Configuration> result = await this.LoginRequestHandler.Handle(request, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldNotBeNull();
        this.ConfigurationService.GetConfiguration(Arg<String>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        this.ApplicationCache.SetConfiguration(Arg<Configuration>.Any(), Arg<MemoryCacheEntryOptions>.Any()).Called(Count.Never());
    }
}
