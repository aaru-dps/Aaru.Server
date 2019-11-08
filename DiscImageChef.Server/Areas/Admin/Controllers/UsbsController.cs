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
    public class UsbsController : Controller
    {
        private readonly DicServerContext _context;

        public UsbsController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/Usbs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usb.ToListAsync());
        }

        // GET: Admin/Usbs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usb = await _context.Usb
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usb == null)
            {
                return NotFound();
            }

            return View(usb);
        }

        // GET: Admin/Usbs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Usbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VendorID,ProductID,Manufacturer,Product,RemovableMedia,Descriptors")] Usb usb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usb);
        }

        // GET: Admin/Usbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usb = await _context.Usb.FindAsync(id);
            if (usb == null)
            {
                return NotFound();
            }
            return View(usb);
        }

        // POST: Admin/Usbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VendorID,ProductID,Manufacturer,Product,RemovableMedia,Descriptors")] Usb usb)
        {
            if (id != usb.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsbExists(usb.Id))
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
            return View(usb);
        }

        // GET: Admin/Usbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usb = await _context.Usb
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usb == null)
            {
                return NotFound();
            }

            return View(usb);
        }

        // POST: Admin/Usbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usb = await _context.Usb.FindAsync(id);
            _context.Usb.Remove(usb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsbExists(int id)
        {
            return _context.Usb.Any(e => e.Id == id);
        }
    }
}
