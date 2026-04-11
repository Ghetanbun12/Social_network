using SocialNetwork.Entities;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DTOs.Notification;

namespace SocialNetwork.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;

    public NotificationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<NotificationResponse>> GetNotificationsAsync(int userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync();

        var sourceUserIds = notifications.Select(n => n.SourceUserId).Distinct().ToList();
        var sourceUsers = await _context.Users
            .Where(u => sourceUserIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u);

        return notifications.Select(n => new NotificationResponse
        {
            Id = n.Id,
            Type = n.Type,
            SourceUserId = n.SourceUserId,
            SourceUsername = sourceUsers.ContainsKey(n.SourceUserId) ? sourceUsers[n.SourceUserId].Username : "Unknown",
            SourceAvatarUrl = sourceUsers.ContainsKey(n.SourceUserId) ? sourceUsers[n.SourceUserId].AvatarUrl : "",
            PostId = n.PostId,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        }).ToList();
    }

    public async Task<bool> MarkAsReadAsync(int userId, int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification == null || notification.UserId != userId) return false;

        notification.IsRead = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task CreateNotificationAsync(int userId, string type, int sourceUserId, int? postId = null)
    {
        if (userId == sourceUserId) return; // Don't notify yourself

        var notification = new Entities.Notification
        {
            UserId = userId,
            Type = type,
            SourceUserId = sourceUserId,
            PostId = postId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
}
