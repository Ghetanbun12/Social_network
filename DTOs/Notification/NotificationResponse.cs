namespace SocialNetwork.DTOs.Notification;

public class NotificationResponse
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // "like", "comment", "follow"
    public int SourceUserId { get; set; }
    public string SourceUsername { get; set; } = string.Empty;
    public string SourceAvatarUrl { get; set; } = string.Empty;
    public int? PostId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
