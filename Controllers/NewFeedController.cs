namespace Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using SocialNetwork.Services.NewFeed;
[Authorize]
[ApiController]
[Route("api/newfeed")]
public class NewFeedController : ControllerBase
{
    private readonly INewFeedService _newFeedService;

    public NewFeedController(INewFeedService newFeedService)
    {
        _newFeedService = newFeedService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNewFeed(int pageNumber = 1, int pageSize = 10)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var newFeed = await _newFeedService.GetNewFeedAsync(userId, pageNumber, pageSize);
        return Ok(newFeed);
    }
}