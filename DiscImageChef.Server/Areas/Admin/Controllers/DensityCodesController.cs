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
    public class DensityCodesController : Controller
    {
        private readonly DicServerContext _context;

        public DensityCodesController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/DensityCodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DensityCode.ToListAsync());
        }

        // GET: Admin/DensityCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var densityCode = await _context.DensityCode
                .FirstOrDefaultAsync(m => m.Id == id);
            if (densityCode == null)
            {
                return NotFound();
            }

            return View(densityCode);
        }

        // GET: Admin/DensityCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/DensityCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code")] DensityCode densityCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(densityCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(densityCode);
        }

        // GET: Admin/DensityCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var densityCode = await _context.DensityCode.FindAsync(id);
            if (densityCode == null)
            {
                return NotFound();
            }
            return View(densityCode);
        }

        // POST: Admin/DensityCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code")] DensityCode densityCode)
        {
            if (id != densityCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(densityCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DensityCodeExists(densityCode.Id))
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
            return View(densityCode);
        }

        // GET: Admin/DensityCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var densityCode = await _context.DensityCode
                .FirstOrDefaultAsync(m => m.Id == id);
            if (densityCode == null)
            {
                return NotFound();
            }

            return View(densityCode);
        }

        // POST: Admin/DensityCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var densityCode = await _context.DensityCode.FindAsync(id);
            _context.DensityCode.Remove(densityCode);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DensityCodeExists(int id)
        {
            return _context.DensityCode.Any(e => e.Id == id);
        }
    }
}
