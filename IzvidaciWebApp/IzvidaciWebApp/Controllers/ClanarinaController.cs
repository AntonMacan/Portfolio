using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Extensions.Selectors;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.Providers.Http.Models;
using IzvidaciWebApp.Providers.Http.Options;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IzvidaciWebApp.Controllers;

public class ClanarinaController : Controller
{
    private readonly IClanarineProvider _clanarineProvider;
    private readonly IClanProvider _clanProvider;
    public ClanarinaController(IClanarineProvider clanarineProvider, IClanProvider clanProvider)
    {
        _clanarineProvider = clanarineProvider;
        _clanProvider = clanProvider;
    }
    public async Task<IActionResult> Index(int sort = 1, bool ascending = true)
    {
        var result = _clanarineProvider.GetAll().Result.Data.AsQueryable();
        result = result.ApplySort(sort, ascending);
        var clanovi = await _clanProvider.GetAll();
        var idClana = result.Select(c => c.ClanId);

        ClanarineViewModel cvm = new ClanarineViewModel();
        cvm.sort = sort;
        cvm.ascending = ascending;
        cvm.clanarine = result.Select(r => new ClanarinaViewModel()
        {
            Id = r.Id,
            Placenost = r.Placenost,
            Iznos = r.Iznos,
            Godina = r.Godina,
            ClanIme = clanovi.Data.Where(c => c.Id == r.ClanId).Select(c => c.Ime).First(),
            ClanPrezime = clanovi.Data.Where(c => c.Id == r.ClanId).Select(c => c.Prezime).First(),
            ClanId = r.ClanId,
            Datum = r.Datum
        });
        return View(cvm);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clanarineProvider.Delete(id);
        if (result.IsFailure)
        {
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PrepareDropDownList()
    {
        var clanovi = await _clanProvider.GetAll();
        var imena = clanovi.Data.Select(c => new { c.Id, c.Prezime }).ToList();

        ViewBag.Clanovi = new SelectList(imena, nameof(Clan.Id), nameof(Clan.Prezime));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var clanarina = await _clanarineProvider.Get(id);
        var c = new ClanarinaViewModel()
        {
            Id = clanarina.Data.Id,
            Placenost = clanarina.Data.Placenost,
            Iznos = clanarina.Data.Iznos,
            Godina = clanarina.Data.Godina,
           // Clan = clanarina.Data.Clan.Prezime,
            ClanId = clanarina.Data.ClanId,
            Datum = clanarina.Data.Datum
            
        };
        await PrepareDropDownList();
        //return View(c);
        return View(clanarina.Data);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Clanarina clanarina)
    {
        if (clanarina == null) return NotFound("Nema poslanih podataka");
        /*
        Clanarina c = new Clanarina(
                                clanarina.Id,
                                clanarina.Placenost,
                                clanarina.Iznos,
                                clanarina.Godina,
                                clanarina.ClanId,
                                clanarina.Datum
                            );
        */
        var result = await _clanarineProvider.Edit(clanarina.Id, clanarina);
        if (result.IsFailure)
        {
            return View(clanarina);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PrepareDropDownList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Clanarina clanarina)
    {
        if (clanarina is not null)
        {
            var result = await _clanarineProvider.Create(clanarina);
            if (result.IsFailure)
            {
                return View();
            }
            TempData[Constants.Message] = $"Clanarina {clanarina.Id} dodana.";
            TempData[Constants.ErrorOccurred] = false;
        }
        else
        {
            return View();
        }

        return RedirectToAction(nameof(Index));
    }
}