using AutoMoq;
using Bogus;
using ManagerUserTaskApi.Application.Handlers;
using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using ManagerUserTaskApi.Infrastructure.Database.Interfaces;
using Moq;
using Sdk.Mediator.Abstractions.Interfaces;
using Xunit;

namespace ManagerUserTaskApi.Application.Tests.HandlersTests.DeleteUserTaskHandlerTests;

[CollectionDefinition(nameof(DeleteUserTaskHandler))]
public class DeleteUserTaskHandlerCollection : ICollectionFixture<DeleteUserTaskHandlerFixture> { }
public class DeleteUserTaskHandlerFixture
{

    public Mock<IMediatorHandler> IMediatorHandlerMock;
    public Mock<IUserTaskRepository> IUserTaskRepositoryMock;

    public DeleteUserTaskHandler GetDeleteUserTaskHandler()
    {
        var mocker = new AutoMoqer();
        mocker.Create<DeleteUserTaskHandler>();
        var handler = mocker.Resolve<DeleteUserTaskHandler>();

        IMediatorHandlerMock = mocker.GetMock<IMediatorHandler>();
        IUserTaskRepositoryMock = mocker.GetMock<IUserTaskRepository>();

        return handler;
    }


    public DeleteUserTask GenerateDeleteUserTaskRequest()
    {
        var command = new Faker<DeleteUserTask>()
            .CustomInstantiator(f => new DeleteUserTask
            {
                Id = f.Random.Guid()
            });

        return command;
    }

    public UserTask GenerateUserTask()
    {
        var command = new Faker<UserTask>()
            .CustomInstantiator(f => new UserTask
            {

                User = f.Person.UserName,
                Date = f.Date.Recent(),
                Description = f.Random.Words(),
                Subject = f.Random.Words(),
                StartTime = f.Date.Recent(),
                EndTime = f.Date.Recent(),

            });

        return command;
    }

}
