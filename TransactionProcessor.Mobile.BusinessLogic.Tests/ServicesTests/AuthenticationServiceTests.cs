using Moq;
using SecurityService.Client;
using SecurityService.DataTransferObjects.Responses;
using Shouldly;
using SimpleResults;
using TransactionProcessor.Mobile.BusinessLogic.Logging;
using TransactionProcessor.Mobile.BusinessLogic.Models;
using TransactionProcessor.Mobile.BusinessLogic.Services;

namespace TransactionProcessor.Mobile.BusinessLogic.Tests.ServicesTests
{
    public class AuthenticationServiceTests{
        private readonly IAuthenticationService AuthenticationService;

        private readonly  Mock<ISecurityServiceClient> SecurityServiceClient;

        private readonly Mock<IApplicationCache> ApplicationCache;
        public AuthenticationServiceTests(){
            Logger.Initialise(new NullLogger());
            this.SecurityServiceClient = new Mock<ISecurityServiceClient>();
            this.ApplicationCache = new Mock<IApplicationCache>();
            this.AuthenticationService = new AuthenticationService(this.SecurityServiceClient.Object,
                                                                   this.ApplicationCache.Object);

            // Standard cache mocking
            this.ApplicationCache.Setup(a => a.GetConfiguration()).Returns(new Configuration());

        }

        [Fact]
        public async Task AuthenticationService_GetToken_TokenReturned(){
            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TokenResponse.Create(TestData.Token, TestData.RefreshToken, TestData.TokenExpiryInMinutes));
            
            Result<TokenResponseModel> token = await this.AuthenticationService.GetToken(TestData.UserName, TestData.Password, CancellationToken.None);
            token.IsSuccess.ShouldBeTrue();
            token.Data.AccessToken.ShouldBe(TestData.Token);
            token.Data.RefreshToken.ShouldBe(TestData.RefreshToken);
            token.Data.ExpiryInMinutes.ShouldBe(TestData.TokenExpiryInMinutes);
        }

        [Fact]
        public async Task AuthenticationService_GetToken_SecurityClientFailed_TokenNotReturned()
        {
            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

            Result<TokenResponseModel> token = await this.AuthenticationService.GetToken(TestData.UserName, TestData.Password, CancellationToken.None);
            token.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AuthenticationService_GetToken_SecurityClientThrowsException_TokenNotReturned()
        {
            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            Result<TokenResponseModel> token = await this.AuthenticationService.GetToken(TestData.UserName, TestData.Password, CancellationToken.None);
            token.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AuthenticationService_RefreshAccessToken_TokenReturned()
        {
            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(TokenResponse.Create(TestData.Token, TestData.RefreshToken, TestData.TokenExpiryInMinutes));
            
            Result<TokenResponseModel> token = await this.AuthenticationService.RefreshAccessToken(TestData.RefreshToken, CancellationToken.None);
            token.IsSuccess.ShouldBeTrue();
            token.Data.AccessToken.ShouldBe(TestData.Token);
            token.Data.RefreshToken.ShouldBe(TestData.RefreshToken);
            token.Data.ExpiryInMinutes.ShouldBe(TestData.TokenExpiryInMinutes);
        }

        [Fact]
        public async Task AuthenticationService_RefreshAccessToken_SecurityClientFailed_TokenNotReturned()
        {
            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Failure());

            Result<TokenResponseModel> token = await this.AuthenticationService.RefreshAccessToken(TestData.RefreshToken, CancellationToken.None);

            token.IsFailed.ShouldBeTrue();
        }

        [Fact]
        public async Task AuthenticationService_RefreshAccessToken_SecurityClientThrowsException_TokenNotReturned()
        {
            this.SecurityServiceClient.Setup(s => s.GetToken(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            Result<TokenResponseModel> token = await this.AuthenticationService.RefreshAccessToken(TestData.RefreshToken, CancellationToken.None);
            
            token.IsFailed.ShouldBeTrue();
        }
    }
}
