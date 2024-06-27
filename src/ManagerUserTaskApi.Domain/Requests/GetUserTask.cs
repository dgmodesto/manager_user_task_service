namespace ManagerUserTaskApi.Domain.Requests;

using Models;
using Sdk.Mediator.Abstractions.Requests;
using System.Text.Json.Serialization;

public class GetUserTask : TwoWayRequestBase<UserTaskModel>
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}