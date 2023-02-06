/*using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.Providers.Http.Options;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers;

public class ProstoriController : Controller
{
    private readonly IProstoriProvider _prostoriProvider;
    public ProstoriController(IProstoriProvider ProstoriProvider)
    {
        _prostoriProvider = ProstoriProvider;
    }
    public async Task<IActionResult> Index()
    {
        var result = _prostoriProvider.GetAll();
       ProstoriViewModel rzv = new ProstoriViewModel();
       rzv.prostori = result.Result.Data.Select(r => new ProstorViewModel
        {
            id = r.Id,
            idUdruge = r.IdUdruge,
            adresa = r.Adresa,
            namjena = r.Namjena,
            dodijelio = r.Dodijelio,
            dodjeljenoDo = r.DodjeljenoDo,
            geoDuzina = r.GeoDuzina,
            geoSirina = r.GeoSirina
        });
        return View(rzv);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = _prostoriProvider.Delete(id).Result;
        if (!result.IsSuccess)
        {
            Console.WriteLine("Not Succesful!");
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var rang = await _prostoriProvider.Get(id);
        var r = new ProstorViewModel()
        {
            id = rang.Data.Id,
            idUdruge = rang.Data.IdUdruge,
            adresa = rang.Data.Adresa,
            namjena = rang.Data.Namjena,
            dodijelio = rang.Data.Dodijelio,
            dodjeljenoDo = rang.Data.DodjeljenoDo,
            geoDuzina = rang.Data.GeoDuzina,
            geoSirina = rang.Data.GeoSirina
        };
        return View(r);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProstorViewModel model)
    {
        Prostor prostor = new Prostor(model.id, model.idUdruge, model.adresa, model.namjena, model.dodijelio, model.dodjeljenoDo, model.geoDuzina, model.geoSirina);
        var result = await _prostoriProvider.Edit(model.id, prostor);
        if (!result.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProstorViewModel model)
    {
        Prostor prostor = new Prostor(model.id, model.idUdruge, model.adresa, model.namjena, model.dodijelio, model.dodjeljenoDo, model.geoDuzina, model.geoSirina);
        var result = await _prostoriProvider.Create(prostor);
        if (!result.IsSuccess)
        {
            Console.Out.WriteLine("Neuspjesno!");
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }


}*/