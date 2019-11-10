using System;
using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class CompactDiscOffsetsController : Controller
    {
        readonly DicServerContext _context;

        public CompactDiscOffsetsController(DicServerContext context) => _context = context;

        // GET: Admin/CompactDiscOffsets
        public async Task<IActionResult> Index() =>
            View(await _context.CdOffsets.OrderBy(o => o.Manufacturer).ThenBy(o => o.Model).ThenBy(o => o.Offset).
                                ToListAsync());

        // GET: Admin/CompactDiscOffsets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            CompactDiscOffset compactDiscOffset = await _context.CdOffsets.FirstOrDefaultAsync(m => m.Id == id);

            if(compactDiscOffset == null)
            {
                return NotFound();
            }

            return View(compactDiscOffset);
        }

        // GET: Admin/CompactDiscOffsets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            CompactDiscOffset compactDiscOffset = await _context.CdOffsets.FindAsync(id);

            if(compactDiscOffset == null)
            {
                return NotFound();
            }

            return View(compactDiscOffset);
        }

        // POST: Admin/CompactDiscOffsets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("Id,AddedWhen,ModifiedWhen,Manufacturer,Model,Offset,Submissions,Agreement")]
            CompactDiscOffset compactDiscOffset)
        {
            if(id != compactDiscOffset.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(compactDiscOffset);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!CompactDiscOffsetExists(compactDiscOffset.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(compactDiscOffset);
        }

        // GET: Admin/CompactDiscOffsets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            CompactDiscOffset compactDiscOffset = await _context.CdOffsets.FirstOrDefaultAsync(m => m.Id == id);

            if(compactDiscOffset == null)
            {
                return NotFound();
            }

            return View(compactDiscOffset);
        }

        // POST: Admin/CompactDiscOffsets/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CompactDiscOffset compactDiscOffset = await _context.CdOffsets.FindAsync(id);
            _context.CdOffsets.Remove(compactDiscOffset);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool CompactDiscOffsetExists(int id) => _context.CdOffsets.Any(e => e.Id == id);

        public IActionResult Update()
        {
            throw new NotImplementedException();
        }
    }
}