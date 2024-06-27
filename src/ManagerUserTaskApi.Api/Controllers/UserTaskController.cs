namespace ManagerUserTaskApi.Api.Controllers;

using Domain.Models;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sdk.Api;
using Sdk.Mediator.Abstractions.Interfaces;
using Sdk.Mediator.Abstractions.Responses;

// to enable api versioning, use the annotation [ApiVersion("version")]
// [ApiVersion("1.1")]
// [ApiVersion("1.0")]

// to not use the versioning notation (v1) in the route, uncomment below, overwriting the versioned route
// [Route("api/[Controller]")]
//[Authorize]
public class UserTaskController : CrudControllerBase<UserTaskModel, ListTasks, GetUserTask, CreateUserTask, UpdateUserTask, DeleteUserTask>
{
    public UserTaskController(IMediatorHandler mediator) : base(mediator)
    {
    }


    [HttpGet("list-user-task-grouped")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Paged<UserTaskGroupModel>>> ListUserTaskGroupAsync([FromQuery] ListUserTaskGroup request)
    {
        return ApiResponse(await _mediator.SendPagedQuery(request));
    }

}