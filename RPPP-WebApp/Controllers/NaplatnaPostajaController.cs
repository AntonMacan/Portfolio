using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

//using RPPP_WebApp.ModelsPartials;
using RPPP_WebApp.Models;

using RPPP_WebApp.ViewModels;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.Extensions;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using NLog.Filters;
using Microsoft.CodeAnalysis.Differencing;

namespace RPPP_WebApp.Controllers
{

    /// <summary>
    /// Kontroler zadužen za naplatne postaje
    /// </summary>
    public class NaplatnaPostajaController : Controller
    {
        private readonly RPPP05Context ctx;
        private readonly ILogger<NaplatnaPostajaController> logger;
        private readonly AppSettings appSettings;

        public NaplatnaPostajaController(RPPP05Context ctx, ILogger<NaplatnaPostajaController> logger, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.appSettings = options.Value;
        }

        /// <summary>
        /// Priprema i prikazuje stranicu s tablicom naplatnih postaja
        /// </summary>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.NaplatnePostajeENC.AsNoTracking();
            int count = await query.CountAsync();

            if (count == 0)
            {
                logger.LogInformation("Ne postoji nijedna naplatna postaja");
                TempData[Constants.Message] = "Ne postoji niti jedna naplatna postaja.";
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

            if(page < 1 || page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = 1, sort, ascending });
            }

            query = query.ApplySort(sort, ascending);



            var naplatnePostaje = await query
   
                                  .Select(np => new NaplatnaPostajaViewModel
                                  {
                                      NaplatnaId = np.NaplatnaId,
                                      Ime = np.Ime,
                                      GeoDuzina = np.GeoDuzina,
                                      GeoSirina = np.GeoSirina,
                                      KontaktBroj = np.KontaktBroj,
                                      naplatnaStaza = np.naplatnaStaza,
                                      VrijemeOtvaranja = np.VrijemeOtvaranja,
                                      VrijemeZatvaranja = np.VrijemeZatvaranja
                                      

                                  })
                                  .Skip((page - 1) * pagesize)
                                  .Take(pagesize)                                             
                                  .ToListAsync();

