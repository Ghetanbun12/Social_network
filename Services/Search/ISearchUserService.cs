namespace SocialNetwork.Services.Search;
using SocialNetwork.DTOs.User;

public interface ISearchUser
{
    Task<List<SearchUser>> SearchUsers(string query,int userId);
}