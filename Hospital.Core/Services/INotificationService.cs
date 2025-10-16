using Hospital.Core.Helpers;
using Hospital.Core.Models;

namespace Hospital.Core.Services
{
    public interface INotificationService
    {
        Task<GenericResponse<Notification>> CreateAsync(Notification notification);
        GenericResponse<IEnumerable<Notification>> GetUserNotifications(string userId);
        Task<GenericResponse<bool>> MarkAsReadAsync(int notificationId);
        Task<GenericResponse<int>> MarkAllAsReadAsync(string userId);
        GenericResponse<int> GetUnreadCount(string userId);
        Task<GenericResponse<bool>> DeleteAsync(int notificationId);
        Task<GenericResponse<int>> DeleteOldNotificationsAsync(DateTime olderThan);
        Task<GenericResponse<Notification>> GetByIdAsync(int id);
    }
}
