using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace FaranegarCrmMvc.Models;

public class SalesReport
{
    public int Id { get; set; }

    [MaxLength(128)] public string? Subject { get; set; }      // شماره فاکتور در Subject
    [MaxLength(128)] public string? InvoiceRef { get; set; }   // ش ک فاکتور

    public int? AccountId { get; set; }
    public Account? Account { get; set; }

    public DateTime? IssueDate { get; set; }
    public string? Airline { get; set; }
    public string? TicketNumber { get; set; }
    public string? PNR { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? TotalAmount { get; set; }

    [MaxLength(64)]
    public string? RowFingerprint { get; set; } // اثرانگشت یکتا برای جلوگیری از رکورد تکراری

    public string? AdditionalJson { get; set; }
    public DateTime ImportedAt { get; set; } = DateTime.UtcNow;

    // --- 52 ستون دقیق اکسل ---
    [DisplayName("ترتیب")][Column("ترتیب", TypeName = "nvarchar(max)")] public string? Ex01 { get; set; }
    [DisplayName("-")][Column("-", TypeName = "nvarchar(max)")] public string? Ex02 { get; set; }
    [DisplayName("نوع")][Column("نوع", TypeName = "nvarchar(max)")] public string? Ex03 { get; set; }
    [DisplayName("تاریخ")][Column("تاریخ", TypeName = "nvarchar(max)")] public string? Ex04 { get; set; }
    [DisplayName("ایرلاین")][Column("ایرلاین", TypeName = "nvarchar(max)")] public string? Ex05 { get; set; }
    [DisplayName("شماره بلیط")][Column("شماره بلیط", TypeName = "nvarchar(max)")] public string? Ex06 { get; set; }
    [DisplayName("تاریخ دریافت")][Column("تاریخ دریافت", TypeName = "nvarchar(max)")] public string? Ex07 { get; set; }
    [DisplayName("ش ک فاکتور")][Column("ش ک فاکتور", TypeName = "nvarchar(max)")] public string? Ex08 { get; set; }
    [DisplayName("بدهکار")][Column("بدهکار", TypeName = "nvarchar(max)")] public string? Ex09 { get; set; }
    [DisplayName("بستانکار")][Column("بستانکار", TypeName = "nvarchar(max)")] public string? Ex10 { get; set; }
    [DisplayName("شماره قرارداد")][Column("شماره قرارداد", TypeName = "nvarchar(max)")] public string? Ex11 { get; set; }
    [DisplayName("طرف حساب")][Column("طرف حساب", TypeName = "nvarchar(max)")] public string? Ex12 { get; set; }
    [DisplayName("تاریخ سند")][Column("تاریخ سند", TypeName = "nvarchar(max)")] public string? Ex13 { get; set; }
    [DisplayName("شماره سند")][Column("شماره سند", TypeName = "nvarchar(max)")] public string? Ex14 { get; set; }
    [DisplayName("صادرکننده")][Column("صادرکننده", TypeName = "nvarchar(max)")] public string? Ex15 { get; set; }
    [DisplayName("خالص Fare")][Column("خالص Fare", TypeName = "nvarchar(max)")] public string? Ex16 { get; set; }
    [DisplayName("کمیسیون تشویقی")][Column("کمیسیون تشویقی", TypeName = "nvarchar(max)")] public string? Ex17 { get; set; }
    [DisplayName("TOTAL")][Column("TOTAL", TypeName = "nvarchar(max)")] public string? Ex18 { get; set; }
    [DisplayName("PNR")][Column("PNR", TypeName = "nvarchar(max)")] public string? Ex19 { get; set; }
    [DisplayName("تخفیف")][Column("تخفیف", TypeName = "nvarchar(max)")] public string? Ex20 { get; set; }
    [DisplayName("FARE")][Column("FARE", TypeName = "nvarchar(max)")] public string? Ex21 { get; set; }
    [DisplayName("I6")][Column("I6", TypeName = "nvarchar(max)")] public string? Ex22 { get; set; }
    [DisplayName("V0")][Column("V0", TypeName = "nvarchar(max)")] public string? Ex23 { get; set; }
    [DisplayName("KU")][Column("KU", TypeName = "nvarchar(max)")] public string? Ex24 { get; set; }
    [DisplayName("LP")][Column("LP", TypeName = "nvarchar(max)")] public string? Ex25 { get; set; }
    [DisplayName("TAX")][Column("TAX", TypeName = "nvarchar(max)")] public string? Ex26 { get; set; }
    [DisplayName("مسیر")][Column("مسیر", TypeName = "nvarchar(max)")] public string? Ex27 { get; set; }
    [DisplayName("نام فارسی مسافر")][Column("نام فارسی مسافر", TypeName = "nvarchar(max)")] public string? Ex28 { get; set; }
    [DisplayName("نام انگلیسی مسافر")][Column("نام انگلیسی مسافر", TypeName = "nvarchar(max)")] public string? Ex29 { get; set; }
    [DisplayName("سن")][Column("سن", TypeName = "nvarchar(max)")] public string? Ex30 { get; set; }
    [DisplayName("کلاس")][Column("کلاس", TypeName = "nvarchar(max)")] public string? Ex31 { get; set; }
    [DisplayName("تاریخ رفت")][Column("تاریخ رفت", TypeName = "nvarchar(max)")] public string? Ex32 { get; set; }
    [DisplayName("تاریخ برگشت")][Column("تاریخ برگشت", TypeName = "nvarchar(max)")] public string? Ex33 { get; set; }
    [DisplayName("تایید حسابداری")][Column("تایید حسابداری", TypeName = "nvarchar(max)")] public string? Ex34 { get; set; }
    [DisplayName("خط بعد")][Column("خط بعد", TypeName = "nvarchar(max)")] public string? Ex35 { get; set; }
    [DisplayName("شماره بلیط قبلی")][Column("شماره بلیط قبلی", TypeName = "nvarchar(max)")] public string? Ex36 { get; set; }
    [DisplayName("نرخ قبلی")][Column("نرخ قبلی", TypeName = "nvarchar(max)")] public string? Ex37 { get; set; }
    [DisplayName("Tax قبلی")][Column("Tax قبلی", TypeName = "nvarchar(max)")] public string? Ex38 { get; set; }
    [DisplayName("جریمه")][Column("جریمه", TypeName = "nvarchar(max)")] public string? Ex39 { get; set; }
    [DisplayName("شماره واچر")][Column("شماره واچر", TypeName = "nvarchar(max)")] public string? Ex40 { get; set; }
    [DisplayName("درخواست کننده")][Column("درخواست کننده", TypeName = "nvarchar(max)")] public string? Ex41 { get; set; }
    [DisplayName("کمیسیون")][Column("کمیسیون", TypeName = "nvarchar(max)")] public string? Ex42 { get; set; }
    [DisplayName("درصد کمیسیون")][Column("درصد کمیسیون", TypeName = "nvarchar(max)")] public string? Ex43 { get; set; }
    [DisplayName("وضعیت کوپن")][Column("وضعیت کوپن", TypeName = "nvarchar(max)")] public string? Ex44 { get; set; }
    [DisplayName("نوع پرداخت")][Column("نوع پرداخت", TypeName = "nvarchar(max)")] public string? Ex45 { get; set; }
    [DisplayName("تصویر بلیت")][Column("تصویر بلیت", TypeName = "nvarchar(max)")] public string? Ex46 { get; set; }
    [DisplayName("Sign")][Column("Sign", TypeName = "nvarchar(max)")] public string? Ex47 { get; set; }
    [DisplayName("گزارش فروش")][Column("گزارش فروش", TypeName = "nvarchar(max)")] public string? Ex48 { get; set; }
    [DisplayName("کامپیوتر ثبت کننده")][Column("کامپیوتر ثبت کننده", TypeName = "nvarchar(max)")] public string? Ex49 { get; set; }
    [DisplayName("کامپیوتر تغییر دهنده")][Column("کامپیوتر تغییر دهنده", TypeName = "nvarchar(max)")] public string? Ex50 { get; set; }
    [DisplayName("توضیحات")][Column("توضیحات", TypeName = "nvarchar(max)")] public string? Ex51 { get; set; }
    [DisplayName("توضیحات فاکتور")][Column("توضیحات فاکتور", TypeName = "nvarchar(max)")] public string? Ex52 { get; set; }

    // سازگاری با ویو/جزییات قدیمی (اختیاری)
    [NotMapped] public string? FareAmount => JsonGet("FARE") ?? JsonGet("Fare");
    [NotMapped] public string? TaxAmount => JsonGet("TAX") ?? JsonGet("Tax");

    private string? JsonGet(string key)
    {
        if (string.IsNullOrWhiteSpace(AdditionalJson)) return null;
        using var doc = JsonDocument.Parse(AdditionalJson);
        if (doc.RootElement.TryGetProperty(key, out var v))
            return v.ValueKind == JsonValueKind.String ? v.GetString() : v.ToString();

        string Norm(string s) => s.Replace("ي", "ی").Replace("ك", "ک").Replace("\u200c", "").Replace("‌", "").Trim();
        var target = Norm(key);
        foreach (var p in doc.RootElement.EnumerateObject())
            if (Norm(p.Name) == target)
                return p.Value.ValueKind == JsonValueKind.String ? p.Value.GetString() : p.Value.ToString();
        return null;
    }
}
