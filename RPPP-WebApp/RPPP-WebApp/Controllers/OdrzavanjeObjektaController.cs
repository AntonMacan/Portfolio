using System.Runtime.Loader;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
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
/// Web API servis za rad sa održavanjima objekata
/// </summary>
public class OdrzavanjeObjektaController : Controller
{
    private readonly RPPP05Context ctx;
    private readonly ILogger<OdrzavanjeObjektaController> logger;
    private readonly AppSettings appSettings;
    
    
    public OdrzavanjeObjektaController(RPPP05Context ctx, ILogger<OdrzavanjeObjektaController> logger, IOptionsSnapshot<AppSettings> options)
    {
        this.ctx = ctx;
       // this.logger = logger;
        this.appSettings = options.Value;
    }
    /// <summary>
    /// Vraća početnu stranicu sa listom svih održavanja objekata
    /// </summary>
    /// <param name="page">Broj željene stranice</param>
    /// <param name="sort">Index stupca po kojem želi se sortirat</param>
    /// <param name="ascending">Je li uzlazno ili silazno sortiranje?</param>
    /// <returns></returns>
    public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
    {
        int pagesize = appSettings.PageSize;

        var query = ctx.OdrzavanjeObjekta.AsNoTracking();
        int count = await query.CountAsync();

        var pagingInfo = new PagingInfo
        {
            CurrentPage = page,
            Sort = sort,
            Ascending = ascending,
            ItemsPerPage = pagesize,
            TotalItems = count
        };
        if (page < 1 || page > pagingInfo.TotalPages)
        {
            return RedirectToAction(nameof(Index), new { page = 1, sort, ascending });
        }

        query = query.ApplySort2(sort, ascending);
     
        var odr = await query
            .Select( s => new OdrzavanjeObjektaViewModel()
            {
                Id = s.Id,
                NazivObjekta = ctx.Objekt.Where(o => o.Id == s.ObjektId).Single().Naziv,
                ObjektId = s.ObjektId,
                datum = s.Datum,
                Opis = s.Opis,
                Ishod = s.Ishod,
                TipNaziv = ctx.TipOdrzavanja.Where(o => o.Id == s.TipId).Single().Naziv,
                TipId = s.TipId,
                Odrzavatelj = s.Odrzavatelj
            })
            .Skip((page - 1) * pagesize)
            .Take(pagesize)
            .ToListAsync();
        var model = new OdrzavanjaObjektaViewModel()
        {
            OdrzavanjeObjekta = odr,
            PagingInfo = pagingInfo
        };

        return View(model);
    }    
    /// <summary>
    /// Vraća formu za unos novog održavanja objekta
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        OdrzavanjeObjektaViewModel o = new OdrzavanjeObjektaViewModel();
        return View(o);
    }
    /// <summary>
    /// Stvara novo održavanje objekta sa poslanim opisom
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OdrzavanjeObjektaViewModel o)
    {
        if (ModelState.IsValid)
        {
            try
            {
                OdrzavanjeObjekta o2 = new OdrzavanjeObjekta
                {
                    Datum = o.datum,
                    Odrzavatelj = o.Odrzavatelj,
                    Ishod = o.Ishod,
                    Opis = o.Opis,
                    ObjektId = o.ObjektId,
                    TipId = o.TipId
                };
                ctx.Add(o2);
                await ctx.SaveChangesAsync();

                TempData[Constants.Message] = $"Odrzavanje objekta dodano. Id odrzavanja {o2.Id}";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index));
            } catch(Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                await PrepareDropDownLists();
                return View(o);
            }
        }
        else
        {
            await PrepareDropDownLists();
            return View(o);
        }
    }
    /// <summary>
    /// Brisanje održavanja objekta određenog s id
    /// </summary>
    /// <param name="id">Vrijednost primarnog ključa (Id održavanja objekta)</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResponseMessage responseMessage;
        Console.WriteLine(id);
        var odrzavanje = await ctx.OdrzavanjeObjekta.FindAsync(id);
        if (odrzavanje != null)
        {
            try
            {
                ctx.Remove(odrzavanje);
                await ctx.SaveChangesAsync();
                responseMessage = new ActionResponseMessage(MessageType.Success, $"Odrzavanje uspješno obrisano.");
            }
            catch (Exception exc)
            {
                responseMessage = new ActionResponseMessage(MessageType.Error, $"Pogreška prilikom brisanja odrzavanja: {exc.CompleteExceptionMessage()}");
            }
        }else
        {
            responseMessage = new ActionResponseMessage(MessageType.Error, $"Odrzavanje {odrzavanje.Id} ne postoji");
        }
        Response.Headers["HX-Trigger"] = JsonSerializer.Serialize(new { showMessage = responseMessage });
        return responseMessage.MessageType == MessageType.Success ?
            new EmptyResult() : await Get(id);
    }
    /// <summary>
    /// Vraća formu za ažuriranje održavanja objekta
    /// </summary>
    /// <param name="id">Id održavanja objekta</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var odrzavanje = await ctx.OdrzavanjeObjekta
            .AsNoTracking()
            .Where(k => k.Id == id)
            .SingleOrDefaultAsync();
        if (odrzavanje != null)
        {
            await PrepareDropDownLists();
            return PartialView(odrzavanje);
        }
        return NotFound($"Odrzavanje ne radi");
    }
    /// <summary>
    /// Ažurira održavanje objekta
    /// </summary>
    /// <param name="odrzavanjeObjekta">Podaci o održavanju objekta</param>
    /// <returns></returns>
    [HttpPost]    
    public async Task<IActionResult> Edit(OdrzavanjeObjekta odrzavanjeObjekta, int objektId, int tipId)
    {
        if (odrzavanjeObjekta == null)
        {
            return NotFound("Nema poslanih podataka");
        }

        bool checkId = await ctx.OdrzavanjeObjekta.AnyAsync(k => k.Id == odrzavanjeObjekta.Id);
        if (!checkId)
        {
            return NotFound($"Neispravan unos");
        }

        odrzavanjeObjekta.ObjektId = objektId;
        odrzavanjeObjekta.TipId = tipId;

        if (ModelState.IsValid)
        {
            try
            {
                ctx.Update(odrzavanjeObjekta);
                var x = await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Get), new { id = odrzavanjeObjekta.Id});
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                return PartialView(odrzavanjeObjekta);
            }
        }
        else
        {
            return PartialView(odrzavanjeObjekta);
        }
    }
    
    /// <summary>
    /// Vraća održavanje objekta čiji je Id jednak vrijednosti parametra id
    /// </summary>
    /// <param name="id">IdOdržavanjaObjetka</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var odrzavanjeObjekta = await ctx.OdrzavanjeObjekta
            .Where((k => k.Id == id))
            .Select(s => new OdrzavanjeObjektaViewModel()
            {
                Id = s.Id,
                NazivObjekta = ctx.Objekt.Where(o => o.Id == id).Single().Naziv,
                ObjektId = s.ObjektId,
                datum = s.Datum,
                Opis = s.Opis,
                Ishod = s.Ishod,
                TipNaziv = ctx.TipOdrzavanja.Where(o => o.Id == id).Single().Naziv,
                TipId = s.TipId,
                Odrzavatelj = s.Odrzavatelj
            })
            .SingleOrDefaultAsync();
        if (odrzavanjeObjekta != null)
        {
            return PartialView(odrzavanjeObjekta);
        }
        else
        {
            return NotFound($"Neispravan id odrzavanja: {id}");
        }
    }
    
    private async Task PrepareDropDownLists()
    {

        var objekti = await ctx.Objekt.Select(
                d => new
                {
                    Ime = d.Naziv,
                    ObjektId = d.Id
                }
                )
            .ToListAsync();


        ViewBag.Objekti = new SelectList(objekti, "ObjektId", "Ime");

        var tipovi = await ctx.TipOdrzavanja.Select(
                d => new
                {
                    Ime = d.Naziv,
                    ObjektId = d.Id
                }
            )
            .ToListAsync();


        ViewBag.Tipovi = new SelectList(tipovi, "ObjektId", "Ime");

    }
    
    
}