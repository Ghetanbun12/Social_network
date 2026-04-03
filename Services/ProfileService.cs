using System.Threading.Tasks;
using SocialNetwork.Entities;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Services;
public class ProfileService
{
    private readonly AppDbContext _context;

    public ProfileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetProfile(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }
    public async Task<bool> UpdateUser(int id, UpdateUserRequest request)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null) return false;
    user.Username = request.Username;
    user.Bio = request.Bio;
    user.AvatarUrl = request.AvatarUrl;

    user.CoverUrl = request.CoverUrl;

    await _context.SaveChangesAsync();
    return true;
}
}