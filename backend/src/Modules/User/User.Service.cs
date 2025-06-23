using Attributes;
using Modules.User.Dto;

namespace Modules.User;


[Injectable(ServiceLifetime.Transient)]
public class UserService
{
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
