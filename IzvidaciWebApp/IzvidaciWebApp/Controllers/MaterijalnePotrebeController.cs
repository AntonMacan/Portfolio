using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers
{
    public class MaterijalnePotrebeController : Controller
    {
        private readonly IMaterijalnaPotrebaProvider mjestoProvider;
        public MaterijalnePotrebeController(IMaterijalnaPotrebaProvider mjestoProvider)
        {
            this.mjestoProvider = mjestoProvider;
        }
        public async Task<IActionResult> Index()
        {
            var result = mjestoProvider.GetAll();
            MaterijalnePotrebeViewModel akt = new MaterijalnePotrebeViewModel();
            akt.mjesta = result.Result.Data.Select(r => {
                return new MaterijalnaPotrebaViewModel
                {
                    IdMaterijalnePotrebe = r.IdMaterijalnaPotreba,
                    Naziv = r.Naziv,
                    Organizator = r.Organizator,
                    Davatelj = r.Davatelj,
                    Zadovoljeno = r.Zadovoljeno
                };
            });
            return View(akt);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MaterijalnaPotrebaViewModel mjesto)
        {
            if (mjesto is not null)
            {
                MaterijalnaPotreba mj = new MaterijalnaPotreba(mjesto.IdMaterijalnePotrebe, mjesto.Naziv, mjesto.Organizator, mjesto.Davatelj, mjesto.Zadovoljeno);

                var result = await mjestoProvider.Create(mj);
                if (!result.IsSuccess)
                {
                    Console.Out.WriteLine("Neuspjesno!");
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await mjestoProvider.Delete(id);
            if (!result.IsSuccess)
            {
                Console.WriteLine("Not Succesful!");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Console.Write("dsadskda\n");
            var mjesto = await mjestoProvider.Get(id);
            var akt = new MaterijalnaPotrebaViewModel()
            {
                IdMaterijalnePotrebe = id,
                Naziv = mjesto.Data.Naziv,
                Organizator = mjesto.Data.Organizator,
                Davatelj = mjesto.Data.Davatelj,
                Zadovoljeno = mjesto.Data.Zadovoljeno

            };
            return View(akt);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MaterijalnaPotrebaViewModel mjesto)
        {
            MaterijalnaPotreba akt = new MaterijalnaPotreba(mjesto.IdMaterijalnePotrebe, mjesto.Naziv, mjesto.Organizator, mjesto.Davatelj, mjesto.Zadovoljeno);
            var result = await mjestoProvider.Edit(mjesto.IdMaterijalnePotrebe, akt);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
