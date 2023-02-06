using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Extensions.Selectors;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers;

public class RangZaslugaController : Controller
{
    private readonly IRangZaslugaProvider _rangZaslugaProvider;
    public RangZaslugaController(IRangZaslugaProvider rangZaslugaProvider)
    {
        _rangZaslugaProvider = rangZaslugaProvider;
    }
    [HttpGet]
    public async Task<IActionResult> Index(int sort = 1, bool ascending = true)
    {
        var result = _rangZaslugaProvider.GetAll().Result.Data.AsQueryable();
        result = result.ApplySort(sort, ascending);
        RangZaslugeViewModel rzv = new RangZaslugeViewModel();
        rzv.sort = sort;
        rzv.ascending = ascending;
       rzv.rangovi = result.Select(r => new RangZaslugaViewModel
        {
            id = r.Id,
            naziv = r.Naziv
        });
        return View(rzv);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _rangZaslugaProvider.Delete(id);
        if (result.IsFailure)
        {
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var rang = await _rangZaslugaProvider.Get(id);
        var r = rang.Data;
        return View(r);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(RangZasluga rangZasluga)
    {
        if(rangZasluga == null) return NotFound("Nema poslanih podataka");
        var result = await _rangZaslugaProvider.Edit(rangZasluga.Id,rangZasluga);
        if (result.IsFailure)
        {
            return View(rangZasluga);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RangZasluga rangZasluga)
    {
        if (rangZasluga is not null)
        {
            var result = await _rangZaslugaProvider.Create(rangZasluga);
            if (result.IsFailure)
            {
                return View();
            }
            TempData[Constants.Message] = $"Rang po zasluzi {rangZasluga.Naziv} dodan. Id mjesta = {rangZasluga.Id}";
            TempData[Constants.ErrorOccurred] = false;
        }        
        else
        {
            return View();
        }

        return RedirectToAction(nameof(Index));
    }
}