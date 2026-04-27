using Microsoft.EntityFrameworkCore;
using SocialNetwork.Entities;

namespace SocialNetwork.Services.SuggestFriend;
public interface ISuggestFriendService
{
    Task<List<User>> SuggestFriendAsync(int userId);
}