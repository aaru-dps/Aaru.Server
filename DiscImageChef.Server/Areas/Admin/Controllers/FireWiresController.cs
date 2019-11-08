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
    public class FireWiresController : Controller
    {
        readonly DicServerContext _context;

        public FireWiresController(DicServerContext context) => _context = context;

        // GET: Admin/FireWires
        public async Task<IActionResult> Index() => View(await _context.FireWire.ToListAsync());

        // GET: Admin/FireWires/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            FireWire fireWire = await _context.FireWire.FirstOrDefaultAsync(m => m.Id == id);

            if(fireWire == null)
            {
                return NotFound();
            }

            return View(fireWire);
        }

        // GET: Admin/FireWires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            FireWire fireWire = await _context.FireWire.FindAsync(id);

            if(fireWire == null)
            {
                return NotFound();
            }

            return View(fireWire);
        }

        // POST: Admin/FireWires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("Id,VendorID,ProductID,Manufacturer,Product,RemovableMedia")]
            FireWire fireWire)
        {
            if(id != fireWire.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(fireWire);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!FireWireExists(fireWire.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(fireWire);
        }

        // GET: Admin/FireWires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            FireWire fireWire = await _context.FireWire.FirstOrDefaultAsync(m => m.Id == id);

            if(fireWire == null)
            {
                return NotFound();
            }

            return View(fireWire);
        }

        // POST: Admin/FireWires/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            FireWire fireWire = await _context.FireWire.FindAsync(id);
            _context.FireWire.Remove(fireWire);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool FireWireExists(int id) => _context.FireWire.Any(e => e.Id == id);
    }
}