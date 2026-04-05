namespace SocialNetwork.Services.Comment;

using System.Transactions;
using SocialNetwork.DTOs.Comment;
using SocialNetwork.Response.comment;

public interface ICommentService
{
    public Task<bool> CreateComment(int userId, CreateCommentRequest request, int postId);
    public Task<IEnumerable<GetCommentAndUser>> GetCommentsAndUsersByPost(int postId);
    public Task DeleteComment(int commentId, int userId);

    public Task<bool> ReplyToComment( int commentId, int userId, CreateCommentRequest request,int postId);
}   
    