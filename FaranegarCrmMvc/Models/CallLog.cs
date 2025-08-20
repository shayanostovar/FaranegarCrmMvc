using System;

namespace FaranegarCrmMvc.Models
{
    public class CallLog
    {
        public int Id { get; set; }

        // شناسه‌های تماس
        public string UniqueId { get; set; } = "";    // یکتای کانال در استریسک (الزامی و یکتا)
        public string? LinkedId { get; set; }         // یک شناسه مشترک برای تمام لگ‌های یک تماس

        // اطلاعات عمومی
        public string? Direction { get; set; }        // Inbound / Outbound / Unknown
        public string? CallerIdNum { get; set; }
        public string? CallerIdName { get; set; }
        public string? Src { get; set; }              // مبدا (from)
        public string? Dst { get; set; }              // مقصد (to)
        public string? SrcChannel { get; set; }       // مثل SIP/1001-...
        public string? DstChannel { get; set; }

        // زمان‌ها (Local)
        public DateTime StartAt { get; set; }
        public DateTime? AnsweredAt { get; set; }
        public DateTime? EndAt { get; set; }

        // وضعیت و کد قطع
        public string? Disposition { get; set; }      // ANSWERED / NO ANSWER / BUSY / FAILED / ABANDONED
        public int? HangupCause { get; set; }
        public string? HangupCauseText { get; set; }

        // مدت‌ها (ثانیه)
        public int? DurationSec { get; set; }
        public int? BillSec { get; set; }

        // ضبط (نام/مسیر؛ خود فایل ذخیره نمی‌شود)
        public string? RecordingFile { get; set; }

        // --- اطلاعات صف (Queue) ---
        public string? QueueName { get; set; }        // نام صف
        public DateTime? QueueJoinAt { get; set; }    // زمان ورود به صف
        public DateTime? QueueLeaveAt { get; set; }   // زمان خروج از صف
        public int? QueueHoldSec { get; set; }        // مدت انتظار تا پاسخ

        // Agent/Extension پاسخ‌گو
        public string? AgentExt { get; set; }         // مثال: 1010
        public string? AgentChannel { get; set; }     // مثال: SIP/1010-00001234
        public DateTime? AgentConnectAt { get; set; } // زمان اتصال Agent

        // مقادیر تکمیلی از رویدادهای صف
        public int? RingSec { get; set; }
        public int? TalkSec { get; set; }

        // دیباگ
        public string? RawJson { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Local
    }
}
