namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class CommandsController : Controller
{
    readonly AaruServerContext _context;

    public CommandsController(AaruServerContext context) => _context = context;

    // GET: Admin/Commands
    public async Task<IActionResult> Index() => View(await _context.Commands.OrderBy(c => c.Name).ToListAsync());
}