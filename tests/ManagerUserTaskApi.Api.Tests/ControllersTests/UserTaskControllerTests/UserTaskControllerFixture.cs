using AutoMoq;
using Bogus;
using ManagerUserTaskApi.Api.Controllers;
using ManagerUserTaskApi.Domain.Models;
using ManagerUserTaskApi.Domain.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sdk.Mediator.Abstractions.Enums;
using Sdk.Mediator.Abstractions.Interfaces;
using Sdk.Mediator.Abstractions.Responses;
using Sdk.Mediator.Notifications;
using System.Collections.Generic;
using Xunit;

namespace ManagerUserTaskApi.Api.Tests.ControllersTests.UserTaskControllerTests;


[CollectionDefinition(nameof(UserTaskControllerCollection))]
public class UserTaskControllerCollection : ICollectionFixture<UserTaskControllerFixture> { }
public class UserTaskControllerFixture
{
    public Mock<IHttpContextAccessor>? HttpContextAccessorMock;

    public Mock<IMediatorHandler> MediatorHandlerMock;
    public Mock<DomainNotificationHandler>? DomainNotificationHandlerMock;
    public Mock<ControllerBase>? ControllerBaseMock;
    public Mock<ILogger<DomainNotificationHandler>> ILoggerMock;

    public UserTaskController GetUserTaskController()
    {
        var mocker = new AutoMoqer();

        ILoggerMock = mocker.GetMock<ILogger<DomainNotificationHandler>>();
        MediatorHandlerMock = mocker.GetMock<IMediatorHandler>();
        DomainNotificationHandlerMock = new Mock<DomainNotificationHandler>(ILoggerMock.Object);
        ControllerBaseMock = mocker.GetMock<ControllerBase>();
        HttpContextAccessorMock = mocker.GetMock<IHttpContextAccessor>();

        return new UserTaskController(MediatorHandlerMock!.Object);
    }
    public Paged<UserTaskGroupModel> GeneratePagedUserTaskGroupModel(List<UserTaskGroupModel> models)
    {
        return new Paged<UserTaskGroupModel>
        {
            CurrentPage = 1,
            FilterBy = "prop",
            OrderBy = "prop",
            PageSize = 10,
            Records = models,
            RecordsInPage = models.Count,
            Sorting = Sorting.Asc,
            TotalPages = 1,
            TotalRecords = models.Count
        };
    }

    public List<UserTaskGroupModel> GenerateUserTaskGroupModel(int quantity)
    {
        return new Faker<UserTaskGroupModel>()
            .CustomInstantiator(f => new UserTaskGroupModel
            {
                Date = f.Date.Recent(),
                UserTasks = GenerateUserTasks(quantity),

            }).Generate(quantity);
    }

    private List<UserTaskModel> GenerateUserTasks(int quantity)
    {
        return new Faker<UserTaskModel>()
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


            }).Generate(quantity);
    }

    public ListUserTaskGroup GenerateListUserTaskGroupRequest(int quantity)
    {
        return new Faker<ListUserTaskGroup>()
            .CustomInstantiator(f => new ListUserTaskGroup
            {
                Page = 1,
                PageSize = quantity
            });
    }

}
