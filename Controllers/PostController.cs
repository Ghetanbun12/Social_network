using SocialNetwork.Services.Post;
namespace SocialNetwork.Controllers;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> CreatePost(int userId, [FromBody] CreatePostRequest request)
    {
        var result = await _postService.CreatePost(userId, request);
        if (!result)
            return BadRequest("Failed to create post");

        return Ok("Post created successfully");
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPostsByUser(int userId)
    {
        var posts = await _postService.GetPostsByUser(userId);
        return Ok(posts);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(int postId, int userId)
    {
        await _postService.DeletePost(postId, userId);
        return Ok("Post deleted successfully");
    }
}