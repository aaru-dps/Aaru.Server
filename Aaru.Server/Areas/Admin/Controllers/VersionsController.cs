using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class VersionsController : Controller
{
    readonly DbContext _context;

    public VersionsController(DbContext context) => _context = context;

    // GET: Admin/Versions
    public async Task<IActionResult> Index() => View(await _context.Versions.OrderBy(v => v.Name).ToListAsync());
}