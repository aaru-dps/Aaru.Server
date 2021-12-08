using System.Linq;
using System.Threading.Tasks;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class PartitionsController : Controller
{
    readonly AaruServerContext _context;

    public PartitionsController(AaruServerContext context) => _context = context;

    // GET: Admin/Partitions
    public async Task<IActionResult> Index() => View(await _context.Partitions.OrderBy(p => p.Name).ToListAsync());
}