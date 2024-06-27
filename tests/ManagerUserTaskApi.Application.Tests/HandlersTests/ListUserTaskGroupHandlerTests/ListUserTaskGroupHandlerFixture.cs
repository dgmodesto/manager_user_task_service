using AutoMoq;
using Bogus;
using ManagerUserTaskApi.Application.Handlers;
using ManagerUserTaskApi.Domain.Models;
using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Infrastructure.Database.Entities;
using ManagerUserTaskApi.Infrastructure.Database.Interfaces;
using Moq;
using Sdk.Db.Abstractions.Pagination;
using Sdk.Mediator.Abstractions.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ManagerUserTaskApi.Application.Tests.HandlersTests.ListUserTaskGroupHandlerTests;

[CollectionDefinition(nameof(ListUserTaskGroupHandlerCollection))]
public class ListUserTaskGroupHandlerCollection : ICollectionFixture<ListUserTaskGroupHandlerFixture> { }
public class ListUserTaskGroupHandlerFixture
{
    public Mock<IMediatorHandler> IMediatorHandlerMock;
    public Mock<IUserTaskRepository> IUserTaskRepositoryMock;

    public ListUserTaskGroupHandler GetListUserTaskGroup()
    {
        var mocker = new AutoMoqer();
        mocker.Create<ListUserTaskGroupHandler>();
        var handler = mocker.Resolve<ListUserTaskGroupHandler>();

        IMediatorHandlerMock = mocker.GetMock<IMediatorHandler>();
        IUserTaskRepositoryMock = mocker.GetMock<IUserTaskRepository>();

        return handler;
    }

    public PaginatedList<UserTaskGroup> GeneratePaginatedListUserTaskGroupEntity(int quantity)
    {
        var userTaskGroup = GenerateUserTaskGroup(quantity);

        var page = new Page(1, 10);

        var order = new Order("Date", true);

        var paginationObject = new PaginationObject
        {
            Page = page,
            Order = order,
            TotalRecords = userTaskGroup.Count
        };

        var paginatedList = new PaginatedList<UserTaskGroup>(userTaskGroup.AsQueryable(), paginationObject);
        return paginatedList;
    }

    public List<UserTaskGroup> GenerateUserTaskGroup(int quantity)
    {
        var entity = new Faker<UserTaskGroup>()
            .CustomInstantiator(f => new UserTaskGroup
            {
                Date = f.Date.Recent(),
                UserTasks = GenerateUserTasks(quantity),
            }).Generate(quantity);

        return entity;
    }

    private List<UserTask> GenerateUserTasks(int quantity)
    {
        return new Faker<UserTask>()
            .CustomInstantiator(f => new UserTask
            {
                User = f.Person.UserName,
                Date = f.Date.Recent(),
                Description = f.Random.Words(),
                Subject = f.Random.Words(),
                StartTime = f.Date.Recent(),
                EndTime = f.Date.Recent(),

            }).Generate(quantity);
    }

    public ListUserTaskGroup GenerateListUserTaskGroupRequest()
    {
        var command = new Faker<ListUserTaskGroup>()
            .CustomInstantiator(f => new ListUserTaskGroup
            {
                Page = f.Random.Number(1, 10),
                PageSize = f.Random.Number(1, 10),
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
