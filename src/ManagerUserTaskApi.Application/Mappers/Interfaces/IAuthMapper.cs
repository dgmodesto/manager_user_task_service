using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Domain.Responses;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Requests;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Responses;

namespace ManagerUserTaskApi.Application.Mappers.Interfaces;
public interface IAuthMapper
{
    LoginRequest Map(LoginUser request);
    UserLogged Map(LoginResponse response);
}