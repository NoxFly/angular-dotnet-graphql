/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Attributes;
using Core.Services;
using Modules.User.Models;

namespace Modules.User;


[Injectable(ServiceLifetime.Transient)]
public class UserService(RealmDatabaseService db)
{
    private readonly RealmDatabaseService _db = db;

    public async Task<UserDto?> FindOneById(string id)
    {
        var user = new UserDto
        {
            Id = id,
            IsAdmin = false,
            Name = "John Doe",
            Password = "hashed-password",
        };

        await Task.Delay(100); // Simulate async operation

        return user;
    }
}
