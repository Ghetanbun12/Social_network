namespace Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using SocialNetwork.DTOs.User;
using SocialNetwork.Services.Search;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SearchUserController : ControllerBase
{
    private readonly ISearchUser _searchUserService;

    public SearchUserController(ISearchUser searchUserService)
    {
        _searchUserService = searchUserService;
    }

    [HttpGet]
    public async Task<IActionResult> SearchUsers([FromQuery] string query)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
        var users = await _searchUserService.SearchUsers(query, userId);
        return Ok(users);
    }
}