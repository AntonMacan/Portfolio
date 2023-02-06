using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers
{
    public class CvrstiObjektZaObitavanjeController : Controller
    {
        private readonly ICvrstiObjektZaObitavanjeProvider objektProvider;

        public CvrstiObjektZaObitavanjeController(ICvrstiObjektZaObitavanjeProvider objektProvider)
        {
            this.objektProvider = objektProvider;
        }

        public async Task<IActionResult> Index()
        {
            var result = objektProvider.GetAll();

            CvrstiObjektiZaObitavanjeViewModel akt = new CvrstiObjektiZaObitavanjeViewModel();

            akt.objekti = result.Result.Data.Select(o =>
            {
                return new CvrstiObjektZaObitavanjeViewModel
                {
                    IdTerenskaLokacija = o.IdTerenskaLokacija,
                    NazivTerenskaLokacija = o.NazivTerenskaLokacija,
                    ImaSanitarniCvor = o.ImaSanitarniCvor,
                    MjestoPbr = o.MjestoPbr,
                    Opis = o.Opis,
                    BrojPredvidenihSpavacihMjesta = o.BrojPredvidenihSpavacihMjesta
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
        public async Task<IActionResult> Create(CvrstiObjektZaObitavanjeViewModel objekt)
        {
            if(objekt is not null)
            {
                CvrstiObjektZaObitavanje obj = new CvrstiObjektZaObitavanje(objekt.IdTerenskaLokacija, objekt.NazivTerenskaLokacija, objekt.Slika, objekt.ImaSanitarniCvor, objekt.MjestoPbr, objekt.Opis, objekt.BrojPredvidenihSpavacihMjesta);

                var result = await objektProvider.Create(obj);
                if (!result.IsSuccess)
                {
                    Console.Out.WriteLine("Neuspjesno");
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await objektProvider.Delete(id);

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var objekt = await objektProvider.Get(id);
            var akt = new CvrstiObjektZaObitavanjeViewModel()
            {
                IdTerenskaLokacija = id,
                NazivTerenskaLokacija = objekt.Data.NazivTerenskaLokacija,
                ImaSanitarniCvor = objekt.Data.ImaSanitarniCvor,
                MjestoPbr = objekt.Data.MjestoPbr,
                Opis = objekt.Data.Opis,
                BrojPredvidenihSpavacihMjesta = objekt.Data.BrojPredvidenihSpavacihMjesta
            };

            return View(akt);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CvrstiObjektZaObitavanjeViewModel objekt)
        {
            CvrstiObjektZaObitavanje akt = new CvrstiObjektZaObitavanje(objekt.IdTerenskaLokacija, objekt.NazivTerenskaLokacija, objekt.Slika, objekt.ImaSanitarniCvor, objekt.MjestoPbr, objekt.Opis, objekt.BrojPredvidenihSpavacihMjesta);

            var result = await objektProvider.Edit(objekt.IdTerenskaLokacija, akt);

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}