using System.Text.Json.Serialization;

namespace ManagerUserTaskApi.Infrastructure.Services.Authentication.Requests;

public class LoginRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}