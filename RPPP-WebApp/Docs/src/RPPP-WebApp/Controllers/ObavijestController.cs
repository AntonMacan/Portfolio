using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RPPP_WebApp.Models;
using RPPP_WebApp.ViewModels;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using RPPP_WebApp.UtilClasses;
using System;

namespace RPPP_WebApp.Controllers
{
    public class ObavijestController : Controller
    {
        private readonly RPPP05Context ctx;
        private readonly ILogger<ObavijestController> logger;
        private readonly AppSettings appSettings;
        private readonly Random random;

        public ObavijestController(RPPP05Context ctx, ILogger<ObavijestController> logger, IOptionsSnapshot<AppSettings> options)
        {
            random = new Random();
            this.ctx = ctx;
            this.logger = logger;
            appSettings = options.Value;
        }

        public async Task<IActionResult> Index(int page = 1, int sort = 3, bool ascending = false)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Obavijesti.AsNoTracking();
            int count = await query.CountAsync();

            if (count == 0)
            {
                logger.LogInformation("Nema obavijesti.");
                TempData[Constants.Message] = "Nema obavijesti.";
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

            List<Obavijesti> obavijesti = query
                .Include(o => o.IdDioniceNavigation)
                .Include(o => o.IdDioniceNavigation.OznakaAutocesteNavigation)
                .Include(o => o.IdDioniceNavigation.UlaznaPostajaNavigation)
                .Include(o => o.IdDioniceNavigation.IzlaznaPostajaNavigation)
                                  .Skip((page - 1) * pagesize)
                                  .Take(pagesize)
                                  .ToList();

            ObavijestViewModel model = new()
            {
                Obavijesti = obavijesti,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillDropdownDionice();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Obavijesti obavijest)
        {
            logger.LogTrace(JsonSerializer.Serialize(obavijest));

            if (ModelState.IsValid)
            {
                try
                {
                    obavijest.VrijemeObjave = DateTime.Now;
                    ctx.Add(obavijest);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Obavijest {obavijest.Naslov} je uspješno dodana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    ctx.Remove(obavijest);
                    if (exc.InnerException != null)
                    {
                        if (exc.InnerException.Message.Contains("Violation of PRIMARY KEY"))
                        {
                            obavijest.Id *= 2;
                            obavijest.Id += random.Next(1, 20);
                            return await Create(obavijest);
                        }
                    }

                    logger.LogError("Pogreška prilikom dodavanja obavijesti.", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await FillDropdownDionice();
                    return View(obavijest);
                }
            }
            else
            {
                await FillDropdownDionice();
                return View(obavijest);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            Obavijesti obavijest = await ctx.Obavijesti
                .Include(o => o.IdDioniceNavigation)
                .Include(o => o.IdDioniceNavigation.OznakaAutocesteNavigation)
                .Include(o => o.IdDioniceNavigation.UlaznaPostajaNavigation)
                .Include(o => o.IdDioniceNavigation.IzlaznaPostajaNavigation)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obavijest == null)
            {
                logger.LogWarning($"Ne postoji obavijest: {id}");
                return NotFound("Ne postoji obavijest: " + id);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await FillDropdownDionice();
                return View(obavijest);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            try
            {
                Obavijesti obavijest = await ctx.Obavijesti
                .Include(o => o.IdDioniceNavigation)
                .Include(o => o.IdDioniceNavigation.OznakaAutocesteNavigation)
                .Include(o => o.IdDioniceNavigation.UlaznaPostajaNavigation)
                .Include(o => o.IdDioniceNavigation.IzlaznaPostajaNavigation)
                .FirstOrDefaultAsync(o => o.Id == id);

                if (obavijest == null)
                    return NotFound($"Obavijest: {id} ne postoji.");

                if (await TryUpdateModelAsync<Obavijesti>(obavijest, "",
                    o => o.Naslov, o => o.Opis, o => o.IdDionice
                 ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;

                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Obavijest je ažurirana.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        await FillDropdownDionice();
                        return View(obavijest);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ažuriranje koncesionara nije uspjelo.");
                    await FillDropdownDionice();
                    return View(obavijest);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                await FillDropdownDionice();
                return RedirectToAction(nameof(Edit), id);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            Obavijesti obavijest = await ctx.Obavijesti.FindAsync(id);
            if (obavijest != null)
            {
                try
                {
                    string naslov = obavijest.Naslov;
                    ctx.Remove(obavijest);
                    await ctx.SaveChangesAsync();
                    logger.LogInformation($"Obavijest {naslov} je uspješno obrisana.");
                    TempData[Constants.Message] = $"Obavijest {naslov} je uspješno obrisana.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Brisanje obavijesti nije uspjelo: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                    logger.LogError("Brisanje obavijesti nije uspjelo: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning($"Ne postoji obavijest: {id} ");
                TempData[Constants.Message] = "Ne postoji obavijest: " + id;
                TempData[Constants.ErrorOccurred] = true;
            }

            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || ctx.Obavijesti == null)
                return NotFound();

            var obavijest = await ctx.Obavijesti
                .Include(o => o.IdDioniceNavigation)
                .Include(o => o.IdDioniceNavigation.OznakaAutocesteNavigation)
                .Include(o => o.IdDioniceNavigation.UlaznaPostajaNavigation)
                .Include(o => o.IdDioniceNavigation.IzlaznaPostajaNavigation)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (obavijest == null)
            {
                return NotFound();
            }

            return View(obavijest);
        }

        private async Task FillDropdownDionice()
        {
            var listaDionica = await ctx.Dionice
                .Include(d => d.OznakaAutocesteNavigation)
                .Include(d => d.UlaznaPostajaNavigation)
                .Include(d => d.IzlaznaPostajaNavigation)
                .Select(d => new
                {
                    Naziv = Converters.GetDionicaName(d),
                    d.Id
                })
                .ToListAsync();

            ViewBag.Dionice = new SelectList(listaDionica, "Id", "Naziv");
        }
    }
}