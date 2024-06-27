namespace ManagerUserTaskApi.Domain.Requests;

using Sdk.Mediator.Abstractions.Requests;
using System.Text.Json.Serialization;

public class DeleteUserTask : OneWayRequestBase
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}