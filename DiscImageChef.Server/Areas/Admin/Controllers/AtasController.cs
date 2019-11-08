using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AtasController : Controller
    {
        private readonly DicServerContext _context;

        public AtasController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/Atas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ata.ToListAsync());
        }

        // GET: Admin/Atas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ata = await _context.Ata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ata == null)
            {
                return NotFound();
            }

            return View(ata);
        }

        // GET: Admin/Atas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Atas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Identify")] DiscImageChef.CommonTypes.Metadata.Ata ata)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ata);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ata);
        }

        // GET: Admin/Atas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ata = await _context.Ata.FindAsync(id);
            if (ata == null)
            {
                return NotFound();
            }
            return View(ata);
        }

        // POST: Admin/Atas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Identify")] DiscImageChef.CommonTypes.Metadata.Ata ata)
        {
            if (id != ata.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtaExists(ata.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ata);
        }

        // GET: Admin/Atas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ata = await _context.Ata
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ata == null)
            {
                return NotFound();
            }

            return View(ata);
        }

        // POST: Admin/Atas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ata = await _context.Ata.FindAsync(id);
            _context.Ata.Remove(ata);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtaExists(int id)
        {
            return _context.Ata.Any(e => e.Id == id);
        }
    }
}
