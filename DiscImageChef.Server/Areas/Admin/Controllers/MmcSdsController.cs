using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class MmcSdsController : Controller
    {
        private readonly DicServerContext _context;

        public MmcSdsController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/MmcSds
        public async Task<IActionResult> Index()
        {
            return View(await _context.MmcSd.ToListAsync());
        }

        // GET: Admin/MmcSds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mmcSd = await _context.MmcSd
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mmcSd == null)
            {
                return NotFound();
            }

            return View(mmcSd);
        }

        // GET: Admin/MmcSds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/MmcSds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CID,CSD,OCR,SCR,ExtendedCSD")] MmcSd mmcSd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mmcSd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mmcSd);
        }

        // GET: Admin/MmcSds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mmcSd = await _context.MmcSd.FindAsync(id);
            if (mmcSd == null)
            {
                return NotFound();
            }
            return View(mmcSd);
        }

        // POST: Admin/MmcSds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CID,CSD,OCR,SCR,ExtendedCSD")] MmcSd mmcSd)
        {
            if (id != mmcSd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mmcSd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MmcSdExists(mmcSd.Id))
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
            return View(mmcSd);
        }

        // GET: Admin/MmcSds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mmcSd = await _context.MmcSd
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mmcSd == null)
            {
                return NotFound();
            }

            return View(mmcSd);
        }

        // POST: Admin/MmcSds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mmcSd = await _context.MmcSd.FindAsync(id);
            _context.MmcSd.Remove(mmcSd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MmcSdExists(int id)
        {
            return _context.MmcSd.Any(e => e.Id == id);
        }
    }
}
