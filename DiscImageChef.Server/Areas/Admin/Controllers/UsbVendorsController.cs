using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class UsbVendorsController : Controller
    {
        readonly DicServerContext _context;

        public UsbVendorsController(DicServerContext context) => _context = context;

        // GET: Admin/UsbVendors
        public async Task<IActionResult> Index() => View(await _context.UsbVendors.ToListAsync());

        // GET: Admin/UsbVendors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UsbVendor usbVendor = await _context.UsbVendors.FirstOrDefaultAsync(m => m.Id == id);

            if(usbVendor == null)
            {
                return NotFound();
            }

            return View(usbVendor);
        }

        // GET: Admin/UsbVendors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UsbVendor usbVendor = await _context.UsbVendors.FindAsync(id);

            if(usbVendor == null)
            {
                return NotFound();
            }

            return View(usbVendor);
        }

        // POST: Admin/UsbVendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VendorId,Vendor,AddedWhen,ModifiedWhen")]
                                              UsbVendor usbVendor)
        {
            if(id != usbVendor.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(usbVendor);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!UsbVendorExists(usbVendor.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(usbVendor);
        }

        // GET: Admin/UsbVendors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UsbVendor usbVendor = await _context.UsbVendors.FirstOrDefaultAsync(m => m.Id == id);

            if(usbVendor == null)
            {
                return NotFound();
            }

            return View(usbVendor);
        }

        // POST: Admin/UsbVendors/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UsbVendor usbVendor = await _context.UsbVendors.FindAsync(id);
            _context.UsbVendors.Remove(usbVendor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool UsbVendorExists(int id) => _context.UsbVendors.Any(e => e.Id == id);
    }
}