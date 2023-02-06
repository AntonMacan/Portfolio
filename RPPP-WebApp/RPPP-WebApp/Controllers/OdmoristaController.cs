using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPPP_WebApp.Models;
using RPPP_WebApp.ViewModels;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.UtilClasses;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using System.Drawing.Printing;
using RPPP_WebApp.Extensions;
using System.Text.Json;

namespace RPPP_WebApp.Controllers
{
    /// <summary>
    /// Servis za rad s odmoristima i sadrzajima odmorista
    /// </summary>
    public class OdmoristaController : Controller
    {
        private readonly RPPP05Context context;
        private readonly AppSettings appSettings;

        public OdmoristaController(RPPP05Context context, IOptionsSnapshot<AppSettings> options)
        {
            this.context = context;
            this.appSettings = options.Value;
        }

        /// <summary>
        /// Vraća stranicu sa listom svih odmorista
        /// </summary>
        /// <param name="page">Broj željene stranice</param>
        /// <param name="sort">Index stupca po kojem želi se sortirat</param>
        /// <param name="ascending">Je li uzlazno ili silazno sortiranje?</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            try
            {
                var query = context.Odmoriste
                    .Include(o => o.Dionica)
                    .ThenInclude(d => d.UlaznaPostajaNavigation)
                    .Include(o => o.Dionica)
                    .ThenInclude(d => d.IzlaznaPostajaNavigation)
                    .Include(o => o.Sadrzaj)
                    .ThenInclude(s => s.TipSadrzaja)
                    .AsNoTracking();
                int count = await query.CountAsync();

                if (count == 0)
                {
                    TempData[Constants.Message] = "Ne postoji niti jedna naplatna postaja.";
                    TempData[Constants.ErrorOccurred] = false;
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

                var odmorista = query
                                      .Skip((page - 1) * pagesize)
                                      .Take(pagesize)
                                      .ToList();

                List<OdmoristeViewModel> odmoristeViewModels = new List<OdmoristeViewModel>();
                foreach (var odmoriste in odmorista)
                {
                    odmoristeViewModels.Add(
                        new OdmoristeViewModel
                        {
                            Id = odmoriste.Id,
                            Naziv = odmoriste.Naziv,
                            Opis = odmoriste.Opis,
                            Smjer = odmoriste.Smjer,
                            NadmorskaVisina = odmoriste.NadmorskaVisina,
                            Stacionaza = odmoriste.StacionazaKm + "+" + odmoriste.StacionazaM + " km",
                            GeografskaDuzina = odmoriste.GeografskaDuzina,
                            GeografskaSirina = odmoriste.GeografskaSirina,
                            DionicaNaziv = Converters.GetDionicaName(odmoriste.Dionica),
                            Sadrzaj = odmoriste.Sadrzaj.ToList()
                        }
                        );
                }

                var model = new OdmoristaViewModel
                {
                    Odmorista = odmoristeViewModels,
                    PagingInfo = pagingInfo
                };

                return View(model);
            }
            catch (SqlException)
            {
                TempData[Constants.Message] = $"Greška pri spajanju na bazu.";
                TempData[Constants.ErrorOccurred] = true;
                return View(new OdmoristaViewModel
                {
                    Odmorista = new List<OdmoristeViewModel>(),
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        Sort = sort,
                        Ascending = ascending,
                        ItemsPerPage = pagesize,
                        TotalItems = 0
                    }
                });
            }
        }

        /// <summary>
        /// Vraća stranicu sa detaljima odabranog odmorista,
        /// listom sadrzaja odmorista, i listom poveznica na multimedijske sadrzaje vezane uz 
        /// odabrano odmoriste
        /// </summary>
        /// <param name="id">Id odmorista koje ce se prikazati</param>
        /// <param name="page">Broj željene stranice</param>
        /// <param name="sort">Index stupca po kojem želi se sortirat</param>
        /// <param name="ascending">Je li uzlazno ili silazno sortiranje?</param>
        /// <returns></returns>
        public async Task<IActionResult> Details(int? id, int page = 1, int sort = 1, bool ascending = true)
        {
            if (id == null || context.Odmoriste == null)
            {
                return NotFound();
            }

            var odmoriste = await context.Odmoriste
                .Include(o => o.Dionica)
                .ThenInclude(d => d.UlaznaPostajaNavigation)
                .Include(o => o.Dionica)
                .ThenInclude(d => d.IzlaznaPostajaNavigation)
                .Include(o => o.Multimedija)
                .Include(o => o.Sadrzaj)
                .ThenInclude(s => s.TipSadrzaja)
                .Include(o => o.Sadrzaj)
                .ThenInclude(s => s.RadnoVrijeme)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (odmoriste == null)
            {
                return NotFound();
            }

            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;

            var multimedija = odmoriste.Multimedija.ToList();
            var sadrzaj = odmoriste.Sadrzaj.ToList();

            foreach (var item in sadrzaj)
            {
                item.RadnoVrijeme.OrderBy(s => s.Dan);
            }

            var odmoristeViewModel = new OdmoristeViewModel
            {
                Id = odmoriste.Id,
                Naziv = odmoriste.Naziv,
                Opis = odmoriste.Opis,
                Smjer = odmoriste.Smjer,
                NadmorskaVisina = odmoriste.NadmorskaVisina,
                Stacionaza = odmoriste.StacionazaKm + "+" + odmoriste.StacionazaM + " km",
                GeografskaDuzina = odmoriste.GeografskaDuzina,
                GeografskaSirina = odmoriste.GeografskaSirina,
                DionicaNaziv = Converters.GetDionicaName(odmoriste.Dionica),
                Multimedija = multimedija,
                Sadrzaj = sadrzaj
            };

            return View(odmoristeViewModel);
        }

