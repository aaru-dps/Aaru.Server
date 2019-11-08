using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ChsController : Controller
    {
        readonly DicServerContext _context;

        public ChsController(DicServerContext context) => _context = context;

        // GET: Admin/Chs
        public async Task<IActionResult> Index() => View(await _context.Chs.ToListAsync());

        // GET: Admin/Chs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Chs chs = await _context.Chs.FirstOrDefaultAsync(m => m.Id == id);

            if(chs == null)
            {
                return NotFound();
            }

            return View(chs);
        }

        // GET: Admin/Chs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Chs chs = await _context.Chs.FindAsync(id);

            if(chs == null)
            {
                return NotFound();
            }

            return View(chs);
        }

        // POST: Admin/Chs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cylinders,Heads,Sectors")] Chs chs)
        {
            if(id != chs.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(chs);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!ChsExists(chs.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(chs);
        }

        // GET: Admin/Chs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Chs chs = await _context.Chs.FirstOrDefaultAsync(m => m.Id == id);

            if(chs == null)
            {
                return NotFound();
            }

            return View(chs);
        }

        // POST: Admin/Chs/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Chs chs = await _context.Chs.FindAsync(id);
            _context.Chs.Remove(chs);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ChsExists(int id) => _context.Chs.Any(e => e.Id == id);
    }
}