namespace SocialNetwork.Entities;
public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public string Privacy { get; set; } = "Public";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public User User { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
}