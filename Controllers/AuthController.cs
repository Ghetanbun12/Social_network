using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services;


namespace SocialNetwork.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _authService.Register(request);
        return Ok("Register success");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _authService.Login(request);
        return Ok(new { token });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var response = await _authService.RefreshToken(request.RefreshToken!);
        return Ok(response);
    }

     [HttpPost("logout")]
    public async Task<IActionResult> Logout(string refreshToken)
    {
        await _authService.Logout(refreshToken);
        return Ok("Logout success");
    }
}