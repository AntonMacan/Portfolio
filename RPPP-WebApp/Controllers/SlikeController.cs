using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RPPP_WebApp.ViewModels;
using RPPP_WebApp.Extensions;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.Models;
using RPPP_WebApp.ViewModels;

namespace RPPP_WebApp.Controllers;
/// <summary>
/// Web API servis za rad sa slikama
/// </summary>
public class SlikeController : Controller
{
    private readonly RPPP05Context ctx;
    private readonly ILogger<SlikeController> logger;
    private readonly AppSettings appSettings;
    
    
    public SlikeController(RPPP05Context ctx, ILogger<SlikeController> logger, IOptionsSnapshot<AppSettings> options)
    {
        this.ctx = ctx;
        this.logger = logger;
        this.appSettings = options.Value;
    }
    
    /// <summary>
    /// Vraća početnu stranicu sa listom svih slika
    /// </summary>
    /// <param name="page">Broj željene stranice</param>
    /// <param name="sort">Index stupca po kojem želi se sortirat</param>
    /// <param name="ascending">Je li uzlazno ili silazno sortiranje?</param>
    /// <returns></returns>
    public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
    {
        int pagesize = appSettings.PageSize;

        var query = ctx.Slike.AsNoTracking();
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

        query = query.ApplySort3(sort, ascending);
     
        var slike = await query
            .Select( s => new SlikaViewModel
            {
                Datum = s.Datum,
                KameraNaziv = ctx.Kamere.Where(k => k.Id == s.KameraId).Single().Naziv,
                Smjer = s.Smjer,
                Url = s.Url,
                KameraId = s.KameraId
            })
            .Skip((page - 1) * pagesize)
            .Take(pagesize)
            .ToListAsync();
        var model = new SlikeViewModel()
        {
            Slike = slike,
            PagingInfo = pagingInfo
        };

        return View(model);
    }   
    
    /// <summary>
    /// Vraća formu za unos nove slike
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PrepareDropDownLists();
        SlikaViewModel s = new SlikaViewModel();
        return View(s);
    }
    /// <summary>
    /// Stvara novu sliku sa poslanim opisom
    /// </summary>
    /// <param name="slika"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SlikaViewModel slika)
    {
        if (ModelState.IsValid)
        {
            try
            {
                Slike s = new Slike
                {
                    Datum = slika.Datum,
                    KameraId = slika.KameraId,
                    Kamera = slika.Kamera,
                    Smjer = slika.Smjer,
                    Url = slika.Url
                };
                ctx.Add(s);
                await ctx.SaveChangesAsync();

                TempData[Constants.Message] = $"Slika dodana.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index));

            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                await PrepareDropDownLists();
                return View(slika);
            }
        }
        else
        {
            await PrepareDropDownLists();
            return View(slika);
        }
    }
    /// <summary>
    /// Brisanje slike određenog s id i date
    /// </summary>
    /// <param name="date">Vrijednost primarnog ključa (Vrijeme i datum slike)</param>
    /// <param name="id">Vrijednost primarnog ključa (Id kamere)</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(DateTime date,int id)
    {
        Console.Out.WriteLine(date + "  "+ id);
        ActionResponseMessage responseMessage;
        var slika = await ctx.Slike.FindAsync(date,id);        
        if (slika != null)
        {
            try
            {
                ctx.Remove(slika);
                await ctx.SaveChangesAsync();
                responseMessage = new ActionResponseMessage(MessageType.Success, $"Slika uspješno obrisana.");          
            }
            catch (Exception exc)
            {          
                responseMessage = new ActionResponseMessage(MessageType.Error, $"Pogreška prilikom brisanja slike: {exc.CompleteExceptionMessage()}");
            }
        }
        else
        {
            responseMessage = new ActionResponseMessage(MessageType.Error, $"Slike ne postoji");
        }
        Response.Headers["HX-Trigger"] = JsonSerializer.Serialize(new { showMessage = responseMessage });
        return responseMessage.MessageType == MessageType.Success ?
            new EmptyResult() : await Get(id,date);
    }
    /// <summary>
    /// Vraća formu za ažuriranje slike
    /// </summary>
    /// <param name="id">Id kamere</param>
    /// <param name="date">Datum i vrijeme slike</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(DateTime date,int id)
    {
        var slika = await ctx.Slike
            .AsNoTracking()
            .Where(k => k.Datum == date && k.KameraId == id)
            .SingleOrDefaultAsync();
        if (slika != null)
        {
            ViewBag.prevId = id;
            ViewBag.prevDate = date;
           // ViewBag.Kamera = ctx.Kamere.Where(k => id == k.Id).Single().Naziv;
           await PrepareDropDownLists();
            return PartialView(slika);
        }
        return NotFound($"Slika ne radi");
    }
    
    /// <summary>
    /// Ažurira sliku
    /// </summary>
    /// <param name="slika">Podaci o slici</param>
    /// <param name="id">Id kamere</param>
    /// <param name="prevIdid">prethodni Id kamere</param>
    /// <param name="prevDate">prethodni vrijeme i datum slike</param>
    /// <returns></returns>
    [HttpPost]    
    public async Task<IActionResult> Edit(Slike slika, int id, int prevId,DateTime prevDate)
    {
        if (slika == null)
        {
            return NotFound("Nema poslanih podataka");
        }

        bool checkId = await ctx.Slike.AnyAsync(k => k.KameraId == prevId && k.Datum == prevDate);
        if (!checkId)
        {
            return NotFound($"Neispravan unos");
        }

        if (ModelState.IsValid && (slika.Datum.Equals(prevDate)) && slika.KameraId == prevId)
        {
            try
            {
                ctx.Update(slika);
                await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Get), new { id = slika.KameraId, datum = slika.Datum});
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                return PartialView(slika);
            }
        }else if (ModelState.IsValid)
        {
            try
            {
                Slike tempSlika = new Slike()
                {
                    Datum = prevDate,
                    KameraId = prevId,
                    Smjer = slika.Smjer,
                    Url = slika.Url,
                    Kamera = slika.Kamera
                };
                ctx.Remove(tempSlika);
                ctx.Add(slika);
                await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Get), new { id = slika.KameraId, datum = slika.Datum});
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                return PartialView(slika);
            }
            
        }
        else
        {
            return PartialView(slika);
        }
    }
    /// <summary>
    /// Vraća sliku čiji je Id jednak vrijednosti parametra id i datum jednak vrijednosti parametra datum
    /// </summary>
    /// <param name="id">IdKamere</param>
    /// <param name="datum">Datum i vrijeme slike</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get(int id, DateTime datum)
    {
        var slike = await ctx.Slike
            .Where((k => k.KameraId == id && k.Datum == datum))
            .Select(k => new SlikaViewModel
            {
                KameraId = id,
                Url = k.Url,
                Datum = datum,
                Smjer = k.Smjer,
                KameraNaziv  = ctx.Kamere.Where(s => s.Id == id).Single().Naziv
            })
            .SingleOrDefaultAsync();
        if (slike != null)
        {
            return PartialView(slike);
        }
        else
        {
            return NotFound($"Neispravan id kamere: {id}");
        }
    }
    private async Task PrepareDropDownLists()
    {
        var kamere = await ctx.Kamere
            .Select(d => new KameraViewModel()
            {
                Id = d.Id,
                Naziv = d.Naziv

            })
            .ToListAsync();
        ViewBag.Kamere = new SelectList(kamere, "Id", "Naziv");
    }
}