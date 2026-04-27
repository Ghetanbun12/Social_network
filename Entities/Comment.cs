namespace SocialNetwork.Entities;
public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }

    
    public int? ParentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
    public Post Post { get; set; }
}