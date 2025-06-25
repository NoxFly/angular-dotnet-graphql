/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

namespace Modules.User;

#region Models

[GraphQLDescription("Represents a user in the system.")]
public class User
{
    [GraphQLDescription("The unique identifier of the user.")]
    [ID]
    public required string Id { get; set; }

    [GraphQLDescription("The full name of the user.")]
    public required string FullName { get; set; }

    [GraphQLDescription("Indicates whether the user has administrative privileges.")]
    [GraphQLDeprecated("Use 'IsAdmin2' instead.")]
    public required bool IsAdmin { get; set; }
}

#endregion
