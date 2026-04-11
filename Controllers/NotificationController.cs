namespace SocialNetwork.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services.Notification;

[Authorize]
[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var result = await _notificationService.GetNotificationsAsync(userId);
        return Ok(result);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
        var result = await _notificationService.MarkAsReadAsync(userId, id);
        if (!result) return NotFound("Notification not found or unauthorized");
        return Ok("Notification marked as read");
    }
}
