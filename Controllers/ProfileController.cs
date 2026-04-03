using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services;

namespace SocialNetwork.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileService;

    public ProfileController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(int id)
    {
        var user = await _profileService.GetProfile(id);
        if (user == null)
            return NotFound("User not found");

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var result = await _profileService.UpdateUser(id, request);
        if (!result)
            return NotFound("User not found or update failed");

        return Ok("User updated successfully");
    }
}