        /// <summary>
        /// Vraca stranicu za unos novog odmorista
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            return View();
        }

        /// <summary>
        /// Obraduje post zahtjev i dodaje novo odmoriste u bazu
        /// </summary>
        /// <param name="odmoriste"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Opis,Smjer,NadmorskaVisina,StacionazaKm,StacionazaM,GeografskaSirina,GeografskaDuzina,DionicaId")] Odmoriste odmoriste)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    context.Add(odmoriste);
                    await context.SaveChangesAsync();
                    TempData[Constants.Message] = $"Odmoriste {odmoriste.Naziv} dodano. Id odmorista = {odmoriste.Id}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData[Constants.Message] = $"Greška pri spremanju u bazu. Odmorište nije dodano.";
                    TempData[Constants.ErrorOccurred] = true;
                    return RedirectToAction(nameof(Index));

                }
            }
            await PrepareDropDownLists();
            return View(odmoriste);
        }

        // <summary>
        /// Vraća formu za ažuriranje odmorista
        /// </summary>
        /// <param name="id">Id odmorista</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, int page = 1, int sort = 1, bool ascending = true)
        {
            if (id == null || context.Odmoriste == null)
            {
                return NotFound();
            }

            var odmoriste = await context.Odmoriste
                .Where(o => o.Id == id)
                .Include(o => o.Dionica)
                .ThenInclude(d => d.UlaznaPostajaNavigation)
                .Include(o => o.Dionica)
                .ThenInclude(d => d.IzlaznaPostajaNavigation)
                .Include(o => o.Sadrzaj)
                .ThenInclude(s => s.TipSadrzaja)
                .Include(o => o.Sadrzaj)
                .ThenInclude(s => s.RadnoVrijeme)
                .FirstOrDefaultAsync();

            if (odmoriste == null)
            {
                return NotFound($"Ne postoji odmoriste s id = {id}");
            }
         
            
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;

            await PrepareDropDownLists();
            return View(odmoriste);
        }

        /// <summary>
        /// Obraduje post zahtjev za azuriranje odmorista
        /// </summary>
        /// <param name="id">Id odmorista</param>
        /// <returns></returns>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,
            [Bind("Id,Naziv,Opis,Smjer,NadmorskaVisina,StacionazaKm,StacionazaM,GeografskaSirina,GeografskaDuzina,DionicaId")] Odmoriste odmoriste,
            int page = 1, int sort = 1, bool ascending = true
            )
        {
            if (id != odmoriste.Id)
            {
                return NotFound();
            }

            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(odmoriste);
                    await context.SaveChangesAsync();
                    TempData[Constants.Message] = "Odmorište ažurirano.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });

                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData[Constants.Message] = $"Greška pri ažuriranju. Odmorište nije dodano.";
                    TempData[Constants.ErrorOccurred] = true;
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException)
                {
                    TempData[Constants.Message] = $"Greška pri ažuriranju. Odmorište nije dodano.";
                    TempData[Constants.ErrorOccurred] = true;
                    return RedirectToAction(nameof(Index));
                }
            }

            //nije proslo validaciju
            await PrepareDropDownLists();
            return View(odmoriste);
        }

        /// <summary>
        /// Brisanje odmorista određenog s id
        /// </summary>
        /// <param name="ascending"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var odmoriste = await context.Odmoriste.FindAsync(id);
            if (odmoriste != null)
            {
                try
                {
                    string ime = odmoriste.Naziv;
                    context.Remove(odmoriste);
                    await context.SaveChangesAsync();
                    
                    TempData[Constants.Message] = $"Odmorište {ime} uspješno obrisano";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja odmorista.";
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji odmoriste s id = {id}";
                TempData[Constants.ErrorOccurred] = true;
            }

            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }
        private bool OdmoristeExists(int id)
        {
          return context.Odmoriste.Any(e => e.Id == id);
        }

        private async Task PrepareDropDownLists()
        {

            var listaDionica = await context.Dionice
                                   .Select(d => new
                                   {
                                       Ime = Converters.GetDionicaName(d.UlaznaPostajaNavigation.Ime,
                                                 d.IzlaznaPostajaNavigation.Ime,
                                                 d.OznakaAutoceste),
                                       DionicaId = d.Id


                                   })
                                   .ToListAsync();

            var listaTipovaSadrzaja = await context.TipSadrzaja.ToListAsync();


            ViewBag.Dionice = new SelectList(listaDionica, "DionicaId", "Ime");
            ViewBag.TipoviSadrzaja = new SelectList(listaTipovaSadrzaja, "Id", "Naziv");
        }

        #region Dinamicko azuriranje sadrzaja

        /// <summary>
        /// Vraca parcijalni pogled za uredivanje sadrzaja odmorista
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditSadrzaj(int id)
        {
            var sadrzaj = await context.Sadrzaj
                .Include(s => s.RadnoVrijeme)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sadrzaj == null)
            {
                return NotFound($"Neispravan id sadrzaja odmorista: {id}");
            }

            await PrepareDropDownLists();
            return PartialView(sadrzaj);
        }

        /// <summary>
        /// Obraduje post zahtjev za uredivanje sadrzaja odmorista
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditSadrzaj(Sadrzaj sadrzaj)
        {
            if (sadrzaj == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            
            bool checkId = await context.Sadrzaj.AnyAsync(s => s.Id == sadrzaj.Id);

            if (!checkId)
            {
                return NotFound($"Neispravan id sadrzaja: {sadrzaj.Id}");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(sadrzaj);
                    await context.SaveChangesAsync();

                    //TempData[Constants.Message] = "Sadržaj uspješno ažuriran.";
                    //TempData[Constants.ErrorOccurred] = false;

                    return RedirectToAction(nameof(GetSadrzaj), new { id = sadrzaj.Id });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return PartialView(sadrzaj);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return PartialView(sadrzaj);
            }
        }

        /// <summary>
        /// Vraca parcijalni pogled za dodavanje novog sadrzaja odmorista
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CreateSadrzaj(int id)
        {
            return PartialView(new Sadrzaj { OdmoristeId = id });
        }

        /// <summary>
        /// Obraduje post zahtjev za dodavanje novog sadrzaja odmorista
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSadrzaj(Sadrzaj sadrzaj)
        {

            if (sadrzaj == null)
            {
                return NotFound("Nema poslanih podataka");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Add(sadrzaj);
                    await context.SaveChangesAsync();

                    return RedirectToAction(nameof(GetSadrzaj), new { id = sadrzaj.Id });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return PartialView(sadrzaj);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return PartialView(sadrzaj);
            }
        }

        /// <summary>
        /// Vraca parcijalni pogled za prikaz podataka o sadrzaju odmorista
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSadrzaj(int id)
        {
            var sadrzaj = await context.Sadrzaj
                .Include(s => s.RadnoVrijeme)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sadrzaj == null)
            {
                return NotFound($"Neispravan id sadrzaja odmorista: {id}");
            }

            await PrepareDropDownLists();
            return PartialView(sadrzaj);
        }

        /// <summary>
        /// Obraduje zahtjev za brisanje sadrzaja sodredenog s id
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteSadrzaj(int id)
        {
            ActionResponseMessage responseMessage;
            var sadrzaj = await context.Sadrzaj.FindAsync(id);

            if (sadrzaj == null)
            {
                return NotFound($"Neispravan id sadrzaja odmorista: {id}");
            }

            try
            {
                string naziv = sadrzaj.Naziv;
                context.Remove(sadrzaj);
                await context.SaveChangesAsync();
                responseMessage = new ActionResponseMessage(MessageType.Success, $"Sadržaj {naziv} sa šifrom {id} uspješno obrisan.");
            }
            catch (Exception exc)
            {
                responseMessage = new ActionResponseMessage(MessageType.Error, $"Pogreška prilikom brisanja sadrzaja: {exc.CompleteExceptionMessage()}");
            }

            Response.Headers["HX-Trigger"] = JsonSerializer.Serialize(new { showMessage = responseMessage });
            return responseMessage.MessageType == MessageType.Success ?
              new EmptyResult() : await GetSadrzaj(id);            
        }

        #endregion
    }
}
