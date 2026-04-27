namespace SocialNetwork.Services.BookMark;

using SocialNetwork.Entities;
using SocialNetwork.DTOs.Posts;

public interface IBookMarkService
{
    Task<Bookmark> CreateBookMarkAsync(int userId, int postId);
    Task<Bookmark?> GetBookMarksAsync(int userId, int postId);
    Task<bool> DeleteBookMarkAsync(int userId, int postId);
}