namespace SocialNetwork.Services;

using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SocialNetwork.Entities;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Helpers;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    public async Task Register(RegisterRequest request)
    {
        var passwordHash = BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
            throw new Exception("User not found");

        bool isValid = BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isValid)
            throw new Exception("Wrong password");

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public async Task<AuthResponse> RefreshToken(string token)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token);

        if (refreshToken == null)
            throw new Exception("Invalid token");

        if (refreshToken.IsRevoked)
            throw new Exception("Token revoked");

        if (refreshToken.ExpiryDate < DateTime.UtcNow)
            throw new Exception("Token expired");

        var user = refreshToken.User;

        // 🔥 TẠO TOKEN MỚI
        var newJwt = JwtHelper.GenerateToken(user);
        var newRefreshToken = JwtHelper.GenerateRefreshToken();

        // 🔥 REVOKE TOKEN CŨ
        refreshToken.IsRevoked = true;

        var newRefresh = new RefreshToken
        {
            Token = newRefreshToken,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };

        _context.RefreshTokens.Add(newRefresh);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = newJwt,
            RefreshToken = newRefreshToken
        };
    }
}