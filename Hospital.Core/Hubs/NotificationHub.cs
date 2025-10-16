using Hospital.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Hospital.Core.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(INotificationService notificationService, ILogger<NotificationHub> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    // ربط المستخدم بالـ group
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");

                    _logger.LogInformation("User {UserId} connected to NotificationHub with connection {ConnectionId}",
                        userId, Context.ConnectionId);

                    // إرسال عدد الإشعارات غير المقروءة عند الاتصال
                    var countResponse = _notificationService.GetUnreadCount(userId);
                    if (countResponse.Succeeded)
                    {
                        await Clients.Caller.SendAsync("UnreadCountUpdated", countResponse.Result);
                        _logger.LogInformation("Sent initial unread count {Count} to user {UserId}",
                            countResponse.Result, userId);
                    }
                }
                else
                {
                    _logger.LogWarning("User connected without valid NameIdentifier claim");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnConnectedAsync for connection {ConnectionId}", Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");

                    _logger.LogInformation("User {UserId} disconnected from NotificationHub with connection {ConnectionId}",
                        userId, Context.ConnectionId);
                }

                if (exception != null)
                {
                    _logger.LogError(exception, "Disconnection exception for connection {ConnectionId}",
                        Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnDisconnectedAsync for connection {ConnectionId}", Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // دالة لتمييز الإشعار كمقروء من خلال الـ Hub
        public async Task MarkNotificationAsRead(int notificationId)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Unauthorized attempt to mark notification as read");
                    return;
                }

                _logger.LogInformation("User {UserId} marking notification {NotificationId} as read",
                    userId, notificationId);

                var response = await _notificationService.MarkAsReadAsync(notificationId);

                if (response.Succeeded)
                {
                    // تحديث العدد للمستخدم
                    var countResponse = _notificationService.GetUnreadCount(userId);
                    if (countResponse.Succeeded)
                    {
                        await Clients.Caller.SendAsync("UnreadCountUpdated", countResponse.Result);
                        _logger.LogInformation("Updated unread count to {Count} for user {UserId}",
                            countResponse.Result, userId);
                    }

                    await Clients.Caller.SendAsync("NotificationMarkedAsRead", notificationId);
                }
                else
                {
                    _logger.LogWarning("Failed to mark notification {NotificationId} as read for user {UserId}: {Message}",
                        notificationId, userId, response.Message);

                    await Clients.Caller.SendAsync("OperationFailed", response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification {NotificationId} as read", notificationId);
                await Clients.Caller.SendAsync("OperationFailed", "An error occurred while marking notification as read");
            }
        }

        // دالة لتمييز جميع الإشعارات كمقروءة
        public async Task MarkAllNotificationsAsRead()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Unauthorized attempt to mark all notifications as read");
                    return;
                }

                _logger.LogInformation("User {UserId} marking all notifications as read", userId);

                var response = await _notificationService.MarkAllAsReadAsync(userId);

                if (response.Succeeded)
                {
                    // تحديث العدد للمستخدم
                    await Clients.Caller.SendAsync("UnreadCountUpdated", 0);
                    await Clients.Caller.SendAsync("AllNotificationsMarkedAsRead");

                    _logger.LogInformation("All notifications marked as read for user {UserId}, count: {Count}",
                        userId, response.Result);
                }
                else
                {
                    _logger.LogWarning("Failed to mark all notifications as read for user {UserId}: {Message}",
                        userId, response.Message);

                    await Clients.Caller.SendAsync("OperationFailed", response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications as read");
                await Clients.Caller.SendAsync("OperationFailed", "An error occurred while marking all notifications as read");
            }
        }

        // دالة لحذف إشعار
        public async Task DeleteNotification(int notificationId)
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Unauthorized attempt to delete notification");
                    return;
                }

                _logger.LogInformation("User {UserId} deleting notification {NotificationId}", userId, notificationId);

                var response = await _notificationService.DeleteAsync(notificationId);

                if (response.Succeeded)
                {
                    // تحديث العدد للمستخدم
                    var countResponse = _notificationService.GetUnreadCount(userId);
                    if (countResponse.Succeeded)
                    {
                        await Clients.Caller.SendAsync("UnreadCountUpdated", countResponse.Result);
                    }

                    await Clients.Caller.SendAsync("NotificationDeleted", notificationId);
                    _logger.LogInformation("Notification {NotificationId} deleted for user {UserId}",
                        notificationId, userId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete notification {NotificationId} for user {UserId}: {Message}",
                        notificationId, userId, response.Message);

                    await Clients.Caller.SendAsync("OperationFailed", response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification {NotificationId}", notificationId);
                await Clients.Caller.SendAsync("OperationFailed", "An error occurred while deleting notification");
            }
        }

        // دالة للبينغ (للتأكد من أن الاتصال نشط)
        public string Ping()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogDebug("Ping received from user {UserId}", userId);
            return "Pong";
        }

        // دالة للحصول على الإشعارات غير المقروءة
        public async Task GetUnreadNotifications()
        {
            try
            {
                var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return;
                }

                var response = _notificationService.GetUserNotifications(userId);

                if (response.Succeeded)
                {
                    var unreadNotifications = response.Result?.Where(n => !n.IsRead).ToList();
                    await Clients.Caller.SendAsync("UnreadNotifications", unreadNotifications);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread notifications for user");
            }
        }
    }
}
