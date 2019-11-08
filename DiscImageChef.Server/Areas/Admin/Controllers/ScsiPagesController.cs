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
    public class ScsiPagesController : Controller
    {
        private readonly DicServerContext _context;

        public ScsiPagesController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/ScsiPages
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScsiPage.ToListAsync());
        }

        // GET: Admin/ScsiPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scsiPage = await _context.ScsiPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scsiPage == null)
            {
                return NotFound();
            }

            return View(scsiPage);
        }

        // GET: Admin/ScsiPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ScsiPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,page,subpage,value")] ScsiPage scsiPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scsiPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scsiPage);
        }

        // GET: Admin/ScsiPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scsiPage = await _context.ScsiPage.FindAsync(id);
            if (scsiPage == null)
            {
                return NotFound();
            }
            return View(scsiPage);
        }

        // POST: Admin/ScsiPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,page,subpage,value")] ScsiPage scsiPage)
        {
            if (id != scsiPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scsiPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScsiPageExists(scsiPage.Id))
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
            return View(scsiPage);
        }

        // GET: Admin/ScsiPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scsiPage = await _context.ScsiPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scsiPage == null)
            {
                return NotFound();
            }

            return View(scsiPage);
        }

        // POST: Admin/ScsiPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scsiPage = await _context.ScsiPage.FindAsync(id);
            _context.ScsiPage.Remove(scsiPage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScsiPageExists(int id)
        {
            return _context.ScsiPage.Any(e => e.Id == id);
        }
    }
}
