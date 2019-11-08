using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class OperatingSystemsController : Controller
    {
        readonly DicServerContext _context;

        public OperatingSystemsController(DicServerContext context) => _context = context;

        // GET: Admin/OperatingSystems
        public async Task<IActionResult> Index() => View(await _context.OperatingSystems.ToListAsync());

        // GET: Admin/OperatingSystems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            OperatingSystem operatingSystem = await _context.OperatingSystems.FirstOrDefaultAsync(m => m.Id == id);

            if(operatingSystem == null)
            {
                return NotFound();
            }

            return View(operatingSystem);
        }

        // GET: Admin/OperatingSystems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            OperatingSystem operatingSystem = await _context.OperatingSystems.FindAsync(id);

            if(operatingSystem == null)
            {
                return NotFound();
            }

            return View(operatingSystem);
        }

        // POST: Admin/OperatingSystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Version,Count")] OperatingSystem operatingSystem)
        {
            if(id != operatingSystem.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(operatingSystem);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!OperatingSystemExists(operatingSystem.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(operatingSystem);
        }

        // GET: Admin/OperatingSystems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            OperatingSystem operatingSystem = await _context.OperatingSystems.FirstOrDefaultAsync(m => m.Id == id);

            if(operatingSystem == null)
            {
                return NotFound();
            }

            return View(operatingSystem);
        }

        // POST: Admin/OperatingSystems/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            OperatingSystem operatingSystem = await _context.OperatingSystems.FindAsync(id);
            _context.OperatingSystems.Remove(operatingSystem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool OperatingSystemExists(int id) => _context.OperatingSystems.Any(e => e.Id == id);
    }
}