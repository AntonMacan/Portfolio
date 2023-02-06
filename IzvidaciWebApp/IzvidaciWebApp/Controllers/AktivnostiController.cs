using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers
{
    public class AktivnostiController : Controller
    {
        private readonly IAktivnostProvider _aktivnostProvider;
        public AktivnostiController(IAktivnostProvider aktivnostProvider)
        {
            _aktivnostProvider = aktivnostProvider;
        }
        public async Task<IActionResult> Index()
        {
            var result = _aktivnostProvider.GetAll();
            AktivnostiViewModel akt = new AktivnostiViewModel();
            akt.aktivnosti = result.Result.Data.Select(r => new AktivnostViewModel
            {
                IdAktivnost = r.Id,
                MjestoPbr = r.MjestoPbr,
                KontaktOsoba = r.KontaktOsoba,
                Opis = r.Opis,
                AkcijaId = r.AkcijaId
            });
            return View(akt);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AktivnostViewModel aktivnost)
        {
            if (aktivnost is not null)
            {
                Aktivnost akt = new Aktivnost(aktivnost.IdAktivnost, aktivnost.MjestoPbr, aktivnost.KontaktOsoba, aktivnost.Opis, aktivnost.AkcijaId);

                var result = await _aktivnostProvider.Create(akt);
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
            var result = await _aktivnostProvider.Delete(id);
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
            var aktivnost = await _aktivnostProvider.Get(id);
            var akt = new AktivnostViewModel()
            {
                IdAktivnost = id,
                MjestoPbr = aktivnost.Data.MjestoPbr,
                KontaktOsoba = aktivnost.Data.KontaktOsoba,
                Opis = aktivnost.Data.Opis,
                AkcijaId = aktivnost.Data.AkcijaId

            };
            return View(akt);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AktivnostViewModel aktivnost)
        {
            Aktivnost akt = new Aktivnost(aktivnost.IdAktivnost, aktivnost.MjestoPbr, aktivnost.KontaktOsoba, aktivnost.Opis, aktivnost.AkcijaId);
            var result = await _aktivnostProvider.Edit(aktivnost.IdAktivnost, akt);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
