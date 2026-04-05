namespace SocialNetwork.Response.comment;

public class GetCommentAndUser
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
   
    public UserResponse User { get; set; }
} 