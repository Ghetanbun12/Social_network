namespace SocialNetwork.Services.Comment;
using SocialNetwork.Entities;
using SocialNetwork.DTOs.Comment;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Response.comment;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;

    public CommentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateComment(int userId, CreateCommentRequest request, int postId)
    {
        try
        {
            var post = await _context.Posts.FindAsync(postId);
            var user = await _context.Users.FindAsync(userId);
            if (post == null || user == null) return false;

            var comment = new Comment
            {
                Content = request.Content,
                UserId = userId,
                PostId = postId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<GetCommentAndUser>> GetCommentsAndUsersByPost(int postId)
    {
        var comments = await _context.Comments
            .Where(c => c.PostId == postId)
            .Include(c => c.User)
            .Select(c => new GetCommentAndUser
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                User = new UserResponse
                {
                    Username = c.User.Username,
                    Bio = c.User.Bio,
                    AvatarUrl = c.User.AvatarUrl
                }
            })
            .ToListAsync();

        return comments;
    }

    public async Task DeleteComment(int commentId, int userId)
    {
        var comment = await _context.Comments.FindAsync(commentId);
        if (comment != null && comment.UserId == userId)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<bool> ReplyToComment(int commentId, int userId, CreateCommentRequest request, int postId)
    {
        try
        {
            var parentComment = await _context.Comments.FindAsync(commentId);
            var user = await _context.Users.FindAsync(userId);
            if (parentComment == null || user == null) return false;

            var replyComment = new Comment
            {
                Content = request.Content,
                UserId = userId,
                PostId = postId,
                ParentId = commentId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(replyComment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}