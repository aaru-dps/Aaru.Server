using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class OperatingSystemsController : Controller
    {
        readonly DicServerContext _context;

        public OperatingSystemsController(DicServerContext context) => _context = context;

        // GET: Admin/OperatingSystems
        public async Task<IActionResult> Index() =>
            View(await _context.OperatingSystems.OrderBy(o => o.Name).ThenBy(o => o.Version).ToListAsync());
    }
}