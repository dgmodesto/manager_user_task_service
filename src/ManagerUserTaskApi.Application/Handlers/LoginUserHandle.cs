using ManagerUserTaskApi.Application.Mappers.Interfaces;
using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Domain.Responses;
using ManagerUserTaskApi.Infrastructure.Services.Authentication;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Requests;
using Sdk.Mediator.Abstractions.Enums;
using Sdk.Mediator.Abstractions.Handlers;
using Sdk.Mediator.Abstractions.Interfaces;
using Sdk.Mediator.Notifications;

namespace ManagerUserTaskApi.Application.Handlers;

public class LoginUserHandle : TwoWayCommandHandler<LoginUser, UserLogged>
{

    private readonly IAuthenticationService _authenticationService;
    private readonly IAuthMapper _mapper;
    private readonly DomainNotificationHandler _notification;
    public LoginUserHandle(
        IMediatorHandler mediator,
        IAuthenticationService authenticationService,
        IAuthMapper mapper)
            : base(mediator)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
        _notification = (DomainNotificationHandler)mediator.GetNotificationHandler();
    }

    protected override async Task<UserLogged?> Handle(LoginUser request)
    {
        var requestIdp = _mapper.Map(request) ?? new LoginRequest();
        var loginResponse = await _authenticationService.Login(requestIdp);

        if (loginResponse == null)
        {
            await NotifyError(FailureReason.BadRequest, "Invalid username or password");
        }

        var response = _mapper.Map(loginResponse) ?? new UserLogged();
        return response;
    }
}
