using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class UsbProductsController : Controller
    {
        readonly DicServerContext _context;

        public UsbProductsController(DicServerContext context) => _context = context;

        // GET: Admin/UsbProducts
        public async Task<IActionResult> Index() => View(await _context.
                                                               UsbProducts.Include(u => u.Vendor).
                                                               OrderBy(p => p.Vendor.Vendor).ThenBy(p => p.Product).
                                                               ThenBy(p => p.ProductId).Select(p => new UsbProductModel
                                                               {
                                                                   ProductId  = p.ProductId, ProductName = p.Product,
                                                                   VendorId   = p.Vendor.Id,
                                                                   VendorName = p.Vendor.Vendor
                                                               }).ToListAsync());
    }
}