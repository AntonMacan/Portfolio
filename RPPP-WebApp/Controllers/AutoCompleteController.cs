
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using RPPP_WebApp.Models;
using RPPP_WebApp.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RPPP_WebApp.Models;

using RPPP_WebApp.UtilClasses;
using RPPP_WebApp.ViewModels;

namespace RPPP_WebApp.Controllers;
/// <summary>
/// Web API servis za automatsko ispunjavanje liste sa objektima koji sadrze odredenu rijec
/// </summary>
public class AutoCompleteController : Controller
{
    private readonly RPPP05Context ctx;
    private readonly AppSettings appSettings;
    
    
    public AutoCompleteController(RPPP05Context ctx, IOptionsSnapshot<AppSettings> options)
    {
        this.ctx = ctx;
        appSettings = options.Value;
    }


    public async Task<IEnumerable<AutoCompleteNaplatna>> NaplatnaPostaja(string term)
    {
        var query = ctx.NaplatnaPostaja
                  .Select(np => new AutoCompleteNaplatna
                  {
                      Id = np.NaplatnaId,
                      Label = np.Ime,
                      GeoDuzina = np.GeoDuzina,
                      GeoSirina = np.GeoSirina
                  })
                  .Where(l => l.Label.Contains(term));

        var list = await query.OrderBy(l => l.Label)
                              .ThenBy(l => l.Id)
                              .Take(appSettings.AutoCompleteCount)
                              .ToListAsync();
        return list;

    }

    /// <summary>
    /// Vraća listu svih kamera koje u nazivu sadrze odredenu rijec
    /// </summary>
    /// <param name="term">Rijec koja je u nazivu koji trazimo</param>
    /// <returns>Vraca listu IdLabel objekata</returns>
    public async Task<IEnumerable<IdLabel>> Kamera(string term)
    {
        var query = ctx.Kamere
            .Select(k => new IdLabel
            {
                Id = k.Id,
                Label = k.Naziv
            })
            .Where(l => l.Label.Contains(term));

        var list = await query.OrderBy(l => l.Label)
            .ThenBy(l => l.Id)
            .Take(appSettings.AutoCompleteCount)
            .ToListAsync();
        return list;
    }
    
    /// <summary>
    /// Vraća listu svih dionica koje u nazivu sadrze odredenu rijec
    /// </summary>
    /// <param name="term">Rijec koja je u nazivu koji trazimo</param>
    /// <returns>Vraca listu IdLabel objekata</returns>
    public async Task<IEnumerable<IdLabel>> Dionica(string term)
    {
        var query = ctx.Dionice
            .Select(d => new IdLabel
            {
                Id = d.Id,
                Label = Converters.GetDionicaName(d.UlaznaPostajaNavigation.Ime, d.IzlaznaPostajaNavigation.Ime,
                    d.OznakaAutoceste)
            });

        List<IdLabel> listQ = new List<IdLabel>();
            foreach (var l in query)
            {
                if (l.Label.ToLower().Contains(term.ToLower()))
                {
                    Console.Write("dobro");
                    listQ.Add(l);
                }
            }
            
        var list = listQ.OrderBy(l => l.Label)
            .ThenBy(l => l.Id)
            .Take(appSettings.AutoCompleteCount)
            .ToList();
        return list;
    }

    public async Task<IEnumerable<IdLabel>> Tipsadrzaja(string term)
    {
        var query = ctx.TipSadrzaja
            .Select(t => new IdLabel
            {
                Id = t.Id,
                Label = t.Naziv
            });
        List<IdLabel> listQ = new List<IdLabel>();
        foreach (var l in query)
        {
            if (l.Label.ToLower().Contains(term.ToLower()))
            {
                listQ.Add(l);
            }
        }
        var list = listQ.OrderBy(l => l.Label)
            .ThenBy(l => l.Id)
            .Take(appSettings.AutoCompleteCount)
            .ToList();
        return list;
    }
    
    /// <summary>
    /// Vraća listu svih objekata koji u nazivu sadrze odredenu rijec
    /// </summary>
    /// <param name="term">Rijec koja je u nazivu koji trazimo</param>
    /// <returns>Vraca listu IdLabel objekata</returns>
    public async Task<IEnumerable<IdLabel>> Objekt(string term)
    {
        var query = ctx.Objekt
            .Select(d => new IdLabel
            {
               Id = d.Id,
               Label = d.Naziv
            })
            .Where(l => l.Label.Contains(term));

        var list = await query.OrderBy(l => l.Label)
            .ThenBy(l => l.Id)
            .Take(appSettings.AutoCompleteCount)
            .ToListAsync();
        return list;
    }
    
    /// <summary>
    /// Vraća listu svih tipova odrzavanja koji u nazivu sadrze odredenu rijec
    /// </summary>
    /// <param name="term">Rijec koja je u nazivu koji trazimo</param>
    /// <returns>Vraca listu IdLabel objekata</returns>
    public async Task<IEnumerable<IdLabel>> Tip(string term)
    {
        var query = ctx.TipOdrzavanja
            .Select(d => new IdLabel
            {
                Id = d.Id,
                Label = d.Naziv
            })
            .Where(l => l.Label.Contains(term));

        var list = await query.OrderBy(l => l.Label)
            .ThenBy(l => l.Id)
            .Take(appSettings.AutoCompleteCount)
            .ToListAsync();
        return list;
    }

    public IEnumerable<IdLabel> Koncesionar(string term)
    {
        var query = ctx.Koncesionari
            .Select(k => new IdLabel
            {
                Id = k.GetHashCode(),
                Label = k.NazivKoncesionara
            });

        List<IdLabel> listQ = new List<IdLabel>();
        foreach (var l in query)
        {
            if (l.Label.ToLower().Contains(term.ToLower()))
            {
                listQ.Add(l);
            }
        }

        var list = listQ.OrderBy(l => l.Label)
            .ThenBy(l => l.Id)
            .Take(appSettings.AutoCompleteCount)
            .ToList();
        return list;
    }
}