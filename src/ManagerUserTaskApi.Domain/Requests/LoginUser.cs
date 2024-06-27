using ManagerUserTaskApi.Domain.Responses;
using Sdk.Mediator.Abstractions.Requests;
using System.Text.Json.Serialization;

namespace ManagerUserTaskApi.Domain.Requests;

public class LoginUser : TwoWayRequestBase<UserLogged>
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
