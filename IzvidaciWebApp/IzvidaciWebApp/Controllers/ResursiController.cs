using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IzvidaciWebApp.Controllers;

public class ResursiController : Controller
{
    private readonly IResursiProvider _resursiProvider;
    private readonly IUdrugeProvider _udrugeProvider;
    private readonly IProstoriProvider _prostoriProvider;
    public ResursiController(IResursiProvider resursiProvider, IUdrugeProvider udrugeProvider, IProstoriProvider prostoriProvider)
    {
        _resursiProvider = resursiProvider;
        _udrugeProvider = udrugeProvider;
        _prostoriProvider = prostoriProvider;
    }
    public async Task<IActionResult> Index()
    {
        
        var result = await _resursiProvider.GetAll();
        ResursiViewModel res = new ResursiViewModel();
        res.Resursi = result.Data.Select(r => { 
            return new ResursViewModel()
            {
                IdResurs = r.Id,
                DatumNabave = DateOnly.Parse(r.DatumNabave.ToString().Split(" ")[0]),
                Prostor = r.Prostor,
                Udruga = r.Udruga,
                IdProstor = r.IdProstor,
                IdUdruga = r.IdUdruge,
                Napomena = r.Napomena,
                Naziv = r.Naziv
            };
        });
        return View(res);
    }
    
    public async Task<IActionResult> IndexPotrosni()
    {
        
        var result = await _resursiProvider.GetAllPotrosni();
        PotrosniResursiViewModel res = new PotrosniResursiViewModel();
        res.Resursi = result.Data.Select(r => { 
            return new PotrosniResursViewModel()
            {
                IdResurs = r.Id,
                DatumNabave = DateOnly.Parse(r.DatumNabave.ToString().Split(" ")[0]),
                Prostor = r.Prostor,
                Udruga = r.Udruga,
                IdProstor = r.IdProstor,
                IdUdruga = r.IdUdruge,
                Napomena = r.Napomena,
                Naziv = r.Naziv,
                RokTrajanja = DateOnly.Parse(r.RokTrajanja.ToString().Split(" ")[0])
            };
        });
        return View(res);
    }
    
    public async Task<IActionResult> IndexTrajni()
    {
        var result = await _resursiProvider.GetAllTrajni();
        TrajniResursiViewModel res = new TrajniResursiViewModel();
        res.Resursi = result.Data.Select(r => { 
            return new TrajniResursViewModel()
            {
                IdResurs = r.Id,
                DatumNabave = DateOnly.Parse(r.DatumNabave.ToString().Split(" ")[0]),
                IdProstor = r.IdProstor,
                Prostor = r.Prostor,
                Udruga = r.Udruga,
                IdUdruga = r.IdUdruge,
                Napomena = r.Napomena,
                Naziv = r.Naziv,
                Dostupno = r.JeDostupno ? "DA" : "NE",
                InventarniBroj = r.InventarniBroj
            };
        });
        return View(res);
    }
    
