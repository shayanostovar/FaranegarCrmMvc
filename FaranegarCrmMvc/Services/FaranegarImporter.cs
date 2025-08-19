using System.Data;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FaranegarCrmMvc.Data;
using FaranegarCrmMvc.Models;

namespace FaranegarCrmMvc.Services
{
    // گزارش خروجی ایمپورت
    public record ImportResult(int Inserted, int DuplicatesInFile, int DuplicatesInDb, int TotalRead);

    /// ایمپورت با حذف تکراری‌ها + نگاشت 52 ستون:
    /// - تشخیص فرمت (Excel 2003 XML / CSV / XLS/XLSX)
    /// - نرمال‌سازی هدر/مقدار‌ها (ی/ي، ک/ك، حذف ZWNJ/NBSP/RTL، فشرده‌سازی فاصله)
    /// - هم‌معنی‌های هدر
    /// - Fallback موقعیتی
    /// - اثرانگشت یکتا روی Ex01..Ex52 و جلوگیری از درج تکراری
    public class FaranegarImporter
    {
        private readonly AppDbContext _db;
        public FaranegarImporter(AppDbContext db) => _db = db;

        // نرمال‌سازی فارسی/عربی + حذف ZWJ/NBSP/RTL + کاهش فاصله‌ها
        private static string Norm(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            var x = s!
                .Replace('ي', 'ی')
                .Replace('ى', 'ی')
                .Replace('ئ', 'ی')
                .Replace('ك', 'ک');
            // حذف کاراکترهای کنترلی رایج
            x = x.Replace("\u200c", "").Replace("\u200f", "").Replace("\u00A0", "").Replace("‌", "");
            // فشرده‌سازی فاصله‌ها
            x = Regex.Replace(x, @"\s+", " ").Trim();
            return x;
        }

        // هم‌معنی‌های رایج برای هدرها
        private static readonly Dictionary<string, string[]> Syn = new(StringComparer.Ordinal)
        {
            [Norm("شماره بلیط")] = new[] { Norm("شماره بلیت") },
            [Norm("شماره بلیط قبلی")] = new[] { Norm("شماره بلیت قبلی") },
            [Norm("TOTAL")] = new[] { Norm("Total") },
            [Norm("FARE")] = new[] { Norm("Fare") },
            [Norm("TAX")] = new[] { Norm("Tax") },
            [Norm("Tax قبلی")] = new[] { Norm("TAX قبلی"), Norm("Tax قبلي"), Norm("TAX قبلي") },
            [Norm("خالص Fare")] = new[] { Norm("خالص FARE"), Norm("Fare خالص") },
            [Norm("درصد کمیسیون")] = new[] { Norm("درصد كميسيون") },
            [Norm("کمیسیون تشویقی")] = new[] { Norm("كميسيون تشويقي") },
            [Norm("کمیسیون")] = new[] { Norm("كميسيون") },
            [Norm("ایرلاین")] = new[] { Norm("ایر لاین") },
            [Norm("تصویر بلیت")] = new[] { Norm("تصوير بليت"), Norm("تصویر بلیط") },
            [Norm("طرف حساب")] = new[] { Norm("مشتری"), Norm("سازمان") },
        };

        public async Task<ImportResult> ImportAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0)
                throw new InvalidOperationException("فایل خالی است.");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var (table, _) = await ReadAsTableAsync(file, ct);
            if (table.Columns.Count == 0) return new ImportResult(0, 0, 0, 0);

            var expected = FaranegarSchema.HEADERS;
            var expectedNorm = expected.Select(Norm).ToArray();

            int totalRead = 0, dupInFile = 0;
            var pending = new List<(SalesReport report, string fp)>();
            var fileSeen = new HashSet<string>(StringComparer.Ordinal);

