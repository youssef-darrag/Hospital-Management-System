using Hospital.Core.Consts;
using Hospital.Core.Helpers;
using Hospital.Core.Models;
using Hospital.Core.Repositories;
using Hospital.Core.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Hospital.EF.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUnitOfWork unitOfWork, IMemoryCache cache, ILogger<NotificationService> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;
        }

        public async Task<GenericResponse<Notification>> CreateAsync(Notification notification)
        {
            try
            {
                notification.CreatedAt = DateTime.UtcNow;
                notification.IsRead = false;

                await _unitOfWork.Notifications.AddAsync(notification);
                await _unitOfWork.CompleteAsync();

                // إزالة الكاش الخاص بالمستخدم
                RemoveUserCache(notification.UserId);

                _logger.LogInformation("Notification created for user {UserId}", notification.UserId);

                return new GenericResponse<Notification>
                {
                    Succeeded = true,
                    Message = "Notification created successfully",
                    Result = notification
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification for user {UserId}", notification.UserId);
                return new GenericResponse<Notification>
                {
                    Succeeded = false,
                    Message = "Failed to create notification"
                };
            }
        }

        public GenericResponse<IEnumerable<Notification>> GetUserNotifications(string userId)
        {
            try
            {
                var cacheKey = $"notifications_{userId}";

                if (!_cache.TryGetValue(cacheKey, out IEnumerable<Notification>? notifications))
                {
                    notifications = _unitOfWork.Notifications.GetAll(criteria: n => n.UserId == userId,
                        orderBy: n => n.CreatedAt, orderByDirection: OrderBy.Descending)
                        .Take(50).ToList();

                    _cache.Set(cacheKey, notifications, TimeSpan.FromMinutes(10));
                }

                return new GenericResponse<IEnumerable<Notification>>
                {
                    Succeeded = true,
                    Result = notifications
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notifications for user {UserId}", userId);
                return new GenericResponse<IEnumerable<Notification>>
                {
                    Succeeded = false,
                    Message = "Failed to get notifications"
                };
            }
        }

        public async Task<GenericResponse<bool>> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return new GenericResponse<bool>
                    {
                        Succeeded = false,
                        Message = "Notification not found"
                    };
                }

                if (!notification.IsRead)
                {
                    notification.IsRead = true;
                    notification.ReadAt = DateTime.UtcNow;

                    _unitOfWork.Notifications.Update(notification);
                    await _unitOfWork.CompleteAsync();

                    // إزالة الكاش
                    RemoveUserCache(notification.UserId);

                    _logger.LogInformation("Notification {NotificationId} marked as read", notificationId);
                }

                return new GenericResponse<bool>
                {
                    Succeeded = true,
                    Message = "Notification marked as read",
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notification {NotificationId} as read", notificationId);
                return new GenericResponse<bool>
                {
                    Succeeded = false,
                    Message = "Failed to mark notification as read"
                };
            }
        }

        public async Task<GenericResponse<int>> MarkAllAsReadAsync(string userId)
        {
            try
            {
                var unreadNotifications = _unitOfWork.Notifications
                    .GetAll(criteria: n => n.UserId == userId && !n.IsRead);

                if (!unreadNotifications.Any())
                {
                    return new GenericResponse<int>
                    {
                        Succeeded = true,
                        Message = "No unread notifications",
                        Result = 0
                    };
                }

                foreach (var notification in unreadNotifications)
                {
                    notification.IsRead = true;
                    notification.ReadAt = DateTime.UtcNow;
                }

                _unitOfWork.Notifications.UpdateRange(unreadNotifications);
                await _unitOfWork.CompleteAsync();

                // إزالة الكاش
                RemoveUserCache(userId);

                var count = unreadNotifications.Count();
                _logger.LogInformation("All notifications marked as read for user {UserId}", userId);

                return new GenericResponse<int>
                {
                    Succeeded = true,
                    Message = "All notifications marked as read",
                    Result = count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking all notifications as read for user {UserId}", userId);
                return new GenericResponse<int>
                {
                    Succeeded = false,
                    Message = "Failed to mark all notifications as read"
                };
            }
        }

        public GenericResponse<int> GetUnreadCount(string userId)
        {
            try
            {
                var cacheKey = $"unread_count_{userId}";

                if (!_cache.TryGetValue(cacheKey, out int count))
                {
                    count = _unitOfWork.Notifications.GetAll(criteria: n =>
                        n.UserId == userId && !n.IsRead).Count();

                    _cache.Set(cacheKey, count, TimeSpan.FromMinutes(5));
                }

                return new GenericResponse<int>
                {
                    Succeeded = true,
                    Result = count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unread count for user {UserId}", userId);
                return new GenericResponse<int>
                {
                    Succeeded = false,
                    Message = "Failed to get unread count",
                    Result = 0
                };
            }
        }

        public async Task<GenericResponse<Notification>> GetByIdAsync(int id)
        {
            try
            {
                var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
                if (notification == null)
                {
                    return new GenericResponse<Notification>
                    {
                        Succeeded = false,
                        Message = "Notification not found"
                    };
                }

                return new GenericResponse<Notification>
                {
                    Succeeded = true,
                    Result = notification
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notification {NotificationId}", id);
                return new GenericResponse<Notification>
                {
                    Succeeded = false,
                    Message = "Failed to get notification"
                };
            }
        }

        public async Task<GenericResponse<bool>> DeleteAsync(int notificationId)
        {
            try
            {
                var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return new GenericResponse<bool>
                    {
                        Succeeded = false,
                        Message = "Notification not found"
                    };
                }

                var userId = notification.UserId;
                _unitOfWork.Notifications.Delete(notification);
                await _unitOfWork.CompleteAsync();

                // إزالة الكاش
                RemoveUserCache(userId);

                _logger.LogInformation("Notification {NotificationId} deleted", notificationId);

                return new GenericResponse<bool>
                {
                    Succeeded = true,
                    Message = "Notification deleted successfully",
                    Result = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting notification {NotificationId}", notificationId);
                return new GenericResponse<bool>
                {
                    Succeeded = false,
                    Message = "Failed to delete notification"
                };
            }
        }

        public async Task<GenericResponse<int>> DeleteOldNotificationsAsync(DateTime olderThan)
        {
            try
            {
                var oldNotifications = _unitOfWork.Notifications.GetAll(criteria: n => n.CreatedAt < olderThan);

                if (!oldNotifications.Any())
                {
                    return new GenericResponse<int>
                    {
                        Succeeded = true,
                        Message = "No old notifications found",
                        Result = 0
                    };
                }

                var affectedUsers = oldNotifications.Select(n => n.UserId).Distinct();

                _unitOfWork.Notifications.DeleteRange(oldNotifications);
                await _unitOfWork.CompleteAsync();

                // إزالة كاش جميع المستخدمين المتأثرين
                foreach (var userId in affectedUsers)
                {
                    RemoveUserCache(userId);
                }

                var count = oldNotifications.Count();
                _logger.LogInformation("Deleted {Count} old notifications", count);

                return new GenericResponse<int>
                {
                    Succeeded = true,
                    Message = "Old notifications deleted successfully",
                    Result = count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting old notifications");
                return new GenericResponse<int>
                {
                    Succeeded = false,
                    Message = "Failed to delete old notifications"
                };
            }
        }

        private void RemoveUserCache(string userId)
        {
            _cache.Remove($"notifications_{userId}");
            _cache.Remove($"unread_count_{userId}");
        }
    }
}