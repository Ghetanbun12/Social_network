namespace SocialNetwork.Services.Search;
using SocialNetwork.DTOs.User;
using Microsoft.EntityFrameworkCore;
public class SearchUserService : ISearchUser
{
    private readonly AppDbContext _dbContext;

    public SearchUserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<SearchUser>> SearchUsers(string query, int userId)
    {
       var users = await _dbContext.Users
            .Where(u => u.Username.Contains(query))
            .Where(u => u.Id != userId) 
            .Select(u => new SearchUser
            {
                UserId = u.Id,
                Username = u.Username

            })
            .ToListAsync();

        return users;
    }
}