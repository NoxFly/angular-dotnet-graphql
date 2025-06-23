/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

namespace Core.Models;

public class ResponseError
{
    public required ResponseErrorDetails Error { get; set; }
}

public class ResponseErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public object? Details { get; set; }
}
