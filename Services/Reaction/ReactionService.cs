namespace SocialNetwork.Services.Reaction;

using SocialNetwork.Entities;
using Microsoft.EntityFrameworkCore;
public class ReactionService : IReactionService
{
    private readonly AppDbContext _context;

    public ReactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddOrUpdateReaction(int userId, int postId, string type)
    {
        var existingReaction = await _context.Reactions
            .FirstOrDefaultAsync(r => r.UserId == userId && r.PostId == postId);

        if (existingReaction != null)
        {
            existingReaction.Type = type;
        }
        else
        {
            var reaction = new Reaction
            {
                UserId = userId,
                PostId = postId,
                Type = type
            };
            await _context.Reactions.AddAsync(reaction);
        }

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task RemoveReaction(int userId, int postId)
    {
        var reaction = await _context.Reactions
            .FirstOrDefaultAsync(r => r.UserId == userId && r.PostId == postId);

        if (reaction != null)
        {
            _context.Reactions.Remove(reaction);
            await _context.SaveChangesAsync();
        }
    }
}