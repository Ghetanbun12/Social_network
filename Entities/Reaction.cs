namespace SocialNetwork.Entities;
public class Reaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Type { get; set; } = "Like";

    // Navigation
    public User User { get; set; }
    public Post Post { get; set; }
}