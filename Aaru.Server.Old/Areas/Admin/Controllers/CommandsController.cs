using DbContext = Aaru.Server.Old.Database.DbContext;

namespace Aaru.Server.Old.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class CommandsController : Controller
{
    readonly DbContext _context;

    public CommandsController(DbContext context) => _context = context;

    // GET: Admin/Commands
    public async Task<IActionResult> Index() => View(await _context.Commands.OrderBy(c => c.Name).ToListAsync());
}