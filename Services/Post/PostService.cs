namespace SocialNetwork.Services.Post;
using SocialNetwork.Entities;
using Microsoft.EntityFrameworkCore;
public class PostService : IPostService
{
    private readonly AppDbContext _context;

    public PostService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CreatePost(int userId, CreatePostRequest dto)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;
        var post = new Post
        {
            Content = dto.Content,
            ImageUrl = dto.ImageUrl,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<Post>> GetPostsByUser(int userId)
    {
        var posts = await _context.Posts
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return posts;
    }
    public async Task DeletePost(int postId, int userId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null || post.UserId != userId) return;
        post.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
    
}