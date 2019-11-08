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
    public class FireWiresController : Controller
    {
        private readonly DicServerContext _context;

        public FireWiresController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/FireWires
        public async Task<IActionResult> Index()
        {
            return View(await _context.FireWire.ToListAsync());
        }

        // GET: Admin/FireWires/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fireWire = await _context.FireWire
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fireWire == null)
            {
                return NotFound();
            }

            return View(fireWire);
        }

        // GET: Admin/FireWires/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/FireWires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VendorID,ProductID,Manufacturer,Product,RemovableMedia")] FireWire fireWire)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fireWire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fireWire);
        }

        // GET: Admin/FireWires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fireWire = await _context.FireWire.FindAsync(id);
            if (fireWire == null)
            {
                return NotFound();
            }
            return View(fireWire);
        }

        // POST: Admin/FireWires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VendorID,ProductID,Manufacturer,Product,RemovableMedia")] FireWire fireWire)
        {
            if (id != fireWire.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fireWire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FireWireExists(fireWire.Id))
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
            return View(fireWire);
        }

        // GET: Admin/FireWires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fireWire = await _context.FireWire
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fireWire == null)
            {
                return NotFound();
            }

            return View(fireWire);
        }

        // POST: Admin/FireWires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fireWire = await _context.FireWire.FindAsync(id);
            _context.FireWire.Remove(fireWire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FireWireExists(int id)
        {
            return _context.FireWire.Any(e => e.Id == id);
        }
    }
}
