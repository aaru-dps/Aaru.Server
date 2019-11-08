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
    public class UsbsController : Controller
    {
        readonly DicServerContext _context;

        public UsbsController(DicServerContext context) => _context = context;

        // GET: Admin/Usbs
        public async Task<IActionResult> Index() => View(await _context.Usb.ToListAsync());

        // GET: Admin/Usbs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Usb usb = await _context.Usb.FirstOrDefaultAsync(m => m.Id == id);

            if(usb == null)
            {
                return NotFound();
            }

            return View(usb);
        }

        // GET: Admin/Usbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Usb usb = await _context.Usb.FindAsync(id);

            if(usb == null)
            {
                return NotFound();
            }

            return View(usb);
        }

        // POST: Admin/Usbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("Id,VendorID,ProductID,Manufacturer,Product,RemovableMedia,Descriptors")]
            Usb usb)
        {
            if(id != usb.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(usb);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!UsbExists(usb.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(usb);
        }

        // GET: Admin/Usbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Usb usb = await _context.Usb.FirstOrDefaultAsync(m => m.Id == id);

            if(usb == null)
            {
                return NotFound();
            }

            return View(usb);
        }

        // POST: Admin/Usbs/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Usb usb = await _context.Usb.FindAsync(id);
            _context.Usb.Remove(usb);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool UsbExists(int id) => _context.Usb.Any(e => e.Id == id);
    }
}