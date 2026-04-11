
namespace SocialNetwork.DTOs.Posts;
using SocialNetwork.Response.comment;
public class PostResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<GetCommentAndUser> Comments { get; set; } = new List<GetCommentAndUser>();
    public int ReactionCount { get; set; }
}