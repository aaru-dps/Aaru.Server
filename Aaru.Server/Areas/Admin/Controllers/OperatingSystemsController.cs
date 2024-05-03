using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class OperatingSystemsController : Controller
{
    readonly DbContext _context;

    public OperatingSystemsController(DbContext context) => _context = context;

    // GET: Admin/OperatingSystems
    public async Task<IActionResult> Index() =>
        View(await _context.OperatingSystems.OrderBy(o => o.Name).ThenBy(o => o.Version).ToListAsync());
}