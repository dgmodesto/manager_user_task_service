using System.Text.Json.Serialization;

namespace ManagerUserTaskApi.Infrastructure.Services.Authentication.Responses;

public class IdPError
{
    [JsonPropertyName("error")]
    public string Error { get; set; } = string.Empty;

    [JsonPropertyName("error_description")]
    public string Description { get; set; } = string.Empty;
}