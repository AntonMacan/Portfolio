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
    /// Servis za rad s multimedijom
    /// </summary>
    public class MultimedijaController : Controller
    {
        private readonly RPPP05Context _context;

        public MultimedijaController(RPPP05Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Vraca stranicu s formom za unos novog multimedijskog sadrzaja
        /// </summary>
        /// <returns></returns>
        public IActionResult Create(int odmoristeId)
        {            
            return View(new Multimedija { OdmoristeId = odmoristeId});
        }

        /// <summary>
        /// Obraduje post zahtjev za unos novog multimedijskog sadrzaja
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Url,OdmoristeId")] Multimedija multimedija)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(multimedija);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    TempData[Constants.Message] = $"Greška pri spremanju u bazu. Poveznica nije dodana.";
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(OdmoristaController.Details),"Odmorista", new {id = multimedija.OdmoristeId});
            }
            //ViewData["OdmoristeId"] = new SelectList(_context.Odmoriste, "Id", "Naziv", multimedija.OdmoristeId);
            return View(multimedija);
        }

        /// <summary>
        /// Vraca stranicu s formom za azuriranje multimedijskog sadrzaja
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id, int odmoristeId)
        {
            if(odmoristeId == 0)
            {
                return NotFound();
            }
            if (id == null || _context.Multimedija == null)
            {
                return NotFound();
            }

            var multimedija = await _context.Multimedija.FindAsync(id);
            if (multimedija == null)
            {
                return NotFound();
            }

            return View(multimedija);
        }

        /// <summary>
        /// Obraduje post zahtjev za azuriranje multimedijskog sadrzaja
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Url,OdmoristeId")] Multimedija multimedija)
        {
            if (id != multimedija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(multimedija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultimedijaExists(multimedija.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData[Constants.Message] = $"Greška pri spremanju u bazu. Poveznica nije ažurirana.";
                        TempData[Constants.ErrorOccurred] = true;
                    }
                }
                return RedirectToAction(nameof(OdmoristaController.Details), "Odmorista", new { id = multimedija.OdmoristeId });
            }
            return View(multimedija);
        }

        /// <summary>
        /// Vraca stranicu sa potvrdom brisanja multimedijskog sadrzaja
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Multimedija == null)
            {
                return NotFound();
            }

            var multimedija = await _context.Multimedija
                .Include(m => m.Odmoriste)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multimedija == null)
            {
                return NotFound();
            }

            return View(multimedija);
        }

        /// <summary>
        /// Obraduje zahtjev za brisanje multimedijskog sadrzaja
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Multimedija == null)
            {
                return Problem("Entity set 'RPPP05Context.Multimedija'  is null.");
            }
            var multimedija = await _context.Multimedija.FindAsync(id);
            int returnId = multimedija.OdmoristeId;
            if (multimedija != null)
            {
                _context.Multimedija.Remove(multimedija);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData[Constants.Message] = $"Nije moguce obrisati poveznicu.";
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(OdmoristaController.Details), "Odmorista", new { id = returnId });
        }

        private bool MultimedijaExists(int id)
        {
          return _context.Multimedija.Any(e => e.Id == id);
        }
    }
}
