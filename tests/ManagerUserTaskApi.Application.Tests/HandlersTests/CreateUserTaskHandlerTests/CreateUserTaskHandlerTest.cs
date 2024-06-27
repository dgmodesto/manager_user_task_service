using ManagerUserTaskApi.Application.Events;
using ManagerUserTaskApi.Application.Handlers;
using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using Moq;
using Sdk.Mediator.Abstractions.Notifications;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ManagerUserTaskApi.Application.Tests.HandlersTests.CreateUserTaskHandlerTests;

[Collection(nameof(CreateUserTaskHandlerCollection))]
public class CreateUserTaskHandlerTest
{

    private CreateUserTaskHandler _handler;
    private readonly CreateUserTaskHandlerFixture _fixture;

    public CreateUserTaskHandlerTest(CreateUserTaskHandlerFixture fixture)
    {
        _fixture = fixture;
        _handler = _fixture.GetCreateUserTaskHandler();
    }


    [Fact]
    public async Task CreateUserTaskHandler_Handle_CreateWithSuccess()
    {
        // Arrange
        var request = _fixture.GenerateCreateUserTaskRequest();
        var response = _fixture.GenerateUserTaskModel();
        var entity = _fixture.GenerateUserTask();

        _fixture.IMediatorHandlerMock
            .Setup(m => m.RaiseEvent(It.IsAny<DomainNotification>()));


        _fixture.IUserTaskMapperMock
            .Setup(m => m.Map(It.IsAny<CreateUserTask>()))
            .Returns(entity);

        _fixture.IUserTaskMapperMock
            .Setup(m => m.Map(It.IsAny<UserTask>()))
            .Returns(response);

        _fixture.IUserTaskRepositoryMock
            .Setup(r => r.InsertAsync<UserTaskCreated>(It.IsAny<UserTask>()));


        var result = await _handler.Handle(request, It.IsAny<CancellationToken>());



        // Assert
        _fixture.IUserTaskMapperMock
            .Verify(m => m.Map(It.IsAny<CreateUserTask>())
            , Times.Once);


        _fixture.IUserTaskMapperMock
         .Verify(m => m.Map(It.IsAny<UserTask>())
         , Times.Once);


        _fixture.IUserTaskRepositoryMock
            .Verify(r => r.InsertAsync<UserTaskCreated>(It.IsAny<UserTask>())
            , Times.Once);


    }
}
