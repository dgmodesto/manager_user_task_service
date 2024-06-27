using ManagerUserTaskApi.Domain.Requests;
using ManagerUserTaskApi.Domain.Responses;
using ManagerUserTaskApi.Infrastructure.Services.Authentication.Responses;
using Microsoft.AspNetCore.Mvc;
using Sdk.Api;
using Sdk.Mediator.Abstractions.Interfaces;

namespace ManagerUserTaskApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ApiControllerBase<UserLogged>
{
    private readonly IMediatorHandler _mediator;
    public AuthController(
        IMediatorHandler mediator) : base(mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Generate Token JWT to acess the endpoints restricted
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    /// GET /api/auth/login
    /// </remarks>
    /// <returns>
    /// The Authentication information
    /// </returns>
    /// <response code="200">Retorna as informações da autenticação.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserLogged), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginUser request)
    {
        /*
         username: dgmodesto
         password: Teste@123
         */
        var response = await _mediator.SendQuery<UserLogged>(request);

        return ApiResponse(response);
    }
}
