'use strict';

/**
 * Notification Hub Handler
 * Manages real-time notifications from SignalR
 */
const NotificationHub = (function () {

    // === Private helper functions ===
    function addNotificationToList(message) {
        const list = document.getElementById("notifList");
        const noNotifItem = document.getElementById("noNotif");

        if (!list) return;

        if (noNotifItem) noNotifItem.remove();

        const li = document.createElement("li");
        li.className = "notification-item";

        const shortMessage = message.length > 50 ? message.substring(0, 50) + '...' : message;

        li.innerHTML = `
            <div class="dropdown-item">
                <div class="d-flex justify-content-between align-items-start">
                    <div class="flex-grow-1 me-2">
                        <div class="small text-truncate">${shortMessage}</div>
                    </div>
                    <small class="text-muted flex-shrink-0">${new Date().toLocaleTimeString()}</small>
                </div>
            </div>
        `;

        const divider = list.querySelector('.dropdown-divider');
        if (divider) {
            list.insertBefore(li, divider);
        } else {
            list.prepend(li);
        }
    }

    function updateUnreadCount(count, isAbsolute = false) {
        const countElement = document.getElementById("notifCount");
        if (!countElement) return;

        let newCount = isAbsolute
            ? count
            : (parseInt(countElement.textContent) || 0) + count;

        countElement.textContent = newCount;
        countElement.style.display = newCount > 0 ? 'inline-block' : 'none';
    }

    async function updateUnreadCountFromServer() {
        try {
            const response = await fetch('/Admin/Notifications/GetUnreadCount');
            if (response.ok) {
                const data = await response.json();
                updateUnreadCount(data.count, true);
            }
        } catch (error) {
            console.error('❌ Error fetching unread count:', error);
        }
    }

    function showNotificationToast(message) {
        if (typeof Swal === 'undefined') return;

        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 5000,
            timerProgressBar: true,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer);
                toast.addEventListener('mouseleave', Swal.resumeTimer);
            }
        });

        Toast.fire({
            icon: 'info',
            title: message
        });
    }

    // === Initialize the hub ===
    async function init() {
        const connection = await SignalRConnection.start("/notificationHub", updateUnreadCountFromServer);

        if (!connection) {
            console.error("⚠ Failed to establish SignalR connection for notifications.");
            return;
        }

        // Events from the hub
        connection.on("ReceiveNotification", function (message) {
            console.log('📨 Received notification:', message);
            addNotificationToList(message);
            updateUnreadCount(1);
            showNotificationToast(message);
        });

        connection.on("UnreadCountUpdated", function (unreadCount) {
            console.log('🔢 Unread count updated:', unreadCount);
            updateUnreadCount(unreadCount, true);
        });

        // Get initial count on load
        await updateUnreadCountFromServer();
    }

    // === Auto initialize ===
    document.addEventListener('DOMContentLoaded', init);

    return {
        init
    };
})();