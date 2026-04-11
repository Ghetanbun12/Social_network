namespace SocialNetwork.Services.NewFeed;
using SocialNetwork.DTOs.Posts;


public interface INewFeedService
{
    public Task<List<PostResponse>> GetNewFeedAsync(int userId, int pageNumber, int pageSize);
}