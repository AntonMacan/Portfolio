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
using System.Text.Json;

namespace RPPP_WebApp.Controllers;
/// <summary>
/// Web API servis za rad sa kamerama
/// </summary>
public class KameraController : Controller
{
    private readonly RPPP05Context ctx;
    private readonly ILogger<KameraController> logger;
    private readonly AppSettings appSettings;
    
    
    public KameraController(RPPP05Context ctx, ILogger<KameraController> logger, IOptionsSnapshot<AppSettings> options)
    {
        this.ctx = ctx;
        this.logger = logger;
        this.appSettings = options.Value;
    }

    /// <summary>
    /// Vraća početnu stranicu sa listom svih kamera
    /// </summary>
    /// <param name="page">Broj željene stranice</param>
    /// <param name="sort">Index stupca po kojem želi se sortirat</param>
    /// <param name="ascending">Je li uzlazno ili silazno sortiranje?</param>
    /// <returns></returns>
    public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
    {
        int pagesize = appSettings.PageSize;
        var query = ctx.Kamere.AsNoTracking();
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

        var kamere = await query
            .Select(k => new KameraViewModel()
            {
                Id = k.Id,
                DionicaNaziv = Converters.GetDionicaName(k.Dionica.UlaznaPostajaNavigation.Ime,
                    k.Dionica.IzlaznaPostajaNavigation.Ime, k.Dionica.OznakaAutoceste),
                Naziv = k.Naziv,
                GeografskaDuzina = k.GeografskaDuzina,
                GeografskaSirina = k.GeografskaSirina,
                Slike = k.Slike.Select(s => new SlikaViewModel
                {
                    Url = s.Url,
                    Smjer = s.Smjer,
                    Datum = s.Datum
                }).ToList()

            })
            .Skip((page - 1) * pagesize)
            .Take(pagesize)
            .ToListAsync();

        var model = new KamereViewModel()
        {
            Kamere = kamere,
            PagingInfo = pagingInfo
        };
        return View(model);

    }
    /// <summary>
    /// Vraća formu za unos nove kamere
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        KameraViewModel kamera = new KameraViewModel();
        return View(kamera);
    }

    /// <summary>
    /// Stvara novu kameru sa poslanim opisom
    /// </summary>
    /// <param name="kamera"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,DionicaId,Naziv,GeografskaSirina,GeografskaDuzina")] KameraViewModel kamera)
    {
    // bug prilikom ubacivanja podataka u bazu => zamjene se sirina i duzina 
        var x = kamera.GeografskaSirina;
        kamera.GeografskaSirina = kamera.GeografskaDuzina;
        kamera.GeografskaDuzina = x;
        if (ModelState.IsValid)
        {
            try
            {
                Kamere k = new Kamere()
                {
                    DionicaId = kamera.DionicaId,
                    GeografskaDuzina = kamera.GeografskaDuzina,
                    GeografskaSirina = kamera.GeografskaSirina,
                    Id = kamera.Id,
                    Naziv = kamera.Naziv,
                    Slike = kamera.Slike.Select(s => new Slike
                    {
                        Datum = s.Datum,
                        KameraId = s.KameraId,
                        Kamera = s.Kamera,
                        Smjer = s.Smjer,
                        Url = s.Url
                    }).ToList()

                };
                ctx.Add(k);
                await ctx.SaveChangesAsync();

                TempData[Constants.Message] = $"Kamera {kamera.Naziv} dodano. Id kamere {kamera.Id}";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index));
            } catch(Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                await PrepareDropDownLists();
                return View(kamera);
            }
        }
        else
        {
            await PrepareDropDownLists();
            return View(kamera);
        }
    }
    
    
    /// <summary>
    /// Brisanje kamere određenog s id
    /// </summary>
    /// <param name="id">Vrijednost primarnog ključa (Id kamere)</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        ActionResponseMessage responseMessage;
        var kamera = await ctx.Kamere.FindAsync(id);          
        if (kamera != null)
        {
            try
            {
                string naziv = kamera.Naziv;
                ctx.Remove(kamera);
                await ctx.SaveChangesAsync();
                responseMessage = new ActionResponseMessage(MessageType.Success, $"Kamera {naziv} sa šifrom {id} uspješno obrisano.");          
            }
            catch (Exception exc)
            {          
                responseMessage = new ActionResponseMessage(MessageType.Error, $"Pogreška prilikom brisanja kamere: {exc.CompleteExceptionMessage()}");
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
    /// Vraća formu za ažuriranje kamere
    /// </summary>
    /// <param name="id">Id kamere</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var kamera = await ctx.Kamere
            .AsNoTracking()
            .Where(k => k.Id == id)
            .SingleOrDefaultAsync();
        // bug 
        var x = kamera.GeografskaSirina;
        kamera.GeografskaSirina = kamera.GeografskaDuzina;
        kamera.GeografskaDuzina = x;
        if (kamera != null)
        {        
            await PrepareDropDownLists();
            return PartialView(kamera);
        }
        return NotFound($"Neispravan id mjesta: {id}");
    }
    /// <summary>
    /// Ažurira kameru
    /// </summary>
    /// <param name="kamera">Podaci o kameri</param>
    /// <returns></returns>
    
    [HttpPost]    
    public async Task<IActionResult> Edit(Kamere kamera)
    {
        if (kamera == null)
        {
            return NotFound("Nema poslanih podataka");
        }
        
        // bug isti kao prije sa zamjenom ta dva podatka
        var x = kamera.GeografskaSirina;
        kamera.GeografskaSirina = kamera.GeografskaDuzina;
        kamera.GeografskaDuzina = x;
        
        bool checkId = await ctx.Kamere.AnyAsync(k => k.Id == kamera.Id);
        if (!checkId)
        {
            return NotFound($"Neispravan id mjesta: {kamera?.Id}");
        }

        if (ModelState.IsValid)
        {
            try
            {
                ctx.Update(kamera);
                await ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Get), new { id = kamera.Id});
            }
            catch (Exception exc)
            {
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                await PrepareDropDownLists();
                return PartialView(kamera);
            }
        }
        else
        {
            await PrepareDropDownLists();
            return PartialView(kamera);
        }
    }
    
    private async Task PrepareDropDownLists()
    {

        var kamere = await ctx.Dionice
            .Select(d => new
            {
                Ime = Converters.GetDionicaName(d.UlaznaPostajaNavigation.Ime,
                    d.IzlaznaPostajaNavigation.Ime,
                    d.OznakaAutoceste),
                DionicaId = d.Id


            })
            .ToListAsync();


        ViewBag.Dionice = new SelectList(kamere, "DionicaId", "Ime");

        
    }
    
    /// <summary>
    /// Vraća kameru čiji je Id jednak vrijednosti parametra id
    /// </summary>
    /// <param name="id">IdKamere</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var kamera = await ctx.Kamere
            .Where((k => k.Id == id))
            .Select(k => new KameraViewModel
            {
                Id = k.Id,
                DionicaNaziv = Converters.GetDionicaName(k.Dionica.UlaznaPostajaNavigation.Ime,
                    k.Dionica.IzlaznaPostajaNavigation.Ime, k.Dionica.OznakaAutoceste),
                Naziv = k.Naziv,
                GeografskaDuzina = k.GeografskaDuzina,
                GeografskaSirina = k.GeografskaSirina
            })
            .SingleOrDefaultAsync();
        if (kamera != null)
        {
            return PartialView(kamera);
        }
        else
        {
            return NotFound($"Neispravan id mjesta: {id}");
        }
    }
}