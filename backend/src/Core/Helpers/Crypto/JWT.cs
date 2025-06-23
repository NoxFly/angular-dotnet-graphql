/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace Core.Models;

public class JWTHelper
{
    public static readonly string _secretKey = RandomHelper.GenerateRandomString(32); // 256 bits
    public static readonly int _tokenExpirationMinutes = 60;

    public static string GenerateToken(string userId, string[] roles)
    {
        var claims = new List<Claim>
        {
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, userId),
            new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.Add(new Claim(ClaimTypes.System, userId));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return jwt;
    }

    public static ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}