using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers
{
    public class AkcijeController : Controller
    {
        private readonly IAkcijaProvider _akcijatProvider;
        public AkcijeController(IAkcijaProvider akcijaProvider)
        {
            _akcijatProvider = akcijaProvider;
        }
        public async Task<IActionResult> Index()
        {
            var result = _akcijatProvider.GetAll();
            AkcijeViewModel akt = new AkcijeViewModel();
            akt.akcije = result.Result.Data.Select(r => new AkcijaViewModel
            {
                IdAkcije = r.IdAkcije,
                Naziv = r.Naziv,
                MjestoPbr = r.MjestoPbr,
                Organizator = r.Organizator,
                KontaktOsoba = r.KontaktOsoba,
                Vrsta = r.Vrsta
            });
            return View(akt);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AkcijaViewModel akcija)
        {
            if (akcija is not null)
            {
                Akcija akt = new Akcija(akcija.IdAkcije, akcija.Naziv, akcija.MjestoPbr, akcija.Organizator, akcija.KontaktOsoba, akcija.Vrsta);

                var result = await _akcijatProvider.Create(akt);
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
            var result = await _akcijatProvider.Delete(id);
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
            var r = await _akcijatProvider.Get(id);
            var akcija = new AkcijaViewModel()
            {
                IdAkcije = id,
                Naziv = r.Data.Naziv,
                MjestoPbr = r.Data.MjestoPbr,
                Organizator = r.Data.Organizator,
                KontaktOsoba = r.Data.KontaktOsoba,
                Vrsta = r.Data.Vrsta

            };
            return View(akcija);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AkcijaViewModel akcija)
        {
            Akcija akt = new Akcija(akcija.IdAkcije, akcija.Naziv, akcija.MjestoPbr, akcija.Organizator, akcija.KontaktOsoba, akcija.Vrsta);
            var result = await _akcijatProvider.Edit(akcija.IdAkcije, akt);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
