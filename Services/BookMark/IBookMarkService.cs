namespace SocialNetwork.Services.BookMark;

using SocialNetwork.Entities;
using SocialNetwork.DTOs.Posts;

public interface IBookMarkService
{
    Task<Bookmark> CreateBookMarkAsync(int userId, int postId);
    Task<List<PostResponse>> GetBookmarksAsync(int userId);
    Task<bool> DeleteBookMarkAsync(int userId, int postId);
}