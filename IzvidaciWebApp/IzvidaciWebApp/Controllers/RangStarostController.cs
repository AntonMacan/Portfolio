using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Extensions.Selectors;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.Providers.Http.Options;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers;

public class RangStarostController : Controller
{
    private readonly IRangStarostProvider _rangStarostProvider;
    public RangStarostController(IRangStarostProvider rangStarostProvider)
    {
        _rangStarostProvider = rangStarostProvider;
    }
    public async Task<IActionResult> Index(int sort = 1, bool ascending = true)
    {
        var result = _rangStarostProvider.GetAll().Result.Data.AsQueryable();
        result = result.ApplySort(sort, ascending);
        RangStarostiViewModel rzv = new RangStarostiViewModel();
        rzv.sort = sort;
        rzv.ascending = ascending;
       rzv.rangovi = result.Select(r => new RangStarostViewModel()
        {
            id = r.Id,
            naziv = r.Naziv
        });
        return View(rzv);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _rangStarostProvider.Delete(id);
        if (result.IsFailure)
        {
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var rang = await _rangStarostProvider.Get(id);
        var r = rang.Data;
        return View(r);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(RangStarost rangStarost)
    {
        if(rangStarost == null) return NotFound("Nema poslanih podataka");
        var result = await _rangStarostProvider.Edit(rangStarost.Id,rangStarost);
        if (result.IsFailure)
        {
            return View(rangStarost);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RangStarost rangStarost)
    {
        if (rangStarost is not null)
        {
            var result = await _rangStarostProvider.Create(rangStarost);
            if (result.IsFailure)
            {
                return View();
            }
            TempData[Constants.Message] = $"Rang po starosti {rangStarost.Naziv} dodan. Id mjesta = {rangStarost.Id}";
            TempData[Constants.ErrorOccurred] = false;
        }
        else
        {
            return View();
        }

        return RedirectToAction(nameof(Index));
    }
}