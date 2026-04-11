using SocialNetwork.Services.Post;
namespace SocialNetwork.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/posts")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var result = await _postService.CreatePost(userId, request);
        if (!result)
            return BadRequest("Failed to create post");

        return Ok("Post created successfully");
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPostsByUser(int userId)
    {
        var currentUserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var posts = await _postService.GetPostsByUser(userId, currentUserId);
        return Ok(posts);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        await _postService.DeletePost(postId, userId);
        return Ok("Post deleted successfully");
    }
}