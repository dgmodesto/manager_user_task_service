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

namespace ManagerUserTaskApi.Api.Tests.ControllersTests.UserTaskControllerTests;

[Collection(nameof(UserTaskControllerCollection))]
public class UserTaskControllerTest
{
    private UserTaskController _controller;
    private readonly UserTaskControllerFixture _fixture;

    public UserTaskControllerTest(UserTaskControllerFixture fixture)
    {
        _fixture = fixture;
        _controller = _fixture.GetUserTaskController();
    }

    [Fact]
    public async Task UserTaskController_ListUserTaskGroupAsync_ReturnStatus200OK()
    {
        //Arrange
        var request = _fixture.GenerateListUserTaskGroupRequest(1);
        var models = _fixture.GenerateUserTaskGroupModel(2);
        var pagedModel = _fixture.GeneratePagedUserTaskGroupModel(models);

        var domainNotificationHandler = new DomainNotificationHandler();

        _fixture.MediatorHandlerMock!
            .Setup(m => m.GetNotificationHandler())
            .Returns(domainNotificationHandler);

        _fixture.MediatorHandlerMock
            .Setup(x => x.SendQuery(It.IsAny<ListUserTaskGroup>()))
            .Returns(Task.FromResult(pagedModel));

        _fixture.DomainNotificationHandlerMock!
            .Setup(x => x.GetNotifications())
            .Returns(It.IsAny<List<DomainNotification>>());

        _fixture.DomainNotificationHandlerMock
            .Setup(n => n.HasNotifications())
            .Returns(It.IsAny<bool>());

        _fixture.ControllerBaseMock!
            .Setup(a => a.Ok(pagedModel));

        _controller = new UserTaskController(_fixture.MediatorHandlerMock!.Object);

        ////Act
        var response = await _controller.ListUserTaskGroupAsync(request);

        //Assert
        var okObjectResult = response.Result as OkObjectResult;
        Assert.NotNull(okObjectResult);
        Assert.Equal((int)HttpStatusCode.OK, okObjectResult?.StatusCode);
    }

    [Fact]
    public async Task SecurityWifiController_List_ReturnStatus400BadRequest()
    {
        //Arrange
        var request = _fixture.GenerateListUserTaskGroupRequest(1);
        var models = _fixture.GenerateUserTaskGroupModel(2);
        var pagedModel = _fixture.GeneratePagedUserTaskGroupModel(models);

        var notificationErrors = new List<DomainNotification>();
        var message = new DomainNotification(FailureReason.BadRequest, "error", "badRequest");

        var domainNotificationHandler = new DomainNotificationHandler();
        domainNotificationHandler?.Handle(message, new CancellationToken());

        _fixture.MediatorHandlerMock!
            .Setup(m => m.GetNotificationHandler())
            .Returns(domainNotificationHandler);

        _fixture.MediatorHandlerMock
            .Setup(x => x.SendQuery(It.IsAny<ListUserTaskGroup>()))
            .Returns(Task.FromResult(pagedModel));

        _fixture.DomainNotificationHandlerMock!
            .Setup(x => x.GetNotifications())
            .Returns(It.IsAny<List<DomainNotification>>());

        _fixture.DomainNotificationHandlerMock
            .Setup(n => n.HasNotifications())
            .Returns(It.IsAny<bool>());

        _fixture.ControllerBaseMock!
            .Setup(a => a.Ok(pagedModel));

        _controller = new UserTaskController(_fixture.MediatorHandlerMock!.Object);

        ////Act
        var response = await _controller.ListUserTaskGroupAsync(request);

        //Assert
        var okObjectResult = response.Result as BadRequestObjectResult;
        Assert.NotNull(okObjectResult);
        Assert.Equal((int)HttpStatusCode.BadRequest, okObjectResult?.StatusCode);
    }
}
