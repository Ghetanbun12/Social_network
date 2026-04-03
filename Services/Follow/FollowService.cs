namespace SocialNetwork.Services.Follow;
using SocialNetwork.Entities;

public class FollowService : IFollowService
{
    public readonly AppDbContext _context;

    public FollowService(AppDbContext context)
    {
        _context = context;
    }
    public async Task FollowUserAsync(int followerId, int followingId)
    {
        if (followerId == followingId) throw new ArgumentException("Cannot follow yourself"); 
        var existingFollow = await _context.Follows.FindAsync(followerId, followingId);
        if (existingFollow != null) throw new InvalidOperationException("Already following this user");
        var follow = new Follow
        {
            FollowerId = followerId,
            FollowingId = followingId,
        };
        _context.Follows.Add(follow);
        await _context.SaveChangesAsync();
        
    }
    public async Task UnfollowUserAsync(int followerId, int followingId)
    {
        if (followerId == followingId) throw new ArgumentException("Cannot unfollow yourself");
        var existingFollow = await _context.Follows.FindAsync(followerId, followingId);
        if (existingFollow == null) throw new InvalidOperationException("Not following this user");
        _context.Follows.Remove(existingFollow);
        await _context.SaveChangesAsync();
    }
}