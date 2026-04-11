namespace SocialNetwork.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SocialNetwork.DTOs.User;
using SocialNetwork.Services.Search;

[Authorize]
[ApiController]
[Route("api/users/search")]
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
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var users = await _searchUserService.SearchUsers(query, userId);
        return Ok(users);
    }
}