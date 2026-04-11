namespace SocialNetwork.Services.BookMark;

using SocialNetwork.Entities;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DTOs.Posts;
using SocialNetwork.Response.comment;

public class BookMarkService : IBookMarkService
{
    private readonly AppDbContext _context;

    public BookMarkService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Bookmark> CreateBookMarkAsync(int userId, int postId)
    {
        var existing = await _context.Bookmarks
            .FirstOrDefaultAsync(b => b.UserId == userId && b.PostId == postId);
        
        if (existing != null) return existing;

        var bookmark = new Bookmark
        {
            UserId = userId,
            PostId = postId
        };

        await _context.Bookmarks.AddAsync(bookmark);
        await _context.SaveChangesAsync();

        return bookmark;
    }

    public async Task<List<PostResponse>> GetBookmarksAsync(int userId)
    {
        var bookmarkedPosts = await _context.Bookmarks
            .Where(b => b.UserId == userId)
            .Include(b => b.Post!)
                .ThenInclude(p => p.User)
            .Include(b => b.Post!)
                .ThenInclude(p => p.Reactions)
            .Include(b => b.Post!)
                .ThenInclude(p => p.Comments)
                    .ThenInclude(c => c.User)
            .Select(b => b.Post)
            .Where(p => p != null && !p.IsDeleted)
            .OrderByDescending(p => p!.CreatedAt)
            .ToListAsync();

        return bookmarkedPosts.Select(p => new PostResponse
        {
            Id = p!.Id,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            User = new UserResponse
            {
                Id = p.User!.Id,
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
    }

    public async Task<bool> DeleteBookMarkAsync(int userId, int postId)
    {
        var bookmark = await _context.Bookmarks
            .FirstOrDefaultAsync(b => b.UserId == userId && b.PostId == postId);

        if (bookmark != null)
        {
            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}