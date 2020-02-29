using System.Linq;
using System.Threading.Tasks;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class OperatingSystemsController : Controller
    {
        readonly AaruServerContext _context;

        public OperatingSystemsController(AaruServerContext context) => _context = context;

        // GET: Admin/OperatingSystems
        public async Task<IActionResult> Index() =>
            View(await _context.OperatingSystems.OrderBy(o => o.Name).ThenBy(o => o.Version).ToListAsync());
    }
}