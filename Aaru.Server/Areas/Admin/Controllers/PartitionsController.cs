using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class PartitionsController : Controller
{
    readonly DbContext _context;

    public PartitionsController(DbContext context) => _context = context;

    // GET: Admin/Partitions
    public async Task<IActionResult> Index() => View(await _context.Partitions.OrderBy(p => p.Name).ToListAsync());
}