            int i = 0;
        foreach(var postaja in naplatnePostaje)
        {
               

                var izlazi = await ctx.Cjenik
                                .Include(c => c.Izlaz)                           
                                .Where(c => c.UlazId == postaja.NaplatnaId)
                                .Select(c => c.Izlaz)
                                .ToListAsync();

                postaja.IzlazneNaplatnePostaje = izlazi;
                postaja.Position = (page - 1) * pagesize + i;
                i++;
        }

                 
            var model = new NaplatnePostajeViewModel
            {
                NaplatnePostaje = naplatnePostaje,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        /// <summary>
        /// Ruta za pogled forme za dodavanj naplatne postaje
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


       /// <summary>
       /// Stvara novu postaju iz poslanog modela iz forme
       /// Sprema ju u bazu
       /// </summary>
       /// <param name="postaja">model postaje</param>
       /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NaplatnaPostaja postaja)
        {
            logger.LogTrace(JsonSerializer.Serialize(postaja));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(postaja);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Naplatna postaja {postaja.Ime} dodana. Id naplatne postaje = {postaja.NaplatnaId}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new {id = postaja.NaplatnaId });
                }
                catch(Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje nove naplatne postaje: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(postaja);
                }
            }
            else
            {
                return View(postaja);
            }
        }


        /// <summary>
        /// Prikazuje stranicu za dodavanje i ažuriranje stavki cjenika naplatne postaje
        /// </summary>
        /// <param name="id">identifikator naplatne postaje</param>
        /// <param name="position">pozicija trenutnog dokumenta u navigacijskoj listi</param>

        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpGet]
        public Task<IActionResult> Edit(int id, int? position, int page = 1, int sort = 1, bool ascending = true)
        {
            return Show(id, position, page, sort, ascending, viewName: nameof(Edit));
        }


        //[HttpGet]
        //public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        //{
        //    var postaja = await ctx.NaplatnaPostaja.AsNoTracking().Where(np => np.NaplatnaId == id).SingleOrDefaultAsync();
        //    if(postaja == null)
        //    {
        //        logger.LogWarning("Ne postoji naplatna postaja s oznakom: {0}", id);
        //        return NotFound("Ne postoji naplatna postaja s oznakom: " + id);
        //    }
        //    else
        //    {
        //        ViewBag.Page = page;
        //        ViewBag.Sort = sort;
        //        ViewBag.Ascending = ascending;
        //        return View(postaja);
        //    }
        //}


        /// <summary>
        /// Sprema promjene u bazu nastale u formi za ažuriranje i unos stavki cjenika nalpatne postaje
        /// </summary>
        /// <param name="model">model naplatne postaje i njezinih stavki cjenika</param>
        /// <param name="position">pozicija trenutnog dokumenta u navigacijskoj listi</param>

        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(NaplatnaPostajaDetailView model, int? position,int page = 1, int sort = 1, bool ascending = true)
        {
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
           
            ViewBag.Position = position;
            if (ModelState.IsValid)
            {
                var naplatna = await ctx.NaplatnaPostaja
                                        .Include(np => np.CjenikUlaz)
                                        .Where(np => np.NaplatnaId == model.NaplatnaID)
                                        .FirstOrDefaultAsync();
                if (naplatna == null)
                {
                    return NotFound("Ne postoji dokument s id-om: " + model.NaplatnaID);
                }

                if (position.HasValue)
                {
                    page = 1 + position.Value / appSettings.PageSize;
                    await SetPreviousAndNext(position.Value, sort, ascending);
                }

                naplatna.NaplatnaId = model.NaplatnaID;
                naplatna.GeoDuzina = model.GeoDuzina;
                naplatna.GeoSirina = model.GeoSirina;
                naplatna.Ime = model.Ime;


                if(model.StavkeCjenika != null)
                {
                    List<int> idStavki = model.StavkeCjenika
                                         .Where(s => s.IzlazID > 0)
                                         .Select(s => s.IzlazID)
                                         .ToList();
                    //izbaci sve koje su nisu više u modelu
                    ctx.RemoveRange(naplatna.CjenikUlaz.Where(s => !idStavki.Contains(s.IzlazId)));


                    foreach (var stavka in model.StavkeCjenika)
                    {
                        //ažuriraj postojeće i dodaj nove
                        Cjenik novaStavka; // potpuno nova ili dohvaćena ona koju treba izmijeniti
                        if (naplatna.CjenikUlaz.Any(c => c.IzlazId == stavka.IzlazID))
                        {
                            novaStavka = naplatna.CjenikUlaz.First(s => s.IzlazId == stavka.IzlazID);
                        }
                        else
                        {
                            novaStavka = new Cjenik();
                            naplatna.CjenikUlaz.Add(novaStavka);
                        }
                        novaStavka.IzlazId = stavka.IzlazID;
                        novaStavka.CijenaIa = stavka.cijenaIA;
                        novaStavka.CijenaI = stavka.cijenaI;
                        novaStavka.CijenaIi = stavka.cijenaII;
                        novaStavka.CijenaIii = stavka.cijenaIII;
                        novaStavka.CijenaIv = stavka.cijenaIV;
                    }
                } else
                {
                    ctx.RemoveRange(naplatna.CjenikUlaz.Where(s => s.UlazId == naplatna.NaplatnaId));
                }
               
                

                

               
                //eventualno umanji iznos za dodatni popust za kupca i sl... nešto što bi bilo poslovno pravilo
                try
                {

                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Naplatna postaja {naplatna.NaplatnaId} uspješno ažurirana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Edit), new
                    {
                        id = naplatna.NaplatnaId,
                        position,                    
                        page,
                        sort,
                        ascending
                    });

                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }


        //[HttpPost, ActionName("EditSlozeni")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true) 
        //{
        //    try
        //    {
        //        var postaja = await ctx.NaplatnaPostaja
        //                            .Where(np => np.NaplatnaId == id)
        //                            .FirstOrDefaultAsync();
        //        if(postaja == null)
        //        {
        //            return NotFound("Neispravna oznaka naplatne postaje: " + id);
        //        }

        //        if (await TryUpdateModelAsync<NaplatnaPostaja>(postaja, "",
        //            np => np.Ime, np => np.GeoDuzina, np => np.GeoSirina
        //         ))
        //        {

        //            ViewBag.Page = page;
        //            ViewBag.Sort = sort;
        //            ViewBag.Ascending = ascending;

        //            try
        //            {
        //                await ctx.SaveChangesAsync();
        //                TempData[Constants.Message] = "Naplatna postaja ažurirana.";
        //                TempData[Constants.ErrorOccurred] = false;
        //                return RedirectToAction(nameof(Index), new {page = page, sort = sort, ascending = ascending});
        //            }
        //            catch(Exception exc)
        //            {
        //                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
        //                return View(postaja);
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Page = page;
        //            ViewBag.Sort = sort;
        //            ViewBag.Ascending = ascending;
        //            ModelState.AddModelError(string.Empty, "Podatke o naplatnoj postaji nije moguće povezati s forme");
        //            return View(postaja);
        //        }
        //    }
        //    catch(Exception exc)
        //    {
        //        ViewBag.Page = page;
        //        ViewBag.Sort = sort;
        //        ViewBag.Ascending = ascending;
        //        TempData[Constants.Message] = exc.CompleteExceptionMessage();
        //        TempData[Constants.ErrorOccurred] = true;
        //        return RedirectToAction(nameof(EditSlozeni), id);
        //    }
        //}



        /// <summary>
        /// Obavlja birsanje naplatne postaje iz baze
        /// </summary>
        /// <param name="id">identifikator baze</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var postaja = await ctx.NaplatnaPostaja.FindAsync(id);
            if(postaja != null)
            {
                try
                {
                    string ime = postaja.Ime;
                    ctx.Remove(postaja);
                    await ctx.SaveChangesAsync();
                    logger.LogInformation($"Naplatna postaja {ime} uspješno obrisana");
                    TempData[Constants.Message] = $"Naplatan postaja {ime} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch(Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja naplatne postaje: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                    logger.LogError("Pogreška prilikom brisanja naplatne postaje: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning("Ne postoji naplatna postaja s oznakom: {0} ", id);
                TempData[Constants.Message] = "Ne postoji naplatna postaja s oznakom: " + id;
                TempData[Constants.ErrorOccurred] = true;
            }

            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }


        /// <summary>
        /// Priprema i prikazuje naplatnu postaju i njezine stavke te navigaciju izmežu nalpatnih postaja
        /// </summary>
        /// <param name="id">identifikator naplatne postaje</param>
        /// <param name="position">pozicija trenutnog dokumenta u navigacijskoj listi</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <param name="viewName">ime pogleda</param>
        /// <returns></returns>
        public async Task<IActionResult> Show(int id, int? position, int page = 1, int sort = 1, bool ascending = true, string viewName = nameof(Show))
        {
            var naplatna = await ctx.NaplatnaPostaja
                                    .Where(np => np.NaplatnaId == id)
                                    .Select(np => new NaplatnaPostajaDetailView
                                    {
                                        NaplatnaID = np.NaplatnaId,
                                        Ime = np.Ime,
                                        GeoDuzina = np.GeoDuzina,
                                        GeoSirina = np.GeoSirina
                                    })
                                    .FirstOrDefaultAsync();


            if (naplatna == null)
            {
                return NotFound($"Naplatna postaja {id} ne postoji");
            } else
            {
                var stavkeCjenika = await ctx.Cjenik
                                             .Where(c => c.UlazId == naplatna.NaplatnaID)
                                             .OrderBy(c => c.IzlazId)
                                             .Include(c => c.Izlaz)
                                             .Select(c => new StavkaCjenikaViewModel
                                             {
                                                 IzlazID = c.IzlazId,
                                                 Ime = c.Izlaz.Ime,
                                                 GeoDuzina = c.Izlaz.GeoDuzina,
                                                 GeoSirina = c.Izlaz.GeoSirina,
                                                 cijenaIA = c.CijenaIa,
                                                 cijenaI = c.CijenaI,
                                                 cijenaII = c.CijenaIi,
                                                 cijenaIII = c.CijenaIii,
                                                 cijenaIV = c.CijenaIv
                                             })
                                             .ToListAsync();

                naplatna.StavkeCjenika = stavkeCjenika;

                if (position.HasValue)
                {
                    page = 1 + position.Value / appSettings.PageSize;
                    await SetPreviousAndNext(position.Value, sort, ascending);
                }

                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;              
                ViewBag.Position = position;

                return View(viewName, naplatna);
            }
        }




      

        /// <summary>
        /// Računa pozicije naplatnih postaja u navigacijskoj listi
        /// </summary>
        /// <param name="position">pozicija u navigacijskoj listi</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        private async Task SetPreviousAndNext(int position, int sort, bool ascending)
        {
            var query = ctx.NaplatnePostajeENC.AsNoTracking();

            query = query.ApplySort(sort, ascending);
            if (position > 0)
            {
                ViewBag.Previous = await query.Skip(position - 1).Select(np => np.NaplatnaId).FirstAsync();
            }
            if (position < await query.CountAsync() - 1)
            {
                ViewBag.Next = await query.Skip(position + 1).Select(np => np.NaplatnaId).FirstAsync();
            }
        }



        
    }
}
