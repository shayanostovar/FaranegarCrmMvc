using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FaranegarCrmMvc.Data;
using FaranegarCrmMvc.Models;

namespace FaranegarCrmMvc.Services
{
    /// <summary>
    /// فقط تماس‌های ورودی که حداقل یک سمتشان روی ترانک‌های to_74931/to_tel30 است
    /// و سمت دیگر داخلی/صف (SIP/Queue) باشد را لاگ می‌کند. همه زمان‌ها Local ذخیره می‌شوند.
    /// رویدادهای صف (Queue) نیز به صورت کامل در QueueLogs ذخیره شده و خلاصهٔ مهم در CallLogs می‌نشیند.
    /// </summary>
    public class AmiListener : BackgroundService
    {
        private readonly IOptions<AmiOptions> _opt;
        private readonly IDbContextFactory<AppDbContext> _dbFactory;
        private readonly ILogger<AmiListener> _logger;

        private static readonly string[] _trunkMarkers = new[] { "to_74931", "to_tel30" };

        // UniqueIdهایی که inbound معتبر هستند
        private readonly ConcurrentDictionary<string, bool> _trackInbound = new(StringComparer.OrdinalIgnoreCase);

        public AmiListener(IOptions<AmiOptions> opt,
                           IDbContextFactory<AppDbContext> dbFactory,
                           ILogger<AmiListener> logger)
        {
            _opt = opt;
            _dbFactory = dbFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try { await RunOnceAsync(stoppingToken); }
                catch (Exception ex) { _logger.LogError(ex, "AMI loop error"); }

                await Task.Delay(TimeSpan.FromSeconds(Math.Max(1, _opt.Value.ReconnectDelaySeconds)), stoppingToken);
            }
        }

        private async Task RunOnceAsync(CancellationToken ct)
        {
            var opt = _opt.Value;

            using var client = new TcpClient { ReceiveTimeout = 30000, SendTimeout = 30000 };
            _logger.LogInformation("Connecting to AMI {Host}:{Port}...", opt.Host, opt.Port);
            await client.ConnectAsync(opt.Host, opt.Port, ct);

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.ASCII, false, 1024, leaveOpen: true);
            using var writer = new StreamWriter(stream, Encoding.ASCII, 1024, leaveOpen: true) { NewLine = "\r\n", AutoFlush = true };

            // Banner
            var banner = await reader.ReadLineAsync();
            _logger.LogInformation("AMI banner: {Banner}", banner);

            // Login
            await writer.WriteLineAsync("Action: Login");
            await writer.WriteLineAsync($"Username: {opt.Username}");
            await writer.WriteLineAsync($"Secret: {opt.Password}");
            await writer.WriteLineAsync("Events: on");
            await writer.WriteLineAsync("");

