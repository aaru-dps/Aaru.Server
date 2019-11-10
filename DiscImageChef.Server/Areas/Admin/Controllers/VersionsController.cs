using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class VersionsController : Controller
    {
        readonly DicServerContext _context;

        public VersionsController(DicServerContext context) => _context = context;

        // GET: Admin/Versions
        public async Task<IActionResult> Index() => View(await _context.Versions.OrderBy(v => v.Value).ToListAsync());
    }
}