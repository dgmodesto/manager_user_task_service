using AutoMoq;
using Bogus;
using ManagerUserTaskApi.Api.Controllers;
using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Sdk.Mediator.Abstractions.Interfaces;
using Sdk.Mediator.Notifications;
using Xunit;

namespace ManagerUserTaskApi.Api.Tests.ControllersTests.AuthControllerTests;

[CollectionDefinition(nameof(AuthControllerCollection))]
public class AuthControllerCollection : ICollectionFixture<AuthControllerFixture> { }
public class AuthControllerFixture
{

    public Mock<IHttpContextAccessor>? HttpContextAccessorMock;

    public Mock<IMediatorHandler> MediatorHandlerMock;
    public Mock<DomainNotificationHandler>? DomainNotificationHandlerMock;
    public Mock<ControllerBase>? ControllerBaseMock;
    public Mock<ILogger<DomainNotificationHandler>> ILoggerMock;

    public AuthController GetAuthController()
    {
        var mocker = new AutoMoqer();

        ILoggerMock = mocker.GetMock<ILogger<DomainNotificationHandler>>();
        MediatorHandlerMock = mocker.GetMock<IMediatorHandler>();
        DomainNotificationHandlerMock = new Mock<DomainNotificationHandler>(ILoggerMock.Object);
        ControllerBaseMock = mocker.GetMock<ControllerBase>();
        HttpContextAccessorMock = mocker.GetMock<IHttpContextAccessor>();

        return new AuthController(MediatorHandlerMock!.Object);
    }

    public LoginResponse GenerateLoginUserResponse()
    {
        return new Faker<LoginResponse>()
           .CustomInstantiator(f => new LoginResponse
           {
               AccessToken = f.Random.AlphaNumeric(100),
               RefreshToken = f.Random.AlphaNumeric(100),
               RefreshExpiresIn = f.Random.Number(360, 3600),
               ExpiresIn = f.Random.Number(360, 3600),
           });
    }

    public LoginUser GenerateLoginUserRequest()
    {
        return new Faker<LoginUser>()
           .CustomInstantiator(f => new LoginUser
           {
               Username = f.Person.UserName,
               Password = f.Random.AlphaNumeric(10)
           });
    }



}
