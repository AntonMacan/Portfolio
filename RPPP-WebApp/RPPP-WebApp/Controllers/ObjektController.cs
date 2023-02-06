using System.Text.Json;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RPPP_WebApp.ViewModels;
using RPPP_WebApp.Extensions;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;
using RPPP_WebApp.ViewModels;

namespace RPPP_WebApp.Controllers;
/// <summary>
/// Web API servis za rad sa objektima
/// </summary>
public class ObjektController : Controller
{
    private readonly RPPP05Context ctx;
    private readonly ILogger<ObjektController> logger;
    private readonly AppSettings appSettings;
    
    
    public ObjektController(RPPP05Context ctx, IOptionsSnapshot<AppSettings> options)
    {
        this.ctx = ctx;
        this.logger = logger;
        this.appSettings = options.Value;
    }
    /// <summary>
    /// Vraća početnu stranicu sa listom svih objekata
    /// </summary>
    /// <param name="page">Broj željene stranice</param>
    /// <param name="sort">Index stupca po kojem želi se sortirat</param>
    /// <param name="ascending">Je li uzlazno ili silazno sortiranje?</param>
    /// <returns></returns>
    public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
    {
        int pagesize = appSettings.PageSize;
        var query = ctx.Objekt.AsNoTracking();
        int count = await query.CountAsync();
        var pagingInfo = new PagingInfo
        {
            CurrentPage = page,
            Sort = sort,
            Ascending = ascending,
            ItemsPerPage = pagesize,
            TotalItems = count
        };

        if (count == 0)
        {
            return RedirectToAction(nameof(Create));
        }

        if (page < 1 || page > pagingInfo.TotalPages)
        {
            return RedirectToAction(nameof(Index), new { page = 1, sort = sort, ascending = ascending });
        }
        query = query.ApplySort(sort, ascending);

        var objekti = await query
            .Select(k => new ObjektViewModel()
            {
                Id = k.Id,
                DionicaNaziv = Converters.GetDionicaName(k.Dionica.UlaznaPostajaNavigation.Ime,
                    k.Dionica.IzlaznaPostajaNavigation.Ime, k.Dionica.OznakaAutoceste),
                Naziv = k.Naziv,
                Opis = k.Opis,
                TipObjekta = k.TipObjekta,
                NadmorskaVisina = k.NadmorskaVisina,
                Stacionaza = k.Stacionaza,
                DimenzijeM = k.DimenzijeM,
                GeografskaDuzina = k.GeografskaDuzina,
                GeografskaSirina = k.GeografskaSirina,
                OdrzavanjeObjekta = k.OdrzavanjeObjekta.Select(o => new OdrzavanjeObjektaViewModel
                {
                    Id = o.Id,
                    datum = o.Datum,
                    Ishod = o.Ishod,
                    NazivObjekta = ctx.Objekt.Where(ob => ob.Id == o.ObjektId).SingleOrDefault().Naziv,
                    ObjektId = o.ObjektId,
                    Odrzavatelj = o.Odrzavatelj,
                    Opis = o.Opis,
                    TipId = o.TipId,
                    TipNaziv = ctx.TipOdrzavanja.Where(t => t.Id == o.TipId).SingleOrDefault().Naziv
                }).ToList()
            })
            .Skip((page - 1) * pagesize)
            .Take(pagesize)
            .ToListAsync();

        var model = new ObjektiViewModel()
        {
            Objekti = objekti,
            PagingInfo = pagingInfo
        };
        return View(model);
    }
    /// <summary>
    /// Vraća formu za unos novog objekta
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ObjektViewModel o = new ObjektViewModel();
        return View(o);
    }
    /// <summary>
    /// Stvara novi objekt sa poslanim opisom
    /// </summary>
    /// <param name="objekt"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create( ObjektViewModel objekt)
    {
        if (ModelState.IsValid)
        {
            try
            {
                Objekt o = new Objekt()
                {
                    Id = objekt.Id,
                    Naziv = objekt.Naziv,
                    Opis = objekt.Opis,
                    TipObjekta = objekt.TipObjekta,
                    NadmorskaVisina = objekt.NadmorskaVisina,
                    Stacionaza = objekt.Stacionaza,
                    DimenzijeM = objekt.DimenzijeM,
                    GeografskaDuzina = objekt.GeografskaDuzina,
                    GeografskaSirina = objekt.GeografskaSirina
                };
                Console.WriteLine(o.Id);
                ctx.Add(o);
                await ctx.SaveChangesAsync();

                TempData[Constants.Message] = $"Objekt {objekt.Naziv} dodano. Id objekta {objekt.Id}";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index));
            } catch(Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                await PrepareDropDownLists();
                return View(objekt);
            }
        }
        else
        {
            await PrepareDropDownLists();
            return View(objekt);
        }
    }
    /// <summary>
    /// Brisanje objekta određenog s id
    /// </summary>
    /// <param name="id">Vrijednost primarnog ključa (Id objekta)</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResponseMessage responseMessage;
        var objekt = await ctx.Objekt.FindAsync(id);          
        if (objekt != null)
        {
            try
            {
                string naziv = objekt.Naziv;
                ctx.Remove(objekt);
                await ctx.SaveChangesAsync();
                responseMessage = new ActionResponseMessage(MessageType.Success, $"Objekt {naziv} sa šifrom {id} uspješno obrisano.");          
            }
            catch (Exception exc)
            {          
                responseMessage = new ActionResponseMessage(MessageType.Error, $"Pogreška prilikom brisanja objekta: {exc.CompleteExceptionMessage()}");
            }
        }
        else
        {
            responseMessage = new ActionResponseMessage(MessageType.Error, $"Kamera sa šifrom {id} ne postoji");
        }

        Response.Headers["HX-Trigger"] = JsonSerializer.Serialize(new { showMessage = responseMessage });
        return responseMessage.MessageType == MessageType.Success ?
            new EmptyResult() : await Get(id);
    }
    /// <summary>
    /// Vraća formu za ažuriranje objekta
    /// </summary>
    /// <param name="id">Id objekta</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var objekt = await ctx.Objekt
            .AsNoTracking()
            .Where(k => k.Id == id)
            .SingleOrDefaultAsync();
        if (objekt != null)
        {        
            await PrepareDropDownLists();
            return PartialView(objekt);
        }
        return NotFound($"Neispravan id objekta: {id}");
    }
    /// <summary>
    /// Ažurira objekt
    /// </summary>
    /// <param name="model">Podaci o objektu</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Edit(Objekt objekt){
        if (objekt == null)
        {
            return NotFound("Nema poslanih podataka");
        }
        bool checkId = await ctx.Objekt.AnyAsync(k => k.Id == objekt.Id);
        if (!checkId)
        {
            return NotFound($"Neispravan id objekta: {objekt?.Id}");
        }

        if (ModelState.IsValid)
        {
            try
            {
                ctx.Update(objekt);
                await ctx.SaveChangesAsync();
                TempData[Constants.Message] = "Objekt ažuriran.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Get), new { id = objekt.Id});
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                await PrepareDropDownLists();
                return View(objekt);
            }
        }
        else
        {
            await PrepareDropDownLists();
            return View(objekt);
        }
    }
    /// <summary>
    /// Vraća objekt čiji je Id jednak vrijednosti parametra id
    /// </summary>
    /// <param name="id">IdObjekta</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var objekt = await ctx.Objekt
            .Where((k => k.Id == id))
            .Select(k => new ObjektViewModel()
            {
                Id = k.Id,
                DionicaNaziv = Converters.GetDionicaName(k.Dionica.UlaznaPostajaNavigation.Ime,
                    k.Dionica.IzlaznaPostajaNavigation.Ime, k.Dionica.OznakaAutoceste),
                Naziv = k.Naziv,
                Opis = k.Opis,
                TipObjekta = k.TipObjekta,
                NadmorskaVisina = k.NadmorskaVisina,
                Stacionaza = k.Stacionaza,
                DimenzijeM = k.DimenzijeM,
                GeografskaDuzina = k.GeografskaDuzina,
                GeografskaSirina = k.GeografskaSirina,
                OdrzavanjeObjekta = k.OdrzavanjeObjekta.Select(o => new OdrzavanjeObjektaViewModel
                    {
                        Id = o.Id,
                        datum = o.Datum,
                        Ishod = o.Ishod,
                        NazivObjekta = ctx.Objekt.Where(ob => ob.Id == o.ObjektId).SingleOrDefault().Naziv,
                        ObjektId = o.ObjektId,
                        Odrzavatelj = o.Odrzavatelj,
                        Opis = o.Opis,
                        TipId = o.TipId,
                        TipNaziv = ctx.TipOdrzavanja.Where(t => t.Id == o.TipId).SingleOrDefault().Naziv
                        
                    }).ToList()
            })
            .SingleOrDefaultAsync();
        if (objekt != null)
        {
            return PartialView(objekt);
        }
        else
        {
            return NotFound($"Neispravan id objekta: {id}");
        }
    }

   

    private async Task PrepareDropDownLists()
    {

        var objekti = await ctx.Dionice
            .Select(d => new
            {
                Ime = Converters.GetDionicaName(d.UlaznaPostajaNavigation.Ime,
                    d.IzlaznaPostajaNavigation.Ime,
                    d.OznakaAutoceste),
                DionicaId = d.Id


            })
            .ToListAsync();


        ViewBag.Dionice = new SelectList(objekti, "DionicaId", "Ime");

        
    }
    
}