/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Attributes;
using Core.Services;

namespace Modules.User;


[Injectable(ServiceLifetime.Transient)]
public class UserService(RealmDatabaseService db)
{
    private readonly RealmDatabaseService _db = db;

    public UserSchema? FindOneByIdSchema(string id)
    {
        return _db.Find<UserSchema>(id);
    }

    public User? FindOneById(string id)
    {
        var user = _db.Find<UserSchema>(id);

        if (user == null)
            return null;

        return new User
        {
            Id = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            IsAdmin = user.IsAdmin
        };
    }
    
    public IQueryable<User> GetUsers()
    {
        return _db.GetAll<UserSchema>().ToList().Select(user => new User
        {
            Id = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            IsAdmin = user.IsAdmin
        }).AsQueryable();
    }
}
