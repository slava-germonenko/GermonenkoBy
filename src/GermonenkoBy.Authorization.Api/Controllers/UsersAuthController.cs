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
    [SwaggerOperation("Authorizes user by login and password.")]
    [SwaggerResponse(200, "Refresh Token.", typeof(ContentResponse<RefreshToken>))]
    [SwaggerResponse(400, "Invalid login/password error", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<RefreshToken>>> AuthorizeAsync(
        [FromBody, SwaggerParameter("Authorization Request")] AuthorizeDto authorizeDto
    )
    {
        var refreshToken = await _authorizationService.AuthorizeAsync(authorizeDto);
        return OkWrapped(refreshToken);
    }

    [HttpPost("refresh")]
    [SwaggerOperation("Refresh the given refresh token (and session of needed).")]
    [SwaggerResponse(200, "Refresh Token.", typeof(ContentResponse<RefreshToken>))]
    [SwaggerResponse(400, "Invalid token error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<RefreshToken>>> RefreshRefreshTokenAsync(
        [FromBody, SwaggerParameter("Refresh Refresh Token DTO.")] RefreshRefreshTokenDto refreshRefreshTokenDto
    )
    {
        var refreshToken = await _authorizationService.RefreshRefreshTokenAsync(refreshRefreshTokenDto);
        return OkWrapped(refreshToken);
    }

    [HttpPost("terminate")]
    [SwaggerOperation("Terminate session and removes the token.")]
    [SwaggerResponse(204, "Success No Content Response.")]
    public async Task<NoContentResult> TerminateSessionAsync(
        [FromBody, SwaggerParameter("Terminate Session Data.")] TerminateSessionDto terminateSessionDto
    )
    {
        await _authorizationService.TerminateSessionAsync(terminateSessionDto.Token);
        return NoContent();
    }
}