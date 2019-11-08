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
    public class MmcController : Controller
    {
        private readonly DicServerContext _context;

        public MmcController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/Mmc
        public async Task<IActionResult> Index()
        {
            return View(await _context.Mmc.ToListAsync());
        }

        // GET: Admin/Mmc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mmc = await _context.Mmc
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mmc == null)
            {
                return NotFound();
            }

            return View(mmc);
        }

        // GET: Admin/Mmc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Mmc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModeSense2AData")] Mmc mmc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mmc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mmc);
        }

        // GET: Admin/Mmc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mmc = await _context.Mmc.FindAsync(id);
            if (mmc == null)
            {
                return NotFound();
            }
            return View(mmc);
        }

        // POST: Admin/Mmc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModeSense2AData")] Mmc mmc)
        {
            if (id != mmc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mmc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MmcExists(mmc.Id))
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
            return View(mmc);
        }

        // GET: Admin/Mmc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mmc = await _context.Mmc
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mmc == null)
            {
                return NotFound();
            }

            return View(mmc);
        }

        // POST: Admin/Mmc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mmc = await _context.Mmc.FindAsync(id);
            _context.Mmc.Remove(mmc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MmcExists(int id)
        {
            return _context.Mmc.Any(e => e.Id == id);
        }
    }
}
