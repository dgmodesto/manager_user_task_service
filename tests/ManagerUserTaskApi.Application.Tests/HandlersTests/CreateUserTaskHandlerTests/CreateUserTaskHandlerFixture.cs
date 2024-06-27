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

namespace ManagerUserTaskApi.Application.Tests.HandlersTests.CreateUserTaskHandlerTests;

[CollectionDefinition(nameof(CreateUserTaskHandlerCollection))]
public class CreateUserTaskHandlerCollection : ICollectionFixture<CreateUserTaskHandlerFixture> { }
public class CreateUserTaskHandlerFixture
{
    public Mock<IMediatorHandler> IMediatorHandlerMock;
    public Mock<IUserTaskMapper> IUserTaskMapperMock;
    public Mock<IUserTaskRepository> IUserTaskRepositoryMock;

    public CreateUserTaskHandler GetCreateUserTaskHandler()
    {
        var mocker = new AutoMoqer();
        mocker.Create<CreateUserTaskHandler>();
        var handler = mocker.Resolve<CreateUserTaskHandler>();

        IMediatorHandlerMock = mocker.GetMock<IMediatorHandler>();
        IUserTaskMapperMock = mocker.GetMock<IUserTaskMapper>();
        IUserTaskRepositoryMock = mocker.GetMock<IUserTaskRepository>();

        return handler;
    }

    public CreateUserTask GenerateCreateUserTaskRequest()
    {
        var command = new Faker<CreateUserTask>()
            .CustomInstantiator(f => new CreateUserTask
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
