namespace SocialNetwork.Services.Post;
using SocialNetwork.Entities;   
using SocialNetwork.DTOs.Posts;

public interface IPostService

{
    Task<bool> CreatePost(int userId, CreatePostRequest dto);
    Task<List<PostResponse>> GetPostsByUser(int userId, int currentUserId);
    Task DeletePost(int postId, int userId);
}