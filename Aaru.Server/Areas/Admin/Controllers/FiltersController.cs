using System.Linq;
using System.Threading.Tasks;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class FiltersController : Controller
{
    readonly AaruServerContext _context;

    public FiltersController(AaruServerContext context) => _context = context;

    // GET: Admin/Filters
    public async Task<IActionResult> Index() => View(await _context.Filters.OrderBy(f => f.Name).ToListAsync());
}