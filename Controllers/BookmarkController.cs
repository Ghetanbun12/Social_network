namespace SocialNetwork.Controllers;

using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services.BookMark;
using Microsoft.AspNetCore.Authorization;
using SocialNetwork.DTOs.Posts;

[Authorize]
[ApiController]
[Route("api/bookmarks")]
public class BookmarkController : ControllerBase
{
    private readonly IBookMarkService _bookMarkService;

    public BookmarkController(IBookMarkService bookMarkService)
    {
        _bookMarkService = bookMarkService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBookmarks()
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var result = await _bookMarkService.GetBookmarksAsync(userId);
        return Ok(result);
    }

    [HttpPost("{postId}")]
    public async Task<IActionResult> AddBookmark(int postId)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        await _bookMarkService.CreateBookMarkAsync(userId, postId);
        return Ok("Bookmarked successfully");
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> RemoveBookmark(int postId)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var result = await _bookMarkService.DeleteBookMarkAsync(userId, postId);
        if (!result) return NotFound("Bookmark not found");
        return Ok("Bookmark removed successfully");
    }
}
