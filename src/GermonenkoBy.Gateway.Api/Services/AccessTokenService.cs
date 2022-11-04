using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using GermonenkoBy.Gateway.Api.Models.Auth;
using GermonenkoBy.Gateway.Api.Options;

namespace GermonenkoBy.Gateway.Api.Services;

public class AccessTokenService
{
    private readonly IOptionsSnapshot<SecurityOptions> _securityOptions;

    private string JwtSecret => _securityOptions.Value.JwtSecret;

    public AccessTokenService(IOptionsSnapshot<SecurityOptions> securityOptions)
    {
        _securityOptions = securityOptions;
    }

    public AccessToken GenerateAccessToken(int userId)
    {
        var issueDate = DateTime.UtcNow;
        var expireDate = issueDate.AddSeconds(_securityOptions.Value.AccessTokenTtlSeconds);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityOptions.Value.JwtSecret));
        var userIdClaim = new Claim("uid", userId.ToString());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new (new []{userIdClaim}),
            Expires = expireDate,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenStringRepresentation = tokenHandler.CreateToken(tokenDescriptor);
        return new AccessToken
        {
            UserId = userId,
            ExpireDate = expireDate,
            Token = tokenHandler.WriteToken(tokenStringRepresentation),
        };
    }

    public TokenValidationParameters GetTokenValidationParameters() => new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret)),
    };
}