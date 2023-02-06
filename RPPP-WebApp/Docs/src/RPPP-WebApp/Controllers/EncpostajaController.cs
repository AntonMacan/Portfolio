using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.Extensions;

using RPPP_WebApp.Models;
using RPPP_WebApp.ViewModels;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Features;
using static System.Net.Mime.MediaTypeNames;

namespace RPPP_WebApp.Controllers
{

    /// <summary>
    /// Kontroler zadužen za ENC postaje
    /// </summary>
    public class EncpostajaController : Controller
    {
        private readonly RPPP05Context ctx;
        private readonly ILogger<EncpostajaController> logger;
        private readonly AppSettings appSettings;

        public EncpostajaController(RPPP05Context ctx, ILogger<EncpostajaController> logger, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.appSettings = options.Value;
        }


        /// <summary>
        /// Priprema podatak za prikaz ENC naplatnig postaja
        /// </summary>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Encpostaja.AsNoTracking();
            int count = await query.CountAsync();

            if (count == 0)
            {
                logger.LogInformation("Ne postoji nijedna ENC postaja");
                TempData[Constants.Message] = "Ne postoji niti jedna ENC postaja.";
                TempData[Constants.ErrorOccurred] = false;
                //return RedirectToAction(nameof(Create));
            }

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

            query = query.ApplySort(sort, ascending);

            var ENCPostaje = query
                                  .Select(enc => new EncpostajaViewModel
                                  {
                                      Encid = enc.Encid,
                                      Ime = enc.Ime,
                                      KontaktBroj = enc.KontaktBroj,
                                      Naplatna = enc.Naplatna.Ime,
                                      VrijemeOtvaranja = enc.VrijemeOtvaranja,
                                      VrijemeZatvaranja = enc.VrijemeZatvaranja,
                                      NaplatnaStaza = enc.NaplatnaStaza,

                                      
                                  })
                                  .Skip((page - 1) * pagesize)
                                  .Take(pagesize)
                                  .ToList();



            


            var model = new EncpostajeViewModel
            {
                Encpostaje = ENCPostaje,
                PagingInfo = pagingInfo
            };

            return View(model);
        }


        /// <summary>
        /// Pogled za fromu dodavanja nove ENC postaje
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Encpostaja model = new Encpostaja
            {
                Naplatna = null
            };

            return View(model);
        }


        /// <summary>
        /// Obavlja provjeru i dodavanje modela ENC postaje u bazu
        /// </summary>
        /// <param name="postaja">model ENC postaje iz forme</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Encpostaja postaja)
        {
            logger.LogTrace(JsonSerializer.Serialize(postaja));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(postaja);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"ENC postaja {postaja.Ime} dodana. Id ENC postaje = {postaja.NaplatnaId}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje nove ENC postaje: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownList();

                    return View(postaja);
                }
            }
            else
            {
                await PrepareDropDownList();
                return View(postaja);
            }
        }




        /// <summary>
        /// Prikaz forme za ažuriranje podataka ENC postaje
        /// </summary>
        /// <param name="id">identifikator postaje</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            
            var postaja = await ctx.Encpostaja.AsNoTracking().Where(enc => enc.Encid == id).SingleOrDefaultAsync();
            if (postaja == null)
            {
                logger.LogWarning("Ne postoji ENC postaja s oznakom: {0}", id);
                return NotFound("Ne postoji ENC postaja s oznakom: " + id);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownList();
                return View(postaja);
            }
        }


        /// <summary>
        /// Provjerava podatke i sprema ažuriranu ENC postaju u bazu
        /// </summary>
        /// <param name="postaja">model ažurirane postaje</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Encpostaja postaja, int page = 1, int sort = 1, bool ascending = true)
        {
            
            if (postaja == null)
            {
                
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.Encpostaja.AnyAsync(enc => enc.Encid == postaja.Encid);
            if (!checkId)
            {
                
                return NotFound($"Neispravan id ENC postaje: {postaja?.Encid}");
            }

            if (ModelState.IsValid)
            {


               
                try
                {
                    ctx.Update(postaja);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "ENC postaja ažurirana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownList();
                    return View(postaja);
                }
            }
            else
            {
                
                await PrepareDropDownList();
                return View(postaja);
            }
        }


        /// <summary>
        /// Obavlja brisanje postaje iz baze
        /// </summary>
        /// <param name="id">identifikator postaje</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var postaja = await ctx.Encpostaja.FindAsync(id);
            if (postaja != null)
            {
                try
                {
                    string ime = postaja.Ime;
                    ctx.Remove(postaja);
                    await ctx.SaveChangesAsync();
                    logger.LogInformation($"ENC postaja {ime} uspješno obrisana");
                    TempData[Constants.Message] = $"ENC postaja {ime} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja ENC postaje: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                    logger.LogError("Pogreška prilikom brisanja ENC postaje: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning("Ne postoji ENC postaja s oznakom: {0} ", id);
                TempData[Constants.Message] = "Ne postoji ENC postaja s oznakom: " + id;
                TempData[Constants.ErrorOccurred] = true;
            }

            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }


        /// <summary>
        /// Priprema padajuću listu za naplatne postaje
        /// Ne koristi se jer se koristi autocomplete
        /// </summary>
        /// <returns></returns>
        private async Task PrepareDropDownList()
        {          
            List<SelectListItem> naplatne = await ctx.NaplatnaPostaja
        
                              .Select(np => new SelectListItem
                              {
                                  
                                     Value = np.NaplatnaId.ToString(),
                                     Text = np.Ime
                                  
                                  
                              })
                             .ToListAsync();
                    
            ViewBag.Naplatne = naplatne;
        }

    }
}
