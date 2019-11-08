using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscImageChef.Server.Models;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PcmciasController : Controller
    {
        private readonly DicServerContext _context;

        public PcmciasController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/Pcmcias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pcmcia.ToListAsync());
        }

        // GET: Admin/Pcmcias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcmcia = await _context.Pcmcia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pcmcia == null)
            {
                return NotFound();
            }

            return View(pcmcia);
        }

        // GET: Admin/Pcmcias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pcmcias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CIS,Compliance,ManufacturerCode,CardCode,Manufacturer,ProductName")] Pcmcia pcmcia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pcmcia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pcmcia);
        }

        // GET: Admin/Pcmcias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcmcia = await _context.Pcmcia.FindAsync(id);
            if (pcmcia == null)
            {
                return NotFound();
            }
            return View(pcmcia);
        }

        // POST: Admin/Pcmcias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CIS,Compliance,ManufacturerCode,CardCode,Manufacturer,ProductName")] Pcmcia pcmcia)
        {
            if (id != pcmcia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pcmcia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PcmciaExists(pcmcia.Id))
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
            return View(pcmcia);
        }

        // GET: Admin/Pcmcias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pcmcia = await _context.Pcmcia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pcmcia == null)
            {
                return NotFound();
            }

            return View(pcmcia);
        }

        // POST: Admin/Pcmcias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pcmcia = await _context.Pcmcia.FindAsync(id);
            _context.Pcmcia.Remove(pcmcia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PcmciaExists(int id)
        {
            return _context.Pcmcia.Any(e => e.Id == id);
        }
    }
}
