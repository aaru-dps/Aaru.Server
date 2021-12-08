namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class BlockDescriptorsController : Controller
{
    readonly AaruServerContext _context;

    public BlockDescriptorsController(AaruServerContext context) => _context = context;

    // GET: Admin/BlockDescriptors
    public async Task<IActionResult> Index() => View(await _context.BlockDescriptor.OrderBy(b => b.BlockLength).
                                                                    ThenBy(b => b.Blocks).ThenBy(b => b.Density).
                                                                    ToListAsync());
}