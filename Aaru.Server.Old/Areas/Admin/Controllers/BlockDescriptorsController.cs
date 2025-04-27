using DbContext = Aaru.Server.Old.Database.DbContext;

namespace Aaru.Server.Old.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class BlockDescriptorsController : Controller
{
    readonly DbContext _context;

    public BlockDescriptorsController(DbContext context) => _context = context;

    // GET: Admin/BlockDescriptors
    public async Task<IActionResult> Index() => View(await _context.BlockDescriptor.OrderBy(b => b.BlockLength)
                                                                   .ThenBy(b => b.Blocks)
                                                                   .ThenBy(b => b.Density)
                                                                   .ToListAsync());
}