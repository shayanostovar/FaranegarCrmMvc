using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FaranegarCrmMvc.Data;

namespace FaranegarCrmMvc.Controllers
{
    public class CallLogsController : Controller
    {
        private readonly AppDbContext _db;
        public CallLogsController(AppDbContext db) => _db = db;

        // /CallLogs?q=...&from=yyyy-MM-dd&to=yyyy-MM-dd&page=1&pageSize=100
        public async Task<IActionResult> Index(string? q, DateTime? from, DateTime? to, int page = 1, int pageSize = 100)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 100 : pageSize;

            var query = _db.CallLogs
                .AsNoTracking()
                .Where(x => x.Direction == "Inbound")  // فقط ورودی‌ها
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x =>
                    (x.CallerIdNum ?? "").Contains(q) ||
                    (x.CallerIdName ?? "").Contains(q) ||
                    (x.Src ?? "").Contains(q) ||
                    (x.Dst ?? "").Contains(q));
            }
            if (from != null) query = query.Where(x => x.StartAt >= from.Value);
            if (to != null) query = query.Where(x => x.StartAt < to.Value.AddDays(1));

            var total = await query.CountAsync();
            var data = await query
                .OrderByDescending(x => x.StartAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Q = q;
            ViewBag.From = from?.ToString("yyyy-MM-dd");
            ViewBag.To = to?.ToString("yyyy-MM-dd");

            return View(data);
        }
    }
}
