using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Sessions.Core;
using GermonenkoBy.Sessions.Core.Dtos;
using GermonenkoBy.Sessions.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GermonenkoBy.Sessions.Api.Controllers;

[ApiController, Route("api/user-sessions")]
public class UserSessionsController : ControllerBaseWrapper
{
    private readonly UserSessionsService _userSessionsService;

    public UserSessionsController(UserSessionsService userSessionsService)
    {
        _userSessionsService = userSessionsService;
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
}