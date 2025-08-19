using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FaranegarCrmMvc.Data;
using FaranegarCrmMvc.Models;

namespace FaranegarCrmMvc.Controllers;

public class AccountsController : Controller
{
    private readonly AppDbContext _db;
    public AccountsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
        => View(await _db.Accounts.OrderBy(a => a.Name).ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Account model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.Accounts.Add(model);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var acc = await _db.Accounts.FindAsync(id);
        return acc is null ? NotFound() : View(acc);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Account model)
    {
        if (!ModelState.IsValid) return View(model);
        var acc = await _db.Accounts.FindAsync(id);
        if (acc is null) return NotFound();
        acc.Name = model.Name; acc.Code = model.Code; acc.Phone = model.Phone; acc.Email = model.Email; acc.IsActive = model.IsActive;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
