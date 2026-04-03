namespace SocialNetwork.Services.Follow;

public interface IFollowService
{
    public Task FollowUserAsync(int followerId, int followingId);
    public Task UnfollowUserAsync(int followerId, int followingId);
}