    [HttpGet]
    public IActionResult CreatePotrosni()
    {
        PrepareDropDown();
        return View();
    }
    
    
    [HttpGet]
    public IActionResult CreateTrajni()
    {
        PrepareDropDown();
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePotrosni(PotrosniResursViewModel? resurs)
    {
        if (resurs is not null)
        {
    
            PotrosniResurs res = new PotrosniResurs(resurs.IdResurs, resurs.Naziv, resurs.Napomena, DateTime.Parse(resurs.DatumNabave.ToString()), resurs.IdUdruga, "", resurs.IdProstor, "",DateTime.Parse(resurs.RokTrajanja.ToString()));

            var result = await _resursiProvider.CreatePotrosni(res);
            if (!result.IsSuccess)
            {
                Console.Out.WriteLine("Neuspjesno!");
                return RedirectToAction(nameof(IndexPotrosni));
            }
        }
    
        return RedirectToAction(nameof(IndexPotrosni));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTrajni(TrajniResursViewModel? resurs)
    {
        if (resurs is not null)
        {
            Console.WriteLine(resurs.DatumNabaveEdit);
            TrajniResurs res = new TrajniResurs(resurs.IdResurs, resurs.Naziv, resurs.Napomena, resurs.DatumNabaveEdit, resurs.IdUdruga, "", resurs.IdProstor, "", resurs.InventarniBroj, resurs.Dostupno.Equals("DA"));
            var result = await _resursiProvider.CreateTrajni(res);
            if (!result.IsSuccess)
            {
                Console.Out.WriteLine("Neuspjesno!");
                return RedirectToAction(nameof(IndexTrajni));
            }
        }
    
        return RedirectToAction(nameof(IndexTrajni));
    }

    private void PrepareDropDown()
    {
        var udruge = _udrugeProvider.GetAll().Result.Data.Select(d => new { d.IdUdruge, d.Naziv });
        ViewBag.Udruge = new SelectList(udruge, "IdUdruge", "Naziv");

        var prostori = _prostoriProvider.GetAll().Result.Data.Select(d => new { d.Id, d.Adresa });
        ViewBag.Prostori = new SelectList(prostori, "Id", "Adresa");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _resursiProvider.Delete(id);
        if (!result.IsSuccess)
        {
            Console.WriteLine("Not Succesful!");
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> EditPotrosni(int id)
    {
        PrepareDropDown();   
        var resurs = (await _resursiProvider.GetPotrosni(id)).Data;
        var res = new PotrosniResursViewModel()
        {
            IdResurs = resurs.Id,
            DatumNabaveEdit = resurs.DatumNabave,
            IdProstor = resurs.IdProstor,
            IdUdruga = resurs.IdUdruge,
            Napomena = resurs.Napomena,
            Naziv = resurs.Naziv,
            RokTrajanjaEdit = resurs.RokTrajanja
        };
        return View(res);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditPotrosni(PotrosniResursViewModel resurs)
    {
        Console.WriteLine(resurs.IdResurs);
        PotrosniResurs res = new PotrosniResurs(resurs.IdResurs, resurs.Naziv, resurs.Napomena, resurs.DatumNabaveEdit, resurs.IdUdruga, "", resurs.IdProstor, "", resurs.RokTrajanjaEdit);
        var result = await _resursiProvider.EditPotrosni(res.Id, res);
        if (!result.IsSuccess)
        {
            return RedirectToAction(nameof(IndexPotrosni));
        }
        return RedirectToAction(nameof(IndexPotrosni));
    }
    
    [HttpGet]
    public async Task<IActionResult> EditTrajni(int id)
    {
        PrepareDropDown();
        var resurs = (await _resursiProvider.GetTrajni(id)).Data;
        var res = new TrajniResursViewModel()
        {
            IdResurs = resurs.Id,
            DatumNabaveEdit = resurs.DatumNabave,
            IdProstor = resurs.IdProstor,
            IdUdruga = resurs.IdUdruge,
            Napomena = resurs.Napomena,
            Naziv = resurs.Naziv,
            Dostupno = resurs.JeDostupno ? "DA" : "NE",
            InventarniBroj = resurs.InventarniBroj
        };
        return View(res);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditTrajni(TrajniResursViewModel resurs)
    {
        Console.WriteLine(resurs.IdResurs);
        TrajniResurs res = new TrajniResurs(resurs.IdResurs, resurs.Naziv, resurs.Napomena, resurs.DatumNabaveEdit, resurs.IdUdruga, "", resurs.IdProstor, "", resurs.InventarniBroj, resurs.Dostupno.Equals("DA"));
        var result = await _resursiProvider.EditTrajni(res.Id, res);
        if (!result.IsSuccess)
        {
            return RedirectToAction(nameof(IndexTrajni));
        }
        return RedirectToAction(nameof(IndexTrajni));
    }
    
}
    