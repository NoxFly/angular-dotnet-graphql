

using Core.Services;
using Modules.User;

/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */
namespace Modules.Post;

public class PostService(RealmDatabaseService db, UserService userService)
{
    private readonly RealmDatabaseService _db = db;
    private readonly UserService _userService = userService;

    public Post? GetPost(string id)
    {
        // Logic to retrieve a post by its ID
        // This is a placeholder implementation
        var post = _db.Find<PostSchema>(id);

        if (post == null)
            return null;

        var user = _userService.FindOneById(post.AuthorId);

        return new Post
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            AuthorId = post.AuthorId,
            Author = user,
        };
    }

    public IQueryable<Post> GetPosts()
    {
        // Logic to retrieve all posts
        // This is a placeholder implementation
        var posts = _db.GetAll<PostSchema>();

        return posts.ToList().Select(post => new Post
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            AuthorId = post.AuthorId,
            Author = _userService.FindOneById(post.AuthorId),
        }).AsQueryable();
    }
}