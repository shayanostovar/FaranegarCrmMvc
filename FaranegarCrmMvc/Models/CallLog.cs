using System;

namespace FaranegarCrmMvc.Models
{
    public class CallLog
    {
        public int Id { get; set; }
        public string UniqueId { get; set; } = "";    // یکتای تماس در استریسک

        public string? Direction { get; set; }        // Inbound / Outbound / Unknown

        public string? CallerIdNum { get; set; }
        public string? CallerIdName { get; set; }

        public string? Src { get; set; }              // مبدا
        public string? Dst { get; set; }              // مقصد
        public string? SrcChannel { get; set; }       // مثل SIP/1001-...
        public string? DstChannel { get; set; }

        public DateTime StartAt { get; set; }         // شروع تماس (Local)
        public DateTime? AnsweredAt { get; set; }     // زمان پاسخ‌گویی
        public DateTime? EndAt { get; set; }          // پایان

        public string? Disposition { get; set; }      // Answered/No Answer/Busy/Failed/Abandoned
        public int? HangupCause { get; set; }
        public string? HangupCauseText { get; set; }

        public int? DurationSec { get; set; }         // کل مدت
        public int? BillSec { get; set; }             // مدت مکالمه پس از پاسخ

        public string? RecordingFile { get; set; }    // فقط نام/مسیر؛ فایل ذخیره نمی‌کنیم

        // --- اطلاعات صف (Queue) ---
        public string? QueueName { get; set; }        // مثال: support-queue
        public DateTime? QueueJoinAt { get; set; }    // زمان ورود به صف
        public DateTime? QueueLeaveAt { get; set; }   // زمان خروج از صف (یا پایان)
        public int? QueueHoldSec { get; set; }        // مدت انتظار تا پاسخ (HoldTime)

        // Agent/Extension پاسخ‌گو
        public string? AgentExt { get; set; }         // مثال: 1010
        public string? AgentChannel { get; set; }     // مثال: SIP/1010-00001234
        public DateTime? AgentConnectAt { get; set; } // زمان اتصال Agent (AgentConnect)

        // مقادیر تکمیلی از Queue رویدادها (اختیاری)
        public int? RingSec { get; set; }             // مدت Ring قبل از پاسخ
        public int? TalkSec { get; set; }             // مدت مکالمه گزارش شده در AgentComplete

        public string? RawJson { get; set; }          // آخرین ایونت خام برای دیباگ
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Local
    }
}
