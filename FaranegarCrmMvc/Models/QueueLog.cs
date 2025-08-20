using System;

namespace FaranegarCrmMvc.Models
{
    public class QueueLog
    {
        public long Id { get; set; }

        public string? UniqueId { get; set; }   // ممکن است برخی ایونت‌ها نداشته باشند
        public string? LinkedId { get; set; }   // برای گروهبندی تمام لگ‌ها زیر یک تماس منطقی
        public string Event { get; set; } = ""; // QueueCallerJoin / AgentConnect / AgentComplete / ...

        public string? Queue { get; set; }
        public int? Position { get; set; }
        public int? Count { get; set; }

        public string? CallerIdNum { get; set; }
        public string? MemberName { get; set; }
        public string? Interface { get; set; }
        public string? AgentChannel { get; set; }

        public int? HoldTime { get; set; }
        public int? RingTime { get; set; }
        public int? TalkTime { get; set; }

        public string? Reason { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.Now; // Local

        public string? RawJson { get; set; }
    }
}
