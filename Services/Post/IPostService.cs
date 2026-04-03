namespace SocialNetwork.Services.Post;
using SocialNetwork.Entities;   

public interface IPostService

{
    Task<bool> CreatePost(int userId, CreatePostRequest dto);
    Task<List<Post>> GetPostsByUser(int userId);
    Task DeletePost(int postId, int userId);
}