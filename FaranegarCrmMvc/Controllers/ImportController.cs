using Microsoft.AspNetCore.Mvc;
using FaranegarCrmMvc.Services;

namespace FaranegarCrmMvc.Controllers;

public class ImportController : Controller
{
    private readonly FaranegarImporter _importer;
    public ImportController(FaranegarImporter importer) => _importer = importer;

    [HttpGet] public IActionResult Index() => View();

    [HttpPost]
    [RequestSizeLimit(200_000_000)]
    public async Task<IActionResult> Index(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) { TempData["Err"] = "فایل انتخاب نشده."; return View(); }
        try
        {
            var res = await _importer.ImportAsync(file, ct);
            TempData["Ok"] = $"ایمپورت انجام شد. درج جدید: {res.Inserted} | تکراری داخل فایل: {res.DuplicatesInFile} | تکراری در دیتابیس: {res.DuplicatesInDb} | کل ردیف‌ها: {res.TotalRead}";
        }
        catch (Exception ex)
        {
            TempData["Err"] = ex.Message;
        }
        return View();
    }
}