            foreach (DataRow r in table.Rows)
            {
                // dict خام از کل ستون‌ها
                var dict = new Dictionary<string, string>(StringComparer.Ordinal);
                for (int c = 0; c < table.Columns.Count; c++)
                {
                    var key = table.Columns[c].ColumnName ?? $"Col{c + 1}";
                    var val = Convert.ToString(r[c])?.Trim() ?? "";
                    dict[key] = val;
                }

                // سطر خالی؟ رد
                if (dict.Values.All(v => string.IsNullOrWhiteSpace(v)))
                    continue;

                totalRead++;

                // lookup نرمال‌شده
                var normLookup = new Dictionary<string, (string origKey, string value)>(StringComparer.Ordinal);
                foreach (var kv in dict) normLookup[Norm(kv.Key)] = (kv.Key, kv.Value);

                string Get(params string[] keys)
                {
                    foreach (var k in keys)
                    {
                        var n = Norm(k);
                        if (normLookup.TryGetValue(n, out var hit) && !string.IsNullOrWhiteSpace(hit.value))
                            return hit.value;
                        if (Syn.TryGetValue(n, out var alts))
                            foreach (var a in alts)
                                if (normLookup.TryGetValue(a, out var hit2) && !string.IsNullOrWhiteSpace(hit2.value))
                                    return hit2.value;
                    }
                    return "";
                }

                DateTime? ParseDate(string s) => DateTime.TryParse(s, out var d) ? d : null;
                decimal? ParseDec(string s)
                {
                    if (string.IsNullOrWhiteSpace(s)) return null;
                    var n = s.Replace(",", "").Replace("٬", "").Replace("٫", "").Replace(" ", "");
                    return decimal.TryParse(n, out var d) ? d : null;
                }

                var invoiceRef = Get("ش ک فاکتور", "شماره فاکتور");
                var subject = string.IsNullOrWhiteSpace(invoiceRef) ? null : $"FAR-{invoiceRef}";
                var accountName = Get("طرف حساب", "مشتری", "سازمان");

                int? accountId = null;
                if (!string.IsNullOrWhiteSpace(accountName))
                {
                    var acc = await _db.Accounts.FirstOrDefaultAsync(a => a.Name == accountName, ct);
                    if (acc is null) { acc = new Account { Name = accountName }; _db.Accounts.Add(acc); await _db.SaveChangesAsync(ct); }
                    accountId = acc.Id;
                }

                var report = new SalesReport
                {
                    Subject = subject,
                    InvoiceRef = invoiceRef,
                    AccountId = accountId,
                    IssueDate = ParseDate(Get("تاریخ", "Issue Date")),
                    Airline = Get("ایرلاین", "Airline"),
                    TicketNumber = Get("شماره بلیط", "شماره بلیت", "TicketNumber"),
                    PNR = Get("PNR"),
                    TotalAmount = ParseDec(Get("TOTAL", "Total")),
                    AdditionalJson = JsonSerializer.Serialize(dict)
                };

                // مقداردهی دقیق Ex01..Ex52
                for (int i = 0; i < expected.Length; i++)
                {
                    var wanted = expected[i];
                    var wantedNorm = expectedNorm[i];

                    string? val = null;

                    if (dict.TryGetValue(wanted, out var v1)) val = v1;
                    if (val is null && normLookup.TryGetValue(wantedNorm, out var hit)) val = hit.value;
                    if (val is null && Syn.TryGetValue(wantedNorm, out var alts))
                        foreach (var a in alts)
                            if (normLookup.TryGetValue(a, out var hit2)) { val = hit2.value; break; }

                    if (val is null) // Fallback موقعیتی
                        val = (i < table.Columns.Count) ? (Convert.ToString(r[i]) ?? "") : "";

                    var prop = typeof(SalesReport).GetProperty($"Ex{(i + 1):00}");
                    prop?.SetValue(report, val);
                }

                // اثرانگشت پایدار
                var fp = ComputeFingerprint(report);

                // حذف تکراری‌های داخل فایل
                if (!fileSeen.Add(fp)) { dupInFile++; continue; }

                report.RowFingerprint = fp;
                pending.Add((report, fp));
            }

            // حذف تکراری‌های موجود در DB
            int dupInDb = 0, inserted = 0;
            if (pending.Count > 0)
            {
                var fpSet = pending.Select(p => p.fp).Distinct(StringComparer.Ordinal).ToList();
                var exists = await _db.SalesReports.AsNoTracking()
                    .Where(x => x.RowFingerprint != null && fpSet.Contains(x.RowFingerprint))
                    .Select(x => x.RowFingerprint!)
                    .ToListAsync(ct);

                var existsSet = new HashSet<string>(exists, StringComparer.Ordinal);

                foreach (var p in pending)
                {
                    if (existsSet.Contains(p.fp)) { dupInDb++; continue; }
                    _db.SalesReports.Add(p.report);
                    inserted++;
                }

                try { await _db.SaveChangesAsync(ct); }
                catch (DbUpdateException) { /* ایندکس یکتا رقابتی → نادیده بگیر */ }
            }

