/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Attributes;
using Core.Helpers.Crypto;
using Modules.Auth.Dto;
using Modules.User;
using Core.Models;
using Core.Helpers;

namespace Modules.Auth;


[Injectable(ServiceLifetime.Transient)]
public class AuthService(UserService userService)
{
    private readonly UserService _userService = userService;

    public async Task<AuthResponse> Login(CredentialsBody credentials, HttpContext httpContext)
    {
        credentials.Password = RSAHelper.Decrypt(credentials.Password);
        credentials.Password = SHAHelper.Hash(credentials.Password);
        credentials.Password = credentials.Password.ToUpper();

        var user = await _userService.FindOneById(credentials.Id);

        if (user == null)
            throw new BadRequestException("Invalid credentials");

        // dev
        user.Password = credentials.Password;

        if(user.Password != credentials.Password)
            throw new BadRequestException("Invalid credentials");

        string[] roles = ["User"];

        if(user.IsAdmin)
            roles = ["User", "Admin"];

        var jwtToken = JWTHelper.GenerateToken(user.Id, roles);

        var expiresAt = DateTimeOffset.UtcNow
            .AddMinutes(JWTHelper._tokenExpirationMinutes)
            .ToUnixTimeSeconds()
            .ToString();

        var response = new AuthResponse
        {
            AccessToken = jwtToken,
            ExpiresAt = expiresAt,
            User = user
        };

        return response;
    }
}
