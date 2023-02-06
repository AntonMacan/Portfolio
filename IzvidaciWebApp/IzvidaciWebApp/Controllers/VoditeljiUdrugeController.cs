using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.Providers;
using IzvidaciWebApp.Providers.Http;
using IzvidaciWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IzvidaciWebApp.Controllers
{
    public class VoditeljiUdrugeController : Controller
    {
        private readonly IVoditeljiUdrugeProvider voditeljiUdrugeProvider;
        public VoditeljiUdrugeController(IVoditeljiUdrugeProvider voditeljiUdrugeProvider)
        {
            this.voditeljiUdrugeProvider = voditeljiUdrugeProvider;
        }
        public async Task<IActionResult> Index()
        {

            var result = await voditeljiUdrugeProvider.GetAll();
            VoditeljiUdrugeViewModel akt = new VoditeljiUdrugeViewModel();
            akt.voditeljiUdruge = result.Data.Select(r => {
                return new VoditeljUdrugeViewModel
                {
                    IdUdruge = r.IdUdruge,
                    IdClan = r.IdClan,
                    Pozicija = r.Pozicija,
                    NaPozicijiDo = r.NaPozicijiDo
                };
            });
            return View(akt);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoditeljUdrugeViewModel voditeljUdruge)
        {
            if (voditeljUdruge is not null)
            {
                VoditeljiUdruge vu = new VoditeljiUdruge(voditeljUdruge.IdUdruge, voditeljUdruge.IdClan, voditeljUdruge.Pozicija, voditeljUdruge.NaPozicijiDo);

                var result = await voditeljiUdrugeProvider.Create(vu);
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
            var result = await voditeljiUdrugeProvider.Delete(id);
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

            var voditeljUdruge = await voditeljiUdrugeProvider.Get(id);
            var akt = new VoditeljUdrugeViewModel()
            {
                IdUdruge = id,
                IdClan = voditeljUdruge.Data.IdClan,
                Pozicija = voditeljUdruge.Data.Pozicija,
                NaPozicijiDo = voditeljUdruge.Data.NaPozicijiDo
            };
            return View(akt);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(VoditeljUdrugeViewModel voditeljUdruge)
        {
            VoditeljiUdruge akt = new VoditeljiUdruge(voditeljUdruge.IdUdruge, voditeljUdruge.IdClan, voditeljUdruge.Pozicija, voditeljUdruge.NaPozicijiDo);
            var result = await voditeljiUdrugeProvider.Edit(voditeljUdruge.IdClan, akt);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
