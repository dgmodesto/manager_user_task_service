using ManagerUserTaskApi.Api.Controllers;
using ManagerUserTaskApi.Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sdk.Mediator.Abstractions.Enums;
using Sdk.Mediator.Abstractions.Notifications;
using Sdk.Mediator.Notifications;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ManagerUserTaskApi.Api.Tests.ControllersTests.AuthControllerTests;

[Collection(nameof(AuthControllerCollection))]
public class AuthControllerTest
{

    private AuthController _controller;
    private readonly AuthControllerFixture _fixture;

    public AuthControllerTest(AuthControllerFixture fixture)
    {
        _fixture = fixture;
        _controller = _fixture.GetAuthController();
    }

    [Fact]
    public async Task AuthController_Login_ReturnStatus200OK()
    {
        ////Arrange
        var request = _fixture.GenerateLoginUserRequest();

        var domainNotificationHandler = new DomainNotificationHandler();

        _fixture.MediatorHandlerMock!
            .Setup(m => m.GetNotificationHandler())
            .Returns(domainNotificationHandler);

        _fixture.MediatorHandlerMock
            .Setup(x => x.SendQuery(It.IsAny<LoginUser>()));

        _fixture.DomainNotificationHandlerMock!
            .Setup(x => x.GetNotifications())
            .Returns(It.IsAny<List<DomainNotification>>());

        _fixture.DomainNotificationHandlerMock
            .Setup(n => n.HasNotifications())
            .Returns(It.IsAny<bool>());

        _controller = new AuthController(_fixture.MediatorHandlerMock!.Object);

        ////Act
        var response = await _controller.Login(request);

        //Assert
        var okObjectResult = response.Result as OkObjectResult;
        Assert.NotNull(okObjectResult);
        Assert.Equal((int)HttpStatusCode.OK, okObjectResult?.StatusCode);
    }

    [Fact]
    public async Task AuthController_Login_ReturnStatus400BadRequest()
    {
        ////Arrange
        var request = _fixture.GenerateLoginUserRequest();


        var notificationErrors = new List<DomainNotification>();
        var message = new DomainNotification(FailureReason.BadRequest, "error", "badRequest");

        var domainNotificationHandler = new DomainNotificationHandler();
        domainNotificationHandler?.Handle(message, new CancellationToken());

        _fixture.MediatorHandlerMock!
            .Setup(m => m.GetNotificationHandler())
            .Returns(domainNotificationHandler);

        _fixture.MediatorHandlerMock
            .Setup(x => x.SendQuery(It.IsAny<LoginUser>()));

        _fixture.DomainNotificationHandlerMock!
            .Setup(x => x.GetNotifications())
            .Returns(It.IsAny<List<DomainNotification>>());

        _fixture.DomainNotificationHandlerMock
            .Setup(n => n.HasNotifications())
            .Returns(It.IsAny<bool>());

        _controller = new AuthController(_fixture.MediatorHandlerMock!.Object);

        ////Act
        var response = await _controller.Login(request);

        //Assert
        var okObjectResult = response.Result as BadRequestObjectResult;
        Assert.NotNull(okObjectResult);
        Assert.Equal((int)HttpStatusCode.BadRequest, okObjectResult?.StatusCode);
    }
}
