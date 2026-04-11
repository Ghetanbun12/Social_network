
namespace SocialNetwork.DTOs.Posts;

using SocialMedia.Response.Reaction;
using SocialNetwork.Response.comment;
public class PostResponse
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;

   

    public UserResponse User { get; set; } = new UserResponse();
    public int ReactionCount { get; set; }
    public string? UserReaction { get; set; }

    public DateTime CreatedAt { get; set; }
    public List<GetCommentAndUser> Comments { get; set; } = new List<GetCommentAndUser>();
  
}