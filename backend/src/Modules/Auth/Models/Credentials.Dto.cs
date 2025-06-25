/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

// using Modules.User.Models;

namespace Modules.Auth;

public class CredentialsBody
{
    public required string Id { get; set; }
    public required string Password { get; set; }
}

public class AuthResponse
{
    public required string AccessToken { get; set; }
    public required string ExpiresAt { get; set; }
    // public required User.Models.User User { get; set; }
}