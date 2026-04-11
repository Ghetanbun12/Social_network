using SocialNetwork.Entities;
using SocialNetwork.DTOs.Notification;

namespace SocialNetwork.Services.Notification;

public interface INotificationService
{
    Task<List<NotificationResponse>> GetNotificationsAsync(int userId);
    Task<bool> MarkAsReadAsync(int userId, int notificationId);
    Task CreateNotificationAsync(int userId, string type, int sourceUserId, int? postId = null);
}
