using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Sessions.Core;
using GermonenkoBy.Sessions.Core.Dtos;
using GermonenkoBy.Sessions.Core.Models;
using GermonenkoBy.Sessions.Core.Services;

namespace GermonenkoBy.Sessions.Api.Controllers;

[ApiController, Route("api/user-sessions")]
public class UserSessionsController : ControllerBaseWrapper
{
    private readonly UserSessionsService _userSessionsService;

    public UserSessionsController(UserSessionsService userSessionsService)
    {
        _userSessionsService = userSessionsService;
    }

    [HttpGet("")]
    [SwaggerResponse(200, "List of User Sessions.", typeof(ContentListResponse<UserSession>))]
    public async Task<ActionResult<ContentListResponse<UserSession>>> GetUserSessionsAsync(
        [FromQuery, SwaggerParameter("User Sessions Filter.")] FilterUserSessionsDto userSessionsFilter,
        [FromServices] UserSessionsSearchService searchService
    )
    {
        var sessions = await searchService.GetUserSessionsAsync(userSessionsFilter);
        return OkWrapped(sessions);
    }

    [HttpPut("")]
    [SwaggerResponse(200, "Started/Updated user session.", typeof(ContentResponse<UserSession>))]
    [SwaggerResponse(400, "Error response.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<UserSession>>> StartOrRefreshSession(
        [FromBody, SwaggerParameter("User Session Data.")] StartUserSessionDto sessionDto
    )
    {
        var session = await _userSessionsService.StartOrRefreshSessionAsync(sessionDto);
        return OkWrapped(session);
    }

    [HttpDelete("{sessionId:guid}")]
    [SwaggerResponse(204, "Success No Content Response.")]
    public async Task<NoContentResult> TerminateSessionAsync(
        [SwaggerParameter("Session ID to terminate.")] Guid sessionId
    )
    {
        await _userSessionsService.TerminateSessionAsync(sessionId);
        return NoContent();
    }
}