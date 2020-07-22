using System.Linq;
using System.Threading.Tasks;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public sealed class VersionsController : Controller
    {
        readonly AaruServerContext _context;

        public VersionsController(AaruServerContext context) => _context = context;

        // GET: Admin/Versions
        public async Task<IActionResult> Index() => View(await _context.Versions.OrderBy(v => v.Name).ToListAsync());
    }
}