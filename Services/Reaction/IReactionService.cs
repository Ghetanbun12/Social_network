namespace SocialNetwork.Services.Reaction;
public interface IReactionService
{
    public Task<bool> AddOrUpdateReaction(int userId, int postId, string type);
    public Task RemoveReaction(int userId, int postId);

    public Task<int> CountReactions(int postId);
}