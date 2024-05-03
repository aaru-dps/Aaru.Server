using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class FilesystemsController : Controller
{
    readonly DbContext _context;

    public FilesystemsController(DbContext context) => _context = context;

    // GET: Admin/Filesystems
    public async Task<IActionResult> Index() => View(await _context.Filesystems.OrderBy(f => f.Name).ToListAsync());
}