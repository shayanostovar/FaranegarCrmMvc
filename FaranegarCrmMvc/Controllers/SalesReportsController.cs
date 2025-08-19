using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FaranegarCrmMvc.Data;
using FaranegarCrmMvc.ViewModels;
using FaranegarCrmMvc.Services;

namespace FaranegarCrmMvc.Controllers;

public class SalesReportsController : Controller
{
    private readonly AppDbContext _db;
    public SalesReportsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? q, int page = 1, int pageSize = 200)
    {
        pageSize = pageSize switch { <= 0 => 50, > 500 => 500, _ => pageSize };

        var query = _db.SalesReports.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(x =>
                (x.Subject != null && x.Subject.Contains(q)) ||
                (x.PNR != null && x.PNR.Contains(q)) ||
                (x.TicketNumber != null && x.TicketNumber.Contains(q)) ||
                (x.InvoiceRef != null && x.InvoiceRef.Contains(q)) ||
                (x.Airline != null && x.Airline.Contains(q))
            );
        }

        var total = await query.CountAsync();

        var data = await query
            .OrderBy(x => x.IssueDate ?? x.ImportedAt) // قدیمی → جدید
            .ThenBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var vm = new SalesReportsIndexVM
        {
            Headers = FaranegarSchema.HEADERS.ToList(),
            Data = data,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.SalesReports.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : View(item);
    }
}
