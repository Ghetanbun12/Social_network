namespace Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services.Reaction;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/reactions")]
public class ReactionController : ControllerBase
{
    private readonly IReactionService _reactionService;
    public ReactionController(IReactionService reactionService)
    {
        _reactionService = reactionService;
    }
    [HttpPost]
    public async Task<IActionResult> AddOrUpdateReaction(int postId, string type)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var result = await _reactionService.AddOrUpdateReaction(userId, postId, type);
        if (!result)
            return BadRequest("Failed to add or update reaction");

        return Ok("Reaction added or updated successfully");
    }
    [HttpDelete]
    public async Task<IActionResult> RemoveReaction(int postId)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        await _reactionService.RemoveReaction(userId, postId);
        return Ok("Reaction removed successfully");
    }
}