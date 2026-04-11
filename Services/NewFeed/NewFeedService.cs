namespace SocialNetwork.Services.NewFeed;
using SocialNetwork.DTOs.Posts;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Response.comment;
using SocialMedia.Response.Reaction;

public class NewFeedService : INewFeedService
{
    public readonly AppDbContext _context ;
    public NewFeedService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<PostResponse>> GetNewFeedAsync(int userId, int pageNumber, int pageSize)
{
    var followingIds = await _context.Follows
        .Where(f => f.FollowerId == userId)
        .Select(f => f.FollowingId)
        .ToListAsync();

    var posts = await _context.Posts
        .Where(p => followingIds.Contains(p.UserId))
        .Include(p => p.User)
        .Include(p => p.Reactions)
        .Include(p => p.Comments)
            .ThenInclude(c => c.User)
        .OrderByDescending(p => p.CreatedAt)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var result = posts.Select(p => new PostResponse
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
        UserReaction = p.Reactions.FirstOrDefault(r => r.UserId == userId)?.Type,
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

    return result;
}
};