            return new ImportResult(inserted, dupInFile, dupInDb, totalRead);
        }

        // اثرانگشت: SHA256 روی Ex01..Ex52 پس از Norm
        private static string ComputeFingerprint(SalesReport r)
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= FaranegarSchema.HEADERS.Length; i++)
            {
                var prop = typeof(SalesReport).GetProperty($"Ex{i.ToString("00")}");
                var val = prop?.GetValue(r) as string ?? "";
                sb.Append(Norm(val)).Append('\u001F'); // جداکننده کنترل
            }
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        // --- خواندن فایل‌ها ---
        private async Task<(DataTable table, bool headerByIndex)> ReadAsTableAsync(IFormFile file, CancellationToken ct)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);
            var bytes = ms.ToArray();

            // Excel 2003 XML؟
            var head = Encoding.UTF8.GetString(bytes, 0, Math.Min(bytes.Length, 4096));
            if (head.Contains("<?xml") && head.Contains("urn:schemas-microsoft-com:office:spreadsheet"))
                return (ParseXmlSpreadsheetSmart(bytes), true);

            // CSV؟
            if (file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                return (CsvToTableSmart(Encoding.UTF8.GetString(bytes)), true);

            // XLS/XLSX
            ms.Position = 0;
            using var reader = ExcelReaderFactory.CreateReader(ms);
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
            });
            var t = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            return (t, false);
        }

        private static DataTable ParseXmlSpreadsheetSmart(byte[] bytes)
        {
            var expectedN = FaranegarSchema.HEADERS.Select(Norm).ToHashSet(StringComparer.Ordinal);

            var table = new DataTable();
            using var s = new MemoryStream(bytes);
            var doc = XDocument.Load(s);
            XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";

            var rows = doc.Descendants(ss + "Row").ToList();
            if (rows.Count == 0) return table;

            int bestIdx = 0, bestScore = -1;
            for (int r = 0; r < Math.Min(8, rows.Count); r++)
            {
                var cells = rows[r].Elements(ss + "Cell")
                                   .Select(c => c.Element(ss + "Data")?.Value?.Trim() ?? "")
                                   .Select(Norm).ToList();
                int score = cells.Count(c => expectedN.Contains(c));
                if (score > bestScore) { bestScore = score; bestIdx = r; }
            }

            var headerCells = rows[bestIdx].Elements(ss + "Cell").ToList();
            var headers = new List<string>();
            foreach (var cell in headerCells)
                headers.Add(cell.Element(ss + "Data")?.Value?.Trim() ?? string.Empty);

            foreach (var h in headers) table.Columns.Add(h);

            for (int r = bestIdx + 1; r < rows.Count; r++)
            {
                var xrow = rows[r];
                var cells = xrow.Elements(ss + "Cell").ToList();
                var dr = table.NewRow();

                for (int c = 0; c < Math.Min(cells.Count, table.Columns.Count); c++)
                    dr[c] = cells[c].Element(ss + "Data")?.Value?.Trim() ?? string.Empty;

                bool allEmpty = true;
                for (int c = 0; c < table.Columns.Count; c++)
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dr[c]))) { allEmpty = false; break; }
                if (!allEmpty) table.Rows.Add(dr);
            }
            return table;
        }

        private static DataTable CsvToTableSmart(string text)
        {
            using var sr = new StringReader(text);
            var lines = new List<string>();
            string? line;
            while ((line = sr.ReadLine()) != null) lines.Add(line);
            if (lines.Count == 0) return new DataTable();

            var expectedN = FaranegarSchema.HEADERS.Select(Norm).ToHashSet(StringComparer.Ordinal);

            int bestIdx = 0, bestScore = -1;
            int limit = Math.Min(5, lines.Count);
            for (int i = 0; i < limit; i++)
            {
                var cells = SplitCsv(lines[i]).Select(Norm).ToList();
                int score = cells.Count(c => expectedN.Contains(c));
                if (score > bestScore) { bestScore = score; bestIdx = i; }
            }

            var table = new DataTable();
            var headers = SplitCsv(lines[bestIdx]);
            foreach (var h in headers) table.Columns.Add(h);

            for (int r = bestIdx + 1; r < lines.Count; r++)
            {
                var vals = SplitCsv(lines[r]);
                var dr = table.NewRow();
                for (int c = 0; c < headers.Count; c++)
                    dr[c] = c < vals.Count ? vals[c] : "";
                bool allEmpty = true;
                for (int c = 0; c < headers.Count; c++)
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(dr[c]))) { allEmpty = false; break; }
                if (!allEmpty) table.Rows.Add(dr);
            }
            return table;
        }

        private static List<string> SplitCsv(string line)
        {
            var res = new List<string>(); var sb = new StringBuilder(); bool q = false;
            foreach (var ch in line)
            {
                if (ch == '"') { q = !q; continue; }
                if (ch == ',' && !q) { res.Add(sb.ToString()); sb.Clear(); } else sb.Append(ch);
            }
            res.Add(sb.ToString());
            return res;
        }
    }
}
