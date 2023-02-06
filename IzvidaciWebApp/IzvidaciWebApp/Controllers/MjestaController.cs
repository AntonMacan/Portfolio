using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers
{
    public class MjestaController : Controller
    {
        private readonly IMjestoProvider mjestoProvider;
        public MjestaController(IMjestoProvider mjestoProvider)
        {
            this.mjestoProvider = mjestoProvider;
        }
        public async Task<IActionResult> Index()
        {
            var result = mjestoProvider.GetAll();
            MjestaViewModel akt = new MjestaViewModel();
            akt.mjesta = result.Result.Data.Select(r => {
                return new MjestoViewModel
                {
                    Pbr = r.PbrMjesta,
                    NazivMjesta = r.NazivMjesta
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
        public async Task<IActionResult> Create(MjestoViewModel mjesto)
        {
            if (mjesto is not null)
            {
                Mjesto mj = new Mjesto(mjesto.Pbr, mjesto.NazivMjesta);

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
            var akt = new MjestoViewModel()
            {
                Pbr = id,
                NazivMjesta = mjesto.Data.NazivMjesta

            };
            return View(akt);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MjestoViewModel mjesto)
        {
            Mjesto akt = new Mjesto(mjesto.Pbr, mjesto.NazivMjesta);
            var result = await mjestoProvider.Edit(mjesto.Pbr, akt);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
