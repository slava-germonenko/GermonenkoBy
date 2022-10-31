using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Authorization.Api.Dtos;
using GermonenkoBy.Authorization.Core.Dtos;
using GermonenkoBy.Authorization.Core.Models;
using GermonenkoBy.Authorization.Core.Services;
using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Authorization.Api.Controllers;

[ApiController, Route("api/users-auth")]
public class AuthController : ControllerBaseWrapper
{
    private readonly DefaultUserAuthorizationService _authorizationService;

    public AuthController(DefaultUserAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("")]
    [SwaggerOperation(
        "Authorize by login and password",
        "Performs user authorization based on provided login and password. Created refresh token and/or session."
    )]
    [SwaggerResponse(200, "Refresh Token.", typeof(ContentResponse<RefreshToken>))]
    [SwaggerResponse(400, "Invalid login/password error", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<RefreshToken>>> AuthorizeAsync(
        [FromBody, SwaggerRequestBody("Authorization Request")] AuthorizeDto authorizeDto
    )
    {
        var refreshToken = await _authorizationService.AuthorizeAsync(authorizeDto);
        return OkWrapped(refreshToken);
    }

    [HttpPost("refresh")]
    [SwaggerOperation("Refresh refresh token", "Refreshes given refresh token and session (if needed).")]
    [SwaggerResponse(200, "Refresh Token.", typeof(ContentResponse<RefreshToken>))]
    [SwaggerResponse(400, "Invalid token error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<RefreshToken>>> RefreshRefreshTokenAsync(
        [FromBody, SwaggerRequestBody("Refresh Refresh Token DTO.")] RefreshRefreshTokenDto refreshRefreshTokenDto
    )
    {
        var refreshToken = await _authorizationService.RefreshRefreshTokenAsync(refreshRefreshTokenDto);
        return OkWrapped(refreshToken);
    }

    [HttpPost("terminate")]
    [SwaggerOperation("Terminates session.", "Removes given token and terminates corresponding session.")]
    [SwaggerResponse(204, "Success No Content Response.")]
    public async Task<NoContentResult> TerminateSessionAsync(
        [FromBody, SwaggerRequestBody("Terminate Session Data.")] TerminateSessionDto terminateSessionDto
    )
    {
        await _authorizationService.TerminateSessionAsync(terminateSessionDto.Token);
        return NoContent();
    }
}