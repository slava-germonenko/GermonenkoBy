using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
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
    [SwaggerOperation("User sessions search.", "Search for user sessions using filters provided via query params.")]
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
    [SwaggerOperation("Start/update user session.", "Starts a new user session. If user session is already started, it will be updated it.")]
    [SwaggerResponse(200, "Started/Updated user session.", typeof(ContentResponse<UserSession>))]
    [SwaggerResponse(400, "Error response.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<UserSession>>> StartOrRefreshSession(
        [FromBody, SwaggerRequestBody("User Session Data.")] StartUserSessionDto sessionDto
    )
    {
        var session = await _userSessionsService.StartOrRefreshSessionAsync(sessionDto);
        return OkWrapped(session);
    }

    [HttpGet("{sessionId:guid}")]
    [SwaggerOperation("Get user session.", "Tries to get user session by ID.")]
    [SwaggerResponse(200, "Found Session.", typeof(ContentResponse<UserSession>))]
    [SwaggerResponse(404, "Not Found Error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<UserSession>>> GetSessionAsync(
        [SwaggerParameter("ID of a session to search for.")] Guid sessionId
    )
    {
        var session = await _userSessionsService.GetSessionAsync(sessionId);
        return OkWrapped(session);
    }

    [HttpDelete("{sessionId:guid}")]
    [SwaggerOperation("Remove session.", "Removes session with the given ID.")]
    [SwaggerResponse(204, "Success No Content Response.")]
    public async Task<NoContentResult> TerminateSessionAsync(
        [SwaggerParameter("Session ID to terminate.")] Guid sessionId
    )
    {
        await _userSessionsService.TerminateSessionAsync(sessionId);
        return NoContent();
    }
}