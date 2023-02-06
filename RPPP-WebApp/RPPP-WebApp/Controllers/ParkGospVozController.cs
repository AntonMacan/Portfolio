using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RPPP_WebApp.Extensions;
using RPPP_WebApp.Extensions.Selectors;
using RPPP_WebApp.ViewModels;
using RPPP_WebApp.Models;
using RPPP_WebApp.UtilClasses;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace RPPP_WebApp.Controllers
{

    /// <summary>
    /// Kontroler za poglede vezane za parkirališta gospodarskih vozila
    /// </summary>
    public class ParkGospVozController : Controller
    {
        private readonly RPPP05Context ctx;
        private readonly AppSettings appData;


        public ParkGospVozController(RPPP05Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            this.appData = options.Value;
        }


        /// <summary>
        /// Priprema i prikazuje pogled tablice parkirališta gospodarskih vozila
        /// </summary>
        /// <param name="page">stranica</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns>Pogled tablice parkirališta gospodarskih vozila</returns>
        public async Task<IActionResult> Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appData.PageSize;

            var query = ctx.ParkGospVoz.AsNoTracking();
            int count = await query.CountAsync();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                Sort = sort,
                Ascending = ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if(count == 0)
            {
                return RedirectToAction(nameof(Create));
            }

            if(page < 1 || page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new {page = 1, sort = sort, ascending = ascending});
            }

            query = query.ApplySort(sort, ascending);

            var parkiralista = await query
                                    .Select(p => new ParkiralisteGospVozViewModel
                                    {
                                        ParkingId = p.ParkingId,
                                        Stacionaža = p.Stacionaža,
                                        Naziv = p.Naziv,
                                        Dionica = Converters.GetDionicaName(p.Dionica.UlaznaPostajaNavigation.Ime,
                                                 p.Dionica.IzlaznaPostajaNavigation.Ime,
                                                 p.Dionica.OznakaAutoceste),

                                        GeoDuzinaUlaz = p.GeoDuzinaUlaz,
                                        GeoSirinaUlaz = p.GeoSirinaUlaz,
                                        BrojMjesta = p.BrojMjesta,
                                        CijenaPoSatu = p.CijenaPoSatu,
                                        StranaCesteUlaz = p.StranaCesteUlaz,


                                    })
                                    .Skip((page - 1) * pagesize)
                                    .Take(pagesize)
                                    .ToListAsync();


            var model = new ParkiralistaGospVozViewModel
            {
                parkiralista = parkiralista,
                PagingInfo = pagingInfo
            };

            return View(model);
        }

        /// <summary>
        /// Vraća pogled na formu za dodavanje novog parkirališta
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PrepareDropDownLists();
            ParkGospVoz model = new ParkGospVoz
            {
                Dionica = null
            };
            return View(model);
        }


        /// <summary>
        /// Obrada POST zahtjeva na formom za dodavanje novog parkirališta
        /// </summary>
        /// <param name="parkiraliste">Model parkirališta koji se obrađuje</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParkGospVoz parkiraliste)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(parkiraliste);
                    await ctx.SaveChangesAsync();

                    TempData[Constants.Message] = $"Parkiralište {parkiraliste.Naziv} dodano. Id parkiralista {parkiraliste.ParkingId}";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                } catch(Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(parkiraliste);
                }
            }
            else
            {
                await PrepareDropDownLists();
                return View(parkiraliste);
            }
        }


        /// <summary>
        /// Obrađuje POST zahtjev brisanja parkirališta 
        /// </summary>
        /// <param name="id">identifikator parkirališta</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var parkiraliste = await ctx.ParkGospVoz.FindAsync(id);
            if (parkiraliste != null)
            {
                try
                {
                    string naziv = parkiraliste.Naziv;
                    ctx.Remove(parkiraliste);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = $"Parkiralište {naziv} sa šifrom {id} obrisano.";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja mjesta: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
            }
            else
            {
                TempData[Constants.Message] = $"Ne postoji parkiralište sa šifrom: {id}";
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page, sort, ascending });
        }

        /// <summary>
        /// Priprema i prikazuje stranicu za ažuriranje parkirališta
        /// </summary>
        /// <param name="id">identifikator parkirališta</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var parkiraliste = await ctx.ParkGospVoz
                                  .AsNoTracking()
                                  .Where(p => p.ParkingId == id)
                                  .SingleOrDefaultAsync();
            if (parkiraliste != null)
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                await PrepareDropDownLists();
                return View(parkiraliste);
            }
            else
            {
                return NotFound($"Neispravan id parkirališta: {id}");
            }
        }


        /// <summary>
        /// Obrađuje POST zahtjev forme za ažuriranje parkirališta
        /// </summary>
        /// <param name="parkiraliste">model parkirališta koje se ažurira</param>
        /// <param name="page">stranica tablice</param>
        /// <param name="sort">stupac sortiranja</param>
        /// <param name="ascending">način sortiranja</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ParkGospVoz parkiraliste, int page = 1, int sort = 1, bool ascending = true)
        {
            if (parkiraliste == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = await ctx.ParkGospVoz.AnyAsync(p => p.ParkingId == parkiraliste.ParkingId);
            if (!checkId)
            {
                return NotFound($"Neispravan id parkirališta: {parkiraliste?.ParkingId}");
            }

            if (ModelState.IsValid)
            {

               
                try
                {
                    ctx.Update(parkiraliste);
                    await ctx.SaveChangesAsync();
                    TempData[Constants.Message] = "Parkiralište ažurirano.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    await PrepareDropDownLists();
                    return View(parkiraliste);
                }
            }
            else
            {
                
                await PrepareDropDownLists();
                return View(parkiraliste);
            }
        }

        
        /// <summary>
        /// Priprema dropdown listu za strane ceste izlaza i stavlja ju u ViewBag
        /// </summary>
        /// <returns></returns>
        private async Task PrepareDropDownLists()
        {

            //var dionice = await ctx.Dionice
            //                       .Select(d => new
            //                       {
            //                           Ime = Converters.GetDionicaName(d.UlaznaPostajaNavigation.Ime,
            //                                     d.IzlaznaPostajaNavigation.Ime,
            //                                     d.OznakaAutoceste),
            //                           DionicaId = d.Id


            //                       })
            //                       .ToListAsync();


            //ViewBag.Dionice = new SelectList(dionice, "DionicaId", "Ime");


            var stranaCeste = new List<string>() { "L", "D", "L/D" };
            ViewBag.StranaCeste = new SelectList(stranaCeste);     
        }

        

    }
}
