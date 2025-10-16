using Hospital.Core.Hubs;
using Hospital.Core.Models;
using Hospital.Core.Services;
using Hospital.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(INotificationService notificationService,
            IHubContext<NotificationHub> hubContext, ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult MyNotifications()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Challenge();

                var response = _notificationService.GetUserNotifications(userId);

                if (!response.Succeeded)
                {
                    TempData["Error"] = response.Message;
                    return View(new NotificationsViewModel());
                }

                var vm = new NotificationsViewModel
                {
                    Notifications = response.Result ?? new List<Notification>(),
                    UnreadCount = response.Result?.Count(n => !n.IsRead) ?? 0,
                    TotalCount = response.Result?.Count() ?? 0
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading notifications for user {UserId}",
                    User.FindFirstValue(ClaimTypes.NameIdentifier));
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead([FromBody] int id)
        {
            try
            {
                Console.WriteLine($"🔔 MarkAsRead called for: {id}");

                var response = await _notificationService.MarkAsReadAsync(id);
                if (!response.Succeeded)
                {
                    Console.WriteLine($"❌ MarkAsRead failed: {response.Message}");
                    return Json(new { success = false, message = response.Message });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var countResponse = _notificationService.GetUnreadCount(userId);

                var unreadCount = countResponse.Succeeded ? countResponse.Result : 0;

                await _hubContext.Clients.User(userId)
                    .SendAsync("UnreadCountUpdated", unreadCount);

                Console.WriteLine($"✅ MarkAsRead successful, count: {unreadCount}");

                return Json(new
                {
                    success = true,
                    unreadCount,
                    message = response.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 MarkAsRead error: {ex.Message}");
                _logger.LogError(ex, "Error marking notification {NotificationId} as read", id);
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            try
            {
                Console.WriteLine($"🔔 MarkAllAsRead called");

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var response = await _notificationService.MarkAllAsReadAsync(userId);

                if (response.Succeeded)
                {
                    var countResponse = _notificationService.GetUnreadCount(userId);
                    var unreadCount = countResponse.Succeeded ? countResponse.Result : 0;

                    await _hubContext.Clients.User(userId)
                        .SendAsync("UnreadCountUpdated", unreadCount);

                    Console.WriteLine($"✅ MarkAllAsRead successful, count: {unreadCount}");

                    return Json(new
                    {
                        success = true,
                        unreadCount,
                        message = response.Message,
                        count = response.Result
                    });
                }

                Console.WriteLine($"❌ MarkAllAsRead failed: {response.Message}");
                return Json(new { success = false, message = response.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 MarkAllAsRead error: {ex.Message}");
                _logger.LogError(ex, "Error marking all notifications as read for user {UserId}",
                    User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                Console.WriteLine($"🔔 Delete called for: {id}");

                var response = await _notificationService.DeleteAsync(id);
                if (!response.Succeeded)
                {
                    Console.WriteLine($"❌ Delete failed: {response.Message}");
                    return Json(new { success = false, message = response.Message });
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var countResponse = _notificationService.GetUnreadCount(userId);
                var unreadCount = countResponse.Succeeded ? countResponse.Result : 0;

                await _hubContext.Clients.User(userId)
                    .SendAsync("UnreadCountUpdated", unreadCount);

                Console.WriteLine($"✅ Delete successful, count: {unreadCount}");

                return Json(new
                {
                    success = true,
                    unreadCount,
                    message = response.Message
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Delete error: {ex.Message}");
                _logger.LogError(ex, "Error deleting notification {NotificationId}", id);
                return Json(new { success = false, message = "An error occurred" });
            }
        }

        [HttpGet]
        public IActionResult GetUnreadCount()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = _notificationService.GetUnreadCount(userId!);

                if (response.Succeeded)
                {
                    return Json(new { count = response.Result });
                }

                return Json(new { count = 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count for user {UserId}",
                    User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Json(new { count = 0 });
            }
        }
    }
}
