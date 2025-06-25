/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

namespace Modules.Post;

[GraphQLDescription("Represents a blog post in the system.")]
public class Post
{
    [GraphQLDescription("The unique identifier of the post.")]
    [ID]
    public string Id { get; set; } = string.Empty;
    
    [GraphQLDescription("The title of the post.")]
    public string Title { get; set; } = string.Empty;

    [GraphQLDescription("The content of the post.")]
    public string Content { get; set; } = string.Empty;

    [GraphQLDescription("The date and time when the post was created.")]
    public DateTimeOffset CreatedAt { get; set; }

    [GraphQLDescription("The date and time when the post was last updated.")]
    public DateTimeOffset UpdatedAt { get; set; }

    [GraphQLDescription("The unique identifier of the author of the post.")]
    public required string AuthorId { get; set; }

    [GraphQLDescription("The author of the post.")]
    public required User.User Author { get; set; }
}