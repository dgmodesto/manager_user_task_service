using ManagerUserTaskApi.Infrastructure.Services.Authentication.Requests;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Responses;

namespace ManagerUserTaskApi.Infrastructure.Services.Authentication;

public interface IAuthenticationService
{
    Task<LoginResponse> Login(LoginRequest request);

}