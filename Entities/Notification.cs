namespace SocialNetwork.Entities;
public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; } // receiver
    public string Type { get; set; } = string.Empty;
    public int SourceUserId { get; set; }
    public int? PostId { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; }
}