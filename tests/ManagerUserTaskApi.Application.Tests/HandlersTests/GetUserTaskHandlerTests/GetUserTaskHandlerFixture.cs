using AutoMoq;
using Bogus;
using ManagerUserTaskApi.Application.Handlers;
using ManagerUserTaskApi.Application.Mappers.Interfaces;
using ManagerUserTaskApi.Domain.Models;
using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using ManagerUserTaskApi.Infrastructure.Database.Interfaces;
using Moq;
using Sdk.Mediator.Abstractions.Interfaces;
using Xunit;

namespace ManagerUserTaskApi.Application.Tests.HandlersTests.GetUserTaskHandlerTests;

[CollectionDefinition(nameof(GetUserTaskHandlerCollection))]
public class GetUserTaskHandlerCollection : ICollectionFixture<GetUserTaskHandlerFixture> { }
public class GetUserTaskHandlerFixture
{
    public Mock<IMediatorHandler> IMediatorHandlerMock;
    public Mock<IUserTaskMapper> IUserTaskMapperMock;
    public Mock<IUserTaskRepository> IUserTaskRepositoryMock;

    public GetUserTaskHandler GetGetUserTaskHandler()
    {
        var mocker = new AutoMoqer();
        mocker.Create<GetUserTaskHandler>();
        var handler = mocker.Resolve<GetUserTaskHandler>();

        IMediatorHandlerMock = mocker.GetMock<IMediatorHandler>();
        IUserTaskMapperMock = mocker.GetMock<IUserTaskMapper>();
        IUserTaskRepositoryMock = mocker.GetMock<IUserTaskRepository>();

        return handler;
    }

    public GetUserTask GenerateGetUserTask()
    {
        var command = new Faker<GetUserTask>()
            .CustomInstantiator(f => new GetUserTask
            {
                Id = f.Random.Guid()
            });

        return command;
    }

    public UserTaskModel GenerateUserTaskModel()
    {
        var command = new Faker<UserTaskModel>()
            .CustomInstantiator(f => new UserTaskModel
            {
                Id = f.Random.Guid().ToString(),
                User = f.Person.UserName,
                Date = f.Date.Recent(),
                Description = f.Random.Words(),
                Subject = f.Random.Words(),
                StartTime = f.Date.Recent(),
                EndTime = f.Date.Recent(),
                CreatedAt = f.Date.Recent(),
                UpdatedAt = f.Date.Recent(),
                Version = f.Random.Number(1, 3)
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
