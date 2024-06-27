using ManagerUserTaskApi.Application.Events;
using ManagerUserTaskApi.Application.Handlers;
using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using MediatR;
using Moq;
using Sdk.Mediator.Abstractions.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ManagerUserTaskApi.Application.Tests.HandlersTests.DeleteUserTaskHandlerTests;

[Collection(nameof(DeleteUserTaskHandler))]
public class DeleteUserTaskHandlerTest
{
    private DeleteUserTaskHandler _handler;
    private readonly DeleteUserTaskHandlerFixture _fixture;

    public DeleteUserTaskHandlerTest(DeleteUserTaskHandlerFixture fixture)
    {
        _fixture = fixture;
        _handler = _fixture.GetDeleteUserTaskHandler();
    }

    [InlineData(true)]
    [InlineData(false)]
    [Theory]
    public async Task DeleteUserTaskHandler_Handle_DeleteWithSuccess(bool hasEntity)
    {
        // Arrange
        var request = _fixture.GenerateDeleteUserTaskRequest();
        var entity = hasEntity ? _fixture.GenerateUserTask() : null;

        _fixture.IMediatorHandlerMock
            .Setup(m => m.RaiseEvent(It.IsAny<DomainNotification>()));

        _fixture.IUserTaskRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(entity);

        _fixture.IUserTaskRepositoryMock
            .Setup(r => r.DeleteAsync<UserTaskDeleted>(It.IsAny<UserTask>()));

        IRequestHandler<DeleteUserTask> handler = new DeleteUserTaskHandler(
           _fixture.IMediatorHandlerMock.Object,
           _fixture.IUserTaskRepositoryMock.Object);


        // Act
        var result = await handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        _fixture.IUserTaskRepositoryMock
            .Verify(r => r.GetByIdAsync(It.IsAny<Guid>())
            , Times.Once);

        _fixture.IUserTaskRepositoryMock
          .Verify(r => r.DeleteAsync<UserTaskDeleted>(It.IsAny<UserTask>())
          , hasEntity ? Times.Once : Times.Never);


        _fixture.IMediatorHandlerMock
            .Verify(m => m.RaiseEvent(It.IsAny<DomainNotification>())
            , !hasEntity ? Times.AtLeastOnce : Times.Never);

    }
}
