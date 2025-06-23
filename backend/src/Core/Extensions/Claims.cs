/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using System.Security.Claims;

namespace Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetId(this ClaimsPrincipal user)
    {
        return user.FindFirst(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub)?.Value ?? "";
    }

    public static string GetRoles(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Role)?.Value ?? "";
    }
}