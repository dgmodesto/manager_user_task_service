namespace Sdk.Api;

using Mediator.Abstractions.Enums;
using Mediator.Abstractions.Interfaces;
using Mediator.Abstractions.Requests;
using Mediator.Abstractions.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[Route("v{version:apiVersion}/[Controller]")]
public class CrudControllerBase<TModel, TList, TGet, TCreate, TUpdate, TDelete> : ApiControllerBase<TModel>
    where TModel : class
    where TList : PagedRequestBase<TModel>
    where TGet : TwoWayRequestBase<TModel>
    where TCreate : TwoWayRequestBase<TModel>
    where TUpdate : OneWayRequestBase
    where TDelete : OneWayRequestBase
{
    protected CrudControllerBase(IMediatorHandler mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Paged<TModel>>> List([FromQuery] TList request)
    {
        return ApiResponse(await _mediator.SendPagedQuery(request));
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TModel>> Get([FromRoute] TGet request)
    {
        return ApiResponse(await _mediator.SendQuery(request));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Post([FromBody] TCreate request)
    {
        return ApiResponse(await _mediator.SendCommand(request), HttpStatusCode.Created);
    }

    [HttpPut("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(Guid Id, TUpdate request)
    {
        var updateCommandType = typeof(TUpdate);
        var idProp = updateCommandType.GetProperties()
            .FirstOrDefault(p => "Id".Equals(p.Name, StringComparison.InvariantCultureIgnoreCase));

        if (idProp is null)
        {
            await NotifyError(FailureReason.BadRequest, "Request BODY doesn't has an Id property");
            return ApiResponse();
        }

        var id = Guid.Parse(idProp.GetValue(request)!.ToString()!);
        if (id != Id)
        {
            await NotifyError(FailureReason.BadRequest, "Id on PATH differs from Id on BODY");
            return ApiResponse();
        }

        await _mediator.SendCommand(request);
        return ApiResponse(HttpStatusCode.NoContent);
    }

    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] TDelete request)
    {
        await _mediator.SendCommand(request);
        return ApiResponse(HttpStatusCode.NoContent);
    }
}