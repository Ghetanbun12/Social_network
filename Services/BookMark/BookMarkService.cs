namespace SocialNetwork.Services.BookMark;

using SocialNetwork.Entities;
using Microsoft.EntityFrameworkCore;

public class BookMarkService : IBookMarkService
{
    private readonly AppDbContext _context;

    public BookMarkService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Bookmark> CreateBookMarkAsync(int userId, int postId)
    {
        var bookmark = new Bookmark
        {
            UserId = userId,
            PostId = postId
        };

        await _context.Bookmarks.AddAsync(bookmark);
        await _context.SaveChangesAsync();

        return bookmark;
    }

    public async Task<Bookmark?> GetBookMarksAsync(int userId, int postId)
    {
        return await _context.Bookmarks
            .FirstOrDefaultAsync(b => b.UserId == userId && b.PostId == postId);
    }

    public async Task<bool> DeleteBookMarkAsync(int userId, int postId)
    {
        var bookmark = await _context.Bookmarks
            .FirstOrDefaultAsync(b => b.UserId == userId && b.PostId == postId);

        if (bookmark != null)
        {
            _context.Bookmarks.Remove(bookmark);
            await _context.SaveChangesAsync();
        }

        return bookmark != null;
    }
}