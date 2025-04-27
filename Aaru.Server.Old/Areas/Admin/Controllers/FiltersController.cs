using DbContext = Aaru.Server.Old.Database.DbContext;

namespace Aaru.Server.Old.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class FiltersController : Controller
{
    readonly DbContext _context;

    public FiltersController(DbContext context) => _context = context;

    // GET: Admin/Filters
    public async Task<IActionResult> Index() => View(await _context.Filters.OrderBy(f => f.Name).ToListAsync());
}