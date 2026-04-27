namespace SocialNetwork.Services.SuggestFriend;

using Microsoft.EntityFrameworkCore;
using SocialNetwork.Entities;

public class SuggestFriendService : ISuggestFriendService
{
    private readonly AppDbContext _context; 

    public SuggestFriendService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> SuggestFriendAsync(int userId)
    {
        var followingIds = await _context.Follows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.FollowingId)
            .ToListAsync();
        var SuggestFriend = await _context.Follows
            .Where(u => followingIds.Contains(u.FollowingId))
            .Select(u => u.FollowerId)
            .Distinct()
            .ToListAsync();
        return await _context.Users
            .Where(u => SuggestFriend.Contains(u.Id) && u.Id != userId)
            .Take(10)
            .ToListAsync();
    }
}