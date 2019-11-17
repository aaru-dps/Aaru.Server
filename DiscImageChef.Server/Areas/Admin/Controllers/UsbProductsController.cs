using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class UsbProductsController : Controller
    {
        readonly DicServerContext _context;

        public UsbProductsController(DicServerContext context) => _context = context;

        // GET: Admin/UsbProducts
        public async Task<IActionResult> Index()
        {
            IIncludableQueryable<UsbProduct, UsbVendor> dicServerContext = _context.UsbProducts.Include(u => u.Vendor);

            return View(await dicServerContext.ToListAsync());
        }
    }
}