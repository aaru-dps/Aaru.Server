using System.Linq;
using System.Threading.Tasks;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class CommandsController : Controller
{
    readonly AaruServerContext _context;

    public CommandsController(AaruServerContext context) => _context = context;

    // GET: Admin/Commands
    public async Task<IActionResult> Index() => View(await _context.Commands.OrderBy(c => c.Name).ToListAsync());
}