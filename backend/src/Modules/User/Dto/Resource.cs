namespace Modules.User.Dto;

#region Models

public class UserDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required bool IsAdmin { get; set; }
}

#endregion
