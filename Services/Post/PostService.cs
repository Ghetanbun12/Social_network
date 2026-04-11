namespace SocialNetwork.Services.Post;
using SocialNetwork.Entities;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DTOs.Posts;
using SocialNetwork.Response.comment;
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
            Content = dto.Content ?? string.Empty,
            ImageUrl = dto.ImageUrl ?? string.Empty,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            Privacy = "Public"
        };
        
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<PostResponse>> GetPostsByUser(int userId, int currentUserId)
    {
        var posts = await _context.Posts
            .Where(p => p.UserId == userId && !p.IsDeleted)
            .Include(p => p.User)
            .Include(p => p.Reactions)
            .Include(p => p.Comments)
                .ThenInclude(c => c.User)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return posts.Select(p => new PostResponse
        {
            Id = p.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            User = new UserResponse
            {
                Id = p.User.Id,
                Username = p.User.Username,
                AvatarUrl = p.User.AvatarUrl
            },
            ReactionCount = p.Reactions.Count(),
            UserReaction = p.Reactions.FirstOrDefault(r => r.UserId == currentUserId)?.Type,
            Comments = p.Comments.Select(c => new GetCommentAndUser
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                User = new UserResponse
                {
                    Id = c.User.Id,
                    Username = c.User.Username,
                    AvatarUrl = c.User.AvatarUrl
                }
            }).ToList()
        }).ToList();
    }
    public async Task DeletePost(int postId, int userId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null || post.UserId != userId) return;
        post.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
    
}