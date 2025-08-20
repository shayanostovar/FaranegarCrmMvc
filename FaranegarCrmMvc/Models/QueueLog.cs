using System;

namespace FaranegarCrmMvc.Models
{
    public class QueueLog
    {
        public long Id { get; set; }

        public string? UniqueId { get; set; }  // ممکن است در برخی ایونت‌ها نیاید
        public string Event { get; set; } = ""; // QueueCallerJoin / QueueCallerLeave / QueueCallerAbandon / AgentCalled / AgentConnect / AgentComplete / QueueMemberStatus / ...

        public string? Queue { get; set; }       // نام صف
        public int? Position { get; set; }       // جایگاه تماس‌گیرنده در صف
        public int? Count { get; set; }          // تعداد اعضای صف/مخاطبین (بسته به ایونت)

        public string? CallerIdNum { get; set; } // شماره تماس‌گیرنده
        public string? MemberName { get; set; }  // نام Agent در صف (اگر مقدور)
        public string? Interface { get; set; }   // مثل SIP/1010
        public string? AgentChannel { get; set; } // مثل SIP/1010-...

        public int? HoldTime { get; set; }       // ثانیه
        public int? RingTime { get; set; }
        public int? TalkTime { get; set; }

        public string? Reason { get; set; }      // علت ترک/Abandon
        public DateTime OccurredAt { get; set; } = DateTime.Now; // Local

        public string? RawJson { get; set; }     // کل پیام خام برای دیباگ
    }
}
