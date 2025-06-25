/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Modules.Post;

namespace Core.Models;


public partial class QueryType
{
    public IQueryable<Post> Posts([Service] PostService postService) => postService.GetPosts();
    public Post? Post([Service] PostService postService, string id) => postService.GetPost(id);
}
