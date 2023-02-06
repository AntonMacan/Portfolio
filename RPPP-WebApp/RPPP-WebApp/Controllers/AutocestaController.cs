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
    /// <summary>
    /// Kontroler za autoceste
    /// </summary>
    public class AutocestaController : Controller
    {
        private readonly RPPP05Context ctx;
        private readonly ILogger<AutocestaController> logger;
        private readonly AppSettings appSettings;
        private readonly Random random;

        public AutocestaController(RPPP05Context ctx, ILogger<AutocestaController> logger, IOptionsSnapshot<AppSettings> options)
        {
            random = new Random();
            this.ctx = ctx;
            this.logger = logger;
            appSettings = options.Value;
        }

        /// <summary>
        /// Vraca tablicu s autocestama
        /// </summary>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce sortiranje</param>
        /// <returns></returns>
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

        /// <summary>
        /// Vraca stranicu za stvaranje nove autoceste
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillDropdownKoncesionari();
            return View();
        }

        /// <summary>
        /// Pomocna metoda koja priprema select listu koncesionara
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Obrađuje post zahtjev za novom autocestom
        /// </summary>
        /// <param name="autocesta">nova autocesta</param>
        /// <returns></returns>
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

        /// <summary>
        /// Vraca stranicu za uredivanje autoceste
        /// </summary>
        /// <param name="oznaka">autocesta koja se ureduje</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce sortiranje</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string oznaka, int page = 1, int sort = 1, bool ascending = true)
        {
            Autoceste autocesta = await ctx.Autoceste.AsNoTracking()
                .Where(a => a.Oznaka == oznaka)
                .Include(a => a.Dionice)
                .ThenInclude(d => d.UlaznaPostajaNavigation)
                .Include(a => a.Dionice)
                .ThenInclude(d => d.IzlaznaPostajaNavigation)
                .SingleOrDefaultAsync();
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

        /// <summary>
        /// Obraduje post zahtjev za izmjenu autoceste
        /// </summary>
        /// <param name="oznaka">autocesta koja je uredena</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce sortiranje</param>
        /// <returns></returns>
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

        /// <summary>
        /// Brise autocestu
        /// </summary>
        /// <param name="oznaka">oznaka autoceste koja se brise</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">rastuce sortiranje</param>
        /// <returns></returns>
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

        /// <summary>
        /// Vraca stranicu s detaljima autoceste
        /// </summary>
        /// <param name="oznaka">oznaka autoceste</param>
        /// <returns></returns>
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

            if (autocesta == null)
            {
                return NotFound();
            }

            return View(autocesta);
        }

        /// <summary>
        /// Vraca parcijalni pogled za uredivanje dionice u detail-u master forme
        /// </summary>
        /// <param name="id">id dionice</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditDionica(int id)
        {
            var dionica = await ctx.Dionice
                .Include(d => d.UlaznaPostajaNavigation)
                .Include(d => d.IzlaznaPostajaNavigation)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dionica == null)
            {
                return NotFound($"Neispravan id dionice: {id}");
            }

            return PartialView(dionica);
        }

        /// <summary>
        /// Obraduje post zahtjev za izmjenom dionice u detail-u master forme
        /// </summary>
        /// <param name="dionica"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditDionica(Dionice dionica)
        {
            if (dionica == null)
            {
                return NotFound("Neispravan zatjev.");
            }

            bool checkId = await ctx.Dionice.AnyAsync(d => d.Id == dionica.Id);

            if (!checkId)
            {
                return NotFound($"Neispravan id dionice: {dionica.Id}");
            }

            var autocesta = await ctx.Autoceste
                .Include(a => a.Dionice)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Oznaka == dionica.OznakaAutoceste);
            int lenBefore = autocesta.Dionice.FirstOrDefault(d => d.Id == dionica.Id).DuljinaKm;
            int newLen = autocesta.DuljinaKm - lenBefore + dionica.DuljinaKm;

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Update(dionica);
                    await ctx.SaveChangesAsync();

                    var cesta = await ctx.Autoceste.FindAsync(dionica.OznakaAutoceste);
                    cesta.DuljinaKm = newLen;
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = "Dionica je uspješno ažurirana.";
                    TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction(nameof(GetDionice), new { id = dionica.Id });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return PartialView(dionica);
                }
            }
            else
            {
                return PartialView(dionica);
            }
        }

        /// <summary>
        /// Vraca parcijalni pogled dionice za detail u master formi
        /// </summary>
        /// <param name="id">id dionice</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDionice(int id)
        {
            var dionica = await ctx.Dionice
                .Include(d => d.UlaznaPostajaNavigation)
                .Include(d => d.IzlaznaPostajaNavigation)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dionica == null)
            {
                return NotFound($"Neispravan id dionice: {id}");
            }

            
            return PartialView(dionica);
        }

        /// <summary>
        /// Brise stavku iz dionice iz master forme
        /// </summary>
        /// <param name="id">id dionice</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteDionica(int id)
        {
            ActionResponseMessage responseMessage;
            var dionica = await ctx.Dionice
                .Include(d => d.UlaznaPostajaNavigation)
                .Include(d => d.IzlaznaPostajaNavigation)
                .Include(d => d.OznakaAutocesteNavigation)
                .FirstOrDefaultAsync(d => d.Id == id);

            var autocesta = dionica.OznakaAutocesteNavigation;
            var red = dionica.DuljinaKm;
            autocesta.DuljinaKm -= red;

            if (dionica == null)
            {
                return NotFound($"Neispravan id dionice: {id}");
            }

            try
            {
                string naziv = Converters.GetDionicaName(dionica);
                ctx.Remove(dionica);
                await ctx.SaveChangesAsync();
                responseMessage = new ActionResponseMessage(MessageType.Success, $"Dionica {naziv} sa id-om {id} je uspješno obrisana.");
            }
            catch (Exception exc)
            {
                responseMessage = new ActionResponseMessage(MessageType.Error, $"Pogreška prilikom brisanja dionice: {exc.CompleteExceptionMessage()}");
            }

            Response.Headers["HX-Trigger"] = JsonSerializer.Serialize(new { showMessage = responseMessage });
            return responseMessage.MessageType == MessageType.Success ?
              new EmptyResult() : await GetDionice(id);
        }

        /// <summary>
        /// Stvara novu stavku dionice u master formi
        /// </summary>
        /// <param name="dionica">Dionica koja se stvara</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateDionica(Dionice dionica)
        {
            if (dionica == null)
            {
                return NotFound("Neispravan zatjev.");
            }

            var autocesta = await ctx.Autoceste
                .Include(a => a.Dionice)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Oznaka == dionica.OznakaAutoceste);
            int newLen = autocesta.DuljinaKm + dionica.DuljinaKm;

            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(dionica);
                    await ctx.SaveChangesAsync();

                    var a = await ctx.Autoceste.FindAsync(autocesta.Oznaka);
                    a.DuljinaKm = newLen;
                    await ctx.SaveChangesAsync();

                    return RedirectToAction(nameof(GetDionice), new { id = dionica.Id });
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
                            return await CreateDionica(dionica);
                        }
                    }

                    logger.LogError("Pogreška prilikom dodavanja dionice.", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return PartialView(dionica);
                }
            }
            else
            {
                return PartialView(dionica);
            }
        }
    }
}