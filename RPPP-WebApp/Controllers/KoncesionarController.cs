using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RPPP_WebApp.Models;
using RPPP_WebApp.ViewModels;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.Extensions;
using System.Text.Json;

namespace RPPP_WebApp.Controllers
{
    /// <summary>
    /// Kontroler za koncesionare
    /// </summary>
    public class KoncesionarController : Controller
    {
        private readonly RPPP05Context ctx;
        private readonly ILogger<KoncesionarController> logger;
        private readonly AppSettings appSettings;

        public KoncesionarController(RPPP05Context ctx, ILogger<KoncesionarController> logger, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            this.logger = logger;
            appSettings = options.Value;
        }

        /// <summary>
        /// Vraca stranicu koncesionara
        /// </summary>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Koncesionari.AsNoTracking();
            int count = await query.CountAsync();

            if (count == 0)
            {
                logger.LogInformation("Ne postoje koncesionari.");
                TempData[Constants.Message] = "Ne postoje koncesionari.";
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
                return RedirectToAction(nameof(Index), new { page = 1, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);

            List<Koncesionari> koncesionari = query
                                  .Skip((page - 1) * pagesize)
                                  .Take(pagesize)
                                  .ToList();

            KoncesionarViewModel model = new()
            {
                Koncesionari = koncesionari,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        /// <summary>
        /// Vraca stranicu za stvaranje novog koncesionara
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Stvara novog koncesionara
        /// </summary>
        /// <param name="koncesionar"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Koncesionari koncesionar)
        {
            logger.LogTrace(JsonSerializer.Serialize(koncesionar));

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(koncesionar);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Koncesionar {koncesionar.NazivKoncesionara} je uspješno dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanja koncesionara.", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(koncesionar);
                }
            }
            else
            {
                return View(koncesionar);
            }
        }

        /// <summary>
        /// Vraca stranicu za uredivanje koncesionara
        /// </summary>
        /// <param name="naziv">naziv koncesionara</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string naziv, int page = 1, int sort = 1, bool ascending = true)
        {
            Koncesionari koncesionar = await ctx.Koncesionari.FindAsync(naziv);
            if (koncesionar == null)
            {
                logger.LogWarning($"Ne postoji koncesionar: {naziv}");
                return NotFound("Ne postoji koncesionar: " + naziv);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(koncesionar);
            }
        }

        /// <summary>
        /// Obraduje zahtjev za uredivanjem koncesionara
        /// </summary>
        /// <param name="naziv">naziv koncesionara</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce</param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string naziv, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Koncesionari koncesionar = await ctx.Koncesionari
                    .FindAsync(naziv);

                if (koncesionar == null)
                    return NotFound($"Koncesionar: {naziv} ne postoji.");

                if (await TryUpdateModelAsync<Koncesionari>(koncesionar, "",
                    k => k.Url
                 ))
                {

                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;

                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Koncesionar je ažuriran.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(koncesionar);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ažuriranje koncesionara nije uspjelo.");
                    return View(koncesionar);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), naziv);
            }
        }

        /// <summary>
        /// Brise koncesionara
        /// </summary>
        /// <param name="naziv">naziv koncesionara</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string naziv, int page = 1, int sort = 1, bool ascending = true)
        {
            Koncesionari koncesionar = await ctx.Koncesionari.FindAsync(naziv);
            if (koncesionar != null)
            {
                try
                {
                    ctx.Remove(koncesionar);
                    await ctx.SaveChangesAsync();
                    logger.LogInformation($"Koncesionar {naziv} je uspješno obrisan.");
                    TempData[Constants.Message] = $"Koncesionar {naziv} je uspješno obrisan.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Brisanje koncesionara nije uspjelo: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                    logger.LogError("Brisanje koncesionara nije uspjelo: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning($"Ne postoji koncesionar: {naziv} ");
                TempData[Constants.Message] = "Ne postoji koncesionar: " + naziv;
                TempData[Constants.ErrorOccurred] = true;
            }

            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        /// <summary>
        /// Vraca detalje koncesionara
        /// </summary>
        /// <param name="naziv">naziv koncesionara</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(string? naziv)
        {
            if (naziv == null || ctx.Koncesionari == null)
                return NotFound();

            var koncesionar = await ctx.Koncesionari
                .FindAsync(naziv);
            if (koncesionar == null)
            {
                return NotFound();
            }

            return View(koncesionar);
        }
    }
}