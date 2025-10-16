﻿namespace Hospital.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public string Message { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
