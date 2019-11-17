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
    }
}