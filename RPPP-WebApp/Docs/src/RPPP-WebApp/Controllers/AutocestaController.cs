using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RPPP_WebApp.Models;
using RPPP_WebApp.ViewModels;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RPPP_WebApp.Controllers
{
    public class AutocestaController : Controller
    {
        private readonly RPPP05Context ctx;
        private readonly ILogger<AutocestaController> logger;
        private readonly AppSettings appSettings;

        public AutocestaController(RPPP05Context ctx, ILogger<AutocestaController> logger, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            this.logger = logger;
            appSettings = options.Value;
        }

        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Autoceste.AsNoTracking();
            int count = await query.CountAsync();

            if (count == 0)
            {
                logger.LogInformation("Ne postoje autoceste.");
                TempData[Constants.Message] = "Ne postoje autoceste.";
                TempData[Constants.ErrorOccurred] = false;
                return NotFound();
            }

            PagingInfo pagingInfo = new()
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if (page < 1 || page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = 1, sort, ascending});
            }

            query = query.ApplySort(sort, ascending);

            List<Autoceste> autoceste = query
                                  .Skip((page - 1) * pagesize)
                                  .Take(pagesize)
                                  .Include(a => a.Dionice)
                                  .ThenInclude(d => d.UlaznaPostajaNavigation)
                                  .Include(a => a.Dionice)
                                  .ThenInclude(d => d.IzlaznaPostajaNavigation)
                                  .ToList();

            AutocestaViewModel model = new()
            {
                Autoceste = autoceste,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillDropdownKoncesionari();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Autoceste autocesta)
        {
            logger.LogTrace(JsonSerializer.Serialize(autocesta));
            autocesta.DuljinaKm = 0;//!!!!
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(autocesta);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Autocesta {autocesta.Oznaka} je uspješno dodana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanja autoceste.", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(autocesta);
                }
            }
            else
            {
                return View(autocesta);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string oznaka, int page = 1, int sort = 1, bool ascending = true)
        {
            Autoceste autocesta = await ctx.Autoceste.AsNoTracking().Where(a => a.Oznaka == oznaka).SingleOrDefaultAsync();
            if (autocesta == null)
            {
                logger.LogWarning($"Ne postoji autocesta: {oznaka}");
                return NotFound("Ne postoji autocesta: " + oznaka);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await FillDropdownKoncesionari();
                return View(autocesta);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string oznaka, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Autoceste autocesta = await ctx.Autoceste
                                    .Where(a => a.Oznaka == oznaka)
                                    .FirstOrDefaultAsync();

                if (autocesta == null)
                    return NotFound($"Autocesta: {oznaka} ne postoji.");

                if (await TryUpdateModelAsync<Autoceste>(autocesta, "",
                    a => a.Pocetak, a => a.Kraj, a => a.Koncesionar, a => a.DuljinaKm
                 ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;

                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Autocesta je ažurirana.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(autocesta);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ažuriranje autoceste nije uspjelo.");
                    return View(autocesta);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), oznaka);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string oznaka, int page = 1, int sort = 1, bool ascending = true)
        {
            Autoceste autocesta = await ctx.Autoceste.FindAsync(oznaka);
            if (autocesta != null)
            {
                try
                {
                    ctx.Remove(autocesta);
                    await ctx.SaveChangesAsync();
                    logger.LogInformation($"Autocesta {oznaka} je uspješno obrisana.");
                    TempData[Constants.Message] = $"Autocesta {oznaka} je uspješno obrisana.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Brisanje autoceste nije uspjelo: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                    logger.LogError("Brisanje autoceste nije uspjelo: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning($"Ne postoji autocesta: {oznaka} ");
                TempData[Constants.Message] = "Ne postoji autocesta: " + oznaka;
                TempData[Constants.ErrorOccurred] = true;
            }

            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        public async Task<IActionResult> Details(string? oznaka)
        {
            if (oznaka == null || ctx.Autoceste == null)
                return NotFound();

            var autocesta = await ctx.Autoceste
                .Include(a => a.Dionice)
                .ThenInclude(d => d.UlaznaPostajaNavigation)
                .Include(a => a.Dionice)
                .ThenInclude(d => d.IzlaznaPostajaNavigation)
                .FirstOrDefaultAsync(a => a.Oznaka == oznaka);

            foreach (var dionica in autocesta.Dionice)
            {
                
            }

            if (autocesta == null)
            {
                return NotFound();
            }

            return View(autocesta);
        }

        private async Task FillDropdownKoncesionari()
        {
            var listaKoncesionara = await ctx.Koncesionari
                                   .Select(k => new
                                   {
                                       Naziv = k.NazivKoncesionara
                                   })
                                   .ToListAsync();

            ViewBag.Koncesionari = new SelectList(listaKoncesionara, "Naziv", "Naziv");
        }
    }
}