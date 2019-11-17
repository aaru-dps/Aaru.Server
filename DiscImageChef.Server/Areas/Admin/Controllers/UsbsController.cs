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
        public async Task<IActionResult> Index() =>
            View(await _context.Usb.OrderBy(u => u.Manufacturer).ThenBy(u => u.Product).ThenBy(u => u.VendorID).
                                ThenBy(u => u.ProductID).ToListAsync());

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
    }
}