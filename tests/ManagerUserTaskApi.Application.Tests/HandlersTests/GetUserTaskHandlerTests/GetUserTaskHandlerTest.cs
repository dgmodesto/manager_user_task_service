using ManagerUserTaskApi.Application.Handlers;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using Moq;
using Sdk.Mediator.Abstractions.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ManagerUserTaskApi.Application.Tests.HandlersTests.GetUserTaskHandlerTests;

[Collection(nameof(GetUserTaskHandlerCollection))]
public class GetUserTaskHandlerTest
{
    private GetUserTaskHandler _handler;
    private readonly GetUserTaskHandlerFixture _fixture;

    public GetUserTaskHandlerTest(GetUserTaskHandlerFixture fixture)
    {
        _fixture = fixture;
        _handler = _fixture.GetGetUserTaskHandler();
    }


    [InlineData(true)]
    [InlineData(false)]
    [Theory]
    public async Task GetUserTaskHandler_Handle_GetUserTaskWithSuccess(bool hasEntity)
    {
        // Arrange
        var request = _fixture.GenerateGetUserTask();
        var entity = hasEntity ? _fixture.GenerateUserTask() : null;
        var model = _fixture.GenerateUserTaskModel();

        _fixture.IMediatorHandlerMock
            .Setup(m => m.RaiseEvent(It.IsAny<DomainNotification>()));

        _fixture.IUserTaskRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(entity);

        _fixture.IUserTaskMapperMock
            .Setup(r => r.Map(It.IsAny<UserTask>()))
            .Returns(model);


        // Act
        var result = await _handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        _fixture.IUserTaskRepositoryMock
            .Verify(r => r.GetByIdAsync(It.IsAny<Guid>())
            , Times.Once);

        _fixture.IUserTaskMapperMock
          .Verify(r => r.Map(It.IsAny<UserTask>())
          , hasEntity ? Times.Once : Times.Never);


        _fixture.IMediatorHandlerMock
            .Verify(m => m.RaiseEvent(It.IsAny<DomainNotification>())
            , !hasEntity ? Times.AtLeastOnce : Times.Never);

    }
}
