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
    public class BlockDescriptorsController : Controller
    {
        private readonly DicServerContext _context;

        public BlockDescriptorsController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/BlockDescriptors
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlockDescriptor.ToListAsync());
        }

        // GET: Admin/BlockDescriptors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blockDescriptor = await _context.BlockDescriptor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blockDescriptor == null)
            {
                return NotFound();
            }

            return View(blockDescriptor);
        }

        // GET: Admin/BlockDescriptors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/BlockDescriptors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Density,Blocks,BlockLength")] BlockDescriptor blockDescriptor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blockDescriptor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blockDescriptor);
        }

        // GET: Admin/BlockDescriptors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blockDescriptor = await _context.BlockDescriptor.FindAsync(id);
            if (blockDescriptor == null)
            {
                return NotFound();
            }
            return View(blockDescriptor);
        }

        // POST: Admin/BlockDescriptors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Density,Blocks,BlockLength")] BlockDescriptor blockDescriptor)
        {
            if (id != blockDescriptor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blockDescriptor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlockDescriptorExists(blockDescriptor.Id))
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
            return View(blockDescriptor);
        }

        // GET: Admin/BlockDescriptors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blockDescriptor = await _context.BlockDescriptor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blockDescriptor == null)
            {
                return NotFound();
            }

            return View(blockDescriptor);
        }

        // POST: Admin/BlockDescriptors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blockDescriptor = await _context.BlockDescriptor.FindAsync(id);
            _context.BlockDescriptor.Remove(blockDescriptor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlockDescriptorExists(int id)
        {
            return _context.BlockDescriptor.Any(e => e.Id == id);
        }
    }
}
