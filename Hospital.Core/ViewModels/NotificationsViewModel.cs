using Hospital.Core.Models;

namespace Hospital.Core.ViewModels
{
    public class NotificationsViewModel
    {
        public IEnumerable<Notification> Notifications { get; set; } = new List<Notification>();
        public int UnreadCount { get; set; }
        public int TotalCount { get; set; }
        public bool HasUnread => UnreadCount > 0;
        public bool HasNotifications => Notifications.Any();
    }
}
