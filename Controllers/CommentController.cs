namespace SocialNetwork.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services.Comment;
using SocialNetwork.Response.comment;   
using SocialNetwork.DTOs.Comment;

[Authorize]
[ApiController]
[Route("api/posts/{postId}/comments")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<IEnumerable<GetCommentAndUser>> GetCommentsAndUsersByPost(int postId)
    {
        return await _commentService.GetCommentsAndUsersByPost(postId);
    }
    [HttpPost]
    public async Task<IActionResult> CreateComment(int postId, [FromBody] CreateCommentRequest request)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var result = await _commentService.CreateComment(userId, request, postId);
        if (!result)
            return BadRequest("Failed to create comment");

        return Ok("Comment created successfully");
    }
    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        await _commentService.DeleteComment(commentId, userId);
        return Ok("Comment deleted successfully");
    }
    [HttpPost("{commentId}/reply")]
    public async Task<IActionResult> ReplyToComment(int commentId, int postId, [FromBody] CreateCommentRequest request)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var result = await _commentService.ReplyToComment(commentId, userId, request, postId);
        if (!result)
            return BadRequest("Failed to reply to comment");

        return Ok("Replied to comment successfully");
    }

}
