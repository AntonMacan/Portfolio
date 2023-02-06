using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPPP_WebApp.Models;

namespace RPPP_WebApp.Controllers
{
    /// <summary>
    /// Servis za rad s tipovima sadrzaja
    /// </summary>
    public class TipSadrzajaController : Controller
    {
        private readonly RPPP05Context _context;

        public TipSadrzajaController(RPPP05Context context)
        {
            _context = context;
        }

        // <summary>
        /// Vraća stranicu sa listom svih tipova sadrzaja
        /// </summary>
        public async Task<IActionResult> Index()
        {
              return View(await _context.TipSadrzaja.ToListAsync());
        }

        /// <summary>
        /// Vraca stranicu s formom za unos novog tipa sadrzaja
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Obraduje post zahtjev za dodavanje novog tipa sadrzaja
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv")] TipSadrzaja tipSadrzaja)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(tipSadrzaja);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    TempData[Constants.Message] = $"Greška pri spremanju u bazu. Tip sadržaja nije dodan.";
                    TempData[Constants.ErrorOccurred] = true;
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipSadrzaja);
        }

        /// <summary>
        /// Vraca stranicu s formom za azuriranje tipa sadrzaja
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipSadrzaja == null)
            {
                return NotFound();
            }

            var tipSadrzaja = await _context.TipSadrzaja.FindAsync(id);
            if (tipSadrzaja == null)
            {
                return NotFound();
            }
            return View(tipSadrzaja);
        }

        /// <summary>
        /// Obraduje post zahtjev za azuriranje tipa sadrzaja
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv")] TipSadrzaja tipSadrzaja)
        {
            if (id != tipSadrzaja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipSadrzaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData[Constants.Message] = $"Greška pri spremanju u bazu. Tip sadržaja nije ažuriran.";
                    TempData[Constants.ErrorOccurred] = true;
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipSadrzaja);
        }

        /// <summary>
        /// Vraca stranicu sa potvrdom brisanja tipa sadrzaja
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipSadrzaja == null)
            {
                return NotFound();
            }

            var tipSadrzaja = await _context.TipSadrzaja
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipSadrzaja == null)
            {
                return NotFound();
            }

            return View(tipSadrzaja);
        }

        /// <summary>
        /// Obraduje zahtjev za brisanje tipa sadrzaja
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipSadrzaja == null)
            {
                return Problem("Entity set 'RPPP05Context.TipSadrzaja'  is null.");
            }
            var tipSadrzaja = await _context.TipSadrzaja.FindAsync(id);
            if (tipSadrzaja != null)
            {
                _context.TipSadrzaja.Remove(tipSadrzaja);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData[Constants.Message] = $"Tip sadržaja ne može bit obrisan.";
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TipSadrzajaExists(int id)
        {
          return _context.TipSadrzaja.Any(e => e.Id == id);
        }
    }
}
