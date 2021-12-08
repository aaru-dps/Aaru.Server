namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class UsbProductsController : Controller
{
    readonly AaruServerContext _context;

    public UsbProductsController(AaruServerContext context) => _context = context;

    // GET: Admin/UsbProducts
    public async Task<IActionResult> Index() => View(await _context.UsbProducts.Include(u => u.Vendor).
                                                                    OrderBy(p => p.Vendor.Vendor).
                                                                    ThenBy(p => p.Product).ThenBy(p => p.ProductId).
                                                                    Select(p => new UsbProductModel
                                                                    {
                                                                        ProductId   = p.ProductId,
                                                                        ProductName = p.Product,
                                                                        VendorId    = p.Vendor.Id,
                                                                        VendorName  = p.Vendor.Vendor
                                                                    }).ToListAsync());
}