            // Keepalive
            _ = Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(20), ct);
                    await writer.WriteLineAsync("Action: Ping");
                    await writer.WriteLineAsync("");
                }
            }, ct);

            var msg = new List<string>(32);
            while (!ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();
                if (line == null) break;

                if (line.Length == 0)
                {
                    if (msg.Count > 0) { _ = HandleMessageAsync(msg.ToArray(), ct); msg.Clear(); }
                    continue;
                }
                msg.Add(line);
            }

            _logger.LogWarning("AMI connection closed.");
        }

        private async Task HandleMessageAsync(string[] lines, CancellationToken ct)
        {
            try
            {
                var d = ParseToDict(lines);
                if (!d.TryGetValue("Event", out var evt)) return;

                var uniqueId = d.TryGetValue("Uniqueid", out var u1) ? u1 :
                               d.TryGetValue("UniqueID", out var u2) ? u2 : null;

                // برخی رویدادهای صف ممکن است UniqueId نداشته باشند؛ در QueueLogs بدون UniqueId ذخیره می‌کنیم
                var hasUid = !string.IsNullOrWhiteSpace(uniqueId);

                // --- اول: فیلتر تماس ورودی معتبر (برای Upsert در CallLogs) ---
                if (hasUid && (!_trackInbound.TryGetValue(uniqueId!, out var inboundOk) || !inboundOk))
                {
                    if (IsInboundFromTrackedTrunks(d))
                    {
                        _trackInbound[uniqueId!] = true;
                    }
                }

                // --- پردازش رویدادهای صف: همیشه QueueLogs را می‌نویسیم (برای رصد کامل صف) ---
                if (IsQueueEvent(evt))
                {
                    await WriteQueueLogAsync(evt, uniqueId, d, ct);

                    // همزمان خلاصهٔ مهم را روی CallLog هم بنویسیم (اگر inbound معتبر و UniqueId داریم)
                    if (hasUid && _trackInbound.TryGetValue(uniqueId!, out var inbound) && inbound)
                    {
                        await ApplyQueueSummaryToCallAsync(evt, uniqueId!, d, ct);
                    }
                    return; // queue events نیازی نیست ادامه پردازش شوند
                }

                // --- بقیهٔ رویدادهای تماس (فقط اگر inbound معتبر است) ---
                if (hasUid && _trackInbound.TryGetValue(uniqueId!, out var doTrack) && doTrack)
                {
                    await UpsertCallAsync(evt, uniqueId!, d, ct);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HandleMessage error");
            }
        }

        private static Dictionary<string, string> ParseToDict(string[] lines)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var l in lines)
            {
                var idx = l.IndexOf(':');
                if (idx <= 0) continue;
                var key = l[..idx].Trim();
                var val = l[(idx + 1)..].Trim();
                dict[key] = val;
            }
            return dict;
        }

        private static bool IsQueueEvent(string evt) =>
            evt.Equals("QueueCallerJoin", StringComparison.OrdinalIgnoreCase) ||
            evt.Equals("QueueCallerLeave", StringComparison.OrdinalIgnoreCase) ||
            evt.Equals("QueueCallerAbandon", StringComparison.OrdinalIgnoreCase) ||
            evt.Equals("AgentCalled", StringComparison.OrdinalIgnoreCase) ||
            evt.Equals("AgentConnect", StringComparison.OrdinalIgnoreCase) ||
            evt.Equals("AgentComplete", StringComparison.OrdinalIgnoreCase) ||
            evt.StartsWith("QueueMember", StringComparison.OrdinalIgnoreCase);

        private static bool IsInboundFromTrackedTrunks(Dictionary<string, string> d)
        {
            var vals = new[]
            {
                d.TryGetValue("Channel", out var ch) ? ch : null,
                d.TryGetValue("DestChannel", out var dch) ? dch : null,
                d.TryGetValue("Dialstring", out var ds) ? ds : null,
                d.TryGetValue("DialString", out var ds2) ? ds2 : null,
                d.TryGetValue("Destination", out var dest) ? dest : null,
                d.TryGetValue("ConnectedLineNum", out var cln) ? cln : null,
                d.TryGetValue("ConnectedLineIDNum", out var cln2) ? cln2 : null,
                d.TryGetValue("Context", out var ctx) ? ctx : null,
                d.TryGetValue("Exten", out var exten) ? exten : null,
                d.TryGetValue("Application", out var app) ? app : null
            }.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            bool hasTrackedTrunk = vals.Any(s => _trunkMarkers.Any(m => s!.Contains(m, StringComparison.OrdinalIgnoreCase)));

            bool looksInternalOrQueue = vals.Any(s =>
                s!.Contains("SIP/", StringComparison.OrdinalIgnoreCase) ||
                s.Contains("PJSIP/", StringComparison.OrdinalIgnoreCase) ||
                s.Contains("Local/", StringComparison.OrdinalIgnoreCase) ||
                s.Contains("@from-queue", StringComparison.OrdinalIgnoreCase) ||
                s.Contains("queue", StringComparison.OrdinalIgnoreCase) ||
                s.Contains("from-queue", StringComparison.OrdinalIgnoreCase)
            );

            return hasTrackedTrunk && looksInternalOrQueue;
        }

        // --- Helpers ---
        private static int? ToInt(string? s) => int.TryParse(s, out var n) ? n : null;

        private static DateTime? ParseAmiDateLocal(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParse(s, out var d))
                return DateTime.SpecifyKind(d, DateTimeKind.Local);
            return null;
        }

        private static string? ExtractExt(string? ifaceOrChan)
        {
            // ورودی مثل: "SIP/1010", "SIP/1010-00001234", "PJSIP/2020", "Local/1010@from-queue"
            if (string.IsNullOrWhiteSpace(ifaceOrChan)) return null;
            var s = ifaceOrChan;

            // Local/xxxx@...
            var localIdx = s.IndexOf("Local/", StringComparison.OrdinalIgnoreCase);
            if (localIdx >= 0)
            {
                var rest = s.Substring(localIdx + 6);
                var at = rest.IndexOf('@');
                var part = at >= 0 ? rest.Substring(0, at) : rest;
                return part.Trim();
            }

            // SIP/xxx یا PJSIP/xxx
            var slash = s.IndexOf('/');
            if (slash >= 0 && slash + 1 < s.Length)
            {
                var rest = s.Substring(slash + 1);
                var dash = rest.IndexOf('-');
                var ext = dash >= 0 ? rest.Substring(0, dash) : rest;
                // فقط عدد/کاراکترهای معمول داخلی
                return ext.Trim();
            }
            return null;
        }

        // --- QueueLogs writer ---
        private async Task WriteQueueLogAsync(string evt, string? uniqueId, Dictionary<string, string> d, CancellationToken ct)
        {
            await using var db = await _dbFactory.CreateDbContextAsync(ct);

            var ql = new QueueLog
            {
                UniqueId = uniqueId,
                Event = evt,
                Queue = d.TryGetValue("Queue", out var q) ? q : null,
                Position = d.TryGetValue("Position", out var pos) ? ToInt(pos) : null,
                Count = d.TryGetValue("Count", out var cnt) ? ToInt(cnt) : null,
                CallerIdNum = d.TryGetValue("CallerIDNum", out var cid) ? cid : null,
                MemberName = d.TryGetValue("MemberName", out var mn) ? mn : null,
                Interface = d.TryGetValue("Interface", out var itf) ? itf : null,
                AgentChannel = d.TryGetValue("Channel", out var ch) ? ch : (d.TryGetValue("DestChannel", out var dch) ? dch : null),
                HoldTime = d.TryGetValue("HoldTime", out var ht) ? ToInt(ht) : null,
                RingTime = d.TryGetValue("RingTime", out var rt) ? ToInt(rt) : null,
                TalkTime = d.TryGetValue("TalkTime", out var tt) ? ToInt(tt) : null,
                Reason = d.TryGetValue("Reason", out var rs) ? rs : null,
                OccurredAt = DateTime.Now,
                RawJson = JsonSerializer.Serialize(d)
            };

            db.QueueLogs.Add(ql);
            await db.SaveChangesAsync(ct);
        }

        // خلاصهٔ مهم صف را روی CallLog بنویس
        private async Task ApplyQueueSummaryToCallAsync(string evt, string uniqueId, Dictionary<string, string> d, CancellationToken ct)
        {
            await using var db = await _dbFactory.CreateDbContextAsync(ct);
            var call = await db.CallLogs.FirstOrDefaultAsync(x => x.UniqueId == uniqueId, ct);
            if (call == null)
            {
                // اگر به هر دلیل هنوز ساخته نشده، همین‌جا بساز
                call = new CallLog { UniqueId = uniqueId, StartAt = DateTime.Now, Direction = "Inbound" };
                db.CallLogs.Add(call);
            }

            // اطلاعات عمومی صف
            if (d.TryGetValue("Queue", out var qname) && !string.IsNullOrWhiteSpace(qname))
                call.QueueName = qname;

            // تعیین داخلی پاسخگو از Interface/Channel/MemberName
            string? iface = d.TryGetValue("Interface", out var itf) ? itf : null;
            string? agentChan = d.TryGetValue("Channel", out var ch) ? ch : (d.TryGetValue("DestChannel", out var dch) ? dch : null);
            var maybeExt = ExtractExt(iface) ?? ExtractExt(agentChan);
            if (!string.IsNullOrWhiteSpace(maybeExt))
            {
                call.AgentExt = maybeExt;
                call.AgentChannel = agentChan ?? iface;
            }

            switch (evt)
            {
                case "QueueCallerJoin":
                    call.QueueJoinAt ??= DateTime.Now;
                    break;

                case "QueueCallerLeave":
                    call.QueueLeaveAt = DateTime.Now;
                    if (d.TryGetValue("Reason", out var reason) && reason.Equals("timeout", StringComparison.OrdinalIgnoreCase))
                        call.Disposition ??= "No Answer";
                    break;

                case "QueueCallerAbandon":
                    call.QueueLeaveAt = DateTime.Now;
                    call.Disposition = "Abandoned";
                    break;

                case "AgentCalled":
                    call.RingSec = d.TryGetValue("RingTime", out var rt) ? ToInt(rt) : call.RingSec;
                    break;

                case "AgentConnect":
                    call.AgentConnectAt ??= DateTime.Now;
                    call.QueueHoldSec = d.TryGetValue("HoldTime", out var ht) ? ToInt(ht) : call.QueueHoldSec;
                    call.AnsweredAt ??= DateTime.Now; // اگر BridgeEnter نیامده بود
                    call.Disposition = "Answered";
                    break;

                case "AgentComplete":
                    call.TalkSec = d.TryGetValue("TalkTime", out var tt) ? ToInt(tt) : call.TalkSec;
                    // EndAt ممکن است بعداً با Hangup آپدیت شود؛ اگر نبود، همین‌جا حدس می‌زنیم
                    call.EndAt ??= DateTime.Now;
                    break;
            }

            // در نهایت ذخیره
            await db.SaveChangesAsync(ct);
        }

        // --- تماس‌های عمومی ---
        private async Task UpsertCallAsync(string evt, string uniqueId, Dictionary<string, string> d, CancellationToken ct)
        {
            await using var db = await _dbFactory.CreateDbContextAsync(ct);
            var call = await db.CallLogs.FirstOrDefaultAsync(x => x.UniqueId == uniqueId, ct);
            if (call == null)
            {
                call = new CallLog
                {
                    UniqueId = uniqueId,
                    StartAt = DateTime.Now,
                    Direction = "Inbound"
                };
                db.CallLogs.Add(call);
            }
            else
            {
                call.Direction = "Inbound";
            }

            switch (evt)
            {
                case "Newchannel":
                    call.SrcChannel = d.TryGetValue("Channel", out var ch) ? ch : call.SrcChannel;
                    call.CallerIdNum = d.TryGetValue("CallerIDNum", out var cnum) ? cnum : call.CallerIdNum;
                    call.CallerIdName = d.TryGetValue("CallerIDName", out var cname) ? cname : call.CallerIdName;
                    call.Src = call.CallerIdNum ?? call.Src;
                    if (call.StartAt == default) call.StartAt = DateTime.Now;
                    call.RawJson = JsonSerializer.Serialize(d);
                    break;

                case "Dial":
                case "DialBegin":
                    call.DstChannel = d.TryGetValue("DestChannel", out var dch) ? dch :
                                      d.TryGetValue("Destination", out var dest) ? dest :
                                      d.TryGetValue("DialString", out var ds) ? ds : call.DstChannel;

                    call.Dst = d.TryGetValue("DestCallerIDNum", out var dnum) ? dnum :
                               d.TryGetValue("DialString", out var dstr) ? dstr : call.Dst;

                    call.RawJson = JsonSerializer.Serialize(d);
                    break;

                case "BridgeEnter":
                    call.AnsweredAt = DateTime.Now;
                    call.Disposition = "Answered";
                    call.RawJson = JsonSerializer.Serialize(d);
                    break;

                case "BridgeLeave":
                    call.RawJson = JsonSerializer.Serialize(d);
                    break;

                case "Hangup":
                    call.EndAt = DateTime.Now;
                    call.HangupCause = d.TryGetValue("Cause", out var cause) ? ToInt(cause) : call.HangupCause;
                    call.HangupCauseText = d.TryGetValue("Cause-txt", out var ct2) ? ct2 : call.HangupCauseText;

                    if (call.AnsweredAt == null)
                        call.Disposition ??= "No Answer";

                    if (call.EndAt != null)
                    {
                        call.DurationSec = (int)Math.Max(0, (call.EndAt.Value - call.StartAt).TotalSeconds);
                        if (call.AnsweredAt != null)
                            call.BillSec = (int)Math.Max(0, (call.EndAt.Value - call.AnsweredAt.Value).TotalSeconds);
                    }
                    call.RawJson = JsonSerializer.Serialize(d);
                    break;

                case "Cdr":
                    call.Src = d.TryGetValue("Source", out var src) ? src : call.Src;
                    call.Dst = d.TryGetValue("Destination", out var dst) ? dst : call.Dst;

                    var start = ParseAmiDateLocal(d.TryGetValue("StartTime", out var st) ? st : null);
                    if (start != null) call.StartAt = start.Value;

                    var end = ParseAmiDateLocal(d.TryGetValue("EndTime", out var et) ? et : null);
                    if (end != null) call.EndAt = end.Value;

                    call.DurationSec = d.TryGetValue("Duration", out var du) ? ToInt(du) : call.DurationSec;
                    call.BillSec = d.TryGetValue("Billsec", out var bs) ? ToInt(bs) :
                                       d.TryGetValue("BillableSeconds", out var bs2) ? ToInt(bs2) : call.BillSec;

                    call.Disposition = d.TryGetValue("Disposition", out var disp) ? disp : call.Disposition;
                    call.RecordingFile = d.TryGetValue("RecordingFile", out var rf) ? rf : call.RecordingFile;
                    call.RawJson = JsonSerializer.Serialize(d);
                    break;
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
