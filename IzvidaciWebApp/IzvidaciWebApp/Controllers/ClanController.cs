using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.Providers.Http.Options;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace IzvidaciWebApp.Controllers;

public class ClanController : Controller
{
    private readonly IClanProvider _clanProvider;
    public ClanController(IClanProvider clanProvider)
    {
        _clanProvider = clanProvider;
    }
    
    public async Task<IActionResult> Index()
    {
        var result = await _clanProvider.GetAll();
        ClanoviViewModel cvm = new ClanoviViewModel();
        cvm.clanovi = result.Data.Select(r => new ClanViewModel()
        {
            Id = r.Id,
            Ime = r.Ime,
            Prezime = r.Prezime,
            DatumRodenja = r.DatumRodenja,
            Slika = r.Slika,
            Adresa = r.Adresa,
            ImaMaramu = r.ImaMaramu,
            DatumMarama = r.DatumMarama,
            MjestoMarama = r.MjestoMarama
        });
        return View(cvm);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clanProvider.Delete(id);
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
        var clan = await _clanProvider.Get(id);
        var c = new ClanViewModel()
        {
            Id = clan.Data.Id,
            Ime = clan.Data.Ime,
            Prezime = clan.Data.Prezime,
            DatumRodenja = clan.Data.DatumRodenja,
            Slika = clan.Data.Slika,
            Adresa = clan.Data.Adresa,
            ImaMaramu = clan.Data.ImaMaramu,
            DatumMarama = clan.Data.DatumMarama,
            MjestoMarama = clan.Data.MjestoMarama

        };
        return View(c);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(ClanViewModel clan)
    {
        Clan c = new Clan(
                              clan.Id,
                              clan.Ime,
                              clan.Prezime,
                              clan.DatumRodenja,
                              clan.Slika,
                              clan.Adresa,
                              clan.ImaMaramu,
                              clan.DatumMarama,
                              clan.MjestoMarama
                            );
        var result = await _clanProvider.Edit(clan.Id, c);
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
    public async Task<IActionResult> Create(ClanViewModel clan)
    {
        if (clan is not null)
        {
            Clan c = new Clan(
                                clan.Id,
                                clan.Ime,
                             clan.Prezime,
                         clan.DatumRodenja,
                       clan.Slika,
                    clan.Adresa,
                  clan.ImaMaramu,
                   clan.DatumMarama,
                    clan.MjestoMarama
                            );

            var result = await _clanProvider.Create(c);
            if (!result.IsSuccess)
            {
                Console.Out.WriteLine("Neuspjesno!");
                return RedirectToAction(nameof(Index));
            }
        }

        return RedirectToAction(nameof(Index));
    }
}