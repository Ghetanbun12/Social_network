
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services.Follow;
namespace SocialNetwork.Controllers;



[ApiController]
[Route("api/follows")]
public class FollowController : ControllerBase
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    [HttpPost("follow")]
    public async Task<IActionResult> FollowUser(int followerId, int followingId)
    {
        await _followService.FollowUserAsync(followerId, followingId);
        return Ok("Followed successfully");
    }

    [HttpPost("unfollow")]
    public async Task<IActionResult> UnfollowUser(int followerId, int followingId)
    {
        await _followService.UnfollowUserAsync(followerId, followingId);
        return Ok("Unfollowed successfully");
    }
}   