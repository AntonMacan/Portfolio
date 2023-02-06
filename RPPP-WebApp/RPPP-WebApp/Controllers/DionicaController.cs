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
    /// <summary>
    /// Kontroler za dionice
    /// </summary>
    public class DionicaController : Controller
    {
        private readonly RPPP05Context ctx;
        private readonly ILogger<DionicaController> logger;
        private readonly AppSettings appSettings;
        private readonly Random random;

        public DionicaController(RPPP05Context ctx, ILogger<DionicaController> logger, IOptionsSnapshot<AppSettings> options)
        {
            random = new Random();
            this.ctx = ctx;
            this.logger = logger;
            appSettings = options.Value;
        }

        /// <summary>
        /// Vraca tablicu dionica
        /// </summary>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Dionice.AsNoTracking();
            int count = await query.CountAsync();

            if (count == 0)
            {
                logger.LogInformation("Ne postoje dionice.");
                TempData[Constants.Message] = "Ne postoje dionice.";
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

            List<Dionice> dionice = query
                .Include(d => d.UlaznaPostajaNavigation)
                .Include(d => d.IzlaznaPostajaNavigation)
                .Include(d => d.OznakaAutocesteNavigation)
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            DionicaViewModel model = new()
            {
                Dionice = dionice,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        /// <summary>
        /// Vraca stranicu za stvaranje nove dionice
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillDropdown();
            return View();
        }

        /// <summary>
        /// Stvara novu dionicu
        /// </summary>
        /// <param name="dionica"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dionice dionica)
        {
            logger.LogTrace(JsonSerializer.Serialize(dionica));

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(dionica);
                    await ctx.SaveChangesAsync();
                    
                    TempData[Constants.Message] = $"Dionica {dionica.Id} je uspješno dodana.";
                    TempData[Constants.ErrorOccurred] = false;

                    await FixAutocestaDuljina(dionica.OznakaAutoceste);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ctx.Remove(dionica);
                    if (exc.InnerException != null)
                    {
                        if (exc.InnerException.Message.Contains("Violation of PRIMARY KEY"))
                        {
                            dionica.Id *= 2;
                            dionica.Id += random.Next(1, 20);
                            return await Create(dionica);
                        }
                    }

                    logger.LogError("Pogreška prilikom dodavanja dionice.", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await FillDropdown();
                    return View(dionica);
                }
            }
            else
            {
                await FillDropdown();
                return View(dionica);
            }
        }

        /// <summary>
        /// Vraca stranicu za uredivanje dionica
        /// </summary>
        /// <param name="id">id dionice</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            Dionice dionica = await ctx.Dionice.FindAsync(id);
            if (dionica == null)
            {
                logger.LogWarning($"Ne postoji dionica: {id}");
                return NotFound("Ne postoji dionica: " + id);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await FillDropdown();
                return View(dionica);
            }
        }

        /// <summary>
        /// Ureduje dionicu
        /// </summary>
        /// <param name="id">id dionice</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce</param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Dionice dionica = await ctx.Dionice
                    .FindAsync(id);

                if (dionica == null)
                    return NotFound($"Dionica: {id} ne postoji.");

                if (await TryUpdateModelAsync<Dionice>(dionica, "",
                    d => d.OznakaAutoceste, d => d.UlaznaPostaja, d => d.IzlaznaPostaja, d => d.DuljinaKm
                 ))
                {

                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;

                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Dionica je ažurirana.";
                        TempData[Constants.ErrorOccurred] = false;

                        await FixAutocestaDuljina(dionica.OznakaAutoceste);
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        await FillDropdown();
                        return View(dionica);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ažuriranje dionice nije uspjelo.");
                    await FillDropdown();
                    return View(dionica);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                await FillDropdown();
                return RedirectToAction(nameof(Edit), id);
            }
        }

        /// <summary>
        /// Brise dionicu
        /// </summary>
        /// <param name="id">id dionice</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            Dionice dionica = await ctx.Dionice.FindAsync(id);
            if (dionica != null)
            {
                try
                {
                    var a = dionica.OznakaAutoceste;
                    ctx.Remove(dionica);
                    await ctx.SaveChangesAsync();
                    logger.LogInformation($"Dionica {id} je uspješno obrisana.");
                    TempData[Constants.Message] = $"Dionica {id} je uspješno obrisana.";

                    await FixAutocestaDuljina(a);
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Brisanje dionice nije uspjelo: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                    logger.LogError("Brisanje dionice nije uspjelo: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning($"Ne postoji dionica: {id} ");
                TempData[Constants.Message] = "Ne postoji dionica: " + id;
                TempData[Constants.ErrorOccurred] = true;
            }

            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        /// <summary>
        /// Vraca detalje dionice
        /// </summary>
        /// <param name="id">id dionice</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || ctx.Dionice == null)
                return NotFound();

            var dionica = await ctx.Dionice
                .Include(d => d.UlaznaPostajaNavigation)
                .Include(d => d.IzlaznaPostajaNavigation)
                .Include(d => d.OznakaAutocesteNavigation)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dionica == null)
            {
                return NotFound();
            }

            return View(dionica);
        }

        /// <summary>
        /// Pomocna metoda za pripremu select liste
        /// </summary>
        /// <returns></returns>
        private async Task FillDropdown()
        {
            await FillDropdownAutocesta();
            await FillDropdownNaplatne();
        }

        /// <summary>
        /// Pomocna metoda za pripremu select liste autocesta
        /// </summary>
        /// <returns></returns>
        private async Task FillDropdownAutocesta()
        {
            var listaAutocesta = await ctx.Autoceste
                                   .Select(a => new
                                   {
                                       Naziv = a.Oznaka
                                   })
                                   .ToListAsync();

            ViewBag.Autoceste = new SelectList(listaAutocesta, "Naziv", "Naziv");
        }

        /// <summary>
        /// Pomocna metoda za pripremu select liste naplatnih postaji
        /// </summary>
        /// <returns></returns>
        private async Task FillDropdownNaplatne()
        {
            var listaNaplatnih = await ctx.NaplatnaPostaja
                                   .Select(n => new
                                   {
                                       Naziv = n.Ime,
                                       ID = n.NaplatnaId
                                   })
                                   .ToListAsync();

            ViewBag.NaplatnePostaje = new SelectList(listaNaplatnih, "ID", "Naziv");
        }

        /// <summary>
        /// Pomocna metoda za korekciju duljine autoceste
        /// </summary>
        /// <param name="oznaka"></param>
        /// <returns></returns>
        private async Task FixAutocestaDuljina(string oznaka)
        {
            var autocesta = await ctx.Autoceste
                .Include(a => a.Dionice)
                .FirstOrDefaultAsync(a => a.Oznaka == oznaka);

            int len = 0;
            foreach (Dionice d in autocesta.Dionice)
                len += d.DuljinaKm;

            autocesta.DuljinaKm = len;

            await ctx.SaveChangesAsync();
        }
    }
}