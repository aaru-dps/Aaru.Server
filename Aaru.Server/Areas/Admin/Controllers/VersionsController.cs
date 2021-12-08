namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class VersionsController : Controller
{
    readonly AaruServerContext _context;

    public VersionsController(AaruServerContext context) => _context = context;

    // GET: Admin/Versions
    public async Task<IActionResult> Index() => View(await _context.Versions.OrderBy(v => v.Name).ToListAsync());
}