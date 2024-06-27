namespace ManagerUserTaskApi.Domain.Requests;

using Sdk.Mediator.Abstractions.Requests;
using System.Text.Json.Serialization;

public class UpdateUserTask : OneWayRequestBase
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("user")]
    public string User { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("version")]
    public int Version { get; set; }
}