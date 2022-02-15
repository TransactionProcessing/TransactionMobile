namespace TransactionMobile.Maui.BusinessLogic.Tests.RequestHandlerTests;

using System;
using System.Threading;
using System.Threading.Tasks;
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
        LoginRequestHandler handler = new LoginRequestHandler(authenticationService.Object);
            
        LoginRequest request = LoginRequest.Create(TestData.UserName,TestData.Password);

        String accessToken = await handler.Handle(request, CancellationToken.None);

        accessToken.ShouldBe(TestData.AccessToken);
